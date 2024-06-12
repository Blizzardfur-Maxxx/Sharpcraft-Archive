namespace SharpCraft.Core.NBT
{
    public class DoubleTag : Tag
    {
        public double Value;

        public DoubleTag() { }

        public DoubleTag(double value)
        {
            Value = value;
        }

        public override void WriteContents(System.IO.BinaryWriter binaryWriter)
        {
            binaryWriter.WriteBEDouble(Value);
        }

        public override void ReadContents(System.IO.BinaryReader binaryReader)
        {
            Value = binaryReader.ReadBEDouble();
        }

        public override byte GetNBTType()
        {
            return 6;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}