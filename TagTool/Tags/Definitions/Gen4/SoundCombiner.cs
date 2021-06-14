using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_combiner", Tag = "scmb", Size = 0x18)]
    public class SoundCombiner : TagStructure
    {
        public List<SoundCombinerDefinitionEntryBlock> Entries;
        // this is determined at post process time
        public float MaximumPlaySeconds;
        // this is determined at post process time
        public float MinimumDistanceDefault;
        // this is determined at post process time
        public float MaximumDistanceDefault;
        
        [TagStructure(Size = 0x18)]
        public class SoundCombinerDefinitionEntryBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sndo","snd!" })]
            public CachedTag Sound;
            // delay before this sound is played
            public Bounds<float> DelayBounds; // seconds
        }
    }
}
