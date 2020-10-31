using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3 Beta.
    /// </summary>
    public class StringIdResolverHalo3Beta : StringIdResolver
    {
        private static readonly int[] SetOffsets = { 0x962, 0x4CD, 0x85C, 0x8B4, 0x911, 0x8DA, 0x921, 0x94E};
        private const int SetMin = 0x4CD;   // Mininum index that goes in a set
        private const int SetMax = 0xFFFF; // Maximum index that goes in a set

        public StringIdResolverHalo3Beta()
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
