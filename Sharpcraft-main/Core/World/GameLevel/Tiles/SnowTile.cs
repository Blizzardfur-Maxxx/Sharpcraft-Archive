using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class SnowTile : Tile
    {
        public SnowTile(int i1, int i2) : base(i1, i2, Material.snow)
        {
            this.SetTicking(true);
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.snowball.id;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 4;
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (world1.GetSavedLightValue(LightLayer.Block, i2, i3, i4) > 11)
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
            }
        }
    }
}