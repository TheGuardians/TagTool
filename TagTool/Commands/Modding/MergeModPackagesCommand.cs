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
        private Dictionary<int, int> TagMapping = new Dictionary<int, int>();

        private int NextTagIndex = 0;

        private readonly  int HOTagCount;

        public MergeModPackagesCommand(HaloOnlineCacheContext cacheContext) :
            base(false,

                "MergeModPackages",
                "Merge 2 mod packages (.pak) files. File 1 has priority over File2. \n",

                "MergeModPackages <File 1> <File 2>",

                "Merge 2 mod packages (.pak) files. File 1 has priority over File2. \n")
        {
            CacheContext = cacheContext;
            HOTagCount = CacheContext.TagCache.Index.Count;
            NextTagIndex = HOTagCount;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return false;

            var modPackage1 = new ModPackageSimplified(new FileInfo(args[0]));
            var modPackage2 = new ModPackageSimplified(new FileInfo(args[1]));

            var resultPackage = new ModPackageSimplified();

            CacheContext.CreateTagCache(resultPackage.TagsStream);
            resultPackage.Tags = new TagCache(resultPackage.TagsStream, new Dictionary<int, string>());

            CacheContext.CreateResourceCache(resultPackage.ResourcesStream);
            resultPackage.Resources = new ResourceCache(resultPackage.ResourcesStream);

            // allocate all tags for mod package
            int maxTagCount = Math.Max(modPackage1.Tags.Index.Count, modPackage2.Tags.Index.Count);
            for(int i = 0; i < maxTagCount; i++)
                resultPackage.Tags.AllocateTag();

            //
            // Merge tags that overwrite existing tags
            //

            // Assume current cache has no mods
            for (int i = 0; i < CacheContext.TagCache.Index.Count; i++)
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
            resultPackage.Metadata = modPackage1.Metadata;
            resultPackage.Save(new FileInfo($"merge_test_{args[0]}"));

            return true;
        }


        private CachedTagInstance ConvertCachedTagInstance(ModPackageSimplified sourceModPack, ModPackageSimplified destModPack, CachedTagInstance tag)
        {
            // Determine if tag requires conversion

            // if tag is in the sourceModPack
            if (sourceModPack.Tags.Index[tag.Index] == null)
                return tag;
            else
            {
                var name = sourceModPack.TagNames.ContainsKey(tag.Index) ? sourceModPack.TagNames[tag.Index] : $"0x{tag.Index:X4}";
                // tag has already been converted
                if (TagMapping.ContainsKey(tag.Index))
                {
                    return destModPack.Tags.Index[TagMapping[tag.Index]];   // get the matching tag in the destination package
                }
                else
                {
                    // mimic the tag allocation process 
                    CachedTagInstance newTag;
                    if(tag.Index < HOTagCount)
                    {
                        newTag = destModPack.Tags.Index[tag.Index];
                    }
                    else
                    {
                        newTag = destModPack.Tags.Index[NextTagIndex];
                        NextTagIndex++;
                    }

                    var tagDefinition = CacheContext.Deserialize(sourceModPack.TagsStream, tag);
                    tagDefinition = ConvertData(sourceModPack, destModPack, tagDefinition);
                    CacheContext.Serialize(destModPack.TagsStream, newTag, tagDefinition);

                    if(sourceModPack.TagNames.ContainsKey(tag.Index))
                        destModPack.TagNames.Add(newTag.Index, sourceModPack.TagNames[tag.Index]);

                    TagMapping.Add(tag.Index, newTag.Index);

                    return newTag;
                }
            }
        }

        private object ConvertData(ModPackageSimplified sourceModPack, ModPackageSimplified destModPack, object data)
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

        private PageableResource ConvertPageableResource(ModPackageSimplified sourceModPack, ModPackageSimplified destModPack, PageableResource resource)
        {
            if (resource.Page.Index == -1)
                return resource;

            var resourceData = sourceModPack.Resources.ExtractRaw(sourceModPack.ResourcesStream, resource.Page.Index, resource.Page.CompressedBlockSize);
            resource.Page.Index = destModPack.Resources.Add(destModPack.ResourcesStream, resourceData, out resource.Page.CompressedBlockSize);
            return resource;
        }

        private IList ConvertCollection(ModPackageSimplified sourceModPack, ModPackageSimplified destModPack, IList collection)
        {
            // return early where possible
            if (collection is null || collection.Count == 0)
                return collection;

            for (var i = 0; i < collection.Count; i++)
            {
                var oldValue = collection[i];
                var newValue = ConvertData(sourceModPack, destModPack, oldValue);
                collection[i] = newValue;
            }

            return collection;
        }

        private T ConvertStructure<T>(ModPackageSimplified sourceModPack, ModPackageSimplified destModPack, T data) where T : TagStructure
        {
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(data.GetType(), CacheContext.Version))
            {
                var oldValue = tagFieldInfo.GetValue(data);

                if (oldValue is null)
                    continue;

                var newValue = ConvertData(sourceModPack, destModPack, oldValue);
                tagFieldInfo.SetValue(data, newValue);
            }

            return data;
        }
    }
}
