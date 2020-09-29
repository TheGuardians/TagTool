using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "equipment", Tag = "eqip", Size = 0x1B4)]
    public class Equipment : TagStructure
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
        public PowerupTypeValue PowerupType;
        public GrenadeTypeValue GrenadeType;
        public float PowerupTime; // seconds
        public CachedTag PickupSound;
        
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
        
        public enum PowerupTypeValue : short
        {
            None,
            DoubleSpeed,
            OverShield,
            ActiveCamouflage,
            FullSpectrumVision,
            Health,
            Grenade
        }
        
        public enum GrenadeTypeValue : short
        {
            HumanFragmentation,
            CovenantPlasma
        }
    }
}

