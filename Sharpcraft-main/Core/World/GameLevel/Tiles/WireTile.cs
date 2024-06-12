using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class WireTile : Tile
    {
        private bool wiresProvidePower = true;
        private HashSet<TilePos> positions = new HashSet<TilePos>();
        public WireTile(int i1, int i2) : base(i1, i2, Material.decoration)
        {
            this.SetShape(0F, 0F, 0F, 1F, 0.0625F, 1F);
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return this.texture;
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.WIRE;
        }

        public override int GetColor(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            return 8388608;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2, i3 - 1, i4);
        }

        private void UpdateAndPropagateCurrentStrength(Level world1, int i2, int i3, int i4)
        {
            this.Func(world1, i2, i3, i4, i2, i3, i4);
            List<TilePos> arrayList5 = new List<TilePos>(this.positions);
            this.positions.Clear();
            for (int i6 = 0; i6 < arrayList5.Count; ++i6)
            {
                TilePos chunkPosition7 = arrayList5[i6];
                world1.UpdateNeighborsAt(chunkPosition7.x, chunkPosition7.y, chunkPosition7.z, this.id);
            }
        }

        private void Func(Level world1, int i2, int i3, int i4, int i5, int i6, int i7)
        {
            int i8 = world1.GetData(i2, i3, i4);
            int i9 = 0;
            this.wiresProvidePower = false;
            bool z10 = world1.IsBlockIndirectlyGettingPowered(i2, i3, i4);
            this.wiresProvidePower = true;
            int i11;
            int i12;
            int i13;
            if (z10)
            {
                i9 = 15;
            }
            else
            {
                for (i11 = 0; i11 < 4; ++i11)
                {
                    i12 = i2;
                    i13 = i4;
                    if (i11 == 0)
                    {
                        i12 = i2 - 1;
                    }

                    if (i11 == 1)
                    {
                        ++i12;
                    }

                    if (i11 == 2)
                    {
                        i13 = i4 - 1;
                    }

                    if (i11 == 3)
                    {
                        ++i13;
                    }

                    if (i12 != i5 || i3 != i6 || i13 != i7)
                    {
                        i9 = this.GetMaxCurrentStrength(world1, i12, i3, i13, i9);
                    }

                    if (world1.IsSolidBlockingTile(i12, i3, i13) && !world1.IsSolidBlockingTile(i2, i3 + 1, i4))
                    {
                        if (i12 != i5 || i3 + 1 != i6 || i13 != i7)
                        {
                            i9 = this.GetMaxCurrentStrength(world1, i12, i3 + 1, i13, i9);
                        }
                    }
                    else if (!world1.IsSolidBlockingTile(i12, i3, i13) && (i12 != i5 || i3 - 1 != i6 || i13 != i7))
                    {
                        i9 = this.GetMaxCurrentStrength(world1, i12, i3 - 1, i13, i9);
                    }
                }

                if (i9 > 0)
                {
                    --i9;
                }
                else
                {
                    i9 = 0;
                }
            }

            if (i8 != i9)
            {
                world1.editingBlocks = true;
                world1.SetData(i2, i3, i4, i9);
                world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
                world1.editingBlocks = false;
                for (i11 = 0; i11 < 4; ++i11)
                {
                    i12 = i2;
                    i13 = i4;
                    int i14 = i3 - 1;
                    if (i11 == 0)
                    {
                        i12 = i2 - 1;
                    }

                    if (i11 == 1)
                    {
                        ++i12;
                    }

                    if (i11 == 2)
                    {
                        i13 = i4 - 1;
                    }

                    if (i11 == 3)
                    {
                        ++i13;
                    }

                    if (world1.IsSolidBlockingTile(i12, i3, i13))
                    {
                        i14 += 2;
                    }

                    int i16 = this.GetMaxCurrentStrength(world1, i12, i3, i13, -1);
                    i9 = world1.GetData(i2, i3, i4);
                    if (i9 > 0)
                    {
                        --i9;
                    }

                    if (i16 >= 0 && i16 != i9)
                    {
                        this.Func(world1, i12, i3, i13, i2, i3, i4);
                    }

                    i16 = this.GetMaxCurrentStrength(world1, i12, i14, i13, -1);
                    i9 = world1.GetData(i2, i3, i4);
                    if (i9 > 0)
                    {
                        --i9;
                    }

                    if (i16 >= 0 && i16 != i9)
                    {
                        this.Func(world1, i12, i14, i13, i2, i3, i4);
                    }
                }

                if (i8 == 0 || i9 == 0)
                {
                    this.positions.Add(new TilePos(i2, i3, i4));
                    this.positions.Add(new TilePos(i2 - 1, i3, i4));
                    this.positions.Add(new TilePos(i2 + 1, i3, i4));
                    this.positions.Add(new TilePos(i2, i3 - 1, i4));
                    this.positions.Add(new TilePos(i2, i3 + 1, i4));
                    this.positions.Add(new TilePos(i2, i3, i4 - 1));
                    this.positions.Add(new TilePos(i2, i3, i4 + 1));
                }
            }
        }

        private void NotifyWireNeighborsOfNeighborChange(Level world1, int i2, int i3, int i4)
        {
            if (world1.GetTile(i2, i3, i4) == this.id)
            {
                world1.UpdateNeighborsAt(i2, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2 - 1, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2 + 1, i3, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3, i4 - 1, this.id);
                world1.UpdateNeighborsAt(i2, i3, i4 + 1, this.id);
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 + 1, i4, this.id);
            }
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            base.OnPlace(world1, i2, i3, i4);
            if (!world1.isRemote)
            {
                this.UpdateAndPropagateCurrentStrength(world1, i2, i3, i4);
                world1.UpdateNeighborsAt(i2, i3 + 1, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                this.NotifyWireNeighborsOfNeighborChange(world1, i2 - 1, i3, i4);
                this.NotifyWireNeighborsOfNeighborChange(world1, i2 + 1, i3, i4);
                this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3, i4 - 1);
                this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3, i4 + 1);
                if (world1.IsSolidBlockingTile(i2 - 1, i3, i4))
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2 - 1, i3 + 1, i4);
                }
                else
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2 - 1, i3 - 1, i4);
                }

                if (world1.IsSolidBlockingTile(i2 + 1, i3, i4))
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2 + 1, i3 + 1, i4);
                }
                else
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2 + 1, i3 - 1, i4);
                }

                if (world1.IsSolidBlockingTile(i2, i3, i4 - 1))
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3 + 1, i4 - 1);
                }
                else
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3 - 1, i4 - 1);
                }

                if (world1.IsSolidBlockingTile(i2, i3, i4 + 1))
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3 + 1, i4 + 1);
                }
                else
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3 - 1, i4 + 1);
                }
            }
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            base.OnBlockRemoval(world1, i2, i3, i4);
            if (!world1.isRemote)
            {
                world1.UpdateNeighborsAt(i2, i3 + 1, i4, this.id);
                world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                this.UpdateAndPropagateCurrentStrength(world1, i2, i3, i4);
                this.NotifyWireNeighborsOfNeighborChange(world1, i2 - 1, i3, i4);
                this.NotifyWireNeighborsOfNeighborChange(world1, i2 + 1, i3, i4);
                this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3, i4 - 1);
                this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3, i4 + 1);
                if (world1.IsSolidBlockingTile(i2 - 1, i3, i4))
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2 - 1, i3 + 1, i4);
                }
                else
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2 - 1, i3 - 1, i4);
                }

                if (world1.IsSolidBlockingTile(i2 + 1, i3, i4))
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2 + 1, i3 + 1, i4);
                }
                else
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2 + 1, i3 - 1, i4);
                }

                if (world1.IsSolidBlockingTile(i2, i3, i4 - 1))
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3 + 1, i4 - 1);
                }
                else
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3 - 1, i4 - 1);
                }

                if (world1.IsSolidBlockingTile(i2, i3, i4 + 1))
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3 + 1, i4 + 1);
                }
                else
                {
                    this.NotifyWireNeighborsOfNeighborChange(world1, i2, i3 - 1, i4 + 1);
                }
            }
        }

        private int GetMaxCurrentStrength(Level world1, int i2, int i3, int i4, int i5)
        {
            if (world1.GetTile(i2, i3, i4) != this.id)
            {
                return i5;
            }
            else
            {
                int i6 = world1.GetData(i2, i3, i4);
                return i6 > i5 ? i6 : i5;
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!world1.isRemote)
            {
                int i6 = world1.GetData(i2, i3, i4);
                bool z7 = this.CanPlaceBlockAt(world1, i2, i3, i4);
                if (!z7)
                {
                    this.DropBlockAsItem(world1, i2, i3, i4, i6);
                    world1.SetTile(i2, i3, i4, 0);
                }
                else
                {
                    this.UpdateAndPropagateCurrentStrength(world1, i2, i3, i4);
                }

                base.NeighborChanged(world1, i2, i3, i4, i5);
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.redstone.id;
        }

        public override bool GetSignal(Level world1, int i2, int i3, int i4, int i5)
        {
            return !this.wiresProvidePower ? false : this.GetDirectSignal(world1, i2, i3, i4, i5);
        }

        public override bool GetDirectSignal(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            if (!this.wiresProvidePower)
            {
                return false;
            }
            else if (iBlockAccess1.GetData(i2, i3, i4) == 0)
            {
                return false;
            }
            else if (i5 == 1)
            {
                return true;
            }
            else
            {
                bool z6 = IsPowerProviderOrWire(iBlockAccess1, i2 - 1, i3, i4, 1) || !iBlockAccess1.IsSolidBlockingTile(i2 - 1, i3, i4) && IsPowerProviderOrWire(iBlockAccess1, i2 - 1, i3 - 1, i4, -1);
                bool z7 = IsPowerProviderOrWire(iBlockAccess1, i2 + 1, i3, i4, 3) || !iBlockAccess1.IsSolidBlockingTile(i2 + 1, i3, i4) && IsPowerProviderOrWire(iBlockAccess1, i2 + 1, i3 - 1, i4, -1);
                bool z8 = IsPowerProviderOrWire(iBlockAccess1, i2, i3, i4 - 1, 2) || !iBlockAccess1.IsSolidBlockingTile(i2, i3, i4 - 1) && IsPowerProviderOrWire(iBlockAccess1, i2, i3 - 1, i4 - 1, -1);
                bool z9 = IsPowerProviderOrWire(iBlockAccess1, i2, i3, i4 + 1, 0) || !iBlockAccess1.IsSolidBlockingTile(i2, i3, i4 + 1) && IsPowerProviderOrWire(iBlockAccess1, i2, i3 - 1, i4 + 1, -1);
                if (!iBlockAccess1.IsSolidBlockingTile(i2, i3 + 1, i4))
                {
                    if (iBlockAccess1.IsSolidBlockingTile(i2 - 1, i3, i4) && IsPowerProviderOrWire(iBlockAccess1, i2 - 1, i3 + 1, i4, -1))
                    {
                        z6 = true;
                    }

                    if (iBlockAccess1.IsSolidBlockingTile(i2 + 1, i3, i4) && IsPowerProviderOrWire(iBlockAccess1, i2 + 1, i3 + 1, i4, -1))
                    {
                        z7 = true;
                    }

                    if (iBlockAccess1.IsSolidBlockingTile(i2, i3, i4 - 1) && IsPowerProviderOrWire(iBlockAccess1, i2, i3 + 1, i4 - 1, -1))
                    {
                        z8 = true;
                    }

                    if (iBlockAccess1.IsSolidBlockingTile(i2, i3, i4 + 1) && IsPowerProviderOrWire(iBlockAccess1, i2, i3 + 1, i4 + 1, -1))
                    {
                        z9 = true;
                    }
                }

                return !z8 && !z7 && !z6 && !z9 && i5 >= 2 && i5 <= 5 ? true : (i5 == 2 && z8 && !z6 && !z7 ? true : (i5 == 3 && z9 && !z6 && !z7 ? true : (i5 == 4 && z6 && !z8 && !z9 ? true : i5 == 5 && z7 && !z8 && !z9)));
            }
        }

        public override bool IsSignalSource()
        {
            return this.wiresProvidePower;
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            if (i6 > 0)
            {
                double d7 = i2 + 0.5 + (random5.NextFloat() - 0.5) * 0.2;
                double d9 = i3 + 0.0625F;
                double d11 = i4 + 0.5 + (random5.NextFloat() - 0.5) * 0.2;
                float f13 = i6 / 15F;
                float f14 = f13 * 0.6F + 0.4F;
                if (i6 == 0)
                {
                    f14 = 0F;
                }

                float f15 = f13 * f13 * 0.7F - 0.5F;
                float f16 = f13 * f13 * 0.6F - 0.7F;
                if (f15 < 0F)
                {
                    f15 = 0F;
                }

                if (f16 < 0F)
                {
                    f16 = 0F;
                }

                world1.AddParticle("reddust", d7, d9, d11, f14, f15, f16);
            }
        }

        public static bool IsPowerProviderOrWire(ILevelSource iBlockAccess0, int i1, int i2, int i3, int i4)
        {
            int i5 = iBlockAccess0.GetTile(i1, i2, i3);
            if (i5 == Tile.redstoneWire.id)
            {
                return true;
            }
            else if (i5 == 0)
            {
                return false;
            }
            else if (Tile.tiles[i5].IsSignalSource())
            {
                return true;
            }
            else if (i5 != Tile.redstoneRepeaterIdle.id && i5 != Tile.redstoneRepeaterActive.id)
            {
                return false;
            }
            else
            {
                int i6 = iBlockAccess0.GetData(i1, i2, i3);
                return i4 == Direction.opposite[i6 & 3];
            }
        }
    }
}