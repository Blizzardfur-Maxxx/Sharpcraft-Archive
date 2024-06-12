using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class DoorTile : Tile
    {
        public DoorTile(int i1, Material material2) : base(i1, material2)
        {
            this.texture = 97;
            if (material2 == Material.metal)
            {
                ++this.texture;
            }

            float f3 = 0.5F;
            float f4 = 1F;
            this.SetShape(0.5F - f3, 0F, 0.5F - f3, 0.5F + f3, f4, 0.5F + f3);
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            if (faceIdx != 0 && faceIdx != TileFace.UP)
            {
                int i3 = this.GetState(i2);
                if ((i3 == 0 || i3 == 2) ^ faceIdx <= TileFace.SOUTH)
                {
                    return this.texture;
                }
                else
                {
                    int i4 = i3 / 2 + ((int)faceIdx & 1 ^ i3);
                    i4 += (i2 & 4) / 4;
                    int i5 = this.texture - (i2 & 8) * 2;
                    if ((i4 & 1) != 0)
                    {
                        i5 = -i5;
                    }

                    return i5;
                }
            }
            else
            {
                return this.texture;
            }
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
            return RenderShape.DOOR;
        }

        public override AABB GetTileAABB(Level world1, int i2, int i3, int i4)
        {
            this.SetBlockBoundsBasedOnState(world1, i2, i3, i4);
            return base.GetTileAABB(world1, i2, i3, i4);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            this.SetBlockBoundsBasedOnState(world1, i2, i3, i4);
            return base.GetAABB(world1, i2, i3, i4);
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            this.SetDir(this.GetState(iBlockAccess1.GetData(i2, i3, i4)));
        }

        public virtual void SetDir(int i1)
        {
            float f2 = 0.1875F;
            this.SetShape(0F, 0F, 0F, 1F, 2F, 1F);
            if (i1 == 0)
            {
                this.SetShape(0F, 0F, 0F, 1F, 1F, f2);
            }

            if (i1 == 1)
            {
                this.SetShape(1F - f2, 0F, 0F, 1F, 1F, 1F);
            }

            if (i1 == 2)
            {
                this.SetShape(0F, 0F, 1F - f2, 1F, 1F, 1F);
            }

            if (i1 == 3)
            {
                this.SetShape(0F, 0F, 0F, f2, 1F, 1F);
            }
        }

        public override void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            this.BlockActivated(world1, i2, i3, i4, entityPlayer5);
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (this.material == Material.metal)
            {
                return true;
            }
            else
            {
                int i6 = world1.GetData(i2, i3, i4);
                if ((i6 & 8) != 0)
                {
                    if (world1.GetTile(i2, i3 - 1, i4) == this.id)
                    {
                        this.BlockActivated(world1, i2, i3 - 1, i4, entityPlayer5);
                    }

                    return true;
                }
                else
                {
                    if (world1.GetTile(i2, i3 + 1, i4) == this.id)
                    {
                        world1.SetData(i2, i3 + 1, i4, (i6 ^ 4) + 8);
                    }

                    world1.SetData(i2, i3, i4, i6 ^ 4);
                    world1.SetTilesDirty(i2, i3 - 1, i4, i2, i3, i4);
                    world1.LevelEvent(entityPlayer5, LevelEventType.DOOR, i2, i3, i4, 0);
                    return true;
                }
            }
        }

        public virtual void OnPoweredBlockChange(Level world1, int i2, int i3, int i4, bool z5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            if ((i6 & 8) != 0)
            {
                if (world1.GetTile(i2, i3 - 1, i4) == this.id)
                {
                    this.OnPoweredBlockChange(world1, i2, i3 - 1, i4, z5);
                }
            }
            else
            {
                bool z7 = (world1.GetData(i2, i3, i4) & 4) > 0;
                if (z7 != z5)
                {
                    if (world1.GetTile(i2, i3 + 1, i4) == this.id)
                    {
                        world1.SetData(i2, i3 + 1, i4, (i6 ^ 4) + 8);
                    }

                    world1.SetData(i2, i3, i4, i6 ^ 4);
                    world1.SetTilesDirty(i2, i3 - 1, i4, i2, i3, i4);
                    world1.LevelEvent((Player)null, LevelEventType.DOOR, i2, i3, i4, 0);
                }
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            if ((i6 & 8) != 0)
            {
                if (world1.GetTile(i2, i3 - 1, i4) != this.id)
                {
                    world1.SetTile(i2, i3, i4, 0);
                }

                if (i5 > 0 && Tile.tiles[i5].IsSignalSource())
                {
                    this.NeighborChanged(world1, i2, i3 - 1, i4, i5);
                }
            }
            else
            {
                bool z7 = false;
                if (world1.GetTile(i2, i3 + 1, i4) != this.id)
                {
                    world1.SetTile(i2, i3, i4, 0);
                    z7 = true;
                }

                if (!world1.IsSolidBlockingTile(i2, i3 - 1, i4))
                {
                    world1.SetTile(i2, i3, i4, 0);
                    z7 = true;
                    if (world1.GetTile(i2, i3 + 1, i4) == this.id)
                    {
                        world1.SetTile(i2, i3 + 1, i4, 0);
                    }
                }

                if (z7)
                {
                    if (!world1.isRemote)
                    {
                        this.DropBlockAsItem(world1, i2, i3, i4, i6);
                    }
                }
                else if (i5 > 0 && Tile.tiles[i5].IsSignalSource())
                {
                    bool z8 = world1.IsBlockIndirectlyGettingPowered(i2, i3, i4) || world1.IsBlockIndirectlyGettingPowered(i2, i3 + 1, i4);
                    this.OnPoweredBlockChange(world1, i2, i3, i4, z8);
                }
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return (i1 & 8) != 0 ? 0 : (this.material == Material.metal ? Item.doorSteel.id : Item.doorWood.id);
        }

        public override HitResult Clip(Level world1, int i2, int i3, int i4, Vec3 vec3D5, Vec3 vec3D6)
        {
            this.SetBlockBoundsBasedOnState(world1, i2, i3, i4);
            return base.Clip(world1, i2, i3, i4, vec3D5, vec3D6);
        }

        public virtual int GetState(int i1)
        {
            return (i1 & 4) == 0 ? i1 - 1 & 3 : i1 & 3;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return i3 >= 127 ? false : world1.IsSolidBlockingTile(i2, i3 - 1, i4) && base.CanPlaceBlockAt(world1, i2, i3, i4) && base.CanPlaceBlockAt(world1, i2, i3 + 1, i4);
        }

        public static bool IsOpen(int i0)
        {
            return (i0 & 4) != 0;
        }

        public override int GetPistonPushReaction()
        {
            return 1;
        }
    }
}