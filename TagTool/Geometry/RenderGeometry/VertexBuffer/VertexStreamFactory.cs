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
        /// <param name="cachePlatform">the engine platform</param>
        /// <param name="stream">The base stream.</param>
        /// <returns>The created vertex stream.</returns>
        public static IVertexStream Create(CacheVersion version, CachePlatform cachePlatform, Stream stream)
        {
            if(cachePlatform == CachePlatform.MCC)
            {
                return new VertexStreamHalo3RetailMCC(stream);
            }
            else
            {
                switch (version)
                {
                    case CacheVersion.Halo3Beta:
                    case CacheVersion.Halo3Retail:
                    case CacheVersion.Halo3ODST:
                        return new VertexStreamXbox(stream);
                    case CacheVersion.HaloOnlineED:
                    case CacheVersion.HaloOnline106708:
                        return new VertexStreamMS23(stream);
                    case CacheVersion.HaloOnline235640:
                    case CacheVersion.HaloOnline301003:
                    case CacheVersion.HaloOnline327043:
                    case CacheVersion.HaloOnline372731:
                    case CacheVersion.HaloOnline416097:
                    case CacheVersion.HaloOnline430475:
                    case CacheVersion.HaloOnline449175:
                    case CacheVersion.HaloOnline454665:
                    case CacheVersion.HaloOnline498295:
                    case CacheVersion.HaloOnline530605:
                    case CacheVersion.HaloOnline532911:
                    case CacheVersion.HaloOnline554482:
                    case CacheVersion.HaloOnline571627:
                    case CacheVersion.HaloOnline700123:
                        return new VertexStreamMS25(stream);

                    case CacheVersion.HaloReach:
                    case CacheVersion.HaloReach11883:
                        return new VertexStreamReach(stream);

                    default:
                        return new VertexStreamMS23(stream);
                }

            }
            
        }
    }
}
