using LWCSGL.OpenGL;
using SharpCraft.Client.GUI;
using SharpCraft.Client.Models;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderLiving<T> : Render<T> where T : Mob
    {
        protected Model mainModel;
        protected Model renderPassModel;

        public RenderLiving(Model modelBase1, float f2)
        {
            this.mainModel = modelBase1;
            this.shadowSize = f2;
        }

        public void SetRenderPassModel(Model modelBase1)
        {
            this.renderPassModel = modelBase1;
        }

        public virtual void DoRenderLiving(T entityLiving1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glPushMatrix();
            GL11.glDisable(GL11C.GL_CULL_FACE);
            this.mainModel.OnGround = this.Func_167_c(entityLiving1, f9);
            if (this.renderPassModel != null)
            {
                this.renderPassModel.OnGround = this.mainModel.OnGround;
            }

            this.mainModel.IsRiding = entityLiving1.IsRiding();
            if (this.renderPassModel != null)
            {
                this.renderPassModel.IsRiding = this.mainModel.IsRiding;
            }

            //try
            //{
                float f10 = entityLiving1.prevRenderYawOffset + (entityLiving1.renderYawOffset - entityLiving1.prevRenderYawOffset) * f9;
                float f11 = entityLiving1.prevYaw + (entityLiving1.yaw - entityLiving1.prevYaw) * f9;
                float f12 = entityLiving1.prevPitch + (entityLiving1.pitch - entityLiving1.prevPitch) * f9;
                this.Func_22012_b(entityLiving1, d2, d4, d6);
                float f13 = this.Func_170_d(entityLiving1, f9);
                this.RotateCorpse(entityLiving1, f13, f10, f9);
                float f14 = 0.0625F;
                GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
                GL11.glScalef(-1.0F, -1.0F, 1.0F);
                this.PreRenderCallback(entityLiving1, f9);
                GL11.glTranslatef(0.0F, -24.0F * f14 - 0.0078125F, 0.0F);
                float f15 = entityLiving1.field_Q + (entityLiving1.field_bd - entityLiving1.field_Q) * f9;
                float f16 = entityLiving1.field_ba - entityLiving1.field_bd * (1.0F - f9);
                if (f15 > 1.0F)
                {
                    f15 = 1.0F;
                }

                this.LoadDownloadableImageTexture(entityLiving1.skinUrl, entityLiving1.GetEntityTexture());
                GL11.glEnable(GL11C.GL_ALPHA_TEST);
                this.mainModel.SetLivingAnimations(entityLiving1, f16, f15, f9);
                this.mainModel.Render(f16, f15, f13, f11 - f10, f12, f14);

                for (int i17 = 0; i17 < 4; ++i17)
                {
                    if (this.ShouldRenderPass(entityLiving1, i17, f9))
                    {
                        this.renderPassModel.Render(f16, f15, f13, f11 - f10, f12, f14);
                        GL11.glDisable(GL11C.GL_BLEND);
                        GL11.glEnable(GL11C.GL_ALPHA_TEST);
                    }
                }

                this.RenderEquippedItems(entityLiving1, f9);
                float f25 = entityLiving1.GetEntityBrightness(f9);
                int i18 = this.GetColorMultiplier(entityLiving1, f25, f9);
                if ((i18 >> 24 & 255) > 0 || entityLiving1.hurtTime > 0 || entityLiving1.deathTime > 0)
                {
                    GL11.glDisable(GL11C.GL_TEXTURE_2D);
                    GL11.glDisable(GL11C.GL_ALPHA_TEST);
                    GL11.glEnable(GL11C.GL_BLEND);
                    GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                    GL11.glDepthFunc(GL11C.GL_EQUAL);
                    if (entityLiving1.hurtTime > 0 || entityLiving1.deathTime > 0)
                    {
                        GL11.glColor4f(f25, 0.0F, 0.0F, 0.4F);
                        this.mainModel.Render(f16, f15, f13, f11 - f10, f12, f14);

                        for (int i19 = 0; i19 < 4; ++i19)
                        {
                            if (this.Func_27005_b(entityLiving1, i19, f9))
                            {
                                GL11.glColor4f(f25, 0.0F, 0.0F, 0.4F);
                                this.renderPassModel.Render(f16, f15, f13, f11 - f10, f12, f14);
                            }
                        }
                    }

                    if ((i18 >> 24 & 255) > 0)
                    {
                        float f26 = (i18 >> 16 & 255) / 255.0F;
                        float f20 = (i18 >> 8 & 255) / 255.0F;
                        float f21 = (i18 & 255) / 255.0F;
                        float f22 = (i18 >> 24 & 255) / 255.0F;
                        GL11.glColor4f(f26, f20, f21, f22);
                        this.mainModel.Render(f16, f15, f13, f11 - f10, f12, f14);

                        for (int i23 = 0; i23 < 4; ++i23)
                        {
                            if (this.Func_27005_b(entityLiving1, i23, f9))
                            {
                                GL11.glColor4f(f26, f20, f21, f22);
                                this.renderPassModel.Render(f16, f15, f13, f11 - f10, f12, f14);
                            }
                        }
                    }

                    GL11.glDepthFunc(GL11C.GL_LEQUAL);
                    GL11.glDisable(GL11C.GL_BLEND);
                    GL11.glEnable(GL11C.GL_ALPHA_TEST);
                    GL11.glEnable(GL11C.GL_TEXTURE_2D);
                }

                GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            //}
            //catch (Exception exception24)
            //{
            //    exception24.PrintStackTrace();
            //}
            //i have yet to see this catch anything so i just left it disabled by commenting it out -pnp

            GL11.glEnable(GL11C.GL_CULL_FACE);
            GL11.glPopMatrix();
            this.PassSpecialRender(entityLiving1, d2, d4, d6);
        }

        protected virtual void Func_22012_b(T entityLiving1, double d2, double d4, double d6)
        {
            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
        }

        protected virtual void RotateCorpse(T entityLiving1, float f2, float f3, float f4)
        {
            GL11.glRotatef(180.0F - f3, 0.0F, 1.0F, 0.0F);
            if (entityLiving1.deathTime > 0)
            {
                float f5 = (entityLiving1.deathTime + f4 - 1.0F) / 20.0F * 1.6F;
                f5 = Mth.Sqrt(f5);
                if (f5 > 1.0F)
                {
                    f5 = 1.0F;
                }

                GL11.glRotatef(f5 * this.GetDeathMaxRotation(entityLiving1), 0.0F, 0.0F, 1.0F);
            }

        }

        protected virtual float Func_167_c(T entityLiving1, float f2)
        {
            return entityLiving1.GetSwingProgress(f2);
        }

        protected virtual float Func_170_d(T entityLiving1, float f2)
        {
            return entityLiving1.ticksExisted + f2;
        }

        protected virtual void RenderEquippedItems(T entityLiving1, float f2)
        {
        }

        protected virtual bool Func_27005_b(T entityLiving1, int i2, float f3)
        {
            return this.ShouldRenderPass(entityLiving1, i2, f3);
        }

        protected virtual bool ShouldRenderPass(T entityLiving1, int i2, float f3)
        {
            return false;
        }

        protected virtual float GetDeathMaxRotation(T entityLiving1)
        {
            return 90.0F;
        }

        protected virtual int GetColorMultiplier(T entityLiving1, float f2, float f3)
        {
            return 0;
        }

        protected virtual void PreRenderCallback(T entityLiving1, float f2)
        {
        }

        protected virtual void PassSpecialRender(T entityLiving1, double d2, double d4, double d6)
        {
            if (Client.IsDebugInfoEnabled())
            {
                this.RenderLivingLabel(entityLiving1, entityLiving1.entityID.ToString(), d2, d4, d6, 64);
            }

        }

        protected virtual void RenderLivingLabel(T entityLiving1, string string2, double d3, double d5, double d7, int i9)
        {
            float f10 = entityLiving1.GetDistanceToEntity(this.renderManager.livingPlayer);
            if (f10 <= i9)
            {
                Font fontRenderer11 = this.GetFont();
                float f12 = 1.6F;
                float f13 = 0.016666668F * f12;
                GL11.glPushMatrix();
                GL11.glTranslatef((float)d3 + 0.0F, (float)d5 + 2.3F, (float)d7);
                GL11.glNormal3f(0.0F, 1.0F, 0.0F);
                GL11.glRotatef(-this.renderManager.playerViewY, 0.0F, 1.0F, 0.0F);
                GL11.glRotatef(this.renderManager.playerViewX, 1.0F, 0.0F, 0.0F);
                GL11.glScalef(-f13, -f13, f13);
                GL11.glDisable(GL11C.GL_LIGHTING);
                GL11.glDepthMask(false);
                GL11.glDisable(GL11C.GL_DEPTH_TEST);
                GL11.glEnable(GL11C.GL_BLEND);
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                Tessellator tessellator14 = Tessellator.Instance;
                sbyte b15 = 0;
                if (string2.Equals("deadmau5"))
                {
                    b15 = -10;
                }

                GL11.glDisable(GL11C.GL_TEXTURE_2D);
                tessellator14.Begin();
                int i16 = fontRenderer11.GetStringWidth(string2) / 2;
                tessellator14.Color(0.0F, 0.0F, 0.0F, 0.25F);
                tessellator14.Vertex(-i16 - 1, -1 + b15, 0.0D);
                tessellator14.Vertex(-i16 - 1, 8 + b15, 0.0D);
                tessellator14.Vertex(i16 + 1, 8 + b15, 0.0D);
                tessellator14.Vertex(i16 + 1, -1 + b15, 0.0D);
                tessellator14.End();
                GL11.glEnable(GL11C.GL_TEXTURE_2D);
                fontRenderer11.DrawString(string2, -fontRenderer11.GetStringWidth(string2) / 2, b15, 553648127);
                GL11.glEnable(GL11C.GL_DEPTH_TEST);
                GL11.glDepthMask(true);
                fontRenderer11.DrawString(string2, -fontRenderer11.GetStringWidth(string2) / 2, b15, 0xffffff); //-1
                GL11.glEnable(GL11C.GL_LIGHTING);
                GL11.glDisable(GL11C.GL_BLEND);
                GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
                GL11.glPopMatrix();
            }
        }

        public override void DoRender(T entity1, double d2, double d4, double d6, float f8, float f9)
        {
            this.DoRenderLiving(entity1, d2, d4, d6, f8, f9);
        }
    }
}
