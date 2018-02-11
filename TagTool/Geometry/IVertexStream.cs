namespace TagTool.Geometry
{
    public interface IVertexStream
    {
        WorldVertex ReadWorldVertex();
        void WriteWorldVertex(WorldVertex v);
        RigidVertex ReadRigidVertex();
        void WriteRigidVertex(RigidVertex v);
        SkinnedVertex ReadSkinnedVertex();
        void WriteSkinnedVertex(SkinnedVertex v);
        ParticleModelVertex ReadParticleModelVertex();
        void WriteParticleModelVertex(ParticleModelVertex v);
        FlatWorldVertex ReadFlatWorldVertex();
        void WriteFlatWorldVertex(FlatWorldVertex v);
        FlatRigidVertex ReadFlatRigidVertex();
        void WriteFlatRigidVertex(FlatRigidVertex v);
        FlatSkinnedVertex ReadFlatSkinnedVertex();
        void WriteFlatSkinnedVertex(FlatSkinnedVertex v);
        ScreenVertex ReadScreenVertex();
        void WriteScreenVertex(ScreenVertex v);
        DebugVertex ReadDebugVertex();
        void WriteDebugVertex(DebugVertex v);
        TransparentVertex ReadTransparentVertex();
        void WriteTransparentVertex(TransparentVertex v);
        ParticleVertex ReadParticleVertex();
        void WriteParticleVertex(ParticleVertex v);
        ContrailVertex ReadContrailVertex();
        void WriteContrailVertex(ContrailVertex v);
        LightVolumeVertex ReadLightVolumeVertex();
        void WriteLightVolumeVertex(LightVolumeVertex v);
        ChudVertexSimple ReadChudVertexSimple();
        void WriteChudVertexSimple(ChudVertexSimple v);
        ChudVertexFancy ReadChudVertexFancy();
        void WriteChudVertexFancy(ChudVertexFancy v);
        DecoratorVertex ReadDecoratorVertex();
        void WriteDecoratorVertex(DecoratorVertex v);
        TinyPositionVertex ReadTinyPositionVertex();
        void WriteTinyPositionVertex(TinyPositionVertex v);
        PatchyFogVertex ReadPatchyFogVertex();
        void WritePatchyFogVertex(PatchyFogVertex v);
        WaterVertex ReadWaterVertex();
        void WriteWaterVertex(WaterVertex v);
        RippleVertex ReadRippleVertex();
        void WriteRippleVertex(RippleVertex v);
        ImplicitVertex ReadImplicitVertex();
        void WriteImplicitVertex(ImplicitVertex v);
        BeamVertex ReadBeamVertex();
        void WriteBeamVertex(BeamVertex v);
        DualQuatVertex ReadDualQuatVertex();
        void WriteDualQuatVertex(DualQuatVertex v);
        StaticPerVertexColorData ReadStaticPerVertexColorData();
        void WriteStaticPerVertexColorData(StaticPerVertexColorData v);
        StaticPerPixelData ReadStaticPerPixelData();
        void WriteStaticPerPixelData(StaticPerPixelData v);
        StaticPerVertexData ReadStaticPerVertexData();
        void WriteStaticPerVertexData(StaticPerVertexData v);
        AmbientPrtData ReadAmbientPrtData();
        void WriteAmbientPrtData(AmbientPrtData v);
        LinearPrtData ReadLinearPrtData();
        void WriteLinearPrtData(LinearPrtData v);
        QuadraticPrtData ReadQuadraticPrtData();
        void WriteQuadraticPrtData(QuadraticPrtData v);
        Unknown1A ReadUnknown1A();
        void WriteUnknown1A(Unknown1A v);
        Unknown1B ReadUnknown1B();
        void WriteUnknown1B(Unknown1B v);
    }
}
