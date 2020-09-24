using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for Halo 3.
    /// </summary>
    public class StringIdResolverHalo2 : StringIdResolver
    {
        public StringIdResolverHalo2()
        {
            LengthBits = 8;
            SetBits = 8;
            IndexBits = 16;
        }

        public override int GetMinSetStringIndex()
        {
            return -1;
        }

        public override int GetMaxSetStringIndex()
        {
            return -1;
        }

        public override int[] GetSetOffsets()
        {
            return null;
        }
    }
}
