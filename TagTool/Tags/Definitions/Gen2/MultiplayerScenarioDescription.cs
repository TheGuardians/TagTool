using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "multiplayer_scenario_description", Tag = "mply", Size = 0xC)]
    public class MultiplayerScenarioDescription : TagStructure
    {
        public List<MultiplayerScenarioDescriptionItem> MultiplayerScenarios;
        
        [TagStructure(Size = 0x44)]
        public class MultiplayerScenarioDescriptionItem : TagStructure
        {
            /// <summary>
            /// net map info
            /// </summary>
            /// <remarks>
            /// these provide the info required by the UI to load a net map
            /// </remarks>
            public CachedTag DescriptiveBitmap;
            public CachedTag DisplayedMapName;
            [TagField(Length = 32)]
            public string ScenarioTagDirectoryPath; // this is the path to the directory containing the scenario tag file of the same name
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
        }
    }
}

