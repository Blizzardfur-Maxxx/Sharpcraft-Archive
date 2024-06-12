namespace SharpCraft.Core.NBT
{
    public class ByteArrayTag : Tag
    {
        public byte[] Value;

        public ByteArrayTag() { }

        public ByteArrayTag(byte[] value)
        {
            Value = value;
        }

        public override void WriteContents(System.IO.BinaryWriter binaryWriter)
        {
            binaryWriter.WriteBEInt(Value.Length);
            binaryWriter.WriteBytes(Value);
        }

        public override void ReadContents(System.IO.BinaryReader binaryReader)
        {
            Value = binaryReader.ReadBytes(binaryReader.ReadBEInt());
        }

        public override byte GetNBTType()
        {
            return 7;
        }

        public override string ToString()
        {
            return string.Format("[{0} bytes]", Value.Length);
        }
    }
}