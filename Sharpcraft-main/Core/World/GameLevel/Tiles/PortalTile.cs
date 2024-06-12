using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class PortalTile : TransparentTile
    {
        public PortalTile(int i1, int i2) : base(i1, i2, Material.portal, false)
        {
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            return null;
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            float f5;
            float f6;
            if (iBlockAccess1.GetTile(i2 - 1, i3, i4) != this.id && iBlockAccess1.GetTile(i2 + 1, i3, i4) != this.id)
            {
                f5 = 0.125F;
                f6 = 0.5F;
                this.SetShape(0.5F - f5, 0F, 0.5F - f6, 0.5F + f5, 1F, 0.5F + f6);
            }
            else
            {
                f5 = 0.5F;
                f6 = 0.125F;
                this.SetShape(0.5F - f5, 0F, 0.5F - f6, 0.5F + f5, 1F, 0.5F + f6);
            }
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public virtual bool TryToCreatePortal(Level world1, int i2, int i3, int i4)
        {
            byte b5 = 0;
            byte b6 = 0;
            if (world1.GetTile(i2 - 1, i3, i4) == Tile.obsidian.id || world1.GetTile(i2 + 1, i3, i4) == Tile.obsidian.id)
            {
                b5 = 1;
            }

            if (world1.GetTile(i2, i3, i4 - 1) == Tile.obsidian.id || world1.GetTile(i2, i3, i4 + 1) == Tile.obsidian.id)
            {
                b6 = 1;
            }

            if (b5 == b6)
            {
                return false;
            }
            else
            {
                if (world1.GetTile(i2 - b5, i3, i4 - b6) == 0)
                {
                    i2 -= b5;
                    i4 -= b6;
                }

                int i7;
                int i8;
                for (i7 = -1; i7 <= 2; ++i7)
                {
                    for (i8 = -1; i8 <= 3; ++i8)
                    {
                        bool z9 = i7 == -1 || i7 == 2 || i8 == -1 || i8 == 3;
                        if (i7 != -1 && i7 != 2 || i8 != -1 && i8 != 3)
                        {
                            int i10 = world1.GetTile(i2 + b5 * i7, i3 + i8, i4 + b6 * i7);
                            if (z9)
                            {
                                if (i10 != Tile.obsidian.id)
                                {
                                    return false;
                                }
                            }
                            else if (i10 != 0 && i10 != Tile.fire.id)
                            {
                                return false;
                            }
                        }
                    }
                }

                world1.editingBlocks = true;
                for (i7 = 0; i7 < 2; ++i7)
                {
                    for (i8 = 0; i8 < 3; ++i8)
                    {
                        world1.SetTile(i2 + b5 * i7, i3 + i8, i4 + b6 * i7, Tile.portal.id);
                    }
                }

                world1.editingBlocks = false;
                return true;
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            byte b6 = 0;
            byte b7 = 1;
            if (world1.GetTile(i2 - 1, i3, i4) == this.id || world1.GetTile(i2 + 1, i3, i4) == this.id)
            {
                b6 = 1;
                b7 = 0;
            }

            int i8;
            for (i8 = i3; world1.GetTile(i2, i8 - 1, i4) == this.id; --i8)
            {
            }

            if (world1.GetTile(i2, i8 - 1, i4) != Tile.obsidian.id)
            {
                world1.SetTile(i2, i3, i4, 0);
            }
            else
            {
                int i9;
                for (i9 = 1; i9 < 4 && world1.GetTile(i2, i8 + i9, i4) == this.id; ++i9)
                {
                }

                if (i9 == 3 && world1.GetTile(i2, i8 + i9, i4) == Tile.obsidian.id)
                {
                    bool z10 = world1.GetTile(i2 - 1, i3, i4) == this.id || world1.GetTile(i2 + 1, i3, i4) == this.id;
                    bool z11 = world1.GetTile(i2, i3, i4 - 1) == this.id || world1.GetTile(i2, i3, i4 + 1) == this.id;
                    if (z10 && z11)
                    {
                        world1.SetTile(i2, i3, i4, 0);
                    }
                    else if ((world1.GetTile(i2 + b6, i3, i4 + b7) != Tile.obsidian.id || world1.GetTile(i2 - b6, i3, i4 - b7) != this.id) && (world1.GetTile(i2 - b6, i3, i4 - b7) != Tile.obsidian.id || world1.GetTile(i2 + b6, i3, i4 + b7) != this.id))
                    {
                        world1.SetTile(i2, i3, i4, 0);
                    }
                }
                else
                {
                    world1.SetTile(i2, i3, i4, 0);
                }
            }
        }

        public override bool ShouldRenderFace(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            if (iBlockAccess1.GetTile(i2, i3, i4) == this.id)
            {
                return false;
            }
            else
            {
                bool z6 = iBlockAccess1.GetTile(i2 - 1, i3, i4) == this.id && iBlockAccess1.GetTile(i2 - 2, i3, i4) != this.id;
                bool z7 = iBlockAccess1.GetTile(i2 + 1, i3, i4) == this.id && iBlockAccess1.GetTile(i2 + 2, i3, i4) != this.id;
                bool z8 = iBlockAccess1.GetTile(i2, i3, i4 - 1) == this.id && iBlockAccess1.GetTile(i2, i3, i4 - 2) != this.id;
                bool z9 = iBlockAccess1.GetTile(i2, i3, i4 + 1) == this.id && iBlockAccess1.GetTile(i2, i3, i4 + 2) != this.id;
                bool z10 = z6 || z7;
                bool z11 = z8 || z9;
                return z10 && i5 == 4 ? true : (z10 && i5 == 5 ? true : (z11 && i5 == 2 ? true : z11 && i5 == 3));
            }
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        public override RenderLayer GetRenderLayer()
        {
            return RenderLayer.RENDERLAYER_ALPHATEST;
        }

        public override void OnEntityCollidedWithBlock(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            if (entity5.ridingEntity == null && entity5.riddenByEntity == null)
            {
                entity5.SetInPortal();
            }
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (random5.NextInt(100) == 0)
            {
                world1.PlaySound(i2 + 0.5, i3 + 0.5, i4 + 0.5, "portal.portal", 1F, random5.NextFloat() * 0.4F + 0.8F);
            }

            for (int i6 = 0; i6 < 4; ++i6)
            {
                double d7 = i2 + random5.NextFloat();
                double d9 = i3 + random5.NextFloat();
                double d11 = i4 + random5.NextFloat();
                double d13 = 0;
                double d15 = 0;
                double d17 = 0;
                int i19 = random5.NextInt(2) * 2 - 1;
                d13 = (random5.NextFloat() - 0.5) * 0.5;
                d15 = (random5.NextFloat() - 0.5) * 0.5;
                d17 = (random5.NextFloat() - 0.5) * 0.5;
                if (world1.GetTile(i2 - 1, i3, i4) != this.id && world1.GetTile(i2 + 1, i3, i4) != this.id)
                {
                    d7 = i2 + 0.5 + 0.25 * i19;
                    d13 = random5.NextFloat() * 2F * i19;
                }
                else
                {
                    d11 = i4 + 0.5 + 0.25 * i19;
                    d17 = random5.NextFloat() * 2F * i19;
                }

                world1.AddParticle("portal", d7, d9, d11, d13, d15, d17);
            }
        }
    }
}