using TagTool.Cache;
using System.Collections;
using System.Collections.Generic;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Retail)]
    public class TagBlock<T> : TagStructure, IList<T> where T : TagStructure
    {
        /// <summary>
        /// The count of the referenced block.
        /// </summary>
        public int Count;

        /// <summary>
        /// The address of the referenced block.
        /// </summary>
        public CacheResourceAddress Address;

        // Non-zero in sbsp resources
        [TagField(MinVersion = CacheVersion.Halo3Retail)]
        public int Unknown;

        [TagField(Flags = TagFieldFlags.Runtime)]
        public List<T> Elements;

        public TagBlock() : this(0, new CacheResourceAddress()) { }

        public TagBlock(int count, CacheResourceAddress address)
        {
            Count = count;
            Address = address;
            Elements = new List<T>();
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
