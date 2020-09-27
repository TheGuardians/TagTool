using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "patchy_fog", Tag = "fpch", Size = 0x58)]
    public class PatchyFog : TagStructure
    {
        /// <summary>
        /// PATCHY FOG
        /// </summary>
        /// <remarks>
        /// Use the separate_layer_depths flag carefully, it's expensive!
        /// </remarks>
        public FlagsValue Flags;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        /// <summary>
        /// MOVEMENT MODIFIERS
        /// </summary>
        public float RotationMultiplier; // [0,1]
        public float StrafingMultiplier; // [0,1]
        public float ZoomMultiplier; // [0,1]
        /// <summary>
        /// NOISE MAP
        /// </summary>
        public float NoiseMapScale; // 0 defaults to 1
        public CachedTag NoiseMap;
        public float NoiseVerticalScaleForward; // 0 defaults to 1
        public float NoiseVerticalScaleUp; // 0 defaults to 1
        public float NoiseOpacityScaleUp; // 0 defaults to 1
        /// <summary>
        /// ANIMATION
        /// </summary>
        public float AnimationPeriod; // seconds
        public Bounds<float> WindVelocity; // world units per second
        public Bounds<float> WindPeriod; // seconds
        public float WindAccelerationWeight; // [0,1]
        public float WindPerpendicularWeight; // [0,1]
        public float WindConstantVelocityX; // world units per second
        public float WindConstantVelocityY; // world units per second
        public float WindConstantVelocityZ; // world units per second
        
        [Flags]
        public enum FlagsValue : ushort
        {
            SeparateLayerDepths = 1 << 0,
            SortBehindTransparents = 1 << 1
        }
    }
}

