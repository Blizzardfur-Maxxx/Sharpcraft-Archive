using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Client.GUI;

namespace SharpCraft.Client.Renderer.Entities
{
    public abstract class Render
    {
        protected EntityRenderDispatcher renderManager;
        protected float shadowSize = 0.0F;
        protected float field_194_c = 1.0F;

        public abstract void DoRender(Entity entity1, double d2, double d4, double d6, float f8, float f9);

        protected void LoadTexture(string string1)
        {
            Textures renderEngine2 = this.renderManager.textures;
            renderEngine2.Bind(renderEngine2.LoadTexture(string1));
        }

        protected bool LoadDownloadableImageTexture(string url, string fallback)
        {
            Textures renderEngine3 = this.renderManager.textures;
            uint i4 = renderEngine3.LoadMemTexture(url, fallback);
            if (i4 != uint.MaxValue)
            {
                renderEngine3.Bind(i4);
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void RenderOffsetAABB(AABB axisAlignedBB0, double d1, double d3, double d5)
        {
            GL11.glDisable(GL11C.GL_TEXTURE_2D);
            Tessellator tessellator7 = Tessellator.Instance;
            GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
            tessellator7.Begin();
            tessellator7.Offset(d1, d3, d5);
            tessellator7.Normal(0.0F, 0.0F, -1.0F);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator7.Normal(0.0F, 0.0F, 1.0F);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator7.Normal(0.0F, -1.0F, 0.0F);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator7.Normal(0.0F, 1.0F, 0.0F);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator7.Normal(-1.0F, 0.0F, 0.0F);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator7.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator7.Normal(1.0F, 0.0F, 0.0F);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator7.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator7.Offset(0.0D, 0.0D, 0.0D);
            tessellator7.End();
            GL11.glEnable(GL11C.GL_TEXTURE_2D);
        }

        public static void RenderAABB(AABB axisAlignedBB0)
        {
            Tessellator tessellator1 = Tessellator.Instance;
            tessellator1.Begin();
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x0, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z0);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y1, axisAlignedBB0.z1);
            tessellator1.Vertex(axisAlignedBB0.x1, axisAlignedBB0.y0, axisAlignedBB0.z1);
            tessellator1.End();
        }
        
        public void SetRenderManager(EntityRenderDispatcher renderManager1)
        {
            this.renderManager = renderManager1;
        }

        public Font GetFont()
        {
            return this.renderManager.GetFont();
        }
        
        public void DoRenderShadowAndFire(Entity entity1, double d2, double d4, double d6, float f8, float f9)
        {
            if (this.renderManager.options.fancyGraphics && this.shadowSize > 0.0F)
            {
                double d10 = this.renderManager.Func_851_a(entity1.x, entity1.y, entity1.z);
                float f12 = (float)((1.0D - d10 / 256.0D) * this.field_194_c);
                if (f12 > 0.0F)
                {
                    this.RenderShadow(entity1, d2, d4, d6, f12, f9);
                }
            }

            if (entity1.IsBurning())
            {
                this.RenderEntityOnFire(entity1, d2, d4, d6, f9);
            }
        }
        
        private void RenderEntityOnFire(Entity entity1, double d2, double d4, double d6, float f8)
        {
            GL11.glDisable(GL11C.GL_LIGHTING);
            int i9 = Tile.fire.texture;
            int i10 = (i9 & 15) << 4;
            int i11 = i9 & 240;
            float f12 = i10 / 256.0F;
            float f13 = (i10 + 15.99F) / 256.0F;
            float f14 = i11 / 256.0F;
            float f15 = (i11 + 15.99F) / 256.0F;
            GL11.glPushMatrix();
            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
            float f16 = entity1.width * 1.4F;
            GL11.glScalef(f16, f16, f16);
            this.LoadTexture("/terrain.png");
            Tessellator tessellator17 = Tessellator.Instance;
            float f18 = 0.5F;
            float f19 = 0.0F;
            float f20 = entity1.height / f16;
            float f21 = (float)(entity1.y - entity1.boundingBox.y0);
            GL11.glRotatef(-this.renderManager.playerViewY, 0.0F, 1.0F, 0.0F);
            GL11.glTranslatef(0.0F, 0.0F, -0.3F + ((int)f20) * 0.02F);
            GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
            float f22 = 0.0F;
            int i23 = 0;
            tessellator17.Begin();

            while (f20 > 0.0F)
            {
                if (i23 % 2 == 0)
                {
                    f12 = i10 / 256.0F;
                    f13 = (i10 + 15.99F) / 256.0F;
                    f14 = i11 / 256.0F;
                    f15 = (i11 + 15.99F) / 256.0F;
                }
                else
                {
                    f12 = i10 / 256.0F;
                    f13 = (i10 + 15.99F) / 256.0F;
                    f14 = (i11 + 16) / 256.0F;
                    f15 = (i11 + 16 + 15.99F) / 256.0F;
                }

                if (i23 / 2 % 2 == 0)
                {
                    float f24 = f13;
                    f13 = f12;
                    f12 = f24;
                }

                tessellator17.VertexUV(f18 - f19, 0.0F - f21, f22, f13, f15);
                tessellator17.VertexUV(-f18 - f19, 0.0F - f21, f22, f12, f15);
                tessellator17.VertexUV(-f18 - f19, 1.4F - f21, f22, f12, f14);
                tessellator17.VertexUV(f18 - f19, 1.4F - f21, f22, f13, f14);
                f20 -= 0.45F;
                f21 -= 0.45F;
                f18 *= 0.9F;
                f22 += 0.03F;
                ++i23;
            }

            tessellator17.End();
            GL11.glPopMatrix();
            GL11.glEnable(GL11C.GL_LIGHTING);
        }
        
        private void RenderShadow(Entity entity1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            Textures renderEngine10 = this.renderManager.textures;
            renderEngine10.Bind(renderEngine10.LoadTexture("%clamp%/misc/shadow.png"));
            Level world11 = this.GetWorldFromRenderManager();
            GL11.glDepthMask(false);
            float f12 = this.shadowSize;
            double d13 = entity1.lastTickPosX + (entity1.x - entity1.lastTickPosX) * f9;
            double d15 = entity1.lastTickPosY + (entity1.y - entity1.lastTickPosY) * f9 + entity1.GetShadowSize();
            double d17 = entity1.lastTickPosZ + (entity1.z - entity1.lastTickPosZ) * f9;
            int i19 = Mth.Floor(d13 - f12);
            int i20 = Mth.Floor(d13 + f12);
            int i21 = Mth.Floor(d15 - f12);
            int i22 = Mth.Floor(d15);
            int i23 = Mth.Floor(d17 - f12);
            int i24 = Mth.Floor(d17 + f12);
            double d25 = d2 - d13;
            double d27 = d4 - d15;
            double d29 = d6 - d17;
            Tessellator tessellator31 = Tessellator.Instance;
            tessellator31.Begin();

            for (int i32 = i19; i32 <= i20; ++i32)
            {
                for (int i33 = i21; i33 <= i22; ++i33)
                {
                    for (int i34 = i23; i34 <= i24; ++i34)
                    {
                        int i35 = world11.GetTile(i32, i33 - 1, i34);
                        if (i35 > 0 && world11.GetRawBrightness(i32, i33, i34) > 3)
                        {
                            this.RenderShadowOnBlock(Tile.tiles[i35], d2, d4 + entity1.GetShadowSize(), d6, i32, i33, i34, f8, f12, d25, d27 + entity1.GetShadowSize(), d29);
                        }
                    }
                }
            }

            tessellator31.End();
            GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
            GL11.glDisable(GL11C.GL_BLEND);
            GL11.glDepthMask(true);
        }

        private Level GetWorldFromRenderManager()
        {
            return this.renderManager.worldObj;
        }

        private void RenderShadowOnBlock(Tile block1, double d2, double d4, double d6, int i8, int i9, int i10, float f11, float f12, double d13, double d15, double d17)
        {
            Tessellator tessellator19 = Tessellator.Instance;
            if (block1.IsCubeShaped())
            {
                double d20 = (f11 - (d4 - (i9 + d15)) / 2.0D) * 0.5D * this.GetWorldFromRenderManager().GetBrightness(i8, i9, i10);
                if (d20 >= 0.0D)
                {
                    if (d20 > 1.0D)
                    {
                        d20 = 1.0D;
                    }

                    tessellator19.Color(1.0F, 1.0F, 1.0F, (float)d20);
                    double d22 = i8 + block1.minX + d13;
                    double d24 = i8 + block1.maxX + d13;
                    double d26 = i9 + block1.minY + d15 + 0.015625D;
                    double d28 = i10 + block1.minZ + d17;
                    double d30 = i10 + block1.maxZ + d17;
                    float f32 = (float)((d2 - d22) / 2.0D / f12 + 0.5D);
                    float f33 = (float)((d2 - d24) / 2.0D / f12 + 0.5D);
                    float f34 = (float)((d6 - d28) / 2.0D / f12 + 0.5D);
                    float f35 = (float)((d6 - d30) / 2.0D / f12 + 0.5D);
                    tessellator19.VertexUV(d22, d26, d28, f32, f34);
                    tessellator19.VertexUV(d22, d26, d30, f32, f35);
                    tessellator19.VertexUV(d24, d26, d30, f33, f35);
                    tessellator19.VertexUV(d24, d26, d28, f33, f34);
                }
            }
        }
    }
    
    public abstract class Render<T> : Render where T : Entity //don't inherit "Render<Entity>", just inherit the Render class
    {
        public override void DoRender(Entity entity1, double d2, double d4, double d6, float f8, float f9)
        {
            DoRender((T)entity1, d2, d4, d6, f8, f9);
        }

        public abstract void DoRender(T entity1, double d2, double d4, double d6, float f8, float f9);
    }
}
