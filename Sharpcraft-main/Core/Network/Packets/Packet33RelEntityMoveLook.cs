using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet33RelEntityMoveLook : Packet30Entity
    {
        public Packet33RelEntityMoveLook()
        {
            rotating = true;
        }

        public Packet33RelEntityMoveLook(int id, sbyte x, sbyte y, sbyte z, 
            sbyte yaw, sbyte pitch) : base(id)
        {
            xPosition = x;
            yPosition = y;
            zPosition = z;
            this.yaw = yaw;
            this.pitch = pitch;
            rotating = true;
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            xPosition = reader.ReadSByte();
            yPosition = reader.ReadSByte();
            zPosition = reader.ReadSByte();
            yaw = reader.ReadSByte();
            pitch = reader.ReadSByte();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(xPosition);
            writer.Write(yPosition);
            writer.Write(zPosition);
            writer.Write(yaw);
            writer.Write(pitch);
        }

        public override int Size()
        {
            return 9;
        }
    }
}