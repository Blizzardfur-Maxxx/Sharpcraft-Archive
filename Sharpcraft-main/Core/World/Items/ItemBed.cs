using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemBed : Item
    {
        public ItemBed(int i1) : base(i1)
        {
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            if (i7 != TileFace.UP)
            {
                return false;
            }
            else
            {
                ++i5;
                BedTile blockBed8 = (BedTile)Tile.bed;
                int i9 = Mth.Floor(entityPlayer2.yaw * 4F / 360F + 0.5) & 3;
                sbyte b10 = 0;
                sbyte b11 = 0;
                if (i9 == 0)
                {
                    b11 = 1;
                }

                if (i9 == 1)
                {
                    b10 = -1;
                }

                if (i9 == 2)
                {
                    b11 = -1;
                }

                if (i9 == 3)
                {
                    b10 = 1;
                }

                if (world3.IsAirBlock(i4, i5, i6) && world3.IsAirBlock(i4 + b10, i5, i6 + b11) && world3.IsSolidBlockingTile(i4, i5 - 1, i6) && world3.IsSolidBlockingTile(i4 + b10, i5 - 1, i6 + b11))
                {
                    world3.SetTileAndData(i4, i5, i6, blockBed8.id, i9);
                    world3.SetTileAndData(i4 + b10, i5, i6 + b11, blockBed8.id, i9 + 8);
                    --itemStack1.stackSize;
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}