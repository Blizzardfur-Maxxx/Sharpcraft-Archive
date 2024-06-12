using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet101CloseWindow : Packet
    {
        public int windowId;

        public Packet101CloseWindow()
        {
        }

        public Packet101CloseWindow(int i1)
        {
            this.windowId = i1;
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleCloseWindow(this);
        }

        public override void Read(BinaryReader reader)
        {
            this.windowId = reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.windowId);
        }

        public override int Size()
        {
            return 1;
        }
    }
}