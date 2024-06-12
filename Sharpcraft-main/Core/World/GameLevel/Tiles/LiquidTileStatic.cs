using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class LiquidTileStatic : LiquidTile
    {
        public LiquidTileStatic(int i1, Material material2) : base(i1, material2)
        {
            this.SetTicking(false);
            if (material2 == Material.lava)
            {
                this.SetTicking(true);
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            base.NeighborChanged(world1, i2, i3, i4, i5);
            if (world1.GetTile(i2, i3, i4) == this.id)
            {
                this.Update(world1, i2, i3, i4);
            }
        }

        private void Update(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            world1.editingBlocks = true;
            world1.SetTileAndDataNoUpdate(i2, i3, i4, this.id - 1, i5);
            world1.SetTilesDirty(i2, i3, i4, i2, i3, i4);
            world1.ScheduleBlockUpdate(i2, i3, i4, this.id - 1, this.GetTickDelay());
            world1.editingBlocks = false;
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (this.material == Material.lava)
            {
                int i6 = random5.NextInt(3);
                for (int i7 = 0; i7 < i6; ++i7)
                {
                    i2 += random5.NextInt(3) - 1;
                    ++i3;
                    i4 += random5.NextInt(3) - 1;
                    int i8 = world1.GetTile(i2, i3, i4);
                    if (i8 == 0)
                    {
                        if (this.IsFlammable(world1, i2 - 1, i3, i4) || this.IsFlammable(world1, i2 + 1, i3, i4) || this.IsFlammable(world1, i2, i3, i4 - 1) || this.IsFlammable(world1, i2, i3, i4 + 1) || this.IsFlammable(world1, i2, i3 - 1, i4) || this.IsFlammable(world1, i2, i3 + 1, i4))
                        {
                            world1.SetTile(i2, i3, i4, Tile.fire.id);
                            return;
                        }
                    }
                    else if (Tile.tiles[i8].material.BlocksMotion())
                    {
                        return;
                    }
                }
            }
        }

        private bool IsFlammable(Level world1, int i2, int i3, int i4)
        {
            return world1.GetMaterial(i2, i3, i4).IsFlammable();
        }
    }
}