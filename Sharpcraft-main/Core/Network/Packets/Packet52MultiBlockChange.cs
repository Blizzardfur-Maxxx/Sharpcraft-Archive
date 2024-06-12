using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using System.IO;


namespace SharpCraft.Core.Network.Packets
{
    public class Packet52MultiBlockChange : Packet
    {
        public int xPosition;
        public int zPosition;
        public short[] coordinateArray;
        public byte[] typeArray;
        public byte[] metadataArray;
        public int size;

        public Packet52MultiBlockChange()
        {
            isChunkDataPacket = true;
        }

        public Packet52MultiBlockChange(int i1, int i2, short[] s3, int i4, Level world5)
        {
            isChunkDataPacket = true;
            this.xPosition = i1;
            this.zPosition = i2;
            this.size = i4;
            this.coordinateArray = new short[i4];
            this.typeArray = new byte[i4];
            this.metadataArray = new byte[i4];
            LevelChunk chunk6 = world5.GetChunk(i1, i2);
            for (int i7 = 0; i7 < i4; ++i7)
            {
                int i8 = s3[i7] >> 12 & 15;
                int i9 = s3[i7] >> 8 & 15;
                int i10 = s3[i7] & 255;
                this.coordinateArray[i7] = s3[i7];
                this.typeArray[i7] = (byte)chunk6.GetBlockID(i8, i10, i9);
                this.metadataArray[i7] = (byte)chunk6.GetBlockMetadata(i8, i10, i9);
            }
        }

        public override void Read(BinaryReader reader)
        {
            this.xPosition = reader.ReadBEInt();
            this.zPosition = reader.ReadBEInt();
            this.size = reader.ReadBEShort() & 65535;
            this.coordinateArray = new short[this.size];
            this.typeArray = new byte[this.size];
            this.metadataArray = new byte[this.size];
            for (int i2 = 0; i2 < this.size; ++i2)
            {
                this.coordinateArray[i2] = reader.ReadBEShort();
            }

            reader.ReadFully(this.typeArray);
            reader.ReadFully(this.metadataArray);
        }

        public override void Write(BinaryWriter writer)
        {
            writer.WriteBEInt(this.xPosition);
            writer.WriteBEInt(this.zPosition);
            writer.WriteBEShort((short)this.size);
            for (int i2 = 0; i2 < this.size; ++i2)
            {
                writer.WriteBEShort((short)this.coordinateArray[i2]);
            }

            writer.Write(this.typeArray);
            writer.Write(this.metadataArray);
        }

        public override void Handle(PacketListener netHandler)
        {
            netHandler.HandleMultiBlockChange(this);
        }

        public override int Size()
        {
            return 10 + this.size * 4;
        }
    }
}