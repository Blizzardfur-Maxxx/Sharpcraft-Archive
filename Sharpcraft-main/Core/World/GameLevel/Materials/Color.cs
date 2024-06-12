namespace SharpCraft.Core.World.GameLevel.Materials
{
    public class Color
    {
        public static readonly Color[] colors = new Color[16];
        public static readonly Color air = new Color(0, 0);
        public static readonly Color grass = new Color(1, 8368696);
        public static readonly Color sand = new Color(2, 16247203);
        public static readonly Color cloth = new Color(3, 10987431);
        public static readonly Color lava = new Color(4, 16711680);
        public static readonly Color ice = new Color(5, 10526975);
        public static readonly Color metal = new Color(6, 10987431);
        public static readonly Color foliage = new Color(7, 31744);
        public static readonly Color snow = new Color(8, 0xFFFFFF);
        public static readonly Color clay = new Color(9, 10791096);
        public static readonly Color dirt = new Color(10, 12020271);
        public static readonly Color stone = new Color(11, 7368816);
        public static readonly Color water = new Color(12, 4210943);
        public static readonly Color wood = new Color(13, 6837042);
        public readonly int rgb;
        public readonly int id;

        private Color(int id, int rgb)
        {
            this.id = id;
            this.rgb = rgb;
            colors[id] = this;
        }
    }
}
