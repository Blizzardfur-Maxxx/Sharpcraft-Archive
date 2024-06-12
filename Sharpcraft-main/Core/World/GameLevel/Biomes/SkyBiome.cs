using SharpCraft.Core.World.Entities.Animals;

namespace SharpCraft.Core.World.GameLevel.Biomes
{
    public class SkyBiome : Biome
    {
        public SkyBiome()
        {
            this.spawnableMonsterList.Clear();
            this.spawnableCreatureList.Clear();
            this.spawnableWaterCreatureList.Clear();
            this.spawnableCreatureList.Add(new MobSpawnerData(typeof(Chicken), 10));
        }

        public override int GetSkyColor(float f1)
        {
            return 0xC0C0FF;
        }
    }
}