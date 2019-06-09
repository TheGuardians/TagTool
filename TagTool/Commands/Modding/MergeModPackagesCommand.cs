using System;
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
