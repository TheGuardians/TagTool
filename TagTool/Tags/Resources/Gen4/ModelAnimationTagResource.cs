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
        public List<ModelAnimationTagResourceMember> GroupMembers;
        
        [TagStructure(Size = 0x68)]
        public class ModelAnimationTagResourceMember : TagStructure
        {
            public int AnimationIndex;
            public uint AnimationChecksum;
            public short FrameCount;
            public sbyte NodeCount;
            public FrameInfoTypeEnum MovementDataType;
            public PackedDataSizesStruct DataSizes;
            public byte[] AnimationData;
            
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
