using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "multiplayer_scenario_description", Tag = "mply", Size = 0x8)]
    public class MultiplayerScenarioDescription : TagStructure
    {
        public List<ScenarioDescriptionBlock> MultiplayerScenarios;
        
        [TagStructure(Size = 0x34)]
        public class ScenarioDescriptionBlock : TagStructure
        {
            /// <summary>
            /// these provide the info required by the UI to load a net map
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DescriptiveBitmap;
            [TagField(ValidTags = new [] { "unic" })]
            public CachedTag DisplayedMapName;
            /// <summary>
            /// this is the path to the directory containing the scenario tag file of the same name
            /// </summary>
            [TagField(Length = 32)]
            public string ScenarioTagDirectoryPath;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
        }
    }
}

