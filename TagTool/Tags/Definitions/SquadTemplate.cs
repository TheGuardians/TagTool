using TagTool.Ai;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "squad_template", Tag = "sqtm", Size = 0x10, MinVersion = CacheVersion.Halo3ODST)]
    public class SquadTemplate
    {
        public StringId Name;
        public List<CellTemplate> CellTemplates;
        
        [TagStructure(Size = 0x60)]
        public class CellTemplate
        {
            public StringId Name;

            public DifficultyFlagsValue DifficultyFlags;

            [TagField(Padding = true, Length = 2)]
            public byte[] Padding1;

            public short MinimumRound;
            public short MaximumRound;
            public short Unknown2;

            public short Unknown3;
            public short Count;
            public short Unknown4;

            public List<ObjectBlock> Characters;
            public List<ObjectBlock> InitialWeapons;
            public List<ObjectBlock> InitialSecondaryWeapons;
            public List<ObjectBlock> InitialEquipment;

            public CharacterGrenadeType GrenadeType;

            [TagField(Padding = true, Length = 2)]
            public byte[] Padding2;

            public CachedTagInstance Vehicle;
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

            [TagStructure(Size = 0x20)]
            public class ObjectBlock
            {
                public DifficultyFlagsValue DifficultyFlags;

                [TagField(Padding = true, Length = 2)]
                public byte[] Padding1;

                public short MinimumRound;
                public short MaximumRound;
                public uint Unknown3;

                public CachedTagInstance Object;
                public short Probability;

                [TagField(Padding = true, Length = 2)]
                public byte[] Padding2;
            }
        }
    }
}