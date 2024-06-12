using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class MobSpawnerTile : EntityTile
    {
        public MobSpawnerTile(int i1, int i2) : base(i1, i2, Material.stone)
        {
        }

        protected override TileEntity NewTileEntity()
        {
            return new TileEntityMobSpawner();
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return 0;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        public override bool IsSolidRender()
        {
            return false;
        }
    }
}