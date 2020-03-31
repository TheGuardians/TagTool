using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Pathfinding;

namespace TagTool.Tags.Resources
{
    [TagStructure(Name = "structure_bsp_cache_file_tag_resources", Size = 0x30)]
    public class StructureBspCacheFileTagResources : TagStructure
	{
        public TagBlock<SurfacesPlanes> SurfacePlanes;
        public TagBlock<PlaneReference> Planes;
        public TagBlock<EdgeToSeamMapping> EdgeToSeams;
        public TagBlock<ResourcePathfinding> PathfindingData;
    }
}