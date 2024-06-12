using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelCreeper : Model
    {
        public ModelRenderer head;
        public ModelRenderer field_1270_b;
        public ModelRenderer body;
        public ModelRenderer leg1;
        public ModelRenderer leg2;
        public ModelRenderer leg3;
        public ModelRenderer leg4;

        public ModelCreeper() : this(0.0F)
        {
        }

        public ModelCreeper(float f1)
        {
            byte b2 = 4;
            this.head = new ModelRenderer(0, 0);
            this.head.AddBox(-4.0F, -8.0F, -4.0F, 8, 8, 8, f1);
            this.head.SetRotationPoint(0.0F, b2, 0.0F);
            this.field_1270_b = new ModelRenderer(32, 0);
            this.field_1270_b.AddBox(-4.0F, -8.0F, -4.0F, 8, 8, 8, f1 + 0.5F);
            this.field_1270_b.SetRotationPoint(0.0F, b2, 0.0F);
            this.body = new ModelRenderer(16, 16);
            this.body.AddBox(-4.0F, 0.0F, -2.0F, 8, 12, 4, f1);
            this.body.SetRotationPoint(0.0F, b2, 0.0F);
            this.leg1 = new ModelRenderer(0, 16);
            this.leg1.AddBox(-2.0F, 0.0F, -2.0F, 4, 6, 4, f1);
            this.leg1.SetRotationPoint(-2.0F, 12 + b2, 4.0F);
            this.leg2 = new ModelRenderer(0, 16);
            this.leg2.AddBox(-2.0F, 0.0F, -2.0F, 4, 6, 4, f1);
            this.leg2.SetRotationPoint(2.0F, 12 + b2, 4.0F);
            this.leg3 = new ModelRenderer(0, 16);
            this.leg3.AddBox(-2.0F, 0.0F, -2.0F, 4, 6, 4, f1);
            this.leg3.SetRotationPoint(-2.0F, 12 + b2, -4.0F);
            this.leg4 = new ModelRenderer(0, 16);
            this.leg4.AddBox(-2.0F, 0.0F, -2.0F, 4, 6, 4, f1);
            this.leg4.SetRotationPoint(2.0F, 12 + b2, -4.0F);
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
            this.head.rotateAngleY = f4 / 57.295776F;
            this.head.rotateAngleX = f5 / 57.295776F;
            this.leg1.rotateAngleX = Mth.Cos(f1 * 0.6662F) * 1.4F * f2;
            this.leg2.rotateAngleX = Mth.Cos(f1 * 0.6662F + Mth.PI) * 1.4F * f2;
            this.leg3.rotateAngleX = Mth.Cos(f1 * 0.6662F + Mth.PI) * 1.4F * f2;
            this.leg4.rotateAngleX = Mth.Cos(f1 * 0.6662F) * 1.4F * f2;
        }
    }
}
