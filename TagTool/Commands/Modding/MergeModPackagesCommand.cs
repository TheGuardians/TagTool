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

            // Assume current cache has no mods
            for(int i = 0; i < CacheContext.TagCache.Index.Count; i++)
            {
                var tag1 = modPackage1.Tags.Index[i];
                var tag2 = modPackage1.Tags.Index[i];

                var name1 = modPackage1.TagNames.ContainsKey(i) ? modPackage1.TagNames[i] : $"0x{i:X4}";
                var name2 = modPackage2.TagNames.ContainsKey(i) ? modPackage2.TagNames[i] : $"0x{i:X4}";

                if(tag1 != null)
                {
                    ConvertCachedTagInstance(modPackage1, resultPackage, tag1);
                }
                else if(tag2 != null)
                {
                    ConvertCachedTagInstance(modPackage2, resultPackage, tag2);
                }
                else
                {
                    resultPackage.Tags.AllocateTag();
                    continue;
                }

            }

            //
            // Merge each mod packages new tags. If tag names compete, package 1 has priority
            //

            for(int i = CacheContext.TagCache.Index.Count; i < modPackage1.Tags.Index.Count; i++)
            {
                var tag1 = modPackage1.Tags.Index[i];
                if (tag1 != null)
                {
                    ConvertCachedTagInstance(modPackage1, resultPackage, tag1);
                }
            }

            for (int i = CacheContext.TagCache.Index.Count; i < modPackage2.Tags.Index.Count; i++)
            {
                var tag2 = modPackage2.Tags.Index[i];
                if (tag2 != null)
                {
                    ConvertCachedTagInstance(modPackage2, resultPackage, tag2);
                }
            }

            //
            // Create resulting mod package header and info
            //

            resultPackage.Header = modPackage1.Header;

            resultPackage.Save(new FileInfo($"merge_test_{args[0]}"));

            /*
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


            */


            return true;
        }


        private CachedTagInstance ConvertCachedTagInstance(ModPackage sourceModPack, ModPackage destModPack, CachedTagInstance tag)
        {
            var name = sourceModPack.TagNames.ContainsKey(tag.Index) ? sourceModPack.TagNames[tag.Index] : $"0x{tag.Index:X4}";
            // tag has already been converted
            if ( destModPack.TagNames.ContainsValue(name))
            {
                return destModPack.Tags.Index[destModPack.TagNames.FirstOrDefault(x => x.Value == name).Key];       // Very slow, create reverse dictionary or another structure
            }
            else
            {
                var newTag = destModPack.Tags.AllocateTag(tag.Group, tag.Name);
                var tagDefinition = CacheContext.Deserialize(sourceModPack.TagsStream, tag);
                tagDefinition = ConvertData(sourceModPack, destModPack, tagDefinition);
                CacheContext.Serialize(destModPack.TagsStream, newTag, tagDefinition);
                destModPack.TagNames.Add(newTag.Index, name);
                return newTag;
            }
        }

        private object ConvertData(ModPackage sourceModPack, ModPackage destModPack, object data)
        {

            var type = data.GetType();
            
            switch (data)
            {
                case null:
                case string _:
                case ValueType _:
                    return data;
                case PageableResource resource:
                    return ConvertPageableResource(sourceModPack, destModPack, resource);
                case TagStructure structure:
                    return ConvertStructure(sourceModPack, destModPack, structure);
                case IList collection:
                    return ConvertCollection(sourceModPack, destModPack, collection);
                case CachedTagInstance tag:
                    return ConvertCachedTagInstance(sourceModPack, destModPack, tag);
            }

            return data;
        }

        private PageableResource ConvertPageableResource(ModPackage sourceModPack, ModPackage destModPack, PageableResource resource)
        {
            var resourceData = sourceModPack.Resources.ExtractRaw(sourceModPack.TagsStream, resource.Page.Index, resource.Page.CompressedBlockSize);
            resource.Page.Index = destModPack.Resources.AddRaw(destModPack.TagsStream, resourceData);
            return resource;
        }

        private IList ConvertCollection(ModPackage sourceModPack, ModPackage destModPack, IList collection)
        {
            if (collection.GetType().GetElementType().IsPrimitive)
                return collection;

            for (var i = 0; i < collection.Count; i++)
            {
                var oldValue = collection[i];
                var newValue = ConvertData(sourceModPack, destModPack, destModPack.TagsStream);
                collection[i] = newValue;
            }

            return collection;
        }

        private T ConvertStructure<T>(ModPackage sourceModPack, ModPackage destModPack, T data) where T : TagStructure
        {
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(typeof(T), CacheContext.Version))
            {
                var oldValue = tagFieldInfo.GetValue(data);
                var newValue = ConvertData(sourceModPack, destModPack, oldValue);
                tagFieldInfo.SetValue(data, newValue);
            }

            return data;
        }
    }
}
