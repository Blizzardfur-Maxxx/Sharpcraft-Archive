using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;
using SharpCraft.Core.Util;
using System.Collections.Generic;
using System;

namespace SharpCraft.Client.Network
{
    public class MultiplayerChunkCache : IChunkSource
    {
        private LevelChunk blankChunk;
        private NullDictionary<ChunkPos, LevelChunk> chunkMapping = new();
        private IList<LevelChunk> chunkList = new List<LevelChunk>();
        private Level worldObj;

        public MultiplayerChunkCache(Level world1)
        {
            this.blankChunk = new EmptyLevelChunk(world1, new byte[32768], 0, 0);
            this.worldObj = world1;
        }

        public virtual bool HasChunk(int i1, int i2)
        {
            return true;
        }

        public virtual void Remove(int i1, int i2)
        {
            LevelChunk chunk3 = this.GetChunk(i1, i2);
            if (!chunk3.IsEmpty())
            {
                chunk3.OnChunkUnload();
            }

            this.chunkMapping.Remove(new ChunkPos(i1, i2));
            this.chunkList.Remove(chunk3);
        }

        public virtual LevelChunk Create(int i1, int i2)
        {
            ChunkPos chunkCoordIntPair3 = new ChunkPos(i1, i2);
            byte[] b4 = new byte[32768];
            LevelChunk chunk5 = new LevelChunk(this.worldObj, b4, i1, i2);
            Array.Fill<byte>(chunk5.skylightMap.data, 255);
            this.chunkMapping.Add(chunkCoordIntPair3, chunk5);
            chunk5.isChunkLoaded = true;
            return chunk5;
        }

        public virtual LevelChunk GetChunk(int i1, int i2)
        {
            ChunkPos chunkCoordIntPair3 = new ChunkPos(i1, i2);
            LevelChunk chunk4 = this.chunkMapping[chunkCoordIntPair3];
            return chunk4 == null ? this.blankChunk : chunk4;
        }

        public virtual bool Save(bool z1, IProgressListener iProgressUpdate2)
        {
            return true;
        }

        public virtual bool Tick()
        {
            return false;
        }

        public virtual bool ShouldSave()
        {
            return false;
        }

        public virtual void PostProcess(IChunkSource iChunkProvider1, int i2, int i3)
        {
        }

        public virtual string GatherStats()
        {
            return "MultiplayerChunkCache: " + this.chunkMapping.Count;
        }
    }
}