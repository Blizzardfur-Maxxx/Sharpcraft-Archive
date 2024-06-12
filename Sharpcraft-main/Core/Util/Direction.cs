using System.Runtime.CompilerServices;

namespace SharpCraft.Core.Util
{
    public class Direction
    {
        public enum Directions : int
        {
            SOUTH = 0,
            WEST = 1,
            NORTH = 2,
            EAST = 3,
            COUNT = 4,
            MASK = 0x3,
            UNDEFINED = 0x7
        }

        public static readonly int[] directions =       { 0, 1, 2, 3 };
        public static readonly int[] opposite =         { 2, 3, 0, 1 };
        public static readonly int[] rotations =        { 3, 4, 2, 5 };
        
        public static readonly int[,] direction = {
            { 1, 0, 3, 2, 5, 4 }, { 1, 0, 5, 4, 2, 3 },
            { 1, 0, 2, 3, 4, 5 }, { 1, 0, 4, 5, 3, 2 }
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Directions GetOpposite(Directions d) 
        {
            return (d + 2) & Directions.MASK;
        }
    }
}
