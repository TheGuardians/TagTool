using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Commands.CollisionModels;
using TagTool.Commands.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags.Definitions;
using CollisionModelGen4 = TagTool.Tags.Definitions.Gen4.CollisionModel;

namespace TagTool.Commands.Porting.Gen4
{
    partial class PortTagGen4Command : Command
	{
		public CollisionModel ConvertCollisionModel( CachedTag tag, CollisionModelGen4 gen4CollisionModel)
		{
			bool containsSuperNodes = false;

			var collisionModelResource = Gen4Cache.ResourceCacheGen4.GetCollisionModelResourceGen4(gen4CollisionModel.RegionsResource);

			var collisionModel = new CollisionModel()
			{
				Flags = (CollisionModelFlags)gen4CollisionModel.Flags,
				Regions = new List<CollisionModel.Region>(),
				Materials = new List<CollisionModel.Material>(),
				Nodes = new List<CollisionModel.Node>(),
				PathfindingSpheres = new List<CollisionModel.PathfindingSphere>()
			};

			// Convert Materials
			foreach (var gen4Material in gen4CollisionModel.Materials)
				collisionModel.Materials.Add(new CollisionModel.Material() { Name = gen4Material.Name });

			
			// Convert Regions
			foreach (var gen4Region in gen4CollisionModel.Regions)
			{
				var region = new CollisionModel.Region()
				{
					Name = gen4Region.Name,
					Permutations = new List<CollisionModel.Region.Permutation>()
				};
				collisionModel.Regions.Add(region);

				// Convert Permutations
				foreach (var gen4Permutation in gen4Region.Permutations)
				{
					var permutation = new CollisionModel.Region.Permutation()
					{
						Name = gen4Permutation.Name,
						BspMoppCodes = new List<Havok.TagHkpMoppCode>(),
						BspPhysics = new List<CollisionBspPhysicsDefinition>(),
						Bsps = new List<CollisionModel.Region.Permutation.Bsp>()
					};
					region.Permutations.Add(permutation);


					// Convert Bsps
					for(int i = 0; i < gen4Permutation.ResourcebspCount; i++)
                    {
						var gen4Bsp = collisionModelResource.Bsps[gen4Permutation.ResourcebspOffset + i];
						if (gen4Bsp.Bsp.Bsp3dSupernodes.Count > 0)
                        {
							new TagToolWarning("Collision geometry contains super nodes.");
							containsSuperNodes = true;
						}

						permutation.Bsps.Add(new CollisionModel.Region.Permutation.Bsp()
						{
							Geometry = gen4Bsp.Bsp,
							NodeIndex = gen4Bsp.NodeIndex,
						});
					}
				}
			}

			// Convert Nodes
			foreach (var gen4Node in gen4CollisionModel.Nodes)
			{
				collisionModel.Nodes.Add(new CollisionModel.Node()
				{
					Name = gen4Node.Name,
					// Flags = (CollisionModel.NodeFlags)gen4Node.Flags,
					FirstChildNode = gen4Node.FirstChildNode,
					NextSiblingNode = gen4Node.NextSiblingNode,
					ParentNode = gen4Node.ParentNode
				});
			}

			// Convert Path Finding Spheres
			foreach (var gen4PathFindingSphere in gen4CollisionModel.PathfindingSpheres)
			{
				collisionModel.PathfindingSpheres.Add(new CollisionModel.PathfindingSphere
				{
					Node = gen4PathFindingSphere.Node,
					Flags = (CollisionModel.PathfindingSphereFlags)gen4PathFindingSphere.Flags,
					Center = gen4PathFindingSphere.Center,
					Radius = gen4PathFindingSphere.Radius
				});
			}

			if(containsSuperNodes)
            {
                GenerateCollisionBSPCommand bspgeneration = new GenerateCollisionBSPCommand(ref collisionModel);
                bspgeneration.Execute(new List<string>());
            }

            return collisionModel;
		}
	}
}
