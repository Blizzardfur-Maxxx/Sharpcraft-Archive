using SharpCraft.Core.NBT;

namespace SharpCraft.Core.World.GameLevel.GameSavedData
{
    public abstract class SavedData
    {
        public readonly string name;
        private bool dirty;
        public SavedData(string string1)
        {
            this.name = string1;
        }

        public abstract void Read(CompoundTag nBTTagCompound1);
        public abstract void Write(CompoundTag nBTTagCompound1);
        public virtual void MarkDirty()
        {
            this.SetDirty(true);
        }

        public virtual void SetDirty(bool z1)
        {
            this.dirty = z1;
        }

        public virtual bool IsDirty()
        {
            return this.dirty;
        }
    }
}