using SharpCraft.Core.World.GameLevel.Materials;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class ClothTile : Tile
    {
        public ClothTile() : base(35, 64, Material.cloth)
        {
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            if (i2 == 0)
            {
                return this.texture;
            }
            else
            {
                i2 = ~(i2 & 15);
                return 113 + ((i2 & 8) >> 3) + (i2 & 7) * 16;
            }
        }

        protected override int GetSpawnResourcesAuxValue(int i1)
        {
            return i1;
        }

        public static int GetMetadataColor0(int i0)
        {
            return ~i0 & 15;
        }

        public static int GetMetadataColor1(int i0)
        {
            return ~i0 & 15;
        }
    }
}