using TagTool.Cache;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TagTool.Tags
{
    public class TagBlock<T> : IList, IList<T>
    {
        public int Count => Elements.Count;
        public List<T> Elements;
        public CacheAddressType AddressType;
        
        public TagBlock()
        {
            Elements = new List<T>();
        }

        public TagBlock(CacheAddressType addressType)
        {
            AddressType = addressType;
            Elements = new List<T>();
        }

        public TagBlock(CacheAddressType addressType, List<T> elements)
        {
            AddressType = addressType;
            Elements = elements;
        }

        public T this[int index]
        {
            get => Elements[index];
            set => Elements[index] = value;
        }

        public bool IsReadOnly => false;

        public bool IsFixedSize => ((IList)Elements).IsFixedSize;

        public object SyncRoot => ((IList)Elements).SyncRoot;

        public bool IsSynchronized => ((IList)Elements).IsSynchronized;

        object IList.this[int index] { get => ((IList)Elements)[index]; set => ((IList)Elements)[index] = value; }

        public void Clear() => Elements.Clear();
        public void RemoveAt(int index) => Elements.RemoveAt(index);
        
        int ICollection<T>.Count => Count;
        public void Add(T item) => Elements.Add(item);
        public bool Contains(T item) => Elements.Contains(item);
        public void CopyTo(T[] array, int arrayIndex) => Elements.CopyTo(array, arrayIndex);
        public IEnumerator<T> GetEnumerator() => Elements.GetEnumerator();
        public int IndexOf(T item) => Elements.IndexOf(item);
        public void Insert(int index, T item) => Elements.Insert(index, item);
        public bool Remove(T item) => Elements.Remove(item);
        
        IEnumerator IEnumerable.GetEnumerator() => Elements.GetEnumerator();

        public int Add(object value)
        {
            return ((IList)Elements).Add(value);
        }

        public bool Contains(object value)
        {
            return ((IList)Elements).Contains(value);
        }

        public int IndexOf(object value)
        {
            return ((IList)Elements).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ((IList)Elements).Insert(index, value);
        }

        public void Remove(object value)
        {
            ((IList)Elements).Remove(value);
        }

        public void CopyTo(Array array, int index)
        {
            ((IList)Elements).CopyTo(array, index);
        }
    }
}
