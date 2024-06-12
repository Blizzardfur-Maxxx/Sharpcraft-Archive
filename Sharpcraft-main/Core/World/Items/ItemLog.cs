using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemLog : TileItem
    {
        public ItemLog(int i1) : base(i1)
        {
            this.SetMaxDamage(0);
            this.SetHasSubtypes(true);
        }

        public override int GetIconFromDamage(int i1)
        {
            return Tile.treeTrunk.GetTexture(Util.Facing.TileFace.NORTH, i1);
        }

        public override int GetMetadata(int i1)
        {
            return i1;
        }
    }
}