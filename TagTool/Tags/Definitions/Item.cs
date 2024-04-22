using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "item", Tag = "item", Size = 0xB4, MinVersion = CacheVersion.Halo3Retail, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "item", Tag = "item", Size = 0xBC, MinVersion = CacheVersion.HaloReach)]
    public class Item : GameObject
    {
        public ItemFlagBits ItemFlags;

        public ObsoleteItemFieldStruct ObsoleteItemFields;

        public StringId PickupMessage;
        public StringId SwapMessage;
        public StringId PickupOrDualWieldMessage;
        public StringId SwapOrDualWieldMessage;
        public StringId PickedUpMessage;
        public StringId SwitchToMessage;
        public StringId SwitchToFromAiMessage;
        public StringId AllWeaponsEmptyMessage;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public StringId NotifyOverheatedMessage;

        [TagField(ValidTags = new[] { "snd!", "scmb" })]
        public CachedTag CollisionSound;
        public List<TagReferenceBlock> PredictedBitmaps;

        [TagField(ValidTags = new[] { "jpt!" })]
        public CachedTag DetonationDamageEffect;
        public Bounds<float> DetonationDelay;
        [TagField(ValidTags = new[] { "effe" })]
        public CachedTag DetonatingEffect;
        [TagField(ValidTags = new[] { "effe" })]
        public CachedTag DetonationEffect;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float CampaignOnGroundScale; // reach has unified ground scale for sp+mp
        public float MultiplayerOnGroundScale;

        public ItemUnitScalesStruct ItemUnitScales;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public GroundedDampingStruct GroundedDamping;

        // If not present, the default from global.globals is used.
        [TagField(ValidTags = new[] { "grfr" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag GroundedFrictionSettings;


        [Flags]
        public enum ItemFlagBits : uint
        {
            None,
            AlwaysMaintainsZUp = 1 << 0,
            DestroyedByExplosions = 1 << 1,
            UnaffectedByGravity = 1 << 2,
            CrateStyleCollisionFilter = 1 << 3,
            CanBePickedUpInVehicle = 1 << 4,
            Bit5 = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7
        }

        [TagStructure(Size = 0xC)]
        public class ObsoleteItemFieldStruct : TagStructure
        {
            public short OldMessageIndex;
            public short SortOrder;
            public float OldMultiplayerOnGroundScale;
            public float OldCampaignOnGroundScale;
        }

        [TagStructure(Size = 0x20)]
        public class ItemUnitScalesStruct : TagStructure
        {
            public UnitScaleStruct SmallUnit;
            public UnitScaleStruct MediumUnit;
            public UnitScaleStruct LargeUnit;
            public UnitScaleStruct HugeUnit;
        }

        [TagStructure(Size = 0x8)]
        public class UnitScaleStruct : TagStructure
        {
            public float Armed;    // being held by unit
            public float Stowed;   // holstered
        }

        [TagStructure(Size = 0x8)]
        public class GroundedDampingStruct : TagStructure
        {
            public float Angular; // ~30 == complete damping, 0 == defaults
            public float Linear; // ~30 == complete damping, 0 == defaults
        }
    }
}