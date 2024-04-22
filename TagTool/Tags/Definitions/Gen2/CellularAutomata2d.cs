using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "cellular_automata2d", Tag = "whip", Size = 0x220)]
    public class CellularAutomata2d : TagStructure
    {
        public short UpdatesPerSecond; // Hz
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public float DeadCellPenalty;
        public float LiveCellBonus;
        [TagField(Length = 0x50, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        public short Width; // cells
        public short Height; // cells
        public float CellWidth; // world units
        public float Height1; // world units
        public RealVector2d Velocity; // cells/update
        [TagField(Length = 0x1C, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        public StringId Marker;
        public InterpolationFlagsValue InterpolationFlags;
        public RealRgbColor BaseColor;
        public RealRgbColor PeakColor;
        [TagField(Length = 0x4C, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public short Width1; // cells
        public short Height2; // cells
        public float CellWidth1; // world units
        public RealVector2d Velocity1; // cells/update
        [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        public StringId Marker1;
        public short TextureWidth; // cells
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        [TagField(Length = 0x30, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag Texture;
        [TagField(Length = 0xA0, Flags = TagFieldFlags.Padding)]
        public byte[] Padding7;
        public List<RulesBlock> Rules;
        
        [Flags]
        public enum InterpolationFlagsValue : uint
        {
            /// <summary>
            /// blends colors in hsv rather than rgb space
            /// </summary>
            BlendInHsv = 1 << 0,
            /// <summary>
            /// blends colors through more hues (goes the long way around the color wheel)
            /// </summary>
            MoreColors = 1 << 1
        }
        
        [TagStructure(Size = 0x54)]
        public class RulesBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealRgbColor TintColor;
            [TagField(Length = 0x20, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<StatesBlock> States;
            
            [TagStructure(Size = 0x60)]
            public class StatesBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public RealRgbColor Color;
                public short CountsAs; // neighbors
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float InitialPlacementWeight;
                [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public short Zero;
                public short One;
                public short Two;
                public short Three;
                public short Four;
                public short Five;
                public short Six;
                public short Seven;
                public short Eight;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
            }
        }
    }
}

