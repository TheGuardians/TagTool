using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using static TagTool.Commands.Porting.PortTagCommand.VertexDeclarationUsage;
using static TagTool.Commands.Porting.PortTagCommand.VertexDeclarationType;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public enum VertexDeclarationUsage : sbyte
        {
            None = -1,
            Position,
            NodeIndices,
            NodeWeights,
            TexCoord,
            Normal,
            Binormal,
            Tangent,
            AnisoBinormal,
            IncidentRadiosity,
            SecondaryTexCoord,
            SecondaryPosition,
            SecondaryNodeIndices,
            SecondaryNodeWeights,
            SecondaryIsqSelect,
            Color,
            TintFactor,
            DsqPlane,
            BillboardOffset,
            BillboardAxis,
            PcaClusterId,
            PcaVertexWeights
        }

        public enum VertexDeclarationType : sbyte
        {
            Skip = -2,
            Unused = -1,
            Float1 = 0,
            Float2,
            Float3,
            Float4,
            UByte4,
            UByte4N,
            RGBA,
            UShort2,
            UShort4,
            UShort2N,
            UShort4N,
            UShort2N_2,
            UShort4N_2,
            UDHen3N,
            DHen3N,
            Half2,
            Half4,
            Dec3N,
            UHenD3N,
            UDec4N,
            Byte4N,
            UByteN,
            UByte2N,
            UByte3N,
            UShortN,
            UShort3N,
            ShortN,
            Short2N,
            Short3N,
            Short4N,
            HenD3N
        }

        private (VertexDeclarationUsage, VertexDeclarationType)[][] VertexDeclarations = new[]
        {
            new[] // 0x01 model_rigid::uncompressed
			{
                (Position, Float3),
            },
            new[] // 0x02 model_rigid::compressed
			{
                (Position, Short3N),
            },
            new[] // 0x03 model_rigid_boned1::uncompressed
			{
                (Position, Float3),
                (NodeIndices, UByteN),
            },
            new[] // 0x04 model_rigid_boned1::compressed
			{
                (Position, Short3N),
                (NodeIndices, UByteN),
                (None, Skip),
            },
            new[] // 0x05 model_skinned2::uncompressed
			{
                (Position, Float3),
                (NodeIndices, UByte2N),
                (NodeWeights, UByte2N),
            },
            new[] // 0x06
			{
                (Position, Short3N),
            },
            new[] // 0x07 model_rigid_boned3::uncompressed
			{
                (Position, Float3),
                (NodeIndices, UByte3N),
            },
            new[] // 0x08 model_skinned3::compressed
			{
                (Position, Short3N),
                (NodeIndices, UByte3N),
                (NodeWeights, UByte3N),
            },
            new[] // 0x09 model_skinned4::uncompressed
			{
                (Position, Float3),
                (NodeIndices, UByte4N),
                (NodeWeights, UByte4N),
            },
            new[] // 0x0A
			{
                (Position, Short3N),
            },
            new[] // 0x0B
			{
                (NodeIndices, UByteN),
            },
            new[] // 0x0C
			{
                (NodeIndices, UByte2N),
                (NodeWeights, UByte2N),
            },
            new[] // 0x0D
			{
                (NodeIndices, UByte3N),
                (NodeWeights, UByte3N),
            },
            new[] // 0x0E
			{
                (NodeIndices, UByte4N),
                (NodeWeights, UByte4N),
            },
            new[] // 0x0F
			{
                (SecondaryPosition, Float3),
            },
            new[] // 0x10
			{
                (SecondaryPosition, Short3N),
            },
            new[] // 0x11
			{
                (SecondaryPosition, Float3),
                (SecondaryNodeIndices, UByteN),
            },
            new[] // 0x12
			{
                (SecondaryPosition, Short3N),
                (SecondaryNodeIndices, UByteN),
                (None, Skip),
            },
            new[] // 0x13
			{
                (SecondaryIsqSelect, UByteN),
            },
            new[] // 0x14
			{
                (Position, Float3),
            },
            new[] // 0x15
			{
                (Position, Short3N),
            },
            new[] // 0x16
			{
                (Position, Float3),
                (NodeIndices, UByteN),
            },
            new[] // 0x17
			{
                (Position, Short3N),
                (NodeIndices, UByteN),
                (None, Skip),
            },
            new[] // 0x18
			{
                (TexCoord, Float2),
            },
            new[] // 0x19
			{
                (TexCoord, Short2N),
            },
            new[] // 0x1A
			{
                (Normal, Float3),
                (Binormal, Float3),
                (Tangent, Float3),
            },
            new[] // 0x1B
			{
                (Normal, HenD3N),
                (Binormal, HenD3N),
                (Tangent, HenD3N),
            },
            new[] // 0x1C
			{
                (AnisoBinormal, Float3),
            },
            new[] // 0x1D
			{
                (AnisoBinormal, HenD3N),
            },
            new[] // 0x1E
			{
                (SecondaryTexCoord, Float2),
            },
            new[] // 0x1F
			{
                (SecondaryTexCoord, Short2N),
            },
            new[] // 0x20
			{
                (TexCoord, Float2),
                (Normal, HenD3N),
            },
            new[] // 0x21
			{
                (TexCoord, Short2N),
                (Normal, HenD3N),
            },
            new[] // 0x22
			{
                (TexCoord, Float2),
                (Normal, HenD3N),
                (Binormal, HenD3N),
                (Tangent, HenD3N),
            },
            new[] // 0x23
			{
                (TexCoord, Short2N),
                (Normal, HenD3N),
                (Binormal, HenD3N),
                (Tangent, HenD3N),
            },
            new[] // 0x24
			{
                (Position, Float2),
                (TexCoord, Float2),
                (Color, RGBA),
            },
            new[] // 0x25
			{
                (Position, Float4),
                (TexCoord, Float2),
                (Color, RGBA),
            },
            new[] // 0x26
			{
                (Position, Float4),
                (NodeIndices, Float2),
                (NodeWeights, Float2),
                (TexCoord, Float2),
                (Normal, Float2),
            },
            new[] // 0x27
			{
                (Position, Float3),
                (TexCoord, Short2N),
                (Color, RGBA),
            },
            new[] // 0x28
			{
                (Position, Float3),
                (TexCoord, Float2),
                (Color, RGBA),
            },
            new[] // 0x29
			{
                (Position, Float3),
                (TexCoord, Float3),
                (Color, RGBA),
            },
            new[] // 0x2A dynamic_vertex
			{
                (Position, Float4),
                (NodeIndices, Float3),
                (NodeWeights, Float1),
                (TexCoord, Float4),
                (Normal, Float3),
                (Binormal, Float2),
                (Tangent, Float4),
                (AnisoBinormal, Float4),
                (SecondaryTexCoord, RGBA),
            },
            new[] // 0x2B
			{
                (Position, UShort4N),
            },
            new[] // 0x2C
			{
                (Position, Float3),
                (Color, RGBA),
            },
            new[] // 0x2D
			{
                (Position, Float3),
                (TexCoord, Short2N),
            },
            new[] // 0x2E lightmap_bucket_vertex.color::uncompressed
			{
                (Color, RGBA),
            },
            new[] // 0x2F lightmap_bucket_vertex.color::compressed
			{
                (Color, UByte3N),
            },
            new[] // 0x30 lightmap_bucket_vertex.incident_direction
			{
                (IncidentRadiosity, HenD3N),
            },
            new[] // 0x31
			{
                (Position, Float4),
                (TexCoord, Float2),
                (Color, RGBA),
                (TintFactor, Float2),
            },
            new[] // 0x32 s_decorator_model_vertex::uncompressed
			{
                (Position, Float3),
                (Normal, Float3),
                (Tangent, Float3),
                (Binormal, Float3),
                (TexCoord, Float2),
            },
            new[] // 0x33 s_decorator_model_vertex::compressed
			{
                (Position, Float3),
                (Normal, HenD3N),
                (Tangent, HenD3N),
                (Binormal, HenD3N),
                (TexCoord, Float2),
            },
            new[] // 0x34 rasterizer_vertex_decorator_decal
			{
                (Position, Float3),
                (TexCoord, Float2),
                (SecondaryTexCoord, Float2),
                (Color, RGBA),
            },
            new[] // 0x35
			{
                (Position, Float3),
                (TexCoord, Short2N),
                (Color, RGBA),
            },
            new[] // 0x36 rasterizer_vertex_decorator_sprite::uncompressed
			{
                (Position, Float3),
                (BillboardOffset, Float3),
                (BillboardAxis, Float3),
                (TexCoord, Float2),
                (Color, RGBA),
            },
            new[] // 0x37 rasterizer_vertex_decorator_sprite::compressed
			{
                (Position, Float3),
                (BillboardOffset, HenD3N),
                (BillboardAxis, HenD3N),
                (TexCoord, Short2N),
                (Color, RGBA),
            },
            new[] // 0x38
			{
                (PcaClusterId, Float1),
                (PcaVertexWeights, Float4),
            },
            new[] // 0x39
			{
                (PcaClusterId, UShortN),
                (PcaVertexWeights, Short4N),
            },
            new[] // 0x3A
			{
                (Position, Float2),
                (NodeIndices, Float2),
                (Binormal, RGBA),
            },
            new[] // 0x3B s_particle_model_vertex::uncompressed
			{
                (Position, Float3),
                (Normal, Float3),
                (Tangent, Float3),
                (Binormal, Float3),
                (TexCoord, Float2),
            },
            new[] // 0x3C s_particle_model_vertex::uncompressed2
			{
                (Position, Float3),
                (Normal, Float3),
                (Tangent, Float3),
                (Binormal, Float3),
                (TexCoord, Float2),
            },
            new[] // 0x3D s_particle_model_vertex::compressed?
			{
                (Position, Float3),
                (Normal, HenD3N),
                (Tangent, HenD3N),
                (Binormal, HenD3N),
                (TexCoord, Float2),
            },
            new[] // 0x3E
			{
                (Position, Float4),
                (TexCoord, Float2),
            },
        };

        private RealQuaternion ReadVertexElement(VertexElementStream stream, VertexDeclarationType type)
        {
            switch (type)
            {
                case Float1:
                    return new RealQuaternion(stream.ReadFloat1());
                case Float2:
                    return new RealQuaternion(stream.ReadFloat2().ToArray());
                case Float3:
                    return new RealQuaternion(stream.ReadFloat3().ToArray());
                case Float4:
                    return new RealQuaternion(stream.ReadFloat4().ToArray());
                case UByte4:
                    return new RealQuaternion(stream.ReadUByte4().Select(i => (float)i));
                case UByte4N:
                    return stream.ReadUByte4N();
                case RGBA:
                    return new RealQuaternion(BitConverter.GetBytes(stream.ReadColor()).Select(i => (float)i));
                case UShort2:
                    return new RealQuaternion(stream.ReadUShort2().ToArray());
                case UShort4:
                    return stream.ReadUShort4();
                case UShort2N:
                    return new RealQuaternion(stream.ReadUShort2N().ToArray());
                case UShort4N:
                    return stream.ReadUShort4N();
                case DHen3N:
                    return new RealQuaternion(stream.ReadDHen3N().ToArray());
                case Half2:
                    return new RealQuaternion(stream.ReadFloat16_2().ToArray());
                case Half4:
                    return stream.ReadFloat16_4();
                case Dec3N:
                    return new RealQuaternion(stream.ReadDec3N().ToArray());
                case Byte4N:
                    return stream.ReadSByte4N();
                case UByteN:
                    return new RealQuaternion(stream.ReadUByteN());
                case UByte2N:
                    return stream.ReadUByte2N();
                case UByte3N:
                    return stream.ReadUByte3N();
                case UShortN:
                    return new RealQuaternion(stream.ReadUShortN());
                case UShort3N:
                    return new RealQuaternion(stream.ReadUShort3N().ToArray());
                case ShortN:
                    return new RealQuaternion(stream.ReadShortN());
                case Short2N:
                    return new RealQuaternion(stream.ReadShort2N().ToArray());
                case Short3N:
                    return new RealQuaternion(stream.ReadShort3N().ToArray());
                case Short4N:
                    return stream.ReadShort4N();
                case HenD3N:
                    return RealQuaternion.FromHenDN3(stream.ReadColor());
                default:
                    throw new NotSupportedException(type.ToString());
            }
        }

        private object ConvertGen2RenderModel(RenderModel mode)
        {
            mode.Geometry = new RenderGeometry
            {
                Meshes = new List<Mesh>()
            };

            foreach (var section in mode.Sections)
            {
                using (var stream = new MemoryStream(BlamCache.GetRawFromID(section.BlockOffset, section.BlockSize)))
                using (var reader = new EndianReader(stream, BlamCache.Reader.Format))
                using (var writer = new EndianWriter(stream, BlamCache.Reader.Format))
                {
                    foreach (var resource in section.Resources)
                    {
                        stream.Position = resource.FieldOffset;

                        switch (resource.Type)
                        {
                            case ResourceTypeGen2.TagBlock:
                                writer.Write(resource.ResoureDataSize / resource.SecondaryLocator);
                                writer.Write(8 + section.SectionDataSize + resource.ResourceDataOffset);
                                break;

                            case ResourceTypeGen2.TagData:
                                writer.Write(resource.ResoureDataSize);
                                writer.Write(8 + section.SectionDataSize + resource.ResourceDataOffset);
                                break;

                            case ResourceTypeGen2.VertexBuffer:
                                break;
                        }
                    }
                    
                    stream.Position = 0;

                    var dataContext = new DataSerializationContext(reader);
                    var mesh = BlamCache.Deserializer.Deserialize<Mesh>(dataContext);

                    mode.Geometry.Meshes.Add(mesh);

                    if (mesh.RawVertices.Count > 0)
                        continue;

                    mesh.RawVertices = new List<Mesh.RawVertex>(section.TotalVertexCount);

                    var currentVertexBuffer = 0;

                    foreach (var resource in section.Resources)
                    {
                        if (resource.Type != ResourceTypeGen2.VertexBuffer)
                            continue;
                        
                        var vertexBuffer = mesh.VertexBuffers[currentVertexBuffer];

                        if (vertexBuffer.TypeIndex == 0)
                            continue;

                        var elementStream = new VertexElementStream(stream, BlamCache.Reader.Format);
                        var declaration = VertexDeclarations[vertexBuffer.TypeIndex - 1];

                        stream.Position = 8 + section.SectionDataSize + resource.ResourceDataOffset;

                        for (var i = 0; i < section.TotalVertexCount; i++)
                        {
                            var vertex = mesh.RawVertices[i];

                            foreach (var entry in declaration)
                            {
                                var item = ReadVertexElement(elementStream, entry.Item2);

                                switch (resource.SecondaryLocator) // stream source
                                {
                                    case 0:
                                        switch (entry.Item1)
                                        {
                                            case Position:
                                                vertex.Point.Position = item.XYZ;
                                                break;

                                            // TODO: Add more
                                        }
                                        /*if (stream_reader.FindStreamedElement(BlamLib.Render.VertexBufferInterface.kTypePosition, ref quat))
                                        {
                                            Point.Position.X = quat.Vector.I;
                                            Point.Position.Y = quat.Vector.J;
                                            Point.Position.Z = quat.Vector.K;
                                        }
                                        if (stream_reader.FindStreamedElement(BlamLib.Render.VertexBufferInterface.kTypeNodeIndices, ref quat))
                                        {
                                            Point.NodeIndex[0].Value = ((int)quat.Vector.I) - 1;
                                            Point.NodeIndex[1].Value = ((int)quat.Vector.J) - 1;
                                            Point.NodeIndex[2].Value = ((int)quat.Vector.K) - 1;
                                            Point.NodeIndex[3].Value = ((int)quat.W) - 1;
                                        }
                                        if (stream_reader.FindStreamedElement(BlamLib.Render.VertexBufferInterface.kTypeNodeWeights, ref quat))
                                        {
                                            Point.NodeWeight[0].Value = quat.Vector.I;
                                            Point.NodeWeight[1].Value = quat.Vector.J;
                                            Point.NodeWeight[2].Value = quat.Vector.K;
                                            Point.NodeWeight[3].Value = quat.W;
                                        }*/
                                        break;

                                    case 1:
                                        /*if (stream_reader.FindStreamedElement(BlamLib.Render.VertexBufferInterface.kTypeTexCoord, ref quat))
                                        {
                                            Texcoord.X = quat.Vector.I;
                                            Texcoord.Y = quat.Vector.J;
                                        }*/
                                        break;

                                    case 2:
                                        /*if (stream_reader.FindStreamedElement(BlamLib.Render.VertexBufferInterface.kTypeNormal, ref quat))
                                        {
                                            Normal.I = quat.Vector.I;
                                            Normal.J = quat.Vector.J;
                                            Normal.K = quat.Vector.K;
                                        }
                                        if (stream_reader.FindStreamedElement(BlamLib.Render.VertexBufferInterface.kTypeBinormal, ref quat))
                                        {
                                            Binormal.I = quat.Vector.I;
                                            Binormal.J = quat.Vector.J;
                                            Binormal.K = quat.Vector.K;
                                        }
                                        if (stream_reader.FindStreamedElement(BlamLib.Render.VertexBufferInterface.kTypeTangent, ref quat))
                                        {
                                            Tangent.I = quat.Vector.I;
                                            Tangent.J = quat.Vector.J;
                                            Tangent.K = quat.Vector.K;
                                        }*/
                                        break;

                                    default:
                                        /*int x = 2;
                                        // The following LM vertex sources only have one element, so we can call this here
                                        stream_reader.GetStreamedElement(0, ref quat);
                                        if (section_info.SectionLightingFlags.Test(k_lighting_flags_HasLmTexCoords) && stream_source == ++x)
                                        {
                                            PrimaryLightmapTexcoord.X = quat.Vector.I;
                                            PrimaryLightmapTexcoord.Y = quat.Vector.J;
                                        }
                                        else if (section_info.SectionLightingFlags.Test(k_lighting_flags_HasLmIncRad) && stream_source == ++x)
                                        {
                                            PrimaryLightmapIncidentDirection.I = quat.Vector.I;
                                            PrimaryLightmapIncidentDirection.J = quat.Vector.J;
                                            PrimaryLightmapIncidentDirection.K = quat.Vector.K;
                                        }
                                        else if (section_info.SectionLightingFlags.Test(k_lighting_flags_HasLmColor) && stream_source == ++x)
                                        {
                                            // alpha is quad.W, which LM color doesn't use
                                            PrimaryLightmapColor.R = quat.Vector.I;
                                            PrimaryLightmapColor.G = quat.Vector.J;
                                            PrimaryLightmapColor.B = quat.Vector.K;
                                        }*/
                                        break;
                                }
                            }
                        }

                        currentVertexBuffer++;
                    }
                }
            }
            
            // TODO: Set up modifications to the 'mode' variable before returning it.
            return mode;
        }
    }
}