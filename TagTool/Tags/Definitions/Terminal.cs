using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;

namespace TagTool.Tags.Definitions
{
    [TagStructure(Name = "device_terminal", Tag = "term", Size = 0x140, MinVersion = CacheVersion.Halo3Retail)]
    public class Terminal : Device
    {
        public uint Unknown;
        public StringId ActionString;
        public short TerminalIndex;
        public short Unknown2;
        public float Unknown3;
        public CachedTagInstance ActivationSound;
        public CachedTagInstance DeactivationSound;
        public CachedTagInstance TranslateSound1;
        public CachedTagInstance TranslateSound2;
        public CachedTagInstance ErrorSound;

        //
        // Screen 1
        //
        public CachedTagInstance DummyStrings1;
        public CachedTagInstance TerminalStrings1;
        public short ScreenUnknown1_1;
        public short ScreenUnknown1_2;
        public short ScreenUnknown1_3;
        public short ScreenUnknown1_4;
        public CachedTagInstance ErrorStrings1;

        //
        // Screen 2
        //
        public CachedTagInstance DummyStrings2;
        public CachedTagInstance TerminalStrings2;
        public short ScreenUnknown2_1;
        public short ScreenUnknown2_2;
        public short ScreenUnknown2_3;
        public short ScreenUnknown2_4;
        public CachedTagInstance ErrorStrings2;

        //
        // Screen 3
        //
        public CachedTagInstance DummyStrings3;
        public CachedTagInstance TerminalStrings3;
        public short ScreenUnknown3_1;
        public short ScreenUnknown3_2;
        public short ScreenUnknown3_3;
        public short ScreenUnknown3_4;
        public CachedTagInstance ErrorStrings3;

        //
        // Screen 4
        //
        public CachedTagInstance DummyStrings4;
        public CachedTagInstance TerminalStrings4;
        public short ScreenUnknown4_1;
        public short ScreenUnknown4_2;
        public short ScreenUnknown4_3;
        public short ScreenUnknown4_4;
        public CachedTagInstance ErrorStrings4;

    }
}