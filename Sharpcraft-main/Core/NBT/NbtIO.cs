using System.IO;
using System.IO.Compression;

namespace SharpCraft.Core.NBT
{
    public static class NbtIO
    {
        public static CompoundTag ReadCompressed(Stream stream)
        {
            BinaryReader binaryReader = new BinaryReader(new GZipStream(stream, CompressionMode.Decompress));

            try
            {
                return Read(binaryReader);
            }
            finally
            {
                binaryReader.Close();
            }
        }

        public static void WriteCompressed(CompoundTag nbtCompound, Stream stream)
        {
            BinaryWriter binaryWriter = new BinaryWriter(new GZipStream(stream, CompressionMode.Compress));

            try
            {
                Write(nbtCompound, binaryWriter);
            }
            finally
            {
                binaryWriter.Flush();
                binaryWriter.Close();
            }
        }

        public static CompoundTag Read(BinaryReader binaryReader)
        {
            Tag nbt = Tag.Read(binaryReader);

            if (nbt is CompoundTag)
                return (CompoundTag)nbt;
            else
                throw new IOException("Root tag must be a named compound tag");
        }

        public static void Write(CompoundTag nbtCompound, BinaryWriter binaryWriter)
        {
            Tag.Write(nbtCompound, binaryWriter);
        }
    }
}