using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelZombie : ModelBiped
    {
        public ModelZombie() { }

        public override void SetRotationAngles(float limbSwing, float limbSwingAmount, float age, float headYaw, float headPitch, float deltaTime)
        {
            base.SetRotationAngles(limbSwing, limbSwingAmount, age, headYaw, headPitch, deltaTime);
            float f7 = Mth.Sin(this.OnGround * Mth.PI);
            float f8 = Mth.Sin((1.0F - (1.0F - this.OnGround) * (1.0F - this.OnGround)) * Mth.PI);
            this.bipedRightArm.rotateAngleZ = 0.0F;
            this.bipedLeftArm.rotateAngleZ = 0.0F;
            this.bipedRightArm.rotateAngleY = -(0.1F - f7 * 0.6F);
            this.bipedLeftArm.rotateAngleY = 0.1F - f7 * 0.6F;
            this.bipedRightArm.rotateAngleX = -1.5707964F;
            this.bipedLeftArm.rotateAngleX = -1.5707964F;
            this.bipedRightArm.rotateAngleX -= f7 * 1.2F - f8 * 0.4F;
            this.bipedLeftArm.rotateAngleX -= f7 * 1.2F - f8 * 0.4F;
            this.bipedRightArm.rotateAngleZ += Mth.Cos(age * 0.09F) * 0.05F + 0.05F;
            this.bipedLeftArm.rotateAngleZ -= Mth.Cos(age * 0.09F) * 0.05F + 0.05F;
            this.bipedRightArm.rotateAngleX += Mth.Sin(age * 0.067F) * 0.05F;
            this.bipedLeftArm.rotateAngleX -= Mth.Sin(age * 0.067F) * 0.05F;
        }
    }
}
