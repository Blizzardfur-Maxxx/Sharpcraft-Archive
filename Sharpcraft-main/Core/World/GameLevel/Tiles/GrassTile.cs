using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class GrassTile : Tile
    {
        public GrassTile(int i1) : base(i1, Material.grass)
        {
            this.texture = 3;
            this.SetTicking(true);
        }

        public override int GetBlockTexture(ILevelSource iBlockAccess1, int i2, int i3, int i4, TileFace i5)
        {
            if (i5 == TileFace.UP)
            {
                return 0;
            }
            else if (i5 == TileFace.DOWN)
            {
                return 2;
            }
            else
            {
                Material material6 = iBlockAccess1.GetMaterial(i2, i3 + 1, i4);
                return material6 != Material.topSnow && material6 != Material.snow ? 3 : 68;
            }
        }

        public override int GetColor(ILevelSource ls, int i2, int i3, int i4)
        {
            ls.GetBiomeSource().Func_a(i2, i4, 1, 1);
            double d5 = ls.GetBiomeSource().temperature[0];
            double d7 = ls.GetBiomeSource().humidity[0];
            return GrassColor.GetColor(d5, d7);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (!world1.isRemote)
            {
                if (world1.GetRawBrightness(i2, i3 + 1, i4) < 4 && Tile.lightBlock[world1.GetTile(i2, i3 + 1, i4)] > 2)
                {
                    if (random5.NextInt(4) != 0)
                    {
                        return;
                    }

                    world1.SetTile(i2, i3, i4, Tile.dirt.id);
                }
                else if (world1.GetRawBrightness(i2, i3 + 1, i4) >= 9)
                {
                    int i6 = i2 + random5.NextInt(3) - 1;
                    int i7 = i3 + random5.NextInt(5) - 3;
                    int i8 = i4 + random5.NextInt(3) - 1;
                    int i9 = world1.GetTile(i6, i7 + 1, i8);
                    if (world1.GetTile(i6, i7, i8) == Tile.dirt.id && world1.GetRawBrightness(i6, i7 + 1, i8) >= 4 && Tile.lightBlock[i9] <= 2)
                    {
                        world1.SetTile(i6, i7, i8, Tile.grass.id);
                    }
                }
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.dirt.GetResource(0, random2);
        }
    }
}