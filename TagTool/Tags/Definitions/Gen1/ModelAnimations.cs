using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen1
{
    [TagStructure(Name = "model_animations", Tag = "antr", Size = 0x80)]
    public class ModelAnimations : TagStructure
    {
        public List<AnimationGraphObjectOverlay> Objects;
        public List<AnimationGraphUnitSeatBlock> Units;
        public List<AnimationGraphWeaponAnimationsBlock> Weapons;
        public List<AnimationGraphVehicleAnimationsBlock> Vehicles;
        public List<DeviceAnimations> Devices;
        public List<UnitDamageAnimations> UnitDamage;
        public List<AnimationGraphFirstPersonWeaponAnimationsBlock> FirstPersonWeapons;
        public List<AnimationGraphSoundReferenceBlock> SoundReferences;
        /// <summary>
        /// 0 uses 0.04 default
        /// </summary>
        public float LimpBodyNodeRadius;
        public FlagsValue Flags;
        [TagField(Length = 0x2)]
        public byte[] Padding;
        public List<AnimationGraphNodeBlock> Nodes;
        public List<AnimationBlock> Animations;
        
        [TagStructure(Size = 0x14)]
        public class AnimationGraphObjectOverlay : TagStructure
        {
            public short Animation;
            public FunctionValue Function;
            public FunctionControlsValue FunctionControls;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0xC)]
            public byte[] Padding1;
            
            public enum FunctionValue : short
            {
                AOut,
                BOut,
                COut,
                DOut
            }
            
            public enum FunctionControlsValue : short
            {
                Frame,
                Scale
            }
        }
        
        [TagStructure(Size = 0x64)]
        public class AnimationGraphUnitSeatBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Label;
            public Angle RightYawPerFrame;
            public Angle LeftYawPerFrame;
            public short RightFrameCount;
            public short LeftFrameCount;
            public Angle DownPitchPerFrame;
            public Angle UpPitchPerFrame;
            public short DownPitchFrameCount;
            public short UpPitchFrameCount;
            [TagField(Length = 0x8)]
            public byte[] Padding;
            public List<UnitSeatAnimationBlock> Animations;
            public List<AnimationGraphUnitSeatIkPoint> IkPoints;
            public List<AnimationGraphWeaponBlock> Weapons;
            
            [TagStructure(Size = 0x2)]
            public class UnitSeatAnimationBlock : TagStructure
            {
                public short Animation;
            }
            
            [TagStructure(Size = 0x40)]
            public class AnimationGraphUnitSeatIkPoint : TagStructure
            {
                /// <summary>
                /// the marker name on the object being attached
                /// </summary>
                [TagField(Length = 32)]
                public string Marker;
                /// <summary>
                /// the marker name object (weapon, vehicle, etc.) the above marker is being attached to
                /// </summary>
                [TagField(Length = 32)]
                public string AttachToMarker;
            }
            
            [TagStructure(Size = 0xBC)]
            public class AnimationGraphWeaponBlock : TagStructure
            {
                [TagField(Length = 32)]
                public string Name;
                /// <summary>
                /// the marker name on the weapon to which the hand is attached (leave blank to use origin)
                /// </summary>
                [TagField(Length = 32)]
                public string GripMarker;
                /// <summary>
                /// the marker name on the unit to which the weapon is attached
                /// </summary>
                [TagField(Length = 32)]
                public string HandMarker;
                public Angle RightYawPerFrame;
                public Angle LeftYawPerFrame;
                public short RightFrameCount;
                public short LeftFrameCount;
                public Angle DownPitchPerFrame;
                public Angle UpPitchPerFrame;
                public short DownPitchFrameCount;
                public short UpPitchFrameCount;
                [TagField(Length = 0x20)]
                public byte[] Padding;
                public List<WeaponClassAnimationBlock> Animations;
                public List<AnimationGraphUnitSeatIkPoint> IkPoints;
                public List<AnimationGraphWeaponTypeBlock> WeaponTypes;
                
                [TagStructure(Size = 0x2)]
                public class WeaponClassAnimationBlock : TagStructure
                {
                    public short Animation;
                }
                
                [TagStructure(Size = 0x40)]
                public class AnimationGraphUnitSeatIkPoint : TagStructure
                {
                    /// <summary>
                    /// the marker name on the object being attached
                    /// </summary>
                    [TagField(Length = 32)]
                    public string Marker;
                    /// <summary>
                    /// the marker name object (weapon, vehicle, etc.) the above marker is being attached to
                    /// </summary>
                    [TagField(Length = 32)]
                    public string AttachToMarker;
                }
                
                [TagStructure(Size = 0x3C)]
                public class AnimationGraphWeaponTypeBlock : TagStructure
                {
                    [TagField(Length = 32)]
                    public string Label;
                    [TagField(Length = 0x10)]
                    public byte[] Padding;
                    public List<WeaponTypeAnimationBlock> Animations;
                    
                    [TagStructure(Size = 0x2)]
                    public class WeaponTypeAnimationBlock : TagStructure
                    {
                        public short Animation;
                    }
                }
            }
        }
        
        [TagStructure(Size = 0x1C)]
        public class AnimationGraphWeaponAnimationsBlock : TagStructure
        {
            [TagField(Length = 0x10)]
            public byte[] Padding;
            public List<WeaponAnimationBlock> Animations;
            
            [TagStructure(Size = 0x2)]
            public class WeaponAnimationBlock : TagStructure
            {
                public short Animation;
            }
        }
        
        [TagStructure(Size = 0x74)]
        public class AnimationGraphVehicleAnimationsBlock : TagStructure
        {
            public Angle RightYawPerFrame;
            public Angle LeftYawPerFrame;
            public short RightFrameCount;
            public short LeftFrameCount;
            public Angle DownPitchPerFrame;
            public Angle UpPitchPerFrame;
            public short DownPitchFrameCount;
            public short UpPitchFrameCount;
            [TagField(Length = 0x44)]
            public byte[] Padding;
            public List<VehicleAnimationBlock> Animations;
            public List<SuspensionAnimationBlock> SuspensionAnimations;
            
            [TagStructure(Size = 0x2)]
            public class VehicleAnimationBlock : TagStructure
            {
                public short Animation;
            }
            
            [TagStructure(Size = 0x14)]
            public class SuspensionAnimationBlock : TagStructure
            {
                public short MassPointIndex;
                public short Animation;
                public float FullExtensionGroundDepth;
                public float FullCompressionGroundDepth;
                [TagField(Length = 0x8)]
                public byte[] Padding;
            }
        }
        
        [TagStructure(Size = 0x60)]
        public class DeviceAnimations : TagStructure
        {
            [TagField(Length = 0x54)]
            public byte[] Padding;
            public List<DeviceAnimationBlock> Animations;
            
            [TagStructure(Size = 0x2)]
            public class DeviceAnimationBlock : TagStructure
            {
                public short Animation;
            }
        }
        
        [TagStructure(Size = 0x2)]
        public class UnitDamageAnimations : TagStructure
        {
            public short Animation;
        }
        
        [TagStructure(Size = 0x1C)]
        public class AnimationGraphFirstPersonWeaponAnimationsBlock : TagStructure
        {
            [TagField(Length = 0x10)]
            public byte[] Padding;
            public List<FirstPersonWeaponBlock> Animations;
            
            [TagStructure(Size = 0x2)]
            public class FirstPersonWeaponBlock : TagStructure
            {
                public short Animation;
            }
        }
        
        [TagStructure(Size = 0x14)]
        public class AnimationGraphSoundReferenceBlock : TagStructure
        {
            [TagField(ValidTags = new [] { "snd!" })]
            public CachedTag Sound;
            [TagField(Length = 0x4)]
            public byte[] Padding;
        }
        
        public enum FlagsValue : ushort
        {
            CompressAllAnimations,
            ForceIdleCompression
        }
        
        [TagStructure(Size = 0x40)]
        public class AnimationGraphNodeBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public short NextSiblingNodeIndex;
            public short FirstChildNodeIndex;
            public short ParentNodeIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            public NodeJointFlagsValue NodeJointFlags;
            public RealVector3d BaseVector;
            public float VectorRange;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            
            public enum NodeJointFlagsValue : uint
            {
                BallSocket,
                Hinge,
                NoMovement
            }
        }
        
        [TagStructure(Size = 0xB4)]
        public class AnimationBlock : TagStructure
        {
            [TagField(Length = 32)]
            public string Name;
            public TypeValue Type;
            public short FrameCount;
            public short FrameSize;
            public FrameInfoTypeValue FrameInfoType;
            public int NodeListChecksum;
            public short NodeCount;
            public short LoopFrameIndex;
            public float Weight;
            public short KeyFrameIndex;
            public short SecondKeyFrameIndex;
            public short NextAnimation;
            public FlagsValue Flags;
            public short Sound;
            public short SoundFrameIndex;
            public sbyte LeftFootFrameIndex;
            public sbyte RightFootFrameIndex;
            [TagField(Length = 0x2)]
            public byte[] Padding;
            [TagField(Length = 0x4)]
            public byte[] Padding1;
            public byte[] FrameInfo;
            [TagField(Length = 2)]
            public int[] NodeTransFlagData;
            [TagField(Length = 0x8)]
            public byte[] Padding2;
            [TagField(Length = 2)]
            public int[] NodeRotationFlagData;
            [TagField(Length = 0x8)]
            public byte[] Padding3;
            [TagField(Length = 2)]
            public int[] NodeScaleFlagData;
            [TagField(Length = 0x4)]
            public byte[] Padding4;
            public int OffsetToCompressedData; // bytes*
            public byte[] DefaultData;
            public byte[] FrameData;
            
            public enum TypeValue : short
            {
                Base,
                Overlay,
                Replacement
            }
            
            public enum FrameInfoTypeValue : short
            {
                None,
                DxDy,
                DxDyDyaw,
                DxDyDzDyaw
            }
            
            public enum FlagsValue : ushort
            {
                CompressedData,
                WorldRelative,
                _25hzPal
            }
        }
    }
}

