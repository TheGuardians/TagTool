using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo2AMP. 
    /// Halo 4 StringId are 5-8-19 (length-set-index) format instead of (8-8-16)
    /// </summary>
    public class StringIdResolverHalo2AMP : StringIdResolver
    {
        private static readonly int[] SetOffsets = { 0x210C, 0x633, 0xEC9, 0xFD4, 0x1061, 0x1159, 0x11A4, 0x11CF, 0x1CD7, 0x1EE7, 0x1F1B, 0x1F9E, 0x1FD2, 0x1FDF, 0x2021 };
        private const int SetMin = 0x633;   // Mininum index that goes in a set
        private const int SetMax = 0x7FFFF; // Maximum index that goes in a set

        public StringIdResolverHalo2AMP()
        {
            LengthBits = 5;
            SetBits = 8;
            IndexBits = 19;
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
