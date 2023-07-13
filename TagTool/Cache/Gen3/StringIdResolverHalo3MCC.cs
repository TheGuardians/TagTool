using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3.
    /// </summary>
    public class StringIdResolverHalo3MCC : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0xD24, 0x499, 0xB13, 0xBA5, 0xBFB, 0xC0B, 0xC57, 0xC90, 0xCA4, 0xCB1 };
        private const int SetMin = 0x499;   // Mininum index that goes in a set
        private const int SetMax = 0xFFFF; // Maximum index that goes in a set (Not in Halo 3?)

        public StringIdResolverHalo3MCC()
        {
            LengthBits = 7;
            SetBits = 7;
            IndexBits = 18;
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
