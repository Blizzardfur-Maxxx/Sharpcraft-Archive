using SharpCraft.Core.World.Items;
using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet102WindowClick : Packet
    {
        public int window_Id;
        public int inventorySlot;
        public int mouseClick;
        public short action;
        public ItemInstance itemStack;
        public bool field;

        public Packet102WindowClick()
        {
        }

        public Packet102WindowClick(int i1, int i2, int i3, bool z4, ItemInstance itemStack5, short s6)
        {
            this.window_Id = i1;
            this.inventorySlot = i2;
            this.mouseClick = i3;
            this.itemStack = itemStack5;
            this.action = s6;
            this.field = z4;
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleWindowClick(this);
        }

        public override void Read(BinaryReader reader)
        {
            this.window_Id = reader.ReadByte();
            this.inventorySlot = reader.ReadBEShort();
            this.mouseClick = reader.ReadByte();
            this.action = reader.ReadBEShort();
            this.field = reader.ReadBoolean();
            short s2 = reader.ReadBEShort();

            if (s2 >= 0)
            {
                byte b3 = reader.ReadByte();
                short s4 = reader.ReadBEShort();
                this.itemStack = new ItemInstance(s2, b3, s4);
            }
            else
            {
                this.itemStack = null;
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.window_Id);
            writer.WriteBEShort((short)this.inventorySlot);
            writer.Write((byte)this.mouseClick);
            writer.WriteBEShort(this.action);
            writer.Write(this.field);

            if (this.itemStack == null)
            {
                writer.WriteBEShort(-1);
            }
            else
            {
                writer.WriteBEShort((short)this.itemStack.itemID);
                writer.Write((byte)this.itemStack.stackSize);
                writer.WriteBEShort((short)this.itemStack.GetItemDamage());
            }
        }

        public override int Size()
        {
            return 11;
        }
    }
}