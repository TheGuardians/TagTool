using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterBoardingFlags : int
    {
        None = 0,
        AirborneBoarding = 1 << 0
    }
}
