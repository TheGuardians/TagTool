using System;
using System.Collections.Generic;
using TagTool.Animations;
using TagTool.Cache;
using TagTool.Common;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "model_animations", Tag = "antr", Size = 0x80, MaxVersion = CacheVersion.HaloCustomEdition)]
    public class ModelAnimationGraph : TagStructure
    {
        public List<AnimationObject> Objects;
        public List<AnimationUnit> Units;
        public List<AnimationReferenceList> Weapons;
        public List<VehicleBlock> Vehicles;
        public List<AnimationReferenceList> Devices;
        public List<AnimationReference> UnitDamage;
        public List<AnimationReferenceList> FirstPersonWeapons;
        public List<AnimationTagReference> SoundReferences;
        public float LimpBodyNodeRadius;
        public CompressionFlags Flags;
        public List<SkeletonNode> Nodes;
        public List<Animation> Animations;

        public class AnimationObject : TagStructure
        {
            public short AnimationIndex;
            public AnimationFunction Function;
            public FunctionControl FunctionControls;

            public enum AnimationFunction : short
            {
                A_out,
                B_out,
                C_out,
                D_out
            }
            public enum FunctionControl : short
            {
                Frame,
                Scale
            }
        }

        [TagStructure(Size = 0x38)]
        public class AnimationUnit : TagStructure
        {
            [TagField(Length = 32, Flags = TagFieldFlags.Label)] public string Label;
            public BlendScreen LookingScreenBounds; //tagblock is inlined
            public List<AnimationReference> Animations;
            public List<ModeIkBlock> IKPoints;
            public List<UnitWeaponBlock> Weapons;
        }

        [TagStructure(Size = 0x40)]
        public class UnitWeaponBlock : TagStructure
        {
            [TagField(Length = 32, Flags = TagFieldFlags.Label)] public string Name;
            [TagField(Length = 32)] public string GripMarker;
            [TagField(Length = 32)] public string HandMarker;
            public BlendScreen AimingScreenBounds; //tagblock is inlined
            public List<AnimationReference> Animations;
            public List<ModeIkBlock> IKPoints;
            public List<WeaponType> WeaponTypes;
        }

        [TagStructure(Size = 0x2C)]
        public class WeaponType : TagStructure
        {
            [TagField(Length = 32, Flags = TagFieldFlags.Label)] public string Label;
            public List<AnimationReference> Animations;
        }

        [TagStructure(Size = 0xC)]
        public class AnimationReferenceList : TagStructure
        {
            public List<AnimationReference> Animations;
        }

        [TagStructure(Size = 0x24)]
        public class VehicleBlock : TagStructure
        {
            public BlendScreen SteeringScreen; //tagblock is inlined
            public List<AnimationReference> Animations;
            public List<VehicleSuspensionBlock> SuspensionAnimations;
        }

        [TagStructure(Size = 0x2)]
        public class AnimationReference : TagStructure
        {
            public short AnimationIndex;
        }

        [TagStructure(Size = 0x40)]
        public class ModeIkBlock : TagStructure
        {
            [TagField(Length = 32)] public string Marker;
            [TagField(Length = 32)] public string AttachToMarker;
        }

        [TagStructure(Size = 0x18)]
        public class BlendScreen : TagStructure
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

        [TagStructure(Size = 0x28)]
        public class VehicleSuspensionBlock : TagStructure
        {
            public short MassPointIndex;
            public short AnimationIndex;
            public float FullExtensionGroundDepth;
            public float FullCompressionGroundDepth;
        }

        public class AnimationTagReference : TagStructure
        {
            public CachedTag Reference;
        }

        [TagStructure(Size = 0x20)]
        public class SkeletonNode : TagStructure
        {
            [TagField(Length = 32, Flags = TagFieldFlags.Label)] public string Name;
            public short NextSiblingNodeIndex;
            public short FirstChildNodeIndex;
            public short ParentNodeIndex;
            public SkeletonNodeJointFlags NodeJointFlags;
            public RealVector3d BaseVector;
            public float VectorRange;

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
        public enum CompressionFlags : int
        {
            None,
            CompressAllAnimations = 1 << 0,
            ForceIdleCompression = 1 << 1,
        }

        public class Animation : TagStructure
        {
            [TagField(Length = 32, Flags = TagFieldFlags.Label)] public string Name;
            public FrameType Type;
            public short FrameCount;
            public short FrameSize;
            public short FrameInfoType;
            public int NodeListChecksum;
            public short NodeCount;
            public short LoopFrameIndex;
            public float Weight;
            public short KeyFrameIndex;
            public short SecondKeyFrameIndex;
            public short NextAnimation;
            public InternalFlags Flags;
            public short Sound;
            public short SoundFrameIndex;
            public short LeftFootFrameIndex;
            public short RightFootFrameIndex;

            public byte[] FrameInfo;
            public byte[] OffsetToCompressedData;
            public byte[] DefaultData;
            public byte[] FrameData;

            [Flags]
            public enum InternalFlags : byte
            {
                None,
                CompressedData = 1 << 0,
                WorldRelative = 1 << 1,
                _25Hz_PAL = 1 << 2,
            }

            public enum FrameType : sbyte
            {
                Base,
                Overlay,
                Replacement
            }
        }
    }
}