using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class TntTile : Tile
    {
        public TntTile(int i1, int i2) : base(i1, i2, Material.explosive)
        {
        }

        public override int GetTexture(TileFace faceIdx)
        {
            return faceIdx == TileFace.DOWN ? this.texture + 2 : (faceIdx == TileFace.UP ? this.texture + 1 : this.texture);
        }

        public override void OnPlace(Level world1, int i2, int i3, int i4)
        {
            base.OnPlace(world1, i2, i3, i4);
            if (world1.IsBlockIndirectlyGettingPowered(i2, i3, i4))
            {
                this.OnBlockDestroyedByPlayer(world1, i2, i3, i4, 1);
                world1.SetTile(i2, i3, i4, 0);
            }
        }

        public override void NeighborChanged(Level world1, int i2, int i3, int i4, int i5)
        {
            if (i5 > 0 && Tile.tiles[i5].IsSignalSource() && world1.IsBlockIndirectlyGettingPowered(i2, i3, i4))
            {
                this.OnBlockDestroyedByPlayer(world1, i2, i3, i4, 1);
                world1.SetTile(i2, i3, i4, 0);
            }
        }

        public override int ResourceCount(JRandom random1)
        {
            return 0;
        }

        public override void WasExploded(Level world1, int i2, int i3, int i4)
        {
            PrimedTnt entityTNTPrimed5 = new PrimedTnt(world1, i2 + 0.5F, i3 + 0.5F, i4 + 0.5F);
            entityTNTPrimed5.fuse = world1.rand.NextInt(entityTNTPrimed5.fuse / 4) + entityTNTPrimed5.fuse / 8;
            world1.AddEntity(entityTNTPrimed5);
        }

        public override void OnBlockDestroyedByPlayer(Level world1, int i2, int i3, int i4, int i5)
        {
            if (!world1.isRemote)
            {
                if ((i5 & 1) == 0)
                {
                    this.SpawnItem(world1, i2, i3, i4, new ItemInstance(Tile.tnt.id, 1, 0));
                }
                else
                {
                    PrimedTnt entityTNTPrimed6 = new PrimedTnt(world1, i2 + 0.5F, i3 + 0.5F, i4 + 0.5F);
                    world1.AddEntity(entityTNTPrimed6);
                    world1.PlaySound(entityTNTPrimed6, "random.fuse", 1F, 1F);
                }
            }
        }

        public override void OnBlockClicked(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            if (entityPlayer5.GetCurrentEquippedItem() != null && entityPlayer5.GetCurrentEquippedItem().itemID == Item.flintAndSteel.id)
            {
                world1.SetDataNoUpdate(i2, i3, i4, 1);
            }

            base.OnBlockClicked(world1, i2, i3, i4, entityPlayer5);
        }

        public override bool BlockActivated(Level world1, int i2, int i3, int i4, Player entityPlayer5)
        {
            return base.BlockActivated(world1, i2, i3, i4, entityPlayer5);
        }
    }
}