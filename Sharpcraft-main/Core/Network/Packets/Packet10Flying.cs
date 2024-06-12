using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet10Flying : Packet
    {
        public double xPosition;
        public double yPosition;
        public double zPosition;
        public double stance;
        public float yaw;
        public float pitch;
        public bool onGround;
        public bool moving;
        public bool rotating;

        public Packet10Flying()
        {
        }

        public Packet10Flying(bool z1)
        {
            this.onGround = z1;
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleFlying(this);
        }

        public override void Read(BinaryReader reader)
        {
            this.onGround = reader.ReadByte() != 0;
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write(this.onGround);
        }

        public override int Size()
        {
            return 1;
        }
    }
}