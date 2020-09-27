using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x7C)]
    public class LensFlare : TagStructure
    {
        /// <summary>
        /// LENS FLARE
        /// </summary>
        public Angle FalloffAngle; // degrees
        public Angle CutoffAngle; // degrees
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unknown1;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Unknown2;
        /// <summary>
        /// OCCLUSION
        /// </summary>
        /// <remarks>
        /// Occlusion factor affects overall lens flare brightness and can also affect scale. Occlusion never affects rotation.
        /// </remarks>
        public float OcclusionRadius; // world units
        public OcclusionOffsetDirectionValue OcclusionOffsetDirection;
        public OcclusionInnerRadiusScaleValue OcclusionInnerRadiusScale;
        public float NearFadeDistance; // world units
        public float FarFadeDistance; // world units
        public CachedTag Bitmap;
        public FlagsValue Flags;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Unknown3;
        public RotationFunctionValue RotationFunction;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public Angle RotationFunctionScale; // degrees
        public RealVector2d CoronaScale; // amount to stretch the corona
        /// <summary>
        /// EFFECT PARAMETERS
        /// </summary>
        /// <remarks>
        /// Only affects lens flares created by effects.
        /// </remarks>
        public FalloffFunctionValue FalloffFunction;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding2;
        public List<LensFlareReflection> Reflections;
        /// <summary>
        /// ANIMATION
        /// </summary>
        public FlagsValue Flags1;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding3;
        public List<FunctionDefinition> Brightness;
        public List<FunctionDefinition> Color;
        public List<FunctionDefinition> Rotation;
        
        public enum OcclusionOffsetDirectionValue : short
        {
            TowardViewer,
            MarkerForward,
            None
        }
        
        public enum OcclusionInnerRadiusScaleValue : short
        {
            None,
            _12,
            _14,
            _18,
            _116,
            _132,
            _164
        }
        
        [Flags]
        public enum FlagsValue : ushort
        {
            Sun = 1 << 0,
            NoOcclusionTest = 1 << 1,
            OnlyRenderInFirstPerson = 1 << 2,
            OnlyRenderInThirdPerson = 1 << 3,
            FadeInMoreQuickly = 1 << 4,
            FadeOutMoreQuickly = 1 << 5,
            ScaleByMarker = 1 << 6
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
        
        [TagStructure(Size = 0x30)]
        public class LensFlareReflection : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public short BitmapIndex;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            public float Position; // along flare axis
            public float RotationOffset; // degrees
            public Bounds<float> Radius; // world units
            public Bounds<float> Brightness; // [0,1]
            /// <summary>
            /// TINT COLOR
            /// </summary>
            /// <remarks>
            /// Tinting and modulating are not the same; 'tinting' a reflection will color the darker regions but leave the highlights, while 'modulating' will color everything uniformly. The modulation factor controls how much the reflection is modulated as opposed to tinted (0 is tinted, 1 is modulated).
            /// </remarks>
            public float ModulationFactor; // [0,1]
            public RealRgbColor Color;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                AlignRotationWithScreenCenter = 1 << 0,
                RadiusNotScaledByDistance = 1 << 1,
                RadiusScaledByOcclusionFactor = 1 << 2,
                OccludedBySolidObjects = 1 << 3,
                IgnoreLightColor = 1 << 4,
                NotAffectedByInnerOcclusion = 1 << 5
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class FunctionDefinition : TagStructure
        {
            public FunctionDefinition Function;
        }
    }
}

