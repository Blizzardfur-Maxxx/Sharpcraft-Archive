using System.IO;

namespace SharpCraft.Core.NBT
{
    public abstract class Tag
    {
        public enum TagType : byte
        {
            TAG_End = 0,
            TAG_Byte = 1,
            TAG_Short = 2,
            TAG_Int = 3,
            TAG_Long = 4,
            TAG_Float = 5,
            TAG_Double = 6,
            TAG_Byte_Array = 7,
            TAG_String = 8,
            TAG_List = 9,
            TAG_Compound = 10,
            //unimplemented in this version of nbt
            //TAG_Int_Array = 11
        }

        private string key;

        public string GetKey()
        {
            return key ?? "";
        }

        public Tag SetKey(string key)
        {
            this.key = key;
            return this;
        }

        public static Tag Read(BinaryReader binaryReader)
        {
            byte id = binaryReader.ReadByte();

            if (id == 0)
                return new EndTag();

            Tag nbt = Create(id);
            nbt.key = binaryReader.ReadMUTF8();
            nbt.ReadContents(binaryReader);

            return nbt;
        }

        public static void Write(Tag nbt, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(nbt.GetNBTType());

            if (nbt.GetNBTType() == 0)
                return;

            binaryWriter.WriteMUTF8(nbt.GetKey());
            nbt.WriteContents(binaryWriter);
        }

        public abstract void WriteContents(BinaryWriter binaryWriter);

        public abstract void ReadContents(BinaryReader binaryReader);

        public abstract byte GetNBTType();

        public static Tag Create(byte id)
        {
            switch (id)
            {
                case 0:
                    return new EndTag();
                case 1:
                    return new ByteTag();
                case 2:
                    return new ShortTag();
                case 3:
                    return new IntTag();
                case 4:
                    return new LongTag();
                case 5:
                    return new FloatTag();
                case 6:
                    return new DoubleTag();
                case 7:
                    return new ByteArrayTag();
                case 8:
                    return new StringTag();
                case 9:
                    return new ListTag<Tag>();
                case 10:
                    return new CompoundTag();
                default:
                    return null;
            }
        }

        public static string GetName(byte id)
        {
            switch (id)
            {
                case 0:
                    return "TAG_End";
                case 1:
                    return "TAG_Byte";
                case 2:
                    return "TAG_Short";
                case 3:
                    return "TAG_Int";
                case 4:
                    return "TAG_Long";
                case 5:
                    return "TAG_Float";
                case 6:
                    return "TAG_Double";
                case 7:
                    return "TAG_Byte_Array";
                case 8:
                    return "TAG_String";
                case 9:
                    return "TAG_List";
                case 10:
                    return "TAG_Compound";
                default:
                    return "UNKNOWN";
            }
        }
    }
}