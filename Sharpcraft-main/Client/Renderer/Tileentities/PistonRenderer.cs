using LWCSGL.OpenGL;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpCraft.Client.Renderer.Tileentities
{
    public class PistonRenderer : TileEntityRenderer<PistonPieceEntity>
    {
        private TileRenderer field_31071_b;

        public override void SetLevel(Level world1)
        {
            this.field_31071_b = new TileRenderer(world1);
        }

        public override void RenderTileEntityAt(PistonPieceEntity tileEntity1, double d2, double d4, double d6, float f8)
        {
            Tile block9 = Tile.tiles[tileEntity1.GetStoredBlockID()];
            if (block9 != null && tileEntity1.Func_31008(f8) < 1.0F)
            {
                Tessellator tessellator10 = Tessellator.Instance;
                this.BindTextureByName("/terrain.png");
                Light.TurnOff();
                GL11.glBlendFunc(GL11C.GL_SRC_ALPHA, GL11C.GL_ONE_MINUS_SRC_ALPHA);
                GL11.glEnable(GL11C.GL_BLEND);
                GL11.glDisable(GL11C.GL_CULL_FACE);
                if (Client.IsAmbientOcclusionEnabled())
                {
                    GL11.glShadeModel(GL11C.GL_SMOOTH);
                }
                else
                {
                    GL11.glShadeModel(GL11C.GL_FLAT);
                }

                tessellator10.Begin();
                tessellator10.Offset((float)d2 - tileEntity1.xCoord + tileEntity1.Func_31017_b(f8), (float)d4 - tileEntity1.yCoord + tileEntity1.Func_31014_c(f8), (float)d6 - tileEntity1.zCoord + tileEntity1.Func_31013_d(f8));
                tessellator10.Color(1, 1, 1);
                if (block9 == Tile.pistonExtension && tileEntity1.Func_31008(f8) < 0.5F)
                {
                    this.field_31071_b.Func_31079_a(block9, tileEntity1.xCoord, tileEntity1.yCoord, tileEntity1.zCoord, false);
                }
                else if (tileEntity1.Func_31012_k() && !tileEntity1.Func_31015())
                {
                    Tile.pistonExtension.Func_31052_a_(((PistonBaseTile)block9).Func_31040_i());
                    this.field_31071_b.Func_31079_a(Tile.pistonExtension, tileEntity1.xCoord, tileEntity1.yCoord, tileEntity1.zCoord, tileEntity1.Func_31008(f8) < 0.5F);
                    Tile.pistonExtension.Func_31051_a();
                    tessellator10.Offset((float)d2 - tileEntity1.xCoord, (float)d4 - tileEntity1.yCoord, (float)d6 - tileEntity1.zCoord);
                    this.field_31071_b.Func_31078_d(block9, tileEntity1.xCoord, tileEntity1.yCoord, tileEntity1.zCoord);
                }
                else
                {
                    this.field_31071_b.Func_31075_a(block9, tileEntity1.xCoord, tileEntity1.yCoord, tileEntity1.zCoord);
                }

                tessellator10.Offset(0.0D, 0.0D, 0.0D);
                tessellator10.End();
                Light.TurnOn();
            }
        }
    }
}
