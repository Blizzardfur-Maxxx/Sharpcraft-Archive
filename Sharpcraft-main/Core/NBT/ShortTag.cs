namespace SharpCraft.Core.NBT
{
    public class ShortTag : Tag
    {
        public short Value;

        public ShortTag() { }

        public ShortTag(short value)
        {
            Value = value;
        }

        public override void WriteContents(System.IO.BinaryWriter binaryWriter)
        {
            binaryWriter.WriteBEShort(Value);
        }

        public override void ReadContents(System.IO.BinaryReader binaryReader)
        {
            Value = binaryReader.ReadBEShort();
        }

        public override byte GetNBTType()
        {
            return 2;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}