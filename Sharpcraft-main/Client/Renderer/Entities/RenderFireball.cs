using LWCSGL.OpenGL;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderFireball : Render<Fireball>
    {
        public override void DoRender(Fireball entityFireball1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glPushMatrix();
            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            float f10 = 2.0F;
            GL11.glScalef(f10 / 1.0F, f10 / 1.0F, f10 / 1.0F);
            int i11 = Item.snowball.GetIconFromDamage(0);
            this.LoadTexture("/gui/items.png");
            Tessellator tessellator12 = Tessellator.Instance;
            float f13 = (i11 % 16 * 16 + 0) / 256.0F;
            float f14 = (i11 % 16 * 16 + 16) / 256.0F;
            float f15 = (i11 / 16 * 16 + 0) / 256.0F;
            float f16 = (i11 / 16 * 16 + 16) / 256.0F;
            float f17 = 1.0F;
            float f18 = 0.5F;
            float f19 = 0.25F;
            GL11.glRotatef(180.0F - this.renderManager.playerViewY, 0.0F, 1.0F, 0.0F);
            GL11.glRotatef(-this.renderManager.playerViewX, 1.0F, 0.0F, 0.0F);
            tessellator12.Begin();
            tessellator12.Normal(0.0F, 1.0F, 0.0F);
            tessellator12.VertexUV(0.0F - f18, 0.0F - f19, 0.0D, f13, f16);
            tessellator12.VertexUV(f17 - f18, 0.0F - f19, 0.0D, f14, f16);
            tessellator12.VertexUV(f17 - f18, 1.0F - f19, 0.0D, f14, f15);
            tessellator12.VertexUV(0.0F - f18, 1.0F - f19, 0.0D, f13, f15);
            tessellator12.End();
            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            GL11.glPopMatrix();
        }

    }
}