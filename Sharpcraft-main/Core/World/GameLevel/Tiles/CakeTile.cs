using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class CakeTile : Tile
    {
        public CakeTile(int i1, int i2) : base(i1, i2, Material.cake)
        {
            this.SetTicking(true);
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            int i5 = iBlockAccess1.GetData(i2, i3, i4);
            float f6 = 0.0625F;
            float f7 = (1 + i5 * 2) / 16F;
            float f8 = 0.5F;
            this.SetShape(f7, 0F, f6, 1F - f6, f8, 1F - f6);
        }

        public override void SetBlockBoundsForItemRender()
        {
            float f1 = 0.0625F;
            float f2 = 0.5F;
            this.SetShape(f1, 0F, f1, 1F - f1, f2, 1F - f1);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            float f6 = 0.0625F;
            float f7 = (1 + i5 * 2) / 16F;
            float f8 = 0.5F;
            return AABB.Of(i2 + f7, i3, i4 + f6, i2 + 1 - f6, i3 + f8 - f6, i4 + 1 - f6);
        }

        public override AABB GetTileAABB(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            float f6 = 0.0625F;
            float f7 = (1 + i5 * 2) / 16F;
            float f8 = 0.5F;
            return AABB.Of(i2 + f7, i3, i4 + f6, i2 + 1 - f6, i3 + f8, i4 + 1 - f6);
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return faceIdx == TileFace.UP ? this.texture : (faceIdx == 0 ? this.texture + 3 : (i2 > 0 && faceIdx == TileFace.WEST ? this.texture + 2 : this.texture + 1));
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.UP ? this.texture : (faceIdx == 0 ? this.texture + 3 : this.texture + 1);
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            this.EatCakeSlice(world1, i2, i3, i4, entityPlayer5);
            return true;
        }

        public override void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            this.EatCakeSlice(world1, i2, i3, i4, entityPlayer5);
        }

        private void EatCakeSlice(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (entityPlayer5.health < 20)
            {
                entityPlayer5.Heal(3);
                int i6 = world1.GetData(i2, i3, i4) + 1;
                if (i6 >= 6)
                {
                    world1.SetTile(i2, i3, i4, 0);
                }
                else
                {
                    world1.SetData(i2, i3, i4, i6);
                    world1.SetTileDirty(i2, i3, i4);
                }
            }
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
            return world1.GetMaterial(i2, i3 - 1, i4).IsSolid();
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return 0;
        }
    }
}