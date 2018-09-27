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
        public CachedTagInstance CollisionSound;
        public List<TagReferenceBlock> PredictedBitmaps;
        public CachedTagInstance DetonationDamageEffect;
        public float DetonationDelayMin;
        public float DetonationDelayMax;
        public CachedTagInstance DetonatingEffect;
        public CachedTagInstance DetonationEffect;
        public float CampaignGroundScale;
        public float MultiplayerGroundScale;
        public float SmallHoldScale;
        public float SmallHolsterScale;
        public float MediumHoldScale;
        public float MediumHolsterScale;
        public float LargeHoldScale;
        public float LargeHolsterScale;
        public float HugeHoldScale;
        public float HugeHolsterScale;
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