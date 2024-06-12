using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities.Monsters;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderSlime : RenderLiving<Slime>
    {
        private Model scaleAmount;

        public RenderSlime(Model modelBase1, Model modelBase2, float f3) : base(modelBase1, f3)
        {
            this.scaleAmount = modelBase2;
        }

        protected override void PreRenderCallback(Slime entityLiving1, float f2)
        {
            int i3 = entityLiving1.GetSlimeSize();
            float f4 = (entityLiving1.field_b + (entityLiving1.field_a - entityLiving1.field_b) * f2) / (i3 * 0.5F + 1.0F);
            float f5 = 1.0F / (f4 + 1.0F);
            float f6 = i3;
            GL11.glScalef(f5 * f6, 1.0F / f5 * f6, f5 * f6);
        }

        protected override bool ShouldRenderPass(Slime entityLiving1, int i2, float f3)
        {
            if (i2 == 0)
            {
                this.SetRenderPassModel(this.scaleAmount);
                GL11.glEnable(GL11C.GL_NORMALIZE);
                GL11.glEnable(GL11C.GL_BLEND);
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                return true;
            }
            else
            {
                if (i2 == 1)
                {
                    GL11.glDisable(GL11C.GL_BLEND);
                    GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
                }

                return false;
            }
        }
    }

}