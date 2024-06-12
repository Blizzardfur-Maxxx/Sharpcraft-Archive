using SharpCraft.Core.World.GameLevel;
using System.IO;
using System.IO.Compression;

namespace SharpCraft.Core.Network.Packets
{
    public class Packet51MapChunk : Packet
    {
        public int xp;
        public int yp;
        public int zp;
        public int xSize;
        public int ySize;
        public int zSize;
        public byte[] chunk;
        private int chunkSize;

        public Packet51MapChunk()
        {
            isChunkDataPacket = true;
        }

        public Packet51MapChunk(int i1, int i2, int i3, int i4, int i5, int i6, Level world7)
        {
            isChunkDataPacket = true;
            this.xp = i1;
            this.yp = i2;
            this.zp = i3;
            this.xSize = i4;
            this.ySize = i5;
            this.zSize = i6;
            MemoryStream encoded = new MemoryStream();
            byte[] data = world7.GetChunkData(i1, i2, i3, i4, i5, i6);
            ZLibStream stream = new ZLibStream(encoded, CompressionLevel.Optimal);

            try
            {
                stream.Write(data, 0, data.Length);
                stream.Flush();
                stream.Close();
                this.chunk = encoded.ToArray();
                this.chunkSize = chunk.Length;
            }
            finally
            {
                encoded.Dispose();
                stream.Dispose();
            }
        }

        public override void Read(BinaryReader reader)
        {
            this.xp = reader.ReadBEInt();
            this.yp = reader.ReadBEShort();
            this.zp = reader.ReadBEInt();
            this.xSize = reader.ReadByte() + 1;
            this.ySize = reader.ReadByte() + 1;
            this.zSize = reader.ReadByte() + 1;
            this.chunkSize = reader.ReadBEInt();
            byte[] b2 = new byte[this.chunkSize];
            reader.ReadFully(b2);
            MemoryStream encodedData = new MemoryStream(b2);
            this.chunk = new byte[this.xSize * this.ySize * this.zSize * 5 / 2];

            try
            {
                ZLibStream stream = new ZLibStream(encodedData, CompressionMode.Decompress);
                stream.Read(chunk, 0, chunk.Length);
                stream.Close();
                stream.Dispose();
            }
            catch (IOException ioe)
            {
                throw new IOException("Bad compressed data format", ioe);
            }
            finally 
            {
                encodedData.Dispose();
            }
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.xp);
            writer.WriteBEShort((short)this.yp);
            writer.WriteBEInt(this.zp);
            writer.Write((byte)(this.xSize - 1));
            writer.Write((byte)(this.ySize - 1));
            writer.Write((byte)(this.zSize - 1));
            writer.WriteBEInt(this.chunkSize);
            writer.Write(this.chunk, 0, this.chunkSize);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleMapChunk(this);
        }

        public override int Size()
        {
            return 17 + this.chunkSize;
        }
    }
}