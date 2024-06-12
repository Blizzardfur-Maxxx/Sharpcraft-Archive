using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.GameLevel.Materials;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class SandTile : Tile
    {
        public static bool fallInstantly = false;
        public SandTile(int i1, int i2) : base(i1, i2, Material.sand)
        {
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            this.TryToFall(world1, i2, i3, i4);
        }

        private void TryToFall(Level world1, int i2, int i3, int i4)
        {
            if (CanFallBelow(world1, i2, i3 - 1, i4) && i3 >= 0)
            {
                byte b8 = 32;
                if (!fallInstantly && world1.CheckChunksExist(i2 - b8, i3 - b8, i4 - b8, i2 + b8, i3 + b8, i4 + b8))
                {
                    FallingTile entityFallingSand9 = new FallingTile(world1, i2 + 0.5F, i3 + 0.5F, i4 + 0.5F, this.id);
                    world1.AddEntity(entityFallingSand9);
                }
                else
                {
                    world1.SetTile(i2, i3, i4, 0);
                    while (CanFallBelow(world1, i2, i3 - 1, i4) && i3 > 0)
                    {
                        --i3;
                    }

                    if (i3 > 0)
                    {
                        world1.SetTile(i2, i3, i4, this.id);
                    }
                }
            }
        }

        public override int GetTickDelay()
        {
            return 3;
        }

        public static bool CanFallBelow(Level world0, int i1, int i2, int i3)
        {
            int i4 = world0.GetTile(i1, i2, i3);
            if (i4 == 0)
            {
                return true;
            }
            else if (i4 == Tile.fire.id)
            {
                return true;
            }
            else
            {
                Material material5 = Tile.tiles[i4].material;
                return material5 == Material.water ? true : material5 == Material.lava;
            }
        }
    }
}