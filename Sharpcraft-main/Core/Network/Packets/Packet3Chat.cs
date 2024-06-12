using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet3Chat : Packet
    {
        public string message;
        public Packet3Chat()
        {
        }

        public Packet3Chat(string string1)
        {
            if (string1.Length > 119)
            {
                string1 = string1.Substring(0, 119);
            }

            this.message = string1;
        }

        public override void Read(BinaryReader reader)
        {
            this.message = reader.ReadUTF16BE(119);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteUTF16BE(this.message);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleChat(this);
        }

        public override int Size()
        {
            return this.message.Length;
        }
    }
}