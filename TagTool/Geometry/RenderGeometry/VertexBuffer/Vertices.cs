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
        public RealVector2d Position { get; set; }
        public RealVector2d Texcoord { get; set; }
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
        public ushort Variant { get; set; } // type index (high 8 bits), motion scale (low 8 bits)
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

    /// <summary>
    /// Each color is some form of RGBE, engine converts it to rgb. Color 4,5 seem unused
    /// </summary>
    public class StaticPerVertexData
    {
        public uint Color1 { get; set; }
        public uint Color2 { get; set; }
        public uint Color3 { get; set; }
        public uint Color4 { get; set; }
        public uint Color5 { get; set; }
    }

    public class AmbientPrtData
    {
        public float SHCoefficient { get; set; }
    }

    public class LinearPrtData
    {
        public RealQuaternion SHCoefficients { get; set; }
    }

    public class QuadraticPrtData
    {
        public RealVector3d SHCoefficients1 { get; set; }
        public RealVector3d SHCoefficients2 { get; set; }
        public RealVector3d SHCoefficients3 { get; set; }
    }

    public class WaterTriangleIndices
    {
        public ushort[] MeshIndices { get; set; }
        public ushort[] WaterIndices { get; set; }
    }

    public class WaterTesselatedParameters
    {
        public RealVector2d LocalInfo { get; set; }
        public float LocalInfoPadd;
        public RealVector2d BaseTex { get; set; }
        public float BaseTexPadd;
    }

    public class WorldWaterVertex : WorldVertex
    {
        public RealVector2d StaticPerPixel;
    }
}
