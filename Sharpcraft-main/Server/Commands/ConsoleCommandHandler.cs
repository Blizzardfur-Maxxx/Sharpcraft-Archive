using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util.Logging;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Items;
using SharpCraft.Server.Config;
using SharpCraft.Server.Entities;
using SharpCraft.Server.Levell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Core;

namespace SharpCraft.Server.Commands
{
    public class ConsoleCommandHandler
    {
        private static Logger logger = Logger.GetLogger(SharedConstants.LOGGER_NS);
        private Server server;
        public ConsoleCommandHandler(Server srv)
        {
            this.server = srv;
        }

        public virtual void HandleCommand(Command serverCommand1)
        {
            string cmd = serverCommand1.command;
            ICommandSource src = serverCommand1.source;
            string user = src.GetUsername();
            ServerConfigurationManager cfgmgr = this.server.configManager;

            if (!cmd.ToLower().StartsWith("help") && !cmd.ToLower().StartsWith("?"))
            {
                if (cmd.ToLower().StartsWith("list"))
                {
                    src.Log("Connected players: " + cfgmgr.GetPlayerList());
                }
                else if (cmd.ToLower().StartsWith("stop"))
                {
                    this.SendNoticeToOps(user, "Stopping the server..");
                    this.server.InitiateShutdown();
                }
                else
                {
                    int i6;
                    ServerLevel worldServer7;
                    if (cmd.ToLower().StartsWith("save-all"))
                    {
                        this.SendNoticeToOps(user, "Forcing save..");
                        if (cfgmgr != null)
                        {
                            cfgmgr.SavePlayerStates();
                        }

                        for (i6 = 0; i6 < this.server.levels.Length; ++i6)
                        {
                            worldServer7 = this.server.levels[i6];
                            worldServer7.Save(true, null);
                        }

                        this.SendNoticeToOps(user, "Save complete.");
                    }
                    else if (cmd.ToLower().StartsWith("save-off"))
                    {
                        this.SendNoticeToOps(user, "Disabling level saving..");
                        for (i6 = 0; i6 < this.server.levels.Length; ++i6)
                        {
                            worldServer7 = this.server.levels[i6];
                            worldServer7.saveDisabled = true;
                        }
                    }
                    else if (cmd.ToLower().StartsWith("save-on"))
                    {
                        this.SendNoticeToOps(user, "Enabling level saving..");
                        for (i6 = 0; i6 < this.server.levels.Length; ++i6)
                        {
                            worldServer7 = this.server.levels[i6];
                            worldServer7.saveDisabled = false;
                        }
                    }
                    else
                    {
                        string string13;
                        if (cmd.ToLower().StartsWith("op "))
                        {
                            string13 = cmd.Substring(cmd.IndexOf(" ")).Trim();
                            cfgmgr.OpPlayer(string13);
                            this.SendNoticeToOps(user, "Opping " + string13);
                            cfgmgr.SendChatMessageToPlayer(string13, "§eYou are now op!");
                        }
                        else if (cmd.ToLower().StartsWith("deop "))
                        {
                            string13 = cmd.Substring(cmd.IndexOf(" ")).Trim();
                            cfgmgr.DeopPlayer(string13);
                            cfgmgr.SendChatMessageToPlayer(string13, "§eYou are no longer op!");
                            this.SendNoticeToOps(user, "De-opping " + string13);
                        }
                        else if (cmd.ToLower().StartsWith("ban-ip "))
                        {
                            string13 = cmd.Substring(cmd.IndexOf(" ")).Trim();
                            cfgmgr.BanIP(string13);
                            this.SendNoticeToOps(user, "Banning ip " + string13);
                        }
                        else if (cmd.ToLower().StartsWith("pardon-ip "))
                        {
                            string13 = cmd.Substring(cmd.IndexOf(" ")).Trim();
                            cfgmgr.PardonIP(string13);
                            this.SendNoticeToOps(user, "Pardoning ip " + string13);
                        }
                        else
                        {
                            ServerPlayer entityPlayerMP14;
                            if (cmd.ToLower().StartsWith("ban "))
                            {
                                string13 = cmd.Substring(cmd.IndexOf(" ")).Trim();
                                cfgmgr.BanPlayer(string13);
                                this.SendNoticeToOps(user, "Banning " + string13);
                                entityPlayerMP14 = cfgmgr.GetPlayerEntity(string13);
                                if (entityPlayerMP14 != null)
                                {
                                    entityPlayerMP14.playerNetServerHandler.KickPlayer("Banned by admin");
                                }
                            }
                            else if (cmd.ToLower().StartsWith("pardon "))
                            {
                                string13 = cmd.Substring(cmd.IndexOf(" ")).Trim();
                                cfgmgr.PardonPlayer(string13);
                                this.SendNoticeToOps(user, "Pardoning " + string13);
                            }
                            else
                            {
                                int i8;
                                if (cmd.ToLower().StartsWith("kick "))
                                {
                                    string13 = cmd.Substring(cmd.IndexOf(" ")).Trim();
                                    entityPlayerMP14 = null;
                                    for (i8 = 0; i8 < cfgmgr.playerEntities.Count; ++i8)
                                    {
                                        ServerPlayer entityPlayerMP9 = cfgmgr.playerEntities[i8];
                                        if (entityPlayerMP9.username.ToLower().Equals(string13))
                                        {
                                            entityPlayerMP14 = entityPlayerMP9;
                                        }
                                    }

                                    if (entityPlayerMP14 != null)
                                    {
                                        entityPlayerMP14.playerNetServerHandler.KickPlayer("Kicked by admin");
                                        this.SendNoticeToOps(user, "Kicking " + entityPlayerMP14.username);
                                    }
                                    else
                                    {
                                        src.Log("Can't find user " + string13 + ". No kick.");
                                    }
                                }
                                else
                                {
                                    ServerPlayer entityPlayerMP15;
                                    String[] string18;
                                    if (cmd.ToLower().StartsWith("tp "))
                                    {
                                        string18 = cmd.Split(" ");
                                        if (string18.Length == 3)
                                        {
                                            entityPlayerMP14 = cfgmgr.GetPlayerEntity(string18[1]);
                                            entityPlayerMP15 = cfgmgr.GetPlayerEntity(string18[2]);
                                            if (entityPlayerMP14 == null)
                                            {
                                                src.Log("Can't find user " + string18[1] + ". No tp.");
                                            }
                                            else if (entityPlayerMP15 == null)
                                            {
                                                src.Log("Can't find user " + string18[2] + ". No tp.");
                                            }
                                            else if (entityPlayerMP14.dimension != entityPlayerMP15.dimension)
                                            {
                                                src.Log("User " + string18[1] + " and " + string18[2] + " are in different dimensions. No tp.");
                                            }
                                            else
                                            {
                                                entityPlayerMP14.playerNetServerHandler.TeleportTo(entityPlayerMP15.x, entityPlayerMP15.y, entityPlayerMP15.z, entityPlayerMP15.yaw, entityPlayerMP15.pitch);
                                                this.SendNoticeToOps(user, "Teleporting " + string18[1] + " to " + string18[2] + ".");
                                            }
                                        }
                                        else
                                        {
                                            src.Log("Syntax error, please provice a source and a target.");
                                        }
                                    }
                                    else
                                    {
                                        string string16;
                                        int i17;
                                        if (cmd.ToLower().StartsWith("give "))
                                        {
                                            string18 = cmd.Split(" ");
                                            if (string18.Length != 3 && string18.Length != 4)
                                            {
                                                return;
                                            }

                                            string16 = string18[1];
                                            entityPlayerMP15 = cfgmgr.GetPlayerEntity(string16);
                                            if (entityPlayerMP15 != null)
                                            {
                                                try
                                                {
                                                    i17 = int.Parse(string18[2]);
                                                    if (Item.items[i17] != null)
                                                    {
                                                        this.SendNoticeToOps(user, "Giving " + entityPlayerMP15.username + " some " + i17);
                                                        int i10 = 1;
                                                        if (string18.Length > 3)
                                                        {
                                                            i10 = this.TryParse(string18[3], 1);
                                                        }

                                                        if (i10 < 1)
                                                        {
                                                            i10 = 1;
                                                        }

                                                        if (i10 > 64)
                                                        {
                                                            i10 = 64;
                                                        }

                                                        entityPlayerMP15.DropPlayerItem(new ItemInstance(i17, i10, 0));
                                                    }
                                                    else
                                                    {
                                                        src.Log("There's no item with id " + i17);
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    src.Log("There's no item with id " + string18[2]);
                                                }
                                            }
                                            else
                                            {
                                                src.Log("Can't find user " + string16);
                                            }
                                        }
                                        else if (cmd.ToLower().StartsWith("time "))
                                        {
                                            string18 = cmd.Split(" ");
                                            if (string18.Length != 3)
                                            {
                                                return;
                                            }

                                            string16 = string18[1];
                                            try
                                            {
                                                i8 = int.Parse(string18[2]);
                                                ServerLevel worldServer19;
                                                if ("add".ToLower().Equals(string16))
                                                {
                                                    for (i17 = 0; i17 < this.server.levels.Length; ++i17)
                                                    {
                                                        worldServer19 = this.server.levels[i17];
                                                        worldServer19.Func_32005(worldServer19.GetTime() + i8);
                                                    }

                                                    this.SendNoticeToOps(user, "Added " + i8 + " to time");
                                                }
                                                else if ("set".ToLower().Equals(string16))
                                                {
                                                    for (i17 = 0; i17 < this.server.levels.Length; ++i17)
                                                    {
                                                        worldServer19 = this.server.levels[i17];
                                                        worldServer19.Func_32005(i8);
                                                    }

                                                    this.SendNoticeToOps(user, "Set time to " + i8);
                                                }
                                                else
                                                {
                                                    src.Log("Unknown method, use either \"add\" or \"set\"");
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                src.Log("Unable to convert time value, " + string18[2]);
                                            }
                                        }
                                        else if (cmd.ToLower().StartsWith("say "))
                                        {
                                            cmd = cmd.Substring(cmd.IndexOf(" ")).Trim();
                                            logger.Info("[" + user + "] " + cmd);
                                            cfgmgr.SendPacketToAllPlayers(new Packet3Chat("§d[Server] " + cmd));
                                        }
                                        else if (cmd.ToLower().StartsWith("tell "))
                                        {
                                            string18 = cmd.Split(" ");
                                            if (string18.Length >= 3)
                                            {
                                                cmd = cmd.Substring(cmd.IndexOf(" ")).Trim();
                                                cmd = cmd.Substring(cmd.IndexOf(" ")).Trim();
                                                logger.Info("[" + user + "->" + string18[1] + "] " + cmd);
                                                cmd = "§7" + user + " whispers " + cmd;
                                                logger.Info(cmd);
                                                if (!cfgmgr.SendPacketToPlayer(string18[1], new Packet3Chat(cmd)))
                                                {
                                                    src.Log("There's no player by that name online.");
                                                }
                                            }
                                        }
                                        else if (cmd.ToLower().StartsWith("whitelist "))
                                        {
                                            this.HandleWhitelist(user, cmd, src);
                                        }
                                        else
                                        {
                                            logger.Info("Unknown console command. Type \"help\" for help.");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                this.PrintHelp(src);
            }
        }

        private void HandleWhitelist(string string1, string string2, ICommandSource iCommandListener3)
        {
            String[] string4 = string2.Split(" ");
            if (string4.Length >= 2)
            {
                string string5 = string4[1].ToLower();
                if ("on".Equals(string5))
                {
                    this.SendNoticeToOps(string1, "Turned on white-listing");
                    this.server.properties.SetProperty("white-list", true);
                }
                else if ("off".Equals(string5))
                {
                    this.SendNoticeToOps(string1, "Turned off white-listing");
                    this.server.properties.SetProperty("white-list", false);
                }
                else if ("list".Equals(string5))
                {
                    HashSet<string> set6 = this.server.configManager.GetWhiteListedIPs();
                    string string7 = "";
                    string string9;
                    for (IEnumerator<string> iterator8 = set6.GetEnumerator(); iterator8.MoveNext(); string7 = string7 + string9 + " ")
                    {
                        string9 = iterator8.Current;
                    }

                    iCommandListener3.Log("White-listed players: " + string7);
                }
                else
                {
                    string string10;
                    if ("add".Equals(string5) && string4.Length == 3)
                    {
                        string10 = string4[2].ToLower();
                        this.server.configManager.AddToWhiteList(string10);
                        this.SendNoticeToOps(string1, "Added " + string10 + " to white-list");
                    }
                    else if ("remove".Equals(string5) && string4.Length == 3)
                    {
                        string10 = string4[2].ToLower();
                        this.server.configManager.RemoveFromWhiteList(string10);
                        this.SendNoticeToOps(string1, "Removed " + string10 + " from white-list");
                    }
                    else if ("reload".Equals(string5))
                    {
                        this.server.configManager.ReloadWhiteList();
                        this.SendNoticeToOps(string1, "Reloaded white-list from file");
                    }
                }
            }
        }

        private void PrintHelp(ICommandSource iCommandListener1)
        {
            iCommandListener1.Log("Console commands:");
            iCommandListener1.Log("   help  or  ?               shows this message");
            iCommandListener1.Log("   kick <player>             removes a player from the server");
            iCommandListener1.Log("   ban <player>              bans a player from the server");
            iCommandListener1.Log("   pardon <player>           pardons a banned player so that they can connect again");
            iCommandListener1.Log("   ban-ip <ip>               bans an IP address from the server");
            iCommandListener1.Log("   pardon-ip <ip>            pardons a banned IP address so that they can connect again");
            iCommandListener1.Log("   op <player>               turns a player into an op");
            iCommandListener1.Log("   deop <player>             removes op status from a player");
            iCommandListener1.Log("   tp <player1> <player2>    moves one player to the same location as another player");
            iCommandListener1.Log("   give <player> <id> [num]  gives a player a resource");
            iCommandListener1.Log("   tell <player> <message>   sends a private message to a player");
            iCommandListener1.Log("   stop                      gracefully stops the server");
            iCommandListener1.Log("   save-all                  forces a server-wide level save");
            iCommandListener1.Log("   save-off                  disables terrain saving (useful for backup scripts)");
            iCommandListener1.Log("   save-on                   re-enables terrain saving");
            iCommandListener1.Log("   list                      lists all currently connected players");
            iCommandListener1.Log("   say <message>             broadcasts a message to all players");
            iCommandListener1.Log("   time <add|set> <amount>   adds to or sets the world time (0-24000)");
        }

        private void SendNoticeToOps(string string1, string string2)
        {
            string string3 = string1 + ": " + string2;
            this.server.configManager.SendChatMessageToAllOps("§7(" + string3 + ")");
            logger.Info(string3);
        }

        private int TryParse(string string1, int i2)
        {
            try
            {
                return int.Parse(string1);
            }
            catch (Exception)
            {
                return i2;
            }
        }
    }
}
