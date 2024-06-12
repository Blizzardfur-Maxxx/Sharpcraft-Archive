using System;

namespace SharpCraft.Core.Util
{
    public class JRandom
    {
        private const long multiplier = 0x5DEECE66DL;
        private const long addend = 0xBL;
        private const long mask = ((1L << 48) - 1);
        private const long seedUniqufier = 8682522807148012L;
        private long seed;
        private double nextNextGaussian;
        private bool haveNextNextGaussian = false;

        public JRandom() : this(seedUniqufier ^ TimeUtil.NanoTime)
        {
        }

        public JRandom(long seed)
        {
            SetSeed(seed);
        }

        public virtual void SetSeed(long seed)
        {
            this.seed = (seed ^ multiplier) & mask;
            haveNextNextGaussian = false;
        }

        protected virtual int Next(int bits)
        {
            seed = (seed * multiplier + addend) & mask;

            return (int)((ulong)seed >> (48 - bits));
        }

        public virtual void NextBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length;)
            {
                for (int rnd = NextInt(), n = Math.Min(bytes.Length - i, 4); n-- > 0; rnd >>= 8)
                {
                    bytes[i++] = (byte)rnd;
                }
            }
        }

        public virtual int NextInt()
        {
            return Next(32);
        }

        public virtual int NextInt(int n)
        {
            if (n <= 0) throw new ArgumentException("n must be positive");

            if ((n & -n) == n) return (int)((n * (long)Next(31)) >> 31);

            int bits, val;

            do
            {
                bits = Next(31);
                val = bits % n;
            } while (bits - val + (n - 1) < 0);

            return val;
        }

        public virtual long NextLong()
        {
            return ((long)Next(32) << 32) + Next(32);
        }

        public virtual bool NextBoolean()
        {
            return Next(1) != 0;
        }

        public virtual float NextFloat()
        {
            return Next(24) / ((float)(1 << 24));
        }

        public virtual double NextDouble()
        {
            return (((long)Next(26) << 27) + Next(27)) / (double)(1L << 53);
        }

        public virtual double NextGaussian()
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
    }
}
