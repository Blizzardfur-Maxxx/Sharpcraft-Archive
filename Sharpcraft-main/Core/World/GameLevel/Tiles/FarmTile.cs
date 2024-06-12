using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class FarmTile : Tile
    {
        public FarmTile(int i1) : base(i1, Material.dirt)
        {
            this.texture = 87;
            this.SetTicking(true);
            this.SetShape(0F, 0F, 0F, 1F, 0.9375F, 1F);
            this.SetLightBlock(255);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return AABB.Of(i2 + 0, i3 + 0, i4 + 0, i2 + 1, i3 + 1, i4 + 1);
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return faceIdx == TileFace.UP && i2 > 0 ? this.texture - 1 : (faceIdx == TileFace.UP ? this.texture : 2);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (random5.NextInt(5) == 0)
            {
                if (!this.IsNearWater(world1, i2, i3, i4) && !world1.CanLightningStrikeAt(i2, i3 + 1, i4))
                {
                    int i6 = world1.GetData(i2, i3, i4);
                    if (i6 > 0)
                    {
                        world1.SetData(i2, i3, i4, i6 - 1);
                    }
                    else if (!this.IsUnderCrops(world1, i2, i3, i4))
                    {
                        world1.SetTile(i2, i3, i4, Tile.dirt.id);
                    }
                }
                else
                {
                    world1.SetData(i2, i3, i4, 7);
                }
            }
        }

        public override void StepOn(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            if (world1.rand.NextInt(4) == 0)
            {
                world1.SetTile(i2, i3, i4, Tile.dirt.id);
            }
        }

        private bool IsUnderCrops(Level world1, int i2, int i3, int i4)
        {
            byte b5 = 0;
            for (int i6 = i2 - b5; i6 <= i2 + b5; ++i6)
            {
                for (int i7 = i4 - b5; i7 <= i4 + b5; ++i7)
                {
                    if (world1.GetTile(i6, i3 + 1, i7) == Tile.crops.id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsNearWater(Level world1, int i2, int i3, int i4)
        {
            for (int i5 = i2 - 4; i5 <= i2 + 4; ++i5)
            {
                for (int i6 = i3; i6 <= i3 + 1; ++i6)
                {
                    for (int i7 = i4 - 4; i7 <= i4 + 4; ++i7)
                    {
                        if (world1.GetMaterial(i5, i6, i7) == Material.water)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            base.NeighborChanged(world1, i2, i3, i4, i5);
            Material material6 = world1.GetMaterial(i2, i3 + 1, i4);
            if (material6.IsSolid())
            {
                world1.SetTile(i2, i3, i4, Tile.dirt.id);
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.dirt.GetResource(0, random2);
        }
    }
}