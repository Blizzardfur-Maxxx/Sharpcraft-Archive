using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class LiquidTileDynamic : LiquidTile
    {
        int nAdjSrcs = 0;
        bool[] optimalDir = new bool[4];
        int[] flowCost = new int[4];
        public LiquidTileDynamic(int i1, Material material2) : base(i1, material2)
        {
        }

        private void SetStatic(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            world1.SetTileAndDataNoUpdate(i2, i3, i4, this.id + 1, i5);
            world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
            world1.SendTileUpdated(i2, i3, i4);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            int i6 = this.GetDepth(world1, i2, i3, i4);
            byte b7 = 1;
            if (this.material == Material.lava && !world1.dimension.isHellWorld)
            {
                b7 = 2;
            }

            bool z8 = true;
            int i10;
            if (i6 > 0)
            {
                sbyte b9 = -100;
                this.nAdjSrcs = 0;
                int i12 = this.GetHighest(world1, i2 - 1, i3, i4, b9);
                i12 = this.GetHighest(world1, i2 + 1, i3, i4, i12);
                i12 = this.GetHighest(world1, i2, i3, i4 - 1, i12);
                i12 = this.GetHighest(world1, i2, i3, i4 + 1, i12);
                i10 = i12 + b7;
                if (i10 >= 8 || i12 < 0)
                {
                    i10 = -1;
                }

                if (this.GetDepth(world1, i2, i3 + 1, i4) >= 0)
                {
                    int i11 = this.GetDepth(world1, i2, i3 + 1, i4);
                    if (i11 >= 8)
                    {
                        i10 = i11;
                    }
                    else
                    {
                        i10 = i11 + 8;
                    }
                }

                if (this.nAdjSrcs >= 2 && this.material == Material.water)
                {
                    if (world1.GetMaterial(i2, i3 - 1, i4).IsSolid())
                    {
                        i10 = 0;
                    }
                    else if (world1.GetMaterial(i2, i3 - 1, i4) == this.material && world1.GetData(i2, i3, i4) == 0)
                    {
                        i10 = 0;
                    }
                }

                if (this.material == Material.lava && i6 < 8 && i10 < 8 && i10 > i6 && random5.NextInt(4) != 0)
                {
                    i10 = i6;
                    z8 = false;
                }

                if (i10 != i6)
                {
                    i6 = i10;
                    if (i10 < 0)
                    {
                        world1.SetTile(i2, i3, i4, 0);
                    }
                    else
                    {
                        world1.SetData(i2, i3, i4, i10);
                        world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
                        world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                    }
                }
                else if (z8)
                {
                    this.SetStatic(world1, i2, i3, i4);
                }
            }
            else
            {
                this.SetStatic(world1, i2, i3, i4);
            }

            if (this.CanSpreadTo(world1, i2, i3 - 1, i4))
            {
                if (i6 >= 8)
                {
                    world1.SetTileAndData(i2, i3 - 1, i4, this.id, i6);
                }
                else
                {
                    world1.SetTileAndData(i2, i3 - 1, i4, this.id, i6 + 8);
                }
            }
            else if (i6 >= 0 && (i6 == 0 || this.IsWaterBlocking(world1, i2, i3 - 1, i4)))
            {
                bool[] z13 = this.GetOptimalFlowDirections(world1, i2, i3, i4);
                i10 = i6 + b7;
                if (i6 >= 8)
                {
                    i10 = 1;
                }

                if (i10 >= 8)
                {
                    return;
                }

                if (z13[0])
                {
                    this.FlowIntoBlock(world1, i2 - 1, i3, i4, i10);
                }

                if (z13[1])
                {
                    this.FlowIntoBlock(world1, i2 + 1, i3, i4, i10);
                }

                if (z13[2])
                {
                    this.FlowIntoBlock(world1, i2, i3, i4 - 1, i10);
                }

                if (z13[3])
                {
                    this.FlowIntoBlock(world1, i2, i3, i4 + 1, i10);
                }
            }
        }

        private void FlowIntoBlock(Level world1, int i2, int i3, int i4, int i5)
        {
            if (this.CanSpreadTo(world1, i2, i3, i4))
            {
                int i6 = world1.GetTile(i2, i3, i4);
                if (i6 > 0)
                {
                    if (this.material == Material.lava)
                    {
                        this.Fizz(world1, i2, i3, i4);
                    }
                    else
                    {
                        Tile.tiles[i6].DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                    }
                }

                world1.SetTileAndData(i2, i3, i4, this.id, i5);
            }
        }

        private int CalculateFlowCost(Level world1, int i2, int i3, int i4, int i5, int i6)
        {
            int i7 = 1000;
            for (int i8 = 0; i8 < 4; ++i8)
            {
                if ((i8 != 0 || i6 != 1) && (i8 != 1 || i6 != 0) && (i8 != 2 || i6 != 3) && (i8 != 3 || i6 != 2))
                {
                    int i9 = i2;
                    int i11 = i4;
                    if (i8 == 0)
                    {
                        i9 = i2 - 1;
                    }

                    if (i8 == 1)
                    {
                        ++i9;
                    }

                    if (i8 == 2)
                    {
                        i11 = i4 - 1;
                    }

                    if (i8 == 3)
                    {
                        ++i11;
                    }

                    if (!this.IsWaterBlocking(world1, i9, i3, i11) && (world1.GetMaterial(i9, i3, i11) != this.material || world1.GetData(i9, i3, i11) != 0))
                    {
                        if (!this.IsWaterBlocking(world1, i9, i3 - 1, i11))
                        {
                            return i5;
                        }

                        if (i5 < 4)
                        {
                            int i12 = this.CalculateFlowCost(world1, i9, i3, i11, i5 + 1, i8);
                            if (i12 < i7)
                            {
                                i7 = i12;
                            }
                        }
                    }
                }
            }

            return i7;
        }

        private bool[] GetOptimalFlowDirections(Level world1, int i2, int i3, int i4)
        {
            int i5;
            int i6;
            for (i5 = 0; i5 < 4; ++i5)
            {
                this.flowCost[i5] = 1000;
                i6 = i2;
                int i8 = i4;
                if (i5 == 0)
                {
                    i6 = i2 - 1;
                }

                if (i5 == 1)
                {
                    ++i6;
                }

                if (i5 == 2)
                {
                    i8 = i4 - 1;
                }

                if (i5 == 3)
                {
                    ++i8;
                }

                if (!this.IsWaterBlocking(world1, i6, i3, i8) && (world1.GetMaterial(i6, i3, i8) != this.material || world1.GetData(i6, i3, i8) != 0))
                {
                    if (!this.IsWaterBlocking(world1, i6, i3 - 1, i8))
                    {
                        this.flowCost[i5] = 0;
                    }
                    else
                    {
                        this.flowCost[i5] = this.CalculateFlowCost(world1, i6, i3, i8, 1, i5);
                    }
                }
            }

            i5 = this.flowCost[0];
            for (i6 = 1; i6 < 4; ++i6)
            {
                if (this.flowCost[i6] < i5)
                {
                    i5 = this.flowCost[i6];
                }
            }

            for (i6 = 0; i6 < 4; ++i6)
            {
                this.optimalDir[i6] = this.flowCost[i6] == i5;
            }

            return this.optimalDir;
        }

        private bool IsWaterBlocking(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetTile(i2, i3, i4);
            if (i5 != Tile.doorWood.id && i5 != Tile.door_iron.id && i5 != Tile.signPost.id && i5 != Tile.ladder.id && i5 != Tile.reed.id)
            {
                if (i5 == 0)
                {
                    return false;
                }
                else
                {
                    Material material6 = Tile.tiles[i5].material;
                    return material6.BlocksMotion();
                }
            }
            else
            {
                return true;
            }
        }

        protected virtual int GetHighest(Level world1, int i2, int i3, int i4, int i5)
        {
            int i6 = this.GetDepth(world1, i2, i3, i4);
            if (i6 < 0)
            {
                return i5;
            }
            else
            {
                if (i6 == 0)
                {
                    ++this.nAdjSrcs;
                }

                if (i6 >= 8)
                {
                    i6 = 0;
                }

                return i5 >= 0 && i6 >= i5 ? i5 : i6;
            }
        }

        private bool CanSpreadTo(Level world1, int i2, int i3, int i4)
        {
            Material material5 = world1.GetMaterial(i2, i3, i4);
            return material5 == this.material ? false : (material5 == Material.lava ? false : !this.IsWaterBlocking(world1, i2, i3, i4));
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            base.OnPlace(world1, i2, i3, i4);
            if (world1.GetTile(i2, i3, i4) == this.id)
            {
                world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
            }
        }
    }
}