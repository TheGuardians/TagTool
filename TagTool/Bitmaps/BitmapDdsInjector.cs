using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using TagTool.Tags;

namespace TagTool.Bitmaps
{
    public class BitmapDdsInjector
    {
        private HaloOnlineCacheContext CacheContext { get; }
        
        public BitmapDdsInjector(HaloOnlineCacheContext cacheContext)
        {
            CacheContext = cacheContext;
        }

        public void InjectDds(TagSerializer serializer, TagDeserializer deserializer, Bitmap bitmap, int imageIndex, Stream ddsStream, ResourceLocation location = ResourceLocation.Textures)
        {
            var resource = bitmap.Resources[imageIndex].Resource;
            var newResource = (resource == null);
            ResourceSerializationContext resourceContext;
            BitmapTextureInteropResource definition;
            if (newResource)
            {
                // Create a new resource reference
                resource = new PageableResource
                {
                    Page = new RawPage(),
                    Resource = new TagResource
                    {
                        ResourceFixups = new List<TagResource.ResourceFixup>(),
                        ResourceDefinitionFixups = new List<TagResource.ResourceDefinitionFixup>(),
                        Type = TagResourceType.Bitmap,
                        Unknown2 = 1
                    }
                };

                bitmap.Resources[imageIndex].Resource = resource;
                resourceContext = new ResourceSerializationContext(resource);
                definition = new BitmapTextureInteropResource
                {
                    Texture = new D3DPointer<BitmapTextureInteropResource.BitmapDefinition>
                    {
                        Definition = new BitmapTextureInteropResource.BitmapDefinition
                        {
                            Data = new TagData(),
                            UnknownData = new TagData(),
                        }
                    }
                };
            }
            else
            {
                // Deserialize the old definition
                resourceContext = new ResourceSerializationContext(resource);
                definition = deserializer.Deserialize<BitmapTextureInteropResource>(resourceContext);
            }
            if (definition.Texture == null || definition.Texture.Definition == null)
                throw new ArgumentException("Invalid bitmap definition");
            var texture = definition.Texture.Definition;
            var imageData = bitmap.Images[imageIndex];

            // Read the DDS header and modify the definition to match
            var dds = DdsHeader.Read(ddsStream);
            var dataSize = (int)(ddsStream.Length - ddsStream.Position);
            texture.Data = new TagData(dataSize, new CacheAddress(CacheAddressType.Resource, 0));
            texture.Width = (short)dds.Width;
            texture.Height = (short)dds.Height;
            texture.Depth = (sbyte)Math.Max(1, dds.Depth);
            texture.MipmapCount = (sbyte)Math.Max(1, dds.MipMapCount);
            texture.Type = BitmapDdsFormatDetection.DetectType(dds);
            texture.D3DFormatUnused = (int)((dds.D3D10Format != DxgiFormat.Bc5UNorm) ? dds.FourCc : DdsFourCc.FromString("ATI2"));
            texture.Format = BitmapDdsFormatDetection.DetectFormat(dds);

            // Set flags based on the format
            switch (texture.Format)
            {
                case BitmapFormat.Dxt1:
                case BitmapFormat.Dxt3:
                case BitmapFormat.Dxt5:
                case BitmapFormat.Dxn:
                    texture.Flags = BitmapFlags.Compressed;
                    break;
                default:
                    texture.Flags = BitmapFlags.None;
                    break;
            }
            if ((texture.Width & (texture.Width - 1)) == 0 && (texture.Height & (texture.Height - 1)) == 0)
                texture.Flags |= BitmapFlags.PowerOfTwoDimensions;

            // If creating a new image, then add a new resource, otherwise replace the existing one
            if (newResource)
            {
                resource.ChangeLocation(location);
                CacheContext.AddResource(resource, ddsStream);
            }
            else
                CacheContext.ReplaceResource(resource, ddsStream);

            // Serialize the new resource definition
            serializer.Serialize(resourceContext, definition);

            // Modify the image data in the bitmap tag to match the definition
            imageData.Width = texture.Width;
            imageData.Height = texture.Height;
            imageData.Depth = texture.Depth;
            imageData.Type = texture.Type;
            imageData.Format = texture.Format;
            imageData.Flags = texture.Flags;
            imageData.MipmapCount = (sbyte)(texture.MipmapCount - 1);
            imageData.DataOffset = texture.Data.Address.Offset;
            imageData.DataSize = texture.Data.Size;
            imageData.Curve = (BitmapImageCurve)texture.Curve;
        }
    }
}
