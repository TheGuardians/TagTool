using System;
using TagTool.Cache;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "render_water_ripple", Tag = "rwrd", Size = 0x4C, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "render_water_ripple", Tag = "rwrd", Size = 0x50, MinVersion = CacheVersion.Halo3ODST)]
    public class RenderWaterRipple : TagStructure
	{
        public TypeFlagsValue TypeFlags;

        //
        // INITIAL SETTINGS
        //

        public float Radius;
        public float Amplitude;
        public float SpreadSpeed;
        public float SpreadBias;
        public float PositionRandomRange;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public float MaxVisibilityDistance;

        //
        // LIFE SETTINGS
        //    What happens during the ripple life. Damping should be always larger than 0.
        //

        public float DurationMax;
        public float DurationMin;
        public float RisePeriodRatio;

        public FunctionValue RiseFunction;
        public FunctionValue DescendFunction;

        //
        // PENDULUM SETTINGS
        //    Only valid in case of the predulum flag has been checked.
        //

        public float PhaseRevolutionSpeed;
        public float PhaseRepeatAlongRadius;

        //
        // SHAPE TRANSITION
        //    Interpolate between shapes in ripple pattern array.
        //

        public float PatternStartIndex;
        public float PatternEndIndex;

        public FunctionValue PatternTransition;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused1 = new byte[2];

        //
        // FOAM
        //    Quick-flashed foam.
        //

        public float FoamOutRadius;
        public float FoamFadeRadius;
        public float FoamDuration;

        public FunctionValue FoamFadeFunction;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused2 = new byte[2];

        [Flags]
        public enum TypeFlagsValue : int
        {
            None = 0,
            RippleDriftedByFlow = 1 << 0,
            AmplitudeChangedByPendulumFunction = 1 << 1,
            DisplayFlashFoam = 1 << 2,
            FoamSizeDefinedInGameUnit = 1 << 3
        }

        public enum FunctionValue : short
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