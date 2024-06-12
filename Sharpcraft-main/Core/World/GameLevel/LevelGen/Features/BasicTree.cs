using SharpCraft.Core.Util;
using System;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class BasicTree : Feature
    {
        static readonly byte[] field_882_a = new[]
        {
            (byte)2,
            (byte)0,
            (byte)0,
            (byte)1,
            (byte)2,
            (byte)1
        };
        JRandom field_881_b = new JRandom();
        Level worldObj;
        int[] basePos = new[]
        {
            0,
            0,
            0
        };
        int field_878_e = 0;
        int height;
        double field_876_g = 0.618;
        double field_874_i = 0.381;
        double field_873_j = 1;
        double field_872_k = 1;
        int field_871_l = 1;
        int field_870_m = 12;
        int field_869_n = 4;
        int[][] field_868_o;

        internal virtual void Func_521_a()
        {
            this.height = (int)(this.field_878_e * this.field_876_g);
            if (this.height >= this.field_878_e)
            {
                this.height = this.field_878_e - 1;
            }

            int i1 = (int)(1.382 + Math.Pow(this.field_872_k * this.field_878_e / 13, 2));
            if (i1 < 1)
            {
                i1 = 1;
            }

            int[][] i2 = new int[i1 * this.field_878_e][];
            ArrayUtil.Init2DArray(i2, 4);
            int i3 = this.basePos[1] + this.field_878_e - this.field_869_n;
            int i4 = 1;
            int i5 = this.basePos[1] + this.height;
            int i6 = i3 - this.basePos[1];
            i2[0][0] = this.basePos[0];
            i2[0][1] = i3;
            i2[0][2] = this.basePos[2];
            i2[0][3] = i5;
            --i3;
            while (true)
            {
                while (i6 >= 0)
                {
                    int i7 = 0;
                    float f8 = this.Func_528_a(i6);
                    if (f8 < 0F)
                    {
                        --i3;
                        --i6;
                    }
                    else
                    {
                        for (double d9 = 0.5D; i7 < i1; ++i7)
                        {
                            double d11 = this.field_873_j * f8 * (this.field_881_b.NextFloat() + 0.328);
                            double d13 = this.field_881_b.NextFloat() * 2 * 3.14159;
                            int i15 = Mth.Floor(d11 * Math.Sin(d13) + this.basePos[0] + d9);
                            int i16 = Mth.Floor(d11 * Math.Cos(d13) + this.basePos[2] + d9);
                            int[] i17 = new[]
                            {
                                i15,
                                i3,
                                i16
                            };
                            int[] i18 = new[]
                            {
                                i15,
                                i3 + this.field_869_n,
                                i16
                            };
                            if (this.Func_524_a(i17, i18) == -1)
                            {
                                int[] i19 = new[]
                                {
                                    this.basePos[0],
                                    this.basePos[1],
                                    this.basePos[2]
                                };
                                double d20 = Math.Sqrt(Math.Pow(Math.Abs(this.basePos[0] - i17[0]), 2) + Math.Pow(Math.Abs(this.basePos[2] - i17[2]), 2));
                                double d22 = d20 * this.field_874_i;
                                if (i17[1] - d22 > i5)
                                {
                                    i19[1] = i5;
                                }
                                else
                                {
                                    i19[1] = (int)(i17[1] - d22);
                                }

                                if (this.Func_524_a(i19, i17) == -1)
                                {
                                    i2[i4][0] = i15;
                                    i2[i4][1] = i3;
                                    i2[i4][2] = i16;
                                    i2[i4][3] = i19[1];
                                    ++i4;
                                }
                            }
                        }

                        --i3;
                        --i6;
                    }
                }

                this.field_868_o = new int[i4][];
                ArrayUtil.Init2DArray(field_868_o, 4);
                Array.Copy(i2, 0, this.field_868_o, 0, i4);
                return;
            }
        }

        internal virtual void Func_523_a(int i1, int i2, int i3, float f4, byte b5, int i6)
        {
            int i7 = (int)(f4 + 0.618);
            byte b8 = field_882_a[b5];
            byte b9 = field_882_a[b5 + 3];
            int[] i10 = new[]
            {
                i1,
                i2,
                i3
            };
            int[] i11 = new[]
            {
                0,
                0,
                0
            };
            int i12 = -i7;
            int i13 = -i7;
            bool cont32 = false;
            for (i11[b5] = i10[b5]; i12 <= i7; ++i12)
            {
            flb:
                if (cont32)
                {
                    cont32 = false;
                    continue;
                }

                i11[b8] = i10[b8] + i12;
                i13 = -i7;
                while (true)
                {
                    while (true)
                    {
                        if (i13 > i7)
                        {
                            cont32 = true;
                            goto flb; //should be continue label32; but not possible unless there is some other way
                        }

                        double d15 = Math.Sqrt(Math.Pow(Math.Abs(i12) + 0.5, 2) + Math.Pow(Math.Abs(i13) + 0.5, 2));
                        if (d15 > f4)
                        {
                            ++i13;
                        }
                        else
                        {
                            i11[b9] = i10[b9] + i13;
                            int i14 = this.worldObj.GetTile(i11[0], i11[1], i11[2]);
                            if (i14 != 0 && i14 != 18)
                            {
                                ++i13;
                            }
                            else
                            {
                                this.worldObj.SetTileNoUpdate(i11[0], i11[1], i11[2], i6);
                                ++i13;
                            }
                        }
                    }
                }
            }
        }

        internal virtual float Func_528_a(int i1)
        {
            if (i1 < ((float)this.field_878_e) * 0.3)
            {
                return -1.618F;
            }
            else
            {
                float f2 = this.field_878_e / 2F;
                float f3 = this.field_878_e / 2F - i1;
                float f4;
                if (f3 == 0F)
                {
                    f4 = f2;
                }
                else if (Math.Abs(f3) >= f2)
                {
                    f4 = 0F;
                }
                else
                {
                    f4 = (float)Math.Sqrt(Math.Pow(Math.Abs(f2), 2) - Math.Pow(Math.Abs(f3), 2));
                }

                f4 *= 0.5F;
                return f4;
            }
        }

        internal virtual float Func_526_b(int i1)
        {
            return i1 >= 0 && i1 < this.field_869_n ? (i1 != 0 && i1 != this.field_869_n - 1 ? 3F : 2F) : -1F;
        }

        internal virtual void Func_520_a(int i1, int i2, int i3)
        {
            int i4 = i2;
            for (int i5 = i2 + this.field_869_n; i4 < i5; ++i4)
            {
                float f6 = this.Func_526_b(i4 - i2);
                this.Func_523_a(i1, i4, i3, f6, (byte)1, 18);
            }
        }

        internal virtual void Func_522_a(int[] i1, int[] i2, int i3)
        {
            int[] i4 = new[]
            {
                0,
                0,
                0
            };
            byte b5 = 0;
            byte b6;
            for (b6 = 0; b5 < 3; ++b5)
            {
                i4[b5] = i2[b5] - i1[b5];
                if (Math.Abs(i4[b5]) > Math.Abs(i4[b6]))
                {
                    b6 = b5;
                }
            }

            if (i4[b6] != 0)
            {
                byte b7 = field_882_a[b6];
                byte b8 = field_882_a[b6 + 3];
                sbyte b9;
                if (i4[b6] > 0)
                {
                    b9 = 1;
                }
                else
                {
                    b9 = -1;
                }

                double d10 = (double)i4[b7] / (double)i4[b6];
                double d12 = (double)i4[b8] / (double)i4[b6];
                int[] i14 = new[]
                {
                    0,
                    0,
                    0
                };
                int i15 = 0;
                for (int i16 = i4[b6] + b9; i15 != i16; i15 += b9)
                {
                    i14[b6] = Mth.Floor(i1[b6] + i15 + 0.5);
                    i14[b7] = Mth.Floor(i1[b7] + i15 * d10 + 0.5);
                    i14[b8] = Mth.Floor(i1[b8] + i15 * d12 + 0.5);
                    this.worldObj.SetTileNoUpdate(i14[0], i14[1], i14[2], i3);
                }
            }
        }

        internal virtual void Func_518_b()
        {
            int i1 = 0;
            for (int i2 = this.field_868_o.Length; i1 < i2; ++i1)
            {
                int i3 = this.field_868_o[i1][0];
                int i4 = this.field_868_o[i1][1];
                int i5 = this.field_868_o[i1][2];
                this.Func_520_a(i3, i4, i5);
            }
        }

        internal virtual bool Func_527_c(int i1)
        {
            return i1 >= this.field_878_e * 0.2;
        }

        internal virtual void Func_529_c()
        {
            int i1 = this.basePos[0];
            int i2 = this.basePos[1];
            int i3 = this.basePos[1] + this.height;
            int i4 = this.basePos[2];
            int[] i5 = new[]
            {
                i1,
                i2,
                i4
            };
            int[] i6 = new[]
            {
                i1,
                i3,
                i4
            };
            this.Func_522_a(i5, i6, 17);
            if (this.field_871_l == 2)
            {
                ++i5[0];
                ++i6[0];
                this.Func_522_a(i5, i6, 17);
                ++i5[2];
                ++i6[2];
                this.Func_522_a(i5, i6, 17);
                i5[0] += -1;
                i6[0] += -1;
                this.Func_522_a(i5, i6, 17);
            }
        }

        internal virtual void Func_525_d()
        {
            int i1 = 0;
            int i2 = this.field_868_o.Length;
            for (int[] i3 = { this.basePos[0], this.basePos[1], this.basePos[2] }; i1 < i2; ++i1)
            {
                int[] i4 = this.field_868_o[i1];
                int[] i5 = new[]
                {
                    i4[0],
                    i4[1],
                    i4[2]
                };
                i3[1] = i4[3];
                int i6 = i3[1] - this.basePos[1];
                if (this.Func_527_c(i6))
                {
                    this.Func_522_a(i3, i5, 17);
                }
            }
        }

        internal virtual int Func_524_a(int[] i1, int[] i2)
        {
            int[] i3 = new[]
            {
                0,
                0,
                0
            };
            byte b4 = 0;
            byte b5;
            for (b5 = 0; b4 < 3; ++b4)
            {
                i3[b4] = i2[b4] - i1[b4];
                if (Math.Abs(i3[b4]) > Math.Abs(i3[b5]))
                {
                    b5 = b4;
                }
            }

            if (i3[b5] == 0)
            {
                return -1;
            }
            else
            {
                byte b6 = field_882_a[b5];
                byte b7 = field_882_a[b5 + 3];
                sbyte b8;
                if (i3[b5] > 0)
                {
                    b8 = 1;
                }
                else
                {
                    b8 = -1;
                }

                double d9 = (double)i3[b6] / (double)i3[b5];
                double d11 = (double)i3[b7] / (double)i3[b5];
                int[] i13 = new[]
                {
                    0,
                    0,
                    0
                };
                int i14 = 0;
                int i15;
                for (i15 = i3[b5] + b8; i14 != i15; i14 += b8)
                {
                    i13[b5] = i1[b5] + i14;
                    i13[b6] = Mth.Floor(i1[b6] + i14 * d9);
                    i13[b7] = Mth.Floor(i1[b7] + i14 * d11);
                    int i16 = this.worldObj.GetTile(i13[0], i13[1], i13[2]);
                    if (i16 != 0 && i16 != 18)
                    {
                        break;
                    }
                }

                return i14 == i15 ? -1 : Math.Abs(i14);
            }
        }

        internal virtual bool Func_519_e()
        {
            int[] i1 = new[]
            {
                this.basePos[0],
                this.basePos[1],
                this.basePos[2]
            };
            int[] i2 = new[]
            {
                this.basePos[0],
                this.basePos[1] + this.field_878_e - 1,
                this.basePos[2]
            };
            int i3 = this.worldObj.GetTile(this.basePos[0], this.basePos[1] - 1, this.basePos[2]);
            if (i3 != 2 && i3 != 3)
            {
                return false;
            }
            else
            {
                int i4 = this.Func_524_a(i1, i2);
                if (i4 == -1)
                {
                    return true;
                }
                else if (i4 < 6)
                {
                    return false;
                }
                else
                {
                    this.field_878_e = i4;
                    return true;
                }
            }
        }

        public override void Init(double d1, double d3, double d5)
        {
            this.field_870_m = (int)(d1 * 12);
            if (d1 > 0.5)
            {
                this.field_869_n = 5;
            }

            this.field_873_j = d3;
            this.field_872_k = d5;
        }

        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            this.worldObj = world1;
            long j6 = random2.NextLong();
            this.field_881_b.SetSeed(j6);
            this.basePos[0] = i3;
            this.basePos[1] = i4;
            this.basePos[2] = i5;
            if (this.field_878_e == 0)
            {
                this.field_878_e = 5 + this.field_881_b.NextInt(this.field_870_m);
            }

            if (!this.Func_519_e())
            {
                return false;
            }
            else
            {
                this.Func_521_a();
                this.Func_518_b();
                this.Func_529_c();
                this.Func_525_d();
                return true;
            }
        }
    }
}