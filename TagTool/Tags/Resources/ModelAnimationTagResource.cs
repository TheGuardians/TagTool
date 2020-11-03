using TagTool.Common;
using System;
using System.Collections.Generic;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "model_animation_tag_resource", Size = 0xC)]
    public class ModelAnimationTagResource : TagStructure
    {
        public TagBlock<GroupMember> GroupMembers;

        [TagStructure(Size = 0x30)]
        public class GroupMember : TagStructure
        {
            public StringId Name;
            public int Checksum;
            public short FrameCount;
            public byte NodeCount;
            public GroupMemberMovementDataType MovementDataType; // sbyte
            public GroupMemberHeaderType CompressedData; // sbyte; 0x0 means no base header
            public GroupMemberHeaderType UncompressedData; // sbyte; 0x0 means no overlay header (there's always one)
            public short DefaultData; // value/offset if MovementDataType.dx_dy or dx_dy_dyaw
            public short PillOffsetData;
            public short MovementData; // with member offset as origin
            public int AnimatedNodeFlags; // always 0x0
            public int StaticNodeFlags; // with OverlayOffset as origin , not member offset

            [TagField(Align = 0x10)]
            public TagData AnimationData; // this will point to an Animation object

            [TagStructure(Size = 0xC)]
            public class Codec : TagStructure
            {
                public AnimationCompressionFormats AnimationCodec; // base/overlay
                public byte RotationNodeCount; // number of nodes with rotation frames (XYZW short per frame)
                public byte PositionNodeCount; // number of nodes with position frames (XYZ float per frame)
                public byte ScaleNodeCount; // number of nodes with position frames (X float per frame); something that affects node render draw distance or lightning
                public float Unknown0;       //very likely a float
                public float PlaybackRate = 1.0f;
            }

            [TagStructure(Size = 0x14)]
            public class Format1 : TagStructure // used by Format3
            {
                public uint DataStart;
                public uint ScaleFramesOffset;
                public uint RotationFramesSize;
                public uint PositionFramesSize;
                public uint ScaleFramesSize;

                // public uint RotationFramesOffset; // hidden, always 0x20
            }

            [TagStructure(Size = 0x14)]
            public class Format8 : TagStructure // Format8; OverlayRotations are 4x uint32) per frame
            {
                public uint PositionFramesOffset;
                public uint ScaleFramesOffset;
                public uint FrameCountPerNode;  // spooky sbyte; FrameCount = FrameCountPerNode / 10
                public uint Unknown2;
                public uint Unknown3; // Unknown3 = Unknown2 / 3
            }

            [TagStructure(Size = 0x24)]
            public class Overlay : TagStructure // Format4,Format6,Format7
            {
                public uint PositionFrameInfoOffset;
                public uint ScaleFrameInfoOffset;
                public uint RotationKeyframesOffset;
                public uint PositionKeyframesOffset;
                public uint ScaleKeyframesOffset;
                public uint RotationFramesOffset;
                public uint PositionFramesOffset;
                public uint ScaleFramesOffset;
                public uint UselessPadding;
            }

            [TagStructure(Size = 0x24)]
            public class Node : TagStructure
            {
                public List<RotationNode> RotationNodes;
                public List<PositionNode> PositionNodes;
                public List<ScaleNode> ScaleNodes;
            }

            [TagStructure(Size = 0xC)]
            public class RotationNode : TagStructure
            {
                public List<RotationFrame> RotationFrames;
            }

            [TagStructure(Size = 0xC)]
            public class PositionNode : TagStructure
            {
                public List<PositionFrame> PositionFrames;
            }

            [TagStructure(Size = 0xC)]
            public class ScaleNode : TagStructure
            {
                public List<ScaleFrame> ScaleFrames;
            }

            [TagStructure(Size = 0x18)]
            public class FrameInfoNode : TagStructure
            {
                public List<FrameInfoDxDy> frameInfoDxDy;
                public List<FrameInfoDxDyDyaw> frameInfoDxDyDyaw;
            }

            [TagStructure(Size = 0x8)]
            public class RotationFrame : TagStructure
            {
                public short X;
                public short Y;
                public short Z;
                public short W;
            }

            [TagStructure(Size = 0x4)]
            public class ScaleFrame : TagStructure
            {
                public uint X;
            }

            [TagStructure(Size = 0x4)]
            public class PositionFramesCountPerNode : TagStructure
            {
                public uint X;
            }

            [TagStructure(Size = 0x1)]
            public class Keyframe : TagStructure
            {
                public byte Frame;
            }

            [TagStructure(Size = 0x2)]
            public class KeyframeType5 : TagStructure
            {
                public short Frame;
            }

            [TagStructure(Size = 0x10)]
            public class RotationFrameFloat : TagStructure
            {
                public uint X;
                public uint Y;
                public uint Z;
                public uint W;
            }

            [TagStructure(Size = 0xC)]
            public class PositionFrame : TagStructure
            {
                public uint X;
                public uint Y;
                public uint Z;
            }

            [TagStructure(Size = 0x4)]
            public class FrameInfo : TagStructure
            {
                public uint FrameCount;
            }

            [TagStructure(Size = 0x4)]
            public class FrameInfoDyaw : TagStructure
            {
                public uint X;
            }

            [TagStructure(Size = 0x8)]
            public class FrameInfoDxDy : TagStructure
            {
                public uint X;
                public uint Y;
            }

            [TagStructure(Size = 0xC)]
            public class FrameInfoDxDyDyaw : TagStructure
            {
                public uint X;
                public uint Y;
                public uint Z;
            }

            [TagStructure(Size = 0x10)]
            public class FrameInfoDxDyDzDyaw : TagStructure
            {
                public uint X;
                public uint Y;
                public uint Z;
                public uint W;
            }

            [TagStructure(Size = 0xC)]
            public class Footer32 : TagStructure
            {
                public PrimaryNodeFlags RotationFlags;
                public PrimaryNodeFlags PositionFlags;
                public PrimaryNodeFlags ScaleFlags;
            }

            [TagStructure(Size = 0x18)]
            public class Footer64 : TagStructure
            {
                public PrimaryNodeFlags RotationFlags1;
                public SecondaryNodeFlags RotationFlags2;
                public PrimaryNodeFlags PositionFlags1;
                public SecondaryNodeFlags PositionFlags2;
                public PrimaryNodeFlags ScaleFlags1;
                public SecondaryNodeFlags ScaleFlags2;
            }

            [TagStructure(Size = 0x18)]
            public class Footer32_Overlay : TagStructure
            {
                public PrimaryNodeFlags RotationFlags;
                public PrimaryNodeFlags PositionFlags;
                public PrimaryNodeFlags ScaleFlags;
                public PrimaryNodeFlags RotationFlags_Overlay;
                public PrimaryNodeFlags PositionFlags_Overlay;
                public PrimaryNodeFlags ScaleFlags_Overlay;
            }

            [TagStructure(Size = 0x30)]
            public class Footer64_Overlay : TagStructure
            {
                public PrimaryNodeFlags RotationFlags1;
                public SecondaryNodeFlags RotationFlags2;
                public PrimaryNodeFlags PositionFlags1;
                public SecondaryNodeFlags PositionFlags2;
                public PrimaryNodeFlags ScaleFlags1;
                public SecondaryNodeFlags ScaleFlags2;
                public PrimaryNodeFlags RotationFlags1_Overlay;
                public SecondaryNodeFlags RotationFlags2_Overlay;
                public PrimaryNodeFlags PositionFlags1_Overlay;
                public SecondaryNodeFlags PositionFlags2_Overlay;
                public PrimaryNodeFlags ScaleFlags1_Overlay;
                public SecondaryNodeFlags ScaleFlags2_Overlay;
            }
        }

        public enum GroupMemberHeaderType : sbyte
        {
            Overlay = 0,
            Base_H3 = 0x18,
            Base_HO = 0x0C
        }

        public enum AnimationCompressionFormats : sbyte
        {
            // ty xbox7887
            None_,        // _no_compression_codec
            Type1,        // _uncompressed_static_data_codec
            Type2_Unused, // _uncompressed_animated_data_codec
            Type3,        // _8byte_quantized_rotation_only_codec
            Type4,        // byte_keyframe_lightly_quantized
            Type5, // word_keyframe_lightly_quantized
            Type6,        // reverse_byte_keyframe_lightly_quantized
            Type7,        // reverse_word_keyframe_lightly_quantized
            Type8         // _blend_screen_codec
        }

        [Flags]
        public enum PrimaryNodeFlags : int
        {
            None = 0,
            Node0 = 1 << 0,
            Node1 = 1 << 1,
            Node2 = 1 << 2,
            Node3 = 1 << 3,
            Node4 = 1 << 4,
            Node5 = 1 << 5,
            Node6 = 1 << 6,
            Node7 = 1 << 7,
            Node8 = 1 << 8,
            Node9 = 1 << 9,
            Node10 = 1 << 10,
            Node11 = 1 << 11,
            Node12 = 1 << 12,
            Node13 = 1 << 13,
            Node14 = 1 << 14,
            Node15 = 1 << 15,
            Node16 = 1 << 16,
            Node17 = 1 << 17,
            Node18 = 1 << 18,
            Node19 = 1 << 19,
            Node20 = 1 << 20,
            Node21 = 1 << 21,
            Node22 = 1 << 22,
            Node23 = 1 << 23,
            Node24 = 1 << 24,
            Node25 = 1 << 25,
            Node26 = 1 << 26,
            Node27 = 1 << 27,
            Node28 = 1 << 28,
            Node29 = 1 << 29,
            Node30 = 1 << 30,
            Node31 = 1 << 31
        }

        [Flags]
        public enum SecondaryNodeFlags : int
        {
            None = 0,
            Node32 = 1 << 32,
            Node33 = 1 << 33,
            Node34 = 1 << 34,
            Node35 = 1 << 35,
            Node36 = 1 << 36,
            Node37 = 1 << 37,
            Node38 = 1 << 38,
            Node39 = 1 << 39,
            Node40 = 1 << 40,
            Node41 = 1 << 41,
            Node42 = 1 << 42,
            Node43 = 1 << 43,
            Node44 = 1 << 44,
            Node45 = 1 << 45,
            Node46 = 1 << 46,
            Node47 = 1 << 47,
            Node48 = 1 << 48,
            Node49 = 1 << 49,
            Node50 = 1 << 50,
            Node51 = 1 << 51,
            Node52 = 1 << 52,
            Node53 = 1 << 53,
            Node54 = 1 << 54,
            Node55 = 1 << 55,
            Node56 = 1 << 56,
            Node57 = 1 << 57,
            Node58 = 1 << 58,
            Node59 = 1 << 59,
            Node60 = 1 << 60,
            Node61 = 1 << 61,
            Node62 = 1 << 62,
            Node63 = 1 << 63
        }

        public enum GroupMemberMovementDataType : sbyte
        {
            None,
            dx_dy,
            dx_dy_dyaw,
            dx_dy_dz_dyaw,
            dx_dy_dz_dangleaxis,
            x_y_z_absolute,
            Auto
        }
    }
}