using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class FireTile : Tile
    {
        private int[] chanceToEncourageFire = new int[256];
        private int[] abilityToCatchFire = new int[256];
        public FireTile(int i1, int i2) : base(i1, i2, Material.fire)
        {
            this.SetTicking(true);
        }

        protected override void Init()
        {
            this.SetBurnRate(Tile.wood.id, 5, 20);
            this.SetBurnRate(Tile.fence.id, 5, 20);
            this.SetBurnRate(Tile.stairs_wood.id, 5, 20);
            this.SetBurnRate(Tile.treeTrunk.id, 5, 5);
            this.SetBurnRate(Tile.leaves.id, 30, 60);
            this.SetBurnRate(Tile.bookshelf.id, 30, 20);
            this.SetBurnRate(Tile.tnt.id, 15, 100);
            this.SetBurnRate(Tile.tallGrass.id, 60, 100);
            this.SetBurnRate(Tile.cloth.id, 30, 60);
        }

        private void SetBurnRate(int i1, int i2, int i3)
        {
            this.chanceToEncourageFire[i1] = i2;
            this.abilityToCatchFire[i1] = i3;
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
            return RenderShape.FIRE;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        public override int GetTickDelay()
        {
            return 40;
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            bool z6 = world1.GetTile(i2, i3 - 1, i4) == Tile.netherrack.id;
            if (!this.CanPlaceBlockAt(world1, i2, i3, i4))
            {
                world1.SetTile(i2, i3, i4, 0);
            }

            if (!z6 && world1.Func_C() && (world1.CanLightningStrikeAt(i2, i3, i4) || world1.CanLightningStrikeAt(i2 - 1, i3, i4) || world1.CanLightningStrikeAt(i2 + 1, i3, i4) || world1.CanLightningStrikeAt(i2, i3, i4 - 1) || world1.CanLightningStrikeAt(i2, i3, i4 + 1)))
            {
                world1.SetTile(i2, i3, i4, 0);
            }
            else
            {
                int i7 = world1.GetData(i2, i3, i4);
                if (i7 < 15)
                {
                    world1.SetDataNoUpdate(i2, i3, i4, i7 + random5.NextInt(3) / 2);
                }

                world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
                if (!z6 && !this.Fun_263(world1, i2, i3, i4))
                {
                    if (!world1.IsSolidBlockingTile(i2, i3 - 1, i4) || i7 > 3)
                    {
                        world1.SetTile(i2, i3, i4, 0);
                    }
                }
                else if (!z6 && !this.CanBlockCatchFire(world1, i2, i3 - 1, i4) && i7 == 15 && random5.NextInt(4) == 0)
                {
                    world1.SetTile(i2, i3, i4, 0);
                }
                else
                {
                    this.TryToCatchBlockOnFire(world1, i2 + 1, i3, i4, 300, random5, i7);
                    this.TryToCatchBlockOnFire(world1, i2 - 1, i3, i4, 300, random5, i7);
                    this.TryToCatchBlockOnFire(world1, i2, i3 - 1, i4, 250, random5, i7);
                    this.TryToCatchBlockOnFire(world1, i2, i3 + 1, i4, 250, random5, i7);
                    this.TryToCatchBlockOnFire(world1, i2, i3, i4 - 1, 300, random5, i7);
                    this.TryToCatchBlockOnFire(world1, i2, i3, i4 + 1, 300, random5, i7);
                    for (int i8 = i2 - 1; i8 <= i2 + 1; ++i8)
                    {
                        for (int i9 = i4 - 1; i9 <= i4 + 1; ++i9)
                        {
                            for (int i10 = i3 - 1; i10 <= i3 + 4; ++i10)
                            {
                                if (i8 != i2 || i10 != i3 || i9 != i4)
                                {
                                    int i11 = 100;
                                    if (i10 > i3 + 1)
                                    {
                                        i11 += (i10 - (i3 + 1)) * 100;
                                    }

                                    int i12 = this.GetChanceOfNeighborsEncouragingFire(world1, i8, i10, i9);
                                    if (i12 > 0)
                                    {
                                        int i13 = (i12 + 40) / (i7 + 30);
                                        if (i13 > 0 && random5.NextInt(i11) <= i13 && (!world1.Func_C() || !world1.CanLightningStrikeAt(i8, i10, i9)) && !world1.CanLightningStrikeAt(i8 - 1, i10, i4) && !world1.CanLightningStrikeAt(i8 + 1, i10, i9) && !world1.CanLightningStrikeAt(i8, i10, i9 - 1) && !world1.CanLightningStrikeAt(i8, i10, i9 + 1))
                                        {
                                            int i14 = i7 + random5.NextInt(5) / 4;
                                            if (i14 > 15)
                                            {
                                                i14 = 15;
                                            }

                                            world1.SetTileAndData(i8, i10, i9, this.id, i14);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void TryToCatchBlockOnFire(Level world1, int i2, int i3, int i4, int i5, JRandom random6, int i7)
        {
            int i8 = this.abilityToCatchFire[world1.GetTile(i2, i3, i4)];
            if (random6.NextInt(i5) < i8)
            {
                bool z9 = world1.GetTile(i2, i3, i4) == Tile.tnt.id;
                if (random6.NextInt(i7 + 10) < 5 && !world1.CanLightningStrikeAt(i2, i3, i4))
                {
                    int i10 = i7 + random6.NextInt(5) / 4;
                    if (i10 > 15)
                    {
                        i10 = 15;
                    }

                    world1.SetTileAndData(i2, i3, i4, this.id, i10);
                }
                else
                {
                    world1.SetTile(i2, i3, i4, 0);
                }

                if (z9)
                {
                    Tile.tnt.OnBlockDestroyedByPlayer(world1, i2, i3, i4, 1);
                }
            }
        }

        private bool Fun_263(Level world1, int i2, int i3, int i4)
        {
            return this.CanBlockCatchFire(world1, i2 + 1, i3, i4) ? true : (this.CanBlockCatchFire(world1, i2 - 1, i3, i4) ? true : (this.CanBlockCatchFire(world1, i2, i3 - 1, i4) ? true : (this.CanBlockCatchFire(world1, i2, i3 + 1, i4) ? true : (this.CanBlockCatchFire(world1, i2, i3, i4 - 1) ? true : this.CanBlockCatchFire(world1, i2, i3, i4 + 1)))));
        }

        private int GetChanceOfNeighborsEncouragingFire(Level world1, int i2, int i3, int i4)
        {
            byte b5 = 0;
            if (!world1.IsAirBlock(i2, i3, i4))
            {
                return 0;
            }
            else
            {
                int i6 = this.GetChanceToEncourageFire(world1, i2 + 1, i3, i4, b5);
                i6 = this.GetChanceToEncourageFire(world1, i2 - 1, i3, i4, i6);
                i6 = this.GetChanceToEncourageFire(world1, i2, i3 - 1, i4, i6);
                i6 = this.GetChanceToEncourageFire(world1, i2, i3 + 1, i4, i6);
                i6 = this.GetChanceToEncourageFire(world1, i2, i3, i4 - 1, i6);
                i6 = this.GetChanceToEncourageFire(world1, i2, i3, i4 + 1, i6);
                return i6;
            }
        }

        public override bool IsCollidable()
        {
            return false;
        }

        public virtual bool CanBlockCatchFire(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            return this.chanceToEncourageFire[iBlockAccess1.GetTile(i2, i3, i4)] > 0;
        }

        public virtual int GetChanceToEncourageFire(Level world1, int i2, int i3, int i4, int i5)
        {
            int i6 = this.chanceToEncourageFire[world1.GetTile(i2, i3, i4)];
            return i6 > i5 ? i6 : i5;
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            return world1.IsSolidBlockingTile(i2, i3 - 1, i4) || this.Fun_263(world1, i2, i3, i4);
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!world1.IsSolidBlockingTile(i2, i3 - 1, i4) && !this.Fun_263(world1, i2, i3, i4))
            {
                world1.SetTile(i2, i3, i4, 0);
            }
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            if (world1.GetTile(i2, i3 - 1, i4) != Tile.obsidian.id || !Tile.portal.TryToCreatePortal(world1, i2, i3, i4))
            {
                if (!world1.IsSolidBlockingTile(i2, i3 - 1, i4) && !this.Fun_263(world1, i2, i3, i4))
                {
                    world1.SetTile(i2, i3, i4, 0);
                }
                else
                {
                    world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
                }
            }
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (random5.NextInt(24) == 0)
            {
                world1.PlaySound(i2 + 0.5F, i3 + 0.5F, i4 + 0.5F, "fire.fire", 1F + (float)random5.NextFloat(), (float)random5.NextFloat() * 0.7F + 0.3F);
            }

            int i6;
            float f7;
            float f8;
            float f9;
            if (!world1.IsSolidBlockingTile(i2, i3 - 1, i4) && !Tile.fire.CanBlockCatchFire(world1, i2, i3 - 1, i4))
            {
                if (Tile.fire.CanBlockCatchFire(world1, i2 - 1, i3, i4))
                {
                    for (i6 = 0; i6 < 2; ++i6)
                    {
                        f7 = i2 + (float)random5.NextFloat() * 0.1F;
                        f8 = i3 + (float)random5.NextFloat();
                        f9 = i4 + (float)random5.NextFloat();
                        world1.AddParticle("largesmoke", f7, f8, f9, 0, 0, 0);
                    }
                }

                if (Tile.fire.CanBlockCatchFire(world1, i2 + 1, i3, i4))
                {
                    for (i6 = 0; i6 < 2; ++i6)
                    {
                        f7 = i2 + 1 - (float)random5.NextFloat() * 0.1F;
                        f8 = i3 + (float)random5.NextFloat();
                        f9 = i4 + (float)random5.NextFloat();
                        world1.AddParticle("largesmoke", f7, f8, f9, 0, 0, 0);
                    }
                }

                if (Tile.fire.CanBlockCatchFire(world1, i2, i3, i4 - 1))
                {
                    for (i6 = 0; i6 < 2; ++i6)
                    {
                        f7 = i2 + (float)random5.NextFloat();
                        f8 = i3 + (float)random5.NextFloat();
                        f9 = i4 + (float)random5.NextFloat() * 0.1F;
                        world1.AddParticle("largesmoke", f7, f8, f9, 0, 0, 0);
                    }
                }

                if (Tile.fire.CanBlockCatchFire(world1, i2, i3, i4 + 1))
                {
                    for (i6 = 0; i6 < 2; ++i6)
                    {
                        f7 = i2 + (float)random5.NextFloat();
                        f8 = i3 + (float)random5.NextFloat();
                        f9 = i4 + 1 - (float)random5.NextFloat() * 0.1F;
                        world1.AddParticle("largesmoke", f7, f8, f9, 0, 0, 0);
                    }
                }

                if (Tile.fire.CanBlockCatchFire(world1, i2, i3 + 1, i4))
                {
                    for (i6 = 0; i6 < 2; ++i6)
                    {
                        f7 = i2 + (float)random5.NextFloat();
                        f8 = i3 + 1 - (float)random5.NextFloat() * 0.1F;
                        f9 = i4 + (float)random5.NextFloat();
                        world1.AddParticle("largesmoke", f7, f8, f9, 0, 0, 0);
                    }
                }
            }
            else
            {
                for (i6 = 0; i6 < 3; ++i6)
                {
                    f7 = i2 + (float)random5.NextFloat();
                    f8 = i3 + (float)random5.NextFloat() * 0.5F + 0.5F;
                    f9 = i4 + (float)random5.NextFloat();
                    world1.AddParticle("largesmoke", f7, f8, f9, 0, 0, 0);
                }
            }
        }
    }
}