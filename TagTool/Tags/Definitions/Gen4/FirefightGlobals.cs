using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "firefight_globals", Tag = "ffgd", Size = 0x2C)]
    public class FirefightGlobals : TagStructure
    {
        public List<FirefightwaveTemplatesBlockStruct> WaveTemplates;
        [TagField(ValidTags = new [] { "coop" })]
        public CachedTag CoOpSpawning;
        [TagField(ValidTags = new [] { "ssdf" })]
        public CachedTag InfluencerDefaultSpawnSettings;
        
        [TagStructure(Size = 0x14)]
        public class FirefightwaveTemplatesBlockStruct : TagStructure
        {
            public StringId Name;
            [TagField(ValidTags = new [] { "wave" })]
            public CachedTag WaveTemplate;
        }
    }
}
