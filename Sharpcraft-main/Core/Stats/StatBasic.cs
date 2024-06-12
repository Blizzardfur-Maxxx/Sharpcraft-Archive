namespace SharpCraft.Core.Stats
{
    public class StatBasic : Stat
    {
        public StatBasic(int i1, string string2, IRequirementsStrategy iStatType3) : base(i1, string2, iStatType3)
        {
        }

        public StatBasic(int i1, string string2) : base(i1, string2)
        {
        }

        public override Stat RegisterStat()
        {
            base.RegisterStat();
            StatList.field_b.Add(this);
            return this;
        }
    }
}