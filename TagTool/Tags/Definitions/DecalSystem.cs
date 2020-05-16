using TagTool.Common;
using System.Collections.Generic;
using System;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "decal_system", Tag = "decs", Size = 0x24)]
    public class DecalSystem : TagStructure
	{
        public FlagsValue Flags;

        /// <summary>
        /// Bigger sizes keep more decals around but use much more memory
        /// </summary>
        [TagField(Format = "Triangles")]
        public short RingBufferSize;

        /// <summary>
        /// Above entry is for MP -- SP can be different
        /// </summary>
        [TagField(Format = "Triangles")]
        public short RingBufferSizeSinglePlayer;

        /// <summary>
        /// Material shader lifetime is modulated from 1 to 0 over this time
        /// </summary>
        public float MaterialShaderFadeTime;

        /// <summary>
        /// If set to non-zero, this will override manual scaling in Sapien and smash it with these values
        /// </summary>
        public RealPoint2d DecalScaleOverride;

        public List<DecalDefinitionBlock> Decal;

        public float RuntimeMaxRadius;

        [Flags]
        public enum FlagsValue : int
        {
            None = 0,
            Bit0 = 1 << 0,
            Bit1 = 1 << 1,
            Bit2 = 1 << 2,
            Bit3 = 1 << 3,
            Bit4 = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15,
            Bit16 = 1 << 16,
            Bit17 = 1 << 17,
            Bit18 = 1 << 18,
            Bit19 = 1 << 19,
            Bit20 = 1 << 20,
            Bit21 = 1 << 21,
            Bit22 = 1 << 22,
            Bit23 = 1 << 23,
            Bit24 = 1 << 24,
            Bit25 = 1 << 25,
            Bit26 = 1 << 26,
            Bit27 = 1 << 27,
            Bit28 = 1 << 28,
            Bit29 = 1 << 29,
            Bit30 = 1 << 30,
            Bit31 = 1 << 31
        }

        [TagStructure(Size = 0x74)]
        public class DecalDefinitionBlock : TagStructure
		{
            public StringId DecalName;

            public FlagsValue Flags;

            public RenderMethod RenderMethod;

            [TagField(Format = "World Units")]
            public Bounds<float> Radius;

            [TagField(Format = "Seconds")]
            public Bounds<float> DecayTime;

            [TagField(Format = "Seconds")]
            public Bounds<float> Lifespan;

            /// <summary>
            /// Projections at greater than this angle will be clamped to this angle
            /// </summary>
            [TagField(Format = "Degrees")]
            public float ClampAngle;

            /// <summary>
            /// Projections at greater than this angle will not be drawn
            /// </summary>
            [TagField(Format = "Degrees")]
            public float CullAngle;

            public int Unknown2; // more flags?

            public float DepthBias;
            public float RuntimeBitmapAspect;

            [Flags]
            public enum FlagsValue : int
            {
                None = 0,
                CustomBlendFactor = 1 << 0,
                Bit1 = 1 << 1,
                Bit2 = 1 << 2,
                Bit3 = 1 << 3,
                Bit4 = 1 << 4,
                Bit5 = 1 << 5,
                Bit6 = 1 << 6,
                Bit7 = 1 << 7,
                Bit8 = 1 << 8,
                Bit9 = 1 << 9,
                Bit10 = 1 << 10,
                Bit11 = 1 << 11,
                Bit12 = 1 << 12,
                Bit13 = 1 << 13,
                Bit14 = 1 << 14,
                Bit15 = 1 << 15,
                Bit16 = 1 << 16,
                Bit17 = 1 << 17,
                Bit18 = 1 << 18,
                Bit19 = 1 << 19,
                Bit20 = 1 << 20,
                Bit21 = 1 << 21,
                Bit22 = 1 << 22,
                Bit23 = 1 << 23,
                Bit24 = 1 << 24,
                Bit25 = 1 << 25,
                Bit26 = 1 << 26,
                Bit27 = 1 << 27,
                Bit28 = 1 << 28,
                Bit29 = 1 << 29,
                Bit30 = 1 << 30,
                Bit31 = 1 << 31
            }
        }
    }
}
