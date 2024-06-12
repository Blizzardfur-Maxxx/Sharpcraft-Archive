using SharpCraft.Core.Util;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class GravelTile : SandTile
    {
        public GravelTile(int i1, int i2) : base(i1, i2)
        {
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return random2.NextInt(10) == 0 ? Item.flint.id : this.id;
        }
    }
}