using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "SuppressedIncident", Tag = "sigd", Size = 0xC)]
    public class SuppressedIncident : TagStructure
    {
        public List<SuppressedIncidentBlock> SuppressedIncidents;
        
        [TagStructure(Size = 0x8)]
        public class SuppressedIncidentBlock : TagStructure
        {
            public StringId IncidentName;
            public SuppressedIncidentFlags SuppressionType;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            [Flags]
            public enum SuppressedIncidentFlags : byte
            {
                SuppressCausePlayerGameEngineEvent = 1 << 0,
                SuppressEffectPlayerGameEngineEvent = 1 << 1,
                SuppressCauseTeamGameEngineEvent = 1 << 2,
                SuppressEffectTeamGameEngineEvent = 1 << 3,
                SuppressMedalDisplay = 1 << 4,
                SuppressMedalStats = 1 << 5,
                SuppressFanfare = 1 << 6,
                SuppressAudio = 1 << 7
            }
        }
    }
}
