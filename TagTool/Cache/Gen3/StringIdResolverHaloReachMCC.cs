namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo Reach. Halo Reach StringId are 7-8-17 (length-set-index) format instead of (8-8-16)
    /// </summary>
    public class StringIdResolverHaloReachMCC : StringIdResolver
    {
        private static readonly int[] SetOffsets = { 0x1746, 0x4CA, 0xB2F, 0xC08, 0xC72, 0xD4B, 0xD73, 0xD78, 0x1438, 0x15AA, 0x15C1, 0x1623, 0x1623, 0x163B, 0x1648, 0x1671, 0x16D3 };
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