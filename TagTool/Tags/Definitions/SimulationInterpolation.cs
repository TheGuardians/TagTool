using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "simulation_interpolation", Tag = "siin", Size = 0xF4)]
    public class SimulationInterpolation : TagStructure
    {
        // High level settings
        public float PositionClientIgnoreTolerance;
        public float AngularSpeedThreshold; // AngularSpeedThresholdForTemporarilyIgnoringRotationUpdates

        // Warp thresholds
        public float PositionWarpThreshold;
        public float PositionWarpThresholdXY;
        public float PositionWarpThresholdRotation;

        public SingleDomainInterpolation PositionWhileControlled;
        public SingleDomainInterpolation RotationWhileControlled;

        public SingleDomainInterpolation PositionWhileNotControlled;
        public SingleDomainInterpolation RotationWhileNotControlled;

        [TagStructure(Size = 0x38)]
        public class SingleDomainInterpolation : TagStructure
        {
            public float DiscrepencyThreshold; // DiscrepencyThresholdAboveWhichWeUseBlending
            public float ComingToRestSpeed;
            public float ComingToRestMaximumIgnoreableError;
            public VelocityBumpsStructure VelocityBumps;
            public BlendingStructure Blending;

            [TagStructure(Size = 0x14)]
            public class VelocityBumpsStructure : TagStructure
            {
                public float VelocityScale;
                public Bounds<float> Velocity;
                public float VelocityDifferenceIgnoreThreshold;
                public float VelocityDifferenceAbsoluteIgnoreThreshold;
            }

            [TagStructure(Size = 0x18)]
            public class BlendingStructure : TagStructure
            {
                public Bounds<float> ObjectSpeed;
                public float FractionAtMinimumObjectSpeed;
                public float FractionAtMaximumObjectSpeed;
                public float MinimumSpeedAtMinimumObjectSpeed;
                public float MinimumSpeedAtMaximumObjectSpeed;
            }
        }
    }
}