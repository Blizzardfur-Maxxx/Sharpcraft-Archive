using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.World.Entities.Monsters
{
    public class Zombie : Monster
    {
        public Zombie(Level world1) : base(world1)
        {
            this.texture = "/mob/zombie.png";
            this.moveSpeed = 0.5F;
            this.attackStrength = 5;
        }

        public override void OnLivingUpdate()
        {
            if (this.worldObj.IsDaytime())
            {
                float f1 = this.GetEntityBrightness(1F);
                if (f1 > 0.5F && this.worldObj.CanCockSeeTheSky(Mth.Floor(this.x), Mth.Floor(this.y), Mth.Floor(this.z)) && this.rand.NextFloat() * 30F < (f1 - 0.4F) * 2F)
                {
                    this.fire = 300;
                }
            }

            base.OnLivingUpdate();
        }

        protected override string GetLivingSound()
        {
            return "mob.zombie";
        }

        protected override string GetHurtSound()
        {
            return "mob.zombiehurt";
        }

        protected override string GetDeathSound()
        {
            return "mob.zombiedeath";
        }

        protected override int GetDropItemId()
        {
            return Item.feather.id;
        }
    }
}