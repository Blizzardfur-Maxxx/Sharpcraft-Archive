using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemSlab : TileItem
    {
        public ItemSlab(int i1) : base(i1)
        {
            this.SetMaxDamage(0);
            this.SetHasSubtypes(true);
        }

        public override int GetIconFromDamage(int i1)
        {
            return Tile.stoneSlabHalf.GetTexture(Util.Facing.TileFace.NORTH, i1);
        }

        public override int GetMetadata(int i1)
        {
            return i1;
        }

        public override string GetItemNameIS(ItemInstance itemStack1)
        {
            return base.GetItemName() + "." + StoneSlabTile.SLAB_NAMES[itemStack1.GetItemDamage()];
        }
    }
}