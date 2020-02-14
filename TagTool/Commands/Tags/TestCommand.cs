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


            //
            // Insert what test command you want below
            //


            var file = new FileInfo(@"D:\halo\maps\halo3\guardian.map");

            var cache = GameCache.Open(file);

            using (var stream = cache.OpenCacheRead())
            {
                var bitmapTag = cache.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\bitmaps\assault_rifle", "bitm");
                bitmapTag = cache.TagCache.GetTag(@"objects\weapons\rifle\battle_rifle\bitmaps\numbers_plate", "bitm");
                var bitmap = cache.Deserialize<Bitmap>(stream, bitmapTag);

                var imageIndex = 0;
                var level = 1;

                var image = bitmap.Images[imageIndex];

                if (image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
                {
                    BitmapTextureInterleavedInteropResource resource = cache.ResourceCache.GetBitmapTextureInterleavedInteropResource(bitmap.InterleavedResources[image.InterleavedTextureIndex1]);
                    BitmapTextureInteropDefinition definition;

                    int pairIndex = 0;
                    
                    if(image.InterleavedTextureIndex2 > 0)
                    {
                        definition = resource.Texture.Definition.Bitmap2;
                        pairIndex = 1;   
                    }
                    else
                    {
                        definition = resource.Texture.Definition.Bitmap1;
                    }
                    TestBitmapConverter(resource.Texture.Definition.PrimaryResourceData.Data, resource.Texture.Definition.SecondaryResourceData.Data, definition, bitmap, imageIndex, level, true, pairIndex);
                }
                else
                {
                    BitmapTextureInteropResource resource = cache.ResourceCache.GetBitmapTextureInteropResource(bitmap.Resources[imageIndex]);
                    TestBitmapConverter(resource.Texture.Definition.PrimaryResourceData.Data, resource.Texture.Definition.SecondaryResourceData.Data, resource.Texture.Definition.Bitmap, bitmap, imageIndex, level, false, 0);
                }
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

        public void TestBitmapConverter(byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, int level, bool isPaired, int pairIndex)
        {
            var d3dFormat = definition.D3DFormat;
            var isTiled = Direct3D.D3D9x.D3D.IsTiled(d3dFormat);

            int targetLevel = level;

            byte[] data;

            if (definition.HighResInSecondaryResource > 0 && targetLevel == 0)
                data = secondaryData;
            else
                data = primaryData;

            if (data == null)
                return;

            uint alignedWidth = (uint)definition.Width >> targetLevel;
            uint alignedHeight = (uint)definition.Height >> targetLevel;
            uint alignedDepth = definition.Depth;
            var gpuFormat = XboxGraphics.XGGetGpuFormat(d3dFormat);
            uint bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(gpuFormat);
            uint blockWidth = 0;
            uint blockHeight = 0;
            
            Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedWidth, ref alignedHeight, ref alignedDepth, bitsPerPixel, gpuFormat, 0, isTiled);

            XboxGraphics.XGGetBlockDimensions(gpuFormat, ref blockWidth, ref blockHeight);

            uint texelPitch = blockWidth * blockHeight * bitsPerPixel / 8;

            uint size = alignedWidth * alignedHeight * bitsPerPixel / 8;
            uint tileSize = 32 * 32 * blockWidth * blockHeight * bitsPerPixel / 8;

            //
            // Offset data to the right surface
            //

            int tileOffset = 0;
            if (isPaired)
            {
                if (pairIndex > 0)
                {
                    tileOffset = (int)tileSize / 2; // hacks?
                }
            }

            if (targetLevel != 0)
            {
                tileOffset += (int)tileSize; // ultra hack for now, gets the next tile
            }

            if (tileOffset > 0)
            {
                byte[] result = new byte[size];
                Array.Copy(data, tileOffset, result, 0, size);
                data = result;
            }

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

            // get surface offset and extract rectangle



        }
    }



}

