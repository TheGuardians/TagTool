using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "vehicle", Tag = "vehi", Size = 0x300)]
    public class Vehicle : TagStructure
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
        public DefaultTeamValue DefaultTeam;
        public ConstantSoundVolumeValue ConstantSoundVolume;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag IntegratedLightToggle;
        public Angle CameraFieldOfView; // degrees
        public float CameraStiffness;
        public UnitCameraStructBlock UnitCamera;
        public UnitSeatAccelerationStructBlock Acceleration;
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
        /// <summary>
        /// if we take this much damage in a short space of time we will play our 'stunned movement' animations
        /// </summary>
        public float StunnedMovementThreshold; // [0,1]
        public float FeignDeathChance; // [0,1]
        public float FeignRepeatChance; // [0,1]
        /// <summary>
        /// automatically created character when this unit is driven
        /// </summary>
        [TagField(ValidTags = new [] { "char" })]
        public CachedTag SpawnedTurretCharacter;
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
        /// <summary>
        /// where the primary weapon is attached
        /// </summary>
        public StringId RightHandNode;
        /// <summary>
        /// where the seconday weapon is attached (for dual-pistol modes)
        /// </summary>
        public StringId LeftHandNode;
        public UnitAdditionalNodeNamesStructBlock MoreDamnNodes;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag MeleeDamage;
        public UnitBoardingMeleeStructBlock YourMomma;
        public MotionSensorBlipSizeValue MotionSensorBlipSize;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        public List<UnitPosturesBlock> Postures;
        public List<UnitHudReferenceBlock> NewHudInterfaces;
        public List<DialogueVariantBlock> DialogueVariants;
        public float GrenadeVelocity; // world units per second
        public GrenadeTypeValue GrenadeType;
        public short GrenadeCount;
        public List<PoweredSeatBlock> PoweredSeats;
        public List<UnitWeaponBlock> Weapons;
        public List<UnitSeatBlock> Seats;
        public UnitBoostStructBlock Boost;
        public UnitLipsyncScalesStructBlock Lipsync;
        public FlagsValue2 Flags2;
        public TypeValue PhysicsType;
        public ControlValue Control;
        public float MaximumForwardSpeed;
        public float MaximumReverseSpeed;
        public float SpeedAcceleration;
        public float SpeedDeceleration;
        public float MaximumLeftTurn;
        public float MaximumRightTurnNegative;
        public float WheelCircumference;
        public float TurnRate;
        public float BlurSpeed;
        /// <summary>
        /// if your type corresponds to something in this list choose it
        /// </summary>
        public SpecificTypeValue SpecificType;
        public PlayerTrainingVehicleTypeValue PlayerTrainingVehicleType;
        public StringId FlipMessage;
        public float TurnScale;
        public float SpeedTurnPenaltyPower052;
        public float SpeedTurnPenalty0None1CanTTurnAtTopSpeed;
        public float MaximumLeftSlide;
        public float MaximumRightSlide;
        public float SlideAcceleration;
        public float SlideDeceleration;
        public float MinimumFlippingAngularVelocity;
        public float MaximumFlippingAngularVelocity;
        /// <summary>
        /// The size determine what kind of seats in larger vehicles it may occupy (i.e. small or large cargo seats)
        /// </summary>
        public VehicleSizeValue VehicleSize;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        public RealEulerAngles2d FixedGunOffset;
        /// <summary>
        /// when the steering is off by more than the cusp angle
        /// the steering will overcompensate more and more.  when it
        /// is less, it
        /// overcompensates less and less.  the exponent
        /// should be something in the neighborhood of 2.0
        /// 
        /// </summary>
        public Angle OverdampenCuspAngle; // degrees
        public float OverdampenExponent;
        public float CrouchTransitionTime; // seconds
        [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;
        /// <summary>
        /// higher moments make engine spin up slower
        /// </summary>
        public float EngineMoment;
        /// <summary>
        /// higher moments make engine spin up slower
        /// </summary>
        public float EngineMaxAngularVelocity;
        public List<GearBlock> Gears;
        /// <summary>
        /// big vehicles need to scale this down.  0 defaults to 1, which is generally a good value.  This is used with alien fighter
        /// physics
        /// </summary>
        public float FlyingTorqueScale;
        /// <summary>
        /// how much do we scale the force the biped the applies down on the seat when he enters. 0 == no acceleration
        /// </summary>
        public float SeatEnteranceAccelerationScale;
        /// <summary>
        /// how much do we scale the force the biped the applies down on the seat when he exits. 0 == no acceleration
        /// </summary>
        public float SeatExitAccelersationScale;
        /// <summary>
        /// human plane physics only. 0 is nothing.  1 is like thowing the engine to full reverse
        /// </summary>
        public float AirFrictionDeceleration;
        /// <summary>
        /// human plane physics only. 0 is default (1)
        /// </summary>
        public float ThrustScale;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag SuspensionSound;
        [TagField(ValidTags = new [] { "snd!" })]
        public CachedTag CrashSound;
        [TagField(ValidTags = new [] { "foot" })]
        public CachedTag Unused;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag SpecialEffect;
        [TagField(ValidTags = new [] { "effe" })]
        public CachedTag UnusedEffect;
        public HavokVehiclePhysicsStructBlock HavokVehiclePhysics;
        
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
            public TagFunction DefaultFunction;
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
            Unused = 1 << 23,
            Unused1 = 1 << 24,
            ActsAsGunnerForParent = 1 << 25,
            ControlledByParentGunner = 1 << 26,
            ParentSPrimaryWeapon = 1 << 27,
            UnitHasBoost = 1 << 28
        }
        
        public enum DefaultTeamValue : short
        {
            Default,
            Player,
            Human,
            Covenant,
            Flood,
            Sentinel,
            Heretic,
            Prophet,
            Unused8,
            Unused9,
            Unused10,
            Unused11,
            Unused12,
            Unused13,
            Unused14,
            Unused15
        }
        
        public enum ConstantSoundVolumeValue : short
        {
            Silent,
            Medium,
            Loud,
            Shout,
            Quiet
        }
        
        [TagStructure(Size = 0x1C)]
        public class UnitCameraStructBlock : TagStructure
        {
            public StringId CameraMarkerName;
            public StringId CameraSubmergedMarkerName;
            public Angle PitchAutoLevel;
            public Bounds<Angle> PitchRange;
            public List<UnitCameraTrackBlock> CameraTracks;
            
            [TagStructure(Size = 0x8)]
            public class UnitCameraTrackBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "trak" })]
                public CachedTag Track;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class UnitSeatAccelerationStructBlock : TagStructure
        {
            public RealVector3d AccelerationRange; // world units per second squared
            public float AccelActionScale; // actions fail [0,1+]
            public float AccelAttachScale; // detach unit [0,1+]
        }
        
        [TagStructure(Size = 0x4)]
        public class UnitAdditionalNodeNamesStructBlock : TagStructure
        {
            /// <summary>
            /// if found, use this gun marker
            /// </summary>
            public StringId PreferredGunNode;
        }
        
        [TagStructure(Size = 0x28)]
        public class UnitBoardingMeleeStructBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag BoardingMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag BoardingMeleeResponse;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag LandingMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag FlurryMeleeDamage;
            [TagField(ValidTags = new [] { "jpt!" })]
            public CachedTag ObstacleSmashDamage;
        }
        
        public enum MotionSensorBlipSizeValue : short
        {
            Medium,
            Small,
            Large
        }
        
        [TagStructure(Size = 0x10)]
        public class UnitPosturesBlock : TagStructure
        {
            public StringId Name;
            public RealVector3d PillOffset;
        }
        
        [TagStructure(Size = 0x8)]
        public class UnitHudReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "nhdt" })]
            public CachedTag NewUnitHudInterface;
        }
        
        [TagStructure(Size = 0xC)]
        public class DialogueVariantBlock : TagStructure
        {
            /// <summary>
            /// variant number to use this dialogue with (must match the suffix in the permutations on the unit's model)
            /// </summary>
            public short VariantNumber;
            [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            [TagField(ValidTags = new [] { "udlg" })]
            public CachedTag Dialogue;
        }
        
        public enum GrenadeTypeValue : short
        {
            HumanFragmentation,
            CovenantPlasma
        }
        
        [TagStructure(Size = 0x8)]
        public class PoweredSeatBlock : TagStructure
        {
            public float DriverPowerupTime; // seconds
            public float DriverPowerdownTime; // seconds
        }
        
        [TagStructure(Size = 0x8)]
        public class UnitWeaponBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "weap" })]
            public CachedTag Weapon;
        }
        
        [TagStructure(Size = 0xB0)]
        public class UnitSeatBlock : TagStructure
        {
            public FlagsValue Flags;
            public StringId Label;
            public StringId MarkerName;
            public StringId EntryMarkerName;
            public StringId BoardingGrenadeMarker;
            public StringId BoardingGrenadeString;
            public StringId BoardingMeleeString;
            /// <summary>
            /// nathan is too lazy to make pings for each seat.
            /// </summary>
            public float PingScale;
            /// <summary>
            /// how much time it takes to evict a rider from a flipped vehicle
            /// </summary>
            public float TurnoverTime; // seconds
            public UnitSeatAccelerationStructBlock Acceleration;
            public float AiScariness;
            public AiSeatTypeValue AiSeatType;
            public short BoardingSeat;
            /// <summary>
            /// how far to interpolate listener position from camera to occupant's head
            /// </summary>
            public float ListenerInterpolationFactor;
            /// <summary>
            /// when the unit velocity is 0, the yaw/pitch rates are the left values
            /// at [max speed reference], the yaw/pitch rates are
            /// the right values.
            /// the max speed reference is what the code uses to generate a clamped speed from 0..1
            /// the exponent
            /// controls how midrange speeds are interpreted.
            /// </summary>
            public Bounds<float> YawRateBounds; // degrees per second
            public Bounds<float> PitchRateBounds; // degrees per second
            public float MinSpeedReference;
            public float MaxSpeedReference;
            public float SpeedExponent;
            public UnitCameraStructBlock UnitCamera;
            public List<UnitHudReferenceBlock> UnitHudInterface;
            public StringId EnterSeatString;
            public Angle YawMinimum;
            public Angle YawMaximum;
            [TagField(ValidTags = new [] { "char" })]
            public CachedTag BuiltInGunner;
            /// <summary>
            /// note: the entry radius shouldn't exceed 3 world units, 
            /// as that is as far as the player will search for a vehicle
            /// to
            /// enter.
            /// </summary>
            /// <summary>
            /// how close to the entry marker a unit must be
            /// </summary>
            public float EntryRadius;
            /// <summary>
            /// angle from marker forward the unit must be
            /// </summary>
            public Angle EntryMarkerConeAngle;
            /// <summary>
            /// angle from unit facing the marker must be
            /// </summary>
            public Angle EntryMarkerFacingAngle;
            public float MaximumRelativeVelocity;
            public StringId InvisibleSeatRegion;
            public int RuntimeInvisibleSeatRegionIndex;
            
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
                AllowAiNoncombatants = 1 << 10,
                BoardingSeat = 1 << 11,
                AiFiringDisabledByMaxAcceleration = 1 << 12,
                BoardingEntersSeat = 1 << 13,
                BoardingNeedAnyPassenger = 1 << 14,
                ControlsOpenAndClose = 1 << 15,
                InvalidForPlayer = 1 << 16,
                InvalidForNonPlayer = 1 << 17,
                GunnerPlayerOnly = 1 << 18,
                InvisibleUnderMajorDamage = 1 << 19
            }
            
            [TagStructure(Size = 0x14)]
            public class UnitSeatAccelerationStructBlock : TagStructure
            {
                public RealVector3d AccelerationRange; // world units per second squared
                public float AccelActionScale; // actions fail [0,1+]
                public float AccelAttachScale; // detach unit [0,1+]
            }
            
            public enum AiSeatTypeValue : short
            {
                None,
                Passenger,
                Gunner,
                SmallCargo,
                LargeCargo,
                Driver
            }
            
            [TagStructure(Size = 0x1C)]
            public class UnitCameraStructBlock : TagStructure
            {
                public StringId CameraMarkerName;
                public StringId CameraSubmergedMarkerName;
                public Angle PitchAutoLevel;
                public Bounds<Angle> PitchRange;
                public List<UnitCameraTrackBlock> CameraTracks;
                
                [TagStructure(Size = 0x8)]
                public class UnitCameraTrackBlock : TagStructure
                {
                    [TagField(ValidTags = new [] { "trak" })]
                    public CachedTag Track;
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class UnitHudReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "nhdt" })]
                public CachedTag NewUnitHudInterface;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class UnitBoostStructBlock : TagStructure
        {
            public float BoostPeakPower;
            public float BoostRisePower;
            public float BoostPeakTime;
            public float BoostFallPower;
            public float DeadTime;
        }
        
        [TagStructure(Size = 0x8)]
        public class UnitLipsyncScalesStructBlock : TagStructure
        {
            public float AttackWeight;
            public float DecayWeight;
        }
        
        [Flags]
        public enum FlagsValue2 : uint
        {
            SpeedWakesPhysics = 1 << 0,
            TurnWakesPhysics = 1 << 1,
            DriverPowerWakesPhysics = 1 << 2,
            GunnerPowerWakesPhysics = 1 << 3,
            ControlOppositeSpeedSetsBrake = 1 << 4,
            SlideWakesPhysics = 1 << 5,
            KillsRidersAtTerminalVelocity = 1 << 6,
            CausesCollisionDamage = 1 << 7,
            AiWeaponCannotRotate = 1 << 8,
            AiDoesNotRequireDriver = 1 << 9,
            AiUnused = 1 << 10,
            AiDriverEnable = 1 << 11,
            AiDriverFlying = 1 << 12,
            AiDriverCanSidestep = 1 << 13,
            AiDriverHovering = 1 << 14,
            VehicleSteersDirectly = 1 << 15,
            Unused = 1 << 16,
            HasEBrake = 1 << 17,
            NoncombatVehicle = 1 << 18,
            NoFrictionWDriver = 1 << 19,
            CanTriggerAutomaticOpeningDoors = 1 << 20,
            AutoaimWhenTeamless = 1 << 21
        }
        
        public enum TypeValue : short
        {
            HumanTank,
            HumanJeep,
            HumanBoat,
            HumanPlane,
            AlienScout,
            AlienFighter,
            Turret
        }
        
        public enum ControlValue : short
        {
            VehicleControlNormal,
            VehicleControlUnused,
            VehicleControlTank
        }
        
        public enum SpecificTypeValue : short
        {
            None,
            Ghost,
            Wraith,
            Spectre,
            SentinelEnforcer
        }
        
        public enum PlayerTrainingVehicleTypeValue : short
        {
            None,
            Warthog,
            WarthogTurret,
            Ghost,
            Banshee,
            Tank,
            Wraith
        }
        
        public enum VehicleSizeValue : short
        {
            Small,
            Large
        }
        
        [TagStructure(Size = 0x44)]
        public class GearBlock : TagStructure
        {
            public TorqueCurveStructBlock LoadedTorqueCurve;
            public TorqueCurveStructBlock1 CruisingTorqueCurve;
            /// <summary>
            /// seconds
            /// </summary>
            public float MinTimeToUpshift;
            /// <summary>
            /// 0-1
            /// </summary>
            public float EngineUpShiftScale;
            public float GearRatio;
            /// <summary>
            /// seconds
            /// </summary>
            public float MinTimeToDownshift;
            /// <summary>
            /// 0-1
            /// </summary>
            public float EngineDownShiftScale;
            
            [TagStructure(Size = 0x18)]
            public class TorqueCurveStructBlock : TagStructure
            {
                public float MinTorque;
                public float MaxTorque;
                public float PeakTorqueScale;
                public float PastPeakTorqueExponent;
                /// <summary>
                /// generally 0 for loading torque and something less than max torque for cruising torque
                /// </summary>
                public float TorqueAtMaxAngularVelocity;
                public float TorqueAt2xMaxAngularVelocity;
            }
            
            [TagStructure(Size = 0x18)]
            public class TorqueCurveStructBlock1 : TagStructure
            {
                public float MinTorque;
                public float MaxTorque;
                public float PeakTorqueScale;
                public float PastPeakTorqueExponent;
                /// <summary>
                /// generally 0 for loading torque and something less than max torque for cruising torque
                /// </summary>
                public float TorqueAtMaxAngularVelocity;
                public float TorqueAt2xMaxAngularVelocity;
            }
        }
        
        [TagStructure(Size = 0x54)]
        public class HavokVehiclePhysicsStructBlock : TagStructure
        {
            public FlagsValue Flags;
            /// <summary>
            /// for friction based vehicles only
            /// </summary>
            public float GroundFriction;
            /// <summary>
            /// for friction based vehicles only
            /// </summary>
            public float GroundDepth;
            /// <summary>
            /// for friction based vehicles only
            /// </summary>
            public float GroundDampFactor;
            /// <summary>
            /// for friction based vehicles only
            /// </summary>
            public float GroundMovingFriction;
            /// <summary>
            /// degrees 0-90
            /// </summary>
            public float GroundMaximumSlope0;
            /// <summary>
            /// degrees 0-90.  and greater than slope 0
            /// </summary>
            public float GroundMaximumSlope1;
            [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            /// <summary>
            /// lift per WU.
            /// </summary>
            public float AntiGravityBankLift;
            /// <summary>
            /// how quickly we bank when we steer
            /// </summary>
            public float SteeringBankReactionScale;
            /// <summary>
            /// value of 0 defaults to 1.  .5 is half gravity
            /// </summary>
            public float GravityScale;
            /// <summary>
            /// generated from the radius of the hkConvexShape for this vehicle
            /// </summary>
            public float Radius;
            public List<AntiGravityPointDefinitionBlock> AntiGravityPoints;
            public List<FrictionPointDefinitionBlock> FrictionPoints;
            public List<VehiclePhantomShapeBlock> ShapePhantomShape;
            
            [Flags]
            public enum FlagsValue : uint
            {
                Invalid = 1 << 0
            }
            
            [TagStructure(Size = 0x4C)]
            public class AntiGravityPointDefinitionBlock : TagStructure
            {
                public StringId MarkerName;
                public FlagsValue Flags;
                public float AntigravStrength;
                public float AntigravOffset;
                public float AntigravHeight;
                public float AntigravDampFactor;
                public float AntigravNormalK1;
                public float AntigravNormalK0;
                public float Radius;
                [TagField(Length = 0xC, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                public StringId DamageSourceRegionName;
                public float DefaultStateError;
                public float MinorDamageError;
                public float MediumDamageError;
                public float MajorDamageError;
                public float DestroyedStateError;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    GetsDamageFromRegion = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x4C)]
            public class FrictionPointDefinitionBlock : TagStructure
            {
                public StringId MarkerName;
                public FlagsValue Flags;
                /// <summary>
                /// (0.0-1.0) fraction of total vehicle mass
                /// </summary>
                public float FractionOfTotalMass;
                public float Radius;
                /// <summary>
                /// radius when the tire is blown off.
                /// </summary>
                public float DamagedRadius;
                public FrictionTypeValue FrictionType;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float MovingFrictionVelocityDiff;
                public float EBrakeMovingFriction;
                public float EBrakeFriction;
                public float EBrakeMovingFrictionVelDiff;
                [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                public StringId CollisionGlobalMaterialName;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding2;
                /// <summary>
                /// only need point can destroy flag set
                /// </summary>
                public ModelStateDestroyedValue ModelStateDestroyed;
                /// <summary>
                /// only need point can destroy flag set
                /// </summary>
                public StringId RegionName;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding3;
                
                [Flags]
                public enum FlagsValue : uint
                {
                    GetsDamageFromRegion = 1 << 0,
                    Powered = 1 << 1,
                    FrontTurning = 1 << 2,
                    RearTurning = 1 << 3,
                    AttachedToEBrake = 1 << 4,
                    CanBeDestroyed = 1 << 5
                }
                
                public enum FrictionTypeValue : short
                {
                    Point,
                    Forward
                }
                
                public enum ModelStateDestroyedValue : short
                {
                    Default,
                    MinorDamage,
                    MediumDamage,
                    MajorDamage,
                    Destroyed
                }
            }
            
            [TagStructure(Size = 0x120)]
            public class VehiclePhantomShapeBlock : TagStructure
            {
                [TagField(Length = 0x4)]
                public byte[] Unknown;
                public short Size;
                public short Count;
                [TagField(Length = 0x4)]
                public byte[] Unknown1;
                [TagField(Length = 0x4)]
                public byte[] Unknown2;
                public int ChildShapesSize;
                public int ChildShapesCapacity;
                [TagField(Length = 4)]
                public CollisionFilterDatum[] CollisionFilter;
                public int MultisphereCount;
                public FlagsValue Flags;
                [TagField(Length = 0x8, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public float X0;
                public float X1;
                public float Y0;
                public float Y1;
                public float Z0;
                public float Z1;
                [TagField(Length = 8)]
                public Unknown3Datum[] Unknown3;
                [TagField(Length = 4)]
                public NumSpheresDatum[] NumSpheres;
                
                public enum ShapeTypeValue : short
                {
                    Sphere,
                    Pill,
                    Box,
                    Triangle,
                    Polyhedron,
                    MultiSphere,
                    Unused0,
                    Unused1,
                    Unused2,
                    Unused3,
                    Unused4,
                    Unused5,
                    Unused6,
                    Unused7,
                    List,
                    Mopp
                }
                
                [TagStructure(Size = 0x8)]
                public class CollisionFilterDatum : TagStructure
                {
                    public ShapeTypeValue ShapeType;
                    public short Shape;
                    public int CollisionFilter;
                }
                
                [Flags]
                public enum FlagsValue : uint
                {
                    HasAabbPhantom = 1 << 0,
                    Unknown = 1 << 1
                }
                
                [TagStructure(Size = 0x10)]
                public class Unknown3Datum : TagStructure
                {
                    public RealVector3d Sphere;
                    [TagField(Length = 0x4)]
                    public byte[] Unknown3;
                }
                
                [TagStructure(Size = 0x10)]
                public class NumSpheresDatum : TagStructure
                {
                    [TagField(Length = 0x4)]
                    public byte[] Unknown3;
                    public short Size1;
                    public short Count1;
                    [TagField(Length = 0x4)]
                    public byte[] Unknown31;
                    public int NumSpheres;
                }
            }
        }
    }
}

