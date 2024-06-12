using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.Util;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel.Storage
{
    public interface ILevelStorageSource
    {
        string GetName();
        ILevelStorage SelectLevel(string name, bool sel);
        IList<LevelSummary> GetLevelList();
        void ClearAll();
        LevelData GetTagDataFor(string string1);
        void DeleteLevel(string string1);
        void RenameLevel(string string1, string string2);
        bool RequiresConversion(string string1);
        bool ConvertLevel(string string1, IProgressListener iProgressUpdate2);
    }
}