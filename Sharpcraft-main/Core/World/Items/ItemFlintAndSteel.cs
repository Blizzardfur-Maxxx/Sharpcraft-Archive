using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class ItemFlintAndSteel : Item
    {
        public ItemFlintAndSteel(int i1) : base(i1)
        {
            this.maxStackSize = 1;
            this.SetMaxDamage(64);
        }

        public override bool OnItemUse(ItemInstance itemStack1, Player entityPlayer2, Level world3, int i4, int i5, int i6, TileFace i7)
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

            int i8 = world3.GetTile(i4, i5, i6);
            if (i8 == 0)
            {
                world3.PlaySound(i4 + 0.5, i5 + 0.5, i6 + 0.5, "fire.ignite", 1F, itemRand.NextFloat() * 0.4F + 0.8F);
                world3.SetTile(i4, i5, i6, Tile.fire.id);
            }

            itemStack1.DamageItem(1, entityPlayer2);
            return true;
        }
    }
}