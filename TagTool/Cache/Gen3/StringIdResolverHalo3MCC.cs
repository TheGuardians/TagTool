using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3.
    /// </summary>
    public class StringIdResolverHalo3MCC : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0xCDC, 0x4C3, 0xACA, 0xB5C, 0xBB2, 0xBC2, 0xC0E, 0xC47, 0xC5B, 0xC68, 0xC69 };
        private const int SetMin = 0x4C3;   // Mininum index that goes in a set
        private const int SetMax = 0xFFFF; // Maximum index that goes in a set (Not in Halo 3?)

        public StringIdResolverHalo3MCC()
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
