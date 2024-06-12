using SharpCraft.Core.NBT;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Items.Crafting;

namespace SharpCraft.Core.World.GameLevel.Tiles.TileEntities
{
    public class TileEntityFurnace : TileEntity, IContainer
    {
        private ItemInstance[] furnaceItemStacks = new ItemInstance[3];
        public int furnaceBurnTime = 0;
        public int currentItemBurnTime = 0;
        public int furnaceCookTime = 0;
        public int GetContainerSize()
        {
            return this.furnaceItemStacks.Length;
        }

        public ItemInstance GetItem(int i1)
        {
            return this.furnaceItemStacks[i1];
        }

        public ItemInstance RemoveItem(int i1, int i2)
        {
            if (this.furnaceItemStacks[i1] != null)
            {
                ItemInstance itemStack3;
                if (this.furnaceItemStacks[i1].stackSize <= i2)
                {
                    itemStack3 = this.furnaceItemStacks[i1];
                    this.furnaceItemStacks[i1] = null;
                    return itemStack3;
                }
                else
                {
                    itemStack3 = this.furnaceItemStacks[i1].SplitStack(i2);
                    if (this.furnaceItemStacks[i1].stackSize == 0)
                    {
                        this.furnaceItemStacks[i1] = null;
                    }

                    return itemStack3;
                }
            }
            else
            {
                return null;
            }
        }

        public void SetItem(int i1, ItemInstance itemStack2)
        {
            this.furnaceItemStacks[i1] = itemStack2;
            if (itemStack2 != null && itemStack2.stackSize > this.GetMaxStackSize())
            {
                itemStack2.stackSize = this.GetMaxStackSize();
            }
        }

        public string GetName()
        {
            return "Furnace";
        }

        public override void Load(CompoundTag nBTTagCompound1)
        {
            base.Load(nBTTagCompound1);
            ListTag<CompoundTag> nBTTagList2 = nBTTagCompound1.GetTagList<CompoundTag>("Items");
            this.furnaceItemStacks = new ItemInstance[this.GetContainerSize()];
            for (int i3 = 0; i3 < nBTTagList2.Count; ++i3)
            {
                CompoundTag nBTTagCompound4 = nBTTagList2[i3];
                byte b5 = nBTTagCompound4.GetByte("Slot");
                if (b5 >= 0 && b5 < this.furnaceItemStacks.Length)
                {
                    this.furnaceItemStacks[b5] = new ItemInstance(nBTTagCompound4);
                }
            }

            this.furnaceBurnTime = nBTTagCompound1.GetShort("BurnTime");
            this.furnaceCookTime = nBTTagCompound1.GetShort("CookTime");
            this.currentItemBurnTime = this.GetItemBurnTime(this.furnaceItemStacks[1]);
        }

        public override void Save(CompoundTag nBTTagCompound1)
        {
            base.Save(nBTTagCompound1);
            nBTTagCompound1.SetShort("BurnTime", (short)this.furnaceBurnTime);
            nBTTagCompound1.SetShort("CookTime", (short)this.furnaceCookTime);
            ListTag<CompoundTag> nBTTagList2 = new ListTag<CompoundTag>();
            for (int i3 = 0; i3 < this.furnaceItemStacks.Length; ++i3)
            {
                if (this.furnaceItemStacks[i3] != null)
                {
                    CompoundTag nBTTagCompound4 = new CompoundTag();
                    nBTTagCompound4.SetByte("Slot", (byte)i3);
                    this.furnaceItemStacks[i3].WriteToNBT(nBTTagCompound4);
                    nBTTagList2.Add(nBTTagCompound4);
                }
            }

            nBTTagCompound1.SetTag("Items", nBTTagList2);
        }

        public int GetMaxStackSize()
        {
            return 64;
        }

        public virtual int GetCookProgressScaled(int i1)
        {
            return this.furnaceCookTime * i1 / 200;
        }

        public virtual int GetBurnTimeRemainingScaled(int i1)
        {
            if (this.currentItemBurnTime == 0)
            {
                this.currentItemBurnTime = 200;
            }

            return this.furnaceBurnTime * i1 / this.currentItemBurnTime;
        }

        public virtual bool IsBurning()
        {
            return this.furnaceBurnTime > 0;
        }

        public override void UpdateEntity()
        {
            bool z1 = this.furnaceBurnTime > 0;
            bool z2 = false;
            if (this.furnaceBurnTime > 0)
            {
                --this.furnaceBurnTime;
            }

            if (!this.worldObj.isRemote)
            {
                if (this.furnaceBurnTime == 0 && this.CanSmelt())
                {
                    this.currentItemBurnTime = this.furnaceBurnTime = this.GetItemBurnTime(this.furnaceItemStacks[1]);
                    if (this.furnaceBurnTime > 0)
                    {
                        z2 = true;
                        if (this.furnaceItemStacks[1] != null)
                        {
                            --this.furnaceItemStacks[1].stackSize;
                            if (this.furnaceItemStacks[1].stackSize == 0)
                            {
                                this.furnaceItemStacks[1] = null;
                            }
                        }
                    }
                }

                if (this.IsBurning() && this.CanSmelt())
                {
                    ++this.furnaceCookTime;
                    if (this.furnaceCookTime == 200)
                    {
                        this.furnaceCookTime = 0;
                        this.SmeltItem();
                        z2 = true;
                    }
                }
                else
                {
                    this.furnaceCookTime = 0;
                }

                if (z1 != this.furnaceBurnTime > 0)
                {
                    z2 = true;
                    FurnaceTile.UpdateFurnaceBlockState(this.furnaceBurnTime > 0, this.worldObj, this.xCoord, this.yCoord, this.zCoord);
                }
            }

            if (z2)
            {
                this.SetChanged();
            }
        }

        private bool CanSmelt()
        {
            if (this.furnaceItemStacks[0] == null)
            {
                return false;
            }
            else
            {
                ItemInstance itemStack1 = FurnaceRecipes.Smelting().GetSmeltingResult(this.furnaceItemStacks[0].GetItem().id);
                return itemStack1 == null ? false : (this.furnaceItemStacks[2] == null ? true : (!this.furnaceItemStacks[2].IsItemEqual(itemStack1) ? false : (this.furnaceItemStacks[2].stackSize < this.GetMaxStackSize() && this.furnaceItemStacks[2].stackSize < this.furnaceItemStacks[2].GetMaxStackSize() ? true : this.furnaceItemStacks[2].stackSize < itemStack1.GetMaxStackSize())));
            }
        }

        public virtual void SmeltItem()
        {
            if (this.CanSmelt())
            {
                ItemInstance itemStack1 = FurnaceRecipes.Smelting().GetSmeltingResult(this.furnaceItemStacks[0].GetItem().id);
                if (this.furnaceItemStacks[2] == null)
                {
                    this.furnaceItemStacks[2] = itemStack1.Copy();
                }
                else if (this.furnaceItemStacks[2].itemID == itemStack1.itemID)
                {
                    ++this.furnaceItemStacks[2].stackSize;
                }

                --this.furnaceItemStacks[0].stackSize;
                if (this.furnaceItemStacks[0].stackSize <= 0)
                {
                    this.furnaceItemStacks[0] = null;
                }
            }
        }

        private int GetItemBurnTime(ItemInstance itemStack1)
        {
            if (itemStack1 == null)
            {
                return 0;
            }
            else
            {
                int i2 = itemStack1.GetItem().id;
                return i2 < 256 && Tile.tiles[i2].material == Material.wood ? 300 : (i2 == Item.stick.id ? 100 : (i2 == Item.coal.id ? 1600 : (i2 == Item.bucketLava.id ? 20000 : (i2 == Tile.sapling.id ? 100 : 0))));
            }
        }

        public bool StillValid(Player entityPlayer1)
        {
            return this.worldObj.GetTileEntity(this.xCoord, this.yCoord, this.zCoord) != this ? false : entityPlayer1.GetDistanceSq(this.xCoord + 0.5, this.yCoord + 0.5, this.zCoord + 0.5) <= 64;
        }
    }
}