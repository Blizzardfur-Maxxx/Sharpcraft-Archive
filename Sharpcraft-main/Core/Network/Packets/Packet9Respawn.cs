using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet9Respawn : Packet
    {
        public sbyte dimension;

        public Packet9Respawn()
        {
        }

        public Packet9Respawn(sbyte b1)
        {
            this.dimension = b1;
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleRespawn(this);
        }

        public override void Read(BinaryReader reader)
        {
            this.dimension = reader.ReadSByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.dimension);
        }

        public override int Size()
        {
            return 1;
        }
    }
}