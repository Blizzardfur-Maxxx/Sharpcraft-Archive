using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.LevelGen.Synthetic;
using System;

namespace SharpCraft.Core.World.GameLevel.Biomes
{
    public class BiomeSource
    {
        private OctaveSimplexNoise field_e;
        private OctaveSimplexNoise field_f;
        private OctaveSimplexNoise field_g;
        public double[] temperature;
        public double[] humidity;
        public double[] field_c;
        public Biome[] field_d;
        protected BiomeSource()
        {
        }

        public BiomeSource(Level world1)
        {
            this.field_e = new OctaveSimplexNoise(new JRandom(world1.GetRandomSeed() * 9871), 4);
            this.field_f = new OctaveSimplexNoise(new JRandom(world1.GetRandomSeed() * 39811), 4);
            this.field_g = new OctaveSimplexNoise(new JRandom(world1.GetRandomSeed() * 543321), 2);
        }

        public virtual Biome GetBiomeGenAt(ChunkPos chunkCoordIntPair1)
        {
            return this.GetBiomeGenAt(chunkCoordIntPair1.x << 4, chunkCoordIntPair1.z << 4);
        }

        public virtual Biome GetBiomeGenAt(int i1, int i2)
        {
            return this.Func_a(i1, i2, 1, 1)[0];
        }

        public virtual double GetTemperature(int i1, int i2)
        {
            this.temperature = this.field_e.GetRegion(this.temperature, i1, i2, 1, 1, 0.02500000037252903, 0.02500000037252903, 0.5);
            return this.temperature[0];
        }

        public virtual Biome[] Func_a(int i1, int i2, int i3, int i4)
        {
            this.field_d = this.LoadBlockGeneratorData(this.field_d, i1, i2, i3, i4);
            return this.field_d;
        }

        public virtual double[] GetTemperatures(double[] d1, int i2, int i3, int i4, int i5)
        {
            if (d1 == null || d1.Length < i4 * i5)
            {
                d1 = new double[i4 * i5];
            }

            d1 = this.field_e.GetRegion(d1, i2, i3, i4, i5, 0.02500000037252903, 0.02500000037252903, 0.25);
            this.field_c = this.field_g.GetRegion(this.field_c, i2, i3, i4, i5, 0.25, 0.25, 0.5882352941176471);
            int i6 = 0;
            for (int i7 = 0; i7 < i4; ++i7)
            {
                for (int i8 = 0; i8 < i5; ++i8)
                {
                    double d9 = this.field_c[i6] * 1.1 + 0.5;
                    double d11 = 0.01;
                    double d13 = 1 - d11;
                    double d15 = (d1[i6] * 0.15 + 0.7) * d13 + d9 * d11;
                    d15 = 1 - (1 - d15) * (1 - d15);
                    if (d15 < 0)
                    {
                        d15 = 0;
                    }

                    if (d15 > 1)
                    {
                        d15 = 1;
                    }

                    d1[i6] = d15;
                    ++i6;
                }
            }

            return d1;
        }

        public virtual Biome[] LoadBlockGeneratorData(Biome[] biomeGenBase1, int i2, int i3, int i4, int i5)
        {
            if (biomeGenBase1 == null || biomeGenBase1.Length < i4 * i5)
            {
                biomeGenBase1 = new Biome[i4 * i5];
            }

            this.temperature = this.field_e.GetRegion(this.temperature, i2, i3, i4, i4, 0.02500000037252903, 0.02500000037252903, 0.25);
            this.humidity = this.field_f.GetRegion(this.humidity, i2, i3, i4, i4, 0.05F, 0.05F, 0.3333333333333333);
            this.field_c = this.field_g.GetRegion(this.field_c, i2, i3, i4, i4, 0.25, 0.25, 0.5882352941176471);
            int i6 = 0;
            for (int i7 = 0; i7 < i4; ++i7)
            {
                for (int i8 = 0; i8 < i5; ++i8)
                {
                    double d9 = this.field_c[i6] * 1.1 + 0.5;
                    double d11 = 0.01;
                    double d13 = 1 - d11;
                    double d15 = (this.temperature[i6] * 0.15 + 0.7) * d13 + d9 * d11;
                    d11 = 0.002;
                    d13 = 1 - d11;
                    double d17 = (this.humidity[i6] * 0.15 + 0.5) * d13 + d9 * d11;
                    d15 = 1 - (1 - d15) * (1 - d15);
                    if (d15 < 0)
                    {
                        d15 = 0;
                    }

                    if (d17 < 0)
                    {
                        d17 = 0;
                    }

                    if (d15 > 1)
                    {
                        d15 = 1;
                    }

                    if (d17 > 1)
                    {
                        d17 = 1;
                    }

                    this.temperature[i6] = d15;
                    this.humidity[i6] = d17;
                    biomeGenBase1[i6++] = Biome.GetBiome(d15, d17);
                }
            }

            return biomeGenBase1;
        }
    }
}