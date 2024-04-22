using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "virtual_keyboard", Tag = "vcky", Size = 0x3C)]
    public class VirtualKeyboard : TagStructure
    {
        [TagField(ValidTags = new [] { "font" })]
        public CachedTag DisplayFont;
        [TagField(ValidTags = new [] { "bitm" })]
        public CachedTag BackgroundBitmap;
        [TagField(ValidTags = new [] { "ustr" })]
        public CachedTag SpecialKeyLabelsStringList;
        public List<VirtualKeyBlock> VirtualKeys;
        
        [TagStructure(Size = 0x50)]
        public class VirtualKeyBlock : TagStructure
        {
            public KeyboardKeyValue KeyboardKey;
            /// <summary>
            /// enter unicode character values as integer numbers
            /// </summary>
            public short LowercaseCharacter;
            public short ShiftCharacter;
            public short CapsCharacter;
            public short SymbolsCharacter;
            public short ShiftCapsCharacter;
            public short ShiftSymbolsCharacter;
            public short CapsSymbolsCharacter;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag UnselectedBackgroundBitmap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag SelectedBackgroundBitmap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag ActiveBackgroundBitmap;
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag StickyBackgroundBitmap;
            
            public enum KeyboardKeyValue : short
            {
                _1,
                _2,
                _3,
                _4,
                _5,
                _6,
                _7,
                _8,
                _9,
                _0,
                A,
                B,
                C,
                D,
                E,
                F,
                G,
                H,
                I,
                J,
                K,
                L,
                M,
                N,
                O,
                P,
                Q,
                R,
                S,
                T,
                U,
                V,
                W,
                X,
                Y,
                Z,
                Done,
                Shift,
                CapsLock,
                Symbols,
                Backspace,
                Left,
                Right,
                Space
            }
        }
    }
}

