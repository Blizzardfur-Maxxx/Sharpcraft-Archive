using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Inventory;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.Items.Crafting
{
    public class Recipes
    {
        private static readonly Recipes instance = new Recipes();
        private List<IRecipe> recipes = new List<IRecipe>();
        public static Recipes GetInstance()
        {
            return instance;
        }

        private Recipes()
        {
            (new RecipesTools()).AddRecipes(this);
            (new RecipesWeapons()).AddRecipes(this);
            (new RecipesIngots()).AddRecipes(this);
            (new RecipesFood()).AddRecipes(this);
            (new StructureRecipes()).AddRecipes(this);
            (new ArmorRecipes()).AddRecipes(this);
            (new RecipesDyes()).AddRecipes(this);
            this.AddRecipe(new ItemInstance(Item.paper, 3), "###", '#', Item.reed);
            this.AddRecipe(new ItemInstance(Item.book, 1), "#", "#", "#", '#', Item.paper);
            this.AddRecipe(new ItemInstance(Tile.fence, 2), "###", "###", '#', Item.stick);
            this.AddRecipe(new ItemInstance(Tile.jukebox, 1), "###", "#X#", "###", '#', Tile.wood, 'X', Item.diamond);
            this.AddRecipe(new ItemInstance(Tile.musicBlock, 1), "###", "#X#", "###", '#', Tile.wood, 'X', Item.redstone);
            this.AddRecipe(new ItemInstance(Tile.bookshelf, 1), "###", "XXX", "###", '#', Tile.wood, 'X', Item.book);
            this.AddRecipe(new ItemInstance(Tile.blockSnow, 1), "##", "##", '#', Item.snowball);
            this.AddRecipe(new ItemInstance(Tile.blockClay, 1), "##", "##", '#', Item.clay);
            this.AddRecipe(new ItemInstance(Tile.redBrick, 1), "##", "##", '#', Item.brick);
            this.AddRecipe(new ItemInstance(Tile.lightgem, 1), "##", "##", '#', Item.lightStoneDust);
            this.AddRecipe(new ItemInstance(Tile.cloth, 1), "##", "##", '#', Item.silk);
            this.AddRecipe(new ItemInstance(Tile.tnt, 1), "X#X", "#X#", "X#X", 'X', Item.gunpowder, '#', Tile.sand);
            this.AddRecipe(new ItemInstance(Tile.stoneSlabHalf, 3, 3), "###", '#', Tile.stoneBrick);
            this.AddRecipe(new ItemInstance(Tile.stoneSlabHalf, 3, 0), "###", '#', Tile.rock);
            this.AddRecipe(new ItemInstance(Tile.stoneSlabHalf, 3, 1), "###", '#', Tile.sandStone);
            this.AddRecipe(new ItemInstance(Tile.stoneSlabHalf, 3, 2), "###", '#', Tile.wood);
            this.AddRecipe(new ItemInstance(Tile.ladder, 2), "# #", "###", "# #", '#', Item.stick);
            this.AddRecipe(new ItemInstance(Item.doorWood, 1), "##", "##", "##", '#', Tile.wood);
            this.AddRecipe(new ItemInstance(Tile.trapdoor, 2), "###", "###", '#', Tile.wood);
            this.AddRecipe(new ItemInstance(Item.doorSteel, 1), "##", "##", "##", '#', Item.ingotIron);
            this.AddRecipe(new ItemInstance(Item.sign, 1), "###", "###", " X ", '#', Tile.wood, 'X', Item.stick);
            this.AddRecipe(new ItemInstance(Item.cake, 1), "AAA", "BEB", "CCC", 'A', Item.bucketMilk, 'B', Item.sugar, 'C', Item.wheat, 'E', Item.egg);
            this.AddRecipe(new ItemInstance(Item.sugar, 1), "#", '#', Item.reed);
            this.AddRecipe(new ItemInstance(Tile.wood, 4), "#", '#', Tile.treeTrunk);
            this.AddRecipe(new ItemInstance(Item.stick, 4), "#", "#", '#', Tile.wood);
            this.AddRecipe(new ItemInstance(Tile.torch, 4), "X", "#", 'X', Item.coal, '#', Item.stick);
            this.AddRecipe(new ItemInstance(Tile.torch, 4), "X", "#", 'X', new ItemInstance(Item.coal, 1, 1), '#', Item.stick);
            this.AddRecipe(new ItemInstance(Item.bowlEmpty, 4), "# #", " # ", '#', Tile.wood);
            this.AddRecipe(new ItemInstance(Tile.rail, 16), "X X", "X#X", "X X", 'X', Item.ingotIron, '#', Item.stick);
            this.AddRecipe(new ItemInstance(Tile.railPowered, 6), "X X", "X#X", "XRX", 'X', Item.ingotGold, 'R', Item.redstone, '#', Item.stick);
            this.AddRecipe(new ItemInstance(Tile.railDetector, 6), "X X", "X#X", "XRX", 'X', Item.ingotIron, 'R', Item.redstone, '#', Tile.pressurePlateStone);
            this.AddRecipe(new ItemInstance(Item.minecartEmpty, 1), "# #", "###", '#', Item.ingotIron);
            this.AddRecipe(new ItemInstance(Tile.pumpkinLantern, 1), "A", "B", 'A', Tile.pumpkin, 'B', Tile.torch);
            this.AddRecipe(new ItemInstance(Item.minecartCrate, 1), "A", "B", 'A', Tile.chest, 'B', Item.minecartEmpty);
            this.AddRecipe(new ItemInstance(Item.minecartPowered, 1), "A", "B", 'A', Tile.furnace, 'B', Item.minecartEmpty);
            this.AddRecipe(new ItemInstance(Item.boat, 1), "# #", "###", '#', Tile.wood);
            this.AddRecipe(new ItemInstance(Item.bucketEmpty, 1), "# #", " # ", '#', Item.ingotIron);
            this.AddRecipe(new ItemInstance(Item.flintAndSteel, 1), "A ", " B", 'A', Item.ingotIron, 'B', Item.flint);
            this.AddRecipe(new ItemInstance(Item.bread, 1), "###", '#', Item.wheat);
            this.AddRecipe(new ItemInstance(Tile.stairs_wood, 4), "#  ", "## ", "###", '#', Tile.wood);
            this.AddRecipe(new ItemInstance(Item.fishingRod, 1), "  #", " #X", "# X", '#', Item.stick, 'X', Item.silk);
            this.AddRecipe(new ItemInstance(Tile.stairs_stone, 4), "#  ", "## ", "###", '#', Tile.stoneBrick);
            this.AddRecipe(new ItemInstance(Item.painting, 1), "###", "#X#", "###", '#', Item.stick, 'X', Tile.cloth);
            this.AddRecipe(new ItemInstance(Item.appleGold, 1), "###", "#X#", "###", '#', Tile.goldBlock, 'X', Item.appleRed);
            this.AddRecipe(new ItemInstance(Tile.lever, 1), "X", "#", '#', Tile.stoneBrick, 'X', Item.stick);
            this.AddRecipe(new ItemInstance(Tile.torchRedstoneActive, 1), "X", "#", '#', Item.stick, 'X', Item.redstone);
            this.AddRecipe(new ItemInstance(Item.redstoneRepeater, 1), "#X#", "III", '#', Tile.torchRedstoneActive, 'X', Item.redstone, 'I', Tile.rock);
            this.AddRecipe(new ItemInstance(Item.pocketSundial, 1), " # ", "#X#", " # ", '#', Item.ingotGold, 'X', Item.redstone);
            this.AddRecipe(new ItemInstance(Item.compass, 1), " # ", "#X#", " # ", '#', Item.ingotIron, 'X', Item.redstone);
            this.AddRecipe(new ItemInstance(Item.mapItem, 1), "###", "#X#", "###", '#', Item.paper, 'X', Item.compass);
            this.AddRecipe(new ItemInstance(Tile.button, 1), "#", "#", '#', Tile.rock);
            this.AddRecipe(new ItemInstance(Tile.pressurePlateStone, 1), "##", '#', Tile.rock);
            this.AddRecipe(new ItemInstance(Tile.pressurePlatePlanks, 1), "##", '#', Tile.wood);
            this.AddRecipe(new ItemInstance(Tile.dispenser, 1), "###", "#X#", "#R#", '#', Tile.stoneBrick, 'X', Item.bow, 'R', Item.redstone);
            this.AddRecipe(new ItemInstance(Tile.pistonBase, 1), "TTT", "#X#", "#R#", '#', Tile.stoneBrick, 'X', Item.ingotIron, 'R', Item.redstone, 'T', Tile.wood);
            this.AddRecipe(new ItemInstance(Tile.pistonStickyBase, 1), "S", "P", 'S', Item.slimeBall, 'P', Tile.pistonBase);
            this.AddRecipe(new ItemInstance(Item.bed, 1), "###", "XXX", '#', Tile.cloth, 'X', Tile.wood);
            this.recipes.Sort(new AnonymousComparator());
            System.Console.WriteLine(this.recipes.Count + " recipes");
        }

        private sealed class AnonymousComparator : IComparer<IRecipe>
        {
            public int Compare(IRecipe iRecipe1, IRecipe iRecipe2)
            {
                return iRecipe1 is ShapelessRecipe && iRecipe2 is ShapedRecipe ? 1 : (iRecipe2 is ShapelessRecipe && iRecipe1 is ShapedRecipe ? -1 : (iRecipe2.GetRecipeSize() < iRecipe1.GetRecipeSize() ? -1 : (iRecipe2.GetRecipeSize() > iRecipe1.GetRecipeSize() ? 1 : 0)));
            }
        }

        internal virtual void AddRecipe(ItemInstance itemStack1, params object[] object2)
        {
            string string3 = "";
            int i4 = 0;
            int i5 = 0;
            int i6 = 0;
            if (object2[i4] is String[])
            {
                String[] string11 = ((String[])object2[i4++]);
                for (int i8 = 0; i8 < string11.Length; ++i8)
                {
                    string string9 = string11[i8];
                    ++i6;
                    i5 = string9.Length;
                    string3 = string3 + string9;
                }
            }
            else
            {
                while (object2[i4] is string)
                {
                    string string7 = (string)object2[i4++];
                    ++i6;
                    i5 = string7.Length;
                    string3 = string3 + string7;
                }
            }

            NullDictionary<char, ItemInstance> hashMap12;
            for (hashMap12 = new NullDictionary<char, ItemInstance>(); i4 < object2.Length; i4 += 2)
            {
                char character13 = (char)object2[i4];
                ItemInstance itemStack15 = null;
                if (object2[i4 + 1] is Item)
                {
                    itemStack15 = new ItemInstance((Item)object2[i4 + 1]);
                }
                else if (object2[i4 + 1] is Tile)
                {
                    itemStack15 = new ItemInstance((Tile)object2[i4 + 1], 1, -1);
                }
                else if (object2[i4 + 1] is ItemInstance)
                {
                    itemStack15 = (ItemInstance)object2[i4 + 1];
                }

                hashMap12[character13] = itemStack15;
            }

            ItemInstance[] itemStack14 = new ItemInstance[i5 * i6];
            for (int i16 = 0; i16 < i5 * i6; ++i16)
            {
                char c10 = string3[i16];
                if (hashMap12.ContainsKey(c10))
                {
                    itemStack14[i16] = hashMap12[c10].Copy();
                }
                else
                {
                    itemStack14[i16] = null;
                }
            }

            this.recipes.Add(new ShapedRecipe(i5, i6, itemStack14, itemStack1));
        }

        internal virtual void AddShapelessRecipe(ItemInstance itemStack1, params object[] object2)
        {
            List<ItemInstance> arrayList3 = new List<ItemInstance>();
            Object[] object4 = object2;
            int i5 = object2.Length;
            for (int i6 = 0; i6 < i5; ++i6)
            {
                object object7 = object4[i6];
                if (object7 is ItemInstance)
                {
                    arrayList3.Add(((ItemInstance)object7).Copy());
                }
                else if (object7 is Item)
                {
                    arrayList3.Add(new ItemInstance((Item)object7));
                }
                else
                {
                    if (!(object7 is Tile))
                    {
                        throw new Exception("Invalid shapeless recipy!");
                    }

                    arrayList3.Add(new ItemInstance((Tile)object7));
                }
            }

            this.recipes.Add(new ShapelessRecipe(itemStack1, arrayList3));
        }

        public virtual ItemInstance FindMatchingRecipe(CraftingContainer inventoryCrafting1)
        {
            for (int i2 = 0; i2 < this.recipes.Count; ++i2)
            {
                IRecipe iRecipe3 = this.recipes[i2];
                if (iRecipe3.Matches(inventoryCrafting1))
                {
                    return iRecipe3.GetCraftingResult(inventoryCrafting1);
                }
            }

            return null;
        }

        public virtual IList<IRecipe> GetRecipeList()
        {
            return this.recipes;
        }
    }
}