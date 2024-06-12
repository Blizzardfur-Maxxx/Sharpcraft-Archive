using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelSquid : Model
    {
        ModelRenderer squidBody;
        ModelRenderer[] squidTentacles = new ModelRenderer[8];

        public ModelSquid() 
        {
            sbyte b1 = -16;
            this.squidBody = new ModelRenderer(0, 0);
            this.squidBody.AddBox(-6.0F, -8.0F, -6.0F, 12, 16, 12);
            this.squidBody.rotationPointY += 24 + b1;

            for (int i2 = 0; i2 < this.squidTentacles.Length; ++i2)
            {
                this.squidTentacles[i2] = new ModelRenderer(48, 0);
                double d3 = i2 * Math.PI * 2.0D / this.squidTentacles.Length;
                float f5 = (float)Math.Cos(d3) * 5.0F;
                float f6 = (float)Math.Sin(d3) * 5.0F;
                this.squidTentacles[i2].AddBox(-1.0F, 0.0F, -1.0F, 2, 18, 2);
                this.squidTentacles[i2].rotationPointX = f5;
                this.squidTentacles[i2].rotationPointZ = f6;
                this.squidTentacles[i2].rotationPointY = 31 + b1;
                d3 = i2 * Math.PI * -2.0D / this.squidTentacles.Length + Math.PI / 2D;
                this.squidTentacles[i2].rotateAngleY = (float)d3;
            }
        }

        public override void SetRotationAngles(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            for (int i7 = 0; i7 < this.squidTentacles.Length; ++i7)
            {
                this.squidTentacles[i7].rotateAngleX = f3;
            }
        }

        public override void Render(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            this.SetRotationAngles(f1, f2, f3, f4, f5, f6);
            this.squidBody.Render(f6);

            for (int i7 = 0; i7 < this.squidTentacles.Length; ++i7)
            {
                this.squidTentacles[i7].Render(f6);
            }
        }
    }
}
