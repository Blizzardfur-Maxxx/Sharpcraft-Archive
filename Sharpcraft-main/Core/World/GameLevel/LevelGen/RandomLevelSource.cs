using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.LevelGen.Features;
using SharpCraft.Core.World.GameLevel.LevelGen.Synthetic;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen
{
    public class RandomLevelSource : IChunkSource
    {
        private JRandom rand;
        private OctavePerlinNoise field_912_k;
        private OctavePerlinNoise field_911_l;
        private OctavePerlinNoise field_910_m;
        private OctavePerlinNoise field_909_n;
        private OctavePerlinNoise field_908_o;
        public OctavePerlinNoise field_922_a;
        public OctavePerlinNoise field_921_b;
        public OctavePerlinNoise mobSpawnerNoise;
        private Level worldObj;
        private double[] field_4180_q;
        private double[] sandNoise = new double[256];
        private double[] gravelNoise = new double[256];
        private double[] stoneNoise = new double[256];
        private LargeFeature field_902_u = new LargeCaveFeature();
        private Biome[] biomesForGeneration;
        double[] field_4185_d;
        double[] field_4184_e;
        double[] field_4183_f;
        double[] field_4182_g;
        double[] field_4181_h;
        int[][] field_914_i = new int[32][];
        private double[] generatedTemperatures;
        public RandomLevelSource(Level world1, long j2)
        {
            ArrayUtil.Init2DArray(field_914_i, 32);
            this.worldObj = world1;
            this.rand = new JRandom(j2);
            this.field_912_k = new OctavePerlinNoise(this.rand, 16);
            this.field_911_l = new OctavePerlinNoise(this.rand, 16);
            this.field_910_m = new OctavePerlinNoise(this.rand, 8);
            this.field_909_n = new OctavePerlinNoise(this.rand, 4);
            this.field_908_o = new OctavePerlinNoise(this.rand, 4);
            this.field_922_a = new OctavePerlinNoise(this.rand, 10);
            this.field_921_b = new OctavePerlinNoise(this.rand, 16);
            this.mobSpawnerNoise = new OctavePerlinNoise(this.rand, 8);
        }

        public virtual void GenerateTerrain(int i1, int i2, byte[] b3, Biome[] biomeGenBase4, double[] d5)
        {
            byte b6 = 4;
            byte b7 = 64;
            int i8 = b6 + 1;
            byte b9 = 17;
            int i10 = b6 + 1;
            this.field_4180_q = this.Func_4061_a(this.field_4180_q, i1 * b6, 0, i2 * b6, i8, b9, i10);
            for (int i11 = 0; i11 < b6; ++i11)
            {
                for (int i12 = 0; i12 < b6; ++i12)
                {
                    for (int i13 = 0; i13 < 16; ++i13)
                    {
                        double d14 = 0.125;
                        double d16 = this.field_4180_q[((i11 + 0) * i10 + i12 + 0) * b9 + i13 + 0];
                        double d18 = this.field_4180_q[((i11 + 0) * i10 + i12 + 1) * b9 + i13 + 0];
                        double d20 = this.field_4180_q[((i11 + 1) * i10 + i12 + 0) * b9 + i13 + 0];
                        double d22 = this.field_4180_q[((i11 + 1) * i10 + i12 + 1) * b9 + i13 + 0];
                        double d24 = (this.field_4180_q[((i11 + 0) * i10 + i12 + 0) * b9 + i13 + 1] - d16) * d14;
                        double d26 = (this.field_4180_q[((i11 + 0) * i10 + i12 + 1) * b9 + i13 + 1] - d18) * d14;
                        double d28 = (this.field_4180_q[((i11 + 1) * i10 + i12 + 0) * b9 + i13 + 1] - d20) * d14;
                        double d30 = (this.field_4180_q[((i11 + 1) * i10 + i12 + 1) * b9 + i13 + 1] - d22) * d14;
                        for (int i32 = 0; i32 < 8; ++i32)
                        {
                            double d33 = 0.25;
                            double d35 = d16;
                            double d37 = d18;
                            double d39 = (d20 - d16) * d33;
                            double d41 = (d22 - d18) * d33;
                            for (int i43 = 0; i43 < 4; ++i43)
                            {
                                int i44 = i43 + i11 * 4 << 11 | 0 + i12 * 4 << 7 | i13 * 8 + i32;
                                short s45 = 128;
                                double d46 = 0.25;
                                double d48 = d35;
                                double d50 = (d37 - d35) * d46;
                                for (int i52 = 0; i52 < 4; ++i52)
                                {
                                    double d53 = d5[(i11 * 4 + i43) * 16 + i12 * 4 + i52];
                                    int i55 = 0;
                                    if (i13 * 8 + i32 < b7)
                                    {
                                        if (d53 < 0.5 && i13 * 8 + i32 >= b7 - 1)
                                        {
                                            i55 = Tile.ice.id;
                                        }
                                        else
                                        {
                                            i55 = Tile.calmWater.id;
                                        }
                                    }

                                    if (d48 > 0)
                                    {
                                        i55 = Tile.rock.id;
                                    }

                                    b3[i44] = (byte)i55;
                                    i44 += s45;
                                    d48 += d50;
                                }

                                d35 += d39;
                                d37 += d41;
                            }

                            d16 += d24;
                            d18 += d26;
                            d20 += d28;
                            d22 += d30;
                        }
                    }
                }
            }
        }

        public virtual void ReplaceBlocksForBiome(int i1, int i2, byte[] b3, Biome[] biomeGenBase4)
        {
            byte b5 = 64;
            double d6 = 8d / 256d;
            this.sandNoise = this.field_909_n.GenerateNoiseOctaves(this.sandNoise, i1 * 16, i2 * 16, 0d, 16, 16, 1, d6, d6, 1d);
            this.gravelNoise = this.field_909_n.GenerateNoiseOctaves(this.gravelNoise, i1 * 16, 109.0134d, i2 * 16, 16, 1, 16, d6, 1d, d6);
            this.stoneNoise = this.field_908_o.GenerateNoiseOctaves(this.stoneNoise, i1 * 16, i2 * 16, 0d, 16, 16, 1, d6 * 2d, d6 * 2d, d6 * 2d);
            for (int i8 = 0; i8 < 16; ++i8)
            {
                for (int i9 = 0; i9 < 16; ++i9)
                {
                    Biome biomeGenBase10 = biomeGenBase4[i8 + i9 * 16];
                    bool z11 = this.sandNoise[i8 + i9 * 16] + this.rand.NextDouble() * 0.2d > 0d;
                    bool z12 = this.gravelNoise[i8 + i9 * 16] + this.rand.NextDouble() * 0.2d > 3d;
                    int i13 = (int)(this.stoneNoise[i8 + i9 * 16] / 3d + 3d + this.rand.NextDouble() * 0.25d);
                    int i14 = -1;
                    byte b15 = biomeGenBase10.topTile;
                    byte b16 = biomeGenBase10.fillerTile;
                    for (int i17 = 127; i17 >= 0; --i17)
                    {
                        int i18 = (i9 * 16 + i8) * 128 + i17;
                        if (i17 <= 0 + this.rand.NextInt(5))
                        {
                            b3[i18] = (byte)Tile.unbreakable.id;
                        }
                        else
                        {
                            byte b19 = b3[i18];
                            if (b19 == 0)
                            {
                                i14 = -1;
                            }
                            else if (b19 == Tile.rock.id)
                            {
                                if (i14 == -1)
                                {
                                    if (i13 <= 0)
                                    {
                                        b15 = 0;
                                        b16 = (byte)Tile.rock.id;
                                    }
                                    else if (i17 >= b5 - 4 && i17 <= b5 + 1)
                                    {
                                        b15 = biomeGenBase10.topTile;
                                        b16 = biomeGenBase10.fillerTile;
                                        if (z12)
                                        {
                                            b15 = 0;
                                        }

                                        if (z12)
                                        {
                                            b16 = (byte)Tile.gravel.id;
                                        }

                                        if (z11)
                                        {
                                            b15 = (byte)Tile.sand.id;
                                        }

                                        if (z11)
                                        {
                                            b16 = (byte)Tile.sand.id;
                                        }
                                    }

                                    if (i17 < b5 && b15 == 0)
                                    {
                                        b15 = (byte)Tile.calmWater.id;
                                    }

                                    i14 = i13;
                                    if (i17 >= b5 - 1)
                                    {
                                        b3[i18] = b15;
                                    }
                                    else
                                    {
                                        b3[i18] = b16;
                                    }
                                }
                                else if (i14 > 0)
                                {
                                    --i14;
                                    b3[i18] = b16;
                                    if (i14 == 0 && b16 == Tile.sand.id)
                                    {
                                        i14 = this.rand.NextInt(4);
                                        b16 = (byte)Tile.sandStone.id;
                                    }
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
            this.rand.SetSeed(i1 * 341873128712 + i2 * 132897987541);
            byte[] b3 = new byte[32768];
            LevelChunk chunk4 = new LevelChunk(this.worldObj, b3, i1, i2);
            this.biomesForGeneration = this.worldObj.GetBiomeSource().LoadBlockGeneratorData(this.biomesForGeneration, i1 * 16, i2 * 16, 16, 16);
            double[] d5 = this.worldObj.GetBiomeSource().temperature;
            this.GenerateTerrain(i1, i2, b3, this.biomesForGeneration, d5);
            this.ReplaceBlocksForBiome(i1, i2, b3, this.biomesForGeneration);
            this.field_902_u.Apply(this, this.worldObj, i1, i2, b3);
            chunk4.CalculateLight();
            return chunk4;
        }

        private double[] Func_4061_a(double[] d1, int i2, int i3, int i4, int i5, int i6, int i7)
        {
            if (d1 == null)
            {
                d1 = new double[i5 * i6 * i7];
            }

            double d8 = 684.412d;
            double d10 = 684.412d;
            double[] d12 = this.worldObj.GetBiomeSource().temperature;
            double[] d13 = this.worldObj.GetBiomeSource().humidity;
            this.field_4182_g = this.field_922_a.GetRegion(this.field_4182_g, i2, i4, i5, i7, 1.121d, 1.121d, 0.5d);
            this.field_4181_h = this.field_921_b.GetRegion(this.field_4181_h, i2, i4, i5, i7, 200d, 200d, 0.5d);
            this.field_4185_d = this.field_910_m.GenerateNoiseOctaves(this.field_4185_d, i2, i3, i4, i5, i6, i7, d8 / 80, d10 / 160d, d8 / 80d);
            this.field_4184_e = this.field_912_k.GenerateNoiseOctaves(this.field_4184_e, i2, i3, i4, i5, i6, i7, d8, d10, d8);
            this.field_4183_f = this.field_911_l.GenerateNoiseOctaves(this.field_4183_f, i2, i3, i4, i5, i6, i7, d8, d10, d8);
            int i14 = 0;
            int i15 = 0;
            int i16 = 16 / i5;
            for (int i17 = 0; i17 < i5; ++i17)
            {
                int i18 = i17 * i16 + i16 / 2;
                for (int i19 = 0; i19 < i7; ++i19)
                {
                    int i20 = i19 * i16 + i16 / 2;
                    double d21 = d12[i18 * 16 + i20];
                    double d23 = d13[i18 * 16 + i20] * d21;
                    double d25 = 1d - d23;
                    d25 *= d25;
                    d25 *= d25;
                    d25 = 1d - d25;
                    double d27 = (this.field_4182_g[i15] + 256d) / 512d;
                    d27 *= d25;
                    if (d27 > 1d)
                    {
                        d27 = 1d;
                    }

                    double d29 = this.field_4181_h[i15] / 8000d;
                    if (d29 < 0d)
                    {
                        d29 = -d29 * 0.3d;
                    }

                    d29 = d29 * 3d - 2d;
                    if (d29 < 0d)
                    {
                        d29 /= 2d;
                        if (d29 < -1d)
                        {
                            d29 = -1d;
                        }

                        d29 /= 1.4d;
                        d29 /= 2d;
                        d27 = 0d;
                    }
                    else
                    {
                        if (d29 > 1d)
                        {
                            d29 = 1d;
                        }

                        d29 /= 8d;
                    }

                    if (d27 < 0d)
                    {
                        d27 = 0d;
                    }

                    d27 += 0.5d;
                    d29 = d29 * i6 / 16d;
                    double d31 = i6 / 2d + d29 * 4d;
                    ++i15;
                    for (int i33 = 0; i33 < i6; ++i33)
                    {
                        double d34 = 0d;
                        double d36 = (i33 - d31) * 12d / d27;
                        if (d36 < 0d)
                        {
                            d36 *= 4d;
                        }

                        double d38 = this.field_4184_e[i14] / 512d;
                        double d40 = this.field_4183_f[i14] / 512d;
                        double d42 = (this.field_4185_d[i14] / 10d + 1d) / 2d;
                        if (d42 < 0d)
                        {
                            d34 = d38;
                        }
                        else if (d42 > 1d)
                        {
                            d34 = d40;
                        }
                        else
                        {
                            d34 = d38 + (d40 - d38) * d42;
                        }

                        d34 -= d36;
                        if (i33 > i6 - 4d)
                        {
                            double d44 = (i33 - (i6 - 4)) / 3F;
                            d34 = d34 * (1d - d44) + -10d * d44;
                        }

                        d1[i14] = d34;
                        ++i14;
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
            Biome biomeGenBase6 = this.worldObj.GetBiomeSource().GetBiomeGenAt(i4 + 16, i5 + 16);
            this.rand.SetSeed(this.worldObj.GetRandomSeed());
            long j7 = this.rand.NextLong() / 2 * 2 + 1;
            long j9 = this.rand.NextLong() / 2 * 2 + 1;
            this.rand.SetSeed(i2 * j7 + i3 * j9 ^ this.worldObj.GetRandomSeed());
            double d11 = 0.25;
            int i13;
            int i14;
            int i15;
            if (this.rand.NextInt(4) == 0)
            {
                i13 = i4 + this.rand.NextInt(16) + 8;
                i14 = this.rand.NextInt(128);
                i15 = i5 + this.rand.NextInt(16) + 8;
                (new LakeFeature(Tile.calmWater.id)).Place(this.worldObj, this.rand, i13, i14, i15);
            }

            if (this.rand.NextInt(8) == 0)
            {
                i13 = i4 + this.rand.NextInt(16) + 8;
                i14 = this.rand.NextInt(this.rand.NextInt(120) + 8);
                i15 = i5 + this.rand.NextInt(16) + 8;
                if (i14 < 64 || this.rand.NextInt(10) == 0)
                {
                    (new LakeFeature(Tile.calmLava.id)).Place(this.worldObj, this.rand, i13, i14, i15);
                }
            }

            int i16;
            for (i13 = 0; i13 < 8; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16) + 8;
                i15 = this.rand.NextInt(128);
                i16 = i5 + this.rand.NextInt(16) + 8;
                (new MonsterRoomFeature()).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            for (i13 = 0; i13 < 10; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16);
                i15 = this.rand.NextInt(128);
                i16 = i5 + this.rand.NextInt(16);
                (new ClayFeature(32)).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            for (i13 = 0; i13 < 20; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16);
                i15 = this.rand.NextInt(128);
                i16 = i5 + this.rand.NextInt(16);
                (new OreFeature(Tile.dirt.id, 32)).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            for (i13 = 0; i13 < 10; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16);
                i15 = this.rand.NextInt(128);
                i16 = i5 + this.rand.NextInt(16);
                (new OreFeature(Tile.gravel.id, 32)).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            for (i13 = 0; i13 < 20; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16);
                i15 = this.rand.NextInt(128);
                i16 = i5 + this.rand.NextInt(16);
                (new OreFeature(Tile.coalOre.id, 16)).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            for (i13 = 0; i13 < 20; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16);
                i15 = this.rand.NextInt(64);
                i16 = i5 + this.rand.NextInt(16);
                (new OreFeature(Tile.ironOre.id, 8)).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            for (i13 = 0; i13 < 2; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16);
                i15 = this.rand.NextInt(32);
                i16 = i5 + this.rand.NextInt(16);
                (new OreFeature(Tile.goldOre.id, 8)).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            for (i13 = 0; i13 < 8; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16);
                i15 = this.rand.NextInt(16);
                i16 = i5 + this.rand.NextInt(16);
                (new OreFeature(Tile.oreRedstone.id, 7)).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            for (i13 = 0; i13 < 1; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16);
                i15 = this.rand.NextInt(16);
                i16 = i5 + this.rand.NextInt(16);
                (new OreFeature(Tile.oreDiamond.id, 7)).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            for (i13 = 0; i13 < 1; ++i13)
            {
                i14 = i4 + this.rand.NextInt(16);
                i15 = this.rand.NextInt(16) + this.rand.NextInt(16);
                i16 = i5 + this.rand.NextInt(16);
                (new OreFeature(Tile.lapisOre.id, 6)).Place(this.worldObj, this.rand, i14, i15, i16);
            }

            d11 = 0.5;
            i13 = (int)((this.mobSpawnerNoise.GetValue(i4 * d11, i5 * d11) / 8 + this.rand.NextDouble() * 4 + 4) / 3);
            i14 = 0;
            if (this.rand.NextInt(10) == 0)
            {
                ++i14;
            }

            if (biomeGenBase6 == Biome.forest)
            {
                i14 += i13 + 5;
            }

            if (biomeGenBase6 == Biome.rainforest)
            {
                i14 += i13 + 5;
            }

            if (biomeGenBase6 == Biome.seasonalForest)
            {
                i14 += i13 + 2;
            }

            if (biomeGenBase6 == Biome.taiga)
            {
                i14 += i13 + 5;
            }

            if (biomeGenBase6 == Biome.desert)
            {
                i14 -= 20;
            }

            if (biomeGenBase6 == Biome.tundra)
            {
                i14 -= 20;
            }

            if (biomeGenBase6 == Biome.plains)
            {
                i14 -= 20;
            }

            int i17;
            for (i15 = 0; i15 < i14; ++i15)
            {
                i16 = i4 + this.rand.NextInt(16) + 8;
                i17 = i5 + this.rand.NextInt(16) + 8;
                Feature worldGenerator18 = biomeGenBase6.GetTreeFeature(this.rand);
                worldGenerator18.Init(1, 1, 1);
                worldGenerator18.Place(this.worldObj, this.rand, i16, this.worldObj.GetHeightValue(i16, i17), i17);
            }

            byte b27 = 0;
            if (biomeGenBase6 == Biome.forest)
            {
                b27 = 2;
            }

            if (biomeGenBase6 == Biome.seasonalForest)
            {
                b27 = 4;
            }

            if (biomeGenBase6 == Biome.taiga)
            {
                b27 = 2;
            }

            if (biomeGenBase6 == Biome.plains)
            {
                b27 = 3;
            }

            int i19;
            int i25;
            for (i16 = 0; i16 < b27; ++i16)
            {
                i17 = i4 + this.rand.NextInt(16) + 8;
                i25 = this.rand.NextInt(128);
                i19 = i5 + this.rand.NextInt(16) + 8;
                (new FlowerFeature(Tile.flower.id)).Place(this.worldObj, this.rand, i17, i25, i19);
            }

            byte b28 = 0;
            if (biomeGenBase6 == Biome.forest)
            {
                b28 = 2;
            }

            if (biomeGenBase6 == Biome.rainforest)
            {
                b28 = 10;
            }

            if (biomeGenBase6 == Biome.seasonalForest)
            {
                b28 = 2;
            }

            if (biomeGenBase6 == Biome.taiga)
            {
                b28 = 1;
            }

            if (biomeGenBase6 == Biome.plains)
            {
                b28 = 10;
            }

            int i20;
            int i21;
            for (i17 = 0; i17 < b28; ++i17)
            {
                byte b26 = 1;
                if (biomeGenBase6 == Biome.rainforest && this.rand.NextInt(3) != 0)
                {
                    b26 = 2;
                }

                i19 = i4 + this.rand.NextInt(16) + 8;
                i20 = this.rand.NextInt(128);
                i21 = i5 + this.rand.NextInt(16) + 8;
                (new TallGrassFeature(Tile.tallGrass.id, b26)).Place(this.worldObj, this.rand, i19, i20, i21);
            }

            b28 = 0;
            if (biomeGenBase6 == Biome.desert)
            {
                b28 = 2;
            }

            for (i17 = 0; i17 < b28; ++i17)
            {
                i25 = i4 + this.rand.NextInt(16) + 8;
                i19 = this.rand.NextInt(128);
                i20 = i5 + this.rand.NextInt(16) + 8;
                (new DeadBushFeature(Tile.deadBush.id)).Place(this.worldObj, this.rand, i25, i19, i20);
            }

            if (this.rand.NextInt(2) == 0)
            {
                i17 = i4 + this.rand.NextInt(16) + 8;
                i25 = this.rand.NextInt(128);
                i19 = i5 + this.rand.NextInt(16) + 8;
                (new FlowerFeature(Tile.rose.id)).Place(this.worldObj, this.rand, i17, i25, i19);
            }

            if (this.rand.NextInt(4) == 0)
            {
                i17 = i4 + this.rand.NextInt(16) + 8;
                i25 = this.rand.NextInt(128);
                i19 = i5 + this.rand.NextInt(16) + 8;
                (new FlowerFeature(Tile.mushroom1.id)).Place(this.worldObj, this.rand, i17, i25, i19);
            }

            if (this.rand.NextInt(8) == 0)
            {
                i17 = i4 + this.rand.NextInt(16) + 8;
                i25 = this.rand.NextInt(128);
                i19 = i5 + this.rand.NextInt(16) + 8;
                (new FlowerFeature(Tile.mushroom2.id)).Place(this.worldObj, this.rand, i17, i25, i19);
            }

            for (i17 = 0; i17 < 10; ++i17)
            {
                i25 = i4 + this.rand.NextInt(16) + 8;
                i19 = this.rand.NextInt(128);
                i20 = i5 + this.rand.NextInt(16) + 8;
                (new ReedsFeature()).Place(this.worldObj, this.rand, i25, i19, i20);
            }

            if (this.rand.NextInt(32) == 0)
            {
                i17 = i4 + this.rand.NextInt(16) + 8;
                i25 = this.rand.NextInt(128);
                i19 = i5 + this.rand.NextInt(16) + 8;
                (new PumpkinFeature()).Place(this.worldObj, this.rand, i17, i25, i19);
            }

            i17 = 0;
            if (biomeGenBase6 == Biome.desert)
            {
                i17 += 10;
            }

            for (i25 = 0; i25 < i17; ++i25)
            {
                i19 = i4 + this.rand.NextInt(16) + 8;
                i20 = this.rand.NextInt(128);
                i21 = i5 + this.rand.NextInt(16) + 8;
                (new CactusFeature()).Place(this.worldObj, this.rand, i19, i20, i21);
            }

            for (i25 = 0; i25 < 50; ++i25)
            {
                i19 = i4 + this.rand.NextInt(16) + 8;
                i20 = this.rand.NextInt(this.rand.NextInt(120) + 8);
                i21 = i5 + this.rand.NextInt(16) + 8;
                (new SpringFeature(Tile.water.id)).Place(this.worldObj, this.rand, i19, i20, i21);
            }

            for (i25 = 0; i25 < 20; ++i25)
            {
                i19 = i4 + this.rand.NextInt(16) + 8;
                i20 = this.rand.NextInt(this.rand.NextInt(this.rand.NextInt(112) + 8) + 8);
                i21 = i5 + this.rand.NextInt(16) + 8;
                (new SpringFeature(Tile.lava.id)).Place(this.worldObj, this.rand, i19, i20, i21);
            }

            this.generatedTemperatures = this.worldObj.GetBiomeSource().GetTemperatures(this.generatedTemperatures, i4 + 8, i5 + 8, 16, 16);
            for (i25 = i4 + 8; i25 < i4 + 8 + 16; ++i25)
            {
                for (i19 = i5 + 8; i19 < i5 + 8 + 16; ++i19)
                {
                    i20 = i25 - (i4 + 8);
                    i21 = i19 - (i5 + 8);
                    int i22 = this.worldObj.GetTopSolidBlock(i25, i19);
                    double d23 = this.generatedTemperatures[i20 * 16 + i21] - (i22 - 64) / 64 * 0.3;
                    if (d23 < 0.5 && i22 > 0 && i22 < 128 && this.worldObj.IsAirBlock(i25, i22, i19) && this.worldObj.GetMaterial(i25, i22 - 1, i19).BlocksMotion() && this.worldObj.GetMaterial(i25, i22 - 1, i19) != Material.ice)
                    {
                        this.worldObj.SetTile(i25, i22, i19, Tile.topSnow.id);
                    }
                }
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
            return "RandomLevelSource";
        }
    }
}