using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Commands.Porting
{
    class CopyTagNamesCommand : Command
    {
        private HaloOnlineCacheContext CacheContext;
        private CacheFile BlamCache;
        private Dictionary<Tag, List<string>> CopiedTags;
        
        private static uint NewNameCount = 0;

        public CopyTagNamesCommand(HaloOnlineCacheContext cacheContext, CacheFile blamCache) :
            base(true,
                
                "CopyTagNames",
                "Copies the names of all the tag references in the specified tag recursively.",
                
                "CopyTagNames <Tag>",

                "Copies the names of all the tag references in the specified tag recursively.")
        {
            CacheContext = cacheContext;
            BlamCache = blamCache;
            CopiedTags = new Dictionary<Tag, List<string>>();
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var blamTags = FindBlamTags(args[0]);

            using (var cacheStream = CacheContext.OpenTagCacheRead())
                foreach (var blamTag in blamTags)
                    if (blamTag != null)
                        foreach (var instance in CacheContext.TagCache.Index)
                            if (instance != null && instance.Name == blamTag.Name && instance.IsInGroup(blamTag.GroupTag))
                                CopyTagNames(cacheStream, instance, blamTag);

            Console.WriteLine($"Number of new tag names: {NewNameCount}");

            return true;
        }

        private void CopyTagNames(Stream cacheStream, CachedTagInstance edTag, CacheFile.IndexItem blamTag)
        {
            if (CopiedTags.ContainsKey(blamTag.GroupTag))
            {
                foreach (var name in CopiedTags[blamTag.GroupTag])
                    if (name == blamTag.Name)
                        return;
            }
            else
            {
                CopiedTags[blamTag.GroupTag] = new List<string>();
            }

            CopiedTags[blamTag.GroupTag].Add(blamTag.Name);

            var edDef = CacheContext.Deserialize(cacheStream, edTag);
            var blamDef = BlamCache.Deserializer.Deserialize(
                new CacheSerializationContext(ref BlamCache, blamTag),
                TagDefinition.Find(blamTag.GroupTag));

            var edInfo = TagStructure.GetTagStructureInfo(edDef.GetType(), CacheContext.Version);
            var blamInfo = TagStructure.GetTagStructureInfo(blamDef.GetType(), BlamCache.Version);

            var oldName = edTag.Name;
            edTag.Name = blamTag.Name;

            if (edTag.Name != oldName)
                NewNameCount++;

            CopyTagNames(cacheStream, edDef, edInfo, blamDef, blamInfo);
        }

        private void CopyTagNames(Stream cacheStream, object edDef, TagStructureInfo edInfo, object blamDef, TagStructureInfo blamInfo)
        {
            foreach (var blamField in TagStructure.GetTagFieldEnumerable(blamInfo.Types[0], blamInfo.Version))
            {
                foreach (var edField in TagStructure.GetTagFieldEnumerable(edInfo.Types[0], edInfo.Version))
                {
                    if (blamField.FieldType != edField.FieldType || edField.FieldInfo.Name != blamField.FieldInfo.Name)
                        continue;

                    var blamValue = blamField.FieldInfo.GetValue(blamDef);
                    var edValue = edField.FieldInfo.GetValue(edDef);

                    if (blamValue is CachedTagInstance blamRef && edValue is CachedTagInstance edRef)
                    {
                        if (blamRef != null && edRef != null && blamRef.IsInGroup(edRef.Group))
                            CopyTagNames(cacheStream, edRef, BlamCache.GetIndexItemFromID(blamRef.Index));
                    }
                    else if (blamField.FieldType.IsArray && blamField.FieldType.GetElementType().IsClass)
                    {
                        var blamArray = (Array)blamValue;
                        var blamArrayInfo = TagStructure.GetTagStructureInfo(blamField.FieldType.GetElementType(), BlamCache.Version);

                        var edArray = (Array)edValue;
                        var edArrayInfo = TagStructure.GetTagStructureInfo(edField.FieldType.GetElementType(), CacheContext.Version);

                        for (var i = 0; i < blamArray.Length; i++)
                            CopyTagNames(cacheStream, edArray.GetValue(i), edArrayInfo, blamArray.GetValue(i), blamArrayInfo);
                    }
                    else if (blamField.FieldType.IsGenericType && blamField.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        var elementType = blamField.FieldType.GenericTypeArguments[0];
                        
                        if (elementType.IsClass)
                        {
                            var blamElements = (IList)blamValue;
                            var blamElementInfo = TagStructure.GetTagStructureInfo(elementType, BlamCache.Version);

                            var edElements = (IList)edValue;
                            var edElementInfo = TagStructure.GetTagStructureInfo(elementType, CacheContext.Version);

                            if (blamElements.Count == edElements.Count)
                                for (var i = 0; i < blamElements.Count; i++)
                                    CopyTagNames(cacheStream, edElements[i], edElementInfo, blamElements[i], blamElementInfo);
                        }
                    }
                    else if (blamField.FieldType.IsSubclassOf(typeof(TagStructure)))
                    {
                        var blamFieldInfo = TagStructure.GetTagStructureInfo(blamField.FieldType, BlamCache.Version);
                        var edFieldInfo = TagStructure.GetTagStructureInfo(edField.FieldType, CacheContext.Version);
                        CopyTagNames(cacheStream, edValue, edFieldInfo, blamValue, blamFieldInfo);
                    }
                }
            }
        }

        private List<CacheFile.IndexItem> FindBlamTags(string tagSpecifier)
        {
            List<CacheFile.IndexItem> result = new List<CacheFile.IndexItem>();

            if (tagSpecifier.Length == 0 || (!char.IsLetter(tagSpecifier[0]) && !tagSpecifier.Contains('*')) || !tagSpecifier.Contains('.'))
            {
                Console.WriteLine($"ERROR: Invalid tag name: {tagSpecifier}");
                return new List<CacheFile.IndexItem>();
            }

            var tagIdentifiers = tagSpecifier.Split('.');

            if (!CacheContext.TryParseGroupTag(tagIdentifiers[1], out var groupTag))
            {
                Console.WriteLine($"ERROR: Invalid tag name: {tagSpecifier}");
                return new List<CacheFile.IndexItem>();
            }

            var tagName = tagIdentifiers[0];

            // find the CacheFile.IndexItem(s)
            if (tagName == "*") result = BlamCache.IndexItems.FindAll(
                item => item != null && item.IsInGroup(groupTag));
            else
            {
                try
                {
                    result.Add(BlamCache.IndexItems.Find(
                    item => item != null && item.IsInGroup(groupTag) && tagName == item.Name));
                }
                catch { }
            }

            if (result.Count == 0)
            {
                Console.WriteLine($"ERROR: Invalid tag name: {tagSpecifier}");
                return new List<CacheFile.IndexItem>();
            }

            return result;
        }
    }
}