using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Entities.Monsters
{
    public class Giant : Monster
    {
        public Giant(Level world1) : base(world1)
        {
            this.texture = "/mob/zombie.png";
            this.moveSpeed = 0.5F;
            this.attackStrength = 50;
            this.health *= 10;
            this.yOffset *= 6F;
            this.SetSize(this.width * 6F, this.height * 6F);
        }

        protected override float GetBlockPathWeight(int i1, int i2, int i3)
        {
            return this.worldObj.GetBrightness(i1, i2, i3) - 0.5F;
        }
    }
}