using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using System.Collections.Generic;
using static TagTool.Tags.TagFieldFlags;
using TagTool.Tags.Resources;
using System;
using TagTool.Tags.Definitions.Common;

namespace TagTool.Geometry
{
    [TagStructure(Name = "render_geometry", Size = 0x84, MaxVersion = CacheVersion.HaloOnline700123)]
    [TagStructure(Name = "render_geometry", Size = 0x9C, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.Original)]
    [TagStructure(Name = "render_geometry", Size = 0xA8, MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
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

        public List<UserDataBlock> UserData;

        public List<PerMeshRawDataBlock> GeometryTagResources; 

        public List<PerMeshMoppBlock> MeshClusterVisibility;

        /// <summary>
        /// The per-mesh node mappings of the render geometry.
        /// </summary>
        public List<PerMeshNodeMap> PerMeshNodeMaps;

        /// <summary>
        /// The per-mesh subpart visibility of the render geometry.
        /// </summary>
        public List<PerMeshSubpartVisibilityBlock> PerMeshSubpartVisibility;


        public List<PerMeshPrtDataBlock> PerMeshPrtData;

        // TODO: review reach definitions
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown10;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown11;
        [TagField(MinVersion = CacheVersion.HaloReach)]
        public uint Unknown12;

        public List<StaticPerPixelLighting> InstancedGeometryPerPixelLighting;

        [TagField(MinVersion = CacheVersion.HaloReach)]
        public List<WaterBoundingBox> WaterBoundingBoxes;

        public TagResourceReference Resource;

        [TagField(MinVersion = CacheVersion.HaloReach, Platform = CachePlatform.MCC)]
        public List<ConstantBufferInteropBlock> ConstantBufferInterop;

        [TagStructure(Size = 0xC)]
        public class ConstantBufferInteropBlock : TagStructure
        {
            public uint InteropPointer;
            public int Unknown;

            [TagField(Length = 0x4, Flags = Padding)]
            public byte[] Padding0;
        }

        [TagStructure(Size = 0x20)]
        public class PerMeshPrtDataBlock : TagStructure
        {
            public byte[] MeshPcaData;
            public List<PerInstancePrtDataBlock> PerInstancePrtData;

            [TagStructure(Size = 0x14)]
            public class PerInstancePrtDataBlock : TagStructure
            {
                public byte[] MeshPcaData;
            }
        }

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
        public class UserDataBlock : TagStructure
        {
            public RenderGeometryUserDataType DataType;
            public sbyte DataCount;
            public ushort DataSize;
            [TagField(MinVersion = CacheVersion.Halo3Retail)]
            public byte[] Data;
            [TagField(MaxVersion = CacheVersion.Halo3Beta, Length = 0x14)]
            public byte[] DataBeta;

            public enum RenderGeometryUserDataType : sbyte
            {
                PrtInfo
            }
        }

        [TagStructure(Size = 0x20)]
        public class PerMeshMoppBlock : TagStructure
		{
            public byte[] MoppCode;
            public List<short> MoppReorderTable;
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

        [TagStructure(Size = 0x1C)]
        public class WaterBoundingBox : TagStructure
        {
            public short MeshIndex;
            public short PartIndex;
            public RealPoint3d PositionBoundLower;
            public RealPoint3d PositionBoundUpper;
        }

        [TagStructure(Size = 0x10, MaxVersion = CacheVersion.HaloOnline700123)]
        [TagStructure(Size = 0x2, MinVersion = CacheVersion.HaloReach)]
        public class StaticPerPixelLighting : TagStructure
		{
            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public List<int> UnusedVertexBuffer;

            public short VertexBufferIndex;

            [TagField(MaxVersion = CacheVersion.HaloOnline700123)]
            public short Unknown1;

            [TagField(Flags = Runtime)]
            public VertexBufferDefinition VertexBuffer;

        }

        /// <summary>
        /// Unused tag mesh data
        /// </summary>
        [TagStructure(Size = 0x2C)]
        public class PerMeshRawDataBlock : TagStructure
        {
            public List<RawVertexBlock> RawVertices;
            public List<IndicesWordBlock> RawIndices;
            public List<RawWaterBlock> RawWaterData;
            public short ParameterizedTextureWidth;
            public short ParameterizedTextureHeight;
            public PerMeshRawDataFlags Flags;

            [Flags]
            public enum PerMeshRawDataFlags : uint
            {
                IndicesAreTriangleStrips = 1 << 0,
                IndicesAreTriangleLists = 1 << 1,
                IndicesAreQuadLists = 1 << 2
            }

            [TagStructure(Size = 0x60)]
            public class RawVertexBlock : TagStructure
            {
                public RealPoint3d Position;
                public RealPoint2d Texcoord;
                public RealPoint3d Normal;
                public RealPoint3d Binormal;
                public RealPoint3d Tangent;
                public RealPoint2d LightmapTexcoord;
                [TagField(Length = 4)]
                public byte[] NodeIndices;
                [TagField(Length = 4)]
                public float[] NodeWeights;
                public RealPoint3d VertexColor;
            }

            [TagStructure(Size = 0x2)]
            public class IndicesWordBlock : TagStructure
            {
                public short Word;
            }

            [TagStructure(Size = 0x18)]
            public class RawWaterBlock : TagStructure
            {
                public List<IndicesWordBlock> RawWaterIndices;
                public List<RawWaterAppendBlock> RawWaterVertices;

                [TagStructure(Size = 0x2)]
                public class IndicesWordBlock : TagStructure
                {
                    public short Word;
                }

                [TagStructure(Size = 0x24)]
                public class RawWaterAppendBlock : TagStructure
                {
                    public RealPoint3d LocalInfo;
                    public RealPoint3d WaterVelocity;
                    public RealPoint3d BaseTexcoord;
                }
            }
        }

        //
        // Methods
        //

        /// <summary>
        /// Set the runtime VertexBufferResources and IndexBufferResources fields given the resource definition
        /// </summary>
        /// <param name="resourceDefinition"></param>
        public void SetResourceBuffers(RenderGeometryApiResourceDefinition resourceDefinition, bool allowShared)
        {
            bool[] convertedVertexBuffers = new bool[resourceDefinition.VertexBuffers.Count];
            bool[] convertedIndexBuffers = new bool[resourceDefinition.IndexBuffers.Count];

            foreach (var mesh in Meshes)
            {
                mesh.ResourceVertexBuffers =  new VertexBufferDefinition[8];
                mesh.ResourceIndexBuffers =  new IndexBufferDefinition[2];

                for(int i = 0; i < mesh.VertexBufferIndices.Length; i++)
                {
                    var vertexBufferIndex = mesh.VertexBufferIndices[i];
                    if (vertexBufferIndex != -1)
                    {
                        if (vertexBufferIndex < resourceDefinition.VertexBuffers.Count)
                        {
                            if(convertedVertexBuffers[vertexBufferIndex] == false || allowShared)
                            {
                                convertedVertexBuffers[vertexBufferIndex] = true;
                                mesh.ResourceVertexBuffers[i] = resourceDefinition.VertexBuffers[vertexBufferIndex].Definition;
                            }
                            else
                            {
                                mesh.VertexBufferIndices[i] = -1;
                                System.Console.WriteLine("Sharing vertex buffers not supported, ignoring it.");
                            }
                        }
                            
                        else
                            mesh.ResourceVertexBuffers[i] = null; // happens on sbsp
                    }
                }

                for(int i = 0; i < mesh.IndexBufferIndices.Length; i++)
                {
                    var indexBufferIndex = mesh.IndexBufferIndices[i];
                    if (indexBufferIndex != -1)
                    {
                        if (indexBufferIndex < resourceDefinition.IndexBuffers.Count)
                        {
                            if(convertedIndexBuffers[indexBufferIndex] == false || allowShared)
                            {
                                mesh.ResourceIndexBuffers[i] = resourceDefinition.IndexBuffers[indexBufferIndex].Definition;
                                convertedIndexBuffers[indexBufferIndex] = true;
                            }
                            else
                            {
                                mesh.IndexBufferIndices[i] = -1;
                                System.Console.WriteLine("Sharing index buffers not supported, ignoring it.");
                            }

                        }
                        else
                            mesh.ResourceIndexBuffers[i] = null; // this happens when loading particle model from gen3, the index buffers are empty but indices are set to 0
                    }
                }
            }

            for(int i = 0; i < InstancedGeometryPerPixelLighting.Count; i++)
            {
                var vertexBufferIndex = InstancedGeometryPerPixelLighting[i].VertexBufferIndex;
                if (vertexBufferIndex != -1)
                {
                    if (vertexBufferIndex < resourceDefinition.VertexBuffers.Count)
                        InstancedGeometryPerPixelLighting[i].VertexBuffer = resourceDefinition.VertexBuffers[vertexBufferIndex].Definition;
                    else
                        InstancedGeometryPerPixelLighting[i].VertexBuffer = null;
                }
            }
        }

        /// <summary>
        /// Generate a valid RenderGeometryApiResourceDefinition from the mesh blocks and sets the values in IndexBufferIndices, VertexBufferIndices
        /// </summary>
        /// <returns></returns>
        public RenderGeometryApiResourceDefinition GetResourceDefinition()
        {
            RenderGeometryApiResourceDefinition result = new RenderGeometryApiResourceDefinition
            {
                IndexBuffers = new TagBlock<D3DStructure<IndexBufferDefinition>>(),
                VertexBuffers = new TagBlock<D3DStructure<VertexBufferDefinition>>()
            };

            // valid for gen3, InteropLocations should also point to the definition.
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

                // if the mesh is unindexed the index in the index buffer should be 0, but the buffer is empty. Copying what h3\ho does.
                if (mesh.Flags.HasFlag(MeshFlags.MeshIsUnindexed))
                {
                    mesh.IndexBufferIndices[0] = 0;
                    mesh.IndexBufferIndices[1] = 0;
                }
            }

            for (int i = 0; i < InstancedGeometryPerPixelLighting.Count; i++)
            {
                var perPixel = InstancedGeometryPerPixelLighting[i];
                if (perPixel.VertexBuffer != null)
                {
                    var d3dPointer = new D3DStructure<VertexBufferDefinition>();
                    d3dPointer.Definition = perPixel.VertexBuffer;
                    result.VertexBuffers.Add(d3dPointer);
                    perPixel.VertexBufferIndex = (short)(result.VertexBuffers.Elements.Count - 1);
                }
                else
                    perPixel.VertexBufferIndex = -1;
            }

            return result;
        }
    }
}