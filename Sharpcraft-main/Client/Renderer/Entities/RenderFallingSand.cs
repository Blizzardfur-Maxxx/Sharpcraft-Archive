using LWCSGL.OpenGL;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.GameLevel.Tiles;
using SharpCraft.Core.World.GameLevel;

namespace SharpCraft.Client.Renderer.Entities
{
    public class RenderFallingSand : Render<FallingTile>
    {
        private TileRenderer field_197_d = new TileRenderer();

        public RenderFallingSand()
        {
            this.shadowSize = 0.5F;
        }

        public override void DoRender(FallingTile entityFallingSand1, double d2, double d4, double d6, float f8, float f9)
        {
            GL11.glPushMatrix();
            GL11.glTranslatef((float)d2, (float)d4, (float)d6);
            this.LoadTexture("/terrain.png");
            Tile block10 = Tile.tiles[entityFallingSand1.blockID];
            Level world11 = entityFallingSand1.GetWorld();
            GL11.glDisable(GL11C.GL_LIGHTING);
            this.field_197_d.RenderBlockFallingSand(block10, world11, Mth.Floor(entityFallingSand1.x), Mth.Floor(entityFallingSand1.y), Mth.Floor(entityFallingSand1.z));
            GL11.glEnable(GL11C.GL_LIGHTING);
            GL11.glPopMatrix();
        }

    }
}