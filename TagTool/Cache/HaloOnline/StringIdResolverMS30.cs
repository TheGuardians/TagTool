namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for 12.1.700123.
    /// </summary>
    public class StringIdResolverMS30 : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0x918, 0x1, 0x685, 0x720, 0x7C4, 0x778, 0x7D0, 0x8F3, 0x90B };
        private const int SetMin = 0x1;   // Mininum index that goes in a set
        private const int SetMax = 0xF27; // Maximum index that goes in a set

        public StringIdResolverMS30()
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
