using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "giant", Tag = "gint", Size = 0x28)]
    public class Giant : Unit
    {
        /// <summary>
        /// The flags of the giant.
        /// </summary>
        public GiantFlagsValue GiantFlags;

        /// <summary>
        /// The acceleration time of the giant in seconds.
        /// </summary>
        public float AccelerationTime;

        /// <summary>
        /// The deceleration time of the giant in seconds.
        /// </summary>
        public float DecelerationTime;

        /// <summary>
        /// The overall animation speed scale of the giant.
        /// </summary>
        public float SpeedScale;

        /// <summary>
        /// The elevation change rate scale per update of the giant.
        /// </summary>
        public float ElevationChangeRate;

        /// <summary>
        /// How far around a foot to search for ground targets to stomp. Set to 0 to not search for targets.
        /// </summary>
        public float FootTargetRadius;

        /// <summary>
        /// The buckle settings of the giant.
        /// </summary>
        public List<BuckleSetting> BuckleSettings;

        /// <summary>
        /// The lower values to drop the ankles of the giant towards the ground when computing IK.
        /// </summary>
        public float AnkleIkScale;

        [TagStructure(Name = "buckle_setting", Size = 0x5C)]
        public class BuckleSetting : TagStructure
		{
            /// <summary>
            /// How many seconds until the giant reaches the ground.
            /// </summary>
            public float LowerTime;

            /// <summary>
            /// The lowering curve type of the giant.
            /// </summary>
            public CurveValue LowerCurve;

            /// <summary>
            /// How many seconds until the giant recovers from reaching the ground.
            /// </summary>
            public float RaiseTime;

            /// <summary>
            /// The raising curve type of the giant.
            /// </summary>
            public CurveValue RaiseCurve;

            /// <summary>
            /// How many seconds for the giant to wait on "Easy" difficulty.
            /// </summary>
            public float PauseTimeEasy;

            /// <summary>
            /// How many seconds for the giant to wait on "Normal" difficulty.
            /// </summary>
            public float PauseTimeNormal;

            /// <summary>
            /// How many seconds for the giant to wait on "Heroic" difficulty.
            /// </summary>
            public float PauseTimeHeroic;

            /// <summary>
            /// How many seconds for the giant to wait on "Legendary" difficulty.
            /// </summary>
            public float PauseTimeLegendary;

            /// <summary>
            /// The buckling gravity scale of the giant. Used to control descent when not 0.
            /// </summary>
            public float BuckleGravityScale;

            /// <summary>
            /// The marker that shows bottom of the giant and center of search area.
            /// </summary>
            public StringId BucklingMarker;

            /// <summary>
            /// The forward-rear world-unit search distance of the giant.
            /// </summary>
            public float ForwardRearScan;

            /// <summary>
            /// The left-right world-unit search distance of the giant.
            /// </summary>
            public float LeftRightScan;

            /// <summary>
            /// The number of forward-rear samples per direction of the giant.
            /// </summary>
            public int ForwardRearSteps;

            /// <summary>
            /// The number of left-right samples per direction of the giant.
            /// </summary>
            public int LeftRightSteps;

            /// <summary>
            /// The giant may rotate the parent node of the buckling marker this much to align with ground.
            /// </summary>
            public Bounds<Angle> PitchBounds;

            /// <summary>
            /// The giant may rotate the parent node of the buckling marker this much to align with ground.
            /// </summary>
            public Bounds<Angle> RollBounds;

            /// <summary>
            /// The animation to use to lower the giant.
            /// </summary>
            public StringId BuckleAnimation;

            /// <summary>
            /// The animation to overlay while the giant is lowering.
            /// </summary>
            public StringId DescentOverlay;

            /// <summary>
            /// The animation to overlay while the giant is paused.
            /// </summary>
            public StringId PauseOverlay;

            /// <summary>
            /// The maximum blend weight of descent overlay of the giant.
            /// </summary>
            public float DescentOverlayScale;

            /// <summary>
            /// The maximum blend weight of paused overlay of the giant.
            /// </summary>
            public float PausedOverlayScale;
            
            public enum CurveValue : int
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

        [Flags]
        public enum GiantFlagsValue : int
        {
            None = 0
        }
    }
}
