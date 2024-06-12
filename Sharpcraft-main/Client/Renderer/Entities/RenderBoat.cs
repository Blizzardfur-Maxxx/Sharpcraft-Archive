using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderBoat : Render<Boat>
    {
        protected Model modelBoat;

        public RenderBoat()
        {
            this.shadowSize = 0.5F;
            this.modelBoat = new ModelBoat();
        }

        public override void DoRender(Boat entityBoat1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glPushMatrix();
            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
            GL11.glRotatef(180.0F - f8, 0.0F, 1.0F, 0.0F);
            float f10 = entityBoat1.boatTimeSinceHit - f9;
            float f11 = entityBoat1.boatCurrentDamage - f9;
            if (f11 < 0.0F)
            {
                f11 = 0.0F;
            }

            if (f10 > 0.0F)
            {
                GL11.glRotatef(Mth.Sin(f10) * f10 * f11 / 10.0F * entityBoat1.boatRockDirection, 1.0F, 0.0F, 0.0F);
            }

            this.LoadTexture("/terrain.png");
            float f12 = 0.75F;
            GL11.glScalef(f12, f12, f12);
            GL11.glScalef(1.0F / f12, 1.0F / f12, 1.0F / f12);
            this.LoadTexture("/item/boat.png");
            GL11.glScalef(-1.0F, -1.0F, 1.0F);
            this.modelBoat.Render(0.0F, 0.0F, -0.1F, 0.0F, 0.0F, 0.0625F);
            GL11.glPopMatrix();
        }
    }
}