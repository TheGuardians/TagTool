using TagTool.Cache;
using TagTool.Common;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags.Definitions.Gen4
{
    [TagStructure(Name = "pca_animation", Tag = "pcaa", Size = 0x4C)]
    public class PcaAnimation : TagStructure
    {
        [TagField(ValidTags = new [] { "mode" })]
        public CachedTag RenderModel;
        [TagField(ValidTags = new [] { "jmad" })]
        public CachedTag AnimationGraph;
        public PcaAnimationTagFlags Pcaflags;
        public int PcaAnimationCount;
        public int PcaChecksum;
        public List<PcaimportedFrameDatablock> FrameData;
        public List<PcaimportedMeshDataBlock> MeshData;
        public TagResourceReference ApiResource;
        
        [Flags]
        public enum PcaAnimationTagFlags : uint
        {
            LookupTableReady = 1 << 0,
            ResourcesGenerated = 1 << 1,
            ResourcesCommitted = 1 << 2
        }
        
        [TagStructure(Size = 0x14)]
        public class PcaimportedFrameDatablock : TagStructure
        {
            public byte[] Coefficients;
        }
        
        [TagStructure(Size = 0x64)]
        public class PcaimportedMeshDataBlock : TagStructure
        {
            public int RenderMeshIndex;
            public int VerticesPerShape;
            public int VertexBufferIndex;
            public RealVector3d PositionScale;
            public float TensionScale;
            public RealVector3d PositionOffset;
            public float TensionOffset;
            public RealVector3d NormalScale;
            public float StretchScale;
            public RealVector3d NormalOffset;
            public float StretchOffset;
            public List<PcaimportedAnimationDataBlock> Animations;
            public List<RawBlendshapeBlock> RawBlendshapeVerts;
            
            [TagStructure(Size = 0x1C)]
            public class PcaimportedAnimationDataBlock : TagStructure
            {
                public StringId Name;
                public int Offset;
                public int Count;
                public int PcaShapeOffset;
                public int PcaCoefficientCount;
                public TagResourceReference CoefficientResource;
            }
            
            [TagStructure(Size = 0x28)]
            public class RawBlendshapeBlock : TagStructure
            {
                public RealVector3d Position;
                public RealVector3d Normal;
                public RealArgbColor TensionAndAmbientOcclusion;
            }
        }
    }
}
