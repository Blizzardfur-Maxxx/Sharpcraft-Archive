using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Items
{
    public class ItemBow : Item
    {
        public ItemBow(int i1) : base(i1)
        {
            this.maxStackSize = 1;
        }

        public override ItemInstance OnItemRightClick(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            if (entityPlayer3.inventory.ConsumeInventoryItem(Item.arrow.id))
            {
                world2.PlaySound(entityPlayer3, "random.bow", 1F, 1F / (itemRand.NextFloat() * 0.4F + 0.8F));
                if (!world2.isRemote)
                {
                    world2.AddEntity(new Arrow(world2, entityPlayer3));
                }
            }

            return itemStack1;
        }
    }
}