using System;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Shaders;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders
{
    public static class ShaderCache
    {
        public static bool ImportTemplate(string tagName, RenderMethodTemplate rmt2, PixelShader pixl, VertexShader vtsh)
        {
            var shaderCache = UseShaderCacheCommand.ShaderCache;
            if (shaderCache == null)
                return false;

            using (var stream = shaderCache.OpenCacheReadWrite())
            {
                try
                {
                    var cachedRmt2Tag = shaderCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);
                    var cachedRmt2 = rmt2.DeepCloneV2();
                    cachedRmt2.PixelShader = shaderCache.TagCache.AllocateTag<PixelShader>(tagName);
                    cachedRmt2.VertexShader = shaderCache.TagCache.AllocateTag<VertexShader>(tagName);
                    shaderCache.Serialize(stream, cachedRmt2.PixelShader, pixl);
                    shaderCache.Serialize(stream, cachedRmt2.VertexShader, vtsh);
                    shaderCache.Serialize(stream, cachedRmt2Tag, cachedRmt2);
                    return true;
                }
                finally
                {
                    shaderCache.SaveStrings();
                    shaderCache.SaveTagNames();
                }
            }
        }

        public static bool ExportTemplate(Stream destCacheStream, GameCache destCache, string tagName, out CachedTag rmt2Tag)
        {
            var shaderCache = UseShaderCacheCommand.ShaderCache;
            if (shaderCache != null && shaderCache.TagCache.TryGetTag<RenderMethodTemplate>(tagName, out CachedTag cachedTemplateTag))
            {
                using (var stream = shaderCache.OpenCacheRead())
                {
                    var cachedRmt2 = shaderCache.Deserialize<RenderMethodTemplate>(stream, cachedTemplateTag);

                    var cachedPixl = shaderCache.Deserialize<PixelShader>(stream, cachedRmt2.PixelShader);
                    var pixlTag = destCache.TagCache.AllocateTag<PixelShader>(cachedTemplateTag.Name);
                    cachedRmt2.PixelShader = pixlTag;
                    destCache.Serialize(destCacheStream, pixlTag, cachedPixl);

                    var cachedVertexShader = shaderCache.Deserialize<VertexShader>(stream, cachedRmt2.VertexShader);
                    var vtshTag = destCache.TagCache.AllocateTag<VertexShader>(cachedTemplateTag.Name);
                    cachedRmt2.VertexShader = vtshTag;
                    destCache.Serialize(destCacheStream, vtshTag, cachedVertexShader);

                    rmt2Tag = destCache.TagCache.AllocateTag<RenderMethodTemplate>(cachedTemplateTag.Name);
                    destCache.Serialize(destCacheStream, rmt2Tag, cachedRmt2);
                    return true;
                }
            }

            rmt2Tag = null;
            return false;
        }
    }
}
