using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Materials;
using System;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class StoneSlabTile : Tile
    {
        public static readonly String[] SLAB_NAMES = new[]
        {
            "stone",
            "sand",
            "wood",
            "cobble"
        };
        private bool blockType;
        public StoneSlabTile(int i1, bool z2) : base(i1, 6, Material.stone)
        {
            this.blockType = z2;
            if (!z2)
            {
                this.SetShape(0F, 0F, 0F, 1F, 0.5F, 1F);
            }

            this.SetLightBlock(255);
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return i2 == 0 ? (faceIdx <= TileFace.UP ? 6 : 5) : (i2 == 1 ? (faceIdx == 0 ? 208 : (faceIdx == TileFace.UP ? 176 : 192)) : (i2 == 2 ? 4 : (i2 == 3 ? 16 : 6)));
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return this.GetTexture(faceIdx, 0);
        }

        public override bool IsSolidRender()
        {
            return this.blockType;
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            if (this != Tile.stoneSlabHalf)
            {
                base.OnPlace(world1, i2, i3, i4);
            }

            int i5 = world1.GetTile(i2, i3 - 1, i4);
            int i6 = world1.GetData(i2, i3, i4);
            int i7 = world1.GetData(i2, i3 - 1, i4);
            if (i6 == i7)
            {
                if (i5 == stoneSlabHalf.id)
                {
                    world1.SetTile(i2, i3, i4, 0);
                    world1.SetTileAndData(i2, i3 - 1, i4, Tile.stoneSlab.id, i6);
                }
            }
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.stoneSlabHalf.id;
        }

        public override int ResourceCount(JRandom random1)
        {
            return this.blockType ? 2 : 1;
        }

        protected override int GetSpawnResourcesAuxValue(int i1)
        {
            return i1;
        }

        public override bool IsCubeShaped()
        {
            return this.blockType;
        }

        public override bool ShouldRenderFace(ILevelSource iBlockAccess1, int i2, int i3, int i4, int i5)
        {
            if (this != Tile.stoneSlabHalf)
            {
                base.ShouldRenderFace(iBlockAccess1, i2, i3, i4, i5);
            }

            return i5 == 1 ? true : (!base.ShouldRenderFace(iBlockAccess1, i2, i3, i4, i5) ? false : (i5 == 0 ? true : iBlockAccess1.GetTile(i2, i3, i4) != this.id));
        }
    }
}