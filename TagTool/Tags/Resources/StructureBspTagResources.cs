using TagTool.Cache;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Havok;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x18, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x30, MinVersion = CacheVersion.Halo3ODST)]
    public class StructureBspTagResources : TagStructure
    {
        public TagBlock<CollisionGeometry> CollisionBsps;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagBlock<LargeCollisionBspBlock> LargeCollisionBsps;

        public TagBlock<InstancedGeometryBlock> InstancedGeometry;

        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagBlock<HavokDatum> HavokData;
    }
}