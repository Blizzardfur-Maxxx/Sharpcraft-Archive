using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using System;
using System.IO;

namespace SharpCraft.Core.World.GameLevel.Chunk
{
    public class ChunkCache : IChunkSource
    {
        private LevelChunk blankChunk;
        private IChunkSource chunkProvider;
        private IChunkStorage chunkLoader;
        private LevelChunk[] chunks;
        private Level worldObj;
        int lastQueriedChunkXPos;
        int lastQueriedChunkZPos;
        private LevelChunk lastQueriedChunk;
        private int curChunkX;
        private int curChunkY;

        public ChunkCache() { }

        public ChunkCache(Level level, IChunkSource source, IChunkStorage storage) 
        {
            this.blankChunk = new LevelChunk(level, new byte[32768], 0, 0);
            this.chunkProvider = source;
            this.chunkLoader = storage;
            this.chunks = new LevelChunk[1024];
            this.worldObj = level;
        }

        public virtual void SetPos(int i1, int i2)
        {
            this.curChunkX = i1;
            this.curChunkY = i2;
        }

        public virtual bool CanChunkExist(int i1, int i2)
        {
            byte b3 = 15;
            return i1 >= this.curChunkX - b3 && i2 >= this.curChunkY - b3 && i1 <= this.curChunkX + b3 && i2 <= this.curChunkY + b3;
        }

        public virtual bool HasChunk(int i1, int i2)
        {
            if (!this.CanChunkExist(i1, i2))
            {
                return false;
            }
            else if (i1 == this.lastQueriedChunkXPos && i2 == this.lastQueriedChunkZPos && this.lastQueriedChunk != null)
            {
                return true;
            }
            else
            {
                int i3 = i1 & 31;
                int i4 = i2 & 31;
                int i5 = i3 + i4 * 32;
                return this.chunks[i5] != null && (this.chunks[i5] == this.blankChunk || this.chunks[i5].IsAtLocation(i1, i2));
            }
        }

        public virtual LevelChunk Create(int i1, int i2)
        {
            return this.GetChunk(i1, i2);
        }

        public virtual LevelChunk GetChunk(int i1, int i2)
        {
            if (i1 == this.lastQueriedChunkXPos && i2 == this.lastQueriedChunkZPos && this.lastQueriedChunk != null)
            {
                return this.lastQueriedChunk;
            }
            else if (!this.worldObj.findingSpawnPoint && !this.CanChunkExist(i1, i2))
            {
                return this.blankChunk;
            }
            else
            {
                int i3 = i1 & 31;
                int i4 = i2 & 31;
                int i5 = i3 + i4 * 32;
                if (!this.HasChunk(i1, i2))
                {
                    if (this.chunks[i5] != null)
                    {
                        this.chunks[i5].OnChunkUnload();
                        this.SaveChunk(this.chunks[i5]);
                        this.SaveExtraChunkData(this.chunks[i5]);
                    }

                    LevelChunk chunk6 = this.Func_542_c(i1, i2);
                    if (chunk6 == null)
                    {
                        if (this.chunkProvider == null)
                        {
                            chunk6 = this.blankChunk;
                        }
                        else
                        {
                            chunk6 = this.chunkProvider.GetChunk(i1, i2);
                            chunk6.LoadChunkBlockMap();
                        }
                    }

                    this.chunks[i5] = chunk6;
                    chunk6.Func_4143();
                    if (this.chunks[i5] != null)
                    {
                        this.chunks[i5].OnChunkLoad();
                    }

                    if (!this.chunks[i5].isTerrainPopulated && this.HasChunk(i1 + 1, i2 + 1) && this.HasChunk(i1, i2 + 1) && this.HasChunk(i1 + 1, i2))
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

                this.lastQueriedChunkXPos = i1;
                this.lastQueriedChunkZPos = i2;
                this.lastQueriedChunk = this.chunks[i5];
                return this.chunks[i5];
            }
        }

        private LevelChunk Func_542_c(int i1, int i2)
        {
            if (this.chunkLoader == null)
            {
                return this.blankChunk;
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
                    return this.blankChunk;
                }
            }
        }

        private void SaveExtraChunkData(LevelChunk chunk1)
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

        private void SaveChunk(LevelChunk chunk1)
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
            int i4 = 0;
            int i5;
            if (iProgressUpdate2 != null)
            {
                for (i5 = 0; i5 < this.chunks.Length; ++i5)
                {
                    if (this.chunks[i5] != null && this.chunks[i5].NeedsSaving(z1))
                    {
                        ++i4;
                    }
                }
            }

            i5 = 0;
            for (int i6 = 0; i6 < this.chunks.Length; ++i6)
            {
                if (this.chunks[i6] != null)
                {
                    if (z1 && !this.chunks[i6].neverSave)
                    {
                        this.SaveExtraChunkData(this.chunks[i6]);
                    }

                    if (this.chunks[i6].NeedsSaving(z1))
                    {
                        this.SaveChunk(this.chunks[i6]);
                        this.chunks[i6].isModified = false;
                        ++i3;
                        if (i3 == 2 && !z1)
                        {
                            return false;
                        }

                        if (iProgressUpdate2 != null)
                        {
                            ++i5;
                            if (i5 % 10 == 0)
                            {
                                iProgressUpdate2.SetLoadingProgress(i5 * 100 / i4);
                            }
                        }
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
            return "ChunkCache: " + this.chunks.Length;
        }
    }
}