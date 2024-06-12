

using SharpCraft.Core.NBT;
using SharpCraft.Core.Network.Packets;
using System;

namespace SharpCraft.Core.World.GameLevel.Tiles.TileEntities
{
    public class TileEntitySign : TileEntity
    {
        public String[] signText = new[]
        {
            "",
            "",
            "",
            ""
        };
        public int lineBeingEdited = -1;
        private bool editable = true;
        public override void Save(CompoundTag nBTTagCompound1)
        {
            base.Save(nBTTagCompound1);
            nBTTagCompound1.SetString("Text1", this.signText[0]);
            nBTTagCompound1.SetString("Text2", this.signText[1]);
            nBTTagCompound1.SetString("Text3", this.signText[2]);
            nBTTagCompound1.SetString("Text4", this.signText[3]);
        }

        public override void Load(CompoundTag nBTTagCompound1)
        {
            this.editable = false;
            base.Load(nBTTagCompound1);
            for (int i2 = 0; i2 < 4; ++i2)
            {
                this.signText[i2] = nBTTagCompound1.GetString("Text" + (i2 + 1));
                if (this.signText[i2].Length > 15)
                {
                    this.signText[i2] = this.signText[i2].Substring(0, 15);
                }
            }
        }

        public override Packet GetDescriptionPacket()
        {
            String[] string1 = new string[4];
            for (int i2 = 0; i2 < 4; ++i2)
            {
                string1[i2] = this.signText[i2];
            }

            return new Packet130UpdateSign(this.xCoord, this.yCoord, this.zCoord, string1);
        }

        public virtual bool IsEditable()
        {
            return this.editable;
        }

        public virtual void SetEditable(bool z1)
        {
            this.editable = z1;
        }
    }
}