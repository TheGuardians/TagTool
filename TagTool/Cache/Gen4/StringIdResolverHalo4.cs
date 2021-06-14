using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 4. 
    /// Halo 4 StringId are 5-8-19 (length-set-index) format instead of (8-8-16)
    /// </summary>
    public class StringIdResolverHalo4 : StringIdResolver
    {
        private static readonly int[] SetOffsets = { 0x20BD, 0x631, 0xEA6, 0xFB1, 0x103E, 0x1136, 0x117D, 0x11A8, 0x1C95, 0x1EA1, 0x1ED4, 0x1ED4, 0x1F57, 0x1F8B, 0x1F98, 0x1FDA };
        private const int SetMin = 0x631;   // Mininum index that goes in a set
        private const int SetMax = 0x7FFFF; // Maximum index that goes in a set

        public StringIdResolverHalo4()
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