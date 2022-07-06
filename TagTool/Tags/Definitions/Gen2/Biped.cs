using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Havok;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "biped", Tag = "bipd", Size = 0x314)]
    public class Biped : TagStructure
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
        [TagField(ValidTags = new[] { "hlmt" })]
        public CachedTag Model;
        [TagField(ValidTags = new[] { "bloc" })]
        public CachedTag CrateObject;
        [TagField(ValidTags = new[] { "shad" })]
        public CachedTag ModifierShader;
        [TagField(ValidTags = new[] { "effe" })]
        public CachedTag CreationEffect;
        [TagField(ValidTags = new[] { "foot" })]
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
        public UnitFlagBits UnitFlags;
        public DefaultTeamValue DefaultTeam;
        public ConstantSoundVolumeValue ConstantSoundVolume;
        [TagField(ValidTags = new[] { "effe" })]
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
        [TagField(ValidTags = new[] { "char" })]
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
        [TagField(ValidTags = new[] { "jpt!" })]
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
        public Angle MovingTurningSpeed; // degrees per second
        public BipedDefinitionFlags BipedFlags;
        public Angle StationaryTurningThreshold;
        public float JumpVelocity; // world units per second
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
        /// <summary>
        /// 0 is the default.  Bipeds are stuned when damaged by vehicle collisions, also some are when they take emp damage
        /// </summary>
        public float StunDuration;
        public float StandingCameraHeight; // world units
        public float CrouchingCameraHeight; // world units
        public float CrouchTransitionTime; // seconds
        /// <summary>
        /// looking-downward angle that starts camera interpolation to fp position
        /// </summary>
        public Angle CameraInterpolationStart; // degrees
        /// <summary>
        /// looking-downward angle at which camera interpolation to fp position is complete
        /// </summary>
        public Angle CameraInterpolationEnd; // degrees
        /// <summary>
        /// amount of fp camera movement forward and back (1.0 is full)
        /// </summary>
        public float CameraForwardMovementScale;
        /// <summary>
        /// amount of fp camera movement side-to-side (1.0 is full)
        /// </summary>
        public float CameraSideMovementScale;
        /// <summary>
        /// amount of fp camera movement vertically (1.0 is full)
        /// </summary>
        public float CameraVerticalMovementScale;
        /// <summary>
        /// fp camera must always be at least this far out from root node
        /// </summary>
        public float CameraExclusionDistance; // world units
        public float AutoaimWidth; // world units
        public BipedLockOnDataStructBlock LockOnData;
        [TagField(Length = 0x10, Flags = TagFieldFlags.Padding)]
        public byte[] Padding5;
        /// <summary>
        /// when the biped ragdolls from a head shot it acceleartes based on this value.  0 defaults to the standard acceleration
        /// scale
        /// </summary>
        public float HeadshotAccelerationScale;
        [TagField(ValidTags = new[] { "effe" })]
        public CachedTag AreaDamageEffect;
        public CharacterPhysicsStructBlock Physics;
        /// <summary>
        /// these are the points where the biped touches the ground
        /// </summary>
        public List<ContactPointBlock> ContactPoints;
        /// <summary>
        /// when the flood reanimate this guy, he turns into a ...
        /// </summary>
        [TagField(ValidTags = new[] { "char" })]
        public CachedTag ReanimationCharacter;
        /// <summary>
        /// when I die, out of the ashes of my death crawls a ...
        /// </summary>
        [TagField(ValidTags = new[] { "char" })]
        public CachedTag DeathSpawnCharacter;
        public short DeathSpawnCount;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding6;

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
                public byte[] Data;
            }
        }

        [TagStructure(Size = 0x18)]
        public class ObjectAttachmentBlock : TagStructure
        {
            [TagField(ValidTags = new[] { "ligh", "MGS2", "tdtl", "cont", "effe", "lsnd", "lens" })]
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
            [TagField(ValidTags = new[] { "ant!", "devo", "whip", "BooM", "tdtl" })]
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
        public enum UnitFlagBits : uint
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
                [TagField(ValidTags = new[] { "trak" })]
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
            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag BoardingMeleeDamage;
            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag BoardingMeleeResponse;
            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag LandingMeleeDamage;
            [TagField(ValidTags = new[] { "jpt!" })]
            public CachedTag FlurryMeleeDamage;
            [TagField(ValidTags = new[] { "jpt!" })]
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
            [TagField(ValidTags = new[] { "nhdt" })]
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
            [TagField(ValidTags = new[] { "udlg" })]
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
            [TagField(ValidTags = new[] { "weap" })]
            public CachedTag Weapon;
        }

        [TagStructure(Size = 0xB0)]
        public class UnitSeatBlock : TagStructure
        {
            public FlagsValue Flags;
            public StringId Label;
            public StringId MarkerName;
            public StringId EntryMarkerSName;
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
            [TagField(ValidTags = new[] { "char" })]
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
                    [TagField(ValidTags = new[] { "trak" })]
                    public CachedTag Track;
                }
            }

            [TagStructure(Size = 0x8)]
            public class UnitHudReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new[] { "nhdt" })]
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
        public enum BipedDefinitionFlags : uint
        {
            None,
            TurnsWithoutAnimating = 1 << 0,
            PassesThroughOtherBipeds = 1 << 1,
            ImmuneToFallingDamage = 1 << 2,
            RotateWhileAirborne = 1 << 3,
            UseLimpBodyPhysics = 1 << 4,
            Unused = 1 << 5,
            RandomSpeedIncrease = 1 << 6,
            Unused_ = 1 << 7,
            SpawnDeathChildrenOnDestroy = 1 << 8,
            StunnedByEmpDamage = 1 << 9,
            DeadPhysicsWhenStunned = 1 << 10,
            AlwaysRagdollWhenDead = 1 << 11
        }

        [TagStructure(Size = 0x8)]
        public class BipedLockOnDataStructBlock : TagStructure
        {
            public FlagsValue Flags;
            public float LockOnDistance;

            [Flags]
            public enum FlagsValue : uint
            {
                LockedByHumanTargeting = 1 << 0,
                LockedByPlasmaTargeting = 1 << 1,
                AlwaysLockedByPlasmaTargeting = 1 << 2
            }
        }

        [TagStructure(Size = 0x94)]
        public class CharacterPhysicsStructBlock : TagStructure
        {
            public BipedPhysicsFlags Flags;
            public float HeightStanding;
            public float HeightCrouching;
            public float Radius;
            public float Mass;
            /// <summary>
            /// collision material used when character is alive
            /// </summary>
            public StringId LivingMaterialName;
            /// <summary>
            /// collision material used when character is dead
            /// </summary>
            public StringId DeadMaterialName;
            [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
            public byte[] Padding;
            public List<PhysicsModel.SpheresBlock> DeadSphereShapes;
            public List<PhysicsModel.PillsBlock> PillShapes;
            public List<PhysicsModel.SpheresBlock> SphereShapes;
            public CharacterPhysicsGroundStructBlock GroundPhysics;
            public CharacterPhysicsFlyingStructBlock FlyingPhysics;
            //public CharacterPhysicsDeadStructBlock DeadPhysics;
            //public CharacterPhysicsSentinelStructBlock SentinelPhysics;

            [TagStructure(Size = 0x30)]
            public class CharacterPhysicsGroundStructBlock : TagStructure
            {
                public Angle MaximumSlopeAngle; // degrees
                public Angle DownhillFalloffAngle; // degrees
                public Angle DownhillCutoffAngle; // degrees
                public Angle UphillFalloffAngle; // degrees
                public Angle UphillCutoffAngle; // degrees
                public float DownhillVelocityScale;
                public float UphillVelocityScale;
                public float RuntimeMinimumNormalK;
                public float RuntimeDownhillK0;
                public float RuntimeDownhillK1;
                public float RuntimeUphillK0;
                public float RuntimeUphillK1;
            }

            [TagStructure(Size = 0x2C)]
            public class CharacterPhysicsFlyingStructBlock : TagStructure
            {
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
            }

            [TagStructure()]
            public class CharacterPhysicsDeadStructBlock : TagStructure
            {
            }

            [TagStructure()]
            public class CharacterPhysicsSentinelStructBlock : TagStructure
            {
            }
        }

        [TagStructure(Size = 0x4)]
        public class ContactPointBlock : TagStructure
        {
            public StringId MarkerName;
        }
    }
}

