using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class TallGrassFeature : Feature
    {
        private int field_28060_a;
        private int field_28059_b;
        public TallGrassFeature(int i1, int i2)
        {
            this.field_28060_a = i1;
            this.field_28059_b = i2;
        }

        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            int i11;
            for (; ((i11 = world1.GetTile(i3, i4, i5)) == 0 || i11 == Tile.leaves.id) && i4 > 0; --i4)
            {
            }

            for (int i7 = 0; i7 < 128; ++i7)
            {
                int i8 = i3 + random2.NextInt(8) - random2.NextInt(8);
                int i9 = i4 + random2.NextInt(4) - random2.NextInt(4);
                int i10 = i5 + random2.NextInt(8) - random2.NextInt(8);
                if (world1.IsAirBlock(i8, i9, i10) && ((Bush)Tile.tiles[this.field_28060_a]).CanBlockStay(world1, i8, i9, i10))
                {
                    world1.SetTileAndDataNoUpdate(i8, i9, i10, this.field_28060_a, this.field_28059_b);
                }
            }

            return true;
        }
    }
}