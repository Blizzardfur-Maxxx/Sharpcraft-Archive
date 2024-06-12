using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Items;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class FurnaceTile : EntityTile
    {
        private JRandom random = new JRandom();
        private readonly bool isActive;
        private static bool keepFurnaceInventory = false;
        public FurnaceTile(int i1, bool z2) : base(i1, Material.stone)
        {
            this.isActive = z2;
            this.texture = 45;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.furnace.id;
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            base.OnPlace(world1, i2, i3, i4);
            this.SetDefaultDirection(world1, i2, i3, i4);
        }

        private void SetDefaultDirection(Level world1, int i2, int i3, int i4)
        {
            if (!world1.isRemote)
            {
                int i5 = world1.GetTile(i2, i3, i4 - 1);
                int i6 = world1.GetTile(i2, i3, i4 + 1);
                int i7 = world1.GetTile(i2 - 1, i3, i4);
                int i8 = world1.GetTile(i2 + 1, i3, i4);
                byte b9 = 3;
                if (Tile.solid[i5] && !Tile.solid[i6])
                {
                    b9 = 3;
                }

                if (Tile.solid[i6] && !Tile.solid[i5])
                {
                    b9 = 2;
                }

                if (Tile.solid[i7] && !Tile.solid[i8])
                {
                    b9 = 5;
                }

                if (Tile.solid[i8] && !Tile.solid[i7])
                {
                    b9 = 4;
                }

                world1.SetData(i2, i3, i4, b9);
            }
        }

        public override int GetBlockTexture(ILevelSource iBlockAccess1, int i2, int i3, int i4, TileFace i5)
        {
            if (i5 == TileFace.UP)
            {
                return this.texture + 17;
            }
            else if (i5 == TileFace.DOWN)
            {
                return this.texture + 17;
            }
            else
            {
                TileFace i6 = (TileFace)iBlockAccess1.GetData(i2, i3, i4);
                return i5 != i6 ? this.texture : (this.isActive ? this.texture + 16 : this.texture - 1);
            }
        }

        public override void AnimateTick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (this.isActive)
            {
                int i6 = world1.GetData(i2, i3, i4);
                float f7 = i2 + 0.5F;
                float f8 = i3 + 0F + (float)random5.NextFloat() * 6F / 16F;
                float f9 = i4 + 0.5F;
                float f10 = 0.52F;
                float f11 = (float)random5.NextFloat() * 0.6F - 0.3F;
                if (i6 == 4)
                {
                    world1.AddParticle("smoke", f7 - f10, f8, f9 + f11, 0, 0, 0);
                    world1.AddParticle("flame", f7 - f10, f8, f9 + f11, 0, 0, 0);
                }
                else if (i6 == 5)
                {
                    world1.AddParticle("smoke", f7 + f10, f8, f9 + f11, 0, 0, 0);
                    world1.AddParticle("flame", f7 + f10, f8, f9 + f11, 0, 0, 0);
                }
                else if (i6 == 2)
                {
                    world1.AddParticle("smoke", f7 + f11, f8, f9 - f10, 0, 0, 0);
                    world1.AddParticle("flame", f7 + f11, f8, f9 - f10, 0, 0, 0);
                }
                else if (i6 == 3)
                {
                    world1.AddParticle("smoke", f7 + f11, f8, f9 + f10, 0, 0, 0);
                    world1.AddParticle("flame", f7 + f11, f8, f9 + f10, 0, 0, 0);
                }
            }
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.UP ? this.texture + 17 : (faceIdx == 0 ? this.texture + 17 : (faceIdx == TileFace.SOUTH ? this.texture - 1 : this.texture));
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (world1.isRemote)
            {
                return true;
            }
            else
            {
                TileEntityFurnace tileEntityFurnace6 = (TileEntityFurnace)world1.GetTileEntity(i2, i3, i4);
                entityPlayer5.DisplayGUIFurnace(tileEntityFurnace6);
                return true;
            }
        }

        public static void UpdateFurnaceBlockState(bool z0, Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4);
            TileEntity tileEntity6 = world1.GetTileEntity(i2, i3, i4);
            keepFurnaceInventory = true;
            if (z0)
            {
                world1.SetTile(i2, i3, i4, Tile.stoneOvenActive.id);
            }
            else
            {
                world1.SetTile(i2, i3, i4, Tile.furnace.id);
            }

            keepFurnaceInventory = false;
            world1.SetData(i2, i3, i4, i5);
            tileEntity6.Validate();
            world1.SetTileEntity(i2, i3, i4, tileEntity6);
        }

        protected override TileEntity NewTileEntity()
        {
            return new TileEntityFurnace();
        }

        public override void OnBlockPlacedBy(Level world1, int i2, int i3, int i4, Mob entityLiving5)
        {
            int i6 = Mth.Floor(entityLiving5.yaw * 4F / 360F + 0.5) & 3;
            if (i6 == 0)
            {
                world1.SetData(i2, i3, i4, 2);
            }

            if (i6 == 1)
            {
                world1.SetData(i2, i3, i4, 5);
            }

            if (i6 == 2)
            {
                world1.SetData(i2, i3, i4, 3);
            }

            if (i6 == 3)
            {
                world1.SetData(i2, i3, i4, 4);
            }
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            if (!keepFurnaceInventory)
            {
                TileEntityFurnace tileEntityFurnace5 = (TileEntityFurnace)world1.GetTileEntity(i2, i3, i4);
                for (int i6 = 0; i6 < tileEntityFurnace5.GetContainerSize(); ++i6)
                {
                    ItemInstance itemStack7 = tileEntityFurnace5.GetItem(i6);
                    if (itemStack7 != null)
                    {
                        float f8 = (float)random.NextFloat() * 0.8F + 0.1F;
                        float f9 = (float)random.NextFloat() * 0.8F + 0.1F;
                        float f10 = (float)random.NextFloat() * 0.8F + 0.1F;
                        while (itemStack7.stackSize > 0)
                        {
                            int i11 = random.NextInt(21) + 10;
                            if (i11 > itemStack7.stackSize)
                            {
                                i11 = itemStack7.stackSize;
                            }

                            itemStack7.stackSize -= i11;
                            ItemEntity entityItem12 = new ItemEntity(world1, i2 + f8, i3 + f9, i4 + f10, new ItemInstance(itemStack7.itemID, i11, itemStack7.GetItemDamage()));
                            float f13 = 0.05F;
                            entityItem12.motionX = (float)random.NextGaussian() * f13;
                            entityItem12.motionY = (float)random.NextGaussian() * f13 + 0.2F;
                            entityItem12.motionZ = (float)random.NextGaussian() * f13;
                            world1.AddEntity(entityItem12);
                        }
                    }
                }
            }

            base.OnBlockRemoval(world1, i2, i3, i4);
        }
    }
}