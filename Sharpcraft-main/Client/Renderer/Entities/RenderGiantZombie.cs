using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities.Monsters;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderGiantZombie : RenderLiving<Giant>
    {
        private float scale;

        public RenderGiantZombie(Model modelBase1, float f2, float f3) : base(modelBase1, f2 * f3)
        {
            this.scale = f3;
        }

        protected override void PreRenderCallback(Giant entityLiving1, float f2)
        {
            GL11.glScalef(this.scale, this.scale, this.scale);
        }
    }
}