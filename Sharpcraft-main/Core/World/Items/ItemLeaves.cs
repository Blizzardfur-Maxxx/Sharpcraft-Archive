using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Items
{
    public class ItemLeaves : TileItem
    {
        public ItemLeaves(int i1) : base(i1)
        {
            this.SetMaxDamage(0);
            this.SetHasSubtypes(true);
        }

        public override int GetMetadata(int i1)
        {
            return i1 | 8;
        }

        public override int GetIconFromDamage(int i1)
        {
            return Tile.leaves.GetTexture(0, i1);
        }

        public override int GetColorFromDamage(int i1)
        {
            return (i1 & 1) == 1 ? FoliageColor.GetFoliageColorPine() : ((i1 & 2) == 2 ? FoliageColor.GetFoliageColorBirch() : FoliageColor.GetItemColor());
        }
    }
}