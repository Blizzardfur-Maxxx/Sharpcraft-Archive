using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpCraft.Client
{
    public class Buffer<T> : IDisposable where T : struct
    {
        private T[] array;
        private GCHandle handle;
        private int capacity;
        private int limit;
        private int position;
        private int mark = -1;

        public Buffer(int capacity) 
        {
            array = new T[capacity];
            handle = GCHandle.Alloc(array, GCHandleType.Pinned);
            this.capacity = capacity;
            limit = capacity;
            position = capacity;
        }

        public Buffer<T> Flip() 
        {
            limit = position;
            position = 0;
            mark = -1;
            return this;
        }

        public Buffer<T> Clear()
        {
            position = 0;
            limit = capacity;
            mark = -1;
            return this;
        }

        public Buffer<T> Mark() 
        {
            mark = position;
            return this;
        }

        public Buffer<T> Reset() 
        {
            if (mark == -1) throw new IndexOutOfRangeException("invalid mark");
            position = mark;

            return this;
        }

        public Buffer<T> Put(int index, T value) 
        {
            if (index < 0 || index >= limit) throw new IndexOutOfRangeException();
            array[index] = value;
            return this;
        }

        public Buffer<T> Put(T value)
        {
            if (position >= limit) throw new IndexOutOfRangeException("Buffer is full");
            array[position++] = value;
            return this;
        }

        public Buffer<T> Put(T[] src, int offset, int length)
        {
            if (length > Remaining()) throw new IndexOutOfRangeException("Buffer is full");
            if (offset < 0 || offset > src.Length) throw new IndexOutOfRangeException();
            if (length < 0 || length > src.Length + offset) throw new IndexOutOfRangeException();

            for (int i = offset; i < offset + length; i++)
            {
                Put(src[i]);
            }

            return this;
        }

        public Buffer<T> Put(T[] src)
        {
            if (src.Length > Remaining()) throw new IndexOutOfRangeException("Buffer is full");

            Put(src, 0, src.Length);

            return this;
        }

        public T Get(int index) => array[index];

        public T Get() 
        {
            return array[position++];
        }

        public int Capacity() => capacity;
        public int Limit() => limit;
        public int Position() => position;
        public int Remaining() => limit - position;

        public Buffer<T> Position(int pos) 
        {
            position = pos;
            return this;
        }

        public Buffer<T> Limit(int nlimit)
        {
            if (nlimit < 0 || limit > capacity) throw new IndexOutOfRangeException();

            limit = nlimit;
            if (mark > limit) mark = -1;

            return this;
        }

        public T[] ToArray() => array[position..limit];

        public void Dispose()
        {
            if (handle.IsAllocated) handle.Free();
            array = null;
            capacity = 0;
            position = 0;
            limit = 0;
            mark = -1;
        }

        public static int GetInt(Buffer<byte> buf, int index, bool bigEndian) 
        {
            if (index < 0 || index >= buf.limit - 3) throw new IndexOutOfRangeException();

            if (bigEndian)
                return (buf.Get(index + 0) << 24) | (buf.Get(index + 1) << 16) | (buf.Get(index + 2) << 8) | (buf.Get(index + 3) << 0);
            else
                return (buf.Get(index + 0) << 0) | (buf.Get(index + 1) << 8) | (buf.Get(index + 2) << 16) | (buf.Get(index + 3) << 24);
        }

        public static void PutInt(Buffer<byte> buf, int index, int value, bool bigEndian)
        {
            if (index < 0 || index >= buf.limit - 3) throw new IndexOutOfRangeException();

            byte[] data =
            [
                (byte)((value >> 24) & 0x000000FF),
                (byte)((value >> 16) & 0x000000FF),
                (byte)((value >> 8) & 0x000000FF),
                (byte)((value >> 0) & 0x000000FF),
            ];

            if (bigEndian)
            {
                buf.Put(index + 0, data[0]);
                buf.Put(index + 1, data[1]);
                buf.Put(index + 2, data[2]);
                buf.Put(index + 3, data[3]);
            }
            else
            {
                buf.Put(index + 0, data[3]);
                buf.Put(index + 1, data[2]);
                buf.Put(index + 2, data[1]);
                buf.Put(index + 3, data[0]);
            }
        }

        public static implicit operator nint(Buffer<T> buffer) 
            => new(buffer.handle.AddrOfPinnedObject().ToInt64() + buffer.position);
    }
}
