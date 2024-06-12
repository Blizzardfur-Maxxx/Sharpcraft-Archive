using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Inventory
{
    public abstract class AbstractContainerMenu
    {
        public IList<ItemInstance> items = new List<ItemInstance>();
        public IList<Slot> slots = new List<Slot>();
        public int windowId = 0;
        private short shortWindowId = 0;
        protected IList<IContainerListener> listeners = new List<IContainerListener>();
        private HashSet<Player> players = new HashSet<Player>();
        protected virtual void AddSlot(Slot slot1)
        {
            slot1.id = this.slots.Count;
            this.slots.Add(slot1);
            this.items.Add(null);
        }

        public virtual void OnCraftGuiOpened(IContainerListener iCrafting1)
        {
            if (this.listeners.Contains(iCrafting1))
            {
                throw new ArgumentException("Listener already listening");
            }
            else
            {
                this.listeners.Add(iCrafting1);
                iCrafting1.UpdateCraftingInventory(this, this.GetItems());
                this.UpdateCraftingResults();
            }
        }

        public virtual IList<ItemInstance> GetItems()
        {
            List<ItemInstance> arrayList1 = new List<ItemInstance>();
            for (int i2 = 0; i2 < this.slots.Count; ++i2)
            {
                arrayList1.Add(this.slots[i2].GetItem());
            }

            return arrayList1;
        }

        public virtual void UpdateCraftingResults()
        {
            for (int i1 = 0; i1 < this.slots.Count; ++i1)
            {
                ItemInstance itemStack2 = this.slots[i1].GetItem();
                ItemInstance itemStack3 = this.items[i1];
                if (!ItemInstance.AreItemStacksEqual(itemStack3, itemStack2))
                {
                    itemStack3 = itemStack2 == null ? null : itemStack2.Copy();
                    this.items[i1] = itemStack3;
                    for (int i4 = 0; i4 < this.listeners.Count; ++i4)
                    {
                        this.listeners[i4].UpdateCraftingInventorySlot(this, i1, itemStack3);
                    }
                }
            }
        }

        public virtual Slot GetSlotFor(IContainer iInventory1, int i2)
        {
            for (int i3 = 0; i3 < this.slots.Count; ++i3)
            {
                Slot slot4 = this.slots[i3];
                if (slot4.IsHere(iInventory1, i2))
                {
                    return slot4;
                }
            }

            return null;
        }

        public virtual Slot GetSlot(int i1)
        {
            return this.slots[i1];
        }

        public virtual ItemInstance GetStackInSlot(int i1)
        {
            Slot slot2 = this.slots[i1];
            return slot2 != null ? slot2.GetItem() : null;
        }

        public virtual ItemInstance Func_27085(int i1, int i2, bool z3, Player entityPlayer4)
        {
            ItemInstance itemStack5 = null;
            if (i2 == 0 || i2 == 1)
            {
                Entities.Players.Inventory inventoryPlayer6 = entityPlayer4.inventory;
                if (i1 == -999)
                {
                    if (inventoryPlayer6.GetItem() != null && i1 == -999)
                    {
                        if (i2 == 0)
                        {
                            entityPlayer4.DropPlayerItem(inventoryPlayer6.GetItem());
                            inventoryPlayer6.SetItem((ItemInstance)null);
                        }

                        if (i2 == 1)
                        {
                            entityPlayer4.DropPlayerItem(inventoryPlayer6.GetItem().SplitStack(1));
                            if (inventoryPlayer6.GetItem().stackSize == 0)
                            {
                                inventoryPlayer6.SetItem((ItemInstance)null);
                            }
                        }
                    }
                }
                else
                {
                    int i10;
                    if (z3)
                    {
                        ItemInstance itemStack7 = this.GetStackInSlot(i1);
                        if (itemStack7 != null)
                        {
                            int i8 = itemStack7.stackSize;
                            itemStack5 = itemStack7.Copy();
                            Slot slot9 = this.slots[i1];
                            if (slot9 != null && slot9.GetItem() != null)
                            {
                                i10 = slot9.GetItem().stackSize;
                                if (i10 < i8)
                                {
                                    this.Func_27085(i1, i2, z3, entityPlayer4);
                                }
                            }
                        }
                    }
                    else
                    {
                        Slot slot12 = this.slots[i1];
                        if (slot12 != null)
                        {
                            slot12.OnSlotChanged();
                            ItemInstance itemStack13 = slot12.GetItem();
                            ItemInstance itemStack14 = inventoryPlayer6.GetItem();
                            if (itemStack13 != null)
                            {
                                itemStack5 = itemStack13.Copy();
                            }

                            if (itemStack13 == null)
                            {
                                if (itemStack14 != null && slot12.IsItemValid(itemStack14))
                                {
                                    i10 = i2 == 0 ? itemStack14.stackSize : 1;
                                    if (i10 > slot12.GetSlotStackLimit())
                                    {
                                        i10 = slot12.GetSlotStackLimit();
                                    }

                                    slot12.PutItem(itemStack14.SplitStack(i10));
                                    if (itemStack14.stackSize == 0)
                                    {
                                        inventoryPlayer6.SetItem((ItemInstance)null);
                                    }
                                }
                            }
                            else if (itemStack14 == null)
                            {
                                i10 = i2 == 0 ? itemStack13.stackSize : (itemStack13.stackSize + 1) / 2;
                                ItemInstance itemStack11 = slot12.DecrStackSize(i10);
                                inventoryPlayer6.SetItem(itemStack11);
                                if (itemStack13.stackSize == 0)
                                {
                                    slot12.PutItem((ItemInstance)null);
                                }

                                slot12.OnPickupFromSlot(inventoryPlayer6.GetItem());
                            }
                            else if (slot12.IsItemValid(itemStack14))
                            {
                                if (itemStack13.itemID == itemStack14.itemID && (!itemStack13.GetHasSubtypes() || itemStack13.GetItemDamage() == itemStack14.GetItemDamage()))
                                {
                                    i10 = i2 == 0 ? itemStack14.stackSize : 1;
                                    if (i10 > slot12.GetSlotStackLimit() - itemStack13.stackSize)
                                    {
                                        i10 = slot12.GetSlotStackLimit() - itemStack13.stackSize;
                                    }

                                    if (i10 > itemStack14.GetMaxStackSize() - itemStack13.stackSize)
                                    {
                                        i10 = itemStack14.GetMaxStackSize() - itemStack13.stackSize;
                                    }

                                    itemStack14.SplitStack(i10);
                                    if (itemStack14.stackSize == 0)
                                    {
                                        inventoryPlayer6.SetItem((ItemInstance)null);
                                    }

                                    itemStack13.stackSize += i10;
                                }
                                else if (itemStack14.stackSize <= slot12.GetSlotStackLimit())
                                {
                                    slot12.PutItem(itemStack14);
                                    inventoryPlayer6.SetItem(itemStack13);
                                }
                            }
                            else if (itemStack13.itemID == itemStack14.itemID && itemStack14.GetMaxStackSize() > 1 && (!itemStack13.GetHasSubtypes() || itemStack13.GetItemDamage() == itemStack14.GetItemDamage()))
                            {
                                i10 = itemStack13.stackSize;
                                if (i10 > 0 && i10 + itemStack14.stackSize <= itemStack14.GetMaxStackSize())
                                {
                                    itemStack14.stackSize += i10;
                                    itemStack13.SplitStack(i10);
                                    if (itemStack13.stackSize == 0)
                                    {
                                        slot12.PutItem((ItemInstance)null);
                                    }

                                    slot12.OnPickupFromSlot(inventoryPlayer6.GetItem());
                                }
                            }
                        }
                    }
                }
            }

            return itemStack5;
        }

        public virtual void Removed(Player entityPlayer1)
        {
            Entities.Players.Inventory inventoryPlayer2 = entityPlayer1.inventory;
            if (inventoryPlayer2.GetItem() != null)
            {
                entityPlayer1.DropPlayerItem(inventoryPlayer2.GetItem());
                inventoryPlayer2.SetItem((ItemInstance)null);
            }
        }

        public virtual void OnCraftMatrixChanged(IContainer iInventory1)
        {
            this.UpdateCraftingResults();
        }

        public virtual void PutStackInSlot(int i1, ItemInstance itemStack2)
        {
            this.GetSlot(i1).PutItem(itemStack2);
        }

        public virtual void PutStacksInSlots(ItemInstance[] itemStack1)
        {
            for (int i2 = 0; i2 < itemStack1.Length; ++i2)
            {
                this.GetSlot(i2).PutItem(itemStack1[i2]);
            }
        }

        public virtual bool GetCanCraft(Player entityPlayer1)
        {
            return !this.players.Contains(entityPlayer1);
        }

        public virtual void SetCanCraft(Player entityPlayer1, bool z2)
        {
            if (z2)
            {
                this.players.Remove(entityPlayer1);
            }
            else
            {
                this.players.Add(entityPlayer1);
            }
        }

        public virtual void Func_20112(int i1, int i2)
        {
        }

        public virtual short Func_20111(Entities.Players.Inventory inventoryPlayer1)
        {
            ++this.shortWindowId;
            return this.shortWindowId;
        }

        public virtual void Func_20113(short s1)
        {
        }

        public virtual void Func_20110(short s1)
        {
        }

        public abstract bool StillValid(Player entityPlayer1);
        protected virtual void Func_28126(ItemInstance itemStack1, int i2, int i3, bool z4)
        {
            int i5 = i2;
            if (z4)
            {
                i5 = i3 - 1;
            }

            Slot slot6;
            ItemInstance itemStack7;
            if (itemStack1.IsStackable())
            {
                while (itemStack1.stackSize > 0 && (!z4 && i5 < i3 || z4 && i5 >= i2))
                {
                    slot6 = this.slots[i5];
                    itemStack7 = slot6.GetItem();
                    if (itemStack7 != null && itemStack7.itemID == itemStack1.itemID && (!itemStack1.GetHasSubtypes() || itemStack1.GetItemDamage() == itemStack7.GetItemDamage()))
                    {
                        int i8 = itemStack7.stackSize + itemStack1.stackSize;
                        if (i8 <= itemStack1.GetMaxStackSize())
                        {
                            itemStack1.stackSize = 0;
                            itemStack7.stackSize = i8;
                            slot6.OnSlotChanged();
                        }
                        else if (itemStack7.stackSize < itemStack1.GetMaxStackSize())
                        {
                            itemStack1.stackSize -= itemStack1.GetMaxStackSize() - itemStack7.stackSize;
                            itemStack7.stackSize = itemStack1.GetMaxStackSize();
                            slot6.OnSlotChanged();
                        }
                    }

                    if (z4)
                    {
                        --i5;
                    }
                    else
                    {
                        ++i5;
                    }
                }
            }

            if (itemStack1.stackSize > 0)
            {
                if (z4)
                {
                    i5 = i3 - 1;
                }
                else
                {
                    i5 = i2;
                }

                while (!z4 && i5 < i3 || z4 && i5 >= i2)
                {
                    slot6 = this.slots[i5];
                    itemStack7 = slot6.GetItem();
                    if (itemStack7 == null)
                    {
                        slot6.PutItem(itemStack1.Copy());
                        slot6.OnSlotChanged();
                        itemStack1.stackSize = 0;
                        break;
                    }

                    if (z4)
                    {
                        --i5;
                    }
                    else
                    {
                        ++i5;
                    }
                }
            }
        }
    }
}