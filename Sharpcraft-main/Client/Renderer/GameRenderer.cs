using LWCSGL.OpenGL;
using SharpCraft.Client.GUI;
using SharpCraft.Client.Players;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpCraft.Client.Gamemode;
using LWCSGL.Input;
using SharpCraft.Client.Particles;
using SharpCraft.Client.Renderer.Culling;
using SharpCraft.Core;

namespace SharpCraft.Client.Renderer
{
    public class GameRenderer
    {
        public static bool field_28135_a = false;
        public static int anaglyphField;
        private Client mc;
        private float farPlaneDistance = 0F;
        public ItemInHandRenderer itemRenderer;
        private int rendererUpdateCount;
        private Entity pointedEntity = null;
        private MouseFilter mouseFilterXAxis = new MouseFilter();
        private MouseFilter mouseFilterYAxis = new MouseFilter();
        private float field_22228_r = 4F;
        private float field_22227_s = 4F;
        private float field_22226_t = 0F;
        private float field_22225_u = 0F;
        private float field_22224_v = 0F;
        private float field_22223_w = 0F;
        private float field_22222_x = 0F;
        private float field_22221_y = 0F;
        private float field_22220_z = 0F;
        private float field_22230_A = 0F;
        private bool cloudFog = false;
        private double cameraZoom = 1;
        private double cameraYaw = 0;
        private double cameraPitch = 0;
        private long prevFrameTime = TimeUtil.MilliTime;
        private long field_28133_I = 0;
        private JRandom random = new JRandom();
        private int rainSoundCounter = 0;
        //float[] fogColorBuffer = new float[4];
        float fogColorRed;
        float fogColorGreen;
        float fogColorBlue;
        private float fogColor2;
        private float fogColor1;

        public GameRenderer(Client instance)
        {
            this.mc = instance;
            this.itemRenderer = new ItemInHandRenderer(instance);
        }

        public virtual void UpdateRenderer()
        {
            this.fogColor2 = this.fogColor1;
            this.field_22227_s = this.field_22228_r;
            this.field_22225_u = this.field_22226_t;
            this.field_22223_w = this.field_22224_v;
            this.field_22221_y = this.field_22222_x;
            this.field_22230_A = this.field_22220_z;
            if (this.mc.renderViewEntity == null)
            {
                this.mc.renderViewEntity = this.mc.player;
            }

            float f1 = this.mc.level.GetBrightness(Mth.Floor(this.mc.renderViewEntity.x), Mth.Floor(this.mc.renderViewEntity.y), Mth.Floor(this.mc.renderViewEntity.z));
            float f2 = (3 - this.mc.options.renderDistance) / 3F;
            float f3 = f1 * (1F - f2) + f2;
            this.fogColor1 += (f3 - this.fogColor1) * 0.1F;
            ++this.rendererUpdateCount;
            this.itemRenderer.UpdateEquippedItem();
            this.AddRainParticles();
        }

        public virtual void GetMouseOver(float f1)
        {
            if (this.mc.renderViewEntity != null)
            {
                if (this.mc.level != null)
                {
                    double d2 = this.mc.gameMode.GetBlockReachDistance();
                    this.mc.objectMouseOver = this.mc.renderViewEntity.RayTrace(d2, f1);
                    double d4 = d2;
                    Vec3 vec3D6 = this.mc.renderViewEntity.GetPosition(f1);
                    if (this.mc.objectMouseOver != null)
                    {
                        d4 = this.mc.objectMouseOver.HitVec.DistanceTo(vec3D6);
                    }

                    if (this.mc.gameMode is CreativeMode)
                    {
                        d2 = 32;
                        d4 = 32;
                    }
                    else
                    {
                        if (d4 > 3)
                        {
                            d4 = 3;
                        }

                        d2 = d4;
                    }

                    Vec3 vec3D7 = this.mc.renderViewEntity.GetLook(f1);
                    Vec3 vec3D8 = vec3D6.AddVector(vec3D7.x * d2, vec3D7.y * d2, vec3D7.z * d2);
                    this.pointedEntity = null;
                    float f9 = 1F;
                    IList<Entity> list10 = this.mc.level.GetEntities(this.mc.renderViewEntity, this.mc.renderViewEntity.boundingBox.AddCoord(vec3D7.x * d2, vec3D7.y * d2, vec3D7.z * d2).Expand(f9, f9, f9));
                    double d11 = 0;
                    for (int i13 = 0; i13 < list10.Count; ++i13)
                    {
                        Entity entity14 = list10[i13];
                        if (entity14.CanBeCollidedWith())
                        {
                            float f15 = entity14.GetCollisionBorderSize();
                            AABB axisAlignedBB16 = entity14.boundingBox.Expand(f15, f15, f15);
                            HitResult movingObjectPosition17 = axisAlignedBB16.Clip(vec3D6, vec3D8);
                            if (axisAlignedBB16.IsVecInside(vec3D6))
                            {
                                if (0 < d11 || d11 == 0)
                                {
                                    this.pointedEntity = entity14;
                                    d11 = 0;
                                }
                            }
                            else if (movingObjectPosition17 != null)
                            {
                                double d18 = vec3D6.DistanceTo(movingObjectPosition17.HitVec);
                                if (d18 < d11 || d11 == 0)
                                {
                                    this.pointedEntity = entity14;
                                    d11 = d18;
                                }
                            }
                        }
                    }

                    if (this.pointedEntity != null && !(this.mc.gameMode is CreativeMode))
                    {
                        this.mc.objectMouseOver = new HitResult(this.pointedEntity);
                    }
                }
            }
        }

        private float GetFOVModifier(float f1)
        {
            Mob entityLiving2 = this.mc.renderViewEntity;
            float f3 = 70F;
            if (entityLiving2.IsInsideOfMaterial(Material.water))
            {
                f3 = 60F;
            }

            if (entityLiving2.health <= 0)
            {
                float f4 = entityLiving2.deathTime + f1;
                f3 /= (1F - 500F / (f4 + 500F)) * 2F + 1F;
            }

            return f3 + this.field_22221_y + (this.field_22222_x - this.field_22221_y) * f1;
        }

        private void HurtCameraEffect(float f1)
        {
            Mob entityLiving2 = this.mc.renderViewEntity;
            float f3 = entityLiving2.hurtTime - f1;
            float f4;
            if (entityLiving2.health <= 0)
            {
                f4 = entityLiving2.deathTime + f1;
                GL11.glRotatef(40F - 8000F / (f4 + 200F), 0F, 0F, 1F);
            }

            if (f3 >= 0F)
            {
                f3 /= entityLiving2.maxHurtTime;
                f3 = Mth.Sin(f3 * f3 * f3 * f3 * Mth.PI);
                f4 = entityLiving2.attackedAtYaw;
                GL11.glRotatef(-f4, 0F, 1F, 0F);
                GL11.glRotatef(-f3 * 14F, 0F, 0F, 1F);
                GL11.glRotatef(f4, 0F, 1F, 0F);
            }
        }

        private void SetupViewBobbing(float f1)
        {
            if (this.mc.renderViewEntity is Player)
            {
                Player entityPlayer2 = (Player)this.mc.renderViewEntity;
                float f3 = entityPlayer2.distanceWalkedModified - entityPlayer2.prevDistanceWalkedModified;
                float f4 = -(entityPlayer2.distanceWalkedModified + f3 * f1);
                float f5 = entityPlayer2.field_775 + (entityPlayer2.field_774 - entityPlayer2.field_775) * f1;
                float f6 = entityPlayer2.cameraPitch + (entityPlayer2.field_R - entityPlayer2.cameraPitch) * f1;
                GL11.glTranslatef(Mth.Sin(f4 * Mth.PI) * f5 * 0.5F, -Math.Abs(Mth.Cos(f4 * Mth.PI) * f5), 0F);
                GL11.glRotatef(Mth.Sin(f4 * Mth.PI) * f5 * 3F, 0F, 0F, 1F);
                GL11.glRotatef(Math.Abs(Mth.Cos(f4 * Mth.PI - 0.2F) * f5) * 5F, 1F, 0F, 0F);
                GL11.glRotatef(f6, 1F, 0F, 0F);
            }
        }

        private void OrientCamera(float f1)
        {
            Mob entityLiving2 = this.mc.renderViewEntity;
            float f3 = entityLiving2.yOffset - 1.62F;
            double d4 = entityLiving2.prevX + (entityLiving2.x - entityLiving2.prevX) * f1;
            double d6 = entityLiving2.prevY + (entityLiving2.y - entityLiving2.prevY) * f1 - f3;
            double d8 = entityLiving2.prevZ + (entityLiving2.z - entityLiving2.prevZ) * f1;
            GL11.glRotatef(this.field_22230_A + (this.field_22220_z - this.field_22230_A) * f1, 0F, 0F, 1F);
            if (entityLiving2.IsSleeping())
            {
                f3 = (float)(f3 + 1);
                GL11.glTranslatef(0F, 0.3F, 0F);
                if (!this.mc.options.field_22273_E)
                {
                    int i10 = this.mc.level.GetTile(Mth.Floor(entityLiving2.x), Mth.Floor(entityLiving2.y), Mth.Floor(entityLiving2.z));
                    if (i10 == Tile.bed.id)
                    {
                        int i11 = this.mc.level.GetData(Mth.Floor(entityLiving2.x), Mth.Floor(entityLiving2.y), Mth.Floor(entityLiving2.z));
                        int i12 = i11 & 3;
                        GL11.glRotatef(i12 * 90, 0F, 1F, 0F);
                    }

                    GL11.glRotatef(entityLiving2.prevYaw + (entityLiving2.yaw - entityLiving2.prevYaw) * f1 + 180F, 0F, -1F, 0F);
                    GL11.glRotatef(entityLiving2.prevPitch + (entityLiving2.pitch - entityLiving2.prevPitch) * f1, -1F, 0F, 0F);
                }
            }
            else if (this.mc.options.thirdPersonView)
            {
                double d27 = this.field_22227_s + (this.field_22228_r - this.field_22227_s) * f1;
                float f13;
                float f28;
                if (this.mc.options.field_22273_E)
                {
                    f28 = this.field_22225_u + (this.field_22226_t - this.field_22225_u) * f1;
                    f13 = this.field_22223_w + (this.field_22224_v - this.field_22223_w) * f1;
                    GL11.glTranslatef(0F, 0F, (float)(-d27));
                    GL11.glRotatef(f13, 1F, 0F, 0F);
                    GL11.glRotatef(f28, 0F, 1F, 0F);
                }
                else
                {
                    f28 = entityLiving2.yaw;
                    f13 = entityLiving2.pitch;
                    double d14 = -Mth.Sin(f28 / 180F * Mth.PI) * Mth.Cos(f13 / 180F * Mth.PI) * d27;
                    double d16 = Mth.Cos(f28 / 180F * Mth.PI) * Mth.Cos(f13 / 180F * Mth.PI) * d27;
                    double d18 = (-Mth.Sin(f13 / 180F * Mth.PI)) * d27;
                    for (int i20 = 0; i20 < 8; ++i20)
                    {
                        float f21 = (i20 & 1) * 2 - 1;
                        float f22 = (i20 >> 1 & 1) * 2 - 1;
                        float f23 = (i20 >> 2 & 1) * 2 - 1;
                        f21 *= 0.1F;
                        f22 *= 0.1F;
                        f23 *= 0.1F;
                        HitResult movingObjectPosition24 = this.mc.level.Clip(Vec3.Of(d4 + f21, d6 + f22, d8 + f23), Vec3.Of(d4 - d14 + f21 + f23, d6 - d18 + f22, d8 - d16 + f23));
                        if (movingObjectPosition24 != null)
                        {
                            double d25 = movingObjectPosition24.HitVec.DistanceTo(Vec3.Of(d4, d6, d8));
                            if (d25 < d27)
                            {
                                d27 = d25;
                            }
                        }
                    }

                    GL11.glRotatef(entityLiving2.pitch - f13, 1F, 0F, 0F);
                    GL11.glRotatef(entityLiving2.yaw - f28, 0F, 1F, 0F);
                    GL11.glTranslatef(0F, 0F, (float)(-d27));
                    GL11.glRotatef(f28 - entityLiving2.yaw, 0F, 1F, 0F);
                    GL11.glRotatef(f13 - entityLiving2.pitch, 1F, 0F, 0F);
                }
            }
            else
            {
                GL11.glTranslatef(0F, 0F, -0.1F);
            }

            if (!this.mc.options.field_22273_E)
            {
                GL11.glRotatef(entityLiving2.prevPitch + (entityLiving2.pitch - entityLiving2.prevPitch) * f1, 1F, 0F, 0F);
                GL11.glRotatef(entityLiving2.prevYaw + (entityLiving2.yaw - entityLiving2.prevYaw) * f1 + 180F, 0F, 1F, 0F);
            }

            GL11.glTranslatef(0F, f3, 0F);
            d4 = entityLiving2.prevX + (entityLiving2.x - entityLiving2.prevX) * f1;
            d6 = entityLiving2.prevY + (entityLiving2.y - entityLiving2.prevY) * f1 - f3;
            d8 = entityLiving2.prevZ + (entityLiving2.z - entityLiving2.prevZ) * f1;
            this.cloudFog = this.mc.renderGlobal.Func_27307_a(d4, d6, d8, f1);
        }

        private void SetupCameraTransform(float f1, int i2)
        {
            this.farPlaneDistance = 256 >> this.mc.options.renderDistance;
            GL11.glMatrixMode(GL11C.GL_PROJECTION);
            GL11.glLoadIdentity();
            float f3 = 0.07F;
            if (this.mc.options.anaglyph)
            {
                GL11.glTranslatef((-(i2 * 2 - 1)) * f3, 0F, 0F);
            }

            if (this.cameraZoom != 1)
            {
                GL11.glTranslatef((float)this.cameraYaw, (float)(-this.cameraPitch), 0F);
                GL11.glScaled(this.cameraZoom, this.cameraZoom, 1);
                GLU.gluPerspective(this.GetFOVModifier(f1), (float)this.mc.displayWidth / (float)this.mc.displayHeight, 0.05F, this.farPlaneDistance * 2F);
            }
            else
            {
                GLU.gluPerspective(this.GetFOVModifier(f1), (float)this.mc.displayWidth / (float)this.mc.displayHeight, 0.05F, this.farPlaneDistance * 2F);
            }

            GL11.glMatrixMode(GL11C.GL_MODELVIEW);
            GL11.glLoadIdentity();
            if (this.mc.options.anaglyph)
            {
                GL11.glTranslatef((i2 * 2 - 1) * 0.1F, 0F, 0F);
            }

            this.HurtCameraEffect(f1);
            if (this.mc.options.viewBobbing)
            {
                this.SetupViewBobbing(f1);
            }

            float f4 = this.mc.player.prevTimeInPortal + (this.mc.player.timeInPortal - this.mc.player.prevTimeInPortal) * f1;
            if (f4 > 0F)
            {
                float f5 = 5F / (f4 * f4 + 5F) - f4 * 0.04F;
                f5 *= f5;
                GL11.glRotatef((this.rendererUpdateCount + f1) * 20F, 0F, 1F, 1F);
                GL11.glScalef(1F / f5, 1F, 1F);
                GL11.glRotatef(-(this.rendererUpdateCount + f1) * 20F, 0F, 1F, 1F);
            }

            this.OrientCamera(f1);
        }

        private void RenderHand(float f1, int i2)
        {
            GL11.glLoadIdentity();
            if (this.mc.options.anaglyph)
            {
                GL11.glTranslatef((i2 * 2 - 1) * 0.1F, 0F, 0F);
            }

            GL11.glPushMatrix();
            this.HurtCameraEffect(f1);
            if (this.mc.options.viewBobbing)
            {
                this.SetupViewBobbing(f1);
            }

            if (!this.mc.options.thirdPersonView && !this.mc.renderViewEntity.IsSleeping() && !this.mc.options.hideGUI)
            {
                this.itemRenderer.RenderItemInFirstPerson(f1);
            }

            GL11.glPopMatrix();
            if (!this.mc.options.thirdPersonView && !this.mc.renderViewEntity.IsSleeping())
            {
                this.itemRenderer.RenderOverlays(f1);
                this.HurtCameraEffect(f1);
            }

            if (this.mc.options.viewBobbing)
            {
                this.SetupViewBobbing(f1);
            }
        }

        public virtual void UpdateCameraAndRender(float alpha)
        {
            if (!Display.IsActive())
            {
                if (TimeUtil.MilliTime - this.prevFrameTime > 500)
                {
                    this.mc.DisplayInGameMenu();
                }
            }
            else
            {
                this.prevFrameTime = TimeUtil.MilliTime;
            }

            Profiler.StartSection("mouse");
            if (this.mc.inGameHasFocus)
            {
                this.mc.mouseHelper.Tick();
                float f2 = this.mc.options.mouseSensitivity * 0.6F + 0.2F;
                float f3 = f2 * f2 * f2 * 8F;
                float f4 = this.mc.mouseHelper.deltaX * f3;
                float f5 = this.mc.mouseHelper.deltaY * f3;
                sbyte b6 = 1;
                if (this.mc.options.invertMouse)
                {
                    b6 = -1;
                }

                if (this.mc.options.smoothCamera)
                {
                    f4 = this.mouseFilterXAxis.Apply(f4, 0.05F * f3);
                    f5 = this.mouseFilterYAxis.Apply(f5, 0.05F * f3);
                }

                this.mc.player.Func_346(f4, f5 * b6);
            }

            Profiler.EndSection();
            if (!this.mc.skipRenderWorld)
            {
                field_28135_a = this.mc.options.anaglyph;
                GuiScale scaledResolution13 = new GuiScale(this.mc.options, this.mc.displayWidth, this.mc.displayHeight);
                int i14 = scaledResolution13.GetWidth();
                int i15 = scaledResolution13.GetHeight();
                int i16 = ((int)Mouse.GetX()) * i14 / this.mc.displayWidth;
                int i17 = i15 - ((int)Mouse.GetY()) * i15 / this.mc.displayHeight;
                short s7 = 200;
                if (this.mc.options.limitFramerate == 1)
                {
                    s7 = 120;
                }

                if (this.mc.options.limitFramerate == 2)
                {
                    s7 = 40;
                }

                long j8;
                if (this.mc.level != null)
                {
                    Profiler.StartSection("level");
                    if (this.mc.options.limitFramerate == 0)
                    {
                        this.RenderWorld(alpha, 0);
                    }
                    else
                    {
                        this.RenderWorld(alpha, this.field_28133_I + 1000000000 / s7);
                    }

                    Profiler.EndStartSection("sleep");
                    if (this.mc.options.limitFramerate == 2)
                    {
                        j8 = (this.field_28133_I + 1000000000 / s7 - TimeUtil.NanoTime) / 1000000;
                        if (j8 > 0 && j8 < 500)
                        {
                            try
                            {
                                Thread.Sleep((int)j8);
                            }
                            catch (Exception interruptedException12)
                            {
                                interruptedException12.PrintStackTrace();
                            }
                        }
                    }

                    this.field_28133_I = TimeUtil.NanoTime;
                    Profiler.EndStartSection("gui");
                    if (!this.mc.options.hideGUI || this.mc.currentScreen != null)
                    {
                        this.mc.ingameGUI.RenderGameOverlay(alpha, this.mc.currentScreen != null, i16, i17);
                    }
                    Profiler.EndSection();
                }
                else
                {
                    GL11.glViewport(0, 0, this.mc.displayWidth, this.mc.displayHeight);
                    GL11.glMatrixMode(GL11C.GL_PROJECTION);
                    GL11.glLoadIdentity();
                    GL11.glMatrixMode(GL11C.GL_MODELVIEW);
                    GL11.glLoadIdentity();
                    this.Func_905_b();
                    //why did notch do this instead of using vsync
                    if (this.mc.options.limitFramerate == 2)
                    {
                        j8 = (this.field_28133_I + 1000000000 / s7 - TimeUtil.NanoTime) / 1000000;
                        if (j8 < 0)
                        {
                            j8 += 10;
                        }

                        if (j8 > 0 && j8 < 500)
                        {
                            try
                            {
                                Thread.Sleep((int)j8);
                            }
                            catch (Exception interruptedException11)
                            {
                                interruptedException11.PrintStackTrace();
                            }
                        }
                    }

                    this.field_28133_I = TimeUtil.NanoTime;
                }

                if (this.mc.currentScreen != null)
                {
                    GL11.glClear(GL11C.GL_DEPTH_BUFFER_BIT);
                    this.mc.currentScreen.DrawScreen(i16, i17, alpha);
                }
            }
        }

        public virtual void RenderWorld(float f1, long j2)
        {
            Profiler.StartSection("lightTex");
            GL11.glEnable(GL11C.GL_CULL_FACE);
            GL11.glEnable(GL11C.GL_DEPTH_TEST);
            if (this.mc.renderViewEntity == null)
            {
                this.mc.renderViewEntity = this.mc.player;
            }

            Profiler.EndStartSection("pick");
            this.GetMouseOver(f1);
            Mob entityLiving4 = this.mc.renderViewEntity;
            LevelRenderer renderGlobal5 = this.mc.renderGlobal;
            ParticleEngine effectRenderer6 = this.mc.effectRenderer;
            double d7 = entityLiving4.lastTickPosX + (entityLiving4.x - entityLiving4.lastTickPosX) * f1;
            double d9 = entityLiving4.lastTickPosY + (entityLiving4.y - entityLiving4.lastTickPosY) * f1;
            double d11 = entityLiving4.lastTickPosZ + (entityLiving4.z - entityLiving4.lastTickPosZ) * f1;
            Profiler.EndStartSection("center");
            IChunkSource iChunkProvider13 = this.mc.level.GetChunkSource();
            int i16;
            if (iChunkProvider13 is ChunkCache)
            {
                ChunkCache chunkProviderLoadOrGenerate14 = (ChunkCache)iChunkProvider13;
                int i15 = Mth.Floor(((int)d7)) >> 4;
                i16 = Mth.Floor(((int)d11)) >> 4;
                chunkProviderLoadOrGenerate14.SetPos(i15, i16);
            }

            for (int i18 = 0; i18 < 2; ++i18)
            {
                if (this.mc.options.anaglyph)
                {
                    anaglyphField = i18;
                    if (anaglyphField == 0)
                    {
                        GL11.glColorMask(false, true, true, false);
                    }
                    else
                    {
                        GL11.glColorMask(true, false, false, false);
                    }
                }

                Profiler.EndStartSection("clear");
                GL11.glViewport(0, 0, this.mc.displayWidth, this.mc.displayHeight);
                this.UpdateFogColor(f1);
                GL11.glClear(GL11C.GL_COLOR_BUFFER_BIT | GL11C.GL_DEPTH_BUFFER_BIT);
                GL11.glEnable(GL11C.GL_CULL_FACE);
                Profiler.EndStartSection("camera");
                this.SetupCameraTransform(f1, i18);
                Profiler.EndStartSection("frustrum");
                Frustum.Get();
                if (this.mc.options.renderDistance < 2)
                {
                    this.SetupFog(-1, f1);
                    Profiler.EndStartSection("sky");
                    renderGlobal5.RenderSky(f1);
                }

                GL11.glEnable(GL11C.GL_FOG);
                this.SetupFog(1, f1);
                if (this.mc.options.ambientOcclusion)
                {
                    GL11.glShadeModel(GL11C.GL_SMOOTH);
                }

                Profiler.EndStartSection("culling");
                FrustrumCuller frustrum19 = new FrustrumCuller();
                frustrum19.Prepare(d7, d9, d11);
                this.mc.renderGlobal.ClipRenderersByFrustrum(frustrum19, f1);
                if (i18 == 0)
                {
                    Profiler.EndStartSection("updatechunks");

                    while (!this.mc.renderGlobal.UpdateRenderers(entityLiving4, false) && j2 != 0)
                    {
                        long j20 = j2 - TimeUtil.NanoTime;
                        if (j20 < 0 || j20 > 1000000000)
                        {
                            break;
                        }
                    }
                }

                this.SetupFog(0, f1);
                GL11.glEnable(GL11C.GL_FOG);
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/terrain.png"));
                Light.TurnOff();
                Profiler.EndStartSection("terrain");
                renderGlobal5.SortAndRender(entityLiving4, 0, f1);
                GL11.glShadeModel(GL11C.GL_FLAT);
                Light.TurnOn();
                Profiler.EndStartSection("entities");
                renderGlobal5.RenderEntities(entityLiving4.GetPosition(f1), frustrum19, f1);
                Profiler.EndStartSection("litParticles");
                effectRenderer6.Func_1187_b(entityLiving4, f1);
                Light.TurnOff();
                this.SetupFog(0, f1);
                Profiler.EndStartSection("particles");
                effectRenderer6.RenderParticles(entityLiving4, f1);
                Player entityPlayer21;
                if (this.mc.objectMouseOver != null && entityLiving4.IsInsideOfMaterial(Material.water) && entityLiving4 is Player)
                {
                    entityPlayer21 = (Player)entityLiving4;
                    GL11.glDisable(GL11C.GL_ALPHA_TEST);
                    Profiler.EndStartSection("outline");
                    renderGlobal5.DrawBlockBreaking(entityPlayer21, this.mc.objectMouseOver, 0, entityPlayer21.inventory.GetCurrentItem(), f1);
                    renderGlobal5.DrawSelectionBox(entityPlayer21, this.mc.objectMouseOver, 0, entityPlayer21.inventory.GetCurrentItem(), f1);
                    GL11.glEnable(GL11C.GL_ALPHA_TEST);
                }

                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                this.SetupFog(0, f1);
                GL11.glEnable(GL11C.GL_BLEND);
                GL11.glDisable(GL11C.GL_CULL_FACE);
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/terrain.png"));
                if (this.mc.options.fancyGraphics)
                {
                    Profiler.EndStartSection("water");
                    if (this.mc.options.ambientOcclusion)
                    {
                        GL11.glShadeModel(GL11C.GL_SMOOTH);
                    }

                    GL11.glColorMask(false, false, false, false);
                    i16 = renderGlobal5.SortAndRender(entityLiving4, 1, f1);
                    if (this.mc.options.anaglyph)
                    {
                        if (anaglyphField == 0)
                        {
                            GL11.glColorMask(false, true, true, true);
                        }
                        else
                        {
                            GL11.glColorMask(true, false, false, true);
                        }
                    }
                    else
                    {
                        GL11.glColorMask(true, true, true, true);
                    }

                    if (i16 > 0)
                    {
                        renderGlobal5.RenderAllRenderLists(1, f1);
                    }

                    GL11.glShadeModel(GL11C.GL_FLAT);
                }
                else
                {
                    Profiler.EndStartSection("water");
                    renderGlobal5.SortAndRender(entityLiving4, 1, f1);
                }

                GL11.glDepthMask(true);
                GL11.glEnable(GL11C.GL_CULL_FACE);
                GL11.glDisable(GL11C.GL_BLEND);
                if (this.cameraZoom == 1 && entityLiving4 is Player && this.mc.objectMouseOver != null && !entityLiving4.IsInsideOfMaterial(Material.water))
                {
                    entityPlayer21 = (Player)entityLiving4;
                    GL11.glDisable(GL11C.GL_ALPHA_TEST);
                    Profiler.EndStartSection("outline");
                    renderGlobal5.DrawBlockBreaking(entityPlayer21, this.mc.objectMouseOver, 0, entityPlayer21.inventory.GetCurrentItem(), f1);
                    renderGlobal5.DrawSelectionBox(entityPlayer21, this.mc.objectMouseOver, 0, entityPlayer21.inventory.GetCurrentItem(), f1);
                    GL11.glEnable(GL11C.GL_ALPHA_TEST);
                }

                Profiler.EndStartSection("weather");
                this.RenderRainSnow(f1);
                GL11.glDisable(GL11C.GL_FOG);
                if (this.pointedEntity != null)
                {
                }

                Profiler.EndStartSection("clouds");
                this.SetupFog(0, f1);
                GL11.glEnable(GL11C.GL_FOG);
                renderGlobal5.RenderClouds(f1);
                GL11.glDisable(GL11C.GL_FOG);
                this.SetupFog(1, f1);

                Profiler.EndStartSection("hand");
                if (this.cameraZoom == 1)
                {
                    GL11.glClear(GL11C.GL_DEPTH_BUFFER_BIT);
                    this.RenderHand(f1, i18);
                }

                if (!this.mc.options.anaglyph)
                {
                    Profiler.EndSection();
                    return;
                }
            }

            GL11.glColorMask(true, true, true, false);
            Profiler.EndSection();
        }

        private void AddRainParticles()
        {
            float f1 = this.mc.level.Func_g(1F);
            if (!this.mc.options.fancyGraphics)
            {
                f1 /= 2F;
            }

            if (f1 != 0F)
            {
                this.random.SetSeed(this.rendererUpdateCount * 312987231);
                Mob entityLiving2 = this.mc.renderViewEntity;
                Level world3 = this.mc.level;
                int i4 = Mth.Floor(entityLiving2.x);
                int i5 = Mth.Floor(entityLiving2.y);
                int i6 = Mth.Floor(entityLiving2.z);
                byte b7 = 10;
                double d8 = 0;
                double d10 = 0;
                double d12 = 0;
                int i14 = 0;
                for (int i15 = 0; i15 < (int)(100F * f1 * f1); ++i15)
                {
                    int i16 = i4 + this.random.NextInt(b7) - this.random.NextInt(b7);
                    int i17 = i6 + this.random.NextInt(b7) - this.random.NextInt(b7);
                    int i18 = world3.GetTopSolidBlock(i16, i17);
                    int i19 = world3.GetTile(i16, i18 - 1, i17);
                    if (i18 <= i5 + b7 && i18 >= i5 - b7 && world3.GetBiomeSource().GetBiomeGenAt(i16, i17).CanThunder())
                    {
                        float f20 = this.random.NextFloat();
                        float f21 = this.random.NextFloat();
                        if (i19 > 0)
                        {
                            if (Tile.tiles[i19].material == Material.lava)
                            {
                                this.mc.effectRenderer.AddEffect(new EntitySmokeFX(world3, i16 + f20, i18 + 0.1F - Tile.tiles[i19].minY, i17 + f21, 0, 0, 0));
                            }
                            else
                            {
                                ++i14;
                                if (this.random.NextInt(i14) == 0)
                                {
                                    d8 = i16 + f20;
                                    d10 = i18 + 0.1F - Tile.tiles[i19].minY;
                                    d12 = i17 + f21;
                                }

                                this.mc.effectRenderer.AddEffect(new EntityRainFX(world3, i16 + f20, i18 + 0.1F - Tile.tiles[i19].minY, i17 + f21));
                            }
                        }
                    }
                }

                if (i14 > 0 && this.random.NextInt(3) < this.rainSoundCounter++)
                {
                    this.rainSoundCounter = 0;
                    if (d10 > entityLiving2.y + 1 && world3.GetTopSolidBlock(Mth.Floor(entityLiving2.x), Mth.Floor(entityLiving2.z)) > Mth.Floor(entityLiving2.y))
                    {
                        this.mc.level.PlaySound(d8, d10, d12, "ambient.weather.rain", 0.1F, 0.5F);
                    }
                    else
                    {
                        this.mc.level.PlaySound(d8, d10, d12, "ambient.weather.rain", 0.2F, 1F);
                    }
                }
            }
        }

        protected virtual void RenderRainSnow(float f1)
        {
            float f2 = this.mc.level.Func_g(f1);
            if (f2 > 0F)
            {
                Mob entityLiving3 = this.mc.renderViewEntity;
                Level world4 = this.mc.level;
                int i5 = Mth.Floor(entityLiving3.x);
                int i6 = Mth.Floor(entityLiving3.y);
                int i7 = Mth.Floor(entityLiving3.z);
                Tessellator tessellator8 = Tessellator.Instance;
                GL11.glDisable(GL11C.GL_CULL_FACE);
                GL11.glNormal3f(0F, 1F, 0F);
                GL11.glEnable(GL11C.GL_BLEND);
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                GL11.glAlphaFunc(GL11C.GL_GREATER, 0.01F);
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/environment/snow.png"));
                double d9 = entityLiving3.lastTickPosX + (entityLiving3.x - entityLiving3.lastTickPosX) * f1;
                double d11 = entityLiving3.lastTickPosY + (entityLiving3.y - entityLiving3.lastTickPosY) * f1;
                double d13 = entityLiving3.lastTickPosZ + (entityLiving3.z - entityLiving3.lastTickPosZ) * f1;
                int i15 = Mth.Floor(d11);
                byte b16 = 5;
                if (this.mc.options.fancyGraphics)
                {
                    b16 = 10;
                }

                Biome[] biomeGenBase17 = world4.GetBiomeSource().Func_a(i5 - b16, i7 - b16, b16 * 2 + 1, b16 * 2 + 1);
                int i18 = 0;
                int i19;
                int i20;
                Biome biomeGenBase21;
                int i22;
                int i23;
                int i24;
                float f26;
                for (i19 = i5 - b16; i19 <= i5 + b16; ++i19)
                {
                    for (i20 = i7 - b16; i20 <= i7 + b16; ++i20)
                    {
                        biomeGenBase21 = biomeGenBase17[i18++];
                        if (biomeGenBase21.IsSnowCovered())
                        {
                            i22 = world4.GetTopSolidBlock(i19, i20);
                            if (i22 < 0)
                            {
                                i22 = 0;
                            }

                            i23 = i22;
                            if (i22 < i15)
                            {
                                i23 = i15;
                            }

                            i24 = i6 - b16;
                            int i25 = i6 + b16;
                            if (i24 < i22)
                            {
                                i24 = i22;
                            }

                            if (i25 < i22)
                            {
                                i25 = i22;
                            }

                            f26 = 1F;
                            if (i24 != i25)
                            {
                                this.random.SetSeed(i19 * i19 * 3121 + i19 * 45238971 + i20 * i20 * 418711 + i20 * 13761);
                                float f27 = this.rendererUpdateCount + f1;
                                float f28 = ((this.rendererUpdateCount & 511) + f1) / 512F;
                                float f29 = this.random.NextFloat() + f27 * 0.01F * (float)this.random.NextGaussian();
                                float f30 = this.random.NextFloat() + f27 * (float)this.random.NextGaussian() * 0.001F;
                                double d31 = i19 + 0.5F - entityLiving3.x;
                                double d33 = i20 + 0.5F - entityLiving3.z;
                                float f35 = Mth.Sqrt(d31 * d31 + d33 * d33) / b16;
                                tessellator8.Begin();
                                float f36 = world4.GetBrightness(i19, i23, i20);
                                GL11.glColor4f(f36, f36, f36, ((1F - f35 * f35) * 0.3F + 0.5F) * f2);
                                tessellator8.Offset(-d9 * 1, -d11 * 1, -d13 * 1);
                                tessellator8.VertexUV(i19 + 0, i24, i20 + 0.5, 0F * f26 + f29, i24 * f26 / 4F + f28 * f26 + f30);
                                tessellator8.VertexUV(i19 + 1, i24, i20 + 0.5, 1F * f26 + f29, i24 * f26 / 4F + f28 * f26 + f30);
                                tessellator8.VertexUV(i19 + 1, i25, i20 + 0.5, 1F * f26 + f29, i25 * f26 / 4F + f28 * f26 + f30);
                                tessellator8.VertexUV(i19 + 0, i25, i20 + 0.5, 0F * f26 + f29, i25 * f26 / 4F + f28 * f26 + f30);
                                tessellator8.VertexUV(i19 + 0.5, i24, i20 + 0, 0F * f26 + f29, i24 * f26 / 4F + f28 * f26 + f30);
                                tessellator8.VertexUV(i19 + 0.5, i24, i20 + 1, 1F * f26 + f29, i24 * f26 / 4F + f28 * f26 + f30);
                                tessellator8.VertexUV(i19 + 0.5, i25, i20 + 1, 1F * f26 + f29, i25 * f26 / 4F + f28 * f26 + f30);
                                tessellator8.VertexUV(i19 + 0.5, i25, i20 + 0, 0F * f26 + f29, i25 * f26 / 4F + f28 * f26 + f30);
                                tessellator8.Offset(0, 0, 0);
                                tessellator8.End();
                            }
                        }
                    }
                }

                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/environment/rain.png"));
                if (this.mc.options.fancyGraphics)
                {
                    b16 = 10;
                }

                i18 = 0;
                for (i19 = i5 - b16; i19 <= i5 + b16; ++i19)
                {
                    for (i20 = i7 - b16; i20 <= i7 + b16; ++i20)
                    {
                        biomeGenBase21 = biomeGenBase17[i18++];
                        if (biomeGenBase21.CanThunder())
                        {
                            i22 = world4.GetTopSolidBlock(i19, i20);
                            i23 = i6 - b16;
                            i24 = i6 + b16;
                            if (i23 < i22)
                            {
                                i23 = i22;
                            }

                            if (i24 < i22)
                            {
                                i24 = i22;
                            }

                            float f37 = 1F;
                            if (i23 != i24)
                            {
                                this.random.SetSeed(i19 * i19 * 3121 + i19 * 45238971 + i20 * i20 * 418711 + i20 * 13761);
                                f26 = ((this.rendererUpdateCount + i19 * i19 * 3121 + i19 * 45238971 + i20 * i20 * 418711 + i20 * 13761 & 31) + f1) / 32F * (3F + this.random.NextFloat());
                                double d38 = i19 + 0.5F - entityLiving3.x;
                                double d39 = i20 + 0.5F - entityLiving3.z;
                                float f40 = Mth.Sqrt(d38 * d38 + d39 * d39) / b16;
                                tessellator8.Begin();
                                float f32 = world4.GetBrightness(i19, 128, i20) * 0.85F + 0.15F;
                                GL11.glColor4f(f32, f32, f32, ((1F - f40 * f40) * 0.5F + 0.5F) * f2);
                                tessellator8.Offset(-d9 * 1, -d11 * 1, -d13 * 1);
                                tessellator8.VertexUV(i19 + 0, i23, i20 + 0.5, 0F * f37, i23 * f37 / 4F + f26 * f37);
                                tessellator8.VertexUV(i19 + 1, i23, i20 + 0.5, 1F * f37, i23 * f37 / 4F + f26 * f37);
                                tessellator8.VertexUV(i19 + 1, i24, i20 + 0.5, 1F * f37, i24 * f37 / 4F + f26 * f37);
                                tessellator8.VertexUV(i19 + 0, i24, i20 + 0.5, 0F * f37, i24 * f37 / 4F + f26 * f37);
                                tessellator8.VertexUV(i19 + 0.5, i23, i20 + 0, 0F * f37, i23 * f37 / 4F + f26 * f37);
                                tessellator8.VertexUV(i19 + 0.5, i23, i20 + 1, 1F * f37, i23 * f37 / 4F + f26 * f37);
                                tessellator8.VertexUV(i19 + 0.5, i24, i20 + 1, 1F * f37, i24 * f37 / 4F + f26 * f37);
                                tessellator8.VertexUV(i19 + 0.5, i24, i20 + 0, 0F * f37, i24 * f37 / 4F + f26 * f37);
                                tessellator8.Offset(0, 0, 0);
                                tessellator8.End();
                            }
                        }
                    }
                }

                GL11.glEnable(GL11C.GL_CULL_FACE);
                GL11.glDisable(GL11C.GL_BLEND);
                GL11.glAlphaFunc(GL11C.GL_GREATER, 0.1F);
            }
        }

        public virtual void Func_905_b()
        {
            GuiScale scaledResolution1 = new GuiScale(this.mc.options, this.mc.displayWidth, this.mc.displayHeight);
            GL11.glClear(GL11C.GL_DEPTH_BUFFER_BIT);
            GL11.glMatrixMode(GL11C.GL_PROJECTION);
            GL11.glLoadIdentity();
            GL11.glOrtho(0, scaledResolution1.WidthScale, scaledResolution1.HeightScale, 0, 1000, 3000);
            GL11.glMatrixMode(GL11C.GL_MODELVIEW);
            GL11.glLoadIdentity();
            GL11.glTranslatef(0F, 0F, -2000F);
        }

        private void UpdateFogColor(float f1)
        {
            Level world2 = this.mc.level;
            Mob entityLiving3 = this.mc.renderViewEntity;
            float f4 = 1F / (4 - this.mc.options.renderDistance);
            f4 = 1F - (float)Math.Pow(f4, 0.25);
            Vec3 vec3D5 = world2.GetSkyColor(this.mc.renderViewEntity, f1);
            float f6 = (float)vec3D5.x;
            float f7 = (float)vec3D5.y;
            float f8 = (float)vec3D5.z;
            Vec3 vec3D9 = world2.GetFogColor(f1);
            this.fogColorRed = (float)vec3D9.x;
            this.fogColorGreen = (float)vec3D9.y;
            this.fogColorBlue = (float)vec3D9.z;
            this.fogColorRed += (f6 - this.fogColorRed) * f4;
            this.fogColorGreen += (f7 - this.fogColorGreen) * f4;
            this.fogColorBlue += (f8 - this.fogColorBlue) * f4;
            float f10 = world2.Func_g(f1);
            float f11;
            float f12;
            if (f10 > 0F)
            {
                f11 = 1F - f10 * 0.5F;
                f12 = 1F - f10 * 0.4F;
                this.fogColorRed *= f11;
                this.fogColorGreen *= f11;
                this.fogColorBlue *= f12;
            }

            f11 = world2.Func_f(f1);
            if (f11 > 0F)
            {
                f12 = 1F - f11 * 0.5F;
                this.fogColorRed *= f12;
                this.fogColorGreen *= f12;
                this.fogColorBlue *= f12;
            }

            if (this.cloudFog)
            {
                Vec3 vec3D16 = world2.GetCloudColor(f1);
                this.fogColorRed = (float)vec3D16.x;
                this.fogColorGreen = (float)vec3D16.y;
                this.fogColorBlue = (float)vec3D16.z;
            }
            else if (entityLiving3.IsInsideOfMaterial(Material.water))
            {
                this.fogColorRed = 0.02F;
                this.fogColorGreen = 0.02F;
                this.fogColorBlue = 0.2F;
            }
            else if (entityLiving3.IsInsideOfMaterial(Material.lava))
            {
                this.fogColorRed = 0.6F;
                this.fogColorGreen = 0.1F;
                this.fogColorBlue = 0F;
            }

            f12 = this.fogColor2 + (this.fogColor1 - this.fogColor2) * f1;
            this.fogColorRed *= f12;
            this.fogColorGreen *= f12;
            this.fogColorBlue *= f12;
            if (this.mc.options.anaglyph)
            {
                float f13 = (this.fogColorRed * 30F + this.fogColorGreen * 59F + this.fogColorBlue * 11F) / 100F;
                float f14 = (this.fogColorRed * 30F + this.fogColorGreen * 70F) / 100F;
                float f15 = (this.fogColorRed * 30F + this.fogColorBlue * 70F) / 100F;
                this.fogColorRed = f13;
                this.fogColorGreen = f14;
                this.fogColorBlue = f15;
            }

            GL11.glClearColor(this.fogColorRed, this.fogColorGreen, this.fogColorBlue, 0F);
        }

        private void SetupFog(int i1, float f2)
        {
            Mob entityLiving3 = this.mc.renderViewEntity;
            //GL11.glFogfv(GL11C.GL_FOG_COLOR, this.GetColorBuffer(this.fogColorRed, this.fogColorGreen, this.fogColorBlue, 1F));
            Fogfv(GL11C.GL_FOG_COLOR, fogColorRed, fogColorGreen, fogColorBlue, 1f);
            GL11.glNormal3f(0F, -1F, 0F);
            GL11.glColor4f(1F, 1F, 1F, 1F);
            if (this.cloudFog)
            {
                GL11.glFogi(GL11C.GL_FOG_MODE, (int)GL11C.GL_EXP);
                GL11.glFogf(GL11C.GL_FOG_DENSITY, 0.1F);
                if (this.mc.options.anaglyph)
                {
                }
            }
            else if (entityLiving3.IsInsideOfMaterial(Material.water))
            {
                GL11.glFogi(GL11C.GL_FOG_MODE, (int)GL11C.GL_EXP);
                GL11.glFogf(GL11C.GL_FOG_DENSITY, 0.1F);
                if (this.mc.options.anaglyph)
                {
                }
            }
            else if (entityLiving3.IsInsideOfMaterial(Material.lava))
            {
                GL11.glFogi(GL11C.GL_FOG_MODE, (int)GL11C.GL_EXP);
                GL11.glFogf(GL11C.GL_FOG_DENSITY, 2F);
                if (this.mc.options.anaglyph)
                {
                }
            }
            else
            {
                GL11.glFogi(GL11C.GL_FOG_MODE, (int)GL11C.GL_LINEAR);
                GL11.glFogf(GL11C.GL_FOG_START, this.farPlaneDistance * 0.25F);
                GL11.glFogf(GL11C.GL_FOG_END, this.farPlaneDistance);
                if (i1 < 0)
                {
                    GL11.glFogf(GL11C.GL_FOG_START, 0F);
                    GL11.glFogf(GL11C.GL_FOG_END, this.farPlaneDistance * 0.8F);
                }

                if (this.mc.GetGLCapabilities().GL_NV_fog_distance)
                {
                    GL11.glFogi(34138, 34139);
                }

                if (this.mc.level.dimension.isNether)
                {
                    GL11.glFogf(GL11C.GL_FOG_START, 0F);
                }
            }

            GL11.glEnable(GL11C.GL_COLOR_MATERIAL);
            GL11.glColorMaterial(GL11C.GL_FRONT, GL11C.GL_AMBIENT);
        }

        private static unsafe void Fogfv(uint pname, float red, float green, float blue, float alpha) 
        {
            Span<float> floats = stackalloc float[4] { red, green, blue, alpha };
            fixed (float* ptr = floats) 
            {
                GL11_PTR.glFogfv(pname, ptr);
            }
        }
        /*
        private float[] GetColorBuffer(float f1, float f2, float f3, float f4)
        {
            fogColorBuffer[0] = f1;
            fogColorBuffer[1] = f2;
            fogColorBuffer[2] = f3;
            fogColorBuffer[3] = f4;
            return fogColorBuffer;
        }
        */
    }
}
