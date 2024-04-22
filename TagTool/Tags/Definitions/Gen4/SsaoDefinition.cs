using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "ssao_definition", Tag = "ssao", Size = 0xB0)]
    public class SsaoDefinition : TagStructure
    {
        public SsaoDownsampleEnum Downsample;
        public SsaoTypeEnum SsaoType;
        public byte Version;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // how much ssao shadowing darkens the ambient lighting
        public float HdaoAmbientShadow;
        // how much ssao shadowing darkens the point lighting ie the sun
        public float HdaoPointShadow;
        // how much the sharp dynamic shadows darken ambient lighting
        public float SharpAmbientShadow;
        // how much the sharp dynamic shadows darken the point lighting ie the sun
        public float SharpPointShadow;
        // unshadowed areas will have this relative intensity
        public float MaxIntensity; // [0-1]
        // fully shadowed areas will have this relative intensity
        public float MinIntensity; // [0-1]
        // controls the shape of the shadow falloff curve
        public float FalloffWidth;
        // controls shadow sensitivity to corners
        public float CornerScale;
        // offsets corner sensitivity, generally to ignore shallow corners
        public float CornerBias;
        // controls the maximum shadow distance, as a ratio of the distance to the object
        public float BoundsScale;
        // offsets the maximum shadow distance, generally to make a region of full darkness before fading
        public float BoundsOffset;
        // completely removes ssao on anything closer than this
        public float NearClip; // world units
        // distance over which the ssao fades in
        public float NearFadeWidth; // world units
        // completely removes ssao on anything farther than this
        public float FarClip; // world units
        // distance over which the ssao fades in
        public float FarFadeWidth; // world units
        public RealArgbColor RuntimeData0;
        public RealArgbColor RuntimeData1;
        public RealArgbColor RuntimeData2;
        public RealArgbColor RuntimeData3;
        public RealArgbColor RuntimeData4;
        public RealArgbColor RuntimeData5;
        public RealArgbColor RuntimeData7;
        
        public enum SsaoDownsampleEnum : sbyte
        {
            BlockDownsample,
            CloverDownsample
        }
        
        public enum SsaoTypeEnum : sbyte
        {
            Off,
            HdaoLarge32Sample,
            HdaoLarge64Predicated,
            HdaoSmall24Sample,
            HdaoScreenshot,
            DebugMask,
            SsaoMidnight16Sample
        }
    }
}
