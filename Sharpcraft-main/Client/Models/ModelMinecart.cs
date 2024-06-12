using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelMinecart : Model
    {
        public ModelRenderer[] sideModels = new ModelRenderer[7];

        public ModelMinecart()
        {
            this.sideModels[0] = new ModelRenderer(0, 10);
            this.sideModels[1] = new ModelRenderer(0, 0);
            this.sideModels[2] = new ModelRenderer(0, 0);
            this.sideModels[3] = new ModelRenderer(0, 0);
            this.sideModels[4] = new ModelRenderer(0, 0);
            this.sideModels[5] = new ModelRenderer(44, 10);
            byte b1 = 20;
            byte b2 = 8;
            byte b3 = 16;
            byte b4 = 4;
            this.sideModels[0].AddBox(-b1 / 2, -b3 / 2, -1.0F, b1, b3, 2, 0.0F);
            this.sideModels[0].SetRotationPoint(0.0F, 0 + b4, 0.0F);
            this.sideModels[5].AddBox(-b1 / 2 + 1, -b3 / 2 + 1, -1.0F, b1 - 2, b3 - 2, 1, 0.0F);
            this.sideModels[5].SetRotationPoint(0.0F, 0 + b4, 0.0F);
            this.sideModels[1].AddBox(-b1 / 2 + 2, -b2 - 1, -1.0F, b1 - 4, b2, 2, 0.0F);
            this.sideModels[1].SetRotationPoint(-b1 / 2 + 1, 0 + b4, 0.0F);
            this.sideModels[2].AddBox(-b1 / 2 + 2, -b2 - 1, -1.0F, b1 - 4, b2, 2, 0.0F);
            this.sideModels[2].SetRotationPoint(b1 / 2 - 1, 0 + b4, 0.0F);
            this.sideModels[3].AddBox(-b1 / 2 + 2, -b2 - 1, -1.0F, b1 - 4, b2, 2, 0.0F);
            this.sideModels[3].SetRotationPoint(0.0F, 0 + b4, -b3 / 2 + 1);
            this.sideModels[4].AddBox(-b1 / 2 + 2, -b2 - 1, -1.0F, b1 - 4, b2, 2, 0.0F);
            this.sideModels[4].SetRotationPoint(0.0F, 0 + b4, b3 / 2 - 1);
            this.sideModels[0].rotateAngleX = Mth.PI / 2F;
            this.sideModels[1].rotateAngleY = 4.712389F;
            this.sideModels[2].rotateAngleY = Mth.PI / 2F;
            this.sideModels[3].rotateAngleY = Mth.PI;
            this.sideModels[5].rotateAngleX = -1.5707964F;
        }

        public override void Render(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            this.sideModels[5].rotationPointY = 4.0F - f3;

            for (int i7 = 0; i7 < 6; ++i7)
            {
                this.sideModels[i7].Render(f6);
            }

        }

        public override void SetRotationAngles(float f1, float f2, float f3, float f4, float f5, float f6)
        {
        }
    }
}
