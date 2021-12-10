using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.Tags;

namespace TagTool.Geometry.BspCollisionGeometry.Utils
{
    class SupernodeToNodeConverter
    {
		public LargeCollisionBspBlock Bsp { get; set; }
		public LargeCollisionBspBlock Convert(LargeCollisionBspBlock bsp)
		{
			Bsp = bsp;
			TagBlock<LargeBsp3dNode> nodelist = new TagBlock<LargeBsp3dNode>();
			buildsupernode(0, 0, nodelist);
			Bsp.Bsp3dNodes = nodelist;
            prune_node_tree();
			return Bsp;
		}

		public int buildsupernode(int supernode_index, int index, TagBlock<LargeBsp3dNode> nodelist)
        {
			if(index > 14) //is a child
            {
				int child = Bsp.Bsp3dSupernodes[supernode_index].ChildIndices[index - 15];
                if (child == 0)
                    return -1;
                else if ((child & 0xC0000000) != 0)
                    return buildnode((int)(child & 0xBFFFFFFF), nodelist);
                else
                    return buildsupernode(child, 0, nodelist);
			}
			LargeBsp3dNode newnode = new LargeBsp3dNode();
			Plane newplane = generate_new_node_plane(Bsp.Bsp3dSupernodes[supernode_index], index);
			Bsp.Planes.Add(newplane);
			newnode.Plane = Bsp.Planes.Count - 1;
			nodelist.Add(newnode);
			int newnode_index = nodelist.Count - 1;
            nodelist[newnode_index].BackChild = buildsupernode(supernode_index, 2 * index + 2, nodelist);
            nodelist[newnode_index].FrontChild = buildsupernode(supernode_index, 2 * index + 1, nodelist);
            if (nodelist[newnode_index].BackChild == -1)
                return nodelist[newnode_index].FrontChild;
            if (nodelist[newnode_index].FrontChild == -1)
                return nodelist[newnode_index].BackChild;

            return newnode_index;
		}

		public int buildnode(int index, TagBlock<LargeBsp3dNode> nodelist)
        {
			//make sure this isn't actually a leaf
			if (BSP_TEST_FLAG(index))
				return index;
			var node = Bsp.Bsp3dNodes[index].DeepClone();
            nodelist.Add(node);
            int newnode_index = nodelist.Count - 1;
            if (!BSP_TEST_FLAG(node.FrontChild))
				nodelist[newnode_index].FrontChild = buildnode(node.FrontChild, nodelist);
			if (!BSP_TEST_FLAG(node.BackChild))
                nodelist[newnode_index].BackChild = buildnode(node.BackChild, nodelist);
			return newnode_index;			
		}

		public bool BSP_TEST_FLAG(int index)
        {
			if (((index >> 31) & 1) == 1)
				return true;
			return false;
        }

		public Plane generate_new_node_plane(Bsp3dSupernode supernode, int plane_index)
		{
			if(plane_index > 14)
				new TagToolError(CommandError.OperationFailed, "Plane index cannot exceed 14!");
			int axis = (supernode.PlaneDimensions >> (2 * plane_index)) & 3;
			if (axis > 2)
				new TagToolError(CommandError.OperationFailed, "Node plane axis cannot exceed 2!");
			Plane plane = new Plane();
			RealPlane3d planevalue = new RealPlane3d();
			switch (axis)
			{
				case 0:
					planevalue.I = 1.0f;
					planevalue.D = (float)supernode.PlaneValues[plane_index];
					break;
				case 1:
					planevalue.J = 1.0f;
					planevalue.D = (float)supernode.PlaneValues[plane_index];
					break;
				case 2:
					planevalue.K = 1.0f;
					planevalue.D = (float)supernode.PlaneValues[plane_index];
					break;
			}
			plane.Value = planevalue;
			return plane;
		}

        public bool prune_node_tree()
        {
            //first collapse the node children, shifting -1 up through any unnecessary nodes
            List<int> valid_node_array = new List<int>();
            TagBlock<LargeBsp3dNode> Nodelist = Bsp.Bsp3dNodes.DeepClone();
            Nodelist[0].FrontChild = collapse_node_children(Nodelist, Nodelist[0].FrontChild);
            Nodelist[0].BackChild = collapse_node_children(Nodelist, Nodelist[0].BackChild);

            //this function now does an inline replacement of the nodes block, removing nodes that only have -1 as children
            int node_count = 0;
            int pruned_nodes_count = 0;
            foreach (var node in Nodelist)
            {
                if (node.FrontChild != -1 || node.BackChild != -1)
                {
                    valid_node_array.Add(node_count++);
                }
                else
                {
                    valid_node_array.Add(-1);
                    pruned_nodes_count++;
                }
            }

            //adjust node children to match new node list
            for (var i = 0; i < Nodelist.Count; i++)
            {
                if ((Nodelist[i].FrontChild & 0x80000000) == 0) //child is another node, not a leaf or -1
                {
                    Nodelist[i].FrontChild = valid_node_array[Nodelist[i].FrontChild];
                }
                if ((Nodelist[i].BackChild & 0x80000000) == 0) //child is another node, not a leaf or -1
                {
                    Nodelist[i].BackChild = valid_node_array[Nodelist[i].BackChild];
                }
            }

            //now complete the inline replacement of the node list
            for (var n = 0; n < Nodelist.Count; n++)
            {
                if (valid_node_array[n] > n)
                {
                    Console.WriteLine("###ERROR: node_table[node_index]>node_index");
                    return false;
                }

                if (valid_node_array[n] != -1)
                {
                    Nodelist[valid_node_array[n]] = Nodelist[n].DeepClone();
                }
            }

            //remove extra nodes from the end of the tree
            while (Nodelist.Count > node_count)
                Nodelist.RemoveAt(Nodelist.Count - 1);

            //write nodes out to main BSP
            Bsp.Bsp3dNodes = Nodelist.DeepClone();
            return true;
        }

        public int collapse_node_children(TagBlock<LargeBsp3dNode> Nodelist, int node_index)
        {
            int absolute_node_index = node_index & 0x7FFFFFFF;

            if ((node_index & 0x80000000) != 0) //if this child is a leaf or -1, just return it
                return node_index;

            //call this function again for both children of this node
            int front_child_node_index = collapse_node_children(Nodelist, Nodelist[absolute_node_index].FrontChild);
            int back_child_node_index = collapse_node_children(Nodelist, Nodelist[absolute_node_index].BackChild);
            Nodelist[absolute_node_index].FrontChild = front_child_node_index;
            Nodelist[absolute_node_index].BackChild = back_child_node_index;

            //if either of this node's children are not -1, return this node index, otherwise return -1
            if (front_child_node_index != -1 || back_child_node_index != -1)
                return node_index;
            return back_child_node_index;
        }
    }
}
