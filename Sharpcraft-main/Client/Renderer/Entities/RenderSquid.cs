using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities.Animals;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderSquid : RenderLiving<Squid>
    {
        public RenderSquid(Model modelBase1, float f2) : base(modelBase1, f2)
        {
        }

        protected override void PreRenderCallback(Squid entityLiving1, float f2)
        {
        }

        protected override float Func_170_d(Squid entityLiving1, float f2)
        {
            float f3 = entityLiving1.field_21082 + (entityLiving1.field_21083 - entityLiving1.field_21082) * f2;
            return f3;
        }

        protected override void RotateCorpse(Squid entityLiving1, float f2, float f3, float f4)
        {
            float f5 = entityLiving1.field_21088 + (entityLiving1.field_21089 - entityLiving1.field_21088) * f4;
            float f6 = entityLiving1.field_21086 + (entityLiving1.field_21087 - entityLiving1.field_21086) * f4;
            GL11.glTranslatef(0.0F, 0.5F, 0.0F);
            GL11.glRotatef(180.0F - f3, 0.0F, 1.0F, 0.0F);
            GL11.glRotatef(f5, 1.0F, 0.0F, 0.0F);
            GL11.glRotatef(f6, 0.0F, 1.0F, 0.0F);
            GL11.glTranslatef(0.0F, -1.2F, 0.0F);
        }

        public override void DoRenderLiving(Squid a, double d2, double d4, double d6, float f8, float f9)
        {
            base.DoRenderLiving(a, d2, d4, d6, f8, f9);
        }

        public override void DoRender(Squid a, double d2, double d4, double d6, float f8, float f9)
        {
            base.DoRenderLiving(a, d2, d4, d6, f8, f9);
        }
    }

}