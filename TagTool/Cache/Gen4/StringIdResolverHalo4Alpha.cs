using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 4 Alpha. 
    /// Halo 4 Alpha StringId are 7-8-17 (length-set-index) format instead of (8-8-16)
    /// </summary>
    public class StringIdResolverHalo4Alpha : StringIdResolver
    {
        private static readonly int[] SetOffsets = { 0x1A69, 0x577, 0xCF1, 0xDCC, 0xE36, 0xF0F, 0xF37, 0xF3D, 0x173C, 0x18F4, 0x190D, 0x1972, 0x198E, 0x199B, 0x19C5, };
        private const int SetMin = 0x577;   // Mininum index that goes in a set
        private const int SetMax = 0x7FFFF; // Maximum index that goes in a set

        public StringIdResolverHalo4Alpha()
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