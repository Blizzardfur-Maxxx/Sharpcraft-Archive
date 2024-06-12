using System;

namespace SharpCraft.Core.World.GameLevel
{
    public class TickNextTickData : IComparable<TickNextTickData>
    {
        private static long nextTickEntryID = 0;
        public int xCoord;
        public int yCoord;
        public int zCoord;
        public int blockID;
        public long scheduledTime;
        private long tickEntryID = nextTickEntryID++;
        public TickNextTickData(int i1, int i2, int i3, int i4)
        {
            this.xCoord = i1;
            this.yCoord = i2;
            this.zCoord = i3;
            this.blockID = i4;
        }

        public override bool Equals(object object1)
        {
            if (!(object1 is TickNextTickData))
            {
                return false;
            }
            else
            {
                TickNextTickData nextTickListEntry2 = (TickNextTickData)object1;
                return this.xCoord == nextTickListEntry2.xCoord && this.yCoord == nextTickListEntry2.yCoord && this.zCoord == nextTickListEntry2.zCoord && this.blockID == nextTickListEntry2.blockID;
            }
        }

        public override int GetHashCode()
        {
            return (this.xCoord * 128 * 1024 + this.zCoord * 128 + this.yCoord) * 256 + this.blockID;
        }

        public virtual TickNextTickData SetScheduledTime(long j1)
        {
            this.scheduledTime = j1;
            return this;
        }

        public int CompareTo(TickNextTickData nextTickListEntry1)
        {
            return this.scheduledTime < nextTickListEntry1.scheduledTime ? -1 : (this.scheduledTime > nextTickListEntry1.scheduledTime ? 1 : (this.tickEntryID < nextTickListEntry1.tickEntryID ? -1 : (this.tickEntryID > nextTickListEntry1.tickEntryID ? 1 : 0)));
        }
    }
}