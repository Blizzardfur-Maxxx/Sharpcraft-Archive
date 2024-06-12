using System.IO;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet105UpdateProgressbar : Packet
    {
        public int windowId;
        public int progressBar;
        public int progressBarValue;
        public Packet105UpdateProgressbar()
        {
        }

        public Packet105UpdateProgressbar(int i1, int i2, int i3)
        {
            this.windowId = i1;
            this.progressBar = i2;
            this.progressBarValue = i3;
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleUpdateProgress(this);
        }

        public override void Read(BinaryReader reader)
        {
            this.windowId = reader.ReadByte();
            this.progressBar = reader.ReadBEShort();
            this.progressBarValue = reader.ReadBEShort();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)this.windowId);
            writer.WriteBEShort((short)this.progressBar);
            writer.WriteBEShort((short)this.progressBarValue);
        }

        public override int Size()
        {
            return 5;
        }
    }
}