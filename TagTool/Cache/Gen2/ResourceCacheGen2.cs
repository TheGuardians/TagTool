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

        public ResourceCacheGen2(GameCacheGen2 cache, bool load = false)
        {
            Cache = cache;
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