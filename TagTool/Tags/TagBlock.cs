using TagTool.Cache;
using System.Collections;
using System.Collections.Generic;
using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
	[TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Retail)]
    public class TagBlock<T> : IList<T> where T : TagStructure
    {
        /// <summary>
        /// The count of the referenced block.
        /// </summary>
        public int Count;

        /// <summary>
        /// The address of the referenced block.
        /// </summary>
        public CacheAddress Address;

        [TagField(Flags = Padding, Length = 4, MinVersion = CacheVersion.Halo3Retail)]
        public byte[] Unused = new byte[4];

        /// <summary>
        /// The list of elements within the tag block.
        /// </summary>
        [TagField(Flags = Runtime)]
        public List<T> Elements;

		public TagBlock() : this(0, new CacheAddress()) { }

		public TagBlock(int count, CacheAddress address)
		{
			Count = count;
			Address = address;
            Elements = new List<T>(count);
		}

		#region IList<TagStructure> Implementation
		int ICollection<T>.Count => Elements.Count;
		public bool IsReadOnly => false;
		public T this[int index] { get => Elements[index]; set => Elements[index] = value; }
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
		#endregion
	}
}
