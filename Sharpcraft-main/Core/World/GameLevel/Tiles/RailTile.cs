using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using System.Collections.Generic;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class RailTile : Tile
    {
        private readonly bool isPowered;
        public static bool IsRailBlockAt(Level world0, int i1, int i2, int i3)
        {
            int i4 = world0.GetTile(i1, i2, i3);
            return i4 == Tile.rail.id || i4 == Tile.railPowered.id || i4 == Tile.railDetector.id;
        }

        public static bool IsRailBlock(int i0)
        {
            return i0 == Tile.rail.id || i0 == Tile.railPowered.id || i0 == Tile.railDetector.id;
        }

        public RailTile(int i1, int i2, bool z3) : base(i1, i2, Material.decoration)
        {
            this.isPowered = z3;
            this.SetShape(0F, 0F, 0F, 1F, 0.125F, 1F);
        }

        public virtual bool IsPowered()
        {
            return this.isPowered;
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override HitResult Clip(Level world1, int i2, int i3, int i4, Vec3 vec3D5, Vec3 vec3D6)
        {
            this.SetBlockBoundsBasedOnState(world1, i2, i3, i4);
            return base.Clip(world1, i2, i3, i4, vec3D5, vec3D6);
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            int i5 = iBlockAccess1.GetData(i2, i3, i4);
            if (i5 >= 2 && i5 <= 5)
            {
                this.SetShape(0F, 0F, 0F, 1F, 0.625F, 1F);
            }
            else
            {
                this.SetShape(0F, 0F, 0F, 1F, 0.125F, 1F);
            }
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            if (this.isPowered)
            {
                if (this.id == Tile.railPowered.id && (i2 & 8) == 0)
                {
                    return this.texture - 16;
                }
            }
            else if (i2 >= 6)
            {
                return this.texture - 16;
            }

            return this.texture;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.RAIL;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 1;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2, i3 - 1, i4);
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            if (!world1.isRemote)
            {
                this.F4031h(world1, i2, i3, i4, true);
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!world1.isRemote)
            {
                int i6 = world1.GetData(i2, i3, i4);
                int i7 = i6;
                if (this.isPowered)
                {
                    i7 = i6 & 7;
                }

                bool z8 = false;
                if (!world1.IsSolidBlockingTile(i2, i3 - 1, i4))
                {
                    z8 = true;
                }

                if (i7 == 2 && !world1.IsSolidBlockingTile(i2 + 1, i3, i4))
                {
                    z8 = true;
                }

                if (i7 == 3 && !world1.IsSolidBlockingTile(i2 - 1, i3, i4))
                {
                    z8 = true;
                }

                if (i7 == 4 && !world1.IsSolidBlockingTile(i2, i3, i4 - 1))
                {
                    z8 = true;
                }

                if (i7 == 5 && !world1.IsSolidBlockingTile(i2, i3, i4 + 1))
                {
                    z8 = true;
                }

                if (z8)
                {
                    this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                    world1.SetTile(i2, i3, i4, 0);
                }
                else if (this.id == Tile.railPowered.id)
                {
                    bool z9 = world1.IsBlockIndirectlyGettingPowered(i2, i3, i4) || world1.IsBlockIndirectlyGettingPowered(i2, i3 + 1, i4);
                    z9 = z9 || this.F27044a(world1, i2, i3, i4, i6, true, 0) || this.F27044a(world1, i2, i3, i4, i6, false, 0);
                    bool z10 = false;
                    if (z9 && (i6 & 8) == 0)
                    {
                        world1.SetData(i2, i3, i4, i7 | 8);
                        z10 = true;
                    }
                    else if (!z9 && (i6 & 8) != 0)
                    {
                        world1.SetData(i2, i3, i4, i7);
                        z10 = true;
                    }

                    if (z10)
                    {
                        world1.UpdateNeighborsAt(i2, i3 - 1, i4, this.id);
                        if (i7 == 2 || i7 == 3 || i7 == 4 || i7 == 5)
                        {
                            world1.UpdateNeighborsAt(i2, i3 + 1, i4, this.id);
                        }
                    }
                }
                else if (i5 > 0 && Tile.tiles[i5].IsSignalSource() && !this.isPowered && new Rail(world1, i2, i3, i4).GetAdjacentTracks() == 3)
                {
                    this.F4031h(world1, i2, i3, i4, false);
                }
            }
        }

        private void F4031h(Level world1, int i2, int i3, int i4, bool z5)
        {
            if (!world1.isRemote)
            {
                (new Rail(world1, i2, i3, i4)).F596a(world1.IsBlockIndirectlyGettingPowered(i2, i3, i4), z5);
            }
        }

        private bool F27044a(Level world1, int i2, int i3, int i4, int i5, bool z6, int i7)
        {
            if (i7 >= 8)
            {
                return false;
            }
            else
            {
                int i8 = i5 & 7;
                bool z9 = true;
                switch (i8)
                {
                    case 0:
                        if (z6)
                        {
                            ++i4;
                        }
                        else
                        {
                            --i4;
                        }

                        break;
                    case 1:
                        if (z6)
                        {
                            --i2;
                        }
                        else
                        {
                            ++i2;
                        }

                        break;
                    case 2:
                        if (z6)
                        {
                            --i2;
                        }
                        else
                        {
                            ++i2;
                            ++i3;
                            z9 = false;
                        }

                        i8 = 1;
                        break;
                    case 3:
                        if (z6)
                        {
                            --i2;
                            ++i3;
                            z9 = false;
                        }
                        else
                        {
                            ++i2;
                        }

                        i8 = 1;
                        break;
                    case 4:
                        if (z6)
                        {
                            ++i4;
                        }
                        else
                        {
                            --i4;
                            ++i3;
                            z9 = false;
                        }

                        i8 = 0;
                        break;
                    case 5:
                        if (z6)
                        {
                            ++i4;
                            ++i3;
                            z9 = false;
                        }
                        else
                        {
                            --i4;
                        }

                        i8 = 0;
                        break;
                }

                return this.Ffff0(world1, i2, i3, i4, z6, i7, i8) ? true : z9 && this.Ffff0(world1, i2, i3 - 1, i4, z6, i7, i8);
            }
        }

        private bool Ffff0(Level world1, int i2, int i3, int i4, bool z5, int i6, int i7)
        {
            int i8 = world1.GetTile(i2, i3, i4);
            if (i8 == Tile.railPowered.id)
            {
                int i9 = world1.GetData(i2, i3, i4);
                int i10 = i9 & 7;
                if (i7 == 1 && (i10 == 0 || i10 == 4 || i10 == 5))
                {
                    return false;
                }

                if (i7 == 0 && (i10 == 1 || i10 == 2 || i10 == 3))
                {
                    return false;
                }

                if ((i9 & 8) != 0)
                {
                    if (!world1.IsBlockIndirectlyGettingPowered(i2, i3, i4) && !world1.IsBlockIndirectlyGettingPowered(i2, i3 + 1, i4))
                    {
                        return this.F27044a(world1, i2, i3, i4, i9, z5, i6 + 1);
                    }

                    return true;
                }
            }

            return false;
        }

        public override int GetPistonPushReaction()
        {
            return 0;
        }

        private class Rail
        {
            private Level worldObj;
            private int trackX;
            private int trackY;
            private int trackZ;
            private readonly bool isPoweredRail;
            private IList<TilePos> connectedTracks;
            public Rail(Level world2, int i3, int i4, int i5)
            {
                this.connectedTracks = new List<TilePos>();
                this.worldObj = world2;
                this.trackX = i3;
                this.trackY = i4;
                this.trackZ = i5;
                int i6 = world2.GetTile(i3, i4, i5);
                int i7 = world2.GetData(i3, i4, i5);
                if (((RailTile)Tile.tiles[i6]).isPowered)
                {
                    this.isPoweredRail = true;
                    i7 &= -9;
                }
                else
                {
                    this.isPoweredRail = false;
                }

                this.SetConnections(i7);
            }

            private void SetConnections(int i1)
            {
                this.connectedTracks.Clear();
                if (i1 == 0)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY, this.trackZ - 1));
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY, this.trackZ + 1));
                }
                else if (i1 == 1)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX - 1, this.trackY, this.trackZ));
                    this.connectedTracks.Add(new TilePos(this.trackX + 1, this.trackY, this.trackZ));
                }
                else if (i1 == 2)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX - 1, this.trackY, this.trackZ));
                    this.connectedTracks.Add(new TilePos(this.trackX + 1, this.trackY + 1, this.trackZ));
                }
                else if (i1 == 3)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX - 1, this.trackY + 1, this.trackZ));
                    this.connectedTracks.Add(new TilePos(this.trackX + 1, this.trackY, this.trackZ));
                }
                else if (i1 == 4)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY + 1, this.trackZ - 1));
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY, this.trackZ + 1));
                }
                else if (i1 == 5)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY, this.trackZ - 1));
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY + 1, this.trackZ + 1));
                }
                else if (i1 == 6)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX + 1, this.trackY, this.trackZ));
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY, this.trackZ + 1));
                }
                else if (i1 == 7)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX - 1, this.trackY, this.trackZ));
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY, this.trackZ + 1));
                }
                else if (i1 == 8)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX - 1, this.trackY, this.trackZ));
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY, this.trackZ - 1));
                }
                else if (i1 == 9)
                {
                    this.connectedTracks.Add(new TilePos(this.trackX + 1, this.trackY, this.trackZ));
                    this.connectedTracks.Add(new TilePos(this.trackX, this.trackY, this.trackZ - 1));
                }
            }

            private void Func_785()
            {
                for (int i1 = 0; i1 < this.connectedTracks.Count; ++i1)
                {
                    Rail railLogic2 = this.GetMinecartTrackLogic(this.connectedTracks[i1]);
                    if (railLogic2 != null && railLogic2.IsConnectedTo(this))
                    {
                        this.connectedTracks[i1] = new TilePos(railLogic2.trackX, railLogic2.trackY, railLogic2.trackZ);
                    }
                    else
                    {
                        this.connectedTracks.RemoveAt(i1--);
                    }
                }
            }

            private bool IsMinecartTrack(int i1, int i2, int i3)
            {
                return RailTile.IsRailBlockAt(this.worldObj, i1, i2, i3) ? true : (RailTile.IsRailBlockAt(this.worldObj, i1, i2 + 1, i3) ? true : RailTile.IsRailBlockAt(this.worldObj, i1, i2 - 1, i3));
            }

            private Rail GetMinecartTrackLogic(TilePos chunkPosition1)
            {
                return RailTile.IsRailBlockAt(this.worldObj, chunkPosition1.x, chunkPosition1.y, chunkPosition1.z) ? new Rail(this.worldObj, chunkPosition1.x, chunkPosition1.y, chunkPosition1.z) : (RailTile.IsRailBlockAt(this.worldObj, chunkPosition1.x, chunkPosition1.y + 1, chunkPosition1.z) ? new Rail(this.worldObj, chunkPosition1.x, chunkPosition1.y + 1, chunkPosition1.z) : (RailTile.IsRailBlockAt(this.worldObj, chunkPosition1.x, chunkPosition1.y - 1, chunkPosition1.z) ? new Rail(this.worldObj, chunkPosition1.x, chunkPosition1.y - 1, chunkPosition1.z) : null));
            }

            private bool IsConnectedTo(Rail railLogic1)
            {
                for (int i2 = 0; i2 < this.connectedTracks.Count; ++i2)
                {
                    TilePos chunkPosition3 = this.connectedTracks[i2];
                    if (chunkPosition3.x == railLogic1.trackX && chunkPosition3.z == railLogic1.trackZ)
                    {
                        return true;
                    }
                }

                return false;
            }

            private bool IsInTrack(int i1, int i2, int i3)
            {
                for (int i4 = 0; i4 < this.connectedTracks.Count; ++i4)
                {
                    TilePos chunkPosition5 = this.connectedTracks[i4];
                    if (chunkPosition5.x == i1 && chunkPosition5.z == i3)
                    {
                        return true;
                    }
                }

                return false;
            }

            public int GetAdjacentTracks()
            {
                int i1 = 0;
                if (this.IsMinecartTrack(this.trackX, this.trackY, this.trackZ - 1))
                {
                    ++i1;
                }

                if (this.IsMinecartTrack(this.trackX, this.trackY, this.trackZ + 1))
                {
                    ++i1;
                }

                if (this.IsMinecartTrack(this.trackX - 1, this.trackY, this.trackZ))
                {
                    ++i1;
                }

                if (this.IsMinecartTrack(this.trackX + 1, this.trackY, this.trackZ))
                {
                    ++i1;
                }

                return i1;
            }

            private bool HandleKeyPress(Rail railLogic1)
            {
                if (this.IsConnectedTo(railLogic1))
                {
                    return true;
                }
                else if (this.connectedTracks.Count == 2)
                {
                    return false;
                }
                else if (this.connectedTracks.Count == 0)
                {
                    return true;
                }
                else
                {
                    TilePos chunkPosition2 = this.connectedTracks[0];
                    return railLogic1.trackY == this.trackY && chunkPosition2.y == this.trackY ? true : true;
                }
            }

            private void Func_788(Rail railLogic1)
            {
                this.connectedTracks.Add(new TilePos(railLogic1.trackX, railLogic1.trackY, railLogic1.trackZ));
                bool z2 = this.IsInTrack(this.trackX, this.trackY, this.trackZ - 1);
                bool z3 = this.IsInTrack(this.trackX, this.trackY, this.trackZ + 1);
                bool z4 = this.IsInTrack(this.trackX - 1, this.trackY, this.trackZ);
                bool z5 = this.IsInTrack(this.trackX + 1, this.trackY, this.trackZ);
                sbyte b6 = -1;
                if (z2 || z3)
                {
                    b6 = 0;
                }

                if (z4 || z5)
                {
                    b6 = 1;
                }

                if (!this.isPoweredRail)
                {
                    if (z3 && z5 && !z2 && !z4)
                    {
                        b6 = 6;
                    }

                    if (z3 && z4 && !z2 && !z5)
                    {
                        b6 = 7;
                    }

                    if (z2 && z4 && !z3 && !z5)
                    {
                        b6 = 8;
                    }

                    if (z2 && z5 && !z3 && !z4)
                    {
                        b6 = 9;
                    }
                }

                if (b6 == 0)
                {
                    if (RailTile.IsRailBlockAt(this.worldObj, this.trackX, this.trackY + 1, this.trackZ - 1))
                    {
                        b6 = 4;
                    }

                    if (RailTile.IsRailBlockAt(this.worldObj, this.trackX, this.trackY + 1, this.trackZ + 1))
                    {
                        b6 = 5;
                    }
                }

                if (b6 == 1)
                {
                    if (RailTile.IsRailBlockAt(this.worldObj, this.trackX + 1, this.trackY + 1, this.trackZ))
                    {
                        b6 = 2;
                    }

                    if (RailTile.IsRailBlockAt(this.worldObj, this.trackX - 1, this.trackY + 1, this.trackZ))
                    {
                        b6 = 3;
                    }
                }

                if (b6 < 0)
                {
                    b6 = 0;
                }

                int i7 = b6;
                if (this.isPoweredRail)
                {
                    i7 = (sbyte)(this.worldObj.GetData(this.trackX, this.trackY, this.trackZ) & 8) | b6;
                }

                this.worldObj.SetData(this.trackX, this.trackY, this.trackZ, i7);
            }

            private bool Func_786(int i1, int i2, int i3)
            {
                Rail railLogic4 = this.GetMinecartTrackLogic(new TilePos(i1, i2, i3));
                if (railLogic4 == null)
                {
                    return false;
                }
                else
                {
                    railLogic4.Func_785();
                    return railLogic4.HandleKeyPress(this);
                }
            }

            public virtual void F596a(bool z1, bool z2)
            {
                bool z3 = this.Func_786(this.trackX, this.trackY, this.trackZ - 1);
                bool z4 = this.Func_786(this.trackX, this.trackY, this.trackZ + 1);
                bool z5 = this.Func_786(this.trackX - 1, this.trackY, this.trackZ);
                bool z6 = this.Func_786(this.trackX + 1, this.trackY, this.trackZ);
                sbyte b7 = -1;
                if ((z3 || z4) && !z5 && !z6)
                {
                    b7 = 0;
                }

                if ((z5 || z6) && !z3 && !z4)
                {
                    b7 = 1;
                }

                if (!this.isPoweredRail)
                {
                    if (z4 && z6 && !z3 && !z5)
                    {
                        b7 = 6;
                    }

                    if (z4 && z5 && !z3 && !z6)
                    {
                        b7 = 7;
                    }

                    if (z3 && z5 && !z4 && !z6)
                    {
                        b7 = 8;
                    }

                    if (z3 && z6 && !z4 && !z5)
                    {
                        b7 = 9;
                    }
                }

                if (b7 == -1)
                {
                    if (z3 || z4)
                    {
                        b7 = 0;
                    }

                    if (z5 || z6)
                    {
                        b7 = 1;
                    }

                    if (!this.isPoweredRail)
                    {
                        if (z1)
                        {
                            if (z4 && z6)
                            {
                                b7 = 6;
                            }

                            if (z5 && z4)
                            {
                                b7 = 7;
                            }

                            if (z6 && z3)
                            {
                                b7 = 9;
                            }

                            if (z3 && z5)
                            {
                                b7 = 8;
                            }
                        }
                        else
                        {
                            if (z3 && z5)
                            {
                                b7 = 8;
                            }

                            if (z6 && z3)
                            {
                                b7 = 9;
                            }

                            if (z5 && z4)
                            {
                                b7 = 7;
                            }

                            if (z4 && z6)
                            {
                                b7 = 6;
                            }
                        }
                    }
                }

                if (b7 == 0)
                {
                    if (RailTile.IsRailBlockAt(this.worldObj, this.trackX, this.trackY + 1, this.trackZ - 1))
                    {
                        b7 = 4;
                    }

                    if (RailTile.IsRailBlockAt(this.worldObj, this.trackX, this.trackY + 1, this.trackZ + 1))
                    {
                        b7 = 5;
                    }
                }

                if (b7 == 1)
                {
                    if (RailTile.IsRailBlockAt(this.worldObj, this.trackX + 1, this.trackY + 1, this.trackZ))
                    {
                        b7 = 2;
                    }

                    if (RailTile.IsRailBlockAt(this.worldObj, this.trackX - 1, this.trackY + 1, this.trackZ))
                    {
                        b7 = 3;
                    }
                }

                if (b7 < 0)
                {
                    b7 = 0;
                }

                this.SetConnections(b7);
                int i8 = b7;
                if (this.isPoweredRail)
                {
                    i8 = (sbyte)(this.worldObj.GetData(this.trackX, this.trackY, this.trackZ) & 8) | b7;
                }

                if (z2 || this.worldObj.GetData(this.trackX, this.trackY, this.trackZ) != i8)
                {
                    this.worldObj.SetData(this.trackX, this.trackY, this.trackZ, i8);
                    for (int i9 = 0; i9 < this.connectedTracks.Count; ++i9)
                    {
                        Rail railLogic10 = this.GetMinecartTrackLogic(this.connectedTracks[i9]);
                        if (railLogic10 != null)
                        {
                            railLogic10.Func_785();
                            if (railLogic10.HandleKeyPress(this))
                            {
                                railLogic10.Func_788(this);
                            }
                        }
                    }
                }
            }
        }
    }
}