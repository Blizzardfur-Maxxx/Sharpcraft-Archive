using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet7UseEntity : Packet
    {
        public int playerEntityId;
        public int targetEntity;
        public int isLeftClick;
        public Packet7UseEntity()
        {
        }

        public Packet7UseEntity(int i1, int i2, int i3)
        {
            this.playerEntityId = i1;
            this.targetEntity = i2;
            this.isLeftClick = i3;
        }

        public override void Read(BinaryReader reader)
        {
            this.playerEntityId = reader.ReadBEInt();
            this.targetEntity = reader.ReadBEInt();
            this.isLeftClick = reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.playerEntityId);
            writer.WriteBEInt(this.targetEntity);
            writer.Write((byte)this.isLeftClick);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleUseEntity(this);
        }

        public override int Size()
        {
            return 9;
        }
    }
}