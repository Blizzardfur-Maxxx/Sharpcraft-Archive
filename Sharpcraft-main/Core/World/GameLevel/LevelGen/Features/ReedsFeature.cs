using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.GameLevel.LevelGen.Features
{
    public class ReedsFeature : Feature
    {
        public override bool Place(Level world1, JRandom random2, int i3, int i4, int i5)
        {
            for (int i6 = 0; i6 < 20; ++i6)
            {
                int i7 = i3 + random2.NextInt(4) - random2.NextInt(4);
                int i8 = i4;
                int i9 = i5 + random2.NextInt(4) - random2.NextInt(4);
                if (world1.IsAirBlock(i7, i4, i9) && (world1.GetMaterial(i7 - 1, i4 - 1, i9) == Material.water || world1.GetMaterial(i7 + 1, i4 - 1, i9) == Material.water || world1.GetMaterial(i7, i4 - 1, i9 - 1) == Material.water || world1.GetMaterial(i7, i4 - 1, i9 + 1) == Material.water))
                {
                    int i10 = 2 + random2.NextInt(random2.NextInt(3) + 1);
                    for (int i11 = 0; i11 < i10; ++i11)
                    {
                        if (Tile.reed.CanBlockStay(world1, i7, i8 + i11, i9))
                        {
                            world1.SetTileNoUpdate(i7, i8 + i11, i9, Tile.reed.id);
                        }
                    }
                }
            }

            return true;
        }
    }
}