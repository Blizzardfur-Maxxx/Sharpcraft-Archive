using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.Entities.Players;
using SharpCraft.Core.World.GameLevel.Materials;
using SharpCraft.Core.World.Items;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.GameLevel.Tiles
{
    public class LeafTile : HalfTransparentTile
    {
        private int baseIndexInPNG;
        int[] adjacentTreeBlocks;
        public LeafTile(int i1, int i2) : base(i1, i2, Material.leaves, false)
        {
            this.baseIndexInPNG = i2;
            this.SetTicking(true);
        }

        public override int GetColor(int i1)
        {
            return (i1 & 1) == 1 ? FoliageColor.GetFoliageColorPine() : ((i1 & 2) == 2 ? FoliageColor.GetFoliageColorBirch() : FoliageColor.GetItemColor());
        }

        public override int GetColor(ILevelSource iBlockAccess1, int i2, int i3, int i4)
        {
            int i5 = iBlockAccess1.GetData(i2, i3, i4);
            if ((i5 & 1) == 1)
            {
                return FoliageColor.GetFoliageColorPine();
            }
            else if ((i5 & 2) == 2)
            {
                return FoliageColor.GetFoliageColorBirch();
            }
            else
            {
                iBlockAccess1.GetBiomeSource().Func_a(i2, i4, 1, 1);
                double temp = iBlockAccess1.GetBiomeSource().temperature[0];
                double humd = iBlockAccess1.GetBiomeSource().humidity[0];
                return FoliageColor.GetColor(temp, humd);
            }
        }

        public override void OnBlockRemoval(Level world1, int i2, int i3, int i4)
        {
            byte b5 = 1;
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
                                world1.SetDataNoUpdate(i2 + i7, i3 + i8, i4 + i9, i11 | 8);
                            }
                        }
                    }
                }
            }
        }

        public override void Tick(Level world1, int i2, int i3, int i4, JRandom random5)
        {
            if (!world1.isRemote)
            {
                int i6 = world1.GetData(i2, i3, i4);
                if ((i6 & 8) != 0)
                {
                    byte b7 = 4;
                    int i8 = b7 + 1;
                    byte b9 = 32;
                    int i10 = b9 * b9;
                    int i11 = b9 / 2;
                    if (this.adjacentTreeBlocks == null)
                    {
                        this.adjacentTreeBlocks = new int[b9 * b9 * b9];
                    }

                    int i12;
                    if (world1.CheckChunksExist(i2 - i8, i3 - i8, i4 - i8, i2 + i8, i3 + i8, i4 + i8))
                    {
                        i12 = -b7;
                        bool break111 = false;
                    //label111:
                        while (true)
                        {
                            int i13;
                            int i14;
                            int i15;
                            if (i12 > b7)
                            {
                                i12 = 1;
                                while (true)
                                {
                                    if (i12 > 4)
                                    {
                                        //break label111;
                                        break111 = true;
                                        break;
                                    }

                                    for (i13 = -b7; i13 <= b7; ++i13)
                                    {
                                        for (i14 = -b7; i14 <= b7; ++i14)
                                        {
                                            for (i15 = -b7; i15 <= b7; ++i15)
                                            {
                                                if (this.adjacentTreeBlocks[(i13 + i11) * i10 + (i14 + i11) * b9 + i15 + i11] == i12 - 1)
                                                {
                                                    if (this.adjacentTreeBlocks[(i13 + i11 - 1) * i10 + (i14 + i11) * b9 + i15 + i11] == -2)
                                                    {
                                                        this.adjacentTreeBlocks[(i13 + i11 - 1) * i10 + (i14 + i11) * b9 + i15 + i11] = i12;
                                                    }

                                                    if (this.adjacentTreeBlocks[(i13 + i11 + 1) * i10 + (i14 + i11) * b9 + i15 + i11] == -2)
                                                    {
                                                        this.adjacentTreeBlocks[(i13 + i11 + 1) * i10 + (i14 + i11) * b9 + i15 + i11] = i12;
                                                    }

                                                    if (this.adjacentTreeBlocks[(i13 + i11) * i10 + (i14 + i11 - 1) * b9 + i15 + i11] == -2)
                                                    {
                                                        this.adjacentTreeBlocks[(i13 + i11) * i10 + (i14 + i11 - 1) * b9 + i15 + i11] = i12;
                                                    }

                                                    if (this.adjacentTreeBlocks[(i13 + i11) * i10 + (i14 + i11 + 1) * b9 + i15 + i11] == -2)
                                                    {
                                                        this.adjacentTreeBlocks[(i13 + i11) * i10 + (i14 + i11 + 1) * b9 + i15 + i11] = i12;
                                                    }

                                                    if (this.adjacentTreeBlocks[(i13 + i11) * i10 + (i14 + i11) * b9 + (i15 + i11 - 1)] == -2)
                                                    {
                                                        this.adjacentTreeBlocks[(i13 + i11) * i10 + (i14 + i11) * b9 + (i15 + i11 - 1)] = i12;
                                                    }

                                                    if (this.adjacentTreeBlocks[(i13 + i11) * i10 + (i14 + i11) * b9 + i15 + i11 + 1] == -2)
                                                    {
                                                        this.adjacentTreeBlocks[(i13 + i11) * i10 + (i14 + i11) * b9 + i15 + i11 + 1] = i12;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    ++i12;
                                }

                                if (break111)
                                    break;
                            }

                            for (i13 = -b7; i13 <= b7; ++i13)
                            {
                                for (i14 = -b7; i14 <= b7; ++i14)
                                {
                                    i15 = world1.GetTile(i2 + i12, i3 + i13, i4 + i14);
                                    if (i15 == Tile.treeTrunk.id)
                                    {
                                        this.adjacentTreeBlocks[(i12 + i11) * i10 + (i13 + i11) * b9 + i14 + i11] = 0;
                                    }
                                    else if (i15 == Tile.leaves.id)
                                    {
                                        this.adjacentTreeBlocks[(i12 + i11) * i10 + (i13 + i11) * b9 + i14 + i11] = -2;
                                    }
                                    else
                                    {
                                        this.adjacentTreeBlocks[(i12 + i11) * i10 + (i13 + i11) * b9 + i14 + i11] = -1;
                                    }
                                }
                            }

                            ++i12;
                        }
                    }

                    i12 = this.adjacentTreeBlocks[i11 * i10 + i11 * b9 + i11];
                    if (i12 >= 0)
                    {
                        world1.SetDataNoUpdate(i2, i3, i4, i6 & -9);
                    }
                    else
                    {
                        this.RemoveLeaves(world1, i2, i3, i4);
                    }
                }
            }
        }

        private void RemoveLeaves(Level world1, int i2, int i3, int i4)
        {
            this.DropBlockAsItem(world1, i2, i3, i4, world1.GetData(i2, i3, i4));
            world1.SetTile(i2, i3, i4, 0);
        }

        public override int ResourceCount(JRandom random1)
        {
            return random1.NextInt(20) == 0 ? 1 : 0;
        }

        public override int GetResource(int i1, JRandom random2)
        {
            return Tile.sapling.id;
        }

        public override void HarvestBlock(Level world1, Player entityPlayer2, int i3, int i4, int i5, int i6)
        {
            if (!world1.isRemote && entityPlayer2.GetCurrentEquippedItem() != null && entityPlayer2.GetCurrentEquippedItem().itemID == Item.shears.id)
            {
                entityPlayer2.AddStat(StatList.mineBlockStatArray[this.id], 1);
                this.SpawnItem(world1, i3, i4, i5, new ItemInstance(Tile.leaves.id, 1, i6 & 3));
            }
            else
            {
                base.HarvestBlock(world1, entityPlayer2, i3, i4, i5, i6);
            }
        }

        protected override int GetSpawnResourcesAuxValue(int i1)
        {
            return i1 & 3;
        }

        public override bool IsSolidRender()
        {
            return !this.graphicsLevel;
        }

        public override int GetTexture(TileFace faceIdx, int i2)
        {
            return (i2 & 3) == 1 ? this.texture + 80 : this.texture;
        }

        public virtual void SetGraphicsLevel(bool z1)
        {
            this.graphicsLevel = z1;
            this.texture = this.baseIndexInPNG + (z1 ? 0 : 1);
        }

        public override void StepOn(Level world1, int i2, int i3, int i4, Entity entity5)
        {
            base.StepOn(world1, i2, i3, i4, entity5);
        }
    }
}