using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_incident_response", Tag = "sirg", Size = 0xC)]
    public class SoundIncidentResponse : TagStructure
    {
        public List<SoundIncidentResponseDataBlock> Responses;
        
        [TagStructure(Size = 0x14)]
        public class SoundIncidentResponseDataBlock : TagStructure
        {
            public SoundAudienceFlags Audience;
            public SoundAudienceFlags ExcludedAudience;
            public SoundSplitscreenFlags SplitScreenFlags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "sgrp" })]
            public CachedTag Response;
            
            [Flags]
            public enum SoundAudienceFlags : byte
            {
                CausePlayer = 1 << 0,
                CauseTeam = 1 << 1,
                EffectPlayer = 1 << 2,
                EffectTeam = 1 << 3,
                Everyone = 1 << 4
            }
            
            [Flags]
            public enum SoundSplitscreenFlags : byte
            {
                DisableIfSplitScreen = 1 << 0,
                DisableIfNotSplitScreen = 1 << 1,
                DisableIfSplitScreenOnDifferentTeams = 1 << 2,
                DisableIfSplitScreenAllOnSameTeam = 1 << 3
            }
        }
    }
}
