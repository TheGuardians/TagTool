using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "atmosphere_fog", Tag = "fogg", Size = 0x12C)]
    public class AtmosphereDefinition : TagStructure
    {
        public AtmosphereFlags Flags;
        public byte Version;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // negative means into the screen
        public float DistanceBias; // world units
        public SoloFogParametersStruct SkyFog;
        public SoloFogParametersStruct GroundFog;
        public SoloFogParametersStruct CeilingFog;
        public FogLightStruct FogLight;
        public float SheetDensity;
        public RealRgbColor ColorTint;
        public RealRgbColor ColorTintInner;
        public float Intensity;
        public float FullIntensityHeight;
        public float HalfIntensityHeight;
        public RealVector3d WindDirection;
        public float ReferencePlaneHeight;
        public VolumeFogParametersDefinition VolumeFog;
        public LightShaftParametersDefinition LightShaft;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag WeatherEffect;
        
        [Flags]
        public enum AtmosphereFlags : ushort
        {
            SkyFogEnabled = 1 << 0,
            GroundFogEnabled = 1 << 1,
            CeilingFogEnabled = 1 << 2,
            FogLightEnabled = 1 << 3,
            PatchyFogEnabled = 1 << 4,
            SkyFogHeightCameraRelative = 1 << 5,
            GroundFogHeightCameraRelative = 1 << 6,
            UseFloatingShadowForFogLight = 1 << 7,
            OnlyUseFogLightForLightShafts = 1 << 8
        }
        
        [TagStructure(Size = 0x2C)]
        public class SoloFogParametersStruct : TagStructure
        {
            public float BaseHeight; // world units
            public float FogHeight; // world units
            public float FogThickness00To10;
            public float FogFalloffEnd;
            public RealRgbColor FogColor;
            // scales color up or down to allow for HDR values
            public float FogColorIntensity;
            public List<SolofogFunctionBlock> Function;
            
            [TagStructure(Size = 0x14)]
            public class SolofogFunctionBlock : TagStructure
            {
                public MappingFunction Mapping;
                
                [TagStructure(Size = 0x14)]
                public class MappingFunction : TagStructure
                {
                    public byte[] Data;
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class FogLightStruct : TagStructure
        {
            public float PitchAngle90To90; // degree
            public float YawAngle0To360; // degree
            public float AngularRadius0To180; // degree
            public RealRgbColor TintColor;
            public float TintColorIntensity;
            public float AngularFalloffSteepness;
            public float DistanceFalloffSteepness;
            public float NearbyCutoffPercentage;
        }
        
        [TagStructure(Size = 0x10)]
        public class VolumeFogParametersDefinition : TagStructure
        {
            public RealRgbColor VolumeFogColor;
            public float VolumeFogColorIntensity;
        }
        
        [TagStructure(Size = 0x20)]
        public class LightShaftParametersDefinition : TagStructure
        {
            public RealRgbColor LightShaftTintColor;
            public float LightShaftIntensity;
            public float LightShaftDecayRate;
            public float MaximumScreenDistance;
            public float SampleDensity;
            public float MinimumCasterDistance;
        }
    }
}
