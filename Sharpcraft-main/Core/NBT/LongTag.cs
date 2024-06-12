namespace SharpCraft.Core.NBT
{
    public class LongTag : Tag
    {
        public long Value;

        public LongTag() { }

        public LongTag(long value)
        {
            Value = value;
        }

        public override void WriteContents(System.IO.BinaryWriter binaryWriter)
        {
            binaryWriter.WriteBELong(Value);
        }

        public override void ReadContents(System.IO.BinaryReader binaryReader)
        {
            Value = binaryReader.ReadBELong();
        }

        public override byte GetNBTType()
        {
            return 4;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}