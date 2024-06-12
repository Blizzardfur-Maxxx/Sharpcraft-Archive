using System.Collections.Generic;
using System.IO;
using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.Storage
{
    public class RegionFileCache
    {
        private static readonly NullDictionary<JFile, SoftReference<RegionFile>> cache = new();
        public static RegionFile GetRegionFile(JFile saveDir, int x, int z)
        {
            lock (typeof(RegionFileCache))
            {
                JFile regionfolder = new JFile(saveDir, "region");
                JFile file = new JFile(regionfolder, "r." + (x >> 5) + "." + (z >> 5) + ".mcr");
                SoftReference<RegionFile> refr = cache[file];
                RegionFile regfile;

                if (refr != null)
                {
                    regfile = refr.Target;
                    if (regfile != null)
                    {
                        return regfile;
                    }
                }

                if (!regionfolder.Exists())
                {
                    //regionfolder.Mkdirs();
                    regionfolder.Mkdir();
                }

                if (cache.Count >= 256)
                {
                    Clear();
                }

                regfile = new RegionFile(file);
                cache[file] = new SoftReference<RegionFile>(regfile);
                return regfile;
            }
        }

        public static void Clear()
        {
            lock (typeof(RegionFileCache))
            {
                foreach (SoftReference<RegionFile> refr in cache.Values)
                {
                    try
                    {
                        RegionFile regfile = refr.Target;
                        if (regfile != null)
                        {
                            regfile.Dispose();
                        }
                    }
                    catch (IOException ioe)
                    {
                        ioe.PrintStackTrace();
                    }
                }

                cache.Clear();
            }
        }

        public static int GetSizeDelta(JFile saveDir, int x, int z)
        {
            RegionFile regfile = GetRegionFile(saveDir, x, z);
            return regfile.GetSizeDelta();
        }

        public static BinaryReader GetInputStream(JFile saveDir, int x, int z)
        {
            RegionFile regfile = GetRegionFile(saveDir, x, z);
            return regfile.GetChunkDataInputStream(x & 31, z & 31);
        }

        public static BinaryWriter GetOutputStream(JFile saveDir, int x, int z)
        {
            RegionFile regfile = GetRegionFile(saveDir, x, z);
            return regfile.GetChunkDataOutputStream(x & 31, z & 31);
        }
    }
}