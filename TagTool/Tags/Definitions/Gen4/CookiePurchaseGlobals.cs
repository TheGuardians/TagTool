using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "cookie_purchase_globals", Tag = "cpgd", Size = 0x150)]
    public class CookiePurchaseGlobals : TagStructure
    {
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> HelmetPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> LeftShoulderPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> RightShoulderPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> ChestPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> LegsPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> ArmsPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyLoadoutDefinitionBlock> App1LoadoutFamilies;
        public List<CookiePurchaseFamilyLoadoutDefinitionBlock> App2LoadoutFamilies;
        public List<CookiePurchaseFamilyLoadoutDefinitionBlock> PrimaryWeaponLoadoutFamilies;
        public List<CookiePurchaseFamilyLoadoutDefinitionBlock> SecondaryWeaponLoadoutFamilies;
        public List<CookiePurchaseFamilyLoadoutDefinitionBlock> GrenadePurchasableLoadoutFamilies;
        public List<CookiePurchaseFamilyLoadoutDefinitionBlock> EquipmentPurchasableLoadoutFamilies;
        public List<CookiePurchaseFamilyLoadoutDefinitionBlock> SlotPurchasableLoadoutFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> VisorTintPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> EliteArmorPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> PrimaryEmblemPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> SecondaryEmblemPurchasableAppearanceFamilies;
        public List<CookiePurchaseFamilyOrdnanceDefinitionBlock> OrdnanceSlotPurchasableFamilies;
        public List<CookiePurchaseFamilyOrdnanceDefinitionBlock> OrdnancePurchasableFamilies;
        public List<CookiePurchaseFamilyAppearanceDefinitionBlock> PortraitPosePurchasableFamilies;
        public List<CookiePurchaseAppearanceDefinitionBlock> PurchasableAppearanceItems;
        public List<CookiePurchaseExternalUnlockableBlockAppearanceDefinition> DlcAppearanceUnlockables;
        public List<CookiePurchaseExternalUnlockableBlockAppearanceDefinition> WaypointAppearanceUnlockables;
        public List<CookiePurchaseLoadoutDefinitionBlock> PurchasableLoadoutItems;
        public List<CookiePurchaseExternalUnlockableBlockLoadoutDefinition> DlcLoadoutUnlockables;
        public List<CookiePurchaseExternalUnlockableBlockLoadoutDefinition> WaypointLoadoutUnlockables;
        public List<CookiePurchaseOrdnanceDefinitionBlock> PurchasableOrdnanceItems;
        public List<CookiePurchaseExternalUnlockableBlockOrdnanceDefinition> DlcOrdnanceUnlockables;
        
        [TagStructure(Size = 0x28)]
        public class CookiePurchaseFamilyAppearanceDefinitionBlock : TagStructure
        {
            // If this is left blank, this family will be treated as a list of top-level items (items w/o a family, e.g. visor
            // tints).
            public StringId DisplayTitle;
            public StringId DisplayDescription;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DisplayBitmap;
            public int SpriteIndex;
            public List<CookiePurchaseAppearanceDefinitionReferenceBlock> PurchasableItems;
            
            [TagStructure(Size = 0x4)]
            public class CookiePurchaseAppearanceDefinitionReferenceBlock : TagStructure
            {
                public PurchaseAppearanceDefinitionReferenceStruct PurchasableItemReference;
                
                [TagStructure(Size = 0x4)]
                public class PurchaseAppearanceDefinitionReferenceStruct : TagStructure
                {
                    public short ItemReference;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class CookiePurchaseFamilyLoadoutDefinitionBlock : TagStructure
        {
            // If this is left blank, this family will be treated as a list of top-level items (items w/o a family, e.g. visor
            // tints).
            public StringId DisplayTitle;
            public StringId DisplayDescription;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DisplayBitmap;
            public int SpriteIndex;
            public List<CookiePurchaseLoadoutDefinitionReferenceBlock> PurchasableItems;
            
            [TagStructure(Size = 0x4)]
            public class CookiePurchaseLoadoutDefinitionReferenceBlock : TagStructure
            {
                public PurchaseLoadoutDefinitionReferenceStruct PurchasableItemReference;
                
                [TagStructure(Size = 0x4)]
                public class PurchaseLoadoutDefinitionReferenceStruct : TagStructure
                {
                    public short ItemReference;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
        }
        
        [TagStructure(Size = 0x28)]
        public class CookiePurchaseFamilyOrdnanceDefinitionBlock : TagStructure
        {
            // If this is left blank, this family will be treated as a list of top-level items (items w/o a family, e.g. visor
            // tints).
            public StringId DisplayTitle;
            public StringId DisplayDescription;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DisplayBitmap;
            public int SpriteIndex;
            public List<CookiePurchaseOrdnanceDefinitionReferenceBlock> PurchasableItems;
            
            [TagStructure(Size = 0x4)]
            public class CookiePurchaseOrdnanceDefinitionReferenceBlock : TagStructure
            {
                public PurchaseOrdnanceDefinitionReferenceStruct PurchasableItemReference;
                
                [TagStructure(Size = 0x4)]
                public class PurchaseOrdnanceDefinitionReferenceStruct : TagStructure
                {
                    public short ItemReference;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
        }
        
        [TagStructure(Size = 0x140)]
        public class CookiePurchaseAppearanceDefinitionBlock : TagStructure
        {
            public StringId PurchaseId;
            public StringId DisplayName;
            public StringId DisplayDescription;
            public StringId ExitExperienceAggregateGroupName;
            public CookiePurchaseFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DisplayBitmap;
            public int SpriteIndex;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DetailDisplayBitmap;
            public int CookieCost;
            public PurchasePrerequisitesUnifiedDefinitionBlock VisiblePrerequisites;
            public PurchasePrerequisitesUnifiedDefinitionBlock PurchasablePrerequisites;
            public PurchasePlayerAppearanceStruct PurchasePlayerAppearance;
            
            [Flags]
            public enum CookiePurchaseFlags : byte
            {
                // this item is automatically given to the player when they meet the conditions
                AutoBuy = 1 << 0,
                // this item will appear as 'worn' when the player isn't wearing any item (for 'default' items)
                Default = 1 << 1,
                // this item will NOT count toward an overall completion value
                DoesnTCountTowardPercentComplete = 1 << 2,
                // this item is never 'new'
                DoesnTCountTowardNewItems = 1 << 3,
                // cannot be purchased locally, only LSP can set
                LspAuthoritative = 1 << 4
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
            
            [TagStructure(Size = 0x3C)]
            public class PurchasePlayerAppearanceStruct : TagStructure
            {
                public List<PurchasePlayerAppearanceEffectModelPermutationBlock> Permutations;
                public List<PurchasePlayerAppearanceEffectNonModelPermutationBlock> ArmorEffectsPermutations;
                public List<PurchasePlayerAppearanceEffectVisorTintBlock> VisorTints;
                public List<PurchasePlayerAppearanceEffectEmblemIndexBlock> EmblemIndices;
                public List<PurchasePlayerAppearancePoseBlock> PortraitPoses;
                
                [TagStructure(Size = 0x8)]
                public class PurchasePlayerAppearanceEffectModelPermutationBlock : TagStructure
                {
                    // See player customization globals, linked from globals.globals.
                    public PlayerModelCustomizationAreaEnum ModelRegion;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // This must match one of the selections in the model customization globals (within the selected region).
                    public StringId ModelCustomizationSelectionName;
                    
                    public enum PlayerModelCustomizationAreaEnum : sbyte
                    {
                        SpartanHelmet,
                        SpartanChest,
                        EliteArmor,
                        Unused1,
                        Unused2,
                        Unused3,
                        Unused4,
                        Unused5
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class PurchasePlayerAppearanceEffectNonModelPermutationBlock : TagStructure
                {
                    // See player customization globals, linked from globals.globals.
                    public PlayerNonModelCustomizationAreaEnum NonModelRegion;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // This must match one of the selections in the model customization globals (within the selected region).
                    public StringId NonModelCustomizationSelectionName;
                    
                    public enum PlayerNonModelCustomizationAreaEnum : sbyte
                    {
                        SpartanArmourEffect,
                        EliteArmourEffect
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class PurchasePlayerAppearanceEffectVisorTintBlock : TagStructure
                {
                    public StringId VisorColorName;
                }
                
                [TagStructure(Size = 0x1)]
                public class PurchasePlayerAppearanceEffectEmblemIndexBlock : TagStructure
                {
                    public sbyte EmblemIndex;
                }
                
                [TagStructure(Size = 0x4)]
                public class PurchasePlayerAppearancePoseBlock : TagStructure
                {
                    public StringId PoseName;
                }
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CookiePurchaseExternalUnlockableBlockAppearanceDefinition : TagStructure
        {
            public List<CookiePurchaseAppearanceDefinitionReferenceBlock> PurchasableItems;
            
            [TagStructure(Size = 0x4)]
            public class CookiePurchaseAppearanceDefinitionReferenceBlock : TagStructure
            {
                public PurchaseAppearanceDefinitionReferenceStruct PurchasableItemReference;
                
                [TagStructure(Size = 0x4)]
                public class PurchaseAppearanceDefinitionReferenceStruct : TagStructure
                {
                    public short ItemReference;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
        }
        
        [TagStructure(Size = 0x138)]
        public class CookiePurchaseLoadoutDefinitionBlock : TagStructure
        {
            public StringId PurchaseId;
            public StringId DisplayName;
            public StringId DisplayDescription;
            public StringId ExitExperienceAggregateGroupName;
            public CookiePurchaseFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DisplayBitmap;
            public int SpriteIndex;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DetailDisplayBitmap;
            public int CookieCost;
            public PurchasePrerequisitesUnifiedDefinitionBlock VisiblePrerequisites;
            public PurchasePrerequisitesUnifiedDefinitionBlock PurchasablePrerequisites;
            public StringId IconStringId;
            public PurchasePlayerLoadoutStruct PurchasePlayerLoadout;
            
            [Flags]
            public enum CookiePurchaseFlags : byte
            {
                // this item is automatically given to the player when they meet the conditions
                AutoBuy = 1 << 0,
                // this item will appear as 'worn' when the player isn't wearing any item (for 'default' items)
                Default = 1 << 1,
                // this item will NOT count toward an overall completion value
                DoesnTCountTowardPercentComplete = 1 << 2,
                // this item is never 'new'
                DoesnTCountTowardNewItems = 1 << 3,
                // cannot be purchased locally, only LSP can set
                LspAuthoritative = 1 << 4
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
            
            [TagStructure(Size = 0x30)]
            public class PurchasePlayerLoadoutStruct : TagStructure
            {
                public List<PurchasePlayerItemBlock> Items;
                public List<PurchasePlayerAppBlock> Apps;
                public List<PurchasePlayerLoadoutSlotBlock> LoadoutSlots;
                public List<PurchasePlayerAppModSlotBlock> AppAndModSlots;
                
                [TagStructure(Size = 0xC)]
                public class PurchasePlayerItemBlock : TagStructure
                {
                    // From the global multiplayer object list.
                    public PlayerItemCategoryEnum ItemCategory;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // This must match one of the global objects.
                    public StringId ObjectName;
                    // e.g. used for weapon skins
                    public byte ObjectVariantIndex;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding1;
                    
                    public enum PlayerItemCategoryEnum : sbyte
                    {
                        Weapon,
                        Grenade,
                        Equipment,
                        Vehicle
                    }
                }
                
                [TagStructure(Size = 0x4)]
                public class PurchasePlayerAppBlock : TagStructure
                {
                    // This must match an item in the custom app globals.
                    public StringId ObjectName;
                }
                
                [TagStructure(Size = 0x1)]
                public class PurchasePlayerLoadoutSlotBlock : TagStructure
                {
                    // Total slots you're allowed.
                    public byte SlotCount;
                }
                
                [TagStructure(Size = 0x2)]
                public class PurchasePlayerAppModSlotBlock : TagStructure
                {
                    // Total apps you're allowed (from 0 to 2).
                    public byte AppCount;
                    // Total mods you're allowed (from 0 to 1).
                    public byte ModCount;
                }
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CookiePurchaseExternalUnlockableBlockLoadoutDefinition : TagStructure
        {
            public List<CookiePurchaseLoadoutDefinitionReferenceBlock> PurchasableItems;
            
            [TagStructure(Size = 0x4)]
            public class CookiePurchaseLoadoutDefinitionReferenceBlock : TagStructure
            {
                public PurchaseLoadoutDefinitionReferenceStruct PurchasableItemReference;
                
                [TagStructure(Size = 0x4)]
                public class PurchaseLoadoutDefinitionReferenceStruct : TagStructure
                {
                    public short ItemReference;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
        }
        
        [TagStructure(Size = 0x11C)]
        public class CookiePurchaseOrdnanceDefinitionBlock : TagStructure
        {
            public StringId PurchaseId;
            public StringId DisplayName;
            public StringId DisplayDescription;
            public StringId ExitExperienceAggregateGroupName;
            public CookiePurchaseFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DisplayBitmap;
            public int SpriteIndex;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag DetailDisplayBitmap;
            public int CookieCost;
            public PurchasePrerequisitesUnifiedDefinitionBlock VisiblePrerequisites;
            public PurchasePrerequisitesUnifiedDefinitionBlock PurchasablePrerequisites;
            public PurchasePlayerOrdnanceStruct PurchasePlayerOrdnance;
            
            [Flags]
            public enum CookiePurchaseFlags : byte
            {
                // this item is automatically given to the player when they meet the conditions
                AutoBuy = 1 << 0,
                // this item will appear as 'worn' when the player isn't wearing any item (for 'default' items)
                Default = 1 << 1,
                // this item will NOT count toward an overall completion value
                DoesnTCountTowardPercentComplete = 1 << 2,
                // this item is never 'new'
                DoesnTCountTowardNewItems = 1 << 3,
                // cannot be purchased locally, only LSP can set
                LspAuthoritative = 1 << 4
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
            public class PurchasePlayerOrdnanceStruct : TagStructure
            {
                public List<PurchasePlayerOrdnanceItemBlock> OrdnanceItems;
                public List<PurchasePlayerOrdnanceSlotBlock> OrdnanceSlots;
                
                [TagStructure(Size = 0x4)]
                public class PurchasePlayerOrdnanceItemBlock : TagStructure
                {
                    // This must match one of the global ordnance objects.
                    public StringId OrdnanceName;
                }
                
                [TagStructure(Size = 0x1)]
                public class PurchasePlayerOrdnanceSlotBlock : TagStructure
                {
                    // Total slots you're allowed.
                    public byte SlotCount;
                }
            }
        }
        
        [TagStructure(Size = 0xC)]
        public class CookiePurchaseExternalUnlockableBlockOrdnanceDefinition : TagStructure
        {
            public List<CookiePurchaseOrdnanceDefinitionReferenceBlock> PurchasableItems;
            
            [TagStructure(Size = 0x4)]
            public class CookiePurchaseOrdnanceDefinitionReferenceBlock : TagStructure
            {
                public PurchaseOrdnanceDefinitionReferenceStruct PurchasableItemReference;
                
                [TagStructure(Size = 0x4)]
                public class PurchaseOrdnanceDefinitionReferenceStruct : TagStructure
                {
                    public short ItemReference;
                    [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                }
            }
        }
    }
}
