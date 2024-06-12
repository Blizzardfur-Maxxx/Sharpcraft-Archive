using SharpCraft.Core.Stats;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Inventory
{
    public class SlotCrafting : Slot
    {
        private readonly IContainer craftMatrix;
        private Player thePlayer;
        public SlotCrafting(Player entityPlayer1, IContainer iInventory2, IContainer iInventory3, int i4, int i5, int i6) : base(iInventory3, i4, i5, i6)
        {
            this.thePlayer = entityPlayer1;
            this.craftMatrix = iInventory2;
        }

        public override bool IsItemValid(ItemInstance itemStack1)
        {
            return false;
        }

        public override void OnPickupFromSlot(ItemInstance itemStack1)
        {
            itemStack1.OnCrafting(this.thePlayer.worldObj, this.thePlayer);
            if (itemStack1.itemID == Tile.workBench.id)
            {
                this.thePlayer.AddStat(AchievementList.buildWorkBench, 1);
            }
            else if (itemStack1.itemID == Item.pickaxeWood.id)
            {
                this.thePlayer.AddStat(AchievementList.buildPickaxe, 1);
            }
            else if (itemStack1.itemID == Tile.furnace.id)
            {
                this.thePlayer.AddStat(AchievementList.buildFurnace, 1);
            }
            else if (itemStack1.itemID == Item.hoeWood.id)
            {
                this.thePlayer.AddStat(AchievementList.buildHoe, 1);
            }
            else if (itemStack1.itemID == Item.bread.id)
            {
                this.thePlayer.AddStat(AchievementList.makeBread, 1);
            }
            else if (itemStack1.itemID == Item.cake.id)
            {
                this.thePlayer.AddStat(AchievementList.bakeCake, 1);
            }
            else if (itemStack1.itemID == Item.pickaxeStone.id)
            {
                this.thePlayer.AddStat(AchievementList.buildBetterPickaxe, 1);
            }
            else if (itemStack1.itemID == Item.swordWood.id)
            {
                this.thePlayer.AddStat(AchievementList.buildSword, 1);
            }

            for (int i2 = 0; i2 < this.craftMatrix.GetContainerSize(); ++i2)
            {
                ItemInstance itemStack3 = this.craftMatrix.GetItem(i2);
                if (itemStack3 != null)
                {
                    this.craftMatrix.RemoveItem(i2, 1);
                    if (itemStack3.GetItem().HasContainerItem())
                    {
                        this.craftMatrix.SetItem(i2, new ItemInstance(itemStack3.GetItem().GetContainerItem()));
                    }
                }
            }
        }
    }
}