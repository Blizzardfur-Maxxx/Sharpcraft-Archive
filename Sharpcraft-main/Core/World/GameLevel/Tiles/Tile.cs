using SharpCraft.Core.i18n;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;
using static SharpCraft.Core.Util.Facing;
using static SharpCraft.Core.World.GameLevel.Tiles.PressurePlateTile;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class Tile
    {
        public enum RenderShape : int
        {
            NONE = -1,
            NORMAL = 0,
            CROSS = 1,
            TORCH = 2,
            FIRE = 3,
            LIQUID = 4,
            WIRE = 5,
            CROPS = 6,
            DOOR = 7,
            LADDER = 8,
            RAIL = 9,
            STAIR = 10,
            FENCE = 11,
            LEVER = 12,
            CACTUS = 13,
            BED = 14,
            REPEATER = 15,
            PISTON_BASE = 16,
            PISTON_EXTENSION = 17
        }

        public enum RenderLayer : int
        {
            RENDERLAYER_OPAQUE = 0,
            RENDERLAYER_ALPHATEST = 1
        }

        public class SoundType
        {
            private readonly string name;
            private readonly float volume;
            private readonly float pitch;

            public SoundType(string name, float volume, float pitch)
            {
                this.name = name;
                this.volume = volume;
                this.pitch = pitch;
            }

            public virtual float GetVolume()
            {
                return volume;
            }

            public virtual float GetPitch()
            {
                return pitch;
            }

            public virtual string GetBreakSound()
            {
                return "step." + name;
            }

            public virtual string GetStepSound()
            {
                return "step." + name;
            }
        }

        public static readonly string TILE_DESCRIPTION_PREFIX = "tile.";
        public static readonly SoundType SOUND_NORMAL = new SoundType("stone", 1F, 1F);
        public static readonly SoundType SOUND_WOOD = new SoundType("wood", 1F, 1F);
        public static readonly SoundType SOUND_GRAVEL = new SoundType("gravel", 1F, 1F);
        public static readonly SoundType SOUND_GRASS = new SoundType("grass", 1F, 1F);
        public static readonly SoundType SOUND_STONE = new SoundType("stone", 1F, 1F);
        public static readonly SoundType SOUND_METAL = new SoundType("stone", 1F, 1.5F);
        public static readonly SoundType SOUND_GLASS = new SoundTypeGlass("stone", 1F, 1F);
        private sealed class SoundTypeGlass : SoundType
        {
            public SoundTypeGlass(string name, float volume, float pitch) : base(name, volume, pitch)
            {
            }

            public override string GetBreakSound()
            {
                return "random.glass";
            }
        }

        public static readonly SoundType SOUND_CLOTH = new SoundType("cloth", 1F, 1F);
        public static readonly SoundType SOUND_SAND = new SoundTypeSand("sand", 1F, 1F);
        private sealed class SoundTypeSand : SoundType
        {
            public SoundTypeSand(string name, float volume, float pitch) : base(name, volume, pitch)
            {
            }

            public override string GetBreakSound()
            {
                return "step.gravel";
            }
        }

        public static readonly Tile[] tiles = new Tile[256];
        public static readonly bool[] shouldTick = new bool[256];
        public static readonly bool[] solid = new bool[256];
        public static readonly bool[] isEntityTile = new bool[256];
        public static readonly int[] lightBlock = new int[256];
        public static readonly bool[] translucent = new bool[256];
        public static readonly int[] lightEmission = new int[256];
        public static readonly bool[] requiresSelfNotify = new bool[256];
        public static readonly Tile rock = (new StoneTile(1, 1)).SetDestroyTime(1.5F).SetExplodeable(10F).SetSoundType(SOUND_STONE).SetDescriptionId("stone");
        public static readonly GrassTile grass = (GrassTile)(new GrassTile(2)).SetDestroyTime(0.6F).SetSoundType(SOUND_GRASS).SetDescriptionId("grass");
        public static readonly Tile dirt = (new DirtTile(3, 2)).SetDestroyTime(0.5F).SetSoundType(SOUND_GRAVEL).SetDescriptionId("dirt");
        public static readonly Tile stoneBrick = (new Tile(4, 16, Material.stone)).SetDestroyTime(2F).SetExplodeable(10F).SetSoundType(SOUND_STONE).SetDescriptionId("stonebrick");
        public static readonly Tile wood = (new Tile(5, 4, Material.wood)).SetDestroyTime(2F).SetExplodeable(5F).SetSoundType(SOUND_WOOD).SetDescriptionId("wood").SetRequiresSelfNotify();
        public static readonly Tile sapling = (new Sapling(6, 15)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetDescriptionId("sapling").SetRequiresSelfNotify();
        public static readonly Tile unbreakable = (new Tile(7, 17, Material.stone)).SetUnbreakable().SetExplodeable(6000000F).SetSoundType(SOUND_STONE).SetDescriptionId("bedrock").SetNotCollectStatistics();
        public static readonly Tile water = (new LiquidTileDynamic(8, Material.water)).SetDestroyTime(100F).SetLightBlock(3).SetDescriptionId("water").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile calmWater = (new LiquidTileStatic(9, Material.water)).SetDestroyTime(100F).SetLightBlock(3).SetDescriptionId("water").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile lava = (new LiquidTileDynamic(10, Material.lava)).SetDestroyTime(0F).SetLightEmission(1F).SetLightBlock(255).SetDescriptionId("lava").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile calmLava = (new LiquidTileStatic(11, Material.lava)).SetDestroyTime(100F).SetLightEmission(1F).SetLightBlock(255).SetDescriptionId("lava").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile sand = (new SandTile(12, 18)).SetDestroyTime(0.5F).SetSoundType(SOUND_SAND).SetDescriptionId("sand");
        public static readonly Tile gravel = (new GravelTile(13, 19)).SetDestroyTime(0.6F).SetSoundType(SOUND_GRAVEL).SetDescriptionId("gravel");
        public static readonly Tile goldOre = (new OreTile(14, 32)).SetDestroyTime(3F).SetExplodeable(5F).SetSoundType(SOUND_STONE).SetDescriptionId("oreGold");
        public static readonly Tile ironOre = (new OreTile(15, 33)).SetDestroyTime(3F).SetExplodeable(5F).SetSoundType(SOUND_STONE).SetDescriptionId("oreIron");
        public static readonly Tile coalOre = (new OreTile(16, 34)).SetDestroyTime(3F).SetExplodeable(5F).SetSoundType(SOUND_STONE).SetDescriptionId("oreCoal");
        public static readonly Tile treeTrunk = (new TreeTrunkTile(17)).SetDestroyTime(2F).SetSoundType(SOUND_WOOD).SetDescriptionId("log").SetRequiresSelfNotify();
        public static readonly LeafTile leaves = (LeafTile)(new LeafTile(18, 52)).SetDestroyTime(0.2F).SetLightBlock(1).SetSoundType(SOUND_GRASS).SetDescriptionId("leaves").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile sponge = (new SpongeTile(19)).SetDestroyTime(0.6F).SetSoundType(SOUND_GRASS).SetDescriptionId("sponge");
        public static readonly Tile glass = (new GlassTile(20, 49, Material.glass, false)).SetDestroyTime(0.3F).SetSoundType(SOUND_GLASS).SetDescriptionId("glass");
        public static readonly Tile lapisOre = (new OreTile(21, 160)).SetDestroyTime(3F).SetExplodeable(5F).SetSoundType(SOUND_STONE).SetDescriptionId("oreLapis");
        public static readonly Tile lapisBlock = (new Tile(22, 144, Material.stone)).SetDestroyTime(3F).SetExplodeable(5F).SetSoundType(SOUND_STONE).SetDescriptionId("blockLapis");
        public static readonly Tile dispenser = (new DispenserTile(23)).SetDestroyTime(3.5F).SetSoundType(SOUND_STONE).SetDescriptionId("dispenser").SetRequiresSelfNotify();
        public static readonly Tile sandStone = (new SandstoneTile(24)).SetSoundType(SOUND_STONE).SetDestroyTime(0.8F).SetDescriptionId("sandStone");
        public static readonly Tile musicBlock = (new NoteTile(25)).SetDestroyTime(0.8F).SetDescriptionId("musicBlock").SetRequiresSelfNotify();
        public static readonly Tile bed = (new BedTile(26)).SetDestroyTime(0.2F).SetDescriptionId("bed").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile railPowered = (new RailTile(27, 179, true)).SetDestroyTime(0.7F).SetSoundType(SOUND_METAL).SetDescriptionId("goldenRail").SetRequiresSelfNotify();
        public static readonly Tile railDetector = (new DectectorRailTile(28, 195)).SetDestroyTime(0.7F).SetSoundType(SOUND_METAL).SetDescriptionId("detectorRail").SetRequiresSelfNotify();
        public static readonly Tile pistonStickyBase = (new PistonBaseTile(29, 106, true)).SetDescriptionId("pistonStickyBase").SetRequiresSelfNotify();
        public static readonly Tile web = (new WebTile(30, 11)).SetLightBlock(1).SetDestroyTime(4F).SetDescriptionId("web");
        public static readonly TallGrassTile tallGrass = (TallGrassTile)(new TallGrassTile(31, 39)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetDescriptionId("tallgrass");
        public static readonly DeadBushTile deadBush = (DeadBushTile)(new DeadBushTile(32, 55)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetDescriptionId("deadbush");
        public static readonly Tile pistonBase = (new PistonBaseTile(33, 107, false)).SetDescriptionId("pistonBase").SetRequiresSelfNotify();
        public static readonly PistonExtensionTile pistonExtension = (PistonExtensionTile)(new PistonExtensionTile(34, 107)).SetRequiresSelfNotify();
        public static readonly Tile cloth = (new ClothTile()).SetDestroyTime(0.8F).SetSoundType(SOUND_CLOTH).SetDescriptionId("cloth").SetRequiresSelfNotify();
        public static readonly PistonMovingPiece pistonMoving = new PistonMovingPiece(36);
        public static readonly Bush flower = (Bush)(new Bush(37, 13)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetDescriptionId("flower");
        public static readonly Bush rose = (Bush)(new Bush(38, 12)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetDescriptionId("rose");
        public static readonly Bush mushroom1 = (Bush)(new Mushroom(39, 29)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetLightEmission(0.125F).SetDescriptionId("mushroom");
        public static readonly Bush mushroom2 = (Bush)(new Mushroom(40, 28)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetDescriptionId("mushroom");
        public static readonly Tile goldBlock = (new MetalTile(41, 23)).SetDestroyTime(3F).SetExplodeable(10F).SetSoundType(SOUND_METAL).SetDescriptionId("blockGold");
        public static readonly Tile ironBlock = (new MetalTile(42, 22)).SetDestroyTime(5F).SetExplodeable(10F).SetSoundType(SOUND_METAL).SetDescriptionId("blockIron");
        public static readonly Tile stoneSlab = (new StoneSlabTile(43, true)).SetDestroyTime(2F).SetExplodeable(10F).SetSoundType(SOUND_STONE).SetDescriptionId("stoneSlab");
        public static readonly Tile stoneSlabHalf = (new StoneSlabTile(44, false)).SetDestroyTime(2F).SetExplodeable(10F).SetSoundType(SOUND_STONE).SetDescriptionId("stoneSlab");
        public static readonly Tile redBrick = (new Tile(45, 7, Material.stone)).SetDestroyTime(2F).SetExplodeable(10F).SetSoundType(SOUND_STONE).SetDescriptionId("brick");
        public static readonly Tile tnt = (new TntTile(46, 8)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetDescriptionId("tnt");
        public static readonly Tile bookshelf = (new BookshelfTile(47, 35)).SetDestroyTime(1.5F).SetSoundType(SOUND_WOOD).SetDescriptionId("bookshelf");
        public static readonly Tile mossStone = (new Tile(48, 36, Material.stone)).SetDestroyTime(2F).SetExplodeable(10F).SetSoundType(SOUND_STONE).SetDescriptionId("stoneMoss");
        public static readonly Tile obsidian = (new ObsidianTile(49, 37)).SetDestroyTime(10F).SetExplodeable(2000F).SetSoundType(SOUND_STONE).SetDescriptionId("obsidian");
        public static readonly Tile torch = (new TorchTile(50, 80)).SetDestroyTime(0F).SetLightEmission(0.9375F).SetSoundType(SOUND_WOOD).SetDescriptionId("torch").SetRequiresSelfNotify();
        public static readonly FireTile fire = (FireTile)(new FireTile(51, 31)).SetDestroyTime(0F).SetLightEmission(1F).SetSoundType(SOUND_WOOD).SetDescriptionId("fire").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile mobSpawner = (new MobSpawnerTile(52, 65)).SetDestroyTime(5F).SetSoundType(SOUND_METAL).SetDescriptionId("mobSpawner").SetNotCollectStatistics();
        public static readonly Tile stairs_wood = (new StairTile(53, wood)).SetDescriptionId("stairsWood").SetRequiresSelfNotify();
        public static readonly Tile chest = (new ChestTile(54)).SetDestroyTime(2.5F).SetSoundType(SOUND_WOOD).SetDescriptionId("chest").SetRequiresSelfNotify();
        public static readonly Tile redstoneWire = (new WireTile(55, 164)).SetDestroyTime(0F).SetSoundType(SOUND_NORMAL).SetDescriptionId("redstoneDust").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile oreDiamond = (new OreTile(56, 50)).SetDestroyTime(3F).SetExplodeable(5F).SetSoundType(SOUND_STONE).SetDescriptionId("oreDiamond");
        public static readonly Tile blockDiamond = (new MetalTile(57, 24)).SetDestroyTime(5F).SetExplodeable(10F).SetSoundType(SOUND_METAL).SetDescriptionId("blockDiamond");
        public static readonly Tile workBench = (new WorkbenchTile(58)).SetDestroyTime(2.5F).SetSoundType(SOUND_WOOD).SetDescriptionId("workbench");
        public static readonly Tile crops = (new CropTile(59, 88)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetDescriptionId("crops").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile farmland = (new FarmTile(60)).SetDestroyTime(0.6F).SetSoundType(SOUND_GRAVEL).SetDescriptionId("farmland");
        public static readonly Tile furnace = (new FurnaceTile(61, false)).SetDestroyTime(3.5F).SetSoundType(SOUND_STONE).SetDescriptionId("furnace").SetRequiresSelfNotify();
        public static readonly Tile stoneOvenActive = (new FurnaceTile(62, true)).SetDestroyTime(3.5F).SetSoundType(SOUND_STONE).SetLightEmission(0.875F).SetDescriptionId("furnace").SetRequiresSelfNotify();
        public static readonly Tile signPost = (new SignTile(63, typeof(TileEntitySign), true)).SetDestroyTime(1F).SetSoundType(SOUND_WOOD).SetDescriptionId("sign").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile doorWood = (new DoorTile(64, Material.wood)).SetDestroyTime(3F).SetSoundType(SOUND_WOOD).SetDescriptionId("doorWood").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile ladder = (new LadderTile(65, 83)).SetDestroyTime(0.4F).SetSoundType(SOUND_WOOD).SetDescriptionId("ladder").SetRequiresSelfNotify();
        public static readonly Tile rail = (new RailTile(66, 128, false)).SetDestroyTime(0.7F).SetSoundType(SOUND_METAL).SetDescriptionId("rail").SetRequiresSelfNotify();
        public static readonly Tile stairs_stone = (new StairTile(67, stoneBrick)).SetDescriptionId("stairsStone").SetRequiresSelfNotify();
        public static readonly Tile signWall = (new SignTile(68, typeof(TileEntitySign), false)).SetDestroyTime(1F).SetSoundType(SOUND_WOOD).SetDescriptionId("sign").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile lever = (new LeverTile(69, 96)).SetDestroyTime(0.5F).SetSoundType(SOUND_WOOD).SetDescriptionId("lever").SetRequiresSelfNotify();
        public static readonly Tile pressurePlateStone = (new PressurePlateTile(70, rock.texture, Sensitivity.mobs, Material.stone)).SetDestroyTime(0.5F).SetSoundType(SOUND_STONE).SetDescriptionId("pressurePlate").SetRequiresSelfNotify();
        public static readonly Tile door_iron = (new DoorTile(71, Material.metal)).SetDestroyTime(5F).SetSoundType(SOUND_METAL).SetDescriptionId("doorIron").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile pressurePlatePlanks = (new PressurePlateTile(72, wood.texture, Sensitivity.everything, Material.wood)).SetDestroyTime(0.5F).SetSoundType(SOUND_WOOD).SetDescriptionId("pressurePlate").SetRequiresSelfNotify();
        public static readonly Tile oreRedstone = (new RedstoneOreTile(73, 51, false)).SetDestroyTime(3F).SetExplodeable(5F).SetSoundType(SOUND_STONE).SetDescriptionId("oreRedstone").SetRequiresSelfNotify();
        public static readonly Tile oreRedstoneGlowing = (new RedstoneOreTile(74, 51, true)).SetLightEmission(0.625F).SetDestroyTime(3F).SetExplodeable(5F).SetSoundType(SOUND_STONE).SetDescriptionId("oreRedstone").SetRequiresSelfNotify();
        public static readonly Tile torchRedstoneIdle = (new NotGateTile(75, 115, false)).SetDestroyTime(0F).SetSoundType(SOUND_WOOD).SetDescriptionId("notGate").SetRequiresSelfNotify();
        public static readonly Tile torchRedstoneActive = (new NotGateTile(76, 99, true)).SetDestroyTime(0F).SetLightEmission(0.5F).SetSoundType(SOUND_WOOD).SetDescriptionId("notGate").SetRequiresSelfNotify();
        public static readonly Tile button = (new ButtonTile(77, rock.texture)).SetDestroyTime(0.5F).SetSoundType(SOUND_STONE).SetDescriptionId("button").SetRequiresSelfNotify();
        public static readonly Tile topSnow = (new TopSnowTile(78, 66)).SetDestroyTime(0.1F).SetSoundType(SOUND_CLOTH).SetDescriptionId("snow");
        public static readonly Tile ice = (new IceTile(79, 67)).SetDestroyTime(0.5F).SetLightBlock(3).SetSoundType(SOUND_GLASS).SetDescriptionId("ice");
        public static readonly Tile blockSnow = (new SnowTile(80, 66)).SetDestroyTime(0.2F).SetSoundType(SOUND_CLOTH).SetDescriptionId("snow");
        public static readonly Tile cactus = (new CactusTile(81, 70)).SetDestroyTime(0.4F).SetSoundType(SOUND_CLOTH).SetDescriptionId("cactus");
        public static readonly Tile blockClay = (new ClayTile(82, 72)).SetDestroyTime(0.6F).SetSoundType(SOUND_GRAVEL).SetDescriptionId("clay");
        public static readonly Tile reed = (new ReedTile(83, 73)).SetDestroyTime(0F).SetSoundType(SOUND_GRASS).SetDescriptionId("reeds").SetNotCollectStatistics();
        public static readonly Tile jukebox = (new JukeboxTile(84, 74)).SetDestroyTime(2F).SetExplodeable(10F).SetSoundType(SOUND_STONE).SetDescriptionId("jukebox").SetRequiresSelfNotify();
        public static readonly Tile fence = (new FenceTile(85, 4)).SetDestroyTime(2F).SetExplodeable(5F).SetSoundType(SOUND_WOOD).SetDescriptionId("fence").SetRequiresSelfNotify();
        public static readonly Tile pumpkin = (new PumpkinTile(86, 102, false)).SetDestroyTime(1F).SetSoundType(SOUND_WOOD).SetDescriptionId("pumpkin").SetRequiresSelfNotify();
        public static readonly Tile netherrack = (new BloodStoneTile(87, 103)).SetDestroyTime(0.4F).SetSoundType(SOUND_STONE).SetDescriptionId("hellrock");
        public static readonly Tile slowSand = (new SoulsandTile(88, 104)).SetDestroyTime(0.5F).SetSoundType(SOUND_SAND).SetDescriptionId("hellsand");
        public static readonly Tile lightgem = (new LightGemTile(89, 105, Material.stone)).SetDestroyTime(0.3F).SetSoundType(SOUND_GLASS).SetLightEmission(1F).SetDescriptionId("lightgem");
        public static readonly PortalTile portal = (PortalTile)(new PortalTile(90, 14)).SetDestroyTime(-1F).SetSoundType(SOUND_GLASS).SetLightEmission(0.75F).SetDescriptionId("portal");
        public static readonly Tile pumpkinLantern = (new PumpkinTile(91, 102, true)).SetDestroyTime(1F).SetSoundType(SOUND_WOOD).SetLightEmission(1F).SetDescriptionId("litpumpkin").SetRequiresSelfNotify();
        public static readonly Tile cake = (new CakeTile(92, 121)).SetDestroyTime(0.5F).SetSoundType(SOUND_CLOTH).SetDescriptionId("cake").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile redstoneRepeaterIdle = (new DiodeTile(93, false)).SetDestroyTime(0F).SetSoundType(SOUND_WOOD).SetDescriptionId("diode").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile redstoneRepeaterActive = (new DiodeTile(94, true)).SetDestroyTime(0F).SetLightEmission(0.625F).SetSoundType(SOUND_WOOD).SetDescriptionId("diode").SetNotCollectStatistics().SetRequiresSelfNotify();
        public static readonly Tile lockedChest = (new LockedChestTile(95)).SetDestroyTime(0F).SetLightEmission(1F).SetSoundType(SOUND_WOOD).SetDescriptionId("lockedchest").SetTicking(true).SetRequiresSelfNotify();
        public static readonly Tile trapdoor = (new TrapdoorTile(96, Material.wood)).SetDestroyTime(3F).SetSoundType(SOUND_WOOD).SetDescriptionId("trapdoor").SetNotCollectStatistics().SetRequiresSelfNotify();
        public int texture;
        public readonly int id;
        internal float destroyTime;
        internal float explosionResistance;
        protected bool blockConstructorCalled;
        protected bool collectStatistics;
        public double minX;
        public double minY;
        public double minZ;
        public double maxX;
        public double maxY;
        public double maxZ;
        public SoundType soundType;
        public float particleGravity;
        public readonly Material material;
        public float slipperiness;
        private string descriptionId;
        protected Tile(int i1, Material material2)
        {
            this.blockConstructorCalled = true;
            this.collectStatistics = true;
            this.soundType = SOUND_NORMAL;
            this.particleGravity = 1F;
            this.slipperiness = 0.6F;
            if (tiles[i1] != null)
            {
                throw new ArgumentException($"Slot {i1} is already occupied by {tiles[i1]} when adding {this}");
            }
            else
            {
                this.material = material2;
                tiles[i1] = this;
                this.id = i1;
                this.SetShape(0F, 0F, 0F, 1F, 1F, 1F);
                solid[i1] = this.IsSolidRender();
                lightBlock[i1] = this.IsSolidRender() ? 255 : 0;
                translucent[i1] = !material2.BlocksLight();
                isEntityTile[i1] = false;
            }
        }

        protected virtual Tile SetRequiresSelfNotify()
        {
            requiresSelfNotify[this.id] = true;
            return this;
        }

        protected virtual void Init()
        {
        }

        protected Tile(int i1, int i2, Material material3) : this(i1, material3)
        {
            this.texture = i2;
        }

        protected virtual Tile SetSoundType(SoundType stepSound1)
        {
            this.soundType = stepSound1;
            return this;
        }

        protected virtual Tile SetLightBlock(int i1)
        {
            lightBlock[this.id] = i1;
            return this;
        }

        protected virtual Tile SetLightEmission(float f1)
        {
            lightEmission[this.id] = (int)(15F * f1);
            return this;
        }

        protected virtual Tile SetExplodeable(float f1)
        {
            this.explosionResistance = f1 * 3F;
            return this;
        }

        public virtual bool IsCubeShaped()
        {
            return true;
        }

        public virtual RenderShape GetRenderShape()
        {
            return RenderShape.NORMAL;
        }

        protected virtual Tile SetDestroyTime(float f1)
        {
            this.destroyTime = f1;
            if (this.explosionResistance < f1 * 5F)
            {
                this.explosionResistance = f1 * 5F;
            }

            return this;
        }

        protected virtual Tile SetUnbreakable()
        {
            this.SetDestroyTime(-1F);
            return this;
        }

        public virtual float GetDestroyTime()
        {
            return this.destroyTime;
        }

        protected virtual Tile SetTicking(bool z1)
        {
            shouldTick[this.id] = z1;
            return this;
        }

        public virtual void SetShape(float f1, float f2, float f3, float f4, float f5, float f6)
        {
            this.minX = f1;
            this.minY = f2;
            this.minZ = f3;
            this.maxX = f4;
            this.maxY = f5;
            this.maxZ = f6;
        }

        public virtual float GetBrightness(ILevelSource ls, int x, int y, int z)
        {
            if (Enhancements.TREE_AO)
            {
                if (ls.GetMaterial(x, y, z) == Material.leaves)
                {
                    float br = ls.GetBrightness(x, y, z);
                    if (br > 0.2F)
                    {
                        br = 0.2F;
                    }

                    return br;
                }
            }

            return ls.GetBrightness(x, y, z, lightEmission[this.id]);
        }

        public virtual bool ShouldRenderFace(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return i5 == 0 && this.minY > 0 ? true : (i5 == 1 && this.maxY < 1 ? true : (i5 == 2 && this.minZ > 0 ? true : (i5 == 3 && this.maxZ < 1 ? true : (i5 == 4 && this.minX > 0 ? true : (i5 == 5 && this.maxX < 1 ? true : !iBlockAccess1.IsSolidRenderTile(i2, i3, i4))))));
        }

        public virtual bool GetIsBlockSolid(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return iBlockAccess1.GetMaterial(i2, i3, i4).IsSolid();
        }

        public virtual int GetBlockTexture(ILevelSource iBlockAccess1, int i2, int i3, int i4, TileFace i5)
        {
            return this.GetTexture(i5, iBlockAccess1.GetData(i2, i3, i4));
        }

        public virtual int GetTexture(TileFace faceIdx, int i2)
        {
            return this.GetTexture(faceIdx);
        }

        public virtual int GetTexture(TileFace faceIdx)
        {
            return this.texture;
        }

        public virtual AABB GetTileAABB(Level world1, int i2, int i3, int i4)
        {
            return AABB.Of(i2 + this.minX, i3 + this.minY, i4 + this.minZ, i2 + this.maxX, i3 + this.maxY, i4 + this.maxZ);
        }

        public virtual void AddAABBs(Level world1, int i2, int i3, int i4, AABB axisAlignedBB5, List<AABB> arrayList6)
        {
            AABB axisAlignedBB7 = this.GetAABB(world1, i2, i3, i4);
            if (axisAlignedBB7 != null && axisAlignedBB5.IntersectsWith(axisAlignedBB7))
            {
                arrayList6.Add(axisAlignedBB7);
            }
        }

        public virtual AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return AABB.Of(i2 + this.minX, i3 + this.minY, i4 + this.minZ, i2 + this.maxX, i3 + this.maxY, i4 + this.maxZ);
        }

        public virtual bool IsSolidRender()
        {
            return true;
        }

        public virtual bool MayPick(int i1, bool z2)
        {
            return this.IsCollidable();
        }

        public virtual bool IsCollidable()
        {
            return true;
        }

        public virtual void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
        }

        public virtual void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
        }

        public virtual void OnBlockDestroyedByPlayer(Level world1, int i2, int i3, int i4, int i5)
        {
        }

        public virtual void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
        }

        public virtual int GetTickDelay()
        {
            return 10;
        }

        public virtual void OnPlace(Level world1, int i2, int i3, int i4)
        {
        }

        public virtual void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
        }

        public virtual int ResourceCount(JRandom random1)
        {
            return 1;
        }

        public virtual int GetResource(int i1, JRandom random2)
        {
            return this.id;
        }

        public virtual float BlockStrength(Player entityPlayer1)
        {
            return this.destroyTime < 0F ? 0F : (!entityPlayer1.CanHarvestBlock(this) ? 1F / this.destroyTime / 100F : entityPlayer1.GetCurrentPlayerStrVsBlock(this) / this.destroyTime / 30F);
        }

        public void DropBlockAsItem(Level world1, int i2, int i3, int i4, int i5)
        {
            this.SpawnResources(world1, i2, i3, i4, i5, 1F);
        }

        public virtual void SpawnResources(Level world1, int i2, int i3, int i4, int i5, float f6)
        {
            if (!world1.isRemote)
            {
                int i7 = this.ResourceCount(world1.rand);
                for (int i8 = 0; i8 < i7; ++i8)
                {
                    if ((float)world1.rand.NextFloat() <= f6)
                    {
                        int i9 = this.GetResource(i5, world1.rand);
                        if (i9 > 0)
                        {
                            this.SpawnItem(world1, i2, i3, i4, new ItemInstance(i9, 1, this.GetSpawnResourcesAuxValue(i5)));
                        }
                    }
                }
            }
        }

        protected virtual void SpawnItem(Level world1, int i2, int i3, int i4, ItemInstance itemStack5)
        {
            if (!world1.isRemote)
            {
                float f6 = 0.7F;
                double d7 = world1.rand.NextFloat() * f6 + (1F - f6) * 0.5;
                double d9 = world1.rand.NextFloat() * f6 + (1F - f6) * 0.5;
                double d11 = world1.rand.NextFloat() * f6 + (1F - f6) * 0.5;
                ItemEntity entityItem13 = new ItemEntity(world1, i2 + d7, i3 + d9, i4 + d11, itemStack5);
                entityItem13.delayBeforeCanPickup = 10;
                world1.AddEntity(entityItem13);
            }
        }

        protected virtual int GetSpawnResourcesAuxValue(int i1)
        {
            return 0;
        }

        public virtual float GetExplosionResistance(Entity entity1)
        {
            return this.explosionResistance / 5F;
        }

        public virtual HitResult Clip(Level world1, int i2, int i3, int i4, Vec3 vec3D5, Vec3 vec3D6)
        {
            this.SetBlockBoundsBasedOnState(world1, i2, i3, i4);
            vec3D5 = vec3D5.AddVector((-i2), (-i3), (-i4));
            vec3D6 = vec3D6.AddVector((-i2), (-i3), (-i4));
            Vec3 vec3D7 = vec3D5.GetIntermediateWithXValue(vec3D6, this.minX);
            Vec3 vec3D8 = vec3D5.GetIntermediateWithXValue(vec3D6, this.maxX);
            Vec3 vec3D9 = vec3D5.GetIntermediateWithYValue(vec3D6, this.minY);
            Vec3 vec3D10 = vec3D5.GetIntermediateWithYValue(vec3D6, this.maxY);
            Vec3 vec3D11 = vec3D5.GetIntermediateWithZValue(vec3D6, this.minZ);
            Vec3 vec3D12 = vec3D5.GetIntermediateWithZValue(vec3D6, this.maxZ);
            if (!this.ContainsX(vec3D7))
            {
                vec3D7 = null;
            }

            if (!this.ContainsX(vec3D8))
            {
                vec3D8 = null;
            }

            if (!this.ContainsY(vec3D9))
            {
                vec3D9 = null;
            }

            if (!this.ContainsY(vec3D10))
            {
                vec3D10 = null;
            }

            if (!this.ContainsZ(vec3D11))
            {
                vec3D11 = null;
            }

            if (!this.ContainsZ(vec3D12))
            {
                vec3D12 = null;
            }

            Vec3 vec3D13 = null;
            if (vec3D7 != null && (vec3D13 == null || vec3D5.DistanceTo(vec3D7) < vec3D5.DistanceTo(vec3D13)))
            {
                vec3D13 = vec3D7;
            }

            if (vec3D8 != null && (vec3D13 == null || vec3D5.DistanceTo(vec3D8) < vec3D5.DistanceTo(vec3D13)))
            {
                vec3D13 = vec3D8;
            }

            if (vec3D9 != null && (vec3D13 == null || vec3D5.DistanceTo(vec3D9) < vec3D5.DistanceTo(vec3D13)))
            {
                vec3D13 = vec3D9;
            }

            if (vec3D10 != null && (vec3D13 == null || vec3D5.DistanceTo(vec3D10) < vec3D5.DistanceTo(vec3D13)))
            {
                vec3D13 = vec3D10;
            }

            if (vec3D11 != null && (vec3D13 == null || vec3D5.DistanceTo(vec3D11) < vec3D5.DistanceTo(vec3D13)))
            {
                vec3D13 = vec3D11;
            }

            if (vec3D12 != null && (vec3D13 == null || vec3D5.DistanceTo(vec3D12) < vec3D5.DistanceTo(vec3D13)))
            {
                vec3D13 = vec3D12;
            }

            if (vec3D13 == null)
            {
                return null;
            }
            else
            {
                TileFace b14 = TileFace.UNDEFINED;
                if (vec3D13 == vec3D7)
                {
                    b14 = TileFace.WEST;
                }

                if (vec3D13 == vec3D8)
                {
                    b14 = TileFace.EAST;
                }

                if (vec3D13 == vec3D9)
                {
                    b14 = TileFace.DOWN;
                }

                if (vec3D13 == vec3D10)
                {
                    b14 = TileFace.UP;
                }

                if (vec3D13 == vec3D11)
                {
                    b14 = TileFace.NORTH;
                }

                if (vec3D13 == vec3D12)
                {
                    b14 = TileFace.SOUTH;
                }

                return new HitResult(i2, i3, i4, b14, vec3D13.AddVector(i2, i3, i4));
            }
        }

        private bool ContainsX(Vec3 vec3D1)
        {
            return vec3D1 == null ? false : vec3D1.y >= this.minY && vec3D1.y <= this.maxY && vec3D1.z >= this.minZ && vec3D1.z <= this.maxZ;
        }

        private bool ContainsY(Vec3 vec3D1)
        {
            return vec3D1 == null ? false : vec3D1.x >= this.minX && vec3D1.x <= this.maxX && vec3D1.z >= this.minZ && vec3D1.z <= this.maxZ;
        }

        private bool ContainsZ(Vec3 vec3D1)
        {
            return vec3D1 == null ? false : vec3D1.x >= this.minX && vec3D1.x <= this.maxX && vec3D1.y >= this.minY && vec3D1.y <= this.maxY;
        }

        public virtual void WasExploded(Level world1, int i2, int i3, int i4)
        {
        }

        public virtual RenderLayer GetRenderLayer()
        {
            return RenderLayer.RENDERLAYER_OPAQUE;
        }

        public virtual bool CanPlaceBlockOnSide(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            return this.CanPlaceBlockAt(world1, i2, i3, i4);
        }

        public virtual bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetTile(i2, i3, i4);
            return i5 == 0 || tiles[i5].material.GetIsGroundCover();
        }

        public virtual bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            return false;
        }

        public virtual void StepOn(Level world1, int i2, int i3, int i4, Entity entity5)
        {
        }

        public virtual void OnBlockPlaced(Level world1, int i2, int i3, int i4, TileFace i5)
        {
        }

        public virtual void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
        }

        public virtual void HandleEntityInside(Level world1, int i2, int i3, int i4, Entity entity5, Vec3 vec3D6)
        {
        }

        public virtual void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
        }

        public virtual int GetColor(int i1)
        {
            return 0xFFFFFF;
        }

        public virtual int GetColor(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            return 0xFFFFFF;
        }

        public virtual bool GetDirectSignal(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return false;
        }

        public virtual bool IsSignalSource()
        {
            return false;
        }

        public virtual void OnEntityCollidedWithBlock(Level world1, int i2, int i3, int i4, Entity entity5)
        {
        }

        public virtual bool GetSignal(Level world1, int i2, int i3, int i4, int i5)
        {
            return false;
        }

        public virtual void SetBlockBoundsForItemRender()
        {
        }

        public virtual void HarvestBlock(Level world1, Player entityPlayer2, int i3, int i4, int i5, int i6)
        {
            entityPlayer2.AddStat(StatList.mineBlockStatArray[this.id], 1);
            this.DropBlockAsItem(world1, i3, i4, i5, i6);
        }

        public virtual bool CanBlockStay(Level world1, int i2, int i3, int i4)
        {
            return true;
        }

        public virtual void OnBlockPlacedBy(Level world1, int i2, int i3, int i4, Mob entityLiving5)
        {
        }

        public virtual Tile SetDescriptionId(string string1)
        {
            this.descriptionId = TILE_DESCRIPTION_PREFIX + string1;
            return this;
        }

        public virtual string GetName()
        {
            return Locale.TranslateKey(this.GetDescriptionId() + ".name");
        }

        public virtual string GetDescriptionId()
        {
            return this.descriptionId;
        }

        public virtual void TileEvent(Level world1, int i2, int i3, int i4, int i5, int i6)
        {
        }

        public virtual bool IsCollectStatistics()
        {
            return this.collectStatistics;
        }

        protected virtual Tile SetNotCollectStatistics()
        {
            this.collectStatistics = false;
            return this;
        }

        public virtual int GetPistonPushReaction()
        {
            return this.material.GetPushReaction();
        }

        static Tile()
        {
            Item.items[cloth.id] = (new ItemCloth(cloth.id - 256)).SetItemName("cloth");
            Item.items[treeTrunk.id] = (new ItemLog(treeTrunk.id - 256)).SetItemName("log");
            Item.items[stoneSlabHalf.id] = (new ItemSlab(stoneSlabHalf.id - 256)).SetItemName("stoneSlab");
            Item.items[sapling.id] = (new ItemSapling(sapling.id - 256)).SetItemName("sapling");
            Item.items[leaves.id] = (new ItemLeaves(leaves.id - 256)).SetItemName("leaves");
            Item.items[pistonBase.id] = new PistonTileItem(pistonBase.id - 256);
            Item.items[pistonStickyBase.id] = new PistonTileItem(pistonStickyBase.id - 256);
            for (int i0 = 0; i0 < 256; ++i0)
            {
                if (tiles[i0] != null && Item.items[i0] == null)
                {
                    Item.items[i0] = new TileItem(i0 - 256);
                    tiles[i0].Init();
                }
            }

            translucent[0] = true;
            StatList.InitBlockStats();
        }
    }
}