using System;

namespace TagTool.Geometry
{
    /// <summary>
    /// Mesh flags.
    /// </summary>
    [Flags]
    public enum MeshFlags : byte
    {
        None = 0,

        /// <summary>
        /// Indicates that the mesh has vertex colors instead of PRT data.
        /// </summary>
        MeshHasVertexColor = 1 << 0,

        UseRegionIndexForSorting = 1 << 1,
        CanBeRenderedInDrawBundles = 1 << 2,
        MeshIsCustomShadowCaster = 1 << 3,
        MeshIsUnindexed = 1 << 4,
        MashShouldRenderInZPrepass = 1 << 5,
        MeshHasWater = 1 << 6,
        MeshHasDecal = 1 << 7
    }
}