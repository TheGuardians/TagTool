using System;
using System.Collections.Generic;
using System.Linq;
using BlamCore.Cache;

namespace TagTool.Commands
{
    static class TagPrinter
    {
        public static void PrintTagShort(CachedTagInstance tag)
        {
            Console.WriteLine("{0} {1:X8} [Offset = 0x{2:X}, Size = 0x{3:X}]", tag.Group.Tag, tag.Index, tag.HeaderOffset, tag.TotalSize);
        }

        public static void PrintTagsShort(IEnumerable<CachedTagInstance> tags)
        {
            var sorted = tags.ToArray();
            Array.Sort(sorted, CompareTags);
            foreach (var tag in sorted)
                PrintTagShort(tag);
        }

        private static int CompareTags(CachedTagInstance lhs, CachedTagInstance rhs)
        {
            var classCompare = lhs.Group.Tag.CompareTo(rhs.Group.Tag);
            if (classCompare != 0)
                return classCompare;
            return lhs.Index.CompareTo(rhs.Index);
        }
    }
}
