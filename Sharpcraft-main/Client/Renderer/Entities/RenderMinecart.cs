using LWCSGL.OpenGL;
using SharpCraft.Client.Models;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.Phys;
using System;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderMinecart : Render<Minecart>
    {
        protected Model modelMinecart;

        public RenderMinecart()
        {
            this.shadowSize = 0.5F;
            this.modelMinecart = new ModelMinecart();
        }

        public override void DoRender(Minecart entityMinecart1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glPushMatrix();
            double d10 = entityMinecart1.lastTickPosX + (entityMinecart1.x - entityMinecart1.lastTickPosX) * f9;
            double d12 = entityMinecart1.lastTickPosY + (entityMinecart1.y - entityMinecart1.lastTickPosY) * f9;
            double d14 = entityMinecart1.lastTickPosZ + (entityMinecart1.z - entityMinecart1.lastTickPosZ) * f9;
            double d16 = 0.3F;
            Vec3 vec3D18 = entityMinecart1.Func_g(d10, d12, d14);
            float f19 = entityMinecart1.prevPitch + (entityMinecart1.pitch - entityMinecart1.prevPitch) * f9;
            if (vec3D18 != null)
            {
                Vec3 vec3D20 = entityMinecart1.Func_515_a(d10, d12, d14, d16);
                Vec3 vec3D21 = entityMinecart1.Func_515_a(d10, d12, d14, -d16);
                if (vec3D20 == null)
                {
                    vec3D20 = vec3D18;
                }

                if (vec3D21 == null)
                {
                    vec3D21 = vec3D18;
                }

                d2 += vec3D18.x - d10;
                d4 += (vec3D20.y + vec3D21.y) / 2.0D - d12;
                d6 += vec3D18.z - d14;
                Vec3 vec3D22 = vec3D21.AddVector(-vec3D20.x, -vec3D20.y, -vec3D20.z);
                if (vec3D22.LengthVector() != 0.0D)
                {
                    vec3D22 = vec3D22.Normalize();
                    f8 = (float)(Math.Atan2(vec3D22.z, vec3D22.x) * 180.0D / Math.PI);
                    f19 = (float)(Math.Atan(vec3D22.y) * 73.0D);
                }
            }

            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
            GL11.glRotatef(180.0F - f8, 0.0F, 1.0F, 0.0F);
            GL11.glRotatef(-f19, 0.0F, 0.0F, 1.0F);
            float f23 = entityMinecart1.minecartTimeSinceHit - f9;
            float f24 = entityMinecart1.minecartCurrentDamage - f9;
            if (f24 < 0.0F)
            {
                f24 = 0.0F;
            }

            if (f23 > 0.0F)
            {
                GL11.glRotatef(Mth.Sin(f23) * f23 * f24 / 10.0F * entityMinecart1.minecartRockDirection, 1.0F, 0.0F, 0.0F);
            }

            if (entityMinecart1.minecartType != 0)
            {
                this.LoadTexture("/terrain.png");
                float f25 = 0.75F;
                GL11.glScalef(f25, f25, f25);
                GL11.glTranslatef(0.0F, 0.3125F, 0.0F);
                GL11.glRotatef(90.0F, 0.0F, 1.0F, 0.0F);
                if (entityMinecart1.minecartType == 1)
                {
                    (new TileRenderer()).RenderBlockOnInventory(Tile.chest, 0, entityMinecart1.GetEntityBrightness(f9));
                }
                else if (entityMinecart1.minecartType == 2)
                {
                    (new TileRenderer()).RenderBlockOnInventory(Tile.furnace, 0, entityMinecart1.GetEntityBrightness(f9));
                }

                GL11.glRotatef(-90.0F, 0.0F, 1.0F, 0.0F);
                GL11.glTranslatef(0.0F, -0.3125F, 0.0F);
                GL11.glScalef(1.0F / f25, 1.0F / f25, 1.0F / f25);
            }

            this.LoadTexture("/item/cart.png");
            GL11.glScalef(-1.0F, -1.0F, 1.0F);
            this.modelMinecart.Render(0.0F, 0.0F, -0.1F, 0.0F, 0.0F, 0.0625F);
            GL11.glPopMatrix();
        }
    }
}