using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "simulated_input", Tag = "sidt", Size = 0x80)]
    public class SimulatedInput : TagStructure
    {
        public SimulatedInputStickStruct Move;
        public SimulatedInputStickStruct Look;
        
        [TagStructure(Size = 0x40)]
        public class SimulatedInputStickStruct : TagStructure
        {
            public SimulatedInputFlags Flags;
            public DirectionTypeEnum ResponseType;
            public MappingTypeEnum MappingType;
            public Bounds<float> Angle; // degrees
            public MappingFunction Mapping;
            public float Duration; // seconds
            // if >0.f, accumulated inpulse will be 'undone' over the give time span
            public float InverseDuration; // seconds
            // 15.0 would randomly adjust length of accumulated impulse +/-15%
            public float InverseRandomLength; // percent
            // apply random adjustment to direction of accumulated impulse
            public Bounds<float> InverseRandomAngle; // degrees
            // linear multiplier of zoom that increases effect; computed for no change at zoom 1
            public float LinearZoomPenalty;
            // multiplier to increase effect proportional to square root of zoom; computed for no change at zoom 1
            public float SquareRootZoomPenalty;
            
            [Flags]
            public enum SimulatedInputFlags : uint
            {
                // Always start with 1.0 instead of any value from damage multiplier value
                IgnoreBaseScaler = 1 << 0,
                // Skip this scaler
                IgnoreAreaControlFalloff = 1 << 1,
                // Skip this scaler
                IgnoreElapsedTimeFunctionScaler = 1 << 2,
                // Skip this scaler
                IgnoreZoomScaler = 1 << 3,
                // Skip this scaler
                IgnoreTickDeltaSecsScaler = 1 << 4
            }
            
            public enum DirectionTypeEnum : short
            {
                AimVector,
                HitVector,
                InverseHitVector,
                AttackerOriginToVictimOrigin,
                VictimOriginToAttackerOrigin
            }
            
            public enum MappingTypeEnum : short
            {
                TopDown,
                ScreenSpace
            }
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
