using SharpCraft.Core.Util;
using SharpCraft.Core.World.Items;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCraft.Core.World.Entities
{
    public class SynchedEntityData
    {
        public class DataItem
        {
            private readonly int objectType;
            private readonly int dataValueId;
            private object watchedObject;
            private bool isWatching;
            public DataItem(int i1, int i2, object object3)
            {
                this.dataValueId = i2;
                this.watchedObject = object3;
                this.objectType = i1;
                this.isWatching = true;
            }

            public virtual int GetDataValueId()
            {
                return this.dataValueId;
            }

            public virtual void SetObject(object object1)
            {
                this.watchedObject = object1;
            }

            public virtual object GetObject()
            {
                return this.watchedObject;
            }

            public virtual int GetObjectType()
            {
                return this.objectType;
            }

            public virtual bool GetWatching()
            {
                return this.isWatching;
            }

            public virtual void SetWatching(bool z1)
            {
                this.isWatching = z1;
            }
        }

        private static readonly NullDictionary<Type, int> dataTypes = new NullDictionary<Type, int>();
        private readonly NullDictionary<int, DataItem> watchedObjects = new NullDictionary<int, DataItem>();
        private bool objectChanged;
        public virtual void AddObject(int i1, object object2)
        {
            Type type = object2.GetType();
            if (type == typeof(byte)) throw new ArgumentException("Use SBYTE!");
            int i3 = dataTypes[type];
            if (i3 > 6 || i3 < 0)
            {
                throw new ArgumentException("Unknown data type: " + object2.GetType());
            }
            else if (i1 > 31)
            {
                throw new ArgumentException("Data value id is too big with " + i1 + "! (Max is " + 31 + ")");
            }
            else if (this.watchedObjects.ContainsKey(i1))
            {
                throw new ArgumentException("Duplicate id value for " + i1 + "!");
            }
            else
            {
                DataItem watchableObject4 = new DataItem(i3, i1, object2);
                this.watchedObjects[i1] = watchableObject4;
            }
        }

        public virtual sbyte GetWatchableObjectByte(int i1)
        {
            return unchecked((sbyte)this.watchedObjects[i1].GetObject());
        }

        public virtual int GetWatchableObjectInt(int i1)
        {
            return (int)this.watchedObjects[i1].GetObject();
        }

        public virtual string GetWatchableObjectString(int i1)
        {
            return (string)this.watchedObjects[i1].GetObject();
        }

        public virtual void UpdateObject(int i1, object newObj)
        {
            DataItem item = this.watchedObjects[i1];

            if (newObj.GetType() == typeof(byte))
                throw new ArgumentException("Use SBYTE!");

            if (!newObj.Equals(item.GetObject()))
            {
                item.SetObject(newObj);
                item.SetWatching(true);
                this.objectChanged = true;
            }
        }

        public virtual bool HasObjectChanged()
        {
            return this.objectChanged;
        }

        public static void WriteObjectsInListToStream(IList<DataItem> list0, BinaryWriter dataOutputStream1)
        {
            if (list0 != null)
            {
                IEnumerator<DataItem> iterator2 = list0.GetEnumerator();
                while (iterator2.MoveNext())
                {
                    DataItem watchableObject3 = iterator2.Current;
                    WriteWatchableObject(dataOutputStream1, watchableObject3);
                }
            }

            dataOutputStream1.Write((sbyte)127);
        }

        public virtual List<DataItem> GetChangedObjects()
        {
            List<DataItem> arrayList1 = null;
            if (this.objectChanged)
            {
                IEnumerator<DataItem> iterator2 = this.watchedObjects.Values.GetEnumerator();
                while (iterator2.MoveNext())
                {
                    DataItem watchableObject3 = iterator2.Current;
                    if (watchableObject3.GetWatching())
                    {
                        watchableObject3.SetWatching(false);
                        if (arrayList1 == null)
                        {
                            arrayList1 = new List<DataItem>();
                        }

                        arrayList1.Add(watchableObject3);
                    }
                }
            }

            this.objectChanged = false;
            return arrayList1;
        }

        public virtual void WriteWatchableObjects(BinaryWriter dataOutputStream1)
        {
            IEnumerator<DataItem> iterator2 = this.watchedObjects.Values.GetEnumerator();
            while (iterator2.MoveNext())
            {
                DataItem watchableObject3 = iterator2.Current;
                WriteWatchableObject(dataOutputStream1, watchableObject3);
            }

            //we have to explicitly cast everything cause the binary writer has overloads for the data types
            dataOutputStream1.Write((sbyte)127);
        }

        private static void WriteWatchableObject(BinaryWriter dataOutputStream0, DataItem watchableObject1)
        {
            int i2 = (watchableObject1.GetObjectType() << 5 | watchableObject1.GetDataValueId() & 31) & 255;
            dataOutputStream0.Write((sbyte)i2);
            switch (watchableObject1.GetObjectType())
            {
                case 0:
                    dataOutputStream0.Write(((sbyte)watchableObject1.GetObject()));
                    break;
                case 1:
                    dataOutputStream0.WriteBEShort(((short)watchableObject1.GetObject()));
                    break;
                case 2:
                    dataOutputStream0.WriteBEInt(((int)watchableObject1.GetObject()));
                    break;
                case 3:
                    dataOutputStream0.WriteBEFloat(((float)watchableObject1.GetObject()));
                    break;
                case 4:
                    dataOutputStream0.WriteMUTF8((string)watchableObject1.GetObject());
                    break;
                case 5:
                    ItemInstance itemStack4 = (ItemInstance)watchableObject1.GetObject();
                    dataOutputStream0.WriteBEShort((short)itemStack4.GetItem().id);
                    dataOutputStream0.Write((sbyte)itemStack4.stackSize);
                    dataOutputStream0.WriteBEShort((short)itemStack4.GetItemDamage());
                    break;
                case 6:
                    Pos chunkCoordinates3 = (Pos)watchableObject1.GetObject();
                    dataOutputStream0.WriteBEInt(chunkCoordinates3.x);
                    dataOutputStream0.WriteBEInt(chunkCoordinates3.y);
                    dataOutputStream0.WriteBEInt(chunkCoordinates3.z);
                    break;
            }
        }

        public static IList<DataItem> ReadWatchableObjects(BinaryReader dataInputStream0)
        {
            List<DataItem> arrayList1 = null;
            for (sbyte b2 = dataInputStream0.ReadSByte(); b2 != 127; b2 = dataInputStream0.ReadSByte())
            {
                if (arrayList1 == null)
                {
                    arrayList1 = new List<DataItem>();
                }

                int i3 = (b2 & 224) >> 5;
                int i4 = b2 & 31;
                DataItem watchableObject5 = null;
                switch (i3)
                {
                    case 0:
                        watchableObject5 = new DataItem(i3, i4, dataInputStream0.ReadSByte());
                        break;
                    case 1:
                        watchableObject5 = new DataItem(i3, i4, dataInputStream0.ReadBEShort());
                        break;
                    case 2:
                        watchableObject5 = new DataItem(i3, i4, dataInputStream0.ReadBEInt());
                        break;
                    case 3:
                        watchableObject5 = new DataItem(i3, i4, dataInputStream0.ReadBEFloat());
                        break;
                    case 4:
                        watchableObject5 = new DataItem(i3, i4, dataInputStream0.ReadMUTF8());
                        break;
                    case 5:
                        short s9 = dataInputStream0.ReadBEShort();
                        sbyte b10 = dataInputStream0.ReadSByte();
                        short s11 = dataInputStream0.ReadBEShort();
                        watchableObject5 = new DataItem(i3, i4, new ItemInstance(s9, b10, s11));
                        break;
                    case 6:
                        int i6 = dataInputStream0.ReadBEInt();
                        int i7 = dataInputStream0.ReadBEInt();
                        int i8 = dataInputStream0.ReadBEInt();
                        watchableObject5 = new DataItem(i3, i4, new Pos(i6, i7, i8));
                        break;
                }

                arrayList1.Add(watchableObject5);
            }

            return arrayList1;
        }

        public virtual void UpdateWatchedObjectsFromList(IList<DataItem> list1)
        {
            IEnumerator<DataItem> iterator2 = list1.GetEnumerator();
            while (iterator2.MoveNext())
            {
                DataItem watchableObject3 = iterator2.Current;
                DataItem watchableObject4 = this.watchedObjects[watchableObject3.GetDataValueId()];
                if (watchableObject4 != null)
                {
                    watchableObject4.SetObject(watchableObject3.GetObject());
                }
            }
        }

        static SynchedEntityData()
        {
            dataTypes[typeof(sbyte)] = 0;
            dataTypes[typeof(short)] = 1;
            dataTypes[typeof(int)] = 2;
            dataTypes[typeof(float)] = 3;
            dataTypes[typeof(string)] = 4;
            dataTypes[typeof(ItemInstance)] = 5;
            dataTypes[typeof(Pos)] = 6;
        }
    }
}