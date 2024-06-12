using System.IO;
namespace SharpCraft.Core.Network.Packets
{
    public class Packet200Statistic : Packet
    {
        public int statId;
        public int statCount;
		
        public Packet200Statistic()
        {
        }

        public Packet200Statistic(int i1, int i2)
        {
            this.statId = i1;
            this.statCount = i2;
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleStatistic(this);
        }

        public override void Read(BinaryReader reader)
        {
            this.statId = reader.ReadBEInt();
            this.statCount = reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.statId);
            writer.Write((byte)this.statCount);
        }

        public override int Size()
        {
            return 6;
        }
    }
}