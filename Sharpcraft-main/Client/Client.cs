using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using SharpCraft.Client.Renderer;
using SharpCraft.Client.GUI;
using SharpCraft.Core;
using LWCSGL.OpenGL;
using LWCSGL.Input;
using static LWCSGL.OpenGL.GL11;
using static LWCSGL.OpenGL.GL11C;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Phys;
using SharpCraft.Client.Gamemode;
using SharpCraft.Client.Particles;
using SharpCraft.Client.Sound;
using SharpCraft.Client.Players;
using SharpCraft.Core.Stats;
using SharpCraft.Client.Models;
using SharpCraft.Client.Texturepack;
using SharpCraft.Core.World.GameLevel.Storage;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using SharpCraft.Core.World.Entities;
using SharpCraft.Client.Stats;
using SharpCraft.Client.GUI.Screens.Achievements;
using SharpCraft.Client.GUI.Screens;
using SharpCraft.Client.Renderer.Ptexture;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Client.GUI.Screens.Multiplayer;
using static SharpCraft.Client.Options;
using SharpCraft.Core.World.GameLevel.Dimensions;
using SharpCraft.Client.GUI.Screens.Inventories;
using System.Net.Http;
using SharpCraft.Client.Network;
using System.Reflection;
using System.Diagnostics;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Client
{
    public class Client
    {
        public const string VERSION_COMMIT = "0000000000000000000000000000000000000000";
        public const int DEFAULT_WIDTH = 854;
        public const int DEFAULT_HEIGHT = 480;
        private static Client instance;
        public GameMode gameMode;
        private bool fullscreen = false;
        private bool crashed = false;
        public int displayWidth;
        public int displayHeight;
        private GLCapabilities glCapabilities;
        private Timer timer = new Timer(20F);
        public Level level;
        public LevelRenderer renderGlobal;
        public LocalPlayer player;
        public Mob renderViewEntity;
        public ParticleEngine effectRenderer;
        public User user = null;
        public bool hideQuitButton = true;
        public volatile bool pause = false;
        public Textures textures;
        public GUI.Font font;
        public Screen currentScreen = null;
        public ProgressRenderer loadingScreen;
        public GameRenderer entityRenderer;
        private ResourceDownloadThread resourceDownloadThread;
        private int ticksRan = 0;
        private int leftClickCounter = 0;
        private int tempDisplayWidth;
        private int tempDisplayHeight;
        public AchivementWidget guiAchievement;
        public Gui ingameGUI;
        public bool skipRenderWorld = false;
        public ModelBiped field_9242_w = new ModelBiped(0F);
        public HitResult objectMouseOver = null;
        public Options options;
        public SoundEngine soundEngine = new SoundEngine();
        public MouseInput mouseHelper;
        public TexturePackRepository texturePackRepo;
        private JFile workingDirectory;
        private ILevelStorageSource lvlStorageSrc;
        public static long[] frameTimes = new long[512];
        public static long[] tickTimes = new long[512];
        public static int numRecordedFrameTimes = 0;
        public static long licenseCheckTime = 0;
        public StatFileWriter statFileWriter;
        private string serverIp;
        private int serverPort;
        private DynamicTexture waterTexture = (Enhancements.WATER_BIOME_COLOR ? new GreyscaleWaterTexture() : new WaterTexture());
        private LavaTexture lavaTexture = new LavaTexture();
        private static JFile workDir = null;
        public volatile bool running = true;
        public string fpsString = "";
        public int fps;
        bool isTakingScreenshot = false;
        long prevFrameTime = -1;
        public bool inGameHasFocus = false;
        private int mouseTicksRan = 0;
        public bool isRaining = false;
        long systemTime = TimeUtil.MilliTime;
        private int joinPlayerCounter = 0;
        private string debugProfilerName = "root";
        private static Icon windowIcon;

        public static Icon WindowIcon
        {
            get
            {
                try
                {
                    if (windowIcon == null)
                        windowIcon = Icon.ExtractAssociatedIcon(Environment.ProcessPath);
                    return windowIcon;
                }
                catch (Exception ex) 
                {
                    ex.PrintStackTrace();
                    return null;
                }
            }
        }

        public enum OS
        {
            linux,
            solaris,
            windows,
            macos,
            unknown
        }

        public Client(int width, int height, bool fullscreen)
        {
            StatList.Init();
            this.tempDisplayHeight = height;
            this.fullscreen = fullscreen;

            Thread timerHackThread = new Thread(() => 
            {
                while (this.running)
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
            timerHackThread.Name = "Timer hack thread";
            timerHackThread.IsBackground = true;
            timerHackThread.Start();
            this.displayWidth = width;
            this.displayHeight = height;
            this.fullscreen = fullscreen;
            if (true)
            {
                this.hideQuitButton = false;
            }

            guiAchievement = new AchivementWidget(this);
            loadingScreen = new ProgressRenderer(this);
            instance = this;
        }

        public virtual void OnCrash(CrashReport unexpectedThrowable1)
        {
            this.crashed = true;
            this.Crash(unexpectedThrowable1);
        }

        public virtual void Crash(CrashReport crashreport)
        {
            Display.GetInternalForm().Hide();
            Mouse.SetGrabbed(false);
            new CrashForm(crashreport).Show();
            while (true) Display.ProcessMessages();
        }

        public virtual void SetServer(string serverIp, int port)
        {
            this.serverIp = serverIp;
            this.serverPort = port;
        }

        private class InventoryAchievementStatFormatter : IStatFormatter
        {
            private Client c;

            public InventoryAchievementStatFormatter(Client c) 
            {
                this.c = c;
            }

            public string Format(string str)
            {
                return string.Format(str, Keyboard.GetKeyName(c.options.keyBindInventory.keyCode));
            }
        }

        public virtual void StartGame()
        {
            if (this.fullscreen)
            {
                Display.SetFullscreen(true);
                this.displayWidth = Display.GetDisplayMode().GetWidth();
                this.displayHeight = Display.GetDisplayMode().GetHeight();
                if (this.displayWidth <= 0)
                {
                    this.displayWidth = 1;
                }

                if (this.displayHeight <= 0)
                {
                    this.displayHeight = 1;
                }
            }
            else
            {
                Display.SetDisplayMode(new DisplayMode(this.displayWidth, this.displayHeight));
            }

            Display.SetTitle(SharedConstants.VERSION_STRING);
            try
            {
                Display.SetIcon(WindowIcon);
            }
            catch (Exception)
            {
            }

            Display.Create();
            Display.SetResizable(true);

            this.workingDirectory = GetWorkingDirectory();
            Console.WriteLine($"Working directory: {workingDirectory.GetAbsolutePath()}");
            this.lvlStorageSrc = new MCRegionLevelStorageSource(new JFile(this.workingDirectory, "saves"));
            this.options = new Options(this, this.workingDirectory);
            this.texturePackRepo = new TexturePackRepository(this, this.workingDirectory);
            this.textures = new Textures(this.texturePackRepo, this.options);
            this.font = new GUI.Font(this.options, "/font/default.png", this.textures);
            WaterColor.SetColorBuffer(this.textures.LoadTexturePixels("/misc/watercolor.png"));
            GrassColor.SetColorBuffer(this.textures.LoadTexturePixels("/misc/grasscolor.png"));
            FoliageColor.SetColorBuffer(this.textures.LoadTexturePixels("/misc/foliagecolor.png"));
            this.entityRenderer = new GameRenderer(this);
            EntityRenderDispatcher.instance.itemRenderer = new ItemInHandRenderer(this);
            this.statFileWriter = new StatFileWriter(this.user, workingDirectory);
            AchievementList.openInventory.SetFormatter(new InventoryAchievementStatFormatter(this));
            this.LoadScreen();
            Keyboard.Create();
            Mouse.Create();
            this.mouseHelper = new MouseInput();
            this.CheckGLError("Pre startup");
            GL11.glEnable(GL11C.GL_TEXTURE_2D);
            GL11.glShadeModel(GL11C.GL_SMOOTH);
            GL11.glClearDepth(1);
            GL11.glEnable(GL11C.GL_DEPTH_TEST);
            GL11.glDepthFunc(GL11C.GL_LEQUAL);
            GL11.glEnable(GL11C.GL_ALPHA_TEST);
            GL11.glAlphaFunc(GL11C.GL_GREATER, 0.1F);
            GL11.glCullFace(GL11C.GL_BACK);
            GL11.glMatrixMode(GL11C.GL_PROJECTION);
            GL11.glLoadIdentity();
            GL11.glMatrixMode(GL11C.GL_MODELVIEW);
            this.CheckGLError("Startup");
            this.glCapabilities = new GLCapabilities();
            this.soundEngine.Init(this.options);
            this.textures.AddDynamicTexture(this.lavaTexture);
            this.textures.AddDynamicTexture(this.waterTexture);
            this.textures.AddDynamicTexture(new PortalTexture());
            this.textures.AddDynamicTexture(new CompassTexture(this));
            this.textures.AddDynamicTexture(new ClockTexture(this));
            this.textures.AddDynamicTexture((Enhancements.WATER_BIOME_COLOR ? new GreyscaleWaterSideTexture() : new WaterSideTexture()));
            this.textures.AddDynamicTexture(new LavaSideTexture());
            this.textures.AddDynamicTexture(new FireTexture(0));
            this.textures.AddDynamicTexture(new FireTexture(1));
            this.renderGlobal = new LevelRenderer(this, this.textures);
            GL11.glViewport(0, 0, this.displayWidth, this.displayHeight);
            this.effectRenderer = new ParticleEngine(this.level, this.textures);
            this.InitBackgroundDownloader();
            this.CheckGLError("Post startup");
            this.ingameGUI = new Gui(this);

            if (this.serverIp != null)
            {
                this.SetScreen(new ConnectScreen(this, this.serverIp, this.serverPort));
            }
            else
            {
                this.SetScreen(new StartMenuScreen());
            }
        }

        private void InitBackgroundDownloader()
        {
            try
            {
                if (this.resourceDownloadThread != null && this.resourceDownloadThread.IsAlive())
                {
                    this.resourceDownloadThread.Dispose();
                }

                if (this.resourceDownloadThread == null || !this.resourceDownloadThread.IsAlive())
                {
                    this.resourceDownloadThread = new ResourceDownloadThread(this, this.workingDirectory);
                    this.resourceDownloadThread.Start();
                }
            }
            catch (Exception)
            {
            }
        }

        private void LoadScreen()
        {
            GuiScale scale = new GuiScale(this.options, this.displayWidth, this.displayHeight);
            GL11.glClear(GL11C.GL_COLOR_BUFFER_BIT | GL11C.GL_DEPTH_BUFFER_BIT);
            GL11.glMatrixMode(GL11C.GL_PROJECTION);
            GL11.glLoadIdentity();
            GL11.glOrtho(0, scale.WidthScale, scale.HeightScale, 0, 1000, 3000);
            GL11.glMatrixMode(GL11C.GL_MODELVIEW);
            GL11.glLoadIdentity();
            GL11.glTranslatef(0F, 0F, -2000F);
            GL11.glViewport(0, 0, this.displayWidth, this.displayHeight);
            GL11.glClearColor(0F, 0F, 0F, 0F);
            Tessellator t = Tessellator.Instance;
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glEnable(GL11C.GL_TEXTURE_2D);
            GL11.glDisable(GL11C.GL_FOG);
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.textures.LoadTexture("/title/mojang.png"));
            t.Begin();
            t.Color(0xFFFFFF);
            t.VertexUV(0, this.displayHeight, 0, 0, 0);
            t.VertexUV(this.displayWidth, this.displayHeight, 0, 0, 0);
            t.VertexUV(this.displayWidth, 0, 0, 0, 0);
            t.VertexUV(0, 0, 0, 0, 0);
            t.End();
            short s3 = 256;
            short s4 = 256;
            GL11.glColor4f(1F, 1F, 1F, 1F);
            t.Color(0xFFFFFF);
            this.Func_6274_a((scale.GetWidth() - s3) / 2, (scale.GetHeight() - s4) / 2, 0, 0, s3, s4);
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glDisable(GL11C.GL_FOG);
            GL11.glEnable(GL11C.GL_ALPHA_TEST);
            GL11.glAlphaFunc(GL11C.GL_GREATER, 0.1F);
            Display.SwapBuffers();
        }

        public virtual void Func_6274_a(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            float u = 0.00390625F;
            float v = 0.00390625F;
            Tessellator t = Tessellator.Instance;
            t.Begin();
            t.VertexUV(i1 + 0, i2 + i6, 0, (i3 + 0) * u, (i4 + i6) * v);
            t.VertexUV(i1 + i5, i2 + i6, 0, (i3 + i5) * u, (i4 + i6) * v);
            t.VertexUV(i1 + i5, i2 + 0, 0, (i3 + i5) * u, (i4 + 0) * v);
            t.VertexUV(i1 + 0, i2 + 0, 0, (i3 + 0) * u, (i4 + 0) * v);
            t.End();
        }

        public static JFile GetWorkingDirectory()
        {
            if (workDir == null)
            {
                workDir = GetWorkingDirectory("sharpcraft");
            }

            return workDir;
        }

        public static JFile GetWorkingDirectory(string applicationName)
        {
            JFile dir = new JFile(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), $".{applicationName}");

            if (!dir.Exists() && !dir.Mkdir())
            {
                throw new Exception("The working directory could not be created: " + dir);
            }
            else
            {
                return dir;
            }
        }

        private static OS GetOs()
        {
            switch (Environment.OSVersion.Platform) 
            {
                case PlatformID.Win32NT:
                    return OS.windows;
                case PlatformID.Unix:
                    return OS.linux;
                default:
                    // I don't think .NET works on tim cock software
                    // Or they don't give a fuck and just couple it with "Unix"
                    return OS.unknown;
            }
        }

        public virtual ILevelStorageSource GetSaveLoader()
        {
            return this.lvlStorageSrc;
        }

        public virtual void SetScreen(Screen screen)
        {
            if (!(this.currentScreen is ErrorScreen))
            {
                if (this.currentScreen != null)
                {
                    this.currentScreen.OnGuiClosed();
                }

                if (screen is StartMenuScreen)
                {
                    this.statFileWriter.Dispose();
                }

                this.statFileWriter.SyncStats();
                if (screen == null && this.level == null)
                {
                    screen = new StartMenuScreen();
                }
                else if (screen == null && this.player.health <= 0)
                {
                    screen = new DeathScreen();
                }

                if (screen is StartMenuScreen)
                {
                    this.ingameGUI.ClearChatMessages();
                }

                this.currentScreen = screen;
                if (screen != null)
                {
                    this.SetIngameNotInFocus();
                    GuiScale scale = new GuiScale(this.options, this.displayWidth, this.displayHeight);
                    int w = scale.GetWidth();
                    int h = scale.GetHeight();
                    screen.Init(this, w, h);
                    this.skipRenderWorld = false;
                }
                else
                {
                    this.SetIngameFocus();
                }
            }
        }

        private void CheckGLError(string str)
        {
            uint glError = GL11.glGetError();
            if (glError != 0)
            {
                string errString = GLU.gluErrorString(glError);
                Console.Error.WriteLine("########## GL ERROR ##########");
                Console.Error.WriteLine("@ " + str);
                Console.Error.WriteLine(glError + ": " + errString);
            }
        }

        public virtual void Destroy()
        {
            try
            {
                this.statFileWriter.Dispose();
                this.statFileWriter.SyncStats();
                try
                {
                    if (this.resourceDownloadThread != null)
                    {
                        this.resourceDownloadThread.Dispose();
                    }
                }
                catch (Exception)
                {
                }

                Console.WriteLine("Stopping!");
                try
                {
                    this.SetLevel(null);
                }
                catch (Exception)
                {
                }

                try
                {
                    MemoryTracker.Release();
                }
                catch (Exception)
                {
                }

                this.soundEngine.Destroy();
                Mouse.Destroy();
                Keyboard.Destroy();
            }
            finally
            {
                Display.Destroy();
                if (!this.crashed)
                {
                    Environment.Exit(0);
                }
            }

            GC.Collect();
        }

        [STAThread]
        public void Run()
        {
            running = true;
            try
            {
                StartGame();
            }
            catch (Exception ex)
            {
                ex.PrintStackTrace();
                OnCrash(new CrashReport("Failed to start game", ex));
                return;
            }

            try
            {
                long j1 = TimeUtil.MilliTime;
                int fps = 0;
                while (running)
                {
                    try
                    {
                        AABB.ClearBBPool();
                        Vec3.ClearV3Pool();
                        Profiler.StartSection("root");
                        if (Display.IsCloseRequested())
                        {
                            Shutdown();
                        }

                        if (pause && level != null)
                        {
                            float f4 = timer.renderPartialTicks;
                            timer.UpdateTimer();
                            timer.renderPartialTicks = f4;
                        }
                        else
                        {
                            timer.UpdateTimer();
                        }

                        long nt = TimeUtil.NanoTime;
                        Profiler.StartSection("tick");

                        for (int tick = 0; tick < timer.elapsedTicks; ++tick)
                        {
                            ++ticksRan;
                            try
                            {
                                Tick();
                            }
                            catch (LevelConflictException)
                            {
                                level = null;
                                SetLevel((Level)null);
                                SetScreen(new LevelConflictScreen());
                            }
                        }

                        Profiler.EndSection();
                        long j24 = TimeUtil.NanoTime - nt;
                        CheckGLError("Pre render");
                        TileRenderer.fancyGrass = options.fancyGraphics;
                        Profiler.StartSection("sound");
                        soundEngine.Update(player, timer.renderPartialTicks);
                        Profiler.EndStartSection("updatelights");
                        if (level != null)
                        {
                            level.UpdateLights();
                        }
                        Profiler.EndSection();
                        Profiler.StartSection("render");
                        Profiler.StartSection("display");
                        GL11.glEnable(GL11C.GL_TEXTURE_2D);
                        if (!Keyboard.IsKeyDown(VirtualKey.F7))
                        {
                            Display.Update();
                        }

                        if (player != null && player.IsEntityInsideOpaqueBlock())
                        {
                            options.thirdPersonView = false;
                        }

                        Profiler.EndSection();
                        if (!skipRenderWorld)
                        {
                            Profiler.StartSection("gameMode");
                            if (gameMode != null)
                            {
                                gameMode.SetPartialTime(timer.renderPartialTicks);
                            }

                            Profiler.EndStartSection("gameRenderer");
                            entityRenderer.UpdateCameraAndRender(timer.renderPartialTicks);
                            Profiler.EndSection();
                        }

                        glFlush();
                        Profiler.EndSection();
                        if (!Display.IsActive() && fullscreen)
                            ToggleFullscreen();

                        Profiler.EndSection();
                        if (options.showDebugInfo)
                        {
                            if (!Profiler.profilingEnabled) 
                            {
                                Profiler.ClearProfiling();
                            }
                            Profiler.profilingEnabled = true;
                            DisplayDebugInfo(j24);
                        }
                        else
                        {
                            Profiler.profilingEnabled = false;
                            prevFrameTime = TimeUtil.NanoTime;
                        }

                        guiAchievement.UpdateAchievementWindow();
                        Profiler.StartSection("root");
                        Thread.Yield();
                        if (Keyboard.IsKeyDown(VirtualKey.F7))
                            Display.Update();

                        SaveScreenshot();
                        if (!fullscreen && (Display.GetWidth() != displayWidth || Display.GetHeight() != displayHeight))
                        {
                            displayWidth = Display.GetWidth();
                            displayHeight = Display.GetHeight();
                            if (displayWidth <= 0)
                            {
                                displayWidth = 1;
                            }

                            if (displayHeight <= 0)
                            {
                                displayHeight = 1;
                            }

                            Resize(displayWidth, displayHeight);
                        }

                        CheckGLError("Post render");
                        ++fps;
                        for (pause = !IsMultiplayerWorld() && currentScreen != null && currentScreen.DoesGuiPauseGame(); TimeUtil.MilliTime >= j1 + 1000; fps = 0)
                        {
                            fpsString = (this.fps = fps) + " fps, " + Chunk.updates + " chunk updates";
                            Chunk.updates = 0;
                            j1 += 1000;
                        }
                        Profiler.EndSection();
                    }
                    catch (LevelConflictException)
                    {
                        level = null;
                        SetLevel(null);
                        SetScreen(new LevelConflictScreen());
                    }
                    catch (OutOfMemoryException)
                    {
                        OnLowMemory();
                        SetScreen(new MemoryErrorScreen());
                        GC.Collect();
                    }
                }
            }
            catch (StopGameException)
            {
            }
            catch (Exception t)
            {
                OnLowMemory();
                t.PrintStackTrace();
                OnCrash(new CrashReport("Unexpected error", t));
            }
            finally
            {
                Destroy();
            }
        }

        public virtual void OnLowMemory()
        {
            try
            {
                this.renderGlobal.ReleaseLists();
            }
            catch (Exception)
            {
            }

            try
            {
                GC.Collect();
                AABB.ClearAABBPool();
                Vec3.ClearVec3Pool();
            }
            catch (Exception)
            {
            }

            try
            {
                GC.Collect();
                this.SetLevel(null);
            }
            catch (Exception)
            {
            }

            GC.Collect();
        }

        private void SaveScreenshot()
        {
            if (Keyboard.IsKeyDown(VirtualKey.F2))
            {
                if (!this.isTakingScreenshot)
                {
                    this.isTakingScreenshot = true;
                    this.ingameGUI.AddChatMessage(Screenshot.Save(workDir, this.displayWidth, this.displayHeight));
                }
            }
            else
            {
                this.isTakingScreenshot = false;
            }
        }

        private void UpdateDebugProfilerName(int i1)
        {
            IList<ProfilerResult> list2 = Profiler.GetProfilingData(this.debugProfilerName);
            if (list2 != null && list2.Count != 0)
            {
                ProfilerResult profilerResult3 = list2[0]; list2.RemoveAt(0);

                if (i1 == 0)
                {
                    if (profilerResult3.name.Length > 0)
                    {
                        int i4 = this.debugProfilerName.LastIndexOf(".");
                        if (i4 >= 0)
                        {
                            this.debugProfilerName = this.debugProfilerName.Substring(0, i4);
                        }
                    }
                }
                else
                {
                    --i1;
                    if (i1 < list2.Count && !((ProfilerResult)list2[i1]).name.Equals("unspecified"))
                    {
                        if (this.debugProfilerName.Length > 0)
                        {
                            this.debugProfilerName = this.debugProfilerName + ".";
                        }

                        this.debugProfilerName = this.debugProfilerName + ((ProfilerResult)list2[i1]).name;
                    }
                }
            }
        }

        private void DisplayDebugInfo(long j1)
        {
            IList<ProfilerResult> list3 = Profiler.GetProfilingData(this.debugProfilerName);
            ProfilerResult profilerResult4 = list3[0]; 
            list3.RemoveAt(0);
            long j5 = 16666666;
            if (this.prevFrameTime == -1)
            {
                this.prevFrameTime = TimeUtil.NanoTime;
            }

            long j7 = TimeUtil.NanoTime;
            tickTimes[numRecordedFrameTimes & frameTimes.Length - 1] = j1;
            frameTimes[numRecordedFrameTimes++ & frameTimes.Length - 1] = j7 - this.prevFrameTime;
            this.prevFrameTime = j7;
            GL11.glClear(GL11C.GL_DEPTH_BUFFER_BIT);
            GL11.glMatrixMode(GL11C.GL_PROJECTION);
            GL11.glEnable(GL11C.GL_COLOR_MATERIAL);
            GL11.glLoadIdentity();
            GL11.glOrtho(0, (double)this.displayWidth, (double)this.displayHeight, 0, 1000, 3000);
            GL11.glMatrixMode(GL11C.GL_MODELVIEW);
            GL11.glLoadIdentity();
            GL11.glTranslatef(0F, 0F, -2000F);
            GL11.glLineWidth(1F);
            GL11.glDisable(GL11C.GL_TEXTURE_2D);
            Tessellator tessellator9 = Tessellator.Instance;
            tessellator9.Begin(7);
            int i10 = (int)(j5 / 200000);
            tessellator9.Color(536870912);
            tessellator9.Vertex(0, (double)(this.displayHeight - i10), 0);
            tessellator9.Vertex(0, (double)this.displayHeight, 0);
            tessellator9.Vertex((double)frameTimes.Length, (double)this.displayHeight, 0);
            tessellator9.Vertex((double)frameTimes.Length, (double)(this.displayHeight - i10), 0);
            tessellator9.Color(0x20200000);
            tessellator9.Vertex(0, (double)(this.displayHeight - i10 * 2), 0);
            tessellator9.Vertex(0, (double)(this.displayHeight - i10), 0);
            tessellator9.Vertex((double)frameTimes.Length, (double)(this.displayHeight - i10), 0);
            tessellator9.Vertex((double)frameTimes.Length, (double)(this.displayHeight - i10 * 2), 0);
            tessellator9.End();
            long j11 = 0;
            int i13;
            for (i13 = 0; i13 < frameTimes.Length; ++i13)
            {
                j11 += frameTimes[i13];
            }

            i13 = (int)(j11 / 200000 / (long)frameTimes.Length);
            tessellator9.Begin(7);
            tessellator9.Color(0x20400000);
            tessellator9.Vertex(0, (double)(this.displayHeight - i13), 0);
            tessellator9.Vertex(0, (double)this.displayHeight, 0);
            tessellator9.Vertex((double)frameTimes.Length, (double)this.displayHeight, 0);
            tessellator9.Vertex((double)frameTimes.Length, (double)(this.displayHeight - i13), 0);
            tessellator9.End();
            tessellator9.Begin(1);
            int i15;
            int i16;
            for (int i14 = 0; i14 < frameTimes.Length; ++i14)
            {
                i15 = (i14 - numRecordedFrameTimes & frameTimes.Length - 1) * 255 / frameTimes.Length;
                i16 = i15 * i15 / 255;
                i16 = i16 * i16 / 255;
                int i17 = i16 * i16 / 255;
                i17 = i17 * i17 / 255;
                if (frameTimes[i14] > j5)
                {
                    tessellator9.Color(unchecked((int)0xFF000000) + i16 * 65536);
                }
                else
                {
                    tessellator9.Color(unchecked((int)0xFF000000) + i16 * 256);
                }

                long j18 = frameTimes[i14] / 200000;
                long j20 = tickTimes[i14] / 200000;
                tessellator9.Vertex((double)(i14 + 0.5), (double)(((long)this.displayHeight - j18) + 0.5F), 0);
                tessellator9.Vertex((double)(i14 + 0.5), (double)(this.displayHeight + 0.5), 0);
                tessellator9.Color(unchecked((int)0xFF000000) + i16 * 65536 + i16 * 256 + i16 * 1);
                tessellator9.Vertex((double)(i14 + 0.5), (double)(((long)this.displayHeight - j18) + 0.5F), 0);
                tessellator9.Vertex((double)(i14 + 0.5), (double)(((long)this.displayHeight - (j18 - j20)) + 0.5), 0);
            }

            tessellator9.End();
            short s26 = 160;
            i15 = this.displayWidth - s26 - 10;
            i16 = this.displayHeight - s26 * 2;
            GL11.glEnable(GL11C.GL_BLEND);
            tessellator9.Begin();
            tessellator9.Color(0, 200);
            tessellator9.Vertex((double)(i15 - s26 * 1.1), (double)(i16 - s26 * 0.6 - 16), 0);
            tessellator9.Vertex((double)(i15 - s26 * 1.1), (double)(i16 + s26 * 2), 0);
            tessellator9.Vertex((double)(i15 + s26 * 1.1), (double)(i16 + s26 * 2), 0);
            tessellator9.Vertex((double)(i15 + s26 * 1.1), (double)(i16 - s26 * 0.6 - 16), 0);
            tessellator9.End();
            GL11.glDisable(GL11C.GL_BLEND);
            double d27 = 0;
            int i21;
            for (int i19 = 0; i19 < list3.Count; ++i19)
            {
                ProfilerResult profilerResult29 = (ProfilerResult)list3[i19];
                i21 = Mth.Floor(profilerResult29.sectionPercentage / 4) + 1;
                tessellator9.Begin(6);
                tessellator9.Color(profilerResult29.GetColor());
                tessellator9.Vertex((double)i15, (double)i16, 0);
                int i22;
                float f23;
                float f24;
                float f25;
                for (i22 = i21; i22 >= 0; --i22)
                {
                    f23 = (float)((d27 + profilerResult29.sectionPercentage * (double)i22 / (double)i21) * (double)(float)Math.PI * 2 / 100);
                    f24 = Mth.Sin(f23) * (float)s26;
                    f25 = Mth.Cos(f23) * (float)s26 * 0.5F;
                    tessellator9.Vertex((double)((float)i15 + f24), (double)((float)i16 - f25), 0);
                }

                tessellator9.End();
                tessellator9.Begin(5);
                tessellator9.Color((profilerResult29.GetColor() & 16711422) >> 1);
                for (i22 = i21; i22 >= 0; --i22)
                {
                    f23 = (float)((d27 + profilerResult29.sectionPercentage * (double)i22 / (double)i21) * (double)(float)Math.PI * 2 / 100);
                    f24 = Mth.Sin(f23) * (float)s26;
                    f25 = Mth.Cos(f23) * (float)s26 * 0.5F;
                    tessellator9.Vertex((double)((float)i15 + f24), (double)((float)i16 - f25), 0);
                    tessellator9.Vertex((double)((float)i15 + f24), (double)((float)i16 - f25 + 10F), 0);
                }

                tessellator9.End();
                d27 += profilerResult29.sectionPercentage;
            }

            GL11.glEnable(GL11C.GL_TEXTURE_2D);
            string string30 = "";
            if (!profilerResult4.name.Equals("unspecified"))
            {
                string30 = string30 + "[0] ";
            }

            if (profilerResult4.name.Length == 0)
            {
                string30 = string30 + "ROOT ";
            }
            else
            {
                string30 = string30 + profilerResult4.name + " ";
            }

            uint i211 = 0xFFFFFF;
            this.font.DrawStringWithShadow(string30, i15 - s26, i16 - s26 / 2 - 16, i211);
            this.font.DrawStringWithShadow(string30 = profilerResult4.globalPercentage.ToString("##0.00") + "%", i15 + s26 - this.font.GetStringWidth(string30), i16 - s26 / 2 - 16, i211);
            for (int i32 = 0; i32 < list3.Count; ++i32)
            {
                ProfilerResult profilerResult31 = (ProfilerResult)list3[i32];
                string string33 = "";
                if (!profilerResult31.name.Equals("unspecified"))
                {
                    string33 = string33 + "[" + (i32 + 1) + "] ";
                }
                else
                {
                    string33 = string33 + "[?] ";
                }

                string33 = string33 + profilerResult31.name;
                this.font.DrawStringWithShadow(string33, i15 - s26, i16 + s26 / 2 + i32 * 8 + 20, (uint)profilerResult31.GetColor());
                this.font.DrawStringWithShadow(string33 = profilerResult31.sectionPercentage.ToString("##0.00") + "%", i15 + s26 - 50 - this.font.GetStringWidth(string33), i16 + s26 / 2 + i32 * 8 + 20, (uint)profilerResult31.GetColor());
                this.font.DrawStringWithShadow(string33 = profilerResult31.globalPercentage.ToString("##0.00") + "%", i15 + s26 - this.font.GetStringWidth(string33), i16 + s26 / 2 + i32 * 8 + 20, (uint)profilerResult31.GetColor());
            }
        }

        public virtual void Shutdown()
        {
            this.running = false;
        }

        public virtual void SetIngameFocus()
        {
            if (Display.IsActive())
            {
                if (!this.inGameHasFocus)
                {
                    this.inGameHasFocus = true;
                    this.mouseHelper.GrabMouse();
                    this.SetScreen((Screen)null);
                    this.leftClickCounter = 10000;
                    this.mouseTicksRan = this.ticksRan + 10000;
                }
            }
        }

        public virtual void SetIngameNotInFocus()
        {
            if (this.inGameHasFocus)
            {
                if (this.player != null)
                {
                    this.player.ResetPlayerKeyState();
                }

                this.inGameHasFocus = false;
                this.mouseHelper.UngrabMouse();
            }
        }

        public virtual void DisplayInGameMenu()
        {
            if (this.currentScreen == null)
            {
                this.SetScreen(new PauseScreen());
            }
        }

        private void Func_6254_a(int i1, bool z2)
        {
            if (!this.gameMode.field_1064_b)
            {
                if (!z2)
                {
                    this.leftClickCounter = 0;
                }

                if (i1 != 0 || this.leftClickCounter <= 0)
                {
                    if (z2 && this.objectMouseOver != null && this.objectMouseOver.TypeOfHit == HitResult.Type.TILE && i1 == 0)
                    {
                        int i3 = this.objectMouseOver.BlockX;
                        int i4 = this.objectMouseOver.BlockY;
                        int i5 = this.objectMouseOver.BlockZ;
                        this.gameMode.SendBlockRemoving(i3, i4, i5, this.objectMouseOver.SideHit);
                        this.effectRenderer.AddBlockHitEffects(i3, i4, i5, this.objectMouseOver.SideHit);
                    }
                    else
                    {
                        this.gameMode.ResetBlockRemoving();
                    }
                }
            }
        }

        private void ClickMouse(int i1)
        {
            if (i1 != 0 || this.leftClickCounter <= 0)
            {
                if (i1 == 0)
                {
                    this.player.SwingItem();
                }

                bool z2 = true;
                if (this.objectMouseOver == null)
                {
                    if (i1 == 0 && !(this.gameMode is CreativeMode))
                    {
                        this.leftClickCounter = 10;
                    }
                }
                else if (this.objectMouseOver.TypeOfHit == HitResult.Type.ENTITY)
                {
                    if (i1 == 0)
                    {
                        this.gameMode.AttackEntity(this.player, this.objectMouseOver.EntityHit);
                    }

                    if (i1 == 1)
                    {
                        this.gameMode.InteractWithEntity(this.player, this.objectMouseOver.EntityHit);
                    }
                }
                else if (this.objectMouseOver.TypeOfHit == HitResult.Type.TILE)
                {
                    int i3 = this.objectMouseOver.BlockX;
                    int i4 = this.objectMouseOver.BlockY;
                    int i5 = this.objectMouseOver.BlockZ;
                    TileFace i6 = this.objectMouseOver.SideHit;
                    if (i1 == 0)
                    {
                        this.gameMode.ClickBlock(i3, i4, i5, this.objectMouseOver.SideHit);
                    }
                    else
                    {
                        ItemInstance itemStack7 = this.player.inventory.GetCurrentItem();
                        int i8 = itemStack7 != null ? itemStack7.stackSize : 0;
                        if (this.gameMode.SendPlaceBlock(this.player, this.level, itemStack7, i3, i4, i5, i6))
                        {
                            z2 = false;
                            this.player.SwingItem();
                        }

                        if (itemStack7 == null)
                        {
                            return;
                        }

                        if (itemStack7.stackSize == 0)
                        {
                            this.player.inventory.mainInventory[this.player.inventory.currentItem] = null;
                        }
                        else if (itemStack7.stackSize != i8)
                        {
                            this.entityRenderer.itemRenderer.Func_9449_b();
                        }
                    }
                }

                if (z2 && i1 == 1)
                {
                    ItemInstance itemStack9 = this.player.inventory.GetCurrentItem();
                    if (itemStack9 != null && this.gameMode.SendUseItem(this.player, this.level, itemStack9))
                    {
                        this.entityRenderer.itemRenderer.Func_9450_c();
                    }
                }
            }
        }

        
        public virtual void ToggleFullscreen()
        {
            try
            {
                fullscreen = !fullscreen;
                if (fullscreen)
                {
                    tempDisplayWidth = displayWidth;
                    tempDisplayHeight = displayHeight;
                    Display.SetDisplayModeAndFullscreen(Display.GetDesktopDisplayMode());
                    displayWidth = Display.GetDisplayMode().GetWidth();
                    displayHeight = Display.GetDisplayMode().GetHeight();
                    
                    if (displayWidth <= 0)
                        displayWidth = 1;

                    if (displayHeight <= 0)
                        displayHeight = 1;
                }
                else
                {
                    displayWidth = tempDisplayWidth;
                    displayHeight = tempDisplayHeight;

                    if (displayWidth <= 0)
                        displayWidth = 1;

                    if (displayHeight <= 0)
                        displayHeight = 1;

                    Display.SetDisplayMode(new DisplayMode(tempDisplayWidth, tempDisplayHeight));
                    Display.SetResizable(true);
                }

                if (currentScreen != null)
                    Resize(displayWidth, displayHeight);

                Display.Update();
            }
            catch (Exception e)
            {
                e.PrintStackTrace();
            }
        }
        
        private void Resize(int i1, int i2)
        {
            if (i1 <= 0)
            {
                i1 = 1;
            }

            if (i2 <= 0)
            {
                i2 = 1;
            }

            this.displayWidth = i1;
            this.displayHeight = i2;
            if (this.currentScreen != null)
            {
                GuiScale scaledResolution3 = new GuiScale(this.options, i1, i2);
                int i4 = scaledResolution3.GetWidth();
                int i5 = scaledResolution3.GetHeight();
                this.currentScreen.Init(this, i4, i5);
            }
        }

        
        private void ClickMiddleMouseButton()
        {
            if (this.objectMouseOver != null)
            {
                int i1 = this.level.GetTile(this.objectMouseOver.BlockX, this.objectMouseOver.BlockY, this.objectMouseOver.BlockZ);
                if (i1 == Tile.grass.id)
                {
                    i1 = Tile.dirt.id;
                }

                if (i1 == Tile.stoneSlab.id)
                {
                    i1 = Tile.stoneSlabHalf.id;
                }

                if (i1 == Tile.unbreakable.id)
                {
                    i1 = Tile.rock.id;
                }

                this.player.inventory.SetCurrentItem(i1, this.gameMode is CreativeMode);
            }
        }
        
        private void CheckLicense()
        {
            new Thread(new ThreadStart(() =>
            {
                try
                {
                    using HttpResponseMessage response = SharedConstants.HTTP_CLIENT
                        .GetAsync(string.Format(SharedConstants.LIC_CHECK, user.name, user.sessionId)).Result;

                    if (response.StatusCode == HttpStatusCode.BadRequest)
                        licenseCheckTime = TimeUtil.MilliTime;
                }
                catch
                {
                }
            })).Start();
        }
        
        public virtual void Tick()
        {
            if (this.ticksRan == 6000)
                this.CheckLicense();

            Profiler.StartSection("stats");
            this.statFileWriter.Tick();
            Profiler.EndStartSection("gui");
            this.ingameGUI.UpdateTick();
            Profiler.EndStartSection("pick");
            this.entityRenderer.GetMouseOver(1F);
            Profiler.EndStartSection("centerChunkSource");
            if (this.player != null)
            {
                IChunkSource cs = this.level.GetChunkSource();
                if (cs is ChunkCache)
                {
                    ChunkCache cache = (ChunkCache)cs;
                    int cx = Mth.Floor(this.player.x) >> 4;
                    int cz = Mth.Floor(this.player.z) >> 4;
                    cache.SetPos(cx, cz);
                }
            }

            Profiler.EndStartSection("gameMode");
            if (!this.pause && this.level != null)
            {
                this.gameMode.UpdateController();
            }

            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.textures.LoadTexture("/terrain.png"));
            Profiler.EndStartSection("textures");
            if (!this.pause)
            {
                this.textures.Tick();
            }

            if (this.currentScreen == null && this.player != null)
            {
                if (this.player.health <= 0)
                {
                    this.SetScreen((Screen)null);
                }
                else if (this.player.IsSleeping() && this.level != null && this.level.isRemote)
                {
                    this.SetScreen(new InBedScreen());
                }
            }
            else if (this.currentScreen != null && this.currentScreen is InBedScreen && !this.player.IsSleeping())
            {
                this.SetScreen((Screen)null);
            }

            if (this.currentScreen != null)
            {
                this.leftClickCounter = 10000;
                this.mouseTicksRan = this.ticksRan + 10000;
            }

            if (this.currentScreen != null)
            {
                this.currentScreen.HandleInput();
                if (this.currentScreen != null)
                {
                    this.currentScreen.UpdateScreen();
                }
            }

            if (this.currentScreen == null || this.currentScreen.field_948_f)
            {
                // originally was mouse and keyboard,
                // but couldn't be fucked to figure out the positions
                Profiler.EndStartSection("input");
                bool break301 = false;
            //label301:
                while (true)
                {
                    if (break301) break;
                    while (true)
                    {
                        if (break301) break;
                        while (true)
                        {
                            long j5;
                            do
                            {
                                if (!Mouse.Next())
                                {
                                    if (this.leftClickCounter > 0)
                                    {
                                        --this.leftClickCounter;
                                    }

                                    while (true)
                                    {
                                        if (break301) break;
                                        while (true)
                                        {
                                            if (break301) break;
                                            do
                                            {
                                                if (!Keyboard.Next())
                                                {
                                                    if (this.currentScreen == null)
                                                    {
                                                        if (Mouse.IsButtonDown(0) && this.ticksRan - this.mouseTicksRan >= this.timer.tps / 4F && this.inGameHasFocus)
                                                        {
                                                            this.ClickMouse(0);
                                                            this.mouseTicksRan = this.ticksRan;
                                                        }

                                                        if (Mouse.IsButtonDown(1) && this.ticksRan - this.mouseTicksRan >= this.timer.tps / 4F && this.inGameHasFocus)
                                                        {
                                                            this.ClickMouse(1);
                                                            this.mouseTicksRan = this.ticksRan;
                                                        }
                                                    }

                                                    this.Func_6254_a(0, this.currentScreen == null && Mouse.IsButtonDown(0) && this.inGameHasFocus);
                                                    break301 = true;
                                                    break;
                                                }

                                                this.player.HandleKeyPress(Keyboard.GetEventKey(), Keyboard.GetEventKeyState());
                                            }
                                            while (!Keyboard.GetEventKeyState());

                                            if (break301) break;
                                            if (Keyboard.GetEventKey() == VirtualKey.F11)
                                            {
                                                this.ToggleFullscreen();
                                            }
                                            else
                                            {
                                                if (this.currentScreen != null)
                                                {
                                                    this.currentScreen.HandleKeyboardInput();
                                                }
                                                else
                                                {
                                                    if (Keyboard.GetEventKey() == VirtualKey.Escape)
                                                    {
                                                        this.DisplayInGameMenu();
                                                    }

                                                    if (Keyboard.GetEventKey() == VirtualKey.S && Keyboard.IsKeyDown(VirtualKey.F3))
                                                    {
                                                        this.ForceReload();
                                                    }

                                                    if (Keyboard.GetEventKey() == VirtualKey.F1)
                                                    {
                                                        this.options.hideGUI = !this.options.hideGUI;
                                                    }
                                                    //TODO this shit
                                                    if (Keyboard.GetEventKey() == VirtualKey.F6) 
                                                    {
                                                        level.SetTime(15000);
                                                        player.inventory.AddItem(new ItemInstance(Item.bed, 1));
                                                    }

                                                    if (Keyboard.GetEventKey() == VirtualKey.F7) 
                                                    {
                                                        Enhancements.client_fly_hack = !Enhancements.client_fly_hack;
                                                    }
                                                    //end t

                                                    if (Keyboard.GetEventKey() == VirtualKey.F3)
                                                    {
                                                        this.options.showDebugInfo = !this.options.showDebugInfo;
                                                    }

                                                    if (Keyboard.GetEventKey() == VirtualKey.F5)
                                                    {
                                                        this.options.thirdPersonView = !this.options.thirdPersonView;
                                                    }

                                                    if (Keyboard.GetEventKey() == VirtualKey.F8)
                                                    {
                                                        this.options.smoothCamera = !this.options.smoothCamera;
                                                    }

                                                    if (Keyboard.GetEventKey() == this.options.keyBindInventory.keyCode)
                                                    {
                                                        this.SetScreen(new GuiInventory(this.player));
                                                    }

                                                    if (Keyboard.GetEventKey() == this.options.keyBindDrop.keyCode)
                                                    {
                                                        this.player.DropCurrentItem();
                                                    }

                                                    if (/*this.IsMultiplayerWorld() &&*/ Keyboard.GetEventKey() == this.options.keyBindChat.keyCode)
                                                    {
                                                        this.SetScreen(new ChatScreen());
                                                    }
                                                }

                                                for (int i6 = 0; i6 < 9; ++i6)
                                                {
                                                    if (Keyboard.GetEventKey() == VirtualKey.D1 + i6)
                                                    {
                                                        this.player.inventory.currentItem = i6;
                                                    }
                                                }

                                                if (this.options.showDebugInfo)
                                                {
                                                    if (Keyboard.GetEventKey() == VirtualKey.D0)
                                                    {
                                                        this.UpdateDebugProfilerName(0);
                                                    }

                                                    for (int i7 = 0; i7 < 9; ++i7)
                                                    {
                                                        if (Keyboard.GetEventKey() == VirtualKey.D1 + i7)
                                                        {
                                                            this.UpdateDebugProfilerName(i7 + 1);
                                                        }
                                                    }
                                                }

                                                if (Keyboard.GetEventKey() == this.options.keyBindToggleFog.keyCode)
                                                {
                                                    this.options.SetOptionValue(Option.RENDER_DISTANCE, !Keyboard.IsKeyDown(VirtualKey.ShiftKey) ? 1 : -1);
                                                }
                                            }
                                        }
                                    }
                                    if (break301) break;
                                }

                                j5 = TimeUtil.MilliTime - this.systemTime;
                                //if (break301) break;
                            }
                            while (j5 > 200);
                            if (break301) break;
                            int i3 = Mouse.GetEventDWheel();
                            if (i3 != 0)
                            {
                                this.player.inventory.ChangeCurrentItem(i3);
                                if (this.options.field_22275_C)
                                {
                                    if (i3 > 0)
                                    {
                                        i3 = 1;
                                    }

                                    if (i3 < 0)
                                    {
                                        i3 = -1;
                                    }

                                    this.options.field_22272_F += i3 * 0.25F;
                                }
                            }

                            if (this.currentScreen == null)
                            {
                                if (!this.inGameHasFocus && Mouse.GetEventButtonState())
                                {
                                    this.SetIngameFocus();
                                }
                                else
                                {
                                    if (Mouse.GetEventButton() == 0 && Mouse.GetEventButtonState())
                                    {
                                        this.ClickMouse(0);
                                        this.mouseTicksRan = this.ticksRan;
                                    }

                                    if (Mouse.GetEventButton() == 1 && Mouse.GetEventButtonState())
                                    {
                                        this.ClickMouse(1);
                                        this.mouseTicksRan = this.ticksRan;
                                    }

                                    if (Mouse.GetEventButton() == 2 && Mouse.GetEventButtonState())
                                    {
                                        this.ClickMiddleMouseButton();
                                    }
                                }
                            }
                            else if (this.currentScreen != null)
                            {
                                this.currentScreen.HandleMouseInput();
                            }
                        }
                    }
                }
            }

            if (this.level != null)
            {
                if (this.player != null)
                {
                    ++this.joinPlayerCounter;
                    if (this.joinPlayerCounter == 30)
                    {
                        this.joinPlayerCounter = 0;
                        this.level.JoinEntityInSurroundings(this.player);
                    }
                }

                this.level.difficultySetting = this.options.difficulty;
                if (this.level.isRemote)
                {
                    this.level.difficultySetting = 3;
                }

                Profiler.EndStartSection("gameRenderer");
                if (!this.pause)
                {
                    this.entityRenderer.UpdateRenderer();
                }

                Profiler.EndStartSection("levelRenderer");
                if (!this.pause)
                {
                    this.renderGlobal.UpdateClouds();
                }

                Profiler.EndStartSection("level");
                if (!this.pause)
                {
                    if (this.level.field_i > 0)
                    {
                        --this.level.field_i;
                    }

                    this.level.TickEntities();
                }

                if (!this.pause || this.IsMultiplayerWorld())
                {
                    this.level.SetSpawnSettings(this.options.difficulty > 0, true);
                    this.level.Tick();
                }

                Profiler.EndStartSection("animateTick");
                if (!this.pause && this.level != null)
                {
                    this.level.RandomDisplayUpdates(Mth.Floor(this.player.x), Mth.Floor(this.player.y), Mth.Floor(this.player.z));
                }

                Profiler.EndStartSection("particles");
                if (!this.pause)
                {
                    this.effectRenderer.UpdateEffects();
                }
            }

            Profiler.EndSection();
            this.systemTime = TimeUtil.MilliTime;
        }
        
        private void ForceReload()
        {
            Console.WriteLine("FORCING RELOAD!");
            this.soundEngine = new SoundEngine();
            this.soundEngine.Init(this.options);
            this.InitBackgroundDownloader();
        }

        public virtual bool IsMultiplayerWorld()
        {
            return this.level != null && this.level.isRemote;
        }

        public virtual void LoadLevel(string levelName, string string2, long seed)
        {
            this.SetLevel((Level)null);
            GC.Collect();
            if (this.lvlStorageSrc.RequiresConversion(levelName))
            {
                this.ConvertLevel(levelName, string2);
            }
            else
            {
                ILevelStorage iSaveHandler5 = this.lvlStorageSrc.SelectLevel(levelName, false);
                Level world6 = null;
                world6 = new Level(iSaveHandler5, string2, seed);
                if (world6.isNewWorld)
                {
                    this.statFileWriter.ReadStat(StatList.createWorld, 1);
                    this.statFileWriter.ReadStat(StatList.startGame, 1);
                    this.SetLevel(world6, "Generating level");
                }
                else
                {
                    this.statFileWriter.ReadStat(StatList.loadWorld, 1);
                    this.statFileWriter.ReadStat(StatList.startGame, 1);
                    this.SetLevel(world6, "Loading level");
                }
            }
        }

        public virtual void ToggleDimension()
        {
            Console.WriteLine("Toggling dimension!!");
            if (this.player.dimension == -1)
            {
                this.player.dimension = 0;
            }
            else
            {
                this.player.dimension = -1;
            }

            this.level.SetEntityDead(this.player);
            this.player.isDead = false;
            double d1 = this.player.x;
            double d3 = this.player.z;
            double d5 = 8;
            Level world7;
            if (this.player.dimension == -1)
            {
                d1 /= d5;
                d3 /= d5;
                this.player.SetLocationAndAngles(d1, this.player.y, d3, this.player.yaw, this.player.pitch);
                if (this.player.IsEntityAlive())
                {
                    this.level.UpdateEntityWithOptionalForce(this.player, false);
                }

                world7 = null;
                world7 = new Level(this.level, Dimension.GetNew(-1));
                this.SetLevel(world7, "Entering the Nether", this.player);
            }
            else
            {
                d1 *= d5;
                d3 *= d5;
                this.player.SetLocationAndAngles(d1, this.player.y, d3, this.player.yaw, this.player.pitch);
                if (this.player.IsEntityAlive())
                {
                    this.level.UpdateEntityWithOptionalForce(this.player, false);
                }

                world7 = null;
                world7 = new Level(this.level, Dimension.GetNew(0));
                this.SetLevel(world7, "Leaving the Nether", this.player);
            }

            this.player.worldObj = this.level;
            if (this.player.IsEntityAlive())
            {
                this.player.SetLocationAndAngles(d1, this.player.y, d3, this.player.yaw, this.player.pitch);
                this.level.UpdateEntityWithOptionalForce(this.player, false);
                (new PortalForcer()).SetExitLocation(this.level, this.player);
            }
        }

        public virtual void SetLevel(Level world1)
        {
            this.SetLevel(world1, "");
        }

        public virtual void SetLevel(Level world1, string string2)
        {
            this.SetLevel(world1, string2, (Player)null);
        }

        public virtual void SetLevel(Level world1, string string2, Player entityPlayer3)
        {
            this.statFileWriter.Dispose();
            this.statFileWriter.SyncStats();
            this.renderViewEntity = null;
            this.loadingScreen.PrintText(string2);
            this.loadingScreen.DisplayLoadingString("");
            this.soundEngine.PlayStreaming((string)null, 0F, 0F, 0F, 0F, 0F);
            if (this.level != null)
            {
                this.level.Save(this.loadingScreen);
            }

            this.level = world1;
            if (world1 != null)
            {
                this.gameMode.Func_717_a(world1);
                if (!this.IsMultiplayerWorld())
                {
                    if (entityPlayer3 == null)
                    {
                        this.player = (LocalPlayer)world1.FindInstanceOf<LocalPlayer>(typeof(LocalPlayer));
                    }
                }
                else if (this.player != null)
                {
                    this.player.PreparePlayerToSpawn();
                    if (world1 != null)
                    {
                        world1.AddEntity(this.player);
                    }
                }

                if (!world1.isRemote)
                {
                    this.PrepareLevel(string2);
                }

                if (this.player == null)
                {
                    this.player = (LocalPlayer)this.gameMode.CreatePlayer(world1);
                    this.player.PreparePlayerToSpawn();
                    this.gameMode.FlipPlayer(this.player);
                }

                this.player.movementInput = new KeyboardInput(this.options);
                if (this.renderGlobal != null)
                {
                    this.renderGlobal.ChangeWorld(world1);
                }

                if (this.effectRenderer != null)
                {
                    this.effectRenderer.ClearEffects(world1);
                }

                this.gameMode.Func_6473_b(this.player);
                if (entityPlayer3 != null)
                {
                    world1.EmptyMethod1();
                }

                IChunkSource iChunkProvider4 = world1.GetChunkSource();
                if (iChunkProvider4 is ChunkCache)
                {
                    ChunkCache chunkProviderLoadOrGenerate5 = (ChunkCache)iChunkProvider4;
                    int i6 = Mth.Floor(((int)this.player.x)) >> 4;
                    int i7 = Mth.Floor(((int)this.player.z)) >> 4;
                    chunkProviderLoadOrGenerate5.SetPos(i6, i7);
                }

                world1.SpawnPlayerWithLoadedChunks(this.player);
                if (world1.isNewWorld)
                {
                    world1.Save(this.loadingScreen);
                }

                this.renderViewEntity = this.player;
            }
            else
            {
                this.player = null;
            }

            GC.Collect();
            this.systemTime = 0;
        }

        private void ConvertLevel(string string1, string string2)
        {
            this.loadingScreen.PrintText("Converting World to " + this.lvlStorageSrc.GetName());
            this.loadingScreen.DisplayLoadingString("This may take a while :)");
            this.lvlStorageSrc.ConvertLevel(string1, this.loadingScreen);
            this.LoadLevel(string1, string2, 0);
        }

        private void PrepareLevel(string string1)
        {
            this.loadingScreen.PrintText(string1);
            this.loadingScreen.DisplayLoadingString("Building terrain");
            short initialRadius = 128;
            int i3 = 0;
            int i4 = initialRadius * 2 / 16 + 1;
            i4 *= i4;
            IChunkSource cs = this.level.GetChunkSource();
            Pos spawnPos = this.level.GetSpawnPos();
            if (this.player != null)
            {
                spawnPos.x = (int)this.player.x;
                spawnPos.z = (int)this.player.z;
            }

            if (cs is ChunkCache)
            {
                ChunkCache chonkcache = (ChunkCache)cs;
                chonkcache.SetPos(spawnPos.x >> 4, spawnPos.z >> 4);
            }

            for (int x = -initialRadius; x <= initialRadius; x += 16)
            {
                for (int z = -initialRadius; z <= initialRadius; z += 16)
                {
                    this.loadingScreen.SetLoadingProgress(i3++ * 100 / i4);
                    this.level.GetTile(spawnPos.x + x, 64, spawnPos.z + z);
                    while (this.level.UpdateLights()) ;

                }
            }

            this.loadingScreen.DisplayLoadingString("Simulating world for a bit");
            this.level.Prepare();
        }
        
        public virtual void InstallResource(string name, JFile file2)
        {
            int slashIdx = name.IndexOf("/", StringComparison.Ordinal);
            string type = name.Substring(0, slashIdx).ToLowerInvariant();
            name = name.Substring(slashIdx + 1);

            switch (type)
            {
                case "sound":
                case "newsound":
                    this.soundEngine.AddSound(name, file2);
                    break;
                
                case "music":
                case "newmusic":
                    this.soundEngine.AddMusic(name, file2);
                    break;
                
                case "streaming":
                    this.soundEngine.AddStreaming(name, file2);
                    break;
            }
        }

        public virtual GLCapabilities GetGLCapabilities()
        {
            return this.glCapabilities;
        }

        public virtual string GetRenderStats()
        {
            return this.renderGlobal.GetDebugInfoRenders();
        }
        public virtual string GetEntityStats()
        {
            return this.renderGlobal.GetDebugInfoEntities();
        }

        public virtual string GetChunkSourceStats()
        {
            return this.level.GetChunkSourceStats();
        }

        public virtual string GetParticleStats()
        {
            return "P: " + this.effectRenderer.getStatistics() + ". T: " + this.level.GetLoadedEntityStats();
        }
        
        public virtual void Respawn(bool bedValid, int dim)
        {
            if (!this.level.isRemote && !this.level.dimension.CanRespawnHere())
            {
                this.ToggleDimension();
            }

            Pos bedPos = default;
            Pos spawnPos = default;
            bool hasBedPos = true;
            if (this.player != null && !bedValid)
            {
                bedPos = this.player.GetSpawnPos();
                if (bedPos != default)
                {
                    spawnPos = Player.GetNearestBedSpawnPos(this.level, bedPos);
                    if (spawnPos == default)
                    {
                        this.player.AddChatMessage("tile.bed.notValid");
                    }
                }
            }

            if (spawnPos == default)
            {
                spawnPos = this.level.GetSpawnPos();
                hasBedPos = false;
            }

            IChunkSource cs = this.level.GetChunkSource();
            if (cs is ChunkCache)
            {
                ChunkCache chonkcache = (ChunkCache)cs;
                chonkcache.SetPos(spawnPos.x >> 4, spawnPos.z >> 4);
            }

            this.level.SetSpawnLocation();
            this.level.UpdateEntityList();
            int eid = 0;
            if (this.player != null)
            {
                eid = this.player.entityID;
                this.level.SetEntityDead(this.player);
            }

            this.renderViewEntity = null;
            this.player = (LocalPlayer)this.gameMode.CreatePlayer(this.level);
            this.player.dimension = dim;
            this.renderViewEntity = this.player;
            this.player.PreparePlayerToSpawn();
            if (hasBedPos)
            {
                this.player.SetSpawn(bedPos);
                this.player.SetLocationAndAngles(spawnPos.x + 0.5F, spawnPos.y + 0.1F, spawnPos.z + 0.5F, 0F, 0F);
            }

            this.gameMode.FlipPlayer(this.player);
            this.level.SpawnPlayerWithLoadedChunks(this.player);
            this.player.movementInput = new KeyboardInput(this.options);
            this.player.entityID = eid;
            this.player.Fun_o();
            this.gameMode.Func_6473_b(this.player);
            this.PrepareLevel("Respawning");
            if (this.currentScreen is DeathScreen)
            {
                this.SetScreen((Screen)null);
            }
        }

        public static void Play(string username, string sessionid)
        {
            Play(username, sessionid, (string)null);
        }

        public static void Play(string username, string sessionid, string server)
        {
            bool fullscreen = false;
            Client craft = new Client(DEFAULT_WIDTH, DEFAULT_HEIGHT, fullscreen);
            Thread thred = new Thread(new ThreadStart(craft.Run));
            thred.Name = "SharpCraft main thread";
            thred.Priority = ThreadPriority.Highest;

            if (username != null && sessionid != null)
            {
                craft.user = new User(username, sessionid);
            }
            else
            {
                craft.user = new User("Player" + TimeUtil.MilliTime % 1000, "");
            }
            Console.WriteLine("Setting user: "+craft.user.name);
            if (server != null)
            {
                string[] ip = server.Split(":");
                craft.SetServer(ip[0], int.Parse(ip[1]));
                Console.WriteLine("Setting server: "+server);
            }

            thred.Start();
        }
        
        public ClientConnection GetSendQueue()
        {
            return this.player is MultiplayerLocalPlayer ? ((MultiplayerLocalPlayer)this.player).sendQueue : null;
        }

        [STAThread]
        public static void Main(string[] args)
        {
            string username = null;
            string sessionid = null;
            username = "Player" + TimeUtil.MilliTime % 1000;
            if (args.Length > 0)
            {
                username = args[0];
            }

            sessionid = "-";
            if (args.Length > 1)
            {
                sessionid = args[1];
            }

            Play(username, sessionid);
        }
        
        public static bool IsGuiEnabled()
        {
            return instance == null || !instance.options.hideGUI;
        }
        
        public static bool IsFancyGraphicsEnabled()
        {
            return instance != null && instance.options.fancyGraphics;
        }

        public static bool IsAmbientOcclusionEnabled()
        {
            return instance != null && instance.options.ambientOcclusion;
        }

        public static bool IsDebugInfoEnabled()
        {
            return instance != null && instance.options.showDebugInfo;
        }

        public virtual bool LineIsCommand(string line)
        {
            if (line.StartsWith('/'))
            {
            }

            return false;
        }
    }
}
