using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterVitalityFlags : int
    {
        None = 0,
        AutoResurrect = 1 << 0
    }
}
