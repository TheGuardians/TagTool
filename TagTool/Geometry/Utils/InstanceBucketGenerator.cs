using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Geometry.Utils
{
    public class InstanceBucketGenerator
    {
        public static void Generate(ScenarioStructureBsp sbsp, StructureBspTagResources resources)
        {
            for (int i = 0; i < sbsp.Clusters.Count; i++)
            {
                var cluster = sbsp.Clusters[i];
                var clusterMesh = sbsp.Geometry.Meshes[cluster.MeshIndex];
                var clusterBounds = new RealRectangle3d(
                    cluster.BoundsX.Lower, cluster.BoundsX.Upper,
                    cluster.BoundsY.Lower, cluster.BoundsY.Upper,
                    cluster.BoundsZ.Lower, cluster.BoundsZ.Upper);

                clusterMesh.InstanceBuckets = new List<Mesh.InstancedBucketBlock>();
                for (int j = 0; j < sbsp.InstancedGeometryInstances.Count; j++)
                {
                    var instance = sbsp.InstancedGeometryInstances[j];
                    var defintion = resources.InstancedGeometry[instance.DefinitionIndex];

                    // probably would be better to do a more accurate intersection test, but this is fine for now
                    if (!MathHelper.SphereIntersectsRectangle3d(instance.WorldBoundingSphereCenter, instance.BoundingSphereRadiusBounds.Upper, clusterBounds))
                        continue;

                    var instanceBucket = clusterMesh.InstanceBuckets.FirstOrDefault(x => x.MeshIndex == defintion.MeshIndex && x.DefinitionIndex == instance.DefinitionIndex);
                    if (instanceBucket == null)
                    {
                        instanceBucket = new Mesh.InstancedBucketBlock();
                        instanceBucket.MeshIndex = defintion.MeshIndex;
                        instanceBucket.DefinitionIndex = instance.DefinitionIndex;
                        instanceBucket.Instances = new List<Mesh.InstancedBucketBlock.InstanceIndexBlock>();
                        clusterMesh.InstanceBuckets.Add(instanceBucket);
                    }

                    instanceBucket.Instances.Add(new Mesh.InstancedBucketBlock.InstanceIndexBlock() { InstanceIndex = (short)j });
                }
            }
        }
    }
}
