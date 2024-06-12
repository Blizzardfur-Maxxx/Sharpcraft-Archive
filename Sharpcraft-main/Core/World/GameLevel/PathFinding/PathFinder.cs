using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.PathFinding
{
    public class PathFinder
    {
        private ILevelSource level;
        private BinaryHeap path = new BinaryHeap();
        private IntHashMap<Node> pointMap = new IntHashMap<Node>();
        private Node[] pathOptions = new Node[32];
        public PathFinder(ILevelSource level)
        {
            this.level = level;
        }

        public virtual Path CreateEntityPathTo(Entity entity1, Entity entity2, float f3)
        {
            return this.CreateEntityPathTo(entity1, entity2.x, entity2.boundingBox.y0, entity2.z, f3);
        }

        public virtual Path CreateEntityPathTo(Entity entity1, int i2, int i3, int i4, float f5)
        {
            return this.CreateEntityPathTo(entity1, i2 + 0.5F, i3 + 0.5F, i4 + 0.5F, f5);
        }

        private Path CreateEntityPathTo(Entity entity1, double d2, double d4, double d6, float f8)
        {
            this.path.Clear();
            this.pointMap.Clear();
            Node pathPoint9 = this.OpenPoint(Mth.Floor(entity1.boundingBox.x0), Mth.Floor(entity1.boundingBox.y0), Mth.Floor(entity1.boundingBox.z0));
            Node pathPoint10 = this.OpenPoint(Mth.Floor(d2 - entity1.width / 2F), Mth.Floor(d4), Mth.Floor(d6 - entity1.width / 2F));
            Node pathPoint11 = new Node(Mth.Floor(entity1.width + 1F), Mth.Floor(entity1.height + 1F), Mth.Floor(entity1.width + 1F));
            Path pathEntity12 = this.AddToPath(entity1, pathPoint9, pathPoint10, pathPoint11, f8);
            return pathEntity12;
        }

        private Path AddToPath(Entity entity1, Node pathPoint2, Node pathPoint3, Node pathPoint4, float f5)
        {
            pathPoint2.totalPathDistance = 0F;
            pathPoint2.distanceToNext = pathPoint2.DistanceTo(pathPoint3);
            pathPoint2.distanceToTarget = pathPoint2.distanceToNext;
            this.path.Clear();
            this.path.Insert(pathPoint2);
            Node pathPoint6 = pathPoint2;
            while (!this.path.IsEmpty())
            {
                Node pathPoint7 = this.path.Pop();
                if (pathPoint7.Equals(pathPoint3))
                {
                    return this.CreateEntityPath(pathPoint2, pathPoint3);
                }

                if (pathPoint7.DistanceTo(pathPoint3) < pathPoint6.DistanceTo(pathPoint3))
                {
                    pathPoint6 = pathPoint7;
                }

                pathPoint7.isFirst = true;
                int i8 = this.FindPathOptions(entity1, pathPoint7, pathPoint4, pathPoint3, f5);
                for (int i9 = 0; i9 < i8; ++i9)
                {
                    Node pathPoint10 = this.pathOptions[i9];
                    float f11 = pathPoint7.totalPathDistance + pathPoint7.DistanceTo(pathPoint10);
                    if (!pathPoint10.InOpenSet() || f11 < pathPoint10.totalPathDistance)
                    {
                        pathPoint10.previous = pathPoint7;
                        pathPoint10.totalPathDistance = f11;
                        pathPoint10.distanceToNext = pathPoint10.DistanceTo(pathPoint3);
                        if (pathPoint10.InOpenSet())
                        {
                            this.path.ChangeCost(pathPoint10, pathPoint10.totalPathDistance + pathPoint10.distanceToNext);
                        }
                        else
                        {
                            pathPoint10.distanceToTarget = pathPoint10.totalPathDistance + pathPoint10.distanceToNext;
                            this.path.Insert(pathPoint10);
                        }
                    }
                }
            }

            if (pathPoint6 == pathPoint2)
            {
                return null;
            }
            else
            {
                return this.CreateEntityPath(pathPoint2, pathPoint6);
            }
        }

        private int FindPathOptions(Entity entity1, Node pathPoint2, Node pathPoint3, Node pathPoint4, float f5)
        {
            int i6 = 0;
            byte b7 = 0;
            if (this.GetVerticalOffset(entity1, pathPoint2.xCoord, pathPoint2.yCoord + 1, pathPoint2.zCoord, pathPoint3) == 1)
            {
                b7 = 1;
            }

            Node pathPoint8 = this.GetSafePoint(entity1, pathPoint2.xCoord, pathPoint2.yCoord, pathPoint2.zCoord + 1, pathPoint3, b7);
            Node pathPoint9 = this.GetSafePoint(entity1, pathPoint2.xCoord - 1, pathPoint2.yCoord, pathPoint2.zCoord, pathPoint3, b7);
            Node pathPoint10 = this.GetSafePoint(entity1, pathPoint2.xCoord + 1, pathPoint2.yCoord, pathPoint2.zCoord, pathPoint3, b7);
            Node pathPoint11 = this.GetSafePoint(entity1, pathPoint2.xCoord, pathPoint2.yCoord, pathPoint2.zCoord - 1, pathPoint3, b7);
            if (pathPoint8 != null && !pathPoint8.isFirst && pathPoint8.DistanceTo(pathPoint4) < f5)
            {
                this.pathOptions[i6++] = pathPoint8;
            }

            if (pathPoint9 != null && !pathPoint9.isFirst && pathPoint9.DistanceTo(pathPoint4) < f5)
            {
                this.pathOptions[i6++] = pathPoint9;
            }

            if (pathPoint10 != null && !pathPoint10.isFirst && pathPoint10.DistanceTo(pathPoint4) < f5)
            {
                this.pathOptions[i6++] = pathPoint10;
            }

            if (pathPoint11 != null && !pathPoint11.isFirst && pathPoint11.DistanceTo(pathPoint4) < f5)
            {
                this.pathOptions[i6++] = pathPoint11;
            }

            return i6;
        }

        private Node GetSafePoint(Entity entity1, int i2, int i3, int i4, Node pathPoint5, int i6)
        {
            Node pathPoint7 = null;
            if (this.GetVerticalOffset(entity1, i2, i3, i4, pathPoint5) == 1)
            {
                pathPoint7 = this.OpenPoint(i2, i3, i4);
            }

            if (pathPoint7 == null && i6 > 0 && this.GetVerticalOffset(entity1, i2, i3 + i6, i4, pathPoint5) == 1)
            {
                pathPoint7 = this.OpenPoint(i2, i3 + i6, i4);
                i3 += i6;
            }

            if (pathPoint7 != null)
            {
                int i8 = 0;
                int i9 = 0;
                while (i3 > 0 && (i9 = this.GetVerticalOffset(entity1, i2, i3 - 1, i4, pathPoint5)) == 1)
                {
                    ++i8;
                    if (i8 >= 4)
                    {
                        return null;
                    }

                    --i3;
                    if (i3 > 0)
                    {
                        pathPoint7 = this.OpenPoint(i2, i3, i4);
                    }
                }

                if (i9 == -2)
                {
                    return null;
                }
            }

            return pathPoint7;
        }

        private Node OpenPoint(int i1, int i2, int i3)
        {
            int i4 = Node.CreateHash(i1, i2, i3);
            Node pathPoint5 = this.pointMap.Get(i4);
            if (pathPoint5 == null)
            {
                pathPoint5 = new Node(i1, i2, i3);
                this.pointMap.Put(i4, pathPoint5);
            }

            return pathPoint5;
        }

        private int GetVerticalOffset(Entity entity1, int i2, int i3, int i4, Node pathPoint5)
        {
            for (int i6 = i2; i6 < i2 + pathPoint5.xCoord; ++i6)
            {
                for (int i7 = i3; i7 < i3 + pathPoint5.yCoord; ++i7)
                {
                    for (int i8 = i4; i8 < i4 + pathPoint5.zCoord; ++i8)
                    {
                        int i9 = this.level.GetTile(i6, i7, i8);
                        if (i9 > 0)
                        {
                            if (i9 != Tile.door_iron.id && i9 != Tile.doorWood.id)
                            {
                                Material material11 = Tile.tiles[i9].material;
                                if (material11.BlocksMotion())
                                {
                                    return 0;
                                }

                                if (material11 == Material.water)
                                {
                                    return -1;
                                }

                                if (material11 == Material.lava)
                                {
                                    return -2;
                                }
                            }
                            else
                            {
                                int i10 = this.level.GetData(i6, i7, i8);
                                if (!DoorTile.IsOpen(i10))
                                {
                                    return 0;
                                }
                            }
                        }
                    }
                }
            }

            return 1;
        }

        private Path CreateEntityPath(Node pathPoint1, Node pathPoint2)
        {
            int i3 = 1;
            Node pathPoint4;
            for (pathPoint4 = pathPoint2; pathPoint4.previous != null; pathPoint4 = pathPoint4.previous)
            {
                ++i3;
            }

            Node[] pathPoint5 = new Node[i3];
            pathPoint4 = pathPoint2;
            --i3;
            for (pathPoint5[i3] = pathPoint2; pathPoint4.previous != null; pathPoint5[i3] = pathPoint4)
            {
                pathPoint4 = pathPoint4.previous;
                --i3;
            }

            return new Path(pathPoint5);
        }
    }
}