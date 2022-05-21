using System;
using System.Collections.Generic;
using System.Linq;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Tags.Definitions;
using RenderModelGen2 = TagTool.Tags.Definitions.Gen2.RenderModel;
using static TagTool.Commands.Porting.Gen2.Gen2RenderGeometryConverter;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
        public RenderModel ConvertRenderModel(RenderModelGen2 gen2RenderModel)
        {
            foreach (var section in gen2RenderModel.Sections)
            {
                var compressor = new VertexCompressor(
                    section.Compression.Count > 0 ?
                        section.Compression[0] :
                    gen2RenderModel.Compression.Count > 0 ?
                        gen2RenderModel.Compression[0] :
                        new RenderGeometryCompression
                        {
                            X = new Bounds<float>(0.0f, 1.0f),
                            Y = new Bounds<float>(0.0f, 1.0f),
                            Z = new Bounds<float>(0.0f, 1.0f),
                            U = new Bounds<float>(0.0f, 1.0f),
                            V = new Bounds<float>(0.0f, 1.0f),
                            U2 = new Bounds<float>(0.0f, 1.0f),
                            V2 = new Bounds<float>(0.0f, 1.0f),
                        });
                section.Meshes = ReadResourceMeshes(Gen2Cache, section.Resource, section.TotalVertexCount, section.GeometryCompressionFlags, section.LightingFlags, compressor);
            }

            //TODO: Convert prt info
            /*
            foreach (var section in gen2RenderModel.PrtInfo)
            {
                using (var stream = new MemoryStream(Gen2Cache.GetCacheRawData(section.Resource.BlockOffset, (int)section.Resource.BlockSize)))
                using (var reader = new EndianReader(stream))
                using (var writer = new EndianWriter(stream))
                {
                    foreach (var resource in section.Resource.TagResources)
                    {
                        stream.Position = resource.FieldOffset;

                        switch (resource.Type)
                        {
                            case TagResourceTypeGen2.TagBlock:
                                writer.Write(resource.ResourceDataSize / resource.SecondaryLocator);
                                writer.Write(8 + section.Resource.SectionDataSize + resource.ResourceDataOffset);
                                break;

                            case TagResourceTypeGen2.TagData:
                                writer.Write(resource.ResourceDataSize);
                                writer.Write(8 + section.Resource.SectionDataSize + resource.ResourceDataOffset);
                                break;

                            case TagResourceTypeGen2.VertexBuffer:
                                break;
                        }
                    }

                    stream.Position = 0;

                    var dataContext = new DataSerializationContext(reader);
                    var rawPcaDatum = Gen2Cache.Deserializer.Deserialize<RenderModelGen2.PrtInfoBlock.RawPcaDatum>(dataContext);

                    section.RawPcaData.Add(rawPcaDatum);

                    // TODO: prt vertex buffers
                }
            }
            */

            var builder = new RenderModelBuilder(Cache);

            foreach (var node in gen2RenderModel.Nodes)
                builder.AddNode(new RenderModel.Node 
                {
                    Name = node.Name,
                    ParentNode = node.ParentNode,
                    FirstChildNode = node.FirstChildNode,
                    NextSiblingNode = node.NextSiblingNode,
                    Flags = (RenderModel.NodeFlags)node.Flags,
                    DefaultTranslation = node.DefaultTranslation,
                    DefaultRotation = node.DefaultRotation,
                    DefaultScale = node.DefaultScale,
                    InverseForward = node.InverseForward,
                    InverseLeft = node.InverseLeft,
                    InverseUp = node.InverseUp,
                    InversePosition = node.InversePosition,
                    DistanceFromParent = node.DistanceFromParent
                });

            foreach (var material in gen2RenderModel.Materials)
                builder.AddMaterial(new RenderMaterial { RenderMethod = material.RenderMethod == null ? Cache.TagCache.GetTag(@"shaders\invalid.shader") : material.RenderMethod });

            foreach (var region in gen2RenderModel.Regions)
            {
                builder.BeginRegion(region.Name);

                foreach (var permutation in region.Permutations)
                {
                    builder.BeginPermutation(permutation.Name);

                    var sectionIndex = permutation.LodSectionIndices.Where(i => i >= 0).Last();

                    if (sectionIndex < 0)
                    {
                        builder.EndPermutation();
                        continue;
                    }

                    var section = gen2RenderModel.Sections[sectionIndex];
                    BuildMeshes(builder, section.Meshes, section.GeometryClassification, section.OpaqueMaxNodesVertex, section.RigidNode);
                    
                    builder.EndPermutation();
                }
                builder.EndRegion();
            }

            RenderModel result = builder.Build(Cache.Serializer);

            result.MarkerGroups = new List<RenderModel.MarkerGroup>();
            //add marker groups
            foreach(var gen2markergroup in gen2RenderModel.MarkerGroups)
            {
                var tempmarker = new RenderModel.MarkerGroup
                {
                    Name = gen2markergroup.Name,
                    Markers = new List<RenderModel.MarkerGroup.Marker>()
                };
                foreach(var gen2marker in gen2markergroup.Markers)
                {
                    tempmarker.Markers.Add(new RenderModel.MarkerGroup.Marker
                    {
                        RegionIndex = gen2marker.RegionIndex,
                        PermutationIndex = gen2marker.PermutationIndex,
                        NodeIndex = gen2marker.NodeIndex,
                        Translation = gen2marker.Translation,
                        Rotation = gen2marker.Rotation,
                        Scale = gen2marker.Scale
                    });
                }
                result.MarkerGroups.Add(tempmarker);
            }

            foreach (var mesh in result.Geometry.Meshes)
                if (mesh.Type == VertexType.Skinned)
                    mesh.RigidNodeIndex = -1;

            return result;
        }
    }
}
