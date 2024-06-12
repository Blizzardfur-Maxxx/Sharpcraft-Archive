using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class RedstoneOreTile : Tile
    {
        private bool lit;
        public RedstoneOreTile(int i1, int i2, bool z3) : base(i1, i2, Material.stone)
        {
            if (z3)
            {
                this.SetTicking(true);
            }

            this.lit = z3;
        }

        public override int GetTickDelay()
        {
            return 30;
        }

        public override void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            this.F320h(world1, i2, i3, i4);
            base.OnBlockClicked(world1, i2, i3, i4, entityPlayer5);
        }

        public override void StepOn(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            this.F320h(world1, i2, i3, i4);
            base.StepOn(world1, i2, i3, i4, entity5);
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            this.F320h(world1, i2, i3, i4);
            return base.BlockActivated(world1, i2, i3, i4, entityPlayer5);
        }

        private void F320h(Level world1, int i2, int i3, int i4)
        {
            this.F319i(world1, i2, i3, i4);
            if (this.id == Tile.oreRedstone.id)
            {
                world1.SetTile(i2, i3, i4, Tile.oreRedstoneGlowing.id);
            }
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (this.id == Tile.oreRedstoneGlowing.id)
            {
                world1.SetTile(i2, i3, i4, Tile.oreRedstone.id);
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.redstone.id;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 4 + random1.NextInt(2);
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (this.lit)
            {
                this.F319i(world1, i2, i3, i4);
            }
        }

        private void F319i(Level world1, int i2, int i3, int i4)
        {
            JRandom random5 = world1.rand;
            double d6 = 0.0625;
            for (int i8 = 0; i8 < 6; ++i8)
            {
                double d9 = i2 + random5.NextFloat();
                double d11 = i3 + random5.NextFloat();
                double d13 = i4 + random5.NextFloat();
                if (i8 == 0 && !world1.IsSolidRenderTile(i2, i3 + 1, i4))
                {
                    d11 = i3 + 1 + d6;
                }

                if (i8 == 1 && !world1.IsSolidRenderTile(i2, i3 - 1, i4))
                {
                    d11 = i3 + 0 - d6;
                }

                if (i8 == 2 && !world1.IsSolidRenderTile(i2, i3, i4 + 1))
                {
                    d13 = i4 + 1 + d6;
                }

                if (i8 == 3 && !world1.IsSolidRenderTile(i2, i3, i4 - 1))
                {
                    d13 = i4 + 0 - d6;
                }

                if (i8 == 4 && !world1.IsSolidRenderTile(i2 + 1, i3, i4))
                {
                    d9 = i2 + 1 + d6;
                }

                if (i8 == 5 && !world1.IsSolidRenderTile(i2 - 1, i3, i4))
                {
                    d9 = i2 + 0 - d6;
                }

                if (d9 < i2 || d9 > i2 + 1 || d11 < 0 || d11 > i3 + 1 || d13 < i4 || d13 > i4 + 1)
                {
                    world1.AddParticle("reddust", d9, d11, d13, 0, 0, 0);
                }
            }
        }
    }
}