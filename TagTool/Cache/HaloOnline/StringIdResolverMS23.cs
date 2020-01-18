namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for 1.106708.
    /// </summary>
    public class StringIdResolverMS23 : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0x90F, 0x1, 0x685, 0x720, 0x7C4, 0x778, 0x7D4, 0x8EA, 0x902 };
        private const int SetMin = 0x1;   // Mininum index that goes in a set
        private const int SetMax = 0xF1E; // Maximum index that goes in a set

        public StringIdResolverMS23()
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
