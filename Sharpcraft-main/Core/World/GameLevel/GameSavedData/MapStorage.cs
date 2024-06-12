using SharpCraft.Core.NBT;
using SharpCraft.Core.Util;
using SharpCraft.Core.World.GameLevel.Storage;
using System;
using System.Collections.Generic;
using System.IO;

namespace SharpCraft.Core.World.GameLevel.GameSavedData
{
    public class MapStorage
    {
        private ILevelStorage field_a;
        private NullDictionary<string, SavedData> loadedDataMap = new NullDictionary<string, SavedData>();
        private IList<SavedData> loadedDataList = new List<SavedData>();
        private NullDictionary<string, short> idCounts = new NullDictionary<string, short>();
        public MapStorage(ILevelStorage iSaveHandler1)
        {
            this.field_a = iSaveHandler1;
            this.LoadIdCounts();
        }

        public virtual SavedData LoadData(Type class1, string string2)
        {
            SavedData mapDataBase3 = this.loadedDataMap[string2];
            if (mapDataBase3 != null)
            {
                return mapDataBase3;
            }
            else
            {
                if (this.field_a != null)
                {
                    try
                    {
                        JFile file4 = this.field_a.GetDataFile(string2);
                        if (file4 != null && file4.Exists())
                        {
                            try
                            {
                                mapDataBase3 = class1.GetConstructor(new Type[] { typeof(string) }).Invoke(new object[] { string2 }) as SavedData;
                            }
                            catch (Exception exception7)
                            {
                                throw new Exception("Failed to instantiate " + class1.ToString(), exception7);
                            }

                            FileStream fileInputStream5 = file4.GetReadStream();
                            CompoundTag nBTTagCompound6 = NbtIO.ReadCompressed(fileInputStream5);
                            fileInputStream5.Dispose();
                            mapDataBase3.Read(nBTTagCompound6.GetCompoundTag("data"));
                        }
                    }
                    catch (Exception exception8)
                    {
                        exception8.PrintStackTrace();
                    }
                }

                if (mapDataBase3 != null)
                {
                    this.loadedDataMap[string2] = mapDataBase3;
                    this.loadedDataList.Add(mapDataBase3);
                }

                return mapDataBase3;
            }
        }

        public virtual void SetData(string string1, SavedData mapDataBase2)
        {
            if (mapDataBase2 == null)
            {
                throw new Exception("Can't set null data");
            }
            else
            {
                if (this.loadedDataMap.ContainsKey(string1))
                {
                    SavedData removekey = this.loadedDataMap[string1];
                    this.loadedDataMap.Remove(string1);
                    this.loadedDataList.Remove(removekey);

                    //this.loadedDataList.Remove(this.loadedDataMap.Remove(string1));
                }

                this.loadedDataMap[string1] = mapDataBase2;
                this.loadedDataList.Add(mapDataBase2);
            }
        }

        public virtual void SaveAllData()
        {
            for (int i1 = 0; i1 < this.loadedDataList.Count; ++i1)
            {
                SavedData mapDataBase2 = this.loadedDataList[i1];
                if (mapDataBase2.IsDirty())
                {
                    this.SaveData(mapDataBase2);
                    mapDataBase2.SetDirty(false);
                }
            }
        }

        private void SaveData(SavedData mapDataBase1)
        {
            if (this.field_a != null)
            {
                try
                {
                    JFile file2 = this.field_a.GetDataFile(mapDataBase1.name);
                    if (file2 != null)
                    {
                        CompoundTag nBTTagCompound3 = new CompoundTag();
                        mapDataBase1.Write(nBTTagCompound3);
                        CompoundTag nBTTagCompound4 = new CompoundTag();
                        nBTTagCompound4.SetCompoundTag("data", nBTTagCompound3);
                        FileStream fileOutputStream5 = file2.GetWriteStream();
                        NbtIO.WriteCompressed(nBTTagCompound4, fileOutputStream5);
                        fileOutputStream5.Dispose();
                    }
                }
                catch (Exception exception6)
                {
                    exception6.PrintStackTrace();
                }
            }
        }

        private void LoadIdCounts()
        {
            try
            {
                this.idCounts.Clear();
                if (this.field_a == null)
                {
                    return;
                }

                JFile file1 = this.field_a.GetDataFile("idcounts");
                if (file1 != null && file1.Exists())
                {
                    BinaryReader dataInputStream2 = new BinaryReader(file1.GetReadStream());
                    CompoundTag nBTTagCompound3 = NbtIO.Read(dataInputStream2);
                    dataInputStream2.Dispose();
                    IEnumerator<Tag> iterator4 = nBTTagCompound3.Values.GetEnumerator();
                    while (iterator4.MoveNext())
                    {
                        Tag nBTBase5 = iterator4.Current;
                        if (nBTBase5 is ShortTag)
                        {
                            ShortTag nBTTagShort6 = (ShortTag)nBTBase5;
                            string string7 = nBTTagShort6.GetKey();
                            short s8 = nBTTagShort6.Value;
                            idCounts[string7] = s8;
                        }
                    }
                }
            }
            catch (Exception exception9)
            {
                exception9.PrintStackTrace();
            }
        }

        public virtual int GetUniqueDataId(string string1)
        {
            short short2 = this.idCounts[string1];
            if (short2 < 0) //had a useless null check
            {
                short2 = 0;
            }
            else
            {
                short2 = (short)(short2 + 1);
            }

            this.idCounts[string1] = short2;
            if (this.field_a == null)
            {
                return short2;
            }
            else
            {
                try
                {
                    JFile file3 = this.field_a.GetDataFile("idcounts");
                    if (file3 != null)
                    {
                        CompoundTag nBTTagCompound4 = new CompoundTag();
                        IEnumerator<string> iterator5 = this.idCounts.Keys.GetEnumerator();
                        while (iterator5.MoveNext())
                        {
                            string string6 = iterator5.Current;
                            short s7 = this.idCounts[string6];
                            nBTTagCompound4.SetShort(string6, s7);
                        }

                        BinaryWriter dataOutputStream9 = new BinaryWriter(file3.GetWriteStream());
                        NbtIO.Write(nBTTagCompound4, dataOutputStream9);
                        dataOutputStream9.Dispose();
                    }
                }
                catch (Exception exception8)
                {
                    exception8.PrintStackTrace();
                }

                return short2;
            }
        }
    }
}