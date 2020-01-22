using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache
{
    public abstract class TagCache
    {
        // TODO: refactor TagGroup to contain a string instead of string ID
        public CacheVersion Version;
        public virtual IEnumerable<CachedTag> TagTable { get; }
        public int Count => TagTable.Count();
        public abstract CachedTag GetTag(uint ID);
        public abstract CachedTag GetTag(int index);
        public abstract CachedTag GetTag(string name, Tag groupTag);

        public abstract CachedTag AllocateTag(TagGroup type, string name = null);

        public abstract CachedTag CreateCachedTag(int index, TagGroup group, string name = null);
        public abstract CachedTag CreateCachedTag();

        // Utilities

        public bool IsTagIndexValid(int tagIndex)
        {
            if (tagIndex > 0 && tagIndex < TagTable.Count())
                return true;
            else
                return false;
        }

        public IEnumerable<CachedTag> FindAllInGroup(Tag groupTag) =>
            NonNull().Where(t => t.IsInGroup(groupTag));

        public IEnumerable<CachedTag> NonNull() =>
            TagTable.Where(t =>
                (t != null) &&
                (t.DefinitionOffset >= 0));

        public CachedTag FindFirstInGroup(Tag groupTag) =>
            NonNull().FirstOrDefault(t => t.IsInGroup(groupTag));

        public bool TryAllocateTag(out CachedTag result, Type type, string name = null)
        {
            result = null;

            try
            {
                var structure = TagStructure.GetTagStructureInfo(type, Version).Structure;

                if (structure == null)
                {
                    Console.WriteLine($"TagStructure attribute not found for type \"{type.Name}\".");
                    return false;
                }

                var groupTag = new Tag(structure.Tag);

                if (!TagGroup.Instances.ContainsKey(groupTag))
                {
                    Console.WriteLine($"TagGroup not found for type \"{type.Name}\" ({structure.Tag}).");
                    return false;
                }

                result = AllocateTag(TagGroup.Instances[groupTag], name);

                if (result == null)
                    return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name}: {e.Message}");
                return false;
            }

            return true;
        }

        public CachedTag AllocateTag(Type type, string name = null)
        {
            if (TryAllocateTag(out var result, type, name))
                return result;

            Console.WriteLine($"Failed to allocate tag of type \"{type.Name}\".");
            return null;
        }

        public CachedTag AllocateTag<T>(string name = null) where T : TagStructure
            => AllocateTag(typeof(T), name);

    }

}
