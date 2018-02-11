using System;
using System.Collections;
using System.Collections.Generic;

namespace TagTool.Common
{
    /// <summary>
    /// Implements a read-only wrapper for a set.
    /// </summary>
    /// <remarks>
    /// Starting in .NET 4.6, HashSet implements IReadOnlyCollection.
    /// This is a quick workaround for earlier versions of .NET.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    public class ReadOnlySet<T> : ISet<T>, IReadOnlyCollection<T>
    {
        private readonly ISet<T> _base;

        /// <summary>
        /// Constructs a read-only wrapper around a set.
        /// </summary>
        /// <param name="baseSet">The base set.</param>
        public ReadOnlySet(ISet<T> baseSet)
        {
            _base = baseSet;
        }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        public int Count => _base.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.
        /// </summary>
        public bool IsReadOnly => true;

        /// <summary>
        /// Adds an element to the current set and returns a value to indicate if the element was successfully added.
        /// </summary>
        /// <param name="item">The element to add to the set.</param>
        /// <returns>
        /// true if the element is added to the set; false if the element is already in the set.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public bool Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <exception cref="System.NotSupportedException"></exception>
        public void Clear()
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return _base.Contains(item);
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _base.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes all elements in the specified collection from the current set.
        /// </summary>
        /// <param name="other">The collection of items to remove from the set.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _base.GetEnumerator();
        }

        /// <summary>
        /// Modifies the current set so that it contains only elements that are also in a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is a proper subset of <paramref name="other" />; otherwise, false.
        /// </returns>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _base.IsProperSubsetOf(other);
        }

        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is a proper superset of <paramref name="other" />; otherwise, false.
        /// </returns>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _base.IsProperSupersetOf(other);
        }

        /// <summary>
        /// Determines whether a set is a subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is a subset of <paramref name="other" />; otherwise, false.
        /// </returns>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _base.IsSubsetOf(other);
        }

        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is a superset of <paramref name="other" />; otherwise, false.
        /// </returns>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _base.IsSupersetOf(other);
        }

        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set and <paramref name="other" /> share at least one common element; otherwise, false.
        /// </returns>
        public bool Overlaps(IEnumerable<T> other)
        {
            return _base.Overlaps(other);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <returns>
        /// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Determines whether the current set and the specified collection contain the same elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        /// true if the current set is equal to <paramref name="other" />; otherwise, false.
        /// </returns>
        public bool SetEquals(IEnumerable<T> other)
        {
            return _base.SetEquals(other);
        }

        /// <summary>
        /// Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Modifies the current set so that it contains all elements that are present in the current set, in the specified collection, or in both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        void ICollection<T>.Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _base.GetEnumerator();
        }
    }
}