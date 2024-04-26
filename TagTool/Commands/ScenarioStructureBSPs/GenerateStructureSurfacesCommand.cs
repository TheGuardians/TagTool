using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Commands.Common;
using TagTool.Commands.Scenarios;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using Plane = System.Numerics.Plane;

namespace TagTool.Commands.ScenarioStructureBSPs
{
    public class GenerateStructureSurfacesCommand : Command
    {
        const float kPlaneDistanceTolerance = 0.1f;
        const float kPlaneNormalAngleTolerance = 0.1f;
        const float kMinTriangleEdgeLength = 0.0375f;
        const float kMinTriangleClipArea = 0.005f;
        const int kMaxTrianglesPerSurface = 40;

        private GameCache Cache { get; }
        private ScenarioStructureBsp Definition { get; }
        private CachedTag Tag { get; }
        private Stream InternalStream { get; }
        private Scenario ScenarioDef { get; }

        public GenerateStructureSurfacesCommand(GameCache cache, CachedTag tag, ScenarioStructureBsp definition, 
            Stream stream = null, Scenario scnrDefinition = null) :
            base(true,

                "GenerateStructureSurfaces",
                "Attempts to regenerate the structure surface blocks needed by the geometry sampler",

                "GenerateStructureSurfaces",

                "Attempts to regenerate the structure surface blocks needed by the geometry sampler")
        {
            Cache = cache;
            Definition = definition;
            Tag = tag;
            InternalStream = stream;
            ScenarioDef = scnrDefinition;
        }

        public override object Execute(List<string> args)
        {
            // Find and deserialize the lightmap bsp
            Stream stream = (InternalStream != null ? InternalStream : Cache.OpenCacheRead());
            ScenarioLightmapBspData lbsp;
            CachedTag lbspTag;

            string lightmapTagName = Tag.Name + ".Lbsp";
            if (!Cache.TagCache.TryGetCachedTag(lightmapTagName, out lbspTag))
            {
                if (!TryGetLbspTag(stream, out lbspTag))
                    return new TagToolWarning("Structure surface generation aborted: Lbsp not found");
            }

            lbsp = Cache.Deserialize<ScenarioLightmapBspData>(stream, lbspTag);
            var renderGeometry = lbsp.Geometry;
            var renderGeometryResource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(renderGeometry.Resource);
            if (renderGeometryResource == null)
                return false;
            renderGeometry.SetResourceBuffers(renderGeometryResource, false);

            var tagResources = Cache.ResourceCache.GetStructureBspTagResources(Definition.CollisionBspResource);
            if (tagResources == null)
                return false;
            GenerateStructureSurfaces(tagResources, renderGeometry);           
            ((GameCacheHaloOnlineBase)Cache).ResourceCaches.ReplaceResource(Definition.CollisionBspResource.HaloOnlinePageableResource, tagResources);

            //optionally generate structure surfaces for sbsp if none are present
            var cacheFileTagResources = Cache.ResourceCache.GetStructureBspCacheFileTagResources(Definition.PathfindingResource);
            if (cacheFileTagResources == null)
                return false;
            if (cacheFileTagResources.SurfacePlanes == null || cacheFileTagResources.SurfacePlanes.Count == 0)
            {
                GenerateStructureBspStructureSurfaces(tagResources, cacheFileTagResources, renderGeometry);
                ((GameCacheHaloOnlineBase)Cache).ResourceCaches.ReplaceResource(Definition.PathfindingResource.HaloOnlinePageableResource, cacheFileTagResources);
            }
            Console.WriteLine("Finished.");
            return true;
        }

        private bool TryGetLbspTag(Stream stream, out CachedTag lbspTag)
        {
            if (ScenarioDef != null)
            {
                int sbspIndex = ScenarioDef.StructureBsps.FindIndex(x => x.StructureBsp == Tag);
                if (ScenarioDef.Lightmap != null && sbspIndex != -1)
                {
                    ScenarioLightmap sLdT = Cache.Deserialize<ScenarioLightmap>(stream, ScenarioDef.Lightmap);
                    if (sLdT.PerPixelLightmapDataReferences?.Count() > sbspIndex)
                    {
                        lbspTag = sLdT.PerPixelLightmapDataReferences?[sbspIndex].LightmapBspData;
                        return true;
                    }
                }
            }

            lbspTag = null;
            return false;
        }

        public void GenerateStructureBspStructureSurfaces(StructureBspTagResources tagResources, StructureBspCacheFileTagResources cacheFileTagResources, RenderGeometry renderGeometry)
        {           
            // Do nothing if the sbsp has no collision
            if (tagResources.CollisionBsps == null || tagResources.CollisionBsps.Count == 0)
                return;

            var collisionBsp = tagResources.CollisionBsps[0];
            cacheFileTagResources.SurfacePlanes = new TagTool.Tags.TagBlock<StructureSurface>(CacheAddressType.Definition);
            foreach (var surface in collisionBsp.Surfaces)
                cacheFileTagResources.SurfacePlanes.Add(new StructureSurface());
            cacheFileTagResources.Planes = new TagTool.Tags.TagBlock<StructureSurfaceToTriangleMapping>(CacheAddressType.Definition);

            for (var i = 0; i < Definition.Clusters.Count; i++)
            {
                Console.WriteLine($"Generating surface triangles for sbsp cluster ({i + 1}/{Definition.Clusters.Count})...");
                if (Definition.Clusters[i].MeshIndex == -1)
                    continue;
                // Do nothing if the sbsp has no vertex data
                var mesh = renderGeometry.Meshes[Definition.Clusters[i].MeshIndex];
                if (mesh.ResourceIndexBuffers[0] == null || mesh.ResourceVertexBuffers[0] == null)
                    continue;

                List<SurfaceTriangle>[] surfaceTriangleLists = GenerateStructureSurfaceTriangleLists(collisionBsp, new MeshData(Cache, Definition.Clusters[i].MeshIndex, -1, renderGeometry));
                PopulateStructureBspStructureSurfaceBlocks(cacheFileTagResources.SurfacePlanes, cacheFileTagResources.Planes, surfaceTriangleLists, i);
            }            
        }

        public void GenerateStructureSurfaces(StructureBspTagResources tagResources, RenderGeometry renderGeometry)
        {
            for (var definitionIndex = 0; definitionIndex < tagResources.InstancedGeometry.Count; definitionIndex++)
            {
                var instanceDef = tagResources.InstancedGeometry[definitionIndex];
                Console.WriteLine($"Generating surface triangles ({definitionIndex + 1}/{tagResources.InstancedGeometry.Count})...");
                GenerateInstancedGeometryStructureSurfaces(instanceDef, renderGeometry);
            }
        }

        public void GenerateInstancedGeometryStructureSurfaces(InstancedGeometryBlock instanceDef, RenderGeometry renderGeometry)
        {
            // Do nothing if the instanced geometry has no collision
            var collisionBsp = instanceDef.CollisionInfo;
            if (collisionBsp.Surfaces.Count == 0)
                return;

            // Do nothing if the instanced geometry has no vertex data
            var mesh = renderGeometry.Meshes[instanceDef.MeshIndex];
            if (mesh.ResourceIndexBuffers[0] == null || mesh.ResourceVertexBuffers[0] == null)
                return;

            List<SurfaceTriangle>[] surfaceTriangleLists = GenerateStructureSurfaceTriangleLists(collisionBsp, new MeshData(Cache, instanceDef.MeshIndex, instanceDef.CompressionIndex, renderGeometry));

            instanceDef.SurfacePlanes = new TagTool.Tags.TagBlock<StructureSurface>(CacheAddressType.Definition);
            instanceDef.Planes = new TagTool.Tags.TagBlock<StructureSurfaceToTriangleMapping>(CacheAddressType.Definition);
            PopulateStructureSurfaceBlocks(instanceDef.SurfacePlanes, instanceDef.Planes, surfaceTriangleLists);
        }

        public static void PopulateStructureBspStructureSurfaceBlocks(
            IList<StructureSurface> structureSurfaceBlock,
            IList<StructureSurfaceToTriangleMapping> triangleMappingBlock,
            List<SurfaceTriangle>[] surfaceTriangleLists,
            int clusterIndex = 0)
        {
            for (int surfaceIndex = 0; surfaceIndex < surfaceTriangleLists.Length; surfaceIndex++)
            {
                // Empty lists should still have a structure surface block.
                var structureSurface = new StructureSurface();
                var triangleList = surfaceTriangleLists[surfaceIndex];
                if (triangleList.Count > 0)
                {
                    if (structureSurfaceBlock[surfaceIndex].SurfaceToTriangleMappingCount > triangleList.Count)
                        continue;

                    // Construct the triangle mapping block
                    structureSurface.FirstSurfaceToTriangleMappingIndex = triangleMappingBlock.Count;
                    foreach (var triangle in triangleList)
                    {
                        triangleMappingBlock.Add(new StructureSurfaceToTriangleMapping() { ClusterIndex = clusterIndex, TriangleIndex = triangle.Index + 2 });
                        structureSurface.SurfaceToTriangleMappingCount++;
                    }
                    structureSurfaceBlock[surfaceIndex] = structureSurface;
                }
            }
        }

        public static void PopulateStructureSurfaceBlocks(
            IList<StructureSurface> structureSurfaceBlock,
            IList<StructureSurfaceToTriangleMapping> triangleMappingBlock,
            List<SurfaceTriangle>[] surfaceTriangleLists,
            int clusterIndex = 0)
        {
            structureSurfaceBlock.Clear();
            triangleMappingBlock.Clear();
            for (int surfaceIndex = 0; surfaceIndex < surfaceTriangleLists.Length; surfaceIndex++)
            {
                // Empty lists should still have a structure surface block.
                var structureSurface = new StructureSurface();
                structureSurfaceBlock.Add(structureSurface);

                var triangleList = surfaceTriangleLists[surfaceIndex];
                if (triangleList.Count > 0)
                {
                    // Construct the triangle mapping block
                    structureSurface.FirstSurfaceToTriangleMappingIndex = triangleMappingBlock.Count;
                    foreach (var triangle in triangleList)
                    {
                        triangleMappingBlock.Add(new StructureSurfaceToTriangleMapping() { ClusterIndex = clusterIndex, TriangleIndex = triangle.Index + 2 });
                        structureSurface.SurfaceToTriangleMappingCount++;
                    }
                }
            }
        }

        public static List<SurfaceTriangle>[] GenerateStructureSurfaceTriangleLists(CollisionGeometry collisionBsp, MeshData meshData)
        {
            var surfaceTriangleLists = new List<SurfaceTriangle>[collisionBsp.Surfaces.Count];
            for(var surfaceIndex = 0; surfaceIndex < collisionBsp.Surfaces.Count; surfaceIndex++)
            {
                surfaceTriangleLists[surfaceIndex] = GenerateSurfaceTrianglesList(collisionBsp, surfaceIndex, meshData);
            };
            return surfaceTriangleLists;
        }

        public static List<SurfaceTriangle> GenerateSurfaceTrianglesList(CollisionGeometry collisionBsp, int surfaceIndex, MeshData mesh)
        {
            var result = new List<SurfaceTriangle>();

            // Get the data we need from the collision bsp
            Surface surface = collisionBsp.Surfaces[surfaceIndex];
            List<Vector3> surfaceVertices = CollectSurfaceVertices(collisionBsp, surfaceIndex);
            Plane plane = GetSurfacePlane(collisionBsp, surface, surfaceVertices);

            // Setup the plane projection parameters
            int projection = ProjectionFromVector3d(plane.Normal);
            int projectionSign = ProjectionSignFromVector3d(plane.Normal, projection);

            // Project the surface onto the plane so that we can work in 2d.
            List<Vector2> projSurface = ProjectPolygon3d(surfaceVertices, projection, projectionSign);
            // Calculate the surface area of the now 2d collision surface polygon
            float surfaceArea = CalculatePolygonArea2d(projSurface);

            // Iterate through each mesh part
            for (int partIndex = 0; partIndex < mesh.Mesh.Parts.Count; partIndex++)
            {
                var part = mesh.Mesh.Parts[partIndex];

                // Iterate through each triangle
                for (int i = part.FirstIndex; i < part.FirstIndex + part.IndexCount; i += 3)
                {
                    // Extract the triangle vertices from the mesh
                    var triangle = new[] {
                        mesh.Vertices[mesh.Indices[i + 0]].Position,
                        mesh.Vertices[mesh.Indices[i + 1]].Position,
                        mesh.Vertices[mesh.Indices[i + 2]].Position,
                    };

                    // Calculate the triangle normal
                    var triangleNormal = Vector3.Normalize(Vector3.Cross(triangle[1] - triangle[0], triangle[2] - triangle[0]));
                    // Avoid any triangles that are angled too much away from the plane.
                    float dot = Vector3.Dot(plane.Normal, triangleNormal);
                    if (dot < (1.0f - kPlaneNormalAngleTolerance))
                        continue;

                    // Ignore triangles that are too far away. There has to be a generous tolerance here to work well with all geometry.
                    if (!CheckPlanePoints3d(plane, triangle, kPlaneDistanceTolerance, out float maxPlaneDist))
                        continue;
                    // Avoid skinny triangles. originally I used angles, but I found side length to work better.
                    if (!CheckPolygonEdgeLengths3d(triangle, kMinTriangleEdgeLength))
                        continue;

                    // Project the triangle onto the surface plane
                    var projTriangle = ProjectPolygon3d(triangle, projection, projectionSign);
                    // Clip the triangle to the surface returning a new polygon (the intersection)
                    var intersection = ClipPolygon2d(projSurface, projTriangle);
                    // Check that we have a valid polygon
                    if (intersection.Count < 3)
                        continue;

                    // Calculate the area of the triangle
                    float triangleArea = CalculatePolygonArea2d(projTriangle);
                    // Calculate the area of the surface the triangle actually covers.
                    float clippedArea = CalculatePolygonArea2d(intersection);

                    // Avoid considering any triangles that barely touch the surface.
                    if ((clippedArea / surfaceArea) < kMinTriangleClipArea)
                        continue;

                    result.Add(new SurfaceTriangle()
                    {
                        Index = i,
                        Area = triangleArea,
                        ClippedArea = clippedArea,
                        SurfaceHeight = maxPlaneDist,
                        Vertices = triangle,
                        Projection = projTriangle,
                        Clipped = intersection
                    });
                }
            }

            // Sort by clipped area (descending)
            result.Sort((a, b) => (a.ClippedArea - b.ClippedArea) > 0.00001f ? -1 : 1);

            if (result.Count > kMaxTrianglesPerSurface)
                result.RemoveRange(kMaxTrianglesPerSurface, result.Count - kMaxTrianglesPerSurface);

            return result;
        }

        private static Plane GetSurfacePlane(CollisionGeometry collisionBsp, Surface surface, List<Vector3> surfaceVertices)
        {
            // Negate the plane if needed.
            var tmpPlane = collisionBsp.Planes[surface.Plane & 0x7fff].Value;
            if ((surface.Plane & 0x8000) != 0)
            {
                tmpPlane.Normal = tmpPlane.Normal *= -1;
                tmpPlane.D = -tmpPlane.D;
            }
            var plane = new Plane();
            plane.Normal = Vector3.Normalize(ToVector3(tmpPlane.Normal));
            plane.D = Vector3.Dot(plane.Normal, surfaceVertices[0]);
            return plane;
        }

        static float Percentile(IList<float> values, float k)
        {
            var pos = (values.Count - 1) * k;
            var index = (int)pos;
            var fractionPart = pos - index;
            var p = values[index];
            if (fractionPart > 0)
                p += fractionPart * (values[index + 1] - values[index]);
            return p;
        }

        private static List<Vector2> ProjectPolygon3d(IList<Vector3> points, int projection, int projectionSign)
        {
            var result = new List<Vector2>();
            foreach (var point in points)
                result.Add(ProjectPoint3d(point, projection, projectionSign));
            return result;
        }

        private static bool PointInPolygon2d(Vector2 point, List<Vector2> points)
        {
            bool result = false;
            int i, j;
            for (i = 0, j = points.Count - 1; i < points.Count; j = i++)
            {
                if ((points[i].Y > point.Y) != (points[j].Y > point.Y) &&
                    (point.X < (points[j].X - points[i].X) * (point.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
                {
                    result = !result;
                }
            }
            return result;
        }

        private static bool RayIntersection2d(Vector2 a, Vector2 ab, Vector2 c, Vector2 cd, out float s, out float t)
        {
            if (Math.Abs(((ab.X * cd.Y) - (ab.Y * cd.X))) > 0.0001f)
            {
                s = (((c.X - a.X) * -cd.Y) + ((c.Y - a.Y) * cd.X)) / ((ab.Y * cd.X) + (ab.X * -cd.Y));
                t = (((c.X - a.X) * -ab.Y) + ((c.Y - a.Y) * ab.X)) / ((ab.Y * cd.X) + (ab.X * -cd.Y));

                return true;
            }
            s = t = 0;
            return false;
        }

        private static List<Vector2> ClipPolygon2d(List<Vector2> subject, List<Vector2> clip)
        {
            bool isInside(Vector2 a, Vector2 b, Vector2 p) => ((b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X)) > 0;

            Vector2 intersectLineSegments(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
            {
                Vector2 ab = b - a;
                Vector2 cd = d - c;
                float s, t;
                // we already know they interesect so no need to check the return value
                RayIntersection2d(a, ab, c, cd, out s, out t);
                if (s < 0) s = 0;
                if (s > 1) s = 1;
                return a + s * ab;
            }

            List<Vector2> inputList;
            var outputList = subject.ToList();

            for (int i1 = 0, i0 = clip.Count - 1; i1 < clip.Count; i0 = i1++)
            {
                inputList = outputList.ToList();
                outputList.Clear();

                Vector2 c_edge0 = clip[i0];
                Vector2 c_edge1 = clip[i1];

                for (int j1 = 0, j0 = inputList.Count - 1; j1 < inputList.Count; j0 = j1++)
                {
                    Vector2 s_edge0 = inputList[j0];
                    Vector2 s_edge1 = inputList[j1];

                    if (isInside(c_edge0, c_edge1, s_edge1))
                    {
                        if (!isInside(c_edge0, c_edge1, s_edge0))
                        {
                            outputList.Add(intersectLineSegments(s_edge0, s_edge1, c_edge0, c_edge1));
                        }

                        outputList.Add(s_edge1);
                    }
                    else if (isInside(c_edge0, c_edge1, s_edge0))
                    {
                        outputList.Add(intersectLineSegments(s_edge0, s_edge1, c_edge0, c_edge1));
                    }
                }
            }

            return outputList;
        }

        private static List<Vector2> ConvexHull2d(List<Vector2> points)
        {
            float cross(Vector2 a, Vector2 b, Vector2 o) => (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);

            points = points.ToList();

            points.Sort((a, b) =>
            {
                if (a.X == b.X)
                    return a.Y < b.Y ? -1 : 1;
                else
                    return a.X < b.X ? -1 : 1;
            });

            var L = new List<Vector2>();
            for (var i = 0; i < points.Count; i++)
            {
                while (L.Count >= 2 && cross(L[L.Count - 2], L[L.Count - 1], points[i]) <= 0)
                {
                    L.RemoveAt(L.Count - 1);
                }
                L.Add(points[i]);
            }
            var U = new List<Vector2>();
            for (var i = points.Count - 1; i >= 0; i--)
            {
                while (U.Count >= 2 && cross(U[U.Count - 2], U[U.Count - 1], points[i]) <= 0)
                {
                    U.RemoveAt(U.Count - 1);
                }
                U.Add(points[i]);
            }
            L.RemoveAt(L.Count - 1);
            U.RemoveAt(U.Count - 1);

            return L.Concat(U).ToList();
        }

        private static float CalculatePolygonArea2d(List<Vector2> points)
        {
            float sum = 0.0f;
            for (int i = 0, i1 = points.Count - 1; i < points.Count; i1 = i++)
            {
                var a = points[i];
                var b = points[i1];
                sum += ((a.X * b.Y) - (a.Y * b.X));
            }
            return Math.Abs(sum) * 0.5f;
        }

        public static bool PointInPolygon2d(IList<Vector3> points, Vector3 test)
        {
            bool result = false;
            int i, j;
            for (i = 0, j = points.Count - 1; i < points.Count; j = i++)
            {
                if ((points[i].Y > test.Y) != (points[j].Y > test.Y) &&
                    (test.X < (points[j].X - points[i].X) * (test.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
                {
                    result = !result;
                }
            }
            return result;
        }

        private static bool CheckPolygonEdgeLengths3d(IList<Vector3> points, float minLength)
        {
            for (var i = 0; i < points.Count; i++)
            {
                if ((points[(i + 1) % points.Count] - points[i]).Length() < minLength)
                    return false;
            }
            return true;
        }

        static float PlaneDistanceToPoint(Plane plane, Vector3 point)
        {
            return Plane.DotNormal(plane, point) - plane.D;
        }

        static void PlaneDistanceToPolygon(Plane plane, IList<Vector3> points, out float minDist, out float maxDist)
        {
            minDist = float.MaxValue;
            maxDist = -float.MaxValue;
            foreach (var point in points)
            {
                float distance = PlaneDistanceToPoint(plane, point);
                if (distance < minDist)
                    minDist = distance;
                if (distance > maxDist)
                    maxDist = distance;
            }
        }

        static bool CheckPlanePoints3d(Plane plane, IList<Vector3> points, float pointDistanceTolerance, out float outMaxDistance)
        {
            outMaxDistance = -float.MaxValue;
            foreach (var point in points)
            {
                float distance = PlaneDistanceToPoint(plane, point);
                if (distance < -pointDistanceTolerance || distance > pointDistanceTolerance)
                    return false;

                if (distance > outMaxDistance)
                    outMaxDistance = distance;
            }
            return true;
        }

        static readonly short[] kProjectionMapping = { 2, 1, 0, 1, 2, 0, 0, 2, 1, 2, 0, 1, 1, 0, 2, 0, 1, 2 };

        static int ProjectionFromVector3d(Vector3 vector)
        {
            float i = Math.Abs(vector.X);
            float j = Math.Abs(vector.Y);
            float k = Math.Abs(vector.Z);
            if (k < j || k < i)
                return j >= i ? 1 : 0;
            else
                return 2;
        }

        static int ProjectionSignFromVector3d(Vector3 vector, int projection)
        {
            return VectorGetComponent(vector, projection) > 0.0f ? 1 : 0;
        }

        private static Vector2 ProjectPoint3d(Vector3 point, int projection, int projectionSign)
        {
            int row = projection * 6 + 3 * projectionSign;
            return new Vector2(VectorGetComponent(point, kProjectionMapping[row]), VectorGetComponent(point, kProjectionMapping[row + 1]));
        }

        public static void VectorSetComponent(ref Vector2 vector, int index, float value)
        {
            switch (index)
            {
                case 0: vector.X = value; break;
                case 1: vector.Y = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public static void VectorSetComponent(ref Vector3 vector, int index, float value)
        {
            switch (index)
            {
                case 0: vector.X = value; break;
                case 1: vector.Y = value; break;
                case 2: vector.Z = value; break;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public static float VectorGetComponent(Vector2 vector, int index)
        {
            switch (index)
            {
                case 0: return vector.X;
                case 1: return vector.Y;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        public static float VectorGetComponent(Vector3 vector, int index)
        {
            switch (index)
            {
                case 0: return vector.X;
                case 1: return vector.Y;
                case 2: return vector.Z;
                default: throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        private static List<Vector3> CollectSurfaceVertices(CollisionGeometry bsp, int surfaceIndex)
        {
            var vertices = new List<Vector3>();
            var surface = bsp.Surfaces[surfaceIndex];
            int edgeIndex = surface.FirstEdge;
            do
            {
                var edge = bsp.Edges[edgeIndex];
                if (edge.RightSurface == surfaceIndex)
                {
                    vertices.Add(ToVector3(bsp.Vertices[edge.EndVertex].Point));
                    edgeIndex = edge.ReverseEdge;
                }
                else
                {
                    vertices.Add(ToVector3(bsp.Vertices[edge.StartVertex].Point));
                    edgeIndex = edge.ForwardEdge;
                }

            } while (edgeIndex != surface.FirstEdge);
            return vertices;
        }


        static Vector3 ToVector3(RealPoint3d point) => new Vector3(point.X, point.Y, point.Z);
        static Vector3 ToVector3(RealVector2d vector) => new Vector3(vector.I, vector.J, 0);
        static Vector3 ToVector3(RealVector3d vector) => new Vector3(vector.I, vector.J, vector.K);

        static ScenarioLightmapBspData GetScenarioLightmapBspData(GameCache cache, Stream stream, ScenarioLightmap lightmap, int sbspIndex)
        {
            if (cache.Version <= CacheVersion.Halo3Retail)
                return lightmap.Lightmaps.Find(x => x.BspIndex == sbspIndex);
            else
                return cache.Deserialize<ScenarioLightmapBspData>(stream, lightmap.PerPixelLightmapDataReferences[sbspIndex].LightmapBspData);
        }

        public class GenericVertex
        {
            public Vector3 Position { get; set; }
            public Vector3 Normal { get; set; }
            public Vector3 TexCoords { get; set; }
            public Vector3 Tangents { get; set; }
            public Vector3 Binormals { get; set; }
            public byte[] Indices { get; set; }
            public float[] Weights { get; set; }
        }

        public class SurfaceTriangle
        {
            public float ClippedArea;
            public float Area;
            public float SurfaceHeight;
            public int Index;
            public IList<Vector3> Vertices;
            public IList<Vector2> Projection;
            public IList<Vector2> Clipped;

            public override string ToString() => $"Index={Index}, Area={Area}, ClippedArea={ClippedArea}";
        }

        public class MeshData
        {
            public List<GenericVertex> Vertices;
            public ushort[] Indices;
            public Mesh Mesh;

            public MeshData(GameCache cache, int meshIndex, int compressionIndex, RenderGeometry renderGeometry)
            {
                Mesh = renderGeometry.Meshes[meshIndex];
                Indices = ReadMeshIndices(cache, Mesh);
                Vertices = ReadMeshVertices(cache, Mesh, compressionIndex == -1 ? null : new VertexCompressor(renderGeometry.Compression[compressionIndex]));
            }

            private static ushort[] ReadMeshIndices(GameCache cache, Mesh mesh)
            {
                var indexBuffer = mesh.ResourceIndexBuffers[0];
                var indexStream = new IndexBufferStream(new MemoryStream(indexBuffer.Data.Data), cache.Endianness);
                return indexStream.ReadIndices((uint)(indexBuffer.Data.Data.Length / 2));
            }

            private static List<GenericVertex> ReadMeshVertices(GameCache cache, Mesh mesh, VertexCompressor compressor = null)
            {
                var vertexBuffer = mesh.ResourceVertexBuffers[0];
                var vertexStream = VertexStreamFactory.Create(cache.Version, cache.Platform, new MemoryStream(vertexBuffer.Data.Data));

                var result = new List<GenericVertex>();
                for (var i = 0; i < vertexBuffer.Count; i++)
                {
                    var rigid = vertexStream.ReadRigidVertex();
                    if(compressor != null)
                    {
                        rigid.Position = compressor.DecompressPosition(rigid.Position);
                        rigid.Texcoord = compressor.DecompressUv(rigid.Texcoord);
                    }                  

                    result.Add(new GenericVertex
                    {
                        Position = ToVector3(rigid.Position.IJK),
                        Normal = ToVector3(rigid.Normal),
                        TexCoords = ToVector3(rigid.Texcoord),
                        Tangents = ToVector3(rigid.Tangent.IJK),
                        Binormals = ToVector3(rigid.Binormal)
                    });
                }
                return result;
            }
        }
    }
}
