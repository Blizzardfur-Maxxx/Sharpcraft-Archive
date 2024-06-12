using SharpCraft.Core.World.Entities.Monsters;

namespace SharpCraft.Core.World.GameLevel.Biomes
{
    public class HellBiome : Biome
    {
        public HellBiome()
        {
            this.spawnableMonsterList.Clear();
            this.spawnableCreatureList.Clear();
            this.spawnableWaterCreatureList.Clear();
            this.spawnableMonsterList.Add(new MobSpawnerData(typeof(Ghast), 10));
            this.spawnableMonsterList.Add(new MobSpawnerData(typeof(PigZombie), 10));
        }
    }
}