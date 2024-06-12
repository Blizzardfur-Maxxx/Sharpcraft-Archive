using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet4UpdateTime : Packet
    {
        public long time;
        public Packet4UpdateTime()
        {
        }

        public Packet4UpdateTime(long j1)
        {
            this.time = j1;
        }

        public override void Read(BinaryReader reader)
        {
            this.time = reader.ReadBELong();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBELong(this.time);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleUpdateTime(this);
        }

        public override int Size()
        {
            return 8;
        }
    }
}