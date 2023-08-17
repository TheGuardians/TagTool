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

        public static void ConvertVertexBuffer(CacheVersion inVersion, CachePlatform inPlatform, CacheVersion outVersion, CachePlatform outPlatform, VertexBufferDefinition vertexBuffer)
        {
            using (var outputStream = new MemoryStream())
            using (var inputStream = new MemoryStream(vertexBuffer.Data.Data))
            {
                var inVertexStream = VertexStreamFactory.Create(inVersion, inPlatform, inputStream);
                var outVertexStream = VertexStreamFactory.Create(outVersion, outPlatform, outputStream);

                VertexStreamReach reachVertexStream = null;
                if (inVersion >= CacheVersion.HaloReach)
                    reachVertexStream = (VertexStreamReach)inVertexStream;

                var count = vertexBuffer.Count;

                switch (vertexBuffer.Format)
                {
                    case VertexBufferFormat.World:
                        ConvertVertices(count, inVertexStream.ReadWorldVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            v.Tangent = ConvertVectorSpace(v.Tangent);

                            if (inVersion >= CacheVersion.HaloReach)
                                v.Binormal = GenerateReachBinormals(v.Normal, v.Tangent.IJK, v.Tangent.W);
                            else
                                v.Binormal = ConvertVectorSpace(v.Binormal);

                            outVertexStream.WriteWorldVertex(v);
                        });
                        break;

                    case VertexBufferFormat.Rigid:
                        ConvertVertices(count, inVertexStream.ReadRigidVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            v.Tangent = ConvertVectorSpace(v.Tangent);

                            if (inVersion >= CacheVersion.HaloReach)
                                v.Binormal = GenerateReachBinormals(v.Normal, v.Tangent.IJK, v.Tangent.W);
                            else
                                v.Binormal = ConvertVectorSpace(v.Binormal);

                            outVertexStream.WriteRigidVertex(v);
                        });
                        break;

                    case VertexBufferFormat.Skinned:
                        ConvertVertices(count, inVertexStream.ReadSkinnedVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            v.Tangent = ConvertVectorSpace(v.Tangent);

                            if (inVersion >= CacheVersion.HaloReach)
                                v.Binormal = GenerateReachBinormals(v.Normal, v.Tangent.IJK, v.Tangent.W);
                            else
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
                            if (inPlatform != CachePlatform.MCC)
                            {
                                v.Color1 = ConvertColorSpace(v.Color1);
                                v.Color2 = ConvertColorSpace(v.Color2);
                                v.Color3 = ConvertColorSpace(v.Color3);
                                v.Color4 = ConvertColorSpace(v.Color4);
                                v.Color5 = ConvertColorSpace(v.Color5);
                            }
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

                    case VertexBufferFormat.ParticleModel:
                        ConvertVertices(count, inVertexStream.ReadParticleModelVertex, (v, i) =>
                        {
                            if (inPlatform != CachePlatform.MCC)
                                v.Normal = ConvertVectorSpace(v.Normal);

                            outVertexStream.WriteParticleModelVertex(v);
                        });
                        break;

                    case VertexBufferFormat.TinyPosition:
                        ConvertVertices(count, inVertexStream.ReadTinyPositionVertex, (v, i) =>
                        {
                            if (!CacheVersionDetection.IsInGen(CacheGeneration.HaloOnline, inVersion))
                                v.Variant = (ushort)UShort2Short(v.Variant);

                            v.Position = ConvertPositionShort(v.Position);
                            v.Normal = ConvertNormal(v.Normal);
                            outVertexStream.WriteTinyPositionVertex(v);
                        });
                        break;
                    
                    case VertexBufferFormat.RigidCompressed:
                        ConvertVertices(count, reachVertexStream.ReadReachRigidVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            v.Tangent = ConvertVectorSpace(v.Tangent);
                            v.Binormal = GenerateReachBinormals(v.Normal, v.Tangent.IJK, v.Tangent.W);
                            outVertexStream.WriteRigidVertex(v);
                        });
                        vertexBuffer.Format = VertexBufferFormat.Rigid;
                        break;

                    case VertexBufferFormat.SkinnedCompressed:
                        ConvertVertices(count, reachVertexStream.ReadReachSkinnedVertex, (v, i) =>
                        {
                            v.Normal = ConvertVectorSpace(v.Normal);
                            v.Tangent = ConvertVectorSpace(v.Tangent);
                            v.Binormal = GenerateReachBinormals(v.Normal, v.Tangent.IJK, v.Tangent.W);
                            outVertexStream.WriteSkinnedVertex(v);
                        });
                        vertexBuffer.Format = VertexBufferFormat.Skinned;
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
        /// Writes the specificed amount of debug data 0xCD
        /// </summary>
        public static void DebugFill(Stream stream, int amount)
        {
            var data = new byte[1] { 0xCD };
            for (int i = 0; i < amount; i++)
                stream.Write(data, 0, 1);
        }

        /// <summary>
        /// Writes the specificed amount of bytes to fill
        /// </summary>
        public static void Fill(Stream stream, int amount)
        {
            var data = new byte[1] { 0x00};
            for (int i = 0; i < amount; i++)
                stream.Write(data, 0, 1);
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

        public static RealVector3d GenerateReachBinormalsNoSign(RealVector2d uv, RealVector3d tangent, RealVector3d normal)
        {
            return Math.Sign(uv.I - 0.5) * RealVector3d.CrossProduct(normal, tangent);
        }

        public static RealVector3d GenerateReachBinormals(RealVector3d normal, RealVector3d tangent, float sign)
        {
            return RealVector3d.CrossProductNoNorm(normal, tangent) * sign;
        }

        /// <summary>
        /// Convert ushort [0,65535] to short [-32767,32767]
        /// </summary>
        private static short UShort2Short(ushort value)
        {
            if (value <= short.MaxValue)
                return (short)(value - short.MaxValue);
            return (short)(value - short.MaxValue - 1);
        }
    }
}
