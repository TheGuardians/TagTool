using System;
using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions.Gen4;

namespace TagTool.Tags.Resources.Gen4
{
    [TagStructure(Size = 0xC)]
    public class ModelAnimationTagResource : TagStructure
    {
        public TagBlock<ModelAnimationTagResourceMember> GroupMembers;
        
        [TagStructure(Size = 0x68)]
        public class ModelAnimationTagResourceMember : TagStructure
        {
            public int AnimationIndex;
            public uint AnimationChecksum;
            public short FrameCount;
            public byte NodeCount;
            public FrameInfoTypeEnum MovementDataType;
            public PackedDataSizesStructActual DataSizes;
            [TagField(DataAlign = 0x10)]
            public TagData AnimationData;
            
            public enum FrameInfoTypeEnum : sbyte
            {
                None,
                DxDy,
                DxDyDyaw,
                DxDyDzDyaw,
                DxDyDzDangleAxis,
                XYZAbsolute,
                Auto
            }

            [TagStructure(Size = 0x48)]
            public class PackedDataSizesStructActual : TagStructure
            {
                public int StaticDataSize;
                public int CompressedDataSize;
                public int StaticNodeFlags;
                public int AnimatedNodeFlags;
                public int MovementData;
                public int PillOffsetData;

                //These fields are only present in reach+, and seem to be the sizes for some additional animation data.
                //The animation data uses the same codec layout as regular animation data, and is tacked on to the AnimationData after the nodeflags
                [TagField(Length = 11)]
                public int[] UnknownDataSizes;

                public int SharedStaticDataSize;
            }

            [TagStructure(Size = 0x48)]
            public class PackedDataSizesStruct : TagStructure
            {
                public int StaticNodeFlags;
                public int AnimatedNodeFlags;
                public int MovementData;
                public int PillOffsetData;
                public int DefaultData;
                public int UncompressedData;
                public int CompressedData;
                public int BlendScreenData;
                public int ObjectSpaceOffsetData;
                public int IkChainEventData;
                public int IkChainControlData;
                public int IkChainProxyData;
                public int IkChainPoleVectorData;
                public int UncompressedObjectSpaceData;
                public int FikAnchorData;
                public int UncompressedObjectSpaceNodeFlags;
                public int CompressedEventCurve;
                public int CompressedStaticPose;
            }
        }
    }
}
