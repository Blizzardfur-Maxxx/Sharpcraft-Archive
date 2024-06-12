using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class PistonBaseTile : Tile
    {
        private bool isSticky;
        private bool ignoreUpdates;
        public PistonBaseTile(int i1, int i2, bool z3) : base(i1, i2, Material.piston)
        {
            this.isSticky = z3;
            this.SetSoundType(SOUND_STONE);
            this.SetDestroyTime(0.5F);
        }

        public virtual int Func_31040_i()
        {
            return this.isSticky ? 106 : 107;
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            TileFace i3 = GetOrientation(i2);
            return i3 > TileFace.EAST ? this.texture : (faceIdx == i3 ? (!IsPowered(i2) && this.minX <= 0 && this.minY <= 0 && this.minZ <= 0 && this.maxX >= 1 && this.maxY >= 1 && this.maxZ >= 1 ? this.texture : 110) : (faceIdx == Facing.opposite(i3) ? 109 : 108));
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.PISTON_BASE;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            return false;
        }

        public override void OnBlockPlacedBy(Level world1, int i2, int i3, int i4, Mob entityLiving5)
        {
            int i6 = DetermineOrientation(world1, i2, i3, i4, (Player)entityLiving5);
            world1.SetData(i2, i3, i4, i6);
            if (!world1.isRemote)
            {
                this.UpdatePistonState(world1, i2, i3, i4);
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!world1.isRemote && !this.ignoreUpdates)
            {
                this.UpdatePistonState(world1, i2, i3, i4);
            }
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            if (!world1.isRemote && world1.GetTileEntity(i2, i3, i4) == null)
            {
                this.UpdatePistonState(world1, i2, i3, i4);
            }
        }

        private void UpdatePistonState(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            TileFace i6 = GetOrientation(i5);
            bool z7 = this.IsPowered(world1, i2, i3, i4, i6);
            if (i5 != 7)
            {
                if (z7 && !IsPowered(i5))
                {
                    if (CanExtend(world1, i2, i3, i4, i6))
                    {
                        world1.SetDataNoUpdate(i2, i3, i4, (int)i6 | 8);
                        world1.TileEvent(i2, i3, i4, 0, (int)i6);
                    }
                }
                else if (!z7 && IsPowered(i5))
                {
                    world1.SetDataNoUpdate(i2, i3, i4, (int)i6);
                    world1.TileEvent(i2, i3, i4, 1, (int)i6);
                }
            }
        }

        private bool IsPowered(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            return i5 != 0 && world1.IsBlockIndirectlyProvidingPowerTo(i2, i3 - 1, i4, 0) ? true : (i5 != TileFace.UP && world1.IsBlockIndirectlyProvidingPowerTo(i2, i3 + 1, i4, 1) ? true : (i5 != TileFace.NORTH && world1.IsBlockIndirectlyProvidingPowerTo(i2, i3, i4 - 1, 2) ? true : (i5 != TileFace.SOUTH && world1.IsBlockIndirectlyProvidingPowerTo(i2, i3, i4 + 1, 3) ? true : (i5 != TileFace.EAST && world1.IsBlockIndirectlyProvidingPowerTo(i2 + 1, i3, i4, 5) ? true : (i5 != TileFace.WEST && world1.IsBlockIndirectlyProvidingPowerTo(i2 - 1, i3, i4, 4) ? true : (world1.IsBlockIndirectlyProvidingPowerTo(i2, i3, i4, 0) ? true : (world1.IsBlockIndirectlyProvidingPowerTo(i2, i3 + 2, i4, 1) ? true : (world1.IsBlockIndirectlyProvidingPowerTo(i2, i3 + 1, i4 - 1, 2) ? true : (world1.IsBlockIndirectlyProvidingPowerTo(i2, i3 + 1, i4 + 1, 3) ? true : (world1.IsBlockIndirectlyProvidingPowerTo(i2 - 1, i3 + 1, i4, 4) ? true : world1.IsBlockIndirectlyProvidingPowerTo(i2 + 1, i3 + 1, i4, 5)))))))))));
        }

        public override void TileEvent(Level world1, int i2, int i3, int i4, int i5, int tf)
        {
            Facing.TileFace i6 = (Facing.TileFace)tf;
            this.ignoreUpdates = true;
            if (i5 == 0)
            {
                if (this.TryExtend(world1, i2, i3, i4, i6))
                {
                    world1.SetData(i2, i3, i4, ((int)i6) | 8);
                    world1.PlaySound(i2 + 0.5, i3 + 0.5, i4 + 0.5, "tile.piston.out", 0.5F, world1.rand.NextFloat() * 0.25F + 0.6F);
                }
            }
            else if (i5 == 1)
            {
                TileEntity tileEntity8 = world1.GetTileEntity(i2 + Facing.xOffset(i6), i3 + Facing.yOffset(i6), i4 + Facing.zOffset(i6));
                if (tileEntity8 != null && tileEntity8 is PistonPieceEntity)
                {
                    ((PistonPieceEntity)tileEntity8).ClearPistonTileEntity();
                }

                world1.SetTileAndDataNoUpdate(i2, i3, i4, Tile.pistonMoving.id, (int)i6);
                world1.SetTileEntity(i2, i3, i4, PistonMovingPiece.GetTileEntity(this.id, (int)i6, i6, false, true));
                if (this.isSticky)
                {
                    int i9 = i2 + Facing.xOffset(i6) * 2;
                    int i10 = i3 + Facing.yOffset(i6) * 2;
                    int i11 = i4 + Facing.zOffset(i6) * 2;
                    int i12 = world1.GetTile(i9, i10, i11);
                    int i13 = world1.GetData(i9, i10, i11);
                    bool z14 = false;
                    if (i12 == Tile.pistonMoving.id)
                    {
                        TileEntity tileEntity15 = world1.GetTileEntity(i9, i10, i11);
                        if (tileEntity15 != null && tileEntity15 is PistonPieceEntity)
                        {
                            PistonPieceEntity tileEntityPiston16 = (PistonPieceEntity)tileEntity15;
                            if (tileEntityPiston16.Func_31009() == i6 && tileEntityPiston16.Func_31015())
                            {
                                tileEntityPiston16.ClearPistonTileEntity();
                                i12 = tileEntityPiston16.GetStoredBlockID();
                                i13 = tileEntityPiston16.GetBlockMetadata();
                                z14 = true;
                            }
                        }
                    }

                    if (z14 || i12 <= 0 || !CanPushBlock(i12, world1, i9, i10, i11, false) || Tile.tiles[i12].GetPistonPushReaction() != 0 && i12 != Tile.pistonBase.id && i12 != Tile.pistonStickyBase.id)
                    {
                        if (!z14)
                        {
                            this.ignoreUpdates = false;
                            world1.SetTile(i2 + Facing.xOffset(i6), i3 + Facing.yOffset(i6), i4 + Facing.zOffset(i6), 0);
                            this.ignoreUpdates = true;
                        }
                    }
                    else
                    {
                        this.ignoreUpdates = false;
                        world1.SetTile(i9, i10, i11, 0);
                        this.ignoreUpdates = true;
                        i2 += Facing.xOffset(i6);
                        i3 += Facing.yOffset(i6);
                        i4 += Facing.zOffset(i6);
                        world1.SetTileAndDataNoUpdate(i2, i3, i4, Tile.pistonMoving.id, i13);
                        world1.SetTileEntity(i2, i3, i4, PistonMovingPiece.GetTileEntity(i12, i13, i6, false, false));
                    }
                }
                else
                {
                    this.ignoreUpdates = false;
                    world1.SetTile(i2 + Facing.xOffset(i6), i3 + Facing.yOffset(i6), i4 + Facing.zOffset(i6), 0);
                    this.ignoreUpdates = true;
                }

                world1.PlaySound(i2 + 0.5, i3 + 0.5, i4 + 0.5, "tile.piston.in", 0.5F, world1.rand.NextFloat() * 0.15F + 0.6F);
            }

            this.ignoreUpdates = false;
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            int i5 = iBlockAccess1.GetData(i2, i3, i4);
            if (IsPowered(i5))
            {
                switch (GetOrientation(i5))
                {
                    case TileFace.DOWN:
                        this.SetShape(0F, 0.25F, 0F, 1F, 1F, 1F);
                        break;
                    case TileFace.UP:
                        this.SetShape(0F, 0F, 0F, 1F, 0.75F, 1F);
                        break;
                    case TileFace.NORTH:
                        this.SetShape(0F, 0F, 0.25F, 1F, 1F, 1F);
                        break;
                    case TileFace.SOUTH:
                        this.SetShape(0F, 0F, 0F, 1F, 1F, 0.75F);
                        break;
                    case TileFace.WEST:
                        this.SetShape(0.25F, 0F, 0F, 1F, 1F, 1F);
                        break;
                    case TileFace.EAST:
                        this.SetShape(0F, 0F, 0F, 0.75F, 1F, 1F);
                        break;
                }
            }
            else
            {
                this.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
            }
        }

        public override void SetBlockBoundsForItemRender()
        {
            this.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
        }

        public override void AddAABBs(Level world1, int i2, int i3, int i4, AABB axisAlignedBB5, List<AABB> arrayList6)
        {
            this.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
            base.AddAABBs(world1, i2, i3, i4, axisAlignedBB5, arrayList6);
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public static TileFace GetOrientation(int i0)
        {
            return (TileFace)i0 & TileFace.MASK;
        }

        public static bool IsPowered(int i0)
        {
            return (i0 & 8) != 0;
        }

        private static int DetermineOrientation(Level world0, int i1, int i2, int i3, Player entityPlayer4)
        {
            if (Mth.Abs((float)entityPlayer4.x - i1) < 2F && Mth.Abs((float)entityPlayer4.z - i3) < 2F)
            {
                double d5 = entityPlayer4.y + 1.82 - entityPlayer4.yOffset;
                if (d5 - i2 > 2)
                {
                    return 1;
                }

                if (i2 - d5 > 0)
                {
                    return 0;
                }
            }

            int i7 = Mth.Floor(entityPlayer4.yaw * 4F / 360F + 0.5) & 3;
            return i7 == 0 ? 2 : (i7 == 1 ? 5 : (i7 == 2 ? 3 : (i7 == 3 ? 4 : 0)));
        }

        private static bool CanPushBlock(int i0, Level world1, int i2, int i3, int i4, bool z5)
        {
            if (i0 == Tile.obsidian.id)
            {
                return false;
            }
            else
            {
                if (i0 != Tile.pistonBase.id && i0 != Tile.pistonStickyBase.id)
                {
                    if (Tile.tiles[i0].GetDestroyTime() == -1F)
                    {
                        return false;
                    }

                    if (Tile.tiles[i0].GetPistonPushReaction() == 2)
                    {
                        return false;
                    }

                    if (!z5 && Tile.tiles[i0].GetPistonPushReaction() == 1)
                    {
                        return false;
                    }
                }
                else if (IsPowered(world1.GetData(i2, i3, i4)))
                {
                    return false;
                }

                TileEntity tileEntity6 = world1.GetTileEntity(i2, i3, i4);
                return tileEntity6 == null;
            }
        }

        private static bool CanExtend(Level world0, int i1, int i2, int i3, TileFace i4)
        {
            int i5 = i1 + Facing.xOffset(i4);
            int i6 = i2 + Facing.yOffset(i4);
            int i7 = i3 + Facing.zOffset(i4);
            int i8 = 0;
            while (true)
            {
                if (i8 < 13)
                {
                    if (i6 <= 0 || i6 >= 127)
                    {
                        return false;
                    }

                    int i9 = world0.GetTile(i5, i6, i7);
                    if (i9 != 0)
                    {
                        if (!CanPushBlock(i9, world0, i5, i6, i7, true))
                        {
                            return false;
                        }

                        if (Tile.tiles[i9].GetPistonPushReaction() != 1)
                        {
                            if (i8 == 12)
                            {
                                return false;
                            }

                            i5 += Facing.xOffset(i4);
                            i6 += Facing.yOffset(i4);
                            i7 += Facing.zOffset(i4);
                            ++i8;
                            continue;
                        }
                    }
                }

                return true;
            }
        }

        private bool TryExtend(Level world1, int i2, int i3, int i4, Facing.TileFace tf)
        {
            int i6 = i2 + Facing.xOffset(tf);
            int i7 = i3 + Facing.yOffset(tf);
            int i8 = i4 + Facing.zOffset(tf);
            int i9 = 0;
            int i10;
            while (i9 < 13)
            {
                if (i7 > 0 && i7 < 127)
                {
                    i10 = world1.GetTile(i6, i7, i8);
                    if (i10 != 0)
                    {
                        if (!CanPushBlock(i10, world1, i6, i7, i8, true))
                        {
                            return false;
                        }

                        if (Tile.tiles[i10].GetPistonPushReaction() != 1)
                        {
                            if (i9 == 12)
                            {
                                return false;
                            }

                            i6 += Facing.xOffset(tf);
                            i7 += Facing.yOffset(tf);
                            i8 += Facing.zOffset(tf);
                            ++i9;
                            continue;
                        }

                        Tile.tiles[i10].DropBlockAsItem(world1, i6, i7, i8, world1.GetData(i6, i7, i8));
                        world1.SetTile(i6, i7, i8, 0);
                    }

                    break;
                }

                return false;
            }

            while (i6 != i2 || i7 != i3 || i8 != i4)
            {
                i9 = i6 - Facing.xOffset(tf);
                i10 = i7 - Facing.yOffset(tf);
                int i11 = i8 - Facing.zOffset(tf);
                int i12 = world1.GetTile(i9, i10, i11);
                int i13 = world1.GetData(i9, i10, i11);
                if (i12 == this.id && i9 == i2 && i10 == i3 && i11 == i4)
                {
                    world1.SetTileAndDataNoUpdate(i6, i7, i8, Tile.pistonMoving.id, (int)tf | (this.isSticky ? 8 : 0));
                    world1.SetTileEntity(i6, i7, i8, PistonMovingPiece.GetTileEntity(Tile.pistonExtension.id, (int)tf | (this.isSticky ? 8 : 0), tf, true, false));
                }
                else
                {
                    world1.SetTileAndDataNoUpdate(i6, i7, i8, Tile.pistonMoving.id, i13);
                    world1.SetTileEntity(i6, i7, i8, PistonMovingPiece.GetTileEntity(i12, i13, tf, true, false));
                }

                i6 = i9;
                i7 = i10;
                i8 = i11;
            }

            return true;
        }
    }
}