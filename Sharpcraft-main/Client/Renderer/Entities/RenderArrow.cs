using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Projectile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderArrow : Render<Arrow>
    {
        public override void DoRender(Arrow entityArrow1, double d2, double d4, double d6, float f8, float f9)
        {
            if (entityArrow1.prevYaw != 0.0F || entityArrow1.prevPitch != 0.0F)
            {
                this.LoadTexture("/item/arrows.png");
                GL11.glPushMatrix();
                GL11.glTranslatef((float)d2, (float)d4, (float)d6);
                GL11.glRotatef(entityArrow1.prevYaw + (entityArrow1.yaw - entityArrow1.prevYaw) * f9 - 90.0F, 0.0F, 1.0F, 0.0F);
                GL11.glRotatef(entityArrow1.prevPitch + (entityArrow1.pitch - entityArrow1.prevPitch) * f9, 0.0F, 0.0F, 1.0F);
                Tessellator tessellator10 = Tessellator.Instance;
                byte b11 = 0;
                float f12 = 0.0F;
                float f13 = 0.5F;
                float f14 = (0 + b11 * 10) / 32.0F;
                float f15 = (5 + b11 * 10) / 32.0F;
                float f16 = 0.0F;
                float f17 = 0.15625F;
                float f18 = (5 + b11 * 10) / 32.0F;
                float f19 = (10 + b11 * 10) / 32.0F;
                float f20 = 0.05625F;
                GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
                float f21 = entityArrow1.arrowShake - f9;
                if (f21 > 0.0F)
                {
                    float f22 = -Mth.Sin(f21 * 3.0F) * f21;
                    GL11.glRotatef(f22, 0.0F, 0.0F, 1.0F);
                }

                GL11.glRotatef(45.0F, 1.0F, 0.0F, 0.0F);
                GL11.glScalef(f20, f20, f20);
                GL11.glTranslatef(-4.0F, 0.0F, 0.0F);
                GL11.glNormal3f(f20, 0.0F, 0.0F);
                tessellator10.Begin();
                tessellator10.VertexUV(-7.0D, -2.0D, -2.0D, f16, f18);
                tessellator10.VertexUV(-7.0D, -2.0D, 2.0D, f17, f18);
                tessellator10.VertexUV(-7.0D, 2.0D, 2.0D, f17, f19);
                tessellator10.VertexUV(-7.0D, 2.0D, -2.0D, f16, f19);
                tessellator10.End();
                GL11.glNormal3f(-f20, 0.0F, 0.0F);
                tessellator10.Begin();
                tessellator10.VertexUV(-7.0D, 2.0D, -2.0D, f16, f18);
                tessellator10.VertexUV(-7.0D, 2.0D, 2.0D, f17, f18);
                tessellator10.VertexUV(-7.0D, -2.0D, 2.0D, f17, f19);
                tessellator10.VertexUV(-7.0D, -2.0D, -2.0D, f16, f19);
                tessellator10.End();

                for (int i23 = 0; i23 < 4; ++i23)
                {
                    GL11.glRotatef(90.0F, 1.0F, 0.0F, 0.0F);
                    GL11.glNormal3f(0.0F, 0.0F, f20);
                    tessellator10.Begin();
                    tessellator10.VertexUV(-8.0D, -2.0D, 0.0D, f12, f14);
                    tessellator10.VertexUV(8.0D, -2.0D, 0.0D, f13, f14);
                    tessellator10.VertexUV(8.0D, 2.0D, 0.0D, f13, f15);
                    tessellator10.VertexUV(-8.0D, 2.0D, 0.0D, f12, f15);
                    tessellator10.End();
                }

                GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
                GL11.glPopMatrix();
            }
        }
    }
}
