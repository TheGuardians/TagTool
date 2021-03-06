using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3 ODST.
    /// </summary>
    public class StringIdResolverHalo3ODSTMCC : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0xF0B, 0x55A, 0xBEE, 0xC89, 0xCE1, 0xDD8, 0xE24, 0xE73, 0xE8B, 0xE98 };
        private const int SetMin = 0x55A;   // Mininum index that goes in a set
        private const int SetMax = 0xFFFF; // Maximum index that goes in a set

        public StringIdResolverHalo3ODSTMCC()
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