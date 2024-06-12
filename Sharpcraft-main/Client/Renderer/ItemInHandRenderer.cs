using LWCSGL.OpenGL;
using SharpCraft.Client.Players;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.GameSavedData.maps;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer
{
    public class ItemInHandRenderer
    {
        private Client mc;
        private ItemInstance itemToRender = null;
        private float equippedProgress = 0F;
        private float prevEquippedProgress = 0F;
        private TileRenderer renderBlocksInstance = new TileRenderer();
        private Minimap mapRenderer;
        private int field_20099_f = -1;
        public ItemInHandRenderer(Client instance)
        {
            this.mc = instance;
            this.mapRenderer = new Minimap(instance.font, instance.options, instance.textures);
        }

        public virtual void RenderItem(Mob entityLiving1, ItemInstance itemStack2)
        {
            GL11.glPushMatrix();
            if (itemStack2.itemID < 256 && TileRenderer.RenderItemIn3d(Tile.tiles[itemStack2.itemID].GetRenderShape()))
            {
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/terrain.png"));
                this.renderBlocksInstance.RenderBlockOnInventory(Tile.tiles[itemStack2.itemID], itemStack2.GetItemDamage(), entityLiving1.GetEntityBrightness(1F));
            }
            else
            {
                if (itemStack2.itemID < 256)
                {
                    GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/terrain.png"));
                }
                else
                {
                    GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadTexture("/gui/items.png"));
                }

                Tessellator tessellator3 = Tessellator.Instance;
                int i4 = entityLiving1.GetItemIcon(itemStack2);
                float f5 = (i4 % 16 * 16 + 0F) / 256F;
                float f6 = (i4 % 16 * 16 + 15.99F) / 256F;
                float f7 = (i4 / 16 * 16 + 0F) / 256F;
                float f8 = (i4 / 16 * 16 + 15.99F) / 256F;
                float f9 = 1F;
                float f10 = 0F;
                float f11 = 0.3F;
                GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
                GL11.glTranslatef(-f10, -f11, 0F);
                float f12 = 1.5F;
                GL11.glScalef(f12, f12, f12);
                GL11.glRotatef(50F, 0F, 1F, 0F);
                GL11.glRotatef(335F, 0F, 0F, 1F);
                GL11.glTranslatef(-0.9375F, -0.0625F, 0F);
                float f13 = 0.0625F;
                tessellator3.Begin();
                tessellator3.Normal(0F, 0F, 1F);
                tessellator3.VertexUV(0, 0, 0, f6, f8);
                tessellator3.VertexUV(f9, 0, 0, f5, f8);
                tessellator3.VertexUV(f9, 1, 0, f5, f7);
                tessellator3.VertexUV(0, 1, 0, f6, f7);
                tessellator3.End();
                tessellator3.Begin();
                tessellator3.Normal(0F, 0F, -1F);
                tessellator3.VertexUV(0, 1, 0F - f13, f6, f7);
                tessellator3.VertexUV(f9, 1, 0F - f13, f5, f7);
                tessellator3.VertexUV(f9, 0, 0F - f13, f5, f8);
                tessellator3.VertexUV(0, 0, 0F - f13, f6, f8);
                tessellator3.End();
                tessellator3.Begin();
                tessellator3.Normal(-1F, 0F, 0F);
                int i14;
                float f15;
                float f16;
                float f17;
                for (i14 = 0; i14 < 16; ++i14)
                {
                    f15 = i14 / 16F;
                    f16 = f6 + (f5 - f6) * f15 - 0.001953125F;
                    f17 = f9 * f15;
                    tessellator3.VertexUV(f17, 0, 0F - f13, f16, f8);
                    tessellator3.VertexUV(f17, 0, 0, f16, f8);
                    tessellator3.VertexUV(f17, 1, 0, f16, f7);
                    tessellator3.VertexUV(f17, 1, 0F - f13, f16, f7);
                }

                tessellator3.End();
                tessellator3.Begin();
                tessellator3.Normal(1F, 0F, 0F);
                for (i14 = 0; i14 < 16; ++i14)
                {
                    f15 = i14 / 16F;
                    f16 = f6 + (f5 - f6) * f15 - 0.001953125F;
                    f17 = f9 * f15 + 0.0625F;
                    tessellator3.VertexUV(f17, 1, 0F - f13, f16, f7);
                    tessellator3.VertexUV(f17, 1, 0, f16, f7);
                    tessellator3.VertexUV(f17, 0, 0, f16, f8);
                    tessellator3.VertexUV(f17, 0, 0F - f13, f16, f8);
                }

                tessellator3.End();
                tessellator3.Begin();
                tessellator3.Normal(0F, 1F, 0F);
                for (i14 = 0; i14 < 16; ++i14)
                {
                    f15 = i14 / 16F;
                    f16 = f8 + (f7 - f8) * f15 - 0.001953125F;
                    f17 = f9 * f15 + 0.0625F;
                    tessellator3.VertexUV(0, f17, 0, f6, f16);
                    tessellator3.VertexUV(f9, f17, 0, f5, f16);
                    tessellator3.VertexUV(f9, f17, 0F - f13, f5, f16);
                    tessellator3.VertexUV(0, f17, 0F - f13, f6, f16);
                }

                tessellator3.End();
                tessellator3.Begin();
                tessellator3.Normal(0F, -1F, 0F);
                for (i14 = 0; i14 < 16; ++i14)
                {
                    f15 = i14 / 16F;
                    f16 = f8 + (f7 - f8) * f15 - 0.001953125F;
                    f17 = f9 * f15;
                    tessellator3.VertexUV(f9, f17, 0, f5, f16);
                    tessellator3.VertexUV(0, f17, 0, f6, f16);
                    tessellator3.VertexUV(0, f17, 0F - f13, f6, f16);
                    tessellator3.VertexUV(f9, f17, 0F - f13, f5, f16);
                }

                tessellator3.End();
                GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            }

            GL11.glPopMatrix();
        }

        public virtual void RenderItemInFirstPerson(float f1)
        {
            float f2 = this.prevEquippedProgress + (this.equippedProgress - this.prevEquippedProgress) * f1;
            LocalPlayer entityPlayerSP3 = this.mc.player;
            float f4 = entityPlayerSP3.prevPitch + (entityPlayerSP3.pitch - entityPlayerSP3.prevPitch) * f1;
            GL11.glPushMatrix();
            GL11.glRotatef(f4, 1F, 0F, 0F);
            GL11.glRotatef(entityPlayerSP3.prevYaw + (entityPlayerSP3.yaw - entityPlayerSP3.prevYaw) * f1, 0F, 1F, 0F);
            Light.TurnOn();
            GL11.glPopMatrix();
            ItemInstance itemStack5 = this.itemToRender;
            float f6 = this.mc.level.GetBrightness(Mth.Floor(entityPlayerSP3.x), Mth.Floor(entityPlayerSP3.y), Mth.Floor(entityPlayerSP3.z));
            float f8;
            float f9;
            float f10;
            if (itemStack5 != null)
            {
                int i7 = Item.items[itemStack5.itemID].GetColorFromDamage(itemStack5.GetItemDamage());
                f8 = (i7 >> 16 & 255) / 255F;
                f9 = (i7 >> 8 & 255) / 255F;
                f10 = (i7 & 255) / 255F;
                GL11.glColor4f(f6 * f8, f6 * f9, f6 * f10, 1F);
            }
            else
            {
                GL11.glColor4f(f6, f6, f6, 1F);
            }

            float f14;
            if (itemStack5 != null && itemStack5.itemID == Item.mapItem.id)
            {
                GL11.glPushMatrix();
                f14 = 0.8F;
                f8 = entityPlayerSP3.GetSwingProgress(f1);
                f9 = Mth.Sin(f8 * Mth.PI);
                f10 = Mth.Sin(Mth.Sqrt(f8) * Mth.PI);
                GL11.glTranslatef(-f10 * 0.4F, Mth.Sin(Mth.Sqrt(f8) * Mth.PI * 2F) * 0.2F, -f9 * 0.2F);
                f8 = 1F - f4 / 45F + 0.1F;
                if (f8 < 0F)
                {
                    f8 = 0F;
                }

                if (f8 > 1F)
                {
                    f8 = 1F;
                }

                f8 = -Mth.Cos(f8 * Mth.PI) * 0.5F + 0.5F;
                GL11.glTranslatef(0F, 0F * f14 - (1F - f2) * 1.2F - f8 * 0.5F + 0.04F, -0.9F * f14);
                GL11.glRotatef(90F, 0F, 1F, 0F);
                GL11.glRotatef(f8 * -85F, 0F, 0F, 1F);
                GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadMemTexture(this.mc.player.skinUrl, this.mc.player.GetEntityTexture()));
                for (int i17 = 0; i17 < 2; ++i17)
                {
                    int i21 = i17 * 2 - 1;
                    GL11.glPushMatrix();
                    GL11.glTranslatef(-0F, -0.6F, 1.1F * i21);
                    GL11.glRotatef(-45 * i21, 1F, 0F, 0F);
                    GL11.glRotatef(-90F, 0F, 0F, 1F);
                    GL11.glRotatef(59F, 0F, 0F, 1F);
                    GL11.glRotatef(-65 * i21, 0F, 1F, 0F);
                    object render11 = EntityRenderDispatcher.instance.GetEntityRenderObjectAsObject(this.mc.player);
                    RenderPlayer renderPlayer12 = ((RenderPlayer)render11);
                    float f13 = 1F;
                    GL11.glScalef(f13, f13, f13);
                    renderPlayer12.DrawFirstPersonHand();
                    GL11.glPopMatrix();
                }

                f9 = entityPlayerSP3.GetSwingProgress(f1);
                f10 = Mth.Sin(f9 * f9 * Mth.PI);
                float f18 = Mth.Sin(Mth.Sqrt(f9) * Mth.PI);
                GL11.glRotatef(-f10 * 20F, 0F, 1F, 0F);
                GL11.glRotatef(-f18 * 20F, 0F, 0F, 1F);
                GL11.glRotatef(-f18 * 80F, 1F, 0F, 0F);
                f9 = 0.38F;
                GL11.glScalef(f9, f9, f9);
                GL11.glRotatef(90F, 0F, 1F, 0F);
                GL11.glRotatef(180F, 0F, 0F, 1F);
                GL11.glTranslatef(-1F, -1F, 0F);
                f10 = 0.015625F;
                GL11.glScalef(f10, f10, f10);
                this.mc.textures.Bind(this.mc.textures.LoadTexture("/misc/mapbg.png"));
                Tessellator tessellator19 = Tessellator.Instance;
                GL11.glNormal3f(0F, 0F, -1F);
                tessellator19.Begin();
                byte b20 = 7;
                tessellator19.VertexUV(0 - b20, 128 + b20, 0, 0, 1);
                tessellator19.VertexUV(128 + b20, 128 + b20, 0, 1, 1);
                tessellator19.VertexUV(128 + b20, 0 - b20, 0, 1, 0);
                tessellator19.VertexUV(0 - b20, 0 - b20, 0, 0, 0);
                tessellator19.End();
                MapItemSavedData mapData22 = Item.mapItem.GetItemMapData(itemStack5, this.mc.level);
                this.mapRenderer.Render(this.mc.player, this.mc.textures, mapData22);
                GL11.glPopMatrix();
            }
            else if (itemStack5 != null)
            {
                GL11.glPushMatrix();
                f14 = 0.8F;
                f8 = entityPlayerSP3.GetSwingProgress(f1);
                f9 = Mth.Sin(f8 * Mth.PI);
                f10 = Mth.Sin(Mth.Sqrt(f8) * Mth.PI);
                GL11.glTranslatef(-f10 * 0.4F, Mth.Sin(Mth.Sqrt(f8) * Mth.PI * 2F) * 0.2F, -f9 * 0.2F);
                GL11.glTranslatef(0.7F * f14, -0.65F * f14 - (1F - f2) * 0.6F, -0.9F * f14);
                GL11.glRotatef(45F, 0F, 1F, 0F);
                GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
                f8 = entityPlayerSP3.GetSwingProgress(f1);
                f9 = Mth.Sin(f8 * f8 * Mth.PI);
                f10 = Mth.Sin(Mth.Sqrt(f8) * Mth.PI);
                GL11.glRotatef(-f9 * 20F, 0F, 1F, 0F);
                GL11.glRotatef(-f10 * 20F, 0F, 0F, 1F);
                GL11.glRotatef(-f10 * 80F, 1F, 0F, 0F);
                f8 = 0.4F;
                GL11.glScalef(f8, f8, f8);
                if (itemStack5.GetItem().ShouldRotateAroundWhenRendering())
                {
                    GL11.glRotatef(180F, 0F, 1F, 0F);
                }

                this.RenderItem(entityPlayerSP3, itemStack5);
                GL11.glPopMatrix();
            }
            else
            {
                GL11.glPushMatrix();
                f14 = 0.8F;
                f8 = entityPlayerSP3.GetSwingProgress(f1);
                f9 = Mth.Sin(f8 * Mth.PI);
                f10 = Mth.Sin(Mth.Sqrt(f8) * Mth.PI);
                GL11.glTranslatef(-f10 * 0.3F, Mth.Sin(Mth.Sqrt(f8) * Mth.PI * 2F) * 0.4F, -f9 * 0.4F);
                GL11.glTranslatef(0.8F * f14, -0.75F * f14 - (1F - f2) * 0.6F, -0.9F * f14);
                GL11.glRotatef(45F, 0F, 1F, 0F);
                GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
                f8 = entityPlayerSP3.GetSwingProgress(f1);
                f9 = Mth.Sin(f8 * f8 * Mth.PI);
                f10 = Mth.Sin(Mth.Sqrt(f8) * Mth.PI);
                GL11.glRotatef(f10 * 70F, 0F, 1F, 0F);
                GL11.glRotatef(-f9 * 20F, 0F, 0F, 1F);
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, this.mc.textures.LoadMemTexture(this.mc.player.skinUrl, this.mc.player.GetEntityTexture()));
                GL11.glTranslatef(-1F, 3.6F, 3.5F);
                GL11.glRotatef(120F, 0F, 0F, 1F);
                GL11.glRotatef(200F, 1F, 0F, 0F);
                GL11.glRotatef(-135F, 0F, 1F, 0F);
                GL11.glScalef(1F, 1F, 1F);
                GL11.glTranslatef(5.6F, 0F, 0F);
                object render15 = EntityRenderDispatcher.instance.GetEntityRenderObjectAsObject(this.mc.player);
                RenderPlayer renderPlayer16 = (RenderPlayer)render15;
                f10 = 1F;
                GL11.glScalef(f10, f10, f10);
                renderPlayer16.DrawFirstPersonHand();
                GL11.glPopMatrix();
            }

            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            Light.TurnOff();
        }

        public virtual void RenderOverlays(float f1)
        {
            GL11.glDisable(GL11C.GL_ALPHA_TEST);
            uint i21;
            if (this.mc.player.IsBurning())
            {
                i21 = this.mc.textures.LoadTexture("/terrain.png");
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i21);
                this.RenderFireInFirstPerson(f1);
            }

            if (this.mc.player.IsEntityInsideOpaqueBlock())
            {
                int i2 = Mth.Floor(this.mc.player.x);
                int i3 = Mth.Floor(this.mc.player.y);
                int i4 = Mth.Floor(this.mc.player.z);
                uint i5 = this.mc.textures.LoadTexture("/terrain.png");
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i5);
                int i6 = this.mc.level.GetTile(i2, i3, i4);
                if (this.mc.level.IsSolidBlockingTile(i2, i3, i4))
                {
                    this.RenderInsideOfBlock(f1, Tile.tiles[i6].GetTexture(Facing.TileFace.NORTH));
                }
                else
                {
                    for (int i7 = 0; i7 < 8; ++i7)
                    {
                        float f8 = ((i7 >> 0) % 2 - 0.5F) * this.mc.player.width * 0.9F;
                        float f9 = ((i7 >> 1) % 2 - 0.5F) * this.mc.player.height * 0.2F;
                        float f10 = ((i7 >> 2) % 2 - 0.5F) * this.mc.player.width * 0.9F;
                        int i11 = Mth.Floor(i2 + f8);
                        int i12 = Mth.Floor(i3 + f9);
                        int i13 = Mth.Floor(i4 + f10);
                        if (this.mc.level.IsSolidBlockingTile(i11, i12, i13))
                        {
                            i6 = this.mc.level.GetTile(i11, i12, i13);
                        }
                    }
                }

                if (Tile.tiles[i6] != null)
                {
                    this.RenderInsideOfBlock(f1, Tile.tiles[i6].GetTexture(Facing.TileFace.NORTH));
                }
            }

            if (this.mc.player.IsInsideOfMaterial(Material.water))
            {
                i21 = this.mc.textures.LoadTexture("/misc/water.png");
                GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i21);
                this.RenderWarpedTextureOverlay(f1);
            }

            GL11.glEnable(GL11C.GL_ALPHA_TEST);
        }

        private void RenderInsideOfBlock(float f1, int i2)
        {
            Tessellator tessellator3 = Tessellator.Instance;
            this.mc.player.GetEntityBrightness(f1);
            float f4 = 0.1F;
            GL11.glColor4f(f4, f4, f4, 0.5F);
            GL11.glPushMatrix();
            float f5 = -1F;
            float f6 = 1F;
            float f7 = -1F;
            float f8 = 1F;
            float f9 = -0.5F;
            float f10 = 0.0078125F;
            float f11 = i2 % 16 / 256F - f10;
            float f12 = (i2 % 16 + 15.99F) / 256F + f10;
            float f13 = i2 / 16 / 256F - f10;
            float f14 = (i2 / 16 + 15.99F) / 256F + f10;
            tessellator3.Begin();
            tessellator3.VertexUV(f5, f7, f9, f12, f14);
            tessellator3.VertexUV(f6, f7, f9, f11, f14);
            tessellator3.VertexUV(f6, f8, f9, f11, f13);
            tessellator3.VertexUV(f5, f8, f9, f12, f13);
            tessellator3.End();
            GL11.glPopMatrix();
            GL11.glColor4f(1F, 1F, 1F, 1F);
        }

        private void RenderWarpedTextureOverlay(float f1)
        {
            Tessellator tessellator2 = Tessellator.Instance;
            float f3 = this.mc.player.GetEntityBrightness(f1);
            GL11.glColor4f(f3, f3, f3, 0.5F);
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            GL11.glPushMatrix();
            float f4 = 4F;
            float f5 = -1F;
            float f6 = 1F;
            float f7 = -1F;
            float f8 = 1F;
            float f9 = -0.5F;
            float f10 = -this.mc.player.yaw / 64F;
            float f11 = this.mc.player.pitch / 64F;
            tessellator2.Begin();
            tessellator2.VertexUV(f5, f7, f9, f4 + f10, f4 + f11);
            tessellator2.VertexUV(f6, f7, f9, 0F + f10, f4 + f11);
            tessellator2.VertexUV(f6, f8, f9, 0F + f10, 0F + f11);
            tessellator2.VertexUV(f5, f8, f9, f4 + f10, 0F + f11);
            tessellator2.End();
            GL11.glPopMatrix();
            GL11.glColor4f(1F, 1F, 1F, 1F);
            GL11.glDisable(GL11C.GL_BLEND);
        }

        private void RenderFireInFirstPerson(float f1)
        {
            Tessellator tessellator2 = Tessellator.Instance;
            GL11.glColor4f(1F, 1F, 1F, 0.9F);
            GL11.glEnable(GL11C.GL_BLEND);
            GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
            float f3 = 1F;
            for (int i4 = 0; i4 < 2; ++i4)
            {
                GL11.glPushMatrix();
                int i5 = Tile.fire.texture + i4 * 16;
                int i6 = (i5 & 15) << 4;
                int i7 = i5 & 240;
                float f8 = i6 / 256F;
                float f9 = (i6 + 15.99F) / 256F;
                float f10 = i7 / 256F;
                float f11 = (i7 + 15.99F) / 256F;
                float f12 = (0F - f3) / 2F;
                float f13 = f12 + f3;
                float f14 = 0F - f3 / 2F;
                float f15 = f14 + f3;
                float f16 = -0.5F;
                GL11.glTranslatef((-(i4 * 2 - 1)) * 0.24F, -0.3F, 0F);
                GL11.glRotatef((i4 * 2 - 1) * 10F, 0F, 1F, 0F);
                tessellator2.Begin();
                tessellator2.VertexUV(f12, f14, f16, f9, f11);
                tessellator2.VertexUV(f13, f14, f16, f8, f11);
                tessellator2.VertexUV(f13, f15, f16, f8, f10);
                tessellator2.VertexUV(f12, f15, f16, f9, f10);
                tessellator2.End();
                GL11.glPopMatrix();
            }

            GL11.glColor4f(1F, 1F, 1F, 1F);
            GL11.glDisable(GL11C.GL_BLEND);
        }

        public virtual void UpdateEquippedItem()
        {
            this.prevEquippedProgress = this.equippedProgress;
            LocalPlayer entityPlayerSP1 = this.mc.player;
            ItemInstance itemStack2 = entityPlayerSP1.inventory.GetCurrentItem();
            bool z4 = this.field_20099_f == entityPlayerSP1.inventory.currentItem && itemStack2 == this.itemToRender;
            if (this.itemToRender == null && itemStack2 == null)
            {
                z4 = true;
            }

            if (itemStack2 != null && this.itemToRender != null && itemStack2 != this.itemToRender && itemStack2.itemID == this.itemToRender.itemID && itemStack2.GetItemDamage() == this.itemToRender.GetItemDamage())
            {
                this.itemToRender = itemStack2;
                z4 = true;
            }

            float f5 = 0.4F;
            float f6 = z4 ? 1F : 0F;
            float f7 = f6 - this.equippedProgress;
            if (f7 < -f5)
            {
                f7 = -f5;
            }

            if (f7 > f5)
            {
                f7 = f5;
            }

            this.equippedProgress += f7;
            if (this.equippedProgress < 0.1F)
            {
                this.itemToRender = itemStack2;
                this.field_20099_f = entityPlayerSP1.inventory.currentItem;
            }
        }

        public virtual void Func_9449_b()
        {
            this.equippedProgress = 0F;
        }

        public virtual void Func_9450_c()
        {
            this.equippedProgress = 0F;
        }
    }
}
