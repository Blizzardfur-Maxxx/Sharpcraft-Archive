using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Ptexture
{
    public class FireTexture : DynamicTexture
    {
        protected float[] field_1133_g = new float[320];
        protected float[] field_1132_h = new float[320];

        public FireTexture(int i1) : base(Tile.fire.texture + i1 * 16)
        {

        }

        public override void OnTick()
        {
            int i2;
            float f4;
            int i5;
            int i6;
            for (int i1 = 0; i1 < 16; ++i1)
            {
                for (i2 = 0; i2 < 20; ++i2)
                {
                    int i3 = 18;
                    f4 = this.field_1133_g[i1 + (i2 + 1) % 20 * 16] * i3;

                    for (i5 = i1 - 1; i5 <= i1 + 1; ++i5)
                    {
                        for (i6 = i2; i6 <= i2 + 1; ++i6)
                        {
                            if (i5 >= 0 && i6 >= 0 && i5 < 16 && i6 < 20)
                            {
                                f4 += this.field_1133_g[i5 + i6 * 16];
                            }

                            ++i3;
                        }
                    }

                    this.field_1132_h[i1 + i2 * 16] = f4 / (i3 * 1.06F);
                    if (i2 >= 19)
                    {
                        this.field_1132_h[i1 + i2 * 16] = (float)(Mth.Random() * Mth.Random() * Mth.Random() * 4.0D + Mth.Random() * 0.1F + 0.2F);
                    }
                }
            }

            float[] f12 = this.field_1132_h;
            this.field_1132_h = this.field_1133_g;
            this.field_1133_g = f12;

            for (i2 = 0; i2 < 256; ++i2)
            {
                float f13 = this.field_1133_g[i2] * 1.8F;
                if (f13 > 1.0F)
                {
                    f13 = 1.0F;
                }

                if (f13 < 0.0F)
                {
                    f13 = 0.0F;
                }

                i5 = (int)(f13 * 155.0F + 100.0F);
                i6 = (int)(f13 * f13 * 255.0F);
                int i7 = (int)(f13 * f13 * f13 * f13 * f13 * f13 * f13 * f13 * f13 * f13 * 255.0F);
                short s8 = 255;
                if (f13 < 0.5F)
                {
                    s8 = 0;
                }

                f4 = (f13 - 0.5F) * 2.0F;
                if (this.anaglyphEnabled)
                {
                    int i9 = (i5 * 30 + i6 * 59 + i7 * 11) / 100;
                    int i10 = (i5 * 30 + i6 * 70) / 100;
                    int i11 = (i5 * 30 + i7 * 70) / 100;
                    i5 = i9;
                    i6 = i10;
                    i7 = i11;
                }

                this.imageData[i2 * 4 + 0] = (byte)i5;
                this.imageData[i2 * 4 + 1] = (byte)i6;
                this.imageData[i2 * 4 + 2] = (byte)i7;
                this.imageData[i2 * 4 + 3] = (byte)s8;
            }

        }
    }

}
