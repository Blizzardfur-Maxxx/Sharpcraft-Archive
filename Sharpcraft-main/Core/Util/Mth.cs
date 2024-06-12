using System;

namespace SharpCraft.Core.Util
{
    // Meth
    public class Mth
    {
        public const float PI = (float)Math.PI;
        private static readonly float[] SINE_TABLE = new float[65536];
        private static readonly JRandom random = new JRandom();

        static Mth()
        {
            for (int i = 0; i < 65536; i++)
            {
                SINE_TABLE[i] = (float)Math.Sin(i * Math.PI * 2.0D / 65536.0D);
            }
        }

        public static float Sin(float value)
        {
            return SINE_TABLE[(int)(value * 10430.378F) & 65535];
        }

        public static float Cos(float value)
        {
            return SINE_TABLE[(int)(value * 10430.378F + 16384.0F) & 65535];
        }
        public static float Sqrt(float value)
        {
            return (float)Math.Sqrt(value);
        }

        public static float Sqrt(double value)
        {
            return (float)Math.Sqrt(value);
        }

        public static int Floor(float value)
        {
            int ival = (int)value;
            return value < ival ? ival - 1 : ival;
        }

        public static int Floor(double value)
        {
            int ival = (int)value;
            return value < ival ? ival - 1 : ival;
        }

        public static long LFloor(double value)
        {
            long lval = (long)value;
            return value < lval ? lval - 1 : lval;
        }

        public static float Abs(float value)
        {
            return value >= 0.0F ? value : -value;
        }

        public static double AbsMax(double a, double b)
        {
            if (a < 0.0D)
                a = -a;

            if (b < 0.0D)
                b = -b;

            return a > b ? a : b;
        }

        public static double Random()
        {
            return random.NextDouble();
        }

        public static int IntFloorDiv(int dividend, int divisor)
        {
            return dividend < 0 ? -((-dividend - 1) / divisor) - 1 : dividend / divisor;
        }

        public static int GetJHashCode(string value)
        {
            int hash = 0;
            if (value.Length > 0)
            {
                for (int i = 0; i < value.Length; i++)
                    hash = 31 * hash + value[i];
            }
            return hash;
        }
    }
}
