namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo Reach. Halo Reach StringId are 7-8-17 (length-set-index) format instead of (8-8-16)
    /// </summary>
    public class StringIdResolverHaloReachMCC : StringIdResolver
    {
        private static readonly int[] SetOffsets = { 0x174F, 0x4CA, 0xB36, 0xC0F, 0xC7B, 0xD54, 0xD7C, 0xD81, 0x1441, 0x15B3, 0x15CA, 0x162C, 0x162C, 0x1644, 0x1651, 0x167A, 0x16DC };
        private const int SetMin = 0x4CA;   // Mininum index that goes in a set
        private const int SetMax = 0x1FFFF; // Maximum index that goes in a set

        public StringIdResolverHaloReachMCC()
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