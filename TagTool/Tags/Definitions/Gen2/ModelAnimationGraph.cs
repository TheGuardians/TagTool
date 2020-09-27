using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen2
{
    [TagStructure(Name = "model_animation_graph", Tag = "jmad", Size = 0xEC)]
    public class ModelAnimationGraph : TagStructure
    {
        public AnimationGraphResources Resources;
        public ModelAnimationGraphContents Content;
        public ModelAnimationRuntimeData RunTimeData;
        /// <summary>
        /// RESULTS OF THE LAST IMPORT
        /// </summary>
        public byte[] LastImportResults;
        public List<AdditionalNodeDataBlock> AdditionalNodeData;
        
        [TagStructure(Size = 0x50)]
        public class AnimationGraphResources : TagStructure
        {
            /// <summary>
            /// GRAPH DATA
            /// </summary>
            public CachedTag ParentAnimationGraph;
            public InheritanceFlagsValue InheritanceFlags;
            public PrivateFlagsValue PrivateFlags;
            public short AnimationCodecPack;
            public List<AnimationGraphNode> SkeletonNodesAbcdcc;
            public List<AnimationGraphSoundReference> SoundReferencesAbcdcc;
            public List<AnimationGraphEffectReference> EffectReferencesAbcdcc;
            public List<AnimationBlendScreen> BlendScreensAbcdcc;
            public List<ModelAnimation> AnimationsAbcdcc;
            
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
            public class AnimationGraphNode : TagStructure
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
            
            [TagStructure(Size = 0x14)]
            public class AnimationGraphSoundReference : TagStructure
            {
                public CachedTag Sound;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                
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
            
            [TagStructure(Size = 0x14)]
            public class AnimationGraphEffectReference : TagStructure
            {
                public CachedTag Effect;
                public FlagsValue Flags;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                
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
            public class AnimationBlendScreen : TagStructure
            {
                public StringId Label;
                public AnimationAimingScreenBounds AimingScreen;
                
                [TagStructure(Size = 0x18)]
                public class AnimationAimingScreenBounds : TagStructure
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
            
            [TagStructure(Size = 0x7C)]
            public class ModelAnimation : TagStructure
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
                public short LoopFrameIndex;
                public short Unknown1;
                public short Unknown2;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public byte[] Unknown3;
                public AnimationDataSizes Unknown4;
                public List<FrameEvent> FrameEventsAbcdcc;
                public List<SoundEvent> SoundEventsAbcdcc;
                public List<EffectEvent> EffectEventsAbcdcc;
                public List<ObjectSpaceNodeData> ObjectSpaceParentNodesAbcdcc;
                
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
                public class AnimationDataSizes : TagStructure
                {
                    public sbyte Unknown1;
                    public sbyte Unknown2;
                    public short Unknown3;
                    public short Unknown4;
                    public short Unknown5;
                    public int Unknown6;
                    public int Unknown7;
                }
                
                [TagStructure(Size = 0x4)]
                public class FrameEvent : TagStructure
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
                public class SoundEvent : TagStructure
                {
                    public short Sound;
                    public short Frame;
                    public StringId MarkerName;
                }
                
                [TagStructure(Size = 0x4)]
                public class EffectEvent : TagStructure
                {
                    public short Effect;
                    public short Frame;
                }
                
                [TagStructure(Size = 0x1C)]
                public class ObjectSpaceNodeData : TagStructure
                {
                    public short NodeIndex;
                    public ComponentFlagsValue ComponentFlags;
                    public QuantizedOrientation Orientation;
                    
                    [Flags]
                    public enum ComponentFlagsValue : ushort
                    {
                        Rotation = 1 << 0,
                        Translation = 1 << 1,
                        Scale = 1 << 2
                    }
                    
                    [TagStructure(Size = 0x18)]
                    public class QuantizedOrientation : TagStructure
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
        
        [TagStructure(Size = 0x24)]
        public class ModelAnimationGraphContents : TagStructure
        {
            /// <summary>
            /// MODE-n-STATE GRAPH
            /// </summary>
            public List<AnimationMode> ModesAabbcc;
            /// <summary>
            /// SPECIAL CASE ANIMS
            /// </summary>
            public List<VehicleSuspension> VehicleSuspensionCcaabb;
            public List<ObjectOverlay> ObjectOverlaysCcaabb;
            
            [TagStructure(Size = 0x1C)]
            public class AnimationMode : TagStructure
            {
                public StringId Label;
                public List<WeaponClass> WeaponClassAabbcc;
                public List<AnimationIkPoint> ModeIkAabbcc;
                
                [TagStructure(Size = 0x1C)]
                public class WeaponClass : TagStructure
                {
                    public StringId Label;
                    public List<WeaponType> WeaponTypeAabbcc;
                    public List<AnimationIkPoint> WeaponIkAabbcc;
                    
                    [TagStructure(Size = 0x4C)]
                    public class WeaponType : TagStructure
                    {
                        public StringId Label;
                        public List<AnimationEntry> ActionsAabbcc;
                        public List<AnimationEntry> OverlaysAabbcc;
                        public List<AnimationDamageActions> DeathAndDamageAabbcc;
                        public List<AnimationTransitionSource> TransitionsAabbcc;
                        public List<HighPrecacheCccccBlock> HighPrecacheCcccc;
                        public List<LowPrecacheCccccBlock> LowPrecacheCcccc;
                        
                        [TagStructure(Size = 0x8)]
                        public class AnimationEntry : TagStructure
                        {
                            public StringId Label;
                            public AnimationId Animation;
                            
                            [TagStructure(Size = 0x4)]
                            public class AnimationId : TagStructure
                            {
                                public short GraphIndex;
                                public short Animation;
                            }
                        }
                        
                        [TagStructure(Size = 0x10)]
                        public class AnimationDamageActions : TagStructure
                        {
                            public StringId Label;
                            public List<AnimationDamageDirection> DirectionsAabbcc;
                            
                            [TagStructure(Size = 0xC)]
                            public class AnimationDamageDirection : TagStructure
                            {
                                public List<AnimationId> RegionsAabbcc;
                                
                                [TagStructure(Size = 0x4)]
                                public class AnimationId : TagStructure
                                {
                                    public AnimationId Animation;
                                }
                            }
                        }
                        
                        [TagStructure(Size = 0x18)]
                        public class AnimationTransitionSource : TagStructure
                        {
                            public StringId FullName; // name of the mode & state of the source
                            public AnimationTransitionState StateInfo;
                            public List<AnimationTransitionDestination> DestinationsAabbcc;
                            
                            [TagStructure(Size = 0x8)]
                            public class AnimationTransitionState : TagStructure
                            {
                                public StringId StateName; // name of the state
                                [TagField(Flags = Padding, Length = 2)]
                                public byte[] Padding1;
                                public sbyte IndexA; // first level sub-index into state
                                public sbyte IndexB; // second level sub-index into state
                            }
                            
                            [TagStructure(Size = 0x14)]
                            public class AnimationTransitionDestination : TagStructure
                            {
                                public StringId FullName; // name of the mode & state this transitions to
                                public StringId Mode; // name of the mode
                                public AnimationTransitionState StateInfo;
                                public AnimationId Animation;
                                
                                [TagStructure(Size = 0x8)]
                                public class AnimationTransitionState : TagStructure
                                {
                                    public StringId StateName; // name of the state
                                    public FrameEventLinkValue FrameEventLink; // which frame event to link to
                                    [TagField(Flags = Padding, Length = 1)]
                                    public byte[] Padding1;
                                    public sbyte IndexA; // first level sub-index into state
                                    public sbyte IndexB; // second level sub-index into state
                                    
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
                                public class AnimationId : TagStructure
                                {
                                    public short GraphIndex;
                                    public short Animation;
                                }
                            }
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class HighPrecacheCccccBlock : TagStructure
                        {
                            public int CacheBlockIndex;
                        }
                        
                        [TagStructure(Size = 0x4)]
                        public class LowPrecacheCccccBlock : TagStructure
                        {
                            public int CacheBlockIndex;
                        }
                    }
                    
                    [TagStructure(Size = 0x8)]
                    public class AnimationIkPoint : TagStructure
                    {
                        public StringId Marker; // the marker name on the object being attached
                        public StringId AttachToMarker; // the marker name object (weapon, vehicle, etc.) the above marker is being attached to
                    }
                }
                
                [TagStructure(Size = 0x8)]
                public class AnimationIkPoint : TagStructure
                {
                    public StringId Marker; // the marker name on the object being attached
                    public StringId AttachToMarker; // the marker name object (weapon, vehicle, etc.) the above marker is being attached to
                }
            }
            
            [TagStructure(Size = 0x28)]
            public class VehicleSuspension : TagStructure
            {
                public StringId Label;
                public AnimationId Animation;
                public StringId MarkerName;
                public float MassPointOffset;
                public float FullExtensionGroundDepth;
                public float FullCompressionGroundDepth;
                /// <summary>
                /// Destroyed Suspension
                /// </summary>
                /// <remarks>
                /// Only Necessary for suspensions with a destroyed state
                /// </remarks>
                public StringId RegionName;
                public float DestroyedMassPointOffset;
                public float DestroyedFullExtensionGroundDepth;
                public float DestroyedFullCompressionGroundDepth;
                
                [TagStructure(Size = 0x4)]
                public class AnimationId : TagStructure
                {
                    public short GraphIndex;
                    public short Animation;
                }
            }
            
            [TagStructure(Size = 0x14)]
            public class ObjectOverlay : TagStructure
            {
                public StringId Label;
                public AnimationId Animation;
                [TagField(Flags = Padding, Length = 2)]
                public byte[] Padding1;
                public FunctionControlsValue FunctionControls;
                public StringId Function;
                [TagField(Flags = Padding, Length = 4)]
                public byte[] Padding2;
                
                [TagStructure(Size = 0x4)]
                public class AnimationId : TagStructure
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
        
        [TagStructure(Size = 0x58)]
        public class ModelAnimationRuntimeData : TagStructure
        {
            /// <summary>
            /// RUN-TIME DATA
            /// </summary>
            public List<AnimationInheritence> InheritenceListBbaaaa;
            public List<WeaponClassListing> WeaponListBbaaaa;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding1;
            [TagField(Flags = Padding, Length = 32)]
            public byte[] Padding2;
            
            [TagStructure(Size = 0x30)]
            public class AnimationInheritence : TagStructure
            {
                public CachedTag InheritedGraph;
                public List<NodeMapBlock> NodeMap;
                public List<NodeMapFlagsBlock> NodeMapFlags;
                public float RootZOffset;
                public int InheritanceFlags;
                
                [TagStructure(Size = 0x2)]
                public class NodeMapBlock : TagStructure
                {
                    public short LocalNode;
                }
                
                [TagStructure(Size = 0x4)]
                public class NodeMapFlagsBlock : TagStructure
                {
                    public int LocalNodeFlags;
                }
            }
            
            [TagStructure(Size = 0x8)]
            public class WeaponClassListing : TagStructure
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
    }
}

