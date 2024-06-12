using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Weather;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet71Weather : Packet
    {
        public int entityId;
        public int x;
        public int y;
        public int z;
        public int type;
        public Packet71Weather()
        {
        }

        public Packet71Weather(Entity entity1)
        {
            this.entityId = entity1.entityID;
            this.x = Mth.Floor(entity1.x * 32);
            this.y = Mth.Floor(entity1.y * 32);
            this.z = Mth.Floor(entity1.z * 32);
            if (entity1 is LightningBolt)
            {
                this.type = 1;
            }
        }

        public override void Read(BinaryReader reader)
        {
            this.entityId = reader.ReadBEInt();
            this.type = reader.ReadByte();
            this.x = reader.ReadBEInt();
            this.y = reader.ReadBEInt();
            this.z = reader.ReadBEInt();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityId);
            writer.Write((byte)this.type);
            writer.WriteBEInt(this.x);
            writer.WriteBEInt(this.y);
            writer.WriteBEInt(this.z);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleWeather(this);
        }

        public override int Size()
        {
            return 17;
        }
    }
}