using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class LargeCaveFeature : LargeFeature
    {
        protected virtual void AddRoom(int i1, int i2, byte[] b3, double d4, double d6, double d8)
        {
            this.AddTunnel(i1, i2, b3, d4, d6, d8, 1F + this.rand.NextFloat() * 6F, 0F, 0F, -1, -1, 0.5);
        }

        protected virtual void AddTunnel(int i1, int i2, byte[] b3, double d4, double d6, double d8, float f10, float f11, float f12, int i13, int i14, double d15)
        {
            double d17 = i1 * 16 + 8;
            double d19 = i2 * 16 + 8;
            float f21 = 0F;
            float f22 = 0F;
            JRandom random23 = new JRandom(this.rand.NextLong());
            if (i14 <= 0)
            {
                int i24 = this.range * 16 - 16;
                i14 = i24 - random23.NextInt(i24 / 4);
            }

            bool z52 = false;
            if (i13 == -1)
            {
                i13 = i14 / 2;
                z52 = true;
            }

            int i25 = random23.NextInt(i14 / 2) + i14 / 4;
            for (bool z26 = random23.NextInt(6) == 0; i13 < i14; ++i13)
            {
                double d27 = 1.5 + Mth.Sin(i13 * Mth.PI / i14) * f10 * 1F;
                double d29 = d27 * d15;
                float f31 = Mth.Cos(f12);
                float f32 = Mth.Sin(f12);
                d4 += Mth.Cos(f11) * f31;
                d6 += f32;
                d8 += Mth.Sin(f11) * f31;
                if (z26)
                {
                    f12 *= 0.92F;
                }
                else
                {
                    f12 *= 0.7F;
                }

                f12 += f22 * 0.1F;
                f11 += f21 * 0.1F;
                f22 *= 0.9F;
                f21 *= 0.75F;
                f22 += (random23.NextFloat() - random23.NextFloat()) * random23.NextFloat() * 2F;
                f21 += (random23.NextFloat() - random23.NextFloat()) * random23.NextFloat() * 4F;
                if (!z52 && i13 == i25 && f10 > 1F)
                {
                    this.AddTunnel(i1, i2, b3, d4, d6, d8, random23.NextFloat() * 0.5F + 0.5F, f11 - Mth.PI / 2F, f12 / 3F, i13, i14, 1);
                    this.AddTunnel(i1, i2, b3, d4, d6, d8, random23.NextFloat() * 0.5F + 0.5F, f11 + Mth.PI / 2F, f12 / 3F, i13, i14, 1);
                    return;
                }

                if (z52 || random23.NextInt(4) != 0)
                {
                    double d33 = d4 - d17;
                    double d35 = d8 - d19;
                    double d37 = i14 - i13;
                    double d39 = f10 + 2F + 16F;
                    if (d33 * d33 + d35 * d35 - d37 * d37 > d39 * d39)
                    {
                        return;
                    }

                    if (d4 >= d17 - 16 - d27 * 2 && d8 >= d19 - 16 - d27 * 2 && d4 <= d17 + 16 + d27 * 2 && d8 <= d19 + 16 + d27 * 2)
                    {
                        int i53 = Mth.Floor(d4 - d27) - i1 * 16 - 1;
                        int i34 = Mth.Floor(d4 + d27) - i1 * 16 + 1;
                        int i54 = Mth.Floor(d6 - d29) - 1;
                        int i36 = Mth.Floor(d6 + d29) + 1;
                        int i55 = Mth.Floor(d8 - d27) - i2 * 16 - 1;
                        int i38 = Mth.Floor(d8 + d27) - i2 * 16 + 1;
                        if (i53 < 0)
                        {
                            i53 = 0;
                        }

                        if (i34 > 16)
                        {
                            i34 = 16;
                        }

                        if (i54 < 1)
                        {
                            i54 = 1;
                        }

                        if (i36 > 120)
                        {
                            i36 = 120;
                        }

                        if (i55 < 0)
                        {
                            i55 = 0;
                        }

                        if (i38 > 16)
                        {
                            i38 = 16;
                        }

                        bool z56 = false;
                        int i40;
                        int i43;
                        for (i40 = i53; !z56 && i40 < i34; ++i40)
                        {
                            for (int i41 = i55; !z56 && i41 < i38; ++i41)
                            {
                                for (int i42 = i36 + 1; !z56 && i42 >= i54 - 1; --i42)
                                {
                                    i43 = (i40 * 16 + i41) * 128 + i42;
                                    if (i42 >= 0 && i42 < 128)
                                    {
                                        if (b3[i43] == Tile.water.id || b3[i43] == Tile.calmWater.id)
                                        {
                                            z56 = true;
                                        }

                                        if (i42 != i54 - 1 && i40 != i53 && i40 != i34 - 1 && i41 != i55 && i41 != i38 - 1)
                                        {
                                            i42 = i54;
                                        }
                                    }
                                }
                            }
                        }

                        if (!z56)
                        {
                            for (i40 = i53; i40 < i34; ++i40)
                            {
                                double d57 = (i40 + i1 * 16 + 0.5 - d4) / d27;
                                for (i43 = i55; i43 < i38; ++i43)
                                {
                                    double d44 = (i43 + i2 * 16 + 0.5 - d8) / d27;
                                    int i46 = (i40 * 16 + i43) * 128 + i36;
                                    bool z47 = false;
                                    if (d57 * d57 + d44 * d44 < 1)
                                    {
                                        for (int i48 = i36 - 1; i48 >= i54; --i48)
                                        {
                                            double d49 = (i48 + 0.5 - d6) / d29;
                                            if (d49 > -0.7 && d57 * d57 + d49 * d49 + d44 * d44 < 1)
                                            {
                                                byte b51 = b3[i46];
                                                if (b51 == Tile.grass.id)
                                                {
                                                    z47 = true;
                                                }

                                                if (b51 == Tile.rock.id || b51 == Tile.dirt.id || b51 == Tile.grass.id)
                                                {
                                                    if (i48 < 10)
                                                    {
                                                        b3[i46] = (byte)Tile.lava.id;
                                                    }
                                                    else
                                                    {
                                                        b3[i46] = 0;
                                                        if (z47 && b3[i46 - 1] == Tile.dirt.id)
                                                        {
                                                            b3[i46 - 1] = (byte)Tile.grass.id;
                                                        }
                                                    }
                                                }
                                            }

                                            --i46;
                                        }
                                    }
                                }
                            }

                            if (z52)
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

        protected override void AddFeature(Level world1, int i2, int i3, int i4, int i5, byte[] b6)
        {
            int i7 = this.rand.NextInt(this.rand.NextInt(this.rand.NextInt(40) + 1) + 1);
            if (this.rand.NextInt(15) != 0)
            {
                i7 = 0;
            }

            for (int i8 = 0; i8 < i7; ++i8)
            {
                double d9 = i2 * 16 + this.rand.NextInt(16);
                double d11 = this.rand.NextInt(this.rand.NextInt(120) + 8);
                double d13 = i3 * 16 + this.rand.NextInt(16);
                int i15 = 1;
                if (this.rand.NextInt(4) == 0)
                {
                    this.AddRoom(i4, i5, b6, d9, d11, d13);
                    i15 += this.rand.NextInt(4);
                }

                for (int i16 = 0; i16 < i15; ++i16)
                {
                    float f17 = this.rand.NextFloat() * Mth.PI * 2F;
                    float f18 = (this.rand.NextFloat() - 0.5F) * 2F / 8F;
                    float f19 = this.rand.NextFloat() * 2F + this.rand.NextFloat();
                    this.AddTunnel(i4, i5, b6, d9, d11, d13, f19, f17, f18, 0, 0, 1);
                }
            }
        }
    }
}