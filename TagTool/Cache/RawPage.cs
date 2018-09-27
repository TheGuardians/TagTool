using TagTool.Tags;
using System;

namespace TagTool.Cache
{
    [TagStructure(Size = 0x58, MaxVersion = CacheVersion.Halo3ODST)]
    [TagStructure(Size = 0x24, MaxVersion = CacheVersion.HaloOnline106708)]
    [TagStructure(Size = 0x28, MinVersion = CacheVersion.HaloOnline235640)]
    public class RawPage : TagStructure
	{
        public short Salt;

        /// <summary>
        /// Gets or sets flags containing information about where the resource is located.
        /// </summary>
        [TagField(MinVersion = CacheVersion.HaloOnline106708, MaxVersion = CacheVersion.HaloOnline106708)]
        public OldRawPageFlags OldFlags;

        /// <summary>
        /// Gets or sets flags containing information about where the resource is located.
        /// </summary>
        [TagField(MinVersion = CacheVersion.HaloOnline235640)]
        public NewRawPageFlags NewFlags;

        public sbyte CompressionCodecIndex;

        [TagField(MinVersion = CacheVersion.HaloOnline235640)]
        public int Unknown1;

        public int Index;

        [TagField(MaxVersion = CacheVersion.Halo3ODST)]
        public int BlockOffset;

        public uint CompressedBlockSize;
        public uint UncompressedBlockSize;
        public uint CrcChecksum;

        [TagField(Length = 20, MaxVersion = CacheVersion.Halo3ODST)]
        public byte[] EntireBufferHash;

        [TagField(Length = 20, MaxVersion = CacheVersion.Halo3ODST)]
        public byte[] FirstChunkHash;

        [TagField(Length = 20, MaxVersion = CacheVersion.Halo3ODST)]
        public byte[] LastChunkHash;

        public short BlockAssetCount;
        public short Unknown3;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown4;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown5;

        [TagField(MinVersion = CacheVersion.HaloOnline106708)]
        public uint Unknown6;
    }

    /// <summary>
    /// Flags related to the location and storage of the resource data.
    /// Only for 1.106708.
    /// </summary>
    [Flags]
    public enum OldRawPageFlags : byte
    {
        /// <summary>
        /// Indicates that the resource's checksum should be validated.
        /// </summary>
        UseChecksum = 1 << 0,

        /// <summary>
        /// Indicates that the resource is in resources.dat.
        /// </summary>
        InResources = 1 << 1,

        /// <summary>
        /// Indicates that the resource is in textures.dat.
        /// </summary>
        InTextures = 1 << 2,

        /// <summary>
        /// Indicates that the resource is in textures_b.dat.
        /// </summary>
        InTexturesB = 1 << 3,

        /// <summary>
        /// Indicates that the resource is in audio.dat.
        /// </summary>
        InAudio = 1 << 4,

        /// <summary>
        /// Indicates that the resource is in resources_b.dat.
        /// </summary>
        InResourcesB = 1 << 5,

        Unknown6 = 1 << 6,

        /// <summary>
        /// Indicates that the resource's checksum should be validated.
        /// Alternate flag for <see cref="UseChecksum"/>.
        /// </summary>
        UseChecksum2 = 1 << 7,

        /// <summary>
        /// Mask for the location part of the flags.
        /// </summary>
        LocationMask = 0x3E,
    }

    /// <summary>
    /// Flags related to the location and storage of the resource data.
    /// Only for versions 1.235640 and newer.
    /// </summary>
    [Flags]
    public enum NewRawPageFlags : byte
    {
        /// <summary>
        /// Indicates that the resource's checksum should be validated.
        /// </summary>
        UseChecksum = 1 << 0,

        /// <summary>
        /// Indicates that the resource is in resources.dat.
        /// </summary>
        InResources = 1 << 1,

        /// <summary>
        /// Indicates that the resource is in textures.dat.
        /// </summary>
        InTextures = 1 << 2,

        /// <summary>
        /// Indicates that the resource is in textures_b.dat.
        /// </summary>
        InTexturesB = 1 << 3,

        /// <summary>
        /// Indicates that the resource is in audio.dat.
        /// </summary>
        InAudio = 1 << 4,

        /// <summary>
        /// Indicates that the resource is in resources_b.dat.
        /// </summary>
        InResourcesB = 1 << 5,

        /// <summary>
        /// Indicates that the resource is in render_models.dat.
        /// </summary>
        InRenderModels = 1 << 6,

        /// <summary>
        /// Indicates that the resource is in lightmaps.dat.
        /// </summary>
        InLightmaps = 1 << 7,

        /// <summary>
        /// Mask for the location part of the flags.
        /// </summary>
        LocationMask = 0xFE,
    }
}
