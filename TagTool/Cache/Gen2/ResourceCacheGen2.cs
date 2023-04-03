using System;
using System.IO;
using System.IO.Compression;
using TagTool.Commands.Common;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags;
using TagTool.Tags.Resources;
using TagTool.BlamFile;
using TagTool.Cache.Resources;

namespace TagTool.Cache.Gen2
{
    public class ResourceCacheGen2 : ResourceCache
    {
        public GameCacheGen2 Cache;

        public ResourceCacheGen2(GameCacheGen2 cache)
        {
            Cache = cache;
        }

        private static uint GetAddressTagResourceReference(TagResourceReference resourceReference)
        {
            return resourceReference.Gen2ResourceAddress;
        }

        public byte[] GetResourceDataFromHandle(TagResourceReference resourceReference, int dataLength)
        {
            var resourceAddress = GetAddressTagResourceReference(resourceReference);

            var cacheFileType = (resourceAddress & 0xC0000000) >> 30;
            int fileOffset = (int)(resourceAddress & 0x3FFFFFFF);

            GameCacheGen2 sourceCache;

            if(cacheFileType != 0)
            {
                string cacheName = "";
                switch (cacheFileType)
                {
                    case 1:
                        cacheName = "mainmenu.map";
                        break;
                    case 2:
                        cacheName = "shared.map";
                        break;
                    case 3:
                        cacheName = "single_player_shared.map";
                        break;

                }
                if (Cache.ResourceCacheReferences.ContainsKey(cacheName))
                    sourceCache = Cache.ResourceCacheReferences[cacheName];
                else
                {
                    new TagToolWarning($"Failed to find cache for resource 0x{resourceAddress:X8}");
                    return null;
                }
            }
            else
                sourceCache = Cache;

            var stream = sourceCache.OpenCacheRead();

            var reader = new EndianReader(stream, Cache.Endianness);

            reader.SeekTo(fileOffset);
            var data = reader.ReadBytes(dataLength);

            reader.Close();

            return data;
        }

        public override Tags.Resources.Gen4.BitmapTextureInteropResource GetBitmapTextureInteropResourceGen4(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }
        public override Tags.Resources.Gen4.ModelAnimationTagResource GetModelAnimationTagResourceGen4(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }
        public override Tags.Resources.Gen4.CollisionModelResource GetCollisionModelResourceGen4(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateBinkResource(BinkResource binkResourceDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateBitmapInterleavedResource(BitmapTextureInterleavedInteropResource bitmapResourceDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateBitmapResource(BitmapTextureInteropResource bitmapResourceDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateModelAnimationGraphResource(ModelAnimationTagResource modelAnimationGraphDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateRenderGeometryApiResource(RenderGeometryApiResourceDefinition renderGeometryDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateSoundResource(SoundResourceDefinition soundResourceDefinition)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateStructureBspCacheFileResource(StructureBspCacheFileTagResources sbspCacheFileResource)
        {
            throw new NotImplementedException();
        }

        public override TagResourceReference CreateStructureBspResource(StructureBspTagResources sbspResource)
        {
            throw new NotImplementedException();
        }

        public override BinkResource GetBinkResource(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override object GetResourceDefinition(TagResourceReference resourceReference, Type definitionType)
        {
            throw new NotImplementedException();
        }

        public override SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public override StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public ResourceData GetTagResourceFromReference(TagResourceReference resourceReference)
        {
            throw new NotImplementedException();
        }

        public bool IsResourceValid(ResourceData tagResource)
        {
            throw new NotImplementedException();
        }

    }
}