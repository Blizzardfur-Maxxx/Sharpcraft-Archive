using System.IO;
using SharpCraft.Core;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Chunk.Storage;
using SharpCraft.Core.World.GameLevel.Dimensions;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel.Storage
{
    public class MCRegionLevelStorage : DirectoryLevelStorage
    {
        public MCRegionLevelStorage(JFile file1, string string2, bool z3) : base(file1, string2, z3)
        {
        }

        public override IChunkStorage CreateChunkStorage(Dimension worldProvider1)
        {
            JFile dim = this.GetDirectory();
            if (worldProvider1 is HellDimension)
            {
                JFile dim1 = new JFile(dim, "DIM-1");
                //dim1.Mkdirs();
                dim1.Mkdir();
                return (Enhancements.THREADED_LEVEL_IO ? new AsyncMCRegionChunkStorage(dim1) : new MCRegionChunkStorage(dim1));
            }
            else
            {
                return (Enhancements.THREADED_LEVEL_IO ? new AsyncMCRegionChunkStorage(dim) : new MCRegionChunkStorage(dim));
            }
        }

        public override void SaveLevelData(LevelData worldInfo1, IList<Player> list2)
        {
            worldInfo1.SetSaveVersion(19132);
            base.SaveLevelData(worldInfo1, list2);
        }

        public override void CloseAll()
        {
            if (Enhancements.THREADED_LEVEL_IO)
            {
                try
                {
                    ThreadedIO.Instance.WaitForFinish();
                }
                catch
                {
                }
            }

            RegionFileCache.Clear();
        }
    }
}