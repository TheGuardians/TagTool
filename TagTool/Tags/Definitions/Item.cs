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
        public short OldMessageIndex;
        public short SortOrder;
        public float OldMultiplayerOnGroundScale;
        public float OldCampaignOnGroundScale;
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
        public CachedTag CollisionSound;
        public List<TagReferenceBlock> PredictedBitmaps;
        public CachedTag DetonationDamageEffect;
        public float DetonationDelayMin;
        public float DetonationDelayMax;
        public CachedTag DetonatingEffect;
        public CachedTag DetonationEffect;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float MultiplayerOnGroundScale;
        public float CampaignOnGroundScale;
        public float SmallUnitHoldScale;
        public float SmallUnitHolsterScale;
        public float MediumUnitHoldScale;
        public float MediumUnitHolsterScale;
        public float LargeUnitHoldScale;
        public float LargeUnitHolsterScale;
        public float HugeUnitHoldScale;
        public float HugeUnitHolsterScale;

        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float GroundedFrictionLength;
        [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
        public float GroundedFrictionUnknown;
        // If not present, the default from global.globals is used.
        [TagField(ValidTags = new[] { "grfr" }, MinVersion = CacheVersion.HaloReach)]
        public CachedTag GroundedFrictionSettings;


        [Flags]
        public enum ItemFlagBits : int
        {
            None,
            AlwaysMaintainsZUp = 1 << 0,
            DestroyedByExplosions = 1 << 1,
            UnaffectedByGravity = 1 << 2
        }
    }
}