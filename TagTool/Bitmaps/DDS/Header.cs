using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;

namespace TagTool.Bitmaps.DDS
{
    public abstract class BaseHeader
    {
        public abstract void Write(EndianWriter writer);
        public abstract bool Read(EndianReader reader);
    }

    public class DDSHeader : BaseHeader
    {
        public readonly int Size = 0x7C;
        public DDSFlags Flags = DDSFlags.Caps | DDSFlags.PixelFormat;
        public int Height;
        public int Width;
        public int PitchOrLinearSize;
        public int Depth;
        public int MipMapCount;
        public byte[] Reserved1 = new byte[0x2C];
        public PixelFormat PixelFormat;
        public DDSComplexityFlags Caps = DDSComplexityFlags.Texture;
        public DDSSurfaceInfoFlags Caps2;
        public int Caps3;
        public int Caps4;
        public int Reserved2;

        public DDSHeader(BitmapTextureInteropResource.BitmapDefinition definition)
        {
            CreateHeaderFromType(definition.Height, definition.Width, definition.Depth, definition.MipmapCount, definition.Format, definition.Type, definition.Flags);
        }

        public DDSHeader(Bitmap.Image image)
        {
            var mipMapCount = image.MipmapCount != 0 ? (1 + image.MipmapCount) : 0;
            CreateHeaderFromType(image.Height, image.Width, image.Depth, mipMapCount, image.Format, image.Type, image.Flags);
        }

        public DDSHeader(BaseBitmap image)
        {
            var mipMapCount = image.MipMapCount != 0 ? (1 + image.MipMapCount) : 0;
            CreateHeaderFromType(image.Height, image.Width, image.Depth, mipMapCount, image.Format, image.Type, image.Flags);
        }


        /// <summary>
        /// Write a DDS header
        /// </summary>
        /// <param name="writer"></param>
        public override void Write(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            writer.Write((uint)0x20534444);   // DDS 
            writer.Write(Size);
            writer.Write((int)Flags);
            writer.Write(Height);
            writer.Write(Width);
            writer.Write(PitchOrLinearSize);
            writer.Write(Depth);
            writer.Write(MipMapCount);
            writer.WriteBlock(Reserved1);
            PixelFormat.Write(writer);
            writer.Write((int)Caps);
            writer.Write((int)Caps2);
            writer.Write(Caps3);
            writer.Write(Caps4);
            writer.Write(Reserved2);
        }

        /// <summary>
        /// Read the content of the DDS header.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>File is valid</returns>
        public override bool Read(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;

            if (reader.ReadInt32() != 0x20534444 && reader.ReadInt32() != Size)
                return false;
            else
            {
                Flags = (DDSFlags)reader.ReadInt32();
                Height = reader.ReadInt32();
                Width = reader.ReadInt32();
                PitchOrLinearSize = reader.ReadInt32();
                Depth = reader.ReadInt32();
                MipMapCount = reader.ReadInt32();
                reader.ReadBlock(Reserved1, 0, 0x2C);
                if (!PixelFormat.Read(reader))
                    return false;
                Caps =(DDSComplexityFlags) reader.ReadInt32();
                Caps2 =(DDSSurfaceInfoFlags) reader.ReadInt32();
                Caps3 = reader.ReadInt32();
                Caps4 = reader.ReadInt32();
                Reserved2 = reader.ReadInt32();

                // Add more verifications here

                return true;
            }

        }

        private void CreateHeaderFromType(int height, int width, int depth, int mipMapCount, BitmapFormat format, BitmapType type, BitmapFlags flags)
        {
            Height = height;
            Width = width;

            Flags |= DDSFlags.Height | DDSFlags.Width;

            switch (type)
            {
                case BitmapType.Texture2D:
                    CreateHeaderTexture2D(mipMapCount, format, flags);
                    break;
                case BitmapType.Texture3D:
                case BitmapType.Array:
                    CreateHeaderVolume(mipMapCount, depth, format, flags);
                    break;

                case BitmapType.CubeMap:
                    CreateHeaderCubemap(mipMapCount, format, flags);
                    break;

            }

            return;
        }

        private void CreateHeaderTexture2D(int mipMapCount, BitmapFormat format, BitmapFlags flags)
        {
            Depth = 0;

            if (mipMapCount > 0)
            {
                MipMapCount = 1 + mipMapCount;
                Flags |= DDSFlags.MipMapCount;
                Caps |= DDSComplexityFlags.MipMap | DDSComplexityFlags.Complex;
            }
            else
                MipMapCount = 0;

            SetTextureFormat(mipMapCount, format, flags);
        }

        private void CreateHeaderVolume(int mipMapCount, int depth, BitmapFormat format, BitmapFlags flags)
        {
            Flags |= DDSFlags.Depth;
            Caps |= DDSComplexityFlags.Complex;
            Caps2 |= DDSSurfaceInfoFlags.Volume;
            Depth = depth;
            SetTextureFormat(mipMapCount, format, flags);
        }

        private void CreateHeaderCubemap(int mipMapCount, BitmapFormat format, BitmapFlags flags)
        {
            Caps |= DDSComplexityFlags.Complex;
            Caps2 |= DDSSurfaceInfoFlags.CubeMapAllFaces;

            SetTextureFormat(mipMapCount, format, flags);
        }

        private void SetTextureFormat(int mipMapCount, BitmapFormat format, BitmapFlags flags)
        {
            if (mipMapCount > 0)
            {
                MipMapCount = mipMapCount;
                Flags |= DDSFlags.MipMapCount;
            }
            else
                MipMapCount = 0;

            if (flags.HasFlag(BitmapFlags.Compressed))
            {
                Flags |= DDSFlags.LinearSize;
                int blockSize = BitmapFormatUtils.GetBlockSize(format);
                int blockDimension = BitmapFormatUtils.GetBlockDimension(format);
                var nearestWidth = blockDimension * ((Height + (blockDimension - 1)) / blockDimension);
                var nearestHeight = blockDimension * ((Width + (blockDimension - 1)) / blockDimension); ;
                PitchOrLinearSize = (nearestWidth * nearestHeight / 16) * blockSize;
            }
            else
            {
                Flags |= DDSFlags.Pitch;
                int bitsPerPixel = BitmapFormatUtils.GetBitsPerPixel(format);
                PitchOrLinearSize = (Width * bitsPerPixel + 7) / 8;
            }

            PixelFormat = new PixelFormat(format, flags);
        }
    }   

    public class PixelFormat : BaseHeader
    {
        public readonly int Size = 0x20;
        public DDSPixelFormatFlags Flags;
        public uint FourCC = 0;
        public int RGBBitCount = 0;
        public uint RBitMask = 0;
        public uint GBitMask = 0;
        public uint BBitMask = 0;
        public uint ABitMask = 0;

        public PixelFormat(BitmapFormat format, BitmapFlags flags)
        {
            SetTextureFormat(format, flags);
        }

        public void SetTextureFormat(BitmapFormat format, BitmapFlags flags)
        {
            if (flags.HasFlag(BitmapFlags.Compressed))
            {
                Flags |= DDSPixelFormatFlags.Compressed;
                switch (format)
                {
                    case BitmapFormat.Dxt5:
                        Flags |= DDSPixelFormatFlags.FourCC;
                        FourCC = 0x35545844;
                        break;

                    case BitmapFormat.Dxt3:
                        Flags |= DDSPixelFormatFlags.FourCC;
                        FourCC = 0x33545844;
                        break;

                    case BitmapFormat.Dxt1:
                        Flags |= DDSPixelFormatFlags.FourCC;
                        FourCC = 0x31545844;
                        break;

                    case BitmapFormat.Dxn:
                        Flags |= DDSPixelFormatFlags.FourCC;
                        FourCC = 0x32495441;
                        break;

                    default:
                        throw new Exception($"Unsupported bitmap format {format}");
                }
            }
            else
            {
                Flags |= DDSPixelFormatFlags.RGB;
                RGBBitCount = BitmapFormatUtils.GetBitsPerPixel(format);

                switch (format)
                {
                    case BitmapFormat.A8:
                    case BitmapFormat.AY8:
                        ABitMask = 0xFF;
                        Flags |= DDSPixelFormatFlags.Alpha;
                        break;

                    case BitmapFormat.Y8:
                        RBitMask = 0xFF;
                        Flags |= DDSPixelFormatFlags.Luminance;
                        break;

                    case BitmapFormat.A8Y8:
                        Flags |= DDSPixelFormatFlags.RGB;
                        RBitMask = 0x00FF; ABitMask = 0xFF00;
                        break;

                    case BitmapFormat.R5G6B5:
                        Flags |= DDSPixelFormatFlags.RGB;
                        RBitMask = 0xF800; GBitMask = 0x07E0; BBitMask = 0x001F;
                        break;

                    case BitmapFormat.A1R5G5B5:
                        Flags |= DDSPixelFormatFlags.RGB;
                        RBitMask = 0x7C00; GBitMask = 0x03E0; BBitMask = 0x001F; ABitMask = 0x8000;
                        break;

                    case BitmapFormat.A4R4G4B4:
                        Flags |= DDSPixelFormatFlags.RGB;
                        RBitMask = 0xF000; GBitMask = 0x0F00; BBitMask = 0x00F0; ABitMask = 0x000F;
                        break;

                    case BitmapFormat.A4R4G4B4Font:
                        Flags |= DDSPixelFormatFlags.RGB;
                        RBitMask = 0xF000; GBitMask = 0x0F00; BBitMask = 0x00F0; ABitMask = 0x000F;
                        break;

                    case BitmapFormat.A8R8G8B8:
                        Flags |= DDSPixelFormatFlags.RGB;
                        RBitMask = 0x00FF0000; GBitMask = 0x0000FF00; BBitMask = 0x000000FF; ABitMask = 0xFF000000;
                        break;

                    case BitmapFormat.X8R8G8B8:
                        Flags |= DDSPixelFormatFlags.RGB;
                        RBitMask = 0x00FF0000; GBitMask = 0x0000FF00; BBitMask = 0x000000FF;
                        break;

                    case BitmapFormat.V8U8:
                        RBitMask = 0xFF00; GBitMask = 0x00FF;
                        Flags |= DDSPixelFormatFlags.BumpDUDV;
                        break;

                    case BitmapFormat.A32B32G32R32F:
                        Flags |= DDSPixelFormatFlags.FourCC;
                        FourCC = 0x74;
                        break;
                    case BitmapFormat.A16B16G16R16F:
                        Flags |= DDSPixelFormatFlags.FourCC;
                        FourCC = 0x71;
                        break;
                    default:
                        throw new Exception($"Unsupported bitmap format {format}");
                }
            }
        }

        /// <summary>
        /// Write the DDS Pixel Format Header
        /// </summary>
        /// <param name="writer"></param>
        public override void Write(EndianWriter writer)
        {
            writer.Format = EndianFormat.LittleEndian;
            writer.Write(Size);
            writer.Write((int)Flags);
            writer.Write(FourCC);
            writer.Write(RGBBitCount);
            writer.Write(RBitMask);
            writer.Write(GBitMask);
            writer.Write(BBitMask);
            writer.Write(ABitMask);
        }

        /// <summary>
        /// Read the DDS Pixel Format Header
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>Valid header</returns>
        public override bool Read(EndianReader reader)
        {
            reader.Format = EndianFormat.LittleEndian;
            if(reader.ReadInt32() != Size)
                return false;
            else
            {
                Flags = (DDSPixelFormatFlags)reader.ReadInt32();
                FourCC = reader.ReadUInt32();
                RGBBitCount = reader.ReadInt32();
                RBitMask = reader.ReadUInt32();
                GBitMask = reader.ReadUInt32();
                BBitMask = reader.ReadUInt32();
                ABitMask = reader.ReadUInt32();
                return true;
            }
        }
    }
   
}
