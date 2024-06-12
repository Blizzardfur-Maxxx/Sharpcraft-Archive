using SharpCraft.Client.GUI;
using SharpCraft.Client.GUI.Screens;
using SharpCraft.Client.GUI.Screens.Achievements;
using SharpCraft.Client.GUI.Screens.Optionss;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel;
using System;

namespace SharpCraft.Client.GUI.Screens
{
    public class PauseScreen : Screen
    {
        private int updateCounter2 = 0;
        private int updateCounter = 0;
        public override void InitGui()
        {
            updateCounter2 = 0;
            buttons.Clear();
            sbyte b1 = -16;
            buttons.Add(new Button(1, width / 2 - 100, height / 4 + 120 + b1, "Save and quit to title"));
            if (mc.IsMultiplayerWorld())
            {
                buttons[0].displayString = "Disconnect";
            }

            buttons.Add(new Button(4, width / 2 - 100, height / 4 + 24 + b1, "Back to game"));
            buttons.Add(new Button(0, width / 2 - 100, height / 4 + 96 + b1, "Options..."));
            buttons.Add(new Button(5, width / 2 - 100, height / 4 + 48 + b1, 98, 20, Locale.TranslateKey("gui.achievements")));
            buttons.Add(new Button(6, width / 2 + 2, height / 4 + 48 + b1, 98, 20, Locale.TranslateKey("gui.stats")));
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.id == 0)
            {
                mc.SetScreen(new OptionsScreen(this, mc.options));
            }

            if (guiButton1.id == 1)
            {
                mc.statFileWriter.ReadStat(StatList.leaveGame, 1);
                if (mc.IsMultiplayerWorld())
                {
                    mc.level.SendQuittingDisconnectingPacket();
                }

                mc.SetLevel((Level)null);
                mc.SetScreen(new StartMenuScreen());
            }

            if (guiButton1.id == 4)
            {
                mc.SetScreen((Screen)null);
                mc.SetIngameFocus();
            }

            if (guiButton1.id == 5)
            {
                mc.SetScreen(new AchievementsScreen(mc.statFileWriter));
            }

            if (guiButton1.id == 6)
            {
                mc.SetScreen(new StatsScreen(this, mc.statFileWriter));
            }
        }

        public override void UpdateScreen()
        {
            base.UpdateScreen();
            ++updateCounter;
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            DrawDefaultBackground();
            bool z4 = !mc.level.Func_650_a(updateCounter2++);
            if (z4 || updateCounter < 20)
            {
                float f5 = (updateCounter % 10 + f3) / 10F;
                f5 = Mth.Sin(f5 * Mth.PI * 2F) * 0.2F + 0.8F;
                int i6 = (int)(255F * f5);
                DrawString(font, "Saving level..", 8, height - 16, i6 << 16 | i6 << 8 | i6);
            }

            DrawCenteredString(font, "Game menu", width / 2, 40, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }
    }
}