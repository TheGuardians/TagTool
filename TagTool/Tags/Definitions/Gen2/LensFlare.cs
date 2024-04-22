using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "lens_flare", Tag = "lens", Size = 0x64)]
    public class LensFlare : TagStructure
    {
        public Angle FalloffAngle; // degrees
        public Angle CutoffAngle; // degrees
        [TagField(Length = 0x4)]
        public byte[] Unknown;
        [TagField(Length = 0x4)]
        public byte[] Unknown1;
        /// <summary>
        /// Occlusion factor affects overall lens flare brightness and can also affect scale. Occlusion never affects rotation.
        /// </summary>
        /// <summary>
        /// radius of the square used to test occlusion
        /// </summary>
        public float OcclusionRadius; // world units
        public OcclusionOffsetDirectionValue OcclusionOffsetDirection;
        public OcclusionInnerRadiusScaleValue OcclusionInnerRadiusScale;
        /// <summary>
        /// distance at which the lens flare brightness is maximum
        /// </summary>
        public float NearFadeDistance; // world units
        /// <summary>
        /// distance at which the lens flare brightness is minimum; set to zero to disable distance fading
        /// </summary>
        public float FarFadeDistance; // world units
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Bitmap;
        public FlagsValue Flags;
        [TagField(Length = 0x2)]
        public byte[] Unknown2;
        public RotationFunctionValue RotationFunction;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public Angle RotationFunctionScale; // degrees
        /// <summary>
        /// amount to stretch the corona
        /// </summary>
        public RealVector2d CoronaScale;
        /// <summary>
        /// Only affects lens flares created by effects.
        /// </summary>
        public FalloffFunctionValue FalloffFunction;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public List<LensFlareReflectionBlock> Reflections;
        public FlagsValue1 Flags1;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public List<LensFlareScalarAnimationBlock> Brightness;
        public List<LensFlareColorAnimationBlock> Color;
        public List<LensFlareScalarAnimationBlock1> Rotation;
        
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
        public class LensFlareReflectionBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short BitmapIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            /// <summary>
            /// 0 is on top of light, 1 is opposite light, 0.5 is the center of the screen, etc.
            /// </summary>
            public float Position; // along flare axis
            public float RotationOffset; // degrees
            /// <summary>
            /// interpolated by external input
            /// </summary>
            public Bounds<float> Radius; // world units
            /// <summary>
            /// interpolated by external input
            /// </summary>
            public Bounds<float> Brightness; // [0,1]
            /// <summary>
            /// Tinting and modulating are not the same; 'tinting' a reflection will color the darker regions but leave the highlights,
            /// while 'modulating' will color everything uniformly. The modulation factor controls how much the reflection is modulated
            /// as opposed to tinted (0 is tinted, 1 is modulated).
            /// </summary>
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
        
        [Flags]
        public enum FlagsValue1 : ushort
        {
            Synchronized = 1 << 0
        }
        
        [TagStructure(Size = 0x8)]
        public class LensFlareScalarAnimationBlock : TagStructure
        {
            public ScalarFunctionStructBlock Function;
            
            [TagStructure(Size = 0x8)]
            public class ScalarFunctionStructBlock : TagStructure
            {
                public MappingFunctionBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class LensFlareColorAnimationBlock : TagStructure
        {
            public ColorFunctionStructBlock Function;
            
            [TagStructure(Size = 0x8)]
            public class ColorFunctionStructBlock : TagStructure
            {
                public MappingFunctionBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class LensFlareScalarAnimationBlock1 : TagStructure
        {
            public ScalarFunctionStructBlock Function;
            
            [TagStructure(Size = 0x8)]
            public class ScalarFunctionStructBlock : TagStructure
            {
                public MappingFunctionBlock Function;
                
                [TagStructure(Size = 0x8)]
                public class MappingFunctionBlock : TagStructure
                {
                    public List<ByteBlock> Data;
                    
                    [TagStructure(Size = 0x1)]
                    public class ByteBlock : TagStructure
                    {
                        public sbyte Value;
                    }
                }
            }
        }
    }
}

