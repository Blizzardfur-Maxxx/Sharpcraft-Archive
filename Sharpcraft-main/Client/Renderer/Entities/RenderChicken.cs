using SharpCraft.Client.Models;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderChicken : RenderLiving<Chicken>
    {
        public RenderChicken(Model modelBase1, float f2) : base(modelBase1, f2)
        {
        }

        protected override float Func_170_d(Chicken entityLiving1, float f2)
        {
            float f3 = entityLiving1.field_756e + (entityLiving1.field_752b - entityLiving1.field_756e) * f2;
            float f4 = entityLiving1.field_757d + (entityLiving1.destPos - entityLiving1.field_757d) * f2;
            return (Mth.Sin(f3) + 1.0F) * f4;
        }

        public override void DoRenderLiving(Chicken entityLiving1, double d2, double d4, double d6, float f8, float f9)
        {
            base.DoRenderLiving(entityLiving1, d2, d4, d6, f8, f9);
        }

        public override void DoRender(Chicken entity1, double d2, double d4, double d6, float f8, float f9)
        {
            base.DoRenderLiving(entity1, d2, d4, d6, f8, f9);
        }
    }
}