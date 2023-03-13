using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 4 PreRelease. 
    /// Halo 4 StringId are 5-8-19 (length-set-index) format instead of (8-8-16)
    /// </summary>
    public class StringIdResolverHalo4PreRelease : StringIdResolver
    {
        private static readonly int[] SetOffsets = { 0x206B, 0x627, 0xE91, 0xF97, 0x1021, 0x1117, 0x115C, 0x1189, 0x1C4F, 0x1E59, 0x1E8C, 0x1F08, 0x1F3C, 0x1F49, 0x1F8B };
        private const int SetMin = 0x627;   // Mininum index that goes in a set
        private const int SetMax = 0x7FFFF; // Maximum index that goes in a set

        public StringIdResolverHalo4PreRelease()
        {
            LengthBits = 5;
            SetBits = 8;
            IndexBits = 19;
        }

        public override int GetMinSetStringIndex()
        {
            return SetMin;
        }

        public override int GetMaxSetStringIndex()
        {
            return SetMax;
        }

        public override int[] GetSetOffsets()
        {
            return SetOffsets;
        }
    }
}
