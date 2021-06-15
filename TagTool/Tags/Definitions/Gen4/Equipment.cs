using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "equipment", Tag = "eqip", Size = 0xCC)]
    public class Equipment : Item
    {
        public float InitialEnergy;
        // after deactivation, energy stays constant for this long before starting to change by 'inactive energy rate'
        public float EnergyRecoveryTime; // seconds
        public float InactiveEnergyRate; // energy/second
        public EquipmentFlags Flags;
        // the marker on the unit to attach this equipment to when it is stowed.
        // The equipment should have a marker named "equipment_stow_anchor"
        public StringId UnitStowMarkerName;
        public EquipmentPickupBehavior PickupBehavior;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        // sets the primary change color on the unit to this if the flag above is checked
        public RealArgbColor ForcedPrimaryChangeColor;
        // sets the secondary change color on the unit to this if the flag above is checked
        public RealArgbColor ForcedSecondaryChangeColor;
        // How large a danger zone we should create around this equipment (0 means no danger zone)
        public float DangerRadius;
        // How far does my target have to be for me to throw this at them?
        public float MinDeploymentDistance; // wus
        // How long I should go unnoticed by nearby enemies
        public float AwarenessTime; // seconds
        // The equipment ability type name used by the ai dialog system used to filter equipment activation dialogue events.
        public StringId AiDialogueEquipmentType;
        public List<OptionalunitCameraBlock> OverrideCamera;
        public List<EquipmentabilityDatum> Abilities;
        public GlobalDamageReportingEnum DamageReportingType;
        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(ValidTags = new [] { "cusc" })]
        public CachedTag HudScreenReference;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag PickupSound;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag EnergyChargedEffect;
        [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
        public CachedTag UnableToActivateSound;
        // High quality player sound bank to be prefetched. Can be empty.
        [TagField(ValidTags = new [] { "sbnk" })]
        public CachedTag PlayerSoundBank;
        public List<EquipmentSoundRtpcblock> SoundRtpcs;
        public List<EquipmentSoundSweetenerBlock> SoundSweeteners;
        
        [Flags]
        public enum EquipmentFlags : uint
        {
            PathfindingObstacle = 1 << 0,
            EquipmentIsDangerousToAi = 1 << 1,
            // if an actor dies while carrying this, it gets deleted immediately
            // does not affect dropping by players
            NeverDroppedByAi = 1 << 2,
            ProtectsParentFromAoe = 1 << 3,
            _ThirdPersonCameraAlways = 1 << 4,
            UseForcedPrimaryChangeColor = 1 << 5,
            UseForcedSecondaryChangeColor = 1 << 6,
            CanNotBePickedUpByPlayer = 1 << 7,
            // used with supercombine attach
            IsRemovedFromWorldOnDeactivation = 1 << 8,
            IsDroppedByPlayer = 1 << 9,
            IsDroppedByAi = 1 << 10
        }
        
        public enum EquipmentPickupBehavior : sbyte
        {
            // picks it up automatically, if you have no equipment in your inventory, otherwise, press RB to swap
            AutomaticIfEmpty,
            // always picks it up, whether or not you have another piece of equipment
            AlwaysAutomatic,
            // press RB to pickup or swap this equipment
            Manual,
            // Never pickup, press RB to activate it in-place
            ActivateOnGround
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
        
        [TagStructure(Size = 0x78)]
        public class OptionalunitCameraBlock : TagStructure
        {
            public UnitCameraStruct UnitCamera;
            
            [TagStructure(Size = 0x78)]
            public class UnitCameraStruct : TagStructure
            {
                public UnitCameraFlags Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public StringId CameraMarkerName;
                public Angle PitchAutoLevel;
                public Bounds<Angle> PitchRange;
                public List<UnitCameraTrackBlock> CameraTracks;
                public Angle PitchMinimumSpring;
                public Angle PitchMmaximumSpring;
                public Angle SpringVelocity;
                // if non-zero, limits the change in look velocity per second while the user is pushing the look stick in the current
                // direction of looking
                public Angle LookAcceleration; // deg/s/s
                // if non-zero, limits the change in look velocity per second while the user is not pushing the look stick or changing
                // directions
                public Angle LookDeceleration; // deg/s/s
                // if non-zero, when the desired velocity change is less than this fraction of the acceleration, starts interpolating
                // the maximum acceleration towards zero.
                // You can think of this as a time in seconds where if the velocity would reach its target in this amount of time or
                // less, it will start taking longer.
                public float LookAccSmoothingFraction;
                // if non-zero, overrides the FOV set in the unit or globals
                public Angle OverrideFov;
                public CameraObstructionStruct CameraObstruction;
                public List<UnitCameraAccelerationDisplacementBlock> CameraAcceleration;
                public List<GamepadStickInfoBlock> MoveStickOverrides;
                public List<GamepadStickInfoBlock> LookStickOverrides;
                
                [Flags]
                public enum UnitCameraFlags : ushort
                {
                    PitchBoundsAbsoluteSpace = 1 << 0,
                    OnlyCollidesWithEnvironment = 1 << 1,
                    // the player controlling this camera will not see their unit.  All other cameras will see this unit
                    HidesPlayerUnitFromCamera = 1 << 2,
                    // for cameras without tracks that use a marker position, specifies that we use the unit's aiming vector instead of
                    // the marker's forward vector.
                    // This results in more accurate aiming and smoother movement when frames are dropped
                    UseAimingVectorInsteadOfMarkerForward = 1 << 3
                }
                
                [TagStructure(Size = 0x20)]
                public class UnitCameraTrackBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "trak" })]
                    public CachedTag Track;
                    [TagField(ValidTags = new [] { "sefc" })]
                    public CachedTag ScreenEffect;
                }
                
                [TagStructure(Size = 0x18)]
                public class CameraObstructionStruct : TagStructure
                {
                    // controls how far from the focus point the outer raycasts start.  0 == cone, 1 == cylinder
                    public float CylinderFraction;
                    // how wide of a cone to test
                    public Angle ObstructionTestAngle; // degrees
                    // how quickly the camera moves inward when it anticipates a collision
                    public float ObstructionMaxInwardAccel; // 1.0/s/s
                    // how quickly the camera returns to a normal position when its anticipated distance is further than its current
                    public float ObstructionMaxOutwardAccel; // 1.0/s/s
                    // maximum speed the camera can move
                    public float ObstructionMaxVelocity; // 1.0/s
                    // when the camera wants to start moving back out, wait this long before doing so
                    public float ObstructionReturnDelay; // s
                }
                
                [TagStructure(Size = 0x70)]
                public class UnitCameraAccelerationDisplacementBlock : TagStructure
                {
                    // how quickly the camera can move to a new displacement (if the velocity suddenly changes).
                    // During this time the aim vector for the unit will be inaccurate, so don't set this too low.
                    // 0 defaults to infinite.
                    public float MaximumCameraVelocity; // wu/s
                    public UnitCameraAccelerationDisplacementFunctionStruct ForwardBack;
                    public UnitCameraAccelerationDisplacementFunctionStruct LeftRight;
                    public UnitCameraAccelerationDisplacementFunctionStruct UpDown;
                    
                    [TagStructure(Size = 0x24)]
                    public class UnitCameraAccelerationDisplacementFunctionStruct : TagStructure
                    {
                        public UnitCameraAccelerationDisplacementInput InputVariable;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public MappingFunction Mapping;
                        // for linear velocity; this is wu/s
                        // for linear acceleration; this is the fraction of the seat acceleration
                        // for angular velocity; this is deg/s
                        public float MaximumValue;
                        // scale factor used when this acceleration component is along the axis of the forward vector of the camera
                        public float CameraScale;
                        // scale factor used when this acceleration component is perpendicular to the camera
                        public float CameraScale1;
                        
                        public enum UnitCameraAccelerationDisplacementInput : sbyte
                        {
                            LinearVelocity,
                            LinearAcceleration,
                            Yaw,
                            Pitch,
                            Roll
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class MappingFunction : TagStructure
                        {
                            public byte[] Data;
                        }
                    }
                }
                
                [TagStructure(Size = 0x28)]
                public class GamepadStickInfoBlock : TagStructure
                {
                    public InputMappingShapeEnum InputShape;
                    [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                    public byte[] Padding;
                    // magnitude of analog input for pegged acceleration to kick in
                    public float PegThreshold01;
                    // time for a pegged look to reach maximum effect
                    public RealPoint2d PeggedTime;
                    // the maximum effect achieved over the duration of the pegged time.
                    public RealPoint2d PeggedScale;
                    // the maximum turning speed during peg
                    public Angle PegMaxAngularVelocity; // degrees per sec
                    public List<InputMappingFunctionBlock> InputMappingFunction;
                    
                    public enum InputMappingShapeEnum : sbyte
                    {
                        None,
                        UnitCircle,
                        UnitSquare
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class InputMappingFunctionBlock : TagStructure
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
        }
        
        [TagStructure(Size = 0x23C)]
        public class EquipmentabilityDatum : TagStructure
        {
            // use negative numbers for infinite duration
            public float Duration; // seconds
            // time before the equipment truly becomes active
            public float WarmupTime; // seconds
            // for equipment that suppresses combat actions, these actions remain suppressed for this long after the equipment
            // deactivates
            // Note that if this equipment also readies weapons when deactivated, the weapon will not be usable until both this
            // timer expires and the ready animation has finished, whichever is longer
            public float CooldownTime; // seconds
            // time in seconds for the phantom volumes on this object to start accelerating things in them
            public float PhantomVolumeActivationTime;
            // energy must be at least this high to activate
            // Like activation cost, but doesn't actually change energy levels
            public float MinimumActivationEnergy;
            // used by UI
            public float LowEnergyWarningThreshold;
            public float ActivationEnergyCost;
            public float DeactivationEnergyCost;
            public float ActiveEnergyRate; // energy/second
            public ScalarFunctionNamedStruct MovementSpeedToEnergyRate; // 1/s
            public float MovementSpeedDomain; // wu/s
            public EquipmentActivationFlags Flags;
            public EquipmentactivationSecondaryFlags SecondaryFlags;
            // -1 means unlimited charges
            public short Charges;
            public EquipmentActivationMode ActivationMode;
            // for AI perception.
            // while active, adjusts noises made by the owner unit by this many 'notches'.
            // note that this is additive, so a positive number is louder and a negative number is quieter
            public sbyte ObjectNoiseAdjustment;
            public List<EquipmentabilityTypeMultiplayerPowerupBlock> MultiplayerPowerup;
            public List<EquipmentabilityTypeSpawnerBlock> Spawner;
            public List<EquipmentabilityTypeAiSpawnerBlock> AiSpawner;
            public List<EquipmentabilityTypeProximityMineBlock> ProximityMine;
            public List<EquipmentabilityTypeMotionTrackerNoiseBlock> MoitionTrackerNoise;
            public List<EquipmentabilityTypeInvincibilityBlock> InvincibilityMode;
            public List<EquipmentabilityTypeTreeOfLifeBlock> TreeOfLife;
            public List<EquipmentabilityTypeShapeshifterBlock> Shapeshifter;
            public List<EquipmentabilityTypePlayerTraitFieldBlock> PlayerTraitField;
            public List<EquipmentabilityTypeAiTraitFieldBlock> AiTraitField;
            public List<EquipmentabilityTypeRepulsorFieldBlock> RepulsorField;
            public List<EquipmentabilityTypeStasisFieldBlock> StasisField;
            public List<EquipmentabilityTypeBallLightningBlock> BallLightning;
            public List<EquipmentabilityTypeDaddyBlock> Iwhbydaddy;
            public List<EquipmentabilityTypeLaserDesignationBlock> LaserDesignation;
            public List<EquipmentabilityTypeSuperJumpBlock> SuperJump;
            public List<EquipmentabilityTypeAmmoPackBlock> AmmoPack;
            public List<EquipmentabilityTypePowerFistBlock> PowerFist;
            public List<EquipmentabilityTypeHealthPackBlock> HealthPack;
            public List<EquipmentabilityTypeJetPackBlock> JetPack;
            public List<EquipmentabilityTypeHologramBlock> Hologram;
            public List<EquipmentabilityTypeSpecialWeaponBlock> SpecialWeapon;
            public List<EquipmentabilityTypeSpecialMoveBlock> SpecialMove;
            public List<EquipmentabilityTypeEngineerShieldsBlock> EngineerShields;
            public List<EquipmentabilityTypeSprintBlock> Sprint;
            public List<EquipmentabilityTypeTeleporterBlock> Teleporter;
            public List<EquipmentabilityTypeAutoTurretBlock> AutoTurret;
            public List<EquipmentabilityTypeVisionModeBlock> VisionMode;
            public List<EquipmentabilityTypeShieldProjectorBlock> ShieldProjector;
            public List<EquipmentabilityTypeProjectileCollectorBlock> ProjectileCollector;
            public List<EquipmentabilityTypeRemoteStrikeBlock> RemoteStrike;
            public List<EquipmentabilityTypeEquipmentHackerBlock> EquipmentHacker;
            public List<EquipmentabilityTypeRemoteVehicleBlock> RemoteVehicle;
            public List<EquipmentabilityTypeSuicideBombBlock> SuicideBomb;
            public List<EquipmentabilityTypeActiveShieldBlock> ActiveShield;
            [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
            public CachedTag ActivationWhileDisabledByPlayerTraitsSound;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag ActivateEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag LoopingEffect;
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag DeactivateEffect;
            public StringId ActivationAnimation;
            public StringId ActiveAnimation;
            public StringId DeactivateAnimation;
            public StringId ActiveAnimationStance;
            
            [Flags]
            public enum EquipmentActivationFlags : uint
            {
                _ThirdPersonCameraWhileActive = 1 << 0,
                _ThirdPersonCameraDuringActivationAnimation = 1 << 1,
                HideReticuleWhileActive = 1 << 2,
                // if checked, this equipment cannot be activated if the user is airborne, and deactivates itself if the user becomes
                // airborne
                CannotBeActiveWhileAirborne = 1 << 3,
                // can't activate in midair, but doesn't turn off if you later become airborne
                CannotActivateWhileAirborne = 1 << 4,
                // if you are standing on another biped you can not use this equipment
                CannotActivateWhileStandingOnBiped = 1 << 5,
                // can't be activated in a seat, and deactivates if a vehicle is entered
                CannotBeActiveInVehicle = 1 << 6,
                // e.g. by mancannon
                CannotBeActiveWhileMotionIsModifiedByPhysics = 1 << 7,
                SuppressesWeaponsWhileActive = 1 << 8,
                SuppressesMeleeWhileActive = 1 << 9,
                SuppressesGrenadesWhileActive = 1 << 10,
                SuppressesDeviceInteractionWhileActive = 1 << 11,
                // probably this means sprint
                SuppressesHeroAssistEquipmentUseWhileActive = 1 << 12,
                SuppressesEnteringVehicle = 1 << 13,
                ActivationAnimSuppressesWeaponsAndMelee = 1 << 14,
                DuckSoundWhileActive = 1 << 15,
                BlocksTrackingWhileActive = 1 << 16,
                // note - if this equipment has an animation cycle, the weapon is readied after the exit animation finishes.
                // Otherwise the weapon-ready happens immediately on deactivation
                ReadiesWeaponOnDeactivation = 1 << 17,
                DropsSupportMustBeReadiedWeaponsOnActivation = 1 << 18,
                // checking this flag will automatically cause the weapon to ready on deactivation
                HidesWeaponOnActivation = 1 << 19,
                // cannot activate while in vehicle, but previously activated equipment remains active
                CannotActivateInVehicle = 1 << 20,
                DeactivatedByFiringWeapon = 1 << 21,
                DeactivatedByReloadingWeapon = 1 << 22,
                DeactivatedBySwitchingWeapon = 1 << 23,
                DeactivatedByThrowingGrenade = 1 << 24,
                DeactivatedByGrenadeAnim = 1 << 25,
                DeactivatedByMeleeAttacking = 1 << 26,
                SuppressesWeaponZoomWhileActive = 1 << 27,
                // Prevents auto-pick-up of weapons set to auto by megalo action weapon_set_picup_priority
                IgnoreAutoPickUpWeaponsWhileActive = 1 << 28,
                CannotCrouchWhileActive = 1 << 29,
                ActivationInterruptsMelee = 1 << 30,
                ActivationInterruptsGrenades = 1u << 31
            }
            
            [Flags]
            public enum EquipmentactivationSecondaryFlags : uint
            {
                // the equipment only suppresses zoom during its activation animation
                SuppressesWeaponZoomDuringActivationAnimation = 1 << 0,
                SuppressesJumpingWhileActive = 1 << 1,
                SuppressesToggleDeactivationWhileActive = 1 << 2,
                // only suppresses jumping during its activation animation
                SuppressesJumpingDuringActivationAnimation = 1 << 3,
                // only suppresses vehicle entry during its activation animation
                SuppressesVehicleEntryDuringActivationAnimation = 1 << 4,
                // only suppresses grenade usage during its activation animation
                SuppressesGrenadeUsageDuringActivationAnimation = 1 << 5,
                HideReticuleDuringActivationAnimation = 1 << 6,
                HideReticuleDuringDuringWeaponReady = 1 << 7,
                DeactivatingDuringWarmupWillApplyDeactivationEnergyPenalty = 1 << 8,
                DeactivatingDuringWarmupWillFireDeactivationEffects = 1 << 9,
                ApplyPlayerTraitsDuringWarmup = 1 << 10,
                ApplyPlayerTraitsDuringCooldown = 1 << 11,
                HeroAssistAbility = 1 << 12,
                // used by Auto Turret to bypass how support weapons force equipment deactivation
                IgnoresNormalSupportWeaponForceDeactivationOfEquipment = 1 << 13
            }
            
            public enum EquipmentActivationMode : sbyte
            {
                // toggles state when X is pressed
                Toggle,
                // activates when X is pressed and deactivates when X is released
                Hold,
                // activates when X is pressed twice in quick succession
                DoubleTap,
                // activates when player shield fails
                ShieldFail,
                // activates when player dies
                Death
            }
            
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
            
            [TagStructure(Size = 0x4)]
            public class EquipmentabilityTypeMultiplayerPowerupBlock : TagStructure
            {
                public MultiplayerPowerupFlavor Flavor;
                
                public enum MultiplayerPowerupFlavor : int
                {
                    RedPowerup,
                    BluePowerup,
                    YellowPowerup,
                    CustomPowerup
                }
            }
            
            [TagStructure(Size = 0x34)]
            public class EquipmentabilityTypeSpawnerBlock : TagStructure
            {
                // distance from players eyeball on the z-plane that this effect spawns
                public float SpawnRadius;
                // z-offset of effect spawn
                public float SpawnZOffset;
                // need a sphere of radius r's free space in order to spawn, otherwise we pick a new spawn location
                public float SpawnAreaRadius;
                // WU/sec
                public float SpawnVelocity;
                public EquipmentSpawnerSpawnType Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "obje" })]
                public CachedTag SpawnedObject;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag SpawnedEffect;
                
                public enum EquipmentSpawnerSpawnType : short
                {
                    AlongAimingVector,
                    CameraPosZPlane,
                    FootPosZPlane
                }
            }
            
            [TagStructure(Size = 0x24)]
            public class EquipmentabilityTypeAiSpawnerBlock : TagStructure
            {
                // distance from players eyeball on the z-plane that this effect spawns
                public float SpawnRadius;
                // z-offset of effect spawn
                public float SpawnZOffset;
                // need a sphere of radius r's free space in order to spawn, otherwise we pick a new spawn location
                public float SpawnAreaRadius;
                // WU/sec
                public float SpawnVelocity;
                public EquipmentSpawnerSpawnType Type;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "char" })]
                public CachedTag SpawnedCharacter;
                
                public enum EquipmentSpawnerSpawnType : short
                {
                    AlongAimingVector,
                    CameraPosZPlane,
                    FootPosZPlane
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class EquipmentabilityTypeProximityMineBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag ExplosionEffect;
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag ExplosionDamageEffect;
                // time before it becomes a proximity mine
                public float ArmTime;
                // seconds after it is created that it must self destruct.  0 means never destroy
                public float SelfDestructTime;
                // seconds object moving at trigger velocity takes to trigger explosion.  This will smooth out sensitivity to velocity
                // noise
                public float TriggerTime;
                // WU/sec at which we trigger explosion
                public float TriggerVelocity;
            }
            
            [TagStructure(Size = 0x14)]
            public class EquipmentabilityTypeMotionTrackerNoiseBlock : TagStructure
            {
                public EquipmentabilityTypeMotionTrackerNoiseFlags Flags;
                // time before it starts making noise
                public float ArmTime;
                // radius in WU that the noise extends to.
                public float NoiseRadius;
                // number of noise points that are generated
                public int NoiseCount;
                // radius in WU that the damage flash noise extends to.
                public float FlashRadius;
                
                [Flags]
                public enum EquipmentabilityTypeMotionTrackerNoiseFlags : uint
                {
                    AffectsSelf = 1 << 0,
                    AffectsFriendlies = 1 << 1
                }
            }
            
            [TagStructure(Size = 0x80)]
            public class EquipmentabilityTypeInvincibilityBlock : TagStructure
            {
                public StringId InvincibilityMaterial;
                public short InvincibilityMaterialType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // while active, shields recharge at this fraction per second
                public float ShieldRechargeRate; // 1.0f/s
                // highest level shield can recharge to (can be up to 4)
                public float ShieldMaxRechargeLevel;
                [TagField(ValidTags = new [] { "cddf" })]
                public CachedTag OverrideCollisionDamage;
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag AiMeleeReflectDamage;
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag PlayerMeleeReflectDamage;
                // active while the equipment is in use (used for shield rendering effects)
                public StringId LoopInvincibilityShieldName;
                // active once the equipment is no longer in use
                public StringId PostInvincibilityShieldName;
                public ScalarFunctionNamedStruct PostInvincibilityTimeToShieldLevelFunction;
                // we use this to specify the domain of the active vertical velocity funtion
                public float MaximumVerticalVelocity; // WU/SEC
                public ScalarFunctionNamedStruct ActiveVerticalVelocityDamping;
                // the effect with the highest threshold will play on deactivation
                public List<EquipmenteffectWithThresholdBlock> ThresholdEffects;
                
                [TagStructure(Size = 0x18)]
                public class EquipmenteffectWithThresholdBlock : TagStructure
                {
                    // how much energy you have to burn to play this effect
                    public float ThresholdEnergyBurned; // 0-1
                    // how much energy to add when playing this effect
                    public float EnergyAdjustment; // -1 to 1
                    [TagField(ValidTags = new [] { "effe" })]
                    public CachedTag Effect;
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class EquipmentabilityTypeTreeOfLifeBlock : TagStructure
            {
                public EquipmentabilityTypeTreeoflifeFlags Flags;
                public StringId OriginMarker;
                public float Radius;
                
                [Flags]
                public enum EquipmentabilityTypeTreeoflifeFlags : uint
                {
                    UnStunsShileds = 1 << 0,
                    UnStunsBody = 1 << 1
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class EquipmentabilityTypeShapeshifterBlock : TagStructure
            {
                public StringId RegionName;
                public StringId InactivePermutationName;
                public StringId ActivePermutationName;
            }
            
            [TagStructure(Size = 0x1C)]
            public class EquipmentabilityTypePlayerTraitFieldBlock : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public List<GameEnginePlayerTraitsBlock> ActivePlayerTraits;
                public List<GameEnginePlayerTraitsBlock> InactivePlayerTraits;
                
                [TagStructure(Size = 0x3C)]
                public class GameEnginePlayerTraitsBlock : TagStructure
                {
                    public List<PlayerTraitsVitalityBlock> VitalityTraits;
                    public List<PlayerTraitsWeaponsBlock> WeaponTraits;
                    public List<PlayerTraitsMovementBlock> MovementTraits;
                    public List<PlayerTraitsAppearanceBlock> AppearanceTraits;
                    public List<PlayerTraitsSensorsBlock> SensorTraits;
                    
                    [TagStructure(Size = 0x40)]
                    public class PlayerTraitsVitalityBlock : TagStructure
                    {
                        public PlayerTraitsVitalityFloatFlags ShouldApplyTrait;
                        public float DamageResistance;
                        public float ShieldMultiplier;
                        public float BodyMultiplier;
                        public float ShieldStunDuration;
                        public float ShieldRechargeRate;
                        public float BodyRechargeRate;
                        public float OvershieldRechargeRate;
                        public float VampirismPercent;
                        // incoming damage multiplied by (1 - resistance)
                        public float ExplosiveDamageResistance;
                        public float WheelmanArmorVehicleStunTimeModifier;
                        public float WheelmanArmorVehicleRechargeTimeModifier;
                        public float WheelmanArmorVehicleEmpDisabledTimeModifier;
                        public float FallDamageMultiplier;
                        public PlayerTraitBoolEnum HeadshotImmunity;
                        public PlayerTraitBoolEnum AssassinationImmunity;
                        public PlayerTraitBoolEnum Deathless;
                        public PlayerTraitBoolEnum FastTrackArmor;
                        public PlayerTraitPowerupCancellationEnum PowerupCancellation;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        [Flags]
                        public enum PlayerTraitsVitalityFloatFlags : uint
                        {
                            DamageResistance = 1 << 0,
                            ShieldMultiplier = 1 << 1,
                            BodyMultiplier = 1 << 2,
                            ShieldStunDuration = 1 << 3,
                            ShieldRechargeRate = 1 << 4,
                            BodyRechargeRate = 1 << 5,
                            OvershieldRechargeRate = 1 << 6,
                            VampirismPercent = 1 << 7,
                            ExplosiveDamageResistance = 1 << 8,
                            WheelmanArmorVehicleStunTimeModifier = 1 << 9,
                            WheelmanArmorVehicleRechargeTimeModifier = 1 << 10,
                            WheelmanArmorVehicleEmpDisabledTimeModifier = 1 << 11,
                            FallDamageMultiplier = 1 << 12
                        }
                        
                        public enum PlayerTraitBoolEnum : sbyte
                        {
                            Unchanged,
                            False,
                            True
                        }
                        
                        public enum PlayerTraitPowerupCancellationEnum : sbyte
                        {
                            Unchanged,
                            None,
                            NoOvershield
                        }
                    }
                    
                    [TagStructure(Size = 0x80)]
                    public class PlayerTraitsWeaponsBlock : TagStructure
                    {
                        public PlayerTraitsWeaponsFloatFlags ShouldApplyTrait;
                        public float DamageMultiplier;
                        public float MeleeDamageMultiplier;
                        public float GrenadeRechargeSecondsFrag;
                        public float GrenadeRechargeSecondsPlasma;
                        public float GrenadeRechargeSecondsSpike;
                        public float HeroEquipmentEnergyUseRateModifier;
                        public float HeroEquipmentEnergyRechargeDelayModifier;
                        public float HeroEquipmentEnergyRechargeRateModifier;
                        public float HeroEquipmentInitialEnergyModifier;
                        public float EquipmentEnergyUseRateModifier;
                        public float EquipmentEnergyRechargeDelayModifier;
                        public float EquipmentEnergyUseRechargeRateModifier;
                        public float EquipmentEnergyInitialEnergyModifier;
                        public float SwitchSpeedModifier;
                        public float ReloadSpeedModifier;
                        public float OrdnancePointsModifier;
                        public float ExplosiveAreaOfEffectRadiusModifier;
                        public float GunnerArmorModifier;
                        public float StabilityArmorModifier;
                        public float DropReconWarningSeconds;
                        public float DropReconDistanceModifier;
                        public float AssassinationSpeedModifier;
                        public PlayerTraitBoolEnum WeaponPickupAllowed;
                        public PlayerTraitInitialGrenadeCountEnum InitialGrenadeCount;
                        public PlayerTraitInfiniteAmmoEnum InfiniteAmmo;
                        public PlayerTraitEquipmentUsageEnum EquipmentUsage;
                        // false will disable all equipment except auto turret
                        public PlayerTraitEquipmentUsageEnum EquipmentUsageExceptingAutoTurret;
                        public PlayerTraitBoolEnum EquipmentDrop;
                        public PlayerTraitBoolEnum InfiniteEquipment;
                        public PlayerTraitBoolEnum WeaponsAmmopack;
                        public PlayerTraitBoolEnum WeaponsGrenadier;
                        // spawns projectile specified in globals.globals
                        public PlayerTraitBoolEnum WeaponsExplodeOnDeathArmormod;
                        public PlayerTraitBoolEnum OrdnanceMarkersVisible;
                        public PlayerTraitBoolEnum WeaponsOrdnanceRerollAvailable;
                        // grenade probabilities defined in grenade_list.game_globals_grenade_list
                        public PlayerTraitBoolEnum WeaponsResourceful;
                        public PlayerTraitBoolEnum WeaponsWellEquipped;
                        public PlayerTraitBoolEnum OrdnanceDisabled;
                        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        public StringId InitialPrimaryWeapon;
                        public StringId InitialSecondaryWeapon;
                        public StringId InitialEquipment;
                        public StringId InitialTacticalPackage;
                        public StringId InitialSupportUpgrade;
                        
                        [Flags]
                        public enum PlayerTraitsWeaponsFloatFlags : uint
                        {
                            DamageMultiplier = 1 << 0,
                            MeleeDamageMultiplier = 1 << 1,
                            GrenadeRechargeSecondsFrag = 1 << 2,
                            GrenadeRechargeSecondsPlasma = 1 << 3,
                            GrenadeRechargeSecondsSpike = 1 << 4,
                            HeroEquipmentEnergyUseRateModifier = 1 << 5,
                            HeroEquipmentEnergyRechargeDelayModifier = 1 << 6,
                            HeroEquipmentEnergyRechargeRateModifier = 1 << 7,
                            HeroEquipmentInitialEnergyModifier = 1 << 8,
                            EquipmentEnergyUseRateModifier = 1 << 9,
                            EquipmentEnergyRechargeDelayModifier = 1 << 10,
                            EquipmentEnergyUseRechargeRateModifier = 1 << 11,
                            EquipmentEnergyInitialEnergyModifier = 1 << 12,
                            SwitchSpeedModifier = 1 << 13,
                            ReloadSpeedModifier = 1 << 14,
                            OrdnancePointsModifier = 1 << 15,
                            ExplosiveAreaOfEffectRadiusModifier = 1 << 16,
                            GunnerArmorModifier = 1 << 17,
                            StabilityArmorModifier = 1 << 18,
                            DropReconWarningSeconds = 1 << 19,
                            DropReconDistanceModifier = 1 << 20,
                            AssassinationSpeedModifier = 1 << 21
                        }
                        
                        public enum PlayerTraitBoolEnum : sbyte
                        {
                            Unchanged,
                            False,
                            True
                        }
                        
                        public enum PlayerTraitInitialGrenadeCountEnum : sbyte
                        {
                            Unchanged,
                            MapDefault,
                            _0,
                            _1Frag,
                            _2Frag,
                            _1Plasma,
                            _2Plasma,
                            _1Type2,
                            _2Type2,
                            _1Type3,
                            _2Type3,
                            _1Type4,
                            _2Type4,
                            _1Type5,
                            _2Type5,
                            _1Type6,
                            _2Type6,
                            _1Type7,
                            _2Type7
                        }
                        
                        public enum PlayerTraitInfiniteAmmoEnum : sbyte
                        {
                            Unchanged,
                            Off,
                            On,
                            BottomlessClip
                        }
                        
                        public enum PlayerTraitEquipmentUsageEnum : sbyte
                        {
                            Unchanged,
                            Off,
                            NotWithObjectives,
                            On
                        }
                    }
                    
                    [TagStructure(Size = 0x1C)]
                    public class PlayerTraitsMovementBlock : TagStructure
                    {
                        public PlayerTraitsMovementFloatFlags ShouldApplyTrait;
                        public float Speed;
                        public float GravityMultiplier;
                        public float JumpMultiplier;
                        public float TurnSpeedMultiplier;
                        public PlayerTraitVehicleUsage VehicleUsage;
                        public PlayerTraitDoubleJump DoubleJump;
                        public PlayerTraitBoolEnum SprintUsage;
                        public PlayerTraitBoolEnum AutomaticMomentumUsage;
                        public PlayerTraitBoolEnum VaultingEnabled;
                        public PlayerTraitBoolEnum Stealthy;
                        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        [Flags]
                        public enum PlayerTraitsMovementFloatFlags : uint
                        {
                            Speed = 1 << 0,
                            GravityMultiplier = 1 << 1,
                            JumpMultiplier = 1 << 2,
                            TurnSpeedMultiplier = 1 << 3
                        }
                        
                        public enum PlayerTraitVehicleUsage : sbyte
                        {
                            Unchanged,
                            None,
                            PassengerOnly,
                            DriverOnly,
                            GunnerOnly,
                            NotPassenger,
                            NotDriver,
                            NotGunner,
                            Full
                        }
                        
                        public enum PlayerTraitDoubleJump : sbyte
                        {
                            Unchanged,
                            Off,
                            On,
                            OnPlusLunge
                        }
                        
                        public enum PlayerTraitBoolEnum : sbyte
                        {
                            Unchanged,
                            False,
                            True
                        }
                    }
                    
                    [TagStructure(Size = 0xC)]
                    public class PlayerTraitsAppearanceBlock : TagStructure
                    {
                        public PlayerTraitActiveCamo ActiveCamo;
                        public PlayerTraitWaypoint Waypoint;
                        public PlayerTraitWaypoint GamertagVisible;
                        public PlayerTraitAura Aura;
                        [TagField(Length = 32)]
                        public string DeathEffect;
                        public StringId AttachedEffect;
                        
                        public enum PlayerTraitActiveCamo : sbyte
                        {
                            Unchanged,
                            Off,
                            Poor,
                            Good,
                            Excellent,
                            Invisible
                        }
                        
                        public enum PlayerTraitWaypoint : sbyte
                        {
                            Unchanged,
                            Off,
                            Allies,
                            All
                        }
                        
                        public enum PlayerTraitAura : sbyte
                        {
                            Unchanged,
                            Off,
                            TeamColor,
                            Black,
                            White
                        }
                    }
                    
                    [TagStructure(Size = 0x14)]
                    public class PlayerTraitsSensorsBlock : TagStructure
                    {
                        public PlayerTraitsSensorsFloatFlags ShouldApplyTrait;
                        public float MotionTrackerRange;
                        public float NemesisDuration; // seconds
                        public PlayerTraitMotionTracker MotionTracker;
                        public PlayerTraitBoolEnum MotionTrackerWhileZoomed;
                        public PlayerTraitBoolEnum DirectionalDamageIndicator;
                        public PlayerTraitBoolEnum VisionMode;
                        public PlayerTraitBoolEnum BattleAwareness;
                        public PlayerTraitBoolEnum ThreatView;
                        public PlayerTraitBoolEnum AuralEnhancement;
                        public PlayerTraitBoolEnum Nemesis;
                        
                        [Flags]
                        public enum PlayerTraitsSensorsFloatFlags : uint
                        {
                            MotionTrackerRange = 1 << 0,
                            NemesisDuration = 1 << 1
                        }
                        
                        public enum PlayerTraitMotionTracker : sbyte
                        {
                            Unchanged,
                            Off,
                            MovingFriendlyBipedsMovingNeutralVehicles,
                            MovingBipedsMovingVehicles,
                            AllBipedsMovingVehicles
                        }
                        
                        public enum PlayerTraitBoolEnum : sbyte
                        {
                            Unchanged,
                            False,
                            True
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0x18)]
            public class EquipmentabilityTypeAiTraitFieldBlock : TagStructure
            {
                public List<AiequipmentTraitsBlock> ActiveAiEquipmentTraits;
                public List<AiequipmentTraitsBlock> InactiveAiEquipmentTraits;
                
                [TagStructure(Size = 0xC)]
                public class AiequipmentTraitsBlock : TagStructure
                {
                    public List<AiequipmentTraitAppearanceBlock> AppearanceTraits;
                    
                    [TagStructure(Size = 0x4)]
                    public class AiequipmentTraitAppearanceBlock : TagStructure
                    {
                        public PlayerTraitActiveCamo ActiveCamoSetting;
                        [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                        public byte[] Padding;
                        
                        public enum PlayerTraitActiveCamo : sbyte
                        {
                            Unchanged,
                            Off,
                            Poor,
                            Good,
                            Excellent,
                            Invisible
                        }
                    }
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class EquipmentabilityTypeRepulsorFieldBlock : TagStructure
            {
                public RepulsorFieldFlags Flags;
                public float Radius; // wu
                public float Power;
                
                [Flags]
                public enum RepulsorFieldFlags : uint
                {
                    AffectsProjectiles = 1 << 0,
                    AffectsVehicles = 1 << 1,
                    AffectsBipeds = 1 << 2
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class EquipmentabilityTypeStasisFieldBlock : TagStructure
            {
                public RepulsorFieldFlags Flags;
                public float Radius; // wu
                public float TimeDilationPlayerCharacters;
                public float TimeDilationProjectiles;
                public float TimeDilationVehicles;
                // everything else
                public float TimeDilationOther;
                public float MaxBipedTurningRate; // no idea what units... something like .2 or so
                // negative values will invert gravity
                public float GravityMultiplier;
                
                [Flags]
                public enum RepulsorFieldFlags : uint
                {
                    AffectsProjectiles = 1 << 0,
                    AffectsVehicles = 1 << 1,
                    AffectsBipeds = 1 << 2
                }
            }
            
            [TagStructure(Size = 0x20)]
            public class EquipmentabilityTypeBallLightningBlock : TagStructure
            {
                public RepulsorFieldFlags Flags;
                public float StartRadius; // wu
                // MUST BE GREATER THAN ZERO - each time lightning chains, this is the multiplier that controls how much the radius is
                // reduced by
                public float ChainRadiusReductionMultiplier; // [0.01, 1]
                public float ChainDelayTimer; // seconds
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag LightningDamage;
                
                [Flags]
                public enum RepulsorFieldFlags : uint
                {
                    AffectsProjectiles = 1 << 0,
                    AffectsVehicles = 1 << 1,
                    AffectsBipeds = 1 << 2
                }
            }
            
            [TagStructure(Size = 0x60)]
            public class EquipmentabilityTypeDaddyBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "obje" })]
                public CachedTag ChildObject;
                public EquipmentabilityTypeDaddyVisibleFlags Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // scale factor for energy gain from shield damage to child.
                // Damage is normalized, so if this value is -1.0 then the equipment will lose all its energy when its child's shield
                // is depleted.
                public float ShieldDamageToEnergyScale;
                // if >0 (and warm up time is >0), object size will scale up from this up to 1.0 over course of warm up time and down
                // over cooldown time
                public float StartingWarmUpObjectScale;
                // the root of the child is offset by this amount from the root of the parent biped
                public RealPoint3d OffsetFromParent;
                // used to adjust hight per biped, regardless of aim direction
                public float VerticalOffsetInWorldSpace;
                // the min and max pitch that the child will follow as you aim up and down.  -90 to 90
                public Bounds<float> MinAndMaxPitch; // degrees
                public float VisualActivationTime; // seconds
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag SpawnEffect;
                public StringId SpawnEffectMarker;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag DespawnEffect;
                public StringId DespawnEffectMarker;
                
                [Flags]
                public enum EquipmentabilityTypeDaddyVisibleFlags : ushort
                {
                    InactiveUnstowed = 1 << 0,
                    InactiveStowed = 1 << 1,
                    ActiveUnstowed = 1 << 2,
                    ActiveStowed = 1 << 3,
                    WarmingUpUnstowed = 1 << 4,
                    WarmingUpStowed = 1 << 5,
                    CoolingDownUnstowed = 1 << 6,
                    CoolingDownStowed = 1 << 7,
                    // as energy goes down, the shield vitality will as well
                    ShieldVitalityTiedToEnergy = 1 << 8
                }
            }
            
            [TagStructure(Size = 0x4)]
            public class EquipmentabilityTypeLaserDesignationBlock : TagStructure
            {
                public float Unused;
            }
            
            [TagStructure(Size = 0x14)]
            public class EquipmentabilityTypeSuperJumpBlock : TagStructure
            {
                public ScalarFunctionNamedStruct EnergyToJump;
            }
            
            [TagStructure(Size = 0x24)]
            public class EquipmentabilityTypeAmmoPackBlock : TagStructure
            {
                public float EnergyChangePerClipAdded;
                public int ExtraFrags;
                public int ExtraPlasma;
                public int ExtraGrenade3;
                public int ExtraGrenade4;
                public int ExtraGrenade5;
                public int ExtraGrenade6;
                public int ExtraGrenade7;
                public int ExtraGrenade8;
            }
            
            [TagStructure(Size = 0x40)]
            public class EquipmentabilityTypePowerFistBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag MeleeDamage;
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag MeleeResponse;
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag ClangDamage;
                [TagField(ValidTags = new [] { "jpt!","drdf" })]
                public CachedTag ClangResponse;
            }
            
            [TagStructure(Size = 0x14)]
            public class EquipmentabilityTypeHealthPackBlock : TagStructure
            {
                public ScalarFunctionNamedStruct HealthGivenOverEnergyUsed;
            }
            
            [TagStructure(Size = 0x70)]
            public class EquipmentabilityTypeJetPackBlock : TagStructure
            {
                public EquipmentabilityTypeJetPackFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScalarFunctionNamedStruct ThrustToAccelerationFunction;
                public ScalarFunctionNamedStruct InfiniteEnergyThrustToAccelerationFunction;
                public float ThrustAttackTime; // s
                public float ThrustDecayTime; // s
                // when falling, scale the thrust by this (so you can 'catch' yourself easier)
                public float NegativeVelocityAccelerationScale;
                public float AirborneAccelerationScale;
                // how hard to kick you upwards if you activate while grounded
                public float GroundedAccelerationMagnitude;
                // how many seconds before landing the jetpack user enters the airborne arc
                public float LandingAnticipationTime; // s
                // Camera direction (0.0) VS movement input (1.0) interpolation
                public float CameraVsMoveInputThrustControl;
                // drag will be applied above this
                public float MinSpeed;
                public float MaxSpeed;
                public float StickForwardThrust;
                public float StickStrafeThrust;
                public float Drag;
                // if 0, drag is always applied
                public float DragCutoffTime;
                public float CowCatcherDuration;
                public List<EquipmentabilityPartCowCatcherBlock> CowCatcherParameters;
                
                [Flags]
                public enum EquipmentabilityTypeJetPackFlags : byte
                {
                    // Jetpack thrust dir is controlled by camera dir and player movement (signifies Zero-G jet pack)
                    UseCameraAndMovementForThrustDir = 1 << 0,
                    // don't let the player change the direction of thrust once they activate
                    CacheThrustAtActivation = 1 << 1
                }
                
                [TagStructure(Size = 0x2C)]
                public class EquipmentabilityPartCowCatcherBlock : TagStructure
                {
                    // world units
                    public float CowCatcherHeight;
                    // world units, the width of the flat front portion of the cow-catcher
                    public float CowCatcherFrontWidth;
                    // world units, the width of the angled side portion of the cow-catcher
                    public float CowCatcherSideWidth;
                    // world units, the depth of the angled side portion of the cow-catcher
                    public float CowCatcherSideDepth;
                    // offset from the unit's origin to put the origin of the cow-catcher at
                    public RealVector3d CowCatcherOffset;
                    // if "hide unit during transit" isn't checked, this can override the unit's collision damage definition during the
                    // teleport
                    [TagField(ValidTags = new [] { "cddf" })]
                    public CachedTag CollisionDamageOverride;
                }
            }
            
            [TagStructure(Size = 0x64)]
            public class EquipmentabilityTypeHologramBlock : TagStructure
            {
                public float HologramDuration; // s
                public CollisionFilterEnum HavokFilterGroup;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag CreationEffect;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag AttachedEffect;
                public StringId AttachedEffectMarker;
                public StringId AttachedEffectPrimaryScale;
                public StringId AttachedEffectSecondaryScale;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag DestructionEffect;
                // how fast shimmer decreases
                public float ShimmerDecreaseRate; // 1.0/s
                // how much to ping shimmer when hit by a bullet
                public float ShimmerBulletPing; // 0-1
                // this is a periodic function with a period of 1 second
                // the shimmer value is used as the range input (interpolates between green and red)
                public ScalarFunctionNamedStruct ShimmerToCamoFunction;
                public EquipmentabilityTypeHologramFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                public enum CollisionFilterEnum : int
                {
                    Everything,
                    EnvironmentDefault,
                    EnvironmentOnly,
                    SmallCrate,
                    Crate,
                    HugeCrate,
                    Item,
                    Projectile,
                    Machine,
                    EarlyMoverMachine,
                    Creature,
                    Biped,
                    DeadBiped,
                    SuperCollidableRagdoll,
                    Ragdoll,
                    Vehicle,
                    Decal,
                    ForgeDynamicScenary,
                    SmallExpensivePlant,
                    TechArtCustom,
                    Proxy,
                    HugeVehicle,
                    IgnoreEnvironment,
                    CharacterPosture,
                    ItemBlocker,
                    User00,
                    ZeroExtent,
                    PhysicalProjectile,
                    EnvironmentInvisibleWall,
                    EnvironmentPlayCollision,
                    EnvironmentBulletCollision
                }
                
                [Flags]
                public enum EquipmentabilityTypeHologramFlags : byte
                {
                    // hologram is automatically bump possessed by the player
                    Driveable = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x10)]
            public class EquipmentabilityTypeSpecialWeaponBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "weap" })]
                public CachedTag Weapon;
            }
            
            [TagStructure(Size = 0x2C)]
            public class EquipmentabilityTypeSpecialMoveBlock : TagStructure
            {
                public StringId Forward;
                public StringId Left;
                public StringId Backward;
                public StringId Right;
                public EquipmentSpecialMoveDefaultDirectionDefinition DefaultDirection;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag DirectionalEffect;
                // AI avoidance radius.  Ignored if zero.
                public float DangerRadius;
                // Use this instead of a trait.  0 defaults to 1.  With a trait, the client will move at non-modified scale until it
                // gets replicated.
                public float SpeedMultiplier;
                
                public enum EquipmentSpecialMoveDefaultDirectionDefinition : sbyte
                {
                    None,
                    Forward,
                    Left,
                    Backward,
                    Right
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class EquipmentabilityTypeEngineerShieldsBlock : TagStructure
            {
                public float Radius;
                public StringId ShieldName;
                public EquipmentEngineerShieldsFlags Flags;
                
                [Flags]
                public enum EquipmentEngineerShieldsFlags : uint
                {
                    GivesShieldsToOwner = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x1)]
            public class EquipmentabilityTypeSprintBlock : TagStructure
            {
                [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x5C)]
            public class EquipmentabilityTypeTeleporterBlock : TagStructure
            {
                public Teleporterflags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float TeleportDistance;
                // the speed at which you cross the teleporter distance; 0 means instant
                public float TravelSpeed;
                // the speed you return to at the end of the teleport, if "hide unit during transit" isn't checked
                public float DeactivationSpeed;
                // an effect that will follow along the travel path
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag TraceEffect;
                // offset from the unit's origin to put the effects at
                public RealVector3d TraceEffectOffset;
                // [-90 to 90] the pitch of the search vector will be clamped to no higher than this when starting a teleport in the
                // air
                public float MaxPitch;
                // [-90 to 90] the pitch of the search vector will be clamped to no higher than this when starting a teleport on the
                // ground
                public float MaxPitch1;
                // value from 0 to 1 for how much we can use the look vector in place of the movement vector
                public float LookVectorWeight;
                // degrees, inside this angle, we use the look vector at full weight
                public float LookVectorFalloffInner;
                // degrees, outside this angle, we only use the movement vector
                public float LookVectorFalloffOuter;
                // degrees, how much to offset the move vector vertically; positive is up, negative is down
                public float MoveVectorVerticalOffset;
                // degrees, the angle to each side of the horizontal probes
                public float HorizontalProbeAngle;
                // degrees, the angle up and down covered by the fan of vertical rays
                public float VerticalProbeAngle;
                // higher is more likely to find a good match, but more expensive
                public int NumberOfVerticalProbes;
                public List<EquipmentabilityPartCowCatcherBlock> CowCatcherParameters;
                
                [Flags]
                public enum Teleporterflags : byte
                {
                    // during the teleport, hide the unit and turn off collision
                    HideUnitDuringTransit = 1 << 0,
                    // makes it so that we always thrust or teleport the full distance; not wise unless "hide unit during transit" is
                    // unchecked
                    DisableShortenedTeleport = 1 << 1,
                    // only the center probe is cast, no extra probes
                    DisableExtraProbes = 1 << 2,
                    // ignores all search vector parameters and uses the aim vector as the search vector
                    ForceAlongAimVector = 1 << 3,
                    // only collides against environment and structures
                    CanPassThroughObjects = 1 << 4,
                    // if stick throw magnitude==0 then search in facing direction, else search in stick direction
                    ForceAlongStickDirection = 1 << 5
                }
                
                [TagStructure(Size = 0x2C)]
                public class EquipmentabilityPartCowCatcherBlock : TagStructure
                {
                    // world units
                    public float CowCatcherHeight;
                    // world units, the width of the flat front portion of the cow-catcher
                    public float CowCatcherFrontWidth;
                    // world units, the width of the angled side portion of the cow-catcher
                    public float CowCatcherSideWidth;
                    // world units, the depth of the angled side portion of the cow-catcher
                    public float CowCatcherSideDepth;
                    // offset from the unit's origin to put the origin of the cow-catcher at
                    public RealVector3d CowCatcherOffset;
                    // if "hide unit during transit" isn't checked, this can override the unit's collision damage definition during the
                    // teleport
                    [TagField(ValidTags = new [] { "cddf" })]
                    public CachedTag CollisionDamageOverride;
                }
            }
            
            [TagStructure(Size = 0x88)]
            public class EquipmentabilityTypeAutoTurretBlock : TagStructure
            {
                public EquipmentAutoTurretFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "obje" })]
                public CachedTag Weapon;
                // z forward, x right, y down.  Set x,z zero to get independent turret, y is altitude off ground
                public RealPoint3d OrbitOffsetFromPlayer;
                // if above are zeros, this is max turret range
                public float OrbitRange;
                public float OrbitRate; // orbits per second
                // if not following player, maximum distance downrange turret will travel
                public float MaximumRange;
                public float MaxVelocity; // units per second
                public float MaxAcceleration; // units per second squared
                // amount of time after a moving turret engages a target before it halts movement
                public float TurretHaltEngageTime; // seconds
                // the multiplier on equipment drain when equipment is in its idle state
                public float TurretIdleEquipmentDrainMultiplier; // [0.1]
                // the multiplier on equipment drain when equipment is in its inactive state
                public float TurretInactiveEquipmentDrainMultiplier; // [0.1]
                // area that must be clear in order for turret to spawn
                public float SpawnRadius; // world units
                // the turret will be inactive for this duration
                public float SpawnInTime; // seconds
                // relative to origin and camera direction without pitch
                public RealVector3d SpawnOffsetFromPlayer; // world units
                public float VerticalBobHeight; // world units
                public float VerticalBobsPerSecond;
                // effect played on the turret when it is spawned into the world
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag SpawnEffect;
                public StringId SpawnEffectMarker;
                public StringId SpawnDissolveType;
                public StringId SpawnDissolveMarker;
                // effect played on the turret while a biped or vehicle passes through it
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag CollisionPhaseEffect;
                
                [Flags]
                public enum EquipmentAutoTurretFlags : byte
                {
                    // if set, turret follows player, otherwise it moves to a specific location
                    TurretFollowsPlayer = 1 << 0,
                    // if set, turret will sink to a height defined in y orbit offset below
                    TurretFakeGravity = 1 << 1
                }
            }
            
            [TagStructure(Size = 0x2C)]
            public class EquipmentabilityTypeVisionModeBlock : TagStructure
            {
                public EquipmentvisionModeFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // 'Other activation tell effect' will be applied to players within this distance
                public float MaximumTellDistance; // wu
                // applied to other players within maximum distance
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag ActivationTellEffect;
                // within 'maxiumum tell distance' blip duration if 'Generate tell motion sensor blip' is checked
                public int MotionSensorTellBlipTicks;
                [TagField(ValidTags = new [] { "vmdx" })]
                public CachedTag VisionMode;
                
                [Flags]
                public enum EquipmentvisionModeFlags : byte
                {
                    ApplyTellDamageResponseToFriends = 1 << 0,
                    GenerateTellMotionSensorBlip = 1 << 1
                }
            }
            
            [TagStructure(Size = 0x50)]
            public class EquipmentabilityTypeShieldProjectorBlock : TagStructure
            {
                // Number of seconds a projected shield will remain active.
                public float Lifetime;
                // Number of seconds between shield projections.
                public float RechargeTime;
                // Number of seconds it takes to activate a projection.
                public float WarmupTime;
                // Percentage of distance between current and desired positions a shield can be moved per update tick.
                public float MaxMovePercentage;
                // Distance from defender's position to project the shield.
                public float OffsetAmount;
                // Minimum distance between defender and attacker required to project shield.
                public float MinProjectionDistance;
                // Max distance from equipment that shield can be projected.
                public float MaxProjectionDistance;
                // Min energy required to activate shield.
                public float MinRequiredEnergyLevel;
                // Beam effect that links equipment to projected shield.
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag ProjectEffect;
                // Effect played at eventual shield projection point during warmup period.
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag WarmupEffect;
                // Crate created to represent projected shield.
                [TagField(ValidTags = new [] { "bloc" })]
                public CachedTag ShieldCrate;
            }
            
            [TagStructure(Size = 0x40)]
            public class EquipmentabilityTypeProjectileCollectorBlock : TagStructure
            {
                // Percent chance (0-1) that collector has to collect each grenade.  Will only attempt to collect each grenade once.
                public float ChanceToCollect;
                // Number of seconds collector must wait between successful collections.
                public Bounds<float> CollectCooldown;
                // Number of seconds collector must wait before throwing a collected projectile at a target.
                public Bounds<float> AttackDelay;
                // Max range in world units that collector can collect and hold onto projectiles.
                public float MaxCollectRange;
                // Collector will try to keep collected projectiles orbiting in this range.
                public Bounds<float> OrbitRadius;
                // Speed at which collected projectiles orbit.
                public float OrbitSpeed;
                // Orbit vertical offset amount.
                public float VerticalOffset;
                // Controls amount of acceleration applied to projectile.
                public float Strength;
                // Initial speed of projectiles when thrown as an attack.
                public float AttackSpeed;
                // Beam effect that links equipment to each collected projectile.
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag CollectEffect;
            }
            
            [TagStructure(Size = 0x24)]
            public class EquipmentabilityTypeRemoteStrikeBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "obje" })]
                public CachedTag Weapon;
                public float MaxVelocity;
                // this is a percentage of max velocity per second - 0.1 reaches max velocity in 10 seconds
                public float Acceleration;
                // the rate at which a moving camera decays when you release the stick.  Lower decays faster.
                public float DecayRate;
                public float CameraInterpolationTime;
                public EquipmentRemoteStrikeFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum EquipmentRemoteStrikeFlags : byte
                {
                    // special version of RS use for ordnance droppod UI
                    OrdnanceUi = 1 << 0,
                    // target designator is in weapon field
                    TargetDesignator = 1 << 1
                }
            }
            
            [TagStructure(Size = 0x3C)]
            public class EquipmentabilityTypeEquipmentHackerBlock : TagStructure
            {
                public EquipmenthackerFlags Flags;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // Will be multiplied by a value from the drain level block.
                public float BaseDrainPerSecond;
                // The targeted equipment will not start regenerating energy until at least this much time has gone by.
                public float EnergyRecoveryDelay;
                // Degrees away from the reticle at which targets are in the cone.
                public float ConeAngleDegrees;
                public List<EquipmenthackerDrainLevel> DrainLevels;
                // an effect that will shoot out of your face
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag ConeEffect;
                // an effect that will play on the target
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag TargetEffect;
                
                [Flags]
                public enum EquipmenthackerFlags : byte
                {
                    DisableFriendlyFire = 1 << 0
                }
                
                [TagStructure(Size = 0x8)]
                public class EquipmenthackerDrainLevel : TagStructure
                {
                    // Targets within this distance will be affected by this multiplier.
                    public float CutoffDistance;
                    // Multiplied by the base drain per second.
                    public float DrainMultiplier;
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class EquipmentabilityTypeRemoteVehicleBlock : TagStructure
            {
                // reference the equipment that will spawn the hologram that will pilot the remote vehicle
                [TagField(ValidTags = new [] { "obje" })]
                public CachedTag HologramSpawner;
                // reference the remote vehicle to be created
                [TagField(ValidTags = new [] { "obje" })]
                public CachedTag Vehicle;
                // hologram gets loaded into this seat in the vehicle
                public StringId SeatLabel;
                // position of scenerio flag with this name. Empty will default to flag named 'remote_vehicle_start_position'
                public StringId SpawnPositionFlag;
            }
            
            [TagStructure(Size = 0x20)]
            public class EquipmentabilityTypeSuicideBombBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag ExplosionDamageEffect;
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag ExplosionEffect;
            }
            
            [TagStructure(Size = 0x1)]
            public class EquipmentabilityTypeActiveShieldBlock : TagStructure
            {
                public ActiveshieldFlags Flags;
                
                [Flags]
                public enum ActiveshieldFlags : byte
                {
                    // false forces full amount of stun following deactivation
                    PauseShieldStunTimer = 1 << 0
                }
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class EquipmentSoundRtpcblock : TagStructure
        {
            // Sound attachment to affect
            public int AttachmentIndex;
            // Function to drive the RTPC
            public StringId Function;
            // WWise RTPC string name
            public StringId RtpcName;
            public int RtpcNameHashValue;
        }
        
        [TagStructure(Size = 0x1C)]
        public class EquipmentSoundSweetenerBlock : TagStructure
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
