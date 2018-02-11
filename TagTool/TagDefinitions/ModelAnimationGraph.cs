using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using System;
using System.Collections.Generic;

namespace TagTool.TagDefinitions
{
    [TagStructure(Name = "model_animation_graph", Tag = "jmad", Size = 0x104)]
    public class ModelAnimationGraph
    {
        public CachedTagInstance ParentAnimationGraph;
        public AnimationInheritanceFlags InheritanceFlags;
        public AnimationPrivateFlags PrivateFlags;
        public short AnimationCodecPack;
        public List<SkeletonNode> SkeletonNodes;
        public List<AnimationTagReference> SoundReferences;
        public List<AnimationTagReference> EffectReferences;
        public List<BlendScreen> BlendScreens;
        public List<Leg> Legs;
        public List<Animation> Animations;
        public List<Mode> Modes;
        public List<VehicleSuspensionBlock> VehicleSuspension;
        public List<ObjectOverlay> ObjectOverlays;
        public List<Inheritance> InheritanceList;
        public List<WeaponListBlock> WeaponList;

        [TagField(Length = 8)]
        public uint[] LeftArmNodes;

        [TagField(Length = 8)]
        public uint[] RightArmNodes;

        public byte[] LastImportResults;

        public List<AdditionalNodeDataBlock> AdditionalNodeData;
        public List<ResourceGroup> ResourceGroups;

        [Flags]
        public enum AnimationInheritanceFlags : byte
        {
            None = 0,
            InheritRootTransScaleOnly = 1 << 0,
            InheritForUseOnPlayer = 1 << 1
        }

        [Flags]
        public enum AnimationPrivateFlags : byte
        {
            None = 0,
            PreparedForCache = 1 << 0,
            Bit1 = 1 << 1,
            ImportedWithCodecCompressors = 1 << 2,
            Bit3 = 1 << 3,
            WrittenToCache = 1 << 4,
            AnimationDataReordered = 1 << 5,
            ReadyForUse = 1 << 6
        }

        [TagStructure(Size = 0x20)]
        public class SkeletonNode
        {
            [TagField(Label = true)]
            public StringId Name;
            public short NextSiblingNodeIndex;
            public short FirstChildNodeIndex;
            public short ParentNodeIndex;
            public SkeletonModelFlags ModelFlags;
            public SkeletonNodeJointFlags NodeJointFlags;
            public RealVector3d BaseVector;
            public float VectorRange;
            public float ZPosition;

            [Flags]
            public enum SkeletonModelFlags : byte
            {
                None = 0,
                PrimaryModel = 1 << 0,
                SecondaryModel = 1 << 1,
                LocalRoot = 1 << 2,
                LeftHand = 1 << 3,
                RightHand = 1 << 4,
                LeftArmMember = 1 << 5
            }

            [Flags]
            public enum SkeletonNodeJointFlags : byte
            {
                None = 0,
                BallSocket = 1 << 0,
                Hinge = 1 << 1,
                NoMovement = 1 << 2
            }
        }

        [Flags]
        public enum AnimationTagReferenceFlags : ushort
        {
            None = 0,
            AllowOnPlayer = 1 << 0,
            LeftArmOnly = 1 << 1,
            RightArmOnly = 1 << 2,
            FirstPersonOnly = 1 << 3,
            ForwardOnly = 1 << 4,
            ReverseOnly = 1 << 5,
            Bit6 = 1 << 6,
            Bit7 = 1 << 7,
            Bit8 = 1 << 8,
            Bit9 = 1 << 9,
            Bit10 = 1 << 10,
            Bit11 = 1 << 11,
            Bit12 = 1 << 12,
            Bit13 = 1 << 13,
            Bit14 = 1 << 14,
            Bit15 = 1 << 15
        }

        [TagStructure(Size = 0x14)]
        public class AnimationTagReference
        {
            public CachedTagInstance Reference;
            public AnimationTagReferenceFlags Flags;
            public short Unknown;
        }

        [TagStructure(Size = 0x1C)]
        public class BlendScreen
        {
            [TagField(Label = true)]
            public StringId Label;
            public Angle RightYawPerFrame;
            public Angle LeftYawPerFrame;
            public short RightFrameCount;
            public short LeftFrameCount;
            public Angle DownPitchPerFrame;
            public Angle UpPitchPerFrame;
            public short DownPitchFrameCount;
            public short UpPitchFrameCount;
        }

        [TagStructure(Size = 0x1C)]
        public class Leg
        {
            public StringId FootMarker;
            public Bounds<float> FootBounds;
            public StringId AnkleMarker;
            public Bounds<float> AnkleBounds;
            public AnchorsValue Anchors;
            public short Unknown;

            public enum AnchorsValue : short
            {
                False,
                True,
            }
        }

        public enum FrameType : sbyte
        {
            Base,
            Overlay,
            Replacement
        }

        public enum FrameMovementDataType : sbyte
        {
            None,
            DxDy,
            DxDyDyaw,
            DxDyDzDyaw
        }

        [TagStructure(Size = 0x88)]
        public class Animation
        {
            [TagField(Label = true)]
            public StringId Name;
            public float Weight;
            public short LoopFrameIndex;
            public ushort PlaybackFlags;
            public sbyte BlendScreen;
            public CompressionValue DesiredCompression;
            public CompressionValue CurrentCompression;
            public sbyte NodeCount;
            public short FrameCount;
            public FrameType Type;
            public FrameMovementDataType FrameInfoType;
            public ushort ProductionFlags;
            public ushort InternalFlags;
            public int NodeListChecksum;
            public int ProductionChecksum;
            public short Unknown;
            public short Unknown2;
            public short PreviousVariantSibling;
            public short NextVariantSibling;
            public short ResourceGroupIndex;
            public short ResourceGroupMemberIndex;
            public List<FrameEvent> FrameEvents;
            public List<SoundEvent> SoundEvents;
            public List<EffectEvent> EffectEvents;
            public List<DialogueEvent> DialogueEvents;
            public List<ObjectSpaceParentNode> ObjectSpaceParentNodes;
            public List<LegAnchoringBlock> LegAnchoring;
            public float Unknown4;
            public float Unknown5;
            public float Unknown6;
            public float Unknown7;
            public float Unknown8;

            public enum CompressionValue : sbyte
            {
                BestScore,
                BestCompression,
                BestAccuracy,
                BestFullframe,
                BestSmallKeyframe,
                BestLargeKeyframe
            }

            public enum FrameEventType : short
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
                BodyImpact,
                LeftFootLock,
                LeftFootUnlock,
                RightFootLock,
                RightFootUnlock,
                BlendRangeMarker,
                StrideExpansion,
                StrideContraction,
                RagdollKeyframe,
                DropWeaponKeyframe,
                MatchA,
                MatchB,
                MatchC,
                MatchD,
                JetpackClosed,
                JetpackOpen,
                SoundEvent,
                EffectEvent,
                LeftHand,
                RightHand,
                StartBAMF,
                EndBAMF,
                Hide
            }

            [TagStructure(Size = 0x4)]
            public class FrameEvent
            {
                public FrameEventType Type;
                public short Frame;
            }

            [TagStructure(Size = 0x8)]
            public class SoundEvent
            {
                public short Sound;
                public short Frame;
                public StringId MarkerName;
            }

            [TagStructure(Size = 0x8, MaxVersion = CacheVersion.Halo3Retail)]
            [TagStructure(Size = 0xC, MinVersion = CacheVersion.Halo3ODST)]
            public class EffectEvent
            {
                public short Effect;
                public short Frame;
                public StringId MarkerName;
                [TagField(MinVersion = CacheVersion.Halo3ODST)]
                public uint Unknown;
            }

            public enum DialogueEventType : short
            {
                Bump,
                Dive,
                Evade,
                Lift,
                Sigh,
                Contempt,
                Anger,
                Fear,
                Relief,
                Sprint,
                SprintEnd,
                AssGrabber,
                KillAss,
                AssGrabbed,
                DieAss
            }

            [TagStructure(Size = 0x4)]
            public class DialogueEvent
            {
                public DialogueEventType EventType;
                public short Frame;
            }

            [Flags]
            public enum ObjectSpaceParentNodeFlags : ushort
            {
                None = 0,
                Rotation = 1 << 0,
                Translation = 1 << 1,
                Scale = 1 << 2,
                MotionRoot = 1 << 3
            }

            [TagStructure(Size = 0x1C)]
            public class ObjectSpaceParentNode
            {
                public short NodeIndex;
                public ObjectSpaceParentNodeFlags Flags;
                
                // public RealQuaternion Rotation; // tried to swap values, nothing worked.
                // public RealPoint3d DefaultTranslation;
                public short RotationX;
                public short RotationY;
                public short RotationZ;
                public short RotationW;
                public RealPoint3d DefaultTranslation;
                
                public float DefaultScale;
            }

            [TagStructure(Size = 0x10)]
            public class LegAnchoringBlock
            {
                public short LegIndex;
                public short Unknown;
                public List<UnknownBlock> Unknown2;

                [TagStructure(Size = 0x14)]
                public class UnknownBlock
                {
                    public short Frame1a;
                    public short Frame2a;
                    public short Frame1b;
                    public short Frame2b;
                    public uint Unknown;
                    public uint Unknown2;
                    public uint Unknown3;
                }
            }
        }

        [TagStructure(Size = 0x28)]
        public class Mode
        {
            [TagField(Label = true)]
            public StringId Label;
            public List<WeaponClassBlock> WeaponClass;
            public List<ModeIkBlock> ModeIk;
            public uint Unknown;
            public uint Unknown2;
            public uint Unknown3;

            [TagStructure(Size = 0x1C, MaxVersion = CacheVersion.Halo3ODST)]
            [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnline106708)]
            public class WeaponClassBlock
            {
                [TagField(Label = true)]
                public StringId Label;
                public List<WeaponTypeBlock> WeaponType;
                public List<ModeIkBlock> WeaponIk;
                [TagField(MinVersion = CacheVersion.HaloOnline106708)]
                public List<SyncActionGroup> SyncActionGroups;

                [TagStructure(Size = 0x34)]
                public class WeaponTypeBlock
                {
                    [TagField(Label = true)]
                    public StringId Label;
                    public List<Action> Actions;
                    public List<Overlay> Overlays;
                    public List<DeathAndDamageBlock> DeathAndDamage;
                    public List<Transition> Transitions;

                    [TagStructure(Size = 0x8)]
                    public class Action
                    {
                        [TagField(Label = true)]
                        public StringId Label;
                        public short GraphIndex;
                        public short Animation;
                    }

                    [TagStructure(Size = 0x8)]
                    public class Overlay
                    {
                        [TagField(Label = true)]
                        public StringId Label;
                        public short GraphIndex;
                        public short Animation;
                    }

                    [TagStructure(Size = 0x10)]
                    public class DeathAndDamageBlock
                    {
                        [TagField(Label = true)]
                        public StringId Label;
                        public List<Direction> Directions;

                        [TagStructure(Size = 0xC)]
                        public class Direction
                        {
                            public List<Region> Regions;

                            [TagStructure(Size = 0x4)]
                            public class Region
                            {
                                public short GraphIndex;
                                public short Animation;
                            }
                        }
                    }

                    [TagStructure(Size = 0x18)]
                    public class Transition
                    {
                        [TagField(Label = true)]
                        public StringId FullName;
                        public StringId StateName;
                        public short Unknown;
                        public sbyte IndexA;
                        public sbyte IndexB;
                        public List<Destination> Destinations;

                        [TagStructure(Size = 0x14)]
                        public class Destination
                        {
                            [TagField(Label = true)]
                            public StringId FullName;
                            public StringId ModeName;
                            public StringId StateName;
                            public FrameEventLinkValue FrameEventLink;
                            public sbyte Unknown;
                            public sbyte IndexA;
                            public sbyte IndexB;
                            public short GraphIndex;
                            public short Animation;

                            public enum FrameEventLinkValue : sbyte
                            {
                                NoKeyframe,
                                KeyframeTypeA,
                                KeyframeTypeB,
                                KeyframeTypeC,
                                KeyframeTypeD
                            }
                        }
                    }
                }

                [TagStructure(Size = 0x10)]
                public class SyncActionGroup
                {
                    [TagField(Label = true)]
                    public StringId Name;
                    public List<SyncAction> SyncActions;

                    [TagStructure(Size = 0x1C)]
                    public class SyncAction
                    {
                        [TagField(Label = true)]
                        public StringId Name;
                        public List<SameTypeParticipant> SameTypeParticipants;
                        public List<OtherParticipant> OtherParticipants;

                        [TagStructure(Size = 0x30)]
                        public class SameTypeParticipant
                        {
                            public ParticipantFlags Flags;
                            public short GraphIndex;
                            public short Animation;
                            public RealPoint3d StartOffset;
                            public RealVector3d StartFacing;
                            public RealVector3d EndOffset;
                            public float TimeUntilHurt;

                            [Flags]
                            public enum ParticipantFlags : int
                            {
                                None = 0,
                                Initiator = 1 << 0,
                                CriticalParticipant = 1 << 1,
                                Disabled = 1 << 2,
                                Airborne = 1 << 3,
                                Partner = 1 << 4
                            }
                        }

                        [TagStructure(Size = 0x14)]
                        public class OtherParticipant
                        {
                            public ParticipantFlags Flags;
                            public CachedTagInstance ObjectType;

                            public enum ParticipantFlags : int
                            {
                                None = 0,
                                SupportsAnyType = 1 << 0,
                                Disabled = 1 << 1
                            }
                        }
                    }
                }
            }

            [TagStructure(Size = 0x8)]
            public class ModeIkBlock
            {
                public StringId Marker;
                public StringId AttachToMarker;
            }
        }

        [TagStructure(Size = 0x28)]
        public class VehicleSuspensionBlock
        {
            [TagField(Label = true)]
            public StringId Label;
            public short GraphIndex;
            public short Animation;
            public StringId MarkerName;
            public float MassPointOffset;
            public float FullExtensionGroundDepth;
            public float FullCompressionGroundDepth;
            public StringId RegionName;
            public float MassPointOffset2;
            public float ExpressionGroundDepth;
            public float CompressionGroundDepth;
        }

        [TagStructure(Size = 0x14)]
        public class ObjectOverlay
        {
            [TagField(Label = true)]
            public StringId Label;
            public short GraphIndex;
            public short Animation;

            [TagField(Padding = true, Length = 2)]
            public byte[] Unused1;

            public FunctionControlsValue FunctionControls;
            public StringId Function;

            [TagField(Padding = true, Length = 4)]
            public byte[] Unused2;

            public enum FunctionControlsValue : short
            {
                Frame,
                Scale
            }
        }

        [Flags]
        public enum InheritanceListFlags : int
        {
            None = 0,
            TightenNodes = 1 << 0
        }

        [TagStructure(Size = 0x30)]
        public class Inheritance
        {
            public CachedTagInstance InheritedGraph;
            public List<NodeMapBlock> NodeMap;
            public List<NodeMapFlag> NodeMapFlags;
            public float RootZOffset;
            public InheritanceListFlags Flags;

            [TagStructure(Size = 0x2)]
            public class NodeMapBlock
            {
                public short LocalNode;
            }

            [TagStructure(Size = 0x4)]
            public class NodeMapFlag
            {
                public int LocalNodeFlags;
            }
        }

        [TagStructure(Size = 0x8)]
        public class WeaponListBlock
        {
            [TagField(Label = true)]
            public StringId WeaponName;
            public StringId WeaponClass;
        }

        [TagStructure(Size = 0x3C)]
        public class AdditionalNodeDataBlock
        {
            [TagField(Label = true)]
            public StringId NodeName;
            public RealQuaternion DefaultRotation;
            public RealPoint3d DefaultTranslation;
            public float DefaultScale;
            public RealPoint3d MinimumBounds;
            public RealPoint3d MaximumBounds;
        }

        [TagStructure(Size = 0xC)]
        public class ResourceGroup
        {
            public int MemberCount;

            [TagField(MaxVersion = CacheVersion.Halo3ODST)]
            public int ZoneAssetDatumIndex;

            [TagField(Pointer = true, MinVersion = CacheVersion.HaloOnline106708)]
            public PageableResource Resource;

            [TagField(Padding = true, Length = 4)]
            public byte[] Padding;
        }
    }
}