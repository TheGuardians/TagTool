using System;

namespace TagTool.Geometry
{
    [Flags]
    public enum RenderGeometryCompressionFlags : ushort
    {
        None = 0,
        CompressedPosition = 1 << 0,
        CompressedTexcoord = 1 << 1,
        CompressionOptimized = 1 << 2
    }
}