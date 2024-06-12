using System.Collections.Generic;
using System.IO;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using SharpCraft.Core.World.GameLevel.Dimensions;
using SharpCraft.Core.World.GameLevel.Storage;

namespace SharpCraft.Client.Network
{
    public class MultiplayerLevelStorage : ILevelStorage
    {
        public virtual LevelData PrepareLevel()
        {
            return null;
        }

        public virtual void CheckSession()
        {
        }

        public virtual IChunkStorage CreateChunkStorage(Dimension worldProvider1)
        {
            return null;
        }

        public virtual void SaveLevelData(LevelData worldInfo1, IList<Player> list2)
        {
        }

        public virtual void SaveLevelData(LevelData worldInfo1)
        {
        }

        public virtual JFile GetDataFile(string string1)
        {
            return null;
        }

        public virtual IPlayerIO GetPlayerIO()
        {
            return null;
        }

        public virtual void CloseAll()
        {
        }
    }
}