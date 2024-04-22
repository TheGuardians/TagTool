using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "render_water_ripple", Tag = "rwrd", Size = 0x50)]
    public class RenderWaterRipple : TagStructure
    {
        public RippleBehaviorFlags Flags;
        public float InitialRadius;
        public float InitialAmplitude;
        public float SpreadSpeed;
        public float SpeedBias;
        public float PositionRandomRange;
        public float MaxVisibilityDistance;
        public float DurationMax;
        public float DurationMin;
        public float RisePeriodRatio;
        public TransitionFunctionEnum RiseFunction;
        public TransitionFunctionEnum DescendFunction;
        public float PhaseRevolutionSpeed;
        public float PhaseRepeatAlongRadius;
        public float PatternStartIdx;
        public float PatternEndIdx;
        public TransitionFunctionEnum PatternTransition;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public float FoamOutRadius;
        public float FoamFadeDistance;
        public float FoamDuration;
        public TransitionFunctionEnum FoamFadeFunction;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        
        [Flags]
        public enum RippleBehaviorFlags : uint
        {
            RippleDriftedByFlow = 1 << 0,
            AmplitudeChangedByPendulumFunction = 1 << 1,
            DisplayFlashFoam = 1 << 2,
            FoamSizeDefinedInGameUnit = 1 << 3
        }
        
        public enum TransitionFunctionEnum : short
        {
            Linear,
            Early,
            VeryEarly,
            Late,
            VeryLate,
            Cosine,
            One,
            Zero
        }
    }
}
