using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.World.GameLevel.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using static SharpCraft.Core.Util.ThreadedIO;

namespace SharpCraft.Core.World.GameLevel.Chunk.Storage
{
    public class AsyncMCRegionChunkStorage : IChunkStorage, IThreadedIOTarget
    {
        private class PendingIO
        {
            public readonly ChunkPos pos;
            public readonly CompoundTag chonkTag;

            public PendingIO(ChunkPos pos, CompoundTag tag)
            {
                this.pos = pos;
                this.chonkTag = tag;
            }
        }

        private IList<PendingIO> pendingIo = new List<PendingIO>();
        private HashSet<ChunkPos> positions = new HashSet<ChunkPos>();
        private object @lock = new object();
        private readonly JFile worldDir;

        public AsyncMCRegionChunkStorage(JFile dir)
        {
            this.worldDir = dir;
        }

        public virtual LevelChunk Load(Level level, int x, int z)
        {
            CompoundTag root = null;
            ChunkPos pos = new ChunkPos(x, z);
            lock (@lock)
            {
                if (this.positions.Contains(pos))
                {
                    for (int i = 0; i < this.positions.Count; i++)
                    {
                        if (this.pendingIo[i].pos.Equals(pos))
                        {
                            root = this.pendingIo[i].chonkTag;
                            break;
                        }
                    }
                }
            }

            if (root == null)
            {
                BinaryReader inputstream = RegionFileCache.GetInputStream(this.worldDir, x, z);
                if (inputstream == null)
                {
                    return null;
                }

                root = NbtIO.Read(inputstream);
            }

            return this.Load(level, x, z, root);
        }

        private LevelChunk Load(Level level, int x, int z, CompoundTag root)
        {
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

        public virtual void Save(Level level, LevelChunk chonk)
        {
            level.CheckSession();
            try
            {
                CompoundTag root = new CompoundTag();
                CompoundTag levelTag = new CompoundTag();
                root.SetTag("Level", levelTag);
                DirectoryChunkStorage.Save(chonk, level, levelTag);
                this.QueuePending(chonk.GetPos(), root);
                LevelData ldata = level.GetLevelData();
                ldata.SetSizeOnDisk(ldata.GetSizeOnDisk() + RegionFileCache.GetSizeDelta(this.worldDir, chonk.xPosition, chonk.zPosition));
            }
            catch (Exception exception7)
            {
                exception7.PrintStackTrace();
            }
        }

        private void QueuePending(ChunkPos pos, CompoundTag root)
        {
            lock (this.@lock)
            {
                if (this.positions.Contains(pos))
                {
                    for (int i4 = 0; i4 < this.pendingIo.Count; ++i4)
                    {
                        if (this.pendingIo[i4].pos.Equals(pos))
                        {
                            this.pendingIo[i4] = new PendingIO(pos, root);
                            return;
                        }
                    }
                }

                this.pendingIo.Add(new PendingIO(pos, root));
                this.positions.Add(pos);
                ThreadedIO.Instance.QueueIO(this);
            }
        }

        private void WriteIO(PendingIO pending)
        {
            BinaryWriter dos = RegionFileCache.GetOutputStream(this.worldDir, pending.pos.x, pending.pos.z);
            NbtIO.Write(pending.chonkTag, dos);
            dos.Dispose();
        }

        public virtual bool Write()
        {
            PendingIO pending = null;
            lock (this.@lock)
            {
                if (this.pendingIo.Count <= 0)
                {
                    return false;
                }

                pending = this.pendingIo[0];
                this.pendingIo.RemoveAt(0);
                this.positions.Remove(pending.pos);
            }

            if (pending != null)
            {
                try
                {
                    this.WriteIO(pending);
                }
                catch (Exception exception4)
                {
                    exception4.PrintStackTrace();
                }
            }

            return true;
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