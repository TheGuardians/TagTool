using System;
using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "device_terminal", Tag = "term", Size = 0x140, MinVersion = CacheVersion.Halo3Retail)]
    public class Terminal : Device
    {
        [TagField(Flags = TagFieldFlags.Padding, Length = 0x4)]
        public byte[] Unused1;

        public StringId ActionString;
        public short TerminalIndex;

        [TagField(Flags = TagFieldFlags.Padding, Length = 0x2)]
        public byte[] Unused2;

        public float TerminalExposure;
        public CachedTag ActivationSound;
        public CachedTag DeactivationSound;
        public CachedTag TranslateSound1;
        public CachedTag TranslateSound2;
        public CachedTag ErrorSound;

        public TerminalScreen EasyScreen;
        public TerminalScreen NormalScreen;
        public TerminalScreen HeroicScreen;
        public TerminalScreen LegendaryScreen;

        [TagStructure(Size = 0x38)]
        public class TerminalScreen : TagStructure
        {
            public CachedTag DummyStrings;
            public CachedTag StoryStrings;
            public DialectTypeFlags DummyDialectTypes;
            public DialectTypeFlags StoryDialectTypes;
            public TerminalImage DummyBlfImage;
            public TerminalImage StoryBlfImage;
            public CachedTag ErrorStrings;

            [Flags]
            public enum DialectTypeFlags : short
            {
                None = 0,
                Military = 1 << 0,
                Public = 1 << 1,
                Private = 1 << 2,
                Librarian = 1 << 3,
                Didact = 1 << 4,
                Mendicant = 1 << 5,
                Offensive = 1 << 6,
                Gravemind = 1 << 7,
                Fleet = 1 << 8,
                Spark = 1 << 9
            }

            public enum TerminalImage : short
            {
                None = -1,
                Image1,
                Image2,
                Image3,
                Image4,
                Image5,
                Image6,
                Image7,
                Image8
            }
        }
    }
}