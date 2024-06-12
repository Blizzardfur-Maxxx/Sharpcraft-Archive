using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Core.Util
{
    public class JRandomAccessFile : IDisposable
    {
        private JFile file;
        private FileStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        public JRandomAccessFile(JFile file, string mode) 
        {
            if (mode != "rw") throw new ArgumentException("Only rw mode is supported!");
            this.file = file;
            stream = File.Open(file.GetAbsolutePath(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }

        public long Length() => stream.Length;
        public void Seek(long pos) => stream.Seek(pos, SeekOrigin.Begin);
        public void Write(int val) => writer.Write((byte)val);
        public void WriteByte(sbyte val) => writer.Write(val);
        public void Write(byte[] val) => writer.WriteBytes(val);
        public void Write(byte[] val, int offset, int length) => writer.Write(val, offset, length);
        public void WriteInt(int val) => writer.WriteBEInt(val);
        public int ReadInt() => reader.ReadBEInt();
        public sbyte ReadByte() => reader.ReadSByte();
        public void Read(byte[] buf) => reader.Read(buf, 0, buf.Length);

        public void Dispose()
        {
            stream.Dispose();
            reader.Dispose();
            writer.Dispose();
        }
    }
}
