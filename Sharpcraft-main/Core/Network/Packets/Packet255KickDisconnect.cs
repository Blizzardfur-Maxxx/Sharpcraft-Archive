using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet255KickDisconnect : Packet
    {
        public string reason;
        public Packet255KickDisconnect()
        {
        }

        public Packet255KickDisconnect(string string1)
        {
            this.reason = string1;
        }

        public override void Read(BinaryReader reader)
        {
            this.reason = reader.ReadUTF16BE(100);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteUTF16BE(this.reason);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleKickDisconnect(this);
        }

        public override int Size()
        {
            return this.reason.Length;
        }
    }
}