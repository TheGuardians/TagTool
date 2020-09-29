using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x468)]
    public class Weapon : TagStructure
    {
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding1;
        public FlagsValue Flags;
        public float BoundingRadius; // world units
        public RealPoint3d BoundingOffset;
        public float AccelerationScale; // [0,+inf]
        public LightmapShadowModeValue LightmapShadowMode;
        public SweetenerSizeValue SweetenerSize;
        [TagField(Flags = Padding, Length = 1)]
        public byte[] Padding2;
        [TagField(Flags = Padding, Length = 4)]
        public byte[] Padding3;
        public float DynamicLightSphereRadius; // sphere to use for dynamic lights and shadows. only used if not 0
        public RealPoint3d DynamicLightSphereOffset; // only used if radius not 0
        public StringId DefaultModelVariant;
        public CachedTag Model;
        public CachedTag CrateObject;
        public CachedTag ModifierShader;
        public CachedTag CreationEffect;
        public CachedTag MaterialEffects;
        public List<ObjectAiProperties> AiProperties;
        public List<ObjectFunctionDefinition> Functions;
        /// <summary>
        /// Applying collision damage
        /// </summary>
        /// <remarks>
        /// for things that want to cause more or less collision damage
        /// </remarks>
        public float ApplyCollisionDamageScale; // 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        /// <summary>
        /// Game collision damage parameters
        /// </summary>
        /// <remarks>
        /// 0 - means take default value from globals.globals
        /// </remarks>
        public float MinGameAccDefault; // 0-oo
        public float MaxGameAccDefault; // 0-oo
        public float MinGameScaleDefault; // 0-1
        public float MaxGameScaleDefault; // 0-1
        /// <summary>
        /// Absolute collision damage parameters
        /// </summary>
        /// <remarks>
        /// 0 - means take default value from globals.globals
        /// </remarks>
        public float MinAbsAccDefault; // 0-oo
        public float MaxAbsAccDefault; // 0-oo
        public float MinAbsScaleDefault; // 0-1
        public float MaxAbsScaleDefault; // 0-1
        public short HudTextMessageIndex;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding4;
        public List<ObjectAttachmentDefinition> Attachments;
        public List<ObjectDefinitionWidget> Widgets;
        public List<OldObjectFunctionDefinition> OldFunctions;
        public List<ObjectChangeColorDefinition> ChangeColors;
        public List<PredictedResource> PredictedResources;
        /// <summary>
        /// $$$ ITEM $$$
        /// </summary>
        public FlagsValue Flags1;
        public short OldMessageIndex;
        public short SortOrder;
        public float MultiplayerOnGroundScale;
        public float CampaignOnGroundScale;
        /// <summary>
        /// NEW hud messages
        /// </summary>
        /// <remarks>
        /// everything you need to display stuff
        /// </remarks>
        public StringId PickupMessage;
        public StringId SwapMessage;
        public StringId PickupOrDualMsg;
        public StringId SwapOrDualMsg;
        public StringId DualOnlyMsg;
        public StringId PickedUpMsg;
        public StringId SingluarQuantityMsg;
        public StringId PluralQuantityMsg;
        public StringId SwitchToMsg;
        public StringId SwitchToFromAiMsg;
        public CachedTag Unused;
        public CachedTag CollisionSound;
        public List<TagReference> PredictedBitmaps;
        public CachedTag DetonationDamageEffect;
        public Bounds<float> DetonationDelay; // seconds
        public CachedTag DetonatingEffect;
        public CachedTag DetonationEffect;
        /// <summary>
        /// $$$ WEAPON $$$
        /// </summary>
        /// <remarks>
        /// All weapons should have 'primary trigger' and 'secondary trigger' markers as appropriate.
        /// Blurred permutations are called '$primary-blur' and '$secondary-blur'.
        /// </remarks>
        public FlagsValue Flags2;
        public StringId Unknown1;
        public SecondaryTriggerModeValue SecondaryTriggerMode;
        public short MaximumAlternateShotsLoaded; // if the second trigger loads alternate ammunition, this is the maximum number of shots that can be loaded at a time
        public float TurnOnTime; // how long after being readied it takes this weapon to switch its 'turned_on' attachment to 1.0
        /// <summary>
        /// old obsolete export to functions
        /// </summary>
        public float ReadyTime; // seconds
        public CachedTag ReadyEffect;
        public CachedTag ReadyDamageEffect;
        /// <summary>
        /// heat
        /// </summary>
        public float HeatRecoveryThreshold; // [0,1]
        public float OverheatedThreshold; // [0,1]
        public float HeatDetonationThreshold; // [0,1]
        public float HeatDetonationFraction; // [0,1]
        public float HeatLossPerSecond; // [0,1]
        public float HeatIllumination; // [0,1]
        public float OverheatedHeatLossPerSecond; // [0,1]
        public CachedTag Overheated;
        public CachedTag OverheatedDamageEffect;
        public CachedTag Detonation;
        public CachedTag DetonationDamageEffect3;
        public CachedTag PlayerMeleeDamage;
        public CachedTag PlayerMeleeResponse;
        /// <summary>
        /// melee aim assist
        /// </summary>
        /// <remarks>
        /// magnetism angle: the maximum angle that magnetism works at full strength
        /// magnetism range: the maximum distance that magnetism works at full strength
        /// throttle magnitude: additional throttle to apply towards target when melee-ing w/ magnetism
        /// throttle minimum distance: minimum distance to target that throttle magnetism kicks in
        /// throttle maximum adjustment angle: maximum angle throttle magnetism will have an effect, relative to the players movement throttle
        /// 
        /// </remarks>
        public MeleeAimAssistParameters MeleeAimAssist;
        /// <summary>
        /// melee damage parameters
        /// </summary>
        /// <remarks>
        /// damage pyramid angles: defines the frustum from the camera that the melee-attack uses to find targets
        /// damage pyramid depth: how far the melee attack searches for a target
        /// </remarks>
        public MeleeDamageParametersStruct MeleeDamageParameters;
        public MeleeDamageReportingTypeValue MeleeDamageReportingType;
        [TagField(Flags = Padding, Length = 1)]
        public byte[] Padding14;
        /// <summary>
        /// zoom
        /// </summary>
        public short MagnificationLevels; // the number of magnification levels this weapon allows
        public Bounds<float> MagnificationRange;
        /// <summary>
        /// weapon aim assist
        /// </summary>
        public AimAssistParameters WeaponAimAssist;
        /// <summary>
        /// movement
        /// </summary>
        public MovementPenalizedValue MovementPenalized;
        [TagField(Flags = Padding, Length = 2)]
        public byte[] Padding25;
        public float ForwardMovementPenalty; // percent slowdown to forward movement for units carrying this weapon
        public float SidewaysMovementPenalty; // percent slowdown to sideways and backward movement for units carrying this weapon
        /// <summary>
        /// AI targeting parameters
        /// </summary>
        public float AiScariness;
        /// <summary>
        /// miscellaneous
        /// </summary>
        public float WeaponPowerOnTime; // seconds
        public float WeaponPowerOffTime; // seconds
        public CachedTag WeaponPowerOnEffect;
        public CachedTag WeaponPowerOffEffect;
        public float AgeHeatRecoveryPenalty; // how much the weapon's heat recovery is penalized as it ages
        public float AgeRateOfFirePenalty; // how much the weapon's rate of fire is penalized as it ages
        public float AgeMisfireStart; // [0,1]
        public float AgeMisfireChance; // [0,1]
        public CachedTag PickupSound;
        public CachedTag ZoomInSound;
        public CachedTag ZoomOutSound;
        public float ActiveCamoDing; // how much to decrease active camo when a round is fired
        public float ActiveCamoRegrowthRate; // how fast to increase active camo (per tick) when a round is fired
        public StringId HandleNode; // the node that get's attached to the unit's hand
        /// <summary>
        /// weapon labels
        /// </summary>
        public StringId WeaponClass;
        public StringId WeaponName;
        public MultiplayerWeaponTypeValue MultiplayerWeaponType;
        /// <summary>
        /// more miscellaneous
        /// </summary>
        public WeaponTypeValue WeaponType;
        public WeaponTrackingStructBlock Tracking;
        public WeaponInterfaceDefinitionNew PlayerInterface;
        public List<PredictedResource> PredictedResources6;
        public List<WeaponMagazineDefinition> Magazines;
        public List<WeaponTriggerDefinition> NewTriggers;
        public List<WeaponBarrelDefinition> Barrels;
        [TagField(Flags = Padding, Length = 8)]
        public byte[] Padding37;
        /// <summary>
        /// first-person movement control
        /// </summary>
        public float MaxMovementAcceleration;
        public float MaxMovementVelocity;
        public float MaxTurningAcceleration;
        public float MaxTurningVelocity;
        public CachedTag DeployedVehicle;
        public CachedTag AgeEffect;
        public CachedTag AgedWeapon;
        public RealVector3d FirstPersonWeaponOffset;
        public RealVector2d FirstPersonScopeSize;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            DoesNotCastShadow = 1 << 0,
            SearchCardinalDirectionLightmapsOnFailure = 1 << 1,
            Unused = 1 << 2,
            NotAPathfindingObstacle = 1 << 3,
            ExtensionOfParent = 1 << 4,
            DoesNotCauseCollisionDamage = 1 << 5,
            EarlyMover = 1 << 6,
            EarlyMoverLocalizedPhysics = 1 << 7,
            UseStaticMassiveLightmapSample = 1 << 8,
            ObjectScalesAttachments = 1 << 9,
            InheritsPlayerSAppearance = 1 << 10,
            DeadBipedsCanTLocalize = 1 << 11,
            AttachToClustersByDynamicSphere = 1 << 12,
            EffectsCreatedByThisObjectDoNotSpawnObjectsInMultiplayer = 1 << 13
        }
        
        public enum LightmapShadowModeValue : short
        {
            Default,
            Never,
            Always
        }
        
        public enum SweetenerSizeValue : sbyte
        {
            Small,
            Medium,
            Large
        }
        
        [TagStructure(Size = 0x10)]
        public class ObjectAiProperties : TagStructure
        {
            public AiFlagsValue AiFlags;
            public StringId AiTypeName; // used for combat dialogue, etc.
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public AiSizeValue AiSize;
            public LeapJumpSpeedValue LeapJumpSpeed;
            
            [Flags]
            public enum AiFlagsValue : uint
            {
                DetroyableCover = 1 << 0,
                PathfindingIgnoreWhenDead = 1 << 1,
                DynamicCover = 1 << 2
            }
            
            public enum AiSizeValue : short
            {
                Default,
                Tiny,
                Small,
                Medium,
                Large,
                Huge,
                Immobile
            }
            
            public enum LeapJumpSpeedValue : short
            {
                None,
                Down,
                Step,
                Crouch,
                Stand,
                Storey,
                Tower,
                Infinite
            }
        }
        
        [TagStructure(Size = 0x24)]
        public class ObjectFunctionDefinition : TagStructure
        {
            public FlagsValue Flags;
            public StringId ImportName;
            public StringId ExportName;
            public StringId TurnOffWith; // if the specified function is off, so is this function
            public float MinValue; // function must exceed this value (after mapping) to be active 0. means do nothing
            public FunctionDefinition DefaultFunction;
            public StringId ScaleBy;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Invert = 1 << 0,
                MappingDoesNotControlsActive = 1 << 1,
                AlwaysActive = 1 << 2,
                RandomTimeOffset = 1 << 3
            }
            
            [TagStructure(Size = 0xC)]
            public class FunctionDefinition : TagStructure
            {
                public List<Byte> Data;
                
                [TagStructure(Size = 0x1)]
                public class Byte : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ObjectAttachmentDefinition : TagStructure
        {
            public CachedTag Type;
            public StringId Marker;
            public ChangeColorValue ChangeColor;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public StringId PrimaryScale;
            public StringId SecondaryScale;
            
            public enum ChangeColorValue : short
            {
                None,
                Primary,
                Secondary,
                Tertiary,
                Quaternary
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class ObjectDefinitionWidget : TagStructure
        {
            public CachedTag Type;
        }
        
        [TagStructure(Size = 0x50)]
        public class OldObjectFunctionDefinition : TagStructure
        {
            [TagField(Flags = Padding, Length = 76)]
            public byte[] Padding1;
            public StringId Unknown1;
        }
        
        [TagStructure(Size = 0x18)]
        public class ObjectChangeColorDefinition : TagStructure
        {
            public List<ObjectChangeColorInitialPermutation> InitialPermutations;
            public List<ObjectChangeColorFunction> Functions;
            
            [TagStructure(Size = 0x20)]
            public class ObjectChangeColorInitialPermutation : TagStructure
            {
                public float Weight;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId VariantName; // if empty, may be used by any model variant
            }
            
            [TagStructure(Size = 0x28)]
            public class ObjectChangeColorFunction : TagStructure
            {
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding1;
                public ScaleFlagsValue ScaleFlags;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId DarkenBy;
                public StringId ScaleBy;
                
                [Flags]
                public enum ScaleFlagsValue : uint
                {
                    BlendInHsv = 1 << 0,
                    MoreColors = 1 << 1
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResource : TagStructure
        {
            public TypeValue Type;
            public short ResourceIndex;
            public int TagIndex;
            
            public enum TypeValue : short
            {
                Bitmap,
                Sound,
                RenderModelGeometry,
                ClusterGeometry,
                ClusterInstancedGeometry,
                LightmapGeometryObjectBuckets,
                LightmapGeometryInstanceBuckets,
                LightmapClusterBitmaps,
                LightmapInstanceBitmaps
            }
        }
        
        [TagStructure(Size = 0x10)]
        public class TagReference : TagStructure
        {
            public CachedTag Bitmap;
        }
        
        public enum SecondaryTriggerModeValue : short
        {
            Normal,
            SlavedToPrimary,
            InhibitsPrimary,
            LoadsAlterateAmmunition,
            LoadsMultiplePrimaryAmmunition
        }
        
        [TagStructure(Size = 0x14)]
        public class MeleeAimAssistParameters : TagStructure
        {
            public Angle MagnetismAngle; // degrees
            public float MagnetismRange; // world units
            public float ThrottleMagnitude;
            public float ThrottleMinimumDistance;
            public Angle ThrottleMaximumAdjustmentAngle; // degrees
        }
        
        [TagStructure(Size = 0x8C)]
        public class MeleeDamageParametersStruct : TagStructure
        {
            public RealEulerAngles2d DamagePyramidAngles;
            public float DamagePyramidDepth;
            /// <summary>
            /// melee combo damage
            /// </summary>
            public CachedTag _1stHitMeleeDamage;
            public CachedTag _1stHitMeleeResponse;
            public CachedTag _2ndHitMeleeDamage;
            public CachedTag _2ndHitMeleeResponse;
            public CachedTag _3rdHitMeleeDamage;
            public CachedTag _3rdHitMeleeResponse;
            public CachedTag LungeMeleeDamage; // this is only important for the energy sword
            public CachedTag LungeMeleeResponse; // this is only important for the energy sword
        }
        
        public enum MeleeDamageReportingTypeValue : sbyte
        {
            TehGuardians11,
            FallingDamage,
            GenericCollisionDamage,
            GenericMeleeDamage,
            GenericExplosion,
            MagnumPistol,
            PlasmaPistol,
            Needler,
            Smg,
            PlasmaRifle,
            BattleRifle,
            Carbine,
            Shotgun,
            SniperRifle,
            BeamRifle,
            RocketLauncher,
            FlakCannon,
            BruteShot,
            Disintegrator,
            BrutePlasmaRifle,
            EnergySword,
            FragGrenade,
            PlasmaGrenade,
            FlagMeleeDamage,
            BombMeleeDamage,
            BombExplosionDamage,
            BallMeleeDamage,
            HumanTurret,
            PlasmaTurret,
            Banshee,
            Ghost,
            Mongoose,
            Scorpion,
            SpectreDriver,
            SpectreGunner,
            WarthogDriver,
            WarthogGunner,
            Wraith,
            Tank,
            SentinelBeam,
            SentinelRpg,
            Teleporter
        }
        
        [TagStructure(Size = 0x24)]
        public class AimAssistParameters : TagStructure
        {
            public Angle AutoaimAngle; // degrees
            public float AutoaimRange; // world units
            public Angle MagnetismAngle; // degrees
            public float MagnetismRange; // world units
            public Angle DeviationAngle; // degrees
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 12)]
            public byte[] Padding2;
        }
        
        public enum MovementPenalizedValue : short
        {
            Always,
            WhenZoomed,
            WhenZoomedOrReloading
        }
        
        public enum MultiplayerWeaponTypeValue : short
        {
            None,
            CtfFlag,
            OddballBall,
            HeadhunterHead,
            JuggernautPowerup
        }
        
        public enum WeaponTypeValue : short
        {
            Undefined,
            Shotgun,
            Needler,
            PlasmaPistol,
            PlasmaRifle,
            RocketLauncher
        }
        
        [TagStructure(Size = 0x4)]
        public class WeaponTrackingStructBlock : TagStructure
        {
            public TrackingTypeValue TrackingType;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            
            public enum TrackingTypeValue : short
            {
                NoTracking,
                HumanTracking,
                PlasmaTracking
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class WeaponInterfaceDefinitionNew : TagStructure
        {
            /// <summary>
            /// interface
            /// </summary>
            public TagReference SharedInterface;
            public List<WeaponFirstPersonInterfaceDefinition> FirstPerson;
            public CachedTag NewHudInterface;
            
            [TagStructure(Size = 0x10)]
            public class TagReference : TagStructure
            {
                [TagField(Flags = Padding, Length = 16)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0x20)]
            public class WeaponFirstPersonInterfaceDefinition : TagStructure
            {
                public CachedTag FirstPersonModel;
                public CachedTag FirstPersonAnimations;
            }
        }
        
        [TagStructure(Size = 0x80)]
        public class WeaponMagazineDefinition : TagStructure
        {
            public FlagsValue Flags;
            public short RoundsRecharged; // per second
            public short RoundsTotalInitial;
            public short RoundsTotalMaximum;
            public short RoundsLoadedMaximum;
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public float ReloadTime; // seconds
            public short RoundsReloaded;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding2;
            public float ChamberTime; // seconds
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding3;
            [TagField(Flags = Padding, Length = 16)]
            public byte[] Padding4;
            public CachedTag ReloadingEffect;
            public CachedTag ReloadingDamageEffect;
            public CachedTag ChamberingEffect;
            public CachedTag ChamberingDamageEffect;
            public List<WeaponAmmunitionObject> Magazines;
            
            [Flags]
            public enum FlagsValue : uint
            {
                WastesRoundsWhenReloaded = 1 << 0,
                EveryRoundMustBeChambered = 1 << 1
            }
            
            [TagStructure(Size = 0x14)]
            public class WeaponAmmunitionObject : TagStructure
            {
                public short Rounds;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public CachedTag Equipment;
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class WeaponTriggerDefinition : TagStructure
        {
            public FlagsValue Flags;
            public InputValue Input;
            public BehaviorValue Behavior;
            public short PrimaryBarrel;
            public short SecondaryBarrel;
            public PredictionValue Prediction;
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding1;
            public WeaponTriggerAutofireStructBlock Autofire;
            public WeaponTriggerChargingStructBlock Charging;
            
            [Flags]
            public enum FlagsValue : uint
            {
                AutofireSingleActionOnly = 1 << 0
            }
            
            public enum InputValue : short
            {
                RightTrigger,
                LeftTrigger,
                MeleeAttack
            }
            
            public enum BehaviorValue : short
            {
                Spew,
                Latch,
                LatchAutofire,
                Charge,
                LatchZoom,
                LatchRocketlauncher
            }
            
            public enum PredictionValue : short
            {
                None,
                Spew,
                Charge
            }
            
            [TagStructure(Size = 0xC)]
            public class WeaponTriggerAutofireStructBlock : TagStructure
            {
                /// <summary>
                /// AUTOFIRE
                /// </summary>
                public float AutofireTime;
                public float AutofireThrow;
                public SecondaryActionValue SecondaryAction;
                public PrimaryActionValue PrimaryAction;
                
                public enum SecondaryActionValue : short
                {
                    Fire,
                    Charge,
                    Track,
                    FireOther
                }
                
                public enum PrimaryActionValue : short
                {
                    Fire,
                    Charge,
                    Track,
                    FireOther
                }
            }
            
            [TagStructure(Size = 0x34)]
            public class WeaponTriggerChargingStructBlock : TagStructure
            {
                /// <summary>
                /// CHARGING
                /// </summary>
                public float ChargingTime; // seconds
                public float ChargedTime; // seconds
                public OverchargedActionValue OverchargedAction;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public float ChargedIllumination; // [0,1]
                public float SpewTime; // seconds
                public CachedTag ChargingEffect; // the charging effect is created once when the trigger begins to charge
                public CachedTag ChargingDamageEffect; // the charging effect is created once when the trigger begins to charge
                
                public enum OverchargedActionValue : short
                {
                    None,
                    Explode,
                    Discharge
                }
            }
        }
        
        [TagStructure(Size = 0x100)]
        public class WeaponBarrelDefinition : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// firing
            /// </summary>
            public Bounds<float> RoundsPerSecond; // the number of firing effects created per second
            public float AccelerationTime; // seconds
            public float DecelerationTime; // seconds
            public float BarrelSpinScale; // scale the barrel spin speed by this amount
            public float BlurredRateOfFire; // a percentage between 0 and 1 which controls how soon in its firing animation the weapon blurs
            public Bounds<short> ShotsPerFire; // allows designer caps to the shots you can fire from one firing action
            public float FireRecoveryTime; // seconds
            public float SoftRecoveryFraction; // how much of the recovery allows shots to be queued
            public short Magazine; // the magazine from which this trigger draws its ammunition
            public short RoundsPerShot; // the number of rounds expended to create a single firing effect
            public short MinimumRoundsLoaded; // the minimum number of rounds necessary to fire the weapon
            public short RoundsBetweenTracers; // the number of non-tracer rounds fired between tracers
            public StringId OptionalBarrelMarkerName;
            /// <summary>
            /// prediction properties
            /// </summary>
            /// <remarks>
            /// what the behavior of this barrel is in a predicted network game
            /// </remarks>
            public PredictionTypeValue PredictionType;
            public FiringNoiseValue FiringNoise; // how loud this weapon appears to the AI
            /// <summary>
            /// error
            /// </summary>
            public float AccelerationTime1; // seconds
            public float DecelerationTime2; // seconds
            public Bounds<float> DamageError; // the range of angles (in degrees) that a damaged weapon will skew fire
            /// <summary>
            /// dual weapon error
            /// </summary>
            public float AccelerationTime3; // seconds
            public float DecelerationTime4; // seconds
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding1;
            public Angle MinimumError; // degrees
            public Bounds<Angle> ErrorAngle; // degrees
            public float DualWieldDamageScale;
            /// <summary>
            /// projectile
            /// </summary>
            public DistributionFunctionValue DistributionFunction;
            public short ProjectilesPerShot;
            public float DistributionAngle; // degrees
            public Angle MinimumError5; // degrees
            public Bounds<Angle> ErrorAngle6; // degrees
            public RealPoint3d FirstPersonOffset; // world units
            public DamageEffectReportingTypeValue DamageEffectReportingType;
            [TagField(Flags = Padding, Length = 3)]
            public byte[] Padding2;
            public CachedTag Projectile;
            public TagReference Eh;
            /// <summary>
            /// misc
            /// </summary>
            public float EjectionPortRecoveryTime; // the amount of time (in seconds) it takes for the ejection port to transition from 1.0 (open) to 0.0 (closed) after a shot has been fired
            public float IlluminationRecoveryTime; // the amount of time (in seconds) it takes the illumination function to transition from 1.0 (bright) to 0.0 (dark) after a shot has been fired
            public float HeatGeneratedPerRound; // [0,1]
            public float AgeGeneratedPerRound; // [0,1]
            public float OverloadTime; // seconds
            /// <summary>
            /// angle change (recoil)
            /// </summary>
            public Bounds<Angle> AngleChangePerShot; // angle change per shot of the weapon during firing
            public float AccelerationTime7; // seconds
            public float DecelerationTime8; // seconds
            public AngleChangeFunctionValue AngleChangeFunction; // function used to scale between initial and final angle change per shot
            [TagField(Flags = Padding, Length = 2)]
            public byte[] Padding3;
            [TagField(Flags = Padding, Length = 8)]
            public byte[] Padding4;
            [TagField(Flags = Padding, Length = 24)]
            public byte[] Padding5;
            public List<BarrelFiringEffect> FiringEffects; // firing effects determine what happens when this trigger is fired
            
            [Flags]
            public enum FlagsValue : uint
            {
                TracksFiredProjectile = 1 << 0,
                RandomFiringEffects = 1 << 1,
                CanFireWithPartialAmmo = 1 << 2,
                ProjectilesUseWeaponOrigin = 1 << 3,
                EjectsDuringChamber = 1 << 4,
                UseErrorWhenUnzoomed = 1 << 5,
                ProjectileVectorCannotBeAdjusted = 1 << 6,
                ProjectilesHaveIdenticalError = 1 << 7,
                ProjectilesFireParallel = 1 << 8,
                CantFireWhenOthersFiring = 1 << 9,
                CantFireWhenOthersRecovering = 1 << 10,
                DonTClearFireBitAfterRecovering = 1 << 11,
                StaggerFireAcrossMultipleMarkers = 1 << 12,
                FiresLockedProjectiles = 1 << 13
            }
            
            public enum PredictionTypeValue : short
            {
                None,
                Continuous,
                Instant
            }
            
            public enum FiringNoiseValue : short
            {
                Silent,
                Medium,
                Loud,
                Shout,
                Quiet
            }
            
            public enum DistributionFunctionValue : short
            {
                Point,
                HorizontalFan
            }
            
            public enum DamageEffectReportingTypeValue : sbyte
            {
                TehGuardians11,
                FallingDamage,
                GenericCollisionDamage,
                GenericMeleeDamage,
                GenericExplosion,
                MagnumPistol,
                PlasmaPistol,
                Needler,
                Smg,
                PlasmaRifle,
                BattleRifle,
                Carbine,
                Shotgun,
                SniperRifle,
                BeamRifle,
                RocketLauncher,
                FlakCannon,
                BruteShot,
                Disintegrator,
                BrutePlasmaRifle,
                EnergySword,
                FragGrenade,
                PlasmaGrenade,
                FlagMeleeDamage,
                BombMeleeDamage,
                BombExplosionDamage,
                BallMeleeDamage,
                HumanTurret,
                PlasmaTurret,
                Banshee,
                Ghost,
                Mongoose,
                Scorpion,
                SpectreDriver,
                SpectreGunner,
                WarthogDriver,
                WarthogGunner,
                Wraith,
                Tank,
                SentinelBeam,
                SentinelRpg,
                Teleporter
            }
            
            [TagStructure(Size = 0x10)]
            public class TagReference : TagStructure
            {
                public CachedTag DamageEffect;
            }
            
            public enum AngleChangeFunctionValue : short
            {
                Linear,
                Early,
                VeryEarly,
                Late,
                VeryLate,
                Cosine,
                One,
                Zero
            }
            
            [TagStructure(Size = 0x64)]
            public class BarrelFiringEffect : TagStructure
            {
                public short ShotCountLowerBound; // the minimum number of times this firing effect will be used, once it has been chosen
                public short ShotCountUpperBound; // the maximum number of times this firing effect will be used, once it has been chosen
                public CachedTag FiringEffect; // this effect is used when the weapon is loaded and fired normally
                public CachedTag MisfireEffect; // this effect is used when the weapon is loaded but fired while overheated
                public CachedTag EmptyEffect; // this effect is used when the weapon is not loaded
                public CachedTag FiringDamage; // this effect is used when the weapon is loaded and fired normally
                public CachedTag MisfireDamage; // this effect is used when the weapon is loaded but fired while overheated
                public CachedTag EmptyDamage; // this effect is used when the weapon is not loaded
            }
        }
    }
}

