using System;
using System.IO;
using System.Text;

namespace TagTool.Bitmaps
{
    /// <summary>
    /// Represents the header of a DirectDraw Surface (DDS) file.
    /// </summary>
    public class DdsHeader
    {
        public DdsHeader()
        {
            Reserved = new byte[44];
            FormatType = DdsFormatType.Rgb;
            SurfaceComplexityFlags = DdsSurfaceComplexityFlags.Texture;
            D3D10Dimension = D3D10Dimension.Texture2D;
            D3D10ArraySize = 1;
            D3D10AlphaMode = D3D10AlphaMode.Straight;
        }

        /// <summary>
        /// The width of the texture in pixels.
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// The height of the texture in pixels.
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// For uncompressed textures, gets or sets the pitch of the texture in bytes. Can be 0.
        /// </summary>
        public uint Pitch { get; set; }

        /// <summary>
        /// For compressed textures, gets or sets the total size of the texture in bytes. Can be 0.
        /// </summary>
        public uint LinearSize { get; set; }

        /// <summary>
        /// The depth of the texture in pixels. Can be 0 or 1 for non-volumetric textures.
        /// </summary>
        public uint Depth { get; set; }

        /// <summary>
        /// The number of mipmaps that the texture has. Can be 0 or 1 for non-mipmapped textures.
        /// </summary>
        public uint MipMapCount { get; set; }

        /// <summary>
        /// The reserved information to store in the file header.
        /// Must be exactly 44 bytes large.
        /// </summary>
        public byte[] Reserved { get; set; }

        /// <summary>
        /// The type of the texture's format.
        /// </summary>
        public DdsFormatType FormatType { get; set; }

        /// <summary>
        /// The FourCC for the texture's format. Can be 0.
        /// </summary>
        public uint FourCc { get; set; }

        /// <summary>
        /// The number of bits per pixel.
        /// </summary>
        public uint BitsPerPixel { get; set; }

        /// <summary>
        /// The R channel bitmask.
        /// </summary>
        public uint RBitMask { get; set; }

        /// <summary>
        /// The G channel bit mask.
        /// </summary>
        public uint GBitMask { get; set; }

        /// <summary>
        /// The B channel bit mask.
        /// </summary>
        public uint BBitMask { get; set; }

        /// <summary>
        /// The A channel bit mask.
        /// </summary>
        public uint ABitMask { get; set; }

        /// <summary>
        /// The surface complexity flags.
        /// </summary>
        public DdsSurfaceComplexityFlags SurfaceComplexityFlags { get; set; }

        /// <summary>
        /// The surface info flags.
        /// </summary>
        public DdsSurfaceInfoFlags SurfaceInfoFlags { get; set; }

        public uint UnusedCaps3 { get; set; }
        public uint UnusedCaps4 { get; set; }
        public uint Reserved2 { get; set; }

        /// <summary>
        /// The D3D10 format.
        /// Can be <see cref="DxgiFormat.Unknown"/> if D3D10 extensions are disabled.
        /// </summary>
        public DxgiFormat D3D10Format { get; set; }

        /// <summary>
        /// The D3D10 dimension of the resource.
        /// Only valid if <see cref="D3D10Format"/> is not <see cref="DxgiFormat.Unknown"/>.
        /// </summary>
        public D3D10Dimension D3D10Dimension { get; set; }

        /// <summary>
        /// The D3D10 misc flags.
        /// Only valid if <see cref="D3D10Format"/> is not <see cref="DxgiFormat.Unknown"/>.
        /// </summary>
        public D3D10MiscFlags D3D10MiscFlags { get; set; }

        /// <summary>
        /// For 2D textures that are also cubemaps, gets and sets the number of cubes.
        /// For 3D textures, this must be 1.
        /// Only valid if <see cref="D3D10Format"/> is not <see cref="DxgiFormat.Unknown"/>.
        /// </summary>
        public uint D3D10ArraySize { get; set; }

        /// <summary>
        /// The D3D10 alpha blending mode.
        /// Only valid if <see cref="D3D10Format"/> is not <see cref="DxgiFormat.Unknown"/>.
        /// </summary>
        public D3D10AlphaMode D3D10AlphaMode { get; set; }

        /// <summary>
        /// Reads a DDS header from a stream. On return, the stream will be positioned at the beginning of the texture data.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <returns>The header that was read.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown if the DDS header is invalid.</exception>
        public static DdsHeader Read(Stream stream)
        {
            var reader = new BinaryReader(stream);
            if (stream.Length - stream.Position < 128)
                throw new InvalidOperationException("Invalid DDS file: header is too small");
            var result = new DdsHeader();

            // Read and verify magic
            if (reader.ReadUInt32() != DdsFourCc.FromString("DDS "))
                throw new InvalidOperationException("Invalid DDS file: invalid header magic");

            // Read the DDS header
            result.ReadDdsHeader(reader);

            // If the format FourCC is 'DX10', read the extended header
            if (result.FourCc == DdsFourCc.FromString("DX10"))
                result.ReadDdsD3D10Header(reader);

            return result;
        }

        /// <summary>
        /// Writes the DDS header to a stream. On return, the stream will be positioned where the texture data should go.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <exception cref="System.InvalidOperationException">Thrown if an error occurs while saving.</exception>
        public void WriteTo(Stream stream)
        {
            var writer = new BinaryWriter(stream);
            writer.Write(DdsFourCc.FromString("DDS "));
            WriteDdsHeader(writer);
            if (D3D10Format != DxgiFormat.Unknown)
                WriteDdsD3D10Header(writer);
        }

        private void ReadDdsHeader(BinaryReader reader)
        {
            // Verify header size
            if (reader.ReadUInt32() != 124)
                throw new InvalidOperationException("Invalid DDS file: invalid DDS_HEADER size");

            // Read and verify flags
            var flags = (DdsFlags)reader.ReadUInt32();
            if ((flags & DdsFlags.HeaderFlags) != DdsFlags.HeaderFlags)
                throw new InvalidOperationException("Invalid DDS file: missing one or more required DDS_HEADER flags");

            // Read dimensions
            Height = reader.ReadUInt32();
            Width = reader.ReadUInt32();

            // Read pitch/linear size (both are stored in the same location)
            var pitchOrLinearSize = reader.ReadUInt32();
            if ((flags & DdsFlags.Pitch) != 0)
                Pitch = pitchOrLinearSize;
            else if ((flags & DdsFlags.LinearSize) != 0)
                LinearSize = pitchOrLinearSize;

            // Read depth (optional)
            var depth = reader.ReadUInt32();
            if ((flags & DdsFlags.Depth) != 0)
                Depth = depth;

            // Read mipmap count (optional)
            var mipMapCount = reader.ReadUInt32();
            if ((flags & DdsFlags.MipMapCount) != 0)
                MipMapCount = mipMapCount;

            // Read reserved/unused data
            Reserved = reader.ReadBytes(44);

            // Read DDS_PIXELFORMAT
            ReadDdsPixelFormat(reader);

            // Read capabilities and unused data
            SurfaceComplexityFlags = (DdsSurfaceComplexityFlags)reader.ReadUInt32();
            SurfaceInfoFlags = (DdsSurfaceInfoFlags)reader.ReadUInt32();
            UnusedCaps3 = reader.ReadUInt32();
            UnusedCaps4 = reader.ReadUInt32();
            Reserved2 = reader.ReadUInt32();
        }

        private void WriteDdsHeader(BinaryWriter writer)
        {
            writer.Write(124);
            writer.Write((uint)CalculateHeaderFlags());
            writer.Write(Height);
            writer.Write(Width);
            writer.Write((Pitch != 0) ? Pitch : LinearSize);
            writer.Write(Depth);
            writer.Write(Math.Max(MipMapCount, 1));
            if (Reserved.Length != 44)
                throw new InvalidOperationException("Reserved buffer must be 44 bytes large");
            writer.Write(Reserved);
            WriteDdsPixelFormat(writer);
            writer.Write((uint)CalculateSurfaceComplexityFlags());
            writer.Write((uint)SurfaceInfoFlags);
            writer.Write(UnusedCaps3);
            writer.Write(UnusedCaps4);
            writer.Write(Reserved2);
        }

        private DdsFlags CalculateHeaderFlags()
        {
            var result = DdsFlags.HeaderFlags;
            if (Pitch != 0)
                result |= DdsFlags.Pitch;
            else if (LinearSize != 0)
                result |= DdsFlags.LinearSize;
            if (MipMapCount > 1)
                result |= DdsFlags.MipMapCount;
            if (Depth > 1)
                result |= DdsFlags.Depth;
            return result;
        }

        private DdsSurfaceComplexityFlags CalculateSurfaceComplexityFlags()
        {
            var result = SurfaceComplexityFlags;
            result |= DdsSurfaceComplexityFlags.Texture;
            if (MipMapCount != 0)
                result |= DdsSurfaceComplexityFlags.Complex | DdsSurfaceComplexityFlags.MipMap;
            if ((SurfaceInfoFlags & DdsSurfaceInfoFlags.CubeMap) != 0)
                result |= DdsSurfaceComplexityFlags.Complex;
            return result;
        }

        private void ReadDdsPixelFormat(BinaryReader reader)
        {
            // Verify header size
            if (reader.ReadUInt32() != 32)
                throw new InvalidOperationException("Invalid DDS file: invalid DDS_PIXELFORMAT size");

            // Read flags and set FormatType accordingly
            var flags = (DdsFormatFlags)reader.ReadUInt32();
            if ((flags & DdsFormatFlags.Alpha) != 0)
                FormatType = DdsFormatType.Alpha;
            else if ((flags & DdsFormatFlags.RGB) != 0)
                FormatType = DdsFormatType.Rgb;
            else if ((flags & DdsFormatFlags.YUV) != 0)
                FormatType = DdsFormatType.Yuv;
            else if ((flags & DdsFormatFlags.Luminance) != 0)
                FormatType = DdsFormatType.Luminance;
            else if ((flags & DdsFormatFlags.FourCC) != 0)
                FormatType = DdsFormatType.Other;
            else
                throw new InvalidOperationException("Invalid DDS file: invalid DDS_PIXELFORMAT flags");

            // Read FourCC code (optional)
            var fourCc = reader.ReadUInt32();
            if ((flags & DdsFormatFlags.FourCC) != 0)
            {
                FourCc = fourCc;
                reader.BaseStream.Position += 20; // Skip masks
            }
            else
            {
                // Read RGB masks
                BitsPerPixel = reader.ReadUInt32();
                RBitMask = reader.ReadUInt32();
                GBitMask = reader.ReadUInt32();
                BBitMask = reader.ReadUInt32();

                // Read alpha mask (optional)
                var alphaMask = reader.ReadUInt32();
                if ((flags & DdsFormatFlags.AlphaPixels) != 0)
                    ABitMask = alphaMask;
            }
        }

        private void WriteDdsPixelFormat(BinaryWriter writer)
        {
            writer.Write(32);
            writer.Write((uint)CalculateFormatFlags());
            writer.Write((D3D10Format == DxgiFormat.Unknown) ? FourCc : DdsFourCc.FromString("DX10"));
            writer.Write(BitsPerPixel);
            writer.Write(RBitMask);
            writer.Write(GBitMask);
            writer.Write(BBitMask);
            writer.Write(ABitMask);
        }

        private DdsFormatFlags CalculateFormatFlags()
        {
            DdsFormatFlags flags;
            switch (FormatType)
            {
                case DdsFormatType.Alpha:
                    flags = DdsFormatFlags.Alpha;
                    break;
                case DdsFormatType.Rgb:
                    flags = DdsFormatFlags.RGB;
                    break;
                case DdsFormatType.Yuv:
                    flags = DdsFormatFlags.YUV;
                    break;
                case DdsFormatType.Luminance:
                    flags = DdsFormatFlags.Luminance;
                    break;
                case DdsFormatType.Other:
                    flags = DdsFormatFlags.FourCC;
                    break;
                default:
                    throw new InvalidOperationException("Unrecognized FormatType: " + FormatType);
            }
            if (FourCc != 0)
                flags |= DdsFormatFlags.FourCC;
            if (ABitMask != 0)
                flags |= DdsFormatFlags.AlphaPixels;
            return flags;
        }

        private void ReadDdsD3D10Header(BinaryReader reader)
        {
            D3D10Format = (DxgiFormat)reader.ReadUInt32();
            D3D10Dimension = (D3D10Dimension)reader.ReadUInt32();
            D3D10MiscFlags = (D3D10MiscFlags)reader.ReadUInt32();
            D3D10ArraySize = reader.ReadUInt32();
            D3D10AlphaMode = (D3D10AlphaMode)reader.ReadUInt32();
        }

        private void WriteDdsD3D10Header(BinaryWriter writer)
        {
            writer.Write((uint)D3D10Format);
            writer.Write((uint)D3D10Dimension);
            writer.Write((uint)D3D10MiscFlags);
            writer.Write(D3D10ArraySize);
            writer.Write((uint)D3D10AlphaMode);
        }

        /// <summary>
        /// Flags listing which features a DDS file supports.
        /// </summary>
        [Flags]
        private enum DdsFlags
        {
            Caps        = 0x1,      // Required
            Height      = 0x2,      // Required
            Width       = 0x4,      // Required
            Pitch       = 0x8,      // Texture is uncompressed and has a pitch
            PixelFormat = 0x1000,   // Required
            MipMapCount = 0x20000,  // Texture has mipmaps
            LinearSize  = 0x80000,  // Texture is compressed and has a linear size
            Depth       = 0x800000, // Texture has depth

            HeaderFlags = Caps | Height | Width | PixelFormat
        }

        /// <summary>
        /// Flags containing information about a DDS file's format. Taken from ddraw.h
        /// </summary>
        [Flags]
        private enum DdsFormatFlags
        {
            AlphaPixels             = 0x1,      // The surface has alpha channel information in the pixel format.
            Alpha                   = 0x2,      // The pixel format contains alpha only information
            FourCC                  = 0x4,      // The FourCC code is valid.
            PaletteIndexed4         = 0x8,      // The surface is 4-bit color indexed.
            PaletteIndexedT08       = 0x10,     // The surface is indexed into a palette which stores indices into the destination surface's 8-bit palette.
            PaletteIndexed8         = 0x20,     // The surface is 8-bit color indexed.
            RGB                     = 0x40,     // The RGB data in the pixel format structure is valid.
            Compressed              = 0x80,     // The surface will accept pixel data in the format specified and compress it during the write.
            RGBToYUV                = 0x100,    // The surface will accept RGB data and translate it during the write to YUV data.  The format of the data to be written
                                                // will be contained in the pixel format structure.  The DDPF_RGB flag will be set.
            YUV                     = 0x200,    // Pixel format is YUV - YUV data in pixel format struct is valid.
            ZBuffer                 = 0x400,    // Pixel format is a z buffer only surface.
            PaletteIndexed1         = 0x800,    // The surface is 1-bit color indexed.
            PaletteIndexed2         = 0x1000,   // The surface is 2-bit color indexed.
            ZPixels                 = 0x2000,   // The surface contains Z information in the pixels.
            StencilBuffer           = 0x4000,   // The surface contains stencil information along with Z.
            PremultipliedAlpha      = 0x8000,   // Premultiplied alpha format -- the color components have been premultiplied by the alpha component.
            Luminance               = 0x20000,  // Luminance data in the pixel format is valid. Use this flag for luminance-only or luminance+alpha surfaces, the bit depth is then ddpf.dwLuminanceBitCount.
            BumpLuminance           = 0x40000,  // Luminance data in the pixel format is valid. Use this flag when hanging luminance off bumpmap surfaces, the bit mask for the luminance portion of the pixel is then ddpf.dwBumpLuminanceBitMask.
            BumpDUDV                = 0x80000   // Bump map dUdV data in the pixel format is valid.

        }
    }

    /// <summary>
    /// Utility class for DDS FourCC values.
    /// </summary>


    /// <summary>
    /// DDS surface complexity flags.
    /// </summary>
    [Flags]
    public enum DdsSurfaceComplexityFlags
    {
        /// <summary>
        /// Indicates that the file contains more than one surface.
        /// </summary>
        Complex = 0x8,

        /// <summary>
        /// Indicates that the file contains mipmaps.
        /// </summary>
        MipMap  = 0x400000,

        /// <summary>
        /// Indicates that the file contains texture info. Required.
        /// </summary>
        Texture = 0x1000
    }

    /// <summary>
    /// DDS surface info flags.
    /// </summary>
    [Flags]
    public enum DdsSurfaceInfoFlags
    {
        CubeMap          = 0x200,
        CubeMapPositiveX = 0x400,
        CubeMapNegativeX = 0x800,
        CubeMapPositiveY = 0x1000,
        CubeMapNegativeY = 0x2000,
        CubeMapPositiveZ = 0x4000,
        CubeMapNegativeZ = 0x8000,
        Volume           = 0x200000
    }

    /// <summary>
    /// DDS format types.
    /// </summary>
    public enum DdsFormatType
    {
        /// <summary>
        /// The texture contains RGB data.
        /// </summary>
        Rgb,

        /// <summary>
        /// The texture contains YUV data.
        /// </summary>
        Yuv,

        /// <summary>
        /// The texture contains luminance data.
        /// </summary>
        Luminance,

        /// <summary>
        /// The texture only contains an alpha channel.
        /// </summary>
        Alpha,

        /// <summary>
        /// The format should be determined by the texture's FourCC code or the D3D10 format.
        /// </summary>
        Other
    }

    /// <summary>
    /// DXGI texture formats.
    /// </summary>
    public enum DxgiFormat
    {
        Unknown                = 0,
        R32G32B32A32Typeless   = 1,
        R32G32B32A32Float      = 2,
        R32G32B32A32UInt       = 3,
        R32G32B32A32Sint       = 4,
        R32G32B32Typeless      = 5,
        R32G32B32Float         = 6,
        R32G32B32UInt          = 7,
        R32G32B32Sint          = 8,
        R16G16B16A16Typeless   = 9,
        R16G16B16A16Float      = 10,
        R16G16B16A16UNorm      = 11,
        R16G16B16A16UInt       = 12,
        R16G16B16A16SNorm      = 13,
        R16G16B16A16Sint       = 14,
        R32G32Typeless         = 15,
        R32G32Float            = 16,
        R32G32UInt             = 17,
        R32G32Sint             = 18,
        R32G8X24Typeless       = 19,
        D32FloatS8X24UInt      = 20,
        R32FloatX8X24Typeless  = 21,
        X32TypelessG8X24UInt   = 22,
        R10G10B10A2Typeless    = 23,
        R10G10B10A2UNorm       = 24,
        R10G10B10A2UInt        = 25,
        R11G11B10Float         = 26,
        R8G8B8A8Typeless       = 27,
        R8G8B8A8UNorm          = 28,
        R8G8B8A8UNormSrgb      = 29,
        R8G8B8A8UInt           = 30,
        R8G8B8A8SNorm          = 31,
        R8G8B8A8Sint           = 32,
        R16G16Typeless         = 33,
        R16G16Float            = 34,
        R16G16UNorm            = 35,
        R16G16UInt             = 36,
        R16G16SNorm            = 37,
        R16G16Sint             = 38,
        R32Typeless            = 39,
        D32Float               = 40,
        R32Float               = 41,
        R32UInt                = 42,
        R32Sint                = 43,
        R24G8Typeless          = 44,
        D24UNormS8UInt         = 45,
        R24UNormX8Typeless     = 46,
        X24TypelessG8UInt      = 47,
        R8G8Typeless           = 48,
        R8G8UNorm              = 49,
        R8G8UInt               = 50,
        R8G8SNorm              = 51,
        R8G8Sint               = 52,
        R16Typeless            = 53,
        R16Float               = 54,
        D16UNorm               = 55,
        R16UNorm               = 56,
        R16UInt                = 57,
        R16SNorm               = 58,
        R16Sint                = 59,
        R8Typeless             = 60,
        R8UNorm                = 61,
        R8UInt                 = 62,
        R8SNorm                = 63,
        R8Sint                 = 64,
        A8UNorm                = 65,
        R1UNorm                = 66,
        R9G9B9E5SharedExp      = 67,
        R8G8B8G8UNorm          = 68,
        G8R8G8B8UNorm          = 69,
        Bc1Typeless            = 70,
        Bc1UNorm               = 71,
        Bc1UNormSrgb           = 72,
        Bc2Typeless            = 73,
        Bc2UNorm               = 74,
        Bc2UNormSrgb           = 75,
        Bc3Typeless            = 76,
        Bc3UNorm               = 77,
        Bc3UNormSrgb           = 78,
        Bc4Typeless            = 79,
        Bc4UNorm               = 80,
        Bc4SNorm               = 81,
        Bc5Typeless            = 82,
        Bc5UNorm               = 83,
        Bc5SNorm               = 84,
        B5G6R5UNorm            = 85,
        B5G5R5A1UNorm          = 86,
        B8G8R8A8UNorm          = 87,
        B8G8R8X8UNorm          = 88,
        R10G10B10XrBiasA2UNorm = 89,
        B8G8R8A8Typeless       = 90,
        B8G8R8A8UNormSrgb      = 91,
        B8G8R8X8Typeless       = 92,
        B8G8R8X8UNormSrgb      = 93,
        Bc6HTypeless           = 94,
        Bc6Huf16               = 95,
        Bc6Hsf16               = 96,
        Bc7Typeless            = 97,
        Bc7UNorm               = 98,
        Bc7UNormSrgb           = 99,
        Ayuv                   = 100,
        Y410                   = 101,
        Y416                   = 102,
        Nv12                   = 103,
        P010                   = 104,
        P016                   = 105,
        Opaque420              = 106,
        Yuy2                   = 107,
        Y210                   = 108,
        Y216                   = 109,
        Nv11                   = 110,
        Ai44                   = 111,
        Ia44                   = 112,
        P8                     = 113,
        A8P8                   = 114,
        B4G4R4A4UNorm          = 115,
        P208                   = 130,
        V208                   = 131,
        V408                   = 132,
        Astc4X4UNorm           = 134,
        Astc4X4UNormSrgb       = 135,
        Astc5X4Typeless        = 137,
        Astc5X4UNorm           = 138,
        Astc5X4UNormSrgb       = 139,
        Astc5X5Typeless        = 141,
        Astc5X5UNorm           = 142,
        Astc5X5UNormSrgb       = 143,
        Astc6X5Typeless        = 145,
        Astc6X5UNorm           = 146,
        Astc6X5UNormSrgb       = 147,
        Astc6X6Typeless        = 149,
        Astc6X6UNorm           = 150,
        Astc6X6UNormSrgb       = 151,
        Astc8X5Typeless        = 153,
        Astc8X5UNorm           = 154,
        Astc8X5UNormSrgb       = 155,
        Astc8X6Typeless        = 157,
        Astc8X6UNorm           = 158,
        Astc8X6UNormSrgb       = 159,
        Astc8X8Typeless        = 161,
        Astc8X8UNorm           = 162,
        Astc8X8UNormSrgb       = 163,
        Astc10X5Typeless       = 165,
        Astc10X5UNorm          = 166,
        Astc10X5UNormSrgb      = 167,
        Astc10X6Typeless       = 169,
        Astc10X6UNorm          = 170,
        Astc10X6UNormSrgb      = 171,
        Astc10X8Typeless       = 173,
        Astc10X8UNorm          = 174,
        Astc10X8UNormSrgb      = 175,
        Astc10X10Typeless      = 177,
        Astc10X10UNorm         = 178,
        Astc10X10UNormSrgb     = 179,
        Astc12X10Typeless      = 181,
        Astc12X10UNorm         = 182,
        Astc12X10UNormSrgb     = 183,
        Astc12X12Typeless      = 185,
        Astc12X12UNorm         = 186,
        Astc12X12UNormSrgb     = 187
    }

    /// <summary>
    /// D3D10 resource dimensions.
    /// </summary>
    public enum D3D10Dimension
    {
        Texture1D = 2,
        Texture2D = 3,
        Texture3D = 4
    }

    /// <summary>
    /// Miscellaneous D3D10 flags.
    /// </summary>
    [Flags]
    public enum D3D10MiscFlags
    {
        None = 0,

        /// <summary>
        /// Indicates that a 2D texture is also a cubemap.
        /// </summary>
        TextureCube = 4
    }

    /// <summary>
    /// D3D10 alpha blending modes.
    /// </summary>
    public enum D3D10AlphaMode
    {
        Unknown,
        Straight,
        Premultiplied,
        Opaque,
        Custom
    }
}
