using System.IO;
using static SharpCraft.Core.Util.Facing;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet14BlockDig : Packet
    {
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public TileFace face;
        public PlayerDigActionType status;
        public Packet14BlockDig()
        {
        }

        public Packet14BlockDig(PlayerDigActionType i1, int i2, int i3, int i4, TileFace i5)
        {
            this.status = i1;
            this.xPosition = i2;
            this.yPosition = i3;
            this.zPosition = i4;
            this.face = i5;
        }

        public override void Read(BinaryReader reader)
        {
            this.status = (PlayerDigActionType)reader.ReadByte();
            this.xPosition = reader.ReadBEInt();
            this.yPosition = reader.ReadByte();
            this.zPosition = reader.ReadBEInt();
            this.face = (TileFace)reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.status);
            writer.WriteBEInt(this.xPosition);
            writer.Write((byte)this.yPosition);
            writer.WriteBEInt(this.zPosition);
            writer.Write((byte)this.face);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleBlockDig(this);
        }

        public override int Size()
        {
            return 11;
        }
    }
}