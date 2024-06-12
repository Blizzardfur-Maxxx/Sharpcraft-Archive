using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;

namespace SharpCraft.Core.World.Entities.Animals
{
    public abstract class Animal : Path, IAnimals
    {
        public Animal(Level world1) : base(world1)
        {
        }

        protected override float GetBlockPathWeight(int i1, int i2, int i3)
        {
            return this.worldObj.GetTile(i1, i2 - 1, i3) == Tile.grass.id ? 10F : this.worldObj.GetBrightness(i1, i2, i3) - 0.5F;
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
            int i1 = Mth.Floor(this.x);
            int i2 = Mth.Floor(this.boundingBox.y0);
            int i3 = Mth.Floor(this.z);
            return this.worldObj.GetTile(i1, i2 - 1, i3) == Tile.grass.id && this.worldObj.IsSkyLit(i1, i2, i3) > 8 && base.GetCanSpawnHere();
        }

        public override int GetTalkInterval()
        {
            return 120;
        }
    }
}