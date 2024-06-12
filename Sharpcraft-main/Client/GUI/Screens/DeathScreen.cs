using LWCSGL.Input;
using LWCSGL.OpenGL;
using SharpCraft.Core.Stats;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI.Screens
{
    public class DeathScreen : Screen
    {
        public override void InitGui()
        {
            this.buttons.Clear();
            this.buttons.Add(new Button(1, this.width / 2 - 100, this.height / 4 + 72, "Respawn"));
            this.buttons.Add(new Button(2, this.width / 2 - 100, this.height / 4 + 96, "Title menu"));
            if (this.mc.user == null)
            {
                this.buttons[1].enabled = false;
            }
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.id == 0)
            {
            }

            if (guiButton1.id == 1)
            {
                this.mc.player.RespawnPlayer();
                this.mc.SetScreen((Screen)null);
            }

            if (guiButton1.id == 2)
            {
                mc.statFileWriter.ReadStat(StatList.leaveGame, 1);
                if (mc.IsMultiplayerWorld())
                {
                    mc.level.SendQuittingDisconnectingPacket();
                }

                this.mc.SetLevel((Level)null);
                this.mc.SetScreen(new StartMenuScreen());
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawGradientRect(0, 0, this.width, this.height, 1615855616, -1602211792);
            GL11.glPushMatrix();
            GL11.glScalef(2F, 2F, 2F);
            this.DrawCenteredString(this.font, "Game over!", this.width / 2 / 2, 30, 0xFFFFFF);
            GL11.glPopMatrix();
            this.DrawCenteredString(this.font, "Score: \u00A7e" + this.mc.player.GetScore(), this.width / 2, 100, 0xFFFFFF);
            base.DrawScreen(i1, i2, f3);
        }

        public override bool DoesGuiPauseGame()
        {
            return false;
        }
    }
}
