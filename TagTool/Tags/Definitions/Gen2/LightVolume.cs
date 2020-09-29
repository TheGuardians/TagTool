using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "light_volume", Tag = "MGS2", Size = 0x14)]
    public class LightVolume : TagStructure
    {
        /// <summary>
        /// LIGHT VOLUME
        /// </summary>
        /// <remarks>
        /// Light volumes are rendered as a sequence of glowy sprites, just like in Metal Gear Solid 2. Each instance of the light volume is rendered separately; this allows, for example, a narrow bright white volume to be overlaid on top of a fuzzy wide colored volume, or anything else you want!
        /// </remarks>
        public float FalloffDistanceFromCamera; // world units
        public float CutoffDistanceFromCamera; // world units
        public List<LightVolumeBlock> Volumes;
        
        [TagStructure(Size = 0xBC)]
        public class LightVolumeBlock : TagStructure
        {
            /// <summary>
            /// VOLUME
            /// </summary>
            /// <remarks>
            /// If no bitmap is selected, the default glow bitmap will be used. Sprite count controls how many sprites are used to render this volume. Using more sprites will result in a smoother and brighter effect, at a slight performance penalty. Don't touch the flags unless you know what you're doing (they should be off by default).
            /// 
            /// Be careful with the 'fuzzy' flag! It should be used on very wide light volumes to make them blend smoothly into solid geometry rather than "cutting" into the zbuffer. Using this feature will make light volumes several times slower when they fill a large portion of the screen.
            /// </remarks>
            public FlagsValue Flags;
            public CachedTag Bitmap;
            public int SpriteCount; // [4,256]
            /// <summary>
            /// OFFSET
            /// </summary>
            /// <remarks>
            /// This function controls the offset along the projected forward axis of the light volume. The output range of this function is the distance in WORLD UNITS from the origin where the first and last sprite are rendered. The input to this function is the fractional value (from 0 to 1) along the projected axis. Using a transition function such as "late" will result in more sprites being bunched up towards the origin and spaced further apart near the end.
            /// 
            /// Note that this and other functions in this tag have a range-input, which is controlled by the output of the FACING function below.
            /// </remarks>
            public FunctionDefinition OffsetFunction;
            /// <summary>
            /// RADIUS
            /// </summary>
            /// <remarks>
            /// This function controls the radius in WORLD UNITS of each sprite along the projected forward axis. Using a smaller min value and a larger max value results in a light volume that looks like a cone.
            /// </remarks>
            public FunctionDefinition RadiusFunction;
            /// <summary>
            /// BRIGHTNESS
            /// </summary>
            /// <remarks>
            /// This function controls the overall brightness (in [0,1]) of each sprite along the projected forward axis. Note that since the sprites are additive, they will be brighter in areas where they overlap more even if this function is constant, so it may be useful to use the brightness function to compensate for this.
            /// </remarks>
            public FunctionDefinition BrightnessFunction;
            /// <summary>
            /// COLOR
            /// </summary>
            /// <remarks>
            /// This function controls the color of each sprite along the projected forward axis. Color is multiplied by brightness to produce the final color that will be applied to the sprite.
            /// </remarks>
            public FunctionDefinition ColorFunction;
            /// <summary>
            /// FACING
            /// </summary>
            /// <remarks>
            /// The input to this function is the facing angle between the light volume and the camera. Zero represents facing towards (parallel) or away from the camera and 1.0 represents facing perpendicular to the camera.
            /// 
            /// The output of this function is fed into the range-input of the functions above.
            /// </remarks>
            public FunctionDefinition FacingFunction;
            public List<LightVolumeAspect> Aspect;
            /// <summary>
            /// POSTPROCESS CONTROL
            /// </summary>
            /// <remarks>
            /// ADVANCED STUFF!! Don't change these values!!
            /// </remarks>
            public float RadiusFracMin; // [0.00390625, 1.0]
            public float DeprecatedXStepExponent; // [0.5, 0.875]
            public int DeprecatedXBufferLength; // [32, 512]
            public int XBufferSpacing; // [1, 256]
            public int XBufferMinIterations; // [1, 256]
            public int XBufferMaxIterations; // [1, 256]
            public float XDeltaMaxError; // [0.001, 0.1]
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Unknown1;
            public List<RealVector2d> Unknown2;
            [TagField(Flags = Padding, Length = 48)]
            public byte[] Unknown3;
            
            [Flags]
            public enum FlagsValue : uint
            {
                ForceLinearRadiusFunction = 1 << 0,
                ForceLinearOffset = 1 << 1,
                ForceDifferentialEvaluation = 1 << 2,
                Fuzzy = 1 << 3,
                NotScaledByEventDuration = 1 << 4,
                ScaledByMarker = 1 << 5
            }
            
            [TagStructure(Size = 0xC)]
            public class FunctionDefinition : TagStructure
            {
                public FunctionDefinition Function;
            }
            
            [TagStructure(Size = 0x24)]
            public class LightVolumeAspect : TagStructure
            {
                /// <summary>
                /// SCALE FACTORS
                /// </summary>
                /// <remarks>
                /// These function control the screenspace aspect ratio of each sprite relative to the projected forward axis. Note that there is no range-input to these functions, because it would not make sense to stretch sprites when the light volume is facing directly towards or away from the camera (which way would we stretch them?)
                /// </remarks>
                /// <summary>
                /// ALONG-AXIS SCALE
                /// </summary>
                /// <remarks>
                /// Values higher than 1 along the axis will cause sprites to elongate and overlap more regardless of the orientation of the light volume, whereas values lower than 1 will cause sprite separation.
                /// </remarks>
                public FunctionDefinition AlongAxis;
                /// <summary>
                /// AWAY-FROM-AXIS SCALE
                /// </summary>
                /// <remarks>
                /// Values higher than 1 away from the axis will cause the light volume to appear thicker.
                /// </remarks>
                public FunctionDefinition AwayFromAxis;
                /// <summary>
                /// PARALLEL SCALE
                /// </summary>
                /// <remarks>
                /// When the light volume is viewed directly towards (parallel) or away from the camera, the scale factors above are interpolated towards this constant value. The threshold angle controls when the light volume is considered to be 100% parallel for these computations. The exponent controls how quickly the transition from using the perpendicular scale factors to using the parallel scale factor happens, e.g.:
                /// 
                /// * exponent=0.0 - perpendicular scale factors will always be used unless below threshold angle
                /// * exponent=0.1 - transition quickly as viewing angle becomes perpendicular
                /// * exponent=1.0 - transition smoothly between perpendicular and parallel
                /// * exponent=9.0 - transition quickly as viewing angle becomes parallel
                /// </remarks>
                public float ParallelScale;
                public Angle ParallelThresholdAngle; // degrees
                public float ParallelExponent;
                
                [TagStructure(Size = 0xC)]
                public class FunctionDefinition : TagStructure
                {
                    public FunctionDefinition Function;
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class RealVector2d : TagStructure
            {
                public RealVector2d Unknown1;
            }
        }
    }
}

