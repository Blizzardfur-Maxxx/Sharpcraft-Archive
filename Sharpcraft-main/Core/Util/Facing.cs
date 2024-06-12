namespace SharpCraft.Core.Util
{
    public class Facing
    {
        public enum TileFace : byte
        {
            DOWN = 0,
            UP = 1,
            NORTH = 2,
            SOUTH = 3,
            WEST = 4,
            EAST = 5,

            //amount of facings
            COUNT = 6,
            //bitmask to cover all facings
            MASK = 0x7,
            //value for undefined facings
            UNDEFINED = 255
        }

        public static TileFace opposite(TileFace facing) 
        {
            return (TileFace)(((byte)facing & ~1) | ((byte)facing & 1) ^ 1);
        }

        public static Direction.Directions GetDirection(TileFace face) 
        {
            switch (face) 
            {
                case TileFace.WEST: return Direction.Directions.WEST;
                case TileFace.EAST: return Direction.Directions.EAST;
                case TileFace.NORTH: return Direction.Directions.NORTH;
                case TileFace.SOUTH: return Direction.Directions.SOUTH;
                default: return Direction.Directions.UNDEFINED;
            }
        }

        public static int xOffset(TileFace face) 
        {
            switch (face) 
            {
                case TileFace.WEST: return -1;
                case TileFace.EAST: return 1;
                default: return 0;
            }
        }

        public static int yOffset(TileFace face)
        {
            switch (face)
            {
                case TileFace.DOWN: return -1;
                case TileFace.UP: return 1;
                default: return 0;
            }
        }

        public static int zOffset(TileFace face)
        {
            switch (face)
            {
                case TileFace.NORTH: return -1;
                case TileFace.SOUTH: return 1;
                default: return 0;
            }
        }
    }
}
