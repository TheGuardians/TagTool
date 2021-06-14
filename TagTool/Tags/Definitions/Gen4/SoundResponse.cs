using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_response", Tag = "sgrp", Size = 0xC)]
    public class SoundResponse : TagStructure
    {
        public List<SoundResponseDataBlockStruct> Responses;
        
        [TagStructure(Size = 0x38)]
        public class SoundResponseDataBlockStruct : TagStructure
        {
            public StringId Name;
            public StringId Channel;
            public StringId Category;
            public int CategoryPriority;
            public float QueueTimeout;
            public float ConsiderationTime;
            public float GapAfterSound;
            public SoundResponseQueueFlags QueueBehavior;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<SoundResponsePermutationBlock> PotentialResponses;
            public List<SoundCombineResponseBlock> Combiners;
            
            [Flags]
            public enum SoundResponseQueueFlags : byte
            {
                BypassQueue = 1 << 0,
                ClearQueue = 1 << 1,
                StopActiveSound = 1 << 2,
                DoNotTrack = 1 << 3
            }
            
            [TagStructure(Size = 0x10)]
            public class SoundResponsePermutationBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag Sound;
            }
            
            [TagStructure(Size = 0x18)]
            public class SoundCombineResponseBlock : TagStructure
            {
                public StringId Name;
                public SoundCombineResponseFlags ResponseBehavior;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "sgrp" })]
                public CachedTag Response;
                
                [Flags]
                public enum SoundCombineResponseFlags : byte
                {
                    CancelQueued = 1 << 0,
                    CancelActive = 1 << 1,
                    CancelNew = 1 << 2
                }
            }
        }
    }
}
