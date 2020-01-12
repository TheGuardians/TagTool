using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Resources;

namespace TagTool.Geometry
{
    public static class VertexBufferConverter
    {
        public static void ConvertVertices<T>(int count, Func<T> readVertex, Action<T, int> writeVertex)
        {
            for (var i = 0; i < count; i++)
                writeVertex(readVertex(), i);
        }

        public static void ConvertVertexBuffer(CacheVersion inVersion, CacheVersion outVersion, VertexBufferDefinition vertexBuffer)
        {
            using (var outputStream = new MemoryStream())
            using (var inputStream = new MemoryStream(vertexBuffer.Data.Data))
            {
                var inVertexStream = VertexStreamFactory.Create(inVersion, inputStream);
                var outVertexStream = VertexStreamFactory.Create(outVersion, outputStream);
                var count = vertexBuffer.Count;

                switch (vertexBuffer.Format)
                {
                    case VertexBufferFormat.World:
                        ConvertVertices(count, inVertexStream.ReadWorldVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            v.Tangent = ConvertVectorSpace(v.Tangent);
                            v.Binormal = ConvertVectorSpace(v.Binormal);
                            outVertexStream.WriteWorldVertex(v);
                        });
                        break;

                    case VertexBufferFormat.Rigid:
                        ConvertVertices(count, inVertexStream.ReadRigidVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            v.Tangent = ConvertVectorSpace(v.Tangent);
                            v.Binormal = ConvertVectorSpace(v.Binormal);
                            outVertexStream.WriteRigidVertex(v);
                        });
                        break;

                    case VertexBufferFormat.Skinned:
                        ConvertVertices(count, inVertexStream.ReadSkinnedVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            v.Tangent = ConvertVectorSpace(v.Tangent);
                            v.Binormal = ConvertVectorSpace(v.Binormal);
                            outVertexStream.WriteSkinnedVertex(v);
                        });
                        break;

                    case VertexBufferFormat.StaticPerPixel:
                        ConvertVertices(count, inVertexStream.ReadStaticPerPixelData, (v, i) => outVertexStream.WriteStaticPerPixelData(v));
                        break;

                    case VertexBufferFormat.StaticPerVertex:
                        ConvertVertices(count, inVertexStream.ReadStaticPerVertexData, (v, i) =>
                        {
                            v.Color1 = ConvertColorSpace(v.Color1);
                            v.Color2 = ConvertColorSpace(v.Color2);
                            v.Color3 = ConvertColorSpace(v.Color3);
                            v.Color4 = ConvertColorSpace(v.Color4);
                            v.Color5 = ConvertColorSpace(v.Color5);
                            outVertexStream.WriteStaticPerVertexData(v);
                        });
                        break;

                    // assume count has been fixed up before this
                    case VertexBufferFormat.AmbientPrt:
                        ConvertVertices(count, inVertexStream.ReadAmbientPrtData, (v, i) => outVertexStream.WriteAmbientPrtData(v));
                        break;

                    case VertexBufferFormat.LinearPrt:
                        ConvertVertices(count, inVertexStream.ReadLinearPrtData, (v, i) =>
                        {
                            v.SHCoefficients = ConvertNormal(v.SHCoefficients);
                            outVertexStream.WriteLinearPrtData(v);
                        });
                        break;

                    case VertexBufferFormat.QuadraticPrt:
                        ConvertVertices(count, inVertexStream.ReadQuadraticPrtData, (v, i) => outVertexStream.WriteQuadraticPrtData(v));
                        break;

                    case VertexBufferFormat.StaticPerVertexColor:
                        ConvertVertices(count, inVertexStream.ReadStaticPerVertexColorData, (v, i) => outVertexStream.WriteStaticPerVertexColorData(v));
                        break;

                    case VertexBufferFormat.Decorator:
                        ConvertVertices(count, inVertexStream.ReadDecoratorVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            outVertexStream.WriteDecoratorVertex(v);
                        });
                        break;

                    case VertexBufferFormat.World2:
                        vertexBuffer.Format = VertexBufferFormat.World;
                        goto case VertexBufferFormat.World;

                        /*
                    case VertexBufferFormat.Unknown1A:

                        var waterData = WaterData[CurrentWaterBuffer];

                        // Reformat Vertex Buffer
                        vertexBuffer.Format = VertexBufferFormat.World;
                        vertexBuffer.VertexSize = 0x34;
                        vertexBuffer.Count = waterData.IndexBufferLength;

                        // Create list of indices for later use.
                        Unknown1BIndices = new List<ushort>();

                        for (int k = 0; k < waterData.PartData.Count(); k++)
                        {
                            Tuple<int, int, bool> currentPartData = waterData.PartData[k];

                            // Not water, add garbage data
                            if (currentPartData.Item3 == false)
                            {
                                for (int j = 0; j < currentPartData.Item2; j++)
                                    WriteUnusedWorldWaterData(outputStream);
                            }
                            else
                            {
                                ConvertVertices(currentPartData.Item2 / 3, inVertexStream.ReadUnknown1A, (v, i) =>
                                {
                                    // Store current stream position
                                    var tempStreamPosition = inputStream.Position;

                                    // Open previous world buffer (H3)
                                    var worldVertexBufferBasePosition = OriginalBufferOffsets[OriginalBufferOffsets.Count() - 3];
                                    inputStream.Position = worldVertexBufferBasePosition;

                                    for (int j = 0; j < 3; j++)
                                    {
                                        inputStream.Position = 0x20 * v.Vertices[j] + worldVertexBufferBasePosition;

                                        WorldVertex w = inVertexStream.ReadWorldVertex();

                                        Unknown1BIndices.Add(v.Indices[j]);

                                        // The last 2 floats in WorldWater are unknown.

                                        outVertexStream.WriteWorldWaterVertex(w);
                                    }

                                    // Restore position for reading the next vertex correctly
                                    inputStream.Position = tempStreamPosition;
                                });
                            }
                        }
                        break;

                    case VertexBufferFormat.Unknown1B:

                        var waterDataB = WaterData[CurrentWaterBuffer];

                        // Adjust vertex size to match HO. Set count of vertices

                        vertexBuffer.VertexSize = 0x18;

                        var originalCount = vertexBuffer.Count;
                        vertexBuffer.Count = waterDataB.IndexBufferLength;

                        var basePosition = inputStream.Position;
                        var unknown1BPosition = 0;

                        for (int k = 0; k < waterDataB.PartData.Count(); k++)
                        {
                            Tuple<int, int, bool> currentPartData = waterDataB.PartData[k];

                            // Not water, add garbage data
                            if (currentPartData.Item3 == false)
                            {
                                for (int j = 0; j < currentPartData.Item2; j++)
                                    WriteUnusedUnknown1BData(outputStream);
                            }
                            else
                            {
                                for (int j = unknown1BPosition; j < Unknown1BIndices.Count() && j - unknown1BPosition < currentPartData.Item2; j++)
                                {
                                    inputStream.Position = basePosition + 0x24 * Unknown1BIndices[j];
                                    ConvertVertices(1, inVertexStream.ReadUnknown1B, (v, i) => outVertexStream.WriteUnknown1B(v));
                                    unknown1BPosition++;
                                }
                            }
                        }

                        // Get to the end of Unknown1B in H3 data
                        inputStream.Position = basePosition + originalCount * 0x24;

                        CurrentWaterBuffer++;

                        break;
                        */
                    case VertexBufferFormat.ParticleModel:
                        ConvertVertices(count, inVertexStream.ReadParticleModelVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            outVertexStream.WriteParticleModelVertex(v);
                        });
                        break;

                    case VertexBufferFormat.TinyPosition:
                        ConvertVertices(count, inVertexStream.ReadTinyPositionVertex, (v, i) =>
                        {
                            v.Position = ConvertPositionShort(v.Position);
                            v.Variant = (ushort)((v.Variant >> 8) & 0xFF);
                            v.Normal = ConvertNormal(v.Normal);
                            outVertexStream.WriteTinyPositionVertex(v);
                        });
                        break;

                    default:
                        throw new NotSupportedException(vertexBuffer.Format.ToString());
                }

                // set proper size of vertices
                vertexBuffer.VertexSize = (short)outVertexStream.GetVertexSize(vertexBuffer.Format);
                // set data
                vertexBuffer.Data.Data = outputStream.ToArray();
            }
        }

        /// <summary>
        /// Writes 0x38 worth of invalid data
        /// </summary>
        /// <param name="outputStream"></param>
        public static void WriteUnusedWorldWaterData(Stream outputStream)
        {
            byte[] data = new byte[1] { 0xCD};
            for (int i = 0; i < 0x38; i++)
            {
                outputStream.Write(data, 0, 1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outputStream"></param>
        public static void WriteUnusedUnknown1BData(Stream outputStream)
        {
            byte[] data = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
            for (int i = 0; i < 6; i++)
            {
                outputStream.Write(data, 0, 4);
            }
        }


        /// <summary> 
        /// Change basis [0,1] to [-1,1] uniformly
        /// </summary>
        public static float ConvertFromNormalBasis(float value)
        {
            value = VertexElementStream.Clamp(value, 0.0f, 1.0f);
            return 2.0f * (value - 0.5f);
        }

        /// <summary> 
        /// Change basis [-1,1] to [0,1] uniformly
        /// </summary>
        public static float ConvertToNormalBasis(float value)
        {
            value = VertexElementStream.Clamp(value);
            return (value / 2.0f) + 0.5f;
        }

        /// <summary>
        /// Modify value to account for some rounding error in gen3 conversion
        /// </summary>
        public static float FixRoundingShort(float value)
        {
            value = VertexElementStream.Clamp(value);
            if (value > 0 && value < 1)
            {
                value += 1.0f / 32767.0f;
                value = VertexElementStream.Clamp(value);
            }
            return value;
        }

        public static uint ConvertColorSpace(uint color)
        {
            uint c1 = color & 0xFF;
            uint c2 = (color >> 8) & 0xFF;
            uint c3 = (color >> 16) & 0xFF;
            uint c4 = (color >> 24) & 0xFF;
            c1 = (c1 + 0x7F) & 0xFF;
            c2 = (c2 + 0x7F) & 0xFF;
            c3 = (c3 + 0x7F) & 0xFF;
            c4 = (c4 + 0x7F) & 0xFF;

            return c1 | (c2 << 8) | (c3 << 16) | (c4 << 24);
        }

        public static float ConvertVectorSpace(float value)
        {
            return value <= 0.5 ? 2.0f * value : (value - 1.0f) * 2.0f;
        }

        public static RealVector3d ConvertVectorSpace(RealVector3d vector)
        {
            return new RealVector3d(ConvertVectorSpace(vector.I), ConvertVectorSpace(vector.J), ConvertVectorSpace(vector.K));
        }

        public static RealQuaternion ConvertVectorSpace(RealQuaternion vector)
        {
            return new RealQuaternion(ConvertVectorSpace(vector.I), ConvertVectorSpace(vector.J), ConvertVectorSpace(vector.K), vector.W);
        }

        /// <summary>
        /// Convert H3 normal to HO normal for tinyposition vertex
        /// </summary>
        public static RealQuaternion ConvertNormal(RealQuaternion normal)
        {
            return new RealQuaternion(normal.ToArray().Select(e => ConvertToNormalBasis(e)).Reverse());
        }

        /// <summary>
        /// Convert H3 position to HO position including rounding error for tinyposition vertex
        /// </summary>
        public static RealVector3d ConvertPositionShort(RealVector3d position)
        {
            return new RealVector3d(position.ToArray().Select(e => FixRoundingShort(ConvertFromNormalBasis(e))).ToArray());
        }
    }
}
