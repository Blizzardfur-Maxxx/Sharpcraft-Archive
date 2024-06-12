using SharpCraft.Core.World.Entities;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet17Sleep : Packet
    {
        public int field_a;
        public int field_b;
        public int field_c;
        public int field_d;
        public int field_e;
        public Packet17Sleep()
        {
        }

        public Packet17Sleep(Entity entity1, int i2, int i3, int i4, int i5)
        {
            this.field_e = i2;
            this.field_b = i3;
            this.field_c = i4;
            this.field_d = i5;
            this.field_a = entity1.entityID;
        }

        public override void Read(BinaryReader reader)
        {
            this.field_a = reader.ReadBEInt();
            this.field_e = reader.ReadByte();
            this.field_b = reader.ReadBEInt();
            this.field_c = reader.ReadByte();
            this.field_d = reader.ReadBEInt();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.field_a);
            writer.Write((byte)this.field_e);
            writer.WriteBEInt(this.field_b);
            writer.Write((byte)this.field_c);
            writer.WriteBEInt(this.field_d);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleSleep(this);
        }

        public override int Size()
        {
            return 14;
        }
    }
}