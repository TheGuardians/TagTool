using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0xA0)]
    public class LensFlare : TagStructure
    {
        public Angle FalloffAngle; // degrees
        public Angle CutoffAngle; // degrees
        // occlusion information will be generated against the size of this reflection
        public int OcclusionReflectionIndex;
        // distance along offset direction used to test occlusion
        public float OcclusionOffsetDistance; // world units
        public LensFlareOcclusionOffsetEnum OcclusionOffsetDirection;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // percent of the corona to occlude against (ie 0.25, 0.125, etc)
        public float OcclusionInnerRadiusScale;
        // distance where the lens flare starts to fade in
        public float NearFadeBeginDistance; // world units
        // distance where the lens flare is fully faded in
        public float NearFadeEndDistance; // world units
        // distance at which the lens flare brightness is maximum
        public float NearFadeDistance; // world units
        // distance at which the lens flare brightness is minimum; set to zero to disable distance fading
        public float FarFadeDistance; // world units
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap;
        public LensFlareFlags Flags;
        public short RuntimeFlags;
        public LensFlareCoronaRotationFunctionEnum RotationFunction;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public Angle RotationFunctionScale; // degrees
        public GlobalReverseTransitionFunctionsEnum FalloffFunction;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public List<LensFlareReflectionBlock> Reflections;
        public LensFlareAnimationFlags AnimationFlags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public List<LensFlareScalarAnimationBlock> TimeBrightness;
        public List<LensFlareScalarAnimationBlock> AgeBrightness;
        public List<LensFlareColorAnimationBlock> TimeColor;
        public List<LensFlareColorAnimationBlock> AgeColor;
        public List<LensFlareScalarAnimationBlock> TimeRotation;
        public List<LensFlareScalarAnimationBlock> AgeRotation;
        
        public enum LensFlareOcclusionOffsetEnum : short
        {
            TowardViewer,
            MarkerForward,
            None
        }
        
        [Flags]
        public enum LensFlareFlags : ushort
        {
            // otherwise remains aligned with screen. turn on render_debug_lens_flares and look at green box
            RotateOcclusionTestingBoxWithLensFlare = 1 << 0,
            NoOcclusionTest = 1 << 1,
            OnlyRenderInFirstPerson = 1 << 2,
            OnlyRenderInThirdPerson = 1 << 3,
            // ignore fancy occlusion box scaling - useful for very big or very far away lens flares (skybox flares, etc)
            UseSimpleOcclusionBoxTest = 1 << 4,
            // unchecked, each reflection evaluates its opacity from the previous reflection in the list
            NoReflectionOpacityFeedback = 1 << 5,
            ScaleByMarker = 1 << 6,
            // otherwise fade based on effect time, for effect attachments only
            DonTAutofade = 1 << 7,
            DonTRenderWhileZoomed = 1 << 8
        }
        
        public enum LensFlareCoronaRotationFunctionEnum : short
        {
            None,
            RotationA,
            RotationB,
            RotationTranslation,
            Translation
        }
        
        public enum GlobalReverseTransitionFunctionsEnum : short
        {
            Linear,
            Late,
            VeryLate,
            Early,
            VeryEarly,
            Cosine,
            Zero,
            One
        }
        
        [Flags]
        public enum LensFlareAnimationFlags : ushort
        {
            Synchronized = 1 << 0
        }
        
        [TagStructure(Size = 0x94)]
        public class LensFlareReflectionBlock : TagStructure
        {
            public StringId Name;
            public LensFlareReflectionFlags Flags;
            public short BitmapIndex;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag BitmapOverride;
            public float RotationOffset; // degrees
            // percent offset in screen space along corona axis - 0.0 is on the corona, 1.0 is primary side edge of the screen,
            // -1.0 is opposite side
            public float AxisOffset; // percent
            // the axis offset times corona offset is clamped between these values
            public Bounds<float> OffsetBounds;
            // interpolated by external input
            public ScalarFunctionNamedStruct RadiusCurve;
            // interpolated by external input
            public ScalarFunctionNamedStruct ScaleCurveX;
            // interpolated by external input
            public ScalarFunctionNamedStruct ScaleCurveY;
            // interpolated by external input
            public ScalarFunctionNamedStruct BrightnessCurve;
            // interpolated by external input
            public ColorFunctionNamedStruct ColorCurve;
            public float ModulationFactor; // [0,1]
            public float TintPower; // [0.1, 16]
            
            [Flags]
            public enum LensFlareReflectionFlags : ushort
            {
                RotateFromCenterOfScreen = 1 << 0,
                RadiusScaledByDistance = 1 << 1,
                RadiusScaledByOcclusionFactor = 1 << 2,
                IgnoreExternalColor = 1 << 3,
                LockToFlareX = 1 << 4,
                LockToFlareY = 1 << 5,
                // The input to the curves below should be multiplied by 1 if at the center of the screen and 0 if at the edge
                ProximityToCenterAsFunctionInput = 1 << 6,
                // Also renders this reflection's mirror image across the flare center
                MirrorAcrossFlare = 1 << 7,
                DisabledForDebugging = 1 << 8,
                FlipUCoordinate = 1 << 9,
                FlipVCoordinate = 1 << 10,
                DrawInWorldSpaceInsteadOfScreenSpace = 1 << 11
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
            
            [TagStructure(Size = 0x14)]
            public class ColorFunctionNamedStruct : TagStructure
            {
                public MappingFunction Function;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class LensFlareScalarAnimationBlock : TagStructure
        {
            public ScalarFunctionNamedStruct Function;
            
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
        
        [TagStructure(Size = 0x24)]
        public class LensFlareColorAnimationBlock : TagStructure
        {
            public LensFlareColorFunctionStruct ColorAnimation;
            
            [TagStructure(Size = 0x24)]
            public class LensFlareColorFunctionStruct : TagStructure
            {
                public StringId InputVariable;
                public StringId RangeVariable;
                public OutputModEnum OutputModifier;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId OutputModifierInput;
                public MappingFunction LensFlareColorMapping;
                
                public enum OutputModEnum : short
                {
                    Unknown,
                    Plus,
                    Times
                }
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
    }
}
