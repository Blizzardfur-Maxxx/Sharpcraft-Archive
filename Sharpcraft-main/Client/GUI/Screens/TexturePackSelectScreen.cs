using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer;
using SharpCraft.Client.Texturepack;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens
{
    public class TexturePackSelectScreen : Screen
    {
        class TexturePackList : AbstractSelectionList
        {
            private readonly TexturePackSelectScreen SELF;
            public TexturePackList(TexturePackSelectScreen texturePackSelectScreen) : base(texturePackSelectScreen.mc, texturePackSelectScreen.width, texturePackSelectScreen.height, 32, texturePackSelectScreen.height - 55 + 4, 36)
            {
                this.SELF = texturePackSelectScreen;
            }

            protected override int GetSize()
            {
                IList<AbstractTexturePack> list1 = this.SELF.mc.texturePackRepo.AvailableTexturePacks();
                return list1.Count;
            }

            protected override void ElementClicked(int i1, bool z2)
            {
                IList<AbstractTexturePack> list3 = this.SELF.mc.texturePackRepo.AvailableTexturePacks();
                this.SELF.mc.texturePackRepo.SetTexturePack(list3[i1]);
                this.SELF.mc.textures.ReloadAll();
            }

            protected override bool IsSelected(int i1)
            {
                IList<AbstractTexturePack> list2 = this.SELF.mc.texturePackRepo.AvailableTexturePacks();
                return this.SELF.mc.texturePackRepo.selectedTexturePack == list2[i1];
            }

            protected override int GetContentHeight()
            {
                return this.GetSize() * 36;
            }

            protected override void DrawBackground()
            {
                this.SELF.DrawDefaultBackground();
            }

            protected override void DrawSlot(int i1, int i2, int i3, int i4, Tessellator tessellator5)
            {
                AbstractTexturePack texturePackBase6 = this.SELF.mc.texturePackRepo.AvailableTexturePacks()[i1];
                texturePackBase6.BindThumbnailTexture(this.SELF.mc);
                GL11.glColor4f(1F, 1F, 1F, 1F);
                tessellator5.Begin();
                tessellator5.Color(0xFFFFFF);
                tessellator5.VertexUV(i2, i3 + i4, 0, 0, 1);
                tessellator5.VertexUV(i2 + 32, i3 + i4, 0, 1, 1);
                tessellator5.VertexUV(i2 + 32, i3, 0, 1, 0);
                tessellator5.VertexUV(i2, i3, 0, 0, 0);
                tessellator5.End();
                this.SELF.DrawString(this.SELF.font, texturePackBase6.texturePackFileName, i2 + 32 + 2, i3 + 1, 0xFFFFFF);
                this.SELF.DrawString(this.SELF.font, texturePackBase6.firstDescriptionLine, i2 + 32 + 2, i3 + 12, 8421504);
                this.SELF.DrawString(this.SELF.font, texturePackBase6.secondDescriptionLine, i2 + 32 + 2, i3 + 12 + 10, 8421504);
            }
        }

        protected Screen guiScreen;
        private int field_6454_o = -1;
        private string fileLocation = "";
        private TexturePackList guiTexturePackSlot;
        public TexturePackSelectScreen(Screen guiScreen1)
        {
            this.guiScreen = guiScreen1;
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            buttons.Add(new SmallButton(5, width / 2 - 154, height - 48, stringTranslate1.TranslateKey("texturePack.openFolder")));
            buttons.Add(new SmallButton(6, width / 2 + 4, height - 48, stringTranslate1.TranslateKey("gui.done")));
            mc.texturePackRepo.UpdateAvaliableTexturePacks();
            fileLocation = (new JFile(Client.GetWorkingDirectory(), "texturepacks")).GetAbsolutePath();
            guiTexturePackSlot = new TexturePackList(this);
            guiTexturePackSlot.RegisterScrollButtons(buttons, 7, 8);
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.enabled)
            {
                if (guiButton1.id == 5)
                {
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = fileLocation,
                        UseShellExecute = true,
                        Verb = "open"
                    });
                }
                else if (guiButton1.id == 6)
                {
                    mc.textures.ReloadAll();
                    mc.SetScreen(guiScreen);
                }
                else
                {
                    guiTexturePackSlot.ActionPerformed(guiButton1);
                }
            }
        }

        protected override void MouseClicked(int i1, int i2, int i3)
        {
            base.MouseClicked(i1, i2, i3);
        }

        protected override void MouseMovedOrUp(int i1, int i2, int i3)
        {
            base.MouseMovedOrUp(i1, i2, i3);
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            guiTexturePackSlot.DrawScreen(i1, i2, f3);
            if (field_6454_o <= 0)
            {
                this.mc.texturePackRepo.UpdateAvaliableTexturePacks();
                field_6454_o += 20;
            }

            I18N stringTranslate4 = I18N.Instance;
            DrawCenteredString(font, stringTranslate4.TranslateKey("texturePack.title"), this.width / 2, 16, 0xFFFFFF);
            DrawCenteredString(this.font, stringTranslate4.TranslateKey("texturePack.folderInfo"), this.width / 2 - 77, this.height - 26, 8421504);
            base.DrawScreen(i1, i2, f3);
        }

        public override void UpdateScreen()
        {
            base.UpdateScreen();
            --field_6454_o;
        }
    }
}
