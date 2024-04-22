using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "giant", Tag = "gint", Size = 0x38)]
    public class Giant : Unit
    {
        public GiantDefinitionFlags GiantFlags;
        public float AccelTime; // acceleration time in seconds
        public float DecelTime; // deceleration time in seconds
        public float MinimumSpeedScale; // as slow as we get
        // overall animation speed scale
        public float SpeedScale;
        public float ElevationChangeRate; // scale per update
        // how far to push the physical body
        public float ProxyBodyOffset; // wu
        // the physical body must move this fraction of the offset for it to make the scarab move.  High values make the
        // scarab more resistant to noise in the physics simulation, but less responsive.
        public float ProxyBodyDeadzone;
        // how many world-units up/down a leg can reach from the ground elevation under the scarab
        public float MaxVerticalReach; // wu
        // how far around a foot to search for ground targets to stomp.  Set to 0 to not search for targets
        public float FootTargetRadius; // wu
        public List<GiantBuckleParametersBlock> BuckleSettings;
        public float AnkleIkScale; // lower values drop the ankles towards the ground when computing ik
        
        [Flags]
        public enum GiantDefinitionFlags : uint
        {
        }
        
        [TagStructure(Size = 0x5C)]
        public class GiantBuckleParametersBlock : TagStructure
        {
            public float LowerTime; // seconds to reach ground
            public SliderMovementPatterns LowerCurve;
            public float RaiseTime; // seconds to recover
            public SliderMovementPatterns RaiseCurve;
            public float PauseTime; // seconds to wait
            public float PauseTime1; // seconds to wait
            public float PauseTime2; // seconds to wait
            public float PauseTime3; // seconds to wait
            public float BuckleGravityScale; // use gravity to control descent when not 0
            public StringId BucklingMarker; // marker that shows bottom of giant and center of search area
            public float ForwardRearScan; // world-unit search distance
            public float LeftRightScan; // world-unit search distance
            public int ForwardRearSteps; // number of samples per direction
            public int LeftRightSteps; // number of samples per direction
            // giant may rotate the parent node of the buckling marker this much to align with ground.
            public Bounds<Angle> PitchBounds; // degrees
            // giant may rotate the parent node of the buckling marker this much to align with ground.
            public Bounds<Angle> RollBounds; // degrees
            public StringId BuckleAnimation; // animation to use to lower the giant
            public StringId DescentOverlay; // animation to overlay while lowering
            public StringId PausedOverlay; // animation to overlay while paused
            public float DescentOverlayScale; // max blend weight of descent overlay
            public float PausedOverlayScale; // max blend weight of paused overlay
            
            public enum SliderMovementPatterns : int
            {
                Linear,
                LightEaseIn,
                FullEaseIn,
                LightEaseOut,
                FullEaseOut,
                LightEaseInAndOut,
                FullEaseInAndOut
            }
        }
    }
}
