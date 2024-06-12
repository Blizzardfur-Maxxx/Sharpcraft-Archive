using SharpCraft.Core.i18n;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Core.Stats
{
    public class Achievement : Stat
    {
        public readonly int displayColumn;
        public readonly int displayRow;
        public readonly Achievement parentAchievement;
        private readonly string achievementDescription;
        private IStatFormatter statStringFormatter;
        public readonly ItemInstance item;
        private bool isSpecial;

        public Achievement(int i1, string string2, int i3, int i4, Item item5, Achievement achievement6) : this(i1, string2, i3, i4, new ItemInstance(item5), achievement6)
        {
        }

        public Achievement(int i1, string string2, int i3, int i4, Tile block5, Achievement achievement6) : this(i1, string2, i3, i4, new ItemInstance(block5), achievement6)
        {
        }

        public Achievement(int i1, string string2, int i3, int i4, ItemInstance itm, Achievement ac) : base(5242880 + i1, Locale.TranslateKey("achievement." + string2))
        {
            this.item = itm;
            this.achievementDescription = Locale.TranslateKey("achievement." + string2 + ".desc");
            this.displayColumn = i3;
            this.displayRow = i4;
            if (i3 < AchievementList.minDisplayColumn)
            {
                AchievementList.minDisplayColumn = i3;
            }

            if (i4 < AchievementList.minDisplayRow)
            {
                AchievementList.minDisplayRow = i4;
            }

            if (i3 > AchievementList.maxDisplayColumn)
            {
                AchievementList.maxDisplayColumn = i3;
            }

            if (i4 > AchievementList.maxDisplayRow)
            {
                AchievementList.maxDisplayRow = i4;
            }

            this.parentAchievement = ac;
        }

        public virtual Achievement SetField_g()
        {
            this.field_g = true;
            return this;
        }

        public virtual Achievement SetSpecial()
        {
            this.isSpecial = true;
            return this;
        }

        public virtual Achievement RegisterAchievement()
        {
            base.RegisterStat();
            AchievementList.achievementList.Add(this);
            return this;
        }

        public override bool Fun_f()
        {
            return true;
        }

        public virtual string GetDescription()
        {
            return this.statStringFormatter != null ? this.statStringFormatter.Format(this.achievementDescription) : this.achievementDescription;
        }

        public virtual Achievement SetFormatter(IStatFormatter iStatStringFormat1)
        {
            this.statStringFormatter = iStatStringFormat1;
            return this;
        }

        public virtual bool GetSpecial()
        {
            return this.isSpecial;
        }

        public override Stat RegisterStat()
        {
            return this.RegisterAchievement();
        }

        public override Stat Func_h()
        {
            return this.SetField_g();
        }
    }
}