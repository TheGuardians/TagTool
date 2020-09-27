using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "item_collection", Tag = "itmc", Size = 0x10)]
    public class ItemCollection : TagStructure
    {
        public List<ItemPermutationDefinition> ItemPermutations;
        public short SpawnTimeInSeconds0Default;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        
        [TagStructure(Size = 0x18)]
        public class ItemPermutationDefinition : TagStructure
        {
            public float Weight; // relatively how likely this item will be chosen
            public CachedTag Item; // which item to 
            public StringId VariantName;
        }
    }
}

