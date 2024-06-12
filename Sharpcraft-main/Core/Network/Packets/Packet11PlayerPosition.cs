using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet11PlayerPosition : Packet10Flying
    {
        public Packet11PlayerPosition()
        {
            this.moving = true;
        }

        public Packet11PlayerPosition(double d1, double d3, double d5, double d7, bool z9)
        {
            this.xPosition = d1;
            this.yPosition = d3;
            this.stance = d5;
            this.zPosition = d7;
            this.onGround = z9;
            this.moving = true;
        }

        public override void Read(BinaryReader reader)
        {
            this.xPosition = reader.ReadBEDouble();
            this.yPosition = reader.ReadBEDouble();
            this.stance = reader.ReadBEDouble();
            this.zPosition = reader.ReadBEDouble();
            base.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEDouble(this.xPosition);
            writer.WriteBEDouble(this.yPosition);
            writer.WriteBEDouble(this.stance);
            writer.WriteBEDouble(this.zPosition);
            base.Write(writer);
        }

        public override int Size()
        {
            return 33;
        }
    }
}