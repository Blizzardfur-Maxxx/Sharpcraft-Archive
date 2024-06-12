using SharpCraft.Core.World.Entities;
using System.Collections.Generic;

using System.IO;
using static SharpCraft.Core.World.Entities.SynchedEntityData;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet40EntityMetadata : Packet
    {
        public int entityId;
        private IList<DataItem> metadata;

        public Packet40EntityMetadata()
        {
        }

        public Packet40EntityMetadata(int i1, SynchedEntityData dataWatcher2)
        {
            this.entityId = i1;
            this.metadata = dataWatcher2.GetChangedObjects();
        }

        public override void Read(BinaryReader reader)
        {
            this.entityId = reader.ReadBEInt();
            this.metadata = SynchedEntityData.ReadWatchableObjects(reader);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityId);
            SynchedEntityData.WriteObjectsInListToStream(this.metadata, writer);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleEntityMetadata(this);
        }

        public override int Size()
        {
            return 5;
        }

        public virtual IList<DataItem> GetMetadata()
        {
            return this.metadata;
        }
    }
}