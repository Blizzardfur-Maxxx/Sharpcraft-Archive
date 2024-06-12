using SharpCraft.Core;
using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.Util.Logging;
using SharpCraft.Core.World.GameLevel.Storage;
using SharpCraft.Core.World.Phys;
using SharpCraft.Server.Commands;
using SharpCraft.Server.Config;
using SharpCraft.Server.Entities;
using SharpCraft.Server.Levell;
using SharpCraft.Server.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;

namespace SharpCraft.Server
{
    public class Server : ICommandSource
    {
        public static Logger logger = Logger.GetLogger(SharedConstants.LOGGER_NS);
        public static NullDictionary<string, int> wooded = new NullDictionary<string,int>();
        public ServerConnection networkServer;
        public ServerProperties properties;
        public ServerLevel[] levels;
        public ServerConfigurationManager configManager;
        private ConsoleCommandHandler commandHandler;
        private bool serverRunning = true;
        public bool serverStopped = false;
        int tickTime = 0;
        public string currentTask;
        public int percentDone;
        private IList<ITickable> tickables = new List<ITickable>();
        private IList<Command> commands = new SynchronizedCollection<Command>();
        public EntityTracker[] entityTracker = new EntityTracker[2];
        public bool onlineMode;
        public bool spawnPeacefulMobs;
        public bool pvpOn;
        public bool allowFlight;
        public int spawnProtectionRadius;

        public Server()
        {
            Thread timerHackThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(2147483647);
                    }
                    catch (Exception)
                    {
                    }
                }
            });
            timerHackThread.IsBackground = true;
            timerHackThread.Start();
        }

        private bool StartServer()
        {
            this.commandHandler = new ConsoleCommandHandler(this);
            Thread consoleReader = new Thread(new ThreadStart(() => 
            {
                TextReader reader = Console.In;
                string line = null;
                try 
                {
                    while (!serverStopped && serverRunning && (line = reader.ReadLine()) != null)
                    {
                        AddCommand(line, this);
                    }
                } catch (Exception ex) 
                {
                    ex.PrintStackTrace();
                }
            }));
            consoleReader.IsBackground = true;
            consoleReader.Start();
            Logging.Init();
            logger.Info("Starting server version " + SharedConstants.VER);
            logger.Info("Loading properties");
            this.properties = new ServerProperties(new JFile("server.properties"));
            string serverIp = this.properties.GetStringProperty("server-ip", "");
            this.onlineMode = this.properties.GetBooleanProperty("online-mode", false);
            this.spawnPeacefulMobs = this.properties.GetBooleanProperty("spawn-animals", true);
            this.pvpOn = this.properties.GetBooleanProperty("pvp", true);
            this.allowFlight = this.properties.GetBooleanProperty("allow-flight", false);
            this.spawnProtectionRadius = this.properties.GetIntProperty("spawn-protection-radius", 16);
            IPAddress ipaddress = IPAddress.Any;
            if (serverIp.Length > 0 && IPAddress.TryParse(serverIp, out IPAddress tmp))
            {
                ipaddress = tmp;
            }

            int port = this.properties.GetIntProperty("server-port", 25565);
            logger.Info("Starting server on " + (serverIp.Length == 0 ? "*" : serverIp) + ":" + port);
            try
            {
                this.networkServer = new ServerConnection(this, ipaddress, port);
            }
            catch (Exception ioe)
            {
                logger.Warning("**** FAILED TO BIND TO PORT!");
                logger.Log(LogLevel.WARNING, "The exception was: " + ioe.ToString());
                logger.Warning("Perhaps a server is already running on that port?");
                return false;
            }

            if (!this.onlineMode)
            {
                logger.Warning("**** SERVER IS RUNNING IN OFFLINE/INSECURE MODE!");
                logger.Warning("The server will make no attempt to authenticate usernames. Beware.");
                logger.Warning("While this makes the game possible to play without internet access, it also opens up the ability for hackers to connect with any username they choose.");
                logger.Warning("To change this, set \"online-mode\" to \"true\" in the server.properties file.");
            }

            this.configManager = new ServerConfigurationManager(this);
            this.entityTracker[0] = new EntityTracker(this, 0);
            this.entityTracker[1] = new EntityTracker(this, -1);
            long j5 = TimeUtil.NanoTime;
            string levelname = this.properties.GetStringProperty("level-name", "world");
            string levelseed = this.properties.GetStringProperty("level-seed", "");
            long seed = (new JRandom()).NextLong();
            if (levelseed.Length > 0)
            {
                try
                {
                    seed = long.Parse(levelseed);
                }
                catch (Exception)
                {
                    seed = Mth.GetJHashCode(levelseed);
                }
            }

            logger.Info("Preparing level \"" + levelname + "\"");
            this.LoadLevel(new MCRegionLevelStorageSource(new JFile(".")), levelname, seed);
            logger.Info("Done (" + (TimeUtil.NanoTime - j5) + "ns)! For help, type \"help\" or \"?\"");
            return true;
        }

        private void LoadLevel(ILevelStorageSource storagesrc, string levelname, long seed)
        {
            if (storagesrc.RequiresConversion(levelname))
            {
                logger.Info("Converting map!");
                storagesrc.ConvertLevel(levelname, new ConvertProgressUpdater());
            }

            this.levels = new ServerLevel[2];
            MCRegionLevelStorage storage = new MCRegionLevelStorage(new JFile("."), levelname, true);
            for (int i = 0; i < this.levels.Length; ++i)
            {
                if (i == 0)
                {
                    this.levels[i] = new ServerLevel(this, storage, levelname, ArrayIndexToDimension(i) , seed);
                }
                else
                {
                    this.levels[i] = new DerivedServerLevel(this, storage, levelname, ArrayIndexToDimension(i), seed, this.levels[0]);
                }

                this.levels[i].AddListener(new ServerLevelListener(this, this.levels[i]));
                this.levels[i].difficultySetting = this.properties.GetBooleanProperty("spawn-monsters", true) ? 1 : 0;
                this.levels[i].SetSpawnSettings(this.properties.GetBooleanProperty("spawn-monsters", true), this.spawnPeacefulMobs);
                this.configManager.SetPlayerIO(this.levels);
            }

            short initialRadius = 196;
            long startTime = TimeUtil.MilliTime;
            for (int i = 0; i < this.levels.Length; ++i)
            {
                logger.Info("Preparing start region for level " + i);
                if (i == 0 || this.properties.GetBooleanProperty("allow-nether", true))
                {
                    ServerLevel level = this.levels[i];
                    Pos spawnPos = level.GetSpawnPos();
                    for (int x = -initialRadius; x <= initialRadius && this.serverRunning; x += 16)
                    {
                        for (int z = -initialRadius; z <= initialRadius && this.serverRunning; z += 16)
                        {
                            long curTime = TimeUtil.MilliTime;
                            if (curTime < startTime)
                            {
                                startTime = curTime;
                            }

                            if (curTime > startTime + 1000)
                            {
                                int a = (initialRadius * 2 + 1) * (initialRadius * 2 + 1);
                                int b = (x + initialRadius) * (initialRadius * 2 + 1) + z + 1;
                                this.OutputPercentRemaining("Preparing spawn area", b * 100 / a);
                                startTime = curTime;
                            }

                            level.serverChunkCache.Create(spawnPos.x + x >> 4, spawnPos.z + z >> 4);
                            while (level.UpdateLights() && this.serverRunning)
                            {
                            }
                        }
                    }
                }
            }

            this.ClearCurrentTask();
        }

        private sealed class ConvertProgressUpdater : IProgressListener
        {
            private long lastTimeMillis;
            public ConvertProgressUpdater()
            {
                lastTimeMillis = TimeUtil.MilliTime;
            }

            public void StartLoading(string str)
            {
            }

            public void DisplayLoadingString(string str)
            {
            }

            public void SetLoadingProgress(int i)
            {
                if (TimeUtil.MilliTime - this.lastTimeMillis >= 1000)
                {
                    this.lastTimeMillis = TimeUtil.MilliTime;
                    logger.Info("Converting... " + i + "%");
                }
            }
        }

        private void OutputPercentRemaining(string task, int percent)
        {
            this.currentTask = task;
            this.percentDone = percent;
            logger.Info(task + ": " + percent + "%");
        }

        private void ClearCurrentTask()
        {
            this.currentTask = null;
            this.percentDone = 0;
        }

        private void SaveServerWorld()
        {
            logger.Info("Saving chunks");
            for (int i1 = 0; i1 < this.levels.Length; ++i1)
            {
                ServerLevel level = this.levels[i1];
                level.Save(true, null);
                level.CloseAll();
            }
        }

        private void StopServer()
        {
            logger.Info("Stopping server");
            if (this.configManager != null)
            {
                this.configManager.SavePlayerStates();
            }

            for (int i1 = 0; i1 < this.levels.Length; ++i1)
            {
                ServerLevel worldServer2 = this.levels[i1];
                if (worldServer2 != null)
                {
                    this.SaveServerWorld();
                }
            }
        }

        public virtual void InitiateShutdown()
        {
            this.serverRunning = false;
        }

        void Run()
        {
            try
            {
                if (this.StartServer())
                {
                    long j1 = TimeUtil.MilliTime;
                    for (long j3 = 0L; this.serverRunning; Thread.Sleep(1))
                    {
                        long j5 = TimeUtil.MilliTime;
                        long j7 = j5 - j1;
                        if (j7 > 2000)
                        {
                            logger.Warning("Can't keep up! Did the system time change, or is the server overloaded?");
                            j7 = 2000;
                        }

                        if (j7 < 0)
                        {
                            logger.Warning("Time ran backwards! Did the system time change?");
                            j7 = 0;
                        }

                        j3 += j7;
                        j1 = j5;
                        if (this.levels[0].IsAllPlayersFullyAsleep())
                        {
                            this.Tick();
                            j3 = 0;
                        }
                        else
                        {
                            while (j3 > 50)
                            {
                                j3 -= 50;
                                this.Tick();
                            }
                        }
                    }
                }
                else
                {
                    while (this.serverRunning)
                    {
                        this.CommandLineParser();
                        try
                        {
                            Thread.Sleep(10);
                        }
                        catch (ThreadInterruptedException ie)
                        {
                            ie.PrintStackTrace();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                e.PrintStackTrace();
                logger.Log(LogLevel.ERROR, "Unexpected exception", e);
                while (this.serverRunning)
                {
                    this.CommandLineParser();
                    try
                    {
                        Thread.Sleep(10);
                    }
                    catch (ThreadInterruptedException ie)
                    {
                        ie.PrintStackTrace();
                    }
                }
            }
            finally
            {
                try
                {
                    this.StopServer();
                    this.serverStopped = true;
                }
                catch (Exception e)
                {
                    e.PrintStackTrace();
                }
                finally
                {
                    Environment.Exit(0);
                }
            }
        }

        private void Tick()
        {
            List<string> removals = new List<string>();
            foreach (string name in wooded.Keys)
            {
                int j = wooded[name];
                if (j > 0)
                {
                    wooded[name] = j - 1;
                }
                else
                {
                    removals.Add(name);
                }
            }

            int i;
            for (i = 0; i < removals.Count; ++i)
            {
                wooded.Remove(removals[i]);
            }

            AABB.ClearBBPool();
            Vec3.ClearV3Pool();
            ++this.tickTime;
            for (i = 0; i < this.levels.Length; ++i)
            {
                if (i == 0 || this.properties.GetBooleanProperty("allow-nether", true))
                {
                    ServerLevel level = this.levels[i];
                    if (this.tickTime % 20 == 0)
                    {
                        this.configManager.SendPacketToAllPlayersInDimension(new Packet4UpdateTime(level.GetTime()), level.dimension.dimension);
                    }

                    level.Tick();
                    while (level.UpdateLights())
                    {
                    }

                    level.TickEntities();
                }
            }

            this.networkServer.Tick();
            this.configManager.Tick();
            for (i = 0; i < this.entityTracker.Length; ++i)
            {
                this.entityTracker[i].Tick();
            }

            for (i = 0; i < this.tickables.Count; ++i)
            {
                this.tickables[i].Tick();
            }

            try
            {
                this.CommandLineParser();
            }
            catch (Exception e)
            {
                logger.Log(LogLevel.WARNING, "Unexpected exception while parsing console command", e);
            }
        }

        public virtual void AddCommand(string command, ICommandSource source)
        {
            this.commands.Add(new Command(command, source));
        }

        public virtual void CommandLineParser()
        {
            while (this.commands.Count > 0)
            {
                Command c = this.commands[0];
                this.commands.RemoveAt(0);
                this.commandHandler.HandleCommand(c);
            }
        }

        public virtual void AddTickable(ITickable tickable)
        {
            this.tickables.Add(tickable);
        }

        public static void Main(String[] args)
        {
            StatList.Init();
            try
            {
                Server server = new Server();
                //if (!GraphicsEnvironment.IsHeadless() && (args.length <= 0 || !args[0].Equals("nogui")))
                //{
                //ServerGUI.InitGui(server);
                //}

                Thread srvThread = new Thread(new ThreadStart(server.Run));
                srvThread.Start();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.ERROR, "Failed to start the server", ex);
            }
        }

        public virtual JFile GetFile(string string1)
        {
            return new JFile(string1);
        }

        public virtual void Log(string string1)
        {
            logger.Info(string1);
        }

        public virtual void LogWarning(string string1)
        {
            logger.Warning(string1);
        }

        public virtual string GetUsername()
        {
            return "CONSOLE";
        }

        public virtual ServerLevel GetLevel(int dimension)
        {
            return levels[DimensionToArrayIndex(dimension)];
        }

        public virtual EntityTracker GetEntityTracker(int dimension)
        {
            return entityTracker[DimensionToArrayIndex(dimension)];
        }

        public static int ArrayIndexToDimension(int idx) 
        {
            if (idx == 0)
            {
                return 0;
            }
            else if (idx == 1)
            {
                return -1;
            }
            else if (idx == 2) 
            {
                return 1;
            }
            return idx;
        }

        public static int DimensionToArrayIndex(int dimension)
        {
            //check reserved dimension slots just in case
            if (dimension == 0) //normal/overworld
            {
                return 0;
            }
            else if (dimension == -1) //nether
            {
                return 1;
            }
            else if (dimension == 1) //sky/end
            {
                return 2;
            }
            else 
            {
                //return the dimension if the id is not reserved
                if (dimension < 0) throw new ArgumentException("Dimension id must not be negative!");
                return dimension;
            }
        }
    }
}
