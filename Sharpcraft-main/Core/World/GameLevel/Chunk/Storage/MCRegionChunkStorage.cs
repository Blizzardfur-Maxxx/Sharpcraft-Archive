using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Storage;
using SharpCraft.Core.Util;
using System.IO;
using SharpCraft.Core.NBT;
using System;

namespace SharpCraft.Core.World.GameLevel.Chunk.Storage
{
    public class MCRegionChunkStorage : IChunkStorage
    {
        private readonly JFile worldDir;

        public MCRegionChunkStorage(JFile dir)
        {
            this.worldDir = dir;
        }

        public virtual LevelChunk Load(Level level, int x, int z)
        {
            BinaryReader inputstream = RegionFileCache.GetInputStream(this.worldDir, x, z);
            if (inputstream != null)
            {
                CompoundTag root = NbtIO.Read(inputstream);
                if (!root.HasKey("Level"))
                {
                    Console.WriteLine("Chunk file at " + x + "," + z + " is missing level data, skipping");
                    return null;
                }
                else if (!root.GetCompoundTag("Level").HasKey("Blocks"))
                {
                    Console.WriteLine("Chunk file at " + x + "," + z + " is missing block data, skipping");
                    return null;
                }
                else
                {
                    LevelChunk chonk = DirectoryChunkStorage.Load(level, root.GetCompoundTag("Level"));
                    if (!chonk.IsAtLocation(x, z))
                    {
                        Console.WriteLine("Chunk file at " + x + "," + z + " is in the wrong location; relocating. (Expected " + x + ", " + z + ", got " + chonk.xPosition + ", " + chonk.zPosition + ")");
                        root.SetInteger("xPos", x);
                        root.SetInteger("zPos", z);
                        chonk = DirectoryChunkStorage.Load(level, root.GetCompoundTag("Level"));
                    }

                    chonk.LoadChunkBlockMap();
                    return chonk;
                }
            }
            else
            {
                return null;
            }
        }

        public virtual void Save(Level level, LevelChunk chonk)
        {
            level.CheckSession();
            try
            {
                BinaryWriter outputstream = RegionFileCache.GetOutputStream(this.worldDir, chonk.xPosition, chonk.zPosition);
                CompoundTag root = new CompoundTag();
                CompoundTag levelTag = new CompoundTag();
                root.SetTag("Level", levelTag);
                DirectoryChunkStorage.Save(chonk, level, levelTag);
                NbtIO.Write(root, outputstream);
                outputstream.Dispose();
                LevelData ldata = level.GetLevelData();
                ldata.SetSizeOnDisk(ldata.GetSizeOnDisk() + RegionFileCache.GetSizeDelta(this.worldDir, chonk.xPosition, chonk.zPosition));
            }
            catch (Exception exception7)
            {
                exception7.PrintStackTrace();
            }
        }

        public virtual void SaveEntities(Level world1, LevelChunk chunk2)
        {
        }

        public virtual void Tick()
        {
        }

        public virtual void Flush()
        {
        }
    }
}