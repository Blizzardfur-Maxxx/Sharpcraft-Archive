using SharpCraft.Core.NBT;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Entities.Players
{
    public class Inventory : IContainer
    {
        // 9 * 4
        public ItemInstance[] mainInventory = new ItemInstance[36];
        public ItemInstance[] armorInventory = new ItemInstance[4];
        public int currentItem = 0;
        public Player player;
        private ItemInstance item;
        public bool inventoryChanged = false;
        public Inventory(Player entityPlayer1)
        {
            this.player = entityPlayer1;
        }

        public virtual ItemInstance GetCurrentItem()
        {
            return this.currentItem < 9 && this.currentItem >= 0 ? this.mainInventory[this.currentItem] : null;
        }

        public static int Func_25054_e()
        {
            return 9;
        }

        private int GetInventorySlotContainItem(int i1)
        {
            for (int i2 = 0; i2 < this.mainInventory.Length; ++i2)
            {
                if (this.mainInventory[i2] != null && this.mainInventory[i2].itemID == i1)
                {
                    return i2;
                }
            }

            return -1;
        }

        private int StoreItemStack(ItemInstance itemStack1)
        {
            for (int i2 = 0; i2 < this.mainInventory.Length; ++i2)
            {
                if (this.mainInventory[i2] != null && this.mainInventory[i2].itemID == itemStack1.itemID && this.mainInventory[i2].IsStackable() && this.mainInventory[i2].stackSize < this.mainInventory[i2].GetMaxStackSize() && this.mainInventory[i2].stackSize < this.GetMaxStackSize() && (!this.mainInventory[i2].GetHasSubtypes() || this.mainInventory[i2].GetItemDamage() == itemStack1.GetItemDamage()))
                {
                    return i2;
                }
            }

            return -1;
        }

        private int GetFirstEmptyStack()
        {
            for (int i1 = 0; i1 < this.mainInventory.Length; ++i1)
            {
                if (this.mainInventory[i1] == null)
                {
                    return i1;
                }
            }

            return -1;
        }

        public virtual void SetCurrentItem(int i1, bool z2)
        {
            int i3 = this.GetInventorySlotContainItem(i1);
            if (i3 >= 0 && i3 < 9)
            {
                this.currentItem = i3;
            }
        }

        public virtual void ChangeCurrentItem(int i1)
        {
            if (i1 > 0)
            {
                i1 = 1;
            }

            if (i1 < 0)
            {
                i1 = -1;
            }

            for (this.currentItem -= i1; this.currentItem < 0; this.currentItem += 9)
            {
            }

            while (this.currentItem >= 9)
            {
                this.currentItem -= 9;
            }
        }

        private int StorePartialItemStack(ItemInstance itemStack1)
        {
            int i2 = itemStack1.itemID;
            int i3 = itemStack1.stackSize;
            int i4 = this.StoreItemStack(itemStack1);
            if (i4 < 0)
            {
                i4 = this.GetFirstEmptyStack();
            }

            if (i4 < 0)
            {
                return i3;
            }
            else
            {
                if (this.mainInventory[i4] == null)
                {
                    this.mainInventory[i4] = new ItemInstance(i2, 0, itemStack1.GetItemDamage());
                }

                int i5 = i3;
                if (i3 > this.mainInventory[i4].GetMaxStackSize() - this.mainInventory[i4].stackSize)
                {
                    i5 = this.mainInventory[i4].GetMaxStackSize() - this.mainInventory[i4].stackSize;
                }

                if (i5 > this.GetMaxStackSize() - this.mainInventory[i4].stackSize)
                {
                    i5 = this.GetMaxStackSize() - this.mainInventory[i4].stackSize;
                }

                if (i5 == 0)
                {
                    return i3;
                }
                else
                {
                    i3 -= i5;
                    this.mainInventory[i4].stackSize += i5;
                    this.mainInventory[i4].animationsToGo = 5;
                    return i3;
                }
            }
        }

        public virtual void DecrementAnimations()
        {
            for (int i1 = 0; i1 < this.mainInventory.Length; ++i1)
            {
                if (this.mainInventory[i1] != null)
                {
                    this.mainInventory[i1].UpdateAnimation(this.player.worldObj, this.player, i1, this.currentItem == i1);
                }
            }
        }

        public virtual bool ConsumeInventoryItem(int i1)
        {
            int i2 = this.GetInventorySlotContainItem(i1);
            if (i2 < 0)
            {
                return false;
            }
            else
            {
                if (--this.mainInventory[i2].stackSize <= 0)
                {
                    this.mainInventory[i2] = null;
                }

                return true;
            }
        }

        public virtual bool AddItem(ItemInstance itemStack1)
        {
            int i2;
            if (itemStack1.IsItemDamaged())
            {
                i2 = this.GetFirstEmptyStack();
                if (i2 >= 0)
                {
                    this.mainInventory[i2] = ItemInstance.CopyItemStack(itemStack1);
                    this.mainInventory[i2].animationsToGo = 5;
                    itemStack1.stackSize = 0;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                do
                {
                    i2 = itemStack1.stackSize;
                    itemStack1.stackSize = this.StorePartialItemStack(itemStack1);
                }
                while (itemStack1.stackSize > 0 && itemStack1.stackSize < i2);
                return itemStack1.stackSize < i2;
            }
        }

        public virtual ItemInstance RemoveItem(int i1, int i2)
        {
            ItemInstance[] itemStack3 = this.mainInventory;
            if (i1 >= this.mainInventory.Length)
            {
                itemStack3 = this.armorInventory;
                i1 -= this.mainInventory.Length;
            }

            if (itemStack3[i1] != null)
            {
                ItemInstance itemStack4;
                if (itemStack3[i1].stackSize <= i2)
                {
                    itemStack4 = itemStack3[i1];
                    itemStack3[i1] = null;
                    return itemStack4;
                }
                else
                {
                    itemStack4 = itemStack3[i1].SplitStack(i2);
                    if (itemStack3[i1].stackSize == 0)
                    {
                        itemStack3[i1] = null;
                    }

                    return itemStack4;
                }
            }
            else
            {
                return null;
            }
        }

        public virtual void SetItem(int i1, ItemInstance itemStack2)
        {
            ItemInstance[] itemStack3 = this.mainInventory;
            if (i1 >= itemStack3.Length)
            {
                i1 -= itemStack3.Length;
                itemStack3 = this.armorInventory;
            }

            itemStack3[i1] = itemStack2;
        }

        public virtual float GetStrVsBlock(Tile block1)
        {
            float f2 = 1F;
            if (this.mainInventory[this.currentItem] != null)
            {
                f2 *= this.mainInventory[this.currentItem].GetStrVsBlock(block1);
            }

            return f2;
        }

        public virtual ListTag<CompoundTag> WriteToNBT(ListTag<CompoundTag> nBTTagList1)
        {
            int i2;
            CompoundTag nBTTagCompound3;
            for (i2 = 0; i2 < this.mainInventory.Length; ++i2)
            {
                if (this.mainInventory[i2] != null)
                {
                    nBTTagCompound3 = new CompoundTag();
                    nBTTagCompound3.SetByte("Slot", (byte)i2);
                    this.mainInventory[i2].WriteToNBT(nBTTagCompound3);
                    nBTTagList1.Add(nBTTagCompound3);
                }
            }

            for (i2 = 0; i2 < this.armorInventory.Length; ++i2)
            {
                if (this.armorInventory[i2] != null)
                {
                    nBTTagCompound3 = new CompoundTag();
                    nBTTagCompound3.SetByte("Slot", (byte)(i2 + 100));
                    this.armorInventory[i2].WriteToNBT(nBTTagCompound3);
                    nBTTagList1.Add(nBTTagCompound3);
                }
            }

            return nBTTagList1;
        }

        public virtual void ReadFromNBT(ListTag<CompoundTag> nBTTagList1)
        {
            this.mainInventory = new ItemInstance[36];
            this.armorInventory = new ItemInstance[4];
            for (int i2 = 0; i2 < nBTTagList1.Count; ++i2)
            {
                CompoundTag nBTTagCompound3 = nBTTagList1[i2];
                int i4 = nBTTagCompound3.GetByte("Slot") & 255;
                ItemInstance itemStack5 = new ItemInstance(nBTTagCompound3);
                if (itemStack5.GetItem() != null)
                {
                    if (i4 >= 0 && i4 < this.mainInventory.Length)
                    {
                        this.mainInventory[i4] = itemStack5;
                    }

                    if (i4 >= 100 && i4 < this.armorInventory.Length + 100)
                    {
                        this.armorInventory[i4 - 100] = itemStack5;
                    }
                }
            }
        }

        public virtual int GetContainerSize()
        {
            return this.mainInventory.Length + 4;
        }

        public virtual ItemInstance GetItem(int i1)
        {
            ItemInstance[] itemStack2 = this.mainInventory;
            if (i1 >= itemStack2.Length)
            {
                i1 -= itemStack2.Length;
                itemStack2 = this.armorInventory;
            }

            return itemStack2[i1];
        }

        public virtual string GetName()
        {
            return "Inventory";
        }

        public virtual int GetMaxStackSize()
        {
            return 64;
        }

        public virtual int GetDamageVsEntity(Entity entity1)
        {
            ItemInstance itemStack2 = this.GetItem(this.currentItem);
            return itemStack2 != null ? itemStack2.GetDamageVsEntity(entity1) : 1;
        }

        public virtual bool CanHarvestBlock(Tile block1)
        {
            if (block1.material.GetIsHarvestable())
            {
                return true;
            }
            else
            {
                ItemInstance itemStack2 = this.GetItem(this.currentItem);
                return itemStack2 != null ? itemStack2.CanHarvestBlock(block1) : false;
            }
        }

        public virtual ItemInstance ArmorItemInSlot(int i1)
        {
            return this.armorInventory[i1];
        }

        public virtual int GetTotalArmorValue()
        {
            int i1 = 0;
            int i2 = 0;
            int i3 = 0;
            for (int i4 = 0; i4 < this.armorInventory.Length; ++i4)
            {
                if (this.armorInventory[i4] != null && this.armorInventory[i4].GetItem() is ItemArmor)
                {
                    int i5 = this.armorInventory[i4].GetMaxDamage();
                    int i6 = this.armorInventory[i4].GetItemDamageForDisplay();
                    int i7 = i5 - i6;
                    i2 += i7;
                    i3 += i5;
                    int i8 = ((ItemArmor)this.armorInventory[i4].GetItem()).damageReduceAmount;
                    i1 += i8;
                }
            }

            if (i3 == 0)
            {
                return 0;
            }
            else
            {
                return (i1 - 1) * i2 / i3 + 1;
            }
        }

        public virtual void DamageArmor(int i1)
        {
            for (int i2 = 0; i2 < this.armorInventory.Length; ++i2)
            {
                if (this.armorInventory[i2] != null && this.armorInventory[i2].GetItem() is ItemArmor)
                {
                    this.armorInventory[i2].DamageItem(i1, this.player);
                    if (this.armorInventory[i2].stackSize == 0)
                    {
                        this.armorInventory[i2].Func_1097(this.player);
                        this.armorInventory[i2] = null;
                    }
                }
            }
        }

        public virtual void DropAllItems()
        {
            int i1;
            for (i1 = 0; i1 < this.mainInventory.Length; ++i1)
            {
                if (this.mainInventory[i1] != null)
                {
                    this.player.DropPlayerItemWithRandomChoice(this.mainInventory[i1], true);
                    this.mainInventory[i1] = null;
                }
            }

            for (i1 = 0; i1 < this.armorInventory.Length; ++i1)
            {
                if (this.armorInventory[i1] != null)
                {
                    this.player.DropPlayerItemWithRandomChoice(this.armorInventory[i1], true);
                    this.armorInventory[i1] = null;
                }
            }
        }

        public virtual void SetChanged()
        {
            this.inventoryChanged = true;
        }

        public virtual void SetItem(ItemInstance itemStack1)
        {
            this.item = itemStack1;
            this.player.OnItemStackChanged(itemStack1);
        }

        public virtual ItemInstance GetItem()
        {
            return this.item;
        }

        public virtual bool StillValid(Player entityPlayer1)
        {
            return this.player.isDead ? false : entityPlayer1.GetDistanceSqToEntity(this.player) <= 64;
        }

        public virtual bool ContainsEqual(ItemInstance itemStack1)
        {
            int i2;
            for (i2 = 0; i2 < this.armorInventory.Length; ++i2)
            {
                if (this.armorInventory[i2] != null && this.armorInventory[i2].IsStackEqual(itemStack1))
                {
                    return true;
                }
            }

            for (i2 = 0; i2 < this.mainInventory.Length; ++i2)
            {
                if (this.mainInventory[i2] != null && this.mainInventory[i2].IsStackEqual(itemStack1))
                {
                    return true;
                }
            }

            return false;
        }
    }
}