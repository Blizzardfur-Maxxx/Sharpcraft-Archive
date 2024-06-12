using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet6SpawnPosition : Packet
    {
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public Packet6SpawnPosition()
        {
        }

        public Packet6SpawnPosition(int i1, int i2, int i3)
        {
            this.xPosition = i1;
            this.yPosition = i2;
            this.zPosition = i3;
        }

        public override void Read(BinaryReader reader)
        {
            this.xPosition = reader.ReadBEInt();
            this.yPosition = reader.ReadBEInt();
            this.zPosition = reader.ReadBEInt();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.xPosition);
            writer.WriteBEInt(this.yPosition);
            writer.WriteBEInt(this.zPosition);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleSpawnPosition(this);
        }

        public override int Size()
        {
            return 12;
        }
    }
}