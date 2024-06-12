using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet29DestroyEntity : Packet
    {
        public int entityId;
        public Packet29DestroyEntity()
        {
        }

        public Packet29DestroyEntity(int i1)
        {
            this.entityId = i1;
        }

        public override void Read(BinaryReader reader)
        {
            this.entityId = reader.ReadBEInt();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.entityId);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleDestroyEntity(this);
        }

        public override int Size()
        {
            return 4;
        }
    }
}