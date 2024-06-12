using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet1Login : Packet
    {
        public int protocolVersion;
        public string username;
        public long mapSeed;
        public sbyte dimension;
		
        public Packet1Login()
        {
        }

        public Packet1Login(string username, int i2)
        {
            this.username = username;
            this.protocolVersion = i2;
        }

        public Packet1Login(string string1, int i2, long j3, sbyte b5)
        {
            this.username = string1;
            this.protocolVersion = i2;
            this.mapSeed = j3;
            this.dimension = b5;
        }

        public override void Read(BinaryReader reader)
        {
            this.protocolVersion = reader.ReadBEInt();
            this.username = reader.ReadUTF16BE(16);
            this.mapSeed = reader.ReadBELong();
            this.dimension = reader.ReadSByte();
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.protocolVersion);
            writer.WriteUTF16BE(this.username);
            writer.WriteBELong(this.mapSeed);
            writer.Write((byte)this.dimension);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleLogin(this);
        }

        public override int Size()
        {
            return 4 + this.username.Length + 4 + 5;
        }
    }
}