using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Client.GUI;

namespace SharpCraft.Client.Renderer.Tileentities
{
    public class SignRenderer : TileEntityRenderer<TileEntitySign>
    {
        private SignModel signModel = new SignModel();

        public override void RenderTileEntityAt(TileEntitySign tileEntity1, double d2, double d4, double d6, float f8)
        {
            Tile block9 = tileEntity1.GetBlockType();
            GL11.glPushMatrix();
            float f10 = 0.6666667F;
            float f12;
            if (block9 == Tile.signPost)
            {
                GL11.glTranslatef((float)d2 + 0.5F, (float)d4 + 0.75F * f10, (float)d6 + 0.5F);
                float f11 = tileEntity1.GetBlockMetadata() * 360 / 16.0F;
                GL11.glRotatef(-f11, 0.0F, 1.0F, 0.0F);
                this.signModel.signStick.showModel = true;
            }
            else
            {
                int i16 = tileEntity1.GetBlockMetadata();
                f12 = 0.0F;
                if (i16 == 2)
                {
                    f12 = 180.0F;
                }

                if (i16 == 4)
                {
                    f12 = 90.0F;
                }

                if (i16 == 5)
                {
                    f12 = -90.0F;
                }

                GL11.glTranslatef((float)d2 + 0.5F, (float)d4 + 0.75F * f10, (float)d6 + 0.5F);
                GL11.glRotatef(-f12, 0.0F, 1.0F, 0.0F);
                GL11.glTranslatef(0.0F, -0.3125F, -0.4375F);
                this.signModel.signStick.showModel = false;
            }

            this.BindTextureByName("/item/sign.png");
            GL11.glPushMatrix();
            GL11.glScalef(f10, -f10, -f10);
            this.signModel.func_887_a();
            GL11.glPopMatrix();
            Font fontRenderer17 = this.GetFontRenderer();
            f12 = 0.016666668F * f10;
            GL11.glTranslatef(0.0F, 0.5F * f10, 0.07F * f10);
            GL11.glScalef(f12, -f12, f12);
            GL11.glNormal3f(0.0F, 0.0F, -1.0F * f12);
            GL11.glDepthMask(false);
            byte b13 = 0;

            for (int i14 = 0; i14 < tileEntity1.signText.Length; ++i14)
            {
                string string15 = tileEntity1.signText[i14];
                if (i14 == tileEntity1.lineBeingEdited)
                {
                    string15 = "> " + string15 + " <";
                    fontRenderer17.DrawString(string15, -fontRenderer17.GetStringWidth(string15) / 2, i14 * 10 - tileEntity1.signText.Length * 5, b13);
                }
                else
                {
                    fontRenderer17.DrawString(string15, -fontRenderer17.GetStringWidth(string15) / 2, i14 * 10 - tileEntity1.signText.Length * 5, b13);
                }
            }

            GL11.glDepthMask(true);
            GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
            GL11.glPopMatrix();
        }
    }
}
