using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "squad_template", Tag = "sqtm", Size = 0x10, MinVersion = CacheVersion.Halo3ODST)]
    public class SquadTemplate : TagStructure
	{
        public StringId Name;
        public List<CellTemplate> CellTemplates;
        
        [TagStructure(Size = 0x60)]
        public class CellTemplate : TagStructure
		{
            public StringId Name;

            public DifficultyFlagsValue DifficultyFlags;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;

            public Bounds<short> RoundRange;
            public Bounds<short> SetRange;

            public short NormalDiffCount; // initial number of actors on normal difficulty
            public MajorUpgradeEnum MajorUpgrade;

            public List<ObjectBlock> Characters;
            public List<ObjectBlock> InitialWeapons;
            public List<ObjectBlock> InitialSecondaryWeapons;
            public List<ObjectBlock> InitialEquipment;

            public CharacterGrenadeType GrenadeType;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;

            [TagField(ValidTags = new[] { "vehi" })]
            public CachedTag Vehicle;

            public StringId VehicleVariant;
            public StringId ActivityName;

            [Flags]
            public enum DifficultyFlagsValue : ushort
            {
                None = 0,
                Easy = 1 << 0,
                Normal = 1 << 1,
                Heroic = 1 << 2,
                Legendary = 1 << 3
            }

            public enum MajorUpgradeEnum : short
            {
                Normal,
                Few,
                Many,
                None,
                All
            }

            [TagStructure(Size = 0x20)]
            public class ObjectBlock : TagStructure
			{
                public DifficultyFlagsValue DifficultyFlags;

                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;

                public Bounds<short> RoundRange;
                public Bounds<short> SetRange;

                public CachedTag Object;
                public short Probability;

                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding2;
            }
        }
    }
}