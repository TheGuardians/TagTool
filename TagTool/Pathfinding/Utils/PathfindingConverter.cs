using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Tags;

namespace TagTool.Pathfinding.Utils
{
    public static class PathfindingConverter
    {
        /// <summary>
        /// Converts gen3 pathfinding from the tag to the resource (H3 -> ODST)
        /// </summary>
        /// <param name="tagPathfinding"></param>
        /// <returns></returns>
        public static ResourcePathfinding CreateResourcePathfindingFromTag(TagPathfinding tagPathfinding)
        {
            var result = new ResourcePathfinding
            {
                Sectors = tagPathfinding.Sectors,
                Links = tagPathfinding.Links,
                References = tagPathfinding.References,
                Bsp2dNodes = tagPathfinding.Bsp2dNodes,
                Vertices = tagPathfinding.Vertices,
                PathfindingHints = tagPathfinding.PathfindingHints,
                InstancedGeometryReferences = tagPathfinding.InstancedGeometryReferences,
                StructureChecksum = tagPathfinding.StructureChecksum,
                GiantPathfinding = tagPathfinding.GiantPathfinding,
                Doors = tagPathfinding.Doors,
                // blocks that are in the definition
                ObjectReferences = tagPathfinding.ObjectReferences,
                Seams = tagPathfinding.Seams,
                JumpSeams = tagPathfinding.JumpSeams
            };

            // set address type to data or definition
            result.Sectors.AddressType = CacheAddressType.Data;
            result.Links.AddressType = CacheAddressType.Data;
            result.References.AddressType = CacheAddressType.Data;
            result.Bsp2dNodes.AddressType = CacheAddressType.Data;
            result.Vertices.AddressType = CacheAddressType.Data;
            result.PathfindingHints.AddressType = CacheAddressType.Data;
            result.InstancedGeometryReferences.AddressType = CacheAddressType.Data;
            result.GiantPathfinding.AddressType = CacheAddressType.Data;
            result.Doors.AddressType = CacheAddressType.Data;

            result.ObjectReferences.AddressType = CacheAddressType.Definition;
            result.Seams.AddressType = CacheAddressType.Definition;
            result.JumpSeams.AddressType = CacheAddressType.Definition;

            foreach (var reference in result.ObjectReferences)
            {
                reference.Bsps.AddressType = CacheAddressType.Definition;
                foreach (var bsp in reference.Bsps)
                    bsp.Bsp2dRefs.AddressType = CacheAddressType.Data;
            }
            foreach (var seam in result.Seams)
                seam.LinkIndices.AddressType = CacheAddressType.Data;
            foreach (var jumpSeam in result.JumpSeams)
                jumpSeam.JumpIndices.AddressType = CacheAddressType.Data;

            return result;
        }

    }
}
