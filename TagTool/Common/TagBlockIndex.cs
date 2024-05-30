using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Tags;

namespace TagTool.Common
{
    [TagStructure(Size = 0x2)]
    public class TagBlockIndex : TagStructure
    {
        public ushort Offset { get => GetOffset(); set => SetOffset(value); }
        public ushort Count { get => GetCount(); set => SetCount(value); }

        private ushort GetCount() => (ushort)(Integer >> 10);
        private ushort GetOffset() => (ushort)(Integer & 0x3FFu);
        private void SetCount(ushort count)
        {
            if (count > 0x3Fu) throw new System.Exception("Out of range");
            var a = GetOffset();
            var b = (count & 0x3F) << 10;
            var value = (ushort)(a | b);
            Integer = value;
        }
        private void SetOffset(ushort _offset)
        {
            if (_offset > 0x3FFu) throw new System.Exception("Out of range");
            var a = (_offset & 0x3FF);
            var b = (GetCount() & 0x3F) << 10;
            var value = (ushort)(a | b);
            Integer = value;
        }

        public ushort Integer;

        public static implicit operator TagBlockIndex(TagBlockIndexGen2 x)
        {
            return new TagBlockIndex { Offset = x.Offset, Count = x.Count };
        }
    }

    [TagStructure(Size = 0x2)]
    public class TagBlockIndexGen2 : TagStructure
    {
        public ushort Offset { get => GetOffset(); set => SetOffset(value); }
        public ushort Count { get => GetCount(); set => SetCount(value); }

        private ushort GetCount() => (ushort)(Integer >> 9);
        private ushort GetOffset() => (ushort)(Integer & 0x1FFu);
        private void SetCount(ushort count)
        {
            if (count > 0x7Fu) throw new System.Exception("Out of range");
            var a = GetOffset();
            var b = (count & 0x7F) << 9;
            var value = (ushort)(a | b);
            Integer = value;
        }
        private void SetOffset(ushort _offset)
        {
            if (_offset > 0x1FFu) throw new System.Exception("Out of range");
            var a = (_offset & 0x1FF);
            var b = (GetCount() & 0x7F) << 9;
            var value = (ushort)(a | b);
            Integer = value;
        }

        public ushort Integer;

        public static implicit operator TagBlockIndexGen2(TagBlockIndex x)
        {
            return new TagBlockIndexGen2 { Offset = x.Offset, Count = x.Count };
        }
    }

    // temp hack
    [TagStructure(Size = 0x2)]
    public class TagBlockIndexGen2Block : TagStructure
    {
        public TagBlockIndexGen2 BlockIndex;
    }
}
