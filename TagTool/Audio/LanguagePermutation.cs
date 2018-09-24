using TagTool.Cache;
using TagTool.Serialization;

namespace TagTool.Audio
{
    [TagStructure(Size = 0x94, MinVersion = CacheVersion.HaloReach)]
    public class LanguagePermutation : TagStructure
	{
        public int ParentPermutationIndex;

        public Permutation English;
        public Permutation Japanese;
        public Permutation German;
        public Permutation French;
        public Permutation Spanish;
        public Permutation LatinAmericanSpanish;
        public Permutation Italian;
        public Permutation Korean;
        public Permutation ChineseTraditional;
        public Permutation ChineseSimplified;
        public Permutation Portuguese;
        public Permutation Polish;

        [TagStructure(Size = 0xC)]
        public class Permutation : TagStructure
		{
            public uint SampleSize;
            public int FirstPermutationChunkIndex;
            public int PermutationChunkCount;
        }
    }
}