using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Cache.Monolithic;
using TagTool.IO;
using System.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Tags;
using TagTool.Common;

namespace TagTool.Commands.Tags
{
    class ImportLooseTagCommand : Command
    {
        GameCacheHaloOnlineBase Cache { get; set; }
        public ImportLooseTagCommand(GameCacheHaloOnlineBase cache) : base(
            false,

            "ImportLooseTag",
            "Import a loose tag from Halo 3 MCC",

            "ImportLooseTag <Tag> <Path>",

            "")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count != 2)
                return new TagToolError(CommandError.ArgCount);

            var path = args[1];

            if (!File.Exists(path))
                return new TagToolError(CommandError.FileNotFound, $"\"{path}\"");

            byte[] tagData;
            using (var inStream = File.OpenRead(path))
            {
                tagData = new byte[inStream.Length];
                inStream.Read(tagData, 0, tagData.Length);
            }

            var singleFileTagReader = new SingleTagFileReader(new PersistChunkReader(new MemoryStream(tagData), Cache.Endianness));

            // read the layout
            var layout = singleFileTagReader.ReadLayout(Cache.Endianness);

            // read and fixup the data
            var FixupContext = new HaloOnlinePersistContext(Cache);
            var newTagData = singleFileTagReader.ReadAndFixupData(0, layout, FixupContext, out uint mainStructOffset);

            var newTagDataReader = new EndianReader(new MemoryStream(newTagData), Cache.Endianness);
            newTagDataReader.SeekTo(mainStructOffset);

            var deserializer = new TagDeserializer(CacheVersion.Halo3Retail, CachePlatform.MCC);

            var definitions = new Cache.Gen3.TagDefinitionsGen3();
            Type looseTagType = null;
            foreach (KeyValuePair<TagGroup, Type> tagType in definitions.Gen3Types)
            {
                if (tagType.Key.Tag == singleFileTagReader.Header.GroupTag)
                {
                    looseTagType = tagType.Value;
                    break;
                }
            }

            if (looseTagType == null)
                return new TagToolError(CommandError.OperationFailed, $"Tag type {singleFileTagReader.Header.GroupTag.ToString()} not valid for gen3 cache!");

            string tagname = args[0];
            if (!tagname.Contains("."))
                tagname += $".{singleFileTagReader.Header.GroupTag}";

            if (Cache.TagCache.TryGetCachedTag(tagname, out var instance))
                return new TagToolError(CommandError.OperationFailed, "Tag name already exists in cache!");

            var info = TagStructure.GetTagStructureInfo(looseTagType, CacheVersion.Halo3Retail, CachePlatform.MCC);
            DataSerializationContext context = new DataSerializationContext(newTagDataReader);
            var result = deserializer.DeserializeStruct(newTagDataReader, context, info);

            //tag resources
            if (layout.ResourceDefinitions.Length > 1)
                new TagToolWarning("Resources supported for tags with only one resource type for the moment");
            else if(FixupContext.TagResourceData.Count > 0 && layout.ResourceDefinitions.Length == 1)
            {
                FixupResources(result, FixupContext, layout, looseTagType);
            }

            var destTag = Cache.TagCache.AllocateTag(looseTagType, args[0].ToString());
            using (var stream = Cache.OpenCacheReadWrite())
                Cache.Serialize(stream, destTag, result);
            Cache.SaveStrings();
            Cache.SaveTagNames();

            Console.WriteLine($"['{destTag.Group.Tag}', 0x{destTag.Index:X4}] {destTag}");

            return true;
        }

        private void FixupResources(object tagDef, HaloOnlinePersistContext FixupContext, TagLayout layout, Type looseTagType)
        {
            for(var i = 0; i < FixupContext.TagResourceData.Count; i++)
            {
                MemoryStream dataStream = new MemoryStream(FixupContext.TagResourceData[i]);
                var chunkReader = new PersistChunkReader(dataStream, Cache.Endianness);        
                var dataReader = new SingleTagFileDataReader(0, layout, FixupContext);
                uint offset = dataReader.ReadResource(chunkReader, out MemoryStream outStream, layout.ResourceDefinitions[0]);

                var newResourceReader = new EndianReader(outStream, Cache.Endianness);
                newResourceReader.SeekTo(offset);
                DataSerializationContext resourceContext = new DataSerializationContext(newResourceReader);
                var deserializer = new TagDeserializer(CacheVersion.Halo3Retail, CachePlatform.MCC);

                switch (looseTagType.Name)
                {
                    case "ModelAnimationGraph":
                        var resourceInfo = TagStructure.GetTagStructureInfo(typeof(ModelAnimationTagResource), CacheVersion.Halo3Retail, CachePlatform.MCC);              
                        var resourceDef = deserializer.DeserializeStruct(newResourceReader, resourceContext, resourceInfo);
                        ModelAnimationGraph jmad = (ModelAnimationGraph)tagDef;
                        ModelAnimationTagResource resource = (ModelAnimationTagResource)resourceDef;
                        resource.GroupMembers.AddressType = CacheAddressType.Definition;
                        jmad.ResourceGroups[i].ResourceReference = Cache.ResourceCache.CreateModelAnimationGraphResource(resource);
                        break;
                    default:
                        new TagToolWarning($"'{layout.ResourceDefinitions[0].Name}' import not yet supported!");
                        break;
                }
            }
            
        }

        public class HaloOnlinePersistContext : ISingleTagFilePersistContext
        {
            public GameCache Cache;
            public List<byte[]> TagResourceData = new List<byte[]>();
            public HaloOnlinePersistContext(GameCache cache)
            {
                Cache = cache;
            }

            public void AddTagResource(DatumHandle resourceHandle, TagResourceXSyncState state)
            {

            }

            public void AddTagResourceData(byte[] data)
            {
                TagResourceData.Add(data);
            }

            public StringId AddStringId(string stringvalue)
            {
                var stringId = Cache.StringTable.GetStringId(stringvalue);
                if (stringId == StringId.Invalid)
                    stringId = Cache.StringTable.AddString(stringvalue);
                return stringId;
            }

            public CachedTag GetTag(Tag groupTag, string name)
            {
                if (Cache.TagCache.TryGetCachedTag($"{name}.{groupTag}", out CachedTag tag))
                    return tag;
                return null;
            }
        }
    }
}