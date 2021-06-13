using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "new_cinematic_lighting", Tag = "nclt", Size = 0x6C)]
    public class NewCinematicLighting : TagStructure
    {
        public int Version;
        public float Direction;
        public float FrontBack;
        public float ShadowInterpolation;
        public float OverallWeight;
        public float DirectWeight;
        public float IndirectWeight;
        public float AirprobeWeight;
        public float SunWeight;
        public RealRgbColor DirectColor;
        public float DirectIntensity;
        public RealRgbColor IndirectColor;
        public float IndirectIntensity;
        public float Interpolation;
        public List<AuthoredLightProbeBlockStruct> AuthoredLightProbe;
        public List<HologramlightingBlockStruct> CortanaLighting;
        public List<CinematicDynamicLightBlock> DynamicLights;
        
        [TagStructure(Size = 0x88)]
        public class AuthoredLightProbeBlockStruct : TagStructure
        {
            public List<AuthoredLightProbeLightsBlock> Lights;
            public float AuthoredLightProbeIntensityScale;
            public float GeneratedAirProbeIntensityScale;
            [TagField(Length = 27)]
            public RealRgbLightprobeArray[]  RawShData;
            public MidnightBooleanEnum IsCameraSpace;
            public MidnightBooleanEnum ApplyToFirstPersonGeometry;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public float IoDirectLightingMinimumPercentage;
            
            public enum MidnightBooleanEnum : sbyte
            {
                Off,
                On
            }
            
            [TagStructure(Size = 0x38)]
            public class AuthoredLightProbeLightsBlock : TagStructure
            {
                public float Direction1;
                public float FrontBack1;
                public RealRgbColor DirectColor1;
                public float DirectIntensity1;
                public float Direction2;
                public float FrontBack2;
                public RealRgbColor DirectColor2;
                public float DirectIntensity2;
                public float AuthoredLightProbeIntensityScale;
                public float GeneratedAirProbeIntensityScale;
            }
            
            [TagStructure(Size = 0x4)]
            public class RealRgbLightprobeArray : TagStructure
            {
                public float ShData;
            }
        }
        
        [TagStructure(Size = 0x60)]
        public class HologramlightingBlockStruct : TagStructure
        {
            public float Intensity;
            public float IntensityInput;
            public HologramtransparencyMode TransparencyMode;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Hologramlight KeyLight;
            public Hologramlight FillLight;
            public Hologramlight RimLight;
            
            public enum HologramtransparencyMode : sbyte
            {
                Cheap,
                Expensive
            }
            
            [TagStructure(Size = 0x1C)]
            public class Hologramlight : TagStructure
            {
                public float Direction;
                public float FrontBack;
                public float Intensity;
                public float IntensityInput;
                public float ForwardInput;
                public float RightInput;
                public float UpInput;
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class CinematicDynamicLightBlock : TagStructure
        {
            public CinematicDynamicLightFlags Flags;
            public float Direction;
            public float FrontBack;
            public float Distance; // world units
            [TagField(ValidTags = new [] { "ligh" })]
            public CachedTag Light;
            
            [Flags]
            public enum CinematicDynamicLightFlags : uint
            {
                DebugThisLight = 1 << 0,
                FollowObject = 1 << 1,
                PositionAtMarker = 1 << 2
            }
        }
    }
}
