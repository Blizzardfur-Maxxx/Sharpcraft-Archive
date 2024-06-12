using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LWCSGL.Input;
using SharpCraft.Client.GUI.Screens;
using SharpCraft.Core;

namespace SharpCraft.Client.GUI.Screens.Multiplayer
{
    public class ChatScreen : Screen
    {
        protected string message = "";
        private int updateCounter;

        public override void InitGui()
        {
            Keyboard.EnableRepeatEvents(true);
        }

        public override void OnGuiClosed()
        {
            Keyboard.EnableRepeatEvents(false);
        }

        public override void UpdateScreen()
        {
            ++updateCounter;
        }

        protected override void KeyTyped(char keyChar, VirtualKey keyCode)
        {
            if (keyCode == VirtualKey.Escape)
            {
                mc.SetScreen(null);
            }
            else if (keyCode == VirtualKey.Enter)
            {
                string finalMsg = message.Trim();

                if (finalMsg.Length > 0)
                {
                    // Nutch unused cooc
                    if (!mc.LineIsCommand(finalMsg))
                        mc.player.SendChatMessage(finalMsg);
                }

                mc.SetScreen(null);
            }
            else
            {
                if (keyCode == VirtualKey.Back && message.Length > 0)
                    message = message.Substring(0, message.Length - 1);

                if (SharedConstants.VALID_TEXT_CHARACTERS.IndexOf(keyChar) >= 0 && message.Length < 100)
                    message += keyChar;
            }
        }

        public override void DrawScreen(int mx, int my, float deltaTime)
        {
            DrawRect(2, height - 14, width - 2, height - 2, int.MinValue);
            DrawString(font, "> " + message + (updateCounter / 6 % 2 == 0 ? "_" : ""), 4, height - 12, 14737632);
            base.DrawScreen(mx, my, deltaTime);
        }

        protected override void MouseClicked(int mx, int my, int button)
        {
            if (button == 0)
            {
                if (mc.ingameGUI.ununsedTabPlayer != null)
                {
                    if (message.Length > 0 && !message.EndsWith(" "))
                        message += " ";

                    message += mc.ingameGUI.ununsedTabPlayer;
                    byte cap = 100;

                    if (message.Length > cap)
                        message = message.Substring(0, cap);
                }
                else
                {
                    base.MouseClicked(mx, my, button);
                }
            }
        }
    }
}
