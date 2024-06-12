using System.Collections.Generic;
using System.Threading;

namespace SharpCraft.Core.Util
{
    public class ThreadedIO
    {
        public static readonly ThreadedIO Instance = new ThreadedIO();
        private SynchronizedCollection<IThreadedIOTarget> queue = new SynchronizedCollection<IThreadedIOTarget>();
        private long queuedCount;
        private long completedCount;
        private volatile bool busy;

        public interface IThreadedIOTarget
        {
            bool Write();
        }

        private ThreadedIO()
        {
            Thread thread = new Thread(new ThreadStart(() => 
            {
                while (true)
                {
                    ProcessQueue();
                }
            }));
            thread.Name = "File IO Thread";
            thread.Priority = ThreadPriority.Lowest;
            thread.Start();
        }

        private void ProcessQueue()
        {
            for (int i = 0; i < queue.Count; i++)
            {
                IThreadedIOTarget tiotarget = queue[i];
                bool result = tiotarget.Write();

                if (!result)
                {
                    queue.RemoveAt(i--);
                    completedCount++;
                }

                if (!busy)
                    Thread.Sleep(10);
                else
                    Thread.Sleep(0);
            }

            if (queue.Count == 0)
                Thread.Sleep(25);
        }

        public void QueueIO(IThreadedIOTarget tiotarget)
        {
            if (queue.Contains(tiotarget))
                return;
            queuedCount++;
            queue.Add(tiotarget);
        }

        public void WaitForFinish()
        {
            busy = true;
            while (queuedCount != completedCount)
                Thread.Sleep(10);
            busy = false;
        }
    }
}
