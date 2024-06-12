using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Items;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderBiped : RenderLiving<Mob>
    {
        protected ModelBiped modelBipedMain;

        public RenderBiped(ModelBiped modelBiped1, float f2) : base(modelBiped1, f2)
        {
            this.modelBipedMain = modelBiped1;
        }

        protected override void RenderEquippedItems(Mob entityLiving1, float f2)
        {
            ItemInstance itemStack3 = entityLiving1.GetHeldItem();
            if (itemStack3 != null)
            {
                GL11.glPushMatrix();
                this.modelBipedMain.bipedRightArm.PostRender(0.0625F);
                GL11.glTranslatef(-0.0625F, 0.4375F, 0.0625F);
                float f4;
                if (itemStack3.itemID < 256 && TileRenderer.RenderItemIn3d(Tile.tiles[itemStack3.itemID].GetRenderShape()))
                {
                    f4 = 0.5F;
                    GL11.glTranslatef(0.0F, 0.1875F, -0.3125F);
                    f4 *= 0.75F;
                    GL11.glRotatef(20.0F, 1.0F, 0.0F, 0.0F);
                    GL11.glRotatef(45.0F, 0.0F, 1.0F, 0.0F);
                    GL11.glScalef(f4, -f4, f4);
                }
                else if (Item.items[itemStack3.itemID].IsFull3D())
                {
                    f4 = 0.625F;
                    GL11.glTranslatef(0.0F, 0.1875F, 0.0F);
                    GL11.glScalef(f4, -f4, f4);
                    GL11.glRotatef(-100.0F, 1.0F, 0.0F, 0.0F);
                    GL11.glRotatef(45.0F, 0.0F, 1.0F, 0.0F);
                }
                else
                {
                    f4 = 0.375F;
                    GL11.glTranslatef(0.25F, 0.1875F, -0.1875F);
                    GL11.glScalef(f4, f4, f4);
                    GL11.glRotatef(60.0F, 0.0F, 0.0F, 1.0F);
                    GL11.glRotatef(-90.0F, 1.0F, 0.0F, 0.0F);
                    GL11.glRotatef(20.0F, 0.0F, 0.0F, 1.0F);
                }

                this.renderManager.itemRenderer.RenderItem(entityLiving1, itemStack3);
                GL11.glPopMatrix();
            }

        }
    }
}