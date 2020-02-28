using TagTool.Cache;
using TagTool.Common;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "device_terminal", Tag = "term", Size = 0x140, MinVersion = CacheVersion.Halo3Retail)]
    public class Terminal : Device
    {
        public uint Unknown1;
        public StringId ActionString;
        public short TerminalIndex;
        public short Unknown2;
        public float Unknown3;
        public CachedTag ActivationSound;
        public CachedTag DeactivationSound;
        public CachedTag TranslateSound1;
        public CachedTag TranslateSound2;
        public CachedTag ErrorSound;

        [TagField(Length = 4)]
        public TerminalScreen[] TerminalScreens = new TerminalScreen[4];

        [TagStructure(Size = 0x38)]
        public class TerminalScreen : TagStructure
        {
            public CachedTag DummyStrings;
            public CachedTag TerminalStrings;
            public short ScreenUnknown1;
            public short ScreenUnknown2;
            public short ScreenUnknown3;
            public short ScreenUnknown4;
            public CachedTag ErrorStrings;
        }
    }
}