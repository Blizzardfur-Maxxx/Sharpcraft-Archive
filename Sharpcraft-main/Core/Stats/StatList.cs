using SharpCraft.Core.i18n;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Items.Crafting;
using System.Collections.Generic;

namespace SharpCraft.Core.Stats
{
    public class StatList
    {
        public static NullDictionary<int, Stat> statIds = new NullDictionary<int, Stat>();
        public static IList<Stat> stats = new List<Stat>();
        public static IList<StatBasic> field_b = new List<StatBasic>();
        public static IList<CraftingStat> field_c = new List<CraftingStat>();
        public static IList<CraftingStat> field_d = new List<CraftingStat>();
        public static Stat startGame = (new StatBasic(1000, Locale.TranslateKey("stat.startGame"))).Func_h().RegisterStat();
        public static Stat createWorld = (new StatBasic(1001, Locale.TranslateKey("stat.createWorld"))).Func_h().RegisterStat();
        public static Stat loadWorld = (new StatBasic(1002, Locale.TranslateKey("stat.loadWorld"))).Func_h().RegisterStat();
        public static Stat joinMP = (new StatBasic(1003, Locale.TranslateKey("stat.joinMultiplayer"))).Func_h().RegisterStat();
        public static Stat leaveGame = (new StatBasic(1004, Locale.TranslateKey("stat.leaveGame"))).Func_h().RegisterStat();
        public static Stat minPlayed = (new StatBasic(1100, Locale.TranslateKey("stat.playOneMinute"), Stat.time)).Func_h().RegisterStat();
        public static Stat distWalked = (new StatBasic(2000, Locale.TranslateKey("stat.walkOneCm"), Stat.dist)).Func_h().RegisterStat();
        public static Stat distSwum = (new StatBasic(2001, Locale.TranslateKey("stat.swimOneCm"), Stat.dist)).Func_h().RegisterStat();
        public static Stat distFallen = (new StatBasic(2002, Locale.TranslateKey("stat.fallOneCm"), Stat.dist)).Func_h().RegisterStat();
        public static Stat distClimbed = (new StatBasic(2003, Locale.TranslateKey("stat.climbOneCm"), Stat.dist)).Func_h().RegisterStat();
        public static Stat distFly = (new StatBasic(2004, Locale.TranslateKey("stat.flyOneCm"), Stat.dist)).Func_h().RegisterStat();
        public static Stat distDove = (new StatBasic(2005, Locale.TranslateKey("stat.diveOneCm"), Stat.dist)).Func_h().RegisterStat();
        public static Stat distCart = (new StatBasic(2006, Locale.TranslateKey("stat.minecartOneCm"), Stat.dist)).Func_h().RegisterStat();
        public static Stat distBoat = (new StatBasic(2007, Locale.TranslateKey("stat.boatOneCm"), Stat.dist)).Func_h().RegisterStat();
        public static Stat distPig = (new StatBasic(2008, Locale.TranslateKey("stat.pigOneCm"), Stat.dist)).Func_h().RegisterStat();
        public static Stat jumps = (new StatBasic(2010, Locale.TranslateKey("stat.jump"))).Func_h().RegisterStat();
        public static Stat drops = (new StatBasic(2011, Locale.TranslateKey("stat.drop"))).Func_h().RegisterStat();
        public static Stat damageDealt = (new StatBasic(2020, Locale.TranslateKey("stat.damageDealt"))).RegisterStat();
        public static Stat damageTaken = (new StatBasic(2021, Locale.TranslateKey("stat.damageTaken"))).RegisterStat();
        public static Stat deaths = (new StatBasic(2022, Locale.TranslateKey("stat.deaths"))).RegisterStat();
        public static Stat mobKills = (new StatBasic(2023, Locale.TranslateKey("stat.mobKills"))).RegisterStat();
        public static Stat playerKills = (new StatBasic(2024, Locale.TranslateKey("stat.playerKills"))).RegisterStat();
        public static Stat fishCaught = (new StatBasic(2025, Locale.TranslateKey("stat.fishCaught"))).RegisterStat();
        public static Stat[] mineBlockStatArray = GetMineBlocks("stat.mineBlock", 16777216);
        public static Stat[] craftingStats;
        public static Stat[] itemUseStats;
        public static Stat[] itemDamageStats;
        private static bool field_D;
        private static bool field_E;
        public static void Init()
        {
        }

        public static void InitBlockStats()
        {
            itemUseStats = GetItemUse(itemUseStats, "stat.useItem", 16908288, 0, Tile.tiles.Length);
            itemDamageStats = GetItemDamage(itemDamageStats, "stat.breakItem", 16973824, 0, Tile.tiles.Length);
            field_D = true;
            Fun_c();
        }

        public static void InitItemStats()
        {
            itemUseStats = GetItemUse(itemUseStats, "stat.useItem", 16908288, Tile.tiles.Length, 32000);
            itemDamageStats = GetItemDamage(itemDamageStats, "stat.breakItem", 16973824, Tile.tiles.Length, 32000);
            field_E = true;
            Fun_c();
        }

        public static void Fun_c()
        {
            if (field_D && field_E)
            {
                HashSet<int> hashSet0 = new HashSet<int>();

                foreach (IRecipe recipe in Recipes.GetInstance().GetRecipeList())
                {
                    hashSet0.Add(recipe.GetRecipeOutput().itemID);
                }

                foreach (ItemInstance item in FurnaceRecipes.Smelting().GetSmeltingList().Values)
                {
                    hashSet0.Add(item.itemID);
                }

                craftingStats = new Stat[32000];
                foreach (int integer5 in hashSet0)
                {
                    if (Item.items[integer5] != null)
                    {
                        string string3 = Locale.TranslateKeyFormat("stat.craftItem", Item.items[integer5].GetNameTranslated());
                        craftingStats[integer5] = (new CraftingStat(16842752 + integer5, string3, integer5)).RegisterStat();
                    }
                }

                ReplaceAllSimilarBlocks(craftingStats);
            }
        }

        private static Stat[] GetMineBlocks(string string0, int i1)
        {
            Stat[] statBase2 = new Stat[256];
            for (int i3 = 0; i3 < 256; ++i3)
            {
                if (Tile.tiles[i3] != null && Tile.tiles[i3].IsCollectStatistics())
                {
                    string string4 = Locale.TranslateKeyFormat(string0, Tile.tiles[i3].GetName());
                    statBase2[i3] = (new CraftingStat(i1 + i3, string4, i3)).RegisterStat();
                    field_d.Add((CraftingStat)statBase2[i3]);
                }
            }

            ReplaceAllSimilarBlocks(statBase2);
            return statBase2;
        }

        private static Stat[] GetItemUse(Stat[] statBase0, string string1, int i2, int i3, int i4)
        {
            if (statBase0 == null)
            {
                statBase0 = new Stat[32000];
            }

            for (int i5 = i3; i5 < i4; ++i5)
            {
                if (Item.items[i5] != null)
                {
                    string string6 = Locale.TranslateKeyFormat(string1, Item.items[i5].GetNameTranslated());
                    statBase0[i5] = (new CraftingStat(i2 + i5, string6, i5)).RegisterStat();
                    if (i5 >= Tile.tiles.Length)
                    {
                        field_c.Add((CraftingStat)statBase0[i5]);
                    }
                }
            }

            ReplaceAllSimilarBlocks(statBase0);
            return statBase0;
        }

        private static Stat[] GetItemDamage(Stat[] statBase0, string string1, int i2, int i3, int i4)
        {
            if (statBase0 == null)
            {
                statBase0 = new Stat[32000];
            }

            for (int i5 = i3; i5 < i4; ++i5)
            {
                if (Item.items[i5] != null && Item.items[i5].IsDamagable())
                {
                    string string6 = Locale.TranslateKeyFormat(string1, Item.items[i5].GetNameTranslated());
                    statBase0[i5] = (new CraftingStat(i2 + i5, string6, i5)).RegisterStat();
                }
            }

            ReplaceAllSimilarBlocks(statBase0);
            return statBase0;
        }

        private static void ReplaceAllSimilarBlocks(Stat[] statBase0)
        {
            ReplaceSimilarBlocks(statBase0, Tile.calmWater.id, Tile.water.id);
            ReplaceSimilarBlocks(statBase0, Tile.calmLava.id, Tile.calmLava.id);
            ReplaceSimilarBlocks(statBase0, Tile.pumpkinLantern.id, Tile.pumpkin.id);
            ReplaceSimilarBlocks(statBase0, Tile.stoneOvenActive.id, Tile.furnace.id);
            ReplaceSimilarBlocks(statBase0, Tile.oreRedstoneGlowing.id, Tile.oreRedstone.id);
            ReplaceSimilarBlocks(statBase0, Tile.redstoneRepeaterActive.id, Tile.redstoneRepeaterIdle.id);
            ReplaceSimilarBlocks(statBase0, Tile.torchRedstoneActive.id, Tile.torchRedstoneIdle.id);
            ReplaceSimilarBlocks(statBase0, Tile.mushroom2.id, Tile.mushroom1.id);
            ReplaceSimilarBlocks(statBase0, Tile.stoneSlab.id, Tile.stoneSlabHalf.id);
            ReplaceSimilarBlocks(statBase0, Tile.grass.id, Tile.dirt.id);
            ReplaceSimilarBlocks(statBase0, Tile.farmland.id, Tile.dirt.id);
        }

        private static void ReplaceSimilarBlocks(Stat[] statBase0, int i1, int i2)
        {
            if (statBase0[i1] != null && statBase0[i2] == null)
            {
                statBase0[i2] = statBase0[i1];
            }
            else
            {
                stats.Remove(statBase0[i1]);
                // TODO: Figure out how to substitute this
                //field_d.Remove((CraftingStat)statBase0[i1]);
                //field_b.Remove((StatBasic)statBase0[i1]);
                statBase0[i1] = statBase0[i2];
            }
        }

        public static Stat GetStat(int i0)
        {
            return statIds[i0];
        }

        static StatList()
        {
            AchievementList.Init();
            field_D = false;
            field_E = false;
        }
    }
}