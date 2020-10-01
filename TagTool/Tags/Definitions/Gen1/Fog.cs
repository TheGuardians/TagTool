using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "fog", Tag = "fog ", Size = 0x18C)]
    public class Fog : TagStructure
    {
        /// <summary>
        /// Setting atmosphere dominant prevents polygon popping when the atmospheric fog maximum density (in the sky tag) is 1 and
        /// the atmospheric fog opaque distance is less than the diameter of the map. However, this flag will cause artifacts when
        /// the camera goes below the fog plane - so it should only be used when the fog plane is close to the ground.
        /// </summary>
        public FlagsValue Flags;
        [TagField(Length = 0x4)]
        public byte[] Padding;
        [TagField(Length = 0x4C)]
        public byte[] Padding1;
        [TagField(Length = 0x4)]
        public byte[] Padding2;
        /// <summary>
        /// planar fog density is clamped to this value
        /// </summary>
        public float MaximumDensity; // [0,1]
        [TagField(Length = 0x4)]
        public byte[] Padding3;
        /// <summary>
        /// the fog becomes opaque at this distance from the viewer
        /// </summary>
        public float OpaqueDistance; // world units
        [TagField(Length = 0x4)]
        public byte[] Padding4;
        /// <summary>
        /// the fog becomes opaque at this distance from its surface
        /// </summary>
        public float OpaqueDepth; // world units
        [TagField(Length = 0x8)]
        public byte[] Padding5;
        /// <summary>
        /// the fog becomes water at this distance from its surface
        /// </summary>
        public float DistanceToWaterPlane; // world units
        public RealRgbColor Color;
        public Flags1Value Flags1;
        /// <summary>
        /// 0 layers disables the effect
        /// </summary>
        public short LayerCount; // [0,4]
        public Bounds<float> DistanceGradient; // world units
        public Bounds<float> DensityGradient; // [0,1]
        /// <summary>
        /// do NOT set this to the same value as maximum_depth
        /// </summary>
        public float StartDistanceFromFogPlane; // world units
        [TagField(Length = 0x4)]
        public byte[] Padding6;
        /// <summary>
        /// 0 defaults to planar fog color
        /// </summary>
        public ArgbColor Color1;
        public float RotationMultiplier; // [0,1]
        public float StrafingMultiplier; // [0,1]
        public float ZoomMultiplier; // [0,1]
        [TagField(Length = 0x8)]
        public byte[] Padding7;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float MapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Map;
        public float AnimationPeriod; // seconds
        [TagField(Length = 0x4)]
        public byte[] Padding8;
        public Bounds<float> WindVelocity; // world units per second
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public Bounds<float> WindPeriod; // seconds
        public float WindAccelerationWeight; // [0,1]
        public float WindPerpendicularWeight; // [0,1]
        [TagField(Length = 0x8)]
        public byte[] Padding9;
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag BackgroundSound;
        [TagField(ValidTags = new [] { "snde" })]
        public CachedTag SoundEnvironment;
        [TagField(Length = 0x78)]
        public byte[] Padding10;
        
        public enum FlagsValue : uint
        {
            IsWater,
            AtmosphereDominant,
            FogScreenOnly
        }
        
        public enum Flags1Value : ushort
        {
            NoEnvironmentMultipass,
            NoModelMultipass,
            NoTextureBasedFalloff
        }
    }
}

