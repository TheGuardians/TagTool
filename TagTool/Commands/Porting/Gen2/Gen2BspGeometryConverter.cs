using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TagTool.Commands.Porting.Gen2.Gen2VertexDefinitions;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Commands.Common;
using TagTool.Tags;
using TagTool.Cache;
using RenderModelGen2 = TagTool.Tags.Definitions.Gen2.RenderModel;

namespace TagTool.Commands.Porting.Gen2
{
    class Gen2BspGeometryConverter
    {
        public static void BuildMeshes(RenderModelBuilder builder, List<Gen2BSPResourceMesh> meshes, RenderGeometryClassification geometrytype, int maxvertexnodes, int rigidnode)
        {
            foreach (var mesh in meshes)
            {
                builder.BeginMesh();

                var worldVertices = new List<WorldVertex>();
                var rigidVertices = new List<RigidVertex>();
                var skinnedVertices = new List<SkinnedVertex>();

                for (var vertex_index = 0; vertex_index < mesh.RawVertices.Count; vertex_index++)
                {
                    var vertex = mesh.RawVertices[vertex_index];
                    switch (geometrytype)
                    {
                        case RenderGeometryClassification.Worldspace:
                            worldVertices.Add(new WorldVertex
                            {
                                Position = new RealQuaternion(vertex.Point.Position.ToArray()),
                                Texcoord = vertex.Texcoord.IJ,
                                Normal = vertex.Normal,
                                Tangent = new RealQuaternion(vertex.Tangent.ToArray()),
                                Binormal = vertex.Binormal
                            });
                            break;

                        case RenderGeometryClassification.Rigid:
                            rigidVertices.Add(new RigidVertex
                            {
                                Position = new RealQuaternion(vertex.Point.Position.ToArray()),
                                Texcoord = vertex.Texcoord.IJ,
                                Normal = vertex.Normal,
                                Tangent = new RealQuaternion(vertex.Tangent.ToArray()),
                                Binormal = vertex.Binormal
                            });
                            break;

                        case RenderGeometryClassification.RigidBoned:
                        case RenderGeometryClassification.Skinned:
                            SkinnedVertex newskinned = new SkinnedVertex
                            {
                                Position = new RealQuaternion(vertex.Point.Position.ToArray()),
                                Texcoord = vertex.Texcoord.IJ,
                                Normal = vertex.Normal,
                                Tangent = new RealQuaternion(vertex.Tangent.ToArray()),
                                Binormal = vertex.Binormal,
                                BlendIndices = vertex.Point.NodeIndices.Select(i => (byte)i).ToArray(),
                                BlendWeights = geometrytype == RenderGeometryClassification.RigidBoned ?
                                    new[] { 1.0f, 0.0f, 0.0f, 0.0f } :
                                    vertex.Point.NodeWeights
                            };
                            skinnedVertices.Add(newskinned);
                            break;

                        default:
                            throw new NotSupportedException(geometrytype.ToString());
                    }
                }

                var indices = new List<ushort>();

                foreach (var part in mesh.Parts)
                {
                    var partIndices = new HashSet<short>();

                    for (var i = part.FirstIndex; i < part.FirstIndex + part.IndexCount; i++)
                        if (!partIndices.Contains(mesh.StripIndices[i].Index))
                            partIndices.Add(mesh.StripIndices[i].Index);

                    builder.BeginPart(part.MaterialIndex, part.FirstIndex, part.IndexCount, (ushort)partIndices.Count);

                    for (var i = 0; i < part.SubPartCount; i++)
                    {
                        var subPart = mesh.SubParts[part.FirstSubPartIndex + i];
                        var subPartIndices = new HashSet<short>();

                        for (var j = subPart.FirstIndex; j < subPart.FirstIndex + subPart.IndexCount; j++)
                            if (!subPartIndices.Contains(mesh.StripIndices[j].Index))
                                subPartIndices.Add(mesh.StripIndices[j].Index);

                        builder.DefineSubPart((ushort)subPart.FirstIndex.Value, (ushort)subPart.IndexCount.Value, (ushort)subPartIndices.Count);
                    }

                    builder.EndPart();
                }

                switch (geometrytype)
                {
                    case RenderGeometryClassification.Worldspace:
                        builder.BindWorldVertexBuffer(worldVertices);
                        break;

                    case RenderGeometryClassification.Rigid:
                        builder.BindRigidVertexBuffer(rigidVertices, (sbyte)rigidnode);
                        break;

                    case RenderGeometryClassification.RigidBoned:
                    case RenderGeometryClassification.Skinned:
                        builder.BindSkinnedVertexBuffer(skinnedVertices);
                        break;

                    default:
                        throw new NotSupportedException(geometrytype.ToString());
                }

                builder.BindIndexBuffer(mesh.StripIndices.Select(i => (ushort)i.Index), IndexBufferFormat.TriangleList);

                builder.EndMesh();
            }
        }

        public static List<Gen2BSPResourceMesh> ReadResourceMeshes(GameCacheGen2 Gen2Cache, CacheFileResourceGen2 Resource, int vertexcount, RenderGeometryCompressionFlags CompressionFlags, TagTool.Tags.Definitions.Gen2.RenderModel.SectionLightingFlags LightingFlags, VertexCompressor compressor)
        {
            List<Gen2BSPResourceMesh> meshes = new List<Gen2BSPResourceMesh>();

            using (var stream = new MemoryStream(Gen2Cache.GetCacheRawData(Resource.BlockOffset, (int)Resource.BlockSize)))
            using (var reader = new EndianReader(stream))
            using (var writer = new EndianWriter(stream))
            {
                //fix up pointers within the resource so it deserializes properly
                foreach (var resourceinstance in Resource.TagResources)
                {
                    stream.Position = resourceinstance.FieldOffset;

                    switch (resourceinstance.Type)
                    {
                        case TagResourceTypeGen2.TagBlock:
                            //count
                            writer.Write(resourceinstance.ResourceDataSize / resourceinstance.SecondaryLocator);
                            //address
                            writer.Write(8 + Resource.SectionDataSize + resourceinstance.ResourceDataOffset);
                            break;

                        case TagResourceTypeGen2.TagData:
                            //size
                            writer.Write(resourceinstance.ResourceDataSize);
                            //address
                            writer.Write(8 + Resource.SectionDataSize + resourceinstance.ResourceDataOffset);
                            break;

                        case TagResourceTypeGen2.VertexBuffer:
                            break;
                    }
                }

                stream.Position = 0;

                var dataContext = new DataSerializationContext(reader);
                var mesh = Gen2Cache.Deserializer.Deserialize<Gen2BSPResourceMesh>(dataContext);

                if (mesh == null)
                    return new List<Gen2BSPResourceMesh>();

                meshes.Add(mesh);
                mesh.RawVertices = new List<Gen2BSPResourceMesh.RawVertex>(vertexcount);

                for (var i = 0; i < vertexcount; i++)
                    mesh.RawVertices.Add(new Gen2BSPResourceMesh.RawVertex
                    {
                        Point = new Gen2BSPResourceMesh.RawPoint
                        {
                            NodeWeights = new[] { 1.0f, 0.0f, 0.0f, 0.0f },
                            NodeIndices = new[] { 0, 0, 0, 0 },
                            UseNewNodeIndices = 1,
                            AdjustedCompoundNodeIndex = -1
                        },
                        SecondaryTexcoord = new RealPoint2d(1.0f, 1.0f)
                    });

                var currentVertexBuffer = 0;

                foreach (var resource in Resource.TagResources)
                {
                    if (resource.Type != TagResourceTypeGen2.VertexBuffer)
                        continue;

                    var vertexBuffer = mesh.VertexBuffers[currentVertexBuffer];

                    if (vertexBuffer.TypeIndex == 0)
                        continue;

                    var elementStream = new VertexElementStream(stream);

                    (string, VertexDeclarationUsage, VertexDeclarationType, int)[] declaration = Gen2Cache.Version == CacheVersion.Halo2Vista ?
                        VertexDeclarationsVista[vertexBuffer.TypeIndex] : VertexDeclarations[vertexBuffer.TypeIndex];

                    //Console.WriteLine($"Vertex type index {vertexBuffer.TypeIndex} of stride {vertexBuffer.StrideIndex} of class {section.GeometryClassification.ToString()}");

                    int calculated_size = CalculateVertexSize(declaration);
                    if (calculated_size != vertexBuffer.StrideIndex)
                        Console.WriteLine($"WARNING: vertex type {vertexBuffer.TypeIndex} of declared size {vertexBuffer.StrideIndex} didn't match defined size of {calculated_size}");

                    for (var i = 0; i < vertexcount; i++)
                    {
                        var vertex = mesh.RawVertices[i];

                        stream.Position = 8 + Resource.SectionDataSize + resource.ResourceDataOffset + ((resource.ResourceDataSize / vertexcount) * i);

                        foreach (var entry in declaration)
                        {
                            if (entry.Item3 == VertexDeclarationType.Skip)
                            {
                                elementStream.SeekTo(entry.Item4, SeekOrigin.Current);
                                continue;
                            }

                            var element = ReadVertexElement(elementStream, entry.Item3);

                            switch (resource.SecondaryLocator) // stream source
                            {
                                case 0:
                                    switch (entry.Item2)
                                    {
                                        case VertexDeclarationUsage.Position:
                                            vertex.Point.Position = element.XYZ;
                                            if (CompressionFlags.HasFlag(RenderGeometryCompressionFlags.CompressedPosition))
                                            {
                                                //halo 2 vista compression appears to be between -1 and 1. Normalize to 0 to 1 range.
                                                if (Gen2Cache.Version == CacheVersion.Halo2Vista)
                                                    vertex.Point.Position = new RealPoint3d((vertex.Point.Position.X + 1) / 2, (vertex.Point.Position.Y + 1) / 2, (vertex.Point.Position.Z + 1) / 2);
                                                vertex.Point.Position = compressor.DecompressPosition(new RealQuaternion(vertex.Point.Position.ToArray())).XYZ;
                                            }
                                            break;

                                        case VertexDeclarationUsage.BlendIndices:
                                            if (element.I != Math.Floor(element.I) ||
                                                element.J != Math.Floor(element.J) ||
                                                element.K != Math.Floor(element.K) ||
                                                element.W != Math.Floor(element.W))
                                            {
                                                new TagToolError(CommandError.OperationFailed, "Blend Index with Non Integer Value!");
                                            }
                                            vertex.Point.NodeIndices = element.ToArray().Select(x => (int)x).ToArray();
                                            //Console.WriteLine($"Boned/Skinned Vertex with blendindices {vertex.Point.NodeIndices[0]},{vertex.Point.NodeIndices[1]},{vertex.Point.NodeIndices[2]},{vertex.Point.NodeIndices[3]}");
                                            break;

                                        case VertexDeclarationUsage.BlendWeight:
                                            vertex.Point.NodeWeights = element.ToArray();
                                            break;
                                    }
                                    break;

                                case 1:
                                    if (entry.Item2 == VertexDeclarationUsage.TextureCoordinate)
                                    {
                                        vertex.Texcoord = element.XY;

                                        if (CompressionFlags.HasFlag(RenderGeometryCompressionFlags.CompressedTexcoord))
                                            vertex.Texcoord = compressor.DecompressUv(new RealVector2d(vertex.Texcoord.ToArray())).XY;
                                    }
                                    break;

                                case 2:
                                    switch (entry.Item2)
                                    {
                                        case VertexDeclarationUsage.Normal:
                                            vertex.Normal = element.IJK;
                                            break;

                                        case VertexDeclarationUsage.Binormal:
                                            vertex.Binormal = element.IJK;
                                            break;

                                        case VertexDeclarationUsage.Tangent:
                                            vertex.Tangent = element.IJK;
                                            break;
                                    }
                                    break;

                                case 3:
                                    if (LightingFlags.HasFlag(RenderModelGen2.SectionLightingFlags.HasLightmapTexcoords))
                                        vertex.PrimaryLightmapTexcoord = element.IJ;
                                    break;

                                case 4:
                                    if (LightingFlags.HasFlag(RenderModelGen2.SectionLightingFlags.HasLightmapIncRad))
                                        vertex.PrimaryLightmapIncidentDirection = element.IJK;
                                    break;

                                case 5:
                                    if (LightingFlags.HasFlag(RenderModelGen2.SectionLightingFlags.HasLightmapColors))
                                        vertex.PrimaryLightmapColor = element.RGB;
                                    break;

                                default:
                                    throw new Exception();
                            }
                        }
                    }

                    currentVertexBuffer++;
                }
            }

            return meshes;
        }
    }
}
