using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "commendation_aggregator_list", Tag = "coag", Size = 0xC)]
    public class CommendationAggregatorList : TagStructure
    {
        public List<Commendationaggregator> Lists;
        
        [TagStructure(Size = 0x28)]
        public class Commendationaggregator : TagStructure
        {
            public StringId Name;
            public StringId DescriptionText;
            public List<CommendationrewardBlock> Rewards;
            public sbyte SequenceIndex;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short MedalSpriteIndex;
            public short GameTypeSpriteIndex;
            public List<CommendationaggregatorDependentStruct> Prerequisites;
            
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
            
            [TagStructure(Size = 0x8)]
            public class CommendationaggregatorDependentStruct : TagStructure
            {
                public AggregatordependentTypeEnum Type;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId Name;
                
                public enum AggregatordependentTypeEnum : sbyte
                {
                    Commendation,
                    Aggregator
                }
            }
        }
    }
}
