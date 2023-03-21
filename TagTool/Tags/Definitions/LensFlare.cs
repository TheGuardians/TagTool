using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using Microsoft.CodeAnalysis.CSharp;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x98, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x9C, MinVersion = CacheVersion.HaloOnlineED, Platform = CachePlatform.Original)]
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x9C, Version = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x98, Version = CacheVersion.Halo3ODST, Platform = CachePlatform.MCC)]
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x9C, Version = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
    public class LensFlare : TagStructure
	{
        public Angle FalloffAngle;
        public Angle CutoffAngle;

        /* OCCLUSION */

        //  For H3->HO conversion: OcclusionReflectionIndex = 0;
        //  Halo 3 lens tags always use their first reflection block element.
        [TagField(Version = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public int OcclusionReflectionIndex;

        public float OcclusionOffsetDistance;

        public OcclusionOffsetDirectionValue OcclusionOffsetDirection;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public OcclusionInnerRadiusScaleEnum OcclusionInnerRadiusScale;

        [TagField(Length = 0x2, Flags = Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] PaddingReach0;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float OcclusionInnerRadiusScaleReach; // percent of the corona to occlude against (ie 0.25, 0.125, etc)

        public float NearFadeBeginDistance; // distance where the lens flare starts to fade in (world units)
        public float NearFadeEndDistance; // distance where the lens flare is fully faded in (world units)
        public float NearFadeDistance; // distance at which the lens flare brightness is maximum (world units)
        public float FarFadeDistance; // distance at which the lens flare brightness is minimum; set to zero to disable distance fading (world units)

        [TagField(ValidTags = new[] { "bitm" })]
        public CachedTag Bitmap;

        public FlagsValue Flags;
        public short RuntimeFlags;

        public RotationFunctionValue RotationFunction;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding0;

        public Angle RotationFunctionScale;

        /* EFFECT PARAMETERS */
        public FalloffFunctionValue FalloffFunction;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;

        //
        //  For H3->HO conversion:
        //      foreach (var reflection in Reflections)
        //      {
        //          reflection.RotationOffset_HO = reflection.RotationOffset_H3;
        //          reflection.TintModulationFactor_HO = reflection.TintModulationFactor_H3;
        //      }
        //
        public List<Reflection> Reflections;

        public AnimationFlagsValue AnimationFlags;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;

        public List<BrightnessBlock> TimeBrightness;
        public List<BrightnessBlock> AgeBrightness;

        public List<ColorBlock> TimeColor;
        public List<ColorBlock> AgeColor;

        public List<RotationBlock> TimeRotation;
        public List<RotationBlock> AgeRotation;

        [Flags]
        public enum OcclusionInnerRadiusScaleEnum : short
        {
            None,
            _12,
            _14,
            _18,
            _116,
            _132,
            _164
        }

        public enum OcclusionOffsetDirectionValue : short
        {
            TowardViewer,
            MarkerForward,
            None
        }

        [Flags]
        public enum FlagsValue : ushort
        {
            None = 0,
            RotateOcclusionTestingBoxWithLensFlare = 1 << 0,
            NoOcclusionTest = 1 << 1,
            OnlyRenderInFirstPerson = 1 << 2,
            OnlyRenderInThirdPerson = 1 << 3,
            NoReflectionOpacityFeedback = 1 << 4,
            ScaleByMarker = 1 << 5,
            DoNotAutofade = 1 << 6,
            DoNotRenderWhileZoomed = 1 << 7,
            UseSeparateXAndYFalloffAngles = 1 << 8
        }

        public enum RotationFunctionValue : short
        {
            None,
            RotationA,
            RotationB,
            RotationTranslation,
            Translation
        }

        public enum FalloffFunctionValue : short
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

        [TagStructure(Size = 0x30, Platform = CachePlatform.Original, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x8C, Platform = CachePlatform.Original, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x58, Platform = CachePlatform.Original, MinVersion = CacheVersion.HaloReach)]
        [TagStructure(Size = 0x9C, Platform = CachePlatform.MCC, Version = CacheVersion.Halo3Retail)]
        [TagStructure(Size = 0x30, Platform = CachePlatform.MCC, Version = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloReach)]
        public class Reflection : TagStructure
		{
            public FlagsValue Flags;

            [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail, Length = 0x2, Flags = Padding)]
            public byte[] H3MCCPadding0;

            public short BitmapIndex;

            [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail, Length = 0x2, Flags = Padding)]
            public byte[] H3MCCPadding1;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint UnknownHO;

            [TagField(ValidTags = new[] { "bitm" }, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagField(ValidTags = new[] { "bitm" }, Platform = CachePlatform.MCC, Version = CacheVersion.Halo3Retail)]
            public CachedTag BitmapOverride;

            [TagField(Platform = CachePlatform.Original, MinVersion = CacheVersion.HaloOnlineED)]
            [TagField(Platform = CachePlatform.MCC, Version = CacheVersion.Halo3Retail)]
            [TagField(Platform = CachePlatform.MCC, Version = CacheVersion.HaloReach)]
            public float RotationOffset; // degrees

            public float AxisOffset; // % offset in screen space along corona axis - 0.0 is on the corona, 1.0 is primary side edge of the screen, -1.0 is opposite side (%)

            [TagField(Platform = CachePlatform.Original, MaxVersion = CacheVersion.Halo3ODST)]
            [TagField(Platform = CachePlatform.MCC, Version = CacheVersion.Halo3ODST)]
            public float RotationOffsetH3;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagField(Platform = CachePlatform.MCC, Version = CacheVersion.Halo3Retail)]
            public Bounds<float> OffsetBounds;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Bounds<float> RadiusBounds;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Bounds<float> BrightnessBounds;

            /* FUNCTIONS */

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            [TagField(Platform = CachePlatform.MCC, Version = CacheVersion.Halo3Retail)]
            public TagFunction RadiusCurveFunction = new TagFunction { Data = new byte[0] };

            [TagField(Platform = CachePlatform.Original, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
            public TagFunction ScaleCurveXFunction = new TagFunction { Data = new byte[0] };

            [TagField(Platform = CachePlatform.Original, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagField(Platform = CachePlatform.MCC, MaxVersion = CacheVersion.Halo3Retail)]
            public TagFunction ScaleCurveYFunction = new TagFunction { Data = new byte[0] };

            [TagField(MinVersion = CacheVersion.HaloOnlineED)]
            [TagField(Platform = CachePlatform.MCC, Version = CacheVersion.Halo3Retail)]
            public TagFunction BrightnessCurveFunction = new TagFunction { Data = new byte[0] };

            /* TINT COLOR */

            [TagField(Platform = CachePlatform.Original, MaxVersion = CacheVersion.Halo3ODST)]
            [TagField(Platform = CachePlatform.MCC, Version = CacheVersion.Halo3ODST)]
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float TintModulationFactorGen3;

            public RealRgbColor TintColor;

            [TagField(Platform = CachePlatform.Original, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            [TagField(Platform = CachePlatform.MCC, Version = CacheVersion.Halo3Retail)]
            public float TintModulationFactor;

            public float TintPower;

            [Flags]
            public enum FlagsValue : ushort
            {
                None = 0,
                AlignRotationWithScreenCenter = 1 << 0,
                RadiusNotScaledByDistance = 1 << 1,
                RadiusScaledByOcclusionFactor = 1 << 2,
                OccludedBySolidObjects = 1 << 3,
                IgnoreLightColor = 1 << 4,
                NotAffectedByInnerOcclusion = 1 << 5
            }
        }
        
        [Flags]
        public enum AnimationFlagsValue : ushort
        {
            None = 0,
            Synchronized = 1 << 0
        }

        [TagStructure(Size = 0x14)]
        public class BrightnessBlock : TagStructure
		{
            public TagFunction Function = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0x24)]
        public class ColorBlock : TagStructure
		{
            public StringId InputVariable;
            public StringId RangeVariable;
            public OutputModifierValue OutputModifier;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding0;

            public StringId OutputModifierInput;
            public TagFunction Function = new TagFunction { Data = new byte[0] };

            public enum OutputModifierValue : short
            {
                None,
                Add,
                Multiply
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class RotationBlock : TagStructure
		{
            public TagFunction Function = new TagFunction { Data = new byte[0] };
        }
    }
}
