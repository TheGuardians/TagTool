using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterChargeFlags : int
    {
        None = 0,
        OffhandMeleeAllowed = 1 << 0,
        BerserkWheneverCharge = 1 << 1,
        DoNotUseBerserkMode = 1 << 2,
        DoNotStowWeaponDuringBerserk = 1 << 3,
        AllowDialogueWhileBerserking = 1 << 4,
        DoNotPlayBerserkAnimation = 1 << 5,
        DoNotShootDuringCharge = 1 << 6,
        AllowLeapWithRangedWeapons = 1 << 7,
        PermanentBerserkOnceInitiated = 1 << 8
    }
}
