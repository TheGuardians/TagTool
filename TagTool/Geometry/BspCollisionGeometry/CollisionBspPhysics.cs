using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Havok;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry
{
    [TagStructure(Size = 0x70, MaxVersion = CacheVersion.Halo3Retail)]
    [TagStructure(Size = 0x80, MinVersion = CacheVersion.Halo3ODST)]
    public class CollisionBspPhysicsDefinition : TagStructure
    {
        public CollisionGeometryShape GeometryShape;
        public HkpBvMoppTreeShape MoppBvTreeShape;
    }
}
