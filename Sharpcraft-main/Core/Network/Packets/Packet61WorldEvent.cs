using SharpCraft.Core.World.GameLevel;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet61WorldEvent : Packet
    {
        public LevelEventType type;
        public int data;
        public int x;
        public int y;
        public int z;

        public Packet61WorldEvent()
        {
        }

        public Packet61WorldEvent(LevelEventType i1, int i2, int i3, int i4, int i5)
        {
            this.type = i1;
            this.x = i2;
            this.y = i3;
            this.z = i4;
            this.data = i5;
        }

        public override void Read(BinaryReader reader)
        {
            this.type = (LevelEventType)reader.ReadBEInt();
            this.x = reader.ReadBEInt();
            this.y = reader.ReadByte();
            this.z = reader.ReadBEInt();
            this.data = reader.ReadBEInt();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt((int)this.type);
            writer.WriteBEInt(this.x);
            writer.Write((byte)this.y);
            writer.WriteBEInt(this.z);
            writer.WriteBEInt(this.data);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleWorldEvent(this);
        }

        public override int Size()
        {
            return 20;
        }
    }
}