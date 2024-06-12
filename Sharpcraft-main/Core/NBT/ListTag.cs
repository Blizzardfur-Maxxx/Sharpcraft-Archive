using System.Collections;
using System.Collections.Generic;

namespace SharpCraft.Core.NBT
{
    public class ListTag<T> : Tag, IList<T> where T : Tag
    {
        private List<T> data = new List<T>();
        private byte tagType;

        public override void WriteContents(System.IO.BinaryWriter binaryWriter)
        {
            tagType = data.Count > 0 ? data[0].GetNBTType() : (byte)1;
            binaryWriter.Write(tagType);
            binaryWriter.WriteBEInt(data.Count);

            foreach (Tag nbt in data)
                nbt.WriteContents(binaryWriter);
        }

        public override void ReadContents(System.IO.BinaryReader binaryReader)
        {
            tagType = binaryReader.ReadByte();
            data = new List<T>();
            int itemsCount = binaryReader.ReadBEInt();

            for (int i = 0; i < itemsCount; i++)
            {
                Tag nbt = Create(tagType);
                nbt.ReadContents(binaryReader);
                data.Add((T)nbt);
            }
        }

        public override byte GetNBTType()
        {
            return 9;
        }

        public override string ToString()
        {
            return data.Count + " entries of type " + GetName(tagType);
        }

        public int IndexOf(T item)
        {
            return data.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            data.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            data.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return data[index];
            }
            set
            {
                data[index] = value;
            }
        }

        public void Add(T item)
        {
            tagType = item.GetNBTType();
            data.Add(item);
        }

        public void Clear()
        {
            data.Clear();
        }

        public bool Contains(T item)
        {
            return data.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            data.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return data.Remove(item);
        }

        public int Count
        {
            get
            {
                return data.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}