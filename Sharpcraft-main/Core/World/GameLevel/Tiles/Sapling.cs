using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.LevelGen.Features;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class Sapling : Bush
    {
        public Sapling(int i1, int i2) : base(i1, i2)
        {
            float f3 = 0.4F;
            this.SetShape(0.5F - f3, 0F, 0.5F - f3, 0.5F + f3, f3 * 2F, 0.5F + f3);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (!world1.isRemote)
            {
                base.Tick(world1, i2, i3, i4, random5);
                if (world1.GetRawBrightness(i2, i3 + 1, i4) >= 9 && random5.NextInt(30) == 0)
                {
                    int i6 = world1.GetData(i2, i3, i4);
                    if ((i6 & 8) == 0)
                    {
                        world1.SetData(i2, i3, i4, i6 | 8);
                    }
                    else
                    {
                        this.GrowTree(world1, i2, i3, i4, random5);
                    }
                }
            }
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            i2 &= 3;
            return i2 == 1 ? 63 : (i2 == 2 ? 79 : base.GetTexture(faceIdx, i2));
        }

        public virtual void GrowTree(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            int i6 = world1.GetData(i2, i3, i4) & 3;
            world1.SetTileNoUpdate(i2, i3, i4, 0);
            object object7 = null;
            if (i6 == 1)
            {
                object7 = new SpruceFeature();
            }
            else if (i6 == 2)
            {
                object7 = new TreeFeature();
            }
            else
            {
                object7 = new BirchFeature();
                if (random5.NextInt(10) == 0)
                {
                    object7 = new BasicTree();
                }
            }

            if (!((Feature)object7).Place(world1, random5, i2, i3, i4))
            {
                world1.SetTileAndDataNoUpdate(i2, i3, i4, this.id, i6);
            }
        }

        protected override int GetSpawnResourcesAuxValue(int i1)
        {
            return i1 & 3;
        }
    }
}