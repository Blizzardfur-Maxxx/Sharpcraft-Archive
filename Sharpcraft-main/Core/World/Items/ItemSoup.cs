using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Items
{
    public class ItemSoup : ItemFood
    {
        public ItemSoup(int i1, int i2) : base(i1, i2, false)
        {
        }

        public override ItemInstance OnItemRightClick(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            base.OnItemRightClick(itemStack1, world2, entityPlayer3);
            return new ItemInstance(Item.bowlEmpty);
        }
    }
}