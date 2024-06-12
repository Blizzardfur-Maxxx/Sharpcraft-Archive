using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Client.Renderer;
using SharpCraft.Client.Stats;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Core.World.Items;
using LWCSGL.Input;

namespace SharpCraft.Client.GUI.Screens.Achievements
{
    public class StatsScreen : Screen
    {
        private abstract class AbstractStatisticsList : AbstractSelectionList
        {
            protected int field_27268_b;
            protected List<CraftingStat> field_27273_c;
            protected IComparer<CraftingStat> snorter;
            protected int field_27271_e;
            protected int field_27270_f;
            protected readonly StatsScreen SELF;
            protected AbstractStatisticsList(StatsScreen SELF) : base(SELF.mc, SELF.width, SELF.height, 32, SELF.height - 64, 20)
            {
                this.SELF = SELF;
                this.field_27268_b = -1;
                this.field_27271_e = -1;
                this.field_27270_f = 0;
                this.Func_27258_a(false);
                this.Func_27259_a(true, 20);
            }

            protected override void ElementClicked(int i1, bool z2)
            {
            }

            protected override bool IsSelected(int i1)
            {
                return false;
            }

            protected override void DrawBackground()
            {
                SELF.DrawDefaultBackground();
            }

            protected override void Func_27260_a(int i1, int i2, Tessellator tessellator3)
            {
                if (!Mouse.IsButtonDown(0))
                {
                    this.field_27268_b = -1;
                }

                if (this.field_27268_b == 0)
                {
                    SELF.Func_27136_c(i1 + 115 - 18, i2 + 1, 0, 0);
                }
                else
                {
                    SELF.Func_27136_c(i1 + 115 - 18, i2 + 1, 0, 18);
                }

                if (this.field_27268_b == 1)
                {
                    SELF.Func_27136_c(i1 + 165 - 18, i2 + 1, 0, 0);
                }
                else
                {
                    SELF.Func_27136_c(i1 + 165 - 18, i2 + 1, 0, 18);
                }

                if (this.field_27268_b == 2)
                {
                    SELF.Func_27136_c(i1 + 215 - 18, i2 + 1, 0, 0);
                }
                else
                {
                    SELF.Func_27136_c(i1 + 215 - 18, i2 + 1, 0, 18);
                }

                if (this.field_27271_e != -1)
                {
                    short s4 = 79;
                    byte b5 = 18;
                    if (this.field_27271_e == 1)
                    {
                        s4 = 129;
                    }
                    else if (this.field_27271_e == 2)
                    {
                        s4 = 179;
                    }

                    if (this.field_27270_f == 1)
                    {
                        b5 = 36;
                    }

                    SELF.Func_27136_c(i1 + s4, i2 + 1, b5, 0);
                }
            }

            protected override void Func_27255_a(int i1, int i2)
            {
                this.field_27268_b = -1;
                if (i1 >= 79 && i1 < 115)
                {
                    this.field_27268_b = 0;
                }
                else if (i1 >= 129 && i1 < 165)
                {
                    this.field_27268_b = 1;
                }
                else if (i1 >= 179 && i1 < 215)
                {
                    this.field_27268_b = 2;
                }

                if (this.field_27268_b >= 0)
                {
                    this.Func_27266_c(this.field_27268_b);
                    SELF.mc.soundEngine.PlaySoundFX("random.click", 1F, 1F);
                }
            }

            public int PubGetSize() { return GetSize(); }

            protected override int GetSize()
            {
                return this.field_27273_c.Count;
            }

            protected CraftingStat Func_27264_b(int i1)
            {
                return this.field_27273_c[i1];
            }

            protected abstract string Func_27263_a(int i1);
            protected virtual void Func_27265_a(CraftingStat statCrafting1, int i2, int i3, bool z4)
            {
                string string5;
                if (statCrafting1 != null)
                {
                    string5 = statCrafting1.Func_27084_a(SELF.field_27156_o.WriteStat(statCrafting1));
                    SELF.DrawString(SELF.font, string5, i2 - SELF.font.GetStringWidth(string5), i3 + 5, z4 ? 0xFFFFFF : 9474192);
                }
                else
                {
                    string5 = "-";
                    SELF.DrawString(SELF.font, string5, i2 - SELF.font.GetStringWidth(string5), i3 + 5, z4 ? 0xFFFFFF : 9474192);
                }
            }

            protected override void Func_27257_b(int i1, int i2)
            {
                if (i2 >= this.top && i2 <= this.bottom)
                {
                    int i3 = this.Func_27256_c(i1, i2);
                    int i4 = SELF.width / 2 - 92 - 16;
                    if (i3 >= 0)
                    {
                        if (i1 < i4 + 40 || i1 > i4 + 40 + 20)
                        {
                            return;
                        }

                        CraftingStat statCrafting9 = this.Func_27264_b(i3);
                        this.Func_27267_a(statCrafting9, i1, i2);
                    }
                    else
                    {
                        string string5 = "";
                        if (i1 >= i4 + 115 - 18 && i1 <= i4 + 115)
                        {
                            string5 = this.Func_27263_a(0);
                        }
                        else if (i1 >= i4 + 165 - 18 && i1 <= i4 + 165)
                        {
                            string5 = this.Func_27263_a(1);
                        }
                        else
                        {
                            if (i1 < i4 + 215 - 18 || i1 > i4 + 215)
                            {
                                return;
                            }

                            string5 = this.Func_27263_a(2);
                        }

                        string5 = ("" + I18N.Instance.TranslateKey(string5)).Trim();
                        if (string5.Length > 0)
                        {
                            int i6 = i1 + 12;
                            int i7 = i2 - 12;
                            int i8 = SELF.font.GetStringWidth(string5);
                            SELF.DrawGradientRect(i6 - 3, i7 - 3, i6 + i8 + 3, i7 + 8 + 3, -1073741824, -1073741824);
                            SELF.font.DrawStringWithShadow(string5, i6, i7, 0xffffffff);
                        }
                    }
                }
            }

            protected virtual void Func_27267_a(CraftingStat statCrafting1, int i2, int i3)
            {
                if (statCrafting1 != null)
                {
                    Item item4 = Item.items[statCrafting1.Fun_b()];
                    string string5 = ("" + I18N.Instance.TranslateNamedKey(item4.GetItemName())).Trim();
                    if (string5.Length > 0)
                    {
                        int i6 = i2 + 12;
                        int i7 = i3 - 12;
                        int i8 = SELF.font.GetStringWidth(string5);
                        SELF.DrawGradientRect(i6 - 3, i7 - 3, i6 + i8 + 3, i7 + 8 + 3, -1073741824, -1073741824);
                        SELF.font.DrawStringWithShadow(string5, i6, i7, 0xffffffff);
                    }
                }
            }

            protected virtual void Func_27266_c(int i1)
            {
                if (i1 != this.field_27271_e)
                {
                    this.field_27271_e = i1;
                    this.field_27270_f = -1;
                }
                else if (this.field_27270_f == -1)
                {
                    this.field_27270_f = 1;
                }
                else
                {
                    this.field_27271_e = -1;
                    this.field_27270_f = 0;
                }

                field_27273_c.Sort(this.snorter);
            }
        }

        private class GeneralStatisticsList : AbstractSelectionList
        {
            private readonly StatsScreen SELF;
            public GeneralStatisticsList(StatsScreen SELF) : base(SELF.mc, SELF.width, SELF.height, 32, SELF.height - 64, 10)
            {
                this.SELF = SELF;
                this.Func_27258_a(false);
            }

            protected override int GetSize()
            {
                return StatList.field_b.Count;
            }

            protected override void ElementClicked(int i1, bool z2)
            {
            }

            protected override bool IsSelected(int i1)
            {
                return false;
            }

            protected override int GetContentHeight()
            {
                return this.GetSize() * 10;
            }

            protected override void DrawBackground()
            {
                SELF.DrawDefaultBackground();
            }

            protected override void DrawSlot(int i1, int i2, int i3, int i4, Tessellator tessellator5)
            {
                Stat statBase6 = StatList.field_b[i1];
                SELF.DrawString(SELF.font, statBase6.statName, i2 + 2, i3 + 1, i1 % 2 == 0 ? 0xFFFFFF : 9474192);
                string string7 = statBase6.Func_27084_a(SELF.field_27156_o.WriteStat(statBase6));
                SELF.DrawString(SELF.font, string7, i2 + 2 + 213 - SELF.font.GetStringWidth(string7), i3 + 1, i1 % 2 == 0 ? 0xFFFFFF : 9474192);
            }
        }

        private class ItemStatisticsList : AbstractStatisticsList
        {
            private class Snorter2 : IComparer<CraftingStat>
            {
                private readonly ItemStatisticsList isl;
                private readonly StatsScreen self;
                public Snorter2(ItemStatisticsList isl, StatsScreen self)
                {
                    this.isl = isl;
                    this.self = self;
                }

                public virtual int Compare(CraftingStat statCrafting1, CraftingStat statCrafting2)
                {
                    int i3 = statCrafting1.Fun_b();
                    int i4 = statCrafting2.Fun_b();
                    Stat statBase5 = null;
                    Stat statBase6 = null;
                    if (isl.field_27271_e == 0)
                    {
                        statBase5 = StatList.itemDamageStats[i3];
                        statBase6 = StatList.itemDamageStats[i4];
                    }
                    else if (isl.field_27271_e == 1)
                    {
                        statBase5 = StatList.craftingStats[i3];
                        statBase6 = StatList.craftingStats[i4];
                    }
                    else if (isl.field_27271_e == 2)
                    {
                        statBase5 = StatList.itemUseStats[i3];
                        statBase6 = StatList.itemUseStats[i4];
                    }

                    if (statBase5 != null || statBase6 != null)
                    {
                        if (statBase5 == null)
                        {
                            return 1;
                        }

                        if (statBase6 == null)
                        {
                            return -1;
                        }

                        int i7 = self.field_27156_o.WriteStat(statBase5);
                        int i8 = self.field_27156_o.WriteStat(statBase6);
                        if (i7 != i8)
                        {
                            return (i7 - i8) * isl.field_27270_f;
                        }
                    }

                    return i3 - i4;
                }
            }

            public ItemStatisticsList(StatsScreen SELF) : base(SELF)
            {
                this.field_27273_c = new List<CraftingStat>();
                IEnumerator<CraftingStat> iterator2 = StatList.field_c.GetEnumerator();
                while (iterator2.MoveNext())
                {
                    CraftingStat statCrafting3 = iterator2.Current;
                    bool z4 = false;
                    int i5 = statCrafting3.Fun_b();
                    if (SELF.field_27156_o.WriteStat(statCrafting3) > 0)
                    {
                        z4 = true;
                    }
                    else if (StatList.itemDamageStats[i5] != null && SELF.field_27156_o.WriteStat(StatList.itemDamageStats[i5]) > 0)
                    {
                        z4 = true;
                    }
                    else if (StatList.craftingStats[i5] != null && SELF.field_27156_o.WriteStat(StatList.craftingStats[i5]) > 0)
                    {
                        z4 = true;
                    }

                    if (z4)
                    {
                        this.field_27273_c.Add(statCrafting3);
                    }
                }

                this.snorter = new Snorter2(this, SELF);
            }

            protected override void Func_27260_a(int i1, int i2, Tessellator tessellator3)
            {
                base.Func_27260_a(i1, i2, tessellator3);
                if (this.field_27268_b == 0)
                {
                    SELF.Func_27136_c(i1 + 115 - 18 + 1, i2 + 1 + 1, 72, 18);
                }
                else
                {
                    SELF.Func_27136_c(i1 + 115 - 18, i2 + 1, 72, 18);
                }

                if (this.field_27268_b == 1)
                {
                    SELF.Func_27136_c(i1 + 165 - 18 + 1, i2 + 1 + 1, 18, 18);
                }
                else
                {
                    SELF.Func_27136_c(i1 + 165 - 18, i2 + 1, 18, 18);
                }

                if (this.field_27268_b == 2)
                {
                    SELF.Func_27136_c(i1 + 215 - 18 + 1, i2 + 1 + 1, 36, 18);
                }
                else
                {
                    SELF.Func_27136_c(i1 + 215 - 18, i2 + 1, 36, 18);
                }
            }

            protected override void DrawSlot(int i1, int i2, int i3, int i4, Tessellator tessellator5)
            {
                CraftingStat statCrafting6 = this.Func_27264_b(i1);
                int i7 = statCrafting6.Fun_b();
                SELF.Func_27138_c(i2 + 40, i3, i7);
                this.Func_27265_a((CraftingStat)StatList.itemDamageStats[i7], i2 + 115, i3, i1 % 2 == 0);
                this.Func_27265_a((CraftingStat)StatList.craftingStats[i7], i2 + 165, i3, i1 % 2 == 0);
                this.Func_27265_a(statCrafting6, i2 + 215, i3, i1 % 2 == 0);
            }

            protected override string Func_27263_a(int i1)
            {
                return i1 == 1 ? "stat.crafted" : (i1 == 2 ? "stat.used" : "stat.depleted");
            }
        }

        private class BlockStatisticsList : AbstractStatisticsList
        {
            private class Snorter : IComparer<CraftingStat>
            {
                private readonly BlockStatisticsList slef;
                private readonly StatsScreen scrn;
                public Snorter(BlockStatisticsList bsl, StatsScreen scrn)
                {
                    this.slef = bsl;
                    this.scrn = scrn;
                }

                public virtual int Compare(CraftingStat statCrafting1, CraftingStat statCrafting2)
                {
                    int i3 = statCrafting1.Fun_b();
                    int i4 = statCrafting2.Fun_b();
                    Stat statBase5 = null;
                    Stat statBase6 = null;
                    if (slef.field_27271_e == 2)
                    {
                        statBase5 = StatList.mineBlockStatArray[i3];
                        statBase6 = StatList.mineBlockStatArray[i4];
                    }
                    else if (slef.field_27271_e == 0)
                    {
                        statBase5 = StatList.craftingStats[i3];
                        statBase6 = StatList.craftingStats[i4];
                    }
                    else if (slef.field_27271_e == 1)
                    {
                        statBase5 = StatList.itemUseStats[i3];
                        statBase6 = StatList.itemUseStats[i4];
                    }

                    if (statBase5 != null || statBase6 != null)
                    {
                        if (statBase5 == null)
                        {
                            return 1;
                        }

                        if (statBase6 == null)
                        {
                            return -1;
                        }

                        int i7 = scrn.field_27156_o.WriteStat(statBase5);
                        int i8 = scrn.field_27156_o.WriteStat(statBase6);
                        if (i7 != i8)
                        {
                            return (i7 - i8) * slef.field_27270_f;
                        }
                    }

                    return i3 - i4;
                }
            }

            public BlockStatisticsList(StatsScreen SELF) : base(SELF)
            {
                this.field_27273_c = new List<CraftingStat>();
                IEnumerator<CraftingStat> iterator2 = StatList.field_d.GetEnumerator();
                while (iterator2.MoveNext())
                {
                    CraftingStat statCrafting3 = iterator2.Current;
                    bool z4 = false;
                    int i5 = statCrafting3.Fun_b();
                    if (SELF.field_27156_o.WriteStat(statCrafting3) > 0)
                    {
                        z4 = true;
                    }
                    else if (StatList.itemUseStats[i5] != null && SELF.field_27156_o.WriteStat(StatList.itemUseStats[i5]) > 0)
                    {
                        z4 = true;
                    }
                    else if (StatList.craftingStats[i5] != null && SELF.field_27156_o.WriteStat(StatList.craftingStats[i5]) > 0)
                    {
                        z4 = true;
                    }

                    if (z4)
                    {
                        this.field_27273_c.Add(statCrafting3);
                    }
                }

                this.snorter = new Snorter(this, SELF);
            }

            protected override void Func_27260_a(int i1, int i2, Tessellator tessellator3)
            {
                base.Func_27260_a(i1, i2, tessellator3);
                if (this.field_27268_b == 0)
                {
                    SELF.Func_27136_c(i1 + 115 - 18 + 1, i2 + 1 + 1, 18, 18);
                }
                else
                {
                    SELF.Func_27136_c(i1 + 115 - 18, i2 + 1, 18, 18);
                }

                if (this.field_27268_b == 1)
                {
                    SELF.Func_27136_c(i1 + 165 - 18 + 1, i2 + 1 + 1, 36, 18);
                }
                else
                {
                    SELF.Func_27136_c(i1 + 165 - 18, i2 + 1, 36, 18);
                }

                if (this.field_27268_b == 2)
                {
                    SELF.Func_27136_c(i1 + 215 - 18 + 1, i2 + 1 + 1, 54, 18);
                }
                else
                {
                    SELF.Func_27136_c(i1 + 215 - 18, i2 + 1, 54, 18);
                }
            }

            protected override void DrawSlot(int i1, int i2, int i3, int i4, Tessellator tessellator5)
            {
                CraftingStat statCrafting6 = this.Func_27264_b(i1);
                int i7 = statCrafting6.Fun_b();
                SELF.Func_27138_c(i2 + 40, i3, i7);
                this.Func_27265_a((CraftingStat)StatList.craftingStats[i7], i2 + 115, i3, i1 % 2 == 0);
                this.Func_27265_a((CraftingStat)StatList.itemUseStats[i7], i2 + 165, i3, i1 % 2 == 0);
                this.Func_27265_a(statCrafting6, i2 + 215, i3, i1 % 2 == 0);
            }

            protected override string Func_27263_a(int i1)
            {
                return i1 == 0 ? "stat.crafted" : (i1 == 1 ? "stat.used" : "stat.mined");
            }
        }

        private static RenderItem field_27153_j = new RenderItem();
        protected Screen field_27152_a;
        protected string field_27154_i = "Select world";
        private GeneralStatisticsList field_27151_l;
        private ItemStatisticsList field_27150_m;
        private BlockStatisticsList field_27157_n;
        private StatFileWriter field_27156_o;
        private AbstractSelectionList field_27155_p = null;
        public StatsScreen(Screen guiScreen1, StatFileWriter statFileWriter2)
        {
            this.field_27152_a = guiScreen1;
            this.field_27156_o = statFileWriter2;
        }

        public override void InitGui()
        {
            this.field_27154_i = Locale.TranslateKey("gui.stats");
            this.field_27151_l = new GeneralStatisticsList(this);
            this.field_27151_l.RegisterScrollButtons(this.buttons, 1, 1);
            this.field_27150_m = new ItemStatisticsList(this);
            this.field_27150_m.RegisterScrollButtons(this.buttons, 1, 1);
            this.field_27157_n = new BlockStatisticsList(this);
            this.field_27157_n.RegisterScrollButtons(this.buttons, 1, 1);
            this.field_27155_p = this.field_27151_l;
            this.Func_27130_k();
        }

        public virtual void Func_27130_k()
        {
            I18N stringTranslate1 = I18N.Instance;
            this.buttons.Add(new Button(0, this.width / 2 + 4, this.height - 28, 150, 20, stringTranslate1.TranslateKey("gui.done")));
            this.buttons.Add(new Button(1, this.width / 2 - 154, this.height - 52, 100, 20, stringTranslate1.TranslateKey("stat.generalButton")));
            Button guiButton2;
            this.buttons.Add(guiButton2 = new Button(2, this.width / 2 - 46, this.height - 52, 100, 20, stringTranslate1.TranslateKey("stat.blocksButton")));
            Button guiButton3;
            this.buttons.Add(guiButton3 = new Button(3, this.width / 2 + 62, this.height - 52, 100, 20, stringTranslate1.TranslateKey("stat.itemsButton")));
            if (this.field_27157_n.PubGetSize() == 0)
            {
                guiButton2.enabled = false;
            }

            if (this.field_27150_m.PubGetSize() == 0)
            {
                guiButton3.enabled = false;
            }
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.enabled)
            {
                if (guiButton1.id == 0)
                {
                    this.mc.SetScreen(this.field_27152_a);
                }
                else if (guiButton1.id == 1)
                {
                    this.field_27155_p = this.field_27151_l;
                }
                else if (guiButton1.id == 3)
                {
                    this.field_27155_p = this.field_27150_m;
                }
                else if (guiButton1.id == 2)
                {
                    this.field_27155_p = this.field_27157_n;
                }
                else
                {
                    this.field_27155_p.ActionPerformed(guiButton1);
                }
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.field_27155_p.DrawScreen(i1, i2, f3);
            this.DrawCenteredString(this.font, this.field_27154_i, this.width / 2, 20, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }

        private void Func_27138_c(int i1, int i2, int i3)
        {
            this.Func_27147_a(i1 + 1, i2 + 1);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            GL11.glPushMatrix();
            GL11.glRotatef(180F, 1F, 0F, 0F);
            Light.TurnOn();
            GL11.glPopMatrix();
            field_27153_j.DrawItemIntoGui(this.font, this.mc.textures, i3, 0, Item.items[i3].GetIconFromDamage(0), i1 + 2, i2 + 2);
            Light.TurnOff();
            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
        }

        private void Func_27147_a(int i1, int i2)
        {
            this.Func_27136_c(i1, i2, 0, 0);
        }

        private void Func_27136_c(int i1, int i2, int i3, int i4)
        {
            uint i5 = this.mc.textures.LoadTexture("/gui/slot.png");
            GL11.glColor4f(1F, 1F, 1F, 1F);
            this.mc.textures.Bind(i5);
            Tessellator tessellator10 = Tessellator.Instance;
            tessellator10.Begin();
            tessellator10.VertexUV(i1 + 0, i2 + 18, this.zLevel, (i3 + 0) * 0.0078125F, (i4 + 18) * 0.0078125F);
            tessellator10.VertexUV(i1 + 18, i2 + 18, this.zLevel, (i3 + 18) * 0.0078125F, (i4 + 18) * 0.0078125F);
            tessellator10.VertexUV(i1 + 18, i2 + 0, this.zLevel, (i3 + 18) * 0.0078125F, (i4 + 0) * 0.0078125F);
            tessellator10.VertexUV(i1 + 0, i2 + 0, this.zLevel, (i3 + 0) * 0.0078125F, (i4 + 0) * 0.0078125F);
            tessellator10.End();
        }
    }
}
