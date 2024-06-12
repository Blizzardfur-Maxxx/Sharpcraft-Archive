namespace SharpCraft.Core.World.GameLevel.Chunk
{
    public class DataLayer
    {
        public readonly byte[] data;
        public DataLayer(int i1)
        {
            this.data = new byte[i1 >> 1];
        }

        public DataLayer(byte[] data)
        {
            this.data = data;
        }

        public virtual int Get(int x, int y, int z)
        {
            int i4 = x << 11 | z << 7 | y;
            int i5 = i4 >> 1;
            int i6 = i4 & 1;
            return i6 == 0 ? this.data[i5] & 15 : this.data[i5] >> 4 & 15;
        }

        public virtual void Set(int x, int y, int z, int data)
        {
            int i5 = x << 11 | z << 7 | y;
            int i6 = i5 >> 1;
            int i7 = i5 & 1;
            if (i7 == 0)
            {
                this.data[i6] = (byte)(this.data[i6] & 240 | data & 15);
            }
            else
            {
                this.data[i6] = (byte)(this.data[i6] & 15 | (data & 15) << 4);
            }
        }

        public virtual bool IsValid()
        {
            return this.data != null;
        }
    }
}