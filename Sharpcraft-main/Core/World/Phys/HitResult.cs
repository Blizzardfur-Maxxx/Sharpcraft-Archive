using SharpCraft.Core.World.Entities;
using static SharpCraft.Core.Util.Facing;

namespace SharpCraft.Core.World.Phys
{
    public class HitResult
    {
        public Type TypeOfHit;
        public int BlockX;
        public int BlockY;
        public int BlockZ;
        public TileFace SideHit;
        public Vec3 HitVec;
        public Entity EntityHit;

        public enum Type
        {
            TILE,
            ENTITY
        }

        public HitResult(int blockX, int blockY, int blockZ, TileFace side, Vec3 vec)
        {
            TypeOfHit = Type.TILE;
            BlockX = blockX;
            BlockY = blockY;
            BlockZ = blockZ;
            SideHit = side;
            HitVec = Vec3.Of(vec.x, vec.y, vec.z);
        }

        public HitResult(Entity entity)
        {
            TypeOfHit = Type.ENTITY;
            EntityHit = entity;
            HitVec = Vec3.Of(entity.x, entity.y, entity.z);
        }
    }
}
