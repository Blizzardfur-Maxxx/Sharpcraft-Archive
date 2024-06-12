using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet50PreChunk : Packet
    {
        public int xPosition;
        public int yPosition;
        public bool mode;

        public Packet50PreChunk()
        {
            isChunkDataPacket = true;
        }

        public Packet50PreChunk(int i1, int i2, bool z3)
        {
            isChunkDataPacket = true;
            this.xPosition = i1;
            this.yPosition = i2;
            this.mode = z3;
        }

        public override void Read(BinaryReader reader)
        {
            this.xPosition = reader.ReadBEInt();
            this.yPosition = reader.ReadBEInt();
            this.mode = reader.ReadByte() != 0;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.xPosition);
            writer.WriteBEInt(this.yPosition);
            writer.Write((byte)(this.mode ? 1 : 0));
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandlePreChunk(this);
        }

        public override int Size()
        {
            return 9;
        }
    }
}