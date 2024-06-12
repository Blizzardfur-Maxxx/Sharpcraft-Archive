using System;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet31RelEntityMove : Packet30Entity
    {
        public Packet31RelEntityMove()
        {
        }

        public Packet31RelEntityMove(int id, sbyte x, sbyte y, sbyte z) : base(id)
        {
            xPosition = x;
            yPosition = y;
            zPosition = z;
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            xPosition = reader.ReadSByte();
            yPosition = reader.ReadSByte();
            zPosition = reader.ReadSByte();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(xPosition);
            writer.Write(yPosition);
            writer.Write(zPosition);
        }

        public override int Size()
        {
            return 7;
        }
    }
}