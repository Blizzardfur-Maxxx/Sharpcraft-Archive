using SharpCraft.Core.Stats;
using SharpCraft.Core.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCraft.Client.Stats
{
    public class StatsSyncher
    {
        private volatile bool busy = false;
        private volatile NullDictionary<Stat, int> field_27437_b = null;
        private volatile NullDictionary<Stat, int> field_27436_c = null;
        private StatFileWriter writer;
        private JFile unsentData;
        private JFile data;
        private JFile unsentTmp;
        private JFile tmp;
        private JFile unsentOld;
        private JFile old;
        private User user;
        private int timeout = 0;
        private int field_27426_m = 0;

        public StatsSyncher(User session1, StatFileWriter statFileWriter2, JFile file3)
        {
            this.unsentData = new JFile(file3, "stats_" + session1.name.ToLower() + "_unsent.dat");
            this.data = new JFile(file3, "stats_" + session1.name.ToLower() + ".dat");
            this.unsentOld = new JFile(file3, "stats_" + session1.name.ToLower() + "_unsent.old");
            this.old = new JFile(file3, "stats_" + session1.name.ToLower() + ".old");
            this.unsentTmp = new JFile(file3, "stats_" + session1.name.ToLower() + "_unsent.tmp");
            this.tmp = new JFile(file3, "stats_" + session1.name.ToLower() + ".tmp");
            if (!session1.name.ToLower().Equals(session1.name))
            {
                this.RenameFile(file3, "stats_" + session1.name + "_unsent.dat", this.unsentData);
                this.RenameFile(file3, "stats_" + session1.name + ".dat", this.data);
                this.RenameFile(file3, "stats_" + session1.name + "_unsent.old", this.unsentOld);
                this.RenameFile(file3, "stats_" + session1.name + ".old", this.old);
                this.RenameFile(file3, "stats_" + session1.name + "_unsent.tmp", this.unsentTmp);
                this.RenameFile(file3, "stats_" + session1.name + ".tmp", this.tmp);
            }

            this.writer = statFileWriter2;
            this.user = session1;
            if (this.unsentData.Exists())
            {
                statFileWriter2.Func_27179_a(this.Func_27415_a(this.unsentData, this.unsentTmp, this.unsentOld));
            }

            this.GetStats();
        }

        private void RenameFile(JFile parent, string filename, JFile dest)
        {
            JFile file4 = new JFile(parent, filename);
            if (file4.Exists() && !file4.IsDirectory() && !dest.Exists())
            {
                file4.RenameTo(dest);
            }
        }

        private NullDictionary<Stat, int> Func_27415_a(JFile file1, JFile file2, JFile file3)
        {
            return file1.Exists() ? this.Func_27408_a(file1) : (file3.Exists() ? this.Func_27408_a(file3) : (file2.Exists() ? this.Func_27408_a(file2) : null));
        }

        private NullDictionary<Stat, int> Func_27408_a(JFile file1)
        {
            StreamReader bufferedReader2 = null;
            try
            {
                bufferedReader2 = new StreamReader(file1.GetReadStream());
                string string3 = "";
                StringBuilder stringBuilder4 = new StringBuilder();
                while ((string3 = bufferedReader2.ReadLine()) != null)
                {
                    stringBuilder4.Append(string3);
                }

                NullDictionary<Stat, int> map5 = StatFileWriter.ReadStatsFromJson(stringBuilder4.ToString());
                return map5;
            }
            catch (Exception exception15)
            {
                exception15.PrintStackTrace();
            }
            finally
            {
                if (bufferedReader2 != null)
                {
                    try
                    {
                        bufferedReader2.Dispose();
                    }
                    catch (Exception exception14)
                    {
                        exception14.PrintStackTrace();
                    }
                }
            }

            return null;
        }

        private void WriteLocal(NullDictionary<Stat, int> map1, JFile file2, JFile file3, JFile file4)
        {
            StreamWriter printWriter5 = new StreamWriter(file3.GetWriteStream());
            try
            {
                printWriter5.Write(StatFileWriter.WriteStatsToJson(this.user.name, "local", map1));
            }
            finally
            {
                printWriter5.Dispose();
            }

            if (file4.Exists())
            {
                file4.Delete();
            }

            if (file2.Exists())
            {
                file2.RenameTo(file4);
            }

            file3.RenameTo(file2);
        }

        public virtual void GetStats()
        {
            if (this.busy)
            {
                throw new InvalidOperationException("Can't get stats from server while StatsSyncher is busy!");
            }
            else
            {
                this.timeout = 100;
                this.busy = true;
                new Thread(() => 
                {
                    try
                    {
                        if (this.field_27437_b != null)
                        {
                            this.WriteLocal(this.field_27437_b, this.data, this.tmp, this.old);
                        }
                        else if (this.data.Exists())
                        {
                            this.field_27437_b = this.Func_27415_a(this.data, this.tmp, this.old);
                        }
                    }
                    catch (Exception exception5)
                    {
                        exception5.PrintStackTrace();
                    }
                    finally
                    {
                        this.busy = false;
                    }
                }).Start();
            }
        }

        public virtual void SaveStats(NullDictionary<Stat, int> map1)
        {
            if (this.busy)
            {
                throw new InvalidOperationException("Can't save stats while StatsSyncher is busy!");
            }
            else
            {
                this.timeout = 100;
                this.busy = true;
                new Thread(() => 
                {
                    try
                    {
                        this.WriteLocal(map1, this.unsentData, this.unsentTmp, this.unsentOld);
                    }
                    catch (Exception exception5)
                    {
                        exception5.PrintStackTrace();
                    }
                    finally
                    {
                        this.busy = false;
                    }
                }).Start();
            }
        }

        public virtual void SyncStatsFileWithMap(NullDictionary<Stat, int> map1)
        {
            int i2 = 30;
            while (this.busy)
            {
                --i2;
                if (i2 <= 0)
                {
                    break;
                }

                Thread.Sleep(100);
            }

            this.busy = true;
            try
            {
                this.WriteLocal(map1, this.unsentData, this.unsentTmp, this.unsentOld);
            }
            catch (Exception ex)
            {
                ex.PrintStackTrace();
            }
            finally
            {
                this.busy = false;
            }
        }

        public virtual bool Func_27420_b()
        {
            return this.timeout <= 0 && !this.busy && this.field_27436_c == null;
        }

        public virtual void Tick()
        {
            if (this.timeout > 0)
            {
                --this.timeout;
            }

            if (this.field_27426_m > 0)
            {
                --this.field_27426_m;
            }

            if (this.field_27436_c != null)
            {
                this.writer.Write(this.field_27436_c);
                this.field_27436_c = null;
            }

            if (this.field_27437_b != null)
            {
                this.writer.PutStats(this.field_27437_b);
                this.field_27437_b = null;
            }
        }
    }
}
