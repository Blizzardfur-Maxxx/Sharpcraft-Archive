using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.LevelGen.Features;
using SharpCraft.Core.World.GameLevel.LevelGen.Synthetic;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;

namespace SharpCraft.Core.World.GameLevel.LevelGen
{
    public class HellRandomLevelSource : IChunkSource
    {
        private JRandom hellRNG;
        private OctavePerlinNoise field_4169_i;
        private OctavePerlinNoise field_4168_j;
        private OctavePerlinNoise field_4167_k;
        private OctavePerlinNoise field_4166_l;
        private OctavePerlinNoise field_4165_m;
        public OctavePerlinNoise field_4177_a;
        public OctavePerlinNoise field_4176_b;
        private Level worldObj;
        private double[] field_4163_o;
        private double[] field_4162_p = new double[256];
        private double[] field_4161_q = new double[256];
        private double[] field_4160_r = new double[256];
        private LargeFeature field_4159_s = new LargeHellCaveFeature();
        double[] field_4175_c;
        double[] field_4174_d;
        double[] field_4173_e;
        double[] field_4172_f;
        double[] field_4171_g;
        public HellRandomLevelSource(Level world1, long j2)
        {
            this.worldObj = world1;
            this.hellRNG = new JRandom(j2);
            this.field_4169_i = new OctavePerlinNoise(this.hellRNG, 16);
            this.field_4168_j = new OctavePerlinNoise(this.hellRNG, 16);
            this.field_4167_k = new OctavePerlinNoise(this.hellRNG, 8);
            this.field_4166_l = new OctavePerlinNoise(this.hellRNG, 4);
            this.field_4165_m = new OctavePerlinNoise(this.hellRNG, 4);
            this.field_4177_a = new OctavePerlinNoise(this.hellRNG, 10);
            this.field_4176_b = new OctavePerlinNoise(this.hellRNG, 16);
        }

        public virtual void Func_4059_a(int i1, int i2, byte[] b3)
        {
            byte b4 = 4;
            byte b5 = 32;
            int i6 = b4 + 1;
            byte b7 = 17;
            int i8 = b4 + 1;
            this.field_4163_o = this.Func_4057_a(this.field_4163_o, i1 * b4, 0, i2 * b4, i6, b7, i8);
            for (int i9 = 0; i9 < b4; ++i9)
            {
                for (int i10 = 0; i10 < b4; ++i10)
                {
                    for (int i11 = 0; i11 < 16; ++i11)
                    {
                        double d12 = 0.125;
                        double d14 = this.field_4163_o[((i9 + 0) * i8 + i10 + 0) * b7 + i11 + 0];
                        double d16 = this.field_4163_o[((i9 + 0) * i8 + i10 + 1) * b7 + i11 + 0];
                        double d18 = this.field_4163_o[((i9 + 1) * i8 + i10 + 0) * b7 + i11 + 0];
                        double d20 = this.field_4163_o[((i9 + 1) * i8 + i10 + 1) * b7 + i11 + 0];
                        double d22 = (this.field_4163_o[((i9 + 0) * i8 + i10 + 0) * b7 + i11 + 1] - d14) * d12;
                        double d24 = (this.field_4163_o[((i9 + 0) * i8 + i10 + 1) * b7 + i11 + 1] - d16) * d12;
                        double d26 = (this.field_4163_o[((i9 + 1) * i8 + i10 + 0) * b7 + i11 + 1] - d18) * d12;
                        double d28 = (this.field_4163_o[((i9 + 1) * i8 + i10 + 1) * b7 + i11 + 1] - d20) * d12;
                        for (int i30 = 0; i30 < 8; ++i30)
                        {
                            double d31 = 0.25;
                            double d33 = d14;
                            double d35 = d16;
                            double d37 = (d18 - d14) * d31;
                            double d39 = (d20 - d16) * d31;
                            for (int i41 = 0; i41 < 4; ++i41)
                            {
                                int i42 = i41 + i9 * 4 << 11 | 0 + i10 * 4 << 7 | i11 * 8 + i30;
                                short s43 = 128;
                                double d44 = 0.25;
                                double d46 = d33;
                                double d48 = (d35 - d33) * d44;
                                for (int i50 = 0; i50 < 4; ++i50)
                                {
                                    int i51 = 0;
                                    if (i11 * 8 + i30 < b5)
                                    {
                                        i51 = Tile.calmLava.id;
                                    }

                                    if (d46 > 0)
                                    {
                                        i51 = Tile.netherrack.id;
                                    }

                                    b3[i42] = (byte)i51;
                                    i42 += s43;
                                    d46 += d48;
                                }

                                d33 += d37;
                                d35 += d39;
                            }

                            d14 += d22;
                            d16 += d24;
                            d18 += d26;
                            d20 += d28;
                        }
                    }
                }
            }
        }

        public virtual void Func_4058_b(int i1, int i2, byte[] b3)
        {
            byte b4 = 64;
            double d5 = 8 / 256;
            this.field_4162_p = this.field_4166_l.GenerateNoiseOctaves(this.field_4162_p, i1 * 16, i2 * 16, 0, 16, 16, 1, d5, d5, 1);
            this.field_4161_q = this.field_4166_l.GenerateNoiseOctaves(this.field_4161_q, i1 * 16, 109.0134, i2 * 16, 16, 1, 16, d5, 1, d5);
            this.field_4160_r = this.field_4165_m.GenerateNoiseOctaves(this.field_4160_r, i1 * 16, i2 * 16, 0, 16, 16, 1, d5 * 2, d5 * 2, d5 * 2);
            for (int i7 = 0; i7 < 16; ++i7)
            {
                for (int i8 = 0; i8 < 16; ++i8)
                {
                    bool z9 = this.field_4162_p[i7 + i8 * 16] + this.hellRNG.NextDouble() * 0.2 > 0;
                    bool z10 = this.field_4161_q[i7 + i8 * 16] + this.hellRNG.NextDouble() * 0.2 > 0;
                    int i11 = (int)(this.field_4160_r[i7 + i8 * 16] / 3 + 3 + this.hellRNG.NextDouble() * 0.25);
                    int i12 = -1;
                    byte b13 = (byte)Tile.netherrack.id;
                    byte b14 = (byte)Tile.netherrack.id;
                    for (int i15 = 127; i15 >= 0; --i15)
                    {
                        int i16 = (i8 * 16 + i7) * 128 + i15;
                        if (i15 >= 127 - this.hellRNG.NextInt(5))
                        {
                            b3[i16] = (byte)Tile.unbreakable.id;
                        }
                        else if (i15 <= 0 + this.hellRNG.NextInt(5))
                        {
                            b3[i16] = (byte)Tile.unbreakable.id;
                        }
                        else
                        {
                            byte b17 = b3[i16];
                            if (b17 == 0)
                            {
                                i12 = -1;
                            }
                            else if (b17 == Tile.netherrack.id)
                            {
                                if (i12 == -1)
                                {
                                    if (i11 <= 0)
                                    {
                                        b13 = 0;
                                        b14 = (byte)Tile.netherrack.id;
                                    }
                                    else if (i15 >= b4 - 4 && i15 <= b4 + 1)
                                    {
                                        b13 = (byte)Tile.netherrack.id;
                                        b14 = (byte)Tile.netherrack.id;
                                        if (z10)
                                        {
                                            b13 = (byte)Tile.gravel.id;
                                        }

                                        if (z10)
                                        {
                                            b14 = (byte)Tile.netherrack.id;
                                        }

                                        if (z9)
                                        {
                                            b13 = (byte)Tile.slowSand.id;
                                        }

                                        if (z9)
                                        {
                                            b14 = (byte)Tile.slowSand.id;
                                        }
                                    }

                                    if (i15 < b4 && b13 == 0)
                                    {
                                        b13 = (byte)Tile.calmLava.id;
                                    }

                                    i12 = i11;
                                    if (i15 >= b4 - 1)
                                    {
                                        b3[i16] = b13;
                                    }
                                    else
                                    {
                                        b3[i16] = b14;
                                    }
                                }
                                else if (i12 > 0)
                                {
                                    --i12;
                                    b3[i16] = b14;
                                }
                            }
                        }
                    }
                }
            }
        }

        public virtual LevelChunk Create(int i1, int i2)
        {
            return this.GetChunk(i1, i2);
        }

        public virtual LevelChunk GetChunk(int i1, int i2)
        {
            this.hellRNG.SetSeed(i1 * 341873128712 + i2 * 132897987541);
            byte[] b3 = new byte[32768];
            this.Func_4059_a(i1, i2, b3);
            this.Func_4058_b(i1, i2, b3);
            this.field_4159_s.Apply(this, this.worldObj, i1, i2, b3);
            LevelChunk chunk4 = new LevelChunk(this.worldObj, b3, i1, i2);
            return chunk4;
        }

        private double[] Func_4057_a(double[] d1, int i2, int i3, int i4, int i5, int i6, int i7)
        {
            if (d1 == null)
            {
                d1 = new double[i5 * i6 * i7];
            }

            double d8 = 684.412;
            double d10 = 2053.236;
            this.field_4172_f = this.field_4177_a.GenerateNoiseOctaves(this.field_4172_f, i2, i3, i4, i5, 1, i7, 1, 0, 1);
            this.field_4171_g = this.field_4176_b.GenerateNoiseOctaves(this.field_4171_g, i2, i3, i4, i5, 1, i7, 100, 0, 100);
            this.field_4175_c = this.field_4167_k.GenerateNoiseOctaves(this.field_4175_c, i2, i3, i4, i5, i6, i7, d8 / 80, d10 / 60, d8 / 80);
            this.field_4174_d = this.field_4169_i.GenerateNoiseOctaves(this.field_4174_d, i2, i3, i4, i5, i6, i7, d8, d10, d8);
            this.field_4173_e = this.field_4168_j.GenerateNoiseOctaves(this.field_4173_e, i2, i3, i4, i5, i6, i7, d8, d10, d8);
            int i12 = 0;
            int i13 = 0;
            double[] d14 = new double[i6];
            int i15;
            for (i15 = 0; i15 < i6; ++i15)
            {
                d14[i15] = Math.Cos(i15 * Math.PI * 6 / i6) * 2;
                double d16 = i15;
                if (i15 > i6 / 2)
                {
                    d16 = i6 - 1 - i15;
                }

                if (d16 < 4)
                {
                    d16 = 4 - d16;
                    d14[i15] -= d16 * d16 * d16 * 10;
                }
            }

            for (i15 = 0; i15 < i5; ++i15)
            {
                for (int i36 = 0; i36 < i7; ++i36)
                {
                    double d17 = (this.field_4172_f[i13] + 256) / 512;
                    if (d17 > 1)
                    {
                        d17 = 1;
                    }

                    double d19 = 0;
                    double d21 = this.field_4171_g[i13] / 8000;
                    if (d21 < 0)
                    {
                        d21 = -d21;
                    }

                    d21 = d21 * 3 - 3;
                    if (d21 < 0)
                    {
                        d21 /= 2;
                        if (d21 < -1)
                        {
                            d21 = -1;
                        }

                        d21 /= 1.4;
                        d21 /= 2;
                        d17 = 0;
                    }
                    else
                    {
                        if (d21 > 1)
                        {
                            d21 = 1;
                        }

                        d21 /= 6;
                    }

                    d17 += 0.5;
                    d21 = d21 * i6 / 16;
                    ++i13;
                    for (int i23 = 0; i23 < i6; ++i23)
                    {
                        double d24 = 0;
                        double d26 = d14[i23];
                        double d28 = this.field_4174_d[i12] / 512;
                        double d30 = this.field_4173_e[i12] / 512;
                        double d32 = (this.field_4175_c[i12] / 10 + 1) / 2;
                        if (d32 < 0)
                        {
                            d24 = d28;
                        }
                        else if (d32 > 1)
                        {
                            d24 = d30;
                        }
                        else
                        {
                            d24 = d28 + (d30 - d28) * d32;
                        }

                        d24 -= d26;
                        double d34;
                        if (i23 > i6 - 4)
                        {
                            d34 = (i23 - (i6 - 4)) / 3F;
                            d24 = d24 * (1 - d34) + -10 * d34;
                        }

                        if (i23 < d19)
                        {
                            d34 = (d19 - i23) / 4;
                            if (d34 < 0)
                            {
                                d34 = 0;
                            }

                            if (d34 > 1)
                            {
                                d34 = 1;
                            }

                            d24 = d24 * (1 - d34) + -10 * d34;
                        }

                        d1[i12] = d24;
                        ++i12;
                    }
                }
            }

            return d1;
        }

        public virtual bool HasChunk(int i1, int i2)
        {
            return true;
        }

        public virtual void PostProcess(IChunkSource iChunkProvider1, int i2, int i3)
        {
            SandTile.fallInstantly = true;
            int i4 = i2 * 16;
            int i5 = i3 * 16;
            int i6;
            int i7;
            int i8;
            int i9;
            for (i6 = 0; i6 < 8; ++i6)
            {
                i7 = i4 + this.hellRNG.NextInt(16) + 8;
                i8 = this.hellRNG.NextInt(120) + 4;
                i9 = i5 + this.hellRNG.NextInt(16) + 8;
                (new HellSpringFeature(Tile.lava.id)).Place(this.worldObj, this.hellRNG, i7, i8, i9);
            }

            i6 = this.hellRNG.NextInt(this.hellRNG.NextInt(10) + 1) + 1;
            int i10;
            for (i7 = 0; i7 < i6; ++i7)
            {
                i8 = i4 + this.hellRNG.NextInt(16) + 8;
                i9 = this.hellRNG.NextInt(120) + 4;
                i10 = i5 + this.hellRNG.NextInt(16) + 8;
                (new HellFireFeature()).Place(this.worldObj, this.hellRNG, i8, i9, i10);
            }

            i6 = this.hellRNG.NextInt(this.hellRNG.NextInt(10) + 1);
            for (i7 = 0; i7 < i6; ++i7)
            {
                i8 = i4 + this.hellRNG.NextInt(16) + 8;
                i9 = this.hellRNG.NextInt(120) + 4;
                i10 = i5 + this.hellRNG.NextInt(16) + 8;
                (new LightGemFeature()).Place(this.worldObj, this.hellRNG, i8, i9, i10);
            }

            for (i7 = 0; i7 < 10; ++i7)
            {
                i8 = i4 + this.hellRNG.NextInt(16) + 8;
                i9 = this.hellRNG.NextInt(128);
                i10 = i5 + this.hellRNG.NextInt(16) + 8;
                (new LightGemFeature2()).Place(this.worldObj, this.hellRNG, i8, i9, i10);
            }

            if (this.hellRNG.NextInt(1) == 0)
            {
                i7 = i4 + this.hellRNG.NextInt(16) + 8;
                i8 = this.hellRNG.NextInt(128);
                i9 = i5 + this.hellRNG.NextInt(16) + 8;
                (new FlowerFeature(Tile.mushroom1.id)).Place(this.worldObj, this.hellRNG, i7, i8, i9);
            }

            if (this.hellRNG.NextInt(1) == 0)
            {
                i7 = i4 + this.hellRNG.NextInt(16) + 8;
                i8 = this.hellRNG.NextInt(128);
                i9 = i5 + this.hellRNG.NextInt(16) + 8;
                (new FlowerFeature(Tile.mushroom2.id)).Place(this.worldObj, this.hellRNG, i7, i8, i9);
            }

            SandTile.fallInstantly = false;
        }

        public virtual bool Save(bool z1, IProgressListener iProgressUpdate2)
        {
            return true;
        }

        public virtual bool Tick()
        {
            return false;
        }

        public virtual bool ShouldSave()
        {
            return true;
        }

        public virtual string GatherStats()
        {
            return "HellRandomLevelSource";
        }
    }
}