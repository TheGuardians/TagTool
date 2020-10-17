using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "patchy_fog", Tag = "fpch", Size = 0x50)]
    public class PatchyFog : TagStructure
    {
        /// <summary>
        /// Use the separate_layer_depths flag carefully, it's expensive!
        /// </summary>
        public FlagsValue Flags;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public float RotationMultiplier; // [0,1]
        public float StrafingMultiplier; // [0,1]
        public float ZoomMultiplier; // [0,1]
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float NoiseMapScale;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag NoiseMap;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float NoiseVerticalScaleForward;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float NoiseVerticalScaleUp;
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
        public float NoiseOpacityScaleUp;
        public float AnimationPeriod; // seconds
        public Bounds<float> WindVelocity; // world units per second
        /// <summary>
        /// 0 defaults to 1
        /// </summary>
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

