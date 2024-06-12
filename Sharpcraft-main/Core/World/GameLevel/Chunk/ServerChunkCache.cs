using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCraft.Core.World.GameLevel.Chunk
{
    public class ServerChunkCache : IChunkSource
    {
        private HashSet<int> droppedChunksSet = new HashSet<int>();
        private LevelChunk dummyChunk;
        private IChunkSource source;
        private IChunkStorage storage;
        private NullDictionary<int, LevelChunk> chunkMap = new NullDictionary<int, LevelChunk>();
        private IList<LevelChunk> chunkList = new List<LevelChunk>();
        private Level world;
        public ServerChunkCache(Level world1, IChunkStorage iChunkLoader2, IChunkSource iChunkProvider3)
        {
            this.dummyChunk = new EmptyLevelChunk(world1, new byte[32768], 0, 0);
            this.world = world1;
            this.storage = iChunkLoader2;
            this.source = iChunkProvider3;
        }

        public virtual bool HasChunk(int i1, int i2)
        {
            return this.chunkMap.ContainsKey(ChunkPos.Hash(i1, i2));
        }

        public virtual LevelChunk Create(int i1, int i2)
        {
            int i3 = ChunkPos.Hash(i1, i2);
            this.droppedChunksSet.Remove(i3);
            LevelChunk chunk4 = this.chunkMap[i3];
            if (chunk4 == null)
            {
                chunk4 = this.LoadChunkFromFile(i1, i2);
                if (chunk4 == null)
                {
                    if (this.source == null)
                    {
                        chunk4 = this.dummyChunk;
                    }
                    else
                    {
                        chunk4 = this.source.GetChunk(i1, i2);
                    }
                }

                this.chunkMap[i3] = chunk4;
                this.chunkList.Add(chunk4);
                if (chunk4 != null)
                {
                    chunk4.Func_4143();
                    chunk4.OnChunkLoad();
                }

                if (!chunk4.isTerrainPopulated && this.HasChunk(i1 + 1, i2 + 1) && this.HasChunk(i1, i2 + 1) && this.HasChunk(i1 + 1, i2))
                {
                    this.PostProcess(this, i1, i2);
                }

                if (this.HasChunk(i1 - 1, i2) && !this.GetChunk(i1 - 1, i2).isTerrainPopulated && this.HasChunk(i1 - 1, i2 + 1) && this.HasChunk(i1, i2 + 1) && this.HasChunk(i1 - 1, i2))
                {
                    this.PostProcess(this, i1 - 1, i2);
                }

                if (this.HasChunk(i1, i2 - 1) && !this.GetChunk(i1, i2 - 1).isTerrainPopulated && this.HasChunk(i1 + 1, i2 - 1) && this.HasChunk(i1, i2 - 1) && this.HasChunk(i1 + 1, i2))
                {
                    this.PostProcess(this, i1, i2 - 1);
                }

                if (this.HasChunk(i1 - 1, i2 - 1) && !this.GetChunk(i1 - 1, i2 - 1).isTerrainPopulated && this.HasChunk(i1 - 1, i2 - 1) && this.HasChunk(i1, i2 - 1) && this.HasChunk(i1 - 1, i2))
                {
                    this.PostProcess(this, i1 - 1, i2 - 1);
                }
            }

            return chunk4;
        }

        public virtual LevelChunk GetChunk(int i1, int i2)
        {
            LevelChunk chunk3 = this.chunkMap[ChunkPos.Hash(i1, i2)];
            return chunk3 == null ? this.Create(i1, i2) : chunk3;
        }

        private LevelChunk LoadChunkFromFile(int i1, int i2)
        {
            if (this.storage == null)
            {
                return null;
            }
            else
            {
                try
                {
                    LevelChunk chunk3 = this.storage.Load(this.world, i1, i2);
                    if (chunk3 != null)
                    {
                        chunk3.lastSaveTime = this.world.GetTime();
                    }

                    return chunk3;
                }
                catch (Exception exception4)
                {
                    exception4.PrintStackTrace();
                    return null;
                }
            }
        }

        private void Func_b(LevelChunk chunk1)
        {
            if (this.storage != null)
            {
                try
                {
                    this.storage.SaveEntities(this.world, chunk1);
                }
                catch (Exception exception3)
                {
                    exception3.PrintStackTrace();
                }
            }
        }

        private void Func_a(LevelChunk chunk1)
        {
            if (this.storage != null)
            {
                try
                {
                    chunk1.lastSaveTime = this.world.GetTime();
                    this.storage.Save(this.world, chunk1);
                }
                catch (IOException iOException3)
                {
                    iOException3.PrintStackTrace();
                }
            }
        }

        public virtual void PostProcess(IChunkSource iChunkProvider1, int i2, int i3)
        {
            LevelChunk chunk4 = this.GetChunk(i2, i3);
            if (!chunk4.isTerrainPopulated)
            {
                chunk4.isTerrainPopulated = true;
                if (this.source != null)
                {
                    this.source.PostProcess(iChunkProvider1, i2, i3);
                    chunk4.SetChunkModified();
                }
            }
        }

        public virtual bool Save(bool z1, IProgressListener iProgressUpdate2)
        {
            int i3 = 0;
            for (int i4 = 0; i4 < this.chunkList.Count; ++i4)
            {
                LevelChunk chunk5 = this.chunkList[i4];
                if (z1 && !chunk5.neverSave)
                {
                    this.Func_b(chunk5);
                }

                if (chunk5.NeedsSaving(z1))
                {
                    this.Func_a(chunk5);
                    chunk5.isModified = false;
                    ++i3;
                    if (i3 == 24 && !z1)
                    {
                        return false;
                    }
                }
            }

            if (z1)
            {
                if (this.storage == null)
                {
                    return true;
                }

                this.storage.Flush();
            }

            return true;
        }

        public virtual bool Tick()
        {
            for (int i1 = 0; i1 < 100; ++i1)
            {
                if (this.droppedChunksSet.Count != 0)
                {
                    int integer2 = this.droppedChunksSet.GetEnumerator().Current;
                    LevelChunk chunk3 = this.chunkMap[integer2];
                    chunk3.OnChunkUnload();
                    this.Func_a(chunk3);
                    this.Func_b(chunk3);
                    this.droppedChunksSet.Remove(integer2);
                    this.chunkMap.Remove(integer2);
                    this.chunkList.Remove(chunk3);
                }
            }

            if (this.storage != null)
            {
                this.storage.Tick();
            }

            return this.source.Tick();
        }

        public virtual bool ShouldSave()
        {
            return true;
        }

        public virtual string GatherStats()
        {
            return "ServerChunkCache: " + this.chunkMap.Count + " Drop: " + this.droppedChunksSet.Count;
        }
    }
}