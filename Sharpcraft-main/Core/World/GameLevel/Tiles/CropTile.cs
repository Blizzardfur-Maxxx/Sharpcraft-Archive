using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Items;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class CropTile : Bush
    {
        public CropTile(int i1, int i2) : base(i1, i2)
        {
            this.texture = i2;
            this.SetTicking(true);
            float f3 = 0.5F;
            this.SetShape(0.5F - f3, 0F, 0.5F - f3, 0.5F + f3, 0.25F, 0.5F + f3);
        }

        protected override bool CanThisPlantGrowOnThisBlockID(int i1)
        {
            return i1 == Tile.farmland.id;
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            base.Tick(world1, i2, i3, i4, random5);
            if (world1.GetRawBrightness(i2, i3 + 1, i4) >= 9)
            {
                int i6 = world1.GetData(i2, i3, i4);
                if (i6 < 7)
                {
                    float f7 = this.GetGrowthRate(world1, i2, i3, i4);
                    if (random5.NextInt((int)(100F / f7)) == 0)
                    {
                        ++i6;
                        world1.SetData(i2, i3, i4, i6);
                    }
                }
            }
        }

        public virtual void Fertilize(Level world1, int i2, int i3, int i4)
        {
            world1.SetData(i2, i3, i4, 7);
        }

        private float GetGrowthRate(Level world1, int i2, int i3, int i4)
        {
            float f5 = 1F;
            int i6 = world1.GetTile(i2, i3, i4 - 1);
            int i7 = world1.GetTile(i2, i3, i4 + 1);
            int i8 = world1.GetTile(i2 - 1, i3, i4);
            int i9 = world1.GetTile(i2 + 1, i3, i4);
            int i10 = world1.GetTile(i2 - 1, i3, i4 - 1);
            int i11 = world1.GetTile(i2 + 1, i3, i4 - 1);
            int i12 = world1.GetTile(i2 + 1, i3, i4 + 1);
            int i13 = world1.GetTile(i2 - 1, i3, i4 + 1);
            bool z14 = i8 == this.id || i9 == this.id;
            bool z15 = i6 == this.id || i7 == this.id;
            bool z16 = i10 == this.id || i11 == this.id || i12 == this.id || i13 == this.id;
            for (int i17 = i2 - 1; i17 <= i2 + 1; ++i17)
            {
                for (int i18 = i4 - 1; i18 <= i4 + 1; ++i18)
                {
                    int i19 = world1.GetTile(i17, i3 - 1, i18);
                    float f20 = 0F;
                    if (i19 == Tile.farmland.id)
                    {
                        f20 = 1F;
                        if (world1.GetData(i17, i3 - 1, i18) > 0)
                        {
                            f20 = 3F;
                        }
                    }

                    if (i17 != i2 || i18 != i4)
                    {
                        f20 /= 4F;
                    }

                    f5 += f20;
                }
            }

            if (z16 || z14 && z15)
            {
                f5 /= 2F;
            }

            return f5;
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            if (i2 < 0)
            {
                i2 = 7;
            }

            return this.texture + i2;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.CROPS;
        }

        public override void SpawnResources(Level world1, int i2, int i3, int i4, int i5, float f6)
        {
            base.SpawnResources(world1, i2, i3, i4, i5, f6);
            if (!world1.isRemote)
            {
                for (int i7 = 0; i7 < 3; ++i7)
                {
                    if (world1.rand.NextInt(15) <= i5)
                    {
                        float f8 = 0.7F;
                        float f9 = world1.rand.NextFloat() * f8 + (1F - f8) * 0.5F;
                        float f10 = world1.rand.NextFloat() * f8 + (1F - f8) * 0.5F;
                        float f11 = world1.rand.NextFloat() * f8 + (1F - f8) * 0.5F;
                        ItemEntity entityItem12 = new ItemEntity(world1, i2 + f9, i3 + f10, i4 + f11, new ItemInstance(Item.seeds));
                        entityItem12.delayBeforeCanPickup = 10;
                        world1.AddEntity(entityItem12);
                    }
                }
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return i1 == 7 ? Item.wheat.id : -1;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 1;
        }
    }
}