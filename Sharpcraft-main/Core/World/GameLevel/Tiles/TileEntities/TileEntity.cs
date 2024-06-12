using SharpCraft.Core.NBT;
using SharpCraft.Core.Network.Packets;
using SharpCraft.Core.Util;
using System;

namespace SharpCraft.Core.World.GameLevel.Tiles.TileEntities
{
    public class TileEntity
    {
        private static NullDictionary<string, Type> nameToClassMap = new NullDictionary<string, Type>();
        private static NullDictionary<Type, string> classToNameMap = new NullDictionary<Type, string>();
        public Level worldObj;
        public int xCoord;
        public int yCoord;
        public int zCoord;
        protected bool tileEntityInvalid;

        private static void AddMapping(Type class0, string string1)
        {
            if (classToNameMap.ContainsKey(class0))
            {
                throw new ArgumentException("Duplicate id: " + string1);
            }
            else
            {
                nameToClassMap[string1] = class0;
                classToNameMap[class0] = string1;
            }
        }

        public virtual void Load(CompoundTag nBTTagCompound1)
        {
            this.xCoord = nBTTagCompound1.GetInteger("x");
            this.yCoord = nBTTagCompound1.GetInteger("y");
            this.zCoord = nBTTagCompound1.GetInteger("z");
        }

        public virtual void Save(CompoundTag nBTTagCompound1)
        {
            string string2 = classToNameMap[this.GetType()];
            if (string2 == null)
            {
                throw new Exception(this.GetType() + " is missing a mapping! This is a bug!");
            }
            else
            {
                nBTTagCompound1.SetString("id", string2);
                nBTTagCompound1.SetInteger("x", this.xCoord);
                nBTTagCompound1.SetInteger("y", this.yCoord);
                nBTTagCompound1.SetInteger("z", this.zCoord);
            }
        }

        public virtual void UpdateEntity()
        {
        }

        public static TileEntity CreateAndLoadEntity(CompoundTag nBTTagCompound0)
        {
            TileEntity tileEntity1 = null;
            try
            {
                Type class2 = nameToClassMap[nBTTagCompound0.GetString("id")];
                if (class2 != null)
                {
                    tileEntity1 = (TileEntity)Activator.CreateInstance(class2);
                }
            }
            catch (Exception exception3)
            {
                exception3.PrintStackTrace();
            }

            if (tileEntity1 != null)
            {
                tileEntity1.Load(nBTTagCompound0);
            }
            else
            {
                Console.WriteLine("Skipping TileEntity with id " + nBTTagCompound0.GetString("id"));
            }

            return tileEntity1;
        }

        public virtual int GetBlockMetadata()
        {
            return this.worldObj.GetData(this.xCoord, this.yCoord, this.zCoord);
        }

        public virtual void SetChanged()
        {
            if (this.worldObj != null)
            {
                this.worldObj.TileEntityChanged(this.xCoord, this.yCoord, this.zCoord, this);
            }
        }

        public virtual Packet GetDescriptionPacket()
        {
            return null;
        }

        public virtual double GetDistanceFrom(double d1, double d3, double d5)
        {
            double d7 = this.xCoord + 0.5 - d1;
            double d9 = this.yCoord + 0.5 - d3;
            double d11 = this.zCoord + 0.5 - d5;
            return d7 * d7 + d9 * d9 + d11 * d11;
        }

        public virtual Tile GetBlockType()
        {
            return Tile.tiles[this.worldObj.GetTile(this.xCoord, this.yCoord, this.zCoord)];
        }

        public virtual bool IsInvalid()
        {
            return this.tileEntityInvalid;
        }

        public virtual void Invalidate()
        {
            this.tileEntityInvalid = true;
        }

        public virtual void Validate()
        {
            this.tileEntityInvalid = false;
        }

        static TileEntity()
        {
            AddMapping(typeof(TileEntityFurnace), "Furnace");
            AddMapping(typeof(TileEntityChest), "Chest");
            AddMapping(typeof(TileEntityRecordPlayer), "RecordPlayer");
            AddMapping(typeof(TileEntityDispenser), "Trap");
            AddMapping(typeof(TileEntitySign), "Sign");
            AddMapping(typeof(TileEntityMobSpawner), "MobSpawner");
            AddMapping(typeof(TileEntityNote), "Music");
            AddMapping(typeof(PistonPieceEntity), "Piston");
        }
    }
}