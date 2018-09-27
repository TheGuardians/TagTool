using TagTool.Cache;
using TagTool.IO;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.IO;
using System.Text;
using TagTool.Serialization;

namespace TagTool.Bitmaps
{
    public class BitmapDdsExtractor
    {
        private HaloOnlineCacheContext CacheContext { get; }

        public BitmapDdsExtractor(HaloOnlineCacheContext cacheContext)
        {
            CacheContext = cacheContext;
        }
        
        public void ExtractDds(Bitmap bitmap, int imageIndex, Stream outStream)
        {
            // TODO: Make sure 3D textures and cube maps work

            // Deserialize the resource definition and verify it
            var resource = bitmap.Resources[imageIndex];
            var resourceContext = new ResourceSerializationContext(resource.Resource);
            var definition = CacheContext.Deserializer.Deserialize<BitmapTextureInteropResource>(resourceContext);
            if (definition.Texture == null || definition.Texture.Definition == null)
                throw new ArgumentException("Invalid bitmap definition");
            var dataReference = definition.Texture.Definition.Data;
            if (dataReference.Address.Type != CacheAddressType.Resource)
                throw new InvalidOperationException("Invalid resource data address");

            var header = CreateDdsHeader(definition);
            var resourceDataStream = new MemoryStream();
            CacheContext.ExtractResource(resource.Resource, resourceDataStream);
            header.WriteTo(outStream);
            resourceDataStream.Position = dataReference.Address.Offset;
            StreamUtil.Copy(resourceDataStream, outStream, dataReference.Size);
        }

        private static DdsHeader CreateDdsHeader(BitmapTextureInteropResource definition)
        {
            var info = definition.Texture.Definition;
            var result = new DdsHeader
            {
                Width = (uint)info.Width,
                Height = (uint)info.Height,
                MipMapCount = (uint)info.MipmapCount
            };
            BitmapDdsFormatDetection.SetUpHeaderForFormat(info.Format, result);
            switch (info.Type)
            {
                case BitmapType.CubeMap:
                    result.SurfaceComplexityFlags = DdsSurfaceComplexityFlags.Complex;
                    result.SurfaceInfoFlags = DdsSurfaceInfoFlags.CubeMap | DdsSurfaceInfoFlags.CubeMapNegativeX |
                                              DdsSurfaceInfoFlags.CubeMapNegativeY | DdsSurfaceInfoFlags.CubeMapNegativeZ |
                                              DdsSurfaceInfoFlags.CubeMapPositiveX | DdsSurfaceInfoFlags.CubeMapPositiveY |
                                              DdsSurfaceInfoFlags.CubeMapPositiveZ;
                    break;
                case BitmapType.Texture3D:
                    result.Depth = (uint)info.Depth;
                    result.SurfaceInfoFlags = DdsSurfaceInfoFlags.Volume;
                    break;
            }
            const string dew = "Doritos(TM) Dew(TM) it right!";
            Encoding.ASCII.GetBytes(dew, 0, dew.Length, result.Reserved, 0);
            return result;
        }    
    }
}
