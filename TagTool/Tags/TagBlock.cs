using TagTool.Cache;
using System.Collections;
using System.Collections.Generic;

namespace TagTool.Tags
{
    public class TagBlock<T> : IList<T>
    {

        public int Count => Elements.Count;
        public List<T> Elements;

        public CacheAddress Address; // REMOVE ASAP
        public CacheAddressType AddressType;
        

        public TagBlock() : this(0, new CacheAddress()) { }

        public TagBlock(int count, CacheAddress address) // REMOVE ASAP
        {
            Address = address;
            Elements = new List<T>(count);
            AddressType = CacheAddressType.Memory;
        }

        public T this[int index]
        {
            get => Elements[index];
            set => Elements[index] = value;
        }

        public bool IsReadOnly => false;

        int ICollection<T>.Count => Count;

        public void Add(T item) => Elements.Add(item);
        public void Clear() => Elements.Clear();
        public bool Contains(T item) => Elements.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => Elements.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => Elements.GetEnumerator();
        public int IndexOf(T item) => Elements.IndexOf(item);
        public void Insert(int index, T item) => Elements.Insert(index, item);
        public bool Remove(T item) => Elements.Remove(item);
        public void RemoveAt(int index) => Elements.RemoveAt(index);
        IEnumerator IEnumerable.GetEnumerator() => Elements.GetEnumerator();
    }
}
