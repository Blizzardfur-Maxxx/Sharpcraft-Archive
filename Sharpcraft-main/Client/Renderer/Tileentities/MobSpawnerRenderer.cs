using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer.Entities;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpCraft.Core.Util;

namespace SharpCraft.Client.Renderer.Tileentities
{
    public class MobSpawnerRenderer : TileEntityRenderer<TileEntityMobSpawner>
    {
        private NullDictionary<string, Entity> entityHashMap = new NullDictionary<string, Entity>();

        public override void RenderTileEntityAt(TileEntityMobSpawner tileEntity1, double d2, double d4, double d6, float f8)
        {
            GL11.glPushMatrix();
            GL11.glTranslatef((float)d2 + 0.5F, (float)d4, (float)d6 + 0.5F);
            Entity entity9 = this.entityHashMap[tileEntity1.GetMobID()];
            if (entity9 == null)
            {
                entity9 = EntityFactory.CreateEntity(tileEntity1.GetMobID(), null);
                this.entityHashMap[tileEntity1.GetMobID()] = entity9;
            }

            if (entity9 != null)
            {
                entity9.SetWorld(tileEntity1.worldObj);
                float f10 = 0.4375F;
                GL11.glTranslatef(0.0F, 0.4F, 0.0F);
                GL11.glRotatef((float)(tileEntity1.yaw2 + (tileEntity1.yaw - tileEntity1.yaw2) * f8) * 10.0F, 0.0F, 1.0F, 0.0F);
                GL11.glRotatef(-30.0F, 1.0F, 0.0F, 0.0F);
                GL11.glTranslatef(0.0F, -0.4F, 0.0F);
                GL11.glScalef(f10, f10, f10);
                entity9.SetLocationAndAngles(d2, d4, d6, 0.0F, 0.0F);
                EntityRenderDispatcher.instance.RenderEntityWithPosYaw(entity9, 0.0D, 0.0D, 0.0D, 0.0F, f8);
            }

            GL11.glPopMatrix();
        }
    }
}
