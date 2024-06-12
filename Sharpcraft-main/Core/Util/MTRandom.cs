using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core.Util
{
    public class MTRandom : JRandom
    {
        private const int N = 624;
        private const int M = 397;
        private const int MATRIX_A = unchecked((int)0x9908b0df);
        private const int UPPER_MASK = unchecked((int)0x80000000);
        private const int LOWER_MASK = 0x7fffffff;
        private static readonly int[] MAG_01 = { 0, MATRIX_A };
        private const double TWO_POW_M32 = 2.3283064365386963E-010D;
        private const int DEFAULT_SEED = 5489;
        private const int MAGIC_MASK1 = unchecked((int)0x9d2c5680);
        private const int MAGIC_MASK2 = unchecked((int)0xefc60000);
        private const int MAGIC_FACTOR1 = 0x6c078965;
        private readonly int[] mt;
        private int mti;
        private bool haveNextNextGaussian;
        private float nextNextGaussian;
        private int mtiFast;

        public MTRandom() : this(TimeUtil.MilliTime)
        {
        }
        public MTRandom(long seed) : this((int)seed)
        {
        }

        public MTRandom(int seed)
        {
            mt = new int[N];
            _setSeed(seed);
        }

        public override bool NextBoolean()
        {
            return (genRandInt32() & UPPER_MASK) != 0;
        }

        public override void NextBytes(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length;)
            {
                for (int rnd = NextInt(), n = Math.Min(bytes.Length - i, 4); n-- > 0; rnd >>= 8)
                {
                    bytes[i++] = (byte)rnd;
                }
            }
        }

        public override double NextDouble()
        {
            return genRandReal2();
        }

        public override float NextFloat()
        {
            return (float) genRandReal2();
        }

        public override double NextGaussian()
        {
            if (haveNextNextGaussian)
            {
                haveNextNextGaussian = false;
                return nextNextGaussian;
            }
            float v1;
            float v2;
            float s;
            do
            {
                v1 = NextFloat() * 2.0F - 1.0F;
                v2 = NextFloat() * 2.0F - 1.0F;
                s = v1 * v1 + v2 * v2;
            } while (s == 0.0F || s > 1.0F);
            float multiplier = (float)Math.Sqrt(-2F * (float)Math.Log(s) / s);
            nextNextGaussian = v2 * multiplier;
            haveNextNextGaussian = true;
            return v1 * multiplier;
        }

        public override int NextInt()
        {
            return genRandInt32() >>> 1;
        }

        public override int NextInt(int bound)
        {
            if (bound > 0)
            {
                return (int)(((genRandInt32()) & 0xffffffffL) % bound);
            }
            else
            {
                return 0;
            }
        }

        public override long NextLong()
        {
            return genRandInt32() & 0xffffffffL;
        }

        public override void SetSeed(long seed)
        {
            _setSeed((int)seed);
        }

        private void _setSeed(int s)
        {
            mti = 625;
            haveNextNextGaussian = false;
            nextNextGaussian = 0.0F;
            initGenRandFast(s);
        }

        private void initGenRand(int i)
        {
            mt[0] = i;
            for (mti = 1; mti < N; mti++)
            {
                mt[mti] = MAGIC_FACTOR1 * (mt[mti - 1] >>> 30 ^ mt[mti - 1]) + mti;
            }

            mtiFast = N;
        }

        private void initGenRandFast(int i)
        {
            mt[0] = i;
            for (mtiFast = 1; mtiFast <= M; mtiFast++)
            {
                mt[mtiFast] = MAGIC_FACTOR1 * (mt[mtiFast - 1] >>> 30 ^ mt[mtiFast - 1]) + mtiFast;
            }

            mti = N;
        }

        private int genRandInt32()
        {
            if (mti == N)
            {
                mti = 0;
            }
            else if (mti > N)
            {
                initGenRand(DEFAULT_SEED);
                mti = 0;
            }
            if (mti >= 227)
            {
                if (mti >= 623)
                {
                    mt[623] = MAG_01[mt[0] & 1] ^ (mt[0] & LOWER_MASK | mt[623] & UPPER_MASK) >>> 1 ^ mt[396];
                }
                else
                {
                    mt[mti] = MAG_01[mt[mti + 1] & 1] ^ (mt[mti + 1] & LOWER_MASK | mt[mti] & UPPER_MASK) >>> 1 ^ mt[mti - 227];
                }
            }
            else
            {
                mt[mti] = MAG_01[mt[mti + 1] & 1] ^ (mt[mti + 1] & LOWER_MASK | mt[mti] & UPPER_MASK) >>> 1 ^ mt[mti + M];
                if (mtiFast < N)
                {
                    mt[mtiFast] = MAGIC_FACTOR1 * (mt[mtiFast - 1] >>> 30 ^ mt[mtiFast - 1]) + mtiFast;
                    mtiFast++;
                }
            }
            int i = mt[mti++];
            i = (i ^ i >>> 11) << 7 & MAGIC_MASK1 ^ i ^ i >>> 11;
            i = i << 15 & MAGIC_MASK2 ^ i ^ (i << 15 & MAGIC_MASK2 ^ i) >>> 18;
            return i;
        }

        private double genRandReal2()
        {
            long i = genRandInt32() & 0xffffffffL;
            return (double)(i * TWO_POW_M32);
        }
    }
}
