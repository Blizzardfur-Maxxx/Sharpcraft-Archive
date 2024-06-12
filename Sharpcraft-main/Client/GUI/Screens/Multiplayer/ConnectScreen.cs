using LWCSGL.Input;
using SharpCraft.Client.Network;
using SharpCraft.Core.i18n;
using SharpCraft.Core.Network.Packets;
using System;
using System.Net.Sockets;
using System.Threading;

namespace SharpCraft.Client.GUI.Screens.Multiplayer
{
    internal class ConnectScreen : Screen
    {
        private ClientConnection connection;
        private bool cancelled = false;

        public ConnectScreen(Client craft, string ip, int port)
        {
            Console.WriteLine($"Connecting to {ip}, {port}");
            craft.SetLevel(null);

            new Thread(new ThreadStart(() => 
            {
                try
                {
                    connection = new ClientConnection(craft, ip, port);
                    if (cancelled) return;
                    connection.AddToSendQueue(new Packet2Handshake(craft.user.name));
                }
                catch (SocketException ex)
                {
                    if (cancelled) return;
                    bool noSuchHost = ex.ErrorCode == 11001; /* WSAHOST_NOT_FOUND */
                    string msg = noSuchHost ? $"Unknown host \'{ip}\'" : ex.Message;
                    craft.SetScreen(new DisconnectionScreen("connect.failed", "disconnect.genericReason", msg));
                }
                catch (Exception ex) 
                {
                    if (cancelled) return;
                    ex.PrintStackTrace();
                    craft.SetScreen(new DisconnectionScreen("connect.failed", "disconnect.genericReason", "" + ex));
                }
            })).Start();
        }

        public override void UpdateScreen()
        {
            if (this.connection != null)
                this.connection.Tick();
        }

        protected override void KeyTyped(char c, VirtualKey vk)
        {
        }

        public override void InitGui()
        {
            I18N stringTranslate1 = I18N.Instance;
            this.buttons.Clear();
            this.buttons.Add(new Button(0, this.width / 2 - 100, this.height / 4 + 120 + 12, stringTranslate1.TranslateKey("gui.cancel")));
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.id == 0)
            {
                this.cancelled = true;
                if (this.connection != null)
                    this.connection.Disconnect();

                this.mc.SetScreen(new StartMenuScreen());
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            I18N stringTranslate4 = I18N.Instance;
            if (this.connection == null)
            {
                this.DrawCenteredString(this.font, stringTranslate4.TranslateKey("connect.connecting"), this.width / 2, this.height / 2 - 50, 0xFFFFFF);
                this.DrawCenteredString(this.font, "", this.width / 2, this.height / 2 - 10, 0xFFFFFF);
            }
            else
            {
                this.DrawCenteredString(this.font, stringTranslate4.TranslateKey("connect.authorizing"), this.width / 2, this.height / 2 - 50, 0xFFFFFF);
                this.DrawCenteredString(this.font, this.connection.LoginStatus, this.width / 2, this.height / 2 - 10, 0xFFFFFF);
            }

            base.DrawScreen(i1, i2, f3);
        }
    }
}