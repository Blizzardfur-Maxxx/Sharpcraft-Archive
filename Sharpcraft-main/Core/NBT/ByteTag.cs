namespace SharpCraft.Core.NBT
{
    public class ByteTag : Tag
    {
        public byte Value;

        public ByteTag() { }

        public ByteTag(byte value)
        {
            Value = value;
        }

        public override void WriteContents(System.IO.BinaryWriter binaryWriter)
        {
            binaryWriter.Write(Value);
        }

        public override void ReadContents(System.IO.BinaryReader binaryReader)
        {
            Value = binaryReader.ReadByte();
        }

        public override byte GetNBTType()
        {
            return 1;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}