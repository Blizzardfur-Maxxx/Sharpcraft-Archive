using LWCSGL.OpenGL;
using SharpCraft.Client.GUI;
using SharpCraft.Client.Models;
using SharpCraft.Client.Players;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderPlayer : RenderLiving<Player>
    {
        private ModelBiped modelBipedMain;
        private ModelBiped modelArmorChestplate = new ModelBiped(1F);
        private ModelBiped modelArmor = new ModelBiped(0.5F);
        private static readonly string[] armorFilenamePrefix = new[]
        {
            "cloth",
            "chain",
            "iron",
            "diamond",
            "gold"
        };

        public RenderPlayer() : base(new ModelBiped(0F), 0.5F)
        {
            modelBipedMain = (ModelBiped)this.mainModel;
        }

        public virtual void RenderPlayer_(Player entityPlayer1, double d2, double d4, double d6, float f8, float f9)
        {
            ItemInstance itemStack10 = entityPlayer1.inventory.GetCurrentItem();
            this.modelArmorChestplate.field_1278_i = this.modelArmor.field_1278_i = this.modelBipedMain.field_1278_i = itemStack10 != null;
            this.modelArmorChestplate.isSneak = this.modelArmor.isSneak = this.modelBipedMain.isSneak = entityPlayer1.IsSneaking();
            double d11 = d4 - entityPlayer1.yOffset;
            if (entityPlayer1.IsSneaking() && !(entityPlayer1 is LocalPlayer))
            {
                d11 -= 0.125;
            }

            base.DoRenderLiving(entityPlayer1, d2, d11, d6, f8, f9);
            this.modelArmorChestplate.isSneak = this.modelArmor.isSneak = this.modelBipedMain.isSneak = false;
            this.modelArmorChestplate.field_1278_i = this.modelArmor.field_1278_i = this.modelBipedMain.field_1278_i = false;
        }

        public virtual void DrawFirstPersonHand()
        {
            this.modelBipedMain.OnGround = 0F;
            this.modelBipedMain.SetRotationAngles(0F, 0F, 0F, 0F, 0F, 0.0625F);
            this.modelBipedMain.bipedRightArm.Render(0.0625F);
        }

        protected override void PassSpecialRender(Player entityLiving1, double d2, double d4, double d6)
        {
            if (Client.IsGuiEnabled() && entityLiving1 != this.renderManager.livingPlayer)
            {
                float f8 = 1.6F;
                float f9 = 0.016666668F * f8;
                float f10 = entityLiving1.GetDistanceToEntity(this.renderManager.livingPlayer);
                float f11 = entityLiving1.IsSneaking() ? 32F : 64F;
                if (f10 < f11)
                {
                    string string12 = entityLiving1.username;
                    if (!entityLiving1.IsSneaking())
                    {
                        if (entityLiving1.IsSleeping())
                        {
                            this.RenderLivingLabel(entityLiving1, string12, d2, d4 - 1.5, d6, 64);
                        }
                        else
                        {
                            this.RenderLivingLabel(entityLiving1, string12, d2, d4, d6, 64);
                        }
                    }
                    else
                    {
                        Font fontRenderer13 = this.GetFont();
                        GL11.glPushMatrix();
                        GL11.glTranslatef((float)d2 + 0F, (float)d4 + 2.3F, (float)d6);
                        GL11.glNormal3f(0F, 1F, 0F);
                        GL11.glRotatef(-this.renderManager.playerViewY, 0F, 1F, 0F);
                        GL11.glRotatef(this.renderManager.playerViewX, 1F, 0F, 0F);
                        GL11.glScalef(-f9, -f9, f9);
                        GL11.glDisable(GL11C.GL_LIGHTING);
                        GL11.glTranslatef(0F, 0.25F / f9, 0F);
                        GL11.glDepthMask(false);
                        GL11.glEnable(GL11C.GL_BLEND);
                        GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                        Tessellator tessellator14 = Tessellator.Instance;
                        GL11.glDisable(GL11C.GL_TEXTURE_2D);
                        tessellator14.Begin();
                        int i15 = fontRenderer13.GetStringWidth(string12) / 2;
                        tessellator14.Color(0F, 0F, 0F, 0.25F);
                        tessellator14.Vertex(-i15 - 1, -1, 0);
                        tessellator14.Vertex(-i15 - 1, 8, 0);
                        tessellator14.Vertex(i15 + 1, 8, 0);
                        tessellator14.Vertex(i15 + 1, -1, 0);
                        tessellator14.End();
                        GL11.glEnable(GL11C.GL_TEXTURE_2D);
                        GL11.glDepthMask(true);
                        fontRenderer13.DrawString(string12, -fontRenderer13.GetStringWidth(string12) / 2, 0, 553648127);
                        GL11.glEnable(GL11C.GL_LIGHTING);
                        GL11.glDisable(GL11C.GL_BLEND);
                        GL11.glColor4f(1F, 1F, 1F, 1F);
                        GL11.glPopMatrix();
                    }
                }
            }
        }

        protected override void PreRenderCallback(Player entityLiving1, float f2)
        {
            float f3 = 0.9375F;
            GL11.glScalef(f3, f3, f3);
        }

        protected override bool ShouldRenderPass(Player entityLiving1, int i2, float f3)
        {
            ItemInstance itemStack4 = entityLiving1.inventory.ArmorItemInSlot(3 - i2);
            if (itemStack4 != null)
            {
                Item item5 = itemStack4.GetItem();
                if (item5 is ItemArmor)
                {
                    ItemArmor itemArmor6 = (ItemArmor)item5;
                    this.LoadTexture("/armor/" + armorFilenamePrefix[itemArmor6.renderIndex] + "_" + (i2 == 2 ? 2 : 1) + ".png");
                    ModelBiped modelBiped7 = i2 == 2 ? this.modelArmor : this.modelArmorChestplate;
                    modelBiped7.bipedHead.showModel = i2 == 0;
                    modelBiped7.bipedHeadwear.showModel = i2 == 0;
                    modelBiped7.bipedBody.showModel = i2 == 1 || i2 == 2;
                    modelBiped7.bipedRightArm.showModel = i2 == 1;
                    modelBiped7.bipedLeftArm.showModel = i2 == 1;
                    modelBiped7.bipedRightLeg.showModel = i2 == 2 || i2 == 3;
                    modelBiped7.bipedLeftLeg.showModel = i2 == 2 || i2 == 3;
                    this.SetRenderPassModel(modelBiped7);
                    return true;
                }
            }

            return false;
        }

        protected override void RenderEquippedItems(Player entityLiving1, float f2)
        {
            ItemInstance itemStack3 = entityLiving1.inventory.ArmorItemInSlot(3);
            if (itemStack3 != null && itemStack3.GetItem().id < 256)
            {
                GL11.glPushMatrix();
                this.modelBipedMain.bipedHead.PostRender(0.0625F);
                if (TileRenderer.RenderItemIn3d(Tile.tiles[itemStack3.itemID].GetRenderShape()))
                {
                    float f4 = 0.625F;
                    GL11.glTranslatef(0F, -0.25F, 0F);
                    GL11.glRotatef(180F, 0F, 1F, 0F);
                    GL11.glScalef(f4, -f4, f4);
                }

                this.renderManager.itemRenderer.RenderItem(entityLiving1, itemStack3);
                GL11.glPopMatrix();
            }

            float f5;
            if (entityLiving1.username.Equals("deadmau5") && this.LoadDownloadableImageTexture(entityLiving1.skinUrl, (string)null))
            {
                for (int i19 = 0; i19 < 2; ++i19)
                {
                    f5 = entityLiving1.prevYaw + (entityLiving1.yaw - entityLiving1.prevYaw) * f2 - (entityLiving1.prevRenderYawOffset + (entityLiving1.renderYawOffset - entityLiving1.prevRenderYawOffset) * f2);
                    float f6 = entityLiving1.prevPitch + (entityLiving1.pitch - entityLiving1.prevPitch) * f2;
                    GL11.glPushMatrix();
                    GL11.glRotatef(f5, 0F, 1F, 0F);
                    GL11.glRotatef(f6, 1F, 0F, 0F);
                    GL11.glTranslatef(0.375F * (i19 * 2 - 1), 0F, 0F);
                    GL11.glTranslatef(0F, -0.375F, 0F);
                    GL11.glRotatef(-f6, 1F, 0F, 0F);
                    GL11.glRotatef(-f5, 0F, 1F, 0F);
                    float f7 = 1.3333334F;
                    GL11.glScalef(f7, f7, f7);
                    this.modelBipedMain.RenderEars(0.0625F);
                    GL11.glPopMatrix();
                }
            }

            if (this.LoadDownloadableImageTexture(entityLiving1.playerCloakUrl, (string)null))
            {
                GL11.glPushMatrix();
                GL11.glTranslatef(0F, 0F, 0.125F);
                double d20 = entityLiving1.field_773 + (entityLiving1.field_770 - entityLiving1.field_773) * f2 - (entityLiving1.prevX + (entityLiving1.x - entityLiving1.prevX) * f2);
                double d22 = entityLiving1.field_772 + (entityLiving1.field_769 - entityLiving1.field_772) * f2 - (entityLiving1.prevY + (entityLiving1.y - entityLiving1.prevY) * f2);
                double d8 = entityLiving1.field_771 + (entityLiving1.field_768 - entityLiving1.field_771) * f2 - (entityLiving1.prevZ + (entityLiving1.z - entityLiving1.prevZ) * f2);
                float f10 = entityLiving1.prevRenderYawOffset + (entityLiving1.renderYawOffset - entityLiving1.prevRenderYawOffset) * f2;
                double d11 = Mth.Sin(f10 * Mth.PI / 180F);
                double d13 = (-Mth.Cos(f10 * Mth.PI / 180F));
                float f15 = (float)d22 * 10F;
                if (f15 < -6F)
                {
                    f15 = -6F;
                }

                if (f15 > 32F)
                {
                    f15 = 32F;
                }

                float f16 = (float)(d20 * d11 + d8 * d13) * 100F;
                float f17 = (float)(d20 * d13 - d8 * d11) * 100F;
                if (f16 < 0F)
                {
                    f16 = 0F;
                }

                float f18 = entityLiving1.field_775 + (entityLiving1.field_774 - entityLiving1.field_775) * f2;
                f15 += Mth.Sin((entityLiving1.prevDistanceWalkedModified + (entityLiving1.distanceWalkedModified - entityLiving1.prevDistanceWalkedModified) * f2) * 6F) * 32F * f18;
                if (entityLiving1.IsSneaking())
                {
                    f15 += 25F;
                }

                GL11.glRotatef(6F + f16 / 2F + f15, 1F, 0F, 0F);
                GL11.glRotatef(f17 / 2F, 0F, 0F, 1F);
                GL11.glRotatef(-f17 / 2F, 0F, 1F, 0F);
                GL11.glRotatef(180F, 0F, 1F, 0F);
                this.modelBipedMain.RenderCloak(0.0625F);
                GL11.glPopMatrix();
            }

            ItemInstance itemStack21 = entityLiving1.inventory.GetCurrentItem();
            if (itemStack21 != null)
            {
                GL11.glPushMatrix();
                this.modelBipedMain.bipedRightArm.PostRender(0.0625F);
                GL11.glTranslatef(-0.0625F, 0.4375F, 0.0625F);
                if (entityLiving1.fishEntity != null)
                {
                    itemStack21 = new ItemInstance(Item.stick);
                }

                if (itemStack21.itemID < 256 && TileRenderer.RenderItemIn3d(Tile.tiles[itemStack21.itemID].GetRenderShape()))
                {
                    f5 = 0.5F;
                    GL11.glTranslatef(0F, 0.1875F, -0.3125F);
                    f5 *= 0.75F;
                    GL11.glRotatef(20F, 1F, 0F, 0F);
                    GL11.glRotatef(45F, 0F, 1F, 0F);
                    GL11.glScalef(f5, -f5, f5);
                }
                else if (Item.items[itemStack21.itemID].IsFull3D())
                {
                    f5 = 0.625F;
                    if (Item.items[itemStack21.itemID].ShouldRotateAroundWhenRendering())
                    {
                        GL11.glRotatef(180F, 0F, 0F, 1F);
                        GL11.glTranslatef(0F, -0.125F, 0F);
                    }

                    GL11.glTranslatef(0F, 0.1875F, 0F);
                    GL11.glScalef(f5, -f5, f5);
                    GL11.glRotatef(-100F, 1F, 0F, 0F);
                    GL11.glRotatef(45F, 0F, 1F, 0F);
                }
                else
                {
                    f5 = 0.375F;
                    GL11.glTranslatef(0.25F, 0.1875F, -0.1875F);
                    GL11.glScalef(f5, f5, f5);
                    GL11.glRotatef(60F, 0F, 0F, 1F);
                    GL11.glRotatef(-90F, 1F, 0F, 0F);
                    GL11.glRotatef(20F, 0F, 0F, 1F);
                }

                this.renderManager.itemRenderer.RenderItem(entityLiving1, itemStack21);
                GL11.glPopMatrix();
            }
        }

        protected override void RotateCorpse(Player a, float f2, float f3, float f4)
        {
            if (a.IsEntityAlive() && a.IsSleeping())
            {
                GL11.glRotatef(a.GetBedOrientationInDegrees(), 0F, 1F, 0F);
                GL11.glRotatef(this.GetDeathMaxRotation(a), 0F, 0F, 1F);
                GL11.glRotatef(270F, 0F, 1F, 0F);
            }
            else
            {
                base.RotateCorpse(a, f2, f3, f4);
            }
        }

        protected override void Func_22012_b(Player a, double d2, double d4, double d6)
        {
            if (a.IsEntityAlive() && a.IsSleeping())
            {
                base.Func_22012_b(a, d2 + a.field_767, d4 + a.field_766, d6 + a.field_765);
            }
            else
            {
                base.Func_22012_b(a, d2, d4, d6);
            }
        }

        public override void DoRenderLiving(Player entityLiving1, double d2, double d4, double d6, float f8, float f9)
        {
            this.RenderPlayer_(entityLiving1, d2, d4, d6, f8, f9);
        }

        public override void DoRender(Player entity1, double d2, double d4, double d6, float f8, float f9)
        {
            this.RenderPlayer_(entity1, d2, d4, d6, f8, f9);
        }
    }
}
