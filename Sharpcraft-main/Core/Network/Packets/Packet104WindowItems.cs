using SharpCraft.Core.World.Items;
using System.Collections.Generic;
using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet104WindowItems : Packet
    {
        public int windowId;
        public ItemInstance[] items;
        public Packet104WindowItems()
        {
        }

        public Packet104WindowItems(int i1, IList<ItemInstance> list2)
        {
            this.windowId = i1;
            this.items = new ItemInstance[list2.Count];
            for (int i3 = 0; i3 < this.items.Length; ++i3)
            {
                ItemInstance itemStack4 = list2[i3];
                this.items[i3] = itemStack4 == null ? null : itemStack4.Copy();
            }
        }

        public override void Read(BinaryReader reader)
        {
            this.windowId = reader.ReadByte();
            short s2 = reader.ReadBEShort();
            this.items = new ItemInstance[s2];
            for (int i3 = 0; i3 < s2; ++i3)
            {
                short s4 = reader.ReadBEShort();
                if (s4 >= 0)
                {
                    byte b5 = reader.ReadByte();
                    short s6 = reader.ReadBEShort();
                    this.items[i3] = new ItemInstance(s4, b5, s6);
                }
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.windowId);
            writer.WriteBEShort((short)this.items.Length);
            for (int i2 = 0; i2 < this.items.Length; ++i2)
            {
                if (this.items[i2] == null)
                {
                    writer.WriteBEShort(-1);
                }
                else
                {
                    writer.WriteBEShort((short)this.items[i2].itemID);
                    writer.Write((byte)this.items[i2].stackSize);
                    writer.WriteBEShort((short)this.items[i2].GetItemDamage());
                }
            }
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleWindowItems(this);
        }

        public override int Size()
        {
            return 3 + this.items.Length * 5;
        }
    }
}