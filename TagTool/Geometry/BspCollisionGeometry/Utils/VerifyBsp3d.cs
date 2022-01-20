using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;

namespace TagTool.Geometry.BspCollisionGeometry.Utils
{
    class VerifyBsp3d
    {
		private List<leaf> Leaves = new List<leaf>();
		public LargeCollisionBspBlock Bsp { get; set; }
		public LargeCollisionBspBlock OldBsp { get; set; }
		public LargeCollisionBSPBuilder Bsp_Builder { get; set; }
		public class leaf
		{
			public int immediate_parent_3dnode;
			public List<int> node_stack = new List<int>();
			public List<int> surface_indices = new List<int>();
		};
		public bool verify_bsp3d_points()
        {
			bool result = true;
			foreach(var vertex in Bsp.Vertices)
            {
				List<int> oldnodestack = new List<int>();
				List<int> newnodestack = new List<int>();
				int leaf_result_old = supernode_test_point(vertex.Point, OldBsp, ref oldnodestack);
				int leaf_result_new = node_test_point(0, vertex.Point, Bsp, ref newnodestack);
				if(leaf_result_old != leaf_result_new)
                {
					result = false;
                }
            }
			return result;
        }

		public bool verify_bsp3d_points_random()
        {
			bool result = true;
			Random rng = new Random();
			float[,] bsp_bounds = bsp_get_bounds();
			for(var i = 0; i < 10000; i++)
            {
				RealPoint3d test_point = new RealPoint3d
				{
					X = (float)rng.NextDouble() * (bsp_bounds[0, 1] - bsp_bounds[0, 0]) + bsp_bounds[0, 0],
					Y = (float)rng.NextDouble() * (bsp_bounds[1, 1] - bsp_bounds[1, 0]) + bsp_bounds[1, 0],
					Z = (float)rng.NextDouble() * (bsp_bounds[2, 1] - bsp_bounds[2, 0]) + bsp_bounds[2, 0]
				};
				List<int> oldnodestack = new List<int>();
				List<int> newnodestack = new List<int>();
				int leaf_result_old = supernode_test_point(test_point, OldBsp, ref oldnodestack);
				int leaf_result_new = node_test_point(0, test_point, Bsp, ref newnodestack);
				if (leaf_result_old != leaf_result_new)
				{
					result = false;
				}
			}
			return result;
        }

		public float[,] bsp_get_bounds()
        {
			float[,] result = new float[3,2];
			foreach(var vertex in Bsp.Vertices)
            {
				if (vertex.Point.X < result[0, 0])
					result[0, 0] = vertex.Point.X;
				if (vertex.Point.X > result[0, 1])
					result[0, 1] = vertex.Point.X;

				if (vertex.Point.Y < result[1, 0])
					result[1, 0] = vertex.Point.Y;
				if (vertex.Point.Y > result[1, 1])
					result[1, 1] = vertex.Point.Y;

				if (vertex.Point.Z < result[2, 0])
					result[2, 0] = vertex.Point.Z;
				if (vertex.Point.Z > result[2, 1])
					result[2, 1] = vertex.Point.Z;
			}
			return result;
        }

		public bool verify_bsp3d()
		{
			Bsp_Builder = new LargeCollisionBSPBuilder();
			Bsp_Builder.Bsp = Bsp;
			for (var i = 0; i < Bsp.Bsp3dNodes.Count + 1; i++)
				Leaves.Add(new leaf());
			populate_leaves(new List<int>(), 0, -1);
			LeafMap leafmapper = new LeafMap();
			leafmapper.Bsp_Builder = Bsp_Builder;
			leafmapper.Bsp = Bsp;
			LeafMap.leafy_bsp leafybsp = new LeafMap.leafy_bsp();
			if (!leafmapper.setup_floating_leaves(ref leafybsp) ||
					!leafmapper.setup_leafy_bsp(ref leafybsp))
				return false;

			bool result = true;
			for(int i = 0; i < Bsp.Leaves.Count; i++)
            {
				/*
				if(Leaves[i].surface_indices.Count != leafybsp.leaves[i].polygon_counts[0, 2])
                {
					Console.WriteLine($"Leaf Index {i} Mismatch!");
					continue;
				}
				*/
				int missing_count = 0;
				foreach (var surface_index in Leaves[i].surface_indices)
                {
					int matching_index = leafybsp.leaves[i].polygons2.FindIndex(poly => poly.surface_index == surface_index);
					if (matching_index == -1 || leafybsp.leaves[i].polygons2[matching_index].polygon_type != 2)
					{
						missing_count++;
					}
				}
				if (missing_count > 0)
				{
					Console.WriteLine($"Leaf Index {i} missing {missing_count}/{Leaves[i].surface_indices.Count}!");
					Console.Write("Node Stack:");
					foreach (var node_index in Leaves[i].node_stack)
						Console.Write($"{node_index},");
					Console.WriteLine();
					result = false;
				}
			}
			if (result == false)
				return false;
			return result;
		}
		public bool populate_leaves(List<int> plane_designators, int bsp3dnode_index, int parent_bsp3dnode_index)
		{
			if (bsp3dnode_index < 0)
			{
				if (bsp3dnode_index == -1)
					return true;
				leaf current_leaf = Leaves[bsp3dnode_index & 0x7FFFFFFF];
				current_leaf.immediate_parent_3dnode = parent_bsp3dnode_index;
				current_leaf.node_stack = plane_designators.DeepClone();
				current_leaf.surface_indices = leaf_collect_surfaces(bsp3dnode_index & 0x7FFFFFFF);
				return true;
			}
			LargeBsp3dNode current_node = Bsp.Bsp3dNodes[bsp3dnode_index];
			for (var i = 0; i < 2; i++)
			{
				int node_child = i == 1 ? current_node.FrontChild : current_node.BackChild;
				plane_designators.Add(bsp3dnode_index);
				if (!populate_leaves(plane_designators, node_child, bsp3dnode_index))
					break;
				plane_designators.RemoveAt(plane_designators.Count - 1);
			}
			return true;
		}

		public List<int> leaf_collect_surfaces(int leaf_index)
		{
			List<int> surface_indices = new List<int>();
			Leaf current_leaf = Bsp.Leaves[leaf_index];
			for(var i = current_leaf.FirstBsp2dReference; 
				i < current_leaf.FirstBsp2dReference + current_leaf.Bsp2dReferenceCount; i++)
            {
				if ((int)i == -1 || current_leaf.Bsp2dReferenceCount == 0)
					break;
				LargeBsp2dReference ref2d = Bsp.Bsp2dReferences[(int)i];
				if((int)(ref2d.Bsp2dNodeIndex & 0x80000000) != 0)
                {
					surface_indices.Add(ref2d.Bsp2dNodeIndex & 0x7FFFFFFF);
					continue;
                }
				surface_indices = traverse_bsp2dnodes(ref2d.Bsp2dNodeIndex, surface_indices);
            }
			return surface_indices;
		}
		public List<int> traverse_bsp2dnodes(int bsp2dnode_index, List<int> surface_indices)
        {
			LargeBsp2dNode node2d = Bsp.Bsp2dNodes[bsp2dnode_index];
			if ((node2d.LeftChild & 0x80000000) == 0)
				surface_indices = traverse_bsp2dnodes(node2d.LeftChild, surface_indices);
			else if(node2d.LeftChild != -1)
				surface_indices.Add(node2d.LeftChild & 0x7FFFFFFF);

			if ((node2d.RightChild & 0x80000000) == 0)
				surface_indices = traverse_bsp2dnodes(node2d.RightChild, surface_indices);
			else if (node2d.RightChild != -1)
				surface_indices.Add(node2d.RightChild & 0x7FFFFFFF);

			return surface_indices;
		}

		public int supernode_test_point(RealPoint3d point, LargeCollisionBspBlock testbsp, ref List<int> nodestack)
        {
			int node_index = 0;
			float[] point_elements = new float[] {point.X, point.Y, point.Z };
			while ((node_index & 0xC0000000) == 0)
			{
				Bsp3dSupernode supernode = testbsp.Bsp3dSupernodes[node_index];
				int child_index = 0;
				for (var i = 0; i < 4; i++)
				{
					int axis = (supernode.PlaneDimensions >> (2 * child_index)) & 3;
					float test_value = point_elements[axis];
					int v6 = test_value - supernode.PlaneValues[child_index] >= 0 ? 1 : 0;
					child_index = 2 * child_index + v6 + 1;
				}
				node_index = supernode.ChildIndices[child_index - 15];
			}
			if (node_index >= 0)
				return node_test_point((int)(node_index & 0xBFFFFFFF), point, testbsp, ref nodestack);
			if (node_index == -1)
				return -1;
			else
				return node_index & 0x7FFFFFFF;
		}

		public int node_test_point(int node_index, RealPoint3d point, LargeCollisionBspBlock testbsp, ref List<int> nodestack)
		{
			while ((node_index & 0x80000000) == 0)
            {
				nodestack.Add(node_index);
				LargeBsp3dNode node = testbsp.Bsp3dNodes[node_index];
				RealPlane3d node_plane = testbsp.Planes[node.Plane].Value;
				if (point.X * node_plane.I + point.Y * node_plane.J + point.Z * node_plane.K - node_plane.D >= 0)
					node_index = node.FrontChild;
				else
					node_index = node.BackChild;
            }
			if (node_index == -1)
				return -1;
			else
				return node_index & 0x7FFFFFFF;
		}

	}
}
