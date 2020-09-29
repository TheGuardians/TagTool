using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "creature", Tag = "crea", Size = 0x1E0)]
    public class Creature : TagStructure
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
        /// $$$ CREATURE $$$
        /// </summary>
        public FlagsValue Flags1;
        public DefaultTeamValue DefaultTeam;
        public MotionSensorBlipSizeValue MotionSensorBlipSize;
        public Angle TurningVelocityMaximum; // degrees per second
        public Angle TurningAccelerationMaximum; // degrees per second squared
        public float CasualTurningModifier; // [0,1]
        public float AutoaimWidth; // world units
        public CharacterPhysicsDefinition Physics;
        public CachedTag ImpactDamage;
        public CachedTag ImpactShieldDamage; // if not specified, uses 'impact damage'
        /// <summary>
        /// death and destruction
        /// </summary>
        public Bounds<float> DestroyAfterDeathTime; // seconds
        
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
        
        [TagStructure(Size = 0xA0)]
        public class CharacterPhysicsDefinition : TagStructure
        {
            public FlagsValue Flags;
            public float HeightStanding;
            public float HeightCrouching;
            public float Radius;
            public float Mass;
            public StringId LivingMaterialName; // collision material used when character is alive
            public StringId DeadMaterialName; // collision material used when character is dead
            [TagField(Flags = Padding, Length = 4)]
            public byte[] Padding1;
            public List<PhysicsModelSphere> DeadSphereShapes;
            public List<PhysicsModelPill> PillShapes;
            public List<PhysicsModelSphere> SphereShapes;
            /// <summary>
            /// ground
            /// </summary>
            public CharacterPhysicsGroundDefinition GroundPhysics;
            /// <summary>
            /// flying
            /// </summary>
            public CharacterPhysicsFlyingDefinition FlyingPhysics;
            /// <summary>
            /// dead
            /// </summary>
            public CharacterPhysicsDeadStructBlock DeadPhysics;
            /// <summary>
            /// sentinel
            /// </summary>
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
            public class PhysicsModelSphere : TagStructure
            {
                public StringId Name;
                public short Material;
                public FlagsValue Flags;
                public float RelativeMassScale;
                public float Friction;
                public float Restitution;
                public float Volume;
                public float Mass;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown1;
                public short Phantom;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown2;
                public short Size;
                public short Count;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown3;
                public float Radius;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown4;
                public short Size1;
                public short Count2;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown5;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown6;
                public RealVector3d RotationI;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown7;
                public RealVector3d RotationJ;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown8;
                public RealVector3d RotationK;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown9;
                public RealVector3d Translation;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown10;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Unused = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x50)]
            public class PhysicsModelPill : TagStructure
            {
                public StringId Name;
                public short Material;
                public FlagsValue Flags;
                public float RelativeMassScale;
                public float Friction;
                public float Restitution;
                public float Volume;
                public float Mass;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Unknown1;
                public short Phantom;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown2;
                public short Size;
                public short Count;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown3;
                public float Radius;
                public RealVector3d Bottom;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown4;
                public RealVector3d Top;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Unknown5;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    Unused = 1 << 0
                }
            }
            
            [TagStructure(Size = 0x30)]
            public class CharacterPhysicsGroundDefinition : TagStructure
            {
                public Angle MaximumSlopeAngle; // degrees
                public Angle DownhillFalloffAngle; // degrees
                public Angle DownhillCutoffAngle; // degrees
                public Angle UphillFalloffAngle; // degrees
                public Angle UphillCutoffAngle; // degrees
                public float DownhillVelocityScale;
                public float UphillVelocityScale;
                [TagField(Flags = Padding, Length = 20)]
                public byte[] Padding1;
            }
            
            [TagStructure(Size = 0x2C)]
            public class CharacterPhysicsFlyingDefinition : TagStructure
            {
                public Angle BankAngle; // degrees
                public float BankApplyTime; // seconds
                public float BankDecayTime; // seconds
                public float PitchRatio; // amount that we pitch up/down when moving up or down
                public float MaxVelocity; // world units per second
                public float MaxSidestepVelocity; // world units per second
                public float Acceleration; // world units per second squared
                public float Deceleration; // world units per second squared
                public Angle AngularVelocityMaximum; // degrees per second
                public Angle AngularAccelerationMaximum; // degrees per second squared
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

