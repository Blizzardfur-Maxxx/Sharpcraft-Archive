using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server.Levell.Chonk
{
    public class ImprovedDedicatedServerChunkCache : IChunkCacheAdapter
    {
        private HashSet<long> droppedChunksSet = new HashSet<long>();
        private LevelChunk dummyChunk;
        private IChunkSource source;
        private IChunkStorage storage;
        public bool chunkLoadOverride = false;
        private LongHashMap<LevelChunk> chunkMap = new LongHashMap<LevelChunk>();
        private IList<LevelChunk> chunkList = new List<LevelChunk>();
        private ServerLevel world;

        public virtual void SetChunkLoadOverride(bool b)
        {
            this.chunkLoadOverride = b;
        }

        public ImprovedDedicatedServerChunkCache(ServerLevel worldServer1, IChunkStorage iChunkLoader2, IChunkSource iChunkProvider3)
        {
            this.dummyChunk = new EmptyLevelChunk(worldServer1, new byte[32768], 0, 0);
            this.world = worldServer1;
            this.storage = iChunkLoader2;
            this.source = iChunkProvider3;
        }

        public virtual bool HasChunk(int i1, int i2)
        {
            return this.chunkMap.Contains(ChunkPos.Lhash(i1, i2));
        }

        public virtual void DropChunk(int i1, int i2)
        {
            if (this.world.dimension.CanRespawnHere())
            {
                Pos poz = this.world.GetSpawnPos();
                int i4 = i1 * 16 + 8 - poz.x;
                int i5 = i2 * 16 + 8 - poz.z;
                short s6 = 128;
                if (i4 < -s6 || i4 > s6 || i5 < -s6 || i5 > s6)
                {
                    this.droppedChunksSet.Add(ChunkPos.Lhash(i1, i2));
                }
            }
            else
            {
                this.droppedChunksSet.Add(ChunkPos.Lhash(i1, i2));
            }
        }

        public virtual void UnloadAllChunks()
        {
            foreach (LevelChunk lc in this.chunkList)
            {
                this.DropChunk(lc.xPosition, lc.zPosition);
            }
        }

        public virtual LevelChunk Create(int i1, int i2)
        {
            long j3 = ChunkPos.Lhash(i1, i2);
            this.droppedChunksSet.Remove(j3);
            LevelChunk chunk5 = (LevelChunk)this.chunkMap.Get(j3);
            if (chunk5 == null)
            {
                chunk5 = this.LoadChunkFromFile(i1, i2);
                if (chunk5 == null)
                {
                    if (this.source == null)
                    {
                        chunk5 = this.dummyChunk;
                    }
                    else
                    {
                        chunk5 = this.source.GetChunk(i1, i2);
                    }
                }

                this.chunkMap.Put(j3, chunk5);
                this.chunkList.Add(chunk5);
                if (chunk5 != null)
                {
                    chunk5.Func_4143();
                    chunk5.OnChunkLoad();
                }

                if (!chunk5.isTerrainPopulated && this.HasChunk(i1 + 1, i2 + 1) && this.HasChunk(i1, i2 + 1) && this.HasChunk(i1 + 1, i2))
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

            return chunk5;
        }

        public virtual LevelChunk GetChunk(int i1, int i2)
        {
            LevelChunk chunk3 = (LevelChunk)this.chunkMap.Get(ChunkPos.Lhash(i1, i2));
            return chunk3 == null ? (!this.world.findingSpawnPoint && !this.chunkLoadOverride ? this.dummyChunk : this.Create(i1, i2)) : chunk3;
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

        private void SaveChunkExtraData(LevelChunk chunk1)
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

        private void SaveChunkData(LevelChunk chunk1)
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
                LevelChunk chunk5 = (LevelChunk)this.chunkList[i4];
                if (z1)
                {
                    this.SaveChunkExtraData(chunk5);
                }

                if (chunk5.NeedsSaving(z1))
                {
                    this.SaveChunkData(chunk5);
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
            if (!this.world.saveDisabled)
            {
                IEnumerator<long> en = this.droppedChunksSet.GetEnumerator();
                for (int i1 = 0; i1 < 100; ++i1)
                {
                    if (this.droppedChunksSet.Count != 0)
                    {
                        if (!en.MoveNext()) break;
                        long long2 = en.Current;
                        LevelChunk cohnk = (LevelChunk)this.chunkMap.Get(long2);
                        cohnk.OnChunkUnload();
                        this.SaveChunkData(cohnk);
                        this.SaveChunkExtraData(cohnk);
                        this.droppedChunksSet.Remove(long2);
                        this.chunkMap.Remove(long2);
                        this.chunkList.Remove(cohnk);
                    }
                }

                if (this.storage != null)
                {
                    this.storage.Tick();
                }
            }

            return this.source.Tick();
        }

        public virtual bool ShouldSave()
        {
            return !this.world.saveDisabled;
        }

        public virtual string GatherStats()
        {
            return "IDedicatedServerChunkCache: " + this.chunkMap.Size() + " Drop: " + this.droppedChunksSet.Count;
        }
    }
}
