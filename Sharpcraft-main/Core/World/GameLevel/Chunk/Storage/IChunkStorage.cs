using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk;

namespace SharpCraft.Core.World.GameLevel.Chunk.Storage
{
    public interface IChunkStorage
    {
        LevelChunk Load(Level world1, int i2, int i3);
        void Save(Level world1, LevelChunk chunk2);
        void SaveEntities(Level world1, LevelChunk chunk2);
        void Tick();
        void Flush();
    }
}