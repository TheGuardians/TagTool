using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterMovementHintFlags : uint
    {
        None = 0,
        VaultStep = 1 << 0,
        VaultCrouch = 1 << 1,
        Unused0 = 1 << 2,
        Unused1 = 1 << 3,
        Unused2 = 1 << 4,
        MountStep = 1 << 5,
        MountCrouch = 1 << 6,
        MountStand = 1 << 7,
        Unused3 = 1 << 8,
        Unused4 = 1 << 9,
        Unused5 = 1 << 10,
        HoistCrouch = 1 << 11,
        HoistStand = 1 << 12,
        Unused6 = 1 << 13,
        Unused7 = 1 << 14,
        Unused8 = 1 << 15
    }
}