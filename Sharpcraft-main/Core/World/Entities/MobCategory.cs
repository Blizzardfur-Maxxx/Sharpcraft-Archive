using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Monsters;
using SharpCraft.Core.World.GameLevel.Materials;
using System;

namespace SharpCraft.Core.World.Entities
{
    public class MobCategory
    {
        private static int counter;
        private static readonly MobCategory[] values = new MobCategory[3];

        public static readonly MobCategory 
            monster = new MobCategory(typeof(IEnemy), 70, Material.air, false),
            creature = new MobCategory(typeof(Animal), 15, Material.air, true),
            waterCreature = new MobCategory(typeof(WaterCreature), 5, Material.water, true);

        private readonly Type mobClass;
        private readonly int maxCount;
        private readonly Material creatureMaterial;
        private readonly bool isPeacefulCreature;

        private MobCategory(Type mobClass, int max, Material material, bool peaceful)
        {
            this.mobClass = mobClass;
            this.maxCount = max;
            this.creatureMaterial = material;
            this.isPeacefulCreature = peaceful;
            values[counter++] = this;
        }

        public Type GetMobClass()
        {
            return this.mobClass;
        }

        public int GetMaxSpawnedCount()
        {
            return this.maxCount;
        }

        public Material GetMaterial()
        {
            return this.creatureMaterial;
        }

        public bool IsPeacefulMob()
        {
            return this.isPeacefulCreature;
        }

        public static MobCategory[] Values()
        {
            return values;
        }
    }
}