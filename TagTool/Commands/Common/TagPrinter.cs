using TagTool.Cache;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TagTool.Commands.Common
{
    static class TagPrinter
    {
        public static void PrintTagShort(CachedTag tag)
        {
            Console.WriteLine("{0} {1:X8} [Offset = 0x{2:X}]", tag.Group.Tag, tag.Index, tag.DefinitionOffset);
        }

        public static void PrintTagsShort(IEnumerable<CachedTag> tags)
        {
            var sorted = tags.ToArray();
            Array.Sort(sorted, CompareTags);
            foreach (var tag in sorted)
                PrintTagShort(tag);
        }

        private static int CompareTags(CachedTag lhs, CachedTag rhs)
        {
            var classCompare = lhs.Group.Tag.CompareTo(rhs.Group.Tag);
            if (classCompare != 0)
                return classCompare;
            return lhs.Index.CompareTo(rhs.Index);
        }
    }
}
