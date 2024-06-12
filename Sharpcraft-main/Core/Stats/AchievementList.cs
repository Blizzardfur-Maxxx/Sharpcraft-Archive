using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using System.Collections.Generic;

namespace SharpCraft.Core.Stats
{
    public class AchievementList
    {
        public static int minDisplayColumn;
        public static int minDisplayRow;
        public static int maxDisplayColumn;
        public static int maxDisplayRow;
        public static IList<Achievement> achievementList = new List<Achievement>();
        public static Achievement openInventory = (new Achievement(0, "openInventory", 0, 0, Item.book, (Achievement)null)).SetField_g().RegisterAchievement();
        public static Achievement mineWood = (new Achievement(1, "mineWood", 2, 1, Tile.treeTrunk, openInventory)).RegisterAchievement();
        public static Achievement buildWorkBench = (new Achievement(2, "buildWorkBench", 4, -1, Tile.workBench, mineWood)).RegisterAchievement();
        public static Achievement buildPickaxe = (new Achievement(3, "buildPickaxe", 4, 2, Item.pickaxeWood, buildWorkBench)).RegisterAchievement();
        public static Achievement buildFurnace = (new Achievement(4, "buildFurnace", 3, 4, Tile.stoneOvenActive, buildPickaxe)).RegisterAchievement();
        public static Achievement acquireIron = (new Achievement(5, "acquireIron", 1, 4, Item.ingotIron, buildFurnace)).RegisterAchievement();
        public static Achievement buildHoe = (new Achievement(6, "buildHoe", 2, -3, Item.hoeWood, buildWorkBench)).RegisterAchievement();
        public static Achievement makeBread = (new Achievement(7, "makeBread", -1, -3, Item.bread, buildHoe)).RegisterAchievement();
        public static Achievement bakeCake = (new Achievement(8, "bakeCake", 0, -5, Item.cake, buildHoe)).RegisterAchievement();
        public static Achievement buildBetterPickaxe = (new Achievement(9, "buildBetterPickaxe", 6, 2, Item.pickaxeStone, buildPickaxe)).RegisterAchievement();
        public static Achievement cookFish = (new Achievement(10, "cookFish", 2, 6, Item.fishCooked, buildFurnace)).RegisterAchievement();
        public static Achievement onARail = (new Achievement(11, "onARail", 2, 3, Tile.rail, acquireIron)).SetSpecial().RegisterAchievement();
        public static Achievement buildSword = (new Achievement(12, "buildSword", 6, -1, Item.swordWood, buildWorkBench)).RegisterAchievement();
        public static Achievement killEnemy = (new Achievement(13, "killEnemy", 8, -1, Item.bone, buildSword)).RegisterAchievement();
        public static Achievement killCow = (new Achievement(14, "killCow", 7, -3, Item.leather, buildSword)).RegisterAchievement();
        public static Achievement flyPig = (new Achievement(15, "flyPig", 8, -4, Item.saddle, killCow)).SetSpecial().RegisterAchievement();
        public static void Init()
        {
        }

        static AchievementList()
        {
            System.Console.WriteLine(achievementList.Count + " achievements");
        }
    }
}