using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public abstract class EntityTile : Tile
    {
        protected EntityTile(int i1, Material material2) : base(i1, material2)
        {
            isEntityTile[i1] = true;
        }

        protected EntityTile(int i1, int i2, Material material3) : base(i1, i2, material3)
        {
            isEntityTile[i1] = true;
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            base.OnPlace(world1, i2, i3, i4);
            world1.SetTileEntity(i2, i3, i4, this.NewTileEntity());
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            base.OnBlockRemoval(world1, i2, i3, i4);
            world1.RemoveTileEntity(i2, i3, i4);
        }

        protected abstract TileEntity NewTileEntity();
    }
}