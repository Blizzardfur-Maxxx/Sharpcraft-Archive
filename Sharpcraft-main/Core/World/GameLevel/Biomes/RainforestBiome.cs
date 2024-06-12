using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.LevelGen.Features;

namespace SharpCraft.Core.World.GameLevel.Biomes
{
    public class RainforestBiome : Biome
    {
        public override Feature GetTreeFeature(JRandom random1)
        {
            if (random1.NextInt(3) == 0)
            {
                return new BasicTree();
            }
            return new BirchFeature();
            //return random1.NextInt(3) == 0 ? new BasicTree() : new BirchFeature();
        }
    }
}