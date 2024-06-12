using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using System.Collections.Generic;
using static SharpCraft.Core.Util.Facing;
using static SharpCraft.Core.World.Entities.Players.Player;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class BedTile : Tile
    {
        public static readonly int[][] headBlockToFootBlockMap = new[]
        {
            new[]
            {
                0,
                1
            },
            new[]
            {
                -1,
                0
            },
            new[]
            {
                0,
                -1
            },
            new[]
            {
                1,
                0
            }
        };
        public BedTile(int i1) : base(i1, 134, Material.cloth)
        {
            this.SetBounds();
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (world1.isRemote)
            {
                return true;
            }
            else
            {
                int i6 = world1.GetData(i2, i3, i4);
                if (!IsBlockFootOfBed(i6))
                {
                    int i7 = GetDirectionFromMetadata(i6);
                    i2 += headBlockToFootBlockMap[i7][0];
                    i4 += headBlockToFootBlockMap[i7][1];
                    if (world1.GetTile(i2, i3, i4) != this.id)
                    {
                        return true;
                    }

                    i6 = world1.GetData(i2, i3, i4);
                }

                if (!world1.dimension.CanRespawnHere())
                {
                    double d16 = i2 + 0.5;
                    double d17 = i3 + 0.5;
                    double d11 = i4 + 0.5;
                    world1.SetTile(i2, i3, i4, 0);
                    int i13 = GetDirectionFromMetadata(i6);
                    i2 += headBlockToFootBlockMap[i13][0];
                    i4 += headBlockToFootBlockMap[i13][1];
                    if (world1.GetTile(i2, i3, i4) == this.id)
                    {
                        world1.SetTile(i2, i3, i4, 0);
                        d16 = (d16 + i2 + 0.5) / 2;
                        d17 = (d17 + i3 + 0.5) / 2;
                        d11 = (d11 + i4 + 0.5) / 2;
                    }

                    world1.Explode(null, i2 + 0.5F, i3 + 0.5F, i4 + 0.5F, 5F, true);
                    return true;
                }
                else
                {
                    if (IsBedOccupied(i6))
                    {
                        Player entityPlayer14 = null;
                        IEnumerator<Player> iterator8 = world1.playerEntities.GetEnumerator();
                        while (iterator8.MoveNext())
                        {
                            Player entityPlayer9 = iterator8.Current;
                            if (entityPlayer9.IsSleeping())
                            {
                                Pos chunkCoordinates10 = entityPlayer9.playerLocation;
                                if (chunkCoordinates10.x == i2 && chunkCoordinates10.y == i3 && chunkCoordinates10.z == i4)
                                {
                                    entityPlayer14 = entityPlayer9;
                                }
                            }
                        }

                        if (entityPlayer14 != null)
                        {
                            entityPlayer5.AddChatMessage("tile.bed.occupied");
                            return true;
                        }

                        SetBedOccupied(world1, i2, i3, i4, false);
                    }

                    BedSleepingProblem enumStatus15 = entityPlayer5.SleepInBedAt(i2, i3, i4);
                    if (enumStatus15 == BedSleepingProblem.OK)
                    {
                        SetBedOccupied(world1, i2, i3, i4, true);
                        return true;
                    }
                    else
                    {
                        if (enumStatus15 == BedSleepingProblem.NOT_POSSIBLE_NOW)
                        {
                            entityPlayer5.AddChatMessage("tile.bed.noSleep");
                        }

                        return true;
                    }
                }
            }
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            if (faceIdx == TileFace.DOWN)
            {
                return Tile.wood.texture;
            }
            else
            {
                int i3 = GetDirectionFromMetadata(i2);
                int i4 = Direction.direction[i3, (int)faceIdx];
                return IsBlockFootOfBed(i2) ? (i4 == 2 ? this.texture + 2 + 16 : (i4 != 5 && i4 != 4 ? this.texture + 1 : this.texture + 1 + 16)) : (i4 == 3 ? this.texture - 1 + 16 : (i4 != 5 && i4 != 4 ? this.texture : this.texture + 16));
            }
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.BED;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            this.SetBounds();
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            int i7 = GetDirectionFromMetadata(i6);
            if (IsBlockFootOfBed(i6))
            {
                if (world1.GetTile(i2 - headBlockToFootBlockMap[i7][0], i3, i4 - headBlockToFootBlockMap[i7][1]) != this.id)
                {
                    world1.SetTile(i2, i3, i4, 0);
                }
            }
            else if (world1.GetTile(i2 + headBlockToFootBlockMap[i7][0], i3, i4 + headBlockToFootBlockMap[i7][1]) != this.id)
            {
                world1.SetTile(i2, i3, i4, 0);
                if (!world1.isRemote)
                {
                    this.DropBlockAsItem(world1, i2, i3, i4, i6);
                }
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return IsBlockFootOfBed(i1) ? 0 : Item.bed.id;
        }

        private void SetBounds()
        {
            this.SetShape(0F, 0F, 0F, 1F, 0.5625F, 1F);
        }

        public static int GetDirectionFromMetadata(int i0)
        {
            return i0 & 3;
        }

        public static bool IsBlockFootOfBed(int i0)
        {
            return (i0 & 8) != 0;
        }

        public static bool IsBedOccupied(int i0)
        {
            return (i0 & 4) != 0;
        }

        public static void SetBedOccupied(Level world0, int i1, int i2, int i3, bool z4)
        {
            int i5 = world0.GetData(i1, i2, i3);
            if (z4)
            {
                i5 |= 4;
            }
            else
            {
                i5 &= -5;
            }

            world0.SetData(i1, i2, i3, i5);
        }

        public static Pos GetNearestEmptyPos(Level world0, int i1, int i2, int i3, int i4)
        {
            int i5 = world0.GetData(i1, i2, i3);
            int i6 = GetDirectionFromMetadata(i5);
            for (int i7 = 0; i7 <= 1; ++i7)
            {
                int i8 = i1 - headBlockToFootBlockMap[i6][0] * i7 - 1;
                int i9 = i3 - headBlockToFootBlockMap[i6][1] * i7 - 1;
                int i10 = i8 + 2;
                int i11 = i9 + 2;
                for (int i12 = i8; i12 <= i10; ++i12)
                {
                    for (int i13 = i9; i13 <= i11; ++i13)
                    {
                        if (world0.IsSolidBlockingTile(i12, i2 - 1, i13) && world0.IsAirBlock(i12, i2, i13) && world0.IsAirBlock(i12, i2 + 1, i13))
                        {
                            if (i4 <= 0)
                            {
                                return new Pos(i12, i2, i13);
                            }

                            --i4;
                        }
                    }
                }
            }

            return default;
        }

        public override void SpawnResources(Level world1, int i2, int i3, int i4, int i5, float f6)
        {
            if (!IsBlockFootOfBed(i5))
            {
                base.SpawnResources(world1, i2, i3, i4, i5, f6);
            }
        }

        public override int GetPistonPushReaction()
        {
            return 1;
        }
    }
}