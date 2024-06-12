using LWCSGL.OpenGL;
using SharpCraft.Client.Renderer;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel;
using SharpCraft.Core.World.GameLevel.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Client.Particles
{
    public class ParticleEngine
    {
        protected Level worldObj;
        private List<Particle>[] fxLayers = new List<Particle>[4];
        private Textures renderer;
        private JRandom rand = new JRandom();

        public ParticleEngine(Level world1, Textures renderEngine2)
        {
            if (world1 != null)
            {
                this.worldObj = world1;
            }

            this.renderer = renderEngine2;

            for (int i3 = 0; i3 < 4; ++i3)
            {
                this.fxLayers[i3] = new List<Particle>();
            }

        }

        public void AddEffect(Particle entityFX1)
        {
            int i2 = entityFX1.GetFXLayer();
            if (this.fxLayers[i2].Count >= 4000)
            {
                this.fxLayers[i2].RemoveAt(0);
            }

            this.fxLayers[i2].Add(entityFX1);
        }

        public void UpdateEffects()
        {
            for (int i1 = 0; i1 < 4; ++i1)
            {
                for (int i2 = 0; i2 < this.fxLayers[i1].Count; ++i2)
                {
                    Particle entityFX3 = this.fxLayers[i1][i2];
                    entityFX3.OnUpdate();
                    if (entityFX3.isDead)
                    {
                        this.fxLayers[i1].RemoveAt(i2--);
                    }
                }
            }

        }

        public void RenderParticles(Entity entity1, float f2)
        {
            float f3 = Mth.Cos(entity1.yaw * Mth.PI / 180.0F);
            float f4 = Mth.Sin(entity1.yaw * Mth.PI / 180.0F);
            float f5 = -f4 * Mth.Sin(entity1.pitch * Mth.PI / 180.0F);
            float f6 = f3 * Mth.Sin(entity1.pitch * Mth.PI / 180.0F);
            float f7 = Mth.Cos(entity1.pitch * Mth.PI / 180.0F);
            Particle.interpPosX = entity1.lastTickPosX + (entity1.x - entity1.lastTickPosX) * f2;
            Particle.interpPosY = entity1.lastTickPosY + (entity1.y - entity1.lastTickPosY) * f2;
            Particle.interpPosZ = entity1.lastTickPosZ + (entity1.z - entity1.lastTickPosZ) * f2;

            for (int i8 = 0; i8 < 3; ++i8)
            {
                if (this.fxLayers[i8].Count != 0)
                {
                    uint i9 = 0;
                    if (i8 == 0)
                    {
                        i9 = this.renderer.LoadTexture("/particles.png");
                    }

                    if (i8 == 1)
                    {
                        i9 = this.renderer.LoadTexture("/terrain.png");
                    }

                    if (i8 == 2)
                    {
                        i9 = this.renderer.LoadTexture("/gui/items.png");
                    }

                    GL11.glBindTexture(GL11C.GL_TEXTURE_2D, i9);
                    Tessellator tessellator10 = Tessellator.Instance;
                    tessellator10.Begin();

                    for (int i11 = 0; i11 < this.fxLayers[i8].Count; ++i11)
                    {
                        Particle entityFX12 = this.fxLayers[i8][i11];
                        entityFX12.RenderParticle(tessellator10, f2, f3, f7, f4, f5, f6);
                    }

                    tessellator10.End();
                }
            }

        }

        public void Func_1187_b(Entity entity1, float f2)
        {
            byte b3 = 3;
            if (this.fxLayers[b3].Count != 0)
            {
                Tessellator tessellator4 = Tessellator.Instance;

                for (int i5 = 0; i5 < this.fxLayers[b3].Count; ++i5)
                {
                    Particle entityFX6 = this.fxLayers[b3][i5];
                    entityFX6.RenderParticle(tessellator4, f2, 0.0F, 0.0F, 0.0F, 0.0F, 0.0F);
                }

            }
        }

        public void ClearEffects(Level world1)
        {
            this.worldObj = world1;

            for (int i2 = 0; i2 < 4; ++i2)
            {
                this.fxLayers[i2].Clear();
            }

        }

        public void AddBlockDestroyEffects(int i1, int i2, int i3, int i4, int i5)
        {
            if (i4 != 0)
            {
                Tile block6 = Tile.tiles[i4];
                byte b7 = 4;

                for (int i8 = 0; i8 < b7; ++i8)
                {
                    for (int i9 = 0; i9 < b7; ++i9)
                    {
                        for (int i10 = 0; i10 < b7; ++i10)
                        {
                            double d11 = i1 + (i8 + 0.5D) / b7;
                            double d13 = i2 + (i9 + 0.5D) / b7;
                            double d15 = i3 + (i10 + 0.5D) / b7;
                            int i17 = this.rand.NextInt(6);
                            this.AddEffect((new EntityDiggingFX(this.worldObj, d11, d13, d15, d11 - i1 - 0.5D, d13 - i2 - 0.5D, d15 - i3 - 0.5D, block6, (TileFace)i17, i5)).Func_4041_a(i1, i2, i3));
                        }
                    }
                }

            }
        }

        public void AddBlockHitEffects(int i1, int i2, int i3, TileFace i4)
        {
            int i5 = this.worldObj.GetTile(i1, i2, i3);
            if (i5 != 0)
            {
                Tile block6 = Tile.tiles[i5];
                float f7 = 0.1F;
                double d8 = i1 + this.rand.NextDouble() * (block6.maxX - block6.minX - f7 * 2.0F) + f7 + block6.minX;
                double d10 = i2 + this.rand.NextDouble() * (block6.maxY - block6.minY - f7 * 2.0F) + f7 + block6.minY;
                double d12 = i3 + this.rand.NextDouble() * (block6.maxZ - block6.minZ - f7 * 2.0F) + f7 + block6.minZ;
                if (i4 == TileFace.DOWN)
                {
                    d10 = i2 + block6.minY - f7;
                }

                if (i4 == TileFace.UP)
                {
                    d10 = i2 + block6.maxY + f7;
                }

                if (i4 == TileFace.NORTH)
                {
                    d12 = i3 + block6.minZ - f7;
                }

                if (i4 == TileFace.SOUTH)
                {
                    d12 = i3 + block6.maxZ + f7;
                }

                if (i4 == TileFace.WEST)
                {
                    d8 = i1 + block6.minX - f7;
                }

                if (i4 == TileFace.EAST)
                {
                    d8 = i1 + block6.maxX + f7;
                }

                this.AddEffect((new EntityDiggingFX(this.worldObj, d8, d10, d12, 0.0D, 0.0D, 0.0D, block6, i4, this.worldObj.GetData(i1, i2, i3))).Func_4041_a(i1, i2, i3).Func_407_b(0.2F).Func_405_d(0.6F));
            }
        }

        public string getStatistics()
        {
            return "" + (this.fxLayers[0].Count + this.fxLayers[1].Count + this.fxLayers[2].Count);
        }
    }
}
