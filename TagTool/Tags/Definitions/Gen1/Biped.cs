using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x4F4)]
    public class Biped : TagStructure
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
        public DefaultTeamValue DefaultTeam;
        public ConstantSoundVolumeValue ConstantSoundVolume;
        /// <summary>
        /// what percent damage applied to us gets applied to our children (i.e., riders)
        /// </summary>
        public float RiderDamageFraction; // [0,+inf]
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag IntegratedLightToggle;
        public AIn1Value AIn1;
        public BIn1Value BIn1;
        public CIn1Value CIn1;
        public DIn1Value DIn1;
        public Angle CameraFieldOfView; // degrees
        public float CameraStiffness;
        [TagField(Length = 32)]
        public string CameraMarkerName;
        [TagField(Length = 32)]
        public string CameraSubmergedMarkerName;
        public Angle PitchAutoLevel;
        public Bounds<Angle> PitchRange;
        public List<UnitCameraTrackBlock> CameraTracks;
        public RealVector3d SeatAccelerationScale;
        [TagField(Length = 0xC)]
        public byte[] Padding5;
        public float SoftPingThreshold; // [0,1]
        public float SoftPingInterruptTime; // seconds
        public float HardPingThreshold; // [0,1]
        public float HardPingInterruptTime; // seconds
        public float HardDeathThreshold; // [0,1]
        public float FeignDeathThreshold; // [0,1]
        public float FeignDeathTime; // seconds
        /// <summary>
        /// this must be set to tell the AI how far it should expect our evade animation to move us
        /// </summary>
        public float DistanceOfEvadeAnim; // world units
        /// <summary>
        /// this must be set to tell the AI how far it should expect our dive animation to move us
        /// </summary>
        public float DistanceOfDiveAnim; // world units
        [TagField(Length = 0x4)]
        public byte[] Padding6;
        /// <summary>
        /// if we take this much damage in a short space of time we will play our 'stunned movement' animations
        /// </summary>
        public float StunnedMovementThreshold; // [0,1]
        public float FeignDeathChance; // [0,1]
        public float FeignRepeatChance; // [0,1]
        /// <summary>
        /// actor variant which we spawn when we are destroyed or self-destruct
        /// </summary>
        [TagField(ValidTags = new [] { "actv" })]
        public CachedTag SpawnedActor;
        /// <summary>
        /// number of actors which we spawn
        /// </summary>
        public Bounds<short> SpawnedActorCount;
        /// <summary>
        /// velocity at which we throw spawned actors
        /// </summary>
        public float SpawnedVelocity;
        public Angle AimingVelocityMaximum; // degrees per second
        public Angle AimingAccelerationMaximum; // degrees per second squared
        public float CasualAimingModifier; // [0,1]
        public Angle LookingVelocityMaximum; // degrees per second
        public Angle LookingAccelerationMaximum; // degrees per second squared
        [TagField(Length = 0x8)]
        public byte[] Padding7;
        /// <summary>
        /// radius around this unit that the AI tries to avoid when entering it as a vehicle (zero = use bounding sphere radius)
        /// </summary>
        public float AiVehicleRadius;
        /// <summary>
        /// danger radius around this unit that the AI tries to avoid
        /// </summary>
        public float AiDangerRadius;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag MeleeDamage;
        public MotionSensorBlipSizeValue MotionSensorBlipSize;
        [TagField(Length = 0x2)]
        public byte[] Padding8;
        [TagField(Length = 0xC)]
        public byte[] Padding9;
        public List<UnitHudReferenceBlock> NewHudInterfaces;
        public List<DialogueVariantBlock> DialogueVariants;
        public float GrenadeVelocity; // world units per second
        public GrenadeTypeValue GrenadeType;
        public short GrenadeCount;
        [TagField(Length = 0x4)]
        public byte[] Padding10;
        public List<PoweredSeatBlock> PoweredSeats;
        public List<UnitWeaponBlock> Weapons;
        public List<UnitSeatBlock> Seats;
        public Angle MovingTurningSpeed; // degrees per second
        public Flags2Value Flags2;
        public Angle StationaryTurningThreshold;
        [TagField(Length = 0x10)]
        public byte[] Padding11;
        public AIn2Value AIn2;
        public BIn2Value BIn2;
        public CIn2Value CIn2;
        public DIn2Value DIn2;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag DonTUse;
        /// <summary>
        /// angle at which we bank left/right when sidestepping or turning while moving forwards
        /// </summary>
        public Angle BankAngle; // degrees
        /// <summary>
        /// time it takes us to apply a bank
        /// </summary>
        public float BankApplyTime; // seconds
        /// <summary>
        /// time it takes us to recover from a bank
        /// </summary>
        public float BankDecayTime; // seconds
        /// <summary>
        /// amount that we pitch up/down when moving up or down
        /// </summary>
        public float PitchRatio;
        /// <summary>
        /// max velocity when not crouching
        /// </summary>
        public float MaxVelocity; // world units per second
        /// <summary>
        /// max sideways or up/down velocity when not crouching
        /// </summary>
        public float MaxSidestepVelocity; // world units per second
        public float Acceleration; // world units per second squared
        public float Deceleration; // world units per second squared
        /// <summary>
        /// turn rate
        /// </summary>
        public Angle AngularVelocityMaximum; // degrees per second
        /// <summary>
        /// turn acceleration rate
        /// </summary>
        public Angle AngularAccelerationMaximum; // degrees per second squared
        /// <summary>
        /// how much slower we fly if crouching (zero = same speed)
        /// </summary>
        public float CrouchVelocityModifier; // [0,1]
        [TagField(Length = 0x8)]
        public byte[] Padding12;
        public Angle MaximumSlopeAngle; // degrees
        public Angle DownhillFalloffAngle; // degrees
        public Angle DownhillCutoffAngle; // degrees
        public float DownhillVelocityScale;
        public Angle UphillFalloffAngle; // degrees
        public Angle UphillCutoffAngle; // degrees
        public float UphillVelocityScale;
        [TagField(Length = 0x18)]
        public byte[] Padding13;
        [TagField(ValidTags = new [] { "foot" })]
        public CachedTag Footsteps;
        [TagField(Length = 0x18)]
        public byte[] Padding14;
        public float JumpVelocity; // world units per second
        [TagField(Length = 0x1C)]
        public byte[] Padding15;
        /// <summary>
        /// the longest amount of time the biped can take to recover from a soft landing
        /// </summary>
        public float MaximumSoftLandingTime; // seconds
        /// <summary>
        /// the longest amount of time the biped can take to recover from a hard landing
        /// </summary>
        public float MaximumHardLandingTime; // seconds
        /// <summary>
        /// below this velocity the biped does not react when landing
        /// </summary>
        public float MinimumSoftLandingVelocity; // world units per second
        /// <summary>
        /// below this velocity the biped will not do a soft landing when returning to the ground
        /// </summary>
        public float MinimumHardLandingVelocity; // world units per second
        /// <summary>
        /// the velocity corresponding to the maximum landing time
        /// </summary>
        public float MaximumHardLandingVelocity; // world units per second
        /// <summary>
        /// the maximum velocity with which a character can strike the ground and live
        /// </summary>
        public float DeathHardLandingVelocity; // world units per second
        [TagField(Length = 0x14)]
        public byte[] Padding16;
        public float StandingCameraHeight; // world units
        public float CrouchingCameraHeight; // world units
        public float CrouchTransitionTime; // seconds
        [TagField(Length = 0x18)]
        public byte[] Padding17;
        public float StandingCollisionHeight; // world units
        public float CrouchingCollisionHeight; // world units
        public float CollisionRadius; // world units
        [TagField(Length = 0x28)]
        public byte[] Padding18;
        public float AutoaimWidth; // world units
        [TagField(Length = 0x8C)]
        public byte[] Padding19;
        /// <summary>
        /// these are the points where the biped touches the ground
        /// </summary>
        public List<ContactPointBlock> ContactPoints;
        
        [Flags]
        public enum FlagsValue : ushort
        {
            DoesNotCastShadow = 1 << 0,
            TransparentSelfOcclusion = 1 << 1,
            BrighterThanItShouldBe = 1 << 2,
            NotAPathfindingObstacle = 1 << 3
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
            
            [Flags]
            public enum FlagsValue : uint
            {
                /// <summary>
                /// result of function is one minus actual result
                /// </summary>
                Invert = 1 << 0,
                Additive = 1 << 1,
                /// <summary>
                /// function does not deactivate when at or below lower bound
                /// </summary>
                AlwaysActive = 1 << 2
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
        
        [Flags]
        public enum Flags1Value : uint
        {
            CircularAiming = 1 << 0,
            DestroyedAfterDying = 1 << 1,
            HalfSpeedInterpolation = 1 << 2,
            FiresFromCamera = 1 << 3,
            EntranceInsideBoundingSphere = 1 << 4,
            Unused = 1 << 5,
            CausesPassengerDialogue = 1 << 6,
            ResistsPings = 1 << 7,
            MeleeAttackIsFatal = 1 << 8,
            DonTRefaceDuringPings = 1 << 9,
            HasNoAiming = 1 << 10,
            SimpleCreature = 1 << 11,
            ImpactMeleeAttachesToUnit = 1 << 12,
            ImpactMeleeDiesOnShields = 1 << 13,
            CannotOpenDoorsAutomatically = 1 << 14,
            MeleeAttackersCannotAttach = 1 << 15,
            NotInstantlyKilledByMelee = 1 << 16,
            ShieldSapping = 1 << 17,
            RunsAroundFlaming = 1 << 18,
            Inconsequential = 1 << 19,
            SpecialCinematicUnit = 1 << 20,
            IgnoredByAutoaiming = 1 << 21,
            ShieldsFryInfectionForms = 1 << 22,
            IntegratedLightCntrlsWeapon = 1 << 23,
            IntegratedLightLastsForever = 1 << 24
        }
        
        public enum DefaultTeamValue : short
        {
            None,
            Player,
            Human,
            Covenant,
            Flood,
            Sentinel,
            Unused6,
            Unused7,
            Unused8,
            Unused9
        }
        
        public enum ConstantSoundVolumeValue : short
        {
            Silent,
            Medium,
            Loud,
            Shout,
            Quiet
        }
        
        public enum AIn1Value : short
        {
            None,
            DriverSeatPower,
            GunnerSeatPower,
            AimingChange,
            MouthAperture,
            IntegratedLightPower,
            CanBlink,
            ShieldSapping
        }
        
        public enum BIn1Value : short
        {
            None,
            DriverSeatPower,
            GunnerSeatPower,
            AimingChange,
            MouthAperture,
            IntegratedLightPower,
            CanBlink,
            ShieldSapping
        }
        
        public enum CIn1Value : short
        {
            None,
            DriverSeatPower,
            GunnerSeatPower,
            AimingChange,
            MouthAperture,
            IntegratedLightPower,
            CanBlink,
            ShieldSapping
        }
        
        public enum DIn1Value : short
        {
            None,
            DriverSeatPower,
            GunnerSeatPower,
            AimingChange,
            MouthAperture,
            IntegratedLightPower,
            CanBlink,
            ShieldSapping
        }
        
        [TagStructure(Size = 0x1C)]
        public class UnitCameraTrackBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "trak" })]
            public CachedTag Track;
            [TagField(Length = 0xC)]
            public byte[] Padding;
        }
        
        public enum MotionSensorBlipSizeValue : short
        {
            Medium,
            Small,
            Large
        }
        
        [TagStructure(Size = 0x30)]
        public class UnitHudReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "unhi" })]
            public CachedTag UnitHudInterface;
            [TagField(Length = 0x20)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x18)]
        public class DialogueVariantBlock : TagStructure
        {
            /// <summary>
            /// variant number to use this dialogue with (must match the suffix in the permutations on the unit's model)
            /// </summary>
            public short VariantNumber;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            [TagField(ValidTags = new [] { "udlg" })]
            public CachedTag Dialogue;
        }
        
        public enum GrenadeTypeValue : short
        {
            HumanFragmentation,
            CovenantPlasma
        }
        
        [TagStructure(Size = 0x44)]
        public class PoweredSeatBlock : TagStructure
        {
            [TagField(Length = 0x4)]
            public byte[] Padding;
            public float DriverPowerupTime; // seconds
            public float DriverPowerdownTime; // seconds
            [TagField(Length = 0x38)]
            public byte[] Padding1;
        }
        
        [TagStructure(Size = 0x24)]
        public class UnitWeaponBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Weapon;
            [TagField(Length = 0x14)]
            public byte[] Padding;
        }
        
        [TagStructure(Size = 0x11C)]
        public class UnitSeatBlock : TagStructure
        {
            public FlagsValue Flags;
            [TagField(Length = 32)]
            public string Label;
            [TagField(Length = 32)]
            public string MarkerName;
            [TagField(Length = 0x20)]
            public byte[] Padding;
            public RealVector3d AccelerationScale;
            [TagField(Length = 0xC)]
            public byte[] Padding1;
            public float YawRate; // degrees per second
            public float PitchRate; // degrees per second
            [TagField(Length = 32)]
            public string CameraMarkerName;
            [TagField(Length = 32)]
            public string CameraSubmergedMarkerName;
            public Angle PitchAutoLevel;
            public Bounds<Angle> PitchRange;
            public List<UnitCameraTrackBlock> CameraTracks;
            public List<UnitHudReferenceBlock> UnitHudInterface;
            [TagField(Length = 0x4)]
            public byte[] Padding2;
            public short HudTextMessageIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding3;
            public Angle YawMinimum;
            public Angle YawMaximum;
            [TagField(ValidTags = new [] { "actv" })]
            public CachedTag BuiltInGunner;
            [TagField(Length = 0x14)]
            public byte[] Padding4;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Invisible = 1 << 0,
                Locked = 1 << 1,
                Driver = 1 << 2,
                Gunner = 1 << 3,
                ThirdPersonCamera = 1 << 4,
                AllowsWeapons = 1 << 5,
                ThirdPersonOnEnter = 1 << 6,
                FirstPersonCameraSlavedToGun = 1 << 7,
                AllowVehicleCommunicationAnimations = 1 << 8,
                NotValidWithoutDriver = 1 << 9,
                AllowAiNoncombatants = 1 << 10
            }
            
            [TagStructure(Size = 0x1C)]
            public class UnitCameraTrackBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "trak" })]
                public CachedTag Track;
                [TagField(Length = 0xC)]
                public byte[] Padding;
            }
            
            [TagStructure(Size = 0x30)]
            public class UnitHudReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "unhi" })]
                public CachedTag UnitHudInterface;
                [TagField(Length = 0x20)]
                public byte[] Padding;
            }
        }
        
        [Flags]
        public enum Flags2Value : uint
        {
            TurnsWithoutAnimating = 1 << 0,
            UsesPlayerPhysics = 1 << 1,
            Flying = 1 << 2,
            PhysicsPillCenteredAtOrigin = 1 << 3,
            Spherical = 1 << 4,
            PassesThroughOtherBipeds = 1 << 5,
            CanClimbAnySurface = 1 << 6,
            ImmuneToFallingDamage = 1 << 7,
            RotateWhileAirborne = 1 << 8,
            UsesLimpBodyPhysics = 1 << 9,
            HasNoDyingAirborne = 1 << 10,
            RandomSpeedIncrease = 1 << 11,
            UnitUsesOldNtscPlayerPhysics = 1 << 12
        }
        
        public enum AIn2Value : short
        {
            None,
            FlyingVelocity
        }
        
        public enum BIn2Value : short
        {
            None,
            FlyingVelocity
        }
        
        public enum CIn2Value : short
        {
            None,
            FlyingVelocity
        }
        
        public enum DIn2Value : short
        {
            None,
            FlyingVelocity
        }
        
        [TagStructure(Size = 0x40)]
        public class ContactPointBlock : TagStructure
        {
            [TagField(Length = 0x20)]
            public byte[] Padding;
            [TagField(Length = 32)]
            public string MarkerName;
        }
    }
}

