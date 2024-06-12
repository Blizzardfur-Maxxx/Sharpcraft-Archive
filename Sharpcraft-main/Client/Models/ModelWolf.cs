using Microsoft.VisualBasic.ApplicationServices;
using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelWolf : Model
    {
        public ModelRenderer wolfHeadMain;
        public ModelRenderer wolfBody;
        public ModelRenderer wolfLeg1;
        public ModelRenderer wolfLeg2;
        public ModelRenderer wolfLeg3;
        public ModelRenderer wolfLeg4;
        ModelRenderer wolfRightEar;
        ModelRenderer wolfLeftEar;
        ModelRenderer wolfSnout;
        ModelRenderer wolfTail;
        ModelRenderer wolfMane;

        public ModelWolf() 
        {
            float f1 = 0.0F;
            float f2 = 13.5F;
            this.wolfHeadMain = new ModelRenderer(0, 0);
            this.wolfHeadMain.AddBox(-3.0F, -3.0F, -2.0F, 6, 6, 4, f1);
            this.wolfHeadMain.SetRotationPoint(-1.0F, f2, -7.0F);
            this.wolfBody = new ModelRenderer(18, 14);
            this.wolfBody.AddBox(-4.0F, -2.0F, -3.0F, 6, 9, 6, f1);
            this.wolfBody.SetRotationPoint(0.0F, 14.0F, 2.0F);
            this.wolfMane = new ModelRenderer(21, 0);
            this.wolfMane.AddBox(-4.0F, -3.0F, -3.0F, 8, 6, 7, f1);
            this.wolfMane.SetRotationPoint(-1.0F, 14.0F, 2.0F);
            this.wolfLeg1 = new ModelRenderer(0, 18);
            this.wolfLeg1.AddBox(-1.0F, 0.0F, -1.0F, 2, 8, 2, f1);
            this.wolfLeg1.SetRotationPoint(-2.5F, 16.0F, 7.0F);
            this.wolfLeg2 = new ModelRenderer(0, 18);
            this.wolfLeg2.AddBox(-1.0F, 0.0F, -1.0F, 2, 8, 2, f1);
            this.wolfLeg2.SetRotationPoint(0.5F, 16.0F, 7.0F);
            this.wolfLeg3 = new ModelRenderer(0, 18);
            this.wolfLeg3.AddBox(-1.0F, 0.0F, -1.0F, 2, 8, 2, f1);
            this.wolfLeg3.SetRotationPoint(-2.5F, 16.0F, -4.0F);
            this.wolfLeg4 = new ModelRenderer(0, 18);
            this.wolfLeg4.AddBox(-1.0F, 0.0F, -1.0F, 2, 8, 2, f1);
            this.wolfLeg4.SetRotationPoint(0.5F, 16.0F, -4.0F);
            this.wolfTail = new ModelRenderer(9, 18);
            this.wolfTail.AddBox(-1.0F, 0.0F, -1.0F, 2, 8, 2, f1);
            this.wolfTail.SetRotationPoint(-1.0F, 12.0F, 8.0F);
            this.wolfRightEar = new ModelRenderer(16, 14);
            this.wolfRightEar.AddBox(-3.0F, -5.0F, 0.0F, 2, 2, 1, f1);
            this.wolfRightEar.SetRotationPoint(-1.0F, f2, -7.0F);
            this.wolfLeftEar = new ModelRenderer(16, 14);
            this.wolfLeftEar.AddBox(1.0F, -5.0F, 0.0F, 2, 2, 1, f1);
            this.wolfLeftEar.SetRotationPoint(-1.0F, f2, -7.0F);
            this.wolfSnout = new ModelRenderer(0, 10);
            this.wolfSnout.AddBox(-2.0F, 0.0F, -5.0F, 3, 3, 4, f1);
            this.wolfSnout.SetRotationPoint(-0.5F, f2, -7.0F);
        }

        public override void Render(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            base.Render(f1, f2, f3, f4, f5, f6);
            this.SetRotationAngles(f1, f2, f3, f4, f5, f6);
            this.wolfHeadMain.RenderWithRotation(f6);
            this.wolfBody.Render(f6);
            this.wolfLeg1.Render(f6);
            this.wolfLeg2.Render(f6);
            this.wolfLeg3.Render(f6);
            this.wolfLeg4.Render(f6);
            this.wolfRightEar.RenderWithRotation(f6);
            this.wolfLeftEar.RenderWithRotation(f6);
            this.wolfSnout.RenderWithRotation(f6);
            this.wolfTail.RenderWithRotation(f6);
            this.wolfMane.Render(f6);
        }


        public override void SetLivingAnimations(Mob entityLiving1, float f2, float f3, float f4)
        {
            Wolf entityWolf5 = (Wolf)entityLiving1;
            if (entityWolf5.IsWolfAngry())
            {
                this.wolfTail.rotateAngleY = 0.0F;
            }
            else
            {
                this.wolfTail.rotateAngleY = Mth.Cos(f2 * 0.6662F) * 1.4F * f3;
            }

            if (entityWolf5.IsSitting())
            {
                this.wolfMane.SetRotationPoint(-1.0F, 16.0F, -3.0F);
                this.wolfMane.rotateAngleX = 1.2566371F;
                this.wolfMane.rotateAngleY = 0.0F;
                this.wolfBody.SetRotationPoint(0.0F, 18.0F, 0.0F);
                this.wolfBody.rotateAngleX = 0.7853982F;
                this.wolfTail.SetRotationPoint(-1.0F, 21.0F, 6.0F);
                this.wolfLeg1.SetRotationPoint(-2.5F, 22.0F, 2.0F);
                this.wolfLeg1.rotateAngleX = 4.712389F;
                this.wolfLeg2.SetRotationPoint(0.5F, 22.0F, 2.0F);
                this.wolfLeg2.rotateAngleX = 4.712389F;
                this.wolfLeg3.rotateAngleX = 5.811947F;
                this.wolfLeg3.SetRotationPoint(-2.49F, 17.0F, -4.0F);
                this.wolfLeg4.rotateAngleX = 5.811947F;
                this.wolfLeg4.SetRotationPoint(0.51F, 17.0F, -4.0F);
            }
            else
            {
                this.wolfBody.SetRotationPoint(0.0F, 14.0F, 2.0F);
                this.wolfBody.rotateAngleX = Mth.PI / 2F;
                this.wolfMane.SetRotationPoint(-1.0F, 14.0F, -3.0F);
                this.wolfMane.rotateAngleX = this.wolfBody.rotateAngleX;
                this.wolfTail.SetRotationPoint(-1.0F, 12.0F, 8.0F);
                this.wolfLeg1.SetRotationPoint(-2.5F, 16.0F, 7.0F);
                this.wolfLeg2.SetRotationPoint(0.5F, 16.0F, 7.0F);
                this.wolfLeg3.SetRotationPoint(-2.5F, 16.0F, -4.0F);
                this.wolfLeg4.SetRotationPoint(0.5F, 16.0F, -4.0F);
                this.wolfLeg1.rotateAngleX = Mth.Cos(f2 * 0.6662F) * 1.4F * f3;
                this.wolfLeg2.rotateAngleX = Mth.Cos(f2 * 0.6662F + Mth.PI) * 1.4F * f3;
                this.wolfLeg3.rotateAngleX = Mth.Cos(f2 * 0.6662F + Mth.PI) * 1.4F * f3;
                this.wolfLeg4.rotateAngleX = Mth.Cos(f2 * 0.6662F) * 1.4F * f3;
            }

            float f6 = entityWolf5.GetInterestedAngle(f4) + entityWolf5.GetShakeAngle(f4, 0.0F);
            this.wolfHeadMain.rotateAngleZ = f6;
            this.wolfRightEar.rotateAngleZ = f6;
            this.wolfLeftEar.rotateAngleZ = f6;
            this.wolfSnout.rotateAngleZ = f6;
            this.wolfMane.rotateAngleZ = entityWolf5.GetShakeAngle(f4, -0.08F);
            this.wolfBody.rotateAngleZ = entityWolf5.GetShakeAngle(f4, -0.16F);
            this.wolfTail.rotateAngleZ = entityWolf5.GetShakeAngle(f4, -0.2F);
            if (entityWolf5.GetWolfShaking())
            {
                float f7 = entityWolf5.GetEntityBrightness(f4) * entityWolf5.GetShadingWhileShaking(f4);
                GL11.glColor3f(f7, f7, f7);
            }

        }

        public override void SetRotationAngles(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            base.SetRotationAngles(f1, f2, f3, f4, f5, f6);
            this.wolfHeadMain.rotateAngleX = f5 / 57.295776F;
            this.wolfHeadMain.rotateAngleY = f4 / 57.295776F;
            this.wolfRightEar.rotateAngleY = this.wolfHeadMain.rotateAngleY;
            this.wolfRightEar.rotateAngleX = this.wolfHeadMain.rotateAngleX;
            this.wolfLeftEar.rotateAngleY = this.wolfHeadMain.rotateAngleY;
            this.wolfLeftEar.rotateAngleX = this.wolfHeadMain.rotateAngleX;
            this.wolfSnout.rotateAngleY = this.wolfHeadMain.rotateAngleY;
            this.wolfSnout.rotateAngleX = this.wolfHeadMain.rotateAngleX;
            this.wolfTail.rotateAngleX = f3;
        }
    }
}
