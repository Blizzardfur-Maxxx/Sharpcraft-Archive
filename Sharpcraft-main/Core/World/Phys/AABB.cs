using System.Collections.Generic;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Phys
{
    public class AABB
    {
        private static List<AABB> pool = new List<AABB>();
        private static int pointer = 0;
        public double x0;
        public double y0;
        public double z0;
        public double x1;
        public double y1;
        public double z1;

        public static AABB CreateAABB(double minX, double minY, double minZ, double maxX, double maxY, double maxZ)
        {
            return new AABB(minX, minY, minZ, maxX, maxY, maxZ);
        }

        public static void ClearAABBPool()
        {
            pool.Clear();
            pointer = 0;
        }

        public static void ClearBBPool()
        {
            pointer = 0;
        }

        public static AABB Of(double d0, double d2, double d4, double d6, double d8, double d10)
        {
            if (pointer >= pool.Count)
            {
                pool.Add(CreateAABB(0.0D, 0.0D, 0.0D, 0.0D, 0.0D, 0.0D));
            }

            return pool[pointer++].SetBounds(d0, d2, d4, d6, d8, d10);
        }

        private AABB(double d1, double d3, double d5, double d7, double d9, double d11)
        {
            x0 = d1;
            y0 = d3;
            z0 = d5;
            x1 = d7;
            y1 = d9;
            z1 = d11;
        }

        public AABB SetBounds(double d1, double d3, double d5, double d7, double d9, double d11)
        {
            x0 = d1;
            y0 = d3;
            z0 = d5;
            x1 = d7;
            y1 = d9;
            z1 = d11;
            return this;
        }

        public AABB AddCoord(double d1, double d3, double d5)
        {
            double d7 = x0;
            double d9 = y0;
            double d11 = z0;
            double d13 = x1;
            double d15 = y1;
            double d17 = z1;
            if (d1 < 0.0D)
            {
                d7 += d1;
            }

            if (d1 > 0.0D)
            {
                d13 += d1;
            }

            if (d3 < 0.0D)
            {
                d9 += d3;
            }

            if (d3 > 0.0D)
            {
                d15 += d3;
            }

            if (d5 < 0.0D)
            {
                d11 += d5;
            }

            if (d5 > 0.0D)
            {
                d17 += d5;
            }

            return Of(d7, d9, d11, d13, d15, d17);
        }

        public AABB Expand(double d1, double d3, double d5)
        {
            double d7 = x0 - d1;
            double d9 = y0 - d3;
            double d11 = z0 - d5;
            double d13 = x1 + d1;
            double d15 = y1 + d3;
            double d17 = z1 + d5;
            return Of(d7, d9, d11, d13, d15, d17);
        }

        public AABB GetOffsetBoundingBox(double d1, double d3, double d5)
        {
            return Of(x0 + d1, y0 + d3, z0 + d5, x1 + d1, y1 + d3, z1 + d5);
        }

        public double CalculateXOffset(AABB axisAlignedBB1, double d2)
        {
            if (axisAlignedBB1.y1 > y0 && axisAlignedBB1.y0 < y1)
            {
                if (axisAlignedBB1.z1 > z0 && axisAlignedBB1.z0 < z1)
                {
                    double d4;
                    if (d2 > 0.0D && axisAlignedBB1.x1 <= x0)
                    {
                        d4 = x0 - axisAlignedBB1.x1;
                        if (d4 < d2)
                        {
                            d2 = d4;
                        }
                    }

                    if (d2 < 0.0D && axisAlignedBB1.x0 >= x1)
                    {
                        d4 = x1 - axisAlignedBB1.x0;
                        if (d4 > d2)
                        {
                            d2 = d4;
                        }
                    }

                    return d2;
                }
                else
                {
                    return d2;
                }
            }
            else
            {
                return d2;
            }
        }

        public double CalculateYOffset(AABB axisAlignedBB1, double d2)
        {
            if (axisAlignedBB1.x1 > x0 && axisAlignedBB1.x0 < x1)
            {
                if (axisAlignedBB1.z1 > z0 && axisAlignedBB1.z0 < z1)
                {
                    double d4;
                    if (d2 > 0.0D && axisAlignedBB1.y1 <= y0)
                    {
                        d4 = y0 - axisAlignedBB1.y1;
                        if (d4 < d2)
                        {
                            d2 = d4;
                        }
                    }

                    if (d2 < 0.0D && axisAlignedBB1.y0 >= y1)
                    {
                        d4 = y1 - axisAlignedBB1.y0;
                        if (d4 > d2)
                        {
                            d2 = d4;
                        }
                    }

                    return d2;
                }
                else
                {
                    return d2;
                }
            }
            else
            {
                return d2;
            }
        }

        public double CalculateZOffset(AABB axisAlignedBB1, double d2)
        {
            if (axisAlignedBB1.x1 > x0 && axisAlignedBB1.x0 < x1)
            {
                if (axisAlignedBB1.y1 > y0 && axisAlignedBB1.y0 < y1)
                {
                    double d4;
                    if (d2 > 0.0D && axisAlignedBB1.z1 <= z0)
                    {
                        d4 = z0 - axisAlignedBB1.z1;
                        if (d4 < d2)
                        {
                            d2 = d4;
                        }
                    }

                    if (d2 < 0.0D && axisAlignedBB1.z0 >= z1)
                    {
                        d4 = z1 - axisAlignedBB1.z0;
                        if (d4 > d2)
                        {
                            d2 = d4;
                        }
                    }

                    return d2;
                }
                else
                {
                    return d2;
                }
            }
            else
            {
                return d2;
            }
        }

        public bool IntersectsWith(AABB axisAlignedBB1)
        {
            return axisAlignedBB1.x1 > x0 && axisAlignedBB1.x0 < x1 ? (axisAlignedBB1.y1 > y0 && axisAlignedBB1.y0 < y1 ? axisAlignedBB1.z1 > z0 && axisAlignedBB1.z0 < z1 : false) : false;
        }

        public AABB Offset(double d1, double d3, double d5)
        {
            x0 += d1;
            y0 += d3;
            z0 += d5;
            x1 += d1;
            y1 += d3;
            z1 += d5;
            return this;
        }

        public bool IsVecInside(Vec3 vec3D1)
        {
            return vec3D1.x > x0 && vec3D1.x < x1 ? (vec3D1.y > y0 && vec3D1.y < y1 ? vec3D1.z > z0 && vec3D1.z < z1 : false) : false;
        }

        public double GetAverageEdgeLength()
        {
            double d1 = x1 - x0;
            double d3 = y1 - y0;
            double d5 = z1 - z0;
            return (d1 + d3 + d5) / 3.0D;
        }

        public AABB GetInsetBoundingBox(double d1, double d3, double d5)
        {
            double d7 = x0 + d1;
            double d9 = y0 + d3;
            double d11 = z0 + d5;
            double d13 = x1 - d1;
            double d15 = y1 - d3;
            double d17 = z1 - d5;
            return Of(d7, d9, d11, d13, d15, d17);
        }

        public AABB Copy()
        {
            return Of(x0, y0, z0, x1, y1, z1);
        }

        public HitResult Clip(Vec3 vec3D1, Vec3 vec3D2)
        {
            Vec3 vec3D3 = vec3D1.GetIntermediateWithXValue(vec3D2, x0);
            Vec3 vec3D4 = vec3D1.GetIntermediateWithXValue(vec3D2, x1);
            Vec3 vec3D5 = vec3D1.GetIntermediateWithYValue(vec3D2, y0);
            Vec3 vec3D6 = vec3D1.GetIntermediateWithYValue(vec3D2, y1);
            Vec3 vec3D7 = vec3D1.GetIntermediateWithZValue(vec3D2, z0);
            Vec3 vec3D8 = vec3D1.GetIntermediateWithZValue(vec3D2, z1);

            if (!IsVecInYZ(vec3D3))
                vec3D3 = null;

            if (!IsVecInYZ(vec3D4))
                vec3D4 = null;

            if (!IsVecInXZ(vec3D5))
                vec3D5 = null;

            if (!IsVecInXZ(vec3D6))
                vec3D6 = null;

            if (!IsVecInXY(vec3D7))
                vec3D7 = null;

            if (!IsVecInXY(vec3D8))
                vec3D8 = null;

            Vec3 vec3D9 = null;
            if (vec3D3 != null && (vec3D9 == null || vec3D1.SquareDistanceTo(vec3D3) < vec3D1.SquareDistanceTo(vec3D9)))
                vec3D9 = vec3D3;

            if (vec3D4 != null && (vec3D9 == null || vec3D1.SquareDistanceTo(vec3D4) < vec3D1.SquareDistanceTo(vec3D9)))
                vec3D9 = vec3D4;

            if (vec3D5 != null && (vec3D9 == null || vec3D1.SquareDistanceTo(vec3D5) < vec3D1.SquareDistanceTo(vec3D9)))
                vec3D9 = vec3D5;

            if (vec3D6 != null && (vec3D9 == null || vec3D1.SquareDistanceTo(vec3D6) < vec3D1.SquareDistanceTo(vec3D9)))
                vec3D9 = vec3D6;

            if (vec3D7 != null && (vec3D9 == null || vec3D1.SquareDistanceTo(vec3D7) < vec3D1.SquareDistanceTo(vec3D9)))
                vec3D9 = vec3D7;

            if (vec3D8 != null && (vec3D9 == null || vec3D1.SquareDistanceTo(vec3D8) < vec3D1.SquareDistanceTo(vec3D9)))
                vec3D9 = vec3D8;

            if (vec3D9 == null)
                return null;
            else
            {
                TileFace b10 = TileFace.UNDEFINED;
                if (vec3D9 == vec3D3)
                    b10 = TileFace.WEST;

                if (vec3D9 == vec3D4)
                    b10 = TileFace.EAST;

                if (vec3D9 == vec3D5)
                    b10 = TileFace.DOWN;

                if (vec3D9 == vec3D6)
                    b10 = TileFace.UP;

                if (vec3D9 == vec3D7)
                    b10 = TileFace.NORTH;

                if (vec3D9 == vec3D8)
                    b10 = TileFace.SOUTH;

                return new HitResult(0, 0, 0, b10, vec3D9);
            }
        }

        private bool IsVecInYZ(Vec3 vec3D1)
        {
            return vec3D1 == null ? false : vec3D1.y >= y0 && vec3D1.y <= y1 && vec3D1.z >= z0 && vec3D1.z <= z1;
        }

        private bool IsVecInXZ(Vec3 vec3D1)
        {
            return vec3D1 == null ? false : vec3D1.x >= x0 && vec3D1.x <= x1 && vec3D1.z >= z0 && vec3D1.z <= z1;
        }

        private bool IsVecInXY(Vec3 vec3D1)
        {
            return vec3D1 == null ? false : vec3D1.x >= x0 && vec3D1.x <= x1 && vec3D1.y >= y0 && vec3D1.y <= y1;
        }

        public void SetBB(AABB bb)
        {
            x0 = bb.x0;
            y0 = bb.y0;
            z0 = bb.z0;
            x1 = bb.x1;
            y1 = bb.y1;
            z1 = bb.z1;
        }

        public override string ToString()
        {
            return $"AABB[{x0}, {y0}, {z0} -> {x1}, {y1}, {z1}]";
        }
    }
}
