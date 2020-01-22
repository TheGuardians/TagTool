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

        //
        // Screen 1
        //
        public CachedTag DummyStrings1;
        public CachedTag TerminalStrings1;
        public short ScreenUnknown1_1;
        public short ScreenUnknown1_2;
        public short ScreenUnknown1_3;
        public short ScreenUnknown1_4;
        public CachedTag ErrorStrings1;

        //
        // Screen 2
        //
        public CachedTag DummyStrings2;
        public CachedTag TerminalStrings2;
        public short ScreenUnknown2_1;
        public short ScreenUnknown2_2;
        public short ScreenUnknown2_3;
        public short ScreenUnknown2_4;
        public CachedTag ErrorStrings2;

        //
        // Screen 3
        //
        public CachedTag DummyStrings3;
        public CachedTag TerminalStrings3;
        public short ScreenUnknown3_1;
        public short ScreenUnknown3_2;
        public short ScreenUnknown3_3;
        public short ScreenUnknown3_4;
        public CachedTag ErrorStrings3;

        //
        // Screen 4
        //
        public CachedTag DummyStrings4;
        public CachedTag TerminalStrings4;
        public short ScreenUnknown4_1;
        public short ScreenUnknown4_2;
        public short ScreenUnknown4_3;
        public short ScreenUnknown4_4;
        public CachedTag ErrorStrings4;

    }
}