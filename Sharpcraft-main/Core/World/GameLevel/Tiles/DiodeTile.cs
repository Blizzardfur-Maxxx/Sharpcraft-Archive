using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class DiodeTile : Tile
    {
        public static readonly double[] CONVERSION = new[]
        {
            -0.0625,
            0.0625,
            0.1875,
            0.3125
        };
        private static readonly int[] DIRECTION = new[]
        {
            1,
            2,
            3,
            4
        };
        private readonly bool isRepeaterPowered;
        public DiodeTile(int i1, bool z2) : base(i1, 6, Material.decoration)
        {
            this.isRepeaterPowered = z2;
            this.SetShape(0F, 0F, 0F, 1F, 0.125F, 1F);
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return !world1.IsSolidBlockingTile(i2, i3 - 1, i4) ? false : base.CanPlaceBlockAt(world1, i2, i3, i4);
        }

        public override bool CanBlockStay(Level world1, int i2, int i3, int i4)
        {
            return !world1.IsSolidBlockingTile(i2, i3 - 1, i4) ? false : base.CanBlockStay(world1, i2, i3, i4);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            bool z7 = this.Func_2(world1, i2, i3, i4, i6);
            if (this.isRepeaterPowered && !z7)
            {
                world1.SetTileAndData(i2, i3, i4, Tile.redstoneRepeaterIdle.id, i6);
            }
            else if (!this.isRepeaterPowered)
            {
                world1.SetTileAndData(i2, i3, i4, Tile.redstoneRepeaterActive.id, i6);
                if (!z7)
                {
                    int i8 = (i6 & 12) >> 2;
                    world1.ScheduleBlockUpdate(i2, i3, i4, Tile.redstoneRepeaterActive.id, DIRECTION[i8] * 2);
                }
            }
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return faceIdx == 0 ? (this.isRepeaterPowered ? 99 : 115) : (faceIdx == TileFace.UP ? (this.isRepeaterPowered ? 147 : 131) : 5);
        }

        public override bool ShouldRenderFace(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return i5 != 0 && i5 != 1;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.REPEATER;
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return this.GetTexture(faceIdx, 0);
        }

        public override bool GetSignal(Level world1, int i2, int i3, int i4, int i5)
        {
            return this.GetDirectSignal(world1, i2, i3, i4, i5);
        }

        public override bool GetDirectSignal(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            if (!this.isRepeaterPowered)
            {
                return false;
            }
            else
            {
                int i6 = iBlockAccess1.GetData(i2, i3, i4) & 3;
                return i6 == 0 && i5 == 3 ? true : (i6 == 1 && i5 == 4 ? true : (i6 == 2 && i5 == 2 ? true : i6 == 3 && i5 == 5));
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!this.CanBlockStay(world1, i2, i3, i4))
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
            }
            else
            {
                int i6 = world1.GetData(i2, i3, i4);
                bool z7 = this.Func_2(world1, i2, i3, i4, i6);
                int i8 = (i6 & 12) >> 2;
                if (this.isRepeaterPowered && !z7)
                {
                    world1.ScheduleBlockUpdate(i2, i3, i4, this.id, DIRECTION[i8] * 2);
                }
                else if (!this.isRepeaterPowered && z7)
                {
                    world1.ScheduleBlockUpdate(i2, i3, i4, this.id, DIRECTION[i8] * 2);
                }
            }
        }

        private bool Func_2(Level world1, int i2, int i3, int i4, int i5)
        {
            int i6 = i5 & 3;
            switch (i6)
            {
                case 0:
                    return world1.IsBlockIndirectlyProvidingPowerTo(i2, i3, i4 + 1, 3) || world1.GetTile(i2, i3, i4 + 1) == Tile.redstoneWire.id && world1.GetData(i2, i3, i4 + 1) > 0;
                case 1:
                    return world1.IsBlockIndirectlyProvidingPowerTo(i2 - 1, i3, i4, 4) || world1.GetTile(i2 - 1, i3, i4) == Tile.redstoneWire.id && world1.GetData(i2 - 1, i3, i4) > 0;
                case 2:
                    return world1.IsBlockIndirectlyProvidingPowerTo(i2, i3, i4 - 1, 2) || world1.GetTile(i2, i3, i4 - 1) == Tile.redstoneWire.id && world1.GetData(i2, i3, i4 - 1) > 0;
                case 3:
                    return world1.IsBlockIndirectlyProvidingPowerTo(i2 + 1, i3, i4, 5) || world1.GetTile(i2 + 1, i3, i4) == Tile.redstoneWire.id && world1.GetData(i2 + 1, i3, i4) > 0;
                default:
                    return false;
            }
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            int i7 = (i6 & 12) >> 2;
            i7 = i7 + 1 << 2 & 12;
            world1.SetData(i2, i3, i4, i7 | i6 & 3);
            return true;
        }

        public override bool IsSignalSource()
        {
            return false;
        }

        public override void OnBlockPlacedBy(Level world1, int i2, int i3, int i4, Mob entityLiving5)
        {
            int i6 = ((Mth.Floor(entityLiving5.yaw * 4F / 360F + 0.5) & 3) + 2) % 4;
            world1.SetData(i2, i3, i4, i6);
            bool z7 = this.Func_2(world1, i2, i3, i4, i6);
            if (z7)
            {
                world1.ScheduleBlockUpdate(i2, i3, i4, this.id, 1);
            }
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            world1.UpdateNeighborsAt(i2 + 1, i3, i4, this.id);
            world1.UpdateNeighborsAt(i2 - 1, i3, i4, this.id);
            world1.UpdateNeighborsAt(i2, i3, i4 + 1, this.id);
            world1.UpdateNeighborsAt(i2, i3, i4 - 1, this.id);
            world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
            world1.UpdateNeighborsAt(i2, i3 + 1, i4, this.id);
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.redstoneRepeater.id;
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (this.isRepeaterPowered)
            {
                int i6 = world1.GetData(i2, i3, i4);
                double d7 = i2 + 0.5F + ((float)random5.NextFloat() - 0.5F) * 0.2;
                double d9 = i3 + 0.4F + ((float)random5.NextFloat() - 0.5F) * 0.2;
                double d11 = i4 + 0.5F + ((float)random5.NextFloat() - 0.5F) * 0.2;
                double d13 = 0;
                double d15 = 0;
                if (random5.NextInt(2) == 0)
                {
                    switch (i6 & 3)
                    {
                        case 0:
                            d15 = -0.3125;
                            break;
                        case 1:
                            d13 = 0.3125;
                            break;
                        case 2:
                            d15 = 0.3125;
                            break;
                        case 3:
                            d13 = -0.3125;
                            break;
                    }
                }
                else
                {
                    int i17 = (i6 & 12) >> 2;
                    switch (i6 & 3)
                    {
                        case 0:
                            d15 = CONVERSION[i17];
                            break;
                        case 1:
                            d13 = -CONVERSION[i17];
                            break;
                        case 2:
                            d15 = -CONVERSION[i17];
                            break;
                        case 3:
                            d13 = CONVERSION[i17];
                            break;
                    }
                }

                world1.AddParticle("reddust", d7 + d13, d9, d11 + d15, 0, 0, 0);
            }
        }
    }
}