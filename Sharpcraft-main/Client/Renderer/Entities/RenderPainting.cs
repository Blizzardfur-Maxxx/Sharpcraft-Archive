using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Items;
using System;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderPainting : Render<Painting>
    {
        private JRandom rand = new JRandom();

        public override void DoRender(Painting entity1, double d2, double d4, double d6, float f8, float f9)
        {
            this.rand.SetSeed(187L);
            GL11.glPushMatrix();
            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
            GL11.glRotatef(f8, 0.0F, 1.0F, 0.0F);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            this.LoadTexture("/art/kz.png");
            Motive enumArt10 = entity1.art;
            float f11 = 0.0625F;
            GL11.glScalef(f11, f11, f11);
            this.Func_159_a(entity1, enumArt10.sizeX, enumArt10.sizeY, enumArt10.offsetX, enumArt10.offsetY);
            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            GL11.glPopMatrix();
        }

        private void Func_159_a(Painting entityPainting1, int i2, int i3, int i4, int i5)
        {
            float f6 = (-i2) / 2.0F;
            float f7 = (-i3) / 2.0F;
            float f8 = -0.5F;
            float f9 = 0.5F;

            for (int i10 = 0; i10 < i2 / 16; ++i10)
            {
                for (int i11 = 0; i11 < i3 / 16; ++i11)
                {
                    float f12 = f6 + (i10 + 1) * 16;
                    float f13 = f6 + i10 * 16;
                    float f14 = f7 + (i11 + 1) * 16;
                    float f15 = f7 + i11 * 16;
                    this.Func_160_a(entityPainting1, (f12 + f13) / 2.0F, (f14 + f15) / 2.0F);
                    float f16 = (i4 + i2 - i10 * 16) / 256.0F;
                    float f17 = (i4 + i2 - (i10 + 1) * 16) / 256.0F;
                    float f18 = (i5 + i3 - i11 * 16) / 256.0F;
                    float f19 = (i5 + i3 - (i11 + 1) * 16) / 256.0F;
                    float f20 = 0.75F;
                    float f21 = 0.8125F;
                    float f22 = 0.0F;
                    float f23 = 0.0625F;
                    float f24 = 0.75F;
                    float f25 = 0.8125F;
                    float f26 = 0.001953125F;
                    float f27 = 0.001953125F;
                    float f28 = 0.7519531F;
                    float f29 = 0.7519531F;
                    float f30 = 0.0F;
                    float f31 = 0.0625F;
                    Tessellator tessellator32 = Tessellator.Instance;
                    tessellator32.Begin();
                    tessellator32.Normal(0.0F, 0.0F, -1.0F);
                    tessellator32.VertexUV(f12, f15, f8, f17, f18);
                    tessellator32.VertexUV(f13, f15, f8, f16, f18);
                    tessellator32.VertexUV(f13, f14, f8, f16, f19);
                    tessellator32.VertexUV(f12, f14, f8, f17, f19);
                    tessellator32.Normal(0.0F, 0.0F, 1.0F);
                    tessellator32.VertexUV(f12, f14, f9, f20, f22);
                    tessellator32.VertexUV(f13, f14, f9, f21, f22);
                    tessellator32.VertexUV(f13, f15, f9, f21, f23);
                    tessellator32.VertexUV(f12, f15, f9, f20, f23);
                    tessellator32.Normal(0.0F, -1.0F, 0.0F);
                    tessellator32.VertexUV(f12, f14, f8, f24, f26);
                    tessellator32.VertexUV(f13, f14, f8, f25, f26);
                    tessellator32.VertexUV(f13, f14, f9, f25, f27);
                    tessellator32.VertexUV(f12, f14, f9, f24, f27);
                    tessellator32.Normal(0.0F, 1.0F, 0.0F);
                    tessellator32.VertexUV(f12, f15, f9, f24, f26);
                    tessellator32.VertexUV(f13, f15, f9, f25, f26);
                    tessellator32.VertexUV(f13, f15, f8, f25, f27);
                    tessellator32.VertexUV(f12, f15, f8, f24, f27);
                    tessellator32.Normal(-1.0F, 0.0F, 0.0F);
                    tessellator32.VertexUV(f12, f14, f9, f29, f30);
                    tessellator32.VertexUV(f12, f15, f9, f29, f31);
                    tessellator32.VertexUV(f12, f15, f8, f28, f31);
                    tessellator32.VertexUV(f12, f14, f8, f28, f30);
                    tessellator32.Normal(1.0F, 0.0F, 0.0F);
                    tessellator32.VertexUV(f13, f14, f8, f29, f30);
                    tessellator32.VertexUV(f13, f15, f8, f29, f31);
                    tessellator32.VertexUV(f13, f15, f9, f28, f31);
                    tessellator32.VertexUV(f13, f14, f9, f28, f30);
                    tessellator32.End();
                }
            }

        }

        private void Func_160_a(Painting entityPainting1, float f2, float f3)
        {
            int i4 = Mth.Floor(entityPainting1.x);
            int i5 = Mth.Floor(entityPainting1.y + f3 / 16.0F);
            int i6 = Mth.Floor(entityPainting1.z);
            if (entityPainting1.direction == 0)
            {
                i4 = Mth.Floor(entityPainting1.x + f2 / 16.0F);
            }

            if (entityPainting1.direction == 1)
            {
                i6 = Mth.Floor(entityPainting1.z - f2 / 16.0F);
            }

            if (entityPainting1.direction == 2)
            {
                i4 = Mth.Floor(entityPainting1.x - f2 / 16.0F);
            }

            if (entityPainting1.direction == 3)
            {
                i6 = Mth.Floor(entityPainting1.z + f2 / 16.0F);
            }

            float f7 = this.renderManager.worldObj.GetBrightness(i4, i5, i6);
            GL11.glColor3f(f7, f7, f7);
        }
    }
}