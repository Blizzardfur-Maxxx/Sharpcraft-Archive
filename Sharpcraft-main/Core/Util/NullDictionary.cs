using System.Collections.Generic;

namespace SharpCraft.Core.Util
{
    public class NullDictionary<TKey, TValue> : Dictionary<TKey, TValue> where TKey : notnull
    {
        public NullDictionary() : base() { }
        public NullDictionary(IDictionary<TKey, TValue> dictionary) : base(dictionary) { }
        public NullDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : base(collection) { }
        public NullDictionary(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public NullDictionary(int capacity) : base(capacity) { }
        public NullDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : base(dictionary, comparer) { }
        public NullDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : base(collection, comparer) { }
        public NullDictionary(int capacity, IEqualityComparer<TKey> comparer) : base(capacity, comparer) { }

        public new TValue this[TKey key]
        {
            get
            {
                bool result = TryGetValue(key, out TValue val);
                return result ? val : default(TValue);
            }
            set => base[key] = value;
        }
    }
}
