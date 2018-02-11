using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x98, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x9C, MinVersion = CacheVersion.HaloOnline106708)]
    public class LensFlare
    {
        public Angle FalloffAngle;
        public Angle CutoffAngle;

        //
        //  For H3->HO conversion:
        //      OcclusionReflectionIndex = 0;
        //
        //  Halo 3 lens tags always use their first reflection block element.
        //

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public int OcclusionReflectionIndex;

        public float OcclusionRadius;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused1;

        public OcclusionOffsetDirectionValue OcclusionOffsetDirection;

        public uint Unknown1;
        public uint Unknown2;

        public float NearFadeDistance;
        public float FarFadeDistance;
        public CachedTagInstance Bitmap;
        public FlagsValue Flags;
        public short RuntimeFlags;

        public RotationFunctionValue RotationFunction;

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused2;

        public Angle RotationFunctionScale;

        public FalloffFunctionValue FalloffFunction;

        [TagField(Padding = true, Length = 2)]
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

        [TagField(Padding = true, Length = 2)]
        public byte[] Unused4;

        public List<BrightnessBlock> TimeBrightness;
        public List<BrightnessBlock> AgeBrightness;

        public List<ColorBlock> TimeColor;
        public List<ColorBlock> AgeColor;

        public List<RotationBlock> TimeRotation;
        public List<RotationBlock> AgeRotation;

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

        [TagStructure(Size = 0x30, MaxVersion = CacheVersion.Halo3ODST)]
        [TagStructure(Size = 0x8C, MinVersion = CacheVersion.HaloOnline106708)]
        public class Reflection
        {
            public FlagsValue Flags;

            public short BitmapIndex;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public uint Unknown2;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public CachedTagInstance BitmapOverride;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public float RotationOffset_HO;

            public float PositionFlareAxis;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float RotationOffset_H3;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public Bounds<float> OffsetBounds;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public Bounds<float> RadiusBounds;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public Bounds<float> BrightnessBounds;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public TagFunction RadiusCurveFunction = new TagFunction { Data = new byte[0] };

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public TagFunction ScaleCurveXFunction = new TagFunction { Data = new byte[0] };

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public TagFunction ScaleCurveYFunction = new TagFunction { Data = new byte[0] };

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
            public TagFunction BrightnessCurveFunction = new TagFunction { Data = new byte[0] };

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public float TintModulationFactor_H3;

            public RealRgbColor TintColor;

            [TagField(MinVersion = CacheVersion.HaloOnline106708)]
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
        public class BrightnessBlock
        {
            public TagFunction Function = new TagFunction { Data = new byte[0] };
        }

        [TagStructure(Size = 0x24)]
        public class ColorBlock
        {
            public StringId InputVariable;

            public StringId RangeVariable;

            public OutputModifierValue OutputModifier;

            [TagField(Padding = true, Length = 2)]
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
        public class RotationBlock
        {
            public TagFunction Function = new TagFunction { Data = new byte[0] };
        }
    }
}
