using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class SpringFeature : Feature
    {
        private int liquidBlockId;
        public SpringFeature(int i1)
        {
            this.liquidBlockId = i1;
        }

        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            if (world1.GetTile(i3, i4 + 1, i5) != Tile.rock.id)
            {
                return false;
            }
            else if (world1.GetTile(i3, i4 - 1, i5) != Tile.rock.id)
            {
                return false;
            }
            else if (world1.GetTile(i3, i4, i5) != 0 && world1.GetTile(i3, i4, i5) != Tile.rock.id)
            {
                return false;
            }
            else
            {
                int i6 = 0;
                if (world1.GetTile(i3 - 1, i4, i5) == Tile.rock.id)
                {
                    ++i6;
                }

                if (world1.GetTile(i3 + 1, i4, i5) == Tile.rock.id)
                {
                    ++i6;
                }

                if (world1.GetTile(i3, i4, i5 - 1) == Tile.rock.id)
                {
                    ++i6;
                }

                if (world1.GetTile(i3, i4, i5 + 1) == Tile.rock.id)
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

                if (i6 == 3 && i7 == 1)
                {
                    world1.SetTile(i3, i4, i5, this.liquidBlockId);
                    world1.scheduledUpdatesAreImmediate = true;
                    Tile.tiles[this.liquidBlockId].Tick(world1, i3, i4, i5, random2);
                    world1.scheduledUpdatesAreImmediate = false;
                }

                return true;
            }
        }
    }
}