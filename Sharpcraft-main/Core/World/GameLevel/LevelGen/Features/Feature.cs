using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public abstract class Feature
    {
        public abstract bool Place(Level world1, JRandom random2, int i3, int i4, int i5);
        public virtual void Init(double d1, double d3, double d5)
        {
        }
    }
}