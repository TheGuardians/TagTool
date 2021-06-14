using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "sound_global_propagation", Tag = "sgp!", Size = 0x48)]
    public class SoundGlobalPropagation : TagStructure
    {
        public SoundPropagationDefinitionStruct UnderwaterPropagation;
        
        [TagStructure(Size = 0x48)]
        public class SoundPropagationDefinitionStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "snde" })]
            public CachedTag SoundEnvironment;
            [TagField(ValidTags = new [] { "lsnd" })]
            public CachedTag BackgroundSound;
            // scale for fog background sound
            public float BackgroundSoundGain; // dB
            // scales the surrounding background sound by this much
            public float EnvironmentDucking; // dB
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag EntrySound;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag ExitSound;
        }
    }
}
