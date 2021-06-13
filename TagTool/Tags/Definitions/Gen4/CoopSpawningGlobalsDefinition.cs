using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "coop_spawning_globals_definition", Tag = "coop", Size = 0x38)]
    public class CoopSpawningGlobalsDefinition : TagStructure
    {
        public short PlayerCooldownTimerSeconds;
        public short BackfieldCooldownTimerSeconds;
        public short UnsafeSpawnTimer;
        public short TeammateDamageTimer;
        public short DeadBodySwitchTime;
        public CoopSpawningFlags Flags;
        // maximal time that the loadout menu can prevent respawn while up
        public short LoadoutMenuSpawnSuppressionTime;
        // cooldown time after loadout menu is dismissed before player spawns in
        public short LoadoutMenuCooldownTime;
        // time before players initially spawn in for choosing starting loadout
        public short LoadoutMenuInitialChoiceTime;
        // maximal time that a player can continue to delay his spawn by switching targets
        public short MaximumSpawnSuppressionTime;
        public float NearbyEnemyCylinderHeight;
        public float NearbyEnemyCylinderRadius;
        // safe and ready to spawn - this value currently ignored
        public SafetyCheckModeEnum Ready;
        // safe but waiting to spawn - this value currently ignored
        public SafetyCheckModeEnum Waiting;
        // spawn target is inside enemy territory volumes
        public SafetyCheckModeEnum EnemyTerritory;
        // spawn target has recently taken damage
        public SafetyCheckModeEnum TeammateDamaged;
        // There is an enemy within the cylinder around the spawn target
        public SafetyCheckModeEnum EnemyNearby;
        // There are dangerous projectiles in the area
        public SafetyCheckModeEnum Projectiles;
        // number of seconds before influence spawning will be used (<= 0 is disabled)
        public float FailoverToInfluenceSpawningTime;
        
        [Flags]
        public enum CoopSpawningFlags : ushort
        {
            // allow loadout menu to delay spawn
            PushToSpawnEnabled = 1 << 0,
            ShowMarkerOnRespawnPlayer = 1 << 1,
            ExterminationSyncsTimers = 1 << 2,
            ExterminationSyncsLocation = 1 << 3,
            RotationAllowedOnPlayer = 1 << 4,
            RotationAllowedOnObject = 1 << 5,
            DisplayLoadoutsIfChanged = 1 << 6,
            AlwaysDisplayLoadoutsOnDeath = 1 << 7,
            DisableSpartanRespawnOnPlayer = 1 << 8,
            DisableSpartanRespawnOnBackfield = 1 << 9,
            DisableEliteRespawnOnPlayer = 1 << 10,
            DisableEliteRespawnOnBackfield = 1 << 11
        }
        
        public enum SafetyCheckModeEnum : int
        {
            HasNoEffect,
            DisplaysWarning,
            PreventsSpawn
        }
    }
}
