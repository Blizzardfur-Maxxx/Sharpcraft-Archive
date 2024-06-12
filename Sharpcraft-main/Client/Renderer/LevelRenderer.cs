using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Client.Renderer.Culling;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Client.Renderer.Tileentities;
using SharpCraft.Client.Particles;
using SharpCraft.Core;
using System.Runtime.InteropServices;

namespace SharpCraft.Client.Renderer
{
    public class LevelRenderer : ILevelListener
    {
        public List<TileEntity> tileEntities = new List<TileEntity>();
        private Level worldObj;
        private Textures renderEngine;
        private List<Chunk> worldRenderersToUpdate = new List<Chunk>();
        private Chunk[] sortedWorldRenderers;
        private Chunk[] worldRenderers;
        private int renderChunksWide;
        private int renderChunksTall;
        private int renderChunksDeep;
        private uint glRenderListBase;
        private Client mc;
        private TileRenderer globalRenderBlocks;
        private uint[] glOcclusionQueryBase;
        private bool occlusionEnabled = false;
        private int cloudOffsetX = 0;
        private uint starGLCallList;
        private uint glSkyList;
        private uint glSkyList2;
        private int minBlockX;
        private int minBlockY;
        private int minBlockZ;
        private int maxBlockX;
        private int maxBlockY;
        private int maxBlockZ;
        private int renderDistance = -1;
        private int renderEntitiesStartupCounter = 2;
        private int countEntitiesTotal;
        private int countEntitiesRendered;
        private int countEntitiesHidden;
        private uint[] occlusionResult = new uint[64];
        private int renderersLoaded;
        private int renderersBeingClipped;
        private int renderersBeingOccluded;
        private int renderersBeingRendered;
        private int renderersSkippingRenderPass;
        private int worldRenderersCheckIndex;
        private IList<Chunk> glRenderLists = new List<Chunk>();
        private RenderList[] allRenderLists = new[]
        {
            new RenderList(),
            new RenderList(),
            new RenderList(),
            new RenderList()
        };
        double prevSortX = -9999;
        double prevSortY = -9999;
        double prevSortZ = -9999;
        public float damagePartialTime;
        int frustrumCheckOffset = 0;

        public LevelRenderer(Client instance, Textures renderEngine2)
        {
            this.mc = instance;
            this.renderEngine = renderEngine2;
            byte b3 = 64;
            this.glRenderListBase = MemoryTracker.GenLists(b3 * b3 * b3 * 3);
            this.occlusionEnabled = instance.GetGLCapabilities().GL_ARB_occlusion_query;

            if (this.occlusionEnabled)
            {
                //this.occlusionResult.Clear();
                this.glOcclusionQueryBase = new uint[b3 * b3 * b3];//new Buffer<uint>(b3 * b3 * b3);

                //dunno about freeing these cause the level renderer is created once on startup and nuked when runtime ends
                //same with the textures
                //this.glOcclusionQueryBase.Clear();
                //this.glOcclusionQueryBase.Position(0);
                //this.glOcclusionQueryBase.Limit(b3 * b3 * b3);
                unsafe
                {
                    fixed (uint* ptr = &glOcclusionQueryBase[0])
                    {
                        GLARB_PTR.glGenQueriesARB(glOcclusionQueryBase.Length, ptr);
                    }
                }
            }

            this.starGLCallList = MemoryTracker.GenLists(3);
            GL11.glPushMatrix();
            GL11.glNewList(this.starGLCallList, GL11C.GL_COMPILE);
            this.RenderStars();
            GL11.glEndList();
            GL11.glPopMatrix();
            Tessellator tessellator4 = Tessellator.Instance;
            this.glSkyList = this.starGLCallList + 1;
            GL11.glNewList(this.glSkyList, GL11C.GL_COMPILE);
            byte b6 = 64;
            int i7 = 256 / b6 + 2;
            float f5 = 16F;
            int i8;
            int i9;
            for (i8 = -b6 * i7; i8 <= b6 * i7; i8 += b6)
            {
                for (i9 = -b6 * i7; i9 <= b6 * i7; i9 += b6)
                {
                    tessellator4.Begin();
                    tessellator4.Vertex(i8 + 0, f5, i9 + 0);
                    tessellator4.Vertex(i8 + b6, f5, i9 + 0);
                    tessellator4.Vertex(i8 + b6, f5, i9 + b6);
                    tessellator4.Vertex(i8 + 0, f5, i9 + b6);
                    tessellator4.End();
                }
            }

            GL11.glEndList();
            this.glSkyList2 = this.starGLCallList + 2;
            GL11.glNewList(this.glSkyList2, GL11C.GL_COMPILE);
            f5 = -16F;
            tessellator4.Begin();
            for (i8 = -b6 * i7; i8 <= b6 * i7; i8 += b6)
            {
                for (i9 = -b6 * i7; i9 <= b6 * i7; i9 += b6)
                {
                    tessellator4.Vertex(i8 + b6, f5, i9 + 0);
                    tessellator4.Vertex(i8 + 0, f5, i9 + 0);
                    tessellator4.Vertex(i8 + 0, f5, i9 + b6);
                    tessellator4.Vertex(i8 + b6, f5, i9 + b6);
                }
            }

            tessellator4.End();
            GL11.glEndList();
        }

        private void RenderStars()
        {
            JRandom random1 = new JRandom(10842);
            Tessellator tessellator2 = Tessellator.Instance;
            tessellator2.Begin();
            for (int i3 = 0; i3 < 1500; ++i3)
            {
                double d4 = random1.NextFloat() * 2F - 1F;
                double d6 = random1.NextFloat() * 2F - 1F;
                double d8 = random1.NextFloat() * 2F - 1F;
                double d10 = 0.25F + random1.NextFloat() * 0.25F;
                double d12 = d4 * d4 + d6 * d6 + d8 * d8;
                if (d12 < 1 && d12 > 0.01)
                {
                    d12 = 1 / Math.Sqrt(d12);
                    d4 *= d12;
                    d6 *= d12;
                    d8 *= d12;
                    double d14 = d4 * 100;
                    double d16 = d6 * 100;
                    double d18 = d8 * 100;
                    double d20 = Math.Atan2(d4, d8);
                    double d22 = Math.Sin(d20);
                    double d24 = Math.Cos(d20);
                    double d26 = Math.Atan2(Math.Sqrt(d4 * d4 + d8 * d8), d6);
                    double d28 = Math.Sin(d26);
                    double d30 = Math.Cos(d26);
                    double d32 = random1.NextDouble() * Math.PI * 2;
                    double d34 = Math.Sin(d32);
                    double d36 = Math.Cos(d32);
                    for (int i38 = 0; i38 < 4; ++i38)
                    {
                        double d39 = 0;
                        double d41 = ((i38 & 2) - 1) * d10;
                        double d43 = ((i38 + 1 & 2) - 1) * d10;
                        double d47 = d41 * d36 - d43 * d34;
                        double d49 = d43 * d36 + d41 * d34;
                        double d53 = d47 * d28 + d39 * d30;
                        double d55 = d39 * d28 - d47 * d30;
                        double d57 = d55 * d22 - d49 * d24;
                        double d61 = d49 * d22 + d55 * d24;
                        tessellator2.Vertex(d14 + d57, d16 + d53, d18 + d61);
                    }
                }
            }

            tessellator2.End();
        }

        public virtual void ChangeWorld(Level world1)
        {
            if (this.worldObj != null)
            {
                this.worldObj.RemoveListener(this);
            }

            this.prevSortX = -9999;
            this.prevSortY = -9999;
            this.prevSortZ = -9999;
            EntityRenderDispatcher.instance.SetLevel(world1);
            this.worldObj = world1;
            this.globalRenderBlocks = new TileRenderer(world1);
            if (world1 != null)
            {
                world1.AddListener(this);
                this.LoadRenderers();
            }
        }

        public virtual void LoadRenderers()
        {
            Tile.leaves.SetGraphicsLevel(this.mc.options.fancyGraphics);
            this.renderDistance = this.mc.options.renderDistance;
            int i1;
            if (this.worldRenderers != null)
            {
                for (i1 = 0; i1 < this.worldRenderers.Length; ++i1)
                {
                    this.worldRenderers[i1].Func_1204_c();
                }
            }
            //512
            i1 = 64 << 3 - this.renderDistance;
            if (i1 > 400)
            {
                i1 = 400;
            }

            this.renderChunksWide = i1 / 16 + 1;
            this.renderChunksTall = 8;
            this.renderChunksDeep = i1 / 16 + 1;
            this.worldRenderers = new Chunk[this.renderChunksWide * this.renderChunksTall * this.renderChunksDeep];
            this.sortedWorldRenderers = new Chunk[this.renderChunksWide * this.renderChunksTall * this.renderChunksDeep];
            uint i2 = 0;
            int i3 = 0;
            this.minBlockX = 0;
            this.minBlockY = 0;
            this.minBlockZ = 0;
            this.maxBlockX = this.renderChunksWide;
            this.maxBlockY = this.renderChunksTall;
            this.maxBlockZ = this.renderChunksDeep;
            int i4;
            for (i4 = 0; i4 < this.worldRenderersToUpdate.Count; ++i4)
            {
                this.worldRenderersToUpdate[i4].needsUpdate = false;
            }

            this.worldRenderersToUpdate.Clear();
            this.tileEntities.Clear();
            for (i4 = 0; i4 < this.renderChunksWide; ++i4)
            {
                for (int i5 = 0; i5 < this.renderChunksTall; ++i5)
                {
                    for (int i6 = 0; i6 < this.renderChunksDeep; ++i6)
                    {
                        this.worldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4] = new Chunk(this.worldObj, this.tileEntities, i4 * 16, i5 * 16, i6 * 16, 16, this.glRenderListBase + i2);
                        if (this.occlusionEnabled)
                        {
                            this.worldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4].glOcclusionQuery = this.glOcclusionQueryBase[i3];
                        }

                        this.worldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4].isWaitingOnOcclusionQuery = false;
                        this.worldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4].isVisible = true;
                        this.worldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4].isInFrustum = true;
                        this.worldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4].chunkIndex = i3++;
                        this.worldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4].MarkDirty();
                        this.sortedWorldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4] = this.worldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4];
                        this.worldRenderersToUpdate.Add(this.worldRenderers[(i6 * this.renderChunksTall + i5) * this.renderChunksWide + i4]);
                        i2 += 3;
                    }
                }
            }

            if (this.worldObj != null)
            {
                Mob entityLiving7 = this.mc.renderViewEntity;
                if (entityLiving7 != null)
                {
                    this.MarkRenderersForNewPosition(Mth.Floor(entityLiving7.x), Mth.Floor(entityLiving7.y), Mth.Floor(entityLiving7.z));
                    Array.Sort(this.sortedWorldRenderers, new EntitySorter(entityLiving7));
                }
            }

            this.renderEntitiesStartupCounter = 2;
        }

        public virtual void RenderEntities(Vec3 vec3D1, ICuller iCamera2, float f3)
        {
            if (this.renderEntitiesStartupCounter > 0)
            {
                --this.renderEntitiesStartupCounter;
            }
            else
            {
                Profiler.StartSection("prepare");
                TileEntityRenderDispatcher.instance.CacheActiveRenderInfo(this.worldObj, this.renderEngine, this.mc.font, this.mc.renderViewEntity, f3);
                EntityRenderDispatcher.instance.CacheActiveRenderInfo(this.worldObj, this.renderEngine, this.mc.font, this.mc.renderViewEntity, this.mc.options, f3);
                this.countEntitiesTotal = 0;
                this.countEntitiesRendered = 0;
                this.countEntitiesHidden = 0;
                Mob entityLiving4 = this.mc.renderViewEntity;
                EntityRenderDispatcher.renderPosX = entityLiving4.lastTickPosX + (entityLiving4.x - entityLiving4.lastTickPosX) * f3;
                EntityRenderDispatcher.renderPosY = entityLiving4.lastTickPosY + (entityLiving4.y - entityLiving4.lastTickPosY) * f3;
                EntityRenderDispatcher.renderPosZ = entityLiving4.lastTickPosZ + (entityLiving4.z - entityLiving4.lastTickPosZ) * f3;
                TileEntityRenderDispatcher.staticPlayerX = entityLiving4.lastTickPosX + (entityLiving4.x - entityLiving4.lastTickPosX) * f3;
                TileEntityRenderDispatcher.staticPlayerY = entityLiving4.lastTickPosY + (entityLiving4.y - entityLiving4.lastTickPosY) * f3;
                TileEntityRenderDispatcher.staticPlayerZ = entityLiving4.lastTickPosZ + (entityLiving4.z - entityLiving4.lastTickPosZ) * f3;
                Profiler.EndStartSection("global");
                IList<Entity> loadedEntities = this.worldObj.GetLoadedEntityList();
                this.countEntitiesTotal = loadedEntities.Count;
                int i;
                Entity entity7;
                for (i = 0; i < this.worldObj.weatherEffects.Count; ++i)
                {
                    entity7 = this.worldObj.weatherEffects[i];
                    ++this.countEntitiesRendered;
                    if (entity7.IsInRangeToRenderVec3D(vec3D1))
                    {
                        EntityRenderDispatcher.instance.RenderEntity(entity7, f3);
                    }
                }

                Profiler.EndStartSection("entities");

                for (i = 0; i < loadedEntities.Count; ++i)
                {
                    entity7 = loadedEntities[i];
                    if (entity7.IsInRangeToRenderVec3D(vec3D1) && (entity7.ignoreFrustumCheck || iCamera2.IsVisible(entity7.boundingBox)) && (entity7 != this.mc.renderViewEntity || this.mc.options.thirdPersonView || this.mc.renderViewEntity.IsSleeping()))
                    {
                        int iy = Mth.Floor(entity7.y);
                        if (iy < 0)
                        {
                            iy = 0;
                        }

                        if (iy >= 128)
                        {
                            iy = 127;
                        }

                        if (this.worldObj.HasChunkAt(Mth.Floor(entity7.x), iy, Mth.Floor(entity7.z)))
                        {
                            ++this.countEntitiesRendered;
                            EntityRenderDispatcher.instance.RenderEntity(entity7, f3);
                        }
                    }
                }

                Profiler.EndStartSection("tileentities");

                for (i = 0; i < this.tileEntities.Count; ++i)
                {
                    TileEntityRenderDispatcher.instance.RenderTileEntity(this.tileEntities[i], f3);
                }

                Profiler.EndSection();
            }
        }

        public virtual string GetDebugInfoRenders()
        {
            return "C: " + this.renderersBeingRendered + "/" + this.renderersLoaded + ". F: " + this.renderersBeingClipped + ", O: " + this.renderersBeingOccluded + ", E: " + this.renderersSkippingRenderPass;
        }

        public virtual string GetDebugInfoEntities()
        {
            return "E: " + this.countEntitiesRendered + "/" + this.countEntitiesTotal + ". B: " + this.countEntitiesHidden + ", I: " + (this.countEntitiesTotal - this.countEntitiesHidden - this.countEntitiesRendered);
        }

        private void MarkRenderersForNewPosition(int i1, int i2, int i3)
        {
            i1 -= 8;
            i2 -= 8;
            i3 -= 8;
            this.minBlockX = int.MaxValue;
            this.minBlockY = int.MaxValue;
            this.minBlockZ = int.MaxValue;
            this.maxBlockX = int.MinValue;
            this.maxBlockY = int.MinValue;
            this.maxBlockZ = int.MinValue;
            int i4 = this.renderChunksWide * 16;
            int i5 = i4 / 2;
            for (int i6 = 0; i6 < this.renderChunksWide; ++i6)
            {
                int i7 = i6 * 16;
                int i8 = i7 + i5 - i1;
                if (i8 < 0)
                {
                    i8 -= i4 - 1;
                }

                i8 /= i4;
                i7 -= i8 * i4;
                if (i7 < this.minBlockX)
                {
                    this.minBlockX = i7;
                }

                if (i7 > this.maxBlockX)
                {
                    this.maxBlockX = i7;
                }

                for (int i9 = 0; i9 < this.renderChunksDeep; ++i9)
                {
                    int i10 = i9 * 16;
                    int i11 = i10 + i5 - i3;
                    if (i11 < 0)
                    {
                        i11 -= i4 - 1;
                    }

                    i11 /= i4;
                    i10 -= i11 * i4;
                    if (i10 < this.minBlockZ)
                    {
                        this.minBlockZ = i10;
                    }

                    if (i10 > this.maxBlockZ)
                    {
                        this.maxBlockZ = i10;
                    }

                    for (int i12 = 0; i12 < this.renderChunksTall; ++i12)
                    {
                        int i13 = i12 * 16;
                        if (i13 < this.minBlockY)
                        {
                            this.minBlockY = i13;
                        }

                        if (i13 > this.maxBlockY)
                        {
                            this.maxBlockY = i13;
                        }

                        Chunk worldRenderer14 = this.worldRenderers[(i9 * this.renderChunksTall + i12) * this.renderChunksWide + i6];
                        bool z15 = worldRenderer14.needsUpdate;
                        worldRenderer14.SetPosition(i7, i13, i10);
                        if (!z15 && worldRenderer14.needsUpdate)
                        {
                            this.worldRenderersToUpdate.Add(worldRenderer14);
                        }
                    }
                }
            }
        }

        public virtual int SortAndRender(Mob entityLiving1, int i2, double d3)
        {
            Profiler.StartSection("sortchunks");

            for (int i5 = 0; i5 < 10; ++i5)
            {
                this.worldRenderersCheckIndex = (this.worldRenderersCheckIndex + 1) % this.worldRenderers.Length;
                Chunk worldRenderer6 = this.worldRenderers[this.worldRenderersCheckIndex];
                if (worldRenderer6.needsUpdate && !this.worldRenderersToUpdate.Contains(worldRenderer6))
                {
                    this.worldRenderersToUpdate.Add(worldRenderer6);
                }
            }

            if (this.mc.options.renderDistance != this.renderDistance)
            {
                this.LoadRenderers();
            }

            if (i2 == 0)
            {
                this.renderersLoaded = 0;
                this.renderersBeingClipped = 0;
                this.renderersBeingOccluded = 0;
                this.renderersBeingRendered = 0;
                this.renderersSkippingRenderPass = 0;
            }

            double d33 = entityLiving1.lastTickPosX + (entityLiving1.x - entityLiving1.lastTickPosX) * d3;
            double d7 = entityLiving1.lastTickPosY + (entityLiving1.y - entityLiving1.lastTickPosY) * d3;
            double d9 = entityLiving1.lastTickPosZ + (entityLiving1.z - entityLiving1.lastTickPosZ) * d3;
            double d11 = entityLiving1.x - this.prevSortX;
            double d13 = entityLiving1.y - this.prevSortY;
            double d15 = entityLiving1.z - this.prevSortZ;
            if (d11 * d11 + d13 * d13 + d15 * d15 > 16)
            {
                this.prevSortX = entityLiving1.x;
                this.prevSortY = entityLiving1.y;
                this.prevSortZ = entityLiving1.z;
                this.MarkRenderersForNewPosition(Mth.Floor(entityLiving1.x), Mth.Floor(entityLiving1.y), Mth.Floor(entityLiving1.z));
                Array.Sort(this.sortedWorldRenderers, new EntitySorter(entityLiving1));
            }

            Light.TurnOff();
            byte b17 = 0;
            int i34;
            if (this.occlusionEnabled && this.mc.options.advancedOpengl && !this.mc.options.anaglyph && i2 == 0)
            {
                byte b18 = 0;
                int i19 = 16;
                this.CheckOcclusionQueryResult(b18, i19);
                for (int i20 = b18; i20 < i19; ++i20)
                {
                    this.sortedWorldRenderers[i20].isVisible = true;
                }

                Profiler.EndStartSection("render");
                i34 = b17 + this.RenderSortedRenderers(b18, i19, i2, d3);
                do
                {
                    Profiler.EndStartSection("occ");
                    int i35 = i19;
                    i19 *= 2;
                    if (i19 > this.sortedWorldRenderers.Length)
                    {
                        i19 = this.sortedWorldRenderers.Length;
                    }

                    GL11.glDisable(GL11C.GL_TEXTURE_2D);
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    GL11.glDisable(GL11C.GL_ALPHA_TEST);
                    GL11.glDisable(GL11C.GL_FOG);
                    GL11.glColorMask(false, false, false, false);
                    GL11.glDepthMask(false);
                    Profiler.StartSection("check");
                    this.CheckOcclusionQueryResult(i35, i19);
                    Profiler.EndSection();
                    GL11.glPushMatrix();
                    float f36 = 0F;
                    float f21 = 0F;
                    float f22 = 0F;
                    for (int i23 = i35; i23 < i19; ++i23)
                    {
                        if (this.sortedWorldRenderers[i23].SkipAllRenderPasses())
                        {
                            this.sortedWorldRenderers[i23].isInFrustum = false;
                        }
                        else
                        {
                            if (!this.sortedWorldRenderers[i23].isInFrustum)
                            {
                                this.sortedWorldRenderers[i23].isVisible = true;
                            }

                            if (this.sortedWorldRenderers[i23].isInFrustum && !this.sortedWorldRenderers[i23].isWaitingOnOcclusionQuery)
                            {
                                float f24 = Mth.Sqrt(this.sortedWorldRenderers[i23].DistanceToEntitySquared(entityLiving1));
                                int i25 = (int)(1F + f24 / 128F);
                                if (this.cloudOffsetX % i25 == i23 % i25)
                                {
                                    Chunk worldRenderer26 = this.sortedWorldRenderers[i23];
                                    float f27 = (float)(worldRenderer26.posXMinus - d33);
                                    float f28 = (float)(worldRenderer26.posYMinus - d7);
                                    float f29 = (float)(worldRenderer26.posZMinus - d9);
                                    float f30 = f27 - f36;
                                    float f31 = f28 - f21;
                                    float f32 = f29 - f22;
                                    if (f30 != 0F || f31 != 0F || f32 != 0F)
                                    {
                                        GL11.glTranslatef(f30, f31, f32);
                                        f36 += f30;
                                        f21 += f31;
                                        f22 += f32;
                                    }

                                    Profiler.StartSection("bb");
                                    GLARB.glBeginQueryARB(GL15C.GL_SAMPLES_PASSED, this.sortedWorldRenderers[i23].glOcclusionQuery);
                                    this.sortedWorldRenderers[i23].CallOcclusionQueryList();
                                    GLARB.glEndQueryARB(GL15C.GL_SAMPLES_PASSED);
                                    Profiler.EndSection();
                                    this.sortedWorldRenderers[i23].isWaitingOnOcclusionQuery = true;
                                }
                            }
                        }
                    }

                    GL11.glPopMatrix();
                    if (this.mc.options.anaglyph)
                    {
                        if (GameRenderer.anaglyphField == 0)
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

                    GL11.glDepthMask(true);
                    GL11.glEnable(GL11C.GL_TEXTURE_2D);
                    GL11.glEnable(GL11C.GL_ALPHA_TEST);
                    GL11.glEnable(GL11C.GL_FOG);
                    Profiler.EndStartSection("render");
                    i34 += this.RenderSortedRenderers(i35, i19, i2, d3);
                }
                while (i19 < this.sortedWorldRenderers.Length);
            }
            else
            {
                Profiler.EndStartSection("render");
                i34 = b17 + this.RenderSortedRenderers(0, this.sortedWorldRenderers.Length, i2, d3);
            }

            Profiler.EndSection();
            return i34;
        }

        private unsafe void CheckOcclusionQueryResult(int i1, int i2)
        {
            for (int i3 = i1; i3 < i2; ++i3)
            {
                if (this.sortedWorldRenderers[i3].isWaitingOnOcclusionQuery)
                {
                    //this.occlusionResult.Clear();
                    fixed (uint* ptr = &occlusionResult[0])
                    {
                        GLARB_PTR.glGetQueryObjectuivARB(this.sortedWorldRenderers[i3].glOcclusionQuery, GL15C.GL_QUERY_RESULT_AVAILABLE, ptr);
                    }
                    if (this.occlusionResult[0] != 0)
                    {
                        this.sortedWorldRenderers[i3].isWaitingOnOcclusionQuery = false;
                        //this.occlusionResult.Clear();
                        fixed (uint* ptr = &occlusionResult[0])
                        {
                            GLARB_PTR.glGetQueryObjectuivARB(this.sortedWorldRenderers[i3].glOcclusionQuery, GL15C.GL_QUERY_RESULT, ptr);
                        }
                        this.sortedWorldRenderers[i3].isVisible = this.occlusionResult[0] != 0;
                    }
                }
            }
        }

        private int RenderSortedRenderers(int i1, int i2, int i3, double d4)
        {
            this.glRenderLists.Clear();
            int i6 = 0;
            for (int i7 = i1; i7 < i2; ++i7)
            {
                if (i3 == 0)
                {
                    ++this.renderersLoaded;
                    if (this.sortedWorldRenderers[i7].skipRenderPass[i3])
                    {
                        ++this.renderersSkippingRenderPass;
                    }
                    else if (!this.sortedWorldRenderers[i7].isInFrustum)
                    {
                        ++this.renderersBeingClipped;
                    }
                    else if (this.occlusionEnabled && !this.sortedWorldRenderers[i7].isVisible)
                    {
                        ++this.renderersBeingOccluded;
                    }
                    else
                    {
                        ++this.renderersBeingRendered;
                    }
                }

                if (!this.sortedWorldRenderers[i7].skipRenderPass[i3] && this.sortedWorldRenderers[i7].isInFrustum && (!this.occlusionEnabled || this.sortedWorldRenderers[i7].isVisible))
                {
                    uint i8 = this.sortedWorldRenderers[i7].GetGLCallListForPass((uint)i3);
                    if (i8 >= 0)
                    {
                        this.glRenderLists.Add(this.sortedWorldRenderers[i7]);
                        ++i6;
                    }
                }
            }

            Mob entityLiving19 = this.mc.renderViewEntity;
            double d20 = entityLiving19.lastTickPosX + (entityLiving19.x - entityLiving19.lastTickPosX) * d4;
            double d10 = entityLiving19.lastTickPosY + (entityLiving19.y - entityLiving19.lastTickPosY) * d4;
            double d12 = entityLiving19.lastTickPosZ + (entityLiving19.z - entityLiving19.lastTickPosZ) * d4;
            int i14 = 0;
            int i15;
            for (i15 = 0; i15 < this.allRenderLists.Length; ++i15)
            {
                this.allRenderLists[i15].Func_859_b();
            }

            for (i15 = 0; i15 < this.glRenderLists.Count; ++i15)
            {
                Chunk worldRenderer16 = this.glRenderLists[i15];
                int i17 = -1;
                for (int i18 = 0; i18 < i14; ++i18)
                {
                    if (this.allRenderLists[i18].IsAt(worldRenderer16.posXMinus, worldRenderer16.posYMinus, worldRenderer16.posZMinus))
                    {
                        i17 = i18;
                    }
                }

                if (i17 < 0)
                {
                    i17 = i14++;
                    this.allRenderLists[i17].Init(worldRenderer16.posXMinus, worldRenderer16.posYMinus, worldRenderer16.posZMinus, d20, d10, d12);
                }

                this.allRenderLists[i17].Add(worldRenderer16.GetGLCallListForPass((uint)i3));
            }

            this.RenderAllRenderLists(i3, d4);
            return i6;
        }

        public virtual void RenderAllRenderLists(int i1, double d2)
        {
            for (int i4 = 0; i4 < this.allRenderLists.Length; ++i4)
            {
                this.allRenderLists[i4].Render();
            }
        }

        public virtual void UpdateClouds()
        {
            ++this.cloudOffsetX;
        }

        public virtual void RenderSky(float f1)
        {
            if (!this.mc.level.dimension.isNether)
            {
                GL11.glDisable(GL11C.GL_TEXTURE_2D);
                Vec3 vec3D2 = this.worldObj.GetSkyColor(this.mc.renderViewEntity, f1);
                float f3 = (float)vec3D2.x;
                float f4 = (float)vec3D2.y;
                float f5 = (float)vec3D2.z;
                float f7;
                float f8;
                if (this.mc.options.anaglyph)
                {
                    float f6 = (f3 * 30F + f4 * 59F + f5 * 11F) / 100F;
                    f7 = (f3 * 30F + f4 * 70F) / 100F;
                    f8 = (f3 * 30F + f5 * 70F) / 100F;
                    f3 = f6;
                    f4 = f7;
                    f5 = f8;
                }

                GL11.glColor3f(f3, f4, f5);
                Tessellator tessellator17 = Tessellator.Instance;
                GL11.glDepthMask(false);
                GL11.glEnable(GL11C.GL_FOG);
                GL11.glColor3f(f3, f4, f5);
                GL11.glCallList(this.glSkyList);
                GL11.glDisable(GL11C.GL_FOG);
                GL11.glDisable(GL11C.GL_ALPHA_TEST);
                GL11.glEnable(GL11C.GL_BLEND);
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                Light.TurnOff();
                float[] f18 = this.worldObj.dimension.CalcSunriseSunsetColors(this.worldObj.GetSunAngle(f1), f1);
                float f9;
                float f10;
                float f11;
                float f12;
                if (f18 != null)
                {
                    GL11.glDisable(GL11C.GL_TEXTURE_2D);
                    GL11.glShadeModel(GL11C.GL_SMOOTH);
                    GL11.glPushMatrix();
                    GL11.glRotatef(90F, 1F, 0F, 0F);
                    f8 = this.worldObj.GetSunAngle(f1);
                    GL11.glRotatef(f8 > 0.5F ? 180F : 0F, 0F, 0F, 1F);
                    f9 = f18[0];
                    f10 = f18[1];
                    f11 = f18[2];
                    float f14;
                    if (this.mc.options.anaglyph)
                    {
                        f12 = (f9 * 30F + f10 * 59F + f11 * 11F) / 100F;
                        float f13 = (f9 * 30F + f10 * 70F) / 100F;
                        f14 = (f9 * 30F + f11 * 70F) / 100F;
                        f9 = f12;
                        f10 = f13;
                        f11 = f14;
                    }

                    tessellator17.Begin(GL11C.GL_TRIANGLE_FAN);
                    tessellator17.Color(f9, f10, f11, f18[3]);
                    tessellator17.Vertex(0, 100, 0);
                    byte b19 = 16;
                    tessellator17.Color(f18[0], f18[1], f18[2], 0F);
                    for (int i20 = 0; i20 <= b19; ++i20)
                    {
                        f14 = i20 * Mth.PI * 2F / b19;
                        float f15 = Mth.Sin(f14);
                        float f16 = Mth.Cos(f14);
                        tessellator17.Vertex(f15 * 120F, f16 * 120F, -f16 * 40F * f18[3]);
                    }

                    tessellator17.End();
                    GL11.glPopMatrix();
                    GL11.glShadeModel(GL11C.GL_FLAT);
                }

                GL11.glEnable(GL11C.GL_TEXTURE_2D);
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE);
                GL11.glPushMatrix();
                f7 = 1F - this.worldObj.Func_g(f1);
                f8 = 0F;
                f9 = 0F;
                f10 = 0F;
                GL11.glColor4f(1F, 1F, 1F, f7);
                GL11.glTranslatef(f8, f9, f10);
                GL11.glRotatef(0F, 0F, 0F, 1F);
                GL11.glRotatef(this.worldObj.GetSunAngle(f1) * 360F, 1F, 0F, 0F);
                f11 = 30F;
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.renderEngine.LoadTexture("/terrain/sun.png"));
                tessellator17.Begin();
                tessellator17.VertexUV((-f11), 100, (-f11), 0, 0);
                tessellator17.VertexUV(f11, 100, (-f11), 1, 0);
                tessellator17.VertexUV(f11, 100, f11, 1, 1);
                tessellator17.VertexUV((-f11), 100, f11, 0, 1);
                tessellator17.End();
                f11 = 20F;
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.renderEngine.LoadTexture("/terrain/moon.png"));
                tessellator17.Begin();
                tessellator17.VertexUV((-f11), -100, f11, 1, 1);
                tessellator17.VertexUV(f11, -100, f11, 0, 1);
                tessellator17.VertexUV(f11, -100, (-f11), 0, 0);
                tessellator17.VertexUV((-f11), -100, (-f11), 1, 0);
                tessellator17.End();
                GL11.glDisable(GL11C.GL_TEXTURE_2D);
                f12 = this.worldObj.GetStarBrightness(f1) * f7;
                if (f12 > 0F)
                {
                    GL11.glColor4f(f12, f12, f12, f12);
                    GL11.glCallList(this.starGLCallList);
                }

                GL11.glColor4f(1F, 1F, 1F, 1F);
                GL11.glDisable(GL11C.GL_BLEND);
                GL11.glEnable(GL11C.GL_ALPHA_TEST);
                GL11.glEnable(GL11C.GL_FOG);
                GL11.glPopMatrix();
                if (this.worldObj.dimension.Func_28112_c())
                {
                    GL11.glColor3f(f3 * 0.2F + 0.04F, f4 * 0.2F + 0.04F, f5 * 0.6F + 0.1F);
                }
                else
                {
                    GL11.glColor3f(f3, f4, f5);
                }

                GL11.glDisable(GL11C.GL_TEXTURE_2D);
                GL11.glCallList(this.glSkyList2);
                GL11.glEnable(GL11C.GL_TEXTURE_2D);
                GL11.glDepthMask(true);
            }
        }

        public virtual void RenderClouds(float f1)
        {
            if (!this.mc.level.dimension.isNether)
            {
                if (this.mc.options.fancyGraphics)
                {
                    this.RenderCloudsFancy(f1);
                }
                else
                {
                    GL11.glDisable(GL11C.GL_CULL_FACE);
                    float f2 = (float)(this.mc.renderViewEntity.lastTickPosY + (this.mc.renderViewEntity.y - this.mc.renderViewEntity.lastTickPosY) * f1);
                    byte b3 = 32;
                    int i4 = 256 / b3;
                    Tessellator tessellator5 = Tessellator.Instance;
                    GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.renderEngine.LoadTexture("/environment/clouds.png"));
                    GL11.glEnable(GL11C.GL_BLEND);
                    GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                    Vec3 vec3D6 = this.worldObj.GetCloudColor(f1);
                    float f7 = (float)vec3D6.x;
                    float f8 = (float)vec3D6.y;
                    float f9 = (float)vec3D6.z;
                    float f10;
                    if (this.mc.options.anaglyph)
                    {
                        f10 = (f7 * 30F + f8 * 59F + f9 * 11F) / 100F;
                        float f11 = (f7 * 30F + f8 * 70F) / 100F;
                        float f12 = (f7 * 30F + f9 * 70F) / 100F;
                        f7 = f10;
                        f8 = f11;
                        f9 = f12;
                    }

                    f10 = 0.00048828125F;
                    double d22 = this.mc.renderViewEntity.prevX + (this.mc.renderViewEntity.x - this.mc.renderViewEntity.prevX) * f1 + (this.cloudOffsetX + f1) * 0.03F;
                    double d13 = this.mc.renderViewEntity.prevZ + (this.mc.renderViewEntity.z - this.mc.renderViewEntity.prevZ) * f1;
                    int i15 = Mth.Floor(d22 / 2048);
                    int i16 = Mth.Floor(d13 / 2048);
                    d22 -= i15 * 2048;
                    d13 -= i16 * 2048;
                    float f17 = this.worldObj.dimension.GetCloudHeight() - f2 + 0.33F;
                    float f18 = (float)(d22 * f10);
                    float f19 = (float)(d13 * f10);
                    tessellator5.Begin();
                    tessellator5.Color(f7, f8, f9, 0.8F);
                    for (int i20 = -b3 * i4; i20 < b3 * i4; i20 += b3)
                    {
                        for (int i21 = -b3 * i4; i21 < b3 * i4; i21 += b3)
                        {
                            tessellator5.VertexUV(i20 + 0, f17, i21 + b3, (i20 + 0) * f10 + f18, (i21 + b3) * f10 + f19);
                            tessellator5.VertexUV(i20 + b3, f17, i21 + b3, (i20 + b3) * f10 + f18, (i21 + b3) * f10 + f19);
                            tessellator5.VertexUV(i20 + b3, f17, i21 + 0, (i20 + b3) * f10 + f18, (i21 + 0) * f10 + f19);
                            tessellator5.VertexUV(i20 + 0, f17, i21 + 0, (i20 + 0) * f10 + f18, (i21 + 0) * f10 + f19);
                        }
                    }

                    tessellator5.End();
                    GL11.glColor4f(1F, 1F, 1F, 1F);
                    GL11.glDisable(GL11C.GL_BLEND);
                    GL11.glEnable(GL11C.GL_CULL_FACE);
                }
            }
        }

        public virtual bool Func_27307_a(double d1, double d3, double d5, float f7)
        {
            return false;
        }

        public virtual void RenderCloudsFancy(float tickAlpha)
        {
            GL11.glDisable(GL11C.GL_CULL_FACE);
            float f2 = (float)(this.mc.renderViewEntity.lastTickPosY + (this.mc.renderViewEntity.y - this.mc.renderViewEntity.lastTickPosY) * tickAlpha);
            Tessellator t = Tessellator.Instance;
            float f4 = 12F;
            float f5 = 4F;
            double d6 = (this.mc.renderViewEntity.prevX + (this.mc.renderViewEntity.x - this.mc.renderViewEntity.prevX) * tickAlpha + (this.cloudOffsetX + tickAlpha) * 0.03F) / f4;
            double d8 = (this.mc.renderViewEntity.prevZ + (this.mc.renderViewEntity.z - this.mc.renderViewEntity.prevZ) * tickAlpha) / f4 + 0.33F;
            float f10 = this.worldObj.dimension.GetCloudHeight() - f2 + 0.33F;
            int i11 = Mth.Floor(d6 / 2048);
            int i12 = Mth.Floor(d8 / 2048);
            d6 -= i11 * 2048;
            d8 -= i12 * 2048;
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.renderEngine.LoadTexture("/environment/clouds.png"));
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            Vec3 cloudColor = this.worldObj.GetCloudColor(tickAlpha);
            float r = (float)cloudColor.x;
            float g = (float)cloudColor.y;
            float b = (float)cloudColor.z;
            float f17;
            float f18;
            float f19;
            if (this.mc.options.anaglyph)
            {
                f17 = (r * 30F + g * 59F + b * 11F) / 100F;
                f18 = (r * 30F + g * 70F) / 100F;
                f19 = (r * 30F + b * 70F) / 100F;
                r = f17;
                g = f18;
                b = f19;
            }

            f17 = (float)(d6 * 0);
            f18 = (float)(d8 * 0);
            f19 = 0.00390625F;
            f17 = Mth.Floor(d6) * f19;
            f18 = Mth.Floor(d8) * f19;
            float f20 = (float)(d6 - Mth.Floor(d6));
            float f21 = (float)(d8 - Mth.Floor(d8));
            byte b22 = 8;
            byte b23 = 3;
            float f24 = 0.0009765625F;
            GL11.glScalef(f4, 1F, f4);
            for (int i = 0; i < 2; ++i)
            {
                if (i == 0)
                {
                    GL11.glColorMask(false, false, false, false);
                }
                else if (this.mc.options.anaglyph)
                {
                    if (GameRenderer.anaglyphField == 0)
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

                for (int i26 = -b23 + 1; i26 <= b23; ++i26)
                {
                    for (int i27 = -b23 + 1; i27 <= b23; ++i27)
                    {
                        t.Begin();
                        float f28 = i26 * b22;
                        float f29 = i27 * b22;
                        float f30 = f28 - f20;
                        float f31 = f29 - f21;
                        if (f10 > -f5 - 1F)
                        {
                            t.Color(r * 0.7F, g * 0.7F, b * 0.7F, 0.8F);
                            t.Normal(0F, -1F, 0F);
                            t.VertexUV(f30 + 0F, f10 + 0F, f31 + b22, (f28 + 0F) * f19 + f17, (f29 + b22) * f19 + f18);
                            t.VertexUV(f30 + b22, f10 + 0F, f31 + b22, (f28 + b22) * f19 + f17, (f29 + b22) * f19 + f18);
                            t.VertexUV(f30 + b22, f10 + 0F, f31 + 0F, (f28 + b22) * f19 + f17, (f29 + 0F) * f19 + f18);
                            t.VertexUV(f30 + 0F, f10 + 0F, f31 + 0F, (f28 + 0F) * f19 + f17, (f29 + 0F) * f19 + f18);
                        }

                        if (f10 <= f5 + 1F)
                        {
                            t.Color(r, g, b, 0.8F);
                            t.Normal(0F, 1F, 0F);
                            t.VertexUV(f30 + 0F, f10 + f5 - f24, f31 + b22, (f28 + 0F) * f19 + f17, (f29 + b22) * f19 + f18);
                            t.VertexUV(f30 + b22, f10 + f5 - f24, f31 + b22, (f28 + b22) * f19 + f17, (f29 + b22) * f19 + f18);
                            t.VertexUV(f30 + b22, f10 + f5 - f24, f31 + 0F, (f28 + b22) * f19 + f17, (f29 + 0F) * f19 + f18);
                            t.VertexUV(f30 + 0F, f10 + f5 - f24, f31 + 0F, (f28 + 0F) * f19 + f17, (f29 + 0F) * f19 + f18);
                        }

                        t.Color(r * 0.9F, g * 0.9F, b * 0.9F, 0.8F);
                        int i32;
                        if (i26 > -1)
                        {
                            t.Normal(-1F, 0F, 0F);
                            for (i32 = 0; i32 < b22; ++i32)
                            {
                                t.VertexUV(f30 + i32 + 0F, f10 + 0F, f31 + b22, (f28 + i32 + 0.5F) * f19 + f17, (f29 + b22) * f19 + f18);
                                t.VertexUV(f30 + i32 + 0F, f10 + f5, f31 + b22, (f28 + i32 + 0.5F) * f19 + f17, (f29 + b22) * f19 + f18);
                                t.VertexUV(f30 + i32 + 0F, f10 + f5, f31 + 0F, (f28 + i32 + 0.5F) * f19 + f17, (f29 + 0F) * f19 + f18);
                                t.VertexUV(f30 + i32 + 0F, f10 + 0F, f31 + 0F, (f28 + i32 + 0.5F) * f19 + f17, (f29 + 0F) * f19 + f18);
                            }
                        }

                        if (i26 <= 1)
                        {
                            t.Normal(1F, 0F, 0F);
                            for (i32 = 0; i32 < b22; ++i32)
                            {
                                t.VertexUV(f30 + i32 + 1F - f24, f10 + 0F, f31 + b22, (f28 + i32 + 0.5F) * f19 + f17, (f29 + b22) * f19 + f18);
                                t.VertexUV(f30 + i32 + 1F - f24, f10 + f5, f31 + b22, (f28 + i32 + 0.5F) * f19 + f17, (f29 + b22) * f19 + f18);
                                t.VertexUV(f30 + i32 + 1F - f24, f10 + f5, f31 + 0F, (f28 + i32 + 0.5F) * f19 + f17, (f29 + 0F) * f19 + f18);
                                t.VertexUV(f30 + i32 + 1F - f24, f10 + 0F, f31 + 0F, (f28 + i32 + 0.5F) * f19 + f17, (f29 + 0F) * f19 + f18);
                            }
                        }

                        t.Color(r * 0.8F, g * 0.8F, b * 0.8F, 0.8F);
                        if (i27 > -1)
                        {
                            t.Normal(0F, 0F, -1F);
                            for (i32 = 0; i32 < b22; ++i32)
                            {
                                t.VertexUV(f30 + 0F, f10 + f5, f31 + i32 + 0F, (f28 + 0F) * f19 + f17, (f29 + i32 + 0.5F) * f19 + f18);
                                t.VertexUV(f30 + b22, f10 + f5, f31 + i32 + 0F, (f28 + b22) * f19 + f17, (f29 + i32 + 0.5F) * f19 + f18);
                                t.VertexUV(f30 + b22, f10 + 0F, f31 + i32 + 0F, (f28 + b22) * f19 + f17, (f29 + i32 + 0.5F) * f19 + f18);
                                t.VertexUV(f30 + 0F, f10 + 0F, f31 + i32 + 0F, (f28 + 0F) * f19 + f17, (f29 + i32 + 0.5F) * f19 + f18);
                            }
                        }

                        if (i27 <= 1)
                        {
                            t.Normal(0F, 0F, 1F);
                            for (i32 = 0; i32 < b22; ++i32)
                            {
                                t.VertexUV(f30 + 0F, f10 + f5, f31 + i32 + 1F - f24, (f28 + 0F) * f19 + f17, (f29 + i32 + 0.5F) * f19 + f18);
                                t.VertexUV(f30 + b22, f10 + f5, f31 + i32 + 1F - f24, (f28 + b22) * f19 + f17, (f29 + i32 + 0.5F) * f19 + f18);
                                t.VertexUV(f30 + b22, f10 + 0F, f31 + i32 + 1F - f24, (f28 + b22) * f19 + f17, (f29 + i32 + 0.5F) * f19 + f18);
                                t.VertexUV(f30 + 0F, f10 + 0F, f31 + i32 + 1F - f24, (f28 + 0F) * f19 + f17, (f29 + i32 + 0.5F) * f19 + f18);
                            }
                        }

                        t.End();
                    }
                }
            }

            GL11.glColor4f(1F, 1F, 1F, 1F);
            GL11.glDisable(GL11C.GL_BLEND);
            GL11.glEnable(GL11C.GL_CULL_FACE);
        }

        public virtual bool UpdateRenderers(Mob entityLiving1, bool z2)
        {
            bool useLegacyMethod = false;
            if (useLegacyMethod)
            {
                worldRenderersToUpdate.Sort(new DistanceSorter(entityLiving1));
                int i17 = this.worldRenderersToUpdate.Count - 1;
                int i18 = this.worldRenderersToUpdate.Count;
                for (int i19 = 0; i19 < i18; ++i19)
                {
                    Chunk worldRenderer20 = this.worldRenderersToUpdate[i17 - i19];
                    if (!z2)
                    {
                        if (worldRenderer20.DistanceToEntitySquared(entityLiving1) > 256F)
                        {
                            if (worldRenderer20.isInFrustum)
                            {
                                if (i19 >= 3)
                                {
                                    return false;
                                }
                            }
                            else if (i19 >= 1)
                            {
                                return false;
                            }
                        }
                    }
                    else if (!worldRenderer20.isInFrustum)
                    {
                        continue;
                    }

                    worldRenderer20.UpdateRenderer();
                    this.worldRenderersToUpdate.Remove(worldRenderer20);
                    worldRenderer20.needsUpdate = false;
                }

                return this.worldRenderersToUpdate.Count == 0;
            }
            else
            {
                byte b4 = 2;
                DistanceSorter renderSorter5 = new DistanceSorter(entityLiving1);
                Chunk[] worldRenderer6 = new Chunk[b4];
                List<Chunk> arrayList7 = null;
                int i8 = this.worldRenderersToUpdate.Count;
                int i9 = 0;
                int i10;
                Chunk worldRenderer11;
                int i12;
                int i13;
                bool continue169 = false;
            //label169:
                for (i10 = 0; i10 < i8; ++i10)
                {
                    worldRenderer11 = this.worldRenderersToUpdate[i10];
                    if (!z2)
                    {
                        if (worldRenderer11.DistanceToEntitySquared(entityLiving1) > 256F)
                        {
                            for (i12 = 0; i12 < b4 && (worldRenderer6[i12] == null || renderSorter5.Compare(worldRenderer6[i12], worldRenderer11) <= 0); ++i12)
                            {
                            }

                            --i12;
                            if (i12 <= 0)
                            {
                                continue;
                            }

                            i13 = i12;
                            while (true)
                            {
                                --i13;
                                if (i13 == 0)
                                {
                                    worldRenderer6[i12] = worldRenderer11;
                                    //continue label169;
                                    continue169 = true;
                                    break;
                                }

                                worldRenderer6[i13 - 1] = worldRenderer6[i13];
                            }
                            if (continue169) 
                            {
                                continue169 = false;
                                continue;
                            }
                        }
                    }
                    else if (!worldRenderer11.isInFrustum)
                    {
                        continue;
                    }

                    if (arrayList7 == null)
                    {
                        arrayList7 = new List<Chunk>();
                    }

                    ++i9;
                    arrayList7.Add(worldRenderer11);
                    this.worldRenderersToUpdate[i10] = null;
                }

                if (arrayList7 != null)
                {
                    if (arrayList7.Count > 1)
                    {
                        arrayList7.Sort(renderSorter5);
                    }

                    for (i10 = arrayList7.Count - 1; i10 >= 0; --i10)
                    {
                        worldRenderer11 = arrayList7[i10];
                        worldRenderer11.UpdateRenderer();
                        worldRenderer11.needsUpdate = false;
                    }
                }

                i10 = 0;
                int i21;
                for (i21 = b4 - 1; i21 >= 0; --i21)
                {
                    Chunk worldRenderer22 = worldRenderer6[i21];
                    if (worldRenderer22 != null)
                    {
                        if (!worldRenderer22.isInFrustum && i21 != b4 - 1)
                        {
                            worldRenderer6[i21] = null;
                            worldRenderer6[0] = null;
                            break;
                        }

                        worldRenderer6[i21].UpdateRenderer();
                        worldRenderer6[i21].needsUpdate = false;
                        ++i10;
                    }
                }

                i21 = 0;
                i12 = 0;
                for (i13 = this.worldRenderersToUpdate.Count; i21 != i13; ++i21)
                {
                    Chunk worldRenderer14 = this.worldRenderersToUpdate[i21];
                    if (worldRenderer14 != null)
                    {
                        bool z15 = false;
                        for (int i16 = 0; i16 < b4 && !z15; ++i16)
                        {
                            if (worldRenderer14 == worldRenderer6[i16])
                            {
                                z15 = true;
                            }
                        }

                        if (!z15)
                        {
                            if (i12 != i21)
                            {
                                this.worldRenderersToUpdate[i12] = worldRenderer14;
                            }

                            ++i12;
                        }
                    }
                }

                while (true)
                {
                    --i21;
                    if (i21 < i12)
                    {
                        return i8 == i9 + i10;
                    }

                    this.worldRenderersToUpdate.RemoveAt(i21);
                }
            }
        }

        public virtual void DrawBlockBreaking(Player entityPlayer1, HitResult movingObjectPosition2, int i3, ItemInstance itemStack4, float f5)
        {
            Tessellator tessellator6 = Tessellator.Instance;
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glEnable(GL11C.GL_ALPHA_TEST);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE);
            GL11.glColor4f(1F, 1F, 1F, (Mth.Sin(TimeUtil.MilliTime / 100F) * 0.2F + 0.4F) * 0.5F);
            int i8;
            if (i3 == 0)
            {
                if (this.damagePartialTime > 0F)
                {
                    GL11.glBlendFunc(GL11C.GL_DST_COLOR, GL11C.GL_SRC_COLOR);
                    uint i7 = this.renderEngine.LoadTexture("/terrain.png");
                    GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i7);
                    GL11.glColor4f(1F, 1F, 1F, 0.5F);
                    GL11.glPushMatrix();
                    i8 = this.worldObj.GetTile(movingObjectPosition2.BlockX, movingObjectPosition2.BlockY, movingObjectPosition2.BlockZ);
                    Tile block9 = i8 > 0 ? Tile.tiles[i8] : null;
                    GL11.glDisable(GL11C.GL_ALPHA_TEST);
                    GL11.glPolygonOffset(-3F, -3F);
                    GL11.glEnable(GL11C.GL_POLYGON_OFFSET_FILL);
                    double d10 = entityPlayer1.lastTickPosX + (entityPlayer1.x - entityPlayer1.lastTickPosX) * f5;
                    double d12 = entityPlayer1.lastTickPosY + (entityPlayer1.y - entityPlayer1.lastTickPosY) * f5;
                    double d14 = entityPlayer1.lastTickPosZ + (entityPlayer1.z - entityPlayer1.lastTickPosZ) * f5;
                    if (block9 == null)
                    {
                        block9 = Tile.rock;
                    }

                    GL11.glEnable(GL11C.GL_ALPHA_TEST);
                    tessellator6.Begin();
                    tessellator6.Offset(-d10, -d12, -d14);
                    tessellator6.NoColor();
                    this.globalRenderBlocks.RenderBlockUsingTexture(block9, movingObjectPosition2.BlockX, movingObjectPosition2.BlockY, movingObjectPosition2.BlockZ, 240 + (int)(this.damagePartialTime * 10F));
                    tessellator6.End();
                    tessellator6.Offset(0, 0, 0);
                    GL11.glDisable(GL11C.GL_ALPHA_TEST);
                    GL11.glPolygonOffset(0F, 0F);
                    GL11.glDisable(GL11C.GL_POLYGON_OFFSET_FILL);
                    GL11.glEnable(GL11C.GL_ALPHA_TEST);
                    GL11.glDepthMask(true);
                    GL11.glPopMatrix();
                }
            }
            else if (itemStack4 != null)
            {
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                float f16 = Mth.Sin(TimeUtil.MilliTime / 100F) * 0.2F + 0.8F;
                GL11.glColor4f(f16, f16, f16, Mth.Sin(TimeUtil.MilliTime / 200F) * 0.2F + 0.5F);
                uint i8_1 = this.renderEngine.LoadTexture("/terrain.png");
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i8_1);
            }

            GL11.glDisable(GL11C.GL_BLEND);
            GL11.glDisable(GL11C.GL_ALPHA_TEST);
        }

        public virtual void DrawSelectionBox(Player entityPlayer1, HitResult movingObjectPosition2, int i3, ItemInstance itemStack4, float f5)
        {
            if (i3 == 0 && movingObjectPosition2.TypeOfHit == HitResult.Type.TILE)
            {
                GL11.glEnable(GL11C.GL_BLEND);
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                GL11.glColor4f(0F, 0F, 0F, 0.4F);
                GL11.glLineWidth(2F);
                GL11.glDisable(GL11C.GL_TEXTURE_2D);
                GL11.glDepthMask(false);
                float f6 = 0.002F;
                int i7 = this.worldObj.GetTile(movingObjectPosition2.BlockX, movingObjectPosition2.BlockY, movingObjectPosition2.BlockZ);
                if (i7 > 0)
                {
                    Tile.tiles[i7].SetBlockBoundsBasedOnState(this.worldObj, movingObjectPosition2.BlockX, movingObjectPosition2.BlockY, movingObjectPosition2.BlockZ);
                    double d8 = entityPlayer1.lastTickPosX + (entityPlayer1.x - entityPlayer1.lastTickPosX) * f5;
                    double d10 = entityPlayer1.lastTickPosY + (entityPlayer1.y - entityPlayer1.lastTickPosY) * f5;
                    double d12 = entityPlayer1.lastTickPosZ + (entityPlayer1.z - entityPlayer1.lastTickPosZ) * f5;
                    this.DrawOutlinedBoundingBox(Tile.tiles[i7].GetTileAABB(this.worldObj, movingObjectPosition2.BlockX, movingObjectPosition2.BlockY, movingObjectPosition2.BlockZ).Expand(f6, f6, f6).GetOffsetBoundingBox(-d8, -d10, -d12));
                }

                GL11.glDepthMask(true);
                GL11.glEnable(GL11C.GL_TEXTURE_2D);
                GL11.glDisable(GL11C.GL_BLEND);
            }
        }

        private void DrawOutlinedBoundingBox(AABB axisAlignedBB1)
        {
            Tessellator tessellator2 = Tessellator.Instance;
            tessellator2.Begin(3);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y0, axisAlignedBB1.z0);
            tessellator2.Vertex(axisAlignedBB1.x1, axisAlignedBB1.y0, axisAlignedBB1.z0);
            tessellator2.Vertex(axisAlignedBB1.x1, axisAlignedBB1.y0, axisAlignedBB1.z1);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y0, axisAlignedBB1.z1);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y0, axisAlignedBB1.z0);
            tessellator2.End();
            tessellator2.Begin(3);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y1, axisAlignedBB1.z0);
            tessellator2.Vertex(axisAlignedBB1.x1, axisAlignedBB1.y1, axisAlignedBB1.z0);
            tessellator2.Vertex(axisAlignedBB1.x1, axisAlignedBB1.y1, axisAlignedBB1.z1);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y1, axisAlignedBB1.z1);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y1, axisAlignedBB1.z0);
            tessellator2.End();
            tessellator2.Begin(1);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y0, axisAlignedBB1.z0);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y1, axisAlignedBB1.z0);
            tessellator2.Vertex(axisAlignedBB1.x1, axisAlignedBB1.y0, axisAlignedBB1.z0);
            tessellator2.Vertex(axisAlignedBB1.x1, axisAlignedBB1.y1, axisAlignedBB1.z0);
            tessellator2.Vertex(axisAlignedBB1.x1, axisAlignedBB1.y0, axisAlignedBB1.z1);
            tessellator2.Vertex(axisAlignedBB1.x1, axisAlignedBB1.y1, axisAlignedBB1.z1);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y0, axisAlignedBB1.z1);
            tessellator2.Vertex(axisAlignedBB1.x0, axisAlignedBB1.y1, axisAlignedBB1.z1);
            tessellator2.End();
        }

        public virtual void Func_949_a(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            int i7 = Mth.IntFloorDiv(i1, 16);
            int i8 = Mth.IntFloorDiv(i2, 16);
            int i9 = Mth.IntFloorDiv(i3, 16);
            int i10 = Mth.IntFloorDiv(i4, 16);
            int i11 = Mth.IntFloorDiv(i5, 16);
            int i12 = Mth.IntFloorDiv(i6, 16);
            for (int i13 = i7; i13 <= i10; ++i13)
            {
                int i14 = i13 % this.renderChunksWide;
                if (i14 < 0)
                {
                    i14 += this.renderChunksWide;
                }

                for (int i15 = i8; i15 <= i11; ++i15)
                {
                    int i16 = i15 % this.renderChunksTall;
                    if (i16 < 0)
                    {
                        i16 += this.renderChunksTall;
                    }

                    for (int i17 = i9; i17 <= i12; ++i17)
                    {
                        int i18 = i17 % this.renderChunksDeep;
                        if (i18 < 0)
                        {
                            i18 += this.renderChunksDeep;
                        }

                        int i19 = (i18 * this.renderChunksTall + i16) * this.renderChunksWide + i14;
                        Chunk worldRenderer20 = this.worldRenderers[i19];
                        if (!worldRenderer20.needsUpdate)
                        {
                            this.worldRenderersToUpdate.Add(worldRenderer20);
                            worldRenderer20.MarkDirty();
                        }
                    }
                }
            }
        }

        public virtual void TileChanged(int i1, int i2, int i3)
        {
            this.Func_949_a(i1 - 1, i2 - 1, i3 - 1, i1 + 1, i2 + 1, i3 + 1);
        }

        public virtual void SetTilesDirty(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            this.Func_949_a(i1 - 1, i2 - 1, i3 - 1, i4 + 1, i5 + 1, i6 + 1);
        }

        public virtual void ClipRenderersByFrustrum(ICuller iCamera1, float f2)
        {
            for (int i3 = 0; i3 < this.worldRenderers.Length; ++i3)
            {
                if (!this.worldRenderers[i3].SkipAllRenderPasses() && (!this.worldRenderers[i3].isInFrustum || (i3 + this.frustrumCheckOffset & 15) == 0))
                {
                    this.worldRenderers[i3].UpdateInFrustrum(iCamera1);
                }
            }

            ++this.frustrumCheckOffset;
        }

        public virtual void PlayStreamingMusic(string string1, int i2, int i3, int i4)
        {
            if (string1 != null)
            {
                this.mc.ingameGUI.SetRecordPlayingMessage("C418 - " + string1);
            }

            this.mc.soundEngine.PlayStreaming(string1, i2, i3, i4, 1F, 1F);
        }

        public virtual void PlaySound(string string1, double d2, double d4, double d6, float f8, float f9)
        {
            float f10 = 16F;
            if (f8 > 1F)
            {
                f10 *= f8;
            }

            if (this.mc.renderViewEntity.GetDistanceSq(d2, d4, d6) < f10 * f10)
            {
                this.mc.soundEngine.PlaySound(string1, (float)d2, (float)d4, (float)d6, f8, f9);
            }
        }

        public virtual void AddParticle(string string1, double d2, double d4, double d6, double d8, double d10, double d12)
        {
            if (this.mc != null && this.mc.renderViewEntity != null && this.mc.effectRenderer != null)
            {
                double d14 = this.mc.renderViewEntity.x - d2;
                double d16 = this.mc.renderViewEntity.y - d4;
                double d18 = this.mc.renderViewEntity.z - d6;
                double d20 = 16;
                if (d14 * d14 + d16 * d16 + d18 * d18 <= d20 * d20)
                {
                    if (string1.Equals("bubble"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntityBubbleFX(this.worldObj, d2, d4, d6, d8, d10, d12));
                    }
                    else if (string1.Equals("smoke"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntitySmokeFX(this.worldObj, d2, d4, d6, d8, d10, d12));
                    }
                    else if (string1.Equals("note"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntityNoteFX(this.worldObj, d2, d4, d6, d8, d10, d12));
                    }
                    else if (string1.Equals("portal"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntityPortalFX(this.worldObj, d2, d4, d6, d8, d10, d12));
                    }
                    else if (string1.Equals("explode"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntityExplodeFX(this.worldObj, d2, d4, d6, d8, d10, d12));
                    }
                    else if (string1.Equals("flame"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntityFlameFX(this.worldObj, d2, d4, d6, d8, d10, d12));
                    }
                    else if (string1.Equals("lava"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntityLavaFX(this.worldObj, d2, d4, d6));
                    }
                    else if (string1.Equals("footstep"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntityFootStepFX(this.renderEngine, this.worldObj, d2, d4, d6));
                    }
                    else if (string1.Equals("splash"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntitySplashFX(this.worldObj, d2, d4, d6, d8, d10, d12));
                    }
                    else if (string1.Equals("largesmoke"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntitySmokeFX(this.worldObj, d2, d4, d6, d8, d10, d12, 2.5F));
                    }
                    else if (string1.Equals("reddust"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntityReddustFX(this.worldObj, d2, d4, d6, (float)d8, (float)d10, (float)d12));
                    }
                    else if (string1.Equals("snowballpoof"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntitySlimeFX(this.worldObj, d2, d4, d6, Item.snowball));
                    }
                    else if (string1.Equals("snowshovel"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntitySnowShovelFX(this.worldObj, d2, d4, d6, d8, d10, d12));
                    }
                    else if (string1.Equals("slime"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntitySlimeFX(this.worldObj, d2, d4, d6, Item.slimeBall));
                    }
                    else if (string1.Equals("heart"))
                    {
                        this.mc.effectRenderer.AddEffect(new EntityHeartFX(this.worldObj, d2, d4, d6, d8, d10, d12));
                    }
                }
            }
        }

        public virtual void EntityAdded(Entity entity1)
        {
            entity1.UpdateCloak();
            if (entity1.skinUrl != null)
            {
                this.renderEngine.AddMemTexture(entity1.skinUrl, new MobSkinTextureProcessor());
            }

            if (entity1.cloakUrl != null)
            {
                this.renderEngine.AddMemTexture(entity1.cloakUrl, new MobSkinTextureProcessor());
            }
        }

        public virtual void EntityRemoved(Entity entity1)
        {
            if (entity1.skinUrl != null)
            {
                this.renderEngine.RemoveMemTexture(entity1.skinUrl);
            }

            if (entity1.cloakUrl != null)
            {
                this.renderEngine.RemoveMemTexture(entity1.cloakUrl);
            }
        }

        public virtual void AllChanged()
        {
            for (int i1 = 0; i1 < this.worldRenderers.Length; ++i1)
            {
                if (this.worldRenderers[i1].isChunkLit && !this.worldRenderers[i1].needsUpdate)
                {
                    this.worldRenderersToUpdate.Add(this.worldRenderers[i1]);
                    this.worldRenderers[i1].MarkDirty();
                }
            }
        }

        public virtual void TileEntityChanged(int i1, int i2, int i3, TileEntity tileEntity4)
        {
        }

        public virtual void ReleaseLists()
        {
            MemoryTracker.Release(this.glRenderListBase);
        }

        public virtual void LevelEvent(Player player, LevelEventType type, int x, int y, int z, int data)
        {
            JRandom random = this.worldObj.rand;
            switch (type)
            {
                case LevelEventType.CLICK_1_0:
                    this.worldObj.PlaySound(x, y, z, "random.click", 1F, 1F);
                    break;
                case LevelEventType.CLICK_1_2:
                    this.worldObj.PlaySound(x, y, z, "random.click", 1F, 1.2F);
                    break;
                case LevelEventType.BOW:
                    this.worldObj.PlaySound(x, y, z, "random.bow", 1F, 1.2F);
                    break;
                case LevelEventType.DOOR:
                    if (Mth.Random() < 0.5)
                    {
                        this.worldObj.PlaySound(x + 0.5, y + 0.5, z + 0.5, "random.door_open", 1F, this.worldObj.rand.NextFloat() * 0.1F + 0.9F);
                    }
                    else
                    {
                        this.worldObj.PlaySound(x + 0.5, y + 0.5, z + 0.5, "random.door_close", 1F, this.worldObj.rand.NextFloat() * 0.1F + 0.9F);
                    }

                    break;
                case LevelEventType.FIZZ:
                    this.worldObj.PlaySound(x + 0.5F, y + 0.5F, z + 0.5F, "random.fizz", 0.5F, 2.6F + (random.NextFloat() - random.NextFloat()) * 0.8F);
                    break;
                case LevelEventType.RECORD:
                    if (Item.items[data] is ItemRecord)
                    {
                        this.worldObj.PlayStreamingMusic(((ItemRecord)Item.items[data]).recordName, x, y, z);
                    }
                    else
                    {
                        this.worldObj.PlayStreamingMusic((string)null, x, y, z);
                    }

                    break;
                case LevelEventType.SMOKE:
                    int i8 = data % 3 - 1;
                    int i9 = data / 3 % 3 - 1;
                    double d10 = x + i8 * 0.6 + 0.5;
                    double d12 = y + 0.5;
                    double d14 = z + i9 * 0.6 + 0.5;
                    for (int i16 = 0; i16 < 10; ++i16)
                    {
                        double d31 = random.NextDouble() * 0.2 + 0.01;
                        double d19 = d10 + i8 * 0.01 + (random.NextDouble() - 0.5) * i9 * 0.5;
                        double d21 = d12 + (random.NextDouble() - 0.5) * 0.5;
                        double d23 = d14 + i9 * 0.01 + (random.NextDouble() - 0.5) * i8 * 0.5;
                        double d25 = i8 * d31 + random.NextGaussian() * 0.01;
                        double d27 = -0.03 + random.NextGaussian() * 0.01;
                        double d29 = i9 * d31 + random.NextGaussian() * 0.01;
                        this.AddParticle("smoke", d19, d21, d23, d25, d27, d29);
                    }

                    return;
                case LevelEventType.BREAK_SOUND:
                    int i161 = data & 255;
                    if (i161 > 0)
                    {
                        Tile block17 = Tile.tiles[i161];
                        this.mc.soundEngine.PlaySound(block17.soundType.GetBreakSound(), x + 0.5F, y + 0.5F, z + 0.5F, (block17.soundType.GetVolume() + 1F) / 2F, block17.soundType.GetPitch() * 0.8F);
                    }

                    this.mc.effectRenderer.AddBlockDestroyEffects(x, y, z, data & 255, data >> 8 & 255);
                    break;
            }
        }
    }
}
