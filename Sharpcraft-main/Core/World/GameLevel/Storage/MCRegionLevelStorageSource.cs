using System.IO;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.Util;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.IO.Compression;

namespace SharpCraft.Core.World.GameLevel.Storage
{
    public partial class MCRegionLevelStorageSource : OldLevelStorageSource
    {
        private partial class FolderFilter
        {
            [GeneratedRegex("[0-9a-z]|([0-9a-z][0-9a-z])")]
            private static partial Regex matcher();

            public bool Accept(JFile file1)
            {
                if (file1.IsDirectory())
                {
                    return matcher().IsMatch(file1.GetName());
                }
                else
                {
                    return false;
                }
            }
        }

        private partial class NameFilter
        {
            [GeneratedRegex("c\\.(-?[0-9a-z]+)\\.(-?[0-9a-z]+)\\.dat")]
            internal static partial Regex matcher();

            public bool Accept(string string2)
            {
                return matcher().IsMatch(string2);
            }
        }

        private class OldChunk : IComparable<OldChunk>
        {
            private readonly JFile file;
            private readonly int xpos;
            private readonly int zpos;

            public OldChunk(JFile file1)
            {
                this.file = file1;
                Match matcher2 = NameFilter.matcher().Match(file1.GetName());

                if (matcher2.Success)
                {
                    this.xpos = (int)Base36.FromString(matcher2.Captures[0].Value, 36);
                    this.zpos = (int)Base36.FromString(matcher2.Captures[1].Value, 36);
                }
                else
                {
                    this.xpos = 0;
                    this.zpos = 0;
                }
            }

            public virtual int CompareTo(OldChunk chunkFile1)
            {
                int i2 = this.xpos >> 5;
                int i3 = chunkFile1.xpos >> 5;
                if (i2 == i3)
                {
                    int i4 = this.zpos >> 5;
                    int i5 = chunkFile1.zpos >> 5;
                    return i4 - i5;
                }
                else
                {
                    return i2 - i3;
                }
            }

            public virtual JFile GetFile()
            {
                return this.file;
            }

            public virtual int GetX()
            {
                return this.xpos;
            }

            public virtual int GetZ()
            {
                return this.zpos;
            }
        }

        public MCRegionLevelStorageSource(JFile file1) : base(file1)
        {
        }

        public override string GetName()
        {
            return "Scaevolus' McRegion";
        }

        public override IList<LevelSummary> GetLevelList()
        {
            List<LevelSummary> arrayList1 = new();
            JFile[] file2 = this.dir.ListFiles();
            JFile[] file3 = file2;
            int i4 = file2.Length;
            for (int i5 = 0; i5 < i4; ++i5)
            {
                JFile file6 = file3[i5];
                if (file6.IsDirectory())
                {
                    string string7 = file6.GetName();
                    LevelData worldInfo8 = this.GetTagDataFor(string7);
                    if (worldInfo8 != null)
                    {
                        bool z9 = worldInfo8.GetSaveVersion() != 19132;
                        string string10 = worldInfo8.GetLevelName();
                        if (string10 == null || string.IsNullOrEmpty(string10))
                        {
                            string10 = string7;
                        }

                        arrayList1.Add(new LevelSummary(string7, string10, worldInfo8.GetLastTimePlayed(), worldInfo8.GetSizeOnDisk(), z9));
                    }
                }
            }

            return arrayList1;
        }

        public override void ClearAll()
        {
            RegionFileCache.Clear();
        }

        public override ILevelStorage SelectLevel(string string1, bool z2)
        {
            return new MCRegionLevelStorage(this.dir, string1, z2);
        }

        public override bool RequiresConversion(string string1)
        {
            LevelData worldInfo2 = this.GetTagDataFor(string1);
            return worldInfo2 != null && worldInfo2.GetSaveVersion() == 0;
        }

        public override bool ConvertLevel(string string1, IProgressListener progress)
        {
            progress.SetLoadingProgress(0);
            List<OldChunk> arrayList3 = new();
            List<JFile> arrayList4 = new();
            List<OldChunk> arrayList5 = new();
            List<JFile> arrayList6 = new();
            JFile currentDir = new JFile(this.dir, string1);
            JFile dim_1 = new JFile(currentDir, "DIM-1");
            Console.WriteLine("Scanning folders...");
            this.ScanDirectory(currentDir, arrayList3, arrayList4);
            if (dim_1.Exists())
            {
                this.ScanDirectory(dim_1, arrayList5, arrayList6);
            }

            int convCount = arrayList3.Count + arrayList5.Count + arrayList4.Count + arrayList6.Count;
            Console.WriteLine("Total conversion count is " + convCount);
            this.ConvertChunks(currentDir, arrayList3, 0, convCount, progress);
            this.ConvertChunks(dim_1, arrayList5, arrayList3.Count, convCount, progress);
            LevelData worldInfo10 = this.GetTagDataFor(string1);
            worldInfo10.SetSaveVersion(19132);
            ILevelStorage iSaveHandler11 = this.SelectLevel(string1, false);
            iSaveHandler11.SaveLevelData(worldInfo10);
            this.DeleteFiles(arrayList4, arrayList3.Count + arrayList5.Count, convCount, progress);
            if (dim_1.Exists())
            {
                this.DeleteFiles(arrayList6, arrayList3.Count + arrayList5.Count + arrayList4.Count, convCount, progress);
            }

            return true;
        }

        private void ScanDirectory(JFile file1, IList<OldChunk> arrayList2, IList<JFile> arrayList3)
        {
            FolderFilter chunkFolderPattern4 = new FolderFilter();
            NameFilter chunkFilePattern5 = new NameFilter();
            JFile[] file6 = file1.ListFiles(chunkFolderPattern4.Accept);
            JFile[] file7 = file6;
            int i8 = file6.Length;
            for (int i9 = 0; i9 < i8; ++i9)
            {
                JFile file10 = file7[i9];
                arrayList3.Add(file10);
                JFile[] file11 = file10.ListFiles(chunkFolderPattern4.Accept);
                JFile[] file12 = file11;
                int i13 = file11.Length;
                for (int i14 = 0; i14 < i13; ++i14)
                {
                    JFile file15 = file12[i14];
                    JFile[] file16 = file15.ListFiles(chunkFilePattern5.Accept);
                    JFile[] file17 = file16;
                    int i18 = file16.Length;
                    for (int i19 = 0; i19 < i18; ++i19)
                    {
                        JFile file20 = file17[i19];
                        arrayList2.Add(new OldChunk(file20));
                    }
                }
            }
        }

        private void ConvertChunks(JFile file1, List<OldChunk> arrayList2, int i3, int i4, IProgressListener iProgressUpdate5)
        {
            arrayList2.Sort();
            byte[] b6 = new byte[4096];
            foreach (OldChunk chunkFile8 in arrayList2)
            {
                int i9 = chunkFile8.GetX();
                int i10 = chunkFile8.GetZ();
                RegionFile regionFile11 = RegionFileCache.GetRegionFile(file1, i9, i10);
                if (!regionFile11.IsChunkSaved(i9 & 31, i10 & 31))
                {
                    try
                    {
                        BinaryReader dataInputStream12 = new BinaryReader(new GZipStream(chunkFile8.GetFile().GetWriteStream(), CompressionMode.Decompress));
                        BinaryWriter dataOutputStream13 = regionFile11.GetChunkDataOutputStream(i9 & 31, i10 & 31);
                        int i17;
                        while ((i17 = dataInputStream12.Read(b6)) != -1)
                        {
                            dataOutputStream13.Write(b6, 0, i17);
                        }

                        dataOutputStream13.Dispose();
                        dataInputStream12.Dispose();
                    }
                    catch (IOException iOException15)
                    {
                        iOException15.PrintStackTrace();
                    }
                }

                ++i3;
                int i16 = (int)Math.Round(100.0D * i3 / i4);
                iProgressUpdate5.SetLoadingProgress(i16);
            }

            RegionFileCache.Clear();
        }

        private void DeleteFiles(IList<JFile> arrayList1, int size, int convertCount, IProgressListener listener)
        {
            foreach (JFile file in arrayList1)
            {
                JFile[] file7 = file.ListFiles();
                DeleteDir(file7);
                file.Delete();
                ++size;
                int progress = (int)Math.Round(100.0D * size / convertCount);
                listener.SetLoadingProgress(progress);
            }
        }
    }
}