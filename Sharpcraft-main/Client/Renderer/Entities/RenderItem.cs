using LWCSGL.OpenGL;
using SharpCraft.Client.GUI;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;
using System;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderItem : Render<ItemEntity>
    {
        private TileRenderer renderBlocks = new TileRenderer();
        private JRandom random = new JRandom();
        public bool field_27004_a = true;

        public RenderItem()
        {
            this.shadowSize = 0.15F;
            this.field_194_c = 0.75F;
        }

        public override void DoRender(ItemEntity entityItem1, double d2, double d4, double d6, float f8, float f9)
        {
            this.random.SetSeed(187L);
            ItemInstance itemStack10 = entityItem1.item;
            GL11.glPushMatrix();
            float f11 = Mth.Sin((entityItem1.age + f9) / 10.0F + entityItem1.mth) * 0.1F + 0.1F;
            float f12 = ((entityItem1.age + f9) / 20.0F + entityItem1.mth) * 57.295776F;
            byte b13 = 1;
            if (entityItem1.item.stackSize > 1)
            {
                b13 = 2;
            }

            if (entityItem1.item.stackSize > 5)
            {
                b13 = 3;
            }

            if (entityItem1.item.stackSize > 20)
            {
                b13 = 4;
            }

            GL11.glTranslatef((float)d2, (float)d4 + f11, (float)d6);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            float f16;
            float f17;
            float f18;
            if (itemStack10.itemID < 256 && TileRenderer.RenderItemIn3d(Tile.tiles[itemStack10.itemID].GetRenderShape()))
            {
                GL11.glRotatef(f12, 0.0F, 1.0F, 0.0F);
                this.LoadTexture("/terrain.png");
                float f28 = 0.25F;
                if (!Tile.tiles[itemStack10.itemID].IsCubeShaped() && itemStack10.itemID != Tile.stoneSlabHalf.id && Tile.tiles[itemStack10.itemID].GetRenderShape() != Tile.RenderShape.PISTON_BASE)
                {
                    f28 = 0.5F;
                }

                GL11.glScalef(f28, f28, f28);

                for (int i29 = 0; i29 < b13; ++i29)
                {
                    GL11.glPushMatrix();
                    if (i29 > 0)
                    {
                        f16 = (this.random.NextFloat() * 2.0F - 1.0F) * 0.2F / f28;
                        f17 = (this.random.NextFloat() * 2.0F - 1.0F) * 0.2F / f28;
                        f18 = (this.random.NextFloat() * 2.0F - 1.0F) * 0.2F / f28;
                        GL11.glTranslatef(f16, f17, f18);
                    }

                    this.renderBlocks.RenderBlockOnInventory(Tile.tiles[itemStack10.itemID], itemStack10.GetItemDamage(), entityItem1.GetEntityBrightness(f9));
                    GL11.glPopMatrix();
                }
            }
            else
            {
                GL11.glScalef(0.5F, 0.5F, 0.5F);
                int i14 = itemStack10.GetIconIndex();
                if (itemStack10.itemID < 256)
                {
                    this.LoadTexture("/terrain.png");
                }
                else
                {
                    this.LoadTexture("/gui/items.png");
                }

                Tessellator tessellator15 = Tessellator.Instance;
                f16 = (i14 % 16 * 16 + 0) / 256.0F;
                f17 = (i14 % 16 * 16 + 16) / 256.0F;
                f18 = (i14 / 16 * 16 + 0) / 256.0F;
                float f19 = (i14 / 16 * 16 + 16) / 256.0F;
                float f20 = 1.0F;
                float f21 = 0.5F;
                float f22 = 0.25F;
                int i23;
                float f24;
                float f25;
                float f26;
                if (this.field_27004_a)
                {
                    i23 = Item.items[itemStack10.itemID].GetColorFromDamage(itemStack10.GetItemDamage());
                    f24 = (i23 >> 16 & 255) / 255.0F;
                    f25 = (i23 >> 8 & 255) / 255.0F;
                    f26 = (i23 & 255) / 255.0F;
                    float f27 = entityItem1.GetEntityBrightness(f9);
                    GL11.glColor4f(f24 * f27, f25 * f27, f26 * f27, 1.0F);
                }

                for (i23 = 0; i23 < b13; ++i23)
                {
                    GL11.glPushMatrix();
                    if (i23 > 0)
                    {
                        f24 = (this.random.NextFloat() * 2.0F - 1.0F) * 0.3F;
                        f25 = (this.random.NextFloat() * 2.0F - 1.0F) * 0.3F;
                        f26 = (this.random.NextFloat() * 2.0F - 1.0F) * 0.3F;
                        GL11.glTranslatef(f24, f25, f26);
                    }

                    GL11.glRotatef(180.0F - this.renderManager.playerViewY, 0.0F, 1.0F, 0.0F);
                    tessellator15.Begin();
                    tessellator15.Normal(0.0F, 1.0F, 0.0F);
                    tessellator15.VertexUV(0.0F - f21, 0.0F - f22, 0.0D, f16, f19);
                    tessellator15.VertexUV(f20 - f21, 0.0F - f22, 0.0D, f17, f19);
                    tessellator15.VertexUV(f20 - f21, 1.0F - f22, 0.0D, f17, f18);
                    tessellator15.VertexUV(0.0F - f21, 1.0F - f22, 0.0D, f16, f18);
                    tessellator15.End();
                    GL11.glPopMatrix();
                }
            }

            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            GL11.glPopMatrix();
        }

        public void DrawItemIntoGui(Font fontRenderer1, Textures renderEngine2, int i3, int i4, int i5, int i6, int i7)
        {
            float f11;
            if (i3 < 256 && TileRenderer.RenderItemIn3d(Tile.tiles[i3].GetRenderShape()))
            {
                renderEngine2.Bind(renderEngine2.LoadTexture("/terrain.png"));
                Tile block14 = Tile.tiles[i3];
                GL11.glPushMatrix();
                GL11.glTranslatef(i6 - 2, i7 + 3, -3.0F);
                GL11.glScalef(10.0F, 10.0F, 10.0F);
                GL11.glTranslatef(1.0F, 0.5F, 1.0F);
                GL11.glScalef(1.0F, 1.0F, -1.0F);
                GL11.glRotatef(210.0F, 1.0F, 0.0F, 0.0F);
                GL11.glRotatef(45.0F, 0.0F, 1.0F, 0.0F);
                int i15 = Item.items[i3].GetColorFromDamage(i4);
                f11 = (i15 >> 16 & 255) / 255.0F;
                float f12 = (i15 >> 8 & 255) / 255.0F;
                float f13 = (i15 & 255) / 255.0F;
                if (this.field_27004_a)
                {
                    GL11.glColor4f(f11, f12, f13, 1.0F);
                }

                GL11.glRotatef(-90.0F, 0.0F, 1.0F, 0.0F);
                this.renderBlocks.field_31088_b = this.field_27004_a;
                this.renderBlocks.RenderBlockOnInventory(block14, i4, 1.0F);
                this.renderBlocks.field_31088_b = true;
                GL11.glPopMatrix();
            }
            else if (i5 >= 0)
            {
                GL11.glDisable(GL11C.GL_LIGHTING);
                if (i3 < 256)
                {
                    renderEngine2.Bind(renderEngine2.LoadTexture("/terrain.png"));
                }
                else
                {
                    renderEngine2.Bind(renderEngine2.LoadTexture("/gui/items.png"));
                }

                int i8 = Item.items[i3].GetColorFromDamage(i4);
                float f9 = (i8 >> 16 & 255) / 255.0F;
                float f10 = (i8 >> 8 & 255) / 255.0F;
                f11 = (i8 & 255) / 255.0F;
                if (this.field_27004_a)
                {
                    GL11.glColor4f(f9, f10, f11, 1.0F);
                }

                this.RenderTexturedQuad(i6, i7, i5 % 16 * 16, i5 / 16 * 16, 16, 16);
                GL11.glEnable(GL11C.GL_LIGHTING);
            }

            GL11.glEnable(GL11C.GL_CULL_FACE);
        }

        public void RenderItemIntoGUI(Font fontRenderer1, Textures renderEngine2, ItemInstance itemStack3, int i4, int i5)
        {
            if (itemStack3 != null)
            {
                this.DrawItemIntoGui(fontRenderer1, renderEngine2, itemStack3.itemID, itemStack3.GetItemDamage(), itemStack3.GetIconIndex(), i4, i5);
            }
        }

        public void RenderItemOverlayIntoGUI(Font fontRenderer1, Textures renderEngine2, ItemInstance itemStack3, int i4, int i5)
        {
            if (itemStack3 != null)
            {
                if (itemStack3.stackSize > 1)
                {
                    String string6 = "" + itemStack3.stackSize;
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    GL11.glDisable(GL11C.GL_DEPTH_TEST);
                    fontRenderer1.DrawStringWithShadow(string6, i4 + 19 - 2 - fontRenderer1.GetStringWidth(string6), i5 + 6 + 3, 0xFFFFFF);
                    GL11.glEnable(GL11C.GL_LIGHTING);
                    GL11.glEnable(GL11C.GL_DEPTH_TEST);
                }

                if (itemStack3.IsItemDamaged())
                {
                    int i11 = (int)Math.Round(13.0D - itemStack3.GetItemDamageForDisplay() * 13.0D / itemStack3.GetMaxDamage());
                    int i7 = (int)Math.Round(255.0D - itemStack3.GetItemDamageForDisplay() * 255.0D / itemStack3.GetMaxDamage());
                    GL11.glDisable(GL11C.GL_LIGHTING);
                    GL11.glDisable(GL11C.GL_DEPTH_TEST);
                    GL11.glDisable(GL11C.GL_TEXTURE_2D);
                    Tessellator tessellator8 = Tessellator.Instance;
                    int i9 = 255 - i7 << 16 | i7 << 8;
                    int i10 = (255 - i7) / 4 << 16 | 16128;
                    this.RenderQuad(tessellator8, i4 + 2, i5 + 13, 13, 2, 0);
                    this.RenderQuad(tessellator8, i4 + 2, i5 + 13, 12, 1, i10);
                    this.RenderQuad(tessellator8, i4 + 2, i5 + 13, i11, 1, i9);
                    GL11.glEnable(GL11C.GL_TEXTURE_2D);
                    GL11.glEnable(GL11C.GL_LIGHTING);
                    GL11.glEnable(GL11C.GL_DEPTH_TEST);
                    GL11.glColor4f(1.0F, 1.0F, 1.0F, 1.0F);
                }

            }
        }

        private void RenderQuad(Tessellator tessellator1, int i2, int i3, int i4, int i5, int i6)
        {
            tessellator1.Begin();
            tessellator1.Color(i6);
            tessellator1.Vertex(i2 + 0, i3 + 0, 0.0D);
            tessellator1.Vertex(i2 + 0, i3 + i5, 0.0D);
            tessellator1.Vertex(i2 + i4, i3 + i5, 0.0D);
            tessellator1.Vertex(i2 + i4, i3 + 0, 0.0D);
            tessellator1.End();
        }

        public void RenderTexturedQuad(int i1, int i2, int i3, int i4, int i5, int i6)
        {
            float f7 = 0.0F;
            float f8 = 0.00390625F;
            float f9 = 0.00390625F;
            Tessellator tessellator10 = Tessellator.Instance;
            tessellator10.Begin();
            tessellator10.VertexUV(i1 + 0, i2 + i6, f7, (i3 + 0) * f8, (i4 + i6) * f9);
            tessellator10.VertexUV(i1 + i5, i2 + i6, f7, (i3 + i5) * f8, (i4 + i6) * f9);
            tessellator10.VertexUV(i1 + i5, i2 + 0, f7, (i3 + i5) * f8, (i4 + 0) * f9);
            tessellator10.VertexUV(i1 + 0, i2 + 0, f7, (i3 + 0) * f8, (i4 + 0) * f9);
            tessellator10.End();
        }
    }
}