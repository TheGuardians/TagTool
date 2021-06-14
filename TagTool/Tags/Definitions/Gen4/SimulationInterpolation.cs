using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "simulation_interpolation", Tag = "siin", Size = 0xF4)]
    public class SimulationInterpolation : TagStructure
    {
        // How much rope clients have WRT their controlled object of this type. They are free to ignore host updates within
        // this tolerance.
        public float PositionClientIgnoreTolerance; // WU
        // If angular speed exceeds this threshold, we will ignore rotational updates (because it's spinning so fast that we
        // can't possibly do a good job w/ any kind of interpolation - instead, just wait for it to settle down a bit).  Note
        // that if a warp threshold is exceeded, rotation will be warped regardless of the value of this parameter.
        public float AngularSpeedThresholdForTemporarilyIgnoringRotationUpdates; // degrees per second
        // Threshold of position error beyond which we will warp instead of interpolating.
        public float PositionWarpThreshold; // WU
        // Separated for bipeds, for whom z discrepancies are more permissible because of jumping. If you don't want a special
        // lower XY threshold, just set this to a very high number.
        public float PositionWarpThresholdXy; // WU
        // Threshold of rotation error beyond which we will warp instead of interpolating.  Set this to 180.0 if you don't
        // want to warp based on rotational deviation (may be important for objects that often spin very fast, e.g. grenades
        // or garbage bits).
        public float RotationWarpThreshold; // degrees
        // All speeds in here are WU/s
        public SingleDomainConfigurationStruct PositionWhileControlled;
        // All speeds in here are degrees/s
        public SingleDomainConfigurationStruct RotationWhileControlled;
        // All speeds in here are WU/s
        public SingleDomainConfigurationStruct PositionWhileUncontrolled;
        // All speeds in here are degrees/s
        public SingleDomainConfigurationStruct RotationWhileUncontrolled;
        
        [TagStructure(Size = 0x38)]
        public class SingleDomainConfigurationStruct : TagStructure
        {
            // Below this threshold we use velocity bumps.
            public float DiscrepancyThresholdAboveWhichWeUseBlending; // WU or degrees
            // When our velocity is below this threshold, we will consider using a blend to minimize at-rest error.
            public float ComingToRestSpeed; // WU/s or degrees/s
            // We will use a blend when our speed is below the coming_to_rest_speed and our error is greater than this.
            public float ComingToRestMaximumIgnorableError; // WU or degrees
            public SingleDomainVelocityBumpsConfigurationStruct VelocityBumps;
            public SingleDomainBlendingConfigurationStruct Blending;
            
            [TagStructure(Size = 0x14)]
            public class SingleDomainVelocityBumpsConfigurationStruct : TagStructure
            {
                // Fraction of delta that becomes the velocity bump
                public float VelocityScale;
                // Minimum size of any given velocity bump
                public float VelocityMin; // WU or degrees per second
                // Maximum size of any given velocity bump
                public float VelocityMax; // WU or degrees per second
                // The new velocity must differ from the old velocity by less than this to allow suppression.
                public float VelocityDifferenceIgnoreThreshold; // WU or degrees per second
                // The new position/rotation must differ from the old position/rotation by less than this to allow suppression.
                public float VelocityDifferenceAbsoluteIgnoreThreshold; // WU or degrees
            }
            
            [TagStructure(Size = 0x18)]
            public class SingleDomainBlendingConfigurationStruct : TagStructure
            {
                // Approximate minimum speed for this object (either controlled or uncontrolled).
                public float MinObjectSpeed; // WU or degrees per second
                // Approximate maximum speed for this object (either controlled or uncontrolled).
                public float MaxObjectSpeed; // WU or degrees per second
                // Fraction of misprediction error consumed each tick if object is traveling at or below min_object_speed.  Linterp is
                // executed at intermediate object speeds.
                public float FractionAtMinObjectSpeed;
                // Fraction of misprediction error consumed each tick if object is traveling at or above max_object_speed.  Linterp is
                // executed at intermediate object speeds.
                public float FractionAtMaxObjectSpeed;
                // Minimum misprediction error consumption speed if object is at or below min_object_speed.  Linterp is executed at
                // intermediate object speeds.
                public float MinSpeedAtMinObjectSpeed; // WU or degrees per second
                // Minimum misprediction error consumption speed if object is at or above max_object_speed. Linterp is executed at
                // intermediate object speeds.
                public float MinSpeedAtMaxObjectSpeed; // WU or degrees per second
            }
        }
    }
}
