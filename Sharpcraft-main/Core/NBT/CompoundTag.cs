using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SharpCraft.Core.NBT
{
    public class CompoundTag : Tag
    {
        private NullDictionary<string, Tag> data = new NullDictionary<string, Tag>();

        public override void WriteContents(BinaryWriter dataOutput)
        {
            foreach (var nbt in data.Values)
                Write(nbt, dataOutput);
            dataOutput.Write((byte)0);
        }

        public override void ReadContents(BinaryReader dataInput)
        {
            data.Clear();

            Tag nbt;
            while ((nbt = Read(dataInput)).GetNBTType() != 0)
                data[nbt.GetKey()] = nbt;
        }

        public ICollection<Tag> GetData()
        {
            return data.Values;
        }

        public override byte GetNBTType()
        {
            return 10;
        }

        public void SetTag(string key, Tag value)
        {
            data[key] = value.SetKey(key);
        }

        public void SetByte(string key, byte value)
        {
            data[key] = new ByteTag(value).SetKey(key);
        }

        public void SetShort(string key, short value)
        {
            data[key] = new ShortTag(value).SetKey(key);
        }

        public void SetInteger(string key, int value)
        {
            data[key] = new IntTag(value).SetKey(key);
        }

        public void SetLong(string key, long value)
        {
            data[key] = new LongTag(value).SetKey(key);
        }

        public void SetFloat(string key, float value)
        {
            data[key] = new FloatTag(value).SetKey(key);
        }

        public void SetDouble(string key, double value)
        {
            data[key] = new DoubleTag(value).SetKey(key);
        }

        public void SetString(string key, string value)
        {
            data[key] = new StringTag(value).SetKey(key);
        }

        public void SetByteArray(string key, byte[] value)
        {
            data[key] = new ByteArrayTag(value).SetKey(key);
        }

        public void SetCompoundTag(string key, CompoundTag value)
        {
            data[key] = value.SetKey(key);
        }

        public void SetBoolean(string key, bool value)
        {
            SetByte(key, (byte)(value ? 1 : 0));
        }

        public bool HasKey(string key)
        {
            return data.ContainsKey(key);
        }

        public byte GetByte(string key)
        {
            return !data.ContainsKey(key) ? (byte)0 : ((ByteTag)data[key]).Value;
        }

        public short GetShort(string key)
        {
            return !data.ContainsKey(key) ? (short)0 : ((ShortTag)data[key]).Value;
        }

        public int GetInteger(string key)
        {
            return !data.ContainsKey(key) ? 0 : ((IntTag)data[key]).Value;
        }

        public long GetLong(string key)
        {
            return !data.ContainsKey(key) ? 0L : ((LongTag)data[key]).Value;
        }

        public float GetFloat(string key)
        {
            return !data.ContainsKey(key) ? 0.0F : ((FloatTag)data[key]).Value;
        }

        public double GetDouble(string key)
        {
            return !data.ContainsKey(key) ? 0.0D : ((DoubleTag)data[key]).Value;
        }

        public string GetString(string key)
        {
            return !data.ContainsKey(key) ? "" : ((StringTag)data[key]).Value;
        }

        public byte[] GetByteArray(string key)
        {
            return !data.ContainsKey(key) ? new byte[0] : ((ByteArrayTag)data[key]).Value;
        }

        public CompoundTag GetCompoundTag(string key)
        {
            return !data.ContainsKey(key) ? new CompoundTag() : (CompoundTag)data[key];
        }

        public ListTag<T> GetTagList<T>(string key) where T : Tag
        {
            if (!data.ContainsKey(key)) return new ListTag<T>();
            
            Tag ttag = data[key];
            if (ttag is ListTag<Tag> tttag) 
            {
                ListTag<T> listConverted = new ListTag<T>();
                foreach (Tag tag in tttag)
                {
                    if (tag.GetType() != typeof(T))
                        throw new ArgumentException("Invalid generics!");
                    listConverted.Add((T)tag);
                }

                return listConverted;
            }
            return (ListTag<T>)ttag;
            
        }

        public bool GetBoolean(string key)
        {
            return GetByte(key) != 0;
        }

        public IEnumerable<Tag> Values
        {
            get
            {
                return data.Values;
            }
        }

        public override string ToString()
        {
            return data.Count + " entries";
        }
    }
}