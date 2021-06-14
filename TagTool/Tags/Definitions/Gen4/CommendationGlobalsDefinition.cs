using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "commendation_globals_definition", Tag = "comg", Size = 0x40)]
    public class CommendationGlobalsDefinition : TagStructure
    {
        [TagField(ValidTags = new [] { "coag" })]
        public CachedTag CommendationAggregators;
        [TagField(ValidTags = new [] { "meco" })]
        public CachedTag MedalAggregators;
        [TagField(ValidTags = new [] { "unic" })]
        public CachedTag CommendationText;
        public short ProgressDisplayTime; // seconds
        public short CompleteDisplayTime; // seconds
        public List<Commendationblock> Commendations;
        
        [TagStructure(Size = 0x84)]
        public class Commendationblock : TagStructure
        {
            public StringId Name;
            public StringId Description;
            public Commendationflags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public CommendationcategoryEnum Category;
            public CommendationsubcategoryEnum Subcategory;
            public sbyte SequenceIndex;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            public short MedalSpriteIndex;
            public short GameTypeSpriteIndex;
            public PurchasePrerequisitesUnifiedDefinitionBlock Prerequisites;
            public List<CommendationlevelBlock> Levels;
            
            [Flags]
            public enum Commendationflags : byte
            {
                Hidden = 1 << 0
            }
            
            public enum CommendationcategoryEnum : sbyte
            {
                Default,
                Weapons,
                Ordnance,
                Enemies,
                Vehicles,
                Player,
                ArmorAbilities,
                Objectives,
                Customs,
                Ugc
            }
            
            public enum CommendationsubcategoryEnum : sbyte
            {
                Default,
                Unsc,
                Covenant,
                Forerunner,
                CovenantEnemy,
                ForerunnerEnemy,
                Campaign,
                WarGames,
                Slayer,
                Regicide,
                Ctf,
                Extraction,
                Oddball,
                KingOfTheHill,
                Dominion,
                Flood
            }
            
            [TagStructure(Size = 0x64)]
            public class PurchasePrerequisitesUnifiedDefinitionBlock : TagStructure
            {
                public StringId PrerequisitePurchasedItemErrorString;
                public StringId PrerequisiteUnlockableErrorString;
                public StringId PrerequisiteOffersErrorString;
                public int PrerequisiteEnlistmentCount;
                public List<PurchasePrerequisiteGradeDefinitionBlock> PrerequisiteGrades;
                public List<PurchasePrerequisiteCommendationDefinitionBlock> PrerequisiteCommendations;
                public List<PurchasePrerequisitePurchasedAppearanceItemDefinitionBlock> PrerequisiteAppearancePurchasedItems;
                public List<PurchasePrerequisitePurchasedLoadoutItemDefinitionBlock> PrerequisiteLoadoutPurchasedItems;
                public List<PurchasePrerequisitePurchasedOrdnanceItemDefinitionBlock> PrerequisiteOrdnancePurchasedItems;
                public List<PurchasePrerequisitesUnlockableDefinitionBlock> PrerequisiteUnlockables;
                public List<PurchasePrerequisitesOfferDefinitionBlock> PrerequisiteOffers;
                
                [TagStructure(Size = 0x8)]
                public class PurchasePrerequisiteGradeDefinitionBlock : TagStructure
                {
                    public int EnlistmentIndex;
                    public int MinimumGrade;
                }
                
                [TagStructure(Size = 0x8)]
                public class PurchasePrerequisiteCommendationDefinitionBlock : TagStructure
                {
                    // This must match one of the commendation names.
                    public StringId CommendationId;
                    public AggregatordependentTypeEnum Type;
                    // This field is ignored if 'type' is set to 'aggregator'.
                    public sbyte MinimumLevel;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    public enum AggregatordependentTypeEnum : sbyte
                    {
                        Commendation,
                        Aggregator
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class PurchasePrerequisitePurchasedAppearanceItemDefinitionBlock : TagStructure
                {
                    public PurchaseAppearanceDefinitionReferenceStruct ItemReference;
                    
                    [TagStructure(Size = 0x4)]
                    public class PurchaseAppearanceDefinitionReferenceStruct : TagStructure
                    {
                        public short ItemReference;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class PurchasePrerequisitePurchasedLoadoutItemDefinitionBlock : TagStructure
                {
                    public PurchaseLoadoutDefinitionReferenceStruct ItemReference;
                    
                    [TagStructure(Size = 0x4)]
                    public class PurchaseLoadoutDefinitionReferenceStruct : TagStructure
                    {
                        public short ItemReference;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class PurchasePrerequisitePurchasedOrdnanceItemDefinitionBlock : TagStructure
                {
                    public PurchaseOrdnanceDefinitionReferenceStruct ItemReference;
                    
                    [TagStructure(Size = 0x4)]
                    public class PurchaseOrdnanceDefinitionReferenceStruct : TagStructure
                    {
                        public short ItemReference;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class PurchasePrerequisitesUnlockableDefinitionBlock : TagStructure
                {
                    public StringId UnlockableName;
                }
                
                [TagStructure(Size = 0x4)]
                public class PurchasePrerequisitesOfferDefinitionBlock : TagStructure
                {
                    public MarketplaceOfferTypeEnum OfferType;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    
                    public enum MarketplaceOfferTypeEnum : sbyte
                    {
                        DeadeyeHelmet,
                        LocusHelmet,
                        GungnirPulse,
                        VenatorRaptor,
                        Cioweb,
                        HazopForest,
                        OceanicCircuit,
                        BattleRifleArctic,
                        BonebreakerEmblem,
                        AssassinEmblem,
                        BulletproofEmblem,
                        SpartanEmblem,
                        MjolnirEmblem,
                        Lce1Emblem,
                        SpartanIvarmorUnique,
                        AssaultRifleUnique,
                        Specializations,
                        UnicornEmblem,
                        UnicornArmor,
                        UnicornLightRifle,
                        LiveEmblem,
                        ScannerHelmet,
                        StriderHelmet,
                        FalconEmblem,
                        Reserved01,
                        Reserved02,
                        Reserved04,
                        Reserved08,
                        Reserved10,
                        Reserved20,
                        Reserved40,
                        ReservedDoNotUse80
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class CommendationlevelBlock : TagStructure
            {
                public StringId LevelName;
                // number of ticks to reach this level
                public short ProgressTicksToLevel;
                // number of ticks between displaying progress toast; 0=never display progress, 1=display every tick, 2=every other,
                // etc.
                public short ProgressDisplayInterval;
                public StringId Achievement;
                // rewards given for reaching this level
                public List<CommendationrewardBlock> LevelUpRewards;
                
                [TagStructure(Size = 0x4)]
                public class CommendationrewardBlock : TagStructure
                {
                    // Type of currency given by this reward.
                    public CurrencytypeEnum CurrencyType;
                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    public short RewardValue;
                    
                    public enum CurrencytypeEnum : sbyte
                    {
                        Cookies,
                        Xp
                    }
                }
            }
        }
    }
}
