using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class CactusTile : Tile
    {
        public CactusTile(int i1, int i2) : base(i1, i2, Material.cactus)
        {
            this.SetTicking(true);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (world1.IsAirBlock(i2, i3 + 1, i4))
            {
                int i6;
                for (i6 = 1; world1.GetTile(i2, i3 - i6, i4) == this.id; ++i6)
                {
                }

                if (i6 < 3)
                {
                    int i7 = world1.GetData(i2, i3, i4);
                    if (i7 == 15)
                    {
                        world1.SetTile(i2, i3 + 1, i4, this.id);
                        world1.SetData(i2, i3, i4, 0);
                    }
                    else
                    {
                        world1.SetData(i2, i3, i4, i7 + 1);
                    }
                }
            }
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            float f5 = 0.0625F;
            return AABB.Of(i2 + f5, i3, i4 + f5, i2 + 1 - f5, i3 + 1 - f5, i4 + 1 - f5);
        }

        public override AABB GetTileAABB(Level world1, int i2, int i3, int i4)
        {
            float f5 = 0.0625F;
            return AABB.Of(i2 + f5, i3, i4 + f5, i2 + 1 - f5, i3 + 1, i4 + 1 - f5);
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.UP ? this.texture - 1 : (faceIdx == 0 ? this.texture + 1 : this.texture);
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.CACTUS;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return !base.CanPlaceBlockAt(world1, i2, i3, i4) ? false : this.CanBlockStay(world1, i2, i3, i4);
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!this.CanBlockStay(world1, i2, i3, i4))
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
            }
        }

        public override bool CanBlockStay(Level world1, int i2, int i3, int i4)
        {
            if (world1.GetMaterial(i2 - 1, i3, i4).IsSolid())
            {
                return false;
            }
            else if (world1.GetMaterial(i2 + 1, i3, i4).IsSolid())
            {
                return false;
            }
            else if (world1.GetMaterial(i2, i3, i4 - 1).IsSolid())
            {
                return false;
            }
            else if (world1.GetMaterial(i2, i3, i4 + 1).IsSolid())
            {
                return false;
            }
            else
            {
                int i5 = world1.GetTile(i2, i3 - 1, i4);
                return i5 == Tile.cactus.id || i5 == Tile.sand.id;
            }
        }

        public override void OnEntityCollidedWithBlock(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            entity5.AttackEntityFrom((Entity)null, 1);
        }
    }
}