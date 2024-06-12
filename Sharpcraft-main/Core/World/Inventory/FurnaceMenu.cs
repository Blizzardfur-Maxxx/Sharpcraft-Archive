using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Inventory
{
    public class FurnaceMenu : AbstractContainerMenu
    {
        private TileEntityFurnace furnace;
        private int cookTime = 0;
        private int burnTime = 0;
        private int itemBurnTime = 0;
        public FurnaceMenu(Entities.Players.Inventory inventoryPlayer1, TileEntityFurnace tileEntityFurnace2)
        {
            this.furnace = tileEntityFurnace2;
            this.AddSlot(new Slot(tileEntityFurnace2, 0, 56, 17));
            this.AddSlot(new Slot(tileEntityFurnace2, 1, 56, 53));
            this.AddSlot(new SlotFurnace(inventoryPlayer1.player, tileEntityFurnace2, 2, 116, 35));
            int i3;
            for (i3 = 0; i3 < 3; ++i3)
            {
                for (int i4 = 0; i4 < 9; ++i4)
                {
                    this.AddSlot(new Slot(inventoryPlayer1, i4 + i3 * 9 + 9, 8 + i4 * 18, 84 + i3 * 18));
                }
            }

            for (i3 = 0; i3 < 9; ++i3)
            {
                this.AddSlot(new Slot(inventoryPlayer1, i3, 8 + i3 * 18, 142));
            }
        }

        public override void OnCraftGuiOpened(IContainerListener iCrafting1)
        {
            base.OnCraftGuiOpened(iCrafting1);
            iCrafting1.UpdateCraftingInventoryInfo(this, 0, this.furnace.furnaceCookTime);
            iCrafting1.UpdateCraftingInventoryInfo(this, 1, this.furnace.furnaceBurnTime);
            iCrafting1.UpdateCraftingInventoryInfo(this, 2, this.furnace.currentItemBurnTime);
        }

        public override void UpdateCraftingResults()
        {
            base.UpdateCraftingResults();
            for (int i1 = 0; i1 < this.listeners.Count; ++i1)
            {
                IContainerListener iCrafting2 = this.listeners[i1];
                if (this.cookTime != this.furnace.furnaceCookTime)
                {
                    iCrafting2.UpdateCraftingInventoryInfo(this, 0, this.furnace.furnaceCookTime);
                }

                if (this.burnTime != this.furnace.furnaceBurnTime)
                {
                    iCrafting2.UpdateCraftingInventoryInfo(this, 1, this.furnace.furnaceBurnTime);
                }

                if (this.itemBurnTime != this.furnace.currentItemBurnTime)
                {
                    iCrafting2.UpdateCraftingInventoryInfo(this, 2, this.furnace.currentItemBurnTime);
                }
            }

            this.cookTime = this.furnace.furnaceCookTime;
            this.burnTime = this.furnace.furnaceBurnTime;
            this.itemBurnTime = this.furnace.currentItemBurnTime;
        }

        public override void Func_20112(int i1, int i2)
        {
            if (i1 == 0)
            {
                this.furnace.furnaceCookTime = i2;
            }

            if (i1 == 1)
            {
                this.furnace.furnaceBurnTime = i2;
            }

            if (i1 == 2)
            {
                this.furnace.currentItemBurnTime = i2;
            }
        }

        public override bool StillValid(Player entityPlayer1)
        {
            return this.furnace.StillValid(entityPlayer1);
        }

        public override ItemInstance GetStackInSlot(int i1)
        {
            ItemInstance itemStack2 = null;
            Slot slot3 = this.slots[i1];
            if (slot3 != null && slot3.HasItem())
            {
                ItemInstance itemStack4 = slot3.GetItem();
                itemStack2 = itemStack4.Copy();
                if (i1 == 2)
                {
                    this.Func_28126(itemStack4, 3, 39, true);
                }
                else if (i1 >= 3 && i1 < 30)
                {
                    this.Func_28126(itemStack4, 30, 39, false);
                }
                else if (i1 >= 30 && i1 < 39)
                {
                    this.Func_28126(itemStack4, 3, 30, false);
                }
                else
                {
                    this.Func_28126(itemStack4, 3, 39, false);
                }

                if (itemStack4.stackSize == 0)
                {
                    slot3.PutItem((ItemInstance)null);
                }
                else
                {
                    slot3.OnSlotChanged();
                }

                if (itemStack4.stackSize == itemStack2.stackSize)
                {
                    return null;
                }

                slot3.OnPickupFromSlot(itemStack4);
            }

            return itemStack2;
        }
    }
}