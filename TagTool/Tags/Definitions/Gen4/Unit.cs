using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "unit", Tag = "unit", Size = 0x40C)]
    public class Unit : GameObject
    {
        public UnitFlagsPart1 Flags;
        public UnitFlagsPart2 Flags2;
        public UnitDefaultTeams DefaultTeam;
        public AiSoundVolumeEnum ConstantSoundVolume;
        [TagField(ValidTags = new [] { "bipd","vehi" })]
        public CachedTag HologramUnitReference;
        public List<CampaignMetagameBucketBlock> CampaignMetagameBucket;
        public List<UnitScreenEffectBlock> ScreenEffects;
        public float CameraStiffness;
        public UnitCameraStruct UnitCamera;
        public UnitCameraStruct SyncActionCamera;
        [TagField(ValidTags = new [] { "drdf" })]
        public CachedTag AssasinationStartDamageResponse;
        [TagField(ValidTags = new [] { "weap" })]
        public CachedTag AssassinationWeapon;
        // the anchor we attach the knife to when we stow it
        public StringId AssassinationWeaponStowMarker;
        // the anchor we attach the knife to when we pull it out
        public StringId AssassinationWeaponOutMarker;
        // the marker on the knife that we anchor to the biped
        public StringId AssassinationWeaponAnchorMarker;
        [TagField(ValidTags = new [] { "sadt" })]
        public CachedTag SeatAcceleration;
        public float SoftPingThreshold; // [0,1]
        public float SoftPingInterruptTime; // seconds
        public float HardPingThreshold; // [0,1]
        public float HardPingInterruptTime; // seconds
        // moving faster than this means you will soft death in the movement direction. zero defaults to damage direction.
        public float SoftDeathDirectionSpeedThreshold; // wu/s
        public float HardDeathThreshold; // [0,1]
        public float FeignDeathThreshold; // [0,1]
        public float FeignDeathTime; // seconds
        // The duration of the pain function
        // 0 defaults to 0.5
        public float PainScreenDuration; // seconds|CCBBAA
        // The time it takes to fade out a damage region that is no longer the most recent damage region to be hit
        public float PainScreenRegionFadeOutDuration; // seconds|CCBBAA
        // The threshold weight below which the focus channel must fall before we can cross fade to another region.
        public float PainScreenRegionFadeOutWeightThreshold; // [0,1]
        // The tolerance angle between next and previous damage directions, below which we randomly vary the ping direction.
        public Angle PainScreenAngleTolerance; // degrees|CCBBAA
        // The maximum random angle to vary the incoming ping direction by if it's too close to the previous ping.
        public Angle PainScreenAngleRandomness; // degrees|CCBBAA
        // The duration of the defensive function
        // 0 defaults to 2.0
        public float DefensiveScreenDuration; // seconds
        // When receiving multiple pings, this is the min percentage of the defensive screen scrub value will fallback to.
        public float DefensiveScreenScrubFallbackFraction; // [0,1]
        // this must be set to tell the AI how far it should expect our dive animation to move us
        public float DistanceOfDiveAnim; // world units
        // ratio of airborne_arc animation to switch off falling overlay
        public float TerminalVelocityFallRatio;
        // 1.0 prevents moving while stunned
        public float StunMovementPenalty; // [0,1]
        // 1.0 prevents turning while stunned
        public float StunTurningPenalty; // [0,1]
        // 1.0 prevents jumping while stunned
        public float StunJumpingPenalty; // [0,1]
        // all stunning damage will last for at least this long
        public float MinimumStunTime; // seconds
        // no stunning damage will last for longer than this
        public float MaximumStunTime; // seconds
        public float FeignDeathChance; // [0,1]
        public float FeignRepeatChance; // [0,1]
        // automatically created character when this unit is driven
        [TagField(ValidTags = new [] { "char" })]
        public CachedTag SpawnedTurretCharacter;
        // number of actors which we spawn
        public Bounds<short> SpawnedActorCount;
        // velocity at which we throw spawned actors
        public float SpawnedVelocity;
        // set this to have your weapon barrel point at its calcualed target instead of matching the aiming of the unit
        // controlling it.  This marker should be along the barrel at point that doesn't move when the barrel pitches up and
        // down.
        public StringId TargetAimingPivotMarkerName;
        public Angle AimingVelocityMaximum; // degrees per second
        public Angle AimingAccelerationMaximum; // degrees per second squared
        public float CasualAimingModifier; // [0,1]
        public Angle LookingVelocityMaximum; // degrees per second
        public Angle LookingAccelerationMaximum; // degrees per second squared
        // Debug value for object velocity that corresponds to a blend screen weight of 1, 0 defaults to 5.0
        public float ObjectVelocityMaximum; // world units per second
        // where the primary weapon is attached
        public StringId RightHandNode;
        // where the seconday weapon is attached (for dual-pistol modes)
        public StringId LeftHandNode;
        public UnitAdditionalNodeNamesStruct MoreDamnNodes;
        public GlobalMeleeClassEnum MeleeDamageClass;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag MeleeDamage;
        // when set, characters will melee with these settings rather than their actual held weapon. (for characters that
        // melee with an off hand)
        [TagField(ValidTags = new [] { "weap" })]
        public CachedTag NativeMeleeOverride;
        public UnitBoardingMeleeStruct YourMomma;
        public GlobalChudBlipType MotionSensorBlipSize;
        public UnitItemOwnerSizeEnum ItemOwnerSize;
        public StringId EquipmentVariantName;
        public StringId GroundedEquipmentVariantName;
        public List<UnitPosturesBlock> Postures;
        public List<HudUnitSoundBlock> HudAudioCues;
        public List<DialogueVariantBlock> DialogueVariants;
        public float GrenadeAngle; // degrees
        public float GrenadeAngleMaxElevation; // degrees
        public float GrenadeAngleMinElevation; // degrees
        public float GrenadeVelocity; // world units per second
        public float GrenadeAngle1; // degrees
        public float GrenadeAngleMaxElevation1; // degrees
        public float GrenadeAngleMinElevation1; // degrees
        public float GrenadeVelocity1; // world units per second
        public float WeaponAngle; // degrees
        public float WeaponAngleMaxElevation; // degrees
        public float WeaponAngleMinElevation; // degrees
        public float WeaponVelocity; // world units per second
        public GlobalGrenadeTypeEnum GrenadeType;
        public short GrenadeCount;
        public List<PoweredSeatBlock> PoweredSeats;
        public List<UnitWeaponBlockStruct> Weapons;
        public List<GlobalTargetTrackingParametersBlock> TargetTracking;
        public List<UnitSeatBlock> Seats;
        // how long the unit takes to open when the hs_function unit_open is called
        // The current open state can be retrieved from the object function unit_open
        public float OpeningTime; // s
        // you don't have to go home, but you can't stay here
        public float ClosingTime; // s
        public float EmpDisabledTime; // seconds
        // Set to -1 for not disabled in MP but disabled in SP
        public float EmpDisabledTime1; // seconds 
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag EmpDisabledEffect;
        public UnitBoostStruct Boost;
        public UnitLipsyncScalesStruct Lipsync;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag ExitAndDetachDamage;
        [TagField(ValidTags = new [] { "weap" })]
        public CachedTag ExitAndDetachWeapon;
        public short ExperienceForKill;
        public short ExperienceForAssist;
        // this is where you stick an equipment that the biped will always have, to implement the bizarrely named hero assist
        [TagField(ValidTags = new [] { "eqip" })]
        public CachedTag HeroAssistEquipment;
        // the speed above which units will bail out of a vehicle instead of just exiting
        public float BailoutThreshold; // wu/s
        // when using iron sights, how much to scale the weapon overlays to steady the gun (0 = rock steady, 1= no dampening)
        public float IronSightWeaponDampening; // (0-1)
        public UnitBirthStruct Birth;
        
        [Flags]
        public enum UnitFlagsPart1 : uint
        {
            CircularAiming = 1 << 0,
            DestroyedAfterDying = 1 << 1,
            HalfSpeedInterpolation = 1 << 2,
            FiresFromCamera = 1 << 3,
            EntranceInsideBoundingSphere = 1 << 4,
            DoesnTShowReadiedWeapon = 1 << 5,
            CausesPassengerDialogue = 1 << 6,
            ResistsPings = 1 << 7,
            MeleeAttackIsFatal = 1 << 8,
            DonTRefaceDuringPings = 1 << 9,
            HasNoAiming = 1 << 10,
            ImpactMeleeAttachesToUnit = 1 << 11,
            ImpactMeleeDiesOnShields = 1 << 12,
            CannotOpenDoorsAutomatically = 1 << 13,
            MeleeAttackersCannotAttach = 1 << 14,
            NotInstantlyKilledByMelee = 1 << 15,
            FlashlightPowerDoesntTransferToWeapon = 1 << 16,
            RunsAroundFlaming = 1 << 17,
            TopLevelForAoeDamage = 1 << 18,
            SpecialCinematicUnit = 1 << 19,
            IgnoredByAutoaiming = 1 << 20,
            ShieldsFryInfectionForms = 1 << 21,
            UseVelocityAsAcceleration = 1 << 22,
            CanDualWield = 1 << 23,
            ActsAsGunnerForParent = 1 << 24,
            ControlledByParentGunner = 1 << 25,
            ParentSPrimaryWeapon = 1 << 26,
            ParentSSecondaryWeapon = 1 << 27,
            UnitHasBoost = 1 << 28,
            UnitHasVectoredThrust = 1 << 29,
            AllowAimWhileOpeningOrClosing = 1 << 30,
            ComputeAccelerationFromAiming = 1u << 31
        }
        
        [Flags]
        public enum UnitFlagsPart2 : uint
        {
            OverrideAllPings = 1 << 0,
            UnitSupportsBailout = 1 << 1,
            FlyingOrVehicleHardPingsAllowed = 1 << 2,
            // if this unit "fires from camera", this flag attempts to match the result, but have the projectile actually come out
            // of the gun
            AttemptToFireFromWeaponMatchingCamera = 1 << 3,
            // Treats a non-vehicle unit as a vehicle for gameplay purposes.  Mantis (biped) hack.
            TreatAsVehicle = 1 << 4,
            DroppedWeaponsCanDissolve = 1 << 5,
            HardPingsNotAllowedForDriverLessVehicle = 1 << 6,
            // Unit does not inflict collision damage to friendly units
            NoFriendlyBumpDamage = 1 << 7,
            // ignores hard pings forced by attachment of sticky grenade
            IgnoresAttachmentFeedbackForcedHardPings = 1 << 8,
            // do not try to find a sprite for this unit.  Just draw a dot.
            UnitAppearsOnRadarAsDotNotSprite = 1 << 9,
            SuppressRadarBlip = 1 << 10,
            // ai that are operating this unit will not ignore the unit's parents when doing line of sight tests (Mammoth turret
            // hack)
            DoNotIgnoreParentsForLineOfSightTests = 1 << 11,
            // projectiles attached to this object do not do attached damage to the object's children (regular aoe damage is still
            // done to top level aoe children)
            DoNotPassAttachedAoeDamageToChildren = 1 << 12,
            // even if this vehicle is being driven by a friendly character, don't generate a pill for it during ai line of fire
            // checks (for very large vehicles i.e. the lich)
            DoNotGenerateAiLineOfFirePillForUnit = 1 << 13
        }
        
        public enum UnitDefaultTeams : short
        {
            Default,
            Player,
            Human,
            Covenant,
            Brute,
            Mule,
            Spare,
            CovenantPlayer,
            Forerunner
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
        
        public enum GlobalMeleeClassEnum : short
        {
            DefaultClass,
            EliteClass
        }
        
        public enum GlobalChudBlipType : short
        {
            Medium,
            Small,
            Large
        }
        
        public enum UnitItemOwnerSizeEnum : short
        {
            Small,
            Medium,
            Large,
            Huge
        }
        
        public enum GlobalGrenadeTypeEnum : short
        {
            HumanFragmentation,
            CovenantPlasma,
            PulseGrenade,
            GrenadeType3,
            GrenadeType4,
            GrenadeType5,
            GrenadeType6,
            GrenadeType7
        }
        
        [TagStructure(Size = 0x8)]
        public class CampaignMetagameBucketBlock : TagStructure
        {
            public CampaignMetagameBucketFlags Flags;
            public CampaignMetagameBucketTypeEnum Type;
            public CampaignMetagameBucketClassEnum Class;
            [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public short PointCount;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            
            [Flags]
            public enum CampaignMetagameBucketFlags : byte
            {
                OnlyCountsWithRiders = 1 << 0
            }
            
            public enum CampaignMetagameBucketTypeEnum : sbyte
            {
                Brute,
                Grunt,
                Jackel,
                Skirmisher,
                Marine,
                Spartan,
                Bugger,
                Hunter,
                FloodInfection,
                FloodCarrier,
                FloodCombat,
                FloodPure,
                Sentinel,
                Elite,
                Engineer,
                Mule,
                Turret,
                Mongoose,
                Warthog,
                Scorpion,
                Hornet,
                Pelican,
                Revenant,
                Seraph,
                Shade,
                Watchtower,
                Ghost,
                Chopper,
                Mauler,
                Wraith,
                Banshee,
                Phantom,
                Scarab,
                Guntower,
                TuningFork,
                Broadsword,
                Mammoth,
                Lich,
                Mantis,
                Wasp,
                Phaeton,
                Bishop,
                Knight,
                Pawn
            }
            
            public enum CampaignMetagameBucketClassEnum : sbyte
            {
                Infantry,
                Leader,
                Hero,
                Specialist,
                LightVehicle,
                HeavyVehicle,
                GiantVehicle,
                StandardVehicle
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class UnitScreenEffectBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "sefc" })]
            public CachedTag ScreenEffect;
        }
        
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
        
        [TagStructure(Size = 0x14)]
        public class UnitAdditionalNodeNamesStruct : TagStructure
        {
            // if found, use this gun marker
            public StringId PreferredGunNode;
            // if found, use this marker to attach live grenades to
            public StringId PreferredGrenadeMarker;
            public List<WeaponSpecificMarkersBlock> WeaponSpecificMarkers;
            
            [TagStructure(Size = 0x14)]
            public class WeaponSpecificMarkersBlock : TagStructure
            {
                public StringId CompleteWeaponName;
                public StringId WeaponClass;
                public StringId WeaponName;
                public StringId RightHandMarker;
                public StringId LeftHandMarker;
            }
        }
        
        [TagStructure(Size = 0x90)]
        public class UnitBoardingMeleeStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag BoardingMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag BoardingMeleeResponse;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag EvictionMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag EvictionMeleeResponse;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag LandingMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag FlurryMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag ObstacleSmashDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag AssassinationPrimaryDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag AssassinationRagdollDamage;
        }
        
        [TagStructure(Size = 0x10)]
        public class UnitPosturesBlock : TagStructure
        {
            public StringId Name;
            public RealVector3d PillOffset;
        }
        
        [TagStructure(Size = 0x24)]
        public class HudUnitSoundBlock : TagStructure
        {
            public List<HudUnitSoundCueBlock> HudAudioCues;
            public float HealthMinor;
            public float HealthMajor;
            public float HealthCritical;
            public float ShieldMinor;
            public float ShieldMajor;
            public float ShieldCritical;
            
            [TagStructure(Size = 0x18)]
            public class HudUnitSoundCueBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "scmb","sndo","lsnd","snd!" })]
                public CachedTag Sound;
                public HudSoundCueFlags LatchedTo;
                public float Scale;
                
                [Flags]
                public enum HudSoundCueFlags : uint
                {
                    HealthRecharging = 1 << 0,
                    HealthMinorDamaged = 1 << 1,
                    HealthMajorDamaged = 1 << 2,
                    HealthCriticalDamaged = 1 << 3,
                    HealthMinor = 1 << 4,
                    HealthMajor = 1 << 5,
                    HealthCritical = 1 << 6,
                    ShieldRecharging = 1 << 7,
                    ShieldMinorDamaged = 1 << 8,
                    ShieldMajorDamaged = 1 << 9,
                    ShieldCriticalDamaged = 1 << 10,
                    ShieldMinor = 1 << 11,
                    ShieldMajor = 1 << 12,
                    ShieldCritical = 1 << 13,
                    PlayerTracked = 1 << 14,
                    PlayerLocked = 1 << 15
                }
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class DialogueVariantBlock : TagStructure
        {
            // variant number to use this dialogue with (must match the suffix in the permutations on the unit's model)
            public short VariantNumber;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "udlg" })]
            public CachedTag Dialogue;
        }
        
        [TagStructure(Size = 0x8)]
        public class PoweredSeatBlock : TagStructure
        {
            public float DriverPowerupTime; // seconds
            public float DriverPowerdownTime; // seconds
        }
        
        [TagStructure(Size = 0xA8)]
        public class UnitWeaponBlockStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Weapon;
            public StringId VariantName;
            public InitialWeaponPosition Position;
            // must be greater than zero for turret to fire
            public float MaximumFiringConeAngle; // degrees
            // minimum time before autoturret will change targets
            public float MinimumRetargetTime; // seconds
            // time after losing a target before turret stops blind firing
            public float BlindFireTime; // seconds
            // 0.0 = no lead, 1.0 = perfect lead
            public float LeadFraction;
            // when non-zero, turret will not prioritize targets above or below this range
            public Bounds<float> EngagementRange; // units
            // 0.0 = no importance, 2.0 = things that are close are twice as important
            public float ProximityScoreMultiplier;
            // 0.0 = no importance, 2.0 = things in front of you are twice as important
            public float DirectionScoreMultiplier;
            // 0.0 = no importance, 2.0 = things that attacked you last are twice as important
            public float AttackerScoreMultiplier;
            // multiplier
            public float TargetingWeightHologram;
            // multiplier
            public float TargetingWeightAutoTurret;
            public float PrimaryFireDelayFromIdle; // seconds
            public float SecondaryFireDelayFromIdle; // seconds
            // how long the turret stays alert after losing a target
            public float CautionDuration; // seconds
            // 0 = infinite
            public float AlertAngularSpeedMax; // radians per second
            // 0 = infinite
            public float IdleAngularSpeedMax; // radians per second
            // (-180 to 0) how far it can rotate past its initial rotation
            public float TargetingYawMin; // degrees
            // (0 to 180) how far it can rotate past its initial rotation
            public float TargetingYawMax; // degrees
            // (-180 to 0) how far it can rotate past its initial rotation
            public float TargetingPitchMin; // degrees
            // (0 to 180) how far it can rotate past its initial rotation
            public float TargetingPitchMax; // degrees
            // (-180 to 0) how far it will look around past its initial rotation
            public float IdleScanningYawMin; // degrees
            // (0 to 180) how far it will look around past its initial rotation
            public float IdleScanningYawMax; // degrees
            // (-180 to 0) how far it will look around past its initial rotation
            public float IdleScanningPitchMin; // degrees
            // (0 to 180) how far it will look around past its initial rotation
            public float IdleScanningPitchMax; // degrees
            // 0 = infinite.  Idle scanning won't look at something that is closer than this distance
            public float IdleScanningMinInterestDistance; // world units
            // effect played on the turret when it goes into alert mode
            [TagField(ValidTags = new [] { "effe" })]
            public CachedTag AlertModeEffect;
            public StringId AlertModeEffectMarker;
            public StringId AlertModeEffectPrimaryScale;
            public StringId AlertModeEffectSecondaryScale;
            public List<SentryPropertiesBlock> SentryProperties;
            // 0 - 1
            public float TargetCamouflageThreshold; // target when players camo level falls below this threshold, full camo = 1
            
            public enum InitialWeaponPosition : int
            {
                PrimaryOrBackpack,
                Secondary
            }
            
            [TagStructure(Size = 0x54)]
            public class SentryPropertiesBlock : TagStructure
            {
                public SentryturretBehaviorFlagDefinition Behavior;
                [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                // the cone that this sentry actually sees with; only used when scanning
                public float SightConeAngle; // degrees
                // how far the sentry can see (sentry will track enemies within alert range, but not necessarily fire)
                public float AlertRange; // world units
                // how far the sentry can shoot
                public float AttackRange; // world units
                // 0 = No preference to targets in attack range; 0.5 = Targets in attack range get 50% score bonus
                public float AttackRangeScoreMultiplier;
                // 0 = Default of 1; 1.5 = light vehicles are attacked at 1.5x attack range
                public float LightVehicleRangeScale;
                // 0 = Default of 1; 1.5 = light vehicles are attacked at 1.5x attack range
                public float HeavyVehicleRangeScale;
                // 0 = Default of 1; 1.5 = light vehicles are attacked at 1.5x attack range
                public float FlyingVehicleRangeScale;
                // 0 = No bonus preference for light vehicles; 0.5 = 50% score bonus for light vehicles
                public float LightVehicleScoreBonus;
                // 0 = No bonus preference for heavy vehicles; 0.5 = 50% score bonus for heavy vehicles
                public float HeavyVehicleScoreBonus;
                // 0 = No bonus preference for flying vehicles; 0.5 = 50% score bonus for flying vehicles
                public float FlyingVehicleScoreBonus;
                // how long the sentry waits before using its primary weapon barrel
                public float PrimaryFireTime; // seconds
                // how long the sentry waits before using its secondary weapon barrel
                public float SecondaryFireTime; // seconds
                // 1st person sound to play when targeted player enters sentry's alert range
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag PlayerEnteredAlertRangeSound;
                // 1st person sound to play when targeted player leaves sentry's alert range
                [TagField(ValidTags = new [] { "scmb","sndo","snd!" })]
                public CachedTag PlayerExitedAlertRangeSound;
                
                [Flags]
                public enum SentryturretBehaviorFlagDefinition : byte
                {
                    PerformsYawScanning = 1 << 0,
                    SecondaryBarrelStartsEnabled = 1 << 1,
                    // Can be used to prevent turret from using its big guns on a mongoose
                    SuppressSecondaryBarrelForLightVehicles = 1 << 2,
                    OnlyUseVehicleRangeScaleIfSecondaryBarrelActive = 1 << 3,
                    OnlyUseVehicleScoreBonusIfSecondaryBarrelActive = 1 << 4
                }
            }
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
        
        [TagStructure(Size = 0x16C)]
        public class UnitSeatBlock : TagStructure
        {
            public UnitSeatFlags Flags;
            public UnitSeatSecondaryFlags SecondaryFlags;
            public StringId Label;
            public StringId MarkerName;
            public StringId EntryMarker;
            public StringId UiMarkerName;
            public StringId UiNavpointName;
            public StringId BoardingGrenadeMarker;
            public StringId BoardingGrenadeString;
            public StringId BoardingMeleeString;
            public StringId InSeatString;
            // nathan is too lazy to make pings for each seat.
            public float PingScale;
            // how much time it takes to evict a rider from a flipped vehicle
            public float TurnoverTime; // seconds
            [TagField(ValidTags = new [] { "sadt" })]
            public CachedTag SeatAcceleration;
            public float AiScariness;
            public GlobalAiSeatTypeEnum AiSeatType;
            public short BoardingSeat;
            // additional seats to eject
            public List<BoardingSeatBlock> AdditionalBoardingSeats;
            // how far to interpolate listener position from camera to occupant's head
            public float ListenerInterpolationFactor;
            public Bounds<float> YawRateBounds; // degrees per second
            public Bounds<float> PitchRateBounds; // degrees per second
            // 0 means use default 17
            public float PitchInterpolationTime; // seconds to interpolate
            // Initial t is computed from velocity/(max speed - min speed)
            public float MinSpeedReference; // world units/sec
            public float MaxSpeedReference;
            // if >0, t is then modified by raising to this exponent and result is used to linearly interpolate yaw/pitch rates
            public float SpeedExponent;
            public UnitCameraStruct UnitCamera;
            [TagField(ValidTags = new [] { "cusc" })]
            public CachedTag HudScreenReference;
            public StringId EnterSeatString;
            public Angle YawMinimum;
            public Angle YawMaximum;
            // only applies when an NPC is considering using this seat
            public Angle YawMinimumForAiOperator;
            // only applies when an NPC is considering using this seat
            public Angle YawMaximumForAiOperator;
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag BuiltInGunner;
            // how close to the entry marker a unit must be
            public float EntryRadius;
            // angle from marker forward the unit must be
            public Angle EntryMarkerConeAngle;
            // angle from unit facing the marker must be
            public Angle EntryMarkerFacingAngle;
            public float MaximumRelativeVelocity;
            public float OpenTime; // seconds
            public float CloseTime; // seconds
            // creates an object function with this name that you can use to query the open state of this seat
            public StringId OpenFunctionName;
            // goes from 0 to 1 over the course of opening and stays at 1 while open.  Drops to 0 immediately when closing starts
            public StringId OpeningFunctionName;
            // goes from 0 to 1 over the course of closing and stays at 1 while closed.  Drops to 0 immediately when opening
            // starts
            public StringId ClosingFunctionName;
            public StringId InvisibleSeatRegion;
            public int RuntimeInvisibleSeatRegionIndex;
            [TagField(ValidTags = new [] { "bloc" })]
            public CachedTag SeatDeathGrabCrate;
            public StringId SeatSelectionString;
            // if exiting in bailout fashion, how much velocity to add in the entry_marker's forward direction
            public float BailoutVelocity; // wu/s
            
            [Flags]
            public enum UnitSeatFlags : uint
            {
                // completely enclosed by vehicle
                Invisible = 1 << 0,
                // when occupied, prevents enemies from entering locked seats
                Locked = 1 << 1,
                Driver = 1 << 2,
                Gunner = 1 << 3,
                ThirdPersonCamera = 1 << 4,
                AllowsWeapons = 1 << 5,
                ThirdPersonOnEnter = 1 << 6,
                FirstPersonCameraSlavedToGun = 1 << 7,
                NotValidWithoutDriver = 1 << 8,
                AllowAiNoncombatants = 1 << 9,
                BoardingSeat = 1 << 10,
                AiFiringDisabledByMaxAcceleration = 1 << 11,
                BoardingEntersSeat = 1 << 12,
                BoardingNeedAnyPassenger = 1 << 13,
                InvalidForPlayer = 1 << 14,
                InvalidForNonPlayer = 1 << 15,
                InvalidForHero = 1 << 16,
                Gunner1 = 1 << 17,
                InvisibleUnderMajorDamage = 1 << 18,
                MeleeInstantKillable = 1 << 19,
                LeaderPreference = 1 << 20,
                AllowsExitAndDetach = 1 << 21,
                BlocksHeadshots = 1 << 22,
                ExitsToGround = 1 << 23,
                ForwardFromAttachment = 1 << 24,
                DisallowAiShooting = 1 << 25,
                PreventsWeaponStowing = 1 << 26,
                TakesTopLevelAoeDamage = 1 << 27,
                // Prevents the unit currently in the seat from exiting regardless of circumstances
                DisallowExit = 1 << 28,
                LocalAiming = 1 << 29,
                PelvisRelativeAttachment = 1 << 30,
                ApplyVelocityOnDeathExit = 1u << 31
            }
            
            [Flags]
            public enum UnitSeatSecondaryFlags : uint
            {
                BipedGrabSeat = 1 << 0,
                LowHangingCargo = 1 << 1,
                NinjaHotSeat = 1 << 2,
                SkipObstacleCheck = 1 << 3,
                SearchParentForEntryMarker = 1 << 4,
                // The unit's aim will no longer be controlled by gunner when it exits
                GunnerReleaseAimOnExit = 1 << 5,
                FullyOpenBeforeAllowingExit = 1 << 6,
                FinishMeleeBeforeAllowingExit = 1 << 7,
                KillParentIfUnitInSeatDies = 1 << 8,
                // opens and closes the cockpit to allow copilot in (if already closed)
                CoPilot = 1 << 9
            }
            
            public enum GlobalAiSeatTypeEnum : short
            {
                None,
                Passenger,
                Gunner,
                SmallCargo,
                LargeCargo,
                Driver
            }
            
            [TagStructure(Size = 0x4)]
            public class BoardingSeatBlock : TagStructure
            {
                public short Seat;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x44)]
        public class UnitBoostStruct : TagStructure
        {
            [TagField(ValidTags = new [] { "cddf" })]
            public CachedTag BoostCollisionDamage;
            public BoostFlags Flags;
            public float BoostPeakPower;
            // if the trigger is fully down, takes this long to reach peak power
            public float BoostRiseTime; // s
            // if the trigger is let go (or peak time expires), takes this long to reach 0 power
            public float BoostFallTime; // s
            // 1, means you burn all your power in one sec.  .1 means you can boost for 10 seconds.
            public float BoostPowerPerSecond;
            public float BoostLowWarningThreshold;
            // 1 means you recharge fully in 1 second.  .1 means you rechage fully in 10 seconds
            public float RechargeRate;
            // how long do you have to be off the tirgger for before boost starts recharging
            public float RechargeDelay; // s
            public MappingFunction TriggerToBoost;
            
            [Flags]
            public enum BoostFlags : uint
            {
                PegsThrottle = 1 << 0
            }
            
            [TagStructure(Size = 0x14)]
            public class MappingFunction : TagStructure
            {
                public byte[] Data;
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class UnitLipsyncScalesStruct : TagStructure
        {
            public float AttackWeight;
            public float DecayWeight;
        }
        
        [TagStructure(Size = 0x8)]
        public class UnitBirthStruct : TagStructure
        {
            public short Seat;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            // if found, this region will be set to destroyed during birth
            public StringId BirthingRegion;
        }
    }
}
