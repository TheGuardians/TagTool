using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Bitmaps;
using TagTool.Tags.Resources;
using TagTool.Bitmaps.Utils;
using TagTool.Bitmaps.DDS;
using TagTool.Geometry;
using TagTool.BlamFile;
using TagTool.Tags.Definitions.Gen1;
using TagTool.Cache.HaloOnline;
using TagTool.Havok;
using System.Linq;
using System.IO.Compression;

namespace TagTool.Commands
{
    
    public class TestCommand : Command
    {
        GameCache Cache;

        public TestCommand(GameCache cache) : base(false, "Test", "Test", "Test", "Test")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            
            var mapFilesFolder = new DirectoryInfo(@"D:\Halo\Maps\Halo3");
            var outDir = new DirectoryInfo("CacheTest");
            if (!outDir.Exists)
                outDir.Create();

            var mapFiles = new List<FileInfo>() { new FileInfo(@"D:\halo online test\maps\tags.dat")};
            //mapFilesFolder.GetFiles("*.map"); // 

            //
            // Insert what test command you want below
            //


            var file = new FileInfo(@"D:\halo\maps\halo3\guardian.map");

            var cache = GameCache.Open(file);

            using (var stream = cache.OpenCacheRead())
            {
                var bitmapTag = cache.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\bitmaps\assault_rifle", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\color_white", "bitm");
                var bitmap = cache.Deserialize<Bitmap>(stream, bitmapTag);
                
                var resource = cache.ResourceCache.GetBitmapTextureInteropResource(bitmap.Resources[0]);

                TestBitmapConverter(resource, bitmap, 0);


                //var filename = bitmapTag.Name.Split('\\').Last();

                //generateXboxFiles(filename, primaryData, secondaryData, bitmapResource);

                return true;
                /*
                byte[] data = null;
                if (bitmapResource.HighResInSecondaryResource == 1)
                {
                    data = secondaryData;
                }
                else
                {
                    data = primaryData;
                }

                var format = XboxGraphics.XGGetGpuFormat(bitmapResource.D3DFormat);
                uint blockWidth = 0;
                uint blockHeight = 0;

                XboxGraphics.XGGetBlockDimensions(format, ref blockWidth, ref blockHeight);
                
                var bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(format);
                uint rowPitch = (uint)(((blockWidth * blockHeight * bitsPerPixel) >> 3) * bitmapResource.Width);
                var point = new XboxGraphics.XGPOINT();

                var test = XboxGraphics.GetMipTailLevelOffsetCoords((uint)bitmapResource.Width, (uint)bitmapResource.Height, (uint)bitmapResource.Depth, 2 , format, true, false, point);
                byte[] result = XboxGraphics.XGUntileTextureLevel((uint)bitmapResource.Width, (uint)bitmapResource.Height, 0, format, XboxGraphics.XGTILE.NONE, rowPitch, point, data, null);
                */
            }

            return true;
        }

        public void generateXboxFiles(string filename, byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition)
        {
            if(primaryData != null)
            {
                using (var stream = new FileStream($"XboxBitmaps/{filename}.primary", FileMode.Create))
                {
                    stream.Write(primaryData, 0, primaryData.Length);
                }
            }
            
            if(secondaryData != null)
            {
                using (var stream = new FileStream($"XboxBitmaps/{filename}.secondary", FileMode.Create))
                {
                    stream.Write(secondaryData, 0, secondaryData.Length);
                }
            }

            var serializer = new TagSerializer(CacheVersion.HaloOnline106708, EndianFormat.LittleEndian);

            using (var stream = new FileStream($"XboxBitmaps/{filename}.bitmapdefinition", FileMode.Create))
            using(var writer = new EndianWriter(stream, EndianFormat.LittleEndian))
            {
                var context = new DataSerializationContext(writer);
                serializer.Serialize(context, definition);
            }
        }

        public void DumpBitmapDDS(string filename, byte[] data, uint width, uint height, uint depth, Bitmap.Image image)
        {
            var file = new FileInfo($"Bitmaps\\{filename}.dds");
            using(var stream = file.OpenWrite())
            using(var writer = new EndianWriter(stream))
            {
                DDSHeader header = new DDSHeader(width, height, depth, 0, image.Format, image.Type, image.Flags);
                DDSFile ddsFile = new DDSFile(header, data);
                ddsFile.Write(writer);
            }
        }

        public void TestBitmapConverter(BitmapTextureInteropResource resource, Bitmap bitmap, int imageIndex)
        {
            var bitmapResourceDef = resource.Texture.Definition.Bitmap;
            var d3dFormat = bitmapResourceDef.D3DFormat;
            var isTiled = Direct3D.D3D9x.D3D.IsTiled(d3dFormat);

            // assuming we are getting level 0

            byte[] data;

            if (bitmapResourceDef.HighResInSecondaryResource > 0)
                data = resource.Texture.Definition.SecondaryResourceData.Data;
            else
                data = resource.Texture.Definition.PrimaryResourceData.Data;

            if (data == null)
                return;

            uint alignedWidth = (uint)bitmapResourceDef.Width;
            uint alignedHeight = (uint)bitmapResourceDef.Height;
            uint alignedDepth = bitmapResourceDef.Depth;
            var gpuFormat = XboxGraphics.XGGetGpuFormat(d3dFormat);
            uint bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(gpuFormat);
            uint blockWidth = 0;
            uint blockHeight = 0;
            

            Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedWidth, ref alignedHeight, ref alignedDepth, bitsPerPixel, gpuFormat, 0, isTiled);

            XboxGraphics.XGGetBlockDimensions(gpuFormat, ref blockWidth, ref blockHeight);

            uint texelPitch = blockWidth * blockHeight * bitsPerPixel / 8;

            DumpBitmapDDS("raw_bitmap", data, alignedWidth, alignedHeight, alignedDepth, bitmap.Images[imageIndex]);

            XboxGraphics.XGEndianSwapSurface(d3dFormat, data);

            DumpBitmapDDS("raw_bitmap_endian_swapped", data, alignedWidth, alignedHeight, alignedDepth, bitmap.Images[imageIndex]);

            // dump dds here

            if (isTiled)
            {
                //
                // Untile texture dumb way
                //

                byte[] result = new byte[data.Length];

                uint nBlockWidth = alignedWidth / blockWidth;
                uint nBlockHeight = alignedHeight / blockHeight;
                for (int i = 0; i < nBlockHeight; i++)
                {
                    for (int j = 0; j < nBlockWidth; j++)
                    {
                        uint offset = (uint)((i * nBlockWidth) + j);
                        uint x = XboxGraphics.XGAddress2DTiledX(offset, nBlockWidth, texelPitch);
                        uint y = XboxGraphics.XGAddress2DTiledY(offset, nBlockWidth, texelPitch);
                        int sourceIndex = (int)(((i * nBlockWidth) * texelPitch) + (j * texelPitch));
                        int destinationIndex = (int)(((y * nBlockWidth) * texelPitch) + (x * texelPitch));
                        Array.Copy(data, sourceIndex, result, destinationIndex, texelPitch);
                    }
                }

                data = result;

                DumpBitmapDDS("bitmap_untiled", data, alignedWidth, alignedHeight, alignedDepth, bitmap.Images[imageIndex]);
            }

            // get level offset and extract rectangle

        }






        public void PrintStringID(int set, int index, int tableIndex, string str, StringIDType type)
        {
            Console.WriteLine($"{tableIndex:X8}, {set:X2}, {index:X4}, {type.ToString()}, {str}");
        }

        public void PrintEnum(int index, string str)
        {
            Console.WriteLine($"string_id_{str} = 0x{index:X4},");
        }
    }



}

