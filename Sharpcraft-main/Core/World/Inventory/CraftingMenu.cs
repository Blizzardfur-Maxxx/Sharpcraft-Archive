using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Items.Crafting;

namespace SharpCraft.Core.World.Inventory
{
    public class CraftingMenu : AbstractContainerMenu
    {
        public CraftingContainer craftMatrix;
        public IContainer craftResult = new ResultContainer();
        private Level wurld;
        private int field_H;
        private int field_I;
        private int field_J;
        public CraftingMenu(Entities.Players.Inventory inventoryPlayer1, Level world2, int i3, int i4, int i5)
        {
            craftMatrix = new CraftingContainer(this, 3, 3);
            this.wurld = world2;
            this.field_H = i3;
            this.field_I = i4;
            this.field_J = i5;
            this.AddSlot(new SlotCrafting(inventoryPlayer1.player, this.craftMatrix, this.craftResult, 0, 124, 35));
            int i6;
            int i7;
            for (i6 = 0; i6 < 3; ++i6)
            {
                for (i7 = 0; i7 < 3; ++i7)
                {
                    this.AddSlot(new Slot(this.craftMatrix, i7 + i6 * 3, 30 + i7 * 18, 17 + i6 * 18));
                }
            }

            for (i6 = 0; i6 < 3; ++i6)
            {
                for (i7 = 0; i7 < 9; ++i7)
                {
                    this.AddSlot(new Slot(inventoryPlayer1, i7 + i6 * 9 + 9, 8 + i7 * 18, 84 + i6 * 18));
                }
            }

            for (i6 = 0; i6 < 9; ++i6)
            {
                this.AddSlot(new Slot(inventoryPlayer1, i6, 8 + i6 * 18, 142));
            }

            this.OnCraftMatrixChanged(this.craftMatrix);
        }

        public override void OnCraftMatrixChanged(IContainer iInventory1)
        {
            this.craftResult.SetItem(0, Recipes.GetInstance().FindMatchingRecipe(this.craftMatrix));
        }

        public override void Removed(Player entityPlayer1)
        {
            base.Removed(entityPlayer1);
            if (!this.wurld.isRemote)
            {
                for (int i2 = 0; i2 < 9; ++i2)
                {
                    ItemInstance itemStack3 = this.craftMatrix.GetItem(i2);
                    if (itemStack3 != null)
                    {
                        entityPlayer1.DropPlayerItem(itemStack3);
                    }
                }
            }
        }

        public override bool StillValid(Player entityPlayer1)
        {
            return this.wurld.GetTile(this.field_H, this.field_I, this.field_J) != Tile.workBench.id ? false : entityPlayer1.GetDistanceSq(this.field_H + 0.5, this.field_I + 0.5, this.field_J + 0.5) <= 64;
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
                    this.Func_28126(itemStack4, 10, 46, true);
                }
                else if (i1 >= 10 && i1 < 37)
                {
                    this.Func_28126(itemStack4, 37, 46, false);
                }
                else if (i1 >= 37 && i1 < 46)
                {
                    this.Func_28126(itemStack4, 10, 37, false);
                }
                else
                {
                    this.Func_28126(itemStack4, 10, 46, false);
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