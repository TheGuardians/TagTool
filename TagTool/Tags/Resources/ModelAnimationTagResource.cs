using TagTool.Common;
using System;
using System.Collections.Generic;
using TagTool.Cache;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "model_animation_tag_resource", Size = 0xC)]
    public class ModelAnimationTagResource : TagStructure
    {
        public TagBlock<GroupMember> GroupMembers;

        [TagStructure(Size = 0x30, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x64, MinVersion = CacheVersion.HaloReach)]
        public class GroupMember : TagStructure
        {
            public StringId Name;
            public int Checksum;
            public short FrameCount;
            public byte NodeCount;
            public GroupMemberMovementDataType MovementDataType; // sbyte

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public PackedDataSizesStructBlock PackedDataSizes;
            [TagField(MinVersion = CacheVersion.HaloReach)]
            public PackedDataSizesStructReach PackedDataSizesReach;

            [TagField(DataAlign = 0x10)]
            public TagData AnimationData; // this will point to an Animation object

            [TagStructure(Size = 0x44)]
            public class PackedDataSizesStructReach : TagStructure
            {
                public int StaticDataSize;
                public int CompressedDataSize;
                public int StaticNodeFlags;
                public int AnimatedNodeFlags;
                public int MovementData;

                //These fields are only present in reach, and seem to be the sizes for some additional animation data.
                //The animation data uses the same codec layout as regular animation data, and is tacked on to the AnimationData after the nodeflags
                [TagField(Length = 12)]
                public int[] UnknownDataSizesReach;               
            }

            [TagStructure(Size = 0x10)]
            public class PackedDataSizesStructBlock : TagStructure
            {
                public byte StaticNodeFlags; // node flags for static data
                public byte AnimatedNodeFlags; // node flags for compressed data
                public short MovementData; // value/offset if MovementDataType.dx_dy or dx_dy_dyaw
                public short PillOffsetData;
                public short StaticDataSize; // with member offset as origin
                public int UncompressedDataSize; // always 0x0
                public int CompressedDataSize; // comes immediately after static data
            }

            [TagStructure(Size = 0xC)]
            public class Codec : TagStructure
            {
                public AnimationCompressionFormats AnimationCodec;
                public byte RotationNodeCount; // number of nodes with rotation frames
                public byte TranslationNodeCount; // number of nodes with position frames
                public byte ScaleNodeCount; // number of nodes with scale frames
                public float ErrorPercentage;
                public float PlaybackRate = 1.0f;
            }

            [TagStructure(Size = 0x14)]
            public class UncompressedHeader : TagStructure // used by Format1, Format2, Format3, Format8
            {
                public uint TranslationDataOffset;
                public uint ScaleDataOffset;
                public uint RotatedNodeBlockSize;
                public uint TranslatedNodeBlockSize;
                public uint ScaledNodeBlockSize;
            }

            [TagStructure(Size = 0x24)]
            public class CompressedHeader : TagStructure // Format4,Format5,Format6,Format7
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
        }

        public enum AnimationCompressionFormats : sbyte
        {
            // ty xbox7887
            None_,        // _no_compression_codec
            Type1,        // _uncompressed_static_data_codec
            Type2_Unused, // _uncompressed_animated_data_codec
            Type3,        // _8byte_quantized_rotation_only_codec
            Type4,        // byte_keyframe_lightly_quantized
            Type5,        // word_keyframe_lightly_quantized
            Type6,        // reverse_byte_keyframe_lightly_quantized
            Type7,        // reverse_word_keyframe_lightly_quantized
            Type8,        // _blend_screen_codec
            Type9         // curve_codec
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