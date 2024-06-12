using SharpCraft.Core.i18n;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Client.Options;

namespace SharpCraft.Client.GUI.Screens.Optionss
{
    public class VideoSettingsScreen : Screen
    {
        private Screen field_22110_h;
        protected string field_22107_a = "Video Settings";
        private Options guiGameSettings;
        private static Option[] field_22108_k = new[]
        {
            Option.GRAPHICS,
            Option.RENDER_DISTANCE,
            Option.AMBIENT_OCCLUSION,
            Option.FRAMERATE_LIMIT,
            Option.ANAGLYPH,
            Option.VIEW_BOBBING,
            Option.GUI_SCALE,
            Option.ADVANCED_OPENGL
        };
        public VideoSettingsScreen(Screen guiScreen1, Options gameSettings2)
        {
            this.field_22110_h = guiScreen1;
            this.guiGameSettings = gameSettings2;
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            this.field_22107_a = stringTranslate1.TranslateKey("options.videoTitle");
            int i2 = 0;
            Option[] enumOptions3 = field_22108_k;
            int i4 = enumOptions3.Length;
            for (int i5 = 0; i5 < i4; ++i5)
            {
                Option enumOptions6 = enumOptions3[i5];
                if (!enumOptions6.GetEnumFloat())
                {
                    this.buttons.Add(new SmallButton(enumOptions6.ReturnEnumOrdinal(), this.width / 2 - 155 + i2 % 2 * 160, this.height / 6 + 24 * (i2 >> 1), enumOptions6, this.guiGameSettings.GetKeyBinding(enumOptions6)));
                }
                else
                {
                    this.buttons.Add(new Slider(enumOptions6.ReturnEnumOrdinal(), this.width / 2 - 155 + i2 % 2 * 160, this.height / 6 + 24 * (i2 >> 1), enumOptions6, this.guiGameSettings.GetKeyBinding(enumOptions6), this.guiGameSettings.GetOptionFloatValue(enumOptions6)));
                }

                ++i2;
            }

            this.buttons.Add(new Button(200, this.width / 2 - 100, this.height / 6 + 168, stringTranslate1.TranslateKey("gui.done")));
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.enabled)
            {
                if (guiButton1.id < 100 && guiButton1 is SmallButton)
                {
                    this.guiGameSettings.SetOptionValue(((SmallButton)guiButton1).ReturnEnumOptions(), 1);
                    guiButton1.displayString = this.guiGameSettings.GetKeyBinding(Option.GetEnumOptions(guiButton1.id));
                }

                if (guiButton1.id == 200)
                {
                    this.mc.options.SaveOptions();
                    this.mc.SetScreen(this.field_22110_h);
                }

                GuiScale scaledResolution2 = new GuiScale(this.mc.options, this.mc.displayWidth, this.mc.displayHeight);
                int i3 = scaledResolution2.GetWidth();
                int i4 = scaledResolution2.GetHeight();
                this.Init(this.mc, i3, i4);
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            this.DrawCenteredString(this.font, this.field_22107_a, this.width / 2, 20, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }
    }
}
