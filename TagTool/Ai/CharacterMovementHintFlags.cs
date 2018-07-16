using System;

namespace TagTool.Ai
{
    [Flags]
    public enum CharacterMovementHintFlags : int
    {
        None = 0,

        VaultStep = 1 << 0,
        VaultCrouch = 1 << 1,

        MountStep = 1 << 5,
        MountCrouch = 1 << 6,
        MountStand = 1 << 7,

        HoistCrouch = 1 << 11,
        HoistStand = 1 << 12
    }
}