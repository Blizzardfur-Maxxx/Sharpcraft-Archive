using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Items.Crafting;

namespace SharpCraft.Core.World.Inventory
{
    public class InventoryMenu : AbstractContainerMenu
    {
        public CraftingContainer craftMatrix;
        public IContainer craftResult;
        public bool isMP;
        public InventoryMenu(Entities.Players.Inventory inventoryPlayer1) : this(inventoryPlayer1, true)
        {
        }

        public InventoryMenu(Entities.Players.Inventory inventoryPlayer1, bool z2)
        {
            this.craftMatrix = new CraftingContainer(this, 2, 2);
            this.craftResult = new ResultContainer();
            this.isMP = false;
            this.isMP = z2;
            this.AddSlot(new SlotCrafting(inventoryPlayer1.player, this.craftMatrix, this.craftResult, 0, 144, 36));
            int i3;
            int i4;
            for (i3 = 0; i3 < 2; ++i3)
            {
                for (i4 = 0; i4 < 2; ++i4)
                {
                    this.AddSlot(new Slot(this.craftMatrix, i4 + i3 * 2, 88 + i4 * 18, 26 + i3 * 18));
                }
            }

            for (i3 = 0; i3 < 4; ++i3)
            {
                this.AddSlot(new AnonymousSlot(i3, inventoryPlayer1, inventoryPlayer1.GetContainerSize() - 1 - i3, 8, 8 + i3 * 18));
            }

            for (i3 = 0; i3 < 3; ++i3)
            {
                for (i4 = 0; i4 < 9; ++i4)
                {
                    this.AddSlot(new Slot(inventoryPlayer1, i4 + (i3 + 1) * 9, 8 + i4 * 18, 84 + i3 * 18));
                }
            }

            for (i3 = 0; i3 < 9; ++i3)
            {
                this.AddSlot(new Slot(inventoryPlayer1, i3, 8 + i3 * 18, 142));
            }

            this.OnCraftMatrixChanged(this.craftMatrix);
        }

        private sealed class AnonymousSlot : Slot
        {
            private readonly int armorType;

            public AnonymousSlot(int armorType, IContainer iInventory1, int i2, int i3, int i4) : base(iInventory1, i2, i3, i4)
            {
                this.armorType = armorType;
            }

            public override int GetSlotStackLimit()
            {
                return 1;
            }

            public override bool IsItemValid(ItemInstance itemStack1)
            {
                return itemStack1.GetItem() is ItemArmor ? ((ItemArmor)itemStack1.GetItem()).armorType == this.armorType : (itemStack1.GetItem().id == Tile.pumpkin.id ? armorType == 0 : false);
            }
        }

        public override void OnCraftMatrixChanged(IContainer iInventory1)
        {
            this.craftResult.SetItem(0, Recipes.GetInstance().FindMatchingRecipe(this.craftMatrix));
        }

        public override void Removed(Player entityPlayer1)
        {
            base.Removed(entityPlayer1);
            for (int i2 = 0; i2 < 4; ++i2)
            {
                ItemInstance itemStack3 = this.craftMatrix.GetItem(i2);
                if (itemStack3 != null)
                {
                    entityPlayer1.DropPlayerItem(itemStack3);
                    this.craftMatrix.SetItem(i2, (ItemInstance)null);
                }
            }
        }

        public override bool StillValid(Player entityPlayer1)
        {
            return true;
        }

        public override ItemInstance GetStackInSlot(int i1)
        {
            ItemInstance itemStack2 = null;
            Slot slot3 = this.slots[i1];
            if (slot3 != null && slot3.HasItem())
            {
                ItemInstance itemStack4 = slot3.GetItem();
                itemStack2 = itemStack4.Copy();
                if (i1 == 0)
                {
                    this.Func_28126(itemStack4, 9, 45, true);
                }
                else if (i1 >= 9 && i1 < 36)
                {
                    this.Func_28126(itemStack4, 36, 45, false);
                }
                else if (i1 >= 36 && i1 < 45)
                {
                    this.Func_28126(itemStack4, 9, 36, false);
                }
                else
                {
                    this.Func_28126(itemStack4, 9, 45, false);
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