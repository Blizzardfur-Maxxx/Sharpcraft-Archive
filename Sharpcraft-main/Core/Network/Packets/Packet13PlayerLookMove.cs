using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet13PlayerLookMove : Packet10Flying
    {
        public Packet13PlayerLookMove()
        {
            this.rotating = true;
            this.moving = true;
        }

        public Packet13PlayerLookMove(double d1, double d3, double d5, double d7, float f9, float f10, bool z11)
        {
            this.xPosition = d1;
            this.yPosition = d3;
            this.stance = d5;
            this.zPosition = d7;
            this.yaw = f9;
            this.pitch = f10;
            this.onGround = z11;
            this.rotating = true;
            this.moving = true;
        }

        public override void Read(BinaryReader reader)
        {
            this.xPosition = reader.ReadBEDouble();
            this.yPosition = reader.ReadBEDouble();
            this.stance = reader.ReadBEDouble();
            this.zPosition = reader.ReadBEDouble();
            this.yaw = reader.ReadBEFloat();
            this.pitch = reader.ReadBEFloat();
            base.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEDouble(this.xPosition);
            writer.WriteBEDouble(this.yPosition);
            writer.WriteBEDouble(this.stance);
            writer.WriteBEDouble(this.zPosition);
            writer.WriteBEFloat(this.yaw);
            writer.WriteBEFloat(this.pitch);
            base.Write(writer);
        }

        public override int Size()
        {
            return 41;
        }
    }
}