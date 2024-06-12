namespace SharpCraft.Core.World.GameLevel.Materials
{
    public class Material
    {
        public static readonly Material air = new GasMaterial(Color.air);
        public static readonly Material grass = new Material(Color.grass);
        public static readonly Material dirt = new Material(Color.dirt);
        public static readonly Material wood = (new Material(Color.wood)).SetFlammable();
        public static readonly Material stone = (new Material(Color.stone)).SetNoHarvest();
        public static readonly Material metal = (new Material(Color.metal)).SetNoHarvest();
        public static readonly Material water = (new LiquidMaterial(Color.water)).SetNoPushMobility();
        public static readonly Material lava = (new LiquidMaterial(Color.lava)).SetNoPushMobility();
        public static readonly Material leaves = (new Material(Color.foliage)).SetFlammable().SetIsTranslucent().SetNoPushMobility();
        public static readonly Material plant = (new DecorationMaterial(Color.foliage)).SetNoPushMobility();
        public static readonly Material sponge = new Material(Color.cloth);
        public static readonly Material cloth = (new Material(Color.cloth)).SetFlammable();
        public static readonly Material fire = (new GasMaterial(Color.air)).SetNoPushMobility();
        public static readonly Material sand = new Material(Color.sand);
        public static readonly Material decoration = (new DecorationMaterial(Color.air)).SetNoPushMobility();
        public static readonly Material glass = (new Material(Color.air)).SetIsTranslucent();
        public static readonly Material explosive = (new Material(Color.lava)).SetFlammable().SetIsTranslucent();
        public static readonly Material coral = (new Material(Color.foliage)).SetNoPushMobility();
        public static readonly Material ice = (new Material(Color.ice)).SetIsTranslucent();
        public static readonly Material topSnow = (new DecorationMaterial(Color.snow)).SetIsGroundCover().SetIsTranslucent().SetNoHarvest().SetNoPushMobility();
        public static readonly Material snow = (new Material(Color.snow)).SetNoHarvest();
        public static readonly Material cactus = (new Material(Color.foliage)).SetIsTranslucent().SetNoPushMobility();
        public static readonly Material clay = new Material(Color.clay);
        public static readonly Material vegetable = (new Material(Color.foliage)).SetNoPushMobility();
        public static readonly Material portal = (new PortalMaterial(Color.air)).SetImmovableMobility();
        public static readonly Material cake = (new Material(Color.air)).SetNoPushMobility();
        public static readonly Material web = (new Material(Color.cloth)).SetNoHarvest().SetNoPushMobility();
        public static readonly Material piston = (new Material(Color.stone)).SetImmovableMobility();

        private bool flammable;
        private bool groundCover;
        private bool isTranslucent;
        public readonly Color color;
        private bool canHarvest = true;
        private int pushReaction;

        public Material(Color color)
        {
            this.color = color;
        }

        public virtual bool IsLiquid()
        {
            return false;
        }

        public virtual bool IsSolid()
        {
            return true;
        }

        public virtual bool BlocksLight()
        {
            return true;
        }

        public virtual bool BlocksMotion()
        {
            return true;
        }

        private Material SetIsTranslucent()
        {
            this.isTranslucent = true;
            return this;
        }

        private Material SetNoHarvest()
        {
            this.canHarvest = false;
            return this;
        }

        private Material SetFlammable()
        {
            this.flammable = true;
            return this;
        }

        public bool IsFlammable()
        {
            return this.flammable;
        }

        public Material SetIsGroundCover()
        {
            this.groundCover = true;
            return this;
        }

        public bool GetIsGroundCover()
        {
            return this.groundCover;
        }

        public bool GetIsTranslucent()
        {
            return this.isTranslucent ? false : this.BlocksMotion();
        }

        public bool GetIsHarvestable()
        {
            return this.canHarvest;
        }

        public int GetPushReaction()
        {
            return this.pushReaction;
        }

        protected Material SetNoPushMobility()
        {
            this.pushReaction = 1;
            return this;
        }

        protected Material SetImmovableMobility()
        {
            this.pushReaction = 2;
            return this;
        }
    }
}
