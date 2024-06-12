using SharpCraft.Core.World.Items;
using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet5PlayerInventory : Packet
    {
        public int entityID;
        public int slot;
        public int itemID;
        public int itemDamage;

        public Packet5PlayerInventory()
        {
        }

        public Packet5PlayerInventory(int i1, int i2, ItemInstance itemStack3)
        {
            this.entityID = i1;
            this.slot = i2;
            if (itemStack3 == null)
            {
                this.itemID = -1;
                this.itemDamage = 0;
            }
            else
            {
                this.itemID = itemStack3.itemID;
                this.itemDamage = itemStack3.GetItemDamage();
            }
        }

        public override void Read(BinaryReader reader)
        {
            this.entityID = reader.ReadBEInt();
            this.slot = reader.ReadBEShort();
            this.itemID = reader.ReadBEShort();
            this.itemDamage = reader.ReadBEShort();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityID);
            writer.WriteBEShort((short)this.slot);
            writer.WriteBEShort((short)this.itemID);
            writer.WriteBEShort((short)this.itemDamage);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandlePlayerInventory(this);
        }

        public override int Size()
        {
            return 8;
        }
    }
}