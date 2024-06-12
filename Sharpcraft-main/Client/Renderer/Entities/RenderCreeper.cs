using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Monsters;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderCreeper : RenderLiving<Creeper>
    {
        private Model field_27008_a = new ModelCreeper(2.0F);

        public RenderCreeper() : base(new ModelCreeper(), 0.5F)
        {
        }

        protected override void PreRenderCallback(Creeper entityLiving1, float f2)
        {
            float f4 = entityLiving1.SetCreeperFlashTime(f2);
            float f5 = 1.0F + Mth.Sin(f4 * 100.0F) * f4 * 0.01F;
            if (f4 < 0.0F)
            {
                f4 = 0.0F;
            }

            if (f4 > 1.0F)
            {
                f4 = 1.0F;
            }

            f4 *= f4;
            f4 *= f4;
            float f6 = (1.0F + f4 * 0.4F) * f5;
            float f7 = (1.0F + f4 * 0.1F) / f5;
            GL11.glScalef(f6, f7, f6);
        }

        protected override int GetColorMultiplier(Creeper entityLiving1, float f2, float f3)
        {
            float f5 = entityLiving1.SetCreeperFlashTime(f3);
            if ((int)(f5 * 10.0F) % 2 == 0)
            {
                return 0;
            }
            else
            {
                int i6 = (int)(f5 * 0.2F * 255.0F);
                if (i6 < 0)
                {
                    i6 = 0;
                }

                if (i6 > 255)
                {
                    i6 = 255;
                }

                int s7 = 255;
                int s8 = 255;
                int s9 = 255;
                return i6 << 24 | s7 << 16 | s8 << 8 | s9;
            }
        }

        protected override bool ShouldRenderPass(Creeper entityLiving1, int i2, float f3)
        {
            if (entityLiving1.GetPowered())
            {
                if (i2 == 1)
                {
                    float f4 = entityLiving1.ticksExisted + f3;
                    this.LoadTexture("/armor/power.png");
                    GL11.glMatrixMode(GL11C.GL_TEXTURE);
                    GL11.glLoadIdentity();
                    float f5 = f4 * 0.01F;
                    float f6 = f4 * 0.01F;
                    GL11.glTranslatef(f5, f6, 0.0F);
                    this.SetRenderPassModel(this.field_27008_a);
                    GL11.glMatrixMode(GL11C.GL_MODELVIEW);
                    GL11.glEnable(GL11C.GL_BLEND);
                    float f7 = 0.5F;
                    GL11.glColor4f(f7, f7, f7, 1.0F);
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    GL11.glBlendFunc(GL11C.GL_ONE, GL11C.GL_ONE);
                    return true;
                }

                if (i2 == 2)
                {
                    GL11.glMatrixMode(GL11C.GL_TEXTURE);
                    GL11.glLoadIdentity();
                    GL11.glMatrixMode(GL11C.GL_MODELVIEW);
                    GL11.glEnable(GL11C.GL_LIGHTING);
                    GL11.glDisable(GL11C.GL_BLEND);
                }
            }

            return false;
        }

        protected override bool Func_27005_b(Creeper entityLiving1, int i2, float f3)
        {
            return false;
        }
    }
}