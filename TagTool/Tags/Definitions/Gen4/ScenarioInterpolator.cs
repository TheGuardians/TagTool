using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "scenario_interpolator", Tag = "sirp", Size = 0xC)]
    public class ScenarioInterpolator : TagStructure
    {
        public List<ScenarioInterpolatorDataBlock> Interpolators;
        
        [TagStructure(Size = 0x34)]
        public class ScenarioInterpolatorDataBlock : TagStructure
        {
            public ScenarioInterpolatorFlags Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId Name;
            public ScalarFunctionNamedStruct Function;
            public float Duration; // seconds
            public ScenarioInterpolatorResetValueEnum MapResetType;
            public float CustomMapResetValue;
            public ScenarioInterpolatorStartValueEnum StartType;
            public ScenarioInterpolatorStopValueEnum StopType;
            public float CustomStopValue;
            
            [Flags]
            public enum ScenarioInterpolatorFlags : ushort
            {
                // will continue to play until stopped.  if checked, no wrap should also be checked
                Loops = 1 << 0,
                // will be started when the map loads.  good for looping interpolators
                AlwaysActive = 1 << 1
            }
            
            public enum ScenarioInterpolatorResetValueEnum : int
            {
                Initial,
                Ending,
                Custom
            }
            
            public enum ScenarioInterpolatorStartValueEnum : int
            {
                Initial,
                Unchanged
            }
            
            public enum ScenarioInterpolatorStopValueEnum : int
            {
                Initial,
                Hold,
                Ending,
                Custom
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
