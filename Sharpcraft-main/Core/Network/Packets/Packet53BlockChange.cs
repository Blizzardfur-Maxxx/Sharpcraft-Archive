using SharpCraft.Core.World.GameLevel;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet53BlockChange : Packet
    {
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public int type;
        public int metadata;

        public Packet53BlockChange()
        {
            isChunkDataPacket = true;
        }

        public Packet53BlockChange(int i1, int i2, int i3, Level world4)
        {
            isChunkDataPacket = true;
            this.xPosition = i1;
            this.yPosition = i2;
            this.zPosition = i3;
            this.type = world4.GetTile(i1, i2, i3);
            this.metadata = world4.GetData(i1, i2, i3);
        }

        public override void Read(BinaryReader reader)
        {
            this.xPosition = reader.ReadBEInt();
            this.yPosition = reader.ReadByte();
            this.zPosition = reader.ReadBEInt();
            this.type = reader.ReadByte();
            this.metadata = reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.xPosition);
            writer.Write((byte)this.yPosition);
            writer.WriteBEInt(this.zPosition);
            writer.Write((byte)this.type);
            writer.Write((byte)this.metadata);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleBlockChange(this);
        }

        public override int Size()
        {
            return 11;
        }
    }
}