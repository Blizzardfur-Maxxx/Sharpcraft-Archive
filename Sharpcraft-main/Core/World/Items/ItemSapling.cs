using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemSapling : TileItem
    {
        public ItemSapling(int i1) : base(i1)
        {
            this.SetMaxDamage(0);
            this.SetHasSubtypes(true);
        }

        public override int GetMetadata(int i1)
        {
            return i1;
        }

        public override int GetIconFromDamage(int i1)
        {
            return Tile.sapling.GetTexture(0, i1);
        }
    }
}