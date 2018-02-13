using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Cache
{
    /// <summary>
    /// Wraps a list of tags.
    /// </summary>
    public class TagCacheIndex : IEnumerable<CachedTagInstance>
    {
        private IList<CachedTagInstance> Tags { get; }

        /// <summary>
        /// Gets the number of tags in the list.
        /// </summary>
        public int Count => Tags.Count;

        /// <summary>
        /// Gets the <see cref="CachedTagInstance"/> with a specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The corresponding tag. Can be <c>null</c>.</returns>
        public CachedTagInstance this[int index]
        {
            get { return Tags[index]; }
            set { Tags[index] = value; }
        }

        /// <summary>
        /// Constructs a tag list which wraps a list of tags.
        /// </summary>
        /// <param name="tags">The list of tags to wrap.</param>
        public TagCacheIndex(IList<CachedTagInstance> tags)
        {
            Tags = tags;
        }

        public IEnumerator<CachedTagInstance> GetEnumerator() =>
            Tags.GetEnumerator();

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            ((System.Collections.IEnumerable)Tags).GetEnumerator();

        internal void RemoveAt(int x) =>
            Tags.RemoveAt(x);

        /// <summary>
        /// Determines whether a tag is in the list.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns><c>true</c> if the tag is in the list.</returns>
        public bool Contains(CachedTagInstance tag)
        {
            if (tag == null)
                throw new ArgumentNullException("tag");

            return (Contains(tag.Index) && Tags[tag.Index] == tag);
        }

        /// <summary>
        /// Determines whether a tag is in the list.
        /// </summary>
        /// <param name="index">The index of the tag to check.</param>
        /// <returns><c>true</c> if a tag with the given index is in the list.</returns>
        public bool Contains(int index) =>
            (index >= 0) &&
            (index < Tags.Count) &&
            (Tags[index] != null);

        /// <summary>
        /// Retrieves an enumerable collection of tags which are not null.
        /// This should be preferred over doing this manually because it also skips tags that are in the process of being created.
        /// </summary>
        /// <returns>A collection of tags which are not null.</returns>
        public IEnumerable<CachedTagInstance> NonNull() =>
            Tags.Where(t =>
                (t != null) &&
                (t.HeaderOffset >= 0));

        /// <summary>
        /// Finds the first tag in a given group.
        /// </summary>
        /// <param name="groupTag">The group tag.</param>
        /// <returns>The first tag in the given group, or <c>null</c> otherwise.</returns>
        public CachedTagInstance FindFirstInGroup(Tag groupTag) =>
            NonNull().FirstOrDefault(t => t.IsInGroup(groupTag));

        /// <summary>
        /// Finds the first tag in a given group.
        /// </summary>
        /// <param name="groupTag">The group tag as a string.</param>
        /// <returns>The first tag in the given group, or <c>null</c> otherwise.</returns>
        public CachedTagInstance FindFirstInGroup(string groupTag) =>
            FindFirstInGroup(new Tag(groupTag));

        /// <summary>
        /// Finds the first tag in a given group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>The first tag in the given group, or <c>null</c> otherwise.</returns>
        public CachedTagInstance FindFirstInGroup(TagGroup group) =>
            FindFirstInGroup(group.Tag);

        /// <summary>
        /// Finds all tags in a given group.
        /// </summary>
        /// <param name="groupTag">The group tag.</param>
        /// <returns>All tags in the given group.</returns>
        public IEnumerable<CachedTagInstance> FindAllInGroup(Tag groupTag) =>
            NonNull().Where(t => t.IsInGroup(groupTag));

        /// <summary>
        /// Finds all tags in a given group.
        /// </summary>
        /// <param name="groupTag">The group tag as a string.</param>
        /// <returns>All tags in the given group.</returns>
        public IEnumerable<CachedTagInstance> FindAllInGroup(string groupTag) =>
            FindAllInGroup(new Tag(groupTag));

        /// <summary>
        /// Finds all tags in a given group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <returns>All tags in the given group.</returns>
        public IEnumerable<CachedTagInstance> FindAllInGroup(TagGroup group) =>
            FindAllInGroup(group.Tag);

        /// <summary>
        /// Finds all tags belonging to at least one group in a collection of groups.
        /// </summary>
        /// <param name="groupTags">The group tags.</param>
        /// <returns>All tags which belong to at least one of the groups.</returns>
        public IEnumerable<CachedTagInstance> FindAllInGroups(ICollection<Tag> groupTags) =>
            NonNull().Where(t =>
                groupTags.Contains(t.Group.Tag) ||
                groupTags.Contains(t.Group.ParentTag) ||
                groupTags.Contains(t.Group.GrandparentTag));

        /// <summary>
        /// Retrieves a set of all tags that a given tag depends on.
        /// </summary>
        /// <param name="tag">The tag to scan.</param>
        /// <returns>A set of all tags that the tag depends on.</returns>
        public HashSet<CachedTagInstance> FindDependencies(CachedTagInstance tag)
        {
            var result = new HashSet<CachedTagInstance>();
            FindDependencies(result, tag);
            return result;
        }

        private void FindDependencies(ISet<CachedTagInstance> results, CachedTagInstance tag)
        {
            foreach (var index in tag.Dependencies)
            {
                if (!Contains(index))
                    continue;
                var dependency = this[index];
                if (results.Contains(dependency))
                    continue;
                results.Add(dependency);
                FindDependencies(results, dependency);
            }
        }
    }
}
