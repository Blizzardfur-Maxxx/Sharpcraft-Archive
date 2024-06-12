using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core.Util
{
    public class CSRandom : JRandom
    {
        private bool haveNextNextGaussian = false;
        private double nextNextGaussian;
        private Random rand;

        public CSRandom() 
        {
            rand = new Random();
        }

        public CSRandom(long seed) //long used for backwards compatibility
        {
            rand = new Random((int)seed);
        }

        public override void NextBytes(byte[] bytes)
        {
            rand.NextBytes(bytes);
        }

        public override double NextDouble()
        {
            return rand.NextDouble();
        }

        public override float NextFloat()
        {
            return rand.NextSingle();
        }

        public override double NextGaussian()
        {
            if (haveNextNextGaussian)
            {
                haveNextNextGaussian = false;
                return nextNextGaussian;
            }
            else
            {
                double v1, v2, s;
                do
                {
                    v1 = 2 * NextDouble() - 1;
                    v2 = 2 * NextDouble() - 1;
                    s = v1 * v1 + v2 * v2;
                } while (s >= 1 || s == 0);
                double multiplier = Math.Sqrt(-2 * Math.Log(s) / s);
                nextNextGaussian = v2 * multiplier;
                haveNextNextGaussian = true;
                return v1 * multiplier;
            }
        }

        public override int NextInt()
        {
            return rand.Next();
        }

        public override int NextInt(int bound)
        {
            return rand.Next(bound);
        }

        public override long NextLong()
        {
            return rand.NextInt64();
        }

        public override void SetSeed(long seed)
        {
            rand = new Random((int)seed);
            haveNextNextGaussian = false;
        }

        public override bool NextBoolean() 
        {
            return rand.Next(1) != 0;
        }
    }
}
