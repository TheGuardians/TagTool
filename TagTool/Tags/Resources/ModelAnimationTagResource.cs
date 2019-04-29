using TagTool.Common;
using System;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "model_animation_tag_resource", Size = 0xC)]
    public class ModelAnimationTagResource : TagStructure
	{
        public List<GroupMember> GroupMembers;

        [TagStructure(Size = 0x10)]
        public class AnimationDataSizes
        {
            public byte StaticNodeFlags;    // BaseHeader; 0x0 means no base header
            public byte AnimatedNodeFlags;  // OverlayHeader; 0x0 means no overlay header (there's always one)
            public ushort MovementData;     // Unknown1; value/offset if MovementDataType.dx_dy or dx_dy_dyaw
            public ushort PillOffsetData;   // Unknown2
            public ushort DefaultData;      // OverlayOffset; with member offset as origin
            public uint UncompressedData;   // Unknown3; always 0x0
            public uint CompressedData;     // FlagsOffset; with OverlayOffset as origin, not member offset
        }

        public enum CompressionCodecType : sbyte
        {
            // ty xbox7887
            None,                                // _no_compression_codec
            UncompressedStaticData,              // _uncompressed_static_data_codec
            UncompressedAnimatedData,            // _uncompressed_animated_data_codec
            _8byteQuantizedRotationOnly,         // _8byte_quantized_rotation_only_codec
            ByteKeyframeLightlyQuantized,        // byte_keyframe_lightly_quantized
            WordKeyframeLightlyQuantized,        // word_keyframe_lightly_quantized
            ReverseByteKeyframeLightlyQuantized, // reverse_byte_keyframe_lightly_quantized
            ReverseWordKeyframeLightlyQuantized, // reverse_word_keyframe_lightly_quantized
            BlendScreenCodec                     // _blend_screen_codec
        }

        [TagStructure(Size = 0xC)]
        public class CompressionCodecData : TagStructure
        {
            public CompressionCodecType Type; // base/overlay
            public byte RotationNodeCount;    // number of nodes with rotation frames (XYZW short per frame)
            public byte PositionNodeCount;    // number of nodes with position frames (XYZ float per frame)
            public byte ScaleNodeCount;       // number of nodes with position frames (X float per frame); something that affects node render draw distance or lightning
            public uint Unknown0;             // always 0x0 (not always 0x0)
            public float PlaybackRate = 1.0f;
        }

        public enum GroupMemberMovementDataType : sbyte
        {
            dyaw,
            dx_dy,
            dx_dy_dyaw,
            dx_dy_dz_dyaw,
            dx_dy_dz_dangleaxis,
            x_y_z_absolute,
            Auto
        }

        [TagStructure(Size = 0x30)]
        public class GroupMember : TagStructure
		{
            /// <summary>
            /// The name of the group member.
            /// </summary>
            [TagField(Flags = Label)]
            public StringId Name;

            /// <summary>
            /// The animation checksum of the group member.
            /// </summary>
            public uint AnimationChecksum;

            /// <summary>
            /// The total number of frames in the group member.
            /// </summary>
            public short FrameCount;

            /// <summary>
            /// The total number of nodes in the group member.
            /// </summary>
            public byte NodeCount;

            /// <summary>
            /// The movement data type of the group member. (8-bit)
            /// </summary>
            public GroupMemberMovementDataType MovementDataType;

            /// <summary>
            /// The animation data sizes of the group member.
            /// </summary>
            public AnimationDataSizes Sizes;

            /// <summary>
            /// The animation data of the group member.
            /// </summary>
            public TagData AnimationData; // this will point to an Animation object

            [TagStructure(Size = 0x14)]
            public class DefaultFrameInfo : TagStructure // used by Format3
			{
                public uint RotationFramesEnd;
                public uint PositionFramesEnd;
                public uint RotationFramesSize;
                public uint PositionFramesSize;
                public uint ScaleFramesSize;
            }

            [TagStructure(Size = 0x14)]
            public class BlendScreenData : TagStructure // Format8; OverlayRotations are 4x uint32) per frame
			{
                public uint RotationFramesEnd;
                public uint PositionFramesEnd;
                public uint FrameCountPerNode;  // spooky sbyte; FrameCount = FrameCountPerNode / 10
                public uint Unknown2;
                public uint Unknown3; // Unknown3 = Unknown2 / 3
            }

            [TagStructure(Size = 0x24)]
            public class Overlay : TagStructure // Format4,Format6,Format7
			{
                public uint RotationNodesEnd;
                public uint PositionNodesEnd;
                public uint ScaleNodesEnd;
                public uint RotationKeyframesEnd;
                public uint PositionKeyframesEnd;
                public uint ScaleKeyframesEnd;
                public uint RotationFramesEnd;
                public uint PositionFramesEnd;
                public uint ScaleFramesEnd;
            }

            [TagStructure(Size = 0x8)]
            public class RotationFrameShort : TagStructure
			{
                public short I;
                public short J;
                public short K;
                public short W;
            }

            [TagStructure(Size = 0x4)]
            public class ScaleFrame : TagStructure
			{
                public float Scale;
            }

            [TagStructure(Size = 0x1)]
            public class ByteKeyframe : TagStructure
			{
                public byte Frame;
            }

            [TagStructure(Size = 0x2)]
            public class WordKeyframe : TagStructure
			{
                public short Frame;
            }

            [TagStructure(Size = 0x10)]
            public class RotationFrame : TagStructure
			{
                public RealQuaternion Rotation;
            }

            [TagStructure(Size = 0xC)]
            public class PositionFrame : TagStructure
			{
                public RealPoint3d Position;
            }

            [TagStructure(Size = 0x4)]
            public class FrameInfo : TagStructure
            {
                public uint Value;

                public uint KeyframesOffset => Value & 0x00FFF000;
                public uint KeyframeCount => Value & 0x00000FFF;
            }

            [TagStructure(Size = 0x4)]
            public class FrameInfoDyaw : TagStructure
			{
                public Angle Yaw;
            }

            [TagStructure(Size = 0x8)]
            public class FrameInfoDxDy : TagStructure
			{
                public RealPoint2d Point;
            }

            [TagStructure(Size = 0xC)]
            public class FrameInfoDxDyDyaw : TagStructure
			{
                public RealPoint2d Point;
                public Angle Yaw;
            }

            [TagStructure(Size = 0x10)]
            public class FrameInfoDxDyDzDyaw : TagStructure
			{
                public RealPoint3d Point;
                public Angle Yaw;
            }

            #region Static/Animated Node Flags
            // DemonicSandwich - http://remnantmods.com/forums/viewtopic.php?f=13&t=1574
            //
            // Just a block of flags. Tick a flag and the respective node will be affected by animation.
            // The size of this block should always be a multiple of 12. It's size is determined my the meta value Node List Size [byte, offset: 61] 
            // When set to 12, the list can handle objects with a node count up to 32 (0-31).
            // When set to 24, the object can have 64 nodes and so on.
            // The block is split into 3 groups of flags.
            // The first group determines what nodes are affected by rotation, the second group for position, and the third group for scale.
            // 
            // If looking at it in hex, the Node ticks for each group will be in order as follows:
            // [7][6][5][4][3][2][1][0] - [15][14][13][12][11][10][9][8] - etc.
            // Each flag corresponding to a Node index.
            //
            // There's one bitfield32 for every 32 nodes that are animated which i'll call a node flags. 
            // There's at least 3 flags if the animation only has an overlay header, which i'll call a flag set.
            // There's at least 6 flags if the animation has both a base header and an overlay header, so 2 sets.
            // If the animated nodes count is over 32, then a new flags set is added.
            // 1 set per header is added, such as 32 nodes = 1 set, 64 = 2 sets, 96 = 3 sets etc , 128-256 maybe max
            #endregion

            [TagStructure(Size = 0x4)]
            public class StaticNodeFlagsData
            {
                //
                // TODO: fix this shit
                //

                public int Flags;
            }

            [TagStructure(Size = 0x4)]
            public class AnimatedNodeFlagsData
            {
                //
                // TODO: fix this shit
                //

                public int Flags;
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
    }
}