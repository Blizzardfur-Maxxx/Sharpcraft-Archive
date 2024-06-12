using SharpCraft.Core.World.GameLevel.Materials;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class SpongeTile : Tile
    {
        public SpongeTile(int i1) : base(i1, Material.sponge)
        {
            this.texture = 48;
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            byte b5 = 2;
            for (int i6 = i2 - b5; i6 <= i2 + b5; ++i6)
            {
                for (int i7 = i3 - b5; i7 <= i3 + b5; ++i7)
                {
                    for (int i8 = i4 - b5; i8 <= i4 + b5; ++i8)
                    {
                        if (world1.GetMaterial(i6, i7, i8) == Material.water)
                        {
                        }
                    }
                }
            }
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            byte b5 = 2;
            for (int i6 = i2 - b5; i6 <= i2 + b5; ++i6)
            {
                for (int i7 = i3 - b5; i7 <= i3 + b5; ++i7)
                {
                    for (int i8 = i4 - b5; i8 <= i4 + b5; ++i8)
                    {
                        world1.UpdateNeighborsAt(i6, i7, i8, world1.GetTile(i6, i7, i8));
                    }
                }
            }
        }
    }
}