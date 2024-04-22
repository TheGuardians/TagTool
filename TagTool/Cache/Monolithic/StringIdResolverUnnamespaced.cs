using System;

namespace TagTool.Cache.Monolithic
{
    public class StringIdResolverUnnamespaced : StringIdResolver
    {
        public StringIdResolverUnnamespaced()
        {
            LengthBits = 0;
            SetBits = 0;
            IndexBits = 31;
        }

        public override int GetMinSetStringIndex()
        {
            return 0;
        }

        public override int GetMaxSetStringIndex()
        {
            return 0;
        }

        public override int[] GetSetOffsets()
        {
            return Array.Empty<int>();
        }
    }
}
