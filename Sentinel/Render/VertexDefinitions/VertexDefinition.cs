using Microsoft.DirectX.Direct3D;
using System;
using System.Collections.Generic;

namespace Sentinel.Render.VertexDefinitions
{
    public abstract class VertexDefinition
    {
        public abstract VertexDeclaration GetDeclaration(Device device);
        public abstract Dictionary<int, Type> GetStreamTypes();

        public virtual VertexFormats GetStreamFormat(int streamIndex) =>
            throw new IndexOutOfRangeException(streamIndex.ToString());

        public static VertexDefinition Get(TagTool.Geometry.VertexType vertexType) =>
            Activator.CreateInstance(VertexTypes[vertexType]) as VertexDefinition;

        public static VertexDefinition Get(TagTool.Geometry.PrtType prtType) =>
            Activator.CreateInstance(PrtTypes[prtType]) as VertexDefinition;

        public static Dictionary<TagTool.Geometry.VertexType, Type> VertexTypes { get; } = new Dictionary<TagTool.Geometry.VertexType, Type>
        {
            [TagTool.Geometry.VertexType.World] = typeof(WorldVertexDefinition),
            [TagTool.Geometry.VertexType.Rigid] = typeof(RigidVertexDefinition),
            [TagTool.Geometry.VertexType.Skinned] = typeof(SkinnedVertexDefinition),
            [TagTool.Geometry.VertexType.ParticleModel] = typeof(ParticleModelVertexDefinition),
            [TagTool.Geometry.VertexType.FlatWorld] = typeof(FlatWorldVertexDefinition),
            [TagTool.Geometry.VertexType.FlatRigid] = typeof(FlatRigidVertexDefinition),
            [TagTool.Geometry.VertexType.FlatSkinned] = typeof(FlatSkinnedVertexDefinition),
            [TagTool.Geometry.VertexType.Screen] = typeof(ScreenVertexDefinition),
            [TagTool.Geometry.VertexType.Debug] = typeof(DebugVertexDefinition),
            [TagTool.Geometry.VertexType.Transparent] = typeof(TransparentVertexDefinition),
            [TagTool.Geometry.VertexType.Particle] = typeof(ParticleVertexDefinition),
            [TagTool.Geometry.VertexType.Contrail] = typeof(ContrailVertexDefinition),
            [TagTool.Geometry.VertexType.LightVolume] = typeof(LightVolumeVertexDefinition),
            [TagTool.Geometry.VertexType.SimpleChud] = typeof(ChudVertexSimpleDefinition),
            [TagTool.Geometry.VertexType.FancyChud] = typeof(ChudVertexFancyDefinition),
            [TagTool.Geometry.VertexType.Decorator] = typeof(DecoratorVertexDefinition),
            [TagTool.Geometry.VertexType.TinyPosition] = typeof(TinyPositionVertexDefinition),
            [TagTool.Geometry.VertexType.PatchyFog] = typeof(PatchyFogVertexDefinition),
            [TagTool.Geometry.VertexType.Water] = typeof(WaterVertexDefinition),
            [TagTool.Geometry.VertexType.Ripple] = typeof(RippleVertexDefinition),
            [TagTool.Geometry.VertexType.Implicit] = typeof(ImplicitVertexDefinition),
            [TagTool.Geometry.VertexType.Beam] = typeof(BeamVertexDefinition),
            [TagTool.Geometry.VertexType.DualQuat] = typeof(DualQuatVertexDefinition)
        };

        public static Dictionary<TagTool.Geometry.PrtType, Type> PrtTypes { get; } = new Dictionary<TagTool.Geometry.PrtType, Type>
        {
            [TagTool.Geometry.PrtType.None] = typeof(StaticShDefinition),
            [TagTool.Geometry.PrtType.Ambient] = typeof(StaticPrtAmbientDefinition),
            [TagTool.Geometry.PrtType.Linear] = typeof(StaticPrtLinearDefinition),
            [TagTool.Geometry.PrtType.Quadratic] = typeof(StaticPrtQuadraticDefinition)
        };
    }
}