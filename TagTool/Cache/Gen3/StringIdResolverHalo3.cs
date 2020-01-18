using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3.
    /// </summary>
    public class StringIdResolverHalo3 : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0xC11, 0x4B7, 0xA7D, 0xB0F, 0xBAF, 0xB63, 0xBBF, 0xBF0, 0xC04 };
        private const int SetMin = 0x4B7;   // Mininum index that goes in a set
        private const int SetMax = 0xFFFF; // Maximum index that goes in a set (Not in Halo 3?)

        public StringIdResolverHalo3()
        {
            LengthBits = 8;
            SetBits = 8;
            IndexBits = 16;
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
