using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet106Transaction : Packet
    {
        public int windowId;
        public short shortWindowId;
        public bool field;
        public Packet106Transaction()
        {
        }

        public Packet106Transaction(int i1, short s2, bool z3)
        {
            this.windowId = i1;
            this.shortWindowId = s2;
            this.field = z3;
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleTransaction(this);
        }

        public override void Read(BinaryReader reader)
        {
            this.windowId = reader.ReadByte();
            this.shortWindowId = reader.ReadBEShort();
            this.field = reader.ReadByte() != 0;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.windowId);
            writer.WriteBEShort((short)this.shortWindowId);
            writer.Write(field);
        }

        public override int Size()
        {
            return 4;
        }
    }
}