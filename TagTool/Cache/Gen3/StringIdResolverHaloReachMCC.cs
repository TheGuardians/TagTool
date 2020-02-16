namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo Reach. Halo Reach StringId are 7-8-17 (length-set-index) format instead of (8-8-16)
    /// </summary>
    public class StringIdResolverHaloReachMCC : StringIdResolver
    {
        private static readonly int[] SetOffsets = { 0x173C, 0x4C9, 0xB2E, 0xC07, 0xC71, 0xD4A, 0xD70, 0xD75, 0x1434, 0x15A4, 0x15B8, 0x161A, 0x1632, 0x1632, 0x163F, 0x1668, 0x16C9 };
        private const int SetMin = 0x4C9;   // Mininum index that goes in a set
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