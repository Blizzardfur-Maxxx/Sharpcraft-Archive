using SharpCraft.Core.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core.Util
{
    //now this wasnt hard to port was it
    public class IntHashMap<V> where V : class
    {
        internal Entry[] slots = new Entry[16];
        private int count;
        private int threshold = 12;
        private readonly float growFactor = 0.75F;

        private static int ComputeHash(int i0)
        {
            //use unchecked if it complains when downgrading the version
            i0 ^= i0 >>> 20 ^ i0 >>> 12;
            return i0 ^ i0 >>> 7 ^ i0 >>> 4;
        }

        private static int GetSlotIndex(int i0, int i1)
        {
            return i0 & i1 - 1;
        }

        public virtual V Get(int i1)
        {
            int i2 = ComputeHash(i1);
            for (Entry mCHashEntry3 = this.slots[GetSlotIndex(i2, this.slots.Length)]; mCHashEntry3 != null; mCHashEntry3 = mCHashEntry3.nextEntry)
            {
                if (mCHashEntry3.key == i1)
                {
                    return mCHashEntry3.value;
                }
            }

            return null;
        }

        public virtual bool ContainsKey(int i1)
        {
            return this.LookupEntry(i1) != null;
        }

        Entry LookupEntry(int i1)
        {
            int i2 = ComputeHash(i1);
            for (Entry mCHashEntry3 = this.slots[GetSlotIndex(i2, this.slots.Length)]; mCHashEntry3 != null; mCHashEntry3 = mCHashEntry3.nextEntry)
            {
                if (mCHashEntry3.key == i1)
                {
                    return mCHashEntry3;
                }
            }

            return null;
        }

        public virtual void Put(int i1, V object2)
        {
            int i3 = ComputeHash(i1);
            int i4 = GetSlotIndex(i3, this.slots.Length);
            for (Entry mCHashEntry5 = this.slots[i4]; mCHashEntry5 != null; mCHashEntry5 = mCHashEntry5.nextEntry)
            {
                if (mCHashEntry5.key == i1)
                {
                    mCHashEntry5.value = object2;
                }
            }

            this.Insert(i3, i1, object2, i4);
        }

        private void Grow(int i1)
        {
            Entry[] mCHashEntry2 = this.slots;
            int i3 = mCHashEntry2.Length;
            if (i3 == 1073741824)
            {
                this.threshold = int.MaxValue;
            }
            else
            {
                Entry[] mCHashEntry4 = new Entry[i1];
                this.CopyTo(mCHashEntry4);
                this.slots = mCHashEntry4;
                this.threshold = (int)(i1 * this.growFactor);
            }
        }

        private void CopyTo(Entry[] mCHashEntry1)
        {
            Entry[] mCHashEntry2 = this.slots;
            int i3 = mCHashEntry1.Length;
            for (int i4 = 0; i4 < mCHashEntry2.Length; ++i4)
            {
                Entry mCHashEntry5 = mCHashEntry2[i4];
                if (mCHashEntry5 != null)
                {
                    mCHashEntry2[i4] = null;
                    Entry mCHashEntry6;
                    do
                    {
                        mCHashEntry6 = mCHashEntry5.nextEntry;
                        int i7 = GetSlotIndex(mCHashEntry5.slotHash, i3);
                        mCHashEntry5.nextEntry = mCHashEntry1[i7];
                        mCHashEntry1[i7] = mCHashEntry5;
                        mCHashEntry5 = mCHashEntry6;
                    }
                    while (mCHashEntry6 != null);
                }
            }
        }

        public virtual V Remove(int i1)
        {
            Entry mCHashEntry2 = this.RemoveEntry(i1);
            return mCHashEntry2 == null ? null : mCHashEntry2.value;
        }

        Entry RemoveEntry(int i1)
        {
            int i2 = ComputeHash(i1);
            int i3 = GetSlotIndex(i2, this.slots.Length);
            Entry mCHashEntry4 = this.slots[i3];
            Entry mCHashEntry5;
            Entry mCHashEntry6;
            for (mCHashEntry5 = mCHashEntry4; mCHashEntry5 != null; mCHashEntry5 = mCHashEntry6)
            {
                mCHashEntry6 = mCHashEntry5.nextEntry;
                if (mCHashEntry5.key == i1)
                {
                    --this.count;
                    if (mCHashEntry4 == mCHashEntry5)
                    {
                        this.slots[i3] = mCHashEntry6;
                    }
                    else
                    {
                        mCHashEntry4.nextEntry = mCHashEntry6;
                    }

                    return mCHashEntry5;
                }

                mCHashEntry4 = mCHashEntry5;
            }

            return mCHashEntry5;
        }

        public virtual void Clear()
        {
            Entry[] mCHashEntry1 = this.slots;
            for (int i2 = 0; i2 < mCHashEntry1.Length; ++i2)
            {
                mCHashEntry1[i2] = null;
            }

            this.count = 0;
        }

        private void Insert(int i1, int i2, V object3, int i4)
        {
            Entry mCHashEntry5 = this.slots[i4];
            this.slots[i4] = new Entry(i1, i2, object3, mCHashEntry5);
            if (this.count++ >= this.threshold)
            {
                this.Grow(2 * this.slots.Length);
            }
        }

        internal class Entry
        {
            internal readonly int key;
            internal V value;
            internal Entry nextEntry;
            internal readonly int slotHash;
            internal Entry(int i1, int i2, V object3, Entry mCHashEntry4)
            {
                this.value = object3;
                this.nextEntry = mCHashEntry4;
                this.key = i2;
                this.slotHash = i1;
            }

            public int GetKey()
            {
                return this.key;
            }

            public object GetValue()
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
                    int tkey = this.GetKey();
                    int okey = that.GetKey();
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
                return ComputeHash(this.key);
            }

            public override string ToString()
            {
                return this.GetKey() + "=" + this.GetValue();
            }
        }

        public int Size() 
        {
            return this.count;
        }
    }
}
