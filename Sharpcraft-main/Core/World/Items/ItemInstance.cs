using SharpCraft.Core.NBT;
using SharpCraft.Core.Stats;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public sealed class ItemInstance
    {
        public int stackSize;
        public int animationsToGo;
        public int itemID;
        private int itemDamage;
        public ItemInstance(Tile block1) : this(block1, 1)
        {
        }

        public ItemInstance(Tile block1, int i2) : this(block1.id, i2, 0)
        {
        }

        public ItemInstance(Tile block1, int i2, int i3) : this(block1.id, i2, i3)
        {
        }

        public ItemInstance(Item item1) : this(item1.id, 1, 0)
        {
        }

        public ItemInstance(Item item1, int i2) : this(item1.id, i2, 0)
        {
        }

        public ItemInstance(Item item1, int i2, int i3) : this(item1.id, i2, i3)
        {
        }

        public ItemInstance(int i1, int i2, int i3)
        {
            this.stackSize = 0;
            this.itemID = i1;
            this.stackSize = i2;
            this.itemDamage = i3;
        }

        public ItemInstance(CompoundTag nBTTagCompound1)
        {
            this.stackSize = 0;
            this.ReadFromNBT(nBTTagCompound1);
        }

        public ItemInstance SplitStack(int i1)
        {
            this.stackSize -= i1;
            return new ItemInstance(this.itemID, i1, this.itemDamage);
        }

        public Item GetItem()
        {
            return Item.items[this.itemID];
        }

        public int GetIconIndex()
        {
            return this.GetItem().GetIconIndex(this);
        }

        public bool UseItem(Player entityPlayer1, Level world2, int i3, int i4, int i5, TileFace i6)
        {
            bool z7 = this.GetItem().OnItemUse(this, entityPlayer1, world2, i3, i4, i5, i6);
            if (z7)
            {
                entityPlayer1.AddStat(StatList.itemUseStats[this.itemID], 1);
            }

            return z7;
        }

        public float GetStrVsBlock(Tile block1)
        {
            return this.GetItem().GetStrVsBlock(this, block1);
        }

        public ItemInstance UseItemRightClick(Level world1, Player entityPlayer2)
        {
            return this.GetItem().OnItemRightClick(this, world1, entityPlayer2);
        }

        public CompoundTag WriteToNBT(CompoundTag nBTTagCompound1)
        {
            nBTTagCompound1.SetShort("id", (short)this.itemID);
            nBTTagCompound1.SetByte("Count", (byte)this.stackSize);
            nBTTagCompound1.SetShort("Damage", (short)this.itemDamage);
            return nBTTagCompound1;
        }

        public void ReadFromNBT(CompoundTag nBTTagCompound1)
        {
            this.itemID = nBTTagCompound1.GetShort("id");
            this.stackSize = nBTTagCompound1.GetByte("Count");
            this.itemDamage = nBTTagCompound1.GetShort("Damage");
        }

        public int GetMaxStackSize()
        {
            return this.GetItem().GetItemStackLimit();
        }

        public bool IsStackable()
        {
            return this.GetMaxStackSize() > 1 && (!this.IsItemStackDamageable() || !this.IsItemDamaged());
        }

        public bool IsItemStackDamageable()
        {
            return Item.items[this.itemID].GetMaxDamage() > 0;
        }

        public bool GetHasSubtypes()
        {
            return Item.items[this.itemID].GetHasSubtypes();
        }

        public bool IsItemDamaged()
        {
            return this.IsItemStackDamageable() && this.itemDamage > 0;
        }

        public int GetItemDamageForDisplay()
        {
            return this.itemDamage;
        }

        public int GetItemDamage()
        {
            return this.itemDamage;
        }

        public void SetItemDamage(int i1)
        {
            this.itemDamage = i1;
        }

        public int GetMaxDamage()
        {
            return Item.items[this.itemID].GetMaxDamage();
        }

        public void DamageItem(int i1, Entity entity2)
        {
            if (this.IsItemStackDamageable())
            {
                this.itemDamage += i1;
                if (this.itemDamage > this.GetMaxDamage())
                {
                    if (entity2 is Player)
                    {
                        ((Player)entity2).AddStat(StatList.itemDamageStats[this.itemID], 1);
                    }

                    --this.stackSize;
                    if (this.stackSize < 0)
                    {
                        this.stackSize = 0;
                    }

                    this.itemDamage = 0;
                }
            }
        }

        public void HitEntity(Mob entityLiving1, Player entityPlayer2)
        {
            bool z3 = Item.items[this.itemID].HitEntity(this, entityLiving1, entityPlayer2);
            if (z3)
            {
                entityPlayer2.AddStat(StatList.itemUseStats[this.itemID], 1);
            }
        }

        public void OnDestroyBlock(int i1, int i2, int i3, int i4, Player entityPlayer5)
        {
            bool z6 = Item.items[this.itemID].OnBlockDestroyed(this, i1, i2, i3, i4, entityPlayer5);
            if (z6)
            {
                entityPlayer5.AddStat(StatList.itemUseStats[this.itemID], 1);
            }
        }

        public int GetDamageVsEntity(Entity entity1)
        {
            return Item.items[this.itemID].GetDamageVsEntity(entity1);
        }

        public bool CanHarvestBlock(Tile block1)
        {
            return Item.items[this.itemID].CanHarvestBlock(block1);
        }

        public void Func_1097(Player entityPlayer1)
        {
        }

        public void UseItemOnEntity(Mob entityLiving1)
        {
            Item.items[this.itemID].SaddleEntity(this, entityLiving1);
        }

        public ItemInstance Copy()
        {
            return new ItemInstance(this.itemID, this.stackSize, this.itemDamage);
        }

        public static bool AreItemStacksEqual(ItemInstance itemStack0, ItemInstance itemStack1)
        {
            return itemStack0 == null && itemStack1 == null ? true : (itemStack0 != null && itemStack1 != null ? itemStack0.IsItemStackEqual(itemStack1) : false);
        }

        private bool IsItemStackEqual(ItemInstance itemStack1)
        {
            return this.stackSize != itemStack1.stackSize ? false : (this.itemID != itemStack1.itemID ? false : this.itemDamage == itemStack1.itemDamage);
        }

        public bool IsItemEqual(ItemInstance itemStack1)
        {
            return this.itemID == itemStack1.itemID && this.itemDamage == itemStack1.itemDamage;
        }

        public string GetItemName()
        {
            return Item.items[this.itemID].GetItemNameIS(this);
        }

        public static ItemInstance CopyItemStack(ItemInstance itemStack0)
        {
            return itemStack0 == null ? null : itemStack0.Copy();
        }

        public override string ToString()
        {
            return this.stackSize + "x" + Item.items[this.itemID].GetItemName() + "@" + this.itemDamage;
        }

        public void UpdateAnimation(Level world1, Entity entity2, int i3, bool z4)
        {
            if (this.animationsToGo > 0)
            {
                --this.animationsToGo;
            }

            Item.items[this.itemID].OnUpdate(this, world1, entity2, i3, z4);
        }

        public void OnCrafting(Level world1, Player entityPlayer2)
        {
            entityPlayer2.AddStat(StatList.craftingStats[this.itemID], this.stackSize);
            Item.items[this.itemID].OnCreated(this, world1, entityPlayer2);
        }

        public bool IsStackEqual(ItemInstance itemStack1)
        {
            return this.itemID == itemStack1.itemID && this.stackSize == itemStack1.stackSize && this.itemDamage == itemStack1.itemDamage;
        }
    }
}