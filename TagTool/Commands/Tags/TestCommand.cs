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


            var file = new FileInfo(Path.Combine(mapFilesFolder.FullName, @"guardian.map"));

            var cache = GameCache.Open(file);

            using (var stream = cache.OpenCacheRead())
            {
                var bitmapTag = cache.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\bitmaps\assault_rifle", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\color_white", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\default_dynamic_cube_map", "bitm");
                bitmapTag = cache.TagCache.GetTag(@"fx\contrails\_bitmaps\wispy_trail", "bitm"); // doesnt work properly
                bitmapTag = cache.TagCache.GetTag(@"fx\decals\_bitmaps\sword_impact_medium_bump", "bitm"); // looks fine
                bitmapTag = cache.TagCache.GetTag(@"fx\decals\breakable_surfaces\glass_crack", "bitm"); //doesnt work properly

                
                var bitmap = cache.Deserialize<Bitmap>(stream, bitmapTag);

                var imageIndex = 0;
                

                var image = bitmap.Images[imageIndex];

                if (image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
                {
                    BitmapTextureInterleavedInteropResource resource = cache.ResourceCache.GetBitmapTextureInterleavedInteropResource(bitmap.InterleavedResources[image.InterleavedTextureIndex1]);
                    BitmapTextureInteropDefinition definition;

                    int pairIndex = 0;

                    if (image.InterleavedTextureIndex2 > 0)
                    {
                        definition = resource.Texture.Definition.Bitmap2;
                        pairIndex = 1;
                    }
                    else
                    {
                        definition = resource.Texture.Definition.Bitmap1;
                    }
                    TestBitmapConverter2(resource.Texture.Definition.PrimaryResourceData.Data, resource.Texture.Definition.SecondaryResourceData.Data, definition, bitmap, imageIndex, true, pairIndex);
                }
                else
                {
                    BitmapTextureInteropResource resource = cache.ResourceCache.GetBitmapTextureInteropResource(bitmap.Resources[imageIndex]);
                    TestBitmapConverter2(resource.Texture.Definition.PrimaryResourceData.Data, resource.Texture.Definition.SecondaryResourceData.Data, resource.Texture.Definition.Bitmap, bitmap, imageIndex, false, 0);
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

        public void DumpBitmapDDS(string filename, byte[] data, uint width, uint height, uint depth, uint mipmapCount, Bitmap.Image image)
        {
            var file = new FileInfo($"Bitmaps\\{filename}.dds");
            file.Directory.Create();
            using(var stream = file.OpenWrite())
            using(var writer = new EndianWriter(stream))
            {
                DDSHeader header = new DDSHeader(width, height, depth, mipmapCount, image.Format, image.Type, image.Flags);
                DDSFile ddsFile = new DDSFile(header, data);
                ddsFile.Write(writer);
            }
        }

        public void TestBitmapConverter2(byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, bool isPaired, int pairIndex)
        {
            byte[] data;
            using(var dataStream = new MemoryStream())
            {
                if (definition.HighResInSecondaryResource > 0)
                {
                    dataStream.Write(secondaryData, 0, secondaryData.Length);
                    StreamUtil.Align(dataStream, 0x1000);

                    if (primaryData != null)
                    {
                        dataStream.Write(primaryData, 0, primaryData.Length);
                        StreamUtil.Align(dataStream, 0x1000);
                    }
                }
                else
                {
                    dataStream.Write(primaryData, 0, primaryData.Length);
                    StreamUtil.Align(dataStream, 0x1000);
                }
                StreamUtil.Align(dataStream, 0x4000);
                data = dataStream.ToArray();
            }
            


            if (data == null)
                return;

            using(var result = new MemoryStream())
            {
                int mipLevelCount = definition.MipmapCount;
                int layerCount = definition.BitmapType == BitmapType.CubeMap ? 6 : definition.Depth;

                for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
                {
                    for (int mipLevel = 0; mipLevel < mipLevelCount; mipLevel++)
                    {
                        ConvertBitmapTest(result, data, definition, bitmap, imageIndex, mipLevel, layerIndex, isPaired, pairIndex);
                    }
                }

                var resultData = result.ToArray();

                DumpBitmapDDS($"bitmap_final", resultData, (uint)definition.Width, (uint)definition.Height, definition.Depth, (uint)definition.MipmapCount, bitmap.Images[imageIndex]);
            }
        }

        public void ConvertBitmapTest(Stream resultStream, byte[] data, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, int level, int layerIndex, bool isPaired, int pairIndex)
        {
            var d3dFormat = definition.D3DFormat;
            var isTiled = Direct3D.D3D9x.D3D.IsTiled(d3dFormat);

            uint blockWidth = 0;
            uint blockHeight = 0;
            uint alignedWidth = (uint)definition.Width >> level;
            uint alignedHeight = (uint)definition.Height >> level;
            uint alignedDepth = definition.Depth;
            var gpuFormat = XboxGraphics.XGGetGpuFormat(d3dFormat);
            uint bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(gpuFormat);

            XboxGraphics.XGGetBlockDimensions(gpuFormat, out blockWidth, out blockHeight);
            var textureType = BitmapUtils.GetXboxBitmapD3DTextureType(definition);
            Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedWidth, ref alignedHeight, ref alignedDepth, bitsPerPixel, gpuFormat, textureType, isTiled);

            // for our purpose aligned height should be at least 4 blocks (extracting mips)
            if (alignedHeight < 4 * blockHeight)
                alignedHeight = 4 * blockHeight;

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

            uint levelOffset = BitmapUtils.GetXboxBitmapLevelOffset(definition, layerIndex, level);
            Console.WriteLine($"Level: {level}, Offset: 0x{levelOffset:X04}");

            tileOffset += (int)levelOffset;

            if (tileOffset > 0)
            {
                byte[] result = new byte[size];
                Array.Copy(data, tileOffset, result, 0, size);
                data = result;
            }

            //DumpBitmapDDS($"raw_bitmap_{targetLevel}", data, alignedWidth, alignedHeight, alignedDepth, bitmap.Images[imageIndex]);

            //DumpBitmapDDS($"raw_bitmap_endian_swapped_{targetLevel}", data, alignedWidth, alignedHeight, alignedDepth, bitmap.Images[imageIndex]);

            // dump dds here
            uint nBlockWidth;
            uint nBlockHeight;
            if (isTiled)
            {
                //
                // Untile texture, assumes input is a square texture
                //

                byte[] result = new byte[size];

                nBlockWidth = alignedWidth / blockWidth;
                nBlockHeight = alignedHeight / blockHeight;
                for (int i = 0; i < nBlockHeight; i++)
                {
                    for (int j = 0; j < nBlockWidth; j++)
                    {
                        uint offset = (uint)((i * nBlockWidth) + j);
                        uint x = XboxGraphics.XGAddress2DTiledX(offset, nBlockWidth, texelPitch);
                        uint y = XboxGraphics.XGAddress2DTiledY(offset, nBlockWidth, texelPitch);
                        int sourceIndex = (int)(((i * nBlockWidth) * texelPitch) + (j * texelPitch));
                        int destinationIndex = (int)(((y * nBlockWidth) * texelPitch) + (x * texelPitch));

                        if (destinationIndex >= result.Length)
                            continue;

                        Array.Copy(data, sourceIndex, result, destinationIndex, texelPitch);
                    }
                }
                data = result;
            }

            //DumpBitmapDDS($"bitmap_untiled_{level}", data, alignedWidth, alignedHeight, alignedDepth, 1 bitmap.Images[imageIndex]);

            // get surface offset and extract rectangle
            XboxGraphics.XGPOINT point = new XboxGraphics.XGPOINT();

            // should find a condition to not call this (level > 0) is not good enough
            if (definition.MipmapCount > 1)
                XboxGraphics.GetMipTailLevelOffsetCoords((uint)definition.Width, (uint)definition.Height, definition.Depth, (uint)level, gpuFormat, false, false, point);

            // use the point to extract the right rectangle out of the texture
            Console.WriteLine($"Texture position in tile x:{point.X}, y:{point.Y}");


            uint logWidth = Direct3D.D3D9x.D3D.Log2Ceiling(definition.Width - 1);
            uint logHeight = Direct3D.D3D9x.D3D.Log2Ceiling(definition.Height - 1);
            uint logDepth = Direct3D.D3D9x.D3D.Log2Ceiling(definition.Depth - 1);
            // find next ceiling power of two, align on block size
            int logLevelWidth = (int)(logWidth - level);
            int logLevelHeight = (int)(logHeight - level);

            if (logLevelHeight < 0) logLevelHeight = 0;
            if (logLevelWidth < 0) logLevelWidth = 0;

            int levelWidth = 1 << logLevelWidth;
            int levelHeight = 1 << logLevelHeight;

            if (levelWidth % blockWidth != 0)
                levelWidth = (int)(levelWidth + blockWidth - levelWidth % blockWidth);

            if (levelHeight % blockHeight != 0)
                levelHeight = (int)(levelHeight + blockHeight - levelHeight % blockHeight);

            byte[] finalData = new byte[levelWidth * levelHeight * bitsPerPixel >> 3];

            nBlockWidth = (uint)(levelWidth / blockWidth);
            nBlockHeight = (uint)(levelHeight / blockHeight);

            uint sliceBlockWidth = alignedWidth / blockWidth;

            // skip these loops if the bitmap is already the proper format
            if (point.X != 0 || point.Y != 0 || finalData.Length != data.Length)
            {
                for (int i = 0; i < nBlockHeight; i++)
                {
                    for (int j = 0; j < nBlockWidth; j++)
                    {
                        uint offset = (uint)(((i + point.Y) * sliceBlockWidth) + j + point.X) * texelPitch;
                        uint destOffset = (uint)((i * nBlockWidth) + j) * texelPitch;
                        Array.Copy(data, offset, finalData, destOffset, texelPitch);
                    }
                }
            }
            else
            {
                Array.Copy(data, 0, finalData, 0, data.Length);
            }

            XboxGraphics.XGEndianSwapSurface(d3dFormat, finalData);

            resultStream.Write(finalData, 0, finalData.Length);
        }

        public void TestBitmapConverter(byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, int level, bool isPaired, int pairIndex)
        {
            byte[] data;

            if (definition.HighResInSecondaryResource > 0)
            {
                var alignedSecondarySize = (secondaryData.Length + 0xFFF) & ~0xFFF;
                var alignedPrimarySize = 0;

                if(primaryData != null)
                    alignedPrimarySize = (primaryData.Length + 0xFFF) & ~0xFFF;

                byte[] result = new byte[alignedPrimarySize + alignedSecondarySize];
                Array.Copy(secondaryData, 0, result, 0, secondaryData.Length);

                if (primaryData != null)
                    Array.Copy(primaryData, 0, result, alignedSecondarySize, primaryData.Length);

                data = result;
            }
            else
            {
                var alignedPrimarySize = (primaryData.Length + 0xFFF) & ~0xFFF;
                byte[] result = new byte[alignedPrimarySize];
                Array.Copy(primaryData, 0, result, 0, primaryData.Length);
                data = result;
            }
                

            if (data == null)
                return;


            var d3dFormat = definition.D3DFormat;
            var isTiled = Direct3D.D3D9x.D3D.IsTiled(d3dFormat);

            uint blockWidth = 0;
            uint blockHeight = 0;
            uint alignedWidth = (uint)definition.Width >> level;
            uint alignedHeight = (uint)definition.Height >> level;
            uint alignedDepth = definition.Depth;
            var gpuFormat = XboxGraphics.XGGetGpuFormat(d3dFormat);
            uint bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(gpuFormat);
            
            XboxGraphics.XGGetBlockDimensions(gpuFormat, out blockWidth, out blockHeight);
            var textureType = BitmapUtils.GetXboxBitmapD3DTextureType(definition);
            Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedWidth, ref alignedHeight, ref alignedDepth, bitsPerPixel, gpuFormat, textureType, isTiled);

            // for our purpose aligned height should be at least 4 blocks (extracting mips)
            if (alignedHeight < 4 * blockHeight)
                alignedHeight = 4 * blockHeight;

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

            uint levelOffset = BitmapUtils.GetXboxBitmapLevelOffset(definition, 0, level);
            Console.WriteLine($"Level: {level}, Offset: 0x{levelOffset:X04}");

            tileOffset += (int)levelOffset;

            if (tileOffset > 0)
            {
                byte[] result = new byte[size];
                Array.Copy(data, tileOffset, result, 0, size);
                data = result;
            }
            
            //DumpBitmapDDS($"raw_bitmap_{targetLevel}", data, alignedWidth, alignedHeight, alignedDepth, bitmap.Images[imageIndex]);

            XboxGraphics.XGEndianSwapSurface(d3dFormat, data);

            //DumpBitmapDDS($"raw_bitmap_endian_swapped_{targetLevel}", data, alignedWidth, alignedHeight, alignedDepth, bitmap.Images[imageIndex]);

            // dump dds here
            uint nBlockWidth;
            uint nBlockHeight;
            if (isTiled)
            {
                //
                // Untile texture, assumes input is a square texture
                //

                byte[] result = new byte[size];

                nBlockWidth = alignedWidth / blockWidth;
                nBlockHeight = alignedHeight / blockHeight;
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

                
            }

            DumpBitmapDDS($"bitmap_untiled_{level}", data, alignedWidth, alignedHeight, alignedDepth, 1, bitmap.Images[imageIndex]);

            // get surface offset and extract rectangle
            XboxGraphics.XGPOINT point = new XboxGraphics.XGPOINT();

            // should find a condition to not call this (level > 0) is not good enough
            if(definition.MipmapCount > 1)
                XboxGraphics.GetMipTailLevelOffsetCoords((uint)definition.Width, (uint)definition.Height, definition.Depth, (uint)level, gpuFormat, false, false, point);

            // use the point to extract the right rectangle out of the texture
            Console.WriteLine($"Texture position in tile x:{point.X}, y:{point.Y}");


            uint logWidth = Direct3D.D3D9x.D3D.Log2Ceiling(definition.Width - 1);
            uint logHeight = Direct3D.D3D9x.D3D.Log2Ceiling(definition.Height - 1);
            uint logDepth = Direct3D.D3D9x.D3D.Log2Ceiling(definition.Depth - 1);
            // find next ceiling power of two, align on block size
            uint logLevelWidth = (uint)(logWidth - level);
            uint logLevelHeight = (uint)(logHeight - level);

            int levelWidth = 1 << (int)logLevelWidth;
            int levelHeight = 1 << (int)logLevelHeight;

            if (levelWidth % blockWidth != 0)
                levelWidth = (int)(levelWidth + blockWidth - levelWidth % blockWidth);

            if (levelHeight % blockHeight != 0)
                levelHeight = (int)(levelHeight + blockHeight - levelHeight % blockHeight);


            int actualWidth = definition.Width >> level;
            int actualHeight = definition.Height >> level;

            if (actualHeight < 1)
                actualHeight = 1;
            if (actualWidth < 1)
                actualWidth = 1;

            byte[] finalData = new byte[levelWidth * levelHeight * bitsPerPixel >> 3];

            nBlockWidth = (uint)(levelWidth / blockWidth);
            nBlockHeight = (uint)(levelHeight / blockHeight);

            uint sliceBlockWidth = alignedWidth / blockWidth;

            // skip these loops if the bitmap is already the proper format
            if(point.X != 0 || point.Y != 0 || finalData.Length != data.Length)
            {
                for (int i = 0; i < nBlockHeight; i++)
                {
                    for (int j = 0; j < nBlockWidth; j++)
                    {
                        uint offset = (uint)(((i + point.Y) * sliceBlockWidth) + j + point.X) * texelPitch;
                        uint destOffset = (uint)((i * nBlockWidth) + j) * texelPitch;
                        Array.Copy(data, offset, finalData, destOffset, texelPitch);
                    }
                }
            }
            else
            {
                finalData = data;
            }
            DumpBitmapDDS($"bitmap_trimmed_{level}", finalData, (uint)actualWidth, (uint)actualHeight, 1, 1, bitmap.Images[imageIndex]);
        }
    }



}

