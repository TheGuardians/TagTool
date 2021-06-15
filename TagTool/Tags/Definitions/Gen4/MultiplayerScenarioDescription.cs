using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "multiplayer_scenario_description", Tag = "mply", Size = 0xC)]
    public class MultiplayerScenarioDescription : TagStructure
    {
        public List<ScenarioDescriptionBlock> MultiplayerScenarios;
        
        [TagStructure(Size = 0x44)]
        public class ScenarioDescriptionBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DescriptiveBitmap;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag DisplayedMapName;
            // this is the path to the directory containing the scenario tag file of the same name
            [TagField(Length = 32)]
            public string ScenarioTagDirectoryPath;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
    }
}
