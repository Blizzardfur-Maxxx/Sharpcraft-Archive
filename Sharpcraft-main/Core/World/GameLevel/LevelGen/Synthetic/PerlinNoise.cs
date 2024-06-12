using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Synthetic
{
    public class PerlinNoise : Synth
    {
        private int[] permutations;
        public double xCoord;
        public double yCoord;
        public double zCoord;

        public PerlinNoise() : this(new JRandom())
        {
        }

        public PerlinNoise(JRandom random1)
        {
            permutations = new int[512];
            xCoord = random1.NextDouble() * 256.0D;
            yCoord = random1.NextDouble() * 256.0D;
            zCoord = random1.NextDouble() * 256.0D;

            int i2;
            for (i2 = 0; i2 < 256; permutations[i2] = i2++)
            {
            }

            for (i2 = 0; i2 < 256; ++i2)
            {
                int i3 = random1.NextInt(256 - i2) + i2;
                int i4 = permutations[i2];
                permutations[i2] = permutations[i3];
                permutations[i3] = i4;
                permutations[i2 + 256] = permutations[i2];
            }

        }

        public double GenerateNoise(double d1, double d3, double d5)
        {
            double d7 = d1 + xCoord;
            double d9 = d3 + yCoord;
            double d11 = d5 + zCoord;
            int i13 = (int)d7;
            int i14 = (int)d9;
            int i15 = (int)d11;
            if (d7 < i13)
            {
                --i13;
            }

            if (d9 < i14)
            {
                --i14;
            }

            if (d11 < i15)
            {
                --i15;
            }

            int i16 = i13 & 255;
            int i17 = i14 & 255;
            int i18 = i15 & 255;
            d7 -= i13;
            d9 -= i14;
            d11 -= i15;
            double d19 = d7 * d7 * d7 * (d7 * (d7 * 6.0D - 15.0D) + 10.0D);
            double d21 = d9 * d9 * d9 * (d9 * (d9 * 6.0D - 15.0D) + 10.0D);
            double d23 = d11 * d11 * d11 * (d11 * (d11 * 6.0D - 15.0D) + 10.0D);
            int i25 = permutations[i16] + i17;
            int i26 = permutations[i25] + i18;
            int i27 = permutations[i25 + 1] + i18;
            int i28 = permutations[i16 + 1] + i17;
            int i29 = permutations[i28] + i18;
            int i30 = permutations[i28 + 1] + i18;
            return this.Lerp(d23, this.Lerp(d21, this.Lerp(d19, this.Grad(this.permutations[i26], d7, d9, d11), this.Grad(this.permutations[i29], d7 - 1.0D, d9, d11)), this.Lerp(d19, this.Grad(this.permutations[i27], d7, d9 - 1.0D, d11), this.Grad(this.permutations[i30], d7 - 1.0D, d9 - 1.0D, d11))), this.Lerp(d21, this.Lerp(d19, this.Grad(this.permutations[i26 + 1], d7, d9, d11 - 1.0D), this.Grad(this.permutations[i29 + 1], d7 - 1.0D, d9, d11 - 1.0D)), this.Lerp(d19, this.Grad(this.permutations[i27 + 1], d7, d9 - 1.0D, d11 - 1.0D), this.Grad(this.permutations[i30 + 1], d7 - 1.0D, d9 - 1.0D, d11 - 1.0D))));
        }

        public double Lerp(double d1, double d3, double d5)
        {
            return d3 + d1 * (d5 - d3);
        }

        public double Tab(int i1, double d2, double d4)
        {
            int i6 = i1 & 15;
            double d7 = (1 - ((i6 & 8) >> 3)) * d2;
            double d9 = i6 < 4 ? 0.0D : (i6 != 12 && i6 != 14 ? d4 : d2);
            return ((i6 & 1) == 0 ? d7 : -d7) + ((i6 & 2) == 0 ? d9 : -d9);
        }

        public double Grad(int i1, double d2, double d4, double d6)
        {
            int i8 = i1 & 15;
            double d9 = i8 < 8 ? d2 : d4;
            double d11 = i8 < 4 ? d4 : (i8 != 12 && i8 != 14 ? d6 : d2);
            return ((i8 & 1) == 0 ? d9 : -d9) + ((i8 & 2) == 0 ? d11 : -d11);
        }

        public double GetValue(double d1, double d3)
        {
            return GenerateNoise(d1, d3, 0.0D);
        }

        public void GetRegion(double[] d1, double d2, double d4, double d6, int i8, int i9, int i10, double d11, double d13, double d15, double d17)
        {
            int i10001;
            int i19;
            int i22;
            double d31;
            double d35;
            int i37;
            double d38;
            int i40;
            int i41;
            double d42;
            int i75;
            if (i9 == 1)
            {
                double d70 = 0.0D;
                double d73 = 0.0D;
                i75 = 0;
                double d77 = 1.0D / d17;

                for (int i30 = 0; i30 < i8; ++i30)
                {
                    d31 = (d2 + i30) * d11 + this.xCoord;
                    int i78 = (int)d31;
                    if (d31 < i78)
                    {
                        --i78;
                    }

                    int i34 = i78 & 255;
                    d31 -= i78;
                    d35 = d31 * d31 * d31 * (d31 * (d31 * 6.0D - 15.0D) + 10.0D);

                    for (i37 = 0; i37 < i10; ++i37)
                    {
                        d38 = (d6 + i37) * d15 + this.zCoord;
                        i40 = (int)d38;
                        if (d38 < i40)
                        {
                            --i40;
                        }

                        i41 = i40 & 255;
                        d38 -= i40;
                        d42 = d38 * d38 * d38 * (d38 * (d38 * 6.0D - 15.0D) + 10.0D);
                        i19 = this.permutations[i34] + 0;
                        int i66 = this.permutations[i19] + i41;
                        int i67 = this.permutations[i34 + 1] + 0;
                        i22 = this.permutations[i67] + i41;
                        d70 = this.Lerp(d35, this.Tab(this.permutations[i66], d31, d38), this.Grad(this.permutations[i22], d31 - 1.0D, 0.0D, d38));
                        d73 = this.Lerp(d35, this.Grad(this.permutations[i66 + 1], d31, 0.0D, d38 - 1.0D), this.Grad(this.permutations[i22 + 1], d31 - 1.0D, 0.0D, d38 - 1.0D));
                        double d79 = this.Lerp(d42, d70, d73);
                        i10001 = i75++;
                        d1[i10001] += d79 * d77;
                    }
                }

            }
            else
            {
                i19 = 0;
                double d20 = 1.0D / d17;
                i22 = -1;
                double d29 = 0.0D;
                d31 = 0.0D;
                double d33 = 0.0D;
                d35 = 0.0D;

                for (i37 = 0; i37 < i8; ++i37)
                {
                    d38 = (d2 + i37) * d11 + this.xCoord;
                    i40 = (int)d38;
                    if (d38 < i40)
                    {
                        --i40;
                    }

                    i41 = i40 & 255;
                    d38 -= i40;
                    d42 = d38 * d38 * d38 * (d38 * (d38 * 6.0D - 15.0D) + 10.0D);

                    for (int i44 = 0; i44 < i10; ++i44)
                    {
                        double d45 = (d6 + i44) * d15 + this.zCoord;
                        int i47 = (int)d45;
                        if (d45 < i47)
                        {
                            --i47;
                        }

                        int i48 = i47 & 255;
                        d45 -= i47;
                        double d49 = d45 * d45 * d45 * (d45 * (d45 * 6.0D - 15.0D) + 10.0D);

                        for (int i51 = 0; i51 < i9; ++i51)
                        {
                            double d52 = (d4 + i51) * d13 + this.yCoord;
                            int i54 = (int)d52;
                            if (d52 < i54)
                            {
                                --i54;
                            }

                            int i55 = i54 & 255;
                            d52 -= i54;
                            double d56 = d52 * d52 * d52 * (d52 * (d52 * 6.0D - 15.0D) + 10.0D);
                            if (i51 == 0 || i55 != i22)
                            {
                                i22 = i55;
                                int i69 = this.permutations[i41] + i55;
                                int i71 = this.permutations[i69] + i48;
                                int i72 = this.permutations[i69 + 1] + i48;
                                int i74 = this.permutations[i41 + 1] + i55;
                                i75 = this.permutations[i74] + i48;
                                int i76 = this.permutations[i74 + 1] + i48;
                                d29 = this.Lerp(d42, this.Grad(this.permutations[i71], d38, d52, d45), this.Grad(this.permutations[i75], d38 - 1.0D, d52, d45));
                                d31 = this.Lerp(d42, this.Grad(this.permutations[i72], d38, d52 - 1.0D, d45), this.Grad(this.permutations[i76], d38 - 1.0D, d52 - 1.0D, d45));
                                d33 = this.Lerp(d42, this.Grad(this.permutations[i71 + 1], d38, d52, d45 - 1.0D), this.Grad(this.permutations[i75 + 1], d38 - 1.0D, d52, d45 - 1.0D));
                                d35 = this.Lerp(d42, this.Grad(this.permutations[i72 + 1], d38, d52 - 1.0D, d45 - 1.0D), this.Grad(this.permutations[i76 + 1], d38 - 1.0D, d52 - 1.0D, d45 - 1.0D));
                            }

                            double d58 = this.Lerp(d56, d29, d31);
                            double d60 = this.Lerp(d56, d33, d35);
                            double d62 = this.Lerp(d49, d58, d60);
                            i10001 = i19++;
                            d1[i10001] += d62 * d20;
                        }
                    }
                }

            }
        }
    }
}
