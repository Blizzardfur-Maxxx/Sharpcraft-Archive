using System;

namespace SharpCraft.Core.Util
{
    public class SoftReference<T> where T : class
    {
        private readonly WeakReference<T> weakReference;

        public SoftReference(T target)
        {
            weakReference = new WeakReference<T>(target);
        }

        public T Target
        {
            get
            {
                weakReference.TryGetTarget(out T target);
                return target;
            }
        }

        public bool IsAlive => weakReference.TryGetTarget(out _);

        public void SetTarget(T target)
        {
            weakReference.SetTarget(target);
        }
    }
}
