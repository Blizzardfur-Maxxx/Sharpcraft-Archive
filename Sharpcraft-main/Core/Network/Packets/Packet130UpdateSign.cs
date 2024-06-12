

using System;

using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet130UpdateSign : Packet
    {
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public String[] signLines;

        public Packet130UpdateSign()
        {
            isChunkDataPacket = true;
        }

        public Packet130UpdateSign(int i1, int i2, int i3, String[] string4)
        {
            isChunkDataPacket = true;
            this.xPosition = i1;
            this.yPosition = i2;
            this.zPosition = i3;
            this.signLines = string4;
        }

        public override void Read(BinaryReader reader)
        {
            this.xPosition = reader.ReadBEInt();
            this.yPosition = reader.ReadBEShort();
            this.zPosition = reader.ReadBEInt();
            this.signLines = new string[4];
            for (int i2 = 0; i2 < 4; ++i2)
            {
                this.signLines[i2] = reader.ReadUTF16BE(15);
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.xPosition);
            writer.WriteBEShort((short)this.yPosition);
            writer.WriteBEInt(this.zPosition);
            for (int i2 = 0; i2 < 4; ++i2)
            {
                writer.WriteUTF16BE(this.signLines[i2]);
            }
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleSignUpdate(this);
        }

        public override int Size()
        {
            int i1 = 0;
            for (int i2 = 0; i2 < 4; ++i2)
            {
                i1 += this.signLines[i2].Length;
            }

            return i1;
        }
    }
}