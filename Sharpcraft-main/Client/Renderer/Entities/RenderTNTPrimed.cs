using LWCSGL.OpenGL;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderTNTPrimed : Render<PrimedTnt>
    {
        private TileRenderer blockRenderer = new TileRenderer();

        public RenderTNTPrimed()
        {
            this.shadowSize = 0.5F;
        }

        public override void DoRender(PrimedTnt entityTNTPrimed1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glPushMatrix();
            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
            float f10;
            if (entityTNTPrimed1.fuse - f9 + 1.0F < 10.0F)
            {
                f10 = 1.0F - (entityTNTPrimed1.fuse - f9 + 1.0F) / 10.0F;
                if (f10 < 0.0F)
                {
                    f10 = 0.0F;
                }

                if (f10 > 1.0F)
                {
                    f10 = 1.0F;
                }

                f10 *= f10;
                f10 *= f10;
                float f11 = 1.0F + f10 * 0.3F;
                GL11.glScalef(f11, f11, f11);
            }

            f10 = (1.0F - (entityTNTPrimed1.fuse - f9 + 1.0F) / 100.0F) * 0.8F;
            this.LoadTexture("/terrain.png");
            this.blockRenderer.RenderBlockOnInventory(Tile.tnt, 0, entityTNTPrimed1.GetEntityBrightness(f9));
            if (entityTNTPrimed1.fuse / 5 % 2 == 0)
            {
                GL11.glDisable(GL11C.GL_TEXTURE_2D);
                GL11.glDisable(GL11C.GL_LIGHTING);
                GL11.glEnable(GL11C.GL_BLEND);
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_DST_ALPHA);
                GL11.glColor4f(1.0F, 1.0F, 1.0F, f10);
                this.blockRenderer.RenderBlockOnInventory(Tile.tnt, 0, 1.0F);
                GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
                GL11.glDisable(GL11C.GL_BLEND);
                GL11.glEnable(GL11C.GL_LIGHTING);
                GL11.glEnable(GL11C.GL_TEXTURE_2D);
            }

            GL11.glPopMatrix();
        }
    }
}