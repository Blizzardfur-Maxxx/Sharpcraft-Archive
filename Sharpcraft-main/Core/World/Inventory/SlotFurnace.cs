using SharpCraft.Core.Stats;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Inventory
{
    public class SlotFurnace : Slot
    {
        private Player thePlayer;
        public SlotFurnace(Player entityPlayer1, IContainer iInventory2, int i3, int i4, int i5) : base(iInventory2, i3, i4, i5)
        {
            this.thePlayer = entityPlayer1;
        }

        public override bool IsItemValid(ItemInstance itemStack1)
        {
            return false;
        }

        public override void OnPickupFromSlot(ItemInstance itemStack1)
        {
            itemStack1.OnCrafting(this.thePlayer.worldObj, this.thePlayer);
            if (itemStack1.itemID == Item.ingotIron.id)
            {
                this.thePlayer.AddStat(AchievementList.acquireIron, 1);
            }

            if (itemStack1.itemID == Item.fishCooked.id)
            {
                this.thePlayer.AddStat(AchievementList.cookFish, 1);
            }

            base.OnPickupFromSlot(itemStack1);
        }
    }
}