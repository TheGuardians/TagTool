using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "item", Tag = "item", Size = 0xB4, MinVersion = CacheVersion.Halo3Retail)]
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
        public CachedTag CollisionSound;
        public List<TagReferenceBlock> PredictedBitmaps;
        public CachedTag DetonationDamageEffect;
        public float DetonationDelayMin;
        public float DetonationDelayMax;
        public CachedTag DetonatingEffect;
        public CachedTag DetonationEffect;
        public float CampaignGroundScale;
        public float MultiplayerGroundScale;
        public float SmallUnitHoldScale;
        public float SmallUnitHolsterScale;
        public float MediumUnitHoldScale;
        public float MediumUnitHolsterScale;
        public float LargeUnitHoldScale;
        public float LargeUnitHolsterScale;
        public float HugeUnitHoldScale;
        public float HugeUnitHolsterScale;
        public float GroundedFrictionLength;
        public float GroundedFrictionUnknown;

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