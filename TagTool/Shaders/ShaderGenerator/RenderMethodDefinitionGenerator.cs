using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HaloShaderGenerator;
using HaloShaderGenerator.Generator;
using HaloShaderGenerator.Globals;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Shaders;
using TagTool.Tags.Definitions;
using static TagTool.Tags.Definitions.RenderMethodDefinition;

namespace TagTool.Shaders.ShaderGenerator
{
    public class RenderMethodDefinitionGenerator
    {
        private static readonly string[] AutoMacroShaderTypes = new string[] { "beam", "contrail", "decal", "light_volume", "particle", "water" };

        public static RenderMethodDefinition GenerateRenderMethodDefinition(GameCache cache, Stream cacheStream, IShaderGenerator generator, string shaderType, out GlobalPixelShader glps, out GlobalVertexShader glvs)
        {
            bool autoMacro = true;// AutoMacroShaderTypes.Contains(shaderType);

            var rmdf = new RenderMethodDefinition
            {
                GlobalOptions = GetOrCreateRmdfGlobalOptions(cache, cacheStream, generator, shaderType),
                Categories = BuildRmdfCategories(cache, cacheStream, generator, shaderType, autoMacro),
                EntryPoints = BuildEntryPoints(generator),
                VertexTypes = BuildVertexTypes(generator),
                Flags = autoMacro ? RenderMethodDefinitionFlags.UseAutomaticMacros : 0,
                Version = 0
            };

            GenerateSharedShaders(cache, cacheStream, generator, shaderType, out rmdf.GlobalPixelShader, out rmdf.GlobalVertexShader, out glps, out glvs);

            return rmdf;
        }

        public static bool UpdateRenderMethodDefinition(GameCache cache, Stream stream, string shaderType)
        {
            var generator = ShaderGenerator.GetGlobalShaderGenerator(shaderType, true);
            if (generator == null)
                return false;

            string rmdfName = $"shaders\\{shaderType}";
            
            if (!cache.TagCache.TryGetTag<RenderMethodDefinition>(rmdfName, out CachedTag rmdfTag)) // generate
            {
                rmdfTag = cache.TagCache.AllocateTag<RenderMethodDefinition>(rmdfName);
                var rmdf = GenerateRenderMethodDefinition(cache, stream, generator, shaderType, out _, out _);
                cache.Serialize(stream, rmdfTag, rmdf);
                (cache as GameCacheHaloOnlineBase).SaveTagNames();
            }
            else // can update
            {
                var rmdf = cache.Deserialize<RenderMethodDefinition>(stream, rmdfTag);
                bool autoMacro = rmdf.Flags.HasFlag(RenderMethodDefinitionFlags.UseAutomaticMacros);

                // make sure all categories exist
                int categoriesAdded = 0;
                for (uint i = (uint)rmdf.Categories.Count; i < generator.GetMethodCount(); i++)
                {
                    var categoryBlock = BuildCategory(cache, stream, i, generator, shaderType, autoMacro);
                    rmdf.Categories.Add(categoryBlock);
                    categoriesAdded++;
                }
                cache.SaveStrings();

                if (categoriesAdded > 0)
                    AdjustTemplateTagNames(cache as GameCacheHaloOnlineBase, shaderType, categoriesAdded, rmdf.Categories.Count);

                // update
                for (uint i = 0; i < generator.GetMethodCount(); i++)
                {
                    UpdateCategory(rmdf.Categories[(int)i], cache, stream, i, generator, shaderType, autoMacro);
                }
                cache.SaveStrings();

                // update glps category dependencies
                var glps = cache.Deserialize<GlobalPixelShader>(stream, rmdf.GlobalPixelShader);

                for (int i = 0; i < rmdf.EntryPoints.Count; i++)
                {
                    foreach (var pass in rmdf.EntryPoints[i].Passes)
                    {
                        if (!pass.Flags.HasFlag(EntryPointBlock.PassBlock.PassFlags.HasSharedPixelShader))
                            continue;

                        for (int j = 0; j < pass.CategoryDependencies.Count; j++)
                        {
                            var categoryOptionCount = rmdf.Categories[pass.CategoryDependencies[j].Category].ShaderOptions.Count;

                            foreach (var dep in glps.EntryPoints[(int)rmdf.EntryPoints[i].EntryPoint].CategoryDependency)
                            {
                                if (dep.DefinitionCategoryIndex == pass.CategoryDependencies[j].Category)
                                {
                                    if (dep.OptionDependency.Count != categoryOptionCount)
                                    {
                                        glps = ShaderGenerator.GenerateSharedPixelShader(cache, generator);

                                        if (!cache.TagCache.TryGetTag<GlobalPixelShader>($"shaders\\{shaderType.ToLower()}_shared_pixel_shaders", out var glpsTag))
                                            glpsTag = cache.TagCache.AllocateTag<GlobalPixelShader>($"shaders\\{shaderType.ToLower()}_shared_pixel_shaders");
                                        cache.Serialize(stream, glpsTag, glps);

                                        // no need to keep looping
                                        cache.Serialize(stream, rmdfTag, rmdf);
                                        return true;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }

                cache.Serialize(stream, rmdfTag, rmdf);
            }

            return true;
        }

        private static CategoryBlock.ShaderOption BuildCategoryOption(GameCache cache, Stream cacheStream, string categoryName, uint categoryIndex, int optionIndex, IShaderGenerator generator, bool autoMacro)
        {
            CategoryBlock.ShaderOption result = new CategoryBlock.ShaderOption();

            string optionName = FixupMethodOptionName(generator.GetMethodOptionNames((int)categoryIndex).GetValue(optionIndex).ToString().ToLower());
            StringId nameId = cache.StringTable.GetStringId(optionName);
            result.Name = nameId != StringId.Invalid ? nameId : cache.StringTable.AddString(optionName);

            var parameters = generator.GetParametersInOption(categoryName, optionIndex, out string rmopName, out _);

            if (rmopName != null)
            {
                if (!cache.TagCache.TryGetTag<RenderMethodOption>(rmopName, out CachedTag rmopTag))
                {
                    RenderMethodOption rmop = BuildRenderMethodOptionFromParameters(cache, parameters);
                    rmopTag = cache.TagCache.AllocateTag<RenderMethodOption>(rmopName);
                    (cache as GameCacheHaloOnlineBase).SaveTagNames();
                    cache.Serialize(cacheStream, rmopTag, rmop);
                }

                result.Option = rmopTag;
            }

            if (autoMacro)
            {
                result.PixelFunction = StringId.Invalid;
                result.VertexFunction = StringId.Invalid;
            }
            else
            { } // TODO

            return result;
        }

        private static CategoryBlock BuildCategory(GameCache cache, Stream cacheStream, uint categoryIndex, IShaderGenerator generator, string shaderType, bool autoMacro)
        {
            CategoryBlock result = new CategoryBlock();

            string categoryName = FixupMethodOptionName(generator.GetMethodNames().GetValue(categoryIndex).ToString().ToLower());
            StringId nameId = cache.StringTable.GetStringId(categoryName);
            result.Name = nameId != StringId.Invalid ? nameId : cache.StringTable.AddString(categoryName);

            result.ShaderOptions = new List<CategoryBlock.ShaderOption>();

            if (shaderType == "black")
                return result;

            for (int i = 0; i < generator.GetMethodOptionCount((int)categoryIndex); i++)
            {
                var optionBlock = BuildCategoryOption(cache, cacheStream, categoryName, categoryIndex, i, generator, autoMacro);
                result.ShaderOptions.Add(optionBlock);
            }

            if (autoMacro)
            {
                result.PixelFunction = StringId.Invalid;
                result.VertexFunction = StringId.Invalid;
            }
            else
            { } // TODO

            return result;
        }

        private static void UpdateCategory(CategoryBlock categoryBlock, GameCache cache, Stream cacheStream, uint categoryIndex, IShaderGenerator generator, string shaderType, bool autoMacro)
        {
            if (shaderType == "black")
                return;

            string categoryName = cache.StringTable.GetString(categoryBlock.Name);

            for (int i = categoryBlock.ShaderOptions.Count; i < generator.GetMethodOptionCount((int)categoryIndex); i++)
            {
                var optionBlock = BuildCategoryOption(cache, cacheStream, categoryName, categoryIndex, i, generator, autoMacro);
                categoryBlock.ShaderOptions.Add(optionBlock);
            }
        }

        private static List<CategoryBlock> BuildRmdfCategories(GameCache cache, Stream cacheStream, IShaderGenerator generator, string shaderType, bool autoMacro)
        {
            List<CategoryBlock> result = new List<CategoryBlock>();

            for (uint i = 0; i < generator.GetMethodCount(); i++)
            {
                var categoryBlock = BuildCategory(cache, cacheStream, i, generator, shaderType, autoMacro);
                result.Add(categoryBlock);
            }

            cache.SaveStrings();
            return result;
        }

        private static CachedTag GetOrCreateRmdfGlobalOptions(GameCache cache, Stream cacheStream, IShaderGenerator generator, string shaderType)
        {
            string globalRmopName = @"shaders\shader_options\global_shader_options";

            switch (shaderType)
            {
                case "beam":
                    globalRmopName = @"shaders\beam_options\global_beam_options";
                    break;
                case "contrail":
                    globalRmopName = @"shaders\contrail_options\global_contrail_options";
                    break;
                case "decal":
                    globalRmopName = @"shaders\decal_options\global_decal_options";
                    break;
                case "light_volume":
                    globalRmopName = @"shaders\light_volume_options\global_light_volume";
                    break;
                case "particle":
                    globalRmopName = @"shaders\particle_options\global_particle_options";
                    break;
                case "screen":
                    globalRmopName = @"shaders\screen_options\global_screen_options";
                    break;
                case "water":
                    globalRmopName = @"shaders\water_options\water_global";
                    break;
            }

            if (!cache.TagCache.TryGetTag<RenderMethodOption>(globalRmopName, out CachedTag rmopTag))
            {
                RenderMethodOption rmop = BuildRenderMethodOptionFromParameters(cache, generator.GetGlobalParameters());
                rmopTag = cache.TagCache.AllocateTag<RenderMethodOption>(globalRmopName);
                cache.Serialize(cacheStream, rmopTag, rmop);
            }

            return rmopTag;
        }

        private static RenderMethodOption BuildRenderMethodOptionFromParameters(GameCache cache, ShaderParameters parameters)
        {
            RenderMethodOption rmop = new RenderMethodOption();
            rmop.Parameters = new List<RenderMethodOption.ParameterBlock>();

            foreach (var parameter in parameters.Parameters)
            {
                if (parameter.CodeType == HLSLType.Xform_2d || parameter.CodeType == HLSLType.Xform_3d)
                    continue;

                RenderMethodOption.ParameterBlock parameterBlock = new RenderMethodOption.ParameterBlock();

                StringId nameId = cache.StringTable.GetStringId(parameter.ParameterName);
                parameterBlock.Name = nameId != StringId.Invalid ? nameId : cache.StringTable.AddString(parameter.ParameterName);

                parameterBlock.RenderMethodExtern = (RenderMethodExtern)parameter.RenderMethodExtern;

                switch (parameter.RegisterType)
                {
                    case RegisterType.Boolean:
                        parameterBlock.Type = RenderMethodOption.ParameterBlock.OptionDataType.Bool;
                        break;
                    case RegisterType.Integer:
                        parameterBlock.Type = RenderMethodOption.ParameterBlock.OptionDataType.Int;
                        break;
                    case RegisterType.Sampler:
                        parameterBlock.Type = RenderMethodOption.ParameterBlock.OptionDataType.Bitmap;
                        break;
                    case RegisterType.Vector:
                        if (parameter.Flags.HasFlag(ShaderParameterFlags.IsColor))
                            parameterBlock.Type = RenderMethodOption.ParameterBlock.OptionDataType.ArgbColor;
                        else if (parameter.CodeType != HLSLType.Float4)
                            parameterBlock.Type = RenderMethodOption.ParameterBlock.OptionDataType.Real;
                        else
                            parameterBlock.Type = RenderMethodOption.ParameterBlock.OptionDataType.Color;
                        break;
                }

                rmop.Parameters.Add(parameterBlock);
            }

            cache.SaveStrings();
            return rmop;
        }

        private static List<EntryPointBlock> BuildEntryPoints(IShaderGenerator generator)
        {
            List<EntryPointBlock> result = new List<EntryPointBlock>();

            foreach (ShaderStage entryPoint in Enum.GetValues(typeof(ShaderStage)))
            {
                if (generator.IsEntryPointSupported(entryPoint))
                {
                    var passBlock = new EntryPointBlock.PassBlock
                    {
                        Flags = generator.IsPixelShaderShared(entryPoint) ? EntryPointBlock.PassBlock.PassFlags.HasSharedPixelShader : 0,
                        CategoryDependencies = new List<EntryPointBlock.PassBlock.CategoryDependency>()
                    };
                    result.Add(new EntryPointBlock { EntryPoint = (EntryPoint_32)entryPoint, Passes = new List<EntryPointBlock.PassBlock> { passBlock } });
                }
            }

            return result;
        }

        private static List<VertexBlock> BuildVertexTypes(IShaderGenerator generator)
        {
            List<VertexBlock> result = new List<VertexBlock>();

            foreach (VertexType vertexType in Enum.GetValues(typeof(VertexType)))
                if (generator.IsVertexFormatSupported(vertexType))
                    result.Add(new VertexBlock { VertexType = (VertexBlock.VertexTypeValue)vertexType });

            return result;
        }

        private static void GenerateSharedShaders(GameCache cache, Stream cacheStream, IShaderGenerator generator,string shaderType, 
            out CachedTag glpsTag, out CachedTag glvsTag, out GlobalPixelShader glps, out GlobalVertexShader glvs)
        {
            glps = ShaderGenerator.GenerateSharedPixelShader(cache, generator);
            glvs = ShaderGenerator.GenerateSharedVertexShader(cache, generator);

            if (!cache.TagCache.TryGetTag<GlobalPixelShader>($"shaders\\{shaderType.ToLower()}_shared_pixel_shaders", out glpsTag))
                glpsTag = cache.TagCache.AllocateTag<GlobalPixelShader>($"shaders\\{shaderType.ToLower()}_shared_pixel_shaders");
            cache.Serialize(cacheStream, glpsTag, glps);

            if (!cache.TagCache.TryGetTag<GlobalVertexShader>($"shaders\\{shaderType.ToLower()}_shared_vertex_shaders", out glvsTag))
                glvsTag = cache.TagCache.AllocateTag<GlobalVertexShader>($"shaders\\{shaderType.ToLower()}_shared_vertex_shaders");
            cache.Serialize(cacheStream, glvsTag, glvs);
        }

        /// <summary>
        /// Contains hardcoded fixups for shader method or option names
        /// </summary>
        private static string FixupMethodOptionName(string input)
        {
            switch (input)
            {
                case "first_person_never_with_rotating_bitmaps":
                    return @"first_person_never_w/rotating_bitmaps";
                case "_3_channel_self_illum":
                    return "3_channel_self_illum";
                case "blackness_no_options":
                    return "blackness(no_options)";
            }

            return input;
        }

        private static void AdjustTemplateTagNames(GameCacheHaloOnlineBase cache, string shaderType, int categoriesAdded, int totalCategories)
        {
            string appendage = string.Concat(Enumerable.Repeat("_0", categoriesAdded));

            foreach (var tag in cache.TagCache.NonNull())
            {
                if (tag.Group.Tag != "rmt2" || tag.Name.StartsWith("ms30\\"))
                    continue;

                var parts = tag.Name.Split('\\');

                int nameCategories = parts[2].Remove(0, 1).Split('_').Length;
                if (nameCategories >= totalCategories)
                    continue;

                string templateType = parts[1].Replace("_templates", "");
                if (templateType == shaderType)
                    tag.Name += appendage;
            }

            cache.SaveTagNames();
        }
    }
}
