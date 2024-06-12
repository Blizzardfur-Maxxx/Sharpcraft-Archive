using SharpCraft.Core.World.Entities;
using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet18Animation : Packet
    {
        public int entityId;
        public EntityAnimationType animate;
        public Packet18Animation()
        {
        }

        public Packet18Animation(Entity entity1, EntityAnimationType i2)
        {
            this.entityId = entity1.entityID;
            this.animate = i2;
        }

        public override void Read(BinaryReader reader)
        {
            this.entityId = reader.ReadBEInt();
            this.animate = (EntityAnimationType)reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityId);
            writer.Write((byte)this.animate);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleArmAnimation(this);
        }

        public override int Size()
        {
            return 5;
        }
    }
}