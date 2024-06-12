using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet23VehicleSpawn : Packet
    {
        public int entityId;
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public int field_e;
        public int field_f;
        public int field_g;
        public SpawnObjectType type;
        public int field_i;
        public Packet23VehicleSpawn()
        {
        }

        public Packet23VehicleSpawn(Entity entity1, SpawnObjectType i2) : this(entity1, i2, 0)
        {
        }

        public Packet23VehicleSpawn(Entity entity1, SpawnObjectType i2, int i3)
        {
            this.entityId = entity1.entityID;
            this.xPosition = Mth.Floor(entity1.x * 32);
            this.yPosition = Mth.Floor(entity1.y * 32);
            this.zPosition = Mth.Floor(entity1.z * 32);
            this.type = i2;
            this.field_i = i3;
            if (i3 > 0)
            {
                double d4 = entity1.motionX;
                double d6 = entity1.motionY;
                double d8 = entity1.motionZ;
                double d10 = 3.9;
                if (d4 < -d10)
                {
                    d4 = -d10;
                }

                if (d6 < -d10)
                {
                    d6 = -d10;
                }

                if (d8 < -d10)
                {
                    d8 = -d10;
                }

                if (d4 > d10)
                {
                    d4 = d10;
                }

                if (d6 > d10)
                {
                    d6 = d10;
                }

                if (d8 > d10)
                {
                    d8 = d10;
                }

                this.field_e = (int)(d4 * 8000);
                this.field_f = (int)(d6 * 8000);
                this.field_g = (int)(d8 * 8000);
            }
        }

        public override void Read(BinaryReader reader)
        {
            this.entityId = reader.ReadBEInt();
            this.type = (SpawnObjectType)reader.ReadByte();
            this.xPosition = reader.ReadBEInt();
            this.yPosition = reader.ReadBEInt();
            this.zPosition = reader.ReadBEInt();
            this.field_i = reader.ReadBEInt();
            if (this.field_i > 0)
            {
                this.field_e = reader.ReadBEShort();
                this.field_f = reader.ReadBEShort();
                this.field_g = reader.ReadBEShort();
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityId);
            writer.Write((byte)this.type);
            writer.WriteBEInt(this.xPosition);
            writer.WriteBEInt(this.yPosition);
            writer.WriteBEInt(this.zPosition);
            writer.WriteBEInt(this.field_i);
            if (this.field_i > 0)
            {
                writer.WriteBEShort((short)this.field_e);
                writer.WriteBEShort((short)this.field_f);
                writer.WriteBEShort((short)this.field_g);
            }
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleVehicleSpawn(this);
        }

        public override int Size()
        {
            return 21 + this.field_i > 0 ? 6 : 0;
        }
    }
}