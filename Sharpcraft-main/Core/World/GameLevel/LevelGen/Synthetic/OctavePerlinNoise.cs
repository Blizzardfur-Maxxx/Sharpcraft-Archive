using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Synthetic
{
    public class OctavePerlinNoise : Synth
    {
        private PerlinNoise[] generatorCollection;
        private int nOctaves;

        public OctavePerlinNoise(JRandom random1, int i2)
        {
            nOctaves = i2;
            generatorCollection = new PerlinNoise[i2];

            for (int i3 = 0; i3 < i2; ++i3)
                generatorCollection[i3] = new PerlinNoise(random1);

        }

        public double GetValue(double d1, double d3)
        {
            double d5 = 0.0D;
            double d7 = 1.0D;

            for (int i9 = 0; i9 < this.nOctaves; ++i9)
            {
                d5 += this.generatorCollection[i9].GetValue(d1 * d7, d3 * d7) / d7;
                d7 /= 2.0D;
            }

            return d5;
        }

        public double[] GenerateNoiseOctaves(double[] d1, double d2, double d4, double d6, int i8, int i9, int i10, double d11, double d13, double d15)
        {
            if (d1 == null)
            {
                d1 = new double[i8 * i9 * i10];
            }
            else
            {
                for (int i17 = 0; i17 < d1.Length; ++i17)
                {
                    d1[i17] = 0.0D;
                }
            }

            double d20 = 1.0D;

            for (int i19 = 0; i19 < nOctaves; ++i19)
            {
                generatorCollection[i19].GetRegion(d1, d2, d4, d6, i8, i9, i10, d11 * d20, d13 * d20, d15 * d20, d20);
                d20 /= 2.0D;
            }

            return d1;
        }

        public double[] GetRegion(double[] d1, int i2, int i3, int i4, int i5, double d6, double d8, double d10)
        {
            return GenerateNoiseOctaves(d1, i2, 10.0D, i3, i4, 1, i5, d6, 1.0D, d8);
        }
    }
}
