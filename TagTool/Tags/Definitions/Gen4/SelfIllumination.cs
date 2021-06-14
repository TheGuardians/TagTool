using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "self_illumination", Tag = "sict", Size = 0xC)]
    public class SelfIllumination : TagStructure
    {
        public List<SelfIlluminationState> States;
        
        [TagStructure(Size = 0x4C)]
        public class SelfIlluminationState : TagStructure
        {
            public StringId Name;
            public SelfIlluminationFlags Flags;
            // Length of illumination animation in seconds.
            public float Length;
            // Used to automatically transition to another state when finished.
            public StringId NextState;
            // Animates intensity over time. 0 - Min Intensity.  1 - Max Intensity
            public ScalarFunctionNamedStruct Intensity;
            // Animates color over time.  0 - White.  1 - Full Color
            public ScalarFunctionNamedStruct Color;
            // Animates On/Off state over time. 0 - Off. 1 - On
            public ScalarFunctionNamedStruct Activation;
            
            [Flags]
            public enum SelfIlluminationFlags : uint
            {
                Looping = 1 << 0,
                TriggeredByAiState = 1 << 1,
                TriggeredByAnimState = 1 << 2,
                TriggeredByDialog = 1 << 3
            }
            
            [TagStructure(Size = 0x14)]
            public class ScalarFunctionNamedStruct : TagStructure
            {
                public MappingFunction Function;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
    }
}
