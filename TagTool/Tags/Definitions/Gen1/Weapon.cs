using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "weapon", Tag = "weap", Size = 0x508)]
    public class Weapon : TagStructure
    {
        [TagField(Length = 0x2)]
        public byte[] Padding;
        public FlagsValue Flags;
        public float BoundingRadius; // world units
        public RealPoint3d BoundingOffset;
        public RealPoint3d OriginOffset;
        /// <summary>
        /// marine 1.0, grunt 1.4, elite 0.9, hunter 0.5, etc.
        /// </summary>
        public float AccelerationScale; // [0,+inf]
        [TagField(Length = 0x4)]
        public byte[] Padding1;
        [TagField(ValidTags = new [] { "mod2" })]
        public CachedTag Model;
        [TagField(ValidTags = new [] { "antr" })]
        public CachedTag AnimationGraph;
        [TagField(Length = 0x28)]
        public byte[] Padding2;
        [TagField(ValidTags = new [] { "coll" })]
        public CachedTag CollisionModel;
        [TagField(ValidTags = new [] { "phys" })]
        public CachedTag Physics;
        [TagField(ValidTags = new [] { "shdr" })]
        public CachedTag ModifierShader;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag CreationEffect;
        [TagField(Length = 0x54)]
        public byte[] Padding3;
        /// <summary>
        /// if set, this radius is used to determine if the object is visible. set it for the pelican.
        /// </summary>
        public float RenderBoundingRadius; // world units
        public AInValue AIn;
        public BInValue BIn;
        public CInValue CIn;
        public DInValue DIn;
        [TagField(Length = 0x2C)]
        public byte[] Padding4;
        public short HudTextMessageIndex;
        public short ForcedShaderPermuationIndex;
        public List<ObjectAttachmentBlock> Attachments;
        public List<ObjectWidgetBlock> Widgets;
        public List<ObjectFunctionBlock> Functions;
        public List<ObjectChangeColors> ChangeColors;
        public List<PredictedResourceBlock> PredictedResources;
        public Flags1Value Flags1;
        /// <summary>
        /// This sets which string from tags\ui\hud\hud_item_messages.unicode_string_list to display.
        /// </summary>
        public short MessageIndex;
        public short SortOrder;
        public float Scale;
        public short HudMessageValueScale;
        [TagField(Length = 0x2)]
        public byte[] Padding5;
        [TagField(Length = 0x10)]
        public byte[] Padding6;
        public AIn1Value AIn1;
        public BIn1Value BIn1;
        public CIn1Value CIn1;
        public DIn1Value DIn1;
        [TagField(Length = 0xA4)]
        public byte[] Padding7;
        [TagField(ValidTags = new [] { "foot" })]
        public CachedTag MaterialEffects;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag CollisionSound;
        [TagField(Length = 0x78)]
        public byte[] Padding8;
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
        public Flags2Value Flags2;
        /// <summary>
        /// the string used to match this weapon to an animation mode in the unit carrying it
        /// </summary>
        [TagField(Length = 32)]
        public string Label;
        public SecondaryTriggerModeValue SecondaryTriggerMode;
        /// <summary>
        /// if the second trigger loads alternate ammunition, this is the maximum number of shots that can be loaded at a time
        /// </summary>
        public short MaximumAlternateShotsLoaded;
        public AIn2Value AIn2;
        public BIn2Value BIn2;
        public CIn2Value CIn2;
        public DIn2Value DIn2;
        public float ReadyTime; // seconds
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag ReadyEffect;
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
        [TagField(Length = 0x10)]
        public byte[] Padding9;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag Overheated;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag Detonation;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag PlayerMeleeDamage;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag PlayerMeleeResponse;
        [TagField(Length = 0x8)]
        public byte[] Padding10;
        /// <summary>
        /// an optional actor variant that specifies the burst geometry and firing patterns to be used with this weapon
        /// </summary>
        [TagField(ValidTags = new [] { "actv" })]
        public CachedTag ActorFiringParameters;
        /// <summary>
        /// the range at which the closer of the two static target reticles will be drawn
        /// </summary>
        public float NearReticleRange; // world units
        /// <summary>
        /// the range at which the farther of the two static target reticles will be drawn
        /// </summary>
        public float FarReticleRange; // world units
        /// <summary>
        /// the maximum range at which the dynamic target reticle will be drawn
        /// </summary>
        public float IntersectionReticleRange;
        [TagField(Length = 0x2)]
        public byte[] Padding11;
        /// <summary>
        /// the number of magnification levels this weapon allows
        /// </summary>
        public short MagnificationLevels;
        public Bounds<float> MagnificationRange;
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
        [TagField(Length = 0x4)]
        public byte[] Padding12;
        public MovementPenalizedValue MovementPenalized;
        [TagField(Length = 0x2)]
        public byte[] Padding13;
        /// <summary>
        /// percent slowdown to forward movement for units carrying this weapon
        /// </summary>
        public float ForwardMovementPenalty;
        /// <summary>
        /// percent slowdown to sideways and backward movement for units carrying this weapon
        /// </summary>
        public float SidewaysMovementPenalty;
        [TagField(Length = 0x4)]
        public byte[] Padding14;
        /// <summary>
        /// minimum range that actors using this weapon will try and target stuff at
        /// </summary>
        public float MinimumTargetRange;
        /// <summary>
        /// how much faster actors look around idly using this weapon (zero is unchanged)
        /// </summary>
        public float LookingTimeModifier;
        [TagField(Length = 0x4)]
        public byte[] Padding15;
        public float LightPowerOnTime; // seconds
        public float LightPowerOffTime; // seconds
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag LightPowerOnEffect;
        [TagField(ValidTags = new [] { "snd!","effe" })]
        public CachedTag LightPowerOffEffect;
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
        [TagField(Length = 0xC)]
        public byte[] Padding16;
        [TagField(ValidTags = new [] { "mod2" })]
        public CachedTag FirstPersonModel;
        [TagField(ValidTags = new [] { "antr" })]
        public CachedTag FirstPersonAnimations;
        [TagField(Length = 0x4)]
        public byte[] Padding17;
        [TagField(ValidTags = new [] { "wphi" })]
        public CachedTag HudInterface;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag PickupSound;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ZoomInSound;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag ZoomOutSound;
        [TagField(Length = 0xC)]
        public byte[] Padding18;
        /// <summary>
        /// how much to decrease active camo when a round is fired
        /// </summary>
        public float ActiveCamoDing;
        /// <summary>
        /// how fast to increase active camo (per tick) when a round is fired
        /// </summary>
        public float ActiveCamoRegrowthRate;
        [TagField(Length = 0xC)]
        public byte[] Padding19;
        [TagField(Length = 0x2)]
        public byte[] Padding20;
        public WeaponTypeValue WeaponType;
        public List<PredictedResourceBlock> PredictedResources1;
        public List<MagazinesBlock> Magazines;
        public List<TriggersBlock> Triggers;
        
        public enum FlagsValue : ushort
        {
            DoesNotCastShadow,
            TransparentSelfOcclusion,
            BrighterThanItShouldBe,
            NotAPathfindingObstacle
        }
        
        public enum AInValue : short
        {
            None,
            BodyVitality,
            ShieldVitality,
            RecentBodyDamage,
            RecentShieldDamage,
            RandomConstant,
            UmbrellaShieldVitality,
            ShieldStun,
            RecentUmbrellaShieldVitality,
            UmbrellaShieldStun,
            /// <summary>
            /// 00 damage
            /// </summary>
            Region,
            /// <summary>
            /// 01 damage
            /// </summary>
            Region1,
            /// <summary>
            /// 02 damage
            /// </summary>
            Region2,
            /// <summary>
            /// 03 damage
            /// </summary>
            Region3,
            /// <summary>
            /// 04 damage
            /// </summary>
            Region4,
            /// <summary>
            /// 05 damage
            /// </summary>
            Region5,
            /// <summary>
            /// 06 damage
            /// </summary>
            Region6,
            /// <summary>
            /// 07 damage
            /// </summary>
            Region7,
            Alive,
            Compass
        }
        
        public enum BInValue : short
        {
            None,
            BodyVitality,
            ShieldVitality,
            RecentBodyDamage,
            RecentShieldDamage,
            RandomConstant,
            UmbrellaShieldVitality,
            ShieldStun,
            RecentUmbrellaShieldVitality,
            UmbrellaShieldStun,
            /// <summary>
            /// 00 damage
            /// </summary>
            Region,
            /// <summary>
            /// 01 damage
            /// </summary>
            Region1,
            /// <summary>
            /// 02 damage
            /// </summary>
            Region2,
            /// <summary>
            /// 03 damage
            /// </summary>
            Region3,
            /// <summary>
            /// 04 damage
            /// </summary>
            Region4,
            /// <summary>
            /// 05 damage
            /// </summary>
            Region5,
            /// <summary>
            /// 06 damage
            /// </summary>
            Region6,
            /// <summary>
            /// 07 damage
            /// </summary>
            Region7,
            Alive,
            Compass
        }
        
        public enum CInValue : short
        {
            None,
            BodyVitality,
            ShieldVitality,
            RecentBodyDamage,
            RecentShieldDamage,
            RandomConstant,
            UmbrellaShieldVitality,
            ShieldStun,
            RecentUmbrellaShieldVitality,
            UmbrellaShieldStun,
            /// <summary>
            /// 00 damage
            /// </summary>
            Region,
            /// <summary>
            /// 01 damage
            /// </summary>
            Region1,
            /// <summary>
            /// 02 damage
            /// </summary>
            Region2,
            /// <summary>
            /// 03 damage
            /// </summary>
            Region3,
            /// <summary>
            /// 04 damage
            /// </summary>
            Region4,
            /// <summary>
            /// 05 damage
            /// </summary>
            Region5,
            /// <summary>
            /// 06 damage
            /// </summary>
            Region6,
            /// <summary>
            /// 07 damage
            /// </summary>
            Region7,
            Alive,
            Compass
        }
        
        public enum DInValue : short
        {
            None,
            BodyVitality,
            ShieldVitality,
            RecentBodyDamage,
            RecentShieldDamage,
            RandomConstant,
            UmbrellaShieldVitality,
            ShieldStun,
            RecentUmbrellaShieldVitality,
            UmbrellaShieldStun,
            /// <summary>
            /// 00 damage
            /// </summary>
            Region,
            /// <summary>
            /// 01 damage
            /// </summary>
            Region1,
            /// <summary>
            /// 02 damage
            /// </summary>
            Region2,
            /// <summary>
            /// 03 damage
            /// </summary>
            Region3,
            /// <summary>
            /// 04 damage
            /// </summary>
            Region4,
            /// <summary>
            /// 05 damage
            /// </summary>
            Region5,
            /// <summary>
            /// 06 damage
            /// </summary>
            Region6,
            /// <summary>
            /// 07 damage
            /// </summary>
            Region7,
            Alive,
            Compass
        }
        
        [TagStructure(Size = 0x48)]
        public class ObjectAttachmentBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "ligh","mgs2","cont","pctl","effe","lsnd" })]
            public CachedTag Type;
            [TagField(Length = 32)]
            public string Marker;
            public PrimaryScaleValue PrimaryScale;
            public SecondaryScaleValue SecondaryScale;
            public ChangeColorValue ChangeColor;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x10)]
            public byte[] Padding1;
            
            public enum PrimaryScaleValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum SecondaryScaleValue : short
            {
                None,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum ChangeColorValue : short
            {
                None,
                A,
                B,
                C,
                D
            }
        }
        
        [TagStructure(Size = 0x20)]
        public class ObjectWidgetBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "flag","ant!","glw!","mgs2","elec" })]
            public CachedTag Reference;
            [TagField(Length = 0x10)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x168)]
        public class ObjectFunctionBlock : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// this is the period for the above function (lower values make the function oscillate quickly, higher values make it
            /// oscillate slowly)
            /// </summary>
            public float Period; // seconds
            /// <summary>
            /// multiply this function by the above period
            /// </summary>
            public ScalePeriodByValue ScalePeriodBy;
            public FunctionValue Function;
            /// <summary>
            /// multiply this function by the result of the above function
            /// </summary>
            public ScaleFunctionByValue ScaleFunctionBy;
            /// <summary>
            /// the curve used for the wobble
            /// </summary>
            public WobbleFunctionValue WobbleFunction;
            /// <summary>
            /// the length of time it takes for the magnitude of this function to complete a wobble
            /// </summary>
            public float WobblePeriod; // seconds
            /// <summary>
            /// the amount of random wobble in the magnitude
            /// </summary>
            public float WobbleMagnitude; // percent
            /// <summary>
            /// if non-zero, all values above the square wave threshold are snapped to 1.0, and all values below it are snapped to 0.0 to
            /// create a square wave.
            /// </summary>
            public float SquareWaveThreshold;
            /// <summary>
            /// the number of discrete values to snap to (e.g., a step count of 5 would snap the function to 0.00,0.25,0.50,0.75 or 1.00)
            /// </summary>
            public short StepCount;
            public MapToValue MapTo;
            /// <summary>
            /// the number of times this function should repeat (e.g., a sawtooth count of 5 would give the function a value of 1.0 at
            /// each of 0.25,0.50,0.75 as well as at 1.0
            /// </summary>
            public short SawtoothCount;
            /// <summary>
            /// add this function to the final result of all of the above math
            /// </summary>
            public AddValue Add;
            /// <summary>
            /// multiply this function (from a weapon, vehicle, etc.) final result of all of the above math
            /// </summary>
            public ScaleResultByValue ScaleResultBy;
            /// <summary>
            /// controls how the bounds, below, are used
            /// </summary>
            public BoundsModeValue BoundsMode;
            public Bounds<float> Bounds;
            [TagField(Length = 0x4)]
            public byte[] Padding;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            /// <summary>
            /// if the specified function is off, so is this function
            /// </summary>
            public short TurnOffWith;
            /// <summary>
            /// applied before clip, ignored if zero
            /// </summary>
            public float ScaleBy;
            [TagField(Length = 0xFC)]
            public byte[] Padding2;
            [TagField(Length = 0x10)]
            public byte[] Padding3;
            [TagField(Length = 32)]
            public string Usage;
            
            public enum FlagsValue : uint
            {
                /// <summary>
                /// result of function is one minus actual result
                /// </summary>
                Invert,
                Additive,
                /// <summary>
                /// function does not deactivate when at or below lower bound
                /// </summary>
                AlwaysActive
            }
            
            public enum ScalePeriodByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum FunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum ScaleFunctionByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum WobbleFunctionValue : short
            {
                One,
                Zero,
                Cosine,
                CosineVariablePeriod,
                DiagonalWave,
                DiagonalWaveVariablePeriod,
                Slide,
                SlideVariablePeriod,
                Noise,
                Jitter,
                Wander,
                Spark
            }
            
            public enum MapToValue : short
            {
                Linear,
                Early,
                VeryEarly,
                Late,
                VeryLate,
                Cosine
            }
            
            public enum AddValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum ScaleResultByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum BoundsModeValue : short
            {
                Clip,
                ClipAndNormalize,
                ScaleToFit
            }
        }
        
        [TagStructure(Size = 0x2C)]
        public class ObjectChangeColors : TagStructure
        {
            public DarkenByValue DarkenBy;
            public ScaleByValue ScaleBy;
            public ScaleFlagsValue ScaleFlags;
            public RealRgbColor ColorLowerBound;
            public RealRgbColor ColorUpperBound;
            public List<ObjectChangeColorPermutations> Permutations;
            
            public enum DarkenByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum ScaleByValue : short
            {
                None,
                AIn,
                BIn,
                CIn,
                DIn,
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum ScaleFlagsValue : uint
            {
                /// <summary>
                /// blends colors in hsv rather than rgb space
                /// </summary>
                BlendInHsv,
                /// <summary>
                /// blends colors through more hues (goes the long way around the color wheel)
                /// </summary>
                MoreColors
            }
            
            [TagStructure(Size = 0x1C)]
            public class ObjectChangeColorPermutations : TagStructure
            {
                public float Weight;
                public RealRgbColor ColorLowerBound;
                public RealRgbColor ColorUpperBound;
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
                Sound
            }
        }
        
        public enum Flags1Value : uint
        {
            AlwaysMaintainsZUp,
            DestroyedByExplosions,
            UnaffectedByGravity
        }
        
        public enum AIn1Value : short
        {
            None
        }
        
        public enum BIn1Value : short
        {
            None
        }
        
        public enum CIn1Value : short
        {
            None
        }
        
        public enum DIn1Value : short
        {
            None
        }
        
        public enum Flags2Value : uint
        {
            VerticalHeatDisplay,
            MutuallyExclusiveTriggers,
            AttacksAutomaticallyOnBump,
            MustBeReadied,
            DoesnTCountTowardMaximum,
            AimAssistsOnlyWhenZoomed,
            PreventsGrenadeThrowing,
            MustBePickedUp,
            HoldsTriggersWhenDropped,
            PreventsMeleeAttack,
            DetonatesWhenDropped,
            CannotFireAtMaximumAge,
            SecondaryTriggerOverridesGrenades,
            ObsoleteDoesNotDepowerActiveCamoInMultilplayer,
            EnablesIntegratedNightVision,
            AisUseWeaponMeleeDamage
        }
        
        public enum SecondaryTriggerModeValue : short
        {
            Normal,
            SlavedToPrimary,
            InhibitsPrimary,
            LoadsAlterateAmmunition,
            LoadsMultiplePrimaryAmmunition
        }
        
        public enum AIn2Value : short
        {
            None,
            Heat,
            PrimaryAmmunition,
            SecondaryAmmunition,
            PrimaryRateOfFire,
            SecondaryRateOfFire,
            Ready,
            PrimaryEjectionPort,
            SecondaryEjectionPort,
            Overheated,
            PrimaryCharged,
            SecondaryCharged,
            Illumination,
            Age,
            IntegratedLight,
            PrimaryFiring,
            SecondaryFiring,
            PrimaryFiringOn,
            SecondaryFiringOn
        }
        
        public enum BIn2Value : short
        {
            None,
            Heat,
            PrimaryAmmunition,
            SecondaryAmmunition,
            PrimaryRateOfFire,
            SecondaryRateOfFire,
            Ready,
            PrimaryEjectionPort,
            SecondaryEjectionPort,
            Overheated,
            PrimaryCharged,
            SecondaryCharged,
            Illumination,
            Age,
            IntegratedLight,
            PrimaryFiring,
            SecondaryFiring,
            PrimaryFiringOn,
            SecondaryFiringOn
        }
        
        public enum CIn2Value : short
        {
            None,
            Heat,
            PrimaryAmmunition,
            SecondaryAmmunition,
            PrimaryRateOfFire,
            SecondaryRateOfFire,
            Ready,
            PrimaryEjectionPort,
            SecondaryEjectionPort,
            Overheated,
            PrimaryCharged,
            SecondaryCharged,
            Illumination,
            Age,
            IntegratedLight,
            PrimaryFiring,
            SecondaryFiring,
            PrimaryFiringOn,
            SecondaryFiringOn
        }
        
        public enum DIn2Value : short
        {
            None,
            Heat,
            PrimaryAmmunition,
            SecondaryAmmunition,
            PrimaryRateOfFire,
            SecondaryRateOfFire,
            Ready,
            PrimaryEjectionPort,
            SecondaryEjectionPort,
            Overheated,
            PrimaryCharged,
            SecondaryCharged,
            Illumination,
            Age,
            IntegratedLight,
            PrimaryFiring,
            SecondaryFiring,
            PrimaryFiringOn,
            SecondaryFiringOn
        }
        
        public enum MovementPenalizedValue : short
        {
            Always,
            WhenZoomed,
            WhenZoomedOrReloading
        }
        
        public enum WeaponTypeValue : short
        {
            Undefined,
            Shotgun,
            Needler,
            PlasmaPistol,
            PlasmaRifle
        }
        
        [TagStructure(Size = 0x70)]
        public class MagazinesBlock : TagStructure
        {
            public FlagsValue Flags;
            public short RoundsRecharged; // per second
            public short RoundsTotalInitial;
            public short RoundsTotalMaximum;
            public short RoundsLoadedMaximum;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            /// <summary>
            /// the length of time it takes to load a single magazine into the weapon
            /// </summary>
            public float ReloadTime; // seconds
            public short RoundsReloaded;
            [TagField(Length = 0x2)]
            public byte[] Padding1;
            /// <summary>
            /// the length of time it takes to chamber the next round
            /// </summary>
            public float ChamberTime; // seconds
            [TagField(Length = 0x8)]
            public byte[] Padding2;
            [TagField(Length = 0x10)]
            public byte[] Padding3;
            [TagField(ValidTags = new [] { "snd!","effe" })]
            public CachedTag ReloadingEffect;
            [TagField(ValidTags = new [] { "snd!","effe" })]
            public CachedTag ChamberingEffect;
            [TagField(Length = 0xC)]
            public byte[] Padding4;
            public List<MagazineObjects> Magazines1;
            
            public enum FlagsValue : uint
            {
                WastesRoundsWhenReloaded,
                EveryRoundMustBeChambered
            }
            
            [TagStructure(Size = 0x1C)]
            public class MagazineObjects : TagStructure
            {
                public short Rounds;
                [TagField(Length = 0xA)]
                public byte[] Padding;
                [TagField(ValidTags = new [] { "eqip" })]
                public CachedTag Equipment;
            }
        }
        
        [TagStructure(Size = 0x114)]
        public class TriggersBlock : TagStructure
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
            /// a percentage between 0 and 1 which controls how soon in its firing animation the weapon blurs
            /// </summary>
            public float BlurredRateOfFire;
            [TagField(Length = 0x8)]
            public byte[] Padding;
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
            [TagField(Length = 0x6)]
            public byte[] Padding1;
            /// <summary>
            /// how loud this weapon appears to the AI
            /// </summary>
            public FiringNoiseValue FiringNoise;
            /// <summary>
            /// the accuracy (between 0.0 and 1.0) of the weapon during firing
            /// </summary>
            public Bounds<float> Error;
            /// <summary>
            /// the continuous firing time it takes for the weapon to achieve its final error
            /// </summary>
            public float AccelerationTime1; // seconds
            /// <summary>
            /// the continuous idle time it takes for the weapon to return to its initial error
            /// </summary>
            public float DecelerationTime1; // seconds
            [TagField(Length = 0x8)]
            public byte[] Padding2;
            /// <summary>
            /// the amount of time it takes for this trigger to become fully charged
            /// </summary>
            public float ChargingTime; // seconds
            /// <summary>
            /// the amount of time this trigger can be charged before becoming overcharged
            /// </summary>
            public float ChargedTime; // seconds
            public OverchargedActionValue OverchargedAction;
            [TagField(Length = 0x2)]
            public byte[] Padding3;
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
            public DistributionFunctionValue DistributionFunction;
            public short ProjectilesPerShot;
            public float DistributionAngle; // degrees
            [TagField(Length = 0x4)]
            public byte[] Padding4;
            public Angle MinimumError; // degrees
            public Bounds<Angle> ErrorAngle; // degrees
            /// <summary>
            /// +x is forward, +z is up, +y is left
            /// </summary>
            public RealPoint3d FirstPersonOffset; // world units
            [TagField(Length = 0x4)]
            public byte[] Padding5;
            [TagField(ValidTags = new [] { "proj" })]
            public CachedTag Projectile;
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
            [TagField(Length = 0xC)]
            public byte[] Padding6;
            /// <summary>
            /// the amount of heat generated each time the trigger is fired
            /// </summary>
            public float HeatGeneratedPerRound; // [0,1]
            /// <summary>
            /// the amount the weapon ages each time the trigger is fired
            /// </summary>
            public float AgeGeneratedPerRound; // [0,1]
            [TagField(Length = 0x4)]
            public byte[] Padding7;
            /// <summary>
            /// the next trigger fires this often while holding down this trigger
            /// </summary>
            public float OverloadTime; // seconds
            [TagField(Length = 0x8)]
            public byte[] Padding8;
            [TagField(Length = 0x20)]
            public byte[] Padding9;
            [TagField(Length = 0x18)]
            public byte[] Padding10;
            /// <summary>
            /// firing effects determine what happens when this trigger is fired
            /// </summary>
            public List<TriggerFiringEffectBlock> FiringEffects;
            
            public enum FlagsValue : uint
            {
                /// <summary>
                /// poo poo ca ca pee pee
                /// </summary>
                TracksFiredProjectile,
                /// <summary>
                /// rather than being chosen sequentially, firing effects are picked randomly
                /// </summary>
                RandomFiringEffects,
                /// <summary>
                /// allows a weapon to be fired as long as there is a non-zero amount of ammunition loaded
                /// </summary>
                CanFireWithPartialAmmo,
                /// <summary>
                /// once fired, this trigger must be released and pressed to fire again
                /// </summary>
                DoesNotRepeatAutomatically,
                /// <summary>
                /// once depressed, this trigger must be released and pressed again to turn it off (and likewise to turn
                /// it back on)
                /// </summary>
                LocksInOnOffState,
                /// <summary>
                /// instead of coming out of the magic first person camera origin, the projectiles for this weapon
                /// actually come out of the gun
                /// </summary>
                ProjectilesUseWeaponOrigin,
                /// <summary>
                /// if this trigger is pressed when its owner drops the weapon (for whatever reason) this trigger stays
                /// down
                /// </summary>
                SticksWhenDropped,
                /// <summary>
                /// this trigger's ejection port is started during the key frame of its chamber animation
                /// </summary>
                EjectsDuringChamber,
                DischargingSpews,
                AnalogRateOfFire,
                UseErrorWhenUnzoomed,
                /// <summary>
                /// projectiles fired by this weapon cannot have their direction adjusted by the AI to hit the target
                /// </summary>
                ProjectileVectorCannotBeAdjusted,
                ProjectilesHaveIdenticalError,
                ProjectileIsClientSideOnly
            }
            
            public enum FiringNoiseValue : short
            {
                Silent,
                Medium,
                Loud,
                Shout,
                Quiet
            }
            
            public enum OverchargedActionValue : short
            {
                None,
                Explode,
                Discharge
            }
            
            public enum DistributionFunctionValue : short
            {
                Point,
                HorizontalFan
            }
            
            [TagStructure(Size = 0x84)]
            public class TriggerFiringEffectBlock : TagStructure
            {
                /// <summary>
                /// the minimum number of times this firing effect will be used, once it has been chosen
                /// </summary>
                public short ShotCountLowerBound;
                /// <summary>
                /// the maximum number of times this firing effect will be used, once it has been chosen
                /// </summary>
                public short ShotCountUpperBound;
                [TagField(Length = 0x20)]
                public byte[] Padding;
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

