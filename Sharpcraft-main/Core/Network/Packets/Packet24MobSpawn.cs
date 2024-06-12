using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using System.Collections.Generic;
using System.IO;
using static SharpCraft.Core.World.Entities.SynchedEntityData;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet24MobSpawn : Packet
    {
        public int entityId;
        public sbyte type;
        public int xPosition;
        public int yPosition;
        public int zPosition;
        public sbyte yaw;
        public sbyte pitch;
        private SynchedEntityData metaData;
        private IList<DataItem> receivedMetadata;

        public Packet24MobSpawn()
        {
        }

        public Packet24MobSpawn(Mob mob)
        {
            entityId = mob.entityID;
            type = (sbyte)EntityFactory.GetNetworkId(mob);
            xPosition = Mth.Floor(mob.x * 32d);
            yPosition = Mth.Floor(mob.y * 32d);
            zPosition = Mth.Floor(mob.z * 32d);
            yaw = (sbyte)(int)(mob.yaw * 256F / 360F);
            pitch = (sbyte)(int)(mob.pitch * 256F / 360F);
            metaData = mob.GetDataWatcher();
        }

        public override void Read(BinaryReader reader)
        {
            entityId = reader.ReadBEInt();
            type = reader.ReadSByte();
            xPosition = reader.ReadBEInt();
            yPosition = reader.ReadBEInt();
            zPosition = reader.ReadBEInt();
            yaw = reader.ReadSByte();
            pitch = reader.ReadSByte();
            receivedMetadata = ReadWatchableObjects(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(entityId);
            writer.Write(type);
            writer.WriteBEInt(xPosition);
            writer.WriteBEInt(yPosition);
            writer.WriteBEInt(zPosition);
            writer.Write(yaw);
            writer.Write(pitch);
            metaData.WriteWatchableObjects(writer);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleMobSpawn(this);
        }

        public override int Size()
        {
            return 20;
        }

        public virtual IList<DataItem> GetMetadata()
        {
            return this.receivedMetadata;
        }
    }
}