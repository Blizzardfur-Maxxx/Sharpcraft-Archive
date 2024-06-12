using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.GameLevel.Tiles.TileEntities
{
    public class TileEntityDispenser : TileEntity, IContainer
    {
        private ItemInstance[] dispenserContents = new ItemInstance[9];
        private JRandom dispenserRandom = new JRandom();
        public int GetContainerSize()
        {
            return 9;
        }

        public ItemInstance GetItem(int i1)
        {
            return this.dispenserContents[i1];
        }

        public ItemInstance RemoveItem(int i1, int i2)
        {
            if (this.dispenserContents[i1] != null)
            {
                ItemInstance itemStack3;
                if (this.dispenserContents[i1].stackSize <= i2)
                {
                    itemStack3 = this.dispenserContents[i1];
                    this.dispenserContents[i1] = null;
                    this.SetChanged();
                    return itemStack3;
                }
                else
                {
                    itemStack3 = this.dispenserContents[i1].SplitStack(i2);
                    if (this.dispenserContents[i1].stackSize == 0)
                    {
                        this.dispenserContents[i1] = null;
                    }

                    this.SetChanged();
                    return itemStack3;
                }
            }
            else
            {
                return null;
            }
        }

        public virtual ItemInstance GetRandomStackFromInventory()
        {
            int i1 = -1;
            int i2 = 1;
            for (int i3 = 0; i3 < this.dispenserContents.Length; ++i3)
            {
                if (this.dispenserContents[i3] != null && this.dispenserRandom.NextInt(i2++) == 0)
                {
                    i1 = i3;
                }
            }

            if (i1 >= 0)
            {
                return this.RemoveItem(i1, 1);
            }
            else
            {
                return null;
            }
        }

        public void SetItem(int i1, ItemInstance itemStack2)
        {
            this.dispenserContents[i1] = itemStack2;
            if (itemStack2 != null && itemStack2.stackSize > this.GetMaxStackSize())
            {
                itemStack2.stackSize = this.GetMaxStackSize();
            }

            this.SetChanged();
        }

        public string GetName()
        {
            return "Trap";
        }

        public override void Load(CompoundTag nBTTagCompound1)
        {
            base.Load(nBTTagCompound1);
            ListTag<CompoundTag> nBTTagList2 = nBTTagCompound1.GetTagList<CompoundTag>("Items");
            this.dispenserContents = new ItemInstance[this.GetContainerSize()];
            for (int i3 = 0; i3 < nBTTagList2.Count; ++i3)
            {
                CompoundTag nBTTagCompound4 = nBTTagList2[i3];
                int i5 = nBTTagCompound4.GetByte("Slot") & 255;
                if (i5 >= 0 && i5 < this.dispenserContents.Length)
                {
                    this.dispenserContents[i5] = new ItemInstance(nBTTagCompound4);
                }
            }
        }

        public override void Save(CompoundTag nBTTagCompound1)
        {
            base.Save(nBTTagCompound1);
            ListTag<CompoundTag> nBTTagList2 = new ListTag<CompoundTag>();
            for (int i3 = 0; i3 < this.dispenserContents.Length; ++i3)
            {
                if (this.dispenserContents[i3] != null)
                {
                    CompoundTag nBTTagCompound4 = new CompoundTag();
                    nBTTagCompound4.SetByte("Slot", (byte)i3);
                    this.dispenserContents[i3].WriteToNBT(nBTTagCompound4);
                    nBTTagList2.Add(nBTTagCompound4);
                }
            }

            nBTTagCompound1.SetTag("Items", nBTTagList2);
        }

        public int GetMaxStackSize()
        {
            return 64;
        }

        public bool StillValid(Player entityPlayer1)
        {
            return this.worldObj.GetTileEntity(this.xCoord, this.yCoord, this.zCoord) != this ? false : entityPlayer1.GetDistanceSq(this.xCoord + 0.5, this.yCoord + 0.5, this.zCoord + 0.5) <= 64;
        }
    }
}