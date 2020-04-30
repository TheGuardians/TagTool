using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TagTool.Cache.Gen3;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache
{
    public abstract class TagCache
    {
        // TODO: refactor TagGroup to contain a string instead of string ID
        public CacheVersion Version;
        public TagDefinitions TagDefinitions;
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

                if (!TagDefinitions.TagDefinitionExists(groupTag))
                {
                    Console.WriteLine($"TagGroup not found for type \"{type.Name}\" ({structure.Tag}).");
                    return false;
                }

                result = AllocateTag(TagDefinitions.GetTagGroupFromTag(groupTag), name);

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

        public bool TryGetCachedTag(int index, out CachedTag instance)
        {
            if (index < 0 || index >= TagTable.Count())
            {
                instance = null;
                return false;
            }

            instance = GetTag(index);
            return true;
        }

        public bool TryGetCachedTag(string name, out CachedTag result)
        {
            if (name.Length == 0)
            {
                result = null;
                return false;
            }

            if (name == "null")
            {
                result = null;
                return true;
            }

            if (name == "*")
            {
                if (TagTable.Count() == 0)
                {
                    result = null;
                    return false;
                }

                result = TagTable.Last();
                return true;
            }

            if (name.StartsWith("*."))
            {
                if (!name.TrySplit('.', out var startNamePieces) || !TryParseGroupTag(startNamePieces[1], out var starGroupTag))
                {
                    result = null;
                    return false;
                }

                result = TagTable.Last(tag => tag != null && tag.IsInGroup(starGroupTag));
                return true;
            }

            if (name.StartsWith("0x"))
            {
                name = name.Substring(2);

                if (name.TrySplit('.', out var hexNamePieces))
                    name = hexNamePieces[0];

                if (!int.TryParse(name, NumberStyles.HexNumber, null, out int tagIndex) || !IsTagIndexValid(tagIndex))
                {
                    result = null;
                    return false;
                }

                result = GetTag(tagIndex);

                if (result == null) // failsafe for null tags
                    result = CreateCachedTag(tagIndex, TagGroup.None);

                return true;
            }

            if (!name.TrySplit('.', out var namePieces) || !TryParseGroupTag(namePieces[namePieces.Length - 1], out var groupTag))
                throw new Exception($"Invalid tag name: {name}");

            var tagName = name.Substring(0, name.Length - (1 + namePieces[namePieces.Length - 1].Length));

            foreach (var instance in TagTable)
            {
                if (instance is null)
                    continue;

                if (instance.IsInGroup(groupTag) && instance.Name == tagName)
                {
                    result = instance;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public bool TryParseGroupTag(string name, out Tag result)
        {
            if(TagDefinitions.GetType() == typeof(TagDefinitionsGen3))
            {
                foreach(var pair in TagDefinitions.Types)
                {
                    TagGroupGen3 group = (TagGroupGen3)pair.Key;
                    if(group.Name == name)
                    {
                        result = group.Tag;
                        return true;
                    }
                }
            }

            var type = TagDefinitions.GetTagDefinitionType(name);

            if (type != null)
            {
                var attribute = TagStructure.GetTagStructureAttribute(type);
                result = new Tag(attribute.Tag);
                return true;
            }

            result = Tag.Null;
            return name == "none" || name == "null";
        }

        public bool TryGetTag<T>(string name, out CachedTag result) where T : TagStructure
        {
            if (name == "none" || name == "null")
            {
                result = null;
                return true;
            }
            var groupTag = TagDefinitions.GetTagDefinitionGroupTag(typeof(T));
            if (!(groupTag is null))
            {
                foreach (var instance in TagTable)
                {
                    if (instance is null)
                        continue;

                    if (instance.Group == groupTag && instance.Name == name)
                    {
                        result = instance;
                        return true;
                    }
                }
            }

            result = null;
            return false;
        }

        public CachedTag GetTag<T>(string name) where T : TagStructure
        {
            if (TryGetTag<T>(name, out var result))
                return result;

            var attribute = TagStructure.GetTagStructureAttribute(typeof(T));
            var typeName = attribute.Name ?? typeof(T).Name.ToSnakeCase();

            throw new KeyNotFoundException($"'{typeName}' tag \"{name}\"");
        }

        public bool TryGetTag(string name, out CachedTag result)
        {
            if (name.Length == 0)
            {
                result = null;
                return false;
            }

            if (name == "null")
            {
                result = null;
                return true;
            }

            if (name == "*")
            {
                if (Count == 0)
                {
                    result = null;
                    return false;
                }

                result = TagTable.Last();
                return true;
            }

            if (name.StartsWith("*."))
            {
                if (!name.TrySplit('.', out var startNamePieces) || !TryParseGroupTag(startNamePieces[1], out var starGroupTag))
                {
                    result = null;
                    return false;
                }

                result = TagTable.Last(tag => tag != null && tag.IsInGroup(starGroupTag));
                return true;
            }

            if (name.StartsWith("0x"))
            {
                name = name.Substring(2);

                if (name.TrySplit('.', out var hexNamePieces))
                    name = hexNamePieces[0];

                if (!int.TryParse(name, NumberStyles.HexNumber, null, out int tagIndex) || (GetTag(tagIndex) == null))
                {
                    result = null;
                    return false;
                }

                result = GetTag(tagIndex);
                return true;
            }

            if (!name.TrySplit('.', out var namePieces) || !TryParseGroupTag(namePieces[namePieces.Length - 1], out var groupTag))
                throw new Exception($"Invalid tag name: {name}");

            //var tagName = namePieces[0];

            var tagName = name.Substring(0, name.Length - (1 + namePieces[namePieces.Length - 1].Length));

            foreach (var instance in TagTable)
            {
                if (instance is null)
                    continue;

                if (instance.IsInGroup(groupTag) && instance.Name == tagName)
                {
                    result = instance;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public CachedTag GetTag(string name)
        {
            if (TryGetTag(name, out var result))
                return result;

            throw new KeyNotFoundException(name);
        }

        public Tag ParseGroupTag(string name)
        {
            if (!TryParseGroupTag(name, out Tag result))
                return Tag.Null;
            else
                return result;
        }
    }

}
