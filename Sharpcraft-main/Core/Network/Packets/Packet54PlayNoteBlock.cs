using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet54PlayNoteBlock : Packet
    {
        public int xLocation;
        public int yLocation;
        public int zLocation;
        public int instrumentType;
        public int pitch;
        public Packet54PlayNoteBlock()
        {
        }

        public Packet54PlayNoteBlock(int i1, int i2, int i3, int i4, int i5)
        {
            this.xLocation = i1;
            this.yLocation = i2;
            this.zLocation = i3;
            this.instrumentType = i4;
            this.pitch = i5;
        }

        public override void Read(BinaryReader reader)
        {
            this.xLocation = reader.ReadBEInt();
            this.yLocation = reader.ReadBEShort();
            this.zLocation = reader.ReadBEInt();
            this.instrumentType = reader.ReadByte();
            this.pitch = reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.xLocation);
            writer.WriteBEShort((short)this.yLocation);
            writer.WriteBEInt(this.zLocation);
            writer.Write((byte)this.instrumentType);
            writer.Write((byte)this.pitch);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleNotePlay(this);
        }

        public override int Size()
        {
            return 12;
        }
    }
}