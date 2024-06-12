using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet32EntityLook : Packet30Entity
    {
        public Packet32EntityLook()
        {
            rotating = true;
        }

        public Packet32EntityLook(int id, sbyte yaw, sbyte pitch) : base(id)
        {
            this.yaw = yaw;
            this.pitch = pitch;
            rotating = true;
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            yaw = reader.ReadSByte();
            pitch = reader.ReadSByte();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(yaw);
            writer.Write(pitch);
        }

        public override int Size()
        {
            return 6;
        }
    }
}