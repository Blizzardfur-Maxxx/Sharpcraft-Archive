using System;

namespace SharpCraft.Core.Util
{
    public class ArrayUtil
    {
        private static void RangeCheck(int arrayLength, int fromIndex, int toIndex)
        {
            if (fromIndex > toIndex)
            {
                throw new ArgumentException(
                        "fromIndex(" + fromIndex + ") > toIndex(" + toIndex + ")");
            }
            if (fromIndex < 0)
            {
                throw new IndexOutOfRangeException(fromIndex.ToString());
            }
            if (toIndex > arrayLength)
            {
                throw new IndexOutOfRangeException(toIndex.ToString());
            }
        }

        public static void Fill<T>(T[] array, int from, int to, T value)
        {
            RangeCheck(array.Length, from, to);
            for (int i = from; i < to; i++)
            { array[i] = value; }
        }

        public static void Init2DArray<T>(T[][] array, int size)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = new T[size];
        }
    }
}
