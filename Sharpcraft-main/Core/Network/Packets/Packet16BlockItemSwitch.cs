using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet16BlockItemSwitch : Packet
    {
        public int id;
        public Packet16BlockItemSwitch()
        {
        }

        public Packet16BlockItemSwitch(int i1)
        {
            this.id = i1;
        }

        public override void Read(BinaryReader reader)
        {
            this.id = reader.ReadBEShort();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEShort((short)this.id);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleBlockItemSwitch(this);
        }

        public override int Size()
        {
            return 2;
        }
    }
}