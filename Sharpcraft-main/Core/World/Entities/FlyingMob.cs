using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Entities
{
    public class FlyingMob : Mob
    {
        public FlyingMob(Level world1) : base(world1)
        {
        }

        protected override void Fall(float f1)
        {
        }

        public override void MoveEntityWithHeading(float f1, float f2)
        {
            if (this.IsInWater())
            {
                this.MoveFlying(f1, f2, 0.02F);
                this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                this.motionX *= 0.8F;
                this.motionY *= 0.8F;
                this.motionZ *= 0.8F;
            }
            else if (this.HandleLavaMovement())
            {
                this.MoveFlying(f1, f2, 0.02F);
                this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                this.motionX *= 0.5;
                this.motionY *= 0.5;
                this.motionZ *= 0.5;
            }
            else
            {
                float f3 = 0.91F;
                if (this.onGround)
                {
                    f3 = 0.54600006F;
                    int i4 = this.worldObj.GetTile(Mth.Floor(this.x), Mth.Floor(this.boundingBox.y0) - 1, Mth.Floor(this.z));
                    if (i4 > 0)
                    {
                        f3 = Tile.tiles[i4].slipperiness * 0.91F;
                    }
                }

                float f8 = 0.16277136F / (f3 * f3 * f3);
                this.MoveFlying(f1, f2, this.onGround ? 0.1F * f8 : 0.02F);
                f3 = 0.91F;
                if (this.onGround)
                {
                    f3 = 0.54600006F;
                    int i5 = this.worldObj.GetTile(Mth.Floor(this.x), Mth.Floor(this.boundingBox.y0) - 1, Mth.Floor(this.z));
                    if (i5 > 0)
                    {
                        f3 = Tile.tiles[i5].slipperiness * 0.91F;
                    }
                }

                this.MoveEntity(this.motionX, this.motionY, this.motionZ);
                this.motionX *= f3;
                this.motionY *= f3;
                this.motionZ *= f3;
            }

            this.field_Q = this.field_bd;
            double d10 = this.x - this.prevX;
            double d9 = this.z - this.prevZ;
            float f7 = Mth.Sqrt(d10 * d10 + d9 * d9) * 4F;
            if (f7 > 1F)
            {
                f7 = 1F;
            }

            this.field_bd += (f7 - this.field_bd) * 0.4F;
            this.field_ba += this.field_bd;
        }

        public override bool IsOnLadder()
        {
            return false;
        }
    }
}