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
                BlendWeight = Stream.ReadFloat8_1()
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
                Normal = new RealQuaternion(Stream.ReadDHenN3(), 1.0f)
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
                Normal = Stream.ReadDHenN3(),
                Tangent = new RealQuaternion(Stream.ReadDHenN3(), 1.0f),
                Binormal = Stream.ReadDHenN3()
            };
        }

        public FlatSkinnedVertex ReadFlatSkinnedVertex()
        {
            return new FlatSkinnedVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHenN3(),
                Tangent = new RealQuaternion(Stream.ReadDHenN3(), 1.0f),
                Binormal = Stream.ReadDHenN3(),
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
                Normal = Stream.ReadDHenN3(),
                Tangent = new RealQuaternion(Stream.ReadDHenN3(), 1.0f),
                Binormal = Stream.ReadDHenN3()
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
                BlendWeight = Stream.ReadSByte4N()
            };
        }

        public ParticleModelVertex ReadParticleModelVertex()
        {
            return new ParticleModelVertex
            {
                Position = new RealVector3d(Stream.ReadUShort4N()),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHenN3()
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
                BlendWeight = Stream.ReadDec3N(),
                BlendWeight2 = Stream.ReadDec3N(),
                BlendWeight3 = Stream.ReadDec3N()
            };
        }

        public RigidVertex ReadRigidVertex()
        {
            return new RigidVertex
            {
                Position = Stream.ReadUShort4N(),
                Texcoord = Stream.ReadUShort2N(),
                Normal = Stream.ReadDHenN3(),
                Tangent = new RealQuaternion(Stream.ReadDHenN3(), 1.0f),
                Binormal = Stream.ReadDHenN3()
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
                Normal = Stream.ReadDHenN3(),
                Tangent = new RealQuaternion(Stream.ReadDHenN3(), 1.0f),
                Binormal = Stream.ReadDHenN3(),
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
                
                Texcoord1 = Stream.ReadSByte4N(),
                Texcoord2 = Stream.ReadSByte4N(),
                Texcoord3 = Stream.ReadSByte4N(),
                Texcoord4 = Stream.ReadSByte4N(),
                Texcoord5 = Stream.ReadSByte4N()
            };
        }

        public TinyPositionVertex ReadTinyPositionVertex()
        {
            return new TinyPositionVertex
            {   
                Position = Stream.ReadUShort4N(),
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
                Normal = Stream.ReadDHenN3(),
                Tangent = new RealQuaternion(Stream.ReadDHenN3(), 1.0f),
                Binormal = Stream.ReadDHenN3()
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
            return new Unknown1A
            {
                Unknown = Stream.ReadColor(),
                Unknown1 = Stream.ReadColor(),
                Unknown2 = Stream.ReadColor(),
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
                Unknown = Stream.ReadFloat1(),
                Unknown1 = Stream.ReadFloat1(),
                Unknown2 = Stream.ReadFloat1(),
                Unknown3 = Stream.ReadFloat1(),
                Unknown4 = Stream.ReadFloat1(),
                Unknown5 = Stream.ReadFloat1(),
                Unknown6 = Stream.ReadFloat1(),
                Unknown7 = Stream.ReadFloat1(),
                Unknown8 = Stream.ReadFloat1(),
            };
        }

        public void WriteUnknown1B(Unknown1B v)
        {
            throw new NotImplementedException();
        }
    }
}
