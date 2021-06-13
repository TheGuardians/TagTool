using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "squad_template", Tag = "sqtm", Size = 0x10)]
    public class SquadTemplate : TagStructure
    {
        public StringId Name;
        public List<CellTemplateBlockStruct> CellTemplates;
        
        [TagStructure(Size = 0x58)]
        public class CellTemplateBlockStruct : TagStructure
        {
            public StringId Name;
            public AiSpawnConditionsStruct PlaceOn;
            // initial number of actors on normal difficulty
            public short NormalDiffCount;
            public MajorUpgradeEnum MajorUpgrade;
            public List<CharacterRefChoiceBlockStruct> CharacterType;
            public List<WeaponRefChoiceBlockStruct> InitialWeapon;
            public List<WeaponRefChoiceBlockStruct> InitialSecondaryWeapon;
            public List<EquipmentRefChoiceBlockStruct> InitialEquipment;
            public GlobalAiGrenadeTypeEnum GrenadeType;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "vehi" })]
            public CachedTag VehicleType;
            public StringId VehicleVariant;
            public StringId ActivityName;
            
            public enum MajorUpgradeEnum : short
            {
                Normal,
                Few,
                Many,
                None,
                All
            }
            
            public enum GlobalAiGrenadeTypeEnum : short
            {
                None,
                HumanGrenade,
                CovenantPlasma,
                BruteClaymore,
                Firebomb
            }
            
            [TagStructure(Size = 0x4)]
            public class AiSpawnConditionsStruct : TagStructure
            {
                public GlobalCampaignDifficultyEnum DifficultyFlags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum GlobalCampaignDifficultyEnum : ushort
                {
                    Easy = 1 << 0,
                    Normal = 1 << 1,
                    Heroic = 1 << 2,
                    Legendary = 1 << 3
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class CharacterRefChoiceBlockStruct : TagStructure
            {
                public AiSpawnConditionsStruct PlaceOn;
                [TagField(ValidTags = new [] { "char" })]
                public CachedTag CharacterType;
                public short Chance;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x18)]
            public class WeaponRefChoiceBlockStruct : TagStructure
            {
                public AiSpawnConditionsStruct PlaceOn;
                [TagField(ValidTags = new [] { "weap" })]
                public CachedTag WeaponType;
                public short Chance;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x18)]
            public class EquipmentRefChoiceBlockStruct : TagStructure
            {
                public AiSpawnConditionsStruct PlaceOn;
                [TagField(ValidTags = new [] { "eqip" })]
                public CachedTag EquipmentType;
                public short Chance;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
    }
}
