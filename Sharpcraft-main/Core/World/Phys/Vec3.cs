using SharpCraft.Core.Util;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Phys
{
    public class Vec3
    {
        private static List<Vec3> pool = new List<Vec3>();
        private static int pointer = 0;
        public double x;
        public double y;
        public double z;

        // Vec3Factory it shits out Vec3's
        // I approve - vlOd
        public static Vec3 CreateVec3(double x, double y, double z)
        {
            return new Vec3(x, y, z);
        }

        public static void ClearVec3Pool()
        {
            pool.Clear();
            pointer = 0;
        }

        public static void ClearV3Pool()
        {
            pointer = 0;
        }

        public static Vec3 Of(double x, double y, double z)
        {
            if (pointer >= pool.Count)
                pool.Add(CreateVec3(0.0D, 0.0D, 0.0D));

            return pool[pointer++].SetComponents(x, y, z);
        }

        private Vec3(double x, double y, double z)
        {
            if (x == -0.0D)
                x = 0.0D;

            if (y == -0.0D)
                y = 0.0D;

            if (z == -0.0D)
                z = 0.0D;

            this.x = x;
            this.y = y;
            this.z = z;
        }

        private Vec3 SetComponents(double d1, double d3, double d5)
        {
            x = d1;
            y = d3;
            z = d5;
            return this;
        }

        public Vec3 Subtract(Vec3 vec3D1)
        {
            return Of(vec3D1.x - x, vec3D1.y - y, vec3D1.z - z);
        }

        public Vec3 Normalize()
        {
            double d1 = Mth.Sqrt(x * x + y * y + z * z);
            return d1 < 1.0E-4D ? Of(0.0D, 0.0D, 0.0D) : Of(x / d1, y / d1, z / d1);
        }

        public Vec3 CrossProduct(Vec3 vec3D1)
        {
            return Of(y * vec3D1.z - z * vec3D1.y, z * vec3D1.x - x * vec3D1.z, x * vec3D1.y - y * vec3D1.x);
        }

        public Vec3 AddVector(double d1, double d3, double d5)
        {
            return Of(x + d1, y + d3, z + d5);
        }

        public double DistanceTo(Vec3 vec3D1)
        {
            double d2 = vec3D1.x - x;
            double d4 = vec3D1.y - y;
            double d6 = vec3D1.z - z;
            return Mth.Sqrt(d2 * d2 + d4 * d4 + d6 * d6);
        }

        public double SquareDistanceTo(Vec3 vec3D1)
        {
            double d2 = vec3D1.x - x;
            double d4 = vec3D1.y - y;
            double d6 = vec3D1.z - z;
            return d2 * d2 + d4 * d4 + d6 * d6;
        }

        public double SquareDistanceTo(double d1, double d3, double d5)
        {
            double d7 = d1 - x;
            double d9 = d3 - y;
            double d11 = d5 - z;
            return d7 * d7 + d9 * d9 + d11 * d11;
        }

        public double LengthVector()
        {
            return Mth.Sqrt(x * x + y * y + z * z);
        }

        public Vec3 GetIntermediateWithXValue(Vec3 vec3D1, double d2)
        {
            double d4 = vec3D1.x - x;
            double d6 = vec3D1.y - y;
            double d8 = vec3D1.z - z;

            if (d4 * d4 < 1.0000000116860974E-7D)
                return null;
            else
            {
                double d10 = (d2 - x) / d4;
                return d10 >= 0.0D && d10 <= 1.0D ? Of(x + d4 * d10, y + d6 * d10, z + d8 * d10) : null;
            }
        }

        public Vec3 GetIntermediateWithYValue(Vec3 vec3D1, double d2)
        {
            double d4 = vec3D1.x - x;
            double d6 = vec3D1.y - y;
            double d8 = vec3D1.z - z;

            if (d6 * d6 < 1.0000000116860974E-7D)
                return null;
            else
            {
                double d10 = (d2 - y) / d6;
                return d10 >= 0.0D && d10 <= 1.0D ? Of(x + d4 * d10, y + d6 * d10, z + d8 * d10) : null;
            }
        }

        public Vec3 GetIntermediateWithZValue(Vec3 vec3D1, double d2)
        {
            double d4 = vec3D1.x - x;
            double d6 = vec3D1.y - y;
            double d8 = vec3D1.z - z;

            // Value, ... fresh from my ass
            if (d8 * d8 < 1.0000000116860974E-7D)
                return null;
            else
            {
                double d10 = (d2 - z) / d8;
                return d10 >= 0.0D && d10 <= 1.0D ? Of(x + d4 * d10, y + d6 * d10, z + d8 * d10) : null;
            }
        }

        public void RotateAroundX(float f1)
        {
            float f2 = Mth.Cos(f1);
            float f3 = Mth.Sin(f1);
            double d4 = x;
            double d6 = y * f2 + z * f3;
            double d8 = z * f2 - y * f3;
            x = d4;
            y = d6;
            z = d8;
        }

        public void RotateAroundY(float f1)
        {
            float f2 = Mth.Cos(f1);
            float f3 = Mth.Sin(f1);
            double d4 = x * f2 + z * f3;
            double d6 = y;
            double d8 = z * f2 - x * f3;
            x = d4;
            y = d6;
            z = d8;
        }

        public override string ToString()
        {
            return $"Vec3({x}, {y}, {z})";
        }
    }
}
