using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderCow : RenderLiving<Cow>
    {
        public RenderCow(Model modelBase1, float f2) : base(modelBase1, f2)
        {
        }

        public override void DoRenderLiving(Cow a, double d2, double d4, double d6, float f8, float f9)
        {
            base.DoRenderLiving(a, d2, d4, d6, f8, f9);
        }

        public override void DoRender(Cow a, double d2, double d4, double d6, float f8, float f9)
        {
            base.DoRenderLiving(a, d2, d4, d6, f8, f9);
        }
    }
}
