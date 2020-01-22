using TagTool.Tags;
using TagTool.Tags.Resources;

namespace TagTool.Cache
{
    public abstract class ResourceCache
    {
        public abstract BinkResource GetBinkResource(TagResourceReference resourceReference);
        public abstract BitmapTextureInteropResource GetBitmapTextureInteropResource(TagResourceReference resourceReference);
        public abstract BitmapTextureInterleavedInteropResource GetBitmapTextureInterleavedInteropResource(TagResourceReference resourceReference);
        public abstract RenderGeometryApiResourceDefinition GetRenderGeometryApiResourceDefinition(TagResourceReference resourceReference);
        public abstract ModelAnimationTagResource GetModelAnimationTagResource(TagResourceReference resourceReference);
        public abstract SoundResourceDefinition GetSoundResourceDefinition(TagResourceReference resourceReference);
        public abstract StructureBspTagResources GetStructureBspTagResources(TagResourceReference resourceReference);
        public abstract StructureBspCacheFileTagResources GetStructureBspCacheFileTagResources(TagResourceReference resourceReference);

        public abstract TagResourceReference CreateBinkResource(BinkResource binkResourceDefinition);
        public abstract TagResourceReference CreateRenderGeometryApiResource(RenderGeometryApiResourceDefinition renderGeometryDefinition);
        public abstract TagResourceReference CreateModelAnimationGraphResource(ModelAnimationTagResource modelAnimationGraphDefinition);
        public abstract TagResourceReference CreateSoundResource(SoundResourceDefinition soundResourceDefinition);
        public abstract TagResourceReference CreateBitmapResource(BitmapTextureInteropResource bitmapResourceDefinition);
        public abstract TagResourceReference CreateBitmapInterleavedResource(BitmapTextureInterleavedInteropResource bitmapResourceDefinition);
        public abstract TagResourceReference CreateStructureBspResource(StructureBspTagResources sbspResource);
        public abstract TagResourceReference CreateStructureBspCacheFileResource(StructureBspCacheFileTagResources sbspCacheFileResource);
    }

}
