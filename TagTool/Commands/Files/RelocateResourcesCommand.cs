using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;

namespace TagTool.Commands.Files
{
    class RelocateResourcesCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }
        private ResourceCache ResourcesB { get; set; }
        private ResourceCache Resources { get; set; }

        public RelocateResourcesCommand(HaloOnlineCacheContext cacheContext) :
            base(true,

                "RelocateResources",
                "Moves all resource out of resources_b.dat and into resources.dat.",

                "RelocateResources",

                "Moves all resource out of the source resource cache and into the destination resource cache.")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 0)
                return false;
            
            ResourcesB = CacheContext.GetResourceCache(ResourceLocation.ResourcesB);
            Resources = CacheContext.GetResourceCache(ResourceLocation.Resources);

            var tags = new Dictionary<int, CachedTagInstance>();
            var relocatedResources = new Dictionary<int, PageableResource>();
            
            using (var tagsStream = CacheContext.OpenTagCacheReadWrite())
            using (var sourceStream = CacheContext.OpenResourceCacheRead(ResourceLocation.ResourcesB))
            using (var destStream = CacheContext.OpenResourceCacheReadWrite(ResourceLocation.Resources))
            {
                for (var i = 0; i < CacheContext.TagCache.Index.Count; i++)
                {
                    if (tags.ContainsKey(i))
                        continue;

                    var tag = tags[i] = CacheContext.GetTag(i);

                    if (tag == null || tag.ResourcePointerOffsets.Count == 0)
                        continue;

                    var isB = false;

                    using (var dataStream = new MemoryStream(CacheContext.TagCache.ExtractTagRaw(tagsStream, tag)))
                    using (var reader = new EndianReader(dataStream))
                    {
                        var dataContext = new DataSerializationContext(reader, null, CacheAddressType.Resource);

                        foreach (var resourcePointerOffset in tag.ResourcePointerOffsets)
                        {
                            reader.BaseStream.Position = resourcePointerOffset;
                            var resourcePointer = reader.ReadUInt32();

                            reader.BaseStream.Position = tag.PointerToOffset(resourcePointer);
                            var resource = CacheContext.Deserializer.Deserialize<PageableResource>(dataContext);

                            if (resource.Page.Index == -1)
                                continue;
                            
                            if (resource.GetLocation(out var location) && location == ResourceLocation.ResourcesB)
                            {
                                isB = true;
                                break;
                            }
                        }
                    }

                    if (!isB)
                        continue;

                    var tagContext = new TagSerializationContext(tagsStream, CacheContext, tag);
                    var tagDefinition = CacheContext.Deserializer.Deserialize(tagContext, TagDefinition.Find(tag.Group.Tag));

                    tagDefinition = ConvertData(tagsStream, sourceStream, destStream, tagDefinition);

                    CacheContext.Serializer.Serialize(tagContext, tagDefinition);
                }
            }

            return true;
        }

        private object ConvertData(Stream tagsStream, Stream sourceStream, Stream destStream, object data)
        {
            if (data == null)
                return null;

            var type = data.GetType();

            if (type == typeof(PageableResource))
            {
                var resource = (PageableResource)data;
                
                if (resource.GetLocation(out var location) && location == ResourceLocation.ResourcesB)
                {
                    resource.ChangeLocation(ResourceLocation.Resources);

                    var resourceData = ResourcesB.ExtractRaw(sourceStream, resource.Page.Index, resource.Page.CompressedBlockSize);
                    resource.Page.Index = Resources.AddRaw(destStream, resourceData);
                }

                return resource;
            }

            if (type.IsPrimitive)
                return data;
            
            if (type.IsArray)
                return ConvertArray(tagsStream, sourceStream, destStream, (Array)data);

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                return ConvertList(tagsStream, sourceStream, destStream, data, type);

            if (type.GetCustomAttributes(typeof(TagStructureAttribute), false).Length > 0)
                return ConvertStructure(tagsStream, sourceStream, destStream, data, type);

            return data;
        }

        private Array ConvertArray(Stream tagsStream, Stream sourceStream, Stream destStream, Array array)
        {
            if (array.GetType().GetElementType().IsPrimitive)
                return array;

            for (var i = 0; i < array.Length; i++)
            {
                var oldValue = array.GetValue(i);
                var newValue = ConvertData(tagsStream, sourceStream, destStream, oldValue);
                array.SetValue(newValue, i);
            }

            return array;
        }

        private object ConvertList(Stream tagsStream, Stream sourceStream, Stream destStream, object list, Type type)
        {
            if (type.GenericTypeArguments[0].IsPrimitive)
                return list;

            var count = (int)type.GetProperty("Count").GetValue(list);

            var getItem = type.GetMethod("get_Item");
            var setItem = type.GetMethod("set_Item");

            for (var i = 0; i < count; i++)
            {
                var oldValue = getItem.Invoke(list, new object[] { i });
                var newValue = ConvertData(tagsStream, sourceStream, destStream, oldValue);
                setItem.Invoke(list, new object[] { i, newValue });
            }

            return list;
        }

        private object ConvertStructure(Stream tagsStream, Stream sourceStream, Stream destStream, object data, Type type)
        {
            var enumerator = ReflectionCache.GetTagFieldEnumerator(ReflectionCache.GetTagStructureInfo(type, CacheContext.Version));

            while (enumerator.Next())
            {
                var oldValue = enumerator.Field.GetValue(data);
                var newValue = ConvertData(tagsStream, sourceStream, destStream, oldValue);
                enumerator.Field.SetValue(data, newValue);
            }

            return data;
        }
    }
}