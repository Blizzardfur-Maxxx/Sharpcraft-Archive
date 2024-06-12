using SharpCraft.Core.NBT;

namespace SharpCraft.Core.World.GameLevel.Tiles.TileEntities
{
    public class TileEntityRecordPlayer : TileEntity
    {
        public int record;
        public override void Load(CompoundTag nBTTagCompound1)
        {
            base.Load(nBTTagCompound1);
            this.record = nBTTagCompound1.GetInteger("Record");
        }

        public override void Save(CompoundTag nBTTagCompound1)
        {
            base.Save(nBTTagCompound1);
            if (this.record > 0)
            {
                nBTTagCompound1.SetInteger("Record", this.record);
            }
        }
    }
}