using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public enum VertexDeclarationUsage : sbyte
        {
            Sample = -1,
            Position,
            BlendIndices,
            BlendWeight,
            TextureCoordinate,
            Normal,
            Binormal,
            Tangent,
            Color
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
            Color,
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

        private (string, VertexDeclarationUsage, VertexDeclarationType, int)[][] VertexDeclarations = new[]
        {
            new (string, VertexDeclarationUsage, VertexDeclarationType, int)[0], // 0x00 Null
            new[] // 0x01 model_rigid::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
            },
            new[] // 0x02 model_rigid::compressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
            },
            new[] // 0x03 model_rigid_boned1::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByteN, 0),
            },
            new[] // 0x04 model_rigid_boned1::compressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByteN, 0),
                ("None", VertexDeclarationUsage.Sample, VertexDeclarationType.Skip, 1),
            },
            new[] // 0x05 model_skinned2::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte2N, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte2N, 0),
            },
            new[] // 0x06
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
            },
            new[] // 0x07 model_rigid_boned3::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte3N, 0),
            },
            new[] // 0x08 model_skinned3::compressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte3N, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte3N, 0),
            },
            new[] // 0x09 model_skinned4::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4N, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            },
            new[] // 0x0A
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
            },
            new[] // 0x0B
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByteN, 0),
            },
            new[] // 0x0C
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte2N, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte2N, 0),
            },
            new[] // 0x0D
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte3N, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte3N, 0),
            },
            new[] // 0x0E
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4N, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            },
            new[] // 0x0F
            {
                ("SecondaryPosition", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 1),
            },
            new[] // 0x10
            {
                ("SecondaryPosition", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 1),
            },
            new[] // 0x11
            {
                ("SecondaryPosition", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 1),
                ("SecondaryNodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByteN, 1),
            },
            new[] // 0x12
            {
                ("SecondaryPosition", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 1),
                ("SecondaryNodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByteN, 1),
                ("None", VertexDeclarationUsage.Sample, VertexDeclarationType.Skip, 1),
            },
            new[] // 0x13
            {
                ("SecondaryIsqSelect", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.UByteN, 2),
            },
            new[] // 0x14
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
            },
            new[] // 0x15
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
            },
            new[] // 0x16
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByteN, 0),
            },
            new[] // 0x17
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByteN, 0),
                ("None", VertexDeclarationUsage.Sample, VertexDeclarationType.Skip, 1),
            },
            new[] // 0x18
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
            },
            new[] // 0x19
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
            },
            new[] // 0x1A
            {
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.Float3, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float3, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.Float3, 0),
            },
            new[] // 0x1B
            {
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x1C
            {
                ("AnisoBinormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float3, 1),
            },
            new[] // 0x1D
            {
                ("AnisoBinormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 1),
            },
            new[] // 0x1E
            {
                ("SecondaryTexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 1),
            },
            new[] // 0x1F
            {
                ("SecondaryTexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 1),
            },
            new[] // 0x20
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x21
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x22
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x23
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x24
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float2, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x25
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float4, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x26
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float4, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.Float2, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.Float2, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.Float2, 0),
            },
            new[] // 0x27
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x28
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x29
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float3, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x2A dynamic_vertex
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float4, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.Float3, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.Float1, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float4, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.Float3, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float2, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.Float4, 0),
                ("AnisoBinormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float4, 1),
                ("SecondaryTexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Color, 1),
            },
            new[] // 0x2B
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.UShort4N, 0),
            },
            new[] // 0x2C
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x2D
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
            },
            new[] // 0x2E lightmap_bucket_vertex.color::uncompressed
            {
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x2F lightmap_bucket_vertex.color::compressed
            {
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.UByte3N, 0),
            },
            new[] // 0x30 lightmap_bucket_vertex.incident_direction
            {
                ("IncidentRadiosity", VertexDeclarationUsage.Color, VertexDeclarationType.HenD3N, 1),
            },
            new[] // 0x31
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float4, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
                ("TintFactor", VertexDeclarationUsage.Color, VertexDeclarationType.Float2, 2),
            },
            new[] // 0x32 s_decorator_model_vertex::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.Float3, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.Float3, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
            },
            new[] // 0x33 s_decorator_model_vertex::compressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.HenD3N, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
            },
            new[] // 0x34 rasterizer_vertex_decorator_decal
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("SecondaryTexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 1),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x35
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x36 rasterizer_vertex_decorator_sprite::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("BillboardOffset", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float3, 4),
                ("BillboardAxis", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float3, 5),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x37 rasterizer_vertex_decorator_sprite::compressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("BillboardOffset", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.HenD3N, 4),
                ("BillboardAxis", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.HenD3N, 5),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x38
            {
                ("PcaClusterId", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float1, 6),
                ("PcaVertexWeights", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float4, 7),
            },
            new[] // 0x39
            {
                ("PcaClusterId", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.UShortN, 6),
                ("PcaVertexWeights", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short4N, 7),
            },
            new[] // 0x3A
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float2, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.Float2, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Color, 0),
            },
            new[] // 0x3B s_particle_model_vertex::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.Float3, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.Float3, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
            },
            new[] // 0x3C s_particle_model_vertex::uncompressed2
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.Float3, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.Float3, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
            },
            new[] // 0x3D s_particle_model_vertex::compressed?
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.HenD3N, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
            },
            new[] // 0x3E
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float4, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
            }
        };

        private RealQuaternion ReadVertexElement(VertexElementStream stream, VertexDeclarationType type)
        {
            switch (type)
            {
                case VertexDeclarationType.Float1:
                    return new RealQuaternion(stream.ReadFloat1());
                case VertexDeclarationType.Float2:
                    return new RealQuaternion(stream.ReadFloat2().ToArray());
                case VertexDeclarationType.Float3:
                    return new RealQuaternion(stream.ReadFloat3().ToArray());
                case VertexDeclarationType.Float4:
                    return new RealQuaternion(stream.ReadFloat4().ToArray());
                case VertexDeclarationType.UByte4:
                    return new RealQuaternion(stream.ReadUByte4().Select(i => (float)i));
                case VertexDeclarationType.UByte4N:
                    return stream.ReadUByte4N();
                case VertexDeclarationType.Color:
                    return new RealQuaternion(BitConverter.GetBytes(stream.ReadColor()).Select(i => (float)i));
                case VertexDeclarationType.UShort2:
                    return new RealQuaternion(stream.ReadUShort2().ToArray());
                case VertexDeclarationType.UShort4:
                    return stream.ReadUShort4();
                case VertexDeclarationType.UShort2N:
                    return new RealQuaternion(stream.ReadUShort2N().ToArray());
                case VertexDeclarationType.UShort4N:
                    return stream.ReadUShort4N();
                case VertexDeclarationType.DHen3N:
                    return new RealQuaternion(stream.ReadDHen3N().ToArray());
                case VertexDeclarationType.Half2:
                    return new RealQuaternion(stream.ReadFloat16_2().ToArray());
                case VertexDeclarationType.Half4:
                    return stream.ReadFloat16_4();
                case VertexDeclarationType.Dec3N:
                    return new RealQuaternion(stream.ReadDec3N().ToArray());
                case VertexDeclarationType.Byte4N:
                    return stream.ReadSByte4N();
                case VertexDeclarationType.UByteN:
                    return new RealQuaternion(stream.ReadUByteN());
                case VertexDeclarationType.UByte2N:
                    return stream.ReadUByte2N();
                case VertexDeclarationType.UByte3N:
                    return stream.ReadUByte3N();
                case VertexDeclarationType.UShortN:
                    return new RealQuaternion(stream.ReadUShortN());
                case VertexDeclarationType.UShort3N:
                    return new RealQuaternion(stream.ReadUShort3N().ToArray());
                case VertexDeclarationType.ShortN:
                    return new RealQuaternion(stream.ReadShortN());
                case VertexDeclarationType.Short2N:
                    return new RealQuaternion(stream.Read(2, () => ((float)stream.ReadShort() + (float)0x7FFF) / (float)0xFFFF));
                case VertexDeclarationType.Short3N:
                    return new RealQuaternion(stream.ReadShort3N().ToArray());
                case VertexDeclarationType.Short4N:
                    return stream.ReadShort4N();
                case VertexDeclarationType.HenD3N:
                    return RealQuaternion.FromHenDN3(stream.ReadColor());
                default:
                    throw new NotSupportedException(type.ToString());
            }
        }

        private object ConvertGen2RenderModel(CachedTagInstance edTag, RenderModel mode)
        {
            foreach (var section in mode.Sections)
            {
                var compressor = new VertexCompressor(
                    section.Compression.Count > 0 ?
                        section.Compression[0] :
                    mode.Compression.Count > 0 ?
                        mode.Compression[0] :
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

                    section.Meshes.Add(mesh);

                    if (mesh.RawVertices.Count > 0)
                        continue;

                    mesh.RawVertices = new List<Mesh.RawVertex>(section.TotalVertexCount);

                    for (var i = 0; i < section.TotalVertexCount; i++)
                        mesh.RawVertices.Add(new Mesh.RawVertex());

                    var currentVertexBuffer = 0;

                    foreach (var resource in section.Resources)
                    {
                        if (resource.Type != ResourceTypeGen2.VertexBuffer)
                            continue;
                        
                        var vertexBuffer = mesh.VertexBuffers[currentVertexBuffer];

                        if (vertexBuffer.TypeIndex == 0)
                            continue;

                        var elementStream = new VertexElementStream(stream, BlamCache.Reader.Format);
                        var declaration = VertexDeclarations[vertexBuffer.TypeIndex];

                        for (var i = 0; i < section.TotalVertexCount; i++)
                        {
                            var vertex = mesh.RawVertices[i];

                            stream.Position = 8 + section.SectionDataSize + resource.ResourceDataOffset + ((resource.ResoureDataSize / section.TotalVertexCount) * i);

                            foreach (var entry in declaration)
                            {
                                if (entry.Item3 == VertexDeclarationType.Skip)
                                {
                                    elementStream.SeekTo(entry.Item4, SeekOrigin.Current);
                                    continue;
                                }
                                
                                switch (resource.SecondaryLocator) // stream source
                                {
                                    case 0:
                                        {
                                            var element = ReadVertexElement(elementStream, entry.Item3);

                                            vertex.Point.NodeWeights = new[] { 1.0f, 0.0f, 0.0f, 0.0f };
                                            vertex.Point.NodeIndices = new[] { -1, -1, -1, -1 };
                                            vertex.Point.UseNewNodeIndices = 1;
                                            vertex.Point.AdjustedCompoundNodeIndex = -1;
                                            vertex.SecondaryTexcoord.Y = 1.0f;

                                            switch (entry.Item2)
                                            {
                                                case VertexDeclarationUsage.Position:
                                                    vertex.Point.Position = element.XYZ;
                                                    if (section.GeometryCompressionFlags.HasFlag(RenderGeometryCompressionFlags.CompressedPosition))
                                                        vertex.Point.Position = compressor.DecompressPosition(new RealQuaternion(vertex.Point.Position.ToArray())).XYZ;
                                                    break;

                                                case VertexDeclarationUsage.BlendIndices:
                                                    vertex.Point.NodeIndices = element.ToArray().Select(x => (int)x).ToArray();
                                                    break;

                                                case VertexDeclarationUsage.BlendWeight:
                                                    vertex.Point.NodeWeights = element.ToArray();
                                                    break;
                                            }
                                            break;
                                        }

                                    case 1:
                                        {
                                            var element = ReadVertexElement(elementStream, entry.Item3);

                                            if (entry.Item2 == VertexDeclarationUsage.TextureCoordinate)
                                            {
                                                vertex.Texcoord = element.XY;

                                                if (section.GeometryCompressionFlags.HasFlag(RenderGeometryCompressionFlags.CompressedTexcoord))
                                                    vertex.Texcoord = compressor.DecompressUv(new RealVector2d(vertex.Texcoord.ToArray())).XY;
                                            }
                                            break;
                                        }

                                    case 2:
                                        {
                                            var element = ReadVertexElement(elementStream, entry.Item3);

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
                                        }

                                    case 3:
                                        {
                                            var element = ReadVertexElement(elementStream, entry.Item3);

                                            if (section.LightingFlags.HasFlag(RenderModel.SectionLightingFlags.HasLightmapTexcoords))
                                                vertex.PrimaryLightmapTexcoord = element.XY;
                                            break;
                                        }

                                    case 4:
                                        {
                                            var element = ReadVertexElement(elementStream, entry.Item3);

                                            if (section.LightingFlags.HasFlag(RenderModel.SectionLightingFlags.HasLightmapIncRad))
                                                vertex.SecondaryLightmapIncidentDirection = element.IJK;
                                            break;
                                        }

                                    case 5:
                                        {
                                            var element = ReadVertexElement(elementStream, entry.Item3);

                                            if (section.LightingFlags.HasFlag(RenderModel.SectionLightingFlags.HasLightmapColors))
                                                vertex.PrimaryLightmapColor = element.RGB;
                                            break;
                                        }

                                    default:
                                        throw new Exception();
                                }
                            }
                        }

                        currentVertexBuffer++;
                    }
                }
            }

            foreach (var section in mode.PrtInfo)
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
                    var rawPcaDatum = BlamCache.Deserializer.Deserialize<RenderModel.PrtInfoBlock.RawPcaDatum>(dataContext);

                    section.RawPcaData.Add(rawPcaDatum);

                    // TODO: prt vertex buffers
                }
            }

            var builder = new RenderModelBuilder(CacheContext.Version);

            foreach (var node in mode.Nodes)
                builder.AddNode(node);

            foreach (var material in mode.Materials)
                builder.AddMaterial(new RenderMaterial { RenderMethod = CacheContext.GetTagInstance<Shader>(@"shaders\invalid") });

            foreach (var region in mode.Regions)
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

                    var section = mode.Sections[sectionIndex];

                    foreach (var mesh in section.Meshes)
                    {
                        builder.BeginMesh();

                        var worldVertices = new List<WorldVertex>();
                        var rigidVertices = new List<RigidVertex>();
                        var skinnedVertices = new List<SkinnedVertex>();

                        foreach (var vertex in mesh.RawVertices)
                        {
                            switch (section.GeometryClassification)
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
                                    skinnedVertices.Add(new SkinnedVertex
                                    {
                                        Position = new RealQuaternion(vertex.Point.Position.ToArray()),
                                        Texcoord = vertex.Texcoord.IJ,
                                        Normal = vertex.Normal,
                                        Tangent = new RealQuaternion(vertex.Tangent.ToArray()),
                                        Binormal = vertex.Binormal,
                                        BlendIndices = vertex.Point.NodeIndices.Select(i => (byte)i).ToArray(),
                                        BlendWeights = section.GeometryClassification == RenderGeometryClassification.RigidBoned ?
                                            new[] { 1.0f, 0.0f, 0.0f, 0.0f } :
                                            vertex.Point.NodeWeights
                                    });
                                    break;
                                    
                                default:
                                    throw new NotSupportedException(section.GeometryClassification.ToString());
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

                                builder.DefineSubPart(subPart.FirstIndex, subPart.IndexCount, (ushort)subPartIndices.Count);
                            }

                            builder.EndPart();
                        }

                        switch (section.GeometryClassification)
                        {
                            case RenderGeometryClassification.Worldspace:
                                builder.BindWorldVertexBuffer(worldVertices);
                                break;

                            case RenderGeometryClassification.Rigid:
                                builder.BindRigidVertexBuffer(rigidVertices, (sbyte)section.RigidNode);
                                break;

                            case RenderGeometryClassification.RigidBoned:
                            case RenderGeometryClassification.Skinned:
                                builder.BindSkinnedVertexBuffer(skinnedVertices);
                                break;

                            default:
                                throw new NotSupportedException(section.GeometryClassification.ToString());
                        }
                        
                        builder.BindIndexBuffer(mesh.StripIndices.Select(i => (ushort)i.Index), IndexBufferFormat.TriangleStrip);

                        builder.EndMesh();
                    }

                    builder.EndPermutation();
                }

                builder.EndRegion();
            }

            using (var resourceDataStream = new MemoryStream())
            {
                var result = builder.Build(CacheContext.Serializer, resourceDataStream);
                result.MarkerGroups = mode.MarkerGroups;

                foreach (var mesh in result.Geometry.Meshes)
                    if (mesh.Type == VertexType.Skinned)
                        mesh.RigidNodeIndex = -1;
                
                resourceDataStream.Position = 0;
                result.Geometry.Resource.ChangeLocation(ResourceLocation.ResourcesB);
                CacheContext.AddResource(result.Geometry.Resource, resourceDataStream);

                mode = result;
            }

            return mode;
        }
    }
}