using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.Chunk
{
    public interface IChunkSource
    {
        bool HasChunk(int i1, int i2);
        LevelChunk GetChunk(int i1, int i2);
        LevelChunk Create(int i1, int i2);
        void PostProcess(IChunkSource iChunkProvider1, int i2, int i3);
        bool Save(bool z1, IProgressListener iProgressUpdate2);
        bool Tick();
        bool ShouldSave();
        string GatherStats();
    }
}