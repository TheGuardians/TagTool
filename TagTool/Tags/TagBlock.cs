using TagTool.Cache;
using TagTool.Serialization;
using System.Collections;
using System.Collections.Generic;

namespace TagTool.Tags
{
    [TagStructure(Size = 0xC)]
    public class TagBlock<T> : IList<T>
    {
        /// <summary>
        /// The count of the referenced block.
        /// </summary>
        public int Count;

        /// <summary>
        /// The address of the referenced block.
        /// </summary>
        public CacheAddress Address;

        public int Unused;

        [TagField(Local = true)]
        private List<T> Elements = new List<T>();

        int ICollection<T>.Count => Elements.Count;
        public bool IsReadOnly => ((IList<T>)Elements).IsReadOnly;

        public TagBlock()
        {
        }

        public TagBlock(int count, CacheAddress address)
        {
            Count = count;
            Address = address;
        }

        public T this[int index]
        {
            get { return Elements[index]; }
            set { Elements[index] = value; }
        }

        public int IndexOf(T item) => Elements.IndexOf(item);

        public void Insert(int index, T item) => Elements.Insert(index, item);

        public void RemoveAt(int index) => Elements.RemoveAt(index);

        public void Add(T item) => Elements.Add(item);

        public void Clear() => Elements.Clear();

        public bool Contains(T item) => Elements.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => Elements.CopyTo(array, arrayIndex);

        public bool Remove(T item) => Elements.Remove(item);

        public IEnumerator<T> GetEnumerator() => Elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Elements.GetEnumerator();
    }
}
