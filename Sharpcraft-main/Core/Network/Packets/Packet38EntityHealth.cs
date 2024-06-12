using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet38EntityHealth : Packet
    {
        public int entityId;
        public byte entityStatus;

        public Packet38EntityHealth()
        {
        }

        public Packet38EntityHealth(int id, byte health)
        {
            entityId = id;
            entityStatus = health;
        }

        public override void Read(BinaryReader reader)
        {
            entityId = reader.ReadBEInt();
            entityStatus = reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(entityId);
            writer.Write(entityStatus);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleEntityStatus(this);
        }

        public override int Size()
        {
            return 5;
        }
    }
}