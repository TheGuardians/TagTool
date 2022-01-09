using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class StructureSurface : TagStructure
    {
        [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        public ushort FirstSurfaceToTriangleMappingIndexOld;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public int FirstSurfaceToTriangleMappingIndex;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        public short SurfaceToTriangleMappingCountOld;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public int SurfaceToTriangleMappingCount;
    }

    [TagStructure(Size = 0x8)]
    public class LargeStructureSurface : TagStructure
    {
        public int FirstSurfaceToTriangleMappingIndex;
        public int SurfaceToTriangleMappingCount;
    }

    [TagStructure(Size = 0x4, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class SmallSurfacesPlanes : TagStructure
    {
        public ushort PlaneIndexOld;
        public short PlaneCountOld;
    }

    [TagStructure(Size = 0x4)]
    public class EdgeToSeamMapping : TagStructure
    {
        public short SeamIndex;  //used in the structure seam global
        public short SeamEdgeIndex;
    }

    public struct StructureSurfaceToTriangleMapping
    {
        public int TriangleIndex;
        public int ClusterIndex;

        public StructureSurfaceToTriangleMapping(int triangleIndex = -1, int clusterIndex = -1)
        {
            TriangleIndex = triangleIndex;
            ClusterIndex = clusterIndex;
        }
    }

    [TagStructure(Size = 0x8)]
    public class SurfaceReference : TagStructure
    {
        public short StripIndex;
        public short LightmapTriangleIndex;
        public int BspNodeIndex;
    }
}
