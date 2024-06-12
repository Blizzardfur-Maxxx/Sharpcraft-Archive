using System;

namespace SharpCraft.Core.World.GameLevel.Storage
{
    public class LevelSummary : IComparable<LevelSummary>
    {
        private readonly string fileName;
        private readonly string displayName;
        private readonly long lastplayed;
        private readonly long sizeondisk;
        private readonly bool field_22167_e;

        public LevelSummary(string string1, string string2, long j3, long j5, bool z7)
        {
            this.fileName = string1;
            this.displayName = string2;
            this.lastplayed = j3;
            this.sizeondisk = j5;
            this.field_22167_e = z7;
        }

        public virtual string GetFileName()
        {
            return this.fileName;
        }

        public virtual string GetDisplayName()
        {
            return this.displayName;
        }

        public virtual long GetSizeOnDisk()
        {
            return this.sizeondisk;
        }

        public virtual bool Func_22161_d()
        {
            return this.field_22167_e;
        }

        public virtual long GetLastPlayed()
        {
            return this.lastplayed;
        }

        public int CompareTo(LevelSummary saveFormatComparator1)
        {
            return this.lastplayed < saveFormatComparator1.lastplayed ? 1 : (this.lastplayed > saveFormatComparator1.lastplayed ? -1 : this.fileName.CompareTo(saveFormatComparator1.fileName));
        }
    }
}