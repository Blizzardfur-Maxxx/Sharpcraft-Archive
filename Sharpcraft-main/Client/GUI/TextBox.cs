using LWCSGL.Input;
using SharpCraft.Client.GUI.Screens;
using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI
{
    public class TextBox : GuiComponent
    {
        private readonly Font fontRenderer;
        private readonly int xPos;
        private readonly int yPos;
        private readonly int width;
        private readonly int height;
        private string text;
        private int maxStringLength;
        private int cursorCounter;
        public bool isFocused = false;
        public bool isEnabled = true;
        private Screen parentGuiScreen;
        public TextBox(Screen guiScreen1, Font fontRenderer2, int i3, int i4, int i5, int i6, string string7)
        {
            this.parentGuiScreen = guiScreen1;
            this.fontRenderer = fontRenderer2;
            this.xPos = i3;
            this.yPos = i4;
            this.width = i5;
            this.height = i6;
            this.SetText(string7);
        }

        public virtual void SetText(string string1)
        {
            this.text = string1;
        }

        public virtual string GetText()
        {
            return this.text;
        }

        public virtual void UpdateCursorCounter()
        {
            ++this.cursorCounter;
        }

        public virtual void TextboxKeyTyped(char c1, VirtualKey i2)
        {
            if (this.isEnabled && this.isFocused)
            {
                if (c1 == '\t')
                {
                    this.parentGuiScreen.SelectNextField();
                }

                if (c1 == (char)0x16)
                {
                    string string3 = Screen.GetClipboardString();
                    if (string3 == null)
                    {
                        string3 = "";
                    }

                    int i4 = 32 - this.text.Length;
                    if (i4 > string3.Length)
                    {
                        i4 = string3.Length;
                    }

                    if (i4 > 0)
                    {
                        this.text = this.text + string3.Substring(0, i4);
                    }
                }

                if (i2 == VirtualKey.Back && this.text.Length > 0)
                {
                    this.text = this.text.Substring(0, this.text.Length - 1);
                }

                if (SharedConstants.VALID_TEXT_CHARACTERS.IndexOf(c1) >= 0 && (this.text.Length < this.maxStringLength || this.maxStringLength == 0))
                {
                    this.text = this.text + c1;
                }
            }
        }

        public virtual void MouseClicked(int i1, int i2, int i3)
        {
            bool z4 = this.isEnabled && i1 >= this.xPos && i1 < this.xPos + this.width && i2 >= this.yPos && i2 < this.yPos + this.height;
            this.SetFocused(z4);
        }

        public virtual void SetFocused(bool z1)
        {
            if (z1 && !this.isFocused)
            {
                this.cursorCounter = 0;
            }

            this.isFocused = z1;
        }

        public virtual void DrawTextBox()
        {
            this.DrawRect(this.xPos - 1, this.yPos - 1, this.xPos + this.width + 1, this.yPos + this.height + 1, -6250336);
            this.DrawRect(this.xPos, this.yPos, this.xPos + this.width, this.yPos + this.height, unchecked((int)0xFF000000));
            if (this.isEnabled)
            {
                bool z1 = this.isFocused && this.cursorCounter / 6 % 2 == 0;
                this.DrawString(this.fontRenderer, this.text + (z1 ? "_" : ""), this.xPos + 4, this.yPos + (this.height - 8) / 2, 14737632);
            }
            else
            {
                this.DrawString(this.fontRenderer, this.text, this.xPos + 4, this.yPos + (this.height - 8) / 2, 7368816);
            }
        }

        public virtual void SetMaxStringLength(int i1)
        {
            this.maxStringLength = i1;
        }
    }
}
