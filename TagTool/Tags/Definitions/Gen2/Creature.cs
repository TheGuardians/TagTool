using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "creature", Tag = "crea", Size = 0x180)]
    public class Creature : TagStructure
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
        public MotionSensorBlipSizeValue MotionSensorBlipSize;
        public Angle TurningVelocityMaximum; // degrees per second
        public Angle TurningAccelerationMaximum; // degrees per second squared
        public float CasualTurningModifier; // [0,1]
        public float AutoaimWidth; // world units
        public CharacterPhysicsStructBlock Physics;
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag ImpactDamage;
        /// <summary>
        /// if not specified, uses 'impact damage'
        /// </summary>
        [TagField(ValidTags = new [] { "jpt!" })]
        public CachedTag ImpactShieldDamage;
        /// <summary>
        /// if non-zero, the creature will destroy itself upon death after this much time
        /// </summary>
        public Bounds<float> DestroyAfterDeathTime; // seconds
        
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
            Unused = 1 << 0,
            InfectionForm = 1 << 1,
            ImmuneToFallingDamage = 1 << 2,
            RotateWhileAirborne = 1 << 3,
            ZappedByShields = 1 << 4,
            AttachUponImpact = 1 << 5,
            NotOnMotionSensor = 1 << 6
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
        
        public enum MotionSensorBlipSizeValue : short
        {
            Medium,
            Small,
            Large
        }
        
        [TagStructure(Size = 0x94)]
        public class CharacterPhysicsStructBlock : TagStructure
        {
            public FlagsValue Flags;
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
            public List<SpheresBlock> DeadSphereShapes;
            public List<PillsBlock> PillShapes;
            public List<SpheresBlock1> SphereShapes;
            public CharacterPhysicsGroundStructBlock GroundPhysics;
            public CharacterPhysicsFlyingStructBlock FlyingPhysics;
            public CharacterPhysicsDeadStructBlock DeadPhysics;
            public CharacterPhysicsSentinelStructBlock SentinelPhysics;
            
            [Flags]
            public enum FlagsValue : uint
            {
                CenteredAtOrigin = 1 << 0,
                ShapeSpherical = 1 << 1,
                UsePlayerPhysics = 1 << 2,
                ClimbAnySurface = 1 << 3,
                Flying = 1 << 4,
                NotPhysical = 1 << 5,
                DeadCharacterCollisionGroup = 1 << 6
            }
            
            [TagStructure(Size = 0x80)]
            public class SpheresBlock : TagStructure
            {
                public StringId Name;
                public short Material;
                public FlagsValue Flags;
                public float RelativeMassScale;
                public float Friction;
                public float Restitution;
                public float Volume;
                public float Mass;
                [TagField(Length = 0x2)]
                public byte[] Unknown;
                public short Phantom;
                [TagField(Length = 0x4)]
                public byte[] Unknown1;
                public short Size;
                public short Count;
                [TagField(Length = 0x4)]
                public byte[] Unknown2;
                public float Radius;
                [TagField(Length = 0x4)]
                public byte[] Unknown3;
                public short Size1;
                public short Count1;
                [TagField(Length = 0x4)]
                public byte[] Unknown4;
                [TagField(Length = 0x4)]
                public byte[] Unknown5;
                public RealVector3d RotationI;
                [TagField(Length = 0x4)]
                public byte[] Unknown6;
                public RealVector3d RotationJ;
                [TagField(Length = 0x4)]
                public byte[] Unknown7;
                public RealVector3d RotationK;
                [TagField(Length = 0x4)]
                public byte[] Unknown8;
                public RealVector3d Translation;
                [TagField(Length = 0x4)]
                public byte[] Unknown9;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Unused = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x50)]
            public class PillsBlock : TagStructure
            {
                public StringId Name;
                public short Material;
                public FlagsValue Flags;
                public float RelativeMassScale;
                public float Friction;
                public float Restitution;
                public float Volume;
                public float Mass;
                [TagField(Length = 0x2)]
                public byte[] Unknown;
                public short Phantom;
                [TagField(Length = 0x4)]
                public byte[] Unknown1;
                public short Size;
                public short Count;
                [TagField(Length = 0x4)]
                public byte[] Unknown2;
                public float Radius;
                public RealVector3d Bottom;
                [TagField(Length = 0x4)]
                public byte[] Unknown3;
                public RealVector3d Top;
                [TagField(Length = 0x4)]
                public byte[] Unknown4;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Unused = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x80)]
            public class SpheresBlock1 : TagStructure
            {
                public StringId Name;
                public short Material;
                public FlagsValue Flags;
                public float RelativeMassScale;
                public float Friction;
                public float Restitution;
                public float Volume;
                public float Mass;
                [TagField(Length = 0x2)]
                public byte[] Unknown;
                public short Phantom;
                [TagField(Length = 0x4)]
                public byte[] Unknown1;
                public short Size;
                public short Count;
                [TagField(Length = 0x4)]
                public byte[] Unknown2;
                public float Radius;
                [TagField(Length = 0x4)]
                public byte[] Unknown3;
                public short Size1;
                public short Count1;
                [TagField(Length = 0x4)]
                public byte[] Unknown4;
                [TagField(Length = 0x4)]
                public byte[] Unknown5;
                public RealVector3d RotationI;
                [TagField(Length = 0x4)]
                public byte[] Unknown6;
                public RealVector3d RotationJ;
                [TagField(Length = 0x4)]
                public byte[] Unknown7;
                public RealVector3d RotationK;
                [TagField(Length = 0x4)]
                public byte[] Unknown8;
                public RealVector3d Translation;
                [TagField(Length = 0x4)]
                public byte[] Unknown9;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Unused = 1 << 0
                }
            }
            
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
                [TagField(Length = 0x14, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
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
    }
}

