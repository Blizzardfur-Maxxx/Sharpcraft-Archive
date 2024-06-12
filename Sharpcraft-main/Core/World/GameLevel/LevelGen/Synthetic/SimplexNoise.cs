using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Synthetic
{
    public class SimplexNoise
    {
        //when you have to graduate 3 times
        private static readonly int[][] grad3 = new int[][] {
            new int[] {1, 1, 0}, new int[] {-1, 1, 0}, new int[] {1, -1, 0},
            new int[] { -1, -1, 0 }, new int[] { 1, 0, 1 }, new int[] { -1, 0, 1 },
            new int[] { 1, 0, -1 }, new int[] { -1, 0, -1 }, new int[] { 0, 1, 1 },
            new int[] { 0, -1, 1 }, new int[] { 0, 1, -1 }, new int[] { 0, -1, -1 }
        };
        private int[] p;
        public double xo;
        public double yo;
        public double zo;
        private const double SQRT_3 = 1.7320508075688772D; //pre-calculated Math.Sqrt(3.0D) since no constant expressions
        private const double F2 = 0.5D * (SQRT_3 - 1.0D);
        private const double G2 = (3.0D - SQRT_3) / 6.0D;

        public SimplexNoise() : this(new JRandom())
        {
        }

        public SimplexNoise(JRandom random1)
        {
            p = new int[512];
            xo = random1.NextDouble() * 256.0D;
            yo = random1.NextDouble() * 256.0D;
            zo = random1.NextDouble() * 256.0D;

            int i2;
            for (i2 = 0; i2 < 256; p[i2] = i2++)
            {
            }

            for (i2 = 0; i2 < 256; ++i2)
            {
                int i3 = random1.NextInt(256 - i2) + i2;
                int i4 = p[i2];
                p[i2] = p[i3];
                p[i3] = i4;
                p[i2 + 256] = p[i2];
            }
        }

        private static int Wrap(double d0)
        {
            return d0 > 0.0D ? (int)d0 : (int)d0 - 1;
        }

        private static double ProcessGrad(int[] i0, double d1, double d3)
        {
            return i0[0] * d1 + i0[1] * d3;
        }

        public void GetRegion(double[] d1, double d2, double d4, int i6, int i7, double d8, double d10, double d12)
        {
            int i14 = 0;

            for (int i15 = 0; i15 < i6; ++i15)
            {
                double d16 = (d2 + i15) * d8 + this.xo;

                for (int i18 = 0; i18 < i7; ++i18)
                {
                    double d19 = (d4 + i18) * d10 + this.yo;
                    double d27 = (d16 + d19) * F2;
                    int i29 = Wrap(d16 + d27);
                    int i30 = Wrap(d19 + d27);
                    double d31 = (i29 + i30) * G2;
                    double d33 = i29 - d31;
                    double d35 = i30 - d31;
                    double d37 = d16 - d33;
                    double d39 = d19 - d35;
                    byte b41;
                    byte b42;
                    if (d37 > d39)
                    {
                        b41 = 1;
                        b42 = 0;
                    }
                    else
                    {
                        b41 = 0;
                        b42 = 1;
                    }

                    double d43 = d37 - b41 + G2;
                    double d45 = d39 - b42 + G2;
                    double d47 = d37 - 1.0D + 2.0D * G2;
                    double d49 = d39 - 1.0D + 2.0D * G2;
                    int i51 = i29 & 255;
                    int i52 = i30 & 255;
                    int i53 = this.p[i51 + this.p[i52]] % 12;
                    int i54 = this.p[i51 + b41 + this.p[i52 + b42]] % 12;
                    int i55 = this.p[i51 + 1 + this.p[i52 + 1]] % 12;
                    double d56 = 0.5D - d37 * d37 - d39 * d39;
                    double d21;
                    if (d56 < 0.0D)
                    {
                        d21 = 0.0D;
                    }
                    else
                    {
                        d56 *= d56;
                        d21 = d56 * d56 * ProcessGrad(grad3[i53], d37, d39);
                    }

                    double d58 = 0.5D - d43 * d43 - d45 * d45;
                    double d23;
                    if (d58 < 0.0D)
                    {
                        d23 = 0.0D;
                    }
                    else
                    {
                        d58 *= d58;
                        d23 = d58 * d58 * ProcessGrad(grad3[i54], d43, d45);
                    }

                    double d60 = 0.5D - d47 * d47 - d49 * d49;
                    double d25;
                    if (d60 < 0.0D)
                    {
                        d25 = 0.0D;
                    }
                    else
                    {
                        d60 *= d60;
                        d25 = d60 * d60 * ProcessGrad(grad3[i55], d47, d49);
                    }

                    int i10001 = i14++;
                    d1[i10001] += 70.0D * (d21 + d23 + d25) * d12;
                }
            }

        }
    }
}
