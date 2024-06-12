using LWCSGL.Input;
using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer.Tileentities;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Core.Network.Packets;

namespace SharpCraft.Client.GUI.Screens.Inventories
{
    public class GuiEditSign : Screen
    {
        protected string screenTitle = "Edit sign message:";
        private TileEntitySign entitySign;
        private int updateCounter;
        private int editLine = 0;

        public GuiEditSign(TileEntitySign tileEntitySign1)
        {
            this.entitySign = tileEntitySign1;
        }

        public override void InitGui()
        {
            this.buttons.Clear();
            Keyboard.EnableRepeatEvents(true);
            this.buttons.Add(new Button(0, this.width / 2 - 100, this.height / 4 + 120, "Done"));
        }

        public override void OnGuiClosed()
        {
            Keyboard.EnableRepeatEvents(false);
            if (this.mc.level.isRemote)
            {
                this.mc.GetSendQueue().AddToSendQueue(new Packet130UpdateSign(this.entitySign.xCoord, this.entitySign.yCoord, this.entitySign.zCoord, this.entitySign.signText));
            }
        }

        public override void UpdateScreen()
        {
            ++this.updateCounter;
        }

        protected override void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.enabled)
            {
                if (guiButton1.id == 0)
                {
                    this.entitySign.SetChanged();
                    this.mc.SetScreen((Screen)null);
                }
            }
        }

        protected override void KeyTyped(char c1, VirtualKey i2)
        {
            if (i2 == VirtualKey.Up)
            {
                this.editLine = this.editLine - 1 & 3;
            }

            if (i2 == VirtualKey.Down || i2 == VirtualKey.Enter)
            {
                this.editLine = this.editLine + 1 & 3;
            }

            if (i2 == VirtualKey.Back && this.entitySign.signText[this.editLine].Length > 0)
            {
                this.entitySign.signText[this.editLine] = this.entitySign.signText[this.editLine].Substring(0, this.entitySign.signText[this.editLine].Length - 1);
            }

            if (SharedConstants.VALID_TEXT_CHARACTERS.IndexOf(c1) >= 0 && this.entitySign.signText[this.editLine].Length < 15)
            {
                this.entitySign.signText[this.editLine] = this.entitySign.signText[this.editLine] + c1;
            }
        }

        public override void DrawScreen(int i1, int i2, float f3)
        {
            this.DrawDefaultBackground();
            this.DrawCenteredString(this.font, this.screenTitle, this.width / 2, 40, 0xFFFFFF);
            GL11.glPushMatrix();
            GL11.glTranslatef(this.width / 2, 0F, 50F);
            float f4 = 93.75F;
            GL11.glScalef(-f4, -f4, -f4);
            GL11.glRotatef(180F, 0F, 1F, 0F);
            Tile block5 = this.entitySign.GetBlockType();
            if (block5 == Tile.signPost)
            {
                float f6 = this.entitySign.GetBlockMetadata() * 360 / 16F;
                GL11.glRotatef(f6, 0F, 1F, 0F);
                GL11.glTranslatef(0F, -1.0625F, 0F);
            }
            else
            {
                int i8 = this.entitySign.GetBlockMetadata();
                float f7 = 0F;
                if (i8 == 2)
                {
                    f7 = 180F;
                }

                if (i8 == 4)
                {
                    f7 = 90F;
                }

                if (i8 == 5)
                {
                    f7 = -90F;
                }

                GL11.glRotatef(f7, 0F, 1F, 0F);
                GL11.glTranslatef(0F, -1.0625F, 0F);
            }

            if (this.updateCounter / 6 % 2 == 0)
            {
                this.entitySign.lineBeingEdited = this.editLine;
            }

            TileEntityRenderDispatcher.instance.RenderTileEntityAt(this.entitySign, -0.5, -0.75, -0.5, 0F);
            this.entitySign.lineBeingEdited = -1;
            GL11.glPopMatrix();
            base.DrawScreen(i1, i2, f3);
        }
    }
}
