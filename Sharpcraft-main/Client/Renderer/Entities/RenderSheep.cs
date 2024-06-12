using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderSheep : RenderLiving<Sheep>
    {
        public RenderSheep(Model modelBase1, Model modelBase2, float f3) : base(modelBase1, f3)
        {
            this.SetRenderPassModel(modelBase2);
        }

        protected override bool ShouldRenderPass(Sheep entityLiving1, int i2, float f3)
        {
            if (i2 == 0 && !entityLiving1.GetSheared())
            {
                this.LoadTexture("/mob/sheep_fur.png");
                float f4 = entityLiving1.GetEntityBrightness(f3);
                int i5 = entityLiving1.GetFleeceColor();
                GL11.glColor3f(f4 * Sheep.COLOR[i5][0], f4 * Sheep.COLOR[i5][1], f4 * Sheep.COLOR[i5][2]);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
