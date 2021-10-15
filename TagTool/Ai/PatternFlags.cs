using System;

namespace TagTool.Ai
{
    /// <summary>
    /// Bitwise flags for <see cref="VocalizationPattern"/>.
    /// </summary>
    [Flags]
    public enum PatternFlags : ushort
    {
        None = 0,
        SubjectVisible = 1 << 0,
        CauseVisible = 1 << 1,
        FriendsPresent = 1 << 2,
        SubjectIsSpeakerSTarget = 1 << 3,
        CauseIsSpeakerSTarget = 1 << 4,
        CauseIsPlayer = 1 << 5,
        CauseIsPrimaryPlayerAlly = 1 << 6,
        CauseIsInfantry = 1 << 7,
        SubjectIsInfantry = 1 << 8,
        SpeakerIsDowned = 1 << 9,
        SpeakerIsInfantry = 1 << 10,
        SpeakerHasLowHealth = 1 << 11,
        CauseIsTargetingPlayer = 1 << 12,
        SubjectDead = 1 << 13,
        CauseDead = 1 << 14
    }

    [Flags]
    public enum PatternFlagsH3 : ushort
    {
        SubjectVisible = 1 << 0,
        CauseVisible = 1 << 1,
        FriendsPresent = 1 << 2,
        SubjectIsSpeakersTarget = 1 << 3,
        CauseIsSpeakersTarget = 1 << 4,
        CauseIsPlayerOrSpeakerIsPlayerAlly = 1 << 5,
        CauseIsPrimaryPlayerAlly = 1 << 6,
        CauseIsInfantry = 1 << 7,
        SubjectIsInfantry = 1 << 8,
        SpeakerIsInfantry = 1 << 9
    }
}
