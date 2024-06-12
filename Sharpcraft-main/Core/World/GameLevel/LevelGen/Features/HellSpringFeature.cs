using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class HellSpringFeature : Feature
    {
        private int field_4158_a;
        public HellSpringFeature(int i1)
        {
            this.field_4158_a = i1;
        }

        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            if (world1.GetTile(i3, i4 + 1, i5) != Tile.netherrack.id)
            {
                return false;
            }
            else if (world1.GetTile(i3, i4, i5) != 0 && world1.GetTile(i3, i4, i5) != Tile.netherrack.id)
            {
                return false;
            }
            else
            {
                int i6 = 0;
                if (world1.GetTile(i3 - 1, i4, i5) == Tile.netherrack.id)
                {
                    ++i6;
                }

                if (world1.GetTile(i3 + 1, i4, i5) == Tile.netherrack.id)
                {
                    ++i6;
                }

                if (world1.GetTile(i3, i4, i5 - 1) == Tile.netherrack.id)
                {
                    ++i6;
                }

                if (world1.GetTile(i3, i4, i5 + 1) == Tile.netherrack.id)
                {
                    ++i6;
                }

                if (world1.GetTile(i3, i4 - 1, i5) == Tile.netherrack.id)
                {
                    ++i6;
                }

                int i7 = 0;
                if (world1.IsAirBlock(i3 - 1, i4, i5))
                {
                    ++i7;
                }

                if (world1.IsAirBlock(i3 + 1, i4, i5))
                {
                    ++i7;
                }

                if (world1.IsAirBlock(i3, i4, i5 - 1))
                {
                    ++i7;
                }

                if (world1.IsAirBlock(i3, i4, i5 + 1))
                {
                    ++i7;
                }

                if (world1.IsAirBlock(i3, i4 - 1, i5))
                {
                    ++i7;
                }

                if (i6 == 4 && i7 == 1)
                {
                    world1.SetTile(i3, i4, i5, this.field_4158_a);
                    world1.scheduledUpdatesAreImmediate = true;
                    Tile.tiles[this.field_4158_a].Tick(world1, i3, i4, i5, random2);
                    world1.scheduledUpdatesAreImmediate = false;
                }

                return true;
            }
        }
    }
}