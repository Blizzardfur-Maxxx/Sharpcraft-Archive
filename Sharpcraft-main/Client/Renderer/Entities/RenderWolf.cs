using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities.Animals;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderWolf : RenderLiving<Wolf>
    {
        public RenderWolf(Model modelBase1, float f2) : base(modelBase1, f2)
        {
        }

        protected override void PreRenderCallback(Wolf entityLiving1, float f2)
        {
        }

        protected override float Func_170_d(Wolf entityLiving1, float f2)
        {
            return entityLiving1.SetTailRotation();
        }

        public override void DoRenderLiving(Wolf a, double d2, double d4, double d6, float f8, float f9)
        {
            base.DoRenderLiving(a, d2, d4, d6, f8, f9);
        }

        public override void DoRender(Wolf a, double d2, double d4, double d6, float f8, float f9)
        {
            base.DoRenderLiving(a, d2, d4, d6, f8, f9);
        }
    }
}