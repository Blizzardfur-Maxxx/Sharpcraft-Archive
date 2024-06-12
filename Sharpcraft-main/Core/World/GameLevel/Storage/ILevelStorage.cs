using System.Collections.Generic;
using System.IO;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using SharpCraft.Core.World.GameLevel.Dimensions;

namespace SharpCraft.Core.World.GameLevel.Storage
{
    public interface ILevelStorage
    {
        LevelData PrepareLevel();
        void CheckSession();
        IChunkStorage CreateChunkStorage(Dimension worldProvider1);
        void SaveLevelData(LevelData worldInfo1, IList<Player> list2);
        void SaveLevelData(LevelData worldInfo1);
        IPlayerIO GetPlayerIO();
        void CloseAll();
        JFile GetDataFile(string string1);
    }
}