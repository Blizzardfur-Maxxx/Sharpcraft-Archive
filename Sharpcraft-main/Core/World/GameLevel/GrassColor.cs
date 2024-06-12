namespace SharpCraft.Core.World.GameLevel
{
    public class GrassColor
    {
        private static int[] grassBuffer = new int[65536];
        public static void SetColorBuffer(int[] buffer)
        {
            grassBuffer = buffer;
        }

        public static int GetColor(double temperature, double humidity)
        {
            humidity *= temperature;
            int x = (int)((1 - temperature) * 255);
            int z = (int)((1 - humidity) * 255);
            return grassBuffer[z << 8 | x];
        }
    }
}