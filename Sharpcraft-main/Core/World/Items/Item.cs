using SharpCraft.Core.i18n;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Items
{
    public class Item
    {
        protected static JRandom itemRand = new JRandom();
        public static Item[] items = new Item[32000];
        public static Item shovelSteel = (new ItemSpade(0, Tier.IRON)).SetIconCoord(2, 5).SetItemName("shovelIron");
        public static Item pickaxeSteel = (new ItemPickaxe(1, Tier.IRON)).SetIconCoord(2, 6).SetItemName("pickaxeIron");
        public static Item axeSteel = (new ItemAxe(2, Tier.IRON)).SetIconCoord(2, 7).SetItemName("hatchetIron");
        public static Item flintAndSteel = (new ItemFlintAndSteel(3)).SetIconCoord(5, 0).SetItemName("flintAndSteel");
        public static Item appleRed = (new ItemFood(4, 4, false)).SetIconCoord(10, 0).SetItemName("apple");
        public static Item bow = (new ItemBow(5)).SetIconCoord(5, 1).SetItemName("bow");
        public static Item arrow = (new Item(6)).SetIconCoord(5, 2).SetItemName("arrow");
        public static Item coal = (new ItemCoal(7)).SetIconCoord(7, 0).SetItemName("coal");
        public static Item diamond = (new Item(8)).SetIconCoord(7, 3).SetItemName("emerald");
        public static Item ingotIron = (new Item(9)).SetIconCoord(7, 1).SetItemName("ingotIron");
        public static Item ingotGold = (new Item(10)).SetIconCoord(7, 2).SetItemName("ingotGold");
        public static Item swordSteel = (new ItemSword(11, Tier.IRON)).SetIconCoord(2, 4).SetItemName("swordIron");
        public static Item swordWood = (new ItemSword(12, Tier.WOOD)).SetIconCoord(0, 4).SetItemName("swordWood");
        public static Item shovelWood = (new ItemSpade(13, Tier.WOOD)).SetIconCoord(0, 5).SetItemName("shovelWood");
        public static Item pickaxeWood = (new ItemPickaxe(14, Tier.WOOD)).SetIconCoord(0, 6).SetItemName("pickaxeWood");
        public static Item axeWood = (new ItemAxe(15, Tier.WOOD)).SetIconCoord(0, 7).SetItemName("hatchetWood");
        public static Item swordStone = (new ItemSword(16, Tier.STONE)).SetIconCoord(1, 4).SetItemName("swordStone");
        public static Item shovelStone = (new ItemSpade(17, Tier.STONE)).SetIconCoord(1, 5).SetItemName("shovelStone");
        public static Item pickaxeStone = (new ItemPickaxe(18, Tier.STONE)).SetIconCoord(1, 6).SetItemName("pickaxeStone");
        public static Item axeStone = (new ItemAxe(19, Tier.STONE)).SetIconCoord(1, 7).SetItemName("hatchetStone");
        public static Item swordDiamond = (new ItemSword(20, Tier.EMERALD)).SetIconCoord(3, 4).SetItemName("swordDiamond");
        public static Item shovelDiamond = (new ItemSpade(21, Tier.EMERALD)).SetIconCoord(3, 5).SetItemName("shovelDiamond");
        public static Item pickaxeDiamond = (new ItemPickaxe(22, Tier.EMERALD)).SetIconCoord(3, 6).SetItemName("pickaxeDiamond");
        public static Item axeDiamond = (new ItemAxe(23, Tier.EMERALD)).SetIconCoord(3, 7).SetItemName("hatchetDiamond");
        public static Item stick = (new Item(24)).SetIconCoord(5, 3).SetFull3D().SetItemName("stick");
        public static Item bowlEmpty = (new Item(25)).SetIconCoord(7, 4).SetItemName("bowl");
        public static Item bowlSoup = (new ItemSoup(26, 10)).SetIconCoord(8, 4).SetItemName("mushroomStew");
        public static Item swordGold = (new ItemSword(27, Tier.GOLD)).SetIconCoord(4, 4).SetItemName("swordGold");
        public static Item shovelGold = (new ItemSpade(28, Tier.GOLD)).SetIconCoord(4, 5).SetItemName("shovelGold");
        public static Item pickaxeGold = (new ItemPickaxe(29, Tier.GOLD)).SetIconCoord(4, 6).SetItemName("pickaxeGold");
        public static Item axeGold = (new ItemAxe(30, Tier.GOLD)).SetIconCoord(4, 7).SetItemName("hatchetGold");
        public static Item silk = (new Item(31)).SetIconCoord(8, 0).SetItemName("string");
        public static Item feather = (new Item(32)).SetIconCoord(8, 1).SetItemName("feather");
        public static Item gunpowder = (new Item(33)).SetIconCoord(8, 2).SetItemName("sulphur");
        public static Item hoeWood = (new ItemHoe(34, Tier.WOOD)).SetIconCoord(0, 8).SetItemName("hoeWood");
        public static Item hoeStone = (new ItemHoe(35, Tier.STONE)).SetIconCoord(1, 8).SetItemName("hoeStone");
        public static Item hoeSteel = (new ItemHoe(36, Tier.IRON)).SetIconCoord(2, 8).SetItemName("hoeIron");
        public static Item hoeDiamond = (new ItemHoe(37, Tier.EMERALD)).SetIconCoord(3, 8).SetItemName("hoeDiamond");
        public static Item hoeGold = (new ItemHoe(38, Tier.GOLD)).SetIconCoord(4, 8).SetItemName("hoeGold");
        public static Item seeds = (new ItemSeeds(39, Tile.crops.id)).SetIconCoord(9, 0).SetItemName("seeds");
        public static Item wheat = (new Item(40)).SetIconCoord(9, 1).SetItemName("wheat");
        public static Item bread = (new ItemFood(41, 5, false)).SetIconCoord(9, 2).SetItemName("bread");
        public static Item helmetLeather = (new ItemArmor(42, 0, 0, 0)).SetIconCoord(0, 0).SetItemName("helmetCloth");
        public static Item plateLeather = (new ItemArmor(43, 0, 0, 1)).SetIconCoord(0, 1).SetItemName("chestplateCloth");
        public static Item legsLeather = (new ItemArmor(44, 0, 0, 2)).SetIconCoord(0, 2).SetItemName("leggingsCloth");
        public static Item bootsLeather = (new ItemArmor(45, 0, 0, 3)).SetIconCoord(0, 3).SetItemName("bootsCloth");
        public static Item helmetChain = (new ItemArmor(46, 1, 1, 0)).SetIconCoord(1, 0).SetItemName("helmetChain");
        public static Item plateChain = (new ItemArmor(47, 1, 1, 1)).SetIconCoord(1, 1).SetItemName("chestplateChain");
        public static Item legsChain = (new ItemArmor(48, 1, 1, 2)).SetIconCoord(1, 2).SetItemName("leggingsChain");
        public static Item bootsChain = (new ItemArmor(49, 1, 1, 3)).SetIconCoord(1, 3).SetItemName("bootsChain");
        public static Item helmetSteel = (new ItemArmor(50, 2, 2, 0)).SetIconCoord(2, 0).SetItemName("helmetIron");
        public static Item plateSteel = (new ItemArmor(51, 2, 2, 1)).SetIconCoord(2, 1).SetItemName("chestplateIron");
        public static Item legsSteel = (new ItemArmor(52, 2, 2, 2)).SetIconCoord(2, 2).SetItemName("leggingsIron");
        public static Item bootsSteel = (new ItemArmor(53, 2, 2, 3)).SetIconCoord(2, 3).SetItemName("bootsIron");
        public static Item helmetDiamond = (new ItemArmor(54, 3, 3, 0)).SetIconCoord(3, 0).SetItemName("helmetDiamond");
        public static Item plateDiamond = (new ItemArmor(55, 3, 3, 1)).SetIconCoord(3, 1).SetItemName("chestplateDiamond");
        public static Item legsDiamond = (new ItemArmor(56, 3, 3, 2)).SetIconCoord(3, 2).SetItemName("leggingsDiamond");
        public static Item bootsDiamond = (new ItemArmor(57, 3, 3, 3)).SetIconCoord(3, 3).SetItemName("bootsDiamond");
        public static Item helmetGold = (new ItemArmor(58, 1, 4, 0)).SetIconCoord(4, 0).SetItemName("helmetGold");
        public static Item plateGold = (new ItemArmor(59, 1, 4, 1)).SetIconCoord(4, 1).SetItemName("chestplateGold");
        public static Item legsGold = (new ItemArmor(60, 1, 4, 2)).SetIconCoord(4, 2).SetItemName("leggingsGold");
        public static Item bootsGold = (new ItemArmor(61, 1, 4, 3)).SetIconCoord(4, 3).SetItemName("bootsGold");
        public static Item flint = (new Item(62)).SetIconCoord(6, 0).SetItemName("flint");
        public static Item porkRaw = (new ItemFood(63, 3, true)).SetIconCoord(7, 5).SetItemName("porkchopRaw");
        public static Item porkCooked = (new ItemFood(64, 8, true)).SetIconCoord(8, 5).SetItemName("porkchopCooked");
        public static Item painting = (new ItemPainting(65)).SetIconCoord(10, 1).SetItemName("painting");
        public static Item appleGold = (new ItemFood(66, 42, false)).SetIconCoord(11, 0).SetItemName("appleGold");
        public static Item sign = (new ItemSign(67)).SetIconCoord(10, 2).SetItemName("sign");
        public static Item doorWood = (new ItemDoor(68, Material.wood)).SetIconCoord(11, 2).SetItemName("doorWood");
        public static Item bucketEmpty = (new ItemBucket(69, 0)).SetIconCoord(10, 4).SetItemName("bucket");
        public static Item bucketWater = (new ItemBucket(70, Tile.water.id)).SetIconCoord(11, 4).SetItemName("bucketWater").SetContainerItem(bucketEmpty);
        public static Item bucketLava = (new ItemBucket(71, Tile.lava.id)).SetIconCoord(12, 4).SetItemName("bucketLava").SetContainerItem(bucketEmpty);
        public static Item minecartEmpty = (new ItemMinecart(72, 0)).SetIconCoord(7, 8).SetItemName("minecart");
        public static Item saddle = (new ItemSaddle(73)).SetIconCoord(8, 6).SetItemName("saddle");
        public static Item doorSteel = (new ItemDoor(74, Material.metal)).SetIconCoord(12, 2).SetItemName("doorIron");
        public static Item redstone = (new ItemRedstone(75)).SetIconCoord(8, 3).SetItemName("redstone");
        public static Item snowball = (new ItemSnowball(76)).SetIconCoord(14, 0).SetItemName("snowball");
        public static Item boat = (new ItemBoat(77)).SetIconCoord(8, 8).SetItemName("boat");
        public static Item leather = (new Item(78)).SetIconCoord(7, 6).SetItemName("leather");
        public static Item bucketMilk = (new ItemBucket(79, -1)).SetIconCoord(13, 4).SetItemName("milk").SetContainerItem(bucketEmpty);
        public static Item brick = (new Item(80)).SetIconCoord(6, 1).SetItemName("brick");
        public static Item clay = (new Item(81)).SetIconCoord(9, 3).SetItemName("clay");
        public static Item reed = (new ItemReed(82, Tile.reed)).SetIconCoord(11, 1).SetItemName("reeds");
        public static Item paper = (new Item(83)).SetIconCoord(10, 3).SetItemName("paper");
        public static Item book = (new Item(84)).SetIconCoord(11, 3).SetItemName("book");
        public static Item slimeBall = (new Item(85)).SetIconCoord(14, 1).SetItemName("slimeball");
        public static Item minecartCrate = (new ItemMinecart(86, 1)).SetIconCoord(7, 9).SetItemName("minecartChest");
        public static Item minecartPowered = (new ItemMinecart(87, 2)).SetIconCoord(7, 10).SetItemName("minecartFurnace");
        public static Item egg = (new ItemEgg(88)).SetIconCoord(12, 0).SetItemName("egg");
        public static Item compass = (new Item(89)).SetIconCoord(6, 3).SetItemName("compass");
        public static Item fishingRod = (new ItemFishingRod(90)).SetIconCoord(5, 4).SetItemName("fishingRod");
        public static Item pocketSundial = (new Item(91)).SetIconCoord(6, 4).SetItemName("clock");
        public static Item lightStoneDust = (new Item(92)).SetIconCoord(9, 4).SetItemName("yellowDust");
        public static Item fishRaw = (new ItemFood(93, 2, false)).SetIconCoord(9, 5).SetItemName("fishRaw");
        public static Item fishCooked = (new ItemFood(94, 5, false)).SetIconCoord(10, 5).SetItemName("fishCooked");
        public static Item dyePowder = (new ItemDye(95)).SetIconCoord(14, 4).SetItemName("dyePowder");
        public static Item bone = (new Item(96)).SetIconCoord(12, 1).SetItemName("bone").SetFull3D();
        public static Item sugar = (new Item(97)).SetIconCoord(13, 0).SetItemName("sugar").SetFull3D();
        public static Item cake = (new ItemReed(98, Tile.cake)).SetMaxStackSize(1).SetIconCoord(13, 1).SetItemName("cake");
        public static Item bed = (new ItemBed(99)).SetMaxStackSize(1).SetIconCoord(13, 2).SetItemName("bed");
        public static Item redstoneRepeater = (new ItemReed(100, Tile.redstoneRepeaterIdle)).SetIconCoord(6, 5).SetItemName("diode");
        public static Item cookie = (new ItemCookie(101, 1, false, 8)).SetIconCoord(12, 5).SetItemName("cookie");
        public static ItemMap mapItem = (ItemMap)(new ItemMap(102)).SetIconCoord(12, 3).SetItemName("map");
        public static ItemShears shears = (ItemShears)(new ItemShears(103)).SetIconCoord(13, 5).SetItemName("shears");
        public static Item record13 = (new ItemRecord(2000, "13")).SetIconCoord(0, 15).SetItemName("record");
        public static Item recordCat = (new ItemRecord(2001, "cat")).SetIconCoord(1, 15).SetItemName("record");
        public readonly int id;
        protected int maxStackSize = 64;
        private int maxDamage = 0;
        protected int iconIndex;
        protected bool bFull3D = false;
        protected bool hasSubtypes = false;
        private Item containerItem = null;
        private string itemName;
        public class Tier
        {
            public static readonly Tier 
                WOOD = new Tier(0, 59, 2.0F, 0),
                STONE = new Tier(1, 131, 4.0F, 1),
                IRON = new Tier(2, 250, 6.0F, 2),
                EMERALD = new Tier(3, 1561, 8.0F, 3),
                GOLD = new Tier(0, 32, 12.0F, 0);

            private readonly int harvestLevel;
            private readonly int maxUses;
            private readonly float efficiencyOnProperMaterial;
            private readonly int damageVsEntity;

            private Tier(int i3, int i4, float f5, int i6)
            {
                this.harvestLevel = i3;
                this.maxUses = i4;
                this.efficiencyOnProperMaterial = f5;
                this.damageVsEntity = i6;
            }

            public int GetMaxUses()
            {
                return this.maxUses;
            }

            public float GetEfficiencyOnProperMaterial()
            {
                return this.efficiencyOnProperMaterial;
            }

            public int GetDamageVsEntity()
            {
                return this.damageVsEntity;
            }

            public int GetHarvestLevel()
            {
                return this.harvestLevel;
            }
        }

        protected Item(int i1)
        {
            this.id = 256 + i1;
            if (items[256 + i1] != null)
            {
                Console.WriteLine("CONFLICT @ " + i1);
            }

            items[256 + i1] = this;
        }

        public virtual Item SetIconIndex(int i1)
        {
            this.iconIndex = i1;
            return this;
        }

        public virtual Item SetMaxStackSize(int i1)
        {
            this.maxStackSize = i1;
            return this;
        }

        public virtual Item SetIconCoord(int i1, int i2)
        {
            this.iconIndex = i1 + i2 * 16;
            return this;
        }

        public virtual int GetIconFromDamage(int i1)
        {
            return this.iconIndex;
        }

        public int GetIconIndex(ItemInstance itemStack1)
        {
            return this.GetIconFromDamage(itemStack1.GetItemDamage());
        }

        public virtual bool OnItemUse(ItemInstance item, Player player, Level level, int x, int y, int z, TileFace face)
        {
            return false;
        }

        public virtual float GetStrVsBlock(ItemInstance itemStack1, Tile block2)
        {
            return 1F;
        }

        public virtual ItemInstance OnItemRightClick(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            return itemStack1;
        }

        public virtual int GetItemStackLimit()
        {
            return this.maxStackSize;
        }

        public virtual int GetMetadata(int i1)
        {
            return 0;
        }

        public virtual bool GetHasSubtypes()
        {
            return this.hasSubtypes;
        }

        protected virtual Item SetHasSubtypes(bool z1)
        {
            this.hasSubtypes = z1;
            return this;
        }

        public virtual int GetMaxDamage()
        {
            return this.maxDamage;
        }

        protected virtual Item SetMaxDamage(int i1)
        {
            this.maxDamage = i1;
            return this;
        }

        public virtual bool IsDamagable()
        {
            return this.maxDamage > 0 && !this.hasSubtypes;
        }

        public virtual bool HitEntity(ItemInstance itemStack1, Mob entityLiving2, Mob entityLiving3)
        {
            return false;
        }

        public virtual bool OnBlockDestroyed(ItemInstance itemStack1, int i2, int i3, int i4, int i5, Mob entityLiving6)
        {
            return false;
        }

        public virtual int GetDamageVsEntity(Entity entity1)
        {
            return 1;
        }

        public virtual bool CanHarvestBlock(Tile block1)
        {
            return false;
        }

        public virtual void SaddleEntity(ItemInstance itemStack1, Mob entityLiving2)
        {
        }

        public virtual Item SetFull3D()
        {
            this.bFull3D = true;
            return this;
        }

        public virtual bool IsFull3D()
        {
            return this.bFull3D;
        }

        public virtual bool ShouldRotateAroundWhenRendering()
        {
            return false;
        }

        public virtual Item SetItemName(string string1)
        {
            this.itemName = "item." + string1;
            return this;
        }

        public virtual string GetItemName()
        {
            return this.itemName;
        }

        public virtual string GetItemNameIS(ItemInstance itemStack1)
        {
            return this.itemName;
        }

        public virtual Item SetContainerItem(Item item1)
        {
            if (this.maxStackSize > 1)
            {
                throw new ArgumentException("Max stack size must be 1 for items with crafting results");
            }
            else
            {
                this.containerItem = item1;
                return this;
            }
        }

        public virtual Item GetContainerItem()
        {
            return this.containerItem;
        }

        public virtual bool HasContainerItem()
        {
            return this.containerItem != null;
        }

        public virtual string GetNameTranslated()
        {
            return Locale.TranslateKey(this.GetItemName() + ".name");
        }

        public virtual int GetColorFromDamage(int i1)
        {
            return 0xFFFFFF;
        }

        public virtual void OnUpdate(ItemInstance itemStack1, Level world2, Entity entity3, int i4, bool z5)
        {
        }

        public virtual void OnCreated(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
        }

        public virtual bool MapItemFunc()
        {
            return false;
        }

        static Item()
        {
            StatList.InitItemStats();
        }
    }
}