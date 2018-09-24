using TagTool.Serialization;
using System.Collections.Generic;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnline106708)]
    public class TagResource : TagStructure
	{
        public CachedTagInstance ParentTag;
        public ushort Salt;
        public TagResourceType Type;
        public byte Flags;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int FixupInformationOffset;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int FixupInformationLength;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int SecondaryFixupInformationOffset;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short Unknown1;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public short PlaySegmentIndex;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] DefinitionData;

        public CacheAddress DefinitionAddress;

        public List<ResourceFixup> ResourceFixups;
        public List<ResourceDefinitionFixup> ResourceDefinitionFixups;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public int Unknown2;

        [TagStructure(Size = 0x8)]
        public class ResourceFixup : TagStructure
		{
            public uint BlockOffset;
            public CacheAddress Address;
        }

        [TagStructure(Size = 0x8)]
        public class ResourceDefinitionFixup : TagStructure
		{
            public CacheAddress Address;
            public int ResourceStructureTypeIndex;
        }

        /// <summary>
        /// D3D object types.
        /// </summary>
        public enum D3DObjectType : int
        {
            VertexBuffer,      // s_tag_d3d_vertex_buffer
            IndexBuffer,       // s_tag_d3d_index_buffer
            Texture,           // s_tag_d3d_texture
            InterleavedTexture // s_tag_d3d_texture_interleaved
        }
    }
}
