using LWCSGL.OpenGL;
using SharpCraft.Client.GUI;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Tileentities
{
    public class TileEntityRenderDispatcher
    {
        private NullDictionary<Type, TileEntityRenderer> renderers = new NullDictionary<Type, TileEntityRenderer>();
        public static TileEntityRenderDispatcher instance = new TileEntityRenderDispatcher();
        private Font fontRenderer;
        public static double staticPlayerX;
        public static double staticPlayerY;
        public static double staticPlayerZ;
        public Textures renderEngine;
        public Level worldObj;
        public Mob entityLivingPlayer;
        public float playerYaw;
        public float playerPitch;
        public double playerX;
        public double playerY;
        public double playerZ;

        private TileEntityRenderDispatcher()
        {
            this.renderers[typeof(TileEntitySign)] = new SignRenderer();
            this.renderers[typeof(TileEntityMobSpawner)] = new MobSpawnerRenderer();
            this.renderers[typeof(PistonPieceEntity)] = new PistonRenderer();

            foreach (TileEntityRenderer e in this.renderers.Values)
            {
                e.SetTileEntityRenderer(this);
            }
        }

        public TileEntityRenderer GetSpecialRendererForClass(Type clazz)
        {
            this.renderers.TryGetValue(clazz, out TileEntityRenderer render);

            if (render == null && clazz != typeof(TileEntity))
            {
                render = this.GetSpecialRendererForClass(clazz.BaseType);
                this.renderers[clazz] = render;
            }

            return render;
        }

        public bool HasSpecialRenderer(TileEntity tileEntity1)
        {
            return this.GetSpecialRendererForEntity(tileEntity1) != null;
        }

        public TileEntityRenderer GetSpecialRendererForEntity(TileEntity tileEntity1)
        {
            return tileEntity1 == null ? null : this.GetSpecialRendererForClass(tileEntity1.GetType());
        }

        public object GetEntityRenderObjectAsObject(TileEntity e)
        {
            return e == null ? null : this.GetSpecialRendererForClass(e.GetType());
        }

        public void CacheActiveRenderInfo(Level world1, Textures renderEngine2, Font fontRenderer3, Mob entityLiving4, float f5)
        {
            if (this.worldObj != world1)
            {
                this.SetLevel(world1);
            }

            this.renderEngine = renderEngine2;
            this.entityLivingPlayer = entityLiving4;
            this.fontRenderer = fontRenderer3;
            this.playerYaw = entityLiving4.prevYaw + (entityLiving4.yaw - entityLiving4.prevYaw) * f5;
            this.playerPitch = entityLiving4.prevPitch + (entityLiving4.pitch - entityLiving4.prevPitch) * f5;
            this.playerX = entityLiving4.lastTickPosX + (entityLiving4.x - entityLiving4.lastTickPosX) * f5;
            this.playerY = entityLiving4.lastTickPosY + (entityLiving4.y - entityLiving4.lastTickPosY) * f5;
            this.playerZ = entityLiving4.lastTickPosZ + (entityLiving4.z - entityLiving4.lastTickPosZ) * f5;
        }

        public void RenderTileEntity(TileEntity tileEntity1, float f2)
        {
            if (tileEntity1.GetDistanceFrom(this.playerX, this.playerY, this.playerZ) < 4096.0D)
            {
                float f3 = this.worldObj.GetBrightness(tileEntity1.xCoord, tileEntity1.yCoord, tileEntity1.zCoord);
                GL11.glColor3f(f3, f3, f3);
                this.RenderTileEntityAt(tileEntity1, tileEntity1.xCoord - staticPlayerX, tileEntity1.yCoord - staticPlayerY, tileEntity1.zCoord - staticPlayerZ, f2);
            }

        }

        public void RenderTileEntityAt(TileEntity tileEntity1, double d2, double d4, double d6, float f8)
        {
            TileEntityRenderer tileEntitySpecialRenderer9 = this.GetSpecialRendererForEntity(tileEntity1);
            if (tileEntitySpecialRenderer9 != null)
            {
                tileEntitySpecialRenderer9.RenderTileEntityAt(tileEntity1, d2, d4, d6, f8);
            }

        }

        public void SetLevel(Level world1)
        {
            this.worldObj = world1;

            foreach (TileEntityRenderer ter in this.renderers.Values)
            {
                if (ter != null)
                {
                    ter.SetLevel(world1);
                }
            }

        }

        public Font GetFontRenderer()
        {
            return this.fontRenderer;
        }
    }
}
