using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x24)]
    public class Dialogue : TagStructure
    {
        public CachedTag GlobalDialogueInfo;
        public FlagsValue Flags;
        public List<VocalizationSound> Vocalizations;
        public StringId MissionDialogueDesignator; // 3-letter mission dialogue designator name
        
        [Flags]
        public enum FlagsValue : uint
        {
            Female = 1 << 0
        }
        
        [TagStructure(Size = 0x18)]
        public class VocalizationSound : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public StringId Vocalization;
            public CachedTag Sound;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                NewVocalization = 1 << 0
            }
        }
    }
}

