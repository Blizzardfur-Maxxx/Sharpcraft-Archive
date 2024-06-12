namespace SharpCraft.Core.World.Entities
{
    public class Motive
    {
        private static int counter;
        private static readonly Motive[] values = new Motive[25]; //not sure if 24 cause there might be a null value if i make it too big by accident

        public static readonly Motive
            Kebab = new Motive("Kebab", 16, 16, 0, 0),
            Aztec = new Motive("Aztec", 16, 16, 16, 0),
            Alban = new Motive("Alban", 16, 16, 32, 0),
            Aztec2 = new Motive("Aztec2", 16, 16, 48, 0),
            Bomb = new Motive("Bomb", 16, 16, 64, 0),
            Plant = new Motive("Plant", 16, 16, 80, 0),
            Wasteland = new Motive("Wasteland", 16, 16, 96, 0),
            Pool = new Motive("Pool", 32, 16, 0, 32),
            Courbet = new Motive("Courbet", 32, 16, 32, 32),
            Sea = new Motive("Sea", 32, 16, 64, 32),
            Sunset = new Motive("Sunset", 32, 16, 96, 32),
            Creebet = new Motive("Creebet", 32, 16, 128, 32),
            Wanderer = new Motive("Wanderer", 16, 32, 0, 64),
            Graham = new Motive("Graham", 16, 32, 16, 64),
            Match = new Motive("Match", 32, 32, 0, 128),
            Bust = new Motive("Bust", 32, 32, 32, 128),
            Stage = new Motive("Stage", 32, 32, 64, 128),
            Void = new Motive("Void", 32, 32, 96, 128),
            SkullAndRoses = new Motive("SkullAndRoses", 32, 32, 128, 128),
            Fighters = new Motive("Fighters", 64, 32, 0, 96),
            Pointer = new Motive("Pointer", 64, 64, 0, 192),
            Pigscene = new Motive("Pigscene", 64, 64, 64, 192),
            BurningSkull = new Motive("BurningSkull", 64, 64, 128, 192),
            Skeleton = new Motive("Skeleton", 64, 48, 192, 64),
            DonkeyKong = new Motive("DonkeyKong", 64, 48, 192, 112);

        public static readonly int maxArtTitleLength = "SkullAndRoses".Length;
        public readonly string title;
        public readonly int sizeX;
        public readonly int sizeY;
        public readonly int offsetX;
        public readonly int offsetY;

        private Motive(string string3, int i4, int i5, int i6, int i7)
        {
            this.title = string3;
            this.sizeX = i4;
            this.sizeY = i5;
            this.offsetX = i6;
            this.offsetY = i7;
            values[counter++] = this;
        }

        public static Motive[] Values()
        {
            return values;
        }
    }
}