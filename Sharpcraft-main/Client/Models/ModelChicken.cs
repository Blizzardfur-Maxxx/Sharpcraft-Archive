using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelChicken : Model
    {
        public ModelRenderer head;
        public ModelRenderer body;
        public ModelRenderer rightLeg;
        public ModelRenderer leftLeg;
        public ModelRenderer rightWing;
        public ModelRenderer leftWing;
        public ModelRenderer bill;
        public ModelRenderer chin;

        public ModelChicken() 
        {
            byte b1 = 16;
            this.head = new ModelRenderer(0, 0);
            this.head.AddBox(-2.0F, -6.0F, -2.0F, 4, 6, 3, 0.0F);
            this.head.SetRotationPoint(0.0F, -1 + b1, -4.0F);
            this.bill = new ModelRenderer(14, 0);
            this.bill.AddBox(-2.0F, -4.0F, -4.0F, 4, 2, 2, 0.0F);
            this.bill.SetRotationPoint(0.0F, -1 + b1, -4.0F);
            this.chin = new ModelRenderer(14, 4);
            this.chin.AddBox(-1.0F, -2.0F, -3.0F, 2, 2, 2, 0.0F);
            this.chin.SetRotationPoint(0.0F, -1 + b1, -4.0F);
            this.body = new ModelRenderer(0, 9);
            this.body.AddBox(-3.0F, -4.0F, -3.0F, 6, 8, 6, 0.0F);
            this.body.SetRotationPoint(0.0F, 0 + b1, 0.0F);
            this.rightLeg = new ModelRenderer(26, 0);
            this.rightLeg.AddBox(-1.0F, 0.0F, -3.0F, 3, 5, 3);
            this.rightLeg.SetRotationPoint(-2.0F, 3 + b1, 1.0F);
            this.leftLeg = new ModelRenderer(26, 0);
            this.leftLeg.AddBox(-1.0F, 0.0F, -3.0F, 3, 5, 3);
            this.leftLeg.SetRotationPoint(1.0F, 3 + b1, 1.0F);
            this.rightWing = new ModelRenderer(24, 13);
            this.rightWing.AddBox(0.0F, 0.0F, -3.0F, 1, 4, 6);
            this.rightWing.SetRotationPoint(-4.0F, -3 + b1, 0.0F);
            this.leftWing = new ModelRenderer(24, 13);
            this.leftWing.AddBox(-1.0F, 0.0F, -3.0F, 1, 4, 6);
            this.leftWing.SetRotationPoint(4.0F, -3 + b1, 0.0F);
        }

        public override void Render(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            this.SetRotationAngles(f1, f2, f3, f4, f5, f6);
            this.head.Render(f6);
            this.bill.Render(f6);
            this.chin.Render(f6);
            this.body.Render(f6);
            this.rightLeg.Render(f6);
            this.leftLeg.Render(f6);
            this.rightWing.Render(f6);
            this.leftWing.Render(f6);
        }

        public override void SetRotationAngles(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            this.head.rotateAngleX = -(f5 / 57.295776F);
            this.head.rotateAngleY = f4 / 57.295776F;
            this.bill.rotateAngleX = this.head.rotateAngleX;
            this.bill.rotateAngleY = this.head.rotateAngleY;
            this.chin.rotateAngleX = this.head.rotateAngleX;
            this.chin.rotateAngleY = this.head.rotateAngleY;
            this.body.rotateAngleX = Mth.PI / 2F;
            this.rightLeg.rotateAngleX = Mth.Cos(f1 * 0.6662F) * 1.4F * f2;
            this.leftLeg.rotateAngleX = Mth.Cos(f1 * 0.6662F + Mth.PI) * 1.4F * f2;
            this.rightWing.rotateAngleZ = f3;
            this.leftWing.rotateAngleZ = -f3;
        }
    }
}
