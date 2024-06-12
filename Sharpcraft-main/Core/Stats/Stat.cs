using System;

namespace SharpCraft.Core.Stats
{
    public class Stat
    {
        public readonly int statId;
        public readonly string statName;
        public bool field_g;
        public string guid;
        private readonly IRequirementsStrategy istattype;
        public static IRequirementsStrategy simple = new AnonymousIRequirementsStrategy();
        private sealed class AnonymousIRequirementsStrategy : IRequirementsStrategy
        {
            public AnonymousIRequirementsStrategy()
            {

            }


            public string Apply(int i)
            {
                return i.ToString();
            }
        }

        private const string DECIMAL_FORMAT = "########0.00";
        public static IRequirementsStrategy time = new AnonymousIRequirementsStrategy1();
        private sealed class AnonymousIRequirementsStrategy1 : IRequirementsStrategy
        {
            public AnonymousIRequirementsStrategy1()
            {

            }


            public string Apply(int i)
            {
                double a = i / 20;
                double b = a / 60;
                double c = b / 60;
                double d = c / 24;
                double e = d / 365;
                return e > 0.5 ? e.ToString(DECIMAL_FORMAT) + " y" : (d > 0.5 ? d.ToString(DECIMAL_FORMAT) + " d" : (c > 0.5 ? c.ToString(DECIMAL_FORMAT) + " h" : (b > 0.5 ? b.ToString(DECIMAL_FORMAT) + " m" : a + " s")));
            }
        }

        public static IRequirementsStrategy dist = new AnonymousIRequirementsStrategy2();
        private sealed class AnonymousIRequirementsStrategy2 : IRequirementsStrategy
        {
            public AnonymousIRequirementsStrategy2()
            {

            }


            public string Apply(int i)
            {
                double a = i / 100;
                double b = a / 1000;
                return b > 0.5 ? b.ToString(DECIMAL_FORMAT) + " km" : (a > 0.5 ? a.ToString(DECIMAL_FORMAT) + " m" : i + " cm");
            }
        }

        public Stat(int id, string name, IRequirementsStrategy iStatType3)
        {
            this.field_g = false;
            this.statId = id;
            this.statName = name;
            this.istattype = iStatType3;
        }

        public Stat(int i1, string string2) : this(i1, string2, simple)
        {
        }

        public virtual Stat Func_h()
        {
            this.field_g = true;
            return this;
        }

        public virtual Stat RegisterStat()
        {
            if (StatList.statIds.ContainsKey(this.statId))
            {
                throw new Exception("Duplicate stat id: \"" + StatList.statIds[this.statId].statName + "\" and \"" + this.statName + "\" at id " + this.statId);
            }
            else
            {
                StatList.stats.Add(this);
                StatList.statIds[this.statId] = this;
                this.guid = AchievementMap.GetGuid(this.statId);
                return this;
            }
        }

        public virtual bool Fun_f()
        {
            return false;
        }

        public virtual string Func_27084_a(int i1)
        {
            return this.istattype.Apply(i1);
        }

        public override string ToString()
        {
            return this.statName;
        }
    }
}