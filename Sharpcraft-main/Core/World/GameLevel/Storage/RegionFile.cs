using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.Storage
{
    public class RegionFile
    {
        private static readonly byte[] emptySector = new byte[4096];
        private readonly JFile fileName;
        private JRandomAccessFile dataFile;
        private readonly int[] offsets = new int[1024];
        private readonly int[] chunkTimestamps = new int[1024];
        private List<bool> sectorFree;
        private int sizeDelta;
        private long lastModified = 0;

        public RegionFile(JFile file1)
        {
            this.fileName = file1;
            this.Debugln("REGION LOAD " + this.fileName);
            this.sizeDelta = 0;
            try
            {
                if (file1.Exists())
                {
                    this.lastModified = file1.LastModified();
                }

                this.dataFile = new JRandomAccessFile(file1, "rw");
                int i2;
                if (this.dataFile.Length() < 4096)
                {
                    for (i2 = 0; i2 < 1024; ++i2)
                    {
                        this.dataFile.WriteInt(0);
                    }

                    for (i2 = 0; i2 < 1024; ++i2)
                    {
                        this.dataFile.WriteInt(0);
                    }

                    this.sizeDelta += 8192;
                }

                if ((this.dataFile.Length() & 4095) != 0)
                {
                    for (i2 = 0; i2 < (this.dataFile.Length() & 4095); ++i2)
                    {
                        this.dataFile.Write(0);
                    }
                }

                i2 = (int)this.dataFile.Length() / 4096;
                this.sectorFree = new(i2);
                int i3;
                for (i3 = 0; i3 < i2; ++i3)
                {
                    this.sectorFree.Add(true);
                }

                this.sectorFree[0] = false;
                this.sectorFree[1] = false;
                this.dataFile.Seek(0);
                int i4;
                for (i3 = 0; i3 < 1024; ++i3)
                {
                    i4 = this.dataFile.ReadInt();
                    this.offsets[i3] = i4;
                    if (i4 != 0 && (i4 >> 8) + (i4 & 255) <= this.sectorFree.Count)
                    {
                        for (int i5 = 0; i5 < (i4 & 255); ++i5)
                        {
                            this.sectorFree[(i4 >> 8) + i5] = false;
                        }
                    }
                }

                for (i3 = 0; i3 < 1024; ++i3)
                {
                    i4 = this.dataFile.ReadInt();
                    this.chunkTimestamps[i3] = i4;
                }
            }
            catch (IOException iOException6)
            {
                iOException6.PrintStackTrace();
            }
        }

        public virtual int GetSizeDelta()
        {
            lock (this)
            {
                int i1 = this.sizeDelta;
                this.sizeDelta = 0;
                return i1;
            }
        }

        private void Debug(string string1)
        {
        }

        private void Debugln(string string1)
        {
            this.Debug(string1 + "\n");
        }

        private void Debug(string string1, int i2, int i3, string string4)
        {
            this.Debug("REGION " + string1 + " " + this.fileName.GetName() + "[" + i2 + "," + i3 + "] = " + string4);
        }

        private void Debug(string string1, int i2, int i3, int i4, string string5)
        {
            this.Debug("REGION " + string1 + " " + this.fileName.GetName() + "[" + i2 + "," + i3 + "] " + i4 + "B = " + string5);
        }

        private void Debugln(string string1, int i2, int i3, string string4)
        {
            this.Debug(string1, i2, i3, string4 + "\n");
        }

        public virtual BinaryReader GetChunkDataInputStream(int i1, int i2)
        {
            lock (this)
            {
                if (this.OutOfBounds(i1, i2))
                {
                    this.Debugln("READ", i1, i2, "out of bounds");
                    return null;
                }
                else
                {
                    try
                    {
                        int i3 = this.GetOffset(i1, i2);
                        if (i3 == 0)
                        {
                            return null;
                        }
                        else
                        {
                            int i4 = i3 >> 8;
                            int i5 = i3 & 255;
                            if (i4 + i5 > this.sectorFree.Count)
                            {
                                this.Debugln("READ", i1, i2, "invalid sector");
                                return null;
                            }
                            else
                            {
                                this.dataFile.Seek(i4 * 4096);
                                int i6 = this.dataFile.ReadInt();
                                if (i6 > 4096 * i5)
                                {
                                    this.Debugln("READ", i1, i2, "invalid length: " + i6 + " > 4096 * " + i5);
                                    return null;
                                }
                                else
                                {
                                    sbyte b7 = this.dataFile.ReadByte();
                                    byte[] b8;
                                    BinaryReader dataInputStream9;
                                    if (b7 == 1)
                                    {
                                        b8 = new byte[i6 - 1];
                                        this.dataFile.Read(b8);
                                        dataInputStream9 = new BinaryReader(new GZipStream(new MemoryStream(b8), CompressionMode.Decompress));
                                        return dataInputStream9;
                                    }
                                    else if (b7 == 2)
                                    {
                                        b8 = new byte[i6 - 1];
                                        this.dataFile.Read(b8);
                                        dataInputStream9 = new BinaryReader(new ZLibStream(new MemoryStream(b8), CompressionMode.Decompress));
                                        return dataInputStream9;
                                    }
                                    else
                                    {
                                        this.Debugln("READ", i1, i2, "unknown version " + b7);
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                    catch (IOException)
                    {
                        this.Debugln("READ", i1, i2, "exception");
                        return null;
                    }
                }
            }
        }

        public virtual BinaryWriter GetChunkDataOutputStream(int i1, int i2)
        {
            return this.OutOfBounds(i1, i2) ? null : new BinaryWriter(
                new ZLibStream(new ChunkBuffer(this, i1, i2), CompressionMode.Compress));
        }

        protected virtual void Write(int i1, int i2, byte[] b3, int i4)
        {
            lock (this)
            {
                try
                {
                    int i5 = this.GetOffset(i1, i2);
                    int i6 = i5 >> 8;
                    int i7 = i5 & 255;
                    int i8 = (i4 + 5) / 4096 + 1;
                    if (i8 >= 256)
                    {
                        return;
                    }

                    if (i6 != 0 && i7 == i8)
                    {
                        this.Debug("SAVE", i1, i2, i4, "rewrite");
                        this.Write(i6, b3, i4);
                    }
                    else
                    {
                        int i9;
                        for (i9 = 0; i9 < i7; ++i9)
                        {
                            this.sectorFree[i6 + i9] = true;
                        }

                        i9 = this.sectorFree.IndexOf(true);
                        int i10 = 0;
                        int i11;
                        if (i9 != -1)
                        {
                            for (i11 = i9; i11 < this.sectorFree.Count; ++i11)
                            {
                                if (i10 != 0)
                                {
                                    if (this.sectorFree[i11])
                                    {
                                        ++i10;
                                    }
                                    else
                                    {
                                        i10 = 0;
                                    }
                                }
                                else if (this.sectorFree[i11])
                                {
                                    i9 = i11;
                                    i10 = 1;
                                }

                                if (i10 >= i8)
                                {
                                    break;
                                }
                            }
                        }

                        if (i10 >= i8)
                        {
                            this.Debug("SAVE", i1, i2, i4, "reuse");
                            i6 = i9;
                            this.SetOffset(i1, i2, i9 << 8 | i8);
                            for (i11 = 0; i11 < i8; ++i11)
                            {
                                this.sectorFree[i6 + i11] = false;
                            }

                            this.Write(i6, b3, i4);
                        }
                        else
                        {
                            this.Debug("SAVE", i1, i2, i4, "grow");
                            this.dataFile.Seek(this.dataFile.Length());
                            i6 = this.sectorFree.Count;
                            for (i11 = 0; i11 < i8; ++i11)
                            {
                                this.dataFile.Write(emptySector);
                                this.sectorFree.Add(false);
                            }

                            this.sizeDelta += 4096 * i8;
                            this.Write(i6, b3, i4);
                            this.SetOffset(i1, i2, i6 << 8 | i8);
                        }
                    }

                    this.SetChunkTimestamp(i1, i2, (int)(TimeUtil.MilliTime / 1000));
                }
                catch (IOException iOException12)
                {
                    iOException12.PrintStackTrace();
                }
            }
        }

        private void Write(int i1, byte[] b2, int i3)
        {
            this.Debugln(" " + i1);
            this.dataFile.Seek(i1 * 4096);
            this.dataFile.WriteInt(i3 + 1);
            this.dataFile.WriteByte(2);
            this.dataFile.Write(b2, 0, i3);
        }

        private bool OutOfBounds(int i1, int i2)
        {
            return i1 < 0 || i1 >= 32 || i2 < 0 || i2 >= 32;
        }

        private int GetOffset(int i1, int i2)
        {
            return this.offsets[i1 + i2 * 32];
        }

        public virtual bool IsChunkSaved(int i1, int i2)
        {
            return this.GetOffset(i1, i2) != 0;
        }

        private void SetOffset(int i1, int i2, int i3)
        {
            this.offsets[i1 + i2 * 32] = i3;
            this.dataFile.Seek((i1 + i2 * 32) * 4);
            this.dataFile.WriteInt(i3);
        }

        private void SetChunkTimestamp(int i1, int i2, int i3)
        {
            this.chunkTimestamps[i1 + i2 * 32] = i3;
            this.dataFile.Seek(4096 + (i1 + i2 * 32) * 4);
            this.dataFile.WriteInt(i3);
        }

        public virtual void Dispose()
        {
            this.dataFile.Dispose();
        }

        private class ChunkBuffer : MemoryStream
        {
            private RegionFile rf;
            private int x;
            private int z;

            public ChunkBuffer(RegionFile rf, int x, int z) : base(8096)
            {
                this.rf = rf;
                this.x = x;
                this.z = z;
            }

            public override void Close()
            {
                byte[] buf = ToArray();
                rf.Write(this.x, this.z, buf, buf.Length);
            }
        }
    }
}