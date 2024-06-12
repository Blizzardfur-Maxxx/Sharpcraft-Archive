using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet12PlayerLook : Packet10Flying
    {
        public Packet12PlayerLook()
        {
            this.rotating = true;
        }

        public Packet12PlayerLook(float f1, float f2, bool z3)
        {
            this.yaw = f1;
            this.pitch = f2;
            this.onGround = z3;
            this.rotating = true;
        }

        public override void Read(BinaryReader reader)
        {
            this.yaw = reader.ReadBEFloat();
            this.pitch = reader.ReadBEFloat();
            base.Read(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEFloat(this.yaw);
            writer.WriteBEFloat(this.pitch);
            base.Write(writer);
        }

        public override int Size()
        {
            return 9;
        }
    }
}