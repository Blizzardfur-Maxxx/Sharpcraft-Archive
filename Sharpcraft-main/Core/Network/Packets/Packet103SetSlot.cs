using SharpCraft.Core.World.Items;
using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet103SetSlot : Packet
    {
        public int windowId;
        public int itemSlot;
        public ItemInstance myItemStack;
        public Packet103SetSlot()
        {
        }

        public Packet103SetSlot(int i1, int i2, ItemInstance itemStack3)
        {
            this.windowId = i1;
            this.itemSlot = i2;
            this.myItemStack = itemStack3 == null ? itemStack3 : itemStack3.Copy();
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleSetSlot(this);
        }

        public override void Read(BinaryReader reader)
        {
            this.windowId = reader.ReadByte();
            this.itemSlot = reader.ReadBEShort();
            short s2 = reader.ReadBEShort();
            if (s2 >= 0)
            {
                byte b3 = reader.ReadByte();
                short s4 = reader.ReadBEShort();
                this.myItemStack = new ItemInstance(s2, b3, s4);
            }
            else
            {
                this.myItemStack = null;
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.windowId);
            writer.WriteBEShort((short)this.itemSlot);
            if (this.myItemStack == null)
            {
                writer.WriteBEShort(-1);
            }
            else
            {
                writer.WriteBEShort((short)this.myItemStack.itemID);
                writer.Write((byte)this.myItemStack.stackSize);
                writer.WriteBEShort((short)this.myItemStack.GetItemDamage());
            }
        }

        public override int Size()
        {
            return 8;
        }
    }
}