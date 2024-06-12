namespace SharpCraft.Core.World.Items
{
    public class PistonTileItem : TileItem
    {
        public PistonTileItem(int i1) : base(i1)
        {
        }

        public override int GetMetadata(int i1)
        {
            return 7;
        }
    }
}