using SharpCraft.Core.i18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Client.Options;

namespace SharpCraft.Client.GUI.Screens.Optionss
{
    public class OptionsScreen : Screen
    {
        private Screen parentScreen;
        protected string screenTitle = "Options";
        private Options options;
        private static Option[] field_22135_k = new[]
        {
            Option.MUSIC,
            Option.SOUND,
            Option.INVERT_MOUSE,
            Option.SENSITIVITY,
            Option.DIFFICULTY
        };
        public OptionsScreen(Screen guiScreen1, Options gameSettings2)
        {
            parentScreen = guiScreen1;
            options = gameSettings2;
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            screenTitle = stringTranslate1.TranslateKey("options.title");
            int i2 = 0;
            Option[] enumOptions3 = field_22135_k;
            int i4 = enumOptions3.Length;
            for (int i5 = 0; i5 < i4; ++i5)
            {
                Option enumOptions6 = enumOptions3[i5];
                if (!enumOptions6.GetEnumFloat())
                {
                    buttons.Add(new SmallButton(enumOptions6.ReturnEnumOrdinal(), width / 2 - 155 + i2 % 2 * 160, height / 6 + 24 * (i2 >> 1), enumOptions6, options.GetKeyBinding(enumOptions6)));
                }
                else
                {
                    buttons.Add(new Slider(enumOptions6.ReturnEnumOrdinal(), width / 2 - 155 + i2 % 2 * 160, height / 6 + 24 * (i2 >> 1), enumOptions6, options.GetKeyBinding(enumOptions6), options.GetOptionFloatValue(enumOptions6)));
                }

                ++i2;
            }

            buttons.Add(new Button(101, width / 2 - 100, height / 6 + 96 + 12, stringTranslate1.TranslateKey("options.video")));
            buttons.Add(new Button(100, width / 2 - 100, height / 6 + 120 + 12, stringTranslate1.TranslateKey("options.controls")));
            buttons.Add(new Button(200, width / 2 - 100, height / 6 + 168, stringTranslate1.TranslateKey("gui.done")));
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.enabled)
            {
                if (guiButton1.id < 100 && guiButton1 is SmallButton)
                {
                    options.SetOptionValue(((SmallButton)guiButton1).ReturnEnumOptions(), 1);
                    guiButton1.displayString = options.GetKeyBinding(Option.GetEnumOptions(guiButton1.id));
                }

                if (guiButton1.id == 101)
                {
                    mc.options.SaveOptions();
                    mc.SetScreen(new VideoSettingsScreen(this, options));
                }

                if (guiButton1.id == 100)
                {
                    mc.options.SaveOptions();
                    mc.SetScreen(new ControlsScreen(this, options));
                }

                if (guiButton1.id == 200)
                {
                    mc.options.SaveOptions();
                    mc.SetScreen(parentScreen);
                }
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            DrawDefaultBackground();
            DrawCenteredString(font, screenTitle, width / 2, 20, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }
    }
}
