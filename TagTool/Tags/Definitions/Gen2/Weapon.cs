using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x31C)]
    public class Weapon : TagStructure
    {
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding;
        public FlagsValue Flags;
        public float BoundingRadius; // world units
        public RealPoint3d BoundingOffset;
        /// <summary>
        /// marine 1.0, grunt 1.4, elite 0.9, hunter 0.5, etc.
        /// </summary>
        public float AccelerationScale; // [0,+inf]
        public LightmapShadowModeValue LightmapShadowMode;
        public SweetenerSizeValue SweetenerSize;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding1;
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding2;
        /// <summary>
        /// sphere to use for dynamic lights and shadows. only used if not 0
        /// </summary>
        public float DynamicLightSphereRadius;
        /// <summary>
        /// only used if radius not 0
        /// </summary>
        public RealPoint3d DynamicLightSphereOffset;
        public StringId DefaultModelVariant;
        [TagField(ValidTags = new [] { "hlmt" })]
        public CachedTag Model;
        [TagField(ValidTags = new [] { "bloc" })]
        public CachedTag CrateObject;
        [TagField(ValidTags = new [] { "shad" })]
        public CachedTag ModifierShader;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag CreationEffect;
        [TagField(ValidTags = new [] { "foot" })]
        public CachedTag MaterialEffects;
        public List<ObjectAiPropertiesBlock> AiProperties;
        public List<ObjectFunctionBlock> Functions;
        /// <summary>
        /// for things that want to cause more or less collision damage
        /// </summary>
        /// <summary>
        /// 0 means 1.  1 is standard scale.  Some things may want to apply more damage
        /// </summary>
        public float ApplyCollisionDamageScale;
        /// <summary>
        /// 0 - means take default value from globals.globals
        /// </summary>
        /// <summary>
        /// 0-oo
        /// </summary>
        public float MinGameAccDefault;
        /// <summary>
        /// 0-oo
        /// </summary>
        public float MaxGameAccDefault;
        /// <summary>
        /// 0-1
        /// </summary>
        public float MinGameScaleDefault;
        /// <summary>
        /// 0-1
        /// </summary>
        public float MaxGameScaleDefault;
        /// <summary>
        /// 0 - means take default value from globals.globals
        /// </summary>
        /// <summary>
        /// 0-oo
        /// </summary>
        public float MinAbsAccDefault;
        /// <summary>
        /// 0-oo
        /// </summary>
        public float MaxAbsAccDefault;
        /// <summary>
        /// 0-1
        /// </summary>
        public float MinAbsScaleDefault;
        /// <summary>
        /// 0-1
        /// </summary>
        public float MaxAbsScaleDefault;
        public short HudTextMessageIndex;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding3;
        public List<ObjectAttachmentBlock> Attachments;
        public List<ObjectWidgetBlock> Widgets;
        public List<OldObjectFunctionBlock> OldFunctions;
        public List<ObjectChangeColors> ChangeColors;
        public List<PredictedResourceBlock> PredictedResources;
        public FlagsValue1 Flags1;
        public short OldMessageIndex;
        public short SortOrder;
        public float MultiplayerOnGroundScale;
        public float CampaignOnGroundScale;
        /// <summary>
        /// everything you need to display stuff
        /// </summary>
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
        [TagField(ValidTags = new [] { "foot" })]
        public CachedTag Unused;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag CollisionSound;
        public List<PredictedBitmapsBlock> PredictedBitmaps;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag DetonationDamageEffect;
        public Bounds<float> DetonationDelay; // seconds
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DetonatingEffect;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag DetonationEffect;
        /// <summary>
        /// All weapons should have 'primary trigger' and 'secondary trigger' markers as appropriate.
        /// Blurred permutations are called
        /// '$primary-blur' and '$secondary-blur'.
        /// </summary>
        public FlagsValue2 Flags2;
        public StringId Unknown;
        public SecondaryTriggerModeValue SecondaryTriggerMode;
        /// <summary>
        /// if the second trigger loads alternate ammunition, this is the maximum number of shots that can be loaded at a time
        /// </summary>
        public short MaximumAlternateShotsLoaded;
        /// <summary>
        /// how long after being readied it takes this weapon to switch its 'turned_on' attachment to 1.0
        /// </summary>
        public float TurnOnTime;
        public float ReadyTime; // seconds
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag ReadyEffect;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag ReadyDamageEffect;
        /// <summary>
        /// the heat value a weapon must return to before leaving the overheated state, once it has become overheated in the first
        /// place
        /// </summary>
        public float HeatRecoveryThreshold; // [0,1]
        /// <summary>
        /// the heat value over which a weapon first becomes overheated (should be greater than the heat recovery threshold)
        /// </summary>
        public float OverheatedThreshold; // [0,1]
        /// <summary>
        /// the heat value above which the weapon has a chance of exploding each time it is fired
        /// </summary>
        public float HeatDetonationThreshold; // [0,1]
        /// <summary>
        /// the percent chance (between 0.0 and 1.0) the weapon will explode when fired over the heat detonation threshold
        /// </summary>
        public float HeatDetonationFraction; // [0,1]
        /// <summary>
        /// the amount of heat lost each second when the weapon is not being fired
        /// </summary>
        public float HeatLossPerSecond; // [0,1]
        /// <summary>
        /// the amount of illumination given off when the weapon is overheated
        /// </summary>
        public float HeatIllumination; // [0,1]
        /// <summary>
        /// the amount of heat lost each second when the weapon is not being fired
        /// </summary>
        public float OverheatedHeatLossPerSecond; // [0,1]
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag Overheated;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag OverheatedDamageEffect;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag Detonation;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag DetonationDamageEffect1;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag PlayerMeleeDamage;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag PlayerMeleeResponse;
        /// <summary>
        /// magnetism angle: the maximum angle that magnetism works at full strength
        /// magnetism range: the maximum distance that
        /// magnetism works at full strength
        /// throttle magnitude: additional throttle to apply towards target when melee-ing w/
        /// magnetism
        /// throttle minimum distance: minimum distance to target that throttle magnetism kicks in
        /// throttle maximum
        /// adjustment angle: maximum angle throttle magnetism will have an effect, relative to the players movement throttle
        /// 
        /// </summary>
        public MeleeAimAssistStructBlock MeleeAimAssist;
        /// <summary>
        /// damage pyramid angles: defines the frustum from the camera that the melee-attack uses to find targets
        /// damage pyramid
        /// depth: how far the melee attack searches for a target
        /// </summary>
        public MeleeDamageParametersStructBlock MeleeDamageParameters;
        public MeleeDamageReportingTypeValue MeleeDamageReportingType;
        [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        /// <summary>
        /// the number of magnification levels this weapon allows
        /// </summary>
        public short MagnificationLevels;
        public Bounds<float> MagnificationRange;
        public AimAssistStructBlock WeaponAimAssist;
        public MovementPenalizedValue MovementPenalized;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        /// <summary>
        /// percent slowdown to forward movement for units carrying this weapon
        /// </summary>
        public float ForwardMovementPenalty;
        /// <summary>
        /// percent slowdown to sideways and backward movement for units carrying this weapon
        /// </summary>
        public float SidewaysMovementPenalty;
        public float AiScariness;
        public float WeaponPowerOnTime; // seconds
        public float WeaponPowerOffTime; // seconds
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag WeaponPowerOnEffect;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag WeaponPowerOffEffect;
        /// <summary>
        /// how much the weapon's heat recovery is penalized as it ages
        /// </summary>
        public float AgeHeatRecoveryPenalty;
        /// <summary>
        /// how much the weapon's rate of fire is penalized as it ages
        /// </summary>
        public float AgeRateOfFirePenalty;
        /// <summary>
        /// the age threshold when the weapon begins to misfire
        /// </summary>
        public float AgeMisfireStart; // [0,1]
        /// <summary>
        /// at age 1.0, the misfire chance per shot
        /// </summary>
        public float AgeMisfireChance; // [0,1]
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PickupSound;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ZoomInSound;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ZoomOutSound;
        /// <summary>
        /// how much to decrease active camo when a round is fired
        /// </summary>
        public float ActiveCamoDing;
        /// <summary>
        /// how fast to increase active camo (per tick) when a round is fired
        /// </summary>
        public float ActiveCamoRegrowthRate;
        /// <summary>
        /// the node that get's attached to the unit's hand
        /// </summary>
        public StringId HandleNode;
        public StringId WeaponClass;
        public StringId WeaponName;
        public MultiplayerWeaponTypeValue MultiplayerWeaponType;
        public WeaponTypeValue WeaponType;
        public WeaponTrackingStructBlock Tracking;
        public WeaponInterfaceStructBlock PlayerInterface;
        public List<PredictedResourceBlock1> PredictedResources1;
        public List<Magazine> Magazines;
        public List<WeaponTrigger> Triggers;
        public List<WeaponBarrels> Barrels;
        [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        public float MaxMovementAcceleration;
        public float MaxMovementVelocity;
        public float MaxTurningAcceleration;
        public float MaxTurningVelocity;
        [TagField(ValidTags = new [] { "vehi" })]
        public CachedTag DeployedVehicle;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag AgeEffect;
        [TagField(ValidTags = new [] { "weap" })]
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
            /// <summary>
            /// object passes all function values to parent and uses parent's markers
            /// </summary>
            ExtensionOfParent = 1 << 4,
            DoesNotCauseCollisionDamage = 1 << 5,
            EarlyMover = 1 << 6,
            EarlyMoverLocalizedPhysics = 1 << 7,
            /// <summary>
            /// cast a ton of rays once and store the results for lighting
            /// </summary>
            UseStaticMassiveLightmapSample = 1 << 8,
            ObjectScalesAttachments = 1 << 9,
            InheritsPlayerSAppearance = 1 << 10,
            DeadBipedsCanTLocalize = 1 << 11,
            /// <summary>
            /// use this for the mac gun on spacestation
            /// </summary>
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
        public class ObjectAiPropertiesBlock : TagStructure
        {
            public AiFlagsValue AiFlags;
            /// <summary>
            /// used for combat dialogue, etc.
            /// </summary>
            public StringId AiTypeName;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
        
        [TagStructure(Size = 0x20)]
        public class ObjectFunctionBlock : TagStructure
        {
            public FlagsValue Flags;
            public StringId ImportName;
            public StringId ExportName;
            /// <summary>
            /// if the specified function is off, so is this function
            /// </summary>
            public StringId TurnOffWith;
            /// <summary>
            /// function must exceed this value (after mapping) to be active 0. means do nothing
            /// </summary>
            public float MinValue;
            public MappingFunctionBlock DefaultFunction;
            public StringId ScaleBy;
            
            [Flags]
            public enum FlagsValue : uint
            {
                /// <summary>
                /// result of function is one minus actual result
                /// </summary>
                Invert = 1 << 0,
                /// <summary>
                /// the curve mapping can make the function active/inactive
                /// </summary>
                MappingDoesNotControlsActive = 1 << 1,
                /// <summary>
                /// function does not deactivate when at or below lower bound
                /// </summary>
                AlwaysActive = 1 << 2,
                /// <summary>
                /// function offsets periodic function input by random value between 0 and 1
                /// </summary>
                RandomTimeOffset = 1 << 3
            }
            
            [TagStructure(Size = 0x8)]
            public class MappingFunctionBlock : TagStructure
            {
                public List<ByteBlock> Data;
                
                [TagStructure(Size = 0x1)]
                public class ByteBlock : TagStructure
                {
                    public sbyte Value;
                }
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class ObjectAttachmentBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ligh","MGS2","tdtl","cont","effe","lsnd","lens" })]
            public CachedTag Type;
            public StringId Marker;
            public ChangeColorValue ChangeColor;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
        
        [TagStructure(Size = 0x8)]
        public class ObjectWidgetBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ant!","devo","whip","BooM","tdtl" })]
            public CachedTag Type;
        }
        
        [TagStructure(Size = 0x50)]
        public class OldObjectFunctionBlock : TagStructure
        {
            [TagField(Length = 0x4C, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public StringId Unknown;
        }
        
        [TagStructure(Size = 0x10)]
        public class ObjectChangeColors : TagStructure
        {
            public List<ObjectChangeColorInitialPermutation> InitialPermutations;
            public List<ObjectChangeColorFunction> Functions;
            
            [TagStructure(Size = 0x20)]
            public class ObjectChangeColorInitialPermutation : TagStructure
            {
                public float Weight;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                /// <summary>
                /// if empty, may be used by any model variant
                /// </summary>
                public StringId VariantName;
            }
            
            [TagStructure(Size = 0x28)]
            public class ObjectChangeColorFunction : TagStructure
            {
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public ScaleFlagsValue ScaleFlags;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
                public StringId DarkenBy;
                public StringId ScaleBy;
                
                [Flags]
                public enum ScaleFlagsValue : uint
                {
                    /// <summary>
                    /// blends colors in hsv rather than rgb space
                    /// </summary>
                    BlendInHsv = 1 << 0,
                    /// <summary>
                    /// blends colors through more hues (goes the long way around the color wheel)
                    /// </summary>
                    MoreColors = 1 << 1
                }
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResourceBlock : TagStructure
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
        
        [Flags]
        public enum FlagsValue1 : uint
        {
            AlwaysMaintainsZUp = 1 << 0,
            DestroyedByExplosions = 1 << 1,
            UnaffectedByGravity = 1 << 2
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedBitmapsBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "bitm" })]
            public CachedTag Bitmap;
        }
        
        [Flags]
        public enum FlagsValue2 : uint
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
            ObsoleteDoesNotDepowerActiveCamoInMultilplayer = 1 << 13,
            EnablesIntegratedNightVision = 1 << 14,
            AisUseWeaponMeleeDamage = 1 << 15,
            ForcesNoBinoculars = 1 << 16,
            LoopFpFiringAnimation = 1 << 17,
            PreventsSprinting = 1 << 18,
            CannotFireWhileBoosting = 1 << 19,
            PreventsDriving = 1 << 20,
            PreventsGunning = 1 << 21,
            CanBeDualWielded = 1 << 22,
            CanOnlyBeDualWielded = 1 << 23,
            MeleeOnly = 1 << 24,
            CantFireIfParentDead = 1 << 25,
            WeaponAgesWithEachKill = 1 << 26,
            WeaponUsesOldDualFireErrorCode = 1 << 27,
            PrimaryTriggerMeleeAttacks = 1 << 28,
            CannotBeUsedByPlayer = 1 << 29
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
        public class MeleeAimAssistStructBlock : TagStructure
        {
            /// <summary>
            /// the maximum angle that magnetism works at full strength
            /// </summary>
            public Angle MagnetismAngle; // degrees
            /// <summary>
            /// the maximum distance that magnetism works at full strength
            /// </summary>
            public float MagnetismRange; // world units
            public float ThrottleMagnitude;
            public float ThrottleMinimumDistance;
            public Angle ThrottleMaximumAdjustmentAngle; // degrees
        }
        
        [TagStructure(Size = 0x4C)]
        public class MeleeDamageParametersStructBlock : TagStructure
        {
            public RealEulerAngles2d DamagePyramidAngles;
            public float DamagePyramidDepth;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag _1stHitMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag _1stHitMeleeResponse;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag _2ndHitMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag _2ndHitMeleeResponse;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag _3rdHitMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag _3rdHitMeleeResponse;
            /// <summary>
            /// this is only important for the energy sword
            /// </summary>
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag LungeMeleeDamage;
            /// <summary>
            /// this is only important for the energy sword
            /// </summary>
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag LungeMeleeResponse;
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
        public class AimAssistStructBlock : TagStructure
        {
            /// <summary>
            /// the maximum angle that autoaim works at full strength
            /// </summary>
            public Angle AutoaimAngle; // degrees
            /// <summary>
            /// the maximum distance that autoaim works at full strength
            /// </summary>
            public float AutoaimRange; // world units
            /// <summary>
            /// the maximum angle that magnetism works at full strength
            /// </summary>
            public Angle MagnetismAngle; // degrees
            /// <summary>
            /// the maximum distance that magnetism works at full strength
            /// </summary>
            public float MagnetismRange; // world units
            /// <summary>
            /// the maximum angle that a projectile is allowed to deviate from the gun barrel
            /// </summary>
            public Angle DeviationAngle; // degrees
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
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
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            
            public enum TrackingTypeValue : short
            {
                NoTracking,
                HumanTracking,
                PlasmaTracking
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class WeaponInterfaceStructBlock : TagStructure
        {
            public WeaponSharedInterfaceStructBlock SharedInterface;
            public List<WeaponFirstPersonInterfaceBlock> FirstPerson;
            [TagField(ValidTags = new [] { "nhdt" })]
            public CachedTag NewHudInterface;
            
            [TagStructure(Size = 0x10)]
            public class WeaponSharedInterfaceStructBlock : TagStructure
            {
                [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x10)]
            public class WeaponFirstPersonInterfaceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "mode" })]
                public CachedTag FirstPersonModel;
                [TagField(ValidTags = new [] { "jmad" })]
                public CachedTag FirstPersonAnimations;
            }
        }
        
        [TagStructure(Size = 0x8)]
        public class PredictedResourceBlock1 : TagStructure
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
        
        [TagStructure(Size = 0x5C)]
        public class Magazine : TagStructure
        {
            public FlagsValue Flags;
            public short RoundsRecharged; // per second
            public short RoundsTotalInitial;
            public short RoundsTotalMaximum;
            public short RoundsLoadedMaximum;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// the length of time it takes to load a single magazine into the weapon
            /// </summary>
            public float ReloadTime; // seconds
            public short RoundsReloaded;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            /// <summary>
            /// the length of time it takes to chamber the next round
            /// </summary>
            public float ChamberTime; // seconds
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(ValidTags = new [] { "snd!","effe" })]
            public CachedTag ReloadingEffect;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag ReloadingDamageEffect;
            [TagField(ValidTags = new [] { "snd!","effe" })]
            public CachedTag ChamberingEffect;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag ChamberingDamageEffect;
            public List<MagazineObject> MagazineObjects;
            
            [Flags]
            public enum FlagsValue : uint
            {
                WastesRoundsWhenReloaded = 1 << 0,
                EveryRoundMustBeChambered = 1 << 1
            }
            
            [TagStructure(Size = 0xC)]
            public class MagazineObject : TagStructure
            {
                public short Rounds;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "eqip" })]
                public CachedTag Equipment;
            }
        }
        
        [TagStructure(Size = 0x40)]
        public class WeaponTrigger : TagStructure
        {
            public FlagsValue Flags;
            public InputValue Input;
            public BehaviorValue Behavior;
            public short PrimaryBarrel;
            public short SecondaryBarrel;
            public PredictionValue Prediction;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
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
            
            [TagStructure(Size = 0x24)]
            public class WeaponTriggerChargingStructBlock : TagStructure
            {
                /// <summary>
                /// the amount of time it takes for this trigger to become fully charged
                /// </summary>
                public float ChargingTime; // seconds
                /// <summary>
                /// the amount of time this trigger can be charged before becoming overcharged
                /// </summary>
                public float ChargedTime; // seconds
                public OverchargedActionValue OverchargedAction;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                /// <summary>
                /// the amount of illumination given off when the weapon is fully charged
                /// </summary>
                public float ChargedIllumination; // [0,1]
                /// <summary>
                /// length of time the weapon will spew (fire continuously) while discharging
                /// </summary>
                public float SpewTime; // seconds
                /// <summary>
                /// the charging effect is created once when the trigger begins to charge
                /// </summary>
                [TagField(ValidTags = new [] { "snd!","effe" })]
                public CachedTag ChargingEffect;
                /// <summary>
                /// the charging effect is created once when the trigger begins to charge
                /// </summary>
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag ChargingDamageEffect;
                
                public enum OverchargedActionValue : short
                {
                    None,
                    Explode,
                    Discharge
                }
            }
        }
        
        [TagStructure(Size = 0xEC)]
        public class WeaponBarrels : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// the number of firing effects created per second
            /// </summary>
            public Bounds<float> RoundsPerSecond;
            /// <summary>
            /// the continuous firing time it takes for the weapon to achieve its final rounds per second
            /// </summary>
            public float AccelerationTime; // seconds
            /// <summary>
            /// the continuous idle time it takes for the weapon to return from its final rounds per second to its initial
            /// </summary>
            public float DecelerationTime; // seconds
            /// <summary>
            /// scale the barrel spin speed by this amount
            /// </summary>
            public float BarrelSpinScale;
            /// <summary>
            /// a percentage between 0 and 1 which controls how soon in its firing animation the weapon blurs
            /// </summary>
            public float BlurredRateOfFire;
            /// <summary>
            /// allows designer caps to the shots you can fire from one firing action
            /// </summary>
            public Bounds<short> ShotsPerFire;
            /// <summary>
            /// how long after a set of shots it takes before the barrel can fire again
            /// </summary>
            public float FireRecoveryTime; // seconds
            /// <summary>
            /// how much of the recovery allows shots to be queued
            /// </summary>
            public float SoftRecoveryFraction;
            /// <summary>
            /// the magazine from which this trigger draws its ammunition
            /// </summary>
            public short Magazine;
            /// <summary>
            /// the number of rounds expended to create a single firing effect
            /// </summary>
            public short RoundsPerShot;
            /// <summary>
            /// the minimum number of rounds necessary to fire the weapon
            /// </summary>
            public short MinimumRoundsLoaded;
            /// <summary>
            /// the number of non-tracer rounds fired between tracers
            /// </summary>
            public short RoundsBetweenTracers;
            public StringId OptionalBarrelMarkerName;
            /// <summary>
            /// what the behavior of this barrel is in a predicted network game
            /// </summary>
            public PredictionTypeValue PredictionType;
            /// <summary>
            /// how loud this weapon appears to the AI
            /// </summary>
            public FiringNoiseValue FiringNoise;
            /// <summary>
            /// the continuous firing time it takes for the weapon to achieve its final error
            /// </summary>
            public float AccelerationTime1; // seconds
            /// <summary>
            /// the continuous idle time it takes for the weapon to return to its initial error
            /// </summary>
            public float DecelerationTime1; // seconds
            /// <summary>
            /// the range of angles (in degrees) that a damaged weapon will skew fire
            /// </summary>
            public Bounds<float> DamageError;
            /// <summary>
            /// the continuous firing time it takes for the weapon to achieve its final error
            /// </summary>
            public float AccelerationTime2; // seconds
            /// <summary>
            /// the continuous idle time it takes for the weapon to return to its initial error
            /// </summary>
            public float DecelerationTime2; // seconds
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public Angle MinimumError; // degrees
            public Bounds<Angle> ErrorAngle; // degrees
            public float DualWieldDamageScale;
            public DistributionFunctionValue DistributionFunction;
            public short ProjectilesPerShot;
            public float DistributionAngle; // degrees
            public Angle MinimumError1; // degrees
            public Bounds<Angle> ErrorAngle1; // degrees
            /// <summary>
            /// +x is forward, +z is up, +y is left
            /// </summary>
            public RealPoint3d FirstPersonOffset; // world units
            public DamageEffectReportingTypeValue DamageEffectReportingType;
            [TagField(Length = 0x3, Flags = TagFieldFlags.Padding)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "proj" })]
            public CachedTag Projectile;
            public WeaponBarrelDamageEffectStructBlock Eh;
            /// <summary>
            /// the amount of time (in seconds) it takes for the ejection port to transition from 1.0 (open) to 0.0 (closed) after a shot
            /// has been fired
            /// </summary>
            public float EjectionPortRecoveryTime;
            /// <summary>
            /// the amount of time (in seconds) it takes the illumination function to transition from 1.0 (bright) to 0.0 (dark) after a
            /// shot has been fired
            /// </summary>
            public float IlluminationRecoveryTime;
            /// <summary>
            /// the amount of heat generated each time the trigger is fired
            /// </summary>
            public float HeatGeneratedPerRound; // [0,1]
            /// <summary>
            /// the amount the weapon ages each time the trigger is fired
            /// </summary>
            public float AgeGeneratedPerRound; // [0,1]
            /// <summary>
            /// the next trigger fires this often while holding down this trigger
            /// </summary>
            public float OverloadTime; // seconds
            /// <summary>
            /// angle change per shot of the weapon during firing
            /// </summary>
            public Bounds<Angle> AngleChangePerShot;
            /// <summary>
            /// the continuous firing time it takes for the weapon to achieve its final angle change per shot
            /// </summary>
            public float AccelerationTime3; // seconds
            /// <summary>
            /// the continuous idle time it takes for the weapon to return to its initial angle change per shot
            /// </summary>
            public float DecelerationTime3; // seconds
            /// <summary>
            /// function used to scale between initial and final angle change per shot
            /// </summary>
            public AngleChangeFunctionValue AngleChangeFunction;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding2;
            [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
            public byte[] Padding3;
            [TagField(Length = 0x18, Flags = TagFieldFlags.Padding)]
            public byte[] Padding4;
            /// <summary>
            /// firing effects determine what happens when this trigger is fired
            /// </summary>
            public List<BarrelFiringEffectBlock> FiringEffects;
            
            [Flags]
            public enum FlagsValue : uint
            {
                /// <summary>
                /// poo poo ca ca pee pee
                /// </summary>
                TracksFiredProjectile = 1 << 0,
                /// <summary>
                /// rather than being chosen sequentially, firing effects are picked randomly
                /// </summary>
                RandomFiringEffects = 1 << 1,
                /// <summary>
                /// allows a weapon to be fired as long as there is a non-zero amount of ammunition loaded
                /// </summary>
                CanFireWithPartialAmmo = 1 << 2,
                /// <summary>
                /// instead of coming out of the magic first person camera origin, the projectiles for this weapon
                /// actually come out of the gun
                /// </summary>
                ProjectilesUseWeaponOrigin = 1 << 3,
                /// <summary>
                /// this trigger's ejection port is started during the key frame of its chamber animation
                /// </summary>
                EjectsDuringChamber = 1 << 4,
                UseErrorWhenUnzoomed = 1 << 5,
                /// <summary>
                /// projectiles fired by this weapon cannot have their direction adjusted by the AI to hit the target
                /// </summary>
                ProjectileVectorCannotBeAdjusted = 1 << 6,
                ProjectilesHaveIdenticalError = 1 << 7,
                /// <summary>
                /// If there are multiple guns for this trigger, the projectiles emerge in parallel beams (rather than
                /// independant aiming)
                /// </summary>
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
            
            [TagStructure(Size = 0x8)]
            public class WeaponBarrelDamageEffectStructBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "jpt!" })]
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
            
            [TagStructure(Size = 0x34)]
            public class BarrelFiringEffectBlock : TagStructure
            {
                /// <summary>
                /// the minimum number of times this firing effect will be used, once it has been chosen
                /// </summary>
                public short ShotCountLowerBound;
                /// <summary>
                /// the maximum number of times this firing effect will be used, once it has been chosen
                /// </summary>
                public short ShotCountUpperBound;
                /// <summary>
                /// this effect is used when the weapon is loaded and fired normally
                /// </summary>
                [TagField(ValidTags = new [] { "snd!","effe" })]
                public CachedTag FiringEffect;
                /// <summary>
                /// this effect is used when the weapon is loaded but fired while overheated
                /// </summary>
                [TagField(ValidTags = new [] { "snd!","effe" })]
                public CachedTag MisfireEffect;
                /// <summary>
                /// this effect is used when the weapon is not loaded
                /// </summary>
                [TagField(ValidTags = new [] { "snd!","effe" })]
                public CachedTag EmptyEffect;
                /// <summary>
                /// this effect is used when the weapon is loaded and fired normally
                /// </summary>
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag FiringDamage;
                /// <summary>
                /// this effect is used when the weapon is loaded but fired while overheated
                /// </summary>
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag MisfireDamage;
                /// <summary>
                /// this effect is used when the weapon is not loaded
                /// </summary>
                [TagField(ValidTags = new [] { "jpt!" })]
                public CachedTag EmptyDamage;
            }
        }
    }
}

