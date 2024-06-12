using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class LockedChestTile : Tile
    {
        public LockedChestTile(int i1) : base(i1, Material.wood)
        {
            this.texture = 26;
        }

        public override int GetBlockTexture(ILevelSource iBlockAccess1, int i2, int i3, int i4, TileFace i5)
        {
            if (i5 == TileFace.UP)
            {
                return this.texture - 1;
            }
            else if (i5 == TileFace.DOWN)
            {
                return this.texture - 1;
            }
            else
            {
                int i6 = iBlockAccess1.GetTile(i2, i3, i4 - 1);
                int i7 = iBlockAccess1.GetTile(i2, i3, i4 + 1);
                int i8 = iBlockAccess1.GetTile(i2 - 1, i3, i4);
                int i9 = iBlockAccess1.GetTile(i2 + 1, i3, i4);
                TileFace b10 = TileFace.SOUTH;
                if (Tile.solid[i6] && !Tile.solid[i7])
                {
                    b10 = TileFace.SOUTH;
                }

                if (Tile.solid[i7] && !Tile.solid[i6])
                {
                    b10 = TileFace.NORTH;
                }

                if (Tile.solid[i8] && !Tile.solid[i9])
                {
                    b10 = TileFace.EAST;
                }

                if (Tile.solid[i9] && !Tile.solid[i8])
                {
                    b10 = TileFace.WEST;
                }

                return i5 == b10 ? this.texture + 1 : this.texture;
            }
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.UP ? this.texture - 1 : (faceIdx == 0 ? this.texture - 1 : (faceIdx == TileFace.SOUTH ? this.texture + 1 : this.texture));
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return true;
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            world1.SetTile(i2, i3, i4, 0);
        }
    }
}