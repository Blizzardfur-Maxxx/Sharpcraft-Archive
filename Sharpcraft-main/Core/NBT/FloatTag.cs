using System.IO;

namespace SharpCraft.Core.NBT 
{
    public class FloatTag : Tag
    {
        public float Value;

        public FloatTag() { }

        public FloatTag(float value)
        {
            Value = value;
        }

        public override void WriteContents(BinaryWriter binaryWriter)
        {
            binaryWriter.WriteBEFloat(Value);
        }

        public override void ReadContents(BinaryReader binaryReader)
        {
            Value = binaryReader.ReadBEFloat();
        }

        public override byte GetNBTType()
        {
            return (byte)5;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}
