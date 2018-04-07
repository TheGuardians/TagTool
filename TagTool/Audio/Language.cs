using System.Collections.Generic;
using TagTool.Common;
using TagTool.Serialization;


namespace TagTool.Audio
{
    [TagStructure(Size = 0x1C)]
    public class LanguageBlock
    {
        public GameLanguage Language;
        public List<PermutationDurationBlock> PermutationDurations;
        public List<PitchRangeDurationBlock> PitchRangeDurations;

        [TagStructure(Size = 0x2)]
        public class PermutationDurationBlock
        {
            public short FrameCount;
        }

        [TagStructure(Size = 0x4)]
        public class PitchRangeDurationBlock
        {
            public short PermutationStartIndex;
            public short PermutationCount;
        }
    }
}