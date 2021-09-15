using TagTool.Common;
using TagTool.IO;
using System;
using System.IO;

namespace TagTool.Geometry
{
    public class VertexStreamReach : IVertexStream
    {
        private VertexElementStream Stream { get; }

        public VertexStreamReach(Stream stream)
        {
            Stream = new VertexElementStream(stream, EndianFormat.BigEndian);
        }

        public AmbientPrtData ReadAmbientPrtData() => null;

        public BeamVertex ReadBeamVertex()
        {
            throw new NotImplementedException();
        }

        public ChudVertexFancy ReadChudVertexFancy()
        {
            return new ChudVertexFancy
            {
                Position = Stream.ReadFloat3(),
                Color = Stream.ReadColor(),
                Texcoord = Stream.ReadFloat2()
            };
        }

        public ChudVertexSimple ReadChudVertexSimple()
        {
            return new ChudVertexSimple
            {
                Position = Stream.ReadFloat2(),
                Texcoord = Stream.ReadFloat2()
            };
        }

        public ContrailVertex ReadContrailVertex()
        {
            throw new NotImplementedException();
        }

        public DebugVertex ReadDebugVertex()
        {
            return new DebugVertex
            {
                Position = Stream.ReadFloat3(),
                Color = Stream.ReadColor()
            };
        }

        public DecoratorVertex ReadDecoratorVertex()
        {
            var vertex = new DecoratorVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = new RealQuaternion(Stream.ReadDHen3N(), 1.0f)
            };
            return vertex;
        }

        public DualQuatVertex ReadDualQuatVertex()
        {
            throw new NotImplementedException();
        }

        public RigidVertex ReadReachRigidVertex()
        {
            return new RigidVertex
            {
                Position = Stream.ReadUDec4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
            };
        }

        public SkinnedVertex ReadReachSkinnedVertex()
        {
            return new SkinnedVertex
            {
                Position = Stream.ReadUDec4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
                BlendIndices = Stream.ReadUByte4(),
                BlendWeights = Stream.ReadUByte4N().ToArray()
            };
        }

        public FlatRigidVertex ReadFlatRigidVertex()
        {
            var vertex = new FlatRigidVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
            };
            vertex.Binormal = RealVector3d.CrossProduct(vertex.Normal, vertex.Tangent.IJK);
            return vertex;
        }

        public FlatSkinnedVertex ReadFlatSkinnedVertex()
        {
            var vertex = new FlatSkinnedVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
                BlendIndices = Stream.ReadUByte4(),
                BlendWeights = Stream.ReadUByte4N().ToArray()
            };
            vertex.Binormal = RealVector3d.CrossProduct(vertex.Normal, vertex.Tangent.IJK);
            return vertex;
        }

        public FlatWorldVertex ReadFlatWorldVertex()
        {
            var vertex = new FlatWorldVertex
            {
                Position = Stream.ReadFloat4(),
                Texcoord = Stream.ReadFloat16_2(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
            };
            vertex.Binormal = RealVector3d.CrossProduct(vertex.Normal, vertex.Tangent.IJK);
            return vertex;
        }

        public ImplicitVertex ReadImplicitVertex()
        {
            return new ImplicitVertex
            {
                Position = Stream.ReadFloat3(),
                Texcoord = Stream.ReadFloat2()
            };
        }

        public LightVolumeVertex ReadLightVolumeVertex()
        {
            throw new NotImplementedException();
        }

        public LinearPrtData ReadLinearPrtData() => null;

        public ParticleModelVertex ReadParticleModelVertex()
        {
            return new ParticleModelVertex
            {
                Position = new RealVector3d(Stream.ReadUShort4N()),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N()
            };
        }

        public ParticleVertex ReadParticleVertex()
        {
            return new ParticleVertex
            {
                Position = Stream.ReadFloat2(),
                Texcoord = Stream.ReadFloat2(),
            };
        }

        public PatchyFogVertex ReadPatchyFogVertex()
        {
            return new PatchyFogVertex
            {
                Position = Stream.ReadFloat4()
            };
        }

        public QuadraticPrtData ReadQuadraticPrtData() => null;

        public RigidVertex ReadRigidVertex()
        {
            var vertex = new RigidVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
            };
            vertex.Binormal = RealVector3d.CrossProduct(vertex.Normal, vertex.Tangent.IJK);
            return vertex;
        }

        public RippleVertex ReadRippleVertex()
        {
            return new RippleVertex
            {
                Position = Stream.ReadFloat4(),
                Texcoord = Stream.ReadFloat4(),
                Texcoord2 = Stream.ReadFloat4(),
                Texcoord3 = Stream.ReadFloat4(),
                Texcoord4 = Stream.ReadFloat4(),
                Texcoord5 = Stream.ReadFloat4(),
                Color = Stream.ReadFloat4(),
                Color2 = Stream.ReadFloat4(),
                Texcoord6 = new short[] { 0, 0 }
            };
        }

        public ScreenVertex ReadScreenVertex()
        {
            return new ScreenVertex
            {
                Position = Stream.ReadFloat2(),
                Texcoord = Stream.ReadFloat2(),
                Color = Stream.ReadColor()
            };
        }

        public SkinnedVertex ReadSkinnedVertex()
        {
            var vertex = new SkinnedVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
                BlendIndices = Stream.ReadUByte4(),
                BlendWeights = Stream.ReadUByte4N().ToArray()
            };
            vertex.Binormal = RealVector3d.CrossProduct(vertex.Normal, vertex.Tangent.IJK);
            return vertex;
        }

        public StaticPerPixelData ReadStaticPerPixelData()
        {
            return new StaticPerPixelData
            {
                Texcoord = Stream.ReadUShort2N()
            };
        }

        public StaticPerVertexColorData ReadStaticPerVertexColorData()
        {
            return new StaticPerVertexColorData
            {
                Color = Stream.ReadFloat3(),
            };
        }

        public StaticPerVertexData ReadStaticPerVertexData()
        {
            return new StaticPerVertexData
            {
                Color1 = Stream.ReadColor(),
                Color2 = Stream.ReadColor(),
                Color3 = Stream.ReadColor(),
                Color4 = Stream.ReadColor(),
                Color5 = Stream.ReadColor()
            };
        }

        public TinyPositionVertex ReadTinyPositionVertex()
        {
            return new TinyPositionVertex
            {   
                Position = Stream.ReadShort4N().IJK,
                // TODO: check the W component
            };
        }

        public TransparentVertex ReadTransparentVertex()
        {
            return new TransparentVertex
            {
                Position = Stream.ReadFloat3(),
                Texcoord = Stream.ReadFloat2(),
                Color = Stream.ReadColor()
            };
        }

        public WaterVertex ReadWaterVertex()
        {
            throw new NotImplementedException();
        }

        public WorldVertex ReadWorldVertex()
        {
            var vertex = new WorldVertex
            {
                Position = Stream.ReadFloat4(),
                Texcoord = Stream.ReadFloat16_2(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
            };
            vertex.Binormal = RealVector3d.CrossProduct(vertex.Normal, vertex.Tangent.IJK);
            return vertex;
        }

        public void WriteAmbientPrtData(AmbientPrtData v)
        {
            throw new NotImplementedException();
        }

        public void WriteBeamVertex(BeamVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteChudVertexFancy(ChudVertexFancy v)
        {
            throw new NotImplementedException();
        }

        public void WriteChudVertexSimple(ChudVertexSimple v)
        {
            throw new NotImplementedException();
        }

        public void WriteContrailVertex(ContrailVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteDebugVertex(DebugVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteDecoratorVertex(DecoratorVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteDualQuatVertex(DualQuatVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteFlatRigidVertex(FlatRigidVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteFlatSkinnedVertex(FlatSkinnedVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteFlatWorldVertex(FlatWorldVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteImplicitVertex(ImplicitVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteLightVolumeVertex(LightVolumeVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteLinearPrtData(LinearPrtData v)
        {
            throw new NotImplementedException();
        }

        public void WriteParticleModelVertex(ParticleModelVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteParticleVertex(ParticleVertex v)
        {
            throw new NotImplementedException();
        }

        public void WritePatchyFogVertex(PatchyFogVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteQuadraticPrtData(QuadraticPrtData v)
        {
            throw new NotImplementedException();
        }

        public void WriteRigidVertex(RigidVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteRippleVertex(RippleVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteScreenVertex(ScreenVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteSkinnedVertex(SkinnedVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteStaticPerPixelData(StaticPerPixelData v)
        {
            throw new NotImplementedException();
        }

        public void WriteStaticPerVertexColorData(StaticPerVertexColorData v)
        {
            throw new NotImplementedException();
        }

        public void WriteStaticPerVertexData(StaticPerVertexData v)
        {
            throw new NotImplementedException();
        }

        public void WriteTinyPositionVertex(TinyPositionVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteTransparentVertex(TransparentVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteWaterVertex(WaterVertex v)
        {
            throw new NotImplementedException();
        }

        public void WriteWorldVertex(WorldVertex v)
        {
            throw new NotImplementedException();
        }

        public Unknown1A ReadUnknown1A()
        {
            var buffer = Stream.ReadUShort6();
            ushort[] vertices = new ushort[3];
            ushort[] indices = new ushort[3];

            for(int i = 0; i<3; i++)
            {
                vertices[i] = buffer[2 * i];
                indices[i] = buffer[2 * i + 1];
            }
            return new Unknown1A
            {
                Vertices = vertices,
                Indices = indices
                
            };
        }

        public void WriteUnknown1A(Unknown1A v)
        {
            throw new NotImplementedException();
        }

        public Unknown1B ReadUnknown1B()
        {
            return new Unknown1B
            {
                Unknown1 = Stream.ReadUShortN(),
                Unknown2 = Stream.ReadUShortN(),
            };
        }

        public void WriteUnknown1B(Unknown1B v)
        {
            throw new NotImplementedException();
        }

        public void WriteWorldWaterVertex(WorldWaterVertex v)
        {
            throw new NotImplementedException();
        }

        public WorldWaterVertex ReadWorldWaterVertex()
        {
            throw new NotImplementedException();
        }

        public int GetVertexSize(VertexBufferFormat type)
        {
            switch (type)
            {
                case VertexBufferFormat.World:
                    return 0x1C;
                case VertexBufferFormat.Rigid:
                    return 0x14;
                case VertexBufferFormat.Skinned:
                    return 0x1C;

                case VertexBufferFormat.RigidCompressed:
                    return 0x10;
                case VertexBufferFormat.SkinnedCompressed:
                    return 0x18;

                case VertexBufferFormat.Decorator:
                    return 0x10;

                case VertexBufferFormat.ParticleModel:
                    return 0x10;
                
                case VertexBufferFormat.TinyPosition:
                    return 0x8;

                case VertexBufferFormat.StaticPerVertexColor:
                    return 0xC;

                default:
                case VertexBufferFormat.AmbientPrt:
                case VertexBufferFormat.LinearPrt:
                case VertexBufferFormat.QuadraticPrt:
                case VertexBufferFormat.World2:
                case VertexBufferFormat.StaticPerPixel:
                case VertexBufferFormat.StaticPerVertex:
                
                case VertexBufferFormat.Unknown1A:
                case VertexBufferFormat.Unknown1B:
                    return -1;
            }
        }
    }
}
