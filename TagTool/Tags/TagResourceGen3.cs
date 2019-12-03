using System.Collections.Generic;
using TagTool.Cache;
using static TagTool.Tags.TagFieldFlags;

namespace TagTool.Tags
{
    [TagStructure(Size = 0x40, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x48, MinVersion = CacheVersion.HaloOnline106708)]
    public class TagResourceGen3 : TagStructure
	{
        public CachedTagInstance ParentTag;
        public ushort Salt;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public sbyte ResourceTypeIndex;
        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public TagResourceTypeGen3 ResourceType;

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
        public short SegmentIndex;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public byte[] DefinitionData;

        public CacheResourceAddress DefinitionAddress;

        public List<ResourceFixup> ResourceFixups = new List<ResourceFixup>();
        public List<ResourceDefinitionFixup> ResourceDefinitionFixups = new List<ResourceDefinitionFixup>();

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public int Unknown2 = 1;

        [TagStructure(Size = 0x8)]
        public class ResourceFixup : TagStructure
		{
            public uint BlockOffset;
            public CacheResourceAddress Address;

            [TagField(Flags = Runtime)]
            public int Type;
            [TagField(Flags = Runtime)]
            public int Offset;
            [TagField(Flags = Runtime)]
            public int RawAddress;
        }

        [TagStructure(Size = 0x8)]
        public class ResourceDefinitionFixup : TagStructure
		{
            public CacheResourceAddress Address;
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
