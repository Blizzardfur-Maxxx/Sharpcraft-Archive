using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet2Handshake : Packet
    {
        public string username;
        public Packet2Handshake()
        {
        }

        public Packet2Handshake(string string1)
        {
            this.username = string1;
        }

        public override void Read(BinaryReader reader)
        {
            this.username = reader.ReadUTF16BE(32);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteUTF16BE(this.username);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleHandshake(this);
        }

        public override int Size()
        {
            return 4 + this.username.Length + 4;
        }
    }
}