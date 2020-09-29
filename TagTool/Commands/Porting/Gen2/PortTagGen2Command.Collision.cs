using System.Collections.Generic;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry.BspCollisionGeometry;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using CollisionModelGen2 = TagTool.Tags.Definitions.Gen2.CollisionModel;

namespace TagTool.Commands.Porting.Gen2
{
	partial class PortTagGen2Command : Command
	{
		public CollisionModel ConvertCollisionModel(CachedTag tag, CollisionModelGen2 gen2CollisionModel)
		{
			var collisionModel = new CollisionModel()
			{
				Flags = (CollisionModelFlags)gen2CollisionModel.Flags,
				Regions = new List<CollisionModel.Region>(),
				Materials = new List<CollisionModel.Material>(),
				Nodes = new List<CollisionModel.Node>(),
				PathfindingSpheres = new List<CollisionModel.PathfindingSphere>()
			};

			// Convert Materials
			foreach (var gen2Material in gen2CollisionModel.Materials)
				collisionModel.Materials.Add(new CollisionModel.Material() { Name = gen2Material.Name });

			// Convert Regions
			foreach (var gen2Region in gen2CollisionModel.Regions)
			{
				var region = new CollisionModel.Region()
				{
					Name = gen2Region.Name,
					Permutations = new List<CollisionModel.Region.Permutation>()
				};
				collisionModel.Regions.Add(region);

				// Convert Permutations
				foreach (var gen2Permutation in gen2Region.Permutations)
				{
					var permutation = new CollisionModel.Region.Permutation()
					{
						Name = gen2Permutation.Name,
						BspMoppCodes = new List<Havok.TagHkpMoppCode>(),
						BspPhysics = new List<CollisionBspPhysicsDefinition>(),
						Bsps = new List<CollisionModel.Region.Permutation.Bsp>()
					};
					region.Permutations.Add(permutation);

					// Convert Bsp Physics
					for (int i = 0; i < gen2Permutation.BspPhysics.Count; i++)
					{
						permutation.BspMoppCodes.Add(ConvertTagMoppCode(gen2Permutation.BspPhysics[i].MoppCodes));
						permutation.BspPhysics.Add(ConvertCollisionBspPhysics(tag, gen2Permutation.BspPhysics[0]));
					}

					// Convert Bsps
					foreach (var gen2Bsp in gen2Permutation.Bsps)
					{
						permutation.Bsps.Add(new CollisionModel.Region.Permutation.Bsp()
						{
							Geometry = ConvertCollisionGeometry(gen2Bsp.Geometry),
							NodeIndex = gen2Bsp.NodeIndex,
							Unused = gen2Bsp.Unused
						});
					}
				}
			}

			// Convert Nodes
			foreach (var gen2Node in gen2CollisionModel.Nodes)
			{
				collisionModel.Nodes.Add(new CollisionModel.Node()
				{
					Name = gen2Node.Name,
					Flags = (CollisionModel.NodeFlags)gen2Node.Flags,
					FirstChildNode = gen2Node.FirstChildNode,
					NextSiblingNode = gen2Node.NextSiblingNode,
					ParentNode = gen2Node.ParentNode
				});
			}

			// Convert Path Finding Spheres
			foreach (var gen2PathFindingSphere in gen2CollisionModel.PathfindingSpheres)
			{
				collisionModel.PathfindingSpheres.Add(new CollisionModel.PathfindingSphere
				{
					Node = gen2PathFindingSphere.Node,
					Flags = (CollisionModel.PathfindingSphereFlags)gen2PathFindingSphere.Flags,
					Center = gen2PathFindingSphere.Center,
					Radius = gen2PathFindingSphere.Radius
				});
			}

			return collisionModel;
		}

		public CollisionGeometry ConvertCollisionGeometry(CollisionGeometry collisionGeometry)
		{
			// Convert Leaves
			foreach (var leaf in collisionGeometry.Leaves)
			{
				leaf.Flags2 = leaf.Flags;
				leaf.FirstBsp2dReference = (ushort)leaf.FirstBsp2dReference_H2;
				leaf.Bsp2dReferenceCount = (ushort)leaf.Bsp2dReferenceCount_H2;
			}

			// Convert Surfaces
			foreach (var surface in collisionGeometry.Surfaces)
			{
				surface.MaterialIndex = surface.MaterialIndex_H2;
				surface.BreakableSurfaceIndex = surface.BreakableSurfaceIndex_H2;
				surface.Flags = surface.Flags_H2;
				surface.Unknown = -1;
			}

			return collisionGeometry;
		}

		public CollisionBspPhysicsDefinition ConvertCollisionBspPhysics(CachedTag tag, CollisionBspPhysicsDefinitionGen2 gen2BspPhysics)
		{
			var bspPhysics = new CollisionBspPhysicsDefinition()
			{
				GeometryShape = new CollisionGeometryShape()
				{
					AABB_Center = gen2BspPhysics.GeometryShape.AABB_Center,
					AABB_Half_Extents = gen2BspPhysics.GeometryShape.AABB_Half_Extents,
					Model = gen2BspPhysics.GeometryShape.Model,
					BspIndex = -1,
					CollisionGeometryShapeKey = 0xffff,
					Type = 2
				},
				MoppBvTreeShape = new Havok.CMoppBvTreeShape()
				{
					Type = 27
				}
			};

			return bspPhysics;
		}

		public Havok.TagHkpMoppCode ConvertTagMoppCode(List<byte> moppCodes)
		{
			return new Havok.TagHkpMoppCode()
			{
				Info = new Havok.CodeInfo { Offset = new RealQuaternion(0, 0, 0, 0) },
				ArrayBase = new Havok.HkArrayBase()
				{
					Size = (uint)moppCodes.Count,
					CapacityAndFlags = (uint)(moppCodes.Count | 0x80000000)
				},
				Data = new TagBlock<byte>(CacheAddressType.Definition, ConvertMoppCodes(moppCodes))
			};
		}

		public List<byte> ConvertMoppCodes(List<byte> moppCodes)
		{
			// TODO
			return moppCodes;
		}
	}
}
