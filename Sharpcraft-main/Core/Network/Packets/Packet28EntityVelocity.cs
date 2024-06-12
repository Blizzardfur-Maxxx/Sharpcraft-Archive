using SharpCraft.Core.World.Entities;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet28EntityVelocity : Packet
    {
        public int entityId;
        public int motionX;
        public int motionY;
        public int motionZ;

        public Packet28EntityVelocity()
        {
        }

        public Packet28EntityVelocity(Entity entity1) : this(entity1.entityID, entity1.motionX, entity1.motionY, entity1.motionZ)
        {
        }

        public Packet28EntityVelocity(int i1, double d2, double d4, double d6)
        {
            this.entityId = i1;
            double d8 = 3.9;
            if (d2 < -d8)
            {
                d2 = -d8;
            }

            if (d4 < -d8)
            {
                d4 = -d8;
            }

            if (d6 < -d8)
            {
                d6 = -d8;
            }

            if (d2 > d8)
            {
                d2 = d8;
            }

            if (d4 > d8)
            {
                d4 = d8;
            }

            if (d6 > d8)
            {
                d6 = d8;
            }

            this.motionX = (int)(d2 * 8000.0D);
            this.motionY = (int)(d4 * 8000.0D);
            this.motionZ = (int)(d6 * 8000.0D);
        }

        public override void Read(BinaryReader reader)
        {
            this.entityId = reader.ReadBEInt();
            this.motionX = reader.ReadBEShort();
            this.motionY = reader.ReadBEShort();
            this.motionZ = reader.ReadBEShort();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityId);
            writer.WriteBEShort((short)this.motionX);
            writer.WriteBEShort((short)this.motionY);
            writer.WriteBEShort((short)this.motionZ);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleEntityVelocity(this);
        }

        public override int Size()
        {
            return 10;
        }
    }
}