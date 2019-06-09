using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;

namespace TagTool.Commands.Modding
{
    class MergeModPackagesCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public MergeModPackagesCommand(HaloOnlineCacheContext cacheContext) :
            base(false,

                "MergeModPackages",
                "Merge 2 mod packages (.pak) files. File 1 has priority over File2. \n",

                "MergeModPackages <File 1> <File 2>",

                "Merge 2 mod packages (.pak) files. File 1 has priority over File2. \n")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var modPackage1 = new ModPackage(new FileInfo(args[0]));
            var modPackage2 = new ModPackage(new FileInfo(args[1]));

            var resultPackage = new ModPackage();

            //
            // Merge tags that overwrite existing tags
            //

            var tagCount = Math.Max(modPackage1.Tags.Index.Count, modPackage2.Tags.Index.Count);

            for (var i = 0; i < tagCount; i++)
            {
                var useTag1 = i < modPackage1.Tags.Index.Count;
                var name1 = modPackage1.TagNames.ContainsKey(i) ?
                    modPackage1.TagNames[i] :
                    $"0x{i:X4}";

                var useTag2 = i < modPackage2.Tags.Index.Count;
                var name2 = modPackage2.TagNames.ContainsKey(i) ?
                    modPackage2.TagNames[i] :
                    $"0x{i:X4}";

                if (useTag1 && useTag2)
                {
                    var tag1 = modPackage1.Tags.Index[i];
                    var tag2 = modPackage2.Tags.Index[i];

                    if (tag1 != null && tag2 != null && !tag2.IsInGroup(tag1.Group) || name1 != name2)
                        throw new FormatException();

                    if (tag1 != null)
                        useTag2 = false;
                    else if (tag2 != null)
                        useTag1 = false;
                    else
                        useTag1 = useTag2 = false;
                }

                if (useTag1 && modPackage1.Tags.Index[i] != null)
                {
                    var srcTag = modPackage1.Tags.Index[i];
                    var destTag = resultPackage.Tags.AllocateTag(srcTag.Group, name1);

                    using (var tagStream = new MemoryStream(modPackage1.Tags.ExtractTagRaw(modPackage1.TagsStream, destTag)))
                    using (var tagReader = new EndianReader(tagStream))
                    {
                        var dataContext = new DataSerializationContext(tagReader);

                        foreach (var offset in srcTag.ResourcePointerOffsets)
                        {
                            tagStream.Position = offset;
                            tagStream.Position = srcTag.PointerToOffset(tagReader.ReadUInt32());

                            var pageable = modPackage1.Deserializer.Deserialize<PageableResource>(dataContext);

                            //
                            // TODO: extract and update resource data from pageable, add to resultPackage
                            //
                        }

                        tagStream.Position = 0;
                        resultPackage.Tags.SetTagDataRaw(resultPackage.TagsStream, destTag, tagStream.ToArray());
                    }
                }
                else if (useTag2 && modPackage2.Tags.Index[i] != null)
                {
                    var srcTag = modPackage2.Tags.Index[i];
                    var destTag = resultPackage.Tags.AllocateTag(srcTag.Group, name2);

                    using (var tagStream = new MemoryStream(modPackage2.Tags.ExtractTagRaw(modPackage2.TagsStream, destTag)))
                    using (var tagReader = new EndianReader(tagStream))
                    {
                        var dataContext = new DataSerializationContext(tagReader);

                        foreach (var offset in srcTag.ResourcePointerOffsets)
                        {
                            tagStream.Position = offset;
                            tagStream.Position = srcTag.PointerToOffset(tagReader.ReadUInt32());

                            var pageable = modPackage2.Deserializer.Deserialize<PageableResource>(dataContext);

                            //
                            // TODO: extract and update resource data from pageable, add to resultPackage
                            //
                        }

                        tagStream.Position = 0;
                        resultPackage.Tags.SetTagDataRaw(resultPackage.TagsStream, destTag, tagStream.ToArray());
                    }
                }
                else
                {
                    resultPackage.Tags.AllocateTag();
                    resultPackage.Tags.UpdateTagOffsets(
                        new BinaryWriter(resultPackage.TagsStream, default, true));
                }
            }

            return true;
        }

        private CachedTagInstance ConvertCachedTagInstance(TagCache destCache, Stream sourceTagCacheStream, Stream destTagCacheStream, Stream sourceResourceCacheStream, Stream destResourceCacheStream, ResourceCache sourceResourceCache, ResourceCache destResourceCache, CachedTagInstance tag)
        {
            var newTag = destCache.AllocateTag(tag.Group, tag.Name);
            var tagDefinition = CacheContext.Deserialize(sourceTagCacheStream, tag);
            tagDefinition = ConvertData(destCache, sourceTagCacheStream, destTagCacheStream, sourceResourceCacheStream, destResourceCacheStream, sourceResourceCache, destResourceCache, tagDefinition);
            CacheContext.Serialize(destTagCacheStream, newTag, tagDefinition);

            return newTag;
        }

        private object ConvertData(TagCache destCache, Stream sourceTagCacheStream, Stream destTagCacheStream, Stream sourceResourceCacheStream, Stream destResourceCacheStream, ResourceCache sourceResourceCache, ResourceCache destResourceCache, object data)
        {

            var type = data.GetType();
            
            switch (data)
            {
                case null:
                case string _:
                case ValueType _:
                    return data;
                case PageableResource resource:
                    return ConvertPageableResource(destCache, sourceResourceCacheStream, destResourceCacheStream, resource, sourceResourceCache, destResourceCache);
                case TagStructure structure:
                    return ConvertStructure(destCache, sourceTagCacheStream, destTagCacheStream, sourceResourceCacheStream, destResourceCacheStream, sourceResourceCache, destResourceCache, structure);
                case IList collection:
                    return ConvertCollection(destCache, sourceTagCacheStream, destTagCacheStream, sourceResourceCacheStream, destResourceCacheStream, sourceResourceCache, destResourceCache, collection);
                case CachedTagInstance tag:
                    return ConvertCachedTagInstance(destCache, sourceTagCacheStream, destTagCacheStream, sourceResourceCacheStream, destResourceCacheStream, sourceResourceCache, destResourceCache, tag);
            }

            return data;
        }

        private PageableResource ConvertPageableResource(TagCache destCache, Stream sourceStream, Stream destStream, PageableResource resource, ResourceCache sourceResourceCache, ResourceCache destResourceCache)
        {
            var resourceData = sourceResourceCache.ExtractRaw(sourceStream, resource.Page.Index, resource.Page.CompressedBlockSize);
            resource.Page.Index = destResourceCache.AddRaw(destStream, resourceData);
            return resource;
        }

        private IList ConvertCollection(TagCache destCache, Stream sourceTagCacheStream, Stream destTagCacheStream, Stream sourceStream, Stream destStream, ResourceCache sourceResourceCache, ResourceCache destResourceCache, IList collection)
        {
            if (collection.GetType().GetElementType().IsPrimitive)
                return collection;

            for (var i = 0; i < collection.Count; i++)
            {
                var oldValue = collection[i];
                var newValue = ConvertData(destCache, sourceTagCacheStream, destTagCacheStream, sourceStream, destStream, sourceResourceCache, destResourceCache, destStream);
                collection[i] = newValue;
            }

            return collection;
        }

        private T ConvertStructure<T>(TagCache destCache, Stream sourceTagCacheStream, Stream destTagCacheStream, Stream sourceStream, Stream destStream, ResourceCache sourceResourceCache, ResourceCache destResourceCache, T data) where T : TagStructure
        {
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(typeof(T), CacheContext.Version))
            {
                var oldValue = tagFieldInfo.GetValue(data);
                var newValue = ConvertData(destCache, sourceTagCacheStream, destTagCacheStream, sourceStream, destStream, sourceResourceCache, destResourceCache, oldValue);
                tagFieldInfo.SetValue(data, newValue);
            }

            return data;
        }
    }
}
