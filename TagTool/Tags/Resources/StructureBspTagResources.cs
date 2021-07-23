using TagTool.Cache;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Havok;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x18, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x30, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Name = "structure_bsp_tag_resources", Size = 0x30, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class StructureBspTagResources : TagStructure
    {
        public TagBlock<CollisionGeometry> CollisionBsps;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public TagBlock<LargeCollisionBspBlock> LargeCollisionBsps;

        public TagBlock<InstancedGeometryBlock> InstancedGeometry;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.Halo3ODST)]
        public TagBlock<HavokDatum> HavokData;
    }
}