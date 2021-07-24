using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    /// <summary>
    /// Represents a list of planes that forms a relatively flat surface. Used in lighting code.
    /// </summary>
    [TagStructure(Size = 0x4, MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
    [TagStructure(Size = 0x8, MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
    public class SurfacesPlanes : TagStructure
    {
        [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        public ushort PlaneIndexOld;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public int PlaneIndexNew;

        [TagField(MaxVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.Original)]
        public short PlaneCountOld;

        [TagField(MinVersion = CacheVersion.Halo3Retail, Platform = CachePlatform.MCC)]
        [TagField(MinVersion = CacheVersion.Halo3ODST, Platform = CachePlatform.Original)]
        public int PlaneCountNew;
    }

    [TagStructure(Size = 0x4)]
    public class EdgeToSeamMapping : TagStructure
    {
        public short SeamIndex;  //used in the structure seam global
        public short SeamIdentifierIndexEdgeMappingIndex;
    }

    [TagStructure(Size = 0x4)]
    public class PlaneReference : TagStructure
    {
        public short Unknown1;
        public short ClusterIndex;
    }

    [TagStructure(Size = 0x8)]
    public class SurfaceReference : TagStructure
    {
        public short StripIndex;
        public short LightmapTriangleIndex;
        public int BspNodeIndex;
    }
}
