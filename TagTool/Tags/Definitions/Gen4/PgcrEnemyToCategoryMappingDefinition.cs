using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "pgcr_enemy_to_category_mapping_definition", Tag = "pcec", Size = 0x18)]
    public class PgcrEnemyToCategoryMappingDefinition : TagStructure
    {
        public List<PgcrEnemyToCategoryListBlock> CharacterCategories;
        public List<PgcrEnemyToCategoryListBlock> VehicleCategories;
        
        [TagStructure(Size = 0x20)]
        public class PgcrEnemyToCategoryListBlock : TagStructure
        {
            public StringId CategoryDisplayName;
            public short SpriteIndex;
            public PgcrEnemyToCategoryEntryFlags Flags;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<PgcrPlayerToCategoryEntryBlock> PlayerTypes;
            public List<PgcrEnemyToCategoryEntryBlock> EnemyTypes;
            
            [Flags]
            public enum PgcrEnemyToCategoryEntryFlags : byte
            {
                CategoryContainsPlayers = 1 << 0
            }
            
            [TagStructure(Size = 0x4)]
            public class PgcrPlayerToCategoryEntryBlock : TagStructure
            {
                public PgcrPlayerTypeEnum PlayerType;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum PgcrPlayerTypeEnum : sbyte
                {
                    Unsc,
                    Covenant
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class PgcrEnemyToCategoryEntryBlock : TagStructure
            {
                public CampaignMetagameBucketTypeWithNoneEnum CharacterType;
                public CampaignMetagameBucketClassWithNoneEnum CharacterClass;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum CampaignMetagameBucketTypeWithNoneEnum : sbyte
                {
                    Any,
                    Brute,
                    Grunt,
                    Jackel,
                    Skirmisher,
                    Marine,
                    Spartan,
                    Bugger,
                    Hunter,
                    FloodInfection,
                    FloodCarrier,
                    FloodCombat,
                    FloodPure,
                    Sentinel,
                    Elite,
                    Engineer,
                    Mule,
                    Turret,
                    Mongoose,
                    Warthog,
                    Scorpion,
                    Hornet,
                    Pelican,
                    Revenant,
                    Seraph,
                    Shade,
                    Watchtower,
                    Ghost,
                    Chopper,
                    Mauler,
                    Wraith,
                    Banshee,
                    Phantom,
                    Scarab,
                    Guntower,
                    TuningFork,
                    Broadsword,
                    Mammoth,
                    Lich,
                    Mantis,
                    Wasp,
                    Phaeton,
                    Bishop,
                    Knight,
                    Pawn
                }
                
                public enum CampaignMetagameBucketClassWithNoneEnum : sbyte
                {
                    Any,
                    Infantry,
                    Leader,
                    Hero,
                    Specialist,
                    LightVehicle,
                    HeavyVehicle,
                    GiantVehicle,
                    StandardVehicle
                }
            }
        }
    }
}
