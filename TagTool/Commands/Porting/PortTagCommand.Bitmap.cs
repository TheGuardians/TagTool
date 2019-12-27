using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Bitmaps;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using TagTool.Bitmaps.Converter;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private Bitmap ConvertBitmap(CacheFile.IndexItem blamTag, Bitmap bitmap, Dictionary<ResourceLocation, Stream> resourceStreams)
        {
            bitmap.Flags = BitmapRuntimeFlags.UsingTagInteropAndTagResource;
            bitmap.UnknownB4 = 0;

            if (BlamCache.Version == CacheVersion.HaloReach)
            {
                bitmap.TightBoundsOld = bitmap.TightBoundsNew;
                
                foreach(var image in bitmap.Images)
                {
                    // For all formats above #38 (reach DXN, CTX1, DXT3a_mono, DXT3a_alpha, DXT5a_mono, DXT5a_alpha, DXN_mono_alpha), subtract 5 to match with H3/ODST/HO enum
                    if (image.Format >= (BitmapFormat)38)
                        image.Format = image.Format - 5;
                }
            }

            //
            // For each bitmaps, apply conversion process and create a new list of resources that will replace the one from H3.
            //

            var tagName = blamTag.Name;
            var resourceList = new List<TagResourceReference>();
            for (int i = 0; i < bitmap.Images.Count(); i++)
            {
                var resource = ConvertBitmap(bitmap, resourceStreams, i, tagName);
                if (resource == null)
                    return null;
                TagResourceReference bitmapResource = new TagResourceReference
                {
                    HaloOnlinePageableResource = resource
                };
                resourceList.Add(bitmapResource);
            }

            bitmap.Resources = resourceList;
            bitmap.InterleavedResources = null;

            //fixup for HO expecting 6 sequences in sensor_blips bitmap
            if (tagName == "ui\\chud\\bitmaps\\sensor_blips")
            {
                bitmap.Sequences.Add(bitmap.Sequences[3]);
                bitmap.Sequences.Add(bitmap.Sequences[3]);
            }

            return bitmap;
        }
        
        private PageableResource ConvertBitmap(Bitmap bitmap, Dictionary<ResourceLocation, Stream> resourceStreams, int imageIndex, string tagName)
        {
            var image = bitmap.Images[imageIndex];
            BaseBitmap baseBitmap = BitmapConverter.ConvertGen3Bitmap(BlamCache, bitmap, imageIndex, BlamCache.Version);

            if (baseBitmap == null)
                return null;

            // fix type enum
            if (baseBitmap.Type == BitmapType.Array)
                baseBitmap.Type = BitmapType.Texture3D;

            SetTagData(baseBitmap, image);
            var dataSize = baseBitmap.Data.Length;

            var resource = new PageableResource
            {
                Page = new RawPage(),
                Resource = new TagResourceGen3
                {
                    ResourceFixups = new List<TagResourceGen3.ResourceFixup>(),
                    ResourceDefinitionFixups = new List<TagResourceGen3.ResourceDefinitionFixup>(),
                    ResourceType = TagResourceTypeGen3.Bitmap,
                    Unknown2 = 1
                }
            };

            using (var dataStream = new MemoryStream(baseBitmap.Data))
            {
                var bitmapResource = new TagResourceReference
                {
                    HaloOnlinePageableResource = resource,
                };
                var resourceContext = new ResourceSerializationContext(CacheContext, resource);

                // Create new definition
                var resourceDefinition = new BitmapTextureInteropResource
                {
                    Texture = new TagStructureReference<BitmapTextureInteropResource.BitmapDefinition>
                    {
                        Definition = new BitmapTextureInteropResource.BitmapDefinition
                        {
                            PrimaryResourceData = new TagData(),
                            SecondaryResourceData = new TagData(),
                        }
                    }
                };

                SetResourceDefinitionData(baseBitmap, image, resourceDefinition.Texture.Definition);

                //
                // Serialize the new resource definition
                //

                var location = bitmap.Usage == 2 ?
                    ResourceLocation.TexturesB : // bump maps
                    ResourceLocation.Textures; // everything else

                resource.ChangeLocation(location);

                if (resource == null)
                    throw new ArgumentNullException("resource");

                if (!dataStream.CanRead)
                    throw new ArgumentException("The input stream is not open for reading", "dataStream");

                var cache = CacheContext.GetResourceCache(location);

                if (!resourceStreams.ContainsKey(location))
                {
                    resourceStreams[location] = FlagIsSet(PortingFlags.Memory) ?
                        new MemoryStream() :
                        (Stream)CacheContext.OpenResourceCacheReadWrite(location);

                    if (FlagIsSet(PortingFlags.Memory))
                        using (var resourceStream = CacheContext.OpenResourceCacheRead(location))
                            resourceStream.CopyTo(resourceStreams[location]);
                }

                dataSize = (int)(dataStream.Length - dataStream.Position);
                var data = new byte[dataSize];
                dataStream.Read(data, 0, dataSize);

                resource.Page.Index = cache.Add(resourceStreams[location], data, out uint compressedSize);
                resource.Page.CompressedBlockSize = compressedSize;
                resource.Page.UncompressedBlockSize = (uint)dataSize;
                resource.DisableChecksum();

                CacheContext.Serializer.Serialize(resourceContext, resourceDefinition);
            }

            return resource;
        }

        private void SetTagData(BaseBitmap bitmap, Bitmap.Image image)
        {
            image.Width = (short) bitmap.Width;
            image.Height = (short) bitmap.Height;
            image.Depth = (sbyte) bitmap.Depth;
            image.Format = bitmap.Format;
            image.Type = bitmap.Type;
            image.MipmapCount = (sbyte)bitmap.MipMapCount;
            image.DataSize = bitmap.Data.Length;
            image.XboxFlags = BitmapFlagsXbox.None;
            image.Flags = bitmap.Flags;
            
            if (image.Format == BitmapFormat.Dxn)
                image.Flags |= BitmapFlags.Unknown3;
            
        }

        private void SetResourceDefinitionData(BaseBitmap bitmap, Bitmap.Image image, BitmapTextureInteropResource.BitmapDefinition definition)
        {
            definition.PrimaryResourceData = new TagData(bitmap.Data.Length, new CacheResourceAddress(CacheResourceAddressType.Resource, 0));
            definition.Bitmap = new BitmapTextureInteropDefinition
            {
                Width = (short)bitmap.Width,
                Height = (short)bitmap.Height,
                Depth = (byte)bitmap.Depth,
                MipmapCount = (byte)(bitmap.MipMapCount + 1),
                BitmapType = bitmap.Type,
                D3DFormat = (D3DFormat)GetUnusedFormat(bitmap.Format),
                Format = bitmap.Format,
                Curve = image.Curve,
                Flags = bitmap.Flags
            };
        }

        private int GetUnusedFormat(BitmapFormat format)
        {
            int result = 0;
            switch (format)
            {
                case BitmapFormat.A8:
                    result = 0x0000001C;
                    break;

                case BitmapFormat.Y8:
                    result = 0x00000032;
                    break;

                case BitmapFormat.A8Y8:
                    result = 0x00000033;
                    break;

                case BitmapFormat.A8R8G8B8:
                    result = 0x00000015;
                    break;

                case BitmapFormat.A16B16G16R16F:
                    result = 0x00000071;
                    break;

                case BitmapFormat.A32B32G32R32F:
                    result = 0x00000074;
                    break;

                case BitmapFormat.Dxt1:
                    result = (int)DdsFourCc.FromString("DXT1");
                    break;

                case BitmapFormat.Dxt3:
                    result = (int)DdsFourCc.FromString("DXT3");
                    break;

                case BitmapFormat.Dxt5:
                    result = (int)DdsFourCc.FromString("DXT5");
                    break;

                case BitmapFormat.Dxn:
                    result = (int)DdsFourCc.FromString("ATI2");
                    break;

            }

            return result;
        }
    }
}