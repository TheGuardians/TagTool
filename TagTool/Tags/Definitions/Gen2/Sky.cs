using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "sky", Tag = "sky ", Size = 0xDC)]
    public class Sky : TagStructure
    {
        public CachedTag RenderModel;
        public CachedTag AnimationGraph;
        public FlagsValue Flags;
        public float RenderModelScale; // Multiplier by which to scale the model's geometry up or down (0 defaults to standard, 0.03).
        public float MovementScale; // How much the sky moves to remain centered on the player (0 defaults to 1.0, which means no parallax).
        public List<SkyCubemap> CubeMap;
        /// <summary>
        /// AMBIENT LIGHT
        /// </summary>
        public RealRgbColor IndoorAmbientColor; // Indoor ambient light color.
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding1;
        public RealRgbColor OutdoorAmbientColor; // Indoor ambient light color.
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding2;
        /// <summary>
        /// FOG
        /// </summary>
        public float FogSpreadDistance; // world units
        public List<SkyAtmosphericFog> AtmosphericFog;
        public List<SkyAtmosphericFog> SecondaryFog;
        public List<SkyFogBlock> SkyFog;
        public List<SkyPatchyFog> PatchyFog;
        /// <summary>
        /// BLOOM OVERRIDE
        /// </summary>
        public float Amount; // [0,1]
        public float Threshold; // [0,1]
        public float Brightness; // [0,1]
        public float GammaPower;
        public List<SkyLight> Lights;
        /// <summary>
        /// ROTATION
        /// </summary>
        public Angle GlobalSkyRotation;
        public List<SkyShaderFunction> ShaderFunctions;
        public List<SkyAnimation> Animations;
        [TagField(Flags = Padding, Length = 12)]
        public byte[] Padding3;
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
        
        [TagStructure(Size = 0x14)]
        public class SkyCubemap : TagStructure
        {
            public CachedTag CubeMapReference;
            public float PowerScale; // 0 Defaults to 1.
        }
        
        [TagStructure(Size = 0x18)]
        public class SkyAtmosphericFog : TagStructure
        {
            public RealRgbColor Color;
            public float MaximumDensity; // [0,1]
            public float StartDistance; // world units
            public float OpaqueDistance; // world units
        }
        
        [TagStructure(Size = 0x10)]
        public class SkyFogBlock : TagStructure
        {
            public RealRgbColor Color;
            public float Density; // [0,1]
        }
        
        [TagStructure(Size = 0x58)]
        public class SkyPatchyFog : TagStructure
        {
            public RealRgbColor Color;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding1;
            public Bounds<float> Density; // [0,1]
            public Bounds<float> Distance; // world units
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding2;
            public CachedTag PatchyFog;
        }
        
        [TagStructure(Size = 0x48)]
        public class SkyLight : TagStructure
        {
            public RealVector3d DirectionVector;
            public RealEulerAngles2d Direction;
            public CachedTag LensFlare;
            public List<SkyLightFog> Fog;
            public List<SkyLightFog> FogOpposite;
            public List<SkyRadiosityLight> Radiosity;
            
            [TagStructure(Size = 0x2C)]
            public class SkyLightFog : TagStructure
            {
                public RealRgbColor Color;
                public float MaximumDensity; // [0,1]
                public float StartDistance; // world units
                public float OpaqueDistance; // world units
                /// <summary>
                /// FOG INFLUENCES
                /// </summary>
                public Bounds<Angle> Cone; // degrees
                public float AtmosphericFogInfluence; // [0,1]
                public float SecondaryFogInfluence; // [0,1]
                public float SkyFogInfluence; // [0,1]
            }
            
            [TagStructure(Size = 0x28)]
            public class SkyRadiosityLight : TagStructure
            {
                public FlagsValue Flags;
                public RealRgbColor Color; // Light color.
                public float Power; // [0,+inf]
                public float TestDistance; // world units
                [TagField(Flags = Padding, Length = 12)]
                public byte[] Padding1;
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
        public class SkyShaderFunction : TagStructure
        {
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            [TagField(Length = 32)]
            public string GlobalFunctionName; // Global function that controls this shader value.
        }
        
        [TagStructure(Size = 0x24)]
        public class SkyAnimation : TagStructure
        {
            public short AnimationIndex; // Index of the animation in the animation graph.
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public float Period; // sec
            [TagField(Flags = Padding, Length = 28)]
            public byte[] Padding2;
        }
    }
}

