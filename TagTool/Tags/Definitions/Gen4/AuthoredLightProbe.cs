using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "authored_light_probe", Tag = "aulp", Size = 0x88)]
    public class AuthoredLightProbe : TagStructure
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
}
