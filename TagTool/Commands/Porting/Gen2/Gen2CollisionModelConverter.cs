using System.IO;
using System.Numerics;
using System.Linq;
using TagTool.Cache;
using TagTool.Cache.HaloOnline;
using TagTool.Tags.Definitions;
using TagTool.Geometry.Jms;
using TagTool.Common;
using MIConvexHull;
using TagTool.Commands.PhysicsModels;
using System.Collections.Generic;

namespace TagTool.Commands.Porting.Gen2
{
    public class ObjConvexHullProcessor
    {

        public CachedTag ConvertCollisionModelToPhysics(CollisionModel collisionModel, Stream cacheStream, CachedTag tag, GameCacheHaloOnlineBase Cache)
        {
            JmsFormat jms = new JmsFormat();

            // Retrieve the hlmt tag that corresponds to the collisionModel
            Model hlmt = GetHlmtFromCollisionModel(tag, cacheStream, Cache);

            // Add nodes to the JMS structure, extracted from the hlmt tag
            BuildNodesFromHlmt(jms, hlmt, Cache);

            // Use the JmsCollExporter to handle the conversion of collision geometry to JMS format
            JmsCollExporter exporter = new JmsCollExporter(Cache, jms);
            exporter.Export(collisionModel);

            GenerateConvexHullForJms(jms);

            // Step 1: Identify referenced vertices
            HashSet<int> referencedVertices = new HashSet<int>();
            foreach (var triangle in jms.Triangles)
            {
                foreach (var vertexIndex in triangle.VertexIndices)
                {
                    referencedVertices.Add(vertexIndex);
                }
            }

            // Step 2: Create a mapping from old indices to new indices
            Dictionary<int, int> indexMapping = new Dictionary<int, int>();
            int newIndex = 0;
            for (int i = 0; i < jms.Vertices.Count; i++)
            {
                if (referencedVertices.Contains(i))
                {
                    indexMapping[i] = newIndex++;
                }
            }

            // Step 3: Filter vertices
            var filteredVertices = jms.Vertices.Where((v, i) => referencedVertices.Contains(i)).ToList();
            jms.Vertices = filteredVertices;

            // Step 4: Update triangle indices
            foreach (var triangle in jms.Triangles)
            {
                for (int i = 0; i < triangle.VertexIndices.Count; i++)
                {
                    triangle.VertexIndices[i] = indexMapping[triangle.VertexIndices[i]];
                }
            }

            var generator = new PhysicsModelGenerator(Cache);

            var result = generator.GeneratePhysicsModelFromJms(cacheStream, jms, tag.Name.ToString(), false, collisionModel, cacheStream);

            var newTag = (CachedTag)result;

            return newTag;
        }

        private Model GetHlmtFromCollisionModel(CachedTag tag, Stream cacheStream, GameCacheHaloOnlineBase Cache)
        {
            var dependsOn = Cache.TagCacheGenHO.NonNull().Where(t => ((CachedTagHaloOnline)t).Dependencies.Contains(tag.Index));
            CachedTag modelTag = null;

            foreach (var dependency in dependsOn)
            {
                var tagName = dependency?.Name ?? $"0x{dependency.Index:X4}";

                if (dependency.Group.ToString() == "model")
                {
                    modelTag = Cache.TagCacheGenHO.GetTag($"{tagName}.{dependency.Group}");
                }
            }
            return Cache.Deserialize<Model>(cacheStream, modelTag);
        }

        public Matrix4x4 MatrixFromNode(RealQuaternion rotation, RealVector3d position)
        {
            var quat = new Quaternion(rotation.I, rotation.J, rotation.K, rotation.W);

            Matrix4x4 rot = Matrix4x4.CreateFromQuaternion(quat);
            rot.Translation = new Vector3(position.I, position.J, position.K);

            return rot;
        }

        public void BuildNodesFromHlmt(JmsFormat jms, Model hlmt, GameCacheHaloOnlineBase Cache)
        {
            foreach (var node in hlmt.Nodes)
            {
                var newnode = new JmsFormat.JmsNode
                {
                    Name = Cache.StringTable.GetString(node.Name),
                    ParentNodeIndex = node.ParentNode,
                    Rotation = node.DefaultRotation,
                    Position = new RealVector3d(node.DefaultTranslation.X, node.DefaultTranslation.Y, node.DefaultTranslation.Z) * 100.0f
                };
                if (!newnode.Name.StartsWith("b_"))
                    newnode.Name = "b_" + newnode.Name;
                if (newnode.ParentNodeIndex != -1)
                {
                    Matrix4x4 transform = MatrixFromNode(newnode.Rotation, newnode.Position);
                    Matrix4x4 parent_transform = MatrixFromNode(jms.Nodes[newnode.ParentNodeIndex].Rotation,
                        jms.Nodes[newnode.ParentNodeIndex].Position);
                    Matrix4x4 result = Matrix4x4.Multiply(transform, parent_transform);

                    Vector3 out_scale = new Vector3();
                    Vector3 out_translate = new Vector3();
                    Quaternion out_rotate = new Quaternion();
                    Matrix4x4.Decompose(result, out out_scale, out out_rotate, out out_translate);
                    newnode.Position = new RealVector3d(out_translate.X * out_scale.X, out_translate.Y * out_scale.Y, out_translate.Z * out_scale.Z);
                    newnode.Rotation = new RealQuaternion(out_rotate.X, out_rotate.Y, out_rotate.Z, out_rotate.W);
                }

                jms.Nodes.Add(newnode);
            }
        }

        public void GenerateConvexHullForJms(JmsFormat jms)
        {
            // Convert JMS vertices to MIConvexHull vertices
            var vertices = jms.Vertices.Select(v => new Vertex(v.Position.X, v.Position.Y, v.Position.Z)).ToList();

            // Generate the convex hull
            var convexHull = ConvexHull.Create(vertices);

            // Update JMS vertices and triangles with the convex hull data
            //jms.Vertices.Clear();
            jms.Triangles.Clear();

            //foreach (var vertex in convexHull.Result.Points)
            //{
            //    jms.Vertices.Add(new JmsFormat.JmsVertex
            //    {
            //        Position = new RealPoint3d((float)vertex.Position[0], (float)vertex.Position[1], (float)vertex.Position[2]),
            //        Normal = new RealVector3d(0, 0, 0), // Assuming a default normal; calculate if needed
            //        NodeSets = new System.Collections.Generic.List<JmsFormat.JmsVertex.NodeSet>
            //        {
            //            new JmsFormat.JmsVertex.NodeSet
            //            {
            //                // Assuming a default node index of 0; adjust as needed
            //                NodeIndex = 0,
            //                NodeWeight = 1.0f // Full influence from the default node
            //            }
            //        },
            //        UvSets = new System.Collections.Generic.List<JmsFormat.JmsVertex.UvSet>
            //        {
            //            new JmsFormat.JmsVertex.UvSet
            //            {
            //                // Default UV coordinates
            //                TextureCoordinates = new RealPoint2d(0.0f, 0.0f)
            //            }
            //        },
            //        VertexColor = new RealRgbColor(0, 0, 0) // Default vertex color; adjust as needed
            //    });
            //}

            foreach (var face in convexHull.Result.Faces)
            {
                // Map the convex hull face vertices back to their indices in jms.Vertices
                var vertexIndices = face.Vertices
                    .Select(v => vertices.IndexOf(v)) // Get the index of each vertex in the convexVertices list
                    .ToList();

                jms.Triangles.Add(new JmsFormat.JmsTriangle
                {
                    MaterialIndex = 0, // Assign the appropriate material index
                    VertexIndices = vertexIndices // Use the mapped indices for the JmsFormat
                });
            }
        }
    }

    // Definition of the vertex for MIConvexHull
    public class Vertex : IVertex
    {
        public double[] Position { get; set; }

        public Vertex(double x, double y, double z)
        {
            Position = new[] { x, y, z };
        }
    }
}
