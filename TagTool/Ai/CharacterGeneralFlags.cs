using System;

namespace TagTool.Ai
{
    /// <summary>
    /// Bitwise flags for <see cref="CharacterGeneralProperties"/>.
    /// </summary>
    [Flags]
    public enum CharacterGeneralFlags : int
    {
        None = 0,
        Swarm = 1 << 0,
        Flying = 1 << 1,
        DualWields = 1 << 2,
        UsesGravemind = 1 << 3,
        DoNotTradeWeapon = 1 << 5,
        DoNotStowWeapon = 1 << 6,
        HeroCharacter = 1 << 7,
        LeaderIndependentPositioning = 1 << 8,
        HasActiveCamo = 1 << 9,
        UseHeadMarkerForLooking = 1 << 10,
        SpaceCharacter = 1 << 11,
        DoNotDropEquipment = 1 << 12,
        DoNotAllowCrouch = 1 << 13,
        DoNotAllowMovingCrouch = 1 << 14,
        CriticalBetrayal = 1 << 15,
        DeathlessCriticalBetrayal = 1 << 16,

        /// <summary>
        /// Non-depleted ai-tracked damage sections prevent instant melee kills.
        /// </summary>
        ArmorPreventsAssassination = 1 << 17,

        /// <summary>
        /// The default is to drop only the currently equipped weapon.
        /// </summary>
        DropAllWeaponsOnDeath = 1 << 18,

        /// <summary>
        /// This will override 'drop all weapons'.
        /// </summary>
        DropNoWeaponsOnDeath = 1 << 19,

        /// <summary>
        /// Cannot be assassinated unless its shield has been depleted.
        /// </summary>
        ShieldPreventsAssassination = 1 << 20,

        /// <summary>
        /// This overrides all other character assassination modifications.
        /// </summary>
        CannotBeAssassinated = 1 << 21
    }
}
