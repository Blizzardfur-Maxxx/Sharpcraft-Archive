using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Items;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class DispenserTile : EntityTile
    {
        private JRandom random = new JRandom();
        public DispenserTile(int i1) : base(i1, Material.stone)
        {
            this.texture = 45;
        }

        public override int GetTickDelay()
        {
            return 4;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.dispenser.id;
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            base.OnPlace(world1, i2, i3, i4);
            this.SetDispenserDefaultDirection(world1, i2, i3, i4);
        }

        private void SetDispenserDefaultDirection(Level world1, int i2, int i3, int i4)
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
                return i5 != i6 ? this.texture : this.texture + 1;
            }
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.UP ? this.texture + 17 : (faceIdx == TileFace.DOWN ? this.texture + 17 : (faceIdx == TileFace.SOUTH ? this.texture + 1 : this.texture));
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (world1.isRemote)
            {
                return true;
            }
            else
            {
                TileEntityDispenser tileEntityDispenser6 = (TileEntityDispenser)world1.GetTileEntity(i2, i3, i4);
                entityPlayer5.DisplayGUIDispenser(tileEntityDispenser6);
                return true;
            }
        }

        private void DispenseItem(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            int i6 = world1.GetData(i2, i3, i4);
            sbyte b9 = 0;
            sbyte b10 = 0;
            if (i6 == 3)
            {
                b10 = 1;
            }
            else if (i6 == 2)
            {
                b10 = -1;
            }
            else if (i6 == 5)
            {
                b9 = 1;
            }
            else
            {
                b9 = -1;
            }

            TileEntityDispenser tileEntityDispenser11 = (TileEntityDispenser)world1.GetTileEntity(i2, i3, i4);
            ItemInstance itemStack12 = tileEntityDispenser11.GetRandomStackFromInventory();
            double d13 = i2 + b9 * 0.6 + 0.5;
            double d15 = i3 + 0.5;
            double d17 = i4 + b10 * 0.6 + 0.5;
            if (itemStack12 == null)
            {
                world1.LevelEvent(LevelEventType.CLICK_1_2, i2, i3, i4, 0);
            }
            else
            {
                if (itemStack12.itemID == Item.arrow.id)
                {
                    Arrow entityArrow19 = new Arrow(world1, d13, d15, d17);
                    entityArrow19.Shoot(b9, 0.1F, b10, 1.1F, 6F);
                    entityArrow19.playerShot = true;
                    world1.AddEntity(entityArrow19);
                    world1.LevelEvent(LevelEventType.BOW, i2, i3, i4, 0);
                }
                else if (itemStack12.itemID == Item.egg.id)
                {
                    ThrownEgg entityEgg22 = new ThrownEgg(world1, d13, d15, d17);
                    entityEgg22.Shoot(b9, 0.1F, b10, 1.1F, 6F);
                    world1.AddEntity(entityEgg22);
                    world1.LevelEvent(LevelEventType.BOW, i2, i3, i4, 0);
                }
                else if (itemStack12.itemID == Item.snowball.id)
                {
                    Snowball entitySnowball23 = new Snowball(world1, d13, d15, d17);
                    entitySnowball23.Shoot(b9, 0.1F, b10, 1.1F, 6F);
                    world1.AddEntity(entitySnowball23);
                    world1.LevelEvent(LevelEventType.BOW, i2, i3, i4, 0);
                }
                else
                {
                    ItemEntity entityItem24 = new ItemEntity(world1, d13, d15 - 0.3, d17, itemStack12);
                    double d20 = random5.NextFloat() * 0.1 + 0.2;
                    entityItem24.motionX = b9 * d20;
                    entityItem24.motionY = 0.2F;
                    entityItem24.motionZ = b10 * d20;
                    entityItem24.motionX += random5.NextGaussian() * 0.0075F * 6;
                    entityItem24.motionY += random5.NextGaussian() * 0.0075F * 6;
                    entityItem24.motionZ += random5.NextGaussian() * 0.0075F * 6;
                    world1.AddEntity(entityItem24);
                    world1.LevelEvent(LevelEventType.CLICK_1_0, i2, i3, i4, 0);
                }

                world1.LevelEvent(LevelEventType.SMOKE, i2, i3, i4, b9 + 1 + (b10 + 1) * 3);
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (i5 > 0 && Tile.tiles[i5].IsSignalSource())
            {
                bool z6 = world1.IsBlockIndirectlyGettingPowered(i2, i3, i4) || world1.IsBlockIndirectlyGettingPowered(i2, i3 + 1, i4);
                if (z6)
                {
                    world1.ScheduleBlockUpdate(i2, i3, i4, this.id, this.GetTickDelay());
                }
            }
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (world1.IsBlockIndirectlyGettingPowered(i2, i3, i4) || world1.IsBlockIndirectlyGettingPowered(i2, i3 + 1, i4))
            {
                this.DispenseItem(world1, i2, i3, i4, (JRandom)random5);
            }
        }

        protected override TileEntity NewTileEntity()
        {
            return new TileEntityDispenser();
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
            TileEntityDispenser tileEntityDispenser5 = (TileEntityDispenser)world1.GetTileEntity(i2, i3, i4);
            for (int i6 = 0; i6 < tileEntityDispenser5.GetContainerSize(); ++i6)
            {
                ItemInstance itemStack7 = tileEntityDispenser5.GetItem(i6);
                if (itemStack7 != null)
                {
                    float f8 = (float)this.random.NextFloat() * 0.8F + 0.1F;
                    float f9 = (float)this.random.NextFloat() * 0.8F + 0.1F;
                    float f10 = (float)this.random.NextFloat() * 0.8F + 0.1F;
                    while (itemStack7.stackSize > 0)
                    {
                        int i11 = this.random.NextInt(21) + 10;
                        if (i11 > itemStack7.stackSize)
                        {
                            i11 = itemStack7.stackSize;
                        }

                        itemStack7.stackSize -= i11;
                        ItemEntity entityItem12 = new ItemEntity(world1, i2 + f8, i3 + f9, i4 + f10, new ItemInstance(itemStack7.itemID, i11, itemStack7.GetItemDamage()));
                        float f13 = 0.05F;
                        entityItem12.motionX = (float)this.random.NextGaussian() * f13;
                        entityItem12.motionY = (float)this.random.NextGaussian() * f13 + 0.2F;
                        entityItem12.motionZ = (float)this.random.NextGaussian() * f13;
                        world1.AddEntity(entityItem12);
                    }
                }
            }

            base.OnBlockRemoval(world1, i2, i3, i4);
        }
    }
}