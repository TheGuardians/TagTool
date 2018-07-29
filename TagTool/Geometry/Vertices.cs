using TagTool.Common;

namespace TagTool.Geometry
{
    public class WorldVertex
    {
        public RealQuaternion Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public RealVector3d Normal { get; set; }
        public RealQuaternion Tangent { get; set; }
        public RealVector3d Binormal { get; set; }
    }

    public class RigidVertex
    {
        public RealQuaternion Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public RealVector3d Normal { get; set; }
        public RealQuaternion Tangent { get; set; }
        public RealVector3d Binormal { get; set; }
    }

    public class SkinnedVertex
    {
        public RealQuaternion Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public RealVector3d Normal { get; set; }
        public RealQuaternion Tangent { get; set; }
        public RealVector3d Binormal { get; set; }
        public byte[] BlendIndices { get; set; }
        public float[] BlendWeights { get; set; }
    }

    public class ParticleModelVertex
    {
        public RealVector3d Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public RealVector3d Normal { get; set; }
    }

    public class FlatWorldVertex
    {
        public RealQuaternion Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public RealVector3d Normal { get; set; }
        public RealQuaternion Tangent { get; set; }
        public RealVector3d Binormal { get; set; }
    }

    public class FlatRigidVertex
    {
        public RealQuaternion Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public RealVector3d Normal { get; set; }
        public RealQuaternion Tangent { get; set; }
        public RealVector3d Binormal { get; set; }
    }

    public class FlatSkinnedVertex
    {
        public RealQuaternion Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public RealVector3d Normal { get; set; }
        public RealQuaternion Tangent { get; set; }
        public RealVector3d Binormal { get; set; }
        public byte[] BlendIndices { get; set; }
        public float[] BlendWeights { get; set; }
    }

    public class ScreenVertex
    {
        public RealVector2d Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public uint Color { get; set; }
    }

    public class DebugVertex
    {
        public RealVector3d Position { get; set; }
        public uint Color { get; set; }
    }

    public class TransparentVertex
    {
        public RealVector3d Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public uint Color { get; set; }
    }

    public class ParticleVertex
    {
    }

    public class ContrailVertex
    {
        public RealQuaternion Position { get; set; }
        public RealQuaternion Position2 { get; set; }
        public RealQuaternion Position3 { get; set; }
        public RealQuaternion Texcoord { get; set; }
        public RealQuaternion Texcoord2 { get; set; }
        public RealVector2d Texcoord3 { get; set; }
        public uint Color { get; set; }
        public uint Color2 { get; set; }
        public RealQuaternion Position4 { get; set; }
    }

    public class LightVolumeVertex
    {
        public short[] Texcoord { get; set; }
    }

    public class ChudVertexSimple
    {
        public RealVector2d Position { get; set; }
        public RealVector2d Texcoord { get; set; }
    }

    public class ChudVertexFancy
    {
        public RealVector3d Position { get; set; }
        public uint Color { get; set; }
        public RealVector2d Texcoord { get; set; }
    }

    public class DecoratorVertex
    {
        public RealQuaternion Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public RealQuaternion Normal { get; set; }
    }

    public class TinyPositionVertex
    {
        public RealVector3d Position { get; set; }
        public ushort Variant { get; set; }
        public RealQuaternion Normal { get; set; }
        public uint Color { get; set; }
    }

    public class PatchyFogVertex
    {
        public RealQuaternion Position { get; set; }
        public RealVector2d Texcoord { get; set; }
    }

    public class WaterVertex
    {
        public RealQuaternion Position { get; set; }
        public RealQuaternion Position2 { get; set; }
        public RealQuaternion Position3 { get; set; }
        public RealQuaternion Position4 { get; set; }
        public RealQuaternion Position5 { get; set; }
        public RealQuaternion Position6 { get; set; }
        public RealQuaternion Position7 { get; set; }
        public RealQuaternion Position8 { get; set; }
        public RealQuaternion Texcoord { get; set; }
        public RealVector3d Texcoord2 { get; set; }
        public RealQuaternion Normal { get; set; }
        public RealQuaternion Normal2 { get; set; }
        public RealQuaternion Normal3 { get; set; }
        public RealQuaternion Normal4 { get; set; }
        public RealVector2d Normal5 { get; set; }
        public RealVector3d Texcoord3 { get; set; }
    }

    public class RippleVertex
    {
        public RealQuaternion Position { get; set; }
        public RealQuaternion Texcoord { get; set; }
        public RealQuaternion Texcoord2 { get; set; }
        public RealQuaternion Texcoord3 { get; set; }
        public RealQuaternion Texcoord4 { get; set; }
        public RealQuaternion Texcoord5 { get; set; }
        public RealQuaternion Color { get; set; }
        public RealQuaternion Color2 { get; set; }
        public short[] Texcoord6 { get; set; }
    }

    public class ImplicitVertex
    {
        public RealVector3d Position { get; set; }
        public RealVector2d Texcoord { get; set; }
    }

    public class BeamVertex
    {
        public RealQuaternion Position { get; set; }
        public RealQuaternion Texcoord { get; set; }
        public RealQuaternion Texcoord2 { get; set; }
        public uint Color { get; set; }
        public RealVector3d Position2 { get; set; }
        public short[] Texcoord3 { get; set; }
    }

    public class DualQuatVertex
    {
        public RealQuaternion Position { get; set; }
        public RealVector2d Texcoord { get; set; }
        public RealVector3d Normal { get; set; }
        public RealQuaternion Tangent { get; set; }
        public RealVector3d Binormal { get; set; }
        public byte[] BlendIndices { get; set; }
        public float[] BlendWeights { get; set; }
    }

    public class StaticPerVertexColorData
    {
        public RealVector3d Color { get; set; }
    }

    public class StaticPerPixelData
    {
        public RealVector2d Texcoord { get; set; }
    }

    public class StaticPerVertexData
    {
        public RealQuaternion Texcoord1 { get; set; }
        public RealQuaternion Texcoord2 { get; set; }
        public RealQuaternion Texcoord3 { get; set; }
        public RealQuaternion Texcoord4 { get; set; }
        public RealQuaternion Texcoord5 { get; set; }
    }

    public class AmbientPrtData
    {
        public float BlendWeight { get; set; }
    }

    public class LinearPrtData
    {
        public RealQuaternion BlendWeight { get; set; }
    }

    public class QuadraticPrtData
    {
        public RealVector3d BlendWeight { get; set; }
        public RealVector3d BlendWeight2 { get; set; }
        public RealVector3d BlendWeight3 { get; set; }
    }

    public class Unknown1A
    {
        public ushort[] Vertices{ get; set; }
        public ushort[] Indices { get; set; }
    }

    public class Unknown1B
    {
        public float Unknown1 { get; set; }
        public float Unknown2 { get; set; }
        public float Unknown3 { get; set; }
        public float Unknown4 { get; set; }
        public float Unknown5 { get; set; }
        public float Unknown6 { get; set; }
        public float Unknown7 { get; set; }
        public float Unknown8 { get; set; }
        public float Unknown9 { get; set; }
    }
}
