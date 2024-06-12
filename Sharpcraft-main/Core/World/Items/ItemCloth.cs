using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemCloth : TileItem
    {
        public ItemCloth(int i1) : base(i1)
        {
            this.SetMaxDamage(0);
            this.SetHasSubtypes(true);
        }

        public override int GetIconFromDamage(int i1)
        {
            return Tile.cloth.GetTexture(Util.Facing.TileFace.NORTH, ClothTile.GetMetadataColor0(i1));
        }

        public override int GetMetadata(int i1)
        {
            return i1;
        }

        public override string GetItemNameIS(ItemInstance itemStack1)
        {
            return base.GetItemName() + "." + ItemDye.COLOR_DESCS[ClothTile.GetMetadataColor0(itemStack1.GetItemDamage())];
        }
    }
}