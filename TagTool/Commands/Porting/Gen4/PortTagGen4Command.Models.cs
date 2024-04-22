using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderModelGen4 = TagTool.Tags.Definitions.Gen4.RenderModel;
using ScenarioLightmapBspGen4 = TagTool.Tags.Definitions.Gen4.ScenarioLightmapBspData;
using TagTool.Tags.Definitions;
using TagTool.Cache;
using TagTool.Geometry;
using TagTool.Common;
using static TagTool.Tags.Definitions.Gen4.StructureDesign.GlobalRenderGeometryStruct.GlobalMeshBlock;
using TagTool.Cache.Resources;
using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Commands.Porting.Gen4
{
    public static class RenderModelConverter
    {
        public static RenderModel Convert(GameCache Cache, RenderModelGen4 gen4RenderModel)
        {
            RenderModel result = new RenderModel();

            //convert regions
            result.Regions = new List<RenderModel.Region>();
            foreach(var reg in gen4RenderModel.Regions)
            {
                RenderModel.Region newRegion = new RenderModel.Region
                {
                    Name = reg.Name,
                    Permutations = new List<RenderModel.Region.Permutation>()
                };
                foreach(var perm in reg.Permutations)
                {
                    newRegion.Permutations.Add(new RenderModel.Region.Permutation
                    {
                        Name = perm.Name,
                        MeshCount = (ushort)perm.MeshCount,
                        MeshIndex = perm.MeshIndex
                    });
                }
                result.Regions.Add(newRegion);
            }

            //convert materials
            result.Materials = new List<RenderMaterial>();
            foreach(var mat in gen4RenderModel.Materials)
            {
                result.Materials.Add(new RenderMaterial
                {
                    RenderMethod = mat.RenderMethod
                });
            }

            //convert markers
            result.MarkerGroups = new List<RenderModel.MarkerGroup>();
            foreach(var mark in gen4RenderModel.MarkerGroups)
            {
                var newMark = new RenderModel.MarkerGroup
                {
                    Name = mark.Name,
                    Markers = new List<RenderModel.MarkerGroup.Marker>()
                };
                foreach(var marker in mark.Markers)
                {
                    newMark.Markers.Add(new RenderModel.MarkerGroup.Marker
                    {
                        NodeIndex = (sbyte)marker.NodeIndex,
                        RegionIndex = marker.RegionIndex,
                        PermutationIndex = marker.PermutationIndex,
                        Translation = marker.Translation,
                        Rotation = marker.Rotation,
                        Scale = marker.Scale,
                    });
                }
                result.MarkerGroups.Add(newMark);
            }

            result.Geometry = ConvertGeometry(gen4RenderModel.RenderGeometry);
            result.LightgenLights = new List<RenderModel.SkygenLight>();

            return result;
        }

        public static RenderGeometry ConvertGeometry(RenderModelGen4.GlobalRenderGeometryStruct gen4Geometry)
        {
            RenderGeometry newGeo = new RenderGeometry();

            //compression
            newGeo.Compression = new List<RenderGeometryCompression>();
            foreach(var compress in gen4Geometry.CompressionInfo)
            {
                newGeo.Compression.Add(new RenderGeometryCompression
                {
                    Flags = (RenderGeometryCompressionFlags)compress.CompressionFlags1,
                    X = compress.X,
                    Y = compress.Y,
                    Z = compress.Z,
                    U = compress.U,
                    V = compress.V,
                });
            }

            //meshes
            newGeo.Meshes = new List<Mesh>();
            foreach(var mesh in gen4Geometry.Meshes)
            {
                var newMesh = new Mesh
                {
                    Parts = new List<Part>(),
                    SubParts = new List<SubPart>(),
                    IndexBufferIndices = new short[2] {mesh.IndexBufferIndex,-1},
                    VertexBufferIndices = ConvertVertexBufferIndices(mesh.VertexBufferIndices),
                    RigidNodeIndex = mesh.RigidNodeIndex,
                    IndexBufferType = (PrimitiveType)mesh.IndexBufferType,
                    ReachType = (VertexTypeReach)mesh.VertexType
                };

                //parts
                foreach(var part in mesh.Parts)
                {
                    newMesh.Parts.Add(new Part
                    {
                        MaterialIndex = part.RenderMethodIndex,
                        TransparentSortingIndex = part.TransparentSortingIndex,
                        FirstIndex = part.IndexStart,
                        IndexCount = part.IndexCount,
                        FirstSubPartIndex = part.SubpartStart,
                        SubPartCount = part.SubpartCount,
                        TypeNew = (Part.PartTypeNew)part.PartType,
                        VertexCount = part.BudgetVertexCount
                    });
                }

                //subparts
                foreach(var subpart in mesh.Subparts)
                {
                    newMesh.SubParts.Add(new SubPart
                    {
                        FirstIndex = subpart.IndexStart,
                        IndexCount = subpart.IndexCount,
                        PartIndex = subpart.PartIndex,
                        VertexCount = subpart.BudgetVertexCount
                    });
                }
                newGeo.Meshes.Add(newMesh);
            }

            newGeo.InstancedGeometryPerPixelLighting = new List<RenderGeometry.StaticPerPixelLighting>();

            return newGeo;
        }

        public static RenderGeometry ConvertGeometry(ScenarioLightmapBspGen4.GlobalRenderGeometryStruct gen4Geometry)
        {
            RenderGeometry newGeo = new RenderGeometry();

            //compression
            newGeo.Compression = new List<RenderGeometryCompression>();
            foreach (var compress in gen4Geometry.CompressionInfo)
            {
                newGeo.Compression.Add(new RenderGeometryCompression
                {
                    Flags = (RenderGeometryCompressionFlags)compress.CompressionFlags1,
                    X = compress.X,
                    Y = compress.Y,
                    Z = compress.Z,
                    U = compress.U,
                    V = compress.V,
                });
            }

            //meshes
            newGeo.Meshes = new List<Mesh>();
            foreach (var mesh in gen4Geometry.Meshes)
            {
                var newMesh = new Mesh
                {
                    Parts = new List<Part>(),
                    SubParts = new List<SubPart>(),
                    IndexBufferIndices = new short[2] { mesh.IndexBufferIndex, -1 },
                    VertexBufferIndices = ConvertVertexBufferIndices(mesh.VertexBufferIndices),
                    RigidNodeIndex = mesh.RigidNodeIndex,
                    IndexBufferType = (PrimitiveType)mesh.IndexBufferType,
                    ReachType = (VertexTypeReach)mesh.VertexType
                };

                //parts
                foreach (var part in mesh.Parts)
                {
                    newMesh.Parts.Add(new Part
                    {
                        MaterialIndex = part.RenderMethodIndex,
                        TransparentSortingIndex = part.TransparentSortingIndex,
                        FirstIndex = part.IndexStart,
                        IndexCount = part.IndexCount,
                        FirstSubPartIndex = part.SubpartStart,
                        SubPartCount = part.SubpartCount,
                        TypeNew = (Part.PartTypeNew)part.PartType,
                        VertexCount = part.BudgetVertexCount
                    });
                }

                //subparts
                foreach (var subpart in mesh.Subparts)
                {
                    newMesh.SubParts.Add(new SubPart
                    {
                        FirstIndex = subpart.IndexStart,
                        IndexCount = subpart.IndexCount,
                        PartIndex = subpart.PartIndex,
                        VertexCount = subpart.BudgetVertexCount
                    });
                }
                newGeo.Meshes.Add(newMesh);
            }

            newGeo.InstancedGeometryPerPixelLighting = new List<RenderGeometry.StaticPerPixelLighting>();

            return newGeo;
        }

        public static RenderGeometryApiResourceDefinition ConvertResource(TagTool.Tags.Resources.Gen4.RenderGeometryApiResourceDefinition gen4Resource)
        {
            RenderGeometryApiResourceDefinition newResource = new RenderGeometryApiResourceDefinition
            {
                VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>(),
                IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>()
            };
            foreach (var buffer in gen4Resource.XenonVertexBuffers)
            {
                newResource.VertexBuffers.Add(new D3DStructure<VertexBufferDefinition> { Definition = buffer.Definition });
            }
            foreach (var buffer in gen4Resource.XenonIndexBuffers)
            {
                newResource.IndexBuffers.Add(new D3DStructure<IndexBufferDefinition> { Definition = buffer.Definition });
            }
            return newResource;
        }

        private static short[] ConvertVertexBufferIndices(RenderModelGen4.GlobalRenderGeometryStruct.GlobalMeshBlock.VertexBufferIndicesWordArray[] indicesArray)
        {
            List<short> VertexBufferIndices = new List<short>();
            for (var i = 0; i < 8; i++)
                VertexBufferIndices.Add((short)indicesArray[i].VertexBufferIndex);
            return VertexBufferIndices.ToArray();
        }

        private static short[] ConvertVertexBufferIndices(ScenarioLightmapBspGen4.GlobalRenderGeometryStruct.GlobalMeshBlock.VertexBufferIndicesWordArray[] indicesArray)
        {
            List<short> VertexBufferIndices = new List<short>();
            for (var i = 0; i < 8; i++)
                VertexBufferIndices.Add((short)indicesArray[i].VertexBufferIndex);
            return VertexBufferIndices.ToArray();
        }
    }
}
