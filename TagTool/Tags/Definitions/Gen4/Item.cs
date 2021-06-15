using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "item", Tag = "item", Size = 0xCC)]
    public class Item : GameObject
    {
        public ItemDefinitionFlags Flags;
        public short OldMessageIndex;
        public short SortOrder;
        public float OldMultiplayerOnGroundScale;
        public float OldCampaignOnGroundScale;
        public StringId PickupMessage;
        public StringId SwapMessage;
        public StringId PickupMessage1;
        public StringId SwapMessage1;
        public StringId PickedUpMsg;
        public StringId SwitchToMsg;
        public StringId SwitchToFromAiMsg;
        public StringId NotifyEmptyMsg;
        public StringId NotifyOverheatedMessage;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag CollisionSound;
        public List<PredictedBitmapsBlock> PredictedBitmaps;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag DetonationDamageEffect;
        public Bounds<float> DetonationDelay; // seconds
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DetonatingEffect;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DetonationEffect;
        public float GroundScale;
        public float SmallUnit;
        public float SmallUnit1;
        public float MediumUnit;
        public float MediumUnit1;
        public float LargeUnit;
        public float LargeUnit1;
        public float HugeUnit;
        public float HugeUnit1;
        // If not present, the default from global.globals is used.
        [TagField(ValidTags = new [] { "grfr" })]
        public CachedTag GroundedFrictionSettings;
        // Used to override the object tossed when item owner is killed.  Is overridden by tossed weapon override.
        [TagField(ValidTags = new [] { "obje" })]
        public CachedTag TossedOverride;
        
        [Flags]
        public enum ItemDefinitionFlags : uint
        {
            AlwaysMaintainsZUp = 1 << 0,
            // like jackal shield
            BlocksHeadshots = 1 << 1,
            UseGroundScaleForAllUnspecifiedAttachments = 1 << 2,
            FixupPositionUponDetachingFromParent = 1 << 3,
            FixupPositionAfterAttachingToParent = 1 << 4
        }
        
        [TagStructure(Size = 0x10)]
        public class PredictedBitmapsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
        }
    }
}
