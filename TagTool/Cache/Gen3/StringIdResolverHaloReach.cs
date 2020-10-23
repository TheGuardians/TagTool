using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo Reach. Halo Reach StringId are 7-8-17 (length-set-index) format instead of (8-8-16)
    /// </summary>
    public class StringIdResolverHaloReach : StringIdResolver
    {
        private static readonly int[] SetOffsets = { 0x16AA, 0x4AE, 0xB13, 0xBEB, 0xC55, 0xD2E, 0xD54, 0xD59, 0x1416, 0x1585, 0x1599, 0x15FB, 0x1613, 0x1620, 0x1649 };
        private const int SetMin = 0x4AE;   // Mininum index that goes in a set
        private const int SetMax = 0x1FFFF; // Maximum index that goes in a set

        public StringIdResolverHaloReach()
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