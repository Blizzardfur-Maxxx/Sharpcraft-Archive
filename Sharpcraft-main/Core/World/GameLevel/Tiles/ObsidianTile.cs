using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class ObsidianTile : StoneTile
    {
        public ObsidianTile(int i1, int i2) : base(i1, i2)
        {
        }

        public override int ResourceCount(JRandom random1)
        {
            return 1;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.obsidian.id;
        }
    }
}