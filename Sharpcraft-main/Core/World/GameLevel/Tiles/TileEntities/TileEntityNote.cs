using SharpCraft.Core.NBT;
using SharpCraft.Core.World.GameLevel.Materials;

namespace SharpCraft.Core.World.GameLevel.Tiles.TileEntities
{
    public class TileEntityNote : TileEntity
    {
        public byte note = 0;
        public bool previousRedstoneState = false;
        public override void Save(CompoundTag nBTTagCompound1)
        {
            base.Save(nBTTagCompound1);
            nBTTagCompound1.SetByte("note", this.note);
        }

        public override void Load(CompoundTag nBTTagCompound1)
        {
            base.Load(nBTTagCompound1);
            this.note = nBTTagCompound1.GetByte("note");
            if (this.note < 0)
            {
                this.note = 0;
            }

            if (this.note > 24)
            {
                this.note = 24;
            }
        }

        public virtual void ChangePitch()
        {
            this.note = (byte)((this.note + 1) % 25);
            this.SetChanged();
        }

        public virtual void TriggerNote(Level world1, int i2, int i3, int i4)
        {
            if (world1.GetMaterial(i2, i3 + 1, i4) == Material.air)
            {
                Material material5 = world1.GetMaterial(i2, i3 - 1, i4);
                byte b6 = 0;
                if (material5 == Material.stone)
                {
                    b6 = 1;
                }

                if (material5 == Material.sand)
                {
                    b6 = 2;
                }

                if (material5 == Material.glass)
                {
                    b6 = 3;
                }

                if (material5 == Material.wood)
                {
                    b6 = 4;
                }

                world1.TileEvent(i2, i3, i4, b6, this.note);
            }
        }
    }
}