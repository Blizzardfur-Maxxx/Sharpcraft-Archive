using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet22Collect : Packet
    {
        public int collectedEntityId;
        public int collectorEntityId;
        public Packet22Collect()
        {
        }

        public Packet22Collect(int i1, int i2)
        {
            this.collectedEntityId = i1;
            this.collectorEntityId = i2;
        }

        public override void Read(BinaryReader reader)
        {
            this.collectedEntityId = reader.ReadBEInt();
            this.collectorEntityId = reader.ReadBEInt();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.collectedEntityId);
            writer.WriteBEInt(this.collectorEntityId);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleCollect(this);
        }

        public override int Size()
        {
            return 8;
        }
    }
}