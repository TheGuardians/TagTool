using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "scenery", Tag = "scen", Size = 0xC4)]
    public class Scenery : TagStructure
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
        /// <summary>
        /// Indicate whether, by default, we should create pathfinding data for this type of scenery
        /// </summary>
        public PathfindingPolicyValue PathfindingPolicy;
        public FlagsValue1 Flags1;
        /// <summary>
        /// Indicate whether, by default, how we should lightmap this type of scenery
        /// </summary>
        public LightmappingPolicyValue LightmappingPolicy;
        [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
        public byte[] Padding4;
        
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
        
        public enum PathfindingPolicyValue : short
        {
            PathfindingCutOut,
            PathfindingStatic,
            PathfindingDynamic,
            PathfindingNone
        }
        
        [Flags]
        public enum FlagsValue1 : ushort
        {
            /// <summary>
            /// stimulates?
            /// </summary>
            PhysicallySimulates = 1 << 0
        }
        
        public enum LightmappingPolicyValue : short
        {
            PerVertex,
            PerPixelNotImplemented,
            Dynamic
        }
    }
}

