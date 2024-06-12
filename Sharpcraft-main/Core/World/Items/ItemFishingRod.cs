using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Items
{
    public class ItemFishingRod : Item
    {
        public ItemFishingRod(int i1) : base(i1)
        {
            this.SetMaxDamage(64);
            this.SetMaxStackSize(1);
        }

        public override bool IsFull3D()
        {
            return true;
        }

        public override bool ShouldRotateAroundWhenRendering()
        {
            return true;
        }

        public override ItemInstance OnItemRightClick(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            if (entityPlayer3.fishEntity != null)
            {
                int i4 = entityPlayer3.fishEntity.CatchFish();
                itemStack1.DamageItem(i4, entityPlayer3);
                entityPlayer3.SwingItem();
            }
            else
            {
                world2.PlaySound(entityPlayer3, "random.bow", 0.5F, 0.4F / (itemRand.NextFloat() * 0.4F + 0.8F));
                if (!world2.isRemote)
                {
                    world2.AddEntity(new FishingHook(world2, entityPlayer3));
                }

                entityPlayer3.SwingItem();
            }

            return itemStack1;
        }
    }
}