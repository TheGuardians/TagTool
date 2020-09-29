using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "item_collection", Tag = "itmc", Size = 0xC)]
    public class ItemCollection : TagStructure
    {
        public List<ItemPermutation> ItemPermutations;
        public short SpawnTimeInSeconds0Default;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        
        [TagStructure(Size = 0x10)]
        public class ItemPermutation : TagStructure
        {
            /// <summary>
            /// relatively how likely this item will be chosen
            /// </summary>
            public float Weight;
            /// <summary>
            /// which item to
            /// </summary>
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Item;
            public StringId VariantName;
        }
    }
}

