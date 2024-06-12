using LWCSGL.Input;
using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace SharpCraft.Client.GUI.Screens
{
    public class Screen : GuiComponent
    {
        protected Client mc;
        public int width;
        public int height;
        protected IList<Button> buttons = new List<Button>();
        public bool field_948_f = false;
        protected Font font;
        private Button selectedButton = null;
        public virtual void DrawScreen(int i1, int i2, float f3)
        {
            for (int i4 = 0; i4 < buttons.Count; ++i4)
            {
                Button guiButton5 = buttons[i4];
                guiButton5.DrawButton(mc, i1, i2);
            }
        }

        protected virtual void KeyTyped(char c, VirtualKey vk)
        {
            if (vk == VirtualKey.Escape)
            {
                mc.SetScreen(null);
                mc.SetIngameFocus();
            }
        }

        public static string GetClipboardString()
        {
            string clipboard = null;
            // For some reason, even if the current thread is a STA one
            // Clipboard APIs still return an empty string
            // So thats why this workaround exists
            // - vlOd
            Thread thread = new Thread(() =>
            {
                try
                {
                    clipboard = Clipboard.GetText();
                }
                catch (Exception ex)
                {
                    ex.PrintStackTrace();
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return clipboard;
        }

        protected virtual void MouseClicked(int x, int y, int btn)
        {
            if (btn == 0)
            {
                for (int i = 0; i < buttons.Count; ++i)
                {
                    Button button = buttons[i];

                    if (button.MousePressed(mc, x, y))
                    {
                        selectedButton = button;
                        mc.soundEngine.PlaySoundFX("random.click", 1F, 1F);
                        ActionPerformed(button);
                    }
                }
            }
        }

        protected virtual void MouseMovedOrUp(int x, int y, int btn)
        {
            if (selectedButton != null && btn == 0)
            {
                selectedButton.MouseReleased(x, y);
                selectedButton = null;
            }
        }

        protected virtual void ActionPerformed(Button guiButton1)
        {
        }

        public virtual void Init(Client instance, int w, int h)
        {
            mc = instance;
            font = instance.font;
            width = w;
            height = h;
            buttons.Clear();
            InitGui();
        }

        public virtual void InitGui()
        {
        }

        public virtual void HandleInput()
        {
            while (Mouse.Next())
            {
                HandleMouseInput();
            }

            while (Keyboard.Next())
            {
                HandleKeyboardInput();
            }
        }

        public virtual void HandleMouseInput()
        {
            int x;
            int y;
            x = (int)(Mouse.GetX() * width / mc.displayWidth);
            y = height - (int)Mouse.GetY() * height / mc.displayHeight ;

            if (Mouse.GetEventButtonState())
            {
                MouseClicked(x, y, Mouse.GetEventButton());
            }
            else
            {
                MouseMovedOrUp(x, y, Mouse.GetEventButton());
            }
        }

        public virtual void HandleKeyboardInput()
        {
            if (Keyboard.GetEventKeyState())
            {
                if (Keyboard.GetEventKey() == VirtualKey.F11)
                {
                    mc.ToggleFullscreen();
                    return;
                }

                KeyTyped(Keyboard.GetEventCharacter(), Keyboard.GetEventKey());
            }
        }

        public virtual void UpdateScreen()
        {
        }

        public virtual void OnGuiClosed()
        {
        }

        public virtual void DrawDefaultBackground()
        {
            DrawWorldBackground(0);
        }

        public virtual void DrawWorldBackground(int offset)
        {
            if (mc.level != null)
            {
                DrawGradientRect(0, 0, width, height, -0x3fefeff0, -0x2fefeff0);
            }
            else
            {
                DrawBackground(offset);
            }
        }

        public virtual void DrawBackground(int offset)
        {
            GL11.glDisable(GL11C.GL_LIGHTING);
            GL11.glDisable(GL11C.GL_FOG);
            Tessellator tessellator = Tessellator.Instance;
            GL11.glBindTexture(GL11C.GL_TEXTURE_2D, mc.textures.LoadTexture("/gui/background.png"));
            GL11.glColor4f(1F, 1F, 1F, 1F);
            float scale = 32F;

            tessellator.Begin();
            tessellator.Color(4210752);
            tessellator.VertexUV(0, height, 0, 0, height / scale + offset);
            tessellator.VertexUV(width, height, 0, width / scale, height / scale + offset);
            tessellator.VertexUV(width, 0, 0, width / scale, 0 + offset);
            tessellator.VertexUV(0, 0, 0, 0, 0 + offset);
            tessellator.End();
        }

        public virtual bool DoesGuiPauseGame()
        {
            return true;
        }

        public virtual void DeleteWorld(bool z1, int i2)
        {
        }

        public virtual void SelectNextField()
        {
        }
    }
}
