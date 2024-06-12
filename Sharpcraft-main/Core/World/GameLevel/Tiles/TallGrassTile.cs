using SharpCraft.Core.Util;
using SharpCraft.Core.World.Items;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class TallGrassTile : Bush
    {
        public TallGrassTile(int i1, int i2) : base(i1, i2)
        {
            float f3 = 0.4F;
            this.SetShape(0.5F - f3, 0F, 0.5F - f3, 0.5F + f3, 0.8F, 0.5F + f3);
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return i2 == 1 ? this.texture : (i2 == 2 ? this.texture + 16 + 1 : (i2 == 0 ? this.texture + 16 : this.texture));
        }

        public override int GetColor(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            int i5 = iBlockAccess1.GetData(i2, i3, i4);
            if (i5 == 0)
            {
                return 0xFFFFFF;
            }
            else
            {
                long j6 = i2 * 3129871 + i4 * 6129781 + i3;
                j6 = j6 * j6 * 42317861 + j6 * 11;
                i2 = (int)(i2 + (j6 >> 14 & 31));
                i3 = (int)(i3 + (j6 >> 19 & 31));
                i4 = (int)(i4 + (j6 >> 24 & 31));
                iBlockAccess1.GetBiomeSource().Func_a(i2, i4, 1, 1);
                double d8 = iBlockAccess1.GetBiomeSource().temperature[0];
                double d10 = iBlockAccess1.GetBiomeSource().humidity[0];
                return GrassColor.GetColor(d8, d10);
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return random2.NextInt(8) == 0 ? Item.seeds.id : -1;
        }
    }
}