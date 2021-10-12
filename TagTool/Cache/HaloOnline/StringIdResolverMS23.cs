using System;
using TagTool.Common;

namespace TagTool.Cache
{
    /// <summary>
    /// StringID resolver for 1.106708.
    /// </summary>
    public class StringIdResolverMS23 : StringIdResolver
    {
        // These values were figured out through trial-and-error
        private static readonly int[] SetOffsets = { 0x90F, 0x1, 0x685, 0x720, 0x7C4, 0x778, 0x7D4, 0x8EA, 0x902 };
        private const int SetMin = 0x1;   // Mininum index that goes in a set
        private const int SetMax = 0xF1E; // Maximum index that goes in a set

        public StringIdResolverMS23()
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

        public bool IsExtendedStringId(StringId stringId)
        {
            return (stringId.Value >> 31) == 1;
        }

        public override int GetIndex(StringId stringId)
        {
            if (IsExtendedStringId(stringId))
                return (int)(stringId.Value & 0x7FFFFFFF);
            else
                return (int)(stringId.Value & 0xFFFF);
        }

        public override int GetSet(StringId stringId)
        {
            if (IsExtendedStringId(stringId))
                return 0;
            else
                return (int)(stringId.Value >> 16) & 0xF;
        }

        public override int GetLength(StringId stringId)
        {
            throw new NotImplementedException();
        }

        public override StringId MakeStringId(int length, int set, int index)
        {
            if(index > ushort.MaxValue)
            {
                if (set != 0)
                    throw new ArgumentOutOfRangeException(nameof(set));
                if (index < 0 || index >= (1 << 30))
                    throw new ArgumentOutOfRangeException(nameof(index));
                
                return new StringId((uint)((1u << 31) | index));
            }
            else
            {
                if (set < 0 || set >= 256)
                    throw new ArgumentOutOfRangeException(nameof(set));
                if (index < 0 || index >= (1 << 16))
                    throw new ArgumentOutOfRangeException(nameof(index));

                return new StringId((uint)((set << 16) | index));
            }
        }
    }
}
