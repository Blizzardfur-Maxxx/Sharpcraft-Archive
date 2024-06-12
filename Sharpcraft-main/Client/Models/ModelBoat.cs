using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelBoat : Model
    {
        public ModelRenderer[] boatSides = new ModelRenderer[5];
        public ModelBoat() 
        {
            this.boatSides[0] = new ModelRenderer(0, 8);
            this.boatSides[1] = new ModelRenderer(0, 0);
            this.boatSides[2] = new ModelRenderer(0, 0);
            this.boatSides[3] = new ModelRenderer(0, 0);
            this.boatSides[4] = new ModelRenderer(0, 0);
            byte b1 = 24;
            byte b2 = 6;
            byte b3 = 20;
            byte b4 = 4;
            this.boatSides[0].AddBox(-b1 / 2, -b3 / 2 + 2, -3.0F, b1, b3 - 4, 4, 0.0F);
            this.boatSides[0].SetRotationPoint(0.0F, 0 + b4, 0.0F);
            this.boatSides[1].AddBox(-b1 / 2 + 2, -b2 - 1, -1.0F, b1 - 4, b2, 2, 0.0F);
            this.boatSides[1].SetRotationPoint(-b1 / 2 + 1, 0 + b4, 0.0F);
            this.boatSides[2].AddBox(-b1 / 2 + 2, -b2 - 1, -1.0F, b1 - 4, b2, 2, 0.0F);
            this.boatSides[2].SetRotationPoint(b1 / 2 - 1, 0 + b4, 0.0F);
            this.boatSides[3].AddBox(-b1 / 2 + 2, -b2 - 1, -1.0F, b1 - 4, b2, 2, 0.0F);
            this.boatSides[3].SetRotationPoint(0.0F, 0 + b4, -b3 / 2 + 1);
            this.boatSides[4].AddBox(-b1 / 2 + 2, -b2 - 1, -1.0F, b1 - 4, b2, 2, 0.0F);
            this.boatSides[4].SetRotationPoint(0.0F, 0 + b4, b3 / 2 - 1);
            this.boatSides[0].rotateAngleX = Mth.PI / 2F;
            this.boatSides[1].rotateAngleY = 4.712389F;
            this.boatSides[2].rotateAngleY = Mth.PI / 2F;
            this.boatSides[3].rotateAngleY = Mth.PI;
        }

        public override void Render(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            for (int i7 = 0; i7 < 5; ++i7)
            {
                this.boatSides[i7].Render(f6);
            }

        }

        public override void SetRotationAngles(float f1, float f2, float f3, float f4, float f5, float f6)
        {
        }
    }
}
