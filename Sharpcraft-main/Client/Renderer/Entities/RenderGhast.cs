using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities.Monsters;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderGhast : RenderLiving<Ghast>
    {
        public RenderGhast() : base(new ModelGhast(), 0.5F)
        {
        }

        protected override void PreRenderCallback(Ghast entityLiving1, float f2)
        {
            float f4 = (entityLiving1.prevAttackCounter + (entityLiving1.attackCounter - entityLiving1.prevAttackCounter) * f2) / 20.0F;
            if (f4 < 0.0F)
            {
                f4 = 0.0F;
            }

            f4 = 1.0F / (f4 * f4 * f4 * f4 * f4 * 2.0F + 1.0F);
            float f5 = (8.0F + f4) / 2.0F;
            float f6 = (8.0F + 1.0F / f4) / 2.0F;
            GL11.glScalef(f6, f5, f6);
            GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
        }
    }
}