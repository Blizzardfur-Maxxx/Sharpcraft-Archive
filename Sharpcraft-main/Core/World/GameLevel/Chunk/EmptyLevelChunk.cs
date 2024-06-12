using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities;
using SharpCraft.Core.World.GameLevel.Tiles.TileEntities;
using SharpCraft.Core.World.Phys;
using System;
using System.Collections.Generic;

namespace SharpCraft.Core.World.GameLevel.Chunk
{
    public class EmptyLevelChunk : LevelChunk
    {
        public EmptyLevelChunk(Level world1, int i2, int i3) : base(world1, i2, i3)
        {
            this.neverSave = true;
        }

        public EmptyLevelChunk(Level world1, byte[] b2, int i3, int i4) : base(world1, b2, i3, i4)
        {
            this.neverSave = true;
        }

        public override bool IsAtLocation(int i1, int i2)
        {
            return i1 == this.xPosition && i2 == this.zPosition;
        }

        public override int GetHeightValue(int i1, int i2)
        {
            return 0;
        }

        public override void Func_a()
        {
        }

        public override void GenerateHeightMap()
        {
        }

        public override void CalculateLight()
        {
        }

        public override void Func_4143()
        {
        }

        public override int GetBlockID(int i1, int i2, int i3)
        {
            return 0;
        }

        public override bool SetBlockIDWithMetadata(int i1, int i2, int i3, int i4, int i5)
        {
            return true;
        }

        public override bool SetBlockID(int i1, int i2, int i3, int i4)
        {
            return true;
        }

        public override int GetBlockMetadata(int i1, int i2, int i3)
        {
            return 0;
        }

        public override void SetBlockMetadata(int i1, int i2, int i3, int i4)
        {
        }

        public override int GetSavedLightValue(LightLayer enumSkyBlock1, int i2, int i3, int i4)
        {
            return 0;
        }

        public override void SetLightValue(LightLayer enumSkyBlock1, int i2, int i3, int i4, int i5)
        {
        }

        public override int IsSkyLit(int i1, int i2, int i3, int i4)
        {
            return 0;
        }

        public override void AddEntity(Entity entity1)
        {
        }

        public override void RemoveEntity(Entity entity1)
        {
        }

        public override void RemoveEntityAtIndex(Entity entity1, int i2)
        {
        }

        public override bool CanBlockSeeTheSky(int i1, int i2, int i3)
        {
            return false;
        }

        public override TileEntity GetChunkBlockTileEntity(int i1, int i2, int i3)
        {
            return null;
        }

        public override void AddTileEntity(TileEntity tileEntity1)
        {
        }

        public override void SetChunkBlockTileEntity(int i1, int i2, int i3, TileEntity tileEntity4)
        {
        }

        public override void RemoveChunkBlockTileEntity(int i1, int i2, int i3)
        {
        }

        public override void OnChunkLoad()
        {
        }

        public override void OnChunkUnload()
        {
        }

        public override void SetChunkModified()
        {
        }

        public override void GetEntitiesWithinAABBForEntity(Entity entity1, AABB axisAlignedBB2, IList<Entity> list3)
        {
        }

        public override void GetEntitiesOfTypeWithinAAAB<E>(Type type, AABB aabb, List<E> entities)
        {
        }

        public override bool NeedsSaving(bool z1)
        {
            return false;
        }

        public override int SetChunkData(byte[] b1, int i2, int i3, int i4, int i5, int i6, int i7, int i8)
        {
            int i9 = i5 - i2;
            int i10 = i6 - i3;
            int i11 = i7 - i4;
            int i12 = i9 * i10 * i11;
            return i12 + i12 / 2 * 3;
        }

        public override int GetChunkData(byte[] b1, int i2, int i3, int i4, int i5, int i6, int i7, int i8)
        {
            int i9 = i5 - i2;
            int i10 = i6 - i3;
            int i11 = i7 - i4;
            int i12 = i9 * i10 * i11;
            int i13 = i12 + i12 / 2 * 3;
            ArrayUtil.Fill(b1, i8, i8 + i13, (byte)0);
            return i13;
        }

        public override JRandom GetRandom(long j1)
        {
            return new JRandom(this.worldObj.GetRandomSeed() + this.xPosition * this.xPosition * 4987142 + this.xPosition * 5947611 + this.zPosition * this.zPosition * 4392871 + this.zPosition * 389711 ^ j1);
        }

        public override bool IsEmpty()
        {
            return true;
        }
    }
}