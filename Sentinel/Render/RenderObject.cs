using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using Sentinel.Render.VertexDefinitions;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using D3DDevice = Microsoft.DirectX.Direct3D.Device;
using D3DPrimitiveType = Microsoft.DirectX.Direct3D.PrimitiveType;
using TagPrimitiveType = TagTool.Geometry.PrimitiveType;

namespace Sentinel.Render
{
    public class RenderObject
    {
        public D3DDevice Device { get; }
        public GameCache Cache { get; }
        public GameObject Object { get; }
        public Model Model { get; }
        public RenderModel RenderModel { get; }
        public RenderGeometryApiResourceDefinition RenderGeometryResource { get; }
        public List<RenderMaterial> Materials { get; }
        public Dictionary<int, VertexBuffer> VertexBuffers { get; }
        public Dictionary<int, IndexBuffer> IndexBuffers { get; }

        public RealPoint3d Position;
        public RealEulerAngles3d Rotation;

        public Matrix World;

        public RenderObject(D3DDevice device, GameCache cache, GameObject definition, RealPoint3d position, RealEulerAngles3d rotation)
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));
            else if (cache == null)
                throw new ArgumentNullException(nameof(cache));
            else if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            Device = device;
            Cache = cache;
            Object = definition;

            Position = position;
            Rotation = rotation;
            UpdateTransform();

            using (var cacheStream = Cache.OpenCacheRead())
            {
                if (Object.Model == null)
                    throw new NullReferenceException(nameof(Object.Model));

                Model = Cache.Deserialize<Model>(cacheStream, Object.Model);

                if (Model.RenderModel == null)
                    throw new NullReferenceException(nameof(Model.RenderModel));

                RenderModel = Cache.Deserialize<RenderModel>(cacheStream, Model.RenderModel);

                if (Model.Variants == null || Model.Variants.Count == 0)
                {
                    var modelVariant = new Model.Variant
                    {
                        Name = Cache.StringTable.GetStringId("default"),
                        Regions = new List<Model.Variant.Region>()
                    };

                    foreach (var region in RenderModel.Regions)
                    {
                        var modelRegion = new Model.Variant.Region
                        {
                            Name = region.Name,
                            RenderModelRegionIndex = (sbyte)RenderModel.Regions.IndexOf(region),
                            Permutations = new List<Model.Variant.Region.Permutation>()
                        };

                        foreach (var permutation in region.Permutations)
                            modelRegion.Permutations.Add(new Model.Variant.Region.Permutation
                            {
                                Name = modelVariant.Name,
                                RenderModelPermutationIndex = (sbyte)region.Permutations.IndexOf(permutation)
                            });

                        modelVariant.Regions.Add(modelRegion);
                    }

                    Model.Variants = new List<Model.Variant> { modelVariant };
                }

                Materials = new List<RenderMaterial>();

                foreach (var material in RenderModel.Materials)
                    Materials.Add(new RenderMaterial(device, Cache, material));

                if (RenderModel.Geometry.Resource == null)
                    throw new NullReferenceException(nameof(RenderModel.Geometry.Resource));

                RenderGeometryResource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(RenderModel.Geometry.Resource);

                VertexBuffers = new Dictionary<int, VertexBuffer>();
                IndexBuffers = new Dictionary<int, IndexBuffer>();

                var compression = RenderModel.Geometry.Compression[0];

                foreach (var mesh in RenderModel.Geometry.Meshes)
                {
                    var renderVertex = VertexDefinition.Get(mesh.Type);
                    var streamTypes = renderVertex.GetStreamTypes();

                    foreach (var streamEntry in streamTypes)
                    {
                        var vertexBufferIndex = mesh.VertexBufferIndices[streamEntry.Key];

                        if (vertexBufferIndex == -1 || VertexBuffers.ContainsKey(vertexBufferIndex))
                            continue;

                        var vbDef = RenderGeometryResource.VertexBuffers[vertexBufferIndex].Definition;

                        var vb = new VertexBuffer(streamEntry.Value, vbDef.Data.Data.Length, device, Usage.DoNotClip, renderVertex.GetStreamFormat(streamEntry.Key), Pool.Managed);
                        var vbData = vb.Lock(0, vbDef.Data.Data.Length, LockFlags.None);

                        var vertices = Array.CreateInstance(streamEntry.Value, vbDef.Count);

                        using (var vbDefData = new MemoryStream(vbDef.Data.Data))
                        using (var vbDefReader = new BinaryReader(vbDefData))
                        {
                            for (var i = 0; i < vbDef.Count; i++)
                            {
                                var handle = GCHandle.Alloc(vbDefReader.ReadBytes(Marshal.SizeOf(streamEntry.Value)), GCHandleType.Pinned);
                                var vertex = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), streamEntry.Value);

                                var positionField = streamEntry.Value.GetField("Position");

                                if (positionField != null)
                                {
                                    var xyz = (Vector3)positionField.GetValue(vertex);

                                    positionField.SetValue(vertex, new Vector3(
                                        xyz.X * compression.X.Length + compression.X.Lower,
                                        xyz.Y * compression.Y.Length + compression.Y.Lower,
                                        xyz.Z * compression.Z.Length + compression.Z.Lower));
                                }

                                var texcoordField = streamEntry.Value.GetField("Texcoord");

                                if (texcoordField != null)
                                {
                                    var uv = (Vector2)texcoordField.GetValue(vertex);

                                    texcoordField.SetValue(vertex, new Vector2(
                                        uv.X * compression.U.Length + compression.U.Lower,
                                        uv.Y * compression.V.Length + compression.V.Lower));
                                }

                                vertices.SetValue(vertex, i);

                                handle.Free();
                            }
                        }

                        vbData.Write(vertices);
                        vb.Unlock();

                        VertexBuffers[vertexBufferIndex] = vb;
                    }

                    foreach (var indexBufferIndex in mesh.IndexBufferIndices)
                    {
                        if (indexBufferIndex == -1/*ushort.MaxValue*/ || IndexBuffers.ContainsKey(indexBufferIndex))
                            continue;

                        var ibDef = RenderGeometryResource.IndexBuffers[indexBufferIndex].Definition;

                        switch (ibDef.Format)
                        {
                            case IndexBufferFormat.PointList:
                                mesh.IndexBufferType = TagPrimitiveType.PointList;
                                break;

                            case IndexBufferFormat.LineList:
                                mesh.IndexBufferType = TagPrimitiveType.LineList;
                                break;

                            case IndexBufferFormat.LineStrip:
                                mesh.IndexBufferType = TagPrimitiveType.LineStrip;
                                break;

                            case IndexBufferFormat.TriangleList:
                                mesh.IndexBufferType = TagPrimitiveType.TriangleList;
                                break;

                            case IndexBufferFormat.TriangleFan:
                                mesh.IndexBufferType = TagPrimitiveType.TriangleFan;
                                break;

                            case IndexBufferFormat.TriangleStrip:
                                mesh.IndexBufferType = TagPrimitiveType.TriangleStrip;
                                break;
                        }

                        var ib = new IndexBuffer(device, ibDef.Data.Data.Length, Usage.DoNotClip, Pool.Managed, true);
                        var ibData = ib.Lock(0, ibDef.Data.Data.Length, LockFlags.None);

                        /*resourceStream.Position = ibDef.Data.Address.Offset;

                        var indices = new ushort[ibDef.Data.Data.Length / 2];

                        for (var i = 0; i < ibDef.Data.Data.Length / 2; i++)
                            indices[i] = reader.ReadUInt16();*/

                        // may not work, hard to test (original code above)
                        short[] indices = new short[(int)Math.Ceiling(ibDef.Data.Data.Length / 2.0)];
                        Buffer.BlockCopy(ibDef.Data.Data, 0, indices, 0, ibDef.Data.Data.Length);

                        ibData.Write(indices);
                        ib.Unlock();

                        IndexBuffers[indexBufferIndex] = ib;
                    }
                }
            }
        }

        private void UpdateTransform()
        {
            var pitchRotation = Matrix.Identity;
            pitchRotation.RotateX(Rotation.Pitch.Radians);

            var rollRotation = Matrix.Identity;
            rollRotation.RotateY(-Rotation.Roll.Radians);

            var yawRotation = Matrix.Identity;
            yawRotation.RotateZ(Rotation.Yaw.Radians);

            var transform = new Matrix();
            transform.Translate(Position.X, Position.Y, Position.Z);

            World = Matrix.Identity;
            World.Multiply(yawRotation);
            World.Multiply(rollRotation);
            World.Multiply(pitchRotation);
            World.Multiply(transform);
        }

        public void Render()
        {
            var variantName = Object.DefaultModelVariant != StringId.Invalid ?
                Cache.StringTable.GetString(Object.DefaultModelVariant) :
                "default";

            var modelVariant = Model.Variants.FirstOrDefault(v => (Cache.StringTable.GetString(v.Name) ?? v.Name.ToString()) == variantName);
            if (modelVariant == null && Model.Variants.Count > 0)
                modelVariant = Model.Variants.First();

            UpdateTransform();
            Device.Transform.World = World;

            foreach (var region in modelVariant.Regions)
            {
                if (region.RenderModelRegionIndex >= RenderModel.Regions.Count)
                    continue;

                var renderModelRegion = RenderModel.Regions[region.RenderModelRegionIndex];

                if (region.Permutations.Count == 0)
                    continue;

                var permutation = region.Permutations[0];

                if (permutation.RenderModelPermutationIndex < 0 || permutation.RenderModelPermutationIndex >= renderModelRegion.Permutations.Count)
                    continue;

                var renderModelPermutation = renderModelRegion.Permutations[permutation.RenderModelPermutationIndex];

                var meshIndex = renderModelPermutation.MeshIndex;
                var meshCount = renderModelPermutation.MeshCount;
                var regionName = Cache.StringTable.GetString(region.Name) ?? region.Name.ToString();
                var permutationName = Cache.StringTable.GetString(permutation.Name) ?? permutation.Name.ToString();

                for (var currentMeshIndex = 0; currentMeshIndex < meshCount; currentMeshIndex++)
                {
                    var mesh = RenderModel.Geometry.Meshes[meshIndex + currentMeshIndex];

                    var renderVertex = VertexDefinition.Get(mesh.Type);
                    var streamTypes = renderVertex.GetStreamTypes();
                    var streamIndex = streamTypes.First().Key;
                    var primitiveType = D3DPrimitiveType.TriangleList;

                    switch (mesh.IndexBufferType)
                    {
                        case TagPrimitiveType.PointList:
                            primitiveType = D3DPrimitiveType.PointList;
                            break;

                        case TagPrimitiveType.LineList:
                            primitiveType = D3DPrimitiveType.LineList;
                            break;

                        case TagPrimitiveType.LineStrip:
                            primitiveType = D3DPrimitiveType.LineStrip;
                            break;

                        case TagPrimitiveType.TriangleList:
                            primitiveType = D3DPrimitiveType.TriangleList;
                            break;

                        case TagPrimitiveType.TriangleFan:
                            primitiveType = D3DPrimitiveType.TriangleFan;
                            break;

                        case TagPrimitiveType.TriangleStrip:
                            primitiveType = D3DPrimitiveType.TriangleStrip;
                            break;
                    }

                    Device.VertexDeclaration = renderVertex.GetDeclaration(Device);
                    Device.SetStreamSource(streamTypes.First().Key, VertexBuffers[mesh.VertexBufferIndices[streamIndex]], 0);
                    Device.Indices = IndexBuffers[mesh.IndexBufferIndices.Where(index => index != -1).First()];

                    foreach (var part in mesh.Parts)
                    {
                        if (part.MaterialIndex != -1)
                        {
                            var material = Materials[part.MaterialIndex];

                            Device.RenderState.AlphaBlendEnable = (part.TypeNew == TagTool.Geometry.Mesh.Part.PartTypeNew.Transparent);

                            foreach (var entry in material.Textures)
                            {
                                var name = entry.Key;

                                if (name.StartsWith("diffuse_map") || name.StartsWith("base_map") || name == "foam_texture")
                                {
                                    Device.SetTexture(0, entry.Value.Texture);
                                    break;
                                }
                            }

                            Device.TextureState[0].ColorOperation = TextureOperation.Modulate;
                            Device.TextureState[0].ColorArgument1 = TextureArgument.TextureColor;
                            Device.TextureState[0].ColorArgument2 = TextureArgument.Current;
                            Device.RenderState.FillMode = FillMode.Solid;
                        }

                        for (var currentSubPartIndex = 0; currentSubPartIndex < part.SubPartCount; currentSubPartIndex++)
                        {
                            var subPart = mesh.SubParts[part.FirstSubPartIndex + currentSubPartIndex];

                            Device.DrawIndexedPrimitives(primitiveType, 0, 0, subPart.VertexCount, subPart.FirstIndex, subPart.IndexCount - (primitiveType == D3DPrimitiveType.TriangleStrip ? 2 : 0));
                        }
                    }
                }
            }
        }
    }
}