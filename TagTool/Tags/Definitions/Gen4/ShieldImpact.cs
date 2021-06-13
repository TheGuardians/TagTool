using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "shield_impact", Tag = "shit", Size = 0x20C)]
    public class ShieldImpact : TagStructure
    {
        public ShieldFlags Flags;
        public short Version;
        public float RecentDamageIntensity;
        public float CurrentDamageIntensity;
        public float DepthFadeRange; // world units
        public float OuterFadeRadius; // [0-1]
        public float CenterRadius; // [0-1]
        public float InnerFadeRadius; // [0-1]
        public ShieldColorFunctionStruct EdgeGlowColor;
        public ShieldScalarFunctionStruct EdgeGlowIntensity;
        public float PlasmaDepthFadeRange; // world units
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag PlasmaNoiseTexture0;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag PlasmaNoiseTexture1;
        public float TilingScale;
        public float ScrollSpeed;
        public float EdgeSharpness;
        public float CenterSharpness;
        public float PlasmaOuterFadeRadius; // [0-1]
        public float PlasmaCenterRadius; // [0-1]
        public float PlasmaInnerFadeRadius; // [0-1]
        public ShieldColorFunctionStruct PlasmaCenterColor;
        public ShieldScalarFunctionStruct PlasmaCenterIntensity;
        public ShieldColorFunctionStruct PlasmaEdgeColor;
        public ShieldScalarFunctionStruct PlasmaEdgeIntensity;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag OscillationTexture0;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag OscillationTexture1;
        public float OscillationTilingScale;
        public float OscillationScrollSpeed;
        public ShieldScalarFunctionStruct ExtrusionAmount;
        public ShieldScalarFunctionStruct OscillationAmplitude;
        public float HitTime; // seconds
        public ShieldColorFunctionStruct HitColor;
        public ShieldScalarFunctionStruct HitIntensity;
        public ShieldScalarFunctionStruct HitRadius;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag HitBlobTexture;
        public RealQuaternion EdgeScales;
        public RealQuaternion EdgeOffsets;
        public RealQuaternion PlasmaScales;
        public RealQuaternion DepthFadeParams;
        
        [Flags]
        public enum ShieldFlags : ushort
        {
            RenderAlways = 1 << 0,
            RenderFirstPerson = 1 << 1,
            DontRenderThirdPerson = 1 << 2
        }
        
        [TagStructure(Size = 0x1C)]
        public class ShieldColorFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public MappingFunction Mapping;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class ShieldScalarFunctionStruct : TagStructure
        {
            public StringId InputVariable;
            public StringId RangeVariable;
            public MappingFunction Mapping;
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
    }
}
