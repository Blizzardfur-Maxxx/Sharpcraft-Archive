using SharpCraft.Core.NBT;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Core.World.Entities.Animals
{
    public class WaterCreature : Path, IAnimals
    {
        public WaterCreature(Level world1) : base(world1)
        {
        }

        public override bool CanBreatheUnderwater()
        {
            return true;
        }

        protected override void WriteEntityToNBT(CompoundTag nBTTagCompound1)
        {
            base.WriteEntityToNBT(nBTTagCompound1);
        }

        protected override void ReadEntityFromNBT(CompoundTag nBTTagCompound1)
        {
            base.ReadEntityFromNBT(nBTTagCompound1);
        }

        public override bool GetCanSpawnHere()
        {
            return this.worldObj.CheckIfAABBIsClear(this.boundingBox);
        }

        public override int GetTalkInterval()
        {
            return 120;
        }
    }
}