using System.Collections.Generic;
using TagTool.Cache.Codecs;
using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Cache.Resources
{
    [TagStructure(Name = "cache_file_resource_layout_table", Tag = "play", Size = 0x3C, MaxVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
    [TagStructure(Name = "cache_file_resource_layout_table", Tag = "play", Size = 0x48, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class ResourceLayoutTable : TagStructure
    {
        public List<CodecDefinition> CodecDefinitions;
        public List<ResourceSharedFile> SharedFiles;
        public List<ResourcePage> Pages;
        public List<ResourceSubpageTable> SubpageTables;
        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public List<UnknownBlockMCC> UnknownMCC;
        public List<ResourceSection> Sections;

        [TagStructure(Size=  0xC)]
        public class UnknownBlockMCC
        {
            public uint Unknown0;
            public uint Unknown4;
            public uint Unknown8;
        }
    }
}