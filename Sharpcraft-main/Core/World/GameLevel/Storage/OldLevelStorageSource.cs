using System.IO;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.Util;
using System.Collections.Generic;
using System;
using SharpCraft.Core.NBT;

namespace SharpCraft.Core.World.GameLevel.Storage
{
    public class OldLevelStorageSource : ILevelStorageSource
    {
        protected readonly JFile dir;
        public OldLevelStorageSource(JFile file1)
        {
            if (!file1.Exists())
            {
                //file1.Mkdirs();
                file1.Mkdir();
            }

            this.dir = file1;
        }

        public virtual string GetName()
        {
            return "Old Format";
        }

        public virtual IList<LevelSummary> GetLevelList()
        {
            List<LevelSummary> arrayList1 = new();
            for (int i2 = 0; i2 < 5; ++i2)
            {
                string string3 = "World" + (i2 + 1);
                LevelData worldInfo4 = this.GetTagDataFor(string3);
                if (worldInfo4 != null)
                {
                    arrayList1.Add(new LevelSummary(string3, "", worldInfo4.GetLastTimePlayed(), worldInfo4.GetSizeOnDisk(), false));
                }
            }

            return arrayList1;
        }

        public virtual void ClearAll()
        {
        }

        public virtual LevelData GetTagDataFor(string string1)
        {
            JFile file2 = new JFile(this.dir, string1);
            if (!file2.Exists())
            {
                return null;
            }
            else
            {
                JFile file3 = new JFile(file2, "level.dat");
                CompoundTag nBTTagCompound4;
                CompoundTag nBTTagCompound5;
                if (file3.Exists())
                {
                    try
                    {
                        nBTTagCompound4 = NbtIO.ReadCompressed(file3.GetReadStream());
                        nBTTagCompound5 = nBTTagCompound4.GetCompoundTag("Data");
                        return new LevelData(nBTTagCompound5);
                    }
                    catch (Exception exception7)
                    {
                        exception7.PrintStackTrace();
                    }
                }

                file3 = new JFile(file2, "level.dat_old");
                if (file3.Exists())
                {
                    try
                    {
                        nBTTagCompound4 = NbtIO.ReadCompressed(file3.GetReadStream());
                        nBTTagCompound5 = nBTTagCompound4.GetCompoundTag("Data");
                        return new LevelData(nBTTagCompound5);
                    }
                    catch (Exception exception6)
                    {
                        exception6.PrintStackTrace();
                    }
                }

                return null;
            }
        }

        public virtual void RenameLevel(string string1, string string2)
        {
            JFile file3 = new JFile(this.dir, string1);
            if (file3.Exists())
            {
                JFile file4 = new JFile(file3, "level.dat");
                if (file4.Exists())
                {
                    try
                    {
                        CompoundTag nBTTagCompound5 = NbtIO.ReadCompressed(file4.GetReadStream());
                        CompoundTag nBTTagCompound6 = nBTTagCompound5.GetCompoundTag("Data");
                        nBTTagCompound6.SetString("LevelName", string2);
                        NbtIO.WriteCompressed(nBTTagCompound5, file4.GetWriteStream());
                    }
                    catch (Exception exception7)
                    {
                        exception7.PrintStackTrace();
                    }
                }
            }
        }

        public virtual void DeleteLevel(string string1)
        {
            JFile file2 = new JFile(this.dir, string1);
            if (file2.Exists())
            {
                DeleteDir(file2.ListFiles());
                file2.Delete();
            }
        }

        protected static void DeleteDir(JFile[] files)
        {
            for (int i = 0; i < files.Length; ++i)
            {
                if (files[i].IsDirectory())
                {
                    DeleteDir(files[i].ListFiles());
                }

                files[i].Delete();
            }
        }

        public virtual ILevelStorage SelectLevel(string string1, bool z2)
        {
            return new DirectoryLevelStorage(this.dir, string1, z2);
        }

        public virtual bool RequiresConversion(string string1)
        {
            return false;
        }

        public virtual bool ConvertLevel(string string1, IProgressListener iProgressUpdate2)
        {
            return false;
        }
    }
}