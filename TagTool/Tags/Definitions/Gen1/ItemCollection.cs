using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "item_collection", Tag = "itmc", Size = 0x5C)]
    public class ItemCollection : TagStructure
    {
        public List<ItemPermutation> ItemPermutations;
        public short SpawnTimeInSeconds0Default;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        [TagField(Length = 0x4C)]
        public byte[] Padding1;
        
        [TagStructure(Size = 0x54)]
        public class ItemPermutation : TagStructure
        {
            [TagField(Length = 0x20)]
            public byte[] Padding;
            /// <summary>
            /// relatively how likely this item will be chosen
            /// </summary>
            public float Weight;
            /// <summary>
            /// which item to
            /// </summary>
            [TagField(ValidTags = new [] { "item" })]
            public CachedTag Item;
            [TagField(Length = 0x20)]
            public byte[] Padding1;
        }
    }
}

