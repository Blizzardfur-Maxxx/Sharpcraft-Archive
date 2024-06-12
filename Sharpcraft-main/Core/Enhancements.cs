namespace SharpCraft.Core
{
    public class Enhancements
    {
        public static bool client_fly_hack = false;
        //self-explainatory
        public const bool FLAT_LEVEL = false;
        //if using flat level, only generates the spawn area and nothing else
        public const bool FLAT_LEVEL_VOID_WORLD = false;
        //forces spawn point to be at 0, 128, 0
        public const bool FORCE_SPAWN_POINT = false;
        //generates chests with all items in said flat level
        public const bool FLAT_LEVEL_ITEM_CHESTS = true;

        public const bool TREE_AO = true;
        public const bool WATER_BIOME_COLOR = true;
        public const float WATER_BIOME_COLOR_MUL = 127F;
        public const float WATER_BIOME_COLOR_ALPHA = 146F;

        public const int SAVE_INTERVAL_TICKS = 1200; /* Default: 40 */
        // Fix memory leaks in the chunk cache (ported from 1.2.5)
        public const bool FIX_CHUNK_CACHE_MEM_LEAK = true;
        // Threaded level I/O (possibly buggy)
        public const bool THREADED_LEVEL_IO = true;
    }
}
