using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Materials;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class PumpkinTile : Tile
    {
        private bool blockType;
        public PumpkinTile(int i1, int i2, bool z3) : base(i1, Material.vegetable)
        {
            this.texture = i2;
            this.SetTicking(true);
            this.blockType = z3;
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            if (faceIdx == TileFace.UP)
            {
                return this.texture;
            }
            else if (faceIdx == TileFace.DOWN)
            {
                return this.texture;
            }
            else
            {
                int i3 = this.texture + 1 + 16;
                if (this.blockType)
                {
                    ++i3;
                }

                return i2 == 2 && faceIdx == TileFace.NORTH ? i3 : (i2 == 3 && faceIdx == TileFace.EAST ? i3 : (i2 == 0 && faceIdx == TileFace.SOUTH ? i3 : (i2 == 1 && faceIdx == TileFace.WEST ? i3 : this.texture + 16)));
            }
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.UP ? this.texture : (faceIdx == 0 ? this.texture : (faceIdx == TileFace.SOUTH ? this.texture + 1 + 16 : this.texture + 16));
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            base.OnPlace(world1, i2, i3, i4);
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetTile(i2, i3, i4);
            return (i5 == 0 || Tile.tiles[i5].material.GetIsGroundCover()) && world1.IsSolidBlockingTile(i2, i3 - 1, i4);
        }

        public override void OnBlockPlacedBy(Level world1, int i2, int i3, int i4, Mob entityLiving5)
        {
            int i6 = Mth.Floor(entityLiving5.yaw * 4F / 360F + 2.5) & 3;
            world1.SetData(i2, i3, i4, i6);
        }
    }
}