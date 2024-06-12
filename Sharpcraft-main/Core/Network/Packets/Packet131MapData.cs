using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet131MapData : Packet
    {
        public short xpos;
        public short ypos;
        public byte[] data;

        public Packet131MapData()
        {
            isChunkDataPacket = true;
        }

        public Packet131MapData(short x, short y, byte[] data)
        {
            isChunkDataPacket = true;
            xpos = x;
            ypos = y;
            this.data = data;
        }

        public override void Read(BinaryReader reader)
        {
            xpos = reader.ReadBEShort();
            ypos = reader.ReadBEShort();
            data = new byte[reader.ReadByte() & 255];
            reader.ReadFully(data);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEShort(xpos);
            writer.WriteBEShort(ypos);
            writer.Write((byte)data.Length);
            writer.Write(data);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleMapData(this);
        }

        public override int Size()
        {
            return 4 + this.data.Length;
        }
    }
}