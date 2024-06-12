using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Models
{
    public class ModelRenderer
    {
        private Vertex[] corners;
        private Polygon[] faces;
        private int textureOffsetX;
        private int textureOffsetY;
        public float rotationPointX;
        public float rotationPointY;
        public float rotationPointZ;
        public float rotateAngleX;
        public float rotateAngleY;
        public float rotateAngleZ;
        private bool compiled = false;
        private uint displayList = 0;
        public bool mirror = false;
        public bool showModel = true;
        public bool field_1402_i = false;

        public ModelRenderer(int i1, int i2)
        {
            this.textureOffsetX = i1;
            this.textureOffsetY = i2;
        }

        public virtual void AddBox(float f1, float f2, float f3, int i4, int i5, int i6)
        {
            this.AddBox(f1, f2, f3, i4, i5, i6, 0F);
        }

        public virtual void AddBox(float f1, float f2, float f3, int i4, int i5, int i6, float f7)
        {
            this.corners = new Vertex[8];
            this.faces = new Polygon[6];
            float f8 = f1 + i4;
            float f9 = f2 + i5;
            float f10 = f3 + i6;
            f1 -= f7;
            f2 -= f7;
            f3 -= f7;
            f8 += f7;
            f9 += f7;
            f10 += f7;
            if (this.mirror)
            {
                float f11 = f8;
                f8 = f1;
                f1 = f11;
            }

            Vertex positionTextureVertex20 = new Vertex(f1, f2, f3, 0F, 0F);
            Vertex positionTextureVertex12 = new Vertex(f8, f2, f3, 0F, 8F);
            Vertex positionTextureVertex13 = new Vertex(f8, f9, f3, 8F, 8F);
            Vertex positionTextureVertex14 = new Vertex(f1, f9, f3, 8F, 0F);
            Vertex positionTextureVertex15 = new Vertex(f1, f2, f10, 0F, 0F);
            Vertex positionTextureVertex16 = new Vertex(f8, f2, f10, 0F, 8F);
            Vertex positionTextureVertex17 = new Vertex(f8, f9, f10, 8F, 8F);
            Vertex positionTextureVertex18 = new Vertex(f1, f9, f10, 8F, 0F);
            this.corners[0] = positionTextureVertex20;
            this.corners[1] = positionTextureVertex12;
            this.corners[2] = positionTextureVertex13;
            this.corners[3] = positionTextureVertex14;
            this.corners[4] = positionTextureVertex15;
            this.corners[5] = positionTextureVertex16;
            this.corners[6] = positionTextureVertex17;
            this.corners[7] = positionTextureVertex18;
            this.faces[0] = new Polygon(new Vertex[] { positionTextureVertex16, positionTextureVertex12, positionTextureVertex13, positionTextureVertex17 }, this.textureOffsetX + i6 + i4, this.textureOffsetY + i6, this.textureOffsetX + i6 + i4 + i6, this.textureOffsetY + i6 + i5);
            this.faces[1] = new Polygon(new Vertex[] { positionTextureVertex20, positionTextureVertex15, positionTextureVertex18, positionTextureVertex14 }, this.textureOffsetX + 0, this.textureOffsetY + i6, this.textureOffsetX + i6, this.textureOffsetY + i6 + i5);
            this.faces[2] = new Polygon(new Vertex[] { positionTextureVertex16, positionTextureVertex15, positionTextureVertex20, positionTextureVertex12 }, this.textureOffsetX + i6, this.textureOffsetY + 0, this.textureOffsetX + i6 + i4, this.textureOffsetY + i6);
            this.faces[3] = new Polygon(new Vertex[] { positionTextureVertex13, positionTextureVertex14, positionTextureVertex18, positionTextureVertex17 }, this.textureOffsetX + i6 + i4, this.textureOffsetY + 0, this.textureOffsetX + i6 + i4 + i4, this.textureOffsetY + i6);
            this.faces[4] = new Polygon(new Vertex[] { positionTextureVertex12, positionTextureVertex20, positionTextureVertex14, positionTextureVertex13 }, this.textureOffsetX + i6, this.textureOffsetY + i6, this.textureOffsetX + i6 + i4, this.textureOffsetY + i6 + i5);
            this.faces[5] = new Polygon(new Vertex[] { positionTextureVertex15, positionTextureVertex16, positionTextureVertex17, positionTextureVertex18 }, this.textureOffsetX + i6 + i4 + i6, this.textureOffsetY + i6, this.textureOffsetX + i6 + i4 + i6 + i4, this.textureOffsetY + i6 + i5);
            if (this.mirror)
            {
                for (int i19 = 0; i19 < this.faces.Length; ++i19)
                {
                    this.faces[i19].FlipFace();
                }
            }
        }

        public virtual void SetRotationPoint(float f1, float f2, float f3)
        {
            this.rotationPointX = f1;
            this.rotationPointY = f2;
            this.rotationPointZ = f3;
        }

        public virtual void Render(float f1)
        {
            if (!this.field_1402_i)
            {
                if (this.showModel)
                {
                    if (!this.compiled)
                    {
                        this.CompileDisplayList(f1);
                    }

                    if (this.rotateAngleX == 0F && this.rotateAngleY == 0F && this.rotateAngleZ == 0F)
                    {
                        if (this.rotationPointX == 0F && this.rotationPointY == 0F && this.rotationPointZ == 0F)
                        {
                            GL11.glCallList(this.displayList);
                        }
                        else
                        {
                            GL11.glTranslatef(this.rotationPointX * f1, this.rotationPointY * f1, this.rotationPointZ * f1);
                            GL11.glCallList(this.displayList);
                            GL11.glTranslatef(-this.rotationPointX * f1, -this.rotationPointY * f1, -this.rotationPointZ * f1);
                        }
                    }
                    else
                    {
                        GL11.glPushMatrix();
                        GL11.glTranslatef(this.rotationPointX * f1, this.rotationPointY * f1, this.rotationPointZ * f1);
                        if (this.rotateAngleZ != 0F)
                        {
                            GL11.glRotatef(this.rotateAngleZ * 57.295776F, 0F, 0F, 1F);
                        }

                        if (this.rotateAngleY != 0F)
                        {
                            GL11.glRotatef(this.rotateAngleY * 57.295776F, 0F, 1F, 0F);
                        }

                        if (this.rotateAngleX != 0F)
                        {
                            GL11.glRotatef(this.rotateAngleX * 57.295776F, 1F, 0F, 0F);
                        }

                        GL11.glCallList(this.displayList);
                        GL11.glPopMatrix();
                    }
                }
            }
        }

        public virtual void RenderWithRotation(float f1)
        {
            if (!this.field_1402_i)
            {
                if (this.showModel)
                {
                    if (!this.compiled)
                    {
                        this.CompileDisplayList(f1);
                    }

                    GL11.glPushMatrix();
                    GL11.glTranslatef(this.rotationPointX * f1, this.rotationPointY * f1, this.rotationPointZ * f1);
                    if (this.rotateAngleY != 0F)
                    {
                        GL11.glRotatef(this.rotateAngleY * 57.295776F, 0F, 1F, 0F);
                    }

                    if (this.rotateAngleX != 0F)
                    {
                        GL11.glRotatef(this.rotateAngleX * 57.295776F, 1F, 0F, 0F);
                    }

                    if (this.rotateAngleZ != 0F)
                    {
                        GL11.glRotatef(this.rotateAngleZ * 57.295776F, 0F, 0F, 1F);
                    }

                    GL11.glCallList(this.displayList);
                    GL11.glPopMatrix();
                }
            }
        }

        public virtual void PostRender(float f1)
        {
            if (!this.field_1402_i)
            {
                if (this.showModel)
                {
                    if (!this.compiled)
                    {
                        this.CompileDisplayList(f1);
                    }

                    if (this.rotateAngleX == 0F && this.rotateAngleY == 0F && this.rotateAngleZ == 0F)
                    {
                        if (this.rotationPointX != 0F || this.rotationPointY != 0F || this.rotationPointZ != 0F)
                        {
                            GL11.glTranslatef(this.rotationPointX * f1, this.rotationPointY * f1, this.rotationPointZ * f1);
                        }
                    }
                    else
                    {
                        GL11.glTranslatef(this.rotationPointX * f1, this.rotationPointY * f1, this.rotationPointZ * f1);
                        if (this.rotateAngleZ != 0F)
                        {
                            GL11.glRotatef(this.rotateAngleZ * 57.295776F, 0F, 0F, 1F);
                        }

                        if (this.rotateAngleY != 0F)
                        {
                            GL11.glRotatef(this.rotateAngleY * 57.295776F, 0F, 1F, 0F);
                        }

                        if (this.rotateAngleX != 0F)
                        {
                            GL11.glRotatef(this.rotateAngleX * 57.295776F, 1F, 0F, 0F);
                        }
                    }
                }
            }
        }

        private void CompileDisplayList(float f1)
        {
            displayList = MemoryTracker.GenLists(1);
            GL11.glNewList(displayList, GL11C.GL_COMPILE);
            Tessellator tessellator2 = Tessellator.Instance;

            for (int i3 = 0; i3 < faces.Length; ++i3)
            {
                faces[i3].Render(tessellator2, f1);
            }

            GL11.glEndList();
            this.compiled = true;
        }
    }
}
