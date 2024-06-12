using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet30Entity : Packet
    {
        public int entityId;
        public sbyte xPosition;
        public sbyte yPosition;
        public sbyte zPosition;
        public sbyte yaw;
        public sbyte pitch;
        public bool rotating = false;

        public Packet30Entity()
        {
        }

        public Packet30Entity(int id)
        {
            entityId = id;
        }

        public override void Read(BinaryReader reader)
        {
            entityId = reader.ReadBEInt();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(entityId);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleEntity(this);
        }

        public override int Size()
        {
            return 4;
        }
    }
}