using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelBiped : Model
    {
        public ModelRenderer bipedHead;
        public ModelRenderer bipedHeadwear;
        public ModelRenderer bipedBody;
        public ModelRenderer bipedRightArm;
        public ModelRenderer bipedLeftArm;
        public ModelRenderer bipedRightLeg;
        public ModelRenderer bipedLeftLeg;
        public ModelRenderer bipedEars;
        public ModelRenderer bipedCloak;
        public bool field_1279_h;
        public bool field_1278_i;
        public bool isSneak;

        public ModelBiped() : this(0F)
        {
        }

        public ModelBiped(float f1) : this(f1, 0F)
        {
        }

        public ModelBiped(float f1, float f2)
        {
            field_1279_h = false;
            field_1278_i = false;
            isSneak = false;
            bipedCloak = new ModelRenderer(0, 0);
            bipedCloak.AddBox(-5F, 0F, -1F, 10, 16, 1, f1);
            bipedEars = new ModelRenderer(24, 0);
            bipedEars.AddBox(-3F, -6F, -1F, 6, 6, 1, f1);
            bipedHead = new ModelRenderer(0, 0);
            bipedHead.AddBox(-4F, -8F, -4F, 8, 8, 8, f1);
            bipedHead.SetRotationPoint(0F, 0F + f2, 0F);
            bipedHeadwear = new ModelRenderer(32, 0);
            bipedHeadwear.AddBox(-4F, -8F, -4F, 8, 8, 8, f1 + 0.5F);
            bipedHeadwear.SetRotationPoint(0F, 0F + f2, 0F);
            bipedBody = new ModelRenderer(16, 16);
            bipedBody.AddBox(-4F, 0F, -2F, 8, 12, 4, f1);
            bipedBody.SetRotationPoint(0F, 0F + f2, 0F);
            bipedRightArm = new ModelRenderer(40, 16);
            bipedRightArm.AddBox(-3F, -2F, -2F, 4, 12, 4, f1);
            bipedRightArm.SetRotationPoint(-5F, 2F + f2, 0F);
            bipedLeftArm = new ModelRenderer(40, 16);
            bipedLeftArm.mirror = true;
            bipedLeftArm.AddBox(-1F, -2F, -2F, 4, 12, 4, f1);
            bipedLeftArm.SetRotationPoint(5F, 2F + f2, 0F);
            bipedRightLeg = new ModelRenderer(0, 16);
            bipedRightLeg.AddBox(-2F, 0F, -2F, 4, 12, 4, f1);
            bipedRightLeg.SetRotationPoint(-2F, 12F + f2, 0F);
            bipedLeftLeg = new ModelRenderer(0, 16);
            bipedLeftLeg.mirror = true;
            bipedLeftLeg.AddBox(-2F, 0F, -2F, 4, 12, 4, f1);
            bipedLeftLeg.SetRotationPoint(2F, 12F + f2, 0F);
        }

        public override void Render(float limbSwing, float limbSwingAmount, float age, float headYaw, float headPitch, float deltaTime)
        {
            SetRotationAngles(limbSwing, limbSwingAmount, age, headYaw, headPitch, deltaTime);
            bipedHead.Render(deltaTime);
            bipedBody.Render(deltaTime);
            bipedRightArm.Render(deltaTime);
            bipedLeftArm.Render(deltaTime);
            bipedRightLeg.Render(deltaTime);
            bipedLeftLeg.Render(deltaTime);
            bipedHeadwear.Render(deltaTime);
        }

        public override void SetRotationAngles(float limbSwing, float limbSwingAmount, float age, float headYaw, float headPitch, float deltaTime)
        {
            bipedHead.rotateAngleY = headYaw / 57.295776F;
            bipedHead.rotateAngleX = headPitch / 57.295776F;
            bipedHeadwear.rotateAngleY = bipedHead.rotateAngleY;
            bipedHeadwear.rotateAngleX = bipedHead.rotateAngleX;
            bipedRightArm.rotateAngleX = Mth.Cos(limbSwing * 0.6662F + Mth.PI) * 2F * limbSwingAmount * 0.5F;
            bipedLeftArm.rotateAngleX = Mth.Cos(limbSwing * 0.6662F) * 2F * limbSwingAmount * 0.5F;
            bipedRightArm.rotateAngleZ = 0F;
            bipedLeftArm.rotateAngleZ = 0F;
            bipedRightLeg.rotateAngleX = Mth.Cos(limbSwing * 0.6662F) * 1.4F * limbSwingAmount;
            bipedLeftLeg.rotateAngleX = Mth.Cos(limbSwing * 0.6662F + Mth.PI) * 1.4F * limbSwingAmount;
            bipedRightLeg.rotateAngleY = 0F;
            bipedLeftLeg.rotateAngleY = 0F;

            if (IsRiding)
            {
                bipedRightArm.rotateAngleX += -0.62831855F;
                bipedLeftArm.rotateAngleX += -0.62831855F;
                bipedRightLeg.rotateAngleX = -1.2566371F;
                bipedLeftLeg.rotateAngleX = -1.2566371F;
                bipedRightLeg.rotateAngleY = 0.31415927F;
                bipedLeftLeg.rotateAngleY = -0.31415927F;
            }

            if (field_1279_h)
            {
                bipedLeftArm.rotateAngleX = bipedLeftArm.rotateAngleX * 0.5F - 0.31415927F;
            }

            if (field_1278_i)
            {
                bipedRightArm.rotateAngleX = bipedRightArm.rotateAngleX * 0.5F - 0.31415927F;
            }

            bipedRightArm.rotateAngleY = 0F;
            bipedLeftArm.rotateAngleY = 0F;
            if (OnGround > -9990F)
            {
                float f7 = OnGround;
                bipedBody.rotateAngleY = Mth.Sin(Mth.Sqrt(f7) * Mth.PI * 2F) * 0.2F;
                bipedRightArm.rotationPointZ = Mth.Sin(bipedBody.rotateAngleY) * 5F;
                bipedRightArm.rotationPointX = -Mth.Cos(bipedBody.rotateAngleY) * 5F;
                bipedLeftArm.rotationPointZ = -Mth.Sin(bipedBody.rotateAngleY) * 5F;
                bipedLeftArm.rotationPointX = Mth.Cos(bipedBody.rotateAngleY) * 5F;
                bipedRightArm.rotateAngleY += bipedBody.rotateAngleY;
                bipedLeftArm.rotateAngleY += bipedBody.rotateAngleY;
                bipedLeftArm.rotateAngleX += bipedBody.rotateAngleY;
                f7 = 1F - OnGround;
                f7 *= f7;
                f7 *= f7;
                f7 = 1F - f7;
                float f8 = Mth.Sin(f7 * Mth.PI);
                float f9 = Mth.Sin(OnGround * Mth.PI) * -(bipedHead.rotateAngleX - 0.7F) * 0.75F;
                bipedRightArm.rotateAngleX = (float)(bipedRightArm.rotateAngleX - (f8 * 1.2 + f9));
                bipedRightArm.rotateAngleY += bipedBody.rotateAngleY * 2F;
                bipedRightArm.rotateAngleZ = Mth.Sin(OnGround * Mth.PI) * -0.4F;
            }

            if (isSneak)
            {
                bipedBody.rotateAngleX = 0.5F;
                bipedRightLeg.rotateAngleX -= 0F;
                bipedLeftLeg.rotateAngleX -= 0F;
                bipedRightArm.rotateAngleX += 0.4F;
                bipedLeftArm.rotateAngleX += 0.4F;
                bipedRightLeg.rotationPointZ = 4F;
                bipedLeftLeg.rotationPointZ = 4F;
                bipedRightLeg.rotationPointY = 9F;
                bipedLeftLeg.rotationPointY = 9F;
                bipedHead.rotationPointY = 1F;
            }
            else
            {
                bipedBody.rotateAngleX = 0F;
                bipedRightLeg.rotationPointZ = 0F;
                bipedLeftLeg.rotationPointZ = 0F;
                bipedRightLeg.rotationPointY = 12F;
                bipedLeftLeg.rotationPointY = 12F;
                bipedHead.rotationPointY = 0F;
            }

            bipedRightArm.rotateAngleZ += Mth.Cos(age * 0.09F) * 0.05F + 0.05F;
            bipedLeftArm.rotateAngleZ -= Mth.Cos(age * 0.09F) * 0.05F + 0.05F;
            bipedRightArm.rotateAngleX += Mth.Sin(age * 0.067F) * 0.05F;
            bipedLeftArm.rotateAngleX -= Mth.Sin(age * 0.067F) * 0.05F;
        }

        public virtual void RenderEars(float f1)
        {
            bipedEars.rotateAngleY = bipedHead.rotateAngleY;
            bipedEars.rotateAngleX = bipedHead.rotateAngleX;
            bipedEars.rotationPointX = 0F;
            bipedEars.rotationPointY = 0F;
            bipedEars.Render(f1);
        }

        public virtual void RenderCloak(float f1)
        {
            bipedCloak.Render(f1);
        }
    }
}
