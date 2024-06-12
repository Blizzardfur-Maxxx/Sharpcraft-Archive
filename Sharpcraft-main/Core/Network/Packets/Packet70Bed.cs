

using System;

using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet70Bed : Packet
    {
        public static readonly string[] tile_bed_notValid = new[]
        {
            "tile.bed.notValid",
            null,
            null
        };
        public int b_ReadByte;

        public Packet70Bed()
        {
        }

        public Packet70Bed(int i1)
        {
            this.b_ReadByte = i1;
        }

        public override void Read(BinaryReader reader)
        {
            this.b_ReadByte = reader.ReadByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.Write((byte)b_ReadByte);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleBed(this);
        }

        public override int Size()
        {
            return 1;
        }
    }
}