namespace SharpCraft.Core.NBT
{
    public class IntTag : Tag
    {
        public int Value;

        public IntTag() { }

        public IntTag(int value)
        {
            Value = value;
        }

        public override void WriteContents(System.IO.BinaryWriter binaryWriter)
        {
            binaryWriter.WriteBEInt(Value);
        }

        public override void ReadContents(System.IO.BinaryReader binaryReader)
        {
            Value = binaryReader.ReadBEInt();
        }

        public override byte GetNBTType()
        {
            return 3;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}