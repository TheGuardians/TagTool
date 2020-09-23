using System;
using System.IO;
using System.IO.Compression;
using TagTool.Common;
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

        private static DatumHandle GetHandleFromTagResourceReference(TagResourceReference resourceReference)
        {
            return resourceReference.Gen2ResourceID;
        }

        public byte[] GetResourceDataFromHandle(TagResourceReference resourceReference, int dataLength)
        {
            var datumHandle = GetHandleFromTagResourceReference(resourceReference);
            if (datumHandle == null)
                return null;

            var cacheFileType = (datumHandle.Value & 0xC0000000) >> 30;
            int fileOffset = (int)(datumHandle.Value & 0x3FFFFFFF);


            GameCacheGen2 sourceCache;

            if(cacheFileType != 0)
            {
                string filename = "";
                switch (cacheFileType)
                {
                    case 1:
                        filename = Path.Combine(Cache.Directory.FullName, "mainmenu.map");
                        break;
                    case 2:
                        filename = Path.Combine(Cache.Directory.FullName, "shared.map");
                        break;
                    case 3:
                        filename = Path.Combine(Cache.Directory.FullName, "single_player_shared.map");
                        break;

                }
                // TODO: make this a function call with a stored reference to caches in the base cache or something better than this
                sourceCache = (GameCacheGen2)GameCache.Open(new FileInfo(filename));
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