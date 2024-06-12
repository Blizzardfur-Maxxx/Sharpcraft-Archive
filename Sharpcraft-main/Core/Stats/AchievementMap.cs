using SharpCraft.Core.Util;
using System;
using System.IO;

namespace SharpCraft.Core.Stats
{
    public class AchievementMap
    {
        public static AchievementMap instance = new AchievementMap();
        private NullDictionary<int, string> map = new NullDictionary<int, string>();
        private AchievementMap()
        {
            try
            {
                StreamReader reader = new StreamReader($"{SharedConstants.ASSETS_CORE_PATH}/achievement/map.txt");
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(',');
                    int guid = int.Parse(split[0]);
                    this.map[guid] = split[1]; //put
                }

                reader.Dispose();
            }
            catch (Exception e)
            {
                e.PrintStackTrace();
            }
        }

        public static string GetGuid(int statId)
        {
            if (!instance.map.TryGetValue(statId, out string guid)) return null;
            return guid;
        }
    }
}