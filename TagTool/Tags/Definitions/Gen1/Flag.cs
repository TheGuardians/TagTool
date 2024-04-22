using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "flag", Tag = "flag", Size = 0x60)]
    public class Flag : TagStructure
    {
        public FlagsValue Flags;
        public TrailingEdgeShapeValue TrailingEdgeShape;
        /// <summary>
        /// zero places the shape exactly on the trailing edge, positive numbers push it off the edge
        /// </summary>
        public short TrailingEdgeShapeOffset; // vertices
        public AttachedEdgeShapeValue AttachedEdgeShape;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        /// <summary>
        /// flag size from attached to trailing edge
        /// </summary>
        public short Width; // vertices
        /// <summary>
        /// flag size along the direction of attachment (should be odd)
        /// </summary>
        public short Height; // vertices
        /// <summary>
        /// width of the cell between each pair of vertices
        /// </summary>
        public float CellWidth; // world units
        /// <summary>
        /// height of the cell between each pair of vertices
        /// </summary>
        public float CellHeight; // world units
        [TagField(ValidTags = new [] { "shdr" })]
        public CachedTag RedFlagShader;
        [TagField(ValidTags = new [] { "pphy" })]
        public CachedTag Physics;
        public float WindNoise; // world units per second^2
        [TagField(Length = 0x8)]
        public byte[] Padding1;
        [TagField(ValidTags = new [] { "shdr" })]
        public CachedTag BlueFlagShader;
        /// <summary>
        /// attachment points determine where the flag is attached
        /// </summary>
        public List<FlagAttachmentPointBlock> AttachmentPoints;
        
        [Flags]
        public enum FlagsValue : uint
        {
        }
        
        public enum TrailingEdgeShapeValue : short
        {
            Flat,
            ConcaveTriangular,
            ConvexTriangular,
            TrapezoidShortTop,
            TrapezoidShortBottom
        }
        
        public enum AttachedEdgeShapeValue : short
        {
            Flat,
            ConcaveTriangular
        }
        
        [TagStructure(Size = 0x34)]
        public class FlagAttachmentPointBlock : TagStructure
        {
            /// <summary>
            /// flag vertices between this attachment point and the next
            /// </summary>
            public short HeightToNextAttachment; // vertices
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            [TagField(Length = 32)]
            public string MarkerName;
        }
    }
}

