using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using SharpCraft.Core.World.Phys;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class TopSnowTile : Tile
    {
        public TopSnowTile(int i1, int i2) : base(i1, i2, Material.topSnow)
        {
            this.SetShape(0F, 0F, 0F, 1F, 0.125F, 1F);
            this.SetTicking(true);
        }

        public override AABB GetAABB(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetData(i2, i3, i4) & 7;
            return i5 >= 3 ? AABB.Of(i2 + this.minX, i3 + this.minY, i4 + this.minZ, i2 + this.maxX, i3 + 0.5F, i4 + this.maxZ) : null;
        }

        public override bool IsSolidRender()
        {
            return false;
        }

        public override bool IsCubeShaped()
        {
            return false;
        }

        public override void SetBlockBoundsBasedOnState(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            int i5 = iBlockAccess1.GetData(i2, i3, i4) & 7;
            float f6 = 2 * (1 + i5) / 16F;
            this.SetShape(0F, 0F, 0F, 1F, f6, 1F);
        }

        public override bool CanPlaceBlockAt(Level world1, int i2, int i3, int i4)
        {
            int i5 = world1.GetTile(i2, i3 - 1, i4);
            return i5 != 0 && Tile.tiles[i5].IsSolidRender() ? world1.GetMaterial(i2, i3 - 1, i4).BlocksMotion() : false;
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            this.Cooc(world1, i2, i3, i4);
        }

        private bool Cooc(Level world1, int i2, int i3, int i4)
        {
            if (!this.CanPlaceBlockAt(world1, i2, i3, i4))
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
                return false;
            }
            else
            {
                return true;
            }
        }

        public override void HarvestBlock(Level world1, Player entityPlayer2, int i3, int i4, int i5, int i6)
        {
            int i7 = Item.snowball.id;
            float f8 = 0.7F;
            double d9 = (float)world1.rand.NextFloat() * f8 + (1F - f8) * 0.5;
            double d11 = (float)world1.rand.NextFloat() * f8 + (1F - f8) * 0.5;
            double d13 = (float)world1.rand.NextFloat() * f8 + (1F - f8) * 0.5;
            ItemEntity entityItem15 = new ItemEntity(world1, i3 + d9, i4 + d11, i5 + d13, new ItemInstance(i7, 1, 0));
            entityItem15.delayBeforeCanPickup = 10;
            world1.AddEntity(entityItem15);
            world1.SetTile(i3, i4, i5, 0);
            entityPlayer2.AddStat(StatList.mineBlockStatArray[this.id], 1);
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Item.snowball.id;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (world1.GetSavedLightValue(LightLayer.Block, i2, i3, i4) > 11)
            {
                this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
                world1.SetTile(i2, i3, i4, 0);
            }
        }

        public override bool ShouldRenderFace(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            return i5 == 1 ? true : base.ShouldRenderFace(iBlockAccess1, i2, i3, i4, i5);
        }
    }
}