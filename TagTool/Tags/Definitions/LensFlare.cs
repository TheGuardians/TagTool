using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x98, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x9C, MinVersion = CacheVersion.HaloOnlineED, Platform = CachePlatform.Original)]
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x9C, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class LensFlare : TagStructure
	{
        public Angle FalloffAngle;
        public Angle CutoffAngle;

        //
        //  For H3->HO conversion:
        //      OcclusionReflectionIndex = 0;
        //
        //  Halo 3 lens tags always use their first reflection block element.
        //

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
        public int OcclusionReflectionIndex;

        public float OcclusionRadius;

        public OcclusionOffsetDirectionValue OcclusionOffsetDirection;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.HaloReach)]
        public byte[] PANTS;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public OcclusionInnerRadiusScaleEnum OcclusionInnerRadiusScale;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public float OcclusionInnerRadiusScaleReach; // percent of the corona to occlude against (ie 0.25, 0.125, etc)

        public float NearFadeBeginDistance; // distance where the lens flare starts to fade in (world units)
        public float NearFadeEndDistance; // distance where the lens flare is fully faded in (world units)

        public float NearFadeDistance; // distance at which the lens flare brightness is maximum (world units)
        public float FarFadeDistance; // distance at which the lens flare brightness is minimum; set to zero to disable distance fading (world units)
        public CachedTag Bitmap;
        public FlagsValue Flags;
        public short RuntimeFlags;

        public RotationFunctionValue RotationFunction;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unused2;

        public Angle RotationFunctionScale;

        public FalloffFunctionValue FalloffFunction;

        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unused3;

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
        public byte[] Unused4;

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

        [TagStructure(Size = 0x30, MaxVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x8C, MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
        [TagStructure(Size = 0x58, MinVersion = CacheVersion.HaloReach, Platform =  CachePlatform.Original)]
        [TagStructure(Size = 0x9C, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        public class Reflection : TagStructure
		{
            public FlagsValue Flags;
            public short BitmapIndex;

            [TagField(Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public uint Unknown2;

            [TagField(Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public CachedTag BitmapOverride;

            [TagField(Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public float RotationOffset_HO;

            [TagField(Platform = CachePlatform.Original)]
            public float PositionFlareAxis;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float RotationOffset_H3;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float RotationOffset_Reach;

            [TagField(Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public Bounds<float> OffsetBounds;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public Bounds<float> RadiusBounds;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Bounds<float> RadiusBoundsReach;
            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public Bounds<float> BrightnessBounds;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public Bounds<float> BrightnessBoundsReach;

            [TagField(Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
            public TagFunction RadiusCurveFunction = new TagFunction { Data = new byte[0] };

            [TagField(Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.HaloOnlineED, Platform = CachePlatform.Original)]
            public TagFunction ScaleCurveXFunction = new TagFunction { Data = new byte[0] };
            [TagField(Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.HaloOnlineED, Platform = CachePlatform.Original)]
            public TagFunction ScaleCurveYFunction = new TagFunction { Data = new byte[0] };

            [TagField(Platform = CachePlatform.MCC)]
            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123, Platform = CachePlatform.Original)]
            public TagFunction BrightnessCurveFunction = new TagFunction { Data = new byte[0] };

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float TintModulationFactor_H3;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public float TintModulationFactor_Reach;

            public RealRgbColor TintColor;

            [TagField(MinVersion = CacheVersion.HaloOnlineED, MaxVersion = CacheVersion.HaloOnline700123)]
            public float TintModulationFactor_HO;

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
            public byte[] Unused;

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
