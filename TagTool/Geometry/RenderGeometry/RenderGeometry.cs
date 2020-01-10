using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Resources;

namespace TagTool.Geometry
{
    [TagStructure(Name = "render_geometry", Size = 0x84)]
    public class RenderGeometry : TagStructure
	{
        /// <summary>
        /// The runtime flags of the render geometry.
        /// </summary>
        public RenderGeometryRuntimeFlags RuntimeFlags;

        /// <summary>
        /// The meshes of the render geometry.
        /// </summary>
        public List<Mesh> Meshes;

        /// <summary>
        /// The compression information of the render geometry.
        /// </summary>
        public List<RenderGeometryCompression> Compression;

        /// <summary>
        /// The bounding spheres of the render geometry.
        /// </summary>
        public List<BoundingSphere> BoundingSpheres;

        public List<UnknownBlock> Unknown2;

        public uint Unknown3;
        public uint Unknown4;
        public uint Unknown5;

        public List<UnknownSection> UnknownSections;

        /// <summary>
        /// The per-mesh node mappings of the render geometry.
        /// </summary>
        public List<PerMeshNodeMap> PerMeshNodeMaps;

        /// <summary>
        /// The per-mesh subpart visibility of the render geometry.
        /// </summary>
        public List<PerMeshSubpartVisibilityBlock> PerMeshSubpartVisibility;

        public uint Unknown7;
        public uint Unknown8;
        public uint Unknown9;

        /// <summary>
        /// The per-mesh level-of-detail data of the render geometry.
        /// </summary>
        public List<PerMeshLodDatum> PerMeshLodData;

        public TagResourceReference Resource;

        [TagStructure(Size = 0x30)]
        public class BoundingSphere : TagStructure
		{
            public RealPlane3d Plane;
            public RealPoint3d Position;
            public float Radius;

            [TagField(Length = 4)]
            public sbyte[] NodeIndices;

            [TagField(Length = 3)]
            public float[] NodeWeights;
        }

        [TagStructure(Size = 0x18)]
        public class UnknownBlock : TagStructure
		{
            public byte UnknownByte1;
            public byte UnknownByte2;
            public short Unknown2;
            public byte[] Unknown3;
        }

        [TagStructure(Size = 0x20)]
        public class UnknownSection : TagStructure
		{
            [TagField(Align = 0x10)]
            public byte[] Unknown;

            public List<UnknownBlock> Unknown2;

            [TagStructure(Size = 0x2)]
			public class UnknownBlock : TagStructure
			{
                public short Unknown;
            }
        }

        [TagStructure(Size = 0xC)]
		public class PerMeshNodeMap : TagStructure
		{
            public List<NodeIndex> NodeIndices;

            [TagStructure(Size = 0x1)]
			public class NodeIndex : TagStructure
			{
                public byte Node;
            }
        }

        [TagStructure(Size = 0xC)]
        public class PerMeshSubpartVisibilityBlock : TagStructure
		{
            public List<BoundingSphere> BoundingSpheres;
        }

        [TagStructure(Size = 0x10)]
        public class PerMeshLodDatum : TagStructure
		{
            public List<Index> Indices;

            public short VertexBufferIndex;

            [TagField(Flags = Padding, Length = 2)]
            public byte[] Unused;

            [TagStructure(Size = 0x4)]
			public class Index : TagStructure
			{
                public int Value;
            }
        }



        //
        // Methods
        //

        /// <summary>
        /// Set the runtime VertexBufferResources and IndexBufferResources fields given the resource definition
        /// </summary>
        /// <param name="resourceDefinition"></param>
        public void SetResourceBuffers(RenderGeometryApiResourceDefinitionTest resourceDefinition)
        {

            foreach(var mesh in Meshes)
            {
                mesh.ResourceVertexBuffers =  new VertexBufferDefinition[8];
                mesh.ResourceIndexBuffers =  new IndexBufferDefinition[2];

                foreach (var vertexBufferIndex in mesh.VertexBufferIndices)
                {
                    if (vertexBufferIndex != -1)
                        mesh.ResourceVertexBuffers[vertexBufferIndex] = resourceDefinition.VertexBuffers[vertexBufferIndex].Definition;
                }

                foreach (var indexBufferIndex in mesh.IndexBufferIndices)
                {
                    if (indexBufferIndex != -1)
                    {
                        if (indexBufferIndex < resourceDefinition.IndexBuffers.Count)
                            mesh.ResourceIndexBuffers[indexBufferIndex] = resourceDefinition.IndexBuffers[indexBufferIndex].Definition;
                        else
                            mesh.ResourceIndexBuffers[indexBufferIndex] = null; // this happens when loading particle model from gen3, the index buffers are empty but indices are set to 0
                    }
                        
                }
            }
        }

        /// <summary>
        /// Generate a valid RenderGeometryApiResourceDefinition from the mesh blocks and sets the values in IndexBufferIndices, VertexBufferIndices
        /// </summary>
        /// <returns></returns>
        public RenderGeometryApiResourceDefinitionTest GetResourceDefinition()
        {
            RenderGeometryApiResourceDefinitionTest result = new RenderGeometryApiResourceDefinitionTest
            {
                IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(),
                VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>()
            };

            // valid for gen3, D3DFixups should also point to the definition.
            result.IndexBuffers.AddressType = CacheAddressType.Definition;
            result.VertexBuffers.AddressType = CacheAddressType.Definition;

            foreach (var mesh in Meshes)
            {

                for(int i = 0; i < mesh.ResourceVertexBuffers.Length; i++)
                {
                    var vertexBuffer = mesh.ResourceVertexBuffers[i];
                    if (vertexBuffer != null)
                    {
                        var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                        d3dPointer.Definition = vertexBuffer;
                        result.VertexBuffers.Add(d3dPointer);
                        mesh.VertexBufferIndices[i] = (short)(result.VertexBuffers.Elements.Count - 1);
                    }
                    else
                        mesh.VertexBufferIndices[i] = - 1;
                }

                for (int i = 0; i < mesh.ResourceIndexBuffers.Length; i++)
                {
                    var indexBuffer = mesh.ResourceIndexBuffers[i];
                    if (indexBuffer != null)
                    {
                        var d3dPointer = new D3DStructure<IndexBufferDefinition>();
                        d3dPointer.Definition = indexBuffer;
                        result.IndexBuffers.Add(d3dPointer);
                        mesh.IndexBufferIndices[i] = (short)(result.IndexBuffers.Elements.Count - 1);
                    }
                    else
                        mesh.IndexBufferIndices[i] = -1;
                }

                // get rid of arrays after creating the resource to prevent bugs in porttag
                mesh.ResourceVertexBuffers = null;
                mesh.ResourceIndexBuffers = null;
            }

            return result;
        }
    }
}