using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Monsters;
using SharpCraft.Core.World.GameLevel.LevelGen.Features;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel.Biomes
{
    public class Biome
    {
        public static readonly Biome rainforest = (new RainforestBiome()).SetColor(588342).SetName("Rainforest").SetViewerColor(2094168);
        public static readonly Biome swampland = (new SwampBiome()).SetColor(522674).SetName("Swampland").SetViewerColor(9154376);
        public static readonly Biome seasonalForest = (new Biome()).SetColor(10215459).SetName("Seasonal Forest");
        public static readonly Biome forest = (new ForestBiome()).SetColor(353825).SetName("Forest").SetViewerColor(5159473);
        public static readonly Biome savanna = (new FlatBiome()).SetColor(14278691).SetName("Savanna");
        public static readonly Biome shrubland = (new Biome()).SetColor(10595616).SetName("Shrubland");
        public static readonly Biome taiga = (new TaigaBiome()).SetColor(3060051).SetName("Taiga").SetSnowCovered().SetViewerColor(8107825);
        public static readonly Biome desert = (new FlatBiome()).SetColor(16421912).SetName("Desert").SetDisableRain();
        public static readonly Biome plains = (new FlatBiome()).SetColor(16767248).SetName("Plains");
        public static readonly Biome iceDesert = (new FlatBiome()).SetColor(16772499).SetName("Ice Desert").SetSnowCovered().SetDisableRain().SetViewerColor(12899129);
        public static readonly Biome tundra = (new Biome()).SetColor(5762041).SetName("Tundra").SetSnowCovered().SetViewerColor(12899129);
        public static readonly Biome hell = (new HellBiome()).SetColor(16711680).SetName("Hell").SetDisableRain();
        public static readonly Biome sky = (new SkyBiome()).SetColor(8421631).SetName("Sky").SetDisableRain();
        public string name;
        public int color;
        public byte topTile = (byte)Tile.grass.id;
        public byte fillerTile = (byte)Tile.dirt.id;
        public int viewerColor = 5169201;
        protected IList<MobSpawnerData> spawnableMonsterList = new List<MobSpawnerData>();
        protected IList<MobSpawnerData> spawnableCreatureList = new List<MobSpawnerData>();
        protected IList<MobSpawnerData> spawnableWaterCreatureList = new List<MobSpawnerData>();
        private bool snowCovered;
        private bool enableRain = true;
        private static Biome[] map = new Biome[4096];
        public class MobSpawnerData
        {
            public Type entityClass;
            public int spawnRarityRate;
            public MobSpawnerData(Type class1, int i2)
            {
                this.entityClass = class1;
                this.spawnRarityRate = i2;
            }
        }

        protected Biome()
        {
            this.spawnableMonsterList.Add(new MobSpawnerData(typeof(Spider), 10));
            this.spawnableMonsterList.Add(new MobSpawnerData(typeof(Zombie), 10));
            this.spawnableMonsterList.Add(new MobSpawnerData(typeof(Skeleton), 10));
            this.spawnableMonsterList.Add(new MobSpawnerData(typeof(Creeper), 10));
            this.spawnableMonsterList.Add(new MobSpawnerData(typeof(Slime), 10));
            this.spawnableCreatureList.Add(new MobSpawnerData(typeof(Sheep), 12));
            this.spawnableCreatureList.Add(new MobSpawnerData(typeof(Pig), 10));
            this.spawnableCreatureList.Add(new MobSpawnerData(typeof(Chicken), 10));
            this.spawnableCreatureList.Add(new MobSpawnerData(typeof(Cow), 8));
            this.spawnableWaterCreatureList.Add(new MobSpawnerData(typeof(Squid), 10));
        }

        private Biome SetDisableRain()
        {
            this.enableRain = false;
            return this;
        }

        public static void Recalc()
        {
            for (int i0 = 0; i0 < 64; ++i0)
            {
                for (int i1 = 0; i1 < 64; ++i1)
                {
                    map[i0 + i1 * 64] = _getBiome(i0 / 63F, i1 / 63F);
                }
            }

            desert.topTile = desert.fillerTile = (byte)Tile.sand.id;
            iceDesert.topTile = iceDesert.fillerTile = (byte)Tile.sand.id;
        }

        public virtual Feature GetTreeFeature(JRandom random1)
        {
            if (random1.NextInt(10) == 0)
            {
                return new BasicTree();
            }
            return new BirchFeature();

            //return (random1.NextInt(10) == 0 ? new BasicTree() : new BirchFeature()); //fuck you
        }

        protected virtual Biome SetSnowCovered()
        {
            this.snowCovered = true;
            return this;
        }

        protected virtual Biome SetName(string string1)
        {
            this.name = string1;
            return this;
        }

        protected virtual Biome SetViewerColor(int i1)
        {
            this.viewerColor = i1;
            return this;
        }

        protected virtual Biome SetColor(int i1)
        {
            this.color = i1;
            return this;
        }

        public static Biome GetBiome(double hum, double temp)
        {
            int h = (int)(hum * 63);
            int t = (int)(temp * 63);
            return map[h + t * 64];
        }

        public static Biome _getBiome(float temp, float humd)
        {
            humd *= temp;
            return temp < 0.1F ? tundra : (humd < 0.2F ? (temp < 0.5F ? tundra : (temp < 0.95F ? savanna : desert)) : (humd > 0.5F && temp < 0.7F ? swampland : (temp < 0.5F ? taiga : (temp < 0.97F ? (humd < 0.35F ? shrubland : forest) : (humd < 0.45F ? plains : (humd < 0.9F ? seasonalForest : rainforest))))));
        }

        public virtual int GetSkyColor(float temperature)
        {
            temperature /= 3F;
            if (temperature < -1F)
            {
                temperature = -1F;
            }

            if (temperature > 1F)
            {
                temperature = 1F;
            }

            return ColorExtensions.HSBtoRGB(0.62222224F - temperature * 0.05F, 0.5F + temperature * 0.1F, 1F);
        }

        public virtual IList<MobSpawnerData> GetMobs(MobCategory enumCreatureType1)
        {
            return enumCreatureType1 == MobCategory.monster ? this.spawnableMonsterList : (enumCreatureType1 == MobCategory.creature ? this.spawnableCreatureList : (enumCreatureType1 == MobCategory.waterCreature ? this.spawnableWaterCreatureList : null));
        }

        public virtual bool IsSnowCovered()
        {
            return this.snowCovered;
        }

        public virtual bool CanThunder()
        {
            return this.snowCovered ? false : this.enableRain;
        }

        static Biome()
        {
            Recalc();
        }
    }
}