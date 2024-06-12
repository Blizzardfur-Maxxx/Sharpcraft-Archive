using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.Items
{
    public class ItemBoat : Item
    {
        public ItemBoat(int i1) : base(i1)
        {
            this.maxStackSize = 1;
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
            HitResult movingObjectPosition24 = world2.Clip(vec3D13, vec3D23, true);
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
                    if (!world2.isRemote)
                    {
                        if (world2.GetTile(i25, i26, i27) == Tile.topSnow.id)
                        {
                            --i26;
                        }

                        world2.AddEntity(new Boat(world2, i25 + 0.5F, i26 + 1F, i27 + 0.5F));
                    }

                    --itemStack1.stackSize;
                }

                return itemStack1;
            }
        }
    }
}