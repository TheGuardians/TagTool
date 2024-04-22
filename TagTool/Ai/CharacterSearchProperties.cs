using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x20)]
    public class CharacterSearchProperties : TagStructure
	{
        public CharacterSearchFlags Flags;
        public Bounds<float> SearchTime;
        public float SearchDistance; // Max distance from our firing positions that we can search (0 value will default to 3wu). Does not affect vehicle search distance (see maxd if you want that value too).
        public Bounds<float> UncoverDistanceBounds; // Distance of uncover point from target. Hard lower limit, soft upper limit.
        public float OrphanOffset; // (0 value will default to 1.8wu)
        public float MinimumOffset; // Minimum offset from the target point to investigate, otherwise we just use the target point itself. Not entirely sure about the justification for this one...
    }
}
