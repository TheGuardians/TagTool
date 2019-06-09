using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
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

                CachedTagInstance tag = null;

                if (useTag1 && useTag2)
                {
                    var tag1 = modPackage1.Tags.Index[i];
                    var tag2 = modPackage2.Tags.Index[i];

                    if (tag1 != null && tag2 != null && !tag2.IsInGroup(tag1.Group) || name1 != name2)
                        throw new FormatException();

                    if (tag1 != null)
                    {
                        tag = resultPackage.Tags.AllocateTag(tag1.Group, tag1.Name);
                        useTag2 = false;
                    }
                    else
                    {
                        tag = resultPackage.Tags.AllocateTag(tag2.Group, tag2.Name);
                        useTag1 = false;
                    }
                }

                if (tag == null)
                    tag = resultPackage.Tags.AllocateTag();

                if (useTag1)
                {
                    resultPackage.Tags.SetTagData(resultPackage.TagsStream, tag,
                        modPackage1.Tags.ExtractTag(modPackage1.TagsStream, tag));

                    //
                    // TODO: extract and update any resources from modPackage1 here
                    //
                }
                else if (useTag2)
                {
                    resultPackage.Tags.SetTagData(resultPackage.TagsStream, tag,
                        modPackage2.Tags.ExtractTag(modPackage1.TagsStream, tag));

                    //
                    // TODO: extract and update any resources from modPackage2 here
                    //
                }
                else
                {
                    resultPackage.Tags.UpdateTagOffsets(
                        new BinaryWriter(resultPackage.TagsStream, default, true));
                }
            }

            return true;
        }

        private CachedTagInstance UpdateTag(TagCache sourceCache, TagCache destCache, Stream sourceStream, Stream destStream, ResourceCache sourceResourceCache, ResourceCache destResourceCache, CachedTagInstance tag)
        {
            var newTag = destCache.AllocateTag(tag.Group, tag.Name);

            //deserialize tag from it's source cache
            var definition = (TagStructure)CacheContext.Deserialize(sourceStream, tag);
            // deserialization will update indices so deserialize any tag in the tree of this.
            definition = ConvertStructure(sourceCache, destCache, sourceStream, destStream, sourceResourceCache, destResourceCache, definition, definition);
            CacheContext.Serialize(destStream, newTag, definition);

            foreach(var resourcePointerOffset in tag.ResourcePointerOffsets)
            {
                if (resourcePointerOffset == 0 || newTag.ResourcePointerOffsets.Contains(resourcePointerOffset))
                    continue;

                // add resources somehow?
            }

            return newTag;
        }
        
        private T ConvertStructure<T>(TagCache sourceCache, TagCache destCache, Stream sourceStream, Stream destStream, ResourceCache sourceResourceCache, ResourceCache destResourceCache, T data, object definition) where T : TagStructure
        {
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(data.GetType(), CacheContext.Version))
            {
                if(tagFieldInfo.FieldType == typeof(CachedTagInstance))
                {
                    var newValue = UpdateTag(sourceCache, destCache, sourceStream, destStream, sourceResourceCache, destResourceCache, (CachedTagInstance)tagFieldInfo.GetValue(data));
                    tagFieldInfo.SetValue(data, newValue);
                }
            }

            return data;
        }

    }
}
