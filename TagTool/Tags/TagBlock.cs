using TagTool.Cache;
using System.Collections;
using System.Collections.Generic;
using System;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo2Vista)]
	[TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3Retail)]
	public abstract class TagBlock : TagStructure, IList<TagStructure>
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
		protected IList<TagStructure> Elements;

		public TagBlock() : this(0, new CacheAddress()) { }
		public TagBlock(int count, CacheAddress address)
		{
			Count = count;
			Address = address;
			Elements = new List<TagStructure>(count);
		}

		#region IList<TagStructure> Implementation
		int ICollection<TagStructure>.Count => Elements.Count;
		public bool IsReadOnly => Elements.IsReadOnly;
		public TagStructure this[int index] { get => Elements[index]; set => Elements[index] = value; }
		public void Add(TagStructure item) => Elements.Add(item);
		public void Clear() => Elements.Clear();
		public bool Contains(TagStructure item) => Elements.Contains(item);
		public void CopyTo(TagStructure[] array, int arrayIndex) => Elements.CopyTo(array, arrayIndex);
		public IEnumerator<TagStructure> GetEnumerator() => Elements.GetEnumerator();
		public int IndexOf(TagStructure item) => Elements.IndexOf(item);
		public void Insert(int index, TagStructure item) => Elements.Insert(index, item);
		public bool Remove(TagStructure item) => Elements.Remove(item);
		public void RemoveAt(int index) => Elements.RemoveAt(index);
		IEnumerator IEnumerable.GetEnumerator() => Elements.GetEnumerator();
		#endregion
	}

	[TagStructure(Size = 0x0)]
	public class TagBlock<T> : TagBlock, IList<T> where T : TagStructure
	{
		public TagBlock() : this(0, new CacheAddress()) { }

		public TagBlock(int count, CacheAddress address) : base(count, address)
		{
			if (typeof(T) == typeof(TagBlock))
				throw new NotSupportedException($"Type parameter must not be `{nameof(TagBlock)}`: `{nameof(TagBlock<T>)}`.");
		}

		#region IList<T> Implementation
		int ICollection<T>.Count => Elements.Count;
		T IList<T>.this[int index] { get => (T)Elements[index]; set => Elements[index] = value; }
        public new T this[int index] { get => (T)Elements[index]; set => Elements[index] = value; }
		public void Add(T value) => Elements.Add(value);
		public bool Contains(T value) => Elements.Contains(value);
		public void CopyTo(T[] array, int arrayIndex) => Elements.CopyTo(array, arrayIndex);
		public int IndexOf(T value) => Elements.IndexOf(value);
		public void Insert(int index, T value) => Elements.Insert(index, value);
		bool ICollection<T>.Remove(T item) => Elements.Remove(item);
		IEnumerator<T> IEnumerable<T>.GetEnumerator() => (IEnumerator<T>)Elements.GetEnumerator();
		#endregion
	}
}
