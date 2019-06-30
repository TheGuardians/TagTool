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
using TagTool.Tags.Resources;

namespace TagTool.Commands.Modding
{
    class ApplyModPackageCommand : Command
    {
        private HaloOnlineCacheContext CacheContext { get; }

        private Dictionary<int, int> TagMapping = new Dictionary<int, int>();

        private int MagicNumber = 0x5AF7;

        private Stream CacheStream;

        public ApplyModPackageCommand(HaloOnlineCacheContext cacheContext) :
            base(false,

                "ApplyModPackage",
                "Apply a mod package to the current cache. \n",

                "ApplyModPackage <File>",

                "Apply a mod package to the current cache. \n")
        {
            CacheContext = cacheContext;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 1)
                return false;

            var filePath = args[0];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} does not exist!");
                return false;
            }

            CacheStream = CacheContext.OpenTagCacheReadWrite();

            var modPackage = new ModPackage(new FileInfo(filePath));

            for (int i = 0; i < modPackage.Tags.Index.Count; i++)
            {
                var modTag = modPackage.Tags.Index[i];

                if(modTag != null)
                {
                    if(!TagMapping.ContainsKey(modTag.Index))
                        ConvertCachedTagInstance(modPackage, modTag);
                }
            }

            // fixup map files

            foreach (var mapFile in modPackage.CacheStreams)
            {
                using (var reader = new EndianReader(mapFile))
                {
                    MapFile map = new MapFile(reader);

                    var modIndex = map.Header.GetScenarioTagIndex();
                    TagMapping.TryGetValue(modIndex, out int newScnrIndex);
                    map.Header.SetScenarioTagIndex(newScnrIndex);
                    var mapName = map.Header.GetName();

                    var mapPath = $"{CacheContext.Directory.FullName}\\{mapName}.map";
                    var file = new FileInfo(mapPath);
                    var fileStream = file.OpenWrite();
                    using(var writer = new EndianWriter(fileStream, map.EndianFormat))
                    {
                        map.Write(writer);
                    }
                }
            }

            CacheStream.Close();
            CacheStream.Dispose();
            CacheContext.SaveTagNames();
            return true;
        }


        private CachedTagInstance ConvertCachedTagInstance(ModPackage modPack, CachedTagInstance modTag)
        {
            // Determine if tag requires conversion
            if (modPack.Tags.Index[modTag.Index] == null)
                return CacheContext.TagCache.Index[modTag.Index];   // references an HO tag
            else
            {
                // tag has already been converted
                if (TagMapping.ContainsKey(modTag.Index))
                {
                    return CacheContext.TagCache.Index[TagMapping[modTag.Index]];   // get the matching tag in the destination package
                }
                else
                {
                    CachedTagInstance newTag;
                    if (modTag.Index <= MagicNumber)
                        newTag = CacheContext.GetTag(modTag.Index);
                    else
                    {
                        newTag = CacheContext.TagCache.AllocateTag(modTag.Group);
                        newTag.Name = modTag.Name;
                    }

                    TagMapping.Add(modTag.Index, newTag.Index);
                    var tagDefinition = CacheContext.Deserialize(new ModPackageTagSerializationContext(modPack.TagsStream, CacheContext, modPack, modTag), TagDefinition.Find(modTag.Group.Tag));
                    tagDefinition = ConvertData(modPack, tagDefinition);
                    CacheContext.Serialize(CacheStream, newTag, tagDefinition);


                    return newTag;
                }
            }
        }

        private object ConvertData(ModPackage modPack, object data)
        {

            var type = data.GetType();
            
            switch (data)
            {
                case null:
                case string _:
                case ValueType _:
                    return data;
                case PageableResource resource:
                    return ConvertPageableResource(modPack, resource);
                case TagStructure structure:
                    return ConvertStructure(modPack, structure);
                case IList collection:
                    return ConvertCollection(modPack, collection);
                case CachedTagInstance tag:
                    return ConvertCachedTagInstance(modPack, tag);
            }

            return data;
        }

        private PageableResource ConvertPageableResource(ModPackage modPack, PageableResource resource)
        {
            if (resource.Page.Index == -1)
                return resource;

            var resourceStream = new MemoryStream();
            modPack.Resources.Decompress(modPack.ResourcesStream, resource.Page.Index, resource.Page.CompressedBlockSize, resourceStream);
            resourceStream.Position = 0;
            resource.ChangeLocation(ResourceLocation.ResourcesB);
            resource.Page.OldFlags &= ~OldRawPageFlags.InMods;
            CacheContext.AddResource(resource, resourceStream);
            
            return resource;
        }

        private IList ConvertCollection(ModPackage modPack, IList collection)
        {
            // return early where possible
            if (collection is null || collection.Count == 0)
                return collection;

            for (var i = 0; i < collection.Count; i++)
            {
                var oldValue = collection[i];
                var newValue = ConvertData(modPack, oldValue);
                collection[i] = newValue;
            }

            return collection;
        }

        private T ConvertStructure<T>(ModPackage modPack, T data) where T : TagStructure
        {
            foreach (var tagFieldInfo in TagStructure.GetTagFieldEnumerable(data.GetType(), CacheContext.Version))
            {
                var oldValue = tagFieldInfo.GetValue(data);

                if (oldValue is null)
                    continue;

                var newValue = ConvertData(modPack, oldValue);
                tagFieldInfo.SetValue(data, newValue);
            }

            return data;
        }
    }
}
