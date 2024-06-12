using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet0KeepAlive : Packet
    {
        public override void Handle(PacketListener netHandler)
        {
        }

        public override void Read(BinaryReader reader)
        {
        }

        public override void Write(BinaryWriter writer)
        {
        }

        public override int Size()
        {
            return 0;
        }
    }
}