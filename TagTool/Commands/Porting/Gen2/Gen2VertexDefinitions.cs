using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Geometry;

namespace TagTool.Commands.Porting.Gen2
{
    class Gen2VertexDefinitions
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
            HenD3N,
            UByte, //added because they are necessary for blend indices (NOT normalized)
            UByte2,
            UByte3,
        }

        public static RealQuaternion ReadVertexElement(VertexElementStream stream, VertexDeclarationType type)
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
                case VertexDeclarationType.UByte:
                    return new RealQuaternion(stream.ReadUByte());
                case VertexDeclarationType.UByte2:
                    return new RealQuaternion(stream.ReadUByte(), stream.ReadUByte());
                case VertexDeclarationType.UByte3:
                    return new RealQuaternion(stream.ReadUByte(), stream.ReadUByte(), stream.ReadUByte());
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
                    return new RealQuaternion(stream.Read(3, () => ((float)stream.ReadShort() + (float)0x7FFF) / (float)0xFFFF));
                case VertexDeclarationType.Short4N:
                    return new RealQuaternion(stream.Read(4, () => ((float)stream.ReadShort() + (float)0x7FFF) / (float)0xFFFF));
                case VertexDeclarationType.HenD3N:
                    return RealQuaternion.FromHenDN3(stream.ReadColor());
                default:
                    throw new NotSupportedException(type.ToString());
            }
        }

        public static int CalculateVertexSize((string, VertexDeclarationUsage, VertexDeclarationType, int)[] vertex)
        {
            int size = 0;
            for (var i = 0; i < vertex.Length; i++)
            {
                if (vertex[i].Item3 == VertexDeclarationType.Skip)
                {
                    size += vertex[i].Item4;
                    continue;
                }
                size += GetVertexElementSize(vertex[i].Item3);
            }
            return size;
        }

        public static int GetVertexElementSize(VertexDeclarationType type)
        {
            switch (type)
            {
                case VertexDeclarationType.Float1:
                    return 0x4;
                case VertexDeclarationType.Float2:
                    return 0x8;
                case VertexDeclarationType.Float3:
                    return 0xC;
                case VertexDeclarationType.Float4:
                    return 0x10;
                case VertexDeclarationType.UByte:
                    return 0x1;
                case VertexDeclarationType.UByte2:
                    return 0x2;
                case VertexDeclarationType.UByte3:
                    return 0x3;
                case VertexDeclarationType.UByte4:
                    return 0x4;
                case VertexDeclarationType.UByte4N:
                    return 0x4;
                case VertexDeclarationType.Color:
                    return 0x4;
                case VertexDeclarationType.UShort2:
                    return 0x4;
                case VertexDeclarationType.UShort4:
                    return 0x8;
                case VertexDeclarationType.UShort2N:
                    return 0x4;
                case VertexDeclarationType.UShort4N:
                    return 0x8;
                case VertexDeclarationType.DHen3N:
                    return 0x4;
                case VertexDeclarationType.Half2:
                    return 0x4;
                case VertexDeclarationType.Half4:
                    return 0x8;
                case VertexDeclarationType.Dec3N:
                    return 0x4;
                case VertexDeclarationType.Byte4N:
                    return 0x4;
                case VertexDeclarationType.UByteN:
                    return 0x1;
                case VertexDeclarationType.UByte2N:
                    return 0x2;
                case VertexDeclarationType.UByte3N:
                    return 0x3;
                case VertexDeclarationType.UShortN:
                    return 0x2;
                case VertexDeclarationType.UShort3N:
                    return 0x6;
                case VertexDeclarationType.ShortN:
                    return 0x2;
                case VertexDeclarationType.Short2N:
                    return 0x4;
                case VertexDeclarationType.Short3N:
                    return 0x6;
                case VertexDeclarationType.Short4N:
                    return 0x8;
                case VertexDeclarationType.HenD3N:
                    return 0x4;
                default:
                    return 0;
            }
        }

        public static (string, VertexDeclarationUsage, VertexDeclarationType, int)[][] VertexDeclarations = new[]
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
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 0),
            },
            new[] // 0x04 model_rigid_boned1::compressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 0),
                ("None", VertexDeclarationUsage.Sample, VertexDeclarationType.Skip, 1),
            },
            new[] // 0x05 model_skinned2::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte2, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte2N, 0),
            },
            new[] // 0x06
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
            },
            new[] // 0x07 model_rigid_boned3::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte3, 0),
            },
            new[] // 0x08 model_skinned3::compressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte3, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte3N, 0),
            },
            new[] // 0x09 model_skinned4::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            },
            new[] // 0x0A
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
            },
            new[] // 0x0B
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 0),
            },
            new[] // 0x0C
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte2, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte2N, 0),
            },
            new[] // 0x0D
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte3, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte3N, 0),
            },
            new[] // 0x0E
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4, 0),
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
                ("SecondaryNodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 1),
            },
            new[] // 0x12
            {
                ("SecondaryPosition", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 1),
                ("SecondaryNodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 1),
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
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 0),
            },
            new[] // 0x17
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 0),
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

        public static Dictionary<int, (string, VertexDeclarationUsage, VertexDeclarationType, int)[]> VistaVertexDictionary =
            new Dictionary<int, (string, VertexDeclarationUsage, VertexDeclarationType, int)[]>
        {
            { 0x1, new[] // 0x01 model_rigid::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
            }
            },
            { 0x2, new[] // 0x02 model_skinned2::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte2, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte2N, 0),
            }
            },
            { 0x4, new[] // 0x04 model_skinned4::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            }
            },
            { 0x6, new[] // 0x06 model_skinned4::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            }
            },
            {0x8, new[] // 0x08 model_skinned4::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            }
            },
            {0x17, new[] // 0x17
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0)
            }
            },
            {0x19, new[] // 0x19
            {
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.Float3, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float3, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.Float3, 0),
            }
            },
            {0x1D, new[] // 0x1D
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0)
            }
            },
            {0x2E, new[] // 0x2E
            {
                ("IncidentRadiosity", VertexDeclarationUsage.Color, VertexDeclarationType.HenD3N, 1),
            }
            },
            {0x2F, new[] // 0x2F
            {
                ("IncidentRadiosity", VertexDeclarationUsage.Color, VertexDeclarationType.Float3, 1),
            }
            }
        };

        public static (string, VertexDeclarationUsage, VertexDeclarationType, int)[][] VertexDeclarationsVista = new[]
        {
            new (string, VertexDeclarationUsage, VertexDeclarationType, int)[0], // 0x00 Null
            new[] // 0x01 model_rigid::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
            },
            new[] // 0x02 model_skinned2::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte2, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte2N, 0),
            },
            new[] // 0x03 model_rigid_boned1::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 0),
            },
            new[] // 0x04 model_skinned4::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            },
            new[] // 0x05 model_skinned2::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte2, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte2N, 0),
            },
            new[] // 0x06 model_skinned4::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            },
            new[] // 0x07 model_rigid_boned3::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte3, 0),
            },
            new[] // 0x08 model_skinned4::uncompressed
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            },
            new[] // 0x09
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
            },
            new[] // 0x0A
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 0),
            },
            new[] // 0x0B
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte2, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte2N, 0),
            },
            new[] // 0x0C
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte3, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte3N, 0),
            },
            new[] // 0x0D
            {
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte4, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.UByte4N, 0),
            },
            new[] // 0x0E
            {
                ("SecondaryPosition", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 1),
            },
            new[] // 0x0F
            {
                ("SecondaryPosition", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 1),
            },
            new[] // 0x10
            {
                ("SecondaryPosition", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 1),
                ("SecondaryNodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 1),
            },
            new[] // 0x11
            {
                ("SecondaryPosition", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 1),
                ("SecondaryNodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 1),
                ("None", VertexDeclarationUsage.Sample, VertexDeclarationType.Skip, 1),
            },
            new[] // 0x12
            {
                ("SecondaryIsqSelect", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.UByteN, 2),
            },
            new[] // 0x13
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
            },
            new[] // 0x14
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
            },
            new[] // 0x15
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 0),
            },
            new[] // 0x16
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Short3N, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.UByte, 0),
                ("None", VertexDeclarationUsage.Sample, VertexDeclarationType.Skip, 1),
            },
            new[] // 0x17
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
            },
            new[] // 0x18
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
            },
            new[] // 0x19
            {
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.Float3, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float3, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.Float3, 0),
            },
            new[] // 0x1A
            {
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x1B
            {
                ("AnisoBinormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.Float3, 1),
            },
            new[] // 0x1C
            {
                ("AnisoBinormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 1),
            },
            new[] // 0x1D
            {
                ("SecondaryTexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 1),
            },
            new[] // 0x1E
            {
                ("SecondaryTexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 1),
            },
            new[] // 0x1F
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x20
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x21
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x22
            {
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.HenD3N, 0),
                ("Binormal", VertexDeclarationUsage.Binormal, VertexDeclarationType.HenD3N, 0),
                ("Tangent", VertexDeclarationUsage.Tangent, VertexDeclarationType.HenD3N, 0),
            },
            new[] // 0x23
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float2, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x24
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float4, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x25
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float4, 0),
                ("NodeIndices", VertexDeclarationUsage.BlendIndices, VertexDeclarationType.Float2, 0),
                ("NodeWeights", VertexDeclarationUsage.BlendWeight, VertexDeclarationType.Float2, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Normal", VertexDeclarationUsage.Normal, VertexDeclarationType.Float2, 0),
            },
            new[] // 0x26
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x27
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float2, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x28
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Float3, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x29 dynamic_vertex
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
            new[] // 0x2A
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.UShort4N, 0),
            },
            new[] // 0x2B
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x2C
            {
                ("Position", VertexDeclarationUsage.Position, VertexDeclarationType.Float3, 0),
                ("TexCoord", VertexDeclarationUsage.TextureCoordinate, VertexDeclarationType.Short2N, 0),
            },
            new[] // 0x2D lightmap_bucket_vertex.color::uncompressed
            {
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.Color, 0),
            },
            new[] // 0x2E lightmap_bucket_vertex.color::compressed
            {
                ("Color", VertexDeclarationUsage.Color, VertexDeclarationType.UByte3N, 0),
            },
            new[] // 0x2F lightmap_bucket_vertex.incident_direction
            {
                ("IncidentRadiosity", VertexDeclarationUsage.Color, VertexDeclarationType.Float3, 1),
            },
            new[] // 0x30
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
    }
}
