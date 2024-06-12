using SharpCraft.Core;
using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util;
using SharpCraft.Core.Util.Logging;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Server.Entities;
using SharpCraft.Server.Gamemode;
using SharpCraft.Server.Levell;
using SharpCraft.Server.Levell.Chonk;
using SharpCraft.Server.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server.Config
{
    public class ServerConfigurationManager
    {
        public static Logger logger = Logger.GetLogger(SharedConstants.LOGGER_NS);
        public IList<ServerPlayer> playerEntities = new List<ServerPlayer>();
        private Server mcServer;
        private ServerChunkDistance[] playerManagerObj = new ServerChunkDistance[2];
        private int maxPlayers;
        private HashSet<string> bannedPlayers = new HashSet<string>();
        private HashSet<string> bannedIPs = new HashSet<string>();
        private HashSet<string> ops = new HashSet<string>();
        private HashSet<string> whiteListedIPs = new HashSet<string>();
        private JFile bannedPlayersFile;
        private JFile ipBanFile;
        private JFile opFile;
        private JFile whitelistPlayersFile;
        private IPlayerIO playerIO;
        private bool whiteListEnforced;
        public ServerConfigurationManager(Server srv)
        {
            this.mcServer = srv;
            this.bannedPlayersFile = srv.GetFile("banned-players.txt");
            this.ipBanFile = srv.GetFile("banned-ips.txt");
            this.opFile = srv.GetFile("ops.txt");
            this.whitelistPlayersFile = srv.GetFile("white-list.txt");
            int i2 = srv.properties.GetIntProperty("view-distance", 10);
            for (int i = 0; i < this.playerManagerObj.Length; i++) 
            {
                this.playerManagerObj[i] = new ServerChunkDistance(srv, Server.ArrayIndexToDimension(i), i2);
            }
            this.maxPlayers = srv.properties.GetIntProperty("max-players", 20);
            this.whiteListEnforced = srv.properties.GetBooleanProperty("white-list", false);
            this.ReadBannedPlayers();
            this.LoadBannedList();
            this.LoadOps();
            this.LoadWhiteList();
            this.WriteBannedPlayers();
            this.SaveBannedList();
            this.SaveOps();
            this.SaveWhiteList();
        }

        public virtual void SetPlayerIO(ServerLevel[] worldServer1)
        {
            this.playerIO = worldServer1[0].GetLevelStorage().GetPlayerIO();
        }

        public virtual void Func_28172_a(ServerPlayer entityPlayerMP1)
        {
            this.playerManagerObj[0].RemovePlayer(entityPlayerMP1);
            this.playerManagerObj[1].RemovePlayer(entityPlayerMP1);
            this.GetChunkDistance(entityPlayerMP1.dimension).AddPlayer(entityPlayerMP1);
            ServerLevel worldServer2 = this.mcServer.GetLevel(entityPlayerMP1.dimension);
            worldServer2.serverChunkCache.Create((int)entityPlayerMP1.x >> 4, (int)entityPlayerMP1.z >> 4);
        }

        public virtual int GetMaxTrackingDistance()
        {
            return this.playerManagerObj[0].GetMaxTrackingDistance();
        }

        private ServerChunkDistance GetChunkDistance(int i1)
        {
            return i1 == -1 ? this.playerManagerObj[1] : this.playerManagerObj[0];
        }

        public virtual void ReadPlayerDataFromFile(ServerPlayer entityPlayerMP1)
        {
            this.playerIO.Read(entityPlayerMP1);
        }

        public virtual void PlayerLoggedIn(ServerPlayer entityPlayerMP1)
        {
            this.playerEntities.Add(entityPlayerMP1);
            ServerLevel worldServer2 = this.mcServer.GetLevel(entityPlayerMP1.dimension);
            worldServer2.serverChunkCache.Create((int)entityPlayerMP1.x >> 4, (int)entityPlayerMP1.z >> 4);
            while (worldServer2.GetCubes(entityPlayerMP1, entityPlayerMP1.boundingBox).Count != 0)
            {
                entityPlayerMP1.SetPosition(entityPlayerMP1.x, entityPlayerMP1.y + 1.0D, entityPlayerMP1.z);
            }

            worldServer2.AddEntity(entityPlayerMP1);
            this.GetChunkDistance(entityPlayerMP1.dimension).AddPlayer(entityPlayerMP1);
            
        }

        public virtual void Func_613_b(ServerPlayer entityPlayerMP1)
        {
            this.GetChunkDistance(entityPlayerMP1.dimension).Func_543_c(entityPlayerMP1);
        }

        public virtual void PlayerLoggedOut(ServerPlayer entityPlayerMP1)
        {
            this.playerIO.Write(entityPlayerMP1);
            this.mcServer.GetLevel(entityPlayerMP1.dimension).SetEntityDead(entityPlayerMP1);
            this.playerEntities.Remove(entityPlayerMP1);
            this.GetChunkDistance(entityPlayerMP1.dimension).RemovePlayer(entityPlayerMP1);
        }

        public virtual ServerPlayer Login(PendingConnection netLoginHandler1, string string2)
        {
            if (this.bannedPlayers.Contains(string2.Trim().ToLower()))
            {
                netLoginHandler1.KickUser("You are banned from this server!");
                return null;
            }
            else if (!this.IsAllowedToLogin(string2))
            {
                netLoginHandler1.KickUser("You are not white-listed on this server!");
                return null;
            }
            else
            {
                string string3 = netLoginHandler1.netConnection.GetRemoteAddress().ToString();
                string3 = string3.Substring(string3.IndexOf('/') + 1);
                string3 = string3.Substring(0, string3.IndexOf(':'));
                if (this.bannedIPs.Contains(string3))
                {
                    netLoginHandler1.KickUser("Your IP address is banned from this server!");
                    return null;
                }
                else if (this.playerEntities.Count >= this.maxPlayers)
                {
                    netLoginHandler1.KickUser("The server is full!");
                    return null;
                }
                else
                {
                    for (int i4 = 0; i4 < this.playerEntities.Count; ++i4)
                    {
                        ServerPlayer entityPlayerMP5 = this.playerEntities[i4];
                        if (entityPlayerMP5.username.ToLower().Equals(string2))
                        {
                            entityPlayerMP5.playerNetServerHandler.KickPlayer("You logged in from another location");
                        }
                    }

                    return new ServerPlayer(this.mcServer, this.mcServer.GetLevel(0), string2, new ServerPlayerGameMode(this.mcServer.GetLevel(0)));
                }
            }
        }

        public virtual ServerPlayer RecreatePlayerEntity(ServerPlayer entityPlayerMP1, int i2)
        {
            this.mcServer.GetEntityTracker(entityPlayerMP1.dimension).RemoveTrackedPlayerSymmetric(entityPlayerMP1);
            this.mcServer.GetEntityTracker(entityPlayerMP1.dimension).UntrackEntity(entityPlayerMP1);
            this.GetChunkDistance(entityPlayerMP1.dimension).RemovePlayer(entityPlayerMP1);
            this.playerEntities.Remove(entityPlayerMP1);
            this.mcServer.GetLevel(entityPlayerMP1.dimension).RemovePlayer(entityPlayerMP1);
            Pos chunkCoordinates3 = entityPlayerMP1.GetSpawnPos();
            entityPlayerMP1.dimension = i2;
            ServerPlayer entityPlayerMP4 = new ServerPlayer(this.mcServer, this.mcServer.GetLevel(entityPlayerMP1.dimension), entityPlayerMP1.username, new ServerPlayerGameMode(this.mcServer.GetLevel(entityPlayerMP1.dimension)));
            entityPlayerMP4.entityID = entityPlayerMP1.entityID;
            entityPlayerMP4.playerNetServerHandler = entityPlayerMP1.playerNetServerHandler;
            ServerLevel worldServer5 = this.mcServer.GetLevel(entityPlayerMP1.dimension);
            if (chunkCoordinates3 != default)
            {
                Pos chunkCoordinates6 = Player.GetNearestBedSpawnPos(this.mcServer.GetLevel(entityPlayerMP1.dimension), chunkCoordinates3);
                if (chunkCoordinates6 != default)
                {
                    entityPlayerMP4.SetLocationAndAngles(chunkCoordinates6.x + 0.5F, chunkCoordinates6.y + 0.1F, chunkCoordinates6.z + 0.5F, 0F, 0F);
                    entityPlayerMP4.SetSpawn(chunkCoordinates3);
                }
                else
                {
                    entityPlayerMP4.playerNetServerHandler.SendPacket(new Packet70Bed(0));
                }
            }

            worldServer5.serverChunkCache.Create((int)entityPlayerMP4.x >> 4, (int)entityPlayerMP4.z >> 4);
            while (worldServer5.GetCubes(entityPlayerMP4, entityPlayerMP4.boundingBox).Count != 0)
            {
                entityPlayerMP4.SetPosition(entityPlayerMP4.x, entityPlayerMP4.y + 1, entityPlayerMP4.z);
            }

            entityPlayerMP4.playerNetServerHandler.SendPacket(new Packet9Respawn((sbyte)entityPlayerMP4.dimension));
            entityPlayerMP4.playerNetServerHandler.TeleportTo(entityPlayerMP4.x, entityPlayerMP4.y, entityPlayerMP4.z, entityPlayerMP4.yaw, entityPlayerMP4.pitch);
            this.Func_28170_a(entityPlayerMP4, worldServer5);
            this.GetChunkDistance(entityPlayerMP4.dimension).AddPlayer(entityPlayerMP4);
            worldServer5.AddEntity(entityPlayerMP4);
            this.playerEntities.Add(entityPlayerMP4);
            entityPlayerMP4.Func_20057_k();
            entityPlayerMP4.Func_22068_s();
            return entityPlayerMP4;
        }

        public virtual void SendPlayerToOtherDimension(ServerPlayer player)
        {
            ServerLevel level = this.mcServer.GetLevel(player.dimension);
            sbyte dimension;
            if (player.dimension == -1)
            {
                dimension = 0;
            }
            else
            {
                dimension = -1;
            }

            player.dimension = dimension;
            ServerLevel worldServer4 = this.mcServer.GetLevel(player.dimension);
            player.playerNetServerHandler.SendPacket(new Packet9Respawn((sbyte)player.dimension));
            level.RemovePlayer(player);
            player.isDead = false;
            double d5 = player.x;
            double d7 = player.z;
            double d9 = 8;
            if (player.dimension == -1)
            {
                d5 /= d9;
                d7 /= d9;
                player.SetLocationAndAngles(d5, player.y, d7, player.yaw, player.pitch);
                if (player.IsEntityAlive())
                {
                    level.UpdateEntityWithOptionalForce(player, false);
                }
            }
            else
            {
                d5 *= d9;
                d7 *= d9;
                player.SetLocationAndAngles(d5, player.y, d7, player.yaw, player.pitch);
                if (player.IsEntityAlive())
                {
                    level.UpdateEntityWithOptionalForce(player, false);
                }
            }

            if (player.IsEntityAlive())
            {
                worldServer4.AddEntity(player);
                player.SetLocationAndAngles(d5, player.y, d7, player.yaw, player.pitch);
                worldServer4.UpdateEntityWithOptionalForce(player, false);
                worldServer4.serverChunkCache.SetChunkLoadOverride(true);
                (new PortalForcer()).SetExitLocation(worldServer4, player);
                worldServer4.serverChunkCache.SetChunkLoadOverride(false);
            }

            this.Func_28172_a(player);
            player.playerNetServerHandler.TeleportTo(player.x, player.y, player.z, player.yaw, player.pitch);
            player.SetWorld(worldServer4);
            this.Func_28170_a(player, worldServer4);
            this.Func_30008_g(player);
        }

        public virtual void Tick()
        {
            for (int i1 = 0; i1 < this.playerManagerObj.Length; ++i1)
            {
                this.playerManagerObj[i1].Tick();
            }
        }

        public virtual void MarkBlockNeedsUpdate(int i1, int i2, int i3, int i4)
        {
            this.GetChunkDistance(i4).MarkBlockNeedsUpdate(i1, i2, i3);
        }

        public virtual void SendPacketToAllPlayers(Packet packet1)
        {
            for (int i2 = 0; i2 < this.playerEntities.Count; ++i2)
            {
                ServerPlayer entityPlayerMP3 = this.playerEntities[i2];
                entityPlayerMP3.playerNetServerHandler.SendPacket(packet1);
            }
        }

        public virtual void SendPacketToAllPlayersInDimension(Packet packet1, int i2)
        {
            for (int i3 = 0; i3 < this.playerEntities.Count; ++i3)
            {
                ServerPlayer entityPlayerMP4 = this.playerEntities[i3];
                if (entityPlayerMP4.dimension == i2)
                {
                    entityPlayerMP4.playerNetServerHandler.SendPacket(packet1);
                }
            }
        }

        public virtual string GetPlayerList()
        {
            string string1 = "";
            for (int i2 = 0; i2 < this.playerEntities.Count; ++i2)
            {
                if (i2 > 0)
                {
                    string1 = string1 + ", ";
                }

                string1 = string1 + this.playerEntities[i2].username;
            }

            return string1;
        }

        public virtual void BanPlayer(string string1)
        {
            this.bannedPlayers.Add(string1.ToLower());
            this.WriteBannedPlayers();
        }

        public virtual void PardonPlayer(string string1)
        {
            this.bannedPlayers.Remove(string1.ToLower());
            this.WriteBannedPlayers();
        }

        private void ReadBannedPlayers()
        {
            LoadPlayerList(bannedPlayers, bannedPlayersFile, "ban");
        }

        private void WriteBannedPlayers()
        {
            SavePlayerList(bannedPlayers, bannedPlayersFile, "ban");
        }

        private static void LoadPlayerList(ISet<string> set, JFile file, string errtype)
        {
            try
            {
                set.Clear();
                StreamReader reader = new StreamReader(file.GetAbsolutePath());

                string ln = "";
                while ((ln = reader.ReadLine()) != null)
                {
                    set.Add(ln.Trim().ToLower());
                }

                reader.Close();
                reader.Dispose();

            }
            catch (Exception ex)
            {
                logger.Warning($"Failed to load {errtype} list: {ex.Message}");
            }
        }

        private static void SavePlayerList(ISet<string> set, JFile file, string errtype) 
        {
            try 
            {
                StreamWriter writer = new StreamWriter(file.GetAbsolutePath());

                foreach (string s in set) 
                {
                    writer.WriteLine(s);
                }
                writer.Close();
                writer.Dispose();

            } catch (Exception ex) 
            {
                logger.Warning($"Failed to save {errtype} list: {ex.Message}");
            }
        }

        public virtual void BanIP(string string1)
        {
            this.bannedIPs.Add(string1.ToLower());
            this.SaveBannedList();
        }

        public virtual void PardonIP(string string1)
        {
            this.bannedIPs.Remove(string1.ToLower());
            this.SaveBannedList();
        }

        private void LoadBannedList()
        {
            LoadPlayerList(bannedIPs, ipBanFile, "ip ban");
        }

        private void SaveBannedList()
        {
            SavePlayerList(bannedIPs, ipBanFile, "ip ban");
        }

        public virtual void OpPlayer(string string1)
        {
            this.ops.Add(string1.ToLower());
            this.SaveOps();
        }

        public virtual void DeopPlayer(string string1)
        {
            this.ops.Remove(string1.ToLower());
            this.SaveOps();
        }

        private void LoadOps()
        {
            LoadPlayerList(ops, opFile, "op");
        }

        private void SaveOps()
        {
            SavePlayerList(ops, opFile, "op");
        }

        private void LoadWhiteList()
        {
            LoadPlayerList(whiteListedIPs, whitelistPlayersFile, "white-list");
        }

        private void SaveWhiteList()
        {
            SavePlayerList(whiteListedIPs, whitelistPlayersFile, "white-list");
        }

        public virtual bool IsAllowedToLogin(string string1)
        {
            string1 = string1.Trim().ToLower();
            return !this.whiteListEnforced || this.ops.Contains(string1) || this.whiteListedIPs.Contains(string1);
        }

        public virtual bool IsOp(string string1)
        {
            return this.ops.Contains(string1.Trim().ToLower());
        }

        public virtual ServerPlayer GetPlayerEntity(string string1)
        {
            for (int i2 = 0; i2 < this.playerEntities.Count; ++i2)
            {
                ServerPlayer entityPlayerMP3 = this.playerEntities[i2];
                if (entityPlayerMP3.username.ToLower().Equals(string1))
                {
                    return entityPlayerMP3;
                }
            }

            return null;
        }

        public virtual void SendChatMessageToPlayer(string string1, string string2)
        {
            ServerPlayer entityPlayerMP3 = this.GetPlayerEntity(string1);
            if (entityPlayerMP3 != null)
            {
                entityPlayerMP3.playerNetServerHandler.SendPacket(new Packet3Chat(string2));
            }
        }

        public virtual void SendPacketToPlayersAroundPoint(double d1, double d3, double d5, double d7, int i9, Packet packet10)
        {
            this.SendPacketToPlayersAroundPoint((Player)null, d1, d3, d5, d7, i9, packet10);
        }

        public virtual void SendPacketToPlayersAroundPoint(Player source, double x, double y, double z, double radius, int dimension, Packet packet)
        {
            for (int i12 = 0; i12 < this.playerEntities.Count; ++i12)
            {
                ServerPlayer player = this.playerEntities[i12];
                if (player != source && player.dimension == dimension)
                {
                    double d14 = x - player.x;
                    double d16 = y - player.y;
                    double d18 = z - player.z;
                    if (d14 * d14 + d16 * d16 + d18 * d18 < radius * radius)
                    {
                        player.playerNetServerHandler.SendPacket(packet);
                    }
                }
            }
        }

        public virtual void SendChatMessageToAllOps(string string1)
        {
            Packet3Chat packet3Chat2 = new Packet3Chat(string1);
            for (int i3 = 0; i3 < this.playerEntities.Count; ++i3)
            {
                ServerPlayer entityPlayerMP4 = this.playerEntities[i3];
                if (this.IsOp(entityPlayerMP4.username))
                {
                    entityPlayerMP4.playerNetServerHandler.SendPacket(packet3Chat2);
                }
            }
        }

        public virtual bool SendPacketToPlayer(string string1, Packet packet2)
        {
            ServerPlayer entityPlayerMP3 = this.GetPlayerEntity(string1);
            if (entityPlayerMP3 != null)
            {
                entityPlayerMP3.playerNetServerHandler.SendPacket(packet2);
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual void SavePlayerStates()
        {
            for (int i1 = 0; i1 < this.playerEntities.Count; ++i1)
            {
                this.playerIO.Write(this.playerEntities[i1]);
            }
        }

        public virtual void SendTileEntityToPlayer(int i1, int i2, int i3, TileEntity tileEntity4)
        {
        }

        public virtual void AddToWhiteList(string string1)
        {
            this.whiteListedIPs.Add(string1);
            this.SaveWhiteList();
        }

        public virtual void RemoveFromWhiteList(string string1)
        {
            this.whiteListedIPs.Remove(string1);
            this.SaveWhiteList();
        }

        public virtual HashSet<string> GetWhiteListedIPs()
        {
            return this.whiteListedIPs;
        }

        public virtual void ReloadWhiteList()
        {
            this.LoadWhiteList();
        }

        public virtual void Func_28170_a(ServerPlayer entityPlayerMP1, ServerLevel worldServer2)
        {
            entityPlayerMP1.playerNetServerHandler.SendPacket(new Packet4UpdateTime(worldServer2.GetTime()));
            if (worldServer2.Func_C())
            {
                entityPlayerMP1.playerNetServerHandler.SendPacket(new Packet70Bed(1));
            }
        }

        public virtual void Func_30008_g(ServerPlayer entityPlayerMP1)
        {
            entityPlayerMP1.Func_28017_a(entityPlayerMP1.inventorySlots);
            entityPlayerMP1.Func_30001_B();
        }
    }
}
