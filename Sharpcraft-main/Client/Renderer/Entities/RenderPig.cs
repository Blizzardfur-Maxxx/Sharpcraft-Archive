using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderPig : RenderLiving<Pig>
    {
        public RenderPig(Model modelBase1, Model modelBase2, float f3) : base(modelBase1, f3)
        {
            this.SetRenderPassModel(modelBase2);
        }

        protected override bool ShouldRenderPass(Pig entityLiving1, int i2, float f3)
        {
            this.LoadTexture("/mob/saddle.png");
            return i2 == 0 && entityLiving1.GetSaddled();
        }
    }
}
