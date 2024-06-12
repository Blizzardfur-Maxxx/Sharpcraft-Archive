using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class TreeTrunkTile : Tile
    {
        public TreeTrunkTile(int i1) : base(i1, Material.wood)
        {
            this.texture = 20;
        }

        public override int ResourceCount(JRandom random1)
        {
            return 1;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.treeTrunk.id;
        }

        public override void HarvestBlock(Level world1, Player entityPlayer2, int i3, int i4, int i5, int i6)
        {
            base.HarvestBlock(world1, entityPlayer2, i3, i4, i5, i6);
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            byte b5 = 4;
            int i6 = b5 + 1;
            if (world1.CheckChunksExist(i2 - i6, i3 - i6, i4 - i6, i2 + i6, i3 + i6, i4 + i6))
            {
                for (int i7 = -b5; i7 <= b5; ++i7)
                {
                    for (int i8 = -b5; i8 <= b5; ++i8)
                    {
                        for (int i9 = -b5; i9 <= b5; ++i9)
                        {
                            int i10 = world1.GetTile(i2 + i7, i3 + i8, i4 + i9);
                            if (i10 == Tile.leaves.id)
                            {
                                int i11 = world1.GetData(i2 + i7, i3 + i8, i4 + i9);
                                if ((i11 & 8) == 0)
                                {
                                    world1.SetDataNoUpdate(i2 + i7, i3 + i8, i4 + i9, i11 | 8);
                                }
                            }
                        }
                    }
                }
            }
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return faceIdx == TileFace.UP ? 21 : (faceIdx == TileFace.DOWN ? 21 : (i2 == 1 ? 116 : (i2 == 2 ? 117 : 20)));
        }

        protected override int GetSpawnResourcesAuxValue(int i1)
        {
            return i1;
        }
    }
}