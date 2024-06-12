using SharpCraft.Core.World.GameLevel.Chunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Server.Levell.Chonk
{
    public interface IChunkCacheAdapter : IChunkSource
    {
        void UnloadAllChunks();
        void DropChunk(int x, int z);
        void SetChunkLoadOverride(bool b);
    }
}
