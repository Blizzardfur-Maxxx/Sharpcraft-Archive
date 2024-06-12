using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemSeeds : Item
    {
        private int field_318_a;
        public ItemSeeds(int i1, int i2) : base(i1)
        {
            this.field_318_a = i2;
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
        {
            if (i7 != TileFace.UP)
            {
                return false;
            }
            else
            {
                int i8 = world3.GetTile(i4, i5, i6);
                if (i8 == Tile.farmland.id && world3.IsAirBlock(i4, i5 + 1, i6))
                {
                    world3.SetTile(i4, i5 + 1, i6, this.field_318_a);
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