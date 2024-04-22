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
        UseVertexBuffersForIndices = 1 << 2,
        MeshHasPerInstanceLighting = 1 << 3,
        MeshIsUnindexed = 1 << 4,
        MashShouldRenderInZPrepass = 1 << 5,
        MeshHasWater = 1 << 6,
        MeshHasDecal = 1 << 7
    }

    [Flags]
    public enum MeshFlagsReach : ushort
    {
        None = 0,

        MeshHasVertexColor = 1 << 0,
        UseRegionIndexForSorting = 1 << 1,
        UseVertexBuffersForIndices = 1 << 2,
        MeshHasPerInstanceLighting = 1 << 3,
        MeshIsUnindexed = 1 << 4,
        SubpartWereMerged = 1 << 5,
        MeshHasFur = 1 << 6,
        MeshHasDecal = 1 << 7,
        MeshDoesntUseCompressedPosition = 1 << 8,
        UseUncompressedVertexFormat = 1 << 9,
        MeshIsPca = 1 << 10,
        MeshCompressionDetermined = 1 << 11,
        MeshHasAuthoredLightmapTextureCoords = 1 << 12,
        MeshHasAUsefulSetOfSecondTextureCoords = 1 << 13,
        MeshHasNoLightmap = 1 << 14,
        PerVertexLighting = 1 << 15
    }
}