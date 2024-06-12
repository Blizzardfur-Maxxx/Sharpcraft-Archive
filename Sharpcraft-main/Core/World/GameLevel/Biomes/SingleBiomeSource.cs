using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk;

namespace SharpCraft.Core.World.GameLevel.Biomes
{
    public class SingleBiomeSource : BiomeSource
    {
        private Biome biome;
        private double temp;
        private double humd;
        public SingleBiomeSource(Biome biom, double temp, double humd)
        {
            this.biome = biom;
            this.temp = temp;
            this.humd = humd;
        }

        public override Biome GetBiomeGenAt(ChunkPos chunkCoordIntPair1)
        {
            return this.biome;
        }

        public override Biome GetBiomeGenAt(int i1, int i2)
        {
            return this.biome;
        }

        public override double GetTemperature(int i1, int i2)
        {
            return this.temp;
        }

        public override Biome[] Func_a(int i1, int i2, int i3, int i4)
        {
            this.field_d = this.LoadBlockGeneratorData(this.field_d, i1, i2, i3, i4);
            return this.field_d;
        }

        public override double[] GetTemperatures(double[] d1, int i2, int i3, int i4, int i5)
        {
            if (d1 == null || d1.Length < i4 * i5)
            {
                d1 = new double[i4 * i5];
            }

            ArrayUtil.Fill(d1, 0, i4 * i5, this.temp);
            return d1;
        }

        public override Biome[] LoadBlockGeneratorData(Biome[] biomeGenBase1, int i2, int i3, int i4, int i5)
        {
            if (biomeGenBase1 == null || biomeGenBase1.Length < i4 * i5)
            {
                biomeGenBase1 = new Biome[i4 * i5];
            }

            if (this.temperature == null || this.temperature.Length < i4 * i5)
            {
                this.temperature = new double[i4 * i5];
                this.humidity = new double[i4 * i5];
            }

            ArrayUtil.Fill(biomeGenBase1, 0, i4 * i5, this.biome);
            ArrayUtil.Fill(this.humidity, 0, i4 * i5, this.humd);
            ArrayUtil.Fill(this.temperature, 0, i4 * i5, this.temp);
            return biomeGenBase1;
        }
    }
}