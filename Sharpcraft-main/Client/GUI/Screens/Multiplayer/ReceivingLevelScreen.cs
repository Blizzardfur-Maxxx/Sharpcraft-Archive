using LWCSGL.Input;
using SharpCraft.Client.Network;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens.Multiplayer
{
    public class ReceivingLevelScreen : Screen
    {
        private ClientConnection netHandler;
        private int updateCounter = 0;

        public ReceivingLevelScreen(ClientConnection netClientHandler1)
        {
            this.netHandler = netClientHandler1;
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
        }

        public override void InitGui()
        {
            this.buttons.Clear();
        }

        public override void UpdateScreen()
        {
            ++this.updateCounter;
            if (this.updateCounter % 20 == 0)
            {
                this.netHandler.AddToSendQueue(new Packet0KeepAlive());
            }

            if (this.netHandler != null)
            {
                this.netHandler.Tick();
            }
        }

        protected override void ActionPerformed(Button guiButton1)
        {
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawBackground(0);
            I18N stringTranslate4 = I18N.Instance;
            this.DrawCenteredString(this.font, stringTranslate4.TranslateKey("multiplayer.downloadingTerrain"), this.width / 2, this.height / 2 - 50, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }
    }
}
