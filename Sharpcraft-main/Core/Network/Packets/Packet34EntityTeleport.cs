using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet34EntityTeleport : Packet
    {
        public int entityId;
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public sbyte yaw;
        public sbyte pitch;

        public Packet34EntityTeleport()
        {
        }

        public Packet34EntityTeleport(Entity entity)
        {
            entityId = entity.entityID;
            xPosition = Mth.Floor(entity.x * 32d);
            yPosition = Mth.Floor(entity.y * 32d);
            zPosition = Mth.Floor(entity.z * 32d);
            yaw = (sbyte)(entity.yaw * 256F / 360F);
            pitch = (sbyte)(entity.pitch * 256F / 360F);
        }

        public Packet34EntityTeleport(int id, int x, int y, int z, sbyte yaw, sbyte pitch)
        {
            entityId = id;
            xPosition = x;
            yPosition = y;
            zPosition = z;
            this.yaw = yaw;
            this.pitch = pitch;
        }

        public override void Read(BinaryReader reader)
        {
            entityId = reader.ReadBEInt();
            xPosition = reader.ReadBEInt();
            yPosition = reader.ReadBEInt();
            zPosition = reader.ReadBEInt();
            yaw = reader.ReadSByte();
            pitch = reader.ReadSByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(entityId);
            writer.WriteBEInt(xPosition);
            writer.WriteBEInt(yPosition);
            writer.WriteBEInt(zPosition);
            writer.Write(yaw);
            writer.Write(pitch);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleEntityTeleport(this);
        }

        public override int Size()
        {
            return 34;
        }
    }
}