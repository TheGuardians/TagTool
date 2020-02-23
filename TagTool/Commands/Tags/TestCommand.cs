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
                /*
                foreach(var tag in cache.TagCache.NonNull())
                {
                    if (tag.IsInGroup("bitm"))
                    {
                        var bitmap2 = cache.Deserialize<Bitmap>(stream, tag);
                        if(bitmap2.Images[0].Format == BitmapFormat.Dxt3aAlpha)
                        {
                            Console.WriteLine(tag.Name);
                        }
                    }
                }*/

                var bitmapTag = cache.TagCache.GetTag(@"objects\weapons\rifle\assault_rifle\bitmaps\assault_rifle", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\color_white", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\default_dynamic_cube_map", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"fx\decals\_bitmaps\sword_impact_medium_bump", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"fx\decals\breakable_surfaces\glass_crack", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"levels\multi\snowbound\bitmaps\cube_icecave_a_cubemap", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"fx\contrails\_bitmaps\wispy_trail", "bitm"); 
                //bitmapTag = cache.TagCache.GetTag(@"ui\chud\bitmaps\monitor_shield_meter", "bitm");
                bitmapTag = cache.TagCache.GetTag(@"ui\chud\bitmaps\elite_shield_flash_bleed", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"ui\chud\bitmaps\energy_meter_left", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"levels\shared\bitmaps\nature\water\water_ripples", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"levels\multi\guardian\bitmaps\guardian_manconnon_bump", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"fx\decals\ground_marks\_bitmaps\generic_foot_bump", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"objects\weapons\rifle\covenant_carbine\bitmaps\covenant_carbine_switch_illum", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"ui\chud\bitmaps\scopes\beam_rifle_scope", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"fx\decals\_bitmaps\blast_scorch_medium", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"fx\decals\_bitmaps\plasma_impact_medium", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"objects\vehicles\mongoose\bitmaps\wheels_alpha", "bitm"); 
                //bitmapTag = cache.TagCache.GetTag(@"objects\characters\masterchief\bitmaps\mp_markv_zbump", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"levels\multi\guardian\sky\bitmaps\guardian_canopy_leaves", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"objects\halograms\bitmaps\forerunner_holo_shapes", "bitm");
                //bitmapTag = cache.TagCache.GetTag(@"objects\halograms\070lc_waypoint_reveal\main_halogram\bitmaps\galaxy_middle", "bitm"); 
                //bitmapTag = cache.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\default_dynamic_cube_map", "bitm");
                bitmapTag = cache.TagCache.GetTag(@"shaders\default_bitmaps\bitmaps\gray_50_percent", "bitm");
                
                //TestConvertAllBitmaps(cache, stream);
                TestConvertBitmap(cache, stream, bitmapTag, 0);
            }
            return true;
        }

        private void TestConvertAllBitmaps(GameCache cache, Stream stream)
        {
            foreach(var bitmapTag in cache.TagCache.NonNull().Where(tag => tag.IsInGroup("bitm")))
                TestConvertBitmap(cache, stream, bitmapTag, 0);
        }

        private void TestConvertBitmap(GameCache cache, Stream stream, CachedTag bitmapTag, int imageIndex)
        {
            var bitmap = cache.Deserialize<Bitmap>(stream, bitmapTag);

            if (bitmap.Images.Count > 1)
                imageIndex = 1;

            var image = bitmap.Images[imageIndex];

            if (image.XboxFlags.HasFlag(BitmapFlagsXbox.UseInterleavedTextures))
            {
                BitmapTextureInterleavedInteropResource resource = cache.ResourceCache.GetBitmapTextureInterleavedInteropResource(bitmap.InterleavedResources[image.InterleavedTextureIndex1]);
                if (resource == null)
                    return;

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
                TestBitmapConverter2(bitmapTag, resource.Texture.Definition.PrimaryResourceData.Data, resource.Texture.Definition.SecondaryResourceData.Data, definition, bitmap, imageIndex, true, pairIndex);
            }
            else
            {
                BitmapTextureInteropResource resource = cache.ResourceCache.GetBitmapTextureInteropResource(bitmap.Resources[imageIndex]);
                if (resource == null)
                    return;

                TestBitmapConverter2(bitmapTag, resource.Texture.Definition.PrimaryResourceData.Data, resource.Texture.Definition.SecondaryResourceData.Data, resource.Texture.Definition.Bitmap, bitmap, imageIndex, false, 0);
            }
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
            using(var stream = file.Create())
            using(var writer = new EndianWriter(stream))
            {
                DDSHeader header = new DDSHeader(width, height, depth, mipmapCount, image.Format, image.Type, image.Flags);
                DDSFile ddsFile = new DDSFile(header, data);
                ddsFile.Write(writer);
            }
        }

        public void TestBitmapConverter2(CachedTag tag, byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, bool isPaired, int pairIndex)
        {
            if (primaryData == null && secondaryData == null)
                return;

            using(var result = new MemoryStream())
            {
                int mipLevelCount = definition.MipmapCount;
                int layerCount = definition.BitmapType == BitmapType.CubeMap ? 6 : definition.Depth;
                
                if (definition.BitmapType == BitmapType.Array && mipLevelCount > 1)
                {
                    mipLevelCount = 1;
                    definition.MipmapCount = 1;
                }


                for (int layerIndex = 0; layerIndex < layerCount; layerIndex++)
                {
                    for (int mipLevel = 0; mipLevel < mipLevelCount; mipLevel++)
                    {
                        ConvertBitmapTest(result, primaryData, secondaryData, definition, bitmap, imageIndex, mipLevel, layerIndex, isPaired, pairIndex);
                    }
                }

                var resultData = result.ToArray();

                var newFormat = BitmapUtils.GetEquivalentBitmapFormat(bitmap.Images[imageIndex].Format);
                bitmap.Images[imageIndex].Format = newFormat;
                definition.Format = newFormat;

                if (!BitmapUtils.IsCompressedFormat(newFormat))
                    bitmap.Images[0].Flags &= ~BitmapFlags.Compressed;

                DumpBitmapDDS($"{tag.Name.Replace("\\", "_")}", resultData, (uint)definition.Width, (uint)definition.Height, definition.Depth, (uint)definition.MipmapCount, bitmap.Images[imageIndex]);
            }
        }

        public void ConvertBitmapTest(Stream resultStream, byte[] primaryData, byte[] secondaryData, BitmapTextureInteropDefinition definition, Bitmap bitmap, int imageIndex, int level, int layerIndex, bool isPaired, int pairIndex)
        {
            byte[] data;
            uint levelOffset;

            var d3dFormat = definition.D3DFormat;
            var isTiled = Direct3D.D3D9x.D3D.IsTiled(d3dFormat);

            uint blockWidth;
            uint blockHeight;

            uint alignedWidth = (uint)definition.Width >> level;
            uint alignedHeight = (uint)definition.Height >> level;

            if (alignedWidth < 1) alignedWidth = 1;
            if (alignedHeight < 1) alignedHeight = 1;

            uint alignedDepth = definition.Depth;
            var gpuFormat = XboxGraphics.XGGetGpuFormat(d3dFormat);
            uint bitsPerPixel = XboxGraphics.XGBitsPerPixelFromGpuFormat(gpuFormat);

            XboxGraphics.XGGetBlockDimensions(gpuFormat, out blockWidth, out blockHeight);
            XboxGraphics.XGPOINT point = new XboxGraphics.XGPOINT();
            if (definition.MipmapCount > 1)
                XboxGraphics.GetMipTailLevelOffsetCoords((uint)definition.Width, (uint)definition.Height, definition.Depth, (uint)level, gpuFormat, false, false, point);

            // use the point to extract the right rectangle out of the texture
            Console.WriteLine($"Texture position in tile x:{point.X}, y:{point.Y}");

            var textureType = BitmapUtils.GetXboxBitmapD3DTextureType(definition);
            Direct3D.D3D9x.D3D.AlignTextureDimensions(ref alignedWidth, ref alignedHeight, ref alignedDepth, bitsPerPixel, gpuFormat, textureType, isTiled);

            if(level > 0)
            {
                // align to next power of two
                if (!Direct3D.D3D9x.D3D.IsPowerOfTwo((int)alignedWidth))
                {
                    alignedWidth = Direct3D.D3D9x.D3D.Log2Ceiling((int)alignedWidth);
                    if (alignedWidth < 0) alignedWidth = 0;
                    alignedWidth = 1u << (int)alignedWidth;
                }

                if (!Direct3D.D3D9x.D3D.IsPowerOfTwo((int)alignedHeight))
                {
                    alignedHeight = Direct3D.D3D9x.D3D.Log2Ceiling((int)alignedHeight);
                    if (alignedHeight < 0) alignedHeight = 0;
                    alignedHeight = 1u << (int)alignedHeight;
                }
            }

            // hacks when the point is outside of the first aligned texture, compute how many tiles you need and extract them (non-square only)
            if(point.X >= 32)
                alignedWidth *= (uint)(1 + point.X / 32);
            if(point.Y >= 32)
                alignedHeight *= (uint)(1 + point.Y / 32);

            uint texelPitch = blockWidth * blockHeight * bitsPerPixel / 8;
            uint size = alignedWidth * alignedHeight * bitsPerPixel / 8;

            // documentation says that each packed mip level should be aligned to 4KB, required to make untiling work smoothly
            size = (uint)((size + 0xFFF) & ~0xFFF);

            uint tileSize = 32 * 32 * blockWidth * blockHeight * bitsPerPixel / 8;

            int tileOffset = 0;

            if (isPaired)
            {
                if (pairIndex > 0)
                {
                    tileOffset = (int)tileSize / 2; // hacks? perhaps should be 1kb
                }
            }

            if (level == 0 && definition.HighResInSecondaryResource > 0)
            {
                levelOffset = BitmapUtils.GetXboxBitmapLevelOffset(definition, layerIndex, level);
                uint alignedSecondaryLength = (uint)((secondaryData.Length + 0xFFF) & ~0xFFF);
                data = new byte[alignedSecondaryLength];
                Array.Copy(secondaryData, 0, data, 0, secondaryData.Length);
            }
            else
            {
                if(definition.HighResInSecondaryResource > 0)
                {
                    levelOffset = BitmapUtils.GetXboxBitmapLevelOffset(definition, layerIndex, level);
                    uint alignedSecondaryLength = (uint)((secondaryData.Length + 0xFFF) & ~0xFFF);
                    levelOffset -= alignedSecondaryLength;
                }
                else
                {
                    levelOffset = BitmapUtils.GetXboxBitmapLevelOffset(definition, layerIndex, level);
                }
                uint alignedPrimaryLength = (uint)((primaryData.Length + 0xFFF) & ~0xFFF);
                data = new byte[alignedPrimaryLength];
                Array.Copy(primaryData, 0, data, 0, primaryData.Length);
            }
                
            
            Console.WriteLine($"Level: {level}, Offset: 0x{levelOffset:X04}");

            tileOffset += (int)levelOffset;

            byte[] tempResult = new byte[size];

            // check if data has enough memory for the requested, size, sometimes it does not (truncated to save memory)
            uint copySize = size;
            if (size + tileOffset >= data.Length)
                copySize = (uint)(data.Length - tileOffset);

            Array.Copy(data, tileOffset, tempResult, 0, copySize);
            data = tempResult;

            uint nBlockWidth;
            uint nBlockHeight;
            if (isTiled)
            {
                //
                // Untile texture
                //

                byte[] result = new byte[size];

                nBlockWidth = alignedWidth / blockWidth;
                nBlockHeight = alignedHeight / blockHeight;
                for (int i = 0; i < nBlockHeight; i++)
                {
                    for (int j = 0; j < nBlockWidth; j++)
                    {
                        int destinationIndex = (int)(i * nBlockWidth + j);  // offset in terms block
                        int destinationOffset = (int)(destinationIndex * texelPitch);
                        uint tiledIndex = XboxGraphics.XGAddress2DTiledOffset((uint)j, (uint)i, nBlockWidth, texelPitch); // returns offset in terms of block
                        uint tiledOffset = tiledIndex * texelPitch;
                        Array.Copy(data, tiledOffset, result, destinationOffset, texelPitch);
                    }
                }
                data = result;
            }

            // find level size aligned to block size

            int levelWidth = definition.Width >> level;
            int levelHeight = definition.Height >> level;

            if (levelWidth < 1)
                levelWidth = 1;
            if (levelHeight < 1)
                levelHeight = 1;

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
            XboxGraphics.XGEndianSwapSurface(d3dFormat, data);
            //DumpBitmapDDS($"bitmap_untiled_{level}", data, (uint)alignedWidth, (uint)alignedHeight, definition.Depth, 1, bitmap.Images[imageIndex]);
            uint actualWidth = (uint)definition.Width >> level;
            uint actualHeight = (uint)definition.Height >> level;

            if (actualWidth < 1)
                actualWidth = 1;
            if (actualHeight < 1)
                actualHeight = 1;

            finalData = BitmapUtils.ConvertXboxFormats(finalData, actualWidth, actualHeight, bitmap.Images[imageIndex].Format);

            resultStream.Write(finalData, 0, finalData.Length);
        }
    }
}

