using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet100OpenWindow : Packet
    {
        public int windowId;
        public InventoryType inventoryType;
        public string windowTitle;
        public int slotsCount;

        public Packet100OpenWindow()
        {
        }

        public Packet100OpenWindow(int i1, InventoryType i2, string string3, int i4)
        {
            this.windowId = i1;
            this.inventoryType = i2;
            this.windowTitle = string3;
            this.slotsCount = i4;
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleOpenWindow(this);
        }

        public override void Read(BinaryReader reader)
        {
            this.windowId = reader.ReadByte();
            this.inventoryType = (InventoryType)reader.ReadByte();
            this.windowTitle = reader.ReadMUTF8();
            this.slotsCount = reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.windowId);
            writer.Write((byte)this.inventoryType);
            writer.WriteMUTF8(this.windowTitle);
            writer.Write((byte)this.slotsCount);
        }

        public override int Size()
        {
            return 3 + this.windowTitle.Length;
        }
    }
}