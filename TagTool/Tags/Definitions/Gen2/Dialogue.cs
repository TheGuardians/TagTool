using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "dialogue", Tag = "udlg", Size = 0x18)]
    public class Dialogue : TagStructure
    {
        [TagField(ValidTags = new [] { "adlg" })]
        public CachedTag GlobalDialogueInfo;
        public FlagsValue Flags;
        public List<SoundReferencesBlock> Vocalizations;
        /// <summary>
        /// 3-letter mission dialogue designator name
        /// </summary>
        public StringId MissionDialogueDesignator;
        
        [Flags]
        public enum FlagsValue : uint
        {
            Female = 1 << 0
        }
        
        [TagStructure(Size = 0x10)]
        public class SoundReferencesBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId Vocalization;
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Sound;
            
            [Flags]
            public enum FlagsValue : ushort
            {
                NewVocalization = 1 << 0
            }
        }
    }
}

