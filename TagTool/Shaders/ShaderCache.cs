using System;
using System.Collections;
using System.IO;
using TagTool.Cache;
using TagTool.Commands.Shaders;
using TagTool.Common;
using TagTool.Tags;
using TagTool.Tags.Definitions;
using static TagTool.Shaders.ShaderMatching.ShaderMatcherNew;

namespace TagTool.Shaders
{
    public static class ShaderCache
    {
        public static bool ImportTemplate(GameCache sourceCache, string tagName, RenderMethodTemplate rmt2, PixelShader pixl, VertexShader vtsh)
        {
            var shaderCache = UseShaderCacheCommand.ShaderCache;
            if (shaderCache == null)
                return false;

            using (var stream = shaderCache.OpenCacheReadWrite())
            {
                try
                {
                    ConvertTemplate(sourceCache, stream, shaderCache, tagName, rmt2, pixl, vtsh);
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
                    var cachedVtsh = shaderCache.Deserialize<VertexShader>(stream, cachedRmt2.VertexShader);

                    // allocate and fixup the shader tags
                    rmt2Tag = ConvertTemplate(shaderCache, destCacheStream, destCache, tagName, cachedRmt2, cachedPixl, cachedVtsh);
                    GenerateRenderMethodDefinitionIfNeeded(destCacheStream, destCache, rmt2Tag);
                    return true;
                }
            }

            rmt2Tag = null;
            return false;
        }

        private static void GenerateRenderMethodDefinitionIfNeeded(Stream destCacheStream, GameCache destCache, CachedTag rmt2Tag)
        {
            Rmt2Descriptor.TryParse(rmt2Tag.Name, out Rmt2Descriptor rmt2Desc);

            var rmdfName = $"shaders\\{rmt2Desc.Type}";

            if (!destCache.TagCache.TryGetTag($"{rmdfName}.rmdf", out CachedTag rmdfTag))
            {
                var generator = rmt2Desc.GetGenerator(true);
                Console.WriteLine($"Generating rmdf for \"{rmt2Desc.Type}\"");
                var rmdf = ShaderGenerator.ShaderGenerator.GenerateRenderMethodDefinition(destCache, destCacheStream, generator, rmt2Desc.Type, out GlobalPixelShader glps, out GlobalVertexShader glvs);
                rmdfTag = destCache.TagCache.AllocateTag<RenderMethodDefinition>(rmdfName);
                destCache.Serialize(destCacheStream, rmdfTag, rmdf);
                (destCache as GameCacheHaloOnlineBase).SaveTagNames();
            }
        }

        private static CachedTag ConvertTemplate(GameCache sourceCache, Stream destCacheStream, GameCache destCache, string tagName, RenderMethodTemplate rmt2, PixelShader pixl, VertexShader vtsh)
        {
            // convert the data
            var cachedRmt2 = (RenderMethodTemplate)ConvertData(sourceCache, destCache, rmt2.DeepCloneV2());
            var cachedPixl = (PixelShader)ConvertData(sourceCache, destCache, pixl);
            var cachedVtsh = (VertexShader)ConvertData(sourceCache, destCache, vtsh);

            var cachedRmt2Tag = destCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);
            cachedRmt2.PixelShader = destCache.TagCache.AllocateTag<PixelShader>(tagName);
            cachedRmt2.VertexShader = destCache.TagCache.AllocateTag<VertexShader>(tagName);

            // serialize everything
            destCache.Serialize(destCacheStream, cachedRmt2.PixelShader, cachedPixl);
            destCache.Serialize(destCacheStream, cachedRmt2.VertexShader, cachedVtsh);
            destCache.Serialize(destCacheStream, cachedRmt2Tag, cachedRmt2);

            return cachedRmt2Tag;
        }

        private static object ConvertData(GameCache sourceCache, GameCache destCache, object data)
        {
            switch (data)
            {
                case StringId stringId:
                    {
                        var str = sourceCache.StringTable.GetString(stringId);
                        var destStringId = destCache.StringTable.GetStringId(str);
                        if (stringId != StringId.Invalid && destStringId == StringId.Invalid)
                            return destCache.StringTable.AddString(str);
                        else
                            return destStringId;
                    }
                case CachedTag srcTag:
                    {
                        destCache.TagCache.TryGetTag($"{srcTag.Name}.{srcTag.Group}", out CachedTag destTag);
                        return destTag;
                    }
                case TagStructure tagStruct:
                    {
                        foreach (var field in tagStruct.GetTagFieldEnumerable(sourceCache.Version, sourceCache.Platform))
                            field.SetValue(data, ConvertData(sourceCache, destCache, field.GetValue(data)));
                        return tagStruct;
                    }
                case IList collection:
                    {
                        for (int i = 0; i < collection.Count; i++)
                            collection[i] = ConvertData(sourceCache, destCache, collection[i]);
                        return collection;
                    }
                default:
                    return data;
            }
        }

        internal static void ImportRenderMethodDefinition(GameCache baseCache, string v, RenderMethodDefinition rmdf)
        {
            throw new NotImplementedException();
        }
    }
}
