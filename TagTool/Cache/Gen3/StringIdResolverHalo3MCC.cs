using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3.
    /// </summary>
    public class StringIdResolverHalo3MCC : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0xCD4, 0x4BB, 0xAC1, 0xB53, 0xBA9, 0xBBD, 0xC05, 0xC3E, 0xC52, 0xC5F, 0xC60 };
        private const int SetMin = 0x4BB;   // Mininum index that goes in a set
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
