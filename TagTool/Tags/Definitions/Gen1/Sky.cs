using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "sky", Tag = "sky ", Size = 0xD0)]
    public class Sky : TagStructure
    {
        [TagField(ValidTags = new [] { "mod2" })]
        public CachedTag Model;
        [TagField(ValidTags = new [] { "antr" })]
        public CachedTag AnimationGraph;
        [TagField(Length = 0x18)]
        public byte[] Padding;
        /// <summary>
        /// the indoor ambient light color
        /// </summary>
        public RealRgbColor IndoorAmbientRadiosityColor;
        /// <summary>
        /// the indoor ambient light power from 0 to infinity
        /// </summary>
        public float IndoorAmbientRadiosityPower;
        /// <summary>
        /// the outdoor ambient light color
        /// </summary>
        public RealRgbColor OutdoorAmbientRadiosityColor;
        /// <summary>
        /// the outdoor ambient light power from 0 to infinity
        /// </summary>
        public float OutdoorAmbientRadiosityPower;
        public RealRgbColor OutdoorFogColor;
        [TagField(Length = 0x8)]
        public byte[] Padding1;
        /// <summary>
        /// density at opaque distance - 0 defaults to 1
        /// </summary>
        public float OutdoorFogMaximumDensity; // [0,1]
        /// <summary>
        /// below this distance there is no fog
        /// </summary>
        public float OutdoorFogStartDistance; // world units
        /// <summary>
        /// beyond this distance surfaces are completely fogged
        /// </summary>
        public float OutdoorFogOpaqueDistance; // world units
        public RealRgbColor IndoorFogColor;
        [TagField(Length = 0x8)]
        public byte[] Padding2;
        /// <summary>
        /// density at opaque distance - 0 defaults to 1
        /// </summary>
        public float IndoorFogMaximumDensity; // [0,1]
        /// <summary>
        /// below this distance there is no fog
        /// </summary>
        public float IndoorFogStartDistance; // world units
        /// <summary>
        /// beyond this distance surfaces are completely fogged
        /// </summary>
        public float IndoorFogOpaqueDistance; // world units
        /// <summary>
        /// used for FOG SCREEN only; not used for planar fog
        /// </summary>
        [TagField(ValidTags = new [] { "fog " })]
        public CachedTag IndoorFogScreen;
        [TagField(Length = 0x4)]
        public byte[] Padding3;
        public List<SkyShaderFunctionBlock> ShaderFunctions;
        public List<SkyAnimationBlock> Animations;
        public List<SkyLightBlock> Lights;
        
        [TagStructure(Size = 0x24)]
        public class SkyShaderFunctionBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Padding;
            /// <summary>
            /// the global function that controls this shader value
            /// </summary>
            [TagField(Length = 32)]
            public string GlobalFunctionName;
        }
        
        [TagStructure(Size = 0x24)]
        public class SkyAnimationBlock : TagStructure
        {
            /// <summary>
            /// the index of the animation in the animation graph
            /// </summary>
            public short AnimationIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public float Period; // seconds
            [TagField(Length = 0x1C)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x74)]
        public class SkyLightBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "lens" })]
            public CachedTag LensFlare;
            /// <summary>
            /// the lens flare for this light will be attached to the specified marker in the model
            /// </summary>
            [TagField(Length = 32)]
            public string LensFlareMarkerName;
            [TagField(Length = 0x1C)]
            public byte[] Padding;
            /// <summary>
            /// these parameters control how the light illuminates the world.
            /// </summary>
            public FlagsValue Flags;
            /// <summary>
            /// light color
            /// </summary>
            public RealRgbColor Color;
            /// <summary>
            /// light power from 0 to infinity
            /// </summary>
            public float Power;
            /// <summary>
            /// the length of the ray for shadow testing.
            /// </summary>
            public float TestDistance;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            /// <summary>
            /// direction toward the light source in the sky.
            /// </summary>
            public RealEulerAngles2d Direction;
            /// <summary>
            /// angular diameter of the light source in the sky.
            /// </summary>
            public Angle Diameter;
            
            [Flags]
            public enum FlagsValue : uint
            {
                AffectsExteriors = 1 << 0,
                AffectsInteriors = 1 << 1
            }
        }
    }
}

