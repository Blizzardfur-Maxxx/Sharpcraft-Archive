using LWCSGL.Input;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client
{
    internal enum OptType
    {
        MUSIC,
        SOUND,
        INVERT_MOUSE,
        SENSITIVITY,
        RENDER_DISTANCE,
        VIEW_BOBBING,
        ANAGLYPH,
        ADVANCED_OPENGL,
        FRAMERATE_LIMIT,
        DIFFICULTY,
        GRAPHICS,
        AMBIENT_OCCLUSION,
        GUI_SCALE
    }

    public class Options
    {
        public class Option
        {
            private static int counter;
            private static readonly Option[] vals = new Option[13];
            private readonly int ord;

            //this needs to be redone but unfortunate tale of using switch()
            public static readonly Option
                MUSIC = new Option("options.music", true, false, OptType.MUSIC),
                SOUND = new Option("options.sound", true, false, OptType.SOUND),
                INVERT_MOUSE = new Option("options.invertMouse", false, true, OptType.INVERT_MOUSE),
                SENSITIVITY = new Option("options.sensitivity", true, false, OptType.SENSITIVITY),
                RENDER_DISTANCE = new Option("options.renderDistance", false, false, OptType.RENDER_DISTANCE),
                VIEW_BOBBING = new Option("options.viewBobbing", false, true, OptType.VIEW_BOBBING),
                ANAGLYPH = new Option("options.anaglyph", false, true, OptType.ANAGLYPH),
                ADVANCED_OPENGL = new Option("options.advancedOpengl", false, true, OptType.ADVANCED_OPENGL),
                FRAMERATE_LIMIT = new Option("options.framerateLimit", false, false, OptType.FRAMERATE_LIMIT),
                DIFFICULTY = new Option("options.difficulty", false, false, OptType.DIFFICULTY),
                GRAPHICS = new Option("options.graphics", false, false, OptType.GRAPHICS),
                AMBIENT_OCCLUSION = new Option("options.ao", false, true, OptType.AMBIENT_OCCLUSION),
                GUI_SCALE = new Option("options.guiScale", false, false, OptType.GUI_SCALE);

            private readonly bool enumFloat;
            private readonly bool enumBoolean;
            private readonly string enumString;
            internal readonly OptType eOptType;

            public static Option[] Values()
            {
                return vals;
            }

            public int Ordinal()
            {
                return this.ord;
            }

            public static Option GetEnumOptions(int i0)
            {
                Option[] enumOptions1 = Values();
                int i2 = enumOptions1.Length;

                for (int i3 = 0; i3 < i2; ++i3)
                {
                    Option enumOptions4 = enumOptions1[i3];
                    if (enumOptions4.ReturnEnumOrdinal() == i0)
                    {
                        return enumOptions4;
                    }
                }

                return null;
            }

            private Option(string string3, bool z4, bool z5, OptType optType)
            {
                this.enumString = string3;
                this.enumFloat = z4;
                this.enumBoolean = z5;
                this.eOptType = optType;
                int c = counter++;
                this.ord = c;
                vals[c] = this;
            }

            public bool GetEnumFloat()
            {
                return this.enumFloat;
            }

            public bool GetEnumBoolean()
            {
                return this.enumBoolean;
            }

            public int ReturnEnumOrdinal()
            {
                return this.Ordinal();
            }

            public string GetEnumString()
            {
                return this.enumString;
            }
        }

        private static readonly string[] RENDER_DISTANCES = { "options.renderDistance.far", "options.renderDistance.normal", "options.renderDistance.short", "options.renderDistance.tiny" };
        private static readonly string[] DIFFICULTIES = { "options.difficulty.peaceful", "options.difficulty.easy", "options.difficulty.normal", "options.difficulty.hard" };
        private static readonly string[] GUISCALES = { "options.guiScale.auto", "options.guiScale.small", "options.guiScale.normal", "options.guiScale.large" };
        private static readonly string[] LIMIT_FRAMERATES = { "performance.max", "performance.balanced", "performance.powersaver" };
        public float musicVolume = 1.0F;
        public float soundVolume = 1.0F;
        public float mouseSensitivity = 0.5F;
        public bool invertMouse = false;
        public int renderDistance = 0;
        public bool viewBobbing = true;
        public bool anaglyph = false;
        public bool advancedOpengl = false;
        public int limitFramerate = 1;
        public bool fancyGraphics = true;
        public bool ambientOcclusion = true;
        public string skin = "Default";
        public KeyMapping keyBindForward = new KeyMapping("key.forward", VirtualKey.W);
        public KeyMapping keyBindLeft = new KeyMapping("key.left", VirtualKey.A);
        public KeyMapping keyBindBack = new KeyMapping("key.back", VirtualKey.S);
        public KeyMapping keyBindRight = new KeyMapping("key.right", VirtualKey.D);
        public KeyMapping keyBindJump = new KeyMapping("key.jump", VirtualKey.Space);
        public KeyMapping keyBindInventory = new KeyMapping("key.inventory", VirtualKey.E);
        public KeyMapping keyBindDrop = new KeyMapping("key.drop", VirtualKey.Q);
        public KeyMapping keyBindChat = new KeyMapping("key.chat", VirtualKey.T);
        public KeyMapping keyBindToggleFog = new KeyMapping("key.fog", VirtualKey.F);
        public KeyMapping keyBindSneak = new KeyMapping("key.sneak", VirtualKey.ShiftKey);
        public KeyMapping[] keyBindings;
        protected Client mc;
        private JFile optionsFile;
        public int difficulty = 2;
        public bool hideGUI = false;
        public bool thirdPersonView = false;
        public bool showDebugInfo = false;
        public string lastServer = "";
        public bool field_22275_C = false;
        public bool smoothCamera = false;
        public bool field_22273_E = false;
        public float field_22272_F = 1.0F;
        public float field_22271_G = 1.0F;
        public int guiScale = 0;

        public Options(Client mc, JFile workingDir) 
        {
            keyBindings = new KeyMapping[] { keyBindForward, keyBindLeft, keyBindBack, keyBindRight, keyBindJump, keyBindSneak, keyBindDrop, keyBindInventory, keyBindChat, keyBindToggleFog};
            this.mc = mc;
            this.optionsFile = new JFile(workingDir, "options.txt");
            LoadOptions();
        }

        [Obsolete("Use the constructor that takes 2 arguments when finished with the port.")]
        public Options() 
        {
        }

        public string GetKeyBindingDescription(int i1)
        {
            I18N stringTranslate2 = I18N.Instance;
            return stringTranslate2.TranslateKey(this.keyBindings[i1].keyDescription);
        }

        public string GetOptionDisplayString(int i1)
        {
            return Keyboard.GetKeyName(this.keyBindings[i1].keyCode);
        }

        public void SetKeyBinding(int i1, int i2)
        {
            this.keyBindings[i1].keyCode = (VirtualKey)i2;
            this.SaveOptions();
        }

        public void SetOptionFloatValue(Option enumOptions1, float f2)
        {
            if (enumOptions1 == Option.MUSIC)
            {
                this.musicVolume = f2;
                this.mc.soundEngine.OnSoundOptionsChanged();
            }

            if (enumOptions1 == Option.SOUND)
            {
                this.soundVolume = f2;
                this.mc.soundEngine.OnSoundOptionsChanged();
            }

            if (enumOptions1 == Option.SENSITIVITY)
            {
                this.mouseSensitivity = f2;
            }

        }

        public void SetOptionValue(Option enumOptions1, int i2)
        {
            if (enumOptions1 == Option.INVERT_MOUSE)
            {
                this.invertMouse = !this.invertMouse;
            }

            if (enumOptions1 == Option.RENDER_DISTANCE)
            {
                this.renderDistance = this.renderDistance + i2 & 3;
            }

            if (enumOptions1 == Option.GUI_SCALE)
            {
                this.guiScale = this.guiScale + i2 & 3;
            }

            if (enumOptions1 == Option.VIEW_BOBBING)
            {
                this.viewBobbing = !this.viewBobbing;
            }

            if (enumOptions1 == Option.ADVANCED_OPENGL)
            {
                this.advancedOpengl = !this.advancedOpengl;
                this.mc.renderGlobal.LoadRenderers();
            }

            if (enumOptions1 == Option.ANAGLYPH)
            {
                this.anaglyph = !this.anaglyph;
                this.mc.textures.ReloadAll();
            }

            if (enumOptions1 == Option.FRAMERATE_LIMIT)
            {
                this.limitFramerate = (this.limitFramerate + i2 + 3) % 3;
            }

            if (enumOptions1 == Option.DIFFICULTY)
            {
                this.difficulty = this.difficulty + i2 & 3;
            }

            if (enumOptions1 == Option.GRAPHICS)
            {
                this.fancyGraphics = !this.fancyGraphics;
                this.mc.renderGlobal.LoadRenderers();
            }

            if (enumOptions1 == Option.AMBIENT_OCCLUSION)
            {
                this.ambientOcclusion = !this.ambientOcclusion;
                this.mc.renderGlobal.LoadRenderers();
            }

            this.SaveOptions();
        }

        public float GetOptionFloatValue(Option enumOptions1)
        {
            return enumOptions1 == Option.MUSIC ? this.musicVolume : (enumOptions1 == Option.SOUND ? this.soundVolume : (enumOptions1 == Option.SENSITIVITY ? this.mouseSensitivity : 0.0F));
        }

        public bool GetOptionOrdinalValue(Option enumOptions1)
        {
            switch (enumOptions1.eOptType)
            {
                case OptType.INVERT_MOUSE:
                    return this.invertMouse;
                case OptType.VIEW_BOBBING:
                    return this.viewBobbing;
                case OptType.ANAGLYPH:
                    return this.anaglyph;
                case OptType.ADVANCED_OPENGL:
                    return this.advancedOpengl;
                case OptType.AMBIENT_OCCLUSION:
                    return this.ambientOcclusion;
                default:
                    return false;
            }
        }

        public string GetKeyBinding(Option enumOptions1)
        {
            I18N stringTranslate2 = I18N.Instance;
            string string3 = stringTranslate2.TranslateKey(enumOptions1.GetEnumString()) + ": ";
            if (enumOptions1.GetEnumFloat())
            {
                float f5 = this.GetOptionFloatValue(enumOptions1);
                return enumOptions1 == Option.SENSITIVITY ? (f5 == 0.0F ? string3 + stringTranslate2.TranslateKey("options.sensitivity.min") : (f5 == 1.0F ? string3 + stringTranslate2.TranslateKey("options.sensitivity.max") : string3 + (int)(f5 * 200.0F) + "%")) : (f5 == 0.0F ? string3 + stringTranslate2.TranslateKey("options.off") : string3 + (int)(f5 * 100.0F) + "%");
            }
            else if (enumOptions1.GetEnumBoolean())
            {
                bool z4 = this.GetOptionOrdinalValue(enumOptions1);
                return z4 ? string3 + stringTranslate2.TranslateKey("options.on") : string3 + stringTranslate2.TranslateKey("options.off");
            }
            else
            {
                return enumOptions1 == Option.RENDER_DISTANCE ? string3 + stringTranslate2.TranslateKey(RENDER_DISTANCES[this.renderDistance]) : (enumOptions1 == Option.DIFFICULTY ? string3 + stringTranslate2.TranslateKey(DIFFICULTIES[this.difficulty]) : (enumOptions1 == Option.GUI_SCALE ? string3 + stringTranslate2.TranslateKey(GUISCALES[this.guiScale]) : (enumOptions1 == Option.FRAMERATE_LIMIT ? string3 + Locale.TranslateKey(LIMIT_FRAMERATES[this.limitFramerate]) : (enumOptions1 == Option.GRAPHICS ? (this.fancyGraphics ? string3 + stringTranslate2.TranslateKey("options.graphics.fancy") : string3 + stringTranslate2.TranslateKey("options.graphics.fast")) : string3))));
            }
        }

        public void LoadOptions()
        {
            try
            {
                if (!optionsFile.Exists())
                    return;
                StreamReader bufferedReader1 = new StreamReader(optionsFile.GetReadStream());

                string string2 = "";

                while((string2 = bufferedReader1.ReadLine()) != null) {
                    try {
                        string[] string3 = string2.Split(':');
                        if(string3[0].Equals("music")) {
                            this.musicVolume = ParseFloat(string3[1]);
                        }

                        if(string3[0].Equals("sound")) {
                            this.soundVolume = ParseFloat(string3[1]);
                        }

                        if(string3[0].Equals("mouseSensitivity")) {
                            this.mouseSensitivity = ParseFloat(string3[1]);
                        }

                        if(string3[0].Equals("invertYMouse")) {
                            this.invertMouse = string3[1].Equals("true");
                        }

                        if(string3[0].Equals("viewDistance")) {
                            this.renderDistance = int.Parse(string3[1]);
                        }

                        if(string3[0].Equals("guiScale")) {
                            this.guiScale = int.Parse(string3[1]);
                        }

                        if(string3[0].Equals("bobView")) {
                            this.viewBobbing = string3[1].Equals("true");
                        }

                        if(string3[0].Equals("anaglyph3d")) {
                            this.anaglyph = string3[1].Equals("true");
                        }

                        if(string3[0].Equals("advancedOpengl")) {
                            this.advancedOpengl = string3[1].Equals("true");
                        }

                        if(string3[0].Equals("fpsLimit")) {
                            this.limitFramerate = int.Parse(string3[1]);
                        }

                        if(string3[0].Equals("difficulty")) {
                            this.difficulty = int.Parse(string3[1]);
                        }

                        if(string3[0].Equals("fancyGraphics")) {
                            this.fancyGraphics = string3[1].Equals("true");
                        }

                        if(string3[0].Equals("ao")) {
                            this.ambientOcclusion = string3[1].Equals("true");
                        }

                        if(string3[0].Equals("skin")) {
                            this.skin = string3[1];
                        }

                        if(string3[0].Equals("lastServer") && string3.Length >= 2) {
                            this.lastServer = string3[1];
                        }

                        for(int i4 = 0; i4 < this.keyBindings.Length; ++i4) {
                            if(string3[0].Equals("key_" + this.keyBindings[i4].keyDescription)) {
                                this.keyBindings[i4].keyCode = (VirtualKey)int.Parse(string3[1]);
                            }
                        }
                    } catch (Exception) {
                        Console.WriteLine("Skipping bad option: " + string2);
                    }
                }
                
                bufferedReader1.Dispose();
            }
            catch (Exception exception6)
            {
                Console.WriteLine("Failed to load options");
                exception6.PrintStackTrace();
            }

        }

        private static float ParseFloat(string string1)
        {
            //funny logic to convert old boolean options for sound to float
            return string1.Equals("true") ? 1.0F : (string1.Equals("false") ? 0.0F : float.Parse(string1));
        }

        private static string ToLowerBool(bool value) 
        {
            return value.ToString().ToLower();
        }

        public void SaveOptions()
        {
            try
            {
                StreamWriter printWriter1 = new StreamWriter(optionsFile.GetWriteStream());
                //the bool options need to be converted ToLower()
                printWriter1.WriteLine("music:" + this.musicVolume);
                printWriter1.WriteLine("sound:" + this.soundVolume);
                printWriter1.WriteLine("invertYMouse:" + ToLowerBool(invertMouse));
                printWriter1.WriteLine("mouseSensitivity:" + this.mouseSensitivity);
                printWriter1.WriteLine("viewDistance:" + this.renderDistance);
                printWriter1.WriteLine("guiScale:" + this.guiScale);
                printWriter1.WriteLine("bobView:" + ToLowerBool(viewBobbing));
                printWriter1.WriteLine("anaglyph3d:" + ToLowerBool(anaglyph));
                printWriter1.WriteLine("advancedOpengl:" + ToLowerBool(advancedOpengl));
                printWriter1.WriteLine("fpsLimit:" + this.limitFramerate);
                printWriter1.WriteLine("difficulty:" + this.difficulty);
                printWriter1.WriteLine("fancyGraphics:" + ToLowerBool(fancyGraphics));
                printWriter1.WriteLine("ao:" + ToLowerBool(ambientOcclusion));
                printWriter1.WriteLine("skin:" + this.skin);
                printWriter1.WriteLine("lastServer:" + this.lastServer);

                for (int i2 = 0; i2 < this.keyBindings.Length; ++i2) {
                    printWriter1.WriteLine(keyBindings[i2].ToString());
                }

                printWriter1.Dispose();
                
            }
            catch (Exception exception3)
            {
                Console.WriteLine("Failed to save options");
                exception3.PrintStackTrace();
            }

        }
    }
}
