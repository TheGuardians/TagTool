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
			return Bsp;
		}

		public int buildsupernode(int supernode_index, int index, TagBlock<LargeBsp3dNode> nodelist)
        {
			if(index > 14) //is a child
            {
				int child = Bsp.Bsp3dSupernodes[supernode_index].ChildIndices[index - 15];
				if (child == -1)
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
			newnode.FrontChild = buildsupernode(supernode_index, 2 * index + 2, nodelist);
			newnode.BackChild = buildsupernode(supernode_index, 2 * index + 1, nodelist);
			nodelist.Add(newnode);
			return nodelist.Count - 1;
		}

		public int buildnode(int index, TagBlock<LargeBsp3dNode> nodelist)
        {
			//make sure this isn't actually a leaf
			if ((index & 0x80000000) > 0)
				return index;
			var node = Bsp.Bsp3dNodes[index].DeepClone();
			if((node.FrontChild & 0x80000000) == 0)
				node.FrontChild = buildnode(node.FrontChild, nodelist);
			if ((node.BackChild & 0x80000000) == 0)
				node.BackChild = buildnode(node.BackChild, nodelist);
			nodelist.Add(node);
			return nodelist.Count - 1;			
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
	}
}
