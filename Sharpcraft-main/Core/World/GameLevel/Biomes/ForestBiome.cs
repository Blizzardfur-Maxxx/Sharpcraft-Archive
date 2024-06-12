using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.GameLevel.LevelGen.Features;

namespace SharpCraft.Core.World.GameLevel.Biomes
{
    public class ForestBiome : Biome
    {
        public ForestBiome()
        {
            this.spawnableCreatureList.Add(new MobSpawnerData(typeof(Wolf), 2));
        }

        public override Feature GetTreeFeature(JRandom random1)
        {
            if (random1.NextInt(5) == 0)
            {
                return new TreeFeature();
            }
            if (random1.NextInt(3) == 0)
            {
                return new BasicTree();
            }
            return new BirchFeature();

            //return random1.NextInt(5) == 0 ? new TreeFeature() : (random1.NextInt(3) == 0 ? new BasicTree() : new BirchFeature());
        }
    }
}