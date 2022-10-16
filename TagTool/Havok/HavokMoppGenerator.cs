using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Geometry.BspCollisionGeometry;

namespace TagTool.Havok
{
    public static partial class HavokMoppGenerator
    {
        public static RenderGeometry.PerMeshMoppBlock GeneratePerMeshMopp(GameCache cache, Mesh mesh)
        {
            return PerMeshMoppGenerator.Generate(cache, mesh);
        }

        public static TagHkpMoppCode GenerateCollisionMopp(CollisionGeometry bsp)
        {
            return CollisionMoppGenerator.Generate(bsp);
        }
    }
}
