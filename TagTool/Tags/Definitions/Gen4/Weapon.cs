using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x3C8)]
    public class Weapon : Item
    {
        public WeaponDefinitionFlags WeaponFlags;
        public WeaponDefinitionSecondaryFlags WeaponSecondaryFlags;
        public StringId UnusedLabel;
        public SecondaryTriggerModes SecondaryTriggerMode;
        // if the second trigger loads alternate ammunition, this is the maximum number of shots that can be loaded at a time
        public short MaximumAlternateShotsLoaded;
        // how long after being readied it takes this weapon to switch its 'turned_on' attachment to 1.0
        public float TurnOnTime;
        // activated when this weapon is charging
        [TagField(ValidTags = new [] { "vmdx" })]
        public CachedTag VisionMode;
        public float VisionCooldownTime; // seconds
        public float ReadyTime; // seconds
        [TagField(ValidTags = new [] { "sndo","effe" })]
        public CachedTag ReadyEffect;
        [TagField(ValidTags = new [] { "jpt!","drdf" })]
        public CachedTag ReadyDamageEffect;
        // the heat value a weapon must return to before leaving the overheated state, once it has become overheated in the
        // first place
        public float HeatRecoveryThreshold; // [0,1]
        // the heat value over which a weapon first becomes overheated (should be greater than the heat recovery threshold)
        public float OverheatedThreshold; // [0,1]
        // the heat value above which the weapon has a chance of exploding each time it is fired
        public float HeatDetonationThreshold; // [0,1]
        // the percent chance (between 0.0 and 1.0) the weapon will explode when fired over the heat detonation threshold
        public float HeatDetonationFraction; // [0,1]
        // the amount of heat lost each second when the weapon is not being fired
        public float HeatLossPerSecond; // [0,1]
        // function values sets the current heat loss per second
        public StringId HeatLoss;
        // function value sets the heat loss per second while weapon is being vented
        public StringId HeatLossVenting;
        public float HeatVentingTime; // seconds
        // heat at which to begin the venting exit animations so that the weapon is just about fully cooled when the exit
        // animation completes.
        public float HeatVentingExitHeat;
        // the amount of illumination given off when the weapon is overheated
        public float HeatIllumination; // [0,1]
        // the amount of heat at which a warning will be displayed on the hud
        public float HeatWarningThreshold;
        // the amount of heat lost each second when the weapon is not being fired
        public float OverheatedHeatLossPerSecond; // [0,1]
        // function values sets the heat loss per second when weapon is overheated
        public StringId OverheatedHeatLoss;
        [TagField(ValidTags = new [] { "sndo","effe" })]
        public CachedTag Overheated;
        [TagField(ValidTags = new [] { "jpt!","drdf" })]
        public CachedTag OverheatedDamageEffect;
        [TagField(ValidTags = new [] { "sndo","effe" })]
        public CachedTag Detonation;
        [TagField(ValidTags = new [] { "jpt!","drdf" })]
        public CachedTag WeaponDetonationDamageEffect;
        public List<MeleeDamageParametersBlock> MeleeDamageParameters;
        // effect that is played in the air between two players that clang with this weapon
        [TagField(ValidTags = new [] { "sndo","effe" })]
        public CachedTag ClangEffect;
        public GlobalDamageReportingEnum MeleeDamageReportingType;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // the number of magnification levels this weapon allows
        public short MagnificationLevels;
        public Bounds<float> MagnificationRange;
        // how often 'zoom effect' will be triggered (while zoomed)
        public sbyte ZoomEffectTicks;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        // effect that is played while zoomed
        [TagField(ValidTags = new [] { "sndo","effe" })]
        public CachedTag ZoomEffect;
        public AimAssistStruct WeaponAimAssist;
        public WeaponBarrelIronSightsStruct IronSightsModifiers;
        // First mode is normal, second mode is iron sights/scoped
        public List<GlobalaimAssistBlock> AimAssistModes;
        public List<GlobalTargetTrackingParametersBlock> TargetTracking;
        public List<GlobalaimSwimBlock> AimSwim;
        // At the min range (or closer), the minimum ballistic arcing is used, at the max (or farther away), the maximum
        // arcing is used
        public Bounds<float> BallisticArcingFiringBounds; // world units
        // Controls speed and degree of arc. 0 = low, fast, 1 = high, slow
        public Bounds<float> BallisticArcingFractionBounds; // [0-1]
        public MovementPenaltyModes MovementPenalized;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        // percent slowdown to forward movement for units carrying this weapon
        public float ForwardMovementPenalty;
        // percent slowdown to sideways and backward movement for units carrying this weapon
        public float SidewaysMovementPenalty;
        // percent slowdown to forward movement for units carrying this weapon with the gunner armor mod active
        public float GunnerArmorModForwardMovementPenalty;
        // percent slowdown to sideways and backward movement for units carrying this weapon with the gunner armor mod active
        public float GunnerArmorModSidewaysMovementPenalty;
        // This will cap the speed at which the player can aim when holding this weapon, probably want to set something
        // sensible for turrets etc
        public float MaximumPitchRate; // degrees per second
        // Ammopack armormod - 0.2 would give 20% additional capacity to an energy weapon
        public float AmmopackPowerCapacity;
        public float AiScariness;
        public float WeaponPowerOnTime; // seconds
        public float WeaponPowerOffTime; // seconds
        [TagField(ValidTags = new [] { "sndo","effe" })]
        public CachedTag WeaponPowerOnEffect;
        [TagField(ValidTags = new [] { "sndo","effe" })]
        public CachedTag WeaponPowerOffEffect;
        // how much the weapon's heat recovery is penalized as it ages
        public float AgeHeatRecoveryPenalty;
        // how much the weapon's rate of fire is penalized as it ages
        public float AgeRateOfFirePenalty;
        // the age threshold when the weapon begins to misfire
        public float AgeMisfireStart; // [0,1]
        // at age 1.0, the misfire chance per shot
        public float AgeMisfireChance; // [0,1]
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag PickupSound;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag ZoomInSound;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag ZoomOutSound;
        // how much to decrease active camo when a round is fired
        public float ActiveCamoDing;
        // the node that get's attached to the unit's hand
        public StringId HandleNode;
        public StringId WeaponClass;
        public StringId WeaponName;
        public FirstTimePickupTypes FirstTimePickupType;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public WeaponTypes WeaponType;
        public short LowAmmoThreshold;
        public WeaponInterfaceStruct PlayerInterface;
        public List<GNullBlock> WeaponPredictedResources;
        public List<Magazines> Magazines1;
        public List<WeaponTriggers> NewTriggers;
        public List<WeaponBarrels> Barrels;
        public List<WeaponscaleshotStruct> ScaleshotParameters;
        public float RuntimeWeaponPowerOnVelocity;
        public float RuntimeWeaponPowerOffVelocity;
        public float MaxMovementAcceleration;
        public float MaxMovementVelocity;
        public float MaxTurningAcceleration;
        public float MaxTurningVelocity;
        [TagField(ValidTags = new [] { "vehi" })]
        public CachedTag DeployedVehicle;
        [TagField(ValidTags = new [] { "weap" })]
        public CachedTag TossedWeapon;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag AgeEffect;
        [TagField(ValidTags = new [] { "weap" })]
        public CachedTag AgedWeapon;
        [TagField(ValidTags = new [] { "foot" })]
        public CachedTag AgedMaterialEffects;
        // aging applied for 'weapon ages when damage is inflicted' or 'weapon ages with each kill' flags
        public float PerKillOrHitAgingAmount;
        public float ExternalAgingAmount;
        public float CampaignExternalAgingAmount;
        public float ExternalHeatAmount;
        // the amount of age the weapon recovers per second
        public float AgeRecoveredPerSecond; // [0,1]
        // the sound or effect played when the weapon's age reaches fully recovered
        [TagField(ValidTags = new [] { "sndo","effe" })]
        public CachedTag AgeFullyRecovered;
        public RealVector3d FirstPersonWeaponOffset;
        public RealVector2d FirstPersonScopeSize;
        // range in degrees. 0 is straight, -90 is down, 90 is up
        public Bounds<float> SupportThirdPersonCameraRange; // degrees
        // seconds
        public float WeaponZoomTime;
        // seconds
        public float WeaponReadyForUseTime;
        // e.g. - 2.0 makes playspeed twice as fast
        public float WeaponReadyFirstPersonAnimationPlaybackScale;
        // begins when tethered projectile is LNKED
        public float TetherTransitionToLinkedTime; // seconds
        // begins when tethered projectile becomes non-LNKED
        public float TetherTransitionFromLinkedTime; // seconds
        public StringId UnitStowAnchorName;
        public List<WeaponScreenEffectBlock> ScreenEffects;
        // High quality player sound bank to be prefetched. Can be empty.
        [TagField(ValidTags = new [] { "sbnk" })]
        public CachedTag PlayerSoundBank;
        // Name of the EQ shareset in WWise to apply when this weapon is active
        public uint PlayerEqPresetName;
        public int PlayerEqPresetHash;
        // Note - this is a direct event string - not a .sound
        public StringId ReloadCancelEvent;
        public int ReloadCancelEventHash;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag SingleShotFireForAutomatics;
        [TagField(ValidTags = new [] { "lsnd" })]
        public CachedTag FiringLoopForAutomatics;
        public List<WeaponSoundRtpcblock> SoundRtpcs;
        public List<WeaponSoundSweetenerBlock> SoundSweeteners;
        public StringId LockingReticleScreenName;
        public StringId LockedOnReticleScreenName;
        
        [Flags]
        public enum WeaponDefinitionFlags : uint
        {
            VerticalHeatDisplay = 1 << 0,
            MutuallyExclusiveTriggers = 1 << 1,
            AttacksAutomaticallyOnBump = 1 << 2,
            MustBeReadied = 1 << 3,
            DoesnTCountTowardMaximum = 1 << 4,
            AimAssistsOnlyWhenZoomed = 1 << 5,
            PreventsGrenadeThrowing = 1 << 6,
            MustBePickedUp = 1 << 7,
            HoldsTriggersWhenDropped = 1 << 8,
            PreventsMeleeAttack = 1 << 9,
            DetonatesWhenDropped = 1 << 10,
            CannotFireAtMaximumAge = 1 << 11,
            SecondaryTriggerOverridesGrenades = 1 << 12,
            SupportWeapon = 1 << 13,
            // for scoped weapons
            HideFpWeaponWhenInIronSights = 1 << 14,
            AisUseWeaponMeleeDamage = 1 << 15,
            AllowsBinoculars = 1 << 16,
            LoopFpFiringAnimation = 1 << 17,
            PreventsCrouching = 1 << 18,
            CannotFireWhileBoosting = 1 << 19,
            UseEmptyMeleeOnEmpty = 1 << 20,
            UsesThirdPersonCamera = 1 << 21,
            CanBeDualWielded = 1 << 22,
            CanOnlyBeDualWielded = 1 << 23,
            MeleeOnly = 1 << 24,
            CantFireIfParentDead = 1 << 25,
            // see 'weapon ages when damage is inflicted', 'per kill or hit aging amount'
            WeaponAgesWithEachKill = 1 << 26,
            AllowsUnaimedLunge = 1 << 27,
            CannotBeUsedByPlayer = 1 << 28,
            HoldFpFiringAnimation = 1 << 29,
            // deviation angle is allowed to be less than primary autoaim angle - for the rocket launcher
            StrictDeviationAngle = 1 << 30,
            // aiming this weapon at another player can be important to them - for lock-on weapons
            NotifiesTargetUnits = 1u << 31
        }
        
        [Flags]
        public enum WeaponDefinitionSecondaryFlags : uint
        {
            MagnetizesOnlyWhenZoomed = 1 << 0,
            ForceEnableEquipmentTossing = 1 << 1,
            // melee-physics dash is disabled on melees that are not lunges
            NonLungeMeleeDashDisabled = 1 << 2,
            DonTDropOnDualWieldMelee = 1 << 3,
            // when checked, this weapon
            // -is deleted when dropped
            // -does not count against maximum inventory
            // -gets deleted on host migrations
            // -prevents swapping or switching weapons
            IsEquipmentSpecialWeapon = 1 << 4,
            UsesGhostReticle = 1 << 5,
            NeverOverheats = 1 << 6,
            // setting this forces effects tracers to come from weapon barrel instead of eye point
            ForceTracersToComeFromWeaponBarrel = 1 << 7,
            CannotFireDuringEmp = 1 << 8,
            WeaponCanHeadshot = 1 << 9,
            // setting this will remove tracking data from AI-fired projectiles
            AiCannotFireTrackingProjectiles = 1 << 10,
            // bishop beam support- primary barrel fires if foe is targeted, secondary if targeting friend
            SecondBarrelFiresIfFriendIsTargeted = 1 << 11,
            // taking damage while wielding this will ping player out of zoom/iron sights
            WeaponUnzoomsOnDamage = 1 << 12,
            // will counteract default support weapon behavior to drop on equipment activation
            DoNotDropOnEquipmentActivation = 1 << 13,
            // used for CTF magnum, weapon can not be dropped or swapped out
            WeaponCanNotBeDropped = 1 << 14,
            DisableFunctionOverlaysDuringReload = 1 << 15,
            // Throws weapon when grenade trigger is pressed
            ThrowWeaponInsteadOfGrenade = 1 << 16,
            // Allows 'must be readied' weapons to have a primary attack
            DoNotDropMustBeReadiedOnPrimaryTrigger = 1 << 17,
            DeleteOnDrop = 1 << 18,
            // Default behavior prevents melee attacks when using device.  Setting this bit allows them.
            AllowMeleeWhenUsingDevice = 1 << 19,
            // Default behavior lowers weapon when using device.  Setting this leaves the weapon up.
            DoNotLowerWeaponWhenUsingDevice = 1 << 20,
            CannotFireWhileZooming = 1 << 21,
            // see 'weapon ages with each kill', 'per kill or hit aging amount'
            WeaponAgesWhenDamageIsInflicted = 1 << 22,
            // Apply weapon's gunner armor mod movement penalty multipliers rather than base multipliers when gunner armor mod is
            // active in _trait_weapons_gunner_armor_modifier
            ApplyGunnerArmorModAbilites = 1 << 23,
            // see momentum globals 'disable soft ping check'
            WieldersSprintIsUnaffectedBySoftPing = 1 << 24,
            // Adds velocity to weapon's default drop (useful for weapons with auto-pickup)
            WeaponDropsFurtherAway = 1 << 25,
            UseAutomaticFiringLoopingSounds = 1 << 26,
            // Keeps the weapon in your hand while being assassinated or performing an assassination (for must_be_readied or
            // support weapons)
            DoNotDropOnAssassination = 1 << 27,
            // Keeps the weapon from dropping or being hidden at the start of assassinations, and must instead be dropped using
            // the drop weapon keyframe
            IsPartOfBody = 1 << 28,
            // Disallows any equipment usage while holding this weapon regardless of any other traits you may have
            ForceDenyEquipmentUse = 1 << 29,
            // Used for Oddball that only shows the pickup prompt when megalo script sets pickup priority to special
            HidePickupPromptUnlessSpecialPickupPriority = 1 << 30,
            // Player will always pickup this weapon if requested, Used for CTF that needs to force storm_magnum_ctf onto the
            // player
            WeaponIgnoresPlayerPickupAllowedTrait = 1u << 31
        }
        
        public enum SecondaryTriggerModes : short
        {
            Normal,
            SlavedToPrimary,
            InhibitsPrimary,
            LoadsAlterateAmmunition,
            LoadsMultiplePrimaryAmmunition
        }
        
        public enum GlobalDamageReportingEnum : sbyte
        {
            Unknown,
            TehGuardians,
            Scripting,
            AiSuicide,
            ForerunnerSmg,
            SpreadGun,
            ForerunnerRifle,
            ForerunnerSniper,
            BishopBeam,
            BoltPistol,
            PulseGrenade,
            IncinerationLauncher,
            MagnumPistol,
            AssaultRifle,
            MarksmanRifle,
            Shotgun,
            BattleRifle,
            SniperRifle,
            RocketLauncher,
            SpartanLaser,
            FragGrenade,
            StickyGrenadeLauncher,
            LightMachineGun,
            RailGun,
            PlasmaPistol,
            Needler,
            GravityHammer,
            EnergySword,
            PlasmaGrenade,
            Carbine,
            BeamRifle,
            AssaultCarbine,
            ConcussionRifle,
            FuelRodCannon,
            Ghost,
            RevenantDriver,
            RevenantGunner,
            Wraith,
            WraithAntiInfantry,
            Banshee,
            BansheeBomb,
            Seraph,
            RevenantDeuxDriver,
            RevenantDeuxGunner,
            LichDriver,
            LichGunner,
            Mongoose,
            WarthogDriver,
            WarthogGunner,
            WarthogGunnerGauss,
            WarthogGunnerRocket,
            Scorpion,
            ScorpionGunner,
            FalconDriver,
            FalconGunner,
            WaspDriver,
            WaspGunner,
            WaspGunnerHeavy,
            MechMelee,
            MechChaingun,
            MechCannon,
            MechRocket,
            Broadsword,
            BroadswordMissile,
            TortoiseDriver,
            TortoiseGunner,
            MacCannon,
            TargetDesignator,
            OrdnanceDropPod,
            OrbitalCruiseMissile,
            PortableShield,
            PersonalAutoTurret,
            ThrusterPack,
            FallingDamage,
            GenericCollisionDamage,
            GenericMeleeDamage,
            GenericExplosion,
            FireDamage,
            BirthdayPartyExplosion,
            FlagMeleeDamage,
            BombMeleeDamage,
            BombExplosionDamage,
            BallMeleeDamage,
            Teleporter,
            TransferDamage,
            ArmorLockCrush,
            HumanTurret,
            PlasmaCannon,
            PlasmaMortar,
            PlasmaTurret,
            ShadeTurret,
            ForerunnerTurret,
            Tank,
            Chopper,
            Hornet,
            Mantis,
            MagnumPistolCtf,
            FloodProngs
        }
        
        public enum MovementPenaltyModes : short
        {
            Always,
            WhenZoomed,
            WhenZoomedOrReloading
        }
        
        public enum FirstTimePickupTypes : sbyte
        {
            Unassigned,
            BoltPistol,
            LightRifle,
            Suppressor,
            BinaryRifle,
            Scattershot,
            IncinerationCannon
        }
        
        public enum WeaponTypes : short
        {
            Undefined,
            Shotgun,
            Needler,
            PlasmaPistol,
            PlasmaRifle,
            RocketLauncher,
            EnergyBlade,
            Splaser,
            Shield,
            ScarabGun,
            WolverineQuad,
            FlakCannon,
            PlasmaLauncher,
            LaserDesignator,
            StickyDetonator
        }
        
        [TagStructure(Size = 0xC8)]
        public class MeleeDamageParametersBlock : TagStructure
        {
            public RealEulerAngles2d DamagePyramidAngles;
            // 0 defaults to 0.8f
            public float DamagePyramidDepth; // wu
            // 0 defaults to 1.22f
            public float MaximumLungeRange; // wu
            // the distance out from the pyramid center to spawn explosive effects.  This value will be clamped to the damage
            // pyramid depth. 0 defaults to the damage pyramid depth
            public float DamageLungeExplosiveDepth; // wu
            public float RuntimeDamageLungeExplosiveFraction;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag MeleeDamage;
            [TagField(ValidTags = new [] { "jpt!","drdf" })]
            public CachedTag MeleeResponse;
            // this is only important for the energy sword
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag LungeMeleeDamage;
            // this is only important for the energy sword
            [TagField(ValidTags = new [] { "jpt!","drdf" })]
            public CachedTag LungeMeleeResponse;
            // this is only important for the energy sword
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag EmptyMeleeDamage;
            // this is only important for the energy sword
            [TagField(ValidTags = new [] { "jpt!","drdf" })]
            public CachedTag EmptyMeleeResponse;
            // this is only important for the energy sword
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag ClangMeleeDamage;
            // this is only important for the energy sword
            [TagField(ValidTags = new [] { "jpt!","drdf" })]
            public CachedTag ClangMeleeResponse;
            // e.g. used by AR to damage sword guy when clanging sword attack
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag ClangMeleeAgainstMeleeWeaponDamage;
            // e.g. used by AR to damage sword guy when clanging sword attack
            [TagField(ValidTags = new [] { "jpt!","drdf" })]
            public CachedTag ClangMeleeAgainstMeleeWeaponDamageResponse;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag LungeMeleeExplosiveDamage;
        }
        
        [TagStructure(Size = 0x44)]
        public class AimAssistStruct : TagStructure
        {
            // the number of seconds that the crosshair needs to be on target before the larger autoaim stick kicks in
            public float AutoaimStickTime; // seconds!
            // the maximum angle that autoaim works to 'stick' a target.  set to zero to use default behavior.
            public Angle AutoaimStickAngle; // degrees!
            // the maximum angle that autoaim works at full strength!
            public Angle AutoaimAngle; // degrees
            // the maximum distance that autoaim works at full strength!
            public float AutoaimRange; // world units
            // at what point the autoaim starts falling off!
            public float AutoaimFalloffRange; // world units
            // at what point the autoaim reaches full power!
            public float AutoaimNearFalloffRange; // world units
            // the maximum angle that magnetism works at full strength!
            public Angle MagnetismAngle; // degrees
            // the maximum distance that magnetism works at full strength!
            public float MagnetismRange; // world units
            // at what point magnetism starts falling off!
            public float MagnetismFalloffRange; // world units
            // at what point magnetism reaches full power!
            public float MagnetismNearFalloffRange; // world units
            // the maximum angle that a projectile is allowed to deviate from the gun barrel due to autoaim OR network lead vector
            // reconstruction!
            public Angle DeviationAngle; // degrees
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
        }
        
        [TagStructure(Size = 0x10)]
        public class WeaponBarrelIronSightsStruct : TagStructure
        {
            // multiplies the spread error - use < 1.0 for more accuracy
            public float SpreadMultiplier;
            // multipliers player maximum move speed
            public float MovementMultiplier;
            public float AimSpeedMultiplier;
            // tick delay before zooming out upon release of iron sights zoom -- use 0 for single-zoom weapons, 5-10 for
            // multi-zooms like sniper rifle
            public float AutoZoomOutTime;
        }
        
        [TagStructure(Size = 0x44)]
        public class GlobalaimAssistBlock : TagStructure
        {
            // the number of seconds that the crosshair needs to be on target before the larger autoaim stick kicks in
            public float AutoaimStickTime; // seconds
            // the maximum angle that autoaim works to 'stick' a target.  set to zero to use default behavior.
            public Angle AutoaimStickAngle; // degrees
            // the maximum angle that autoaim works at full strength
            public Angle AutoaimAngle; // degrees
            // the maximum distance that autoaim works at full strength
            public float AutoaimRange; // world units
            // at what point the autoaim starts falling off
            public float AutoaimFalloffRange; // world units
            // at what point the autoaim reaches full power
            public float AutoaimNearFalloffRange; // world units
            // the maximum angle that magnetism works at full strength
            public Angle MagnetismAngle; // degrees
            // the maximum distance that magnetism works at full strength
            public float MagnetismRange; // world units
            // at what point magnetism starts falling off
            public float MagnetismFalloffRange; // world units
            // at what point magnetism reaches full power
            public float MagnetismNearFalloffRange; // world units
            // the maximum angle that a projectile is allowed to deviate from the gun barrel due to autoaim OR network lead vector
            // reconstruction
            public Angle DeviationAngle; // degrees
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
        }
        
        [TagStructure(Size = 0x38)]
        public class GlobalTargetTrackingParametersBlock : TagStructure
        {
            // specify the kinds of targets this tracking system can lock on
            public List<TrackingTypeBlock> TrackingTypes;
            public float AcquireTime; // s
            public float GraceTime; // s
            public float DecayTime; // s
            [TagField(ValidTags = new [] { "scmb","sndo","lsnd","snd!" })]
            public CachedTag TrackingSound;
            [TagField(ValidTags = new [] { "scmb","sndo","lsnd","snd!" })]
            public CachedTag LockedSound;
            
            [TagStructure(Size = 0x4)]
            public class TrackingTypeBlock : TagStructure
            {
                public StringId TrackingType;
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class GlobalaimSwimBlock : TagStructure
        {
            public float XPeriod; // seconds
            public float XAmplitude;
            public float YPeriod; // seconds
            public float YAmplitude;
            public float NonMovingCrouched;
            public float NonMovingStanding;
            public float Moving;
            public float ToCrouched; // seconds
            public float ToStanding; // seconds
        }
        
        [TagStructure(Size = 0x3C)]
        public class WeaponInterfaceStruct : TagStructure
        {
            public WeaponSharedInterfaceStruct SharedInterface;
            public List<WeaponFirstPersonInterfaceBlock> FirstPerson;
            [TagField(ValidTags = new [] { "cusc" })]
            public CachedTag HudScreenReference;
            // the parent of the weapon can indicate that this hud should be used instead of the default
            [TagField(ValidTags = new [] { "cusc" })]
            public CachedTag AlternateHudScreenReference;
            
            [TagStructure(Size = 0x10)]
            public class WeaponSharedInterfaceStruct : TagStructure
            {
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x28)]
            public class WeaponFirstPersonInterfaceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "mode" })]
                public CachedTag FirstPersonModel;
                [TagField(ValidTags = new [] { "jmad" })]
                public CachedTag FirstPersonAnimations;
                // the multiplier by the standard first person FOV to use when this weapon is held
                public float FirstPersonFovScale;
                // the distance to apply depth of field to the weapon
                public float FirstPersonDofDistance;
            }
        }
        

        [TagStructure(Size = 0x84)]
        public class Magazines : TagStructure
        {
            public MagazineFlags Flags;
            public short RoundsRecharged; // per second
            public short RoundsTotalInitial;
            public short RoundsTotalMaximum;
            public short RoundsLoadedMaximum;
            public short RuntimeRoundsInventoryMaximum;
            // AmmoPack armormod - alternate total_initial value due to AmmoPack
            public short AmmopackRoundsTotalInitial;
            // AmmoPack armormod - alternate total_maximum value due to armor mod AmmoPack
            public short AmmopackRoundsTotalMaximum;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // the length of time we wait before saying the reload dialogue
            public float ReloadDialogueTime; // seconds
            public short RoundsReloaded;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            // the length of time it takes to chamber the next round
            public float ChamberTime; // seconds - NOT USED!
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(ValidTags = new [] { "sndo","effe" })]
            public CachedTag ReloadingEffect;
            [TagField(ValidTags = new [] { "jpt!","drdf" })]
            public CachedTag ReloadingDamageEffect;
            [TagField(ValidTags = new [] { "sndo","effe" })]
            public CachedTag ChamberingEffect;
            [TagField(ValidTags = new [] { "jpt!","drdf" })]
            public CachedTag ChamberingDamageEffect;
            public List<MagazineObjects> Magazines1;
            
            [Flags]
            public enum MagazineFlags : uint
            {
                WastesRoundsWhenReloaded = 1 << 0,
                EveryRoundMustBeChambered = 1 << 1,
                // will prevent reload until fire is complete (sticky det)
                MagazineCannotChangeStateWhileFiring = 1 << 2,
                AllowOverheatedReloadWhenEmpty = 1 << 3,
                BottomlessInventory = 1 << 4
            }
            
            [TagStructure(Size = 0x14)]
            public class MagazineObjects : TagStructure
            {
                public short Rounds;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "eqip" })]
                public CachedTag Equipment;
            }
        }
        
        [TagStructure(Size = 0xAC)]
        public class WeaponTriggers : TagStructure
        {
            public WeaponTriggerDefinitionFlags Flags;
            public WeaponTriggerInputs Input;
            public WeaponTriggerBehaviors Behavior;
            public short PrimaryBarrel;
            public short SecondaryBarrel;
            public TriggerPredictionTypeEnum Prediction;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public WeaponTriggerAutofireStruct Autofire;
            public WeaponTriggerChargingStruct Charging;
            // created once player is able to release the tethered projectile
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag DoubleLatchProjectileReleasableEffect;
            // created when player releases the tethered projectile
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag DoubleLatchProjectileReleasedEffect;
            
            [Flags]
            public enum WeaponTriggerDefinitionFlags : uint
            {
                AutofireSingleActionOnly = 1 << 0
            }
            
            public enum WeaponTriggerInputs : short
            {
                RightTrigger,
                LeftTrigger,
                MeleeAttack,
                AiOnlySecondary
            }
            
            public enum WeaponTriggerBehaviors : short
            {
                // fires its primary action barrel whenever the trigger is down
                Spew,
                // fires its primary action barrel when the trigger is down and then latches
                Latch,
                // fires its primary action barrel once if pulsed quickly or if not depressed all the way, otherwise does secondary
                // behavior
                LatchAutofire,
                // tethers projectiles if latched long enough, on release the tethered projectile detonates
                LatchTether,
                // charges the trigger
                Charge,
                // latched; fires its primary action barrel when unzoomed, secondary when zoomed
                LatchZoom,
                SpewCharge,
                SwordCharge,
                PaintTarget,
                // projectile is tethered by 1st latch, 2nd releases and detonates it
                DoubleLatchTether,
                // like charge, but pays attention to any magazine and will not charge unless magazine is idle and barrel is actually
                // able to fire
                ChargeWithMagazine
            }
            
            public enum TriggerPredictionTypeEnum : short
            {
                None,
                Spew,
                Charge
            }
            
            [TagStructure(Size = 0xC)]
            public class WeaponTriggerAutofireStruct : TagStructure
            {
                public float AutofireTime;
                public float AutofireThrow;
                public WeaponTriggerAutofireActions SecondaryAction;
                public WeaponTriggerAutofireActions PrimaryAction;
                
                public enum WeaponTriggerAutofireActions : short
                {
                    Fire,
                    Charge,
                    FireOther
                }
            }
            
            [TagStructure(Size = 0x70)]
            public class WeaponTriggerChargingStruct : TagStructure
            {
                // the amount of time it takes for this trigger to become fully charged
                public float ChargingTime; // seconds
                // the amount of time this trigger can be charged before becoming overcharged
                public float ChargedTime; // seconds
                public WeaponTriggerOverchargedActions OverchargedAction;
                public WeaponTriggerChargingFlags Flags;
                // 96 was the constant in code for the pp
                public short CancelledTriggerThrow;
                // the amount of illumination given off when the weapon is fully charged
                public float ChargedIllumination; // [0,1]
                // the charging effect is created once when the trigger begins to charge
                [TagField(ValidTags = new [] { "sndo","effe" })]
                public CachedTag ChargingEffect;
                // the charging effect is created once when the trigger begins to charge
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag ChargingDamageEffect;
                // plays every tick you're charging or charged, scaled to charging fraction
                [TagField(ValidTags = new [] { "drdf" })]
                public CachedTag ChargingContinuousDamageResponse;
                // how much battery to drain per second when charged
                public float ChargedDrainRate;
                // the discharging effect is created once when the trigger releases its charge
                [TagField(ValidTags = new [] { "sndo","effe" })]
                public CachedTag DischargeEffect;
                // the discharging effect is created once when the trigger releases its charge
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag DischargeDamageEffect;
                public List<WeaponTriggerChargingFireFraction> FireFractions;
                
                public enum WeaponTriggerOverchargedActions : sbyte
                {
                    None,
                    Explode,
                    Discharge
                }
                
                [Flags]
                public enum WeaponTriggerChargingFlags : byte
                {
                    // discharging a partially charged weapon will spew for the charged fraction of the spew time set below
                    CanFireFromPartialCharge = 1 << 0,
                    // if magazine present, do not fire more than current rounds loaded (mantis rocket launcher)
                    LimitToCurrentRoundsLoaded = 1 << 1,
                    // spew-charge triggers only
                    WontChargeUnlessTrackedTargetIsValid = 1 << 2
                }
                
                [TagStructure(Size = 0x4)]
                public class WeaponTriggerChargingFireFraction : TagStructure
                {
                    // charging fraction at which the weapon should additionally fire a shot.
                    public float ChargeFraction; // [0.1]
                }
            }
        }
        
        [TagStructure(Size = 0x190)]
        public class WeaponBarrels : TagStructure
        {
            public WeaponBarrelFlags Flags;
            public WeaponBarrelFiringParametersStruct Firing;
            // the magazine from which this trigger draws its ammunition
            public short Magazine;
            // the number of rounds expended to create a single firing effect
            public short RoundsPerShot;
            // the minimum number of rounds necessary to fire the weapon
            public short MinimumRoundsLoaded;
            // the number of non-tracer rounds fired between tracers
            public short RoundsBetweenTracers;
            public StringId OptionalBarrelMarkerName;
            // how loud this weapon appears to the AI
            public AiSoundVolumeEnum FiringNoise;
            public BarrelPredictionTypeEnum PredictionType;
            // Valid only for barrels set to prediction type "continuous". Controls how many projectiles per second can be
            // individually synchronized (use debug_projectiles to diagnose).
            public float EventSynchronizedProjectilesPerSecond;
            // Valid only for barrels set to prediction type "continuous". If the barrel's current error level is over this value
            // (zero to one scale), we will not consider synchronizing projectiles with individual events (use debug_projectiles
            // to diagnose).
            public float MaximumBarrelErrorForEventSynchronization;
            public WeaponBarrelFiringErrorStruct FiringError;
            public WeaponBarrelDistributionFunctions DistributionFunction;
            public short ProjectilesPerShot;
            // Custom vectors must be set in distribution function above
            public List<ProjectiledistributionCustomVector> CustomVectors;
            // used by distribution function 'horizontal fan' above
            public float DistributionAngle; // degrees
            // projectile direction is randomly selected between this and max_error_angle below
            public Angle MinimumError; // degrees
            // current barrel_error is linearly interpolated between these to generate max_error_angle
            public Bounds<Angle> ErrorAngle; // degrees (max_error_angle)
            public WeaponBarrelProjectileAccuracyPenaltyStruct AccuracyPenalties;
            public List<WeaponBarrelFirstPersonOffsetBlock> FirstPersonOffset;
            public GlobalDamageReportingEnum DamageEffectReportingType;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "proj" })]
            public CachedTag Projectile;
            [TagField(ValidTags = new [] { "proj" })]
            public CachedTag OptionalSecondaryProjectile;
            public WeaponBarrelDamageEffectStruct Eh;
            [TagField(ValidTags = new [] { "bloc" })]
            public CachedTag CrateProjectile;
            public float CrateProjectileSpeed;
            // the amount of time (in seconds) it takes for the ejection port to transition from 1.0 (open) to 0.0 (closed) after
            // a shot has been fired
            public float EjectionPortRecoveryTime;
            // the amount of time (in seconds) it takes the illumination function to transition from 1.0 (bright) to 0.0 (dark)
            // after a shot has been fired
            public float IlluminationRecoveryTime;
            // the amount of heat generated each time the barrel fires. Unlike the name suggests, this amount of heat is NOT
            // applied per projectile created.
            public float HeatGeneratedPerRound; // [0,1]
            // function value sets the amount of heat to add to the weapon each tick the barrel is firing
            public StringId HeatGeneratedPerRoundFunction;
            // the amount the weapon ages each time the trigger is fired
            public float AgeGeneratedPerRound; // [0,1]
            // the amount the weapon ages each time the trigger is fired
            public float CampaignAgeGeneratedPerRound; // [0,1]
            // the next trigger fires this often while holding down this trigger
            public float OverloadTime; // seconds
            public float RuntimeIlluminationRecoveryRate;
            public float RuntimeEjectionPortRecoveryRate;
            public float RuntimeRateOfFireAccelerationRate;
            public float RuntimeRateOfFireDecelerationRate;
            public float RuntimeErrorDecelerationRate;
            // firing effects determine what happens when this trigger is fired
            public List<BarrelFiringEffectBlock> FiringEffects;
            
            [Flags]
            public enum WeaponBarrelFlags : uint
            {
                // rather than being chosen sequentially, firing effects are picked randomly
                RandomFiringEffects = 1 << 0,
                // allows a weapon to be fired as long as there is a non-zero amount of ammunition loaded
                CanFireWithPartialAmmo = 1 << 1,
                // instead of coming out of the magic first person camera origin, the projectiles for this weapon actually come out of
                // the gun
                ProjectilesUseWeaponOrigin = 1 << 2,
                // this trigger's ejection port is started during the key frame of its chamber animation
                EjectsDuringChamber = 1 << 3,
                // projectiles fired by this weapon cannot have their direction adjusted by the AI to hit the target
                ProjectileVectorCannotBeAdjusted = 1 << 4,
                ProjectilesHaveIdenticalError = 1 << 5,
                // If there are multiple guns for this trigger, the projectiles emerge in parallel beams (rather than independant
                // aiming)
                ProjectilesFireParallel = 1 << 6,
                CantFireWhenOthersFiring = 1 << 7,
                CantFireWhenOthersRecovering = 1 << 8,
                DonTClearFireBitAfterRecovering = 1 << 9,
                StaggerFireAcrossMultipleMarkers = 1 << 10,
                CanFireAtMaximumAge = 1 << 11,
                Use1FiringEffectPerBurst = 1 << 12,
                // the barrel will not fire if all markers are pointed further away from the desired aiming vector than the aim assist
                // deviation angle
                PreventMarkerDeviation = 1 << 13,
                // disables of the barrel error inversely with the zoom magnification
                ErrorIgnoresZoom = 1 << 14,
                // projectiles shoot out the marker direction instead of the player's aim vector
                ProjectileFiresInMarkerDirection = 1 << 15,
                // Prevents projectile origin from changing when object is outside of BSP; Useful for units that can be placed inside
                // physics.
                SkipTestForObjectBeingOutsideBsp = 1 << 16,
                OnlyReloadIfAllBarrelsIdle = 1 << 17,
                // the weapon's owner cannot switch weapons unless this barrel is in the idle state
                OnlySwitchWeaponsIfBarrelIdle = 1 << 18
            }
            
            public enum AiSoundVolumeEnum : short
            {
                // ai will not respond to this sound
                Silent,
                Quiet,
                Medium,
                Shout,
                // ai can hear this sound at any range
                Loud
            }
            
            public enum BarrelPredictionTypeEnum : short
            {
                None,
                Continuous,
                Instant
            }
            
            public enum WeaponBarrelDistributionFunctions : short
            {
                Point,
                HorizontalFan,
                CustomVectors,
                CustomPositions
            }
            
            [TagStructure(Size = 0x34)]
            public class WeaponBarrelFiringParametersStruct : TagStructure
            {
                // the number of firing effects created per second
                public Bounds<float> RoundsPerSecond;
                // function value sets the current rate of fire when the barrel is firing
                public StringId RateOfFireAcceleration;
                // the continuous firing time it takes for the weapon to achieve its final rounds per second
                public float AccelerationTime; // seconds
                // function value sets the current rate of fire when the barrel is not firing
                public StringId RateOfFireDeceleration;
                // the continuous idle time it takes for the weapon to return from its final rounds per second to its initial
                public float DecelerationTime; // seconds
                // scale the barrel spin speed by this amount
                public float BarrelSpinScale;
                // a percentage between 0 and 1 which controls how soon in its firing animation the weapon blurs
                public float BlurredRateOfFire;
                // allows designer caps to the shots you can fire from one firing action
                public Bounds<short> ShotsPerFire;
                // how long after a set of shots it takes before the barrel can fire again
                public float FireRecoveryTime; // seconds
                // how much of the recovery allows shots to be queued
                public float SoftRecoveryFraction;
                // how long after a set of shots it takes before the weapon can melee
                public float MeleeFireRecoveryTime; // seconds
                // how much of the melee recovery allows melee to be queued
                public float MeleeSoftRecoveryFraction;
            }
            
            [TagStructure(Size = 0x18)]
            public class WeaponBarrelFiringErrorStruct : TagStructure
            {
                // the continuous idle time it would take for a barrel_error of 1.0 to return to its minimum value.
                // Minimum value is usually 0.0 but sprinting can override this. See
                // 'globals@Player information.momentum and sprinting.min weapon error'
                public float DecelerationTime; // seconds
                // the range of angles (in degrees) that a damaged weapon will skew fire
                public Bounds<float> DamageError;
                // yaw rate is doubled
                public Angle MinErrorLookPitchRate;
                // yaw rate is doubled
                public Angle FullErrorLookPitchRate;
                // use to soften or sharpen the rate ding
                public float LookPitchErrorPower;
            }
            
            [TagStructure(Size = 0x8)]
            public class ProjectiledistributionCustomVector : TagStructure
            {
                // x-y offset - +x is right, +y is up
                public RealPoint2d PointOffset; // [-1.1]
            }
            
            [TagStructure(Size = 0x78)]
            public class WeaponBarrelProjectileAccuracyPenaltyStruct : TagStructure
            {
                // percentage accuracy lost when reloading
                public float ReloadPenalty;
                // percentage accuracy lost when switching weapons
                public float SwitchPenalty;
                // percentage accuracy lost when zooming in/out
                public float ZoomPenalty;
                // percentage accuracy lost when jumping
                public float JumpPenalty;
                public WeaponBarrelProjectileAccuracyPenaltyFunctionStruct SingleWieldPenalties;
                public WeaponBarrelProjectileAccuracyPenaltyFunctionStruct DualWieldPenalties;
                
                [TagStructure(Size = 0x34)]
                public class WeaponBarrelProjectileAccuracyPenaltyFunctionStruct : TagStructure
                {
                    // percentage accuracy lost when the barrel has fired
                    public List<WeaponBarrelFunctionBlock> FiringPenaltyFunction;
                    // percentage accuracy lost when the barrel has fired from a crouched position
                    public List<WeaponBarrelFunctionBlock> FiringCrouchedPenaltyFunction;
                    // percentage accuracy lost when moving
                    public List<WeaponBarrelFunctionBlock> MovingPenaltyFunction;
                    // percentage accuracy lost when turning the camera
                    public List<WeaponBarrelFunctionBlock> TurningPenaltyFunction;
                    // angle which represents the maximum input to the turning penalty function.
                    public float ErrorAngleMaxRotation;
                    
                    [TagStructure(Size = 0x14)]
                    public class WeaponBarrelFunctionBlock : TagStructure
                    {
                        public ScalarFunctionNamedStruct Function;
                        
                        [TagStructure(Size = 0x14)]
                        public class ScalarFunctionNamedStruct : TagStructure
                        {
                            public MappingFunction Function;
                            
                            [TagStructure(Size = 0x14)]
                            public class MappingFunction : TagStructure
                            {
                                public byte[] Data;
                            }
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class WeaponBarrelFirstPersonOffsetBlock : TagStructure
            {
                // +x is forward, +z is up, +y is left
                public RealPoint3d FirstPersonOffset; // world units
            }
            
            [TagStructure(Size = 0x10)]
            public class WeaponBarrelDamageEffectStruct : TagStructure
            {
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag DamageEffect;
            }
            
            [TagStructure(Size = 0xF4)]
            public class BarrelFiringEffectBlock : TagStructure
            {
                // the minimum number of times this firing effect will be used, once it has been chosen
                public short ShotCountLowerBound;
                // the maximum number of times this firing effect will be used, once it has been chosen
                public short ShotCountUpperBound;
                // this effect is used when the weapon is loaded and fired normally
                [TagField(ValidTags = new [] { "sndo","effe" })]
                public CachedTag FiringEffect;
                // this effect is used when the weapon is loaded and will do critical damage
                [TagField(ValidTags = new [] { "sndo","effe" })]
                public CachedTag CriticalEffect;
                // this effect is used when the weapon is loaded but fired while overheated
                [TagField(ValidTags = new [] { "sndo","effe" })]
                public CachedTag MisfireEffect;
                // this effect is used when the weapon is not loaded
                [TagField(ValidTags = new [] { "sndo","effe" })]
                public CachedTag EmptyEffect;
                // this effect is used when the weapon is not loaded
                [TagField(ValidTags = new [] { "sndo","effe" })]
                public CachedTag OptionalSecondaryFiringEffect;
                // this effect is used when the weapon is loaded and fired normally
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag FiringDamage;
                // this effect is used when the weapon is loaded and will do critical damage
                [TagField(ValidTags = new [] { "sndo","effe" })]
                public CachedTag CriticalDamage;
                // this effect is used when the weapon is loaded but fired while overheated
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag MisfireDamage;
                // this effect is used when the weapon is not loaded
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag EmptyDamage;
                // this effect is used when the weapon is loaded and fired normally
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag OptionalSecondaryFiringDamage;
                // this effect is used when the weapon is loaded and fired normally
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag FiringRiderDamage;
                // this effect is used when the weapon is loaded and will do critical damage
                [TagField(ValidTags = new [] { "sndo","effe" })]
                public CachedTag CriticalRiderDamage;
                // this effect is used when the weapon is loaded but fired while overheated
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag MisfireRiderDamage;
                // this effect is used when the weapon is not loaded
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag EmptyRiderDamage;
                // this effect is used when the weapon is loaded and fired normally
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag OptionalSecondaryFiringRiderDamage;
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class WeaponscaleshotStruct : TagStructure
        {
            public WeaponscaleshotFlags Flags;
            // the amount of scaleshot power you get when this weapon is fired
            public float PowerPerShot;
            // the amount of scaleshot power you get when a projectile from this weapon hits an enemy
            public float PowerPerHit;
            // the change per second in power
            public float PowerChangePerSecond;
            public List<WeaponscaleshotLevelStruct> PowerLevels;
            
            [Flags]
            public enum WeaponscaleshotFlags : uint
            {
                // if true, once you hit a power level, you stay at that level or higher until your power value hits 0; otherwise your
                // power level is recomputed constantly
                LevelLatches = 1 << 0
            }
            
            [TagStructure(Size = 0x2C)]
            public class WeaponscaleshotLevelStruct : TagStructure
            {
                public WeaponscaleshotLevelFlags Flags;
                // the minimum power at which this scaleshot level kicks in
                public float MinimumPowerLevel;
                // the change per shot in power when you're at this level
                public float PowerChangePerShot;
                // the change per second in power when you're at this level
                public float PowerChangePerSecond;
                // the projectile to fire at this level
                [TagField(ValidTags = new [] { "proj" })]
                public CachedTag Projectile;
                // firing effects when firing at this level
                public List<BarrelFiringEffectBlock> FiringEffects;
                
                [Flags]
                public enum WeaponscaleshotLevelFlags : uint
                {
                    // if true, as long as the weapon is at this level, you cannot gain positive power, only negative values are applied
                    CannotGainPowerAtThisLevel = 1 << 0
                }
                
                [TagStructure(Size = 0xF4)]
                public class BarrelFiringEffectBlock : TagStructure
                {
                    // the minimum number of times this firing effect will be used, once it has been chosen
                    public short ShotCountLowerBound;
                    // the maximum number of times this firing effect will be used, once it has been chosen
                    public short ShotCountUpperBound;
                    // this effect is used when the weapon is loaded and fired normally
                    [TagField(ValidTags = new [] { "sndo","effe" })]
                    public CachedTag FiringEffect;
                    // this effect is used when the weapon is loaded and will do critical damage
                    [TagField(ValidTags = new [] { "sndo","effe" })]
                    public CachedTag CriticalEffect;
                    // this effect is used when the weapon is loaded but fired while overheated
                    [TagField(ValidTags = new [] { "sndo","effe" })]
                    public CachedTag MisfireEffect;
                    // this effect is used when the weapon is not loaded
                    [TagField(ValidTags = new [] { "sndo","effe" })]
                    public CachedTag EmptyEffect;
                    // this effect is used when the weapon is not loaded
                    [TagField(ValidTags = new [] { "sndo","effe" })]
                    public CachedTag OptionalSecondaryFiringEffect;
                    // this effect is used when the weapon is loaded and fired normally
                    [TagField(ValidTags = new [] { "jpt!","drdf" })]
                    public CachedTag FiringDamage;
                    // this effect is used when the weapon is loaded and will do critical damage
                    [TagField(ValidTags = new [] { "sndo","effe" })]
                    public CachedTag CriticalDamage;
                    // this effect is used when the weapon is loaded but fired while overheated
                    [TagField(ValidTags = new [] { "jpt!","drdf" })]
                    public CachedTag MisfireDamage;
                    // this effect is used when the weapon is not loaded
                    [TagField(ValidTags = new [] { "jpt!","drdf" })]
                    public CachedTag EmptyDamage;
                    // this effect is used when the weapon is loaded and fired normally
                    [TagField(ValidTags = new [] { "jpt!","drdf" })]
                    public CachedTag OptionalSecondaryFiringDamage;
                    // this effect is used when the weapon is loaded and fired normally
                    [TagField(ValidTags = new [] { "jpt!","drdf" })]
                    public CachedTag FiringRiderDamage;
                    // this effect is used when the weapon is loaded and will do critical damage
                    [TagField(ValidTags = new [] { "sndo","effe" })]
                    public CachedTag CriticalRiderDamage;
                    // this effect is used when the weapon is loaded but fired while overheated
                    [TagField(ValidTags = new [] { "jpt!","drdf" })]
                    public CachedTag MisfireRiderDamage;
                    // this effect is used when the weapon is not loaded
                    [TagField(ValidTags = new [] { "jpt!","drdf" })]
                    public CachedTag EmptyRiderDamage;
                    // this effect is used when the weapon is loaded and fired normally
                    [TagField(ValidTags = new [] { "jpt!","drdf" })]
                    public CachedTag OptionalSecondaryFiringRiderDamage;
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class WeaponScreenEffectBlock : TagStructure
        {
            public WeaponScreenEffectFlags Flags;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "sefc" })]
            public CachedTag ScreenEffect;
            
            [Flags]
            public enum WeaponScreenEffectFlags : byte
            {
                OverridesUnitAndCameraScreenEffects = 1 << 0,
                Unzoomed = 1 << 1,
                ZoomLevel1 = 1 << 2,
                ZoomLevel2 = 1 << 3
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class WeaponSoundRtpcblock : TagStructure
        {
            // Sound attachment to affect - leave empty for main body
            public int AttachmentIndex;
            // Function to drive the RTPC
            public StringId Function;
            // WWise RTPC string name
            public uint RtpcName;
            public int RtpcNameHashValue;
        }
        
        [TagStructure(Size = 0x1C)]
        public class WeaponSoundSweetenerBlock : TagStructure
        {
            // Function to trigger the sweetener
            public StringId Function;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag Sound;
            // value of the function (between 0 and 1) where the sound is triggered
            public float SwitchPoint;
            // 0 for triggering while function is decreasing, 1 for increasing (more modes to come?)
            public int Mode;
        }
    }
}
