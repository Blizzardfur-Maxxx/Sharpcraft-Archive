using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemRedstone : Item
    {
        public ItemRedstone(int i1) : base(i1)
        {
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            if (world3.GetTile(i4, i5, i6) != Tile.topSnow.id)
            {
                if (i7 == TileFace.DOWN)
                {
                    --i5;
                }

                if (i7 == TileFace.UP)
                {
                    ++i5;
                }

                if (i7 == TileFace.NORTH)
                {
                    --i6;
                }

                if (i7 == TileFace.SOUTH)
                {
                    ++i6;
                }

                if (i7 == TileFace.WEST)
                {
                    --i4;
                }

                if (i7 == TileFace.EAST)
                {
                    ++i4;
                }

                if (!world3.IsAirBlock(i4, i5, i6))
                {
                    return false;
                }
            }

            if (Tile.redstoneWire.CanPlaceBlockAt(world3, i4, i5, i6))
            {
                --itemStack1.stackSize;
                world3.SetTile(i4, i5, i6, Tile.redstoneWire.id);
            }

            return true;
        }
    }
}