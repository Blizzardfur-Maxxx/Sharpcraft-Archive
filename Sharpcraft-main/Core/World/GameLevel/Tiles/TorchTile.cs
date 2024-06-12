using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class TorchTile : Tile
    {
        public TorchTile(int i1, int i2) : base(i1, i2, Material.decoration)
        {
            this.SetTicking(true);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override RenderShape GetRenderShape()
        {
            return RenderShape.TORCH;
        }

        private bool CheckFence(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2, i3, i4) || world1.GetTile(i2, i3, i4) == Tile.fence.id;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2 - 1, i3, i4) ? true : (world1.IsSolidBlockingTile(i2 + 1, i3, i4) ? true : (world1.IsSolidBlockingTile(i2, i3, i4 - 1) ? true : (world1.IsSolidBlockingTile(i2, i3, i4 + 1) ? true : this.CheckFence(world1, i2, i3 - 1, i4))));
        }

        public override void OnBlockPlaced(Level world1, int i2, int i3, int i4, TileFace i5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            if (i5 == TileFace.UP && this.CheckFence(world1, i2, i3 - 1, i4))
            {
                i6 = 5;
            }

            if (i5 == TileFace.NORTH && world1.IsSolidBlockingTile(i2, i3, i4 + 1))
            {
                i6 = 4;
            }

            if (i5 == TileFace.SOUTH && world1.IsSolidBlockingTile(i2, i3, i4 - 1))
            {
                i6 = 3;
            }

            if (i5 == TileFace.WEST && world1.IsSolidBlockingTile(i2 + 1, i3, i4))
            {
                i6 = 2;
            }

            if (i5 == TileFace.EAST && world1.IsSolidBlockingTile(i2 - 1, i3, i4))
            {
                i6 = 1;
            }

            world1.SetData(i2, i3, i4, i6);
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            base.Tick(world1, i2, i3, i4, random5);
            if (world1.GetData(i2, i3, i4) == 0)
            {
                this.OnPlace(world1, i2, i3, i4);
            }
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            if (world1.IsSolidBlockingTile(i2 - 1, i3, i4))
            {
                world1.SetData(i2, i3, i4, 1);
            }
            else if (world1.IsSolidBlockingTile(i2 + 1, i3, i4))
            {
                world1.SetData(i2, i3, i4, 2);
            }
            else if (world1.IsSolidBlockingTile(i2, i3, i4 - 1))
            {
                world1.SetData(i2, i3, i4, 3);
            }
            else if (world1.IsSolidBlockingTile(i2, i3, i4 + 1))
            {
                world1.SetData(i2, i3, i4, 4);
            }
            else if (this.CheckFence(world1, i2, i3 - 1, i4))
            {
                world1.SetData(i2, i3, i4, 5);
            }

            this.DropTorchIfCantStay(world1, i2, i3, i4);
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (this.DropTorchIfCantStay(world1, i2, i3, i4))
            {
                int i6 = world1.GetData(i2, i3, i4);
                bool z7 = false;
                if (!world1.IsSolidBlockingTile(i2 - 1, i3, i4) && i6 == 1)
                {
                    z7 = true;
                }

                if (!world1.IsSolidBlockingTile(i2 + 1, i3, i4) && i6 == 2)
                {
                    z7 = true;
                }

                if (!world1.IsSolidBlockingTile(i2, i3, i4 - 1) && i6 == 3)
                {
                    z7 = true;
                }

                if (!world1.IsSolidBlockingTile(i2, i3, i4 + 1) && i6 == 4)
                {
                    z7 = true;
                }

                if (!this.CheckFence(world1, i2, i3 - 1, i4) && i6 == 5)
                {
                    z7 = true;
                }

                if (z7)
                {
                    this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                    world1.SetTile(i2, i3, i4, 0);
                }
            }
        }

        private bool DropTorchIfCantStay(Level world1, int i2, int i3, int i4)
        {
            if (!this.CanPlaceBlockAt(world1, i2, i3, i4))
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override HitResult Clip(Level world1, int i2, int i3, int i4, Vec3 vec3D5, Vec3 vec3D6)
        {
            int i7 = world1.GetData(i2, i3, i4) & 7;
            float f8 = 0.15F;
            if (i7 == 1)
            {
                this.SetShape(0F, 0.2F, 0.5F - f8, f8 * 2F, 0.8F, 0.5F + f8);
            }
            else if (i7 == 2)
            {
                this.SetShape(1F - f8 * 2F, 0.2F, 0.5F - f8, 1F, 0.8F, 0.5F + f8);
            }
            else if (i7 == 3)
            {
                this.SetShape(0.5F - f8, 0.2F, 0F, 0.5F + f8, 0.8F, f8 * 2F);
            }
            else if (i7 == 4)
            {
                this.SetShape(0.5F - f8, 0.2F, 1F - f8 * 2F, 0.5F + f8, 0.8F, 1F);
            }
            else
            {
                f8 = 0.1F;
                this.SetShape(0.5F - f8, 0F, 0.5F - f8, 0.5F + f8, 0.6F, 0.5F + f8);
            }

            return base.Clip(world1, i2, i3, i4, vec3D5, vec3D6);
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            double d7 = i2 + 0.5F;
            double d9 = i3 + 0.7F;
            double d11 = i4 + 0.5F;
            double d13 = 0.22F;
            double d15 = 0.27F;
            if (i6 == 1)
            {
                world1.AddParticle("smoke", d7 - d15, d9 + d13, d11, 0, 0, 0);
                world1.AddParticle("flame", d7 - d15, d9 + d13, d11, 0, 0, 0);
            }
            else if (i6 == 2)
            {
                world1.AddParticle("smoke", d7 + d15, d9 + d13, d11, 0, 0, 0);
                world1.AddParticle("flame", d7 + d15, d9 + d13, d11, 0, 0, 0);
            }
            else if (i6 == 3)
            {
                world1.AddParticle("smoke", d7, d9 + d13, d11 - d15, 0, 0, 0);
                world1.AddParticle("flame", d7, d9 + d13, d11 - d15, 0, 0, 0);
            }
            else if (i6 == 4)
            {
                world1.AddParticle("smoke", d7, d9 + d13, d11 + d15, 0, 0, 0);
                world1.AddParticle("flame", d7, d9 + d13, d11 + d15, 0, 0, 0);
            }
            else
            {
                world1.AddParticle("smoke", d7, d9, d11, 0, 0, 0);
                world1.AddParticle("flame", d7, d9, d11, 0, 0, 0);
            }
        }
    }
}