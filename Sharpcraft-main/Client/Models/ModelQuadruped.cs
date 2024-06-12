using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelQuadruped : Model
    {
        public ModelRenderer head = new ModelRenderer(0, 0);
        public ModelRenderer body;
        public ModelRenderer leg1;
        public ModelRenderer leg2;
        public ModelRenderer leg3;
        public ModelRenderer leg4;

        public ModelQuadruped(int i1, float f2)
        {
            this.head.AddBox(-4.0F, -4.0F, -8.0F, 8, 8, 8, f2);
            this.head.SetRotationPoint(0.0F, 18 - i1, -6.0F);
            this.body = new ModelRenderer(28, 8);
            this.body.AddBox(-5.0F, -10.0F, -7.0F, 10, 16, 8, f2);
            this.body.SetRotationPoint(0.0F, 17 - i1, 2.0F);
            this.leg1 = new ModelRenderer(0, 16);
            this.leg1.AddBox(-2.0F, 0.0F, -2.0F, 4, i1, 4, f2);
            this.leg1.SetRotationPoint(-3.0F, 24 - i1, 7.0F);
            this.leg2 = new ModelRenderer(0, 16);
            this.leg2.AddBox(-2.0F, 0.0F, -2.0F, 4, i1, 4, f2);
            this.leg2.SetRotationPoint(3.0F, 24 - i1, 7.0F);
            this.leg3 = new ModelRenderer(0, 16);
            this.leg3.AddBox(-2.0F, 0.0F, -2.0F, 4, i1, 4, f2);
            this.leg3.SetRotationPoint(-3.0F, 24 - i1, -5.0F);
            this.leg4 = new ModelRenderer(0, 16);
            this.leg4.AddBox(-2.0F, 0.0F, -2.0F, 4, i1, 4, f2);
            this.leg4.SetRotationPoint(3.0F, 24 - i1, -5.0F);
        }

        public override void Render(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            this.SetRotationAngles(f1, f2, f3, f4, f5, f6);
            this.head.Render(f6);
            this.body.Render(f6);
            this.leg1.Render(f6);
            this.leg2.Render(f6);
            this.leg3.Render(f6);
            this.leg4.Render(f6);
        }

        public override void SetRotationAngles(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            this.head.rotateAngleX = f5 / 57.295776F;
            this.head.rotateAngleY = f4 / 57.295776F;
            this.body.rotateAngleX = Mth.PI / 2F;
            this.leg1.rotateAngleX = Mth.Cos(f1 * 0.6662F) * 1.4F * f2;
            this.leg2.rotateAngleX = Mth.Cos(f1 * 0.6662F + Mth.PI) * 1.4F * f2;
            this.leg3.rotateAngleX = Mth.Cos(f1 * 0.6662F + Mth.PI) * 1.4F * f2;
            this.leg4.rotateAngleX = Mth.Cos(f1 * 0.6662F) * 1.4F * f2;
        }
    }
}
