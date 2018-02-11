namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3 ODST.
    /// </summary>
    public class StringIdResolverHalo3ODST : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0xD4B, 0x519, 0xBA3, 0xC3E, 0xCE2, 0xC96, 0xCF7, 0xD26, 0xD3E };
        private const int SetMin = 0x519;   // Mininum index that goes in a set
        private const int SetMax = 0xFFFF; // Maximum index that goes in a set

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