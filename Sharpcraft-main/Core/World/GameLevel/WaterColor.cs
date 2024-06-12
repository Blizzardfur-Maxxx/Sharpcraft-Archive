namespace SharpCraft.Core.World.GameLevel
{
    public class WaterColor
    {
        private static int[] waterBuffer = new int[65536];
        public static void SetColorBuffer(int[] buffer)
        {
            waterBuffer = buffer;
        }

        public static int GetColor(double temperature, double humidity)
        {
            humidity *= temperature;
            int x = (int)((1 - temperature) * 255);
            int z = (int)((1 - humidity) * 255);
            return waterBuffer[z << 8 | x];
        }
    }
}