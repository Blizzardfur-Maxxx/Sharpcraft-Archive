using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Client.Renderer.Culling;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Client.Renderer.Tileentities;

namespace SharpCraft.Client.Renderer
{
    public class Chunk
    {
        public Level worldObj;
        private uint glRenderList = uint.MaxValue;
        private static Tessellator tessellator = Tessellator.Instance;
        public static int updates = 0;
        public int posX;
        public int posY;
        public int posZ;
        public int sizeWidth;
        public int sizeHeight;
        public int sizeDepth;
        public int posXMinus;
        public int posYMinus;
        public int posZMinus;
        public int posXClip;
        public int posYClip;
        public int posZClip;
        public bool isInFrustum = false;
        public bool[] skipRenderPass = new bool[2];
        public int posXPlus;
        public int posYPlus;
        public int posZPlus;
        public float rendererRadius;
        public bool needsUpdate;
        public AABB rendererBoundingBox;
        public int chunkIndex;
        public bool isVisible = true;
        public bool isWaitingOnOcclusionQuery;
        public uint glOcclusionQuery;
        public bool isChunkLit;
        private bool isInitialized = false;
        public List<TileEntity> tileEntityRenderers = new List<TileEntity>();
        private List<TileEntity> tileEntities;

        public Chunk(Level world1, List<TileEntity> list2, int i3, int i4, int i5, int i6, uint i7)
        {
            this.worldObj = world1;
            this.tileEntities = list2;
            this.sizeWidth = this.sizeHeight = this.sizeDepth = i6;
            this.rendererRadius = Mth.Sqrt(this.sizeWidth * this.sizeWidth + this.sizeHeight * this.sizeHeight + this.sizeDepth * this.sizeDepth) / 2F;
            this.glRenderList = i7;
            this.posX = -999;
            this.SetPosition(i3, i4, i5);
            this.needsUpdate = false;
        }

        public virtual void SetPosition(int i1, int i2, int i3)
        {
            if (i1 != this.posX || i2 != this.posY || i3 != this.posZ)
            {
                this.SetDontDraw();
                this.posX = i1;
                this.posY = i2;
                this.posZ = i3;
                this.posXPlus = i1 + this.sizeWidth / 2;
                this.posYPlus = i2 + this.sizeHeight / 2;
                this.posZPlus = i3 + this.sizeDepth / 2;
                this.posXClip = i1 & 1023;
                this.posYClip = i2;
                this.posZClip = i3 & 1023;
                this.posXMinus = i1 - this.posXClip;
                this.posYMinus = i2 - this.posYClip;
                this.posZMinus = i3 - this.posZClip;
                float f4 = 6F;
                this.rendererBoundingBox = AABB.CreateAABB(i1 - f4, i2 - f4, i3 - f4, i1 + this.sizeWidth + f4, i2 + this.sizeHeight + f4, i3 + this.sizeDepth + f4);
                GL11.glNewList(this.glRenderList + 2, GL11C.GL_COMPILE);
                Render.RenderAABB(AABB.Of(this.posXClip - f4, this.posYClip - f4, this.posZClip - f4, this.posXClip + this.sizeWidth + f4, this.posYClip + this.sizeHeight + f4, this.posZClip + this.sizeDepth + f4));
                GL11.glEndList();
                this.MarkDirty();
            }
        }

        private void SetupGLTranslation()
        {
            GL11.glTranslatef(this.posXClip, this.posYClip, this.posZClip);
        }

        public virtual void UpdateRenderer()
        {
            if (this.needsUpdate)
            {
                ++updates;
                int i1 = this.posX;
                int i2 = this.posY;
                int i3 = this.posZ;
                int i4 = this.posX + this.sizeWidth;
                int i5 = this.posY + this.sizeHeight;
                int i6 = this.posZ + this.sizeDepth;
                for (int i7 = 0; i7 < 2; ++i7)
                {
                    this.skipRenderPass[i7] = true;
                }

                LevelChunk.isLit = false;
                HashSet<TileEntity> hashSet21 = new HashSet<TileEntity>();
                foreach (TileEntity te in this.tileEntityRenderers) { hashSet21.Add(te); }
                //hashSet21.AddAll(this.tileEntityRenderers);
                this.tileEntityRenderers.Clear();
                byte b8 = 1;
                Region chunkCache9 = new Region(this.worldObj, i1 - b8, i2 - b8, i3 - b8, i4 + b8, i5 + b8, i6 + b8);
                TileRenderer renderBlocks10 = new TileRenderer(chunkCache9);
                for (uint i11 = 0; i11 < 2; ++i11)
                {
                    bool z12 = false;
                    bool z13 = false;
                    bool z14 = false;
                    for (int i15 = i2; i15 < i5; ++i15)
                    {
                        for (int i16 = i3; i16 < i6; ++i16)
                        {
                            for (int i17 = i1; i17 < i4; ++i17)
                            {
                                int i18 = chunkCache9.GetTile(i17, i15, i16);
                                if (i18 > 0)
                                {
                                    if (!z14)
                                    {
                                        z14 = true;
                                        GL11.glNewList(this.glRenderList + i11, GL11C.GL_COMPILE);
                                        GL11.glPushMatrix();
                                        this.SetupGLTranslation();
                                        float f19 = 1.000001F;
                                        GL11.glTranslatef((-this.sizeDepth) / 2F, (-this.sizeHeight) / 2F, (-this.sizeDepth) / 2F);
                                        GL11.glScalef(f19, f19, f19);
                                        GL11.glTranslatef(this.sizeDepth / 2F, this.sizeHeight / 2F, this.sizeDepth / 2F);
                                        tessellator.Begin();
                                        tessellator.Offset((-this.posX), (-this.posY), (-this.posZ));
                                    }

                                    if (i11 == 0 && Tile.isEntityTile[i18])
                                    {
                                        TileEntity tileEntity23 = chunkCache9.GetTileEntity(i17, i15, i16);
                                        if (TileEntityRenderDispatcher.instance.HasSpecialRenderer(tileEntity23))
                                        {
                                            this.tileEntityRenderers.Add(tileEntity23);
                                        }
                                    }

                                    Tile block24 = Tile.tiles[i18];
                                    int i20 = (int)block24.GetRenderLayer();
                                    if (i20 != i11)
                                    {
                                        z12 = true;
                                    }
                                    else if (i20 == i11)
                                    {
                                        z13 |= renderBlocks10.RenderBlockByRenderType(block24, i17, i15, i16);
                                    }
                                }
                            }
                        }
                    }

                    if (z14)
                    {
                        tessellator.End();
                        GL11.glPopMatrix();
                        GL11.glEndList();
                        tessellator.Offset(0, 0, 0);
                    }
                    else
                    {
                        z13 = false;
                    }

                    if (z13)
                    {
                        this.skipRenderPass[i11] = false;
                    }

                    if (!z12)
                    {
                        break;
                    }
                }

                HashSet<TileEntity> hashSet22 = new HashSet<TileEntity>();
                foreach (TileEntity te in this.tileEntityRenderers) { hashSet22.Add(te); }
                foreach(TileEntity te in hashSet21) { hashSet22.Remove(te); }
                foreach (TileEntity te in hashSet22) {this.tileEntities.Add(te);}
                foreach (TileEntity te in this.tileEntityRenderers) { hashSet21.Remove(te); }
                foreach (TileEntity te in hashSet21) { this.tileEntities.Remove(te); }
                this.isChunkLit = LevelChunk.isLit;
                this.isInitialized = true;
            }
        }

        public virtual float DistanceToEntitySquared(Entity entity1)
        {
            float f2 = (float)(entity1.x - this.posXPlus);
            float f3 = (float)(entity1.y - this.posYPlus);
            float f4 = (float)(entity1.z - this.posZPlus);
            return f2 * f2 + f3 * f3 + f4 * f4;
        }

        public virtual void SetDontDraw()
        {
            for (int i1 = 0; i1 < 2; ++i1)
            {
                this.skipRenderPass[i1] = true;
            }

            this.isInFrustum = false;
            this.isInitialized = false;
        }

        public virtual void Func_1204_c()
        {
            this.SetDontDraw();
            this.worldObj = null;
        }

        public virtual uint GetGLCallListForPass(uint i1)
        {
            return !this.isInFrustum ? uint.MaxValue : (!this.skipRenderPass[i1] ? this.glRenderList + i1 : uint.MaxValue);
        }

        public virtual void UpdateInFrustrum(ICuller iCamera1)
        {
            this.isInFrustum = iCamera1.IsVisible(this.rendererBoundingBox);
        }

        public virtual void CallOcclusionQueryList()
        {
            GL11.glCallList(this.glRenderList + 2);
        }

        public virtual bool SkipAllRenderPasses()
        {
            return !this.isInitialized ? false : this.skipRenderPass[0] && this.skipRenderPass[1];
        }

        public virtual void MarkDirty()
        {
            this.needsUpdate = true;
        }
    }
}
