using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "model_animation_graph", Tag = "jmad", Size = 0xAC, MinVersion = CacheVersion.Halo2Vista)]
    [TagStructure(Name = "model_animation_graph", Tag = "jmad", Size = 0xB4, MaxVersion = CacheVersion.Halo2Xbox)]
    public class ModelAnimationGraph : TagStructure
    {
        public AnimationGraphResourcesStructBlock Resources;
        public AnimationGraphContentsStructBlock Content;
        public ModelAnimationRuntimeDataStructBlock RunTimeData;
        public byte[] LastImportResults;
        public List<AdditionalNodeDataBlock> AdditionalNodeData;
        [TagField(MaxVersion = CacheVersion.Halo2Xbox)]
        public List<AnimationRawData> AnimationData;
        
        [TagStructure(Size = 0x34)]
        public class AnimationGraphResourcesStructBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "jmad" })]
            public CachedTag ParentAnimationGraph;
            public InheritanceFlagsValue InheritanceFlags;
            public PrivateFlagsValue PrivateFlags;
            public short AnimationCodecPack;
            public List<AnimationGraphNodeBlock> SkeletonNodesAbcdcc;
            public List<AnimationGraphSoundReferenceBlock> SoundReferencesAbcdcc;
            public List<AnimationGraphEffectReferenceBlock> EffectReferencesAbcdcc;
            public List<AnimationBlendScreenBlock> BlendScreensAbcdcc;
            public List<AnimationPoolBlock> AnimationsAbcdcc;
            
            [Flags]
            public enum InheritanceFlagsValue : byte
            {
                InheritRootTransScaleOnly = 1 << 0,
                InheritForUseOnPlayer = 1 << 1
            }
            
            [Flags]
            public enum PrivateFlagsValue : byte
            {
                PreparedForCache = 1 << 0,
                Unused = 1 << 1,
                ImportedWithCodecCompressors = 1 << 2,
                UnusedSmellyFlag = 1 << 3,
                WrittenToCache = 1 << 4,
                AnimationDataReordered = 1 << 5
            }
            
            [TagStructure(Size = 0x20)]
            public class AnimationGraphNodeBlock : TagStructure
            {
                public StringId Name;
                public short NextSiblingNodeIndex;
                public short FirstChildNodeIndex;
                public short ParentNodeIndex;
                public ModelFlagsValue ModelFlags;
                public NodeJointFlagsValue NodeJointFlags;
                public RealVector3d BaseVector;
                public float VectorRange;
                public float ZPos;
                
                [Flags]
                public enum ModelFlagsValue : byte
                {
                    PrimaryModel = 1 << 0,
                    SecondaryModel = 1 << 1,
                    LocalRoot = 1 << 2,
                    LeftHand = 1 << 3,
                    RightHand = 1 << 4,
                    LeftArmMember = 1 << 5
                }
                
                [Flags]
                public enum NodeJointFlagsValue : byte
                {
                    BallSocket = 1 << 0,
                    Hinge = 1 << 1,
                    NoMovement = 1 << 2
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class AnimationGraphSoundReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "snd!" })]
                public CachedTag Sound;
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    AllowOnPlayer = 1 << 0,
                    LeftArmOnly = 1 << 1,
                    RightArmOnly = 1 << 2,
                    FirstPersonOnly = 1 << 3,
                    ForwardOnly = 1 << 4,
                    ReverseOnly = 1 << 5
                }
            }
            
            [TagStructure(Size = 0xC)]
            public class AnimationGraphEffectReferenceBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "effe" })]
                public CachedTag Effect;
                public FlagsValue Flags;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                
                [Flags]
                public enum FlagsValue : ushort
                {
                    AllowOnPlayer = 1 << 0,
                    LeftArmOnly = 1 << 1,
                    RightArmOnly = 1 << 2,
                    FirstPersonOnly = 1 << 3,
                    ForwardOnly = 1 << 4,
                    ReverseOnly = 1 << 5
                }
            }
            
            [TagStructure(Size = 0x1C)]
            public class AnimationBlendScreenBlock : TagStructure
            {
                public StringId Label;
                public AnimationAimingScreenStructBlock AimingScreen;
                
                [TagStructure(Size = 0x18)]
                public class AnimationAimingScreenStructBlock : TagStructure
                {
                    public Angle RightYawPerFrame;
                    public Angle LeftYawPerFrame;
                    public short RightFrameCount;
                    public short LeftFrameCount;
                    public Angle DownPitchPerFrame;
                    public Angle UpPitchPerFrame;
                    public short DownPitchFrameCount;
                    public short UpPitchFrameCount;
                }
            }
            
            [TagStructure(Size = 0x60, MinVersion = CacheVersion.Halo2Vista)]
            [TagStructure(Size = 0x6C, MaxVersion = CacheVersion.Halo2Xbox)]
            public class AnimationPoolBlock : TagStructure
            {
                public StringId Name;
                public int NodeListChecksum;
                public int ProductionChecksum;
                public int ImportChecksum;
                public TypeValue Type;
                public FrameInfoTypeValue FrameInfoType;
                public sbyte BlendScreen;
                public sbyte NodeCount;
                public short FrameCount;
                public InternalFlagsValue InternalFlags;
                public ProductionFlagsValue ProductionFlags;
                public PlaybackFlagsValue PlaybackFlags;
                public DesiredCompressionValue DesiredCompression;
                public CurrentCompressionValue CurrentCompression;
                public float Weight;

                [TagField(MaxVersion = CacheVersion.Halo2Xbox)]
                public int ResourceParentGraphIndex;
                [TagField(MaxVersion = CacheVersion.Halo2Xbox)]
                public int ResourceIndex;
                [TagField(MaxVersion = CacheVersion.Halo2Xbox)]
                public int ResourceBlockOffset;
                [TagField(MaxVersion = CacheVersion.Halo2Xbox)]
                public short ResourceUnknown;

                public short LoopFrameIndex;
                public short PreviousVariantSibling;
                public short NextVariantSibling;

                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding, MinVersion = CacheVersion.Halo2Vista)]
                public byte[] Padding;

                public byte[] AnimationData;
                public PackedDataSizesStructBlock DataSizes;
                public List<AnimationFrameEventBlock> FrameEventsAbcdcc;
                public List<AnimationSoundEventBlock> SoundEventsAbcdcc;
                public List<AnimationEffectEventBlock> EffectEventsAbcdcc;
                public List<ObjectSpaceNodeDataBlock> ObjectSpaceParentNodesAbcdcc;
                
                public enum TypeValue : sbyte
                {
                    Base,
                    Overlay,
                    Replacement
                }
                
                public enum FrameInfoTypeValue : sbyte
                {
                    None,
                    DxDy,
                    DxDyDyaw,
                    DxDyDzDyaw
                }
                
                [Flags]
                public enum InternalFlagsValue : byte
                {
                    Unused0 = 1 << 0,
                    WorldRelative = 1 << 1,
                    Unused1 = 1 << 2,
                    Unused2 = 1 << 3,
                    Unused3 = 1 << 4,
                    CompressionDisabled = 1 << 5,
                    OldProductionChecksum = 1 << 6,
                    ValidProductionChecksum = 1 << 7
                }
                
                [Flags]
                public enum ProductionFlagsValue : byte
                {
                    DoNotMonitorChanges = 1 << 0,
                    VerifySoundEvents = 1 << 1,
                    DoNotInheritForPlayerGraphs = 1 << 2
                }
                
                [Flags]
                public enum PlaybackFlagsValue : ushort
                {
                    DisableInterpolationIn = 1 << 0,
                    DisableInterpolationOut = 1 << 1,
                    DisableModeIk = 1 << 2,
                    DisableWeaponIk = 1 << 3,
                    DisableWeaponAim1stPerson = 1 << 4,
                    DisableLookScreen = 1 << 5,
                    DisableTransitionAdjustment = 1 << 6
                }
                
                public enum DesiredCompressionValue : sbyte
                {
                    BestScore,
                    BestCompression,
                    BestAccuracy,
                    BestFullframe,
                    BestSmallKeyframe,
                    BestLargeKeyframe
                }
                
                public enum CurrentCompressionValue : sbyte
                {
                    BestScore,
                    BestCompression,
                    BestAccuracy,
                    BestFullframe,
                    BestSmallKeyframe,
                    BestLargeKeyframe
                }
                
                [TagStructure(Size = 0x10)]
                public class PackedDataSizesStructBlock : TagStructure
                {
                    public byte StaticNodeFlags;
                    public byte AnimatedNodeFlags;
                    public short MovementData;
                    public short PillOffsetData;
                    public short StaticDataSize;
                    public int UncompressedDataSize;
                    public int CompressedDataSize;
                }
                
                [TagStructure(Size = 0x4)]
                public class AnimationFrameEventBlock : TagStructure
                {
                    public TypeValue Type;
                    public short Frame;
                    
                    public enum TypeValue : short
                    {
                        PrimaryKeyframe,
                        SecondaryKeyframe,
                        LeftFoot,
                        RightFoot,
                        AllowInterruption,
                        TransitionA,
                        TransitionB,
                        TransitionC,
                        TransitionD,
                        BothFeetShuffle,
                        BodyImpact
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class AnimationSoundEventBlock : TagStructure
                {
                    public short Sound;
                    public short Frame;
                    public StringId MarkerName;
                }
                
                [TagStructure(Size = 0x4)]
                public class AnimationEffectEventBlock : TagStructure
                {
                    public short Effect;
                    public short Frame;
                }
                
                [TagStructure(Size = 0x1C)]
                public class ObjectSpaceNodeDataBlock : TagStructure
                {
                    public short NodeIndex;
                    public ComponentFlagsValue ComponentFlags;
                    public QuantizedOrientationStructBlock Orientation;
                    
                    [Flags]
                    public enum ComponentFlagsValue : ushort
                    {
                        Rotation = 1 << 0,
                        Translation = 1 << 1,
                        Scale = 1 << 2
                    }
                    
                    [TagStructure(Size = 0x18)]
                    public class QuantizedOrientationStructBlock : TagStructure
                    {
                        public short RotationX;
                        public short RotationY;
                        public short RotationZ;
                        public short RotationW;
                        public RealPoint3d DefaultTranslation;
                        public float DefaultScale;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x18)]
        public class AnimationGraphContentsStructBlock : TagStructure
        {
            public List<AnimationModeBlock> ModesAabbcc;
            public List<VehicleSuspensionBlock> VehicleSuspensionCcaabb;
            public List<ObjectAnimationBlock> ObjectOverlaysCcaabb;
            
            [TagStructure(Size = 0x14)]
            public class AnimationModeBlock : TagStructure
            {
                public StringId Label;
                public List<WeaponClassBlock> WeaponClassAabbcc;
                public List<AnimationIkBlock> ModeIkAabbcc;
                
                [TagStructure(Size = 0x14)]
                public class WeaponClassBlock : TagStructure
                {
                    public StringId Label;
                    public List<WeaponTypeBlock> WeaponTypeAabbcc;
                    public List<AnimationIkBlock> WeaponIkAabbcc;
                    
                    [TagStructure(Size = 0x34)]
                    public class WeaponTypeBlock : TagStructure
                    {
                        public StringId Label;
                        public List<AnimationEntryBlock> ActionsAabbcc;
                        public List<AnimationEntryBlock1> OverlaysAabbcc;
                        public List<DamageAnimationBlock> DeathAndDamageAabbcc;
                        public List<AnimationTransitionBlock> TransitionsAabbcc;
                        public List<PrecacheListBlock> HighPrecacheCcccc;
                        public List<PrecacheListBlock1> LowPrecacheCcccc;
                        
                        [TagStructure(Size = 0x8)]
                        public class AnimationEntryBlock : TagStructure
                        {
                            public StringId Label;
                            public AnimationIndexStructBlock Animation;
                            
                            [TagStructure(Size = 0x4)]
                            public class AnimationIndexStructBlock : TagStructure
                            {
                                public short GraphIndex;
                                public short Animation;
                            }
                        }
                        
                        [TagStructure(Size = 0x8)]
                        public class AnimationEntryBlock1 : TagStructure
                        {
                            public StringId Label;
                            public AnimationIndexStructBlock Animation;
                            
                            [TagStructure(Size = 0x4)]
                            public class AnimationIndexStructBlock : TagStructure
                            {
                                public short GraphIndex;
                                public short Animation;
                            }
                        }
                        
                        [TagStructure(Size = 0xC)]
                        public class DamageAnimationBlock : TagStructure
                        {
                            public StringId Label;
                            public List<DamageDirectionBlock> DirectionsAabbcc;
                            
                            [TagStructure(Size = 0x8)]
                            public class DamageDirectionBlock : TagStructure
                            {
                                public List<DamageRegionBlock> RegionsAabbcc;
                                
                                [TagStructure(Size = 0x4)]
                                public class DamageRegionBlock : TagStructure
                                {
                                    public AnimationIndexStructBlock Animation;
                                    
                                    [TagStructure(Size = 0x4)]
                                    public class AnimationIndexStructBlock : TagStructure
                                    {
                                        public short GraphIndex;
                                        public short Animation;
                                    }
                                }
                            }
                        }
                        
                        [TagStructure(Size = 0x14)]
                        public class AnimationTransitionBlock : TagStructure
                        {
                            /// <summary>
                            /// name of the mode &amp; state of the source
                            /// </summary>
                            public StringId FullName;
                            public AnimationTransitionStateStructBlock StateInfo;
                            public List<AnimationTransitionDestinationBlock> DestinationsAabbcc;
                            
                            [TagStructure(Size = 0x8)]
                            public class AnimationTransitionStateStructBlock : TagStructure
                            {
                                /// <summary>
                                /// name of the state
                                /// </summary>
                                public StringId StateName;
                                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                                public byte[] Padding;
                                /// <summary>
                                /// first level sub-index into state
                                /// </summary>
                                public sbyte IndexA;
                                /// <summary>
                                /// second level sub-index into state
                                /// </summary>
                                public sbyte IndexB;
                            }
                            
                            [TagStructure(Size = 0x14)]
                            public class AnimationTransitionDestinationBlock : TagStructure
                            {
                                /// <summary>
                                /// name of the mode &amp; state this transitions to
                                /// </summary>
                                public StringId FullName;
                                /// <summary>
                                /// name of the mode
                                /// </summary>
                                public StringId Mode;
                                public AnimationDestinationStateStructBlock StateInfo;
                                public AnimationIndexStructBlock Animation;
                                
                                [TagStructure(Size = 0x8)]
                                public class AnimationDestinationStateStructBlock : TagStructure
                                {
                                    /// <summary>
                                    /// name of the state
                                    /// </summary>
                                    public StringId StateName;
                                    /// <summary>
                                    /// which frame event to link to
                                    /// </summary>
                                    public FrameEventLinkValue FrameEventLink;
                                    [TagField(Length = 0x1, Flags = TagFieldFlags.Padding)]
                                    public byte[] Padding;
                                    /// <summary>
                                    /// first level sub-index into state
                                    /// </summary>
                                    public sbyte IndexA;
                                    /// <summary>
                                    /// second level sub-index into state
                                    /// </summary>
                                    public sbyte IndexB;
                                    
                                    public enum FrameEventLinkValue : sbyte
                                    {
                                        NoKeyframe,
                                        KeyframeTypeA,
                                        KeyframeTypeB,
                                        KeyframeTypeC,
                                        KeyframeTypeD
                                    }
                                }
                                
                                [TagStructure(Size = 0x4)]
                                public class AnimationIndexStructBlock : TagStructure
                                {
                                    public short GraphIndex;
                                    public short Animation;
                                }
                            }
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class PrecacheListBlock : TagStructure
                        {
                            public int CacheBlockIndex;
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class PrecacheListBlock1 : TagStructure
                        {
                            public int CacheBlockIndex;
                        }
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class AnimationIkBlock : TagStructure
                    {
                        /// <summary>
                        /// the marker name on the object being attached
                        /// </summary>
                        public StringId Marker;
                        /// <summary>
                        /// the marker name object (weapon, vehicle, etc.) the above marker is being attached to
                        /// </summary>
                        public StringId AttachToMarker;
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class AnimationIkBlock : TagStructure
                {
                    /// <summary>
                    /// the marker name on the object being attached
                    /// </summary>
                    public StringId Marker;
                    /// <summary>
                    /// the marker name object (weapon, vehicle, etc.) the above marker is being attached to
                    /// </summary>
                    public StringId AttachToMarker;
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class VehicleSuspensionBlock : TagStructure
            {
                public StringId Label;
                public AnimationIndexStructBlock Animation;
                public StringId MarkerName;
                public float MassPointOffset;
                public float FullExtensionGroundDepth;
                public float FullCompressionGroundDepth;
                /// <summary>
                /// Only Necessary for suspensions with a destroyed state
                /// </summary>
                public StringId RegionName;
                public float DestroyedMassPointOffset;
                public float DestroyedFullExtensionGroundDepth;
                public float DestroyedFullCompressionGroundDepth;
                
                [TagStructure(Size = 0x4)]
                public class AnimationIndexStructBlock : TagStructure
                {
                    public short GraphIndex;
                    public short Animation;
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class ObjectAnimationBlock : TagStructure
            {
                public StringId Label;
                public AnimationIndexStructBlock Animation;
                [TagField(Length = 0x2, Flags = TagFieldFlags.Padding)]
                public byte[] Padding;
                public FunctionControlsValue FunctionControls;
                public StringId Function;
                [TagField(Length = 0x4, Flags = TagFieldFlags.Padding)]
                public byte[] Padding1;
                
                [TagStructure(Size = 0x4)]
                public class AnimationIndexStructBlock : TagStructure
                {
                    public short GraphIndex;
                    public short Animation;
                }
                
                public enum FunctionControlsValue : short
                {
                    Frame,
                    Scale
                }
            }
        }
        
        [TagStructure(Size = 0x50)]
        public class ModelAnimationRuntimeDataStructBlock : TagStructure
        {
            public List<InheritedAnimationBlock> InheritenceListBbaaaa;
            public List<WeaponClassLookupBlock> WeaponListBbaaaa;
            [TagField(Length = 8)]
            public uint[] LeftArmNodes;
            [TagField(Length = 8)]
            public uint[] RightArmNodes;

            [TagStructure(Size = 0x20)]
            public class InheritedAnimationBlock : TagStructure
            {
                [TagField(ValidTags = new [] { "jmad" })]
                public CachedTag InheritedGraph;
                public List<InheritedAnimationNodeMapBlock> NodeMap;
                public List<InheritedAnimationNodeMapFlagBlock> NodeMapFlags;
                public float RootZOffset;
                public int InheritanceFlags;
                
                [TagStructure(Size = 0x2)]
                public class InheritedAnimationNodeMapBlock : TagStructure
                {
                    public short LocalNode;
                }
                
                [TagStructure(Size = 0x4)]
                public class InheritedAnimationNodeMapFlagBlock : TagStructure
                {
                    public int LocalNodeFlags;
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class WeaponClassLookupBlock : TagStructure
            {
                public StringId WeaponName;
                public StringId WeaponClass;
            }
        }
        
        [TagStructure(Size = 0x3C)]
        public class AdditionalNodeDataBlock : TagStructure
        {
            public StringId NodeName;
            public RealQuaternion DefaultRotation;
            public RealPoint3d DefaultTranslation;
            public float DefaultScale;
            public RealPoint3d MinBounds;
            public RealPoint3d MaxBounds;
        }

        [TagStructure(Size = 0x14)]
        public class AnimationRawData : TagStructure
        {
            public int OwnerTagIndex;
            public int DataSize;
            public uint RawDataOffset;
            [TagField(Length = 0x8)]
            public byte[] pad = new byte[0x8];
        }
    }
}

