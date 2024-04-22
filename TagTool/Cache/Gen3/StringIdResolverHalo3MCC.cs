using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3.
    /// </summary>
    public class StringIdResolverHalo3MCC : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0xD4F, 0x4C4, 0xB3E, 0xBD0, 0xC26, 0xC36, 0xC82, 0xCBB, 0xCCF, 0xCDC };
        private const int SetMin = 0x4C4;   // Mininum index that goes in a set
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
