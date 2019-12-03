using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using TagTool.Serialization;

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
                        var dataContext = new DataSerializationContext(reader, null, CacheResourceAddressType.Resource);

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

                    var tagDefinition = CacheContext.Deserialize(tagsStream, tag);

                    tagDefinition = ConvertData(tagsStream, sourceStream, destStream, tagDefinition);

                    CacheContext.Serialize(tagsStream, tag, tagDefinition);
                }
            }

            return true;
        }

        private object ConvertData(Stream tagsStream, Stream sourceStream, Stream destStream, object data)
        {
  
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

   			switch (data)
			{
				case null:
				case string _:
				case ValueType _:
					return data;
				case PageableResource resource:
					return ConvertPageableResource(sourceStream, destStream, resource);
				case TagStructure structure:
					return ConvertStructure(tagsStream, sourceStream, destStream, structure);
				case IList collection:
					return ConvertCollection(tagsStream, sourceStream, destStream, collection);
			}

			return data;
		}

		private PageableResource ConvertPageableResource(Stream sourceStream, Stream destStream, PageableResource resource)
		{
			if (resource.GetLocation(out var location) && location == ResourceLocation.ResourcesB)
			{
				resource.ChangeLocation(ResourceLocation.Resources);

				var resourceData = ResourcesB.ExtractRaw(sourceStream, resource.Page.Index, resource.Page.CompressedBlockSize);
				resource.Page.Index = Resources.AddRaw(destStream, resourceData);
			}

			return resource;
		}

		private IList ConvertCollection(Stream tagsStream, Stream sourceStream, Stream destStream, IList collection)
		{
			if (collection.GetType().GetElementType().IsPrimitive)
				return collection;
			
			for (var i = 0; i < collection.Count; i++)
			{
				var oldValue = collection[i];
				var newValue = ConvertData(tagsStream, sourceStream, destStream, destStream);
				collection[i] = newValue;
			}

			return collection;
		}

		private T ConvertStructure<T>(Stream tagsStream, Stream sourceStream, Stream destStream, T data) where T : TagStructure
        {
			foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(typeof(T), CacheContext.Version))
			{
				var oldValue = tagFieldInfo.GetValue(data);
				var newValue = ConvertData(tagsStream, sourceStream, destStream, oldValue);
				tagFieldInfo.SetValue(data, newValue);
			}

            return data;
        }
    }
}