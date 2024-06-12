using SharpCraft.Core.Util;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class Mushroom : Bush
    {
        public Mushroom(int i1, int i2) : base(i1, i2)
        {
            float f3 = 0.2F;
            this.SetShape(0.5F - f3, 0F, 0.5F - f3, 0.5F + f3, f3 * 2F, 0.5F + f3);
            this.SetTicking(true);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (random5.NextInt(100) == 0)
            {
                int i6 = i2 + random5.NextInt(3) - 1;
                int i7 = i3 + random5.NextInt(2) - random5.NextInt(2);
                int i8 = i4 + random5.NextInt(3) - 1;
                if (world1.IsAirBlock(i6, i7, i8) && this.CanBlockStay(world1, i6, i7, i8))
                {
                    random5.NextInt(3);
                    random5.NextInt(3);
                    if (world1.IsAirBlock(i6, i7, i8) && this.CanBlockStay(world1, i6, i7, i8))
                    {
                        world1.SetTile(i6, i7, i8, this.id);
                    }
                }
            }
        }

        protected override bool CanThisPlantGrowOnThisBlockID(int i1)
        {
            return Tile.solid[i1];
        }

        public override bool CanBlockStay(Level world1, int i2, int i3, int i4)
        {
            return i3 >= 0 && i3 < 128 ? world1.IsSkyLit(i2, i3, i4) < 13 && this.CanThisPlantGrowOnThisBlockID(world1.GetTile(i2, i3 - 1, i4)) : false;
        }
    }
}