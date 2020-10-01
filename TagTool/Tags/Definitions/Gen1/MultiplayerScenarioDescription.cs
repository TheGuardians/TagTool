using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "multiplayer_scenario_description", Tag = "mply", Size = 0xC)]
    public class MultiplayerScenarioDescription : TagStructure
    {
        public List<ScenarioDescriptionBlock> MultiplayerScenarios;
        
        [TagStructure(Size = 0x44)]
        public class ScenarioDescriptionBlock : TagStructure
        {
            /// <summary>
            /// these provide the info required by the UI to load a net map
            /// </summary>
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DescriptiveBitmap;
            [TagField(ValidTags = new [] { "ustr" })]
            public CachedTag DisplayedMapName;
            /// <summary>
            /// this is the path to the directory containing the scenario tag file of the same name
            /// </summary>
            [TagField(Length = 32)]
            public string ScenarioTagDirectoryPath;
            [TagField(Length = 0x4)]
            public byte[] Padding;
        }
    }
}

