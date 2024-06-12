namespace SharpCraft.Core.Stats
{
    public class CraftingStat : Stat
    {
        private readonly int field_a;
        public CraftingStat(int i1, string string2, int i3) : base(i1, string2)
        {
            this.field_a = i3;
        }

        public virtual int Fun_b()
        {
            return this.field_a;
        }
    }
}