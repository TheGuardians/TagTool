using System.Collections.Generic;
using System;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "scenario_hs_source_file", Tag = "hsc*", Size = 0x44)]
    public class ScenarioHsSourceFile : TagStructure
    {
        [TagField(Length = 32)]
        public string Name;
        public byte[] Source;
        public List<HsReferencesBlock> ExternalReferences;
        public HsSourceFileFlags Flags;

        [Flags]
        public enum HsSourceFileFlags : uint
        {
            GeneratedAtRuntime = 1 << 0,
            AiFragments = 1 << 1
        }

        [TagStructure(Size = 0x10)]
        public class HsReferencesBlock : TagStructure
        {
            public CachedTag Reference;
        }
    }
}