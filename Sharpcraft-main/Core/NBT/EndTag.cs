namespace SharpCraft.Core.NBT
{
    public class EndTag : Tag
    {
        public override void WriteContents(System.IO.BinaryWriter binaryWriter)
        {
        }

        public override void ReadContents(System.IO.BinaryReader binaryReader)
        {
        }

        public override byte GetNBTType()
        {
            return 0;
        }

        public override string ToString()
        {
            return "END";
        }
    }
}