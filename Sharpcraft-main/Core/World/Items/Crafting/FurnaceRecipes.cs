using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class FurnaceRecipes
    {
        private static readonly FurnaceRecipes smeltingBase = new FurnaceRecipes();
        private NullDictionary<int, ItemInstance> smeltingList = new NullDictionary<int, ItemInstance>();
        public static FurnaceRecipes Smelting()
        {
            return smeltingBase;
        }

        private FurnaceRecipes()
        {
            this.AddSmelting(Tile.ironOre.id, new ItemInstance(Item.ingotIron));
            this.AddSmelting(Tile.goldOre.id, new ItemInstance(Item.ingotGold));
            this.AddSmelting(Tile.oreDiamond.id, new ItemInstance(Item.diamond));
            this.AddSmelting(Tile.sand.id, new ItemInstance(Tile.glass));
            this.AddSmelting(Item.porkRaw.id, new ItemInstance(Item.porkCooked));
            this.AddSmelting(Item.fishRaw.id, new ItemInstance(Item.fishCooked));
            this.AddSmelting(Tile.stoneBrick.id, new ItemInstance(Tile.rock));
            this.AddSmelting(Item.clay.id, new ItemInstance(Item.brick));
            this.AddSmelting(Tile.cactus.id, new ItemInstance(Item.dyePowder, 1, 2));
            this.AddSmelting(Tile.treeTrunk.id, new ItemInstance(Item.coal, 1, 1));
        }

        public virtual void AddSmelting(int i1, ItemInstance itemStack2)
        {
            this.smeltingList[i1] = itemStack2;
        }

        public virtual ItemInstance GetSmeltingResult(int i1)
        {
            return this.smeltingList[i1];
        }

        public virtual NullDictionary<int, ItemInstance> GetSmeltingList()
        {
            return this.smeltingList;
        }
    }
}