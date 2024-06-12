using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.Items
{
    public class ItemBucket : Item
    {
        private int isFull;
        public ItemBucket(int i1, int i2) : base(i1)
        {
            this.maxStackSize = 1;
            this.isFull = i2;
        }

        public override ItemInstance OnItemRightClick(ItemInstance itemStack1, Level world2, Player entityPlayer3)
        {
            float f4 = 1F;
            float f5 = entityPlayer3.prevPitch + (entityPlayer3.pitch - entityPlayer3.prevPitch) * f4;
            float f6 = entityPlayer3.prevYaw + (entityPlayer3.yaw - entityPlayer3.prevYaw) * f4;
            double d7 = entityPlayer3.prevX + (entityPlayer3.x - entityPlayer3.prevX) * f4;
            double d9 = entityPlayer3.prevY + (entityPlayer3.y - entityPlayer3.prevY) * f4 + 1.62 - entityPlayer3.yOffset;
            double d11 = entityPlayer3.prevZ + (entityPlayer3.z - entityPlayer3.prevZ) * f4;
            Vec3 vec3D13 = Vec3.Of(d7, d9, d11);
            float f14 = Mth.Cos(-f6 * 0.017453292F - Mth.PI);
            float f15 = Mth.Sin(-f6 * 0.017453292F - Mth.PI);
            float f16 = -Mth.Cos(-f5 * 0.017453292F);
            float f17 = Mth.Sin(-f5 * 0.017453292F);
            float f18 = f15 * f16;
            float f20 = f14 * f16;
            double d21 = 5;
            Vec3 vec3D23 = vec3D13.AddVector(f18 * d21, f17 * d21, f20 * d21);
            HitResult movingObjectPosition24 = world2.Clip(vec3D13, vec3D23, this.isFull == 0);
            if (movingObjectPosition24 == null)
            {
                return itemStack1;
            }
            else
            {
                if (movingObjectPosition24.TypeOfHit == HitResult.Type.TILE)
                {
                    int i25 = movingObjectPosition24.BlockX;
                    int i26 = movingObjectPosition24.BlockY;
                    int i27 = movingObjectPosition24.BlockZ;
                    if (!world2.CanMineBlock(entityPlayer3, i25, i26, i27))
                    {
                        return itemStack1;
                    }

                    if (this.isFull == 0)
                    {
                        if (world2.GetMaterial(i25, i26, i27) == Material.water && world2.GetData(i25, i26, i27) == 0)
                        {
                            world2.SetTile(i25, i26, i27, 0);
                            return new ItemInstance(Item.bucketWater);
                        }

                        if (world2.GetMaterial(i25, i26, i27) == Material.lava && world2.GetData(i25, i26, i27) == 0)
                        {
                            world2.SetTile(i25, i26, i27, 0);
                            return new ItemInstance(Item.bucketLava);
                        }
                    }
                    else
                    {
                        if (this.isFull < 0)
                        {
                            return new ItemInstance(Item.bucketEmpty);
                        }

                        if (movingObjectPosition24.SideHit == Facing.TileFace.DOWN)
                        {
                            --i26;
                        }

                        if (movingObjectPosition24.SideHit == Facing.TileFace.UP)
                        {
                            ++i26;
                        }

                        if (movingObjectPosition24.SideHit == Facing.TileFace.NORTH)
                        {
                            --i27;
                        }

                        if (movingObjectPosition24.SideHit == Facing.TileFace.SOUTH)
                        {
                            ++i27;
                        }

                        if (movingObjectPosition24.SideHit == Facing.TileFace.WEST)
                        {
                            --i25;
                        }

                        if (movingObjectPosition24.SideHit == Facing.TileFace.EAST)
                        {
                            ++i25;
                        }

                        if (world2.IsAirBlock(i25, i26, i27) || !world2.GetMaterial(i25, i26, i27).IsSolid())
                        {
                            if (world2.dimension.isHellWorld && this.isFull == Tile.water.id)
                            {
                                world2.PlaySound(d7 + 0.5, d9 + 0.5, d11 + 0.5, "random.fizz", 0.5F, 2.6F + (world2.rand.NextFloat() - world2.rand.NextFloat()) * 0.8F);
                                for (int i28 = 0; i28 < 8; ++i28)
                                {
                                    world2.AddParticle("largesmoke", i25 + Mth.Random(), i26 + Mth.Random(), i27 + Mth.Random(), 0, 0, 0);
                                }
                            }
                            else
                            {
                                world2.SetTileAndData(i25, i26, i27, this.isFull, 0);
                            }

                            return new ItemInstance(Item.bucketEmpty);
                        }
                    }
                }
                else if (this.isFull == 0 && movingObjectPosition24.EntityHit is Cow)
                {
                    return new ItemInstance(Item.bucketMilk);
                }

                return itemStack1;
            }
        }
    }
}