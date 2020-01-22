using TagTool.Common;
using TagTool.IO;
using System;
using System.IO;

namespace TagTool.Geometry
{
    public class VertexStreamXbox : IVertexStream
    {
        private VertexElementStream Stream { get; }

        public VertexStreamXbox(Stream stream)
        {
            Stream = new VertexElementStream(stream, EndianFormat.BigEndian);
        }

        public AmbientPrtData ReadAmbientPrtData()
        {
            return new AmbientPrtData
            {
                SHCoefficient = Stream.ReadFloat8_1()
            };
        }

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

        public FlatRigidVertex ReadFlatRigidVertex()
        {
            return new FlatRigidVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
                Binormal = Stream.ReadDHen3N()
            };
        }

        public FlatSkinnedVertex ReadFlatSkinnedVertex()
        {
            return new FlatSkinnedVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
                Binormal = Stream.ReadDHen3N(),
                BlendIndices = Stream.ReadUByte4(),
                BlendWeights = Stream.ReadUByte4N().ToArray()
            };
        }

        public FlatWorldVertex ReadFlatWorldVertex()
        {
            return new FlatWorldVertex
            {
                Position = new RealQuaternion(Stream.ReadFloat3(), 0.0f),
                Texcoord = Stream.ReadFloat2(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
                Binormal = Stream.ReadDHen3N()
            };
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

        public LinearPrtData ReadLinearPrtData()
        {
            return new LinearPrtData
            {
                SHCoefficients = Stream.ReadSByte4N()
            };
        }

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
            throw new NotImplementedException();
        }

        public PatchyFogVertex ReadPatchyFogVertex()
        {
            return new PatchyFogVertex
            {
                Position = Stream.ReadFloat4()
            };
        }

        public QuadraticPrtData ReadQuadraticPrtData()
        {
            return new QuadraticPrtData
            {
                SHCoefficients1 = Stream.ReadDec3N(),
                SHCoefficients2 = Stream.ReadDec3N(),
                SHCoefficients3 = Stream.ReadDec3N()
            };
        }

        public RigidVertex ReadRigidVertex()
        {
            return new RigidVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
                Binormal = Stream.ReadDHen3N()
            };
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
            return new SkinnedVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
                Binormal = Stream.ReadDHen3N(),
                BlendIndices = Stream.ReadUByte4(),
                BlendWeights = Stream.ReadUByte4N().ToArray()
            };
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
                Position = Stream.ReadUShort3N(),
                Variant = Stream.ReadUShort(),
                Normal = Stream.ReadSByte4N(),
                Color = Stream.ReadColor()
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
            return new WorldVertex
            {
                Position = new RealQuaternion(Stream.ReadFloat3(), 0.0f),
                Texcoord = Stream.ReadFloat2(),
                Normal = Stream.ReadDHen3N(),
                Tangent = new RealQuaternion(Stream.ReadDHen3N(), 1.0f),
                Binormal = Stream.ReadDHen3N()
            };
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
                Unknown1 = Stream.ReadFloat1(),
                Unknown2 = Stream.ReadFloat1(),
                Unknown3 = Stream.ReadFloat1(),
                Unknown4 = Stream.ReadFloat1(),
                Unknown5 = Stream.ReadFloat1(),
                Unknown6 = Stream.ReadFloat1(),
                Unknown7 = Stream.ReadFloat1(),
                Unknown8 = Stream.ReadFloat1(),
                Unknown9 = Stream.ReadFloat1(),
            };
        }

        public void WriteUnknown1B(Unknown1B v)
        {
            throw new NotImplementedException();
        }

        public void WriteWorldWaterVertex(WorldVertex v)
        {
            throw new NotImplementedException();
        }

        public WorldVertex ReadWorldWaterVertex()
        {
            throw new NotImplementedException();
        }

        public int GetVertexSize(VertexBufferFormat type)
        {
            switch (type)
            {
                case VertexBufferFormat.AmbientPrt:
                    return 0x4;
                case VertexBufferFormat.LinearPrt:
                    return 0x4;
                case VertexBufferFormat.QuadraticPrt:
                    return 0xC;
                case VertexBufferFormat.Decorator:
                    return 0x10;
                case VertexBufferFormat.ParticleModel:
                    return 0x10;
                case VertexBufferFormat.Rigid:
                    return 0x18;
                case VertexBufferFormat.Skinned:
                    return 0x20;
                case VertexBufferFormat.StaticPerPixel:
                    return 0x4;
                case VertexBufferFormat.StaticPerVertex:
                    return 0x14;
                case VertexBufferFormat.StaticPerVertexColor:
                    return 0xC;
                case VertexBufferFormat.TinyPosition:
                    return 0x10;
                case VertexBufferFormat.World:
                case VertexBufferFormat.World2:
                    return 0x20;
                case VertexBufferFormat.Unknown1A:
                    return 0xC;
                case VertexBufferFormat.Unknown1B:
                    return 0x24;
                default:
                    return -1;
            }
        }
    }
}
