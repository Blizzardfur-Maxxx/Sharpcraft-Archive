using SharpCraft.Core.World.Entities;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet19EntityAction : Packet
    {
        public int entityId;
        public EntityActionType state;
        public Packet19EntityAction()
        {
        }

        public Packet19EntityAction(Entity entity1, EntityActionType i2)
        {
            this.entityId = entity1.entityID;
            this.state = i2;
        }

        public override void Read(BinaryReader reader)
        {
            this.entityId = reader.ReadBEInt();
            this.state = (EntityActionType)reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityId);
            writer.Write((byte)this.state);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleEntityAction(this);
        }

        public override int Size()
        {
            return 5;
        }
    }
}