namespace SharpCraft.Core.World.GameLevel
{
    public class FoliageColor
    {
        private static int[] foliageBuffer = new int[65536];
        public static void SetColorBuffer(int[] i0)
        {
            foliageBuffer = i0;
        }

        public static int GetColor(double temperature, double humidity)
        {
            humidity *= temperature;
            int x = (int)((1 - temperature) * 255);
            int z = (int)((1 - humidity) * 255);
            return foliageBuffer[z << 8 | x];
        }

        public static int GetFoliageColorPine()
        {
            return 0x619961;
        }

        public static int GetFoliageColorBirch()
        {
            return 0x80A755;
        }

        public static int GetItemColor()
        {
            return 0x48B518;
        }
    }
}