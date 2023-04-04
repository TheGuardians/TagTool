using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3 ODST.
    /// </summary>
    public class StringIdResolverHalo3ODSTMCC : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0xFBC, 0x55B, 0xBEF, 0xC8A, 0xCE2, 0xDD9, 0xE25, 0xE74, 0xE8C, 0xE99 };
        private const int SetMin = 0x55A;   // Mininum index that goes in a set
        private const int SetMax = 0xFFFF; // Maximum index that goes in a set

        public StringIdResolverHalo3ODSTMCC()
        {
            LengthBits = 7;
            SetBits = 8;
            IndexBits = 17;
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