using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Biomes;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.LevelGen.Features;
using SharpCraft.Core.World.GameLevel.LevelGen.Synthetic;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen
{
    public class SkyRandomLevelSource : IChunkSource
    {
        private JRandom random;
        private OctavePerlinNoise noise16_0;
        private OctavePerlinNoise noise16_1;
        private OctavePerlinNoise noise8_0;
        private OctavePerlinNoise noise4_0;
        private OctavePerlinNoise noise4_1;
        public OctavePerlinNoise noise10_0;
        public OctavePerlinNoise noise16_2;
        public OctavePerlinNoise noise8_1;
        private Level level;
        private double[] field_q;
        private double[] field_r = new double[256];
        private double[] field_s = new double[256];
        private double[] field_t = new double[256];
        private LargeFeature caveCarver = new LargeCaveFeature();
        private Biome[] biomes;
        double[] field_d;
        double[] field_e;
        double[] field_f;
        double[] field_g;
        double[] field_h;
        int[,] field_i = new int[32, 32];
        private double[] field_w;
        public SkyRandomLevelSource(Level world1, long j2)
        {
            this.level = world1;
            this.random = new JRandom(j2);
            this.noise16_0 = new OctavePerlinNoise(this.random, 16);
            this.noise16_1 = new OctavePerlinNoise(this.random, 16);
            this.noise8_0 = new OctavePerlinNoise(this.random, 8);
            this.noise4_0 = new OctavePerlinNoise(this.random, 4);
            this.noise4_1 = new OctavePerlinNoise(this.random, 4);
            this.noise10_0 = new OctavePerlinNoise(this.random, 10);
            this.noise16_2 = new OctavePerlinNoise(this.random, 16);
            this.noise8_1 = new OctavePerlinNoise(this.random, 8);
        }

        public virtual void Func_a(int i1, int i2, byte[] b3, Biome[] biomeGenBase4, double[] d5)
        {
            byte b6 = 2;
            int i7 = b6 + 1;
            byte b8 = 33;
            int i9 = b6 + 1;
            this.field_q = this.Func_c(this.field_q, i1 * b6, 0, i2 * b6, i7, b8, i9);
            for (int i10 = 0; i10 < b6; ++i10)
            {
                for (int i11 = 0; i11 < b6; ++i11)
                {
                    for (int i12 = 0; i12 < 32; ++i12)
                    {
                        double d13 = 0.25;
                        double d15 = this.field_q[((i10 + 0) * i9 + i11 + 0) * b8 + i12 + 0];
                        double d17 = this.field_q[((i10 + 0) * i9 + i11 + 1) * b8 + i12 + 0];
                        double d19 = this.field_q[((i10 + 1) * i9 + i11 + 0) * b8 + i12 + 0];
                        double d21 = this.field_q[((i10 + 1) * i9 + i11 + 1) * b8 + i12 + 0];
                        double d23 = (this.field_q[((i10 + 0) * i9 + i11 + 0) * b8 + i12 + 1] - d15) * d13;
                        double d25 = (this.field_q[((i10 + 0) * i9 + i11 + 1) * b8 + i12 + 1] - d17) * d13;
                        double d27 = (this.field_q[((i10 + 1) * i9 + i11 + 0) * b8 + i12 + 1] - d19) * d13;
                        double d29 = (this.field_q[((i10 + 1) * i9 + i11 + 1) * b8 + i12 + 1] - d21) * d13;
                        for (int i31 = 0; i31 < 4; ++i31)
                        {
                            double d32 = 0.125;
                            double d34 = d15;
                            double d36 = d17;
                            double d38 = (d19 - d15) * d32;
                            double d40 = (d21 - d17) * d32;
                            for (int i42 = 0; i42 < 8; ++i42)
                            {
                                int i43 = i42 + i10 * 8 << 11 | 0 + i11 * 8 << 7 | i12 * 4 + i31;
                                short s44 = 128;
                                double d45 = 0.125;
                                double d47 = d34;
                                double d49 = (d36 - d34) * d45;
                                for (int i51 = 0; i51 < 8; ++i51)
                                {
                                    int i52 = 0;
                                    if (d47 > 0)
                                    {
                                        i52 = Tile.rock.id;
                                    }

                                    b3[i43] = (byte)i52;
                                    i43 += s44;
                                    d47 += d49;
                                }

                                d34 += d38;
                                d36 += d40;
                            }

                            d15 += d23;
                            d17 += d25;
                            d19 += d27;
                            d21 += d29;
                        }
                    }
                }
            }
        }

        public virtual void Func_b(int i1, int i2, byte[] b3, Biome[] biomeGenBase4)
        {
            double d5 = 8 / 256;
            this.field_r = this.noise4_0.GenerateNoiseOctaves(this.field_r, i1 * 16, i2 * 16, 0, 16, 16, 1, d5, d5, 1);
            this.field_s = this.noise4_0.GenerateNoiseOctaves(this.field_s, i1 * 16, 109.0134, i2 * 16, 16, 1, 16, d5, 1, d5);
            this.field_t = this.noise4_1.GenerateNoiseOctaves(this.field_t, i1 * 16, i2 * 16, 0, 16, 16, 1, d5 * 2, d5 * 2, d5 * 2);
            for (int i7 = 0; i7 < 16; ++i7)
            {
                for (int i8 = 0; i8 < 16; ++i8)
                {
                    Biome biomeGenBase9 = biomeGenBase4[i7 + i8 * 16];
                    int i10 = (int)(this.field_t[i7 + i8 * 16] / 3 + 3 + this.random.NextDouble() * 0.25);
                    int i11 = -1;
                    byte b12 = biomeGenBase9.topTile;
                    byte b13 = biomeGenBase9.fillerTile;
                    for (int i14 = 127; i14 >= 0; --i14)
                    {
                        int i15 = (i8 * 16 + i7) * 128 + i14;
                        byte b16 = b3[i15];
                        if (b16 == 0)
                        {
                            i11 = -1;
                        }
                        else if (b16 == Tile.rock.id)
                        {
                            if (i11 == -1)
                            {
                                if (i10 <= 0)
                                {
                                    b12 = 0;
                                    b13 = (byte)Tile.rock.id;
                                }

                                i11 = i10;
                                if (i14 >= 0)
                                {
                                    b3[i15] = b12;
                                }
                                else
                                {
                                    b3[i15] = b13;
                                }
                            }
                            else if (i11 > 0)
                            {
                                --i11;
                                b3[i15] = b13;
                                if (i11 == 0 && b13 == Tile.sand.id)
                                {
                                    i11 = this.random.NextInt(4);
                                    b13 = (byte)Tile.sandStone.id;
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
            this.random.SetSeed(i1 * 341873128712 + i2 * 132897987541);
            byte[] b3 = new byte[32768];
            LevelChunk chunk4 = new LevelChunk(this.level, b3, i1, i2);
            this.biomes = this.level.GetBiomeSource().LoadBlockGeneratorData(this.biomes, i1 * 16, i2 * 16, 16, 16);
            double[] d5 = this.level.GetBiomeSource().temperature;
            this.Func_a(i1, i2, b3, this.biomes, d5);
            this.Func_b(i1, i2, b3, this.biomes);
            this.caveCarver.Apply(this, this.level, i1, i2, b3);
            chunk4.CalculateLight();
            return chunk4;
        }

        private double[] Func_c(double[] d1, int i2, int i3, int i4, int i5, int i6, int i7)
        {
            if (d1 == null)
            {
                d1 = new double[i5 * i6 * i7];
            }

            double d8 = 684.412;
            double d10 = 684.412;
            double[] d12 = this.level.GetBiomeSource().temperature;
            double[] d13 = this.level.GetBiomeSource().humidity;
            this.field_g = this.noise10_0.GetRegion(this.field_g, i2, i4, i5, i7, 1.121, 1.121, 0.5);
            this.field_h = this.noise16_2.GetRegion(this.field_h, i2, i4, i5, i7, 200, 200, 0.5);
            d8 *= 2;
            this.field_d = this.noise8_0.GenerateNoiseOctaves(this.field_d, i2, i3, i4, i5, i6, i7, d8 / 80, d10 / 160, d8 / 80);
            this.field_e = this.noise16_0.GenerateNoiseOctaves(this.field_e, i2, i3, i4, i5, i6, i7, d8, d10, d8);
            this.field_f = this.noise16_1.GenerateNoiseOctaves(this.field_f, i2, i3, i4, i5, i6, i7, d8, d10, d8);
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
                    double d25 = 1 - d23;
                    d25 *= d25;
                    d25 *= d25;
                    d25 = 1 - d25;
                    double d27 = (this.field_g[i15] + 256) / 512;
                    d27 *= d25;
                    if (d27 > 1)
                    {
                        d27 = 1;
                    }

                    double d29 = this.field_h[i15] / 8000;
                    if (d29 < 0)
                    {
                        d29 = -d29 * 0.3;
                    }

                    d29 = d29 * 3 - 2;
                    if (d29 > 1)
                    {
                        d29 = 1;
                    }

                    d29 /= 8;
                    d29 = 0;
                    if (d27 < 0)
                    {
                        d27 = 0;
                    }

                    d27 += 0.5;
                    d29 = d29 * i6 / 16;
                    ++i15;
                    double d31 = i6 / 2;
                    for (int i33 = 0; i33 < i6; ++i33)
                    {
                        double d34 = 0;
                        double d36 = (i33 - d31) * 8 / d27;
                        if (d36 < 0)
                        {
                            d36 *= -1;
                        }

                        double d38 = this.field_e[i14] / 512;
                        double d40 = this.field_f[i14] / 512;
                        double d42 = (this.field_d[i14] / 10 + 1) / 2;
                        if (d42 < 0)
                        {
                            d34 = d38;
                        }
                        else if (d42 > 1)
                        {
                            d34 = d40;
                        }
                        else
                        {
                            d34 = d38 + (d40 - d38) * d42;
                        }

                        d34 -= 8;
                        byte b44 = 32;
                        double d45;
                        if (i33 > i6 - b44)
                        {
                            d45 = (i33 - (i6 - b44)) / (b44 - 1F);
                            d34 = d34 * (1 - d45) + -30 * d45;
                        }

                        b44 = 8;
                        if (i33 < b44)
                        {
                            d45 = (b44 - i33) / (b44 - 1F);
                            d34 = d34 * (1 - d45) + -30 * d45;
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
            Biome biomeGenBase6 = this.level.GetBiomeSource().GetBiomeGenAt(i4 + 16, i5 + 16);
            this.random.SetSeed(this.level.GetRandomSeed());
            long j7 = this.random.NextLong() / 2 * 2 + 1;
            long j9 = this.random.NextLong() / 2 * 2 + 1;
            this.random.SetSeed(i2 * j7 + i3 * j9 ^ this.level.GetRandomSeed());
            double d11 = 0.25;
            int i13;
            int i14;
            int i15;
            if (this.random.NextInt(4) == 0)
            {
                i13 = i4 + this.random.NextInt(16) + 8;
                i14 = this.random.NextInt(128);
                i15 = i5 + this.random.NextInt(16) + 8;
                (new LakeFeature(Tile.calmWater.id)).Place(this.level, this.random, i13, i14, i15);
            }

            if (this.random.NextInt(8) == 0)
            {
                i13 = i4 + this.random.NextInt(16) + 8;
                i14 = this.random.NextInt(this.random.NextInt(120) + 8);
                i15 = i5 + this.random.NextInt(16) + 8;
                if (i14 < 64 || this.random.NextInt(10) == 0)
                {
                    (new LakeFeature(Tile.calmLava.id)).Place(this.level, this.random, i13, i14, i15);
                }
            }

            int i16;
            for (i13 = 0; i13 < 8; ++i13)
            {
                i14 = i4 + this.random.NextInt(16) + 8;
                i15 = this.random.NextInt(128);
                i16 = i5 + this.random.NextInt(16) + 8;
                (new MonsterRoomFeature()).Place(this.level, this.random, i14, i15, i16);
            }

            for (i13 = 0; i13 < 10; ++i13)
            {
                i14 = i4 + this.random.NextInt(16);
                i15 = this.random.NextInt(128);
                i16 = i5 + this.random.NextInt(16);
                (new ClayFeature(32)).Place(this.level, this.random, i14, i15, i16);
            }

            for (i13 = 0; i13 < 20; ++i13)
            {
                i14 = i4 + this.random.NextInt(16);
                i15 = this.random.NextInt(128);
                i16 = i5 + this.random.NextInt(16);
                (new OreFeature(Tile.dirt.id, 32)).Place(this.level, this.random, i14, i15, i16);
            }

            for (i13 = 0; i13 < 10; ++i13)
            {
                i14 = i4 + this.random.NextInt(16);
                i15 = this.random.NextInt(128);
                i16 = i5 + this.random.NextInt(16);
                (new OreFeature(Tile.gravel.id, 32)).Place(this.level, this.random, i14, i15, i16);
            }

            for (i13 = 0; i13 < 20; ++i13)
            {
                i14 = i4 + this.random.NextInt(16);
                i15 = this.random.NextInt(128);
                i16 = i5 + this.random.NextInt(16);
                (new OreFeature(Tile.coalOre.id, 16)).Place(this.level, this.random, i14, i15, i16);
            }

            for (i13 = 0; i13 < 20; ++i13)
            {
                i14 = i4 + this.random.NextInt(16);
                i15 = this.random.NextInt(64);
                i16 = i5 + this.random.NextInt(16);
                (new OreFeature(Tile.ironOre.id, 8)).Place(this.level, this.random, i14, i15, i16);
            }

            for (i13 = 0; i13 < 2; ++i13)
            {
                i14 = i4 + this.random.NextInt(16);
                i15 = this.random.NextInt(32);
                i16 = i5 + this.random.NextInt(16);
                (new OreFeature(Tile.goldOre.id, 8)).Place(this.level, this.random, i14, i15, i16);
            }

            for (i13 = 0; i13 < 8; ++i13)
            {
                i14 = i4 + this.random.NextInt(16);
                i15 = this.random.NextInt(16);
                i16 = i5 + this.random.NextInt(16);
                (new OreFeature(Tile.oreRedstone.id, 7)).Place(this.level, this.random, i14, i15, i16);
            }

            for (i13 = 0; i13 < 1; ++i13)
            {
                i14 = i4 + this.random.NextInt(16);
                i15 = this.random.NextInt(16);
                i16 = i5 + this.random.NextInt(16);
                (new OreFeature(Tile.oreDiamond.id, 7)).Place(this.level, this.random, i14, i15, i16);
            }

            for (i13 = 0; i13 < 1; ++i13)
            {
                i14 = i4 + this.random.NextInt(16);
                i15 = this.random.NextInt(16) + this.random.NextInt(16);
                i16 = i5 + this.random.NextInt(16);
                (new OreFeature(Tile.lapisOre.id, 6)).Place(this.level, this.random, i14, i15, i16);
            }

            d11 = 0.5;
            i13 = (int)((this.noise8_1.GetValue(i4 * d11, i5 * d11) / 8 + this.random.NextDouble() * 4 + 4) / 3);
            i14 = 0;
            if (this.random.NextInt(10) == 0)
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
                i16 = i4 + this.random.NextInt(16) + 8;
                i17 = i5 + this.random.NextInt(16) + 8;
                Feature worldGenerator18 = biomeGenBase6.GetTreeFeature(this.random);
                worldGenerator18.Init(1, 1, 1);
                worldGenerator18.Place(this.level, this.random, i16, this.level.GetHeightValue(i16, i17), i17);
            }

            int i23;
            for (i15 = 0; i15 < 2; ++i15)
            {
                i16 = i4 + this.random.NextInt(16) + 8;
                i17 = this.random.NextInt(128);
                i23 = i5 + this.random.NextInt(16) + 8;
                (new FlowerFeature(Tile.flower.id)).Place(this.level, this.random, i16, i17, i23);
            }

            if (this.random.NextInt(2) == 0)
            {
                i15 = i4 + this.random.NextInt(16) + 8;
                i16 = this.random.NextInt(128);
                i17 = i5 + this.random.NextInt(16) + 8;
                (new FlowerFeature(Tile.rose.id)).Place(this.level, this.random, i15, i16, i17);
            }

            if (this.random.NextInt(4) == 0)
            {
                i15 = i4 + this.random.NextInt(16) + 8;
                i16 = this.random.NextInt(128);
                i17 = i5 + this.random.NextInt(16) + 8;
                (new FlowerFeature(Tile.mushroom1.id)).Place(this.level, this.random, i15, i16, i17);
            }

            if (this.random.NextInt(8) == 0)
            {
                i15 = i4 + this.random.NextInt(16) + 8;
                i16 = this.random.NextInt(128);
                i17 = i5 + this.random.NextInt(16) + 8;
                (new FlowerFeature(Tile.mushroom2.id)).Place(this.level, this.random, i15, i16, i17);
            }

            for (i15 = 0; i15 < 10; ++i15)
            {
                i16 = i4 + this.random.NextInt(16) + 8;
                i17 = this.random.NextInt(128);
                i23 = i5 + this.random.NextInt(16) + 8;
                (new ReedsFeature()).Place(this.level, this.random, i16, i17, i23);
            }

            if (this.random.NextInt(32) == 0)
            {
                i15 = i4 + this.random.NextInt(16) + 8;
                i16 = this.random.NextInt(128);
                i17 = i5 + this.random.NextInt(16) + 8;
                (new PumpkinFeature()).Place(this.level, this.random, i15, i16, i17);
            }

            i15 = 0;
            if (biomeGenBase6 == Biome.desert)
            {
                i15 += 10;
            }

            int i19;
            for (i16 = 0; i16 < i15; ++i16)
            {
                i17 = i4 + this.random.NextInt(16) + 8;
                i23 = this.random.NextInt(128);
                i19 = i5 + this.random.NextInt(16) + 8;
                (new CactusFeature()).Place(this.level, this.random, i17, i23, i19);
            }

            for (i16 = 0; i16 < 50; ++i16)
            {
                i17 = i4 + this.random.NextInt(16) + 8;
                i23 = this.random.NextInt(this.random.NextInt(120) + 8);
                i19 = i5 + this.random.NextInt(16) + 8;
                (new SpringFeature(Tile.water.id)).Place(this.level, this.random, i17, i23, i19);
            }

            for (i16 = 0; i16 < 20; ++i16)
            {
                i17 = i4 + this.random.NextInt(16) + 8;
                i23 = this.random.NextInt(this.random.NextInt(this.random.NextInt(112) + 8) + 8);
                i19 = i5 + this.random.NextInt(16) + 8;
                (new SpringFeature(Tile.lava.id)).Place(this.level, this.random, i17, i23, i19);
            }

            this.field_w = this.level.GetBiomeSource().GetTemperatures(this.field_w, i4 + 8, i5 + 8, 16, 16);
            for (i16 = i4 + 8; i16 < i4 + 8 + 16; ++i16)
            {
                for (i17 = i5 + 8; i17 < i5 + 8 + 16; ++i17)
                {
                    i23 = i16 - (i4 + 8);
                    i19 = i17 - (i5 + 8);
                    int i20 = this.level.GetTopSolidBlock(i16, i17);
                    double d21 = this.field_w[i23 * 16 + i19] - (i20 - 64) / 64 * 0.3;
                    if (d21 < 0.5 && i20 > 0 && i20 < 128 && this.level.IsAirBlock(i16, i20, i17) && this.level.GetMaterial(i16, i20 - 1, i17).BlocksMotion() && this.level.GetMaterial(i16, i20 - 1, i17) != Material.ice)
                    {
                        this.level.SetTile(i16, i20, i17, Tile.topSnow.id);
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
            return "SkyRandomLevelSource";
        }
    }
}