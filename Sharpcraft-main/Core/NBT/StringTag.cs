namespace SharpCraft.Core.NBT
{
    public class StringTag : Tag
    {
        public string Value;

        public StringTag() { }

        public StringTag(string value)
        {
            Value = value;
        }

        public override void WriteContents(System.IO.BinaryWriter binaryWriter)
        {
            binaryWriter.WriteMUTF8(Value);
        }

        public override void ReadContents(System.IO.BinaryReader binaryReader)
        {
            Value = binaryReader.ReadMUTF8();
        }

        public override byte GetNBTType()
        {
            return 8;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}