using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core.Util
{
    //now this wasnt hard to port was it
    public class LongHashMap<V> where V : class
    {
        private Entry[] hashArray = new Entry[16];
        private int numHashElements;
        private int capacity = 12;
        private readonly float percentUsable = 0.75F;
        private static int GetHashedKey(long j0)
        {
            return Hash((int)(j0 ^ j0 >>> 32));
        }

        //use unchecked if it complains when downgrading the version
        private static int Hash(int i0)
        {
            i0 ^= i0 >>> 20 ^ i0 >>> 12;
            return i0 ^ i0 >>> 7 ^ i0 >>> 4;
        }

        private static int GetHashIndex(int i0, int i1)
        {
            return i0 & i1 - 1;
        }

        public virtual bool Contains(long j1)
        {
            return this.Get(j1) != null;
        }

        public virtual V Get(long j1)
        {
            int i3 = GetHashedKey(j1);
            for (Entry entry4 = this.hashArray[GetHashIndex(i3, this.hashArray.Length)]; entry4 != null; entry4 = entry4.nextEntry)
            {
                if (entry4.key == j1)
                {
                    return entry4.value;
                }
            }

            return null;
        }

        public virtual void Put(long j1, V object3)
        {
            int i4 = GetHashedKey(j1);
            int i5 = GetHashIndex(i4, this.hashArray.Length);
            for (Entry entry6 = this.hashArray[i5]; entry6 != null; entry6 = entry6.nextEntry)
            {
                if (entry6.key == j1)
                {
                    entry6.value = object3;
                }
            }

            this.CreateKey(i4, j1, object3, i5);
        }

        private void ResizeTable(int i1)
        {
            Entry[] entry2 = this.hashArray;
            int i3 = entry2.Length;
            if (i3 == 1073741824)
            {
                this.capacity = int.MaxValue;
            }
            else
            {
                Entry[] entry4 = new Entry[i1];
                this.CopyHashTableTo(entry4);
                this.hashArray = entry4;
                this.capacity = (int)(i1 * this.percentUsable);
            }
        }

        private void CopyHashTableTo(Entry[] entry1)
        {
            Entry[] entry2 = this.hashArray;
            int i3 = entry1.Length;
            for (int i4 = 0; i4 < entry2.Length; ++i4)
            {
                Entry entry5 = entry2[i4];
                if (entry5 != null)
                {
                    entry2[i4] = null;
                    Entry entry6;
                    do
                    {
                        entry6 = entry5.nextEntry;
                        int i7 = GetHashIndex(entry5.slotHash, i3);
                        entry5.nextEntry = entry1[i7];
                        entry1[i7] = entry5;
                        entry5 = entry6;
                    }
                    while (entry6 != null);
                }
            }
        }

        public virtual V Remove(long j1)
        {
            Entry entry3 = this.RemoveKey(j1);
            return entry3 == null ? null : entry3.value;
        }

        Entry RemoveKey(long j1)
        {
            int i3 = GetHashedKey(j1);
            int i4 = GetHashIndex(i3, this.hashArray.Length);
            Entry entry5 = this.hashArray[i4];
            Entry entry6;
            Entry entry7;
            for (entry6 = entry5; entry6 != null; entry6 = entry7)
            {
                entry7 = entry6.nextEntry;
                if (entry6.key == j1)
                {
                    --this.numHashElements;
                    if (entry5 == entry6)
                    {
                        this.hashArray[i4] = entry7;
                    }
                    else
                    {
                        entry5.nextEntry = entry7;
                    }

                    return entry6;
                }

                entry5 = entry6;
            }

            return entry6;
        }

        private void CreateKey(int i1, long j2, V object4, int i5)
        {
            Entry entry6 = this.hashArray[i5];
            this.hashArray[i5] = new Entry(i1, j2, object4, entry6);
            if (this.numHashElements++ >= this.capacity)
            {
                this.ResizeTable(2 * this.hashArray.Length);
            }
        }

        internal class Entry
        {
            internal readonly long key;
            internal V value;
            internal Entry nextEntry;
            internal readonly int slotHash;

            internal Entry(int i1, long j2, V object4, Entry playerHashEntry5)
            {
                this.value = object4;
                this.nextEntry = playerHashEntry5;
                this.key = j2;
                this.slotHash = i1;
            }

            public long GetKey()
            {
                return this.key;
            }

            public V GetValue()
            {
                return this.value;
            }

            public override bool Equals(object obj)
            {
                if (!(obj is Entry))
                {
                    return false;
                }
                else
                {
                    Entry that = (Entry)obj;
                    long tkey = this.GetKey();
                    long okey = that.GetKey();
                    if (tkey == okey)
                    {
                        object tval = this.GetValue();
                        object oval = that.GetValue();
                        if (tval == oval || tval != null && tval.Equals(oval))
                        {
                            return true;
                        }
                    }

                    return false;
                }
            }

            public override int GetHashCode()
            {
                return GetHashedKey(this.key);
            }

            public override string ToString()
            {
                return this.GetKey() + "=" + this.GetValue();
            }
        }

        public virtual int Size()
        {
            return this.numHashElements;
        }
    }
}
