using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "InfinityUIImages", Tag = "iuii", Size = 0xC)]
    public class InfinityUiimages : TagStructure
    {
        public List<InfinityMissionSeasonImagesDefinition> Seasons;
        
        [TagStructure(Size = 0x20)]
        public class InfinityMissionSeasonImagesDefinition : TagStructure
        {
            public int SeasonNumber;
            [TagField(ValidTags = new [] { "bitm" })]
            // displayed where the missions would be, when the epilogue is selected
            public CachedTag EpilogueImage;
            public List<InfinityMissionImagesDefinition> SeasonImages;
            
            [TagStructure(Size = 0x40)]
            public class InfinityMissionImagesDefinition : TagStructure
            {
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag CardImage;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag DetailImage;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag LobbyImage;
                [TagField(ValidTags = new [] { "bitm" })]
                public CachedTag MatchImage;
            }
        }
    }
}
