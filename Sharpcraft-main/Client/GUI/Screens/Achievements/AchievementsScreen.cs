using LWCSGL.Input;
using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Client.Stats;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Achievements
{
    public class AchievementsScreen : Screen
    {
        private static readonly int field_27126_s = AchievementList.minDisplayColumn * 24 - 112;
        private static readonly int field_27125_t = AchievementList.minDisplayRow * 24 - 112;
        private static readonly int field_27124_u = AchievementList.maxDisplayColumn * 24 - 77;
        private static readonly int field_27123_v = AchievementList.maxDisplayRow * 24 - 77;
        protected int field_27121_a = 256;
        protected int field_27119_i = 202;
        protected int field_27118_j = 0;
        protected int field_27117_l = 0;
        protected double field_27116_m;
        protected double field_27115_n;
        protected double field_27114_o;
        protected double field_27113_p;
        protected double field_27112_q;
        protected double field_27111_r;
        private int field_27122_w = 0;
        private StatFileWriter field_27120_x;
        public AchievementsScreen(StatFileWriter statFileWriter1)
        {
            this.field_27120_x = statFileWriter1;
            short s2 = 141;
            short s3 = 141;
            this.field_27116_m = this.field_27114_o = this.field_27112_q = AchievementList.openInventory.displayColumn * 24 - s2 / 2 - 12;
            this.field_27115_n = this.field_27113_p = this.field_27111_r = AchievementList.openInventory.displayRow * 24 - s3 / 2;
        }

        public override void InitGui()
        {
            this.buttons.Clear();
            this.buttons.Add(new SmallButton(1, this.width / 2 + 24, this.height / 2 + 74, 80, 20, Locale.TranslateKey("gui.done")));
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.id == 1)
            {
                this.mc.SetScreen((Screen)null);
                this.mc.SetIngameFocus();
            }

            base.ActionPerformed(guiButton1);
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
            if (i2 == this.mc.options.keyBindInventory.keyCode)
            {
                this.mc.SetScreen((Screen)null);
                this.mc.SetIngameFocus();
            }
            else
            {
                base.KeyTyped(c1, i2);
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            if (Mouse.IsButtonDown(0))
            {
                int i4 = (this.width - this.field_27121_a) / 2;
                int i5 = (this.height - this.field_27119_i) / 2;
                int i6 = i4 + 8;
                int i7 = i5 + 17;
                if ((this.field_27122_w == 0 || this.field_27122_w == 1) && i1 >= i6 && i1 < i6 + 224 && i2 >= i7 && i2 < i7 + 155)
                {
                    if (this.field_27122_w == 0)
                    {
                        this.field_27122_w = 1;
                    }
                    else
                    {
                        this.field_27114_o -= i1 - this.field_27118_j;
                        this.field_27113_p -= i2 - this.field_27117_l;
                        this.field_27112_q = this.field_27116_m = this.field_27114_o;
                        this.field_27111_r = this.field_27115_n = this.field_27113_p;
                    }

                    this.field_27118_j = i1;
                    this.field_27117_l = i2;
                }

                if (this.field_27112_q < field_27126_s)
                {
                    this.field_27112_q = field_27126_s;
                }

                if (this.field_27111_r < field_27125_t)
                {
                    this.field_27111_r = field_27125_t;
                }

                if (this.field_27112_q >= field_27124_u)
                {
                    this.field_27112_q = field_27124_u - 1;
                }

                if (this.field_27111_r >= field_27123_v)
                {
                    this.field_27111_r = field_27123_v - 1;
                }
            }
            else
            {
                this.field_27122_w = 0;
            }

            this.DrawDefaultBackground();
            this.Func_27109_b(i1, i2, f3);
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glDisable(GL11C.GL_DEPTH_TEST);
            this.Func_27110_k();
            GL11.glEnable(GL11C.GL_LIGHTING);
            GL11.glEnable(GL11C.GL_DEPTH_TEST);
        }

        public override void UpdateScreen()
        {
            this.field_27116_m = this.field_27114_o;
            this.field_27115_n = this.field_27113_p;
            double d1 = this.field_27112_q - this.field_27114_o;
            double d3 = this.field_27111_r - this.field_27113_p;
            if (d1 * d1 + d3 * d3 < 4)
            {
                this.field_27114_o += d1;
                this.field_27113_p += d3;
            }
            else
            {
                this.field_27114_o += d1 * 0.85;
                this.field_27113_p += d3 * 0.85;
            }
        }

        protected virtual void Func_27110_k()
        {
            int i1 = (this.width - this.field_27121_a) / 2;
            int i2 = (this.height - this.field_27119_i) / 2;
            this.font.DrawString("Achievements", i1 + 15, i2 + 5, 4210752);
        }

        protected virtual void Func_27109_b(int i1, int i2, float f3)
        {
            int i4 = Mth.Floor(this.field_27116_m + (this.field_27114_o - this.field_27116_m) * f3);
            int i5 = Mth.Floor(this.field_27115_n + (this.field_27113_p - this.field_27115_n) * f3);
            if (i4 < field_27126_s)
            {
                i4 = field_27126_s;
            }

            if (i5 < field_27125_t)
            {
                i5 = field_27125_t;
            }

            if (i4 >= field_27124_u)
            {
                i4 = field_27124_u - 1;
            }

            if (i5 >= field_27123_v)
            {
                i5 = field_27123_v - 1;
            }

            uint i6 = this.mc.textures.LoadTexture("/terrain.png");
            uint i7 = this.mc.textures.LoadTexture("/achievement/bg.png");
            int i8 = (this.width - this.field_27121_a) / 2;
            int i9 = (this.height - this.field_27119_i) / 2;
            int i10 = i8 + 16;
            int i11 = i9 + 17;
            this.zLevel = 0F;
            GL11.glDepthFunc(GL11C.GL_GEQUAL);
            GL11.glPushMatrix();
            GL11.glTranslatef(0F, 0F, -200F);
            GL11.glEnable(GL11C.GL_TEXTURE_2D);
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            GL11.glEnable(GL11C.GL_COLOR_MATERIAL);
            this.mc.textures.Bind(i6);
            int i12 = i4 + 288 >> 4;
            int i13 = i5 + 288 >> 4;
            int i14 = (i4 + 288) % 16;
            int i15 = (i5 + 288) % 16;
            JRandom random21 = new JRandom();
            for (int i22 = 0; i22 * 16 - i15 < 155; ++i22)
            {
                float f23 = 0.6F - (i13 + i22) / 25F * 0.3F;
                GL11.glColor4f(f23, f23, f23, 1F);
                for (int i24 = 0; i24 * 16 - i14 < 224; ++i24)
                {
                    random21.SetSeed(1234 + i12 + i24);
                    random21.NextInt();
                    int i25 = random21.NextInt(1 + i13 + i22) + (i13 + i22) / 2;
                    int i26 = Tile.sand.texture;
                    if (i25 <= 37 && i13 + i22 != 35)
                    {
                        if (i25 == 22)
                        {
                            if (random21.NextInt(2) == 0)
                            {
                                i26 = Tile.oreDiamond.texture;
                            }
                            else
                            {
                                i26 = Tile.oreRedstone.texture;
                            }
                        }
                        else if (i25 == 10)
                        {
                            i26 = Tile.ironOre.texture;
                        }
                        else if (i25 == 8)
                        {
                            i26 = Tile.coalOre.texture;
                        }
                        else if (i25 > 4)
                        {
                            i26 = Tile.rock.texture;
                        }
                        else if (i25 > 0)
                        {
                            i26 = Tile.dirt.texture;
                        }
                    }
                    else
                    {
                        i26 = Tile.unbreakable.texture;
                    }

                    this.DrawTexturedModalRect(i10 + i24 * 16 - i14, i11 + i22 * 16 - i15, i26 % 16 << 4, i26 >> 4 << 4, 16, 16);
                }
            }

            GL11.glEnable(GL11C.GL_DEPTH_TEST);
            GL11.glDepthFunc(GL11C.GL_LEQUAL);
            GL11.glDisable(GL11C.GL_TEXTURE_2D);
            int i16;
            int i17;
            int i33;
            int i38;
            for (i12 = 0; i12 < AchievementList.achievementList.Count; ++i12)
            {
                Achievement achievement28 = AchievementList.achievementList[i12];
                if (achievement28.parentAchievement != null)
                {
                    i14 = achievement28.displayColumn * 24 - i4 + 11 + i10;
                    i15 = achievement28.displayRow * 24 - i5 + 11 + i11;
                    i16 = achievement28.parentAchievement.displayColumn * 24 - i4 + 11 + i10;
                    i17 = achievement28.parentAchievement.displayRow * 24 - i5 + 11 + i11;
                    bool z19 = this.field_27120_x.HasAchievementUnlocked(achievement28);
                    bool z20 = this.field_27120_x.Func_27181_b(achievement28);
                    i38 = Math.Sin(TimeUtil.MilliTime % 600 / 600 * Math.PI * 2) > 0.6 ? 255 : 130;
                    if (z19)
                    {
                        i33 = -9408400;
                    }
                    else if (z20)
                    {
                        i33 = 65280 + (i38 << 24);
                    }
                    else
                    {
                        i33 = unchecked((int)0xFF000000); //notch i hate u
                    }

                    this.Func_27100_a(i14, i16, i15, i33);
                    this.Func_27099_b(i16, i15, i17, i33);
                }
            }

            Achievement achievement27 = null;
            RenderItem renderItem29 = new RenderItem();
            GL11.glPushMatrix();
            GL11.glRotatef(180F, 1F, 0F, 0F);
            Light.TurnOn();
            GL11.glPopMatrix();
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            GL11.glEnable(GL11C.GL_COLOR_MATERIAL);
            int i34;
            for (i14 = 0; i14 < AchievementList.achievementList.Count; ++i14)
            {
                Achievement achievement30 = AchievementList.achievementList[i14];
                i16 = achievement30.displayColumn * 24 - i4;
                i17 = achievement30.displayRow * 24 - i5;
                if (i16 >= -24 && i17 >= -24 && i16 <= 224 && i17 <= 155)
                {
                    float f35;
                    if (this.field_27120_x.HasAchievementUnlocked(achievement30))
                    {
                        f35 = 1F;
                        GL11.glColor4f(f35, f35, f35, 1F);
                    }
                    else if (this.field_27120_x.Func_27181_b(achievement30))
                    {
                        f35 = Math.Sin(TimeUtil.MilliTime % 600 / 600 * Math.PI * 2) < 0.6 ? 0.6F : 0.8F;
                        GL11.glColor4f(f35, f35, f35, 1F);
                    }
                    else
                    {
                        f35 = 0.3F;
                        GL11.glColor4f(f35, f35, f35, 1F);
                    }

                    this.mc.textures.Bind(i7);
                    i33 = i10 + i16;
                    i34 = i11 + i17;
                    if (achievement30.GetSpecial())
                    {
                        this.DrawTexturedModalRect(i33 - 2, i34 - 2, 26, 202, 26, 26);
                    }
                    else
                    {
                        this.DrawTexturedModalRect(i33 - 2, i34 - 2, 0, 202, 26, 26);
                    }

                    if (!this.field_27120_x.Func_27181_b(achievement30))
                    {
                        float f36 = 0.1F;
                        GL11.glColor4f(f36, f36, f36, 1F);
                        renderItem29.field_27004_a = false;
                    }

                    GL11.glEnable(GL11C.GL_LIGHTING);
                    GL11.glEnable(GL11C.GL_CULL_FACE);
                    renderItem29.RenderItemIntoGUI(this.mc.font, this.mc.textures, achievement30.item, i33 + 3, i34 + 3);
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    if (!this.field_27120_x.Func_27181_b(achievement30))
                    {
                        renderItem29.field_27004_a = true;
                    }

                    GL11.glColor4f(1F, 1F, 1F, 1F);
                    if (i1 >= i10 && i2 >= i11 && i1 < i10 + 224 && i2 < i11 + 155 && i1 >= i33 && i1 <= i33 + 22 && i2 >= i34 && i2 <= i34 + 22)
                    {
                        achievement27 = achievement30;
                    }
                }
            }

            GL11.glDisable(GL11C.GL_DEPTH_TEST);
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glColor4f(1F, 1F, 1F, 1F);
            this.mc.textures.Bind(i7);
            this.DrawTexturedModalRect(i8, i9, 0, 0, this.field_27121_a, this.field_27119_i);
            GL11.glPopMatrix();
            this.zLevel = 0F;
            GL11.glDepthFunc(GL11C.GL_LEQUAL);
            GL11.glDisable(GL11C.GL_DEPTH_TEST);
            GL11.glEnable(GL11C.GL_TEXTURE_2D);
            base.DrawScreen(i1, i2, f3);
            if (achievement27 != null)
            {
                string string31 = achievement27.statName;
                string string32 = achievement27.GetDescription();
                i17 = i1 + 12;
                i33 = i2 - 4;
                if (this.field_27120_x.Func_27181_b(achievement27))
                {
                    i34 = Math.Max(this.font.GetStringWidth(string31), 120);
                    int i37 = this.font.DrawMultiline(string32, i34);
                    if (this.field_27120_x.HasAchievementUnlocked(achievement27))
                    {
                        i37 += 12;
                    }

                    this.DrawGradientRect(i17 - 3, i33 - 3, i17 + i34 + 3, i33 + i37 + 3 + 12, -1073741824, -1073741824);
                    this.font.DrawMultiline(string32, i17, i33 + 12, i34, 0xA0A0A0);
                    if (this.field_27120_x.HasAchievementUnlocked(achievement27))
                    {
                        this.font.DrawStringWithShadow(Locale.TranslateKey("achievement.taken"), i17, i33 + i37 + 4, 0x9090FF);
                    }
                }
                else
                {
                    i34 = Math.Max(this.font.GetStringWidth(string31), 120);
                    if (achievement27.parentAchievement == null) return;
                    string string39 = Locale.TranslateKeyFormat("achievement.requires", achievement27.parentAchievement.statName);
                    i38 = this.font.DrawMultiline(string39, i34);
                    this.DrawGradientRect(i17 - 3, i33 - 3, i17 + i34 + 3, i33 + i38 + 12 + 3, -1073741824, -1073741824);
                    this.font.DrawMultiline(string39, i17, i33 + 12, i34, 0x705050);
                }

                this.font.DrawStringWithShadow(string31, i17, i33, (this.field_27120_x.Func_27181_b(achievement27) ? (achievement27.GetSpecial() ? 0xFFFF80 : uint.MaxValue) : (achievement27.GetSpecial() ? (uint)0x808040 : 0x808080)));
            }

            GL11.glEnable(GL11C.GL_DEPTH_TEST);
            GL11.glEnable(GL11C.GL_LIGHTING);
            Light.TurnOff();
        }

        public override bool DoesGuiPauseGame()
        {
            return true;
        }
    }
}
