using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sky", Tag = "sky ", Size = 0xAC)]
    public class Sky : TagStructure
    {
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag RenderModel;
        [TagField(ValidTags = new [] { "jmad" })]
        public CachedTag AnimationGraph;
        public FlagsValue Flags;
        /// <summary>
        /// Multiplier by which to scale the model's geometry up or down (0 defaults to standard, 0.03).
        /// </summary>
        public float RenderModelScale;
        /// <summary>
        /// How much the sky moves to remain centered on the player (0 defaults to 1.0, which means no parallax).
        /// </summary>
        public float MovementScale;
        public List<SkyCubemapBlock> CubeMap;
        /// <summary>
        /// Indoor ambient light color.
        /// </summary>
        public RealRgbColor IndoorAmbientColor;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        /// <summary>
        /// Indoor ambient light color.
        /// </summary>
        public RealRgbColor OutdoorAmbientColor;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        /// <summary>
        /// How far fog spreads into adjacent clusters.
        /// </summary>
        public float FogSpreadDistance; // world units
        public List<SkyAtmosphericFogBlock> AtmosphericFog;
        public List<SkyAtmosphericFogBlock1> SecondaryFog;
        public List<SkyFogBlock> SkyFog;
        public List<SkyPatchyFogBlock> PatchyFog;
        public float Amount; // [0,1]
        public float Threshold; // [0,1]
        public float Brightness; // [0,1]
        public float GammaPower;
        public List<SkyLightBlock> Lights;
        public Angle GlobalSkyRotation;
        public List<SkyShaderFunctionBlock> ShaderFunctions;
        public List<SkyAnimationBlock> Animations;
        [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public RealRgbColor ClearColor;
        
        [Flags]
        public enum FlagsValue : uint
        {
            FixedInWorldSpace = 1 << 0,
            Depreciated = 1 << 1,
            SkyCastsLightFromBelow = 1 << 2,
            DisableSkyInLightmaps = 1 << 3,
            FogOnlyAffectsContainingClusters = 1 << 4,
            UseClearColor = 1 << 5
        }
        
        [TagStructure(Size = 0xC)]
        public class SkyCubemapBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag CubeMapReference;
            /// <summary>
            /// 0 Defaults to 1.
            /// </summary>
            public float PowerScale;
        }
        
        [TagStructure(Size = 0x18)]
        public class SkyAtmosphericFogBlock : TagStructure
        {
            public RealRgbColor Color;
            /// <summary>
            /// Fog density is clamped to this value.
            /// </summary>
            public float MaximumDensity; // [0,1]
            /// <summary>
            /// Before this distance there is no fog.
            /// </summary>
            public float StartDistance; // world units
            /// <summary>
            /// Fog becomes opaque (maximum density) at this distance from the viewer.
            /// </summary>
            public float OpaqueDistance; // world units
        }
        
        [TagStructure(Size = 0x18)]
        public class SkyAtmosphericFogBlock1 : TagStructure
        {
            public RealRgbColor Color;
            /// <summary>
            /// Fog density is clamped to this value.
            /// </summary>
            public float MaximumDensity; // [0,1]
            /// <summary>
            /// Before this distance there is no fog.
            /// </summary>
            public float StartDistance; // world units
            /// <summary>
            /// Fog becomes opaque (maximum density) at this distance from the viewer.
            /// </summary>
            public float OpaqueDistance; // world units
        }
        
        [TagStructure(Size = 0x10)]
        public class SkyFogBlock : TagStructure
        {
            public RealRgbColor Color;
            /// <summary>
            /// Fog density is clamped to this value.
            /// </summary>
            public float Density; // [0,1]
        }
        
        [TagStructure(Size = 0x50)]
        public class SkyPatchyFogBlock : TagStructure
        {
            public RealRgbColor Color;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Bounds<float> Density; // [0,1]
            public Bounds<float> Distance; // world units
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "fpch" })]
            public CachedTag PatchyFog;
        }
        
        [TagStructure(Size = 0x34)]
        public class SkyLightBlock : TagStructure
        {
            public RealVector3d DirectionVector;
            public RealEulerAngles2d Direction;
            [TagField(ValidTags = new [] { "lens" })]
            public CachedTag LensFlare;
            public List<SkyLightFogBlock> Fog;
            public List<SkyLightFogBlock1> FogOpposite;
            public List<SkyRadiosityLightBlock> Radiosity;
            
            [TagStructure(Size = 0x2C)]
            public class SkyLightFogBlock : TagStructure
            {
                public RealRgbColor Color;
                /// <summary>
                /// Fog density is clamped to this value.
                /// </summary>
                public float MaximumDensity; // [0,1]
                /// <summary>
                /// Before this distance there is no fog.
                /// </summary>
                public float StartDistance; // world units
                /// <summary>
                /// Fog becomes opaque (maximum density) at this distance from the viewer.
                /// </summary>
                public float OpaqueDistance; // world units
                public Bounds<Angle> Cone; // degrees
                public float AtmosphericFogInfluence; // [0,1]
                public float SecondaryFogInfluence; // [0,1]
                public float SkyFogInfluence; // [0,1]
            }
            
            [TagStructure(Size = 0x2C)]
            public class SkyLightFogBlock1 : TagStructure
            {
                public RealRgbColor Color;
                /// <summary>
                /// Fog density is clamped to this value.
                /// </summary>
                public float MaximumDensity; // [0,1]
                /// <summary>
                /// Before this distance there is no fog.
                /// </summary>
                public float StartDistance; // world units
                /// <summary>
                /// Fog becomes opaque (maximum density) at this distance from the viewer.
                /// </summary>
                public float OpaqueDistance; // world units
                public Bounds<Angle> Cone; // degrees
                public float AtmosphericFogInfluence; // [0,1]
                public float SecondaryFogInfluence; // [0,1]
                public float SkyFogInfluence; // [0,1]
            }
            
            [TagStructure(Size = 0x28)]
            public class SkyRadiosityLightBlock : TagStructure
            {
                public FlagsValue Flags;
                /// <summary>
                /// Light color.
                /// </summary>
                public RealRgbColor Color;
                /// <summary>
                /// Light power from 0 to infinity.
                /// </summary>
                public float Power; // [0,+inf]
                /// <summary>
                /// Length of the ray for shadow testing.
                /// </summary>
                public float TestDistance; // world units
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                /// <summary>
                /// Angular diameter of the light source in the sky.
                /// </summary>
                public Angle Diameter; // degrees
                
                [Flags]
                public enum FlagsValue : uint
                {
                    AffectsExteriors = 1 << 0,
                    AffectsInteriors = 1 << 1,
                    DirectIlluminationInLightmaps = 1 << 2,
                    IndirectIlluminationInLightmaps = 1 << 3
                }
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class SkyShaderFunctionBlock : TagStructure
        {
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// Global function that controls this shader value.
            /// </summary>
            [TagField(Length = 32)]
            public string GlobalFunctionName;
        }
        
        [TagStructure(Size = 0x24)]
        public class SkyAnimationBlock : TagStructure
        {
            /// <summary>
            /// Index of the animation in the animation graph.
            /// </summary>
            public short AnimationIndex;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float Period; // sec
            [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
        }
    }
}

