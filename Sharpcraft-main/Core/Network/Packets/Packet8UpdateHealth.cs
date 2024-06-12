using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet8UpdateHealth : Packet
    {
        public int healthMP;
        public Packet8UpdateHealth()
        {
        }

        public Packet8UpdateHealth(int i1)
        {
            this.healthMP = i1;
        }

        public override void Read(BinaryReader reader)
        {
            this.healthMP = reader.ReadBEShort();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEShort((short)this.healthMP);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleHealth(this);
        }

        public override int Size()
        {
            return 2;
        }
    }
}