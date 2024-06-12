using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCraft.Core.World.GameLevel.Chunk
{
    public class ImprovedServerChunkCache : IChunkSource
    {
        private HashSet<long> droppedChunksSet = new HashSet<long>();
        private LevelChunk emptyChunk;
        private IChunkSource chunkProvider;
        private IChunkStorage chunkLoader;
        private LongHashMap<LevelChunk> chunkMap = new LongHashMap<LevelChunk>();
        private IList<LevelChunk> chunkList = new List<LevelChunk>();
        private Level worldObj;
        private int dorp;
        public ImprovedServerChunkCache(Level world1, IChunkStorage iChunkLoader2, IChunkSource iChunkProvider3)
        {
            this.emptyChunk = new EmptyLevelChunk(world1, 0, 0);
            this.worldObj = world1;
            this.chunkLoader = iChunkLoader2;
            this.chunkProvider = iChunkProvider3;
        }

        public virtual bool HasChunk(int i1, int i2)
        {
            return this.chunkMap.Contains(ChunkPos.Lhash(i1, i2));
        }

        public virtual void DropChunk(int i1, int i2)
        {
            Pos chunkCoordinates3 = this.worldObj.GetSpawnPos();
            int i4 = i1 * 16 + 8 - chunkCoordinates3.x;
            int i5 = i2 * 16 + 8 - chunkCoordinates3.z;
            short s6 = 128;
            if (i4 < -s6 || i4 > s6 || i5 < -s6 || i5 > s6)
            {
                this.droppedChunksSet.Add(ChunkPos.Lhash(i1, i2));
            }
        }

        public virtual LevelChunk Create(int i1, int i2)
        {
            long j3 = ChunkPos.Lhash(i1, i2);
            this.droppedChunksSet.Remove(j3);
            LevelChunk chunk5 = chunkMap.Get(j3);
            if (chunk5 == null)
            {
                int i6 = 1875004;
                if (i1 < -i6 || i2 < -i6 || i1 >= i6 || i2 >= i6)
                {
                    return this.emptyChunk;
                }

                chunk5 = this.LoadChunkFromFile(i1, i2);
                if (chunk5 == null)
                {
                    if (this.chunkProvider == null)
                    {
                        chunk5 = this.emptyChunk;
                    }
                    else
                    {
                        chunk5 = this.chunkProvider.GetChunk(i1, i2);
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
            LevelChunk chunk3 = chunkMap.Get(ChunkPos.Lhash(i1, i2));
            return chunk3 == null ? this.Create(i1, i2) : chunk3;
        }

        private LevelChunk LoadChunkFromFile(int i1, int i2)
        {
            if (this.chunkLoader == null)
            {
                return null;
            }
            else
            {
                try
                {
                    LevelChunk chunk3 = this.chunkLoader.Load(this.worldObj, i1, i2);
                    if (chunk3 != null)
                    {
                        chunk3.lastSaveTime = this.worldObj.GetTime();
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
            if (this.chunkLoader != null)
            {
                try
                {
                    this.chunkLoader.SaveEntities(this.worldObj, chunk1);
                }
                catch (Exception exception3)
                {
                    exception3.PrintStackTrace();
                }
            }
        }

        private void SaveChunkData(LevelChunk chunk1)
        {
            if (this.chunkLoader != null)
            {
                try
                {
                    chunk1.lastSaveTime = this.worldObj.GetTime();
                    this.chunkLoader.Save(this.worldObj, chunk1);
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
                if (this.chunkProvider != null)
                {
                    this.chunkProvider.PostProcess(iChunkProvider1, i2, i3);
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
                if (this.chunkLoader == null)
                {
                    return true;
                }

                this.chunkLoader.Flush();
            }

            return true;
        }

        public virtual bool Tick()
        {
            int i1;
            IEnumerator<long> enumerator = droppedChunksSet.GetEnumerator();
            for (i1 = 0; i1 < 100; ++i1)
            {
                if (this.droppedChunksSet.Count != 0)
                {
                    if (!enumerator.MoveNext()) break;
                    long long2 = enumerator.Current;
                    LevelChunk chunk3 = this.chunkMap.Get(long2);
                    chunk3.OnChunkUnload();
                    this.SaveChunkData(chunk3);
                    this.SaveChunkExtraData(chunk3);
                    this.droppedChunksSet.Remove(long2);
                    this.chunkMap.Remove(long2);
                    this.chunkList.Remove(chunk3);
                }
            }

            for (i1 = 0; i1 < 10; ++i1)
            {
                if (this.dorp >= this.chunkList.Count)
                {
                    this.dorp = 0;
                    break;
                }

                LevelChunk chunk4 = (LevelChunk)this.chunkList[this.dorp++];
                Player entityPlayer5 = this.worldObj.Func_48456_a((double)(chunk4.xPosition << 4) + 8, (double)(chunk4.zPosition << 4) + 8, 288);
                if (entityPlayer5 == null)
                {
                    this.DropChunk(chunk4.xPosition, chunk4.zPosition);
                }
            }

            if (this.chunkLoader != null)
            {
                this.chunkLoader.Tick();
            }

            return this.chunkProvider.Tick();
        }

        public virtual bool ShouldSave()
        {
            return true;
        }

        public virtual string GatherStats()
        {
            return "ImprovedServerChunkCache: " + this.chunkMap.Size() + " Drop: " + this.droppedChunksSet.Count;
        }
    }
}