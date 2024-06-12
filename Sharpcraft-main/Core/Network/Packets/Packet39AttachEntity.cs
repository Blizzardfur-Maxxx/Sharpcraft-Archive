using SharpCraft.Core.World.Entities;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet39AttachEntity : Packet
    {
        public int entityId;
        public int vehicleEntityId;
        public Packet39AttachEntity()
        {
        }

        public Packet39AttachEntity(Entity entity1, Entity entity2)
        {
            this.entityId = entity1.entityID;
            this.vehicleEntityId = entity2 != null ? entity2.entityID : -1;
        }

        public override int Size()
        {
            return 8;
        }

        public override void Read(BinaryReader reader)
        {
            this.entityId = reader.ReadBEInt();
            this.vehicleEntityId = reader.ReadBEInt();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityId);
            writer.WriteBEInt(this.vehicleEntityId);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleAttachEntity(this);
        }
    }
}