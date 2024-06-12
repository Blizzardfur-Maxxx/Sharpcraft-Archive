using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.Entities.Animals;
using SharpCraft.Core.World.Entities.Items;
using SharpCraft.Core.World.Entities.Monsters;
using SharpCraft.Core.World.Entities.Projectile;
using SharpCraft.Core.World.GameLevel;
using System;

namespace SharpCraft.Core.World.Entities
{
    //it shits out entities
    public class EntityFactory
    {
        private static NullDictionary<string, Type> stringToClassMapping = new NullDictionary<string, Type>();
        private static NullDictionary<Type, string> classToStringMapping = new NullDictionary<Type, string>();
        private static NullDictionary<int, Type> IDtoClassMapping = new NullDictionary<int, Type>();
        private static NullDictionary<Type, int> classToIDMapping = new NullDictionary<Type, int>();
        private static void AddMapping(Type class1, string s, int i)
        {
            stringToClassMapping[s] = class1;
            classToStringMapping[class1] = s;
            IDtoClassMapping[i] = class1;
            classToIDMapping[class1] = i;
        }

        static EntityFactory()
        {
            AddMapping(typeof(Arrow), "Arrow", 10);
            AddMapping(typeof(Snowball), "Snowball", 11);
            AddMapping(typeof(ItemEntity), "Item", 1);
            AddMapping(typeof(Painting), "Painting", 9);
            AddMapping(typeof(Mob), "Mob", 48);
            AddMapping(typeof(Monster), "Monster", 49);
            AddMapping(typeof(Creeper), "Creeper", 50);
            AddMapping(typeof(Skeleton), "Skeleton", 51);
            AddMapping(typeof(Spider), "Spider", 52);
            AddMapping(typeof(Giant), "Giant", 53);
            AddMapping(typeof(Zombie), "Zombie", 54);
            AddMapping(typeof(Slime), "Slime", 55);
            AddMapping(typeof(Ghast), "Ghast", 56);
            AddMapping(typeof(PigZombie), "PigZombie", 57);
            AddMapping(typeof(Pig), "Pig", 90);
            AddMapping(typeof(Sheep), "Sheep", 91);
            AddMapping(typeof(Cow), "Cow", 92);
            AddMapping(typeof(Chicken), "Chicken", 93);
            AddMapping(typeof(Squid), "Squid", 94);
            AddMapping(typeof(Wolf), "Wolf", 95);
            AddMapping(typeof(PrimedTnt), "PrimedTnt", 20);
            AddMapping(typeof(FallingTile), "FallingSand", 21);
            AddMapping(typeof(Minecart), "Minecart", 40);
            AddMapping(typeof(Boat), "Boat", 41);
        }

        public static Entity CreateEntity(string string0, Level world1)
        {
            Entity entity2 = null;
            try
            {
                Type class3 = stringToClassMapping[string0];
                if (class3 != null)
                {
                    entity2 = class3.GetConstructor(new Type[] { typeof(Level) }).Invoke(new object[] { world1 }) as Entity;
                }
            }
            catch (Exception exception4)
            {
                exception4.PrintStackTrace();
            }

            return entity2;
        }

        public static Entity LoadEntity(CompoundTag nBTTagCompound0, Level world1)
        {
            Entity entity2 = null;
            try
            {
                Type class3 = stringToClassMapping[nBTTagCompound0.GetString("id")];
                if (class3 != null)
                {
                    entity2 = class3.GetConstructor(new Type[] { typeof(Level) }).Invoke(new object[] { world1 }) as Entity;
                }
            }
            catch (Exception exception4)
            {
                exception4.PrintStackTrace();
            }

            if (entity2 != null)
            {
                entity2.ReadFromNBT(nBTTagCompound0);
            }
            else
            {
                System.Console.WriteLine("Skipping Entity with id " + nBTTagCompound0.GetString("id"));
            }

            return entity2;
        }

        public static Entity CreateEntity(int i0, Level world1)
        {
            Entity entity2 = null;
            try
            {
                Type class3 = IDtoClassMapping[i0];
                if (class3 != null)
                {
                    entity2 = class3.GetConstructor(new Type[] { typeof(Level) }).Invoke(new object[] { world1 }) as Entity;
                }
            }
            catch (Exception exception4)
            {
                exception4.PrintStackTrace();
            }

            if (entity2 == null)
            {
                System.Console.WriteLine("Skipping Entity with id " + i0);
            }

            return entity2;
        }

        public static int GetNetworkId(Entity entity0)
        {
            return classToIDMapping[entity0.GetType()];
        }

        public static string GetEncodeId(Entity entity0)
        {
            return classToStringMapping[entity0.GetType()];
        }
    }
}