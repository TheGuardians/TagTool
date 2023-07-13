using TagTool.Common;
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x28, Platform = Cache.CachePlatform.Original)]
    [TagStructure(Size = 0x24, Platform = Cache.CachePlatform.MCC)]
    public class CharacterPreSearchProperties : TagStructure
	{
        public CharacterPreSearchFlags Flags;
        public Bounds<float> MinimumPreSearchTime; // If the min presearch time expires and the target is (actually) outside the min-certainty radius, presearch turns off (seconds)
        public Bounds<float> MaximumPreSearchTime; // Presearch turns off after the given time (seconds)
        public float MinimumCertaintyRadius;
        [TagField(Platform = Cache.CachePlatform.Original)]
        public float DEPRECATED;
        public Bounds<float> MinimumSuppressingTime; // if the min suppressing time expires and the target is outside the min-certainty radius, suppressing fire turns off
        public short MaxSuppressCount; // the maximum number of guys allowed to be suppressing at one time

        [TagField(Length = 2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
    }
}
