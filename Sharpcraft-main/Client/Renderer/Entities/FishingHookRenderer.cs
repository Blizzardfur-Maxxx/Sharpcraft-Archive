using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Client.Renderer.Entities
{
    public class FishingHookRenderer : Render<FishingHook>
    {
        public override void DoRender(FishingHook entityFish1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glPushMatrix();
            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
            GL11.glEnable(GL12C.GL_RESCALE_NORMAL);
            GL11.glScalef(0.5F, 0.5F, 0.5F);
            byte b10 = 1;
            byte b11 = 2;
            this.LoadTexture("/particles.png");
            Tessellator tessellator12 = Tessellator.Instance;
            float f13 = (b10 * 8 + 0) / 128.0F;
            float f14 = (b10 * 8 + 8) / 128.0F;
            float f15 = (b11 * 8 + 0) / 128.0F;
            float f16 = (b11 * 8 + 8) / 128.0F;
            float f17 = 1.0F;
            float f18 = 0.5F;
            float f19 = 0.5F;
            GL11.glRotatef(180.0F - this.renderManager.playerViewY, 0.0F, 1.0F, 0.0F);
            GL11.glRotatef(-this.renderManager.playerViewX, 1.0F, 0.0F, 0.0F);
            tessellator12.Begin();
            tessellator12.Normal(0.0F, 1.0F, 0.0F);
            tessellator12.VertexUV(0.0F - f18, 0.0F - f19, 0.0D, f13, f16);
            tessellator12.VertexUV(f17 - f18, 0.0F - f19, 0.0D, f14, f16);
            tessellator12.VertexUV(f17 - f18, 1.0F - f19, 0.0D, f14, f15);
            tessellator12.VertexUV(0.0F - f18, 1.0F - f19, 0.0D, f13, f15);
            tessellator12.End();
            GL11.glDisable(GL12C.GL_RESCALE_NORMAL);
            GL11.glPopMatrix();
            if (entityFish1.owner != null)
            {
                float f20 = (entityFish1.owner.prevYaw + (entityFish1.owner.yaw - entityFish1.owner.prevYaw) * f9) * Mth.PI / 180.0F;
                double d21 = Mth.Sin(f20);
                double d23 = Mth.Cos(f20);
                float f25 = entityFish1.owner.GetSwingProgress(f9);
                float f26 = Mth.Sin(Mth.Sqrt(f25) * Mth.PI);
                Vec3 vec3D27 = Vec3.Of(-0.5D, 0.03D, 0.8D);
                vec3D27.RotateAroundX(-(entityFish1.owner.prevPitch + (entityFish1.owner.pitch - entityFish1.owner.prevPitch) * f9) * Mth.PI / 180.0F);
                vec3D27.RotateAroundY(-(entityFish1.owner.prevYaw + (entityFish1.owner.yaw - entityFish1.owner.prevYaw) * f9) * Mth.PI / 180.0F);
                vec3D27.RotateAroundY(f26 * 0.5F);
                vec3D27.RotateAroundX(-f26 * 0.7F);
                double d28 = entityFish1.owner.prevX + (entityFish1.owner.x - entityFish1.owner.prevX) * f9 + vec3D27.x;
                double d30 = entityFish1.owner.prevY + (entityFish1.owner.y - entityFish1.owner.prevY) * f9 + vec3D27.y;
                double d32 = entityFish1.owner.prevZ + (entityFish1.owner.z - entityFish1.owner.prevZ) * f9 + vec3D27.z;
                if (this.renderManager.options.thirdPersonView)
                {
                    f20 = (entityFish1.owner.prevRenderYawOffset + (entityFish1.owner.renderYawOffset - entityFish1.owner.prevRenderYawOffset) * f9) * Mth.PI / 180.0F;
                    d21 = Mth.Sin(f20);
                    d23 = Mth.Cos(f20);
                    d28 = entityFish1.owner.prevX + (entityFish1.owner.x - entityFish1.owner.prevX) * f9 - d23 * 0.35D - d21 * 0.85D;
                    d30 = entityFish1.owner.prevY + (entityFish1.owner.y - entityFish1.owner.prevY) * f9 - 0.45D;
                    d32 = entityFish1.owner.prevZ + (entityFish1.owner.z - entityFish1.owner.prevZ) * f9 - d21 * 0.35D + d23 * 0.85D;
                }

                double d34 = entityFish1.prevX + (entityFish1.x - entityFish1.prevX) * f9;
                double d36 = entityFish1.prevY + (entityFish1.y - entityFish1.prevY) * f9 + 0.25D;
                double d38 = entityFish1.prevZ + (entityFish1.z - entityFish1.prevZ) * f9;
                double d40 = ((float)(d28 - d34));
                double d42 = ((float)(d30 - d36));
                double d44 = ((float)(d32 - d38));
                GL11.glDisable(GL11C.GL_TEXTURE_2D);
                GL11.glDisable(GL11C.GL_LIGHTING);
                tessellator12.Begin(3);
                tessellator12.Color(0);
                byte b46 = 16;

                for (int i47 = 0; i47 <= b46; ++i47)
                {
                    float f48 = (float)i47 / (float)b46;
                    tessellator12.Vertex(d2 + d40 * f48, d4 + d42 * (f48 * f48 + f48) * 0.5D + 0.25D, d6 + d44 * f48);
                }

                tessellator12.End();
                GL11.glEnable(GL11C.GL_LIGHTING);
                GL11.glEnable(GL11C.GL_TEXTURE_2D);
            }

        }
    }
}