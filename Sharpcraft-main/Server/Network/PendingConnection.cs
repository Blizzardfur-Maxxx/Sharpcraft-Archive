using SharpCraft.Core;
using SharpCraft.Core.Network;
using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util;
using SharpCraft.Core.Util.Logging;
using SharpCraft.Server.Entities;
using SharpCraft.Server.Levell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCraft.Server.Network
{
    public class PendingConnection : PacketListener
    {
        public static Logger logger = Logger.GetLogger(SharedConstants.LOGGER_NS);
        private static JRandom rand = new JRandom();
        public Connection netConnection;
        public bool finishedProcessing = false;
        private Server mcServer;
        private int loginTimer = 0;
        private string username = null;
        private Packet1Login packet1login = null;
        private string serverId = "";
        public PendingConnection(Server srv, Socket socket2, string string3)
        {
            this.mcServer = srv;
            this.netConnection = new Connection(socket2, string3, this);
            this.netConnection.chunkDataSendCounter = 0;
        }

        public virtual void TryLogin()
        {
            if (this.packet1login != null)
            {
                this.DoLogin(this.packet1login);
                this.packet1login = null;
            }

            if (this.loginTimer++ == 600)
            {
                this.KickUser("Took too long to log in");
            }
            else
            {
                this.netConnection.Tick();
            }
        }

        public virtual void KickUser(string string1)
        {
            try
            {
                logger.Info("Disconnecting " + this.GetUserAndIPString() + ": " + string1);
                this.netConnection.AddToSendQueue(new Packet255KickDisconnect(string1));
                this.netConnection.Shutdown();
                this.finishedProcessing = true;
            }
            catch (Exception exception3)
            {
                exception3.PrintStackTrace();
            }
        }

        public override void HandleHandshake(Packet2Handshake packet2Handshake1)
        {
            if (this.mcServer.onlineMode)
            {
                this.serverId = BitConverter.ToString(BitConverter.GetBytes(rand.NextLong())).Replace("-", "").ToLower();
                this.netConnection.AddToSendQueue(new Packet2Handshake(this.serverId));
            }
            else
            {
                this.netConnection.AddToSendQueue(new Packet2Handshake("-"));
            }
        }

        public override void HandleLogin(Packet1Login packet)
        {
            this.username = packet.username;
            if (packet.protocolVersion != SharedConstants.PROTOCOL_VERSION)
            {
                if (packet.protocolVersion > SharedConstants.PROTOCOL_VERSION)
                {
                    this.KickUser("Outdated server!");
                }
                else
                {
                    this.KickUser("Outdated client!");
                }
            }
            else
            {
                if (!this.mcServer.onlineMode)
                {
                    this.DoLogin(packet);
                }
                else
                {
                    Thread thread = new Thread(new ThreadStart( () => 
                    {
                        try 
                        {
                            string srvId = serverId;
                            using HttpResponseMessage mesg = SharedConstants.HTTP_CLIENT.GetAsync(String.Format(SharedConstants.CHECKSERVER, packet.username, srvId)).Result;
                            using Stream strm = mesg.Content.ReadAsStream();
                            TextReader reader = new StreamReader(strm);
                            string line = reader.ReadLine();
                            reader.Dispose();
                            if (line.Equals("YES"))
                            {
                                packet1login = packet;
                            }
                            else 
                            {
                                KickUser("Failed to verify username!");
                            }
                        } 
                        catch (Exception e) 
                        {
                            KickUser($"Failed to verify username! [internal error {e}]");
                            e.PrintStackTrace();
                        }
                    }));
                    thread.Start();
                }
            }
        }

        public virtual void DoLogin(Packet1Login packet1Login1)
        {
            ServerPlayer entityPlayerMP2 = this.mcServer.configManager.Login(this, packet1Login1.username);
            if (entityPlayerMP2 != null)
            {
                this.mcServer.configManager.ReadPlayerDataFromFile(entityPlayerMP2);
                entityPlayerMP2.SetWorld(this.mcServer.GetLevel(entityPlayerMP2.dimension));
                logger.Info(this.GetUserAndIPString() + " logged in with entity id " + entityPlayerMP2.entityID + " at (" + entityPlayerMP2.x + ", " + entityPlayerMP2.y + ", " + entityPlayerMP2.z + ")");
                ServerLevel worldServer3 = this.mcServer.GetLevel(entityPlayerMP2.dimension);
                Pos chunkCoordinates4 = worldServer3.GetSpawnPos();
                PlayerConnection netServerHandler5 = new PlayerConnection(this.mcServer, this.netConnection, entityPlayerMP2);
                netServerHandler5.SendPacket(new Packet1Login("", entityPlayerMP2.entityID, worldServer3.GetRandomSeed(), (sbyte)worldServer3.dimension.dimension));
                netServerHandler5.SendPacket(new Packet6SpawnPosition(chunkCoordinates4.x, chunkCoordinates4.y, chunkCoordinates4.z));
                this.mcServer.configManager.Func_28170_a(entityPlayerMP2, worldServer3);
                this.mcServer.configManager.SendPacketToAllPlayers(new Packet3Chat("§e" + entityPlayerMP2.username + " joined the game."));
                this.mcServer.configManager.PlayerLoggedIn(entityPlayerMP2);
                netServerHandler5.TeleportTo(entityPlayerMP2.x, entityPlayerMP2.y, entityPlayerMP2.z, entityPlayerMP2.yaw, entityPlayerMP2.pitch);
                this.mcServer.networkServer.AddPlayerConnection(netServerHandler5);
                netServerHandler5.SendPacket(new Packet4UpdateTime(worldServer3.GetTime()));
                entityPlayerMP2.Func_20057_k();
            }

            this.finishedProcessing = true;
        }

        public override void OnDisconnect(string string1, params object[] object2)
        {
            logger.Info(this.GetUserAndIPString() + " lost connection");
            this.finishedProcessing = true;
        }

        protected override void OnUnhandledPacket(Packet packet1)
        {
            this.KickUser("Protocol error");
        }

        public virtual string GetUserAndIPString()
        {
            return this.username != null ? this.username + " [" + this.netConnection.GetRemoteAddress().ToString() + "]" : this.netConnection.GetRemoteAddress().ToString();
        }

        public override bool IsServerPacketListener()
        {
            return true;
        }
    }
}
