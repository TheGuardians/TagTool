using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "cellular_automata", Tag = "devo", Size = 0x234)]
    public class CellularAutomata : TagStructure
    {
        /// <summary>
        /// parameters
        /// </summary>
        public short UpdatesPerSecond; // Hz
        public short XWidth; // cells
        public short YDepth; // cells
        public short ZHeight; // cells
        public float XWidth1; // world units
        public float YDepth2; // world units
        public float ZHeight3; // world units
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding1;
        public StringId Marker;
        /// <summary>
        /// cell birth
        /// </summary>
        public float CellBirthChance; // [0,1]
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding2;
        /// <summary>
        /// gene mutation
        /// </summary>
        public int CellGeneMutates1In; // times
        public int VirusGeneMutations1In; // times
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding3;
        /// <summary>
        /// cell infection
        /// </summary>
        public Bounds<short> InfectedCellLifespan; // updates
        public short MinimumInfectionAge; // updates
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding4;
        public float CellInfectionChance; // [0,1]
        public float InfectionThreshold; // [0,1]
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding5;
        /// <summary>
        /// initial state
        /// </summary>
        public float NewCellFilledChance; // [0,1]
        public float NewCellInfectedChance; // [0,1]
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding6;
        /// <summary>
        /// detail texture
        /// </summary>
        public float DetailTextureChangeChance; // [0,1]
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding7;
        public short DetailTextureWidth; // cells
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding8;
        public CachedTag DetailTexture;
        /// <summary>
        /// mask texture
        /// </summary>
        [TagField(Flags = Padding, Length = 32)]
        public byte[] Padding9;
        public CachedTag MaskBitmap;
        [TagField(Flags = Padding, Length = 240)]
        public byte[] Padding10;
    }
}

