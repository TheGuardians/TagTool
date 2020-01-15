using TagTool.Common;
using TagTool.Tags;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Havok
{
    [TagStructure(Size = 0x34)]
    public class HavokDatum : TagStructure
    {
        public int PrefabIndex;
        public TagBlock<HavokGeometry> HavokGeometries;
        public TagBlock<HavokGeometry> HavokInvertedGeometries;
        public RealPoint3d ShapesBoundsMinimum;
        public RealPoint3d ShapesBoundsMaximum;

        [TagStructure(Size = 0x20)]
        public class HavokGeometry : TagStructure
        {
            public int Unknown;
            public int CollisionType;
            public int ShapeCount;
            public TagData Data;
        }
    }
}
