using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "cellular_automata2d", Tag = "whip", Size = 0x22C)]
    public class CellularAutomata2d : TagStructure
    {
        /// <summary>
        /// properties
        /// </summary>
        public short UpdatesPerSecond; // Hz
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public float DeadCellPenalty;
        public float LiveCellBonus;
        [TagField(Flags = Padding, Length = 80)]
        public byte[] Padding2;
        /// <summary>
        /// height map
        /// </summary>
        public short Width; // cells
        public short Height; // cells
        public float CellWidth; // world units
        public float Height1; // world units
        public RealVector2d Velocity; // cells/update
        [TagField(Flags = Padding, Length = 28)]
        public byte[] Padding3;
        public StringId Marker;
        public InterpolationFlagsValue InterpolationFlags;
        public RealRgbColor BaseColor;
        public RealRgbColor PeakColor;
        [TagField(Flags = Padding, Length = 76)]
        public byte[] Padding4;
        /// <summary>
        /// detail map
        /// </summary>
        public short Width2; // cells
        public short Height3; // cells
        public float CellWidth4; // world units
        public RealVector2d Velocity5; // cells/update
        [TagField(Flags = Padding, Length = 48)]
        public byte[] Padding5;
        public StringId Marker6;
        public short TextureWidth; // cells
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding6;
        [TagField(Flags = Padding, Length = 48)]
        public byte[] Padding7;
        public CachedTag Texture;
        [TagField(Flags = Padding, Length = 160)]
        public byte[] Padding8;
        public List<Ca2dRule> Rules;
        
        [Flags]
        public enum InterpolationFlagsValue : uint
        {
            BlendInHsv = 1 << 0,
            MoreColors = 1 << 1
        }
        
        [TagStructure(Size = 0x58)]
        public class Ca2dRule : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public RealRgbColor TintColor;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
            public List<Ca2dRuleState> States;
            
            [TagStructure(Size = 0x60)]
            public class Ca2dRuleState : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                public RealRgbColor Color;
                public short CountsAs; // neighbors
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public float InitialPlacementWeight;
                [TagField(Flags = Padding, Length = 24)]
                public byte[] Padding2;
                public short Zero;
                public short One;
                public short Two;
                public short Three;
                public short Four;
                public short Five;
                public short Six;
                public short Seven;
                public short Eight;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding3;
            }
        }
    }
}

