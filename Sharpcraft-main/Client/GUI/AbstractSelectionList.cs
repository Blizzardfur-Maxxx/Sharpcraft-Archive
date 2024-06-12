using LWCSGL.Input;
using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.GUI
{
    public abstract class AbstractSelectionList
    {
        private readonly Client mc;
        private readonly int width;
        private readonly int height;
        protected readonly int top;
        protected readonly int bottom;
        private readonly int right;
        private readonly int left;
        protected readonly int posZ;
        private int scrollUpButtonID;
        private int scrollDownButtonID;
        private float initialClickY = -2F;
        private float scrollMultiplier;
        private float amountScrolled;
        private int selectedElement = -1;
        private long lastClicked = 0;
        private bool field_25123_p = true;
        private bool field_27262_q;
        private int field_27261_r;
        public AbstractSelectionList(Client instance, int i2, int i3, int i4, int i5, int i6)
        {
            mc = instance;
            width = i2;
            height = i3;
            top = i4;
            bottom = i5;
            posZ = i6;
            left = 0;
            right = i2;
        }

        public virtual void Func_27258_a(bool z1)
        {
            field_25123_p = z1;
        }

        protected virtual void Func_27259_a(bool z1, int i2)
        {
            field_27262_q = z1;
            field_27261_r = i2;
            if (!z1)
            {
                field_27261_r = 0;
            }
        }

        protected abstract int GetSize();
        protected abstract void ElementClicked(int i1, bool z2);
        protected abstract bool IsSelected(int i1);
        protected virtual int GetContentHeight()
        {
            return GetSize() * posZ + field_27261_r;
        }

        protected abstract void DrawBackground();
        protected abstract void DrawSlot(int i1, int i2, int i3, int i4, Tessellator tessellator5);
        protected virtual void Func_27260_a(int i1, int i2, Tessellator tessellator3)
        {
        }

        protected virtual void Func_27255_a(int i1, int i2)
        {
        }

        protected virtual void Func_27257_b(int i1, int i2)
        {
        }

        public virtual int Func_27256_c(int i1, int i2)
        {
            int i3 = width / 2 - 110;
            int i4 = width / 2 + 110;
            int i5 = i2 - top - field_27261_r + (int)amountScrolled - 4;
            int i6 = i5 / posZ;
            return i1 >= i3 && i1 <= i4 && i6 >= 0 && i5 >= 0 && i6 < GetSize() ? i6 : -1;
        }

        public virtual void RegisterScrollButtons(IList<Button> list1, int i2, int i3)
        {
            scrollUpButtonID = i2;
            scrollDownButtonID = i3;
        }

        private void BindAmountScrolled()
        {
            int i1 = GetContentHeight() - (bottom - top - 4);
            if (i1 < 0)
            {
                i1 /= 2;
            }

            if (amountScrolled < 0F)
            {
                amountScrolled = 0F;
            }

            if (amountScrolled > i1)
            {
                amountScrolled = i1;
            }
        }

        public virtual void ActionPerformed(Button guiButton1)
        {
            if (guiButton1.enabled)
            {
                if (guiButton1.id == scrollUpButtonID)
                {
                    amountScrolled -= posZ * 2 / 3;
                    initialClickY = -2F;
                    BindAmountScrolled();
                }
                else if (guiButton1.id == scrollDownButtonID)
                {
                    amountScrolled += posZ * 2 / 3;
                    initialClickY = -2F;
                    BindAmountScrolled();
                }
            }
        }

        public virtual void DrawScreen(int i1, int i2, float f3)
        {
            DrawBackground();
            int i4 = GetSize();
            int i5 = width / 2 + 124;
            int i6 = i5 + 6;
            int i9;
            int i10;
            int i11;
            int i13;
            int i19;
            if (Mouse.IsButtonDown(0))
            {
                if (initialClickY == -1F)
                {
                    bool z7 = true;
                    if (i2 >= top && i2 <= bottom)
                    {
                        int i8 = width / 2 - 110;
                        i9 = width / 2 + 110;
                        i10 = i2 - top - field_27261_r + (int)amountScrolled - 4;
                        i11 = i10 / posZ;
                        if (i1 >= i8 && i1 <= i9 && i11 >= 0 && i10 >= 0 && i11 < i4)
                        {
                            bool z12 = i11 == selectedElement && TimeUtil.MilliTime - lastClicked < 250;
                            ElementClicked(i11, z12);
                            selectedElement = i11;
                            lastClicked = TimeUtil.MilliTime;
                        }
                        else if (i1 >= i8 && i1 <= i9 && i10 < 0)
                        {
                            Func_27255_a(i1 - i8, i2 - top + (int)amountScrolled - 4);
                            z7 = false;
                        }

                        if (i1 >= i5 && i1 <= i6)
                        {
                            scrollMultiplier = -1F;
                            i19 = GetContentHeight() - (bottom - top - 4);
                            if (i19 < 1)
                            {
                                i19 = 1;
                            }

                            i13 = (int)((bottom - top) * (bottom - top) / (float)GetContentHeight());
                            if (i13 < 32)
                            {
                                i13 = 32;
                            }

                            if (i13 > bottom - top - 8)
                            {
                                i13 = bottom - top - 8;
                            }

                            scrollMultiplier /= (bottom - top - i13) / (float)i19;
                        }
                        else
                        {
                            scrollMultiplier = 1F;
                        }

                        if (z7)
                        {
                            initialClickY = i2;
                        }
                        else
                        {
                            initialClickY = -2F;
                        }
                    }
                    else
                    {
                        initialClickY = -2F;
                    }
                }
                else if (initialClickY >= 0F)
                {
                    amountScrolled -= (i2 - initialClickY) * scrollMultiplier;
                    initialClickY = i2;
                }
            }
            else
            {
                initialClickY = -1F;
            }

            BindAmountScrolled();
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glDisable(GL11C.GL_FOG);
            Tessellator tessellator16 = Tessellator.Instance;
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, mc.textures.LoadTexture("/gui/background.png"));
            GL11.glColor4f(1F, 1F, 1F, 1F);
            float f17 = 32F;
            tessellator16.Begin();
            tessellator16.Color(2105376);
            tessellator16.VertexUV(left, bottom, 0, left / f17, (bottom + (int)amountScrolled) / f17);
            tessellator16.VertexUV(right, bottom, 0, right / f17, (bottom + (int)amountScrolled) / f17);
            tessellator16.VertexUV(right, top, 0, right / f17, (top + (int)amountScrolled) / f17);
            tessellator16.VertexUV(left, top, 0, left / f17, (top + (int)amountScrolled) / f17);
            tessellator16.End();
            i9 = width / 2 - 92 - 16;
            i10 = top + 4 - (int)amountScrolled;
            if (field_27262_q)
            {
                Func_27260_a(i9, i10, tessellator16);
            }

            int i14;
            for (i11 = 0; i11 < i4; ++i11)
            {
                i19 = i10 + i11 * posZ + field_27261_r;
                i13 = posZ - 4;
                if (i19 <= bottom && i19 + i13 >= top)
                {
                    if (field_25123_p && IsSelected(i11))
                    {
                        i14 = width / 2 - 110;
                        int i15 = width / 2 + 110;
                        GL11.glColor4f(1F, 1F, 1F, 1F);
                        GL11.glDisable(GL11C.GL_TEXTURE_2D);
                        tessellator16.Begin();
                        tessellator16.Color(8421504);
                        tessellator16.VertexUV(i14, i19 + i13 + 2, 0, 0, 1);
                        tessellator16.VertexUV(i15, i19 + i13 + 2, 0, 1, 1);
                        tessellator16.VertexUV(i15, i19 - 2, 0, 1, 0);
                        tessellator16.VertexUV(i14, i19 - 2, 0, 0, 0);
                        tessellator16.Color(0);
                        tessellator16.VertexUV(i14 + 1, i19 + i13 + 1, 0, 0, 1);
                        tessellator16.VertexUV(i15 - 1, i19 + i13 + 1, 0, 1, 1);
                        tessellator16.VertexUV(i15 - 1, i19 - 1, 0, 1, 0);
                        tessellator16.VertexUV(i14 + 1, i19 - 1, 0, 0, 0);
                        tessellator16.End();
                        GL11.glEnable(GL11C.GL_TEXTURE_2D);
                    }

                    DrawSlot(i11, i9, i19, i13, tessellator16);
                }
            }

            GL11.glDisable(GL11C.GL_DEPTH_TEST);
            byte b18 = 4;
            OverlayBackground(0, top, 255, 255);
            OverlayBackground(bottom, height, 255, 255);
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            GL11.glDisable(GL11C.GL_ALPHA_TEST);
            GL11.glShadeModel(GL11C.GL_SMOOTH);
            GL11.glDisable(GL11C.GL_TEXTURE_2D);
            tessellator16.Begin();
            tessellator16.Color(0, 0);
            tessellator16.VertexUV(left, top + b18, 0, 0, 1);
            tessellator16.VertexUV(right, top + b18, 0, 1, 1);
            tessellator16.Color(0, 255);
            tessellator16.VertexUV(right, top, 0, 1, 0);
            tessellator16.VertexUV(left, top, 0, 0, 0);
            tessellator16.End();
            tessellator16.Begin();
            tessellator16.Color(0, 255);
            tessellator16.VertexUV(left, bottom, 0, 0, 1);
            tessellator16.VertexUV(right, bottom, 0, 1, 1);
            tessellator16.Color(0, 0);
            tessellator16.VertexUV(right, bottom - b18, 0, 1, 0);
            tessellator16.VertexUV(left, bottom - b18, 0, 0, 0);
            tessellator16.End();
            i19 = GetContentHeight() - (bottom - top - 4);
            if (i19 > 0)
            {
                i13 = (bottom - top) * (bottom - top) / GetContentHeight();
                if (i13 < 32)
                {
                    i13 = 32;
                }

                if (i13 > bottom - top - 8)
                {
                    i13 = bottom - top - 8;
                }

                i14 = (int)amountScrolled * (bottom - top - i13) / i19 + top;
                if (i14 < top)
                {
                    i14 = top;
                }

                tessellator16.Begin();
                tessellator16.Color(0, 255);
                tessellator16.VertexUV(i5, bottom, 0, 0, 1);
                tessellator16.VertexUV(i6, bottom, 0, 1, 1);
                tessellator16.VertexUV(i6, top, 0, 1, 0);
                tessellator16.VertexUV(i5, top, 0, 0, 0);
                tessellator16.End();
                tessellator16.Begin();
                tessellator16.Color(8421504, 255);
                tessellator16.VertexUV(i5, i14 + i13, 0, 0, 1);
                tessellator16.VertexUV(i6, i14 + i13, 0, 1, 1);
                tessellator16.VertexUV(i6, i14, 0, 1, 0);
                tessellator16.VertexUV(i5, i14, 0, 0, 0);
                tessellator16.End();
                tessellator16.Begin();
                tessellator16.Color(12632256, 255);
                tessellator16.VertexUV(i5, i14 + i13 - 1, 0, 0, 1);
                tessellator16.VertexUV(i6 - 1, i14 + i13 - 1, 0, 1, 1);
                tessellator16.VertexUV(i6 - 1, i14, 0, 1, 0);
                tessellator16.VertexUV(i5, i14, 0, 0, 0);
                tessellator16.End();
            }

            Func_27257_b(i1, i2);
            GL11.glEnable(GL11C.GL_TEXTURE_2D);
            GL11.glShadeModel(GL11C.GL_FLAT);
            GL11.glEnable(GL11C.GL_ALPHA_TEST);
            GL11.glDisable(GL11C.GL_BLEND);
        }

        private void OverlayBackground(int i1, int i2, int i3, int i4)
        {
            Tessellator tessellator5 = Tessellator.Instance;
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, mc.textures.LoadTexture("/gui/background.png"));
            GL11.glColor4f(1F, 1F, 1F, 1F);
            float f6 = 32F;
            tessellator5.Begin();
            tessellator5.Color(4210752, i4);
            tessellator5.VertexUV(0, i2, 0, 0, i2 / f6);
            tessellator5.VertexUV(width, i2, 0, width / f6, i2 / f6);
            tessellator5.Color(4210752, i3);
            tessellator5.VertexUV(width, i1, 0, width / f6, i1 / f6);
            tessellator5.VertexUV(0, i1, 0, 0, i1 / f6);
            tessellator5.End();
        }
    }
}
