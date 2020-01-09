using System;

namespace TagTool.Geometry
{
    [Flags]
    public enum RenderGeometryRuntimeFlags : int
    {
        None = 0,
        Processed = 1 << 0,
        Available = 1 << 1,
        HasValidBudgets = 1 << 2,
        ManualResourceCalibration = 1 << 3,
        KeepRawGeometry = 1 << 4,
        DoNotUseCompressedVertexPositions = 1 << 5,
        PcaAnimationTableSorted = 1 << 6,
        HasDualQuat = 1 << 7
    }
}