using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Items
{
    public class ItemEgg : Item
    {
        public ItemEgg(int i1) : base(i1)
        {
            this.maxStackSize = 16;
        }

        public override ItemInstance OnItemRightClick(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            --itemStack1.stackSize;
            world2.PlaySound(entityPlayer3, "random.bow", 0.5F, 0.4F / (itemRand.NextFloat() * 0.4F + 0.8F));
            if (!world2.isRemote)
            {
                world2.AddEntity(new ThrownEgg(world2, entityPlayer3));
            }

            return itemStack1;
        }
    }
}