using LWCSGL.OpenGL;
using SharpCraft.Core.World.Entities;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderSnowball : Render
    {
        private int v;

        public RenderSnowball(int v)
        {
            this.v = v;
        }

        public override void DoRender(Entity entity1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glPushMatrix();
            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            GL11.glScalef(0.5F, 0.5F, 0.5F);
            this.LoadTexture("/gui/items.png");
            Tessellator tessellator10 = Tessellator.Instance;
            float f11 = (this.v % 16 * 16 + 0) / 256.0F;
            float f12 = (this.v % 16 * 16 + 16) / 256.0F;
            float f13 = (this.v / 16 * 16 + 0) / 256.0F;
            float f14 = (this.v / 16 * 16 + 16) / 256.0F;
            float f15 = 1.0F;
            float f16 = 0.5F;
            float f17 = 0.25F;
            GL11.glRotatef(180.0F - this.renderManager.playerViewY, 0.0F, 1.0F, 0.0F);
            GL11.glRotatef(-this.renderManager.playerViewX, 1.0F, 0.0F, 0.0F);
            tessellator10.Begin();
            tessellator10.Normal(0.0F, 1.0F, 0.0F);
            tessellator10.VertexUV(0.0F - f16, 0.0F - f17, 0.0D, f11, f14);
            tessellator10.VertexUV(f15 - f16, 0.0F - f17, 0.0D, f12, f14);
            tessellator10.VertexUV(f15 - f16, 1.0F - f17, 0.0D, f12, f13);
            tessellator10.VertexUV(0.0F - f16, 1.0F - f17, 0.0D, f11, f13);
            tessellator10.End();
            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            GL11.glPopMatrix();
        }
    }
}