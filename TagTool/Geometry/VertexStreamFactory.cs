using TagTool.Cache;
using System.IO;

namespace TagTool.Geometry
{
    public static class VertexStreamFactory
    {
        /// <summary>
        /// Creates a vertex stream for a given engine version.
        /// </summary>
        /// <param name="version">The engine version.</param>
        /// <param name="stream">The base stream.</param>
        /// <returns>The created vertex stream.</returns>
        public static IVertexStream Create(CacheVersion version, Stream stream)
        {
            if (CacheVersionDetection.Compare(version, CacheVersion.Halo3ODST) <= 0)
                return new VertexStreamXbox(stream);
            if (CacheVersionDetection.Compare(version, CacheVersion.HaloOnline235640) >= 0)
                return new VertexStreamMS25(stream);
            return new VertexStreamMS23(stream);
        }
    }
}
