using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.GameLevel.LevelGen.Features;

namespace SharpCraft.Core.World.GameLevel.Biomes
{
    public class TaigaBiome : Biome
    {
        public TaigaBiome()
        {
            this.spawnableCreatureList.Add(new MobSpawnerData(typeof(Wolf), 2));
        }

        public override Feature GetTreeFeature(JRandom random1)
        {
            if (random1.NextInt(3) == 0)
            {
                return new PineFeature();
            }
            return new SpruceFeature();
            //return random1.NextInt(3) == 0 ? new PineFeature() : new SpruceFeature();
        }
    }
}