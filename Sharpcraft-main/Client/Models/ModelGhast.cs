using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelGhast : Model
    {
        ModelRenderer body;
        ModelRenderer[] tentacles = new ModelRenderer[9];

        public ModelGhast()
        {
            sbyte b1 = -16;
            this.body = new ModelRenderer(0, 0);
            this.body.AddBox(-8.0F, -8.0F, -8.0F, 16, 16, 16);
            this.body.rotationPointY += 24 + b1;
            JRandom random2 = new JRandom(1660L);

            for (int i3 = 0; i3 < this.tentacles.Length; ++i3)
            {
                this.tentacles[i3] = new ModelRenderer(0, 0);
                float f4 = ((i3 % 3 - i3 / 3 % 2 * 0.5F + 0.25F) / 2.0F * 2.0F - 1.0F) * 5.0F;
                float f5 = (i3 / 3 / 2.0F * 2.0F - 1.0F) * 5.0F;
                int i6 = random2.NextInt(7) + 8;
                this.tentacles[i3].AddBox(-1.0F, 0.0F, -1.0F, 2, i6, 2);
                this.tentacles[i3].rotationPointX = f4;
                this.tentacles[i3].rotationPointZ = f5;
                this.tentacles[i3].rotationPointY = 31 + b1;
            }

        }

        public override void SetRotationAngles(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            for (int i7 = 0; i7 < this.tentacles.Length; ++i7)
            {
                this.tentacles[i7].rotateAngleX = 0.2F * Mth.Sin(f3 * 0.3F + i7) + 0.4F;
            }

        }

        public override void Render(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            this.SetRotationAngles(f1, f2, f3, f4, f5, f6);
            this.body.Render(f6);

            for (int i7 = 0; i7 < this.tentacles.Length; ++i7)
            {
                this.tentacles[i7].Render(f6);
            }

        }
    }
}
