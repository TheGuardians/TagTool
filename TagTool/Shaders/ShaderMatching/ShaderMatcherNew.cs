using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Threading.Tasks;

namespace TagTool.Shaders.ShaderMatching
{
    public class ShaderMatcherNew
    {
        private GameCache BaseCache;
        private GameCache PortingCache;
        private Stream BaseCacheStream;
        private Stream PortingCacheStream;
        private Commands.Porting.PortTagCommand PortTagCommand;
        // shader type, definition
        private Dictionary<string, RenderMethodDefinition> RenderMethodDefinitions;
        private Dictionary<string, RenderMethodDefinition> PortingRenderMethodDefinitions;
        // tag name, definition
        private Dictionary<string, RenderMethodOption> RenderMethodOptions;
        private Dictionary<string, RenderMethodOption> PortingRenderMethodOptions;

        public bool IsInitialized { get; private set; } = false;
        public bool UseMs30 { get; set; } = false;
        public bool PerfectMatchesOnly { get; set; } = false;

        static Dictionary<string, int[]> MethodWeights = new Dictionary<string, int[]>()
        {
           ["default"] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
           ["shader"] = new int[] { 1, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0 }
        };

        public ShaderMatcherNew()
        {
        }

        public void Init(GameCache baseCache, 
            GameCache portingCache, 
            Stream baseCacheStream, 
            Stream portingCacheStream,
            Commands.Porting.PortTagCommand portTagCommand,
            bool useMS30 = false, 
            bool perfectMatchesOnly = false)
        {
            UseMs30 = useMS30;
            PerfectMatchesOnly = perfectMatchesOnly;
            BaseCache = baseCache;
            PortingCache = portingCache;
            BaseCacheStream = baseCacheStream;
            PortingCacheStream = portingCacheStream;
            IsInitialized = true;
            PortTagCommand = portTagCommand;

            // we need to store all of these for async. will save cpu time for map ports since we no longer deserialize for every shader tag
            RenderMethodDefinitions = new Dictionary<string, RenderMethodDefinition>();
            PortingRenderMethodDefinitions = new Dictionary<string, RenderMethodDefinition>();
            RenderMethodOptions = new Dictionary<string, RenderMethodOption>();
            PortingRenderMethodOptions = new Dictionary<string, RenderMethodOption>();

            foreach (var rmdfTag in baseCache.TagCache.NonNull().Where(x => x.Group.Tag == "rmdf" && !x.Name.StartsWith("ms30\\")))
                RenderMethodDefinitions.Add(rmdfTag.Name.Remove(0, 8), baseCache.Deserialize<RenderMethodDefinition>(baseCacheStream, rmdfTag));
            foreach (var rmdfTag in portingCache.TagCache.NonNull().Where(x => x.Group.Tag == "rmdf"))
                PortingRenderMethodDefinitions.Add(rmdfTag.Name.Remove(0, 8), portingCache.Deserialize<RenderMethodDefinition>(portingCacheStream, rmdfTag));

            foreach (var rmopTag in baseCache.TagCache.NonNull().Where(x => x.Group.Tag == "rmop" && !x.Name.StartsWith("ms30\\")))
                RenderMethodOptions.Add(rmopTag.Name, baseCache.Deserialize<RenderMethodOption>(baseCacheStream, rmopTag));
            foreach (var rmopTag in portingCache.TagCache.NonNull().Where(x => x.Group.Tag == "rmop"))
                PortingRenderMethodOptions.Add(rmopTag.Name, portingCache.Deserialize<RenderMethodOption>(portingCacheStream, rmopTag));
        }

        public void DeInit()
        {
            UseMs30 = false;
            PerfectMatchesOnly = false;
            BaseCache = null;
            PortingCache = null;
            BaseCacheStream = null;
            PortingCacheStream = null;
            IsInitialized = false;
            PortTagCommand = null;
            RenderMethodDefinitions = null;
            PortingRenderMethodDefinitions = null;
            RenderMethodOptions = null;
            PortingRenderMethodOptions = null;
        }

        public Dictionary<StringId, RenderMethodOption.ParameterBlock.OptionDataType> GetOptionParameters(List<byte> options, RenderMethodDefinition rmdf)
        {
            Dictionary<StringId, RenderMethodOption.ParameterBlock.OptionDataType> optionParameters = new Dictionary<StringId, RenderMethodOption.ParameterBlock.OptionDataType>();

            for (int i = 0; i < options.Count; i++)
            {
                if (rmdf.Categories[i].ShaderOptions[options[i]].Option != null)
                {
                    var rmop = RenderMethodOptions[rmdf.Categories[i].ShaderOptions[options[i]].Option.Name];
                    foreach (var parameter in rmop.Parameters)
                        if (!optionParameters.ContainsKey(parameter.Name))
                            optionParameters.Add(parameter.Name, parameter.Type);
                }
            }

            return optionParameters;
        }

        public Dictionary<StringId, RenderMethodOption.ParameterBlock> GetOptionBlocks(List<byte> options, RenderMethodDefinition rmdf)
        {
            Dictionary<StringId, RenderMethodOption.ParameterBlock> optionBlocks = new Dictionary<StringId, RenderMethodOption.ParameterBlock>();

            for (int i = 0; i < options.Count; i++)
            {
                if (rmdf.Categories[i].ShaderOptions[options[i]].Option != null)
                {
                    var rmop = RenderMethodOptions[rmdf.Categories[i].ShaderOptions[options[i]].Option.Name];
                    foreach (var parameter in rmop.Parameters)
                        if (!optionBlocks.ContainsKey(parameter.Name))
                            optionBlocks.Add(parameter.Name, parameter);
                }
            }

            return optionBlocks;
        }

        public Dictionary<StringId, CachedTag> GetOptionBitmaps(List<byte> options, RenderMethodDefinition rmdf)
        {
            Dictionary<StringId, CachedTag> optionBitmaps = new Dictionary<StringId, CachedTag>();

            for (int i = 0; i < options.Count; i++)
            {
                if (rmdf.Categories[i].ShaderOptions[options[i]].Option != null)
                {
                    var rmop = RenderMethodOptions[rmdf.Categories[i].ShaderOptions[options[i]].Option.Name];
                    foreach (var parameter in rmop.Parameters)
                        if (parameter.Type == RenderMethodOption.ParameterBlock.OptionDataType.Bitmap && parameter.DefaultSamplerBitmap != null && !optionBitmaps.ContainsKey(parameter.Name))
                            optionBitmaps.Add(parameter.Name, parameter.DefaultSamplerBitmap);
                }
            }

            return optionBitmaps;
        }

        /// <summary>
        /// Find the closest template in the base cache to the input template.
        /// </summary>
        public CachedTag FindClosestTemplate(CachedTag sourceRmt2Tag, RenderMethodTemplate sourceRmt2, bool canGenerate)
        {
            Debug.Assert(IsInitialized);
    
            Rmt2Descriptor sourceRmt2Desc;
            if (!Rmt2Descriptor.TryParse(sourceRmt2Tag.Name, out sourceRmt2Desc))
            {
                new TagToolError(CommandError.OperationFailed, $"Invalid rmt2 name '{sourceRmt2Tag.Name}'");
                return null;
            }

            // rebuild options to match base cache
            sourceRmt2Desc = RebuildRmt2Options(sourceRmt2Desc);

            string tagName = $"shaders\\{sourceRmt2Desc.Type}_templates\\_{string.Join("_", sourceRmt2Desc.Options)}";

            var relevantRmt2s = new List<Rmt2Pairing>();

            Dictionary<CachedTag, long> ShaderTemplateValues = new Dictionary<CachedTag, long>();
            ParticleSorter particleTemplateSorter = new ParticleSorter();
            BeamSorter beamTemplateSorter = new BeamSorter();
            ContrailSorter contrailTemplateSorter = new ContrailSorter();
            LightVolumeSorter lightvolumeTemplateSorter = new LightVolumeSorter();
            ShaderSorter shaderTemplateSorter = new ShaderSorter();
            HalogramSorter halogramTemplateSorter = new HalogramSorter();
            TerrainSorter terrainTemplateSorter = new TerrainSorter();
            FoliageSorter foliageTemplateSorter = new FoliageSorter();
            DecalSorter decalTemplateSorter = new DecalSorter();
            ScreenSorter screenTemplateSorter = new ScreenSorter();
            WaterSorter waterTemplateSorter = new WaterSorter();

            // search
            foreach (var rmt2Tag in BaseCache.TagCache.NonNull().Where(tag => tag.IsInGroup("rmt2")))
            {
                Rmt2Descriptor destRmt2Desc;
                if (rmt2Tag.Name == null || rmt2Tag.Name == "" || !Rmt2Descriptor.TryParse(rmt2Tag.Name, out destRmt2Desc))
                    continue;

                // ignore ms30 templates if desired
                if (!UseMs30 && destRmt2Desc.IsMs30)
                    continue;

                // ignore templates that are not of the same type
                if (destRmt2Desc.Type != sourceRmt2Desc.Type)
                    continue;

                int[] weights;
                if (!MethodWeights.TryGetValue(sourceRmt2Desc.Type, out weights))
                    weights = MethodWeights["default"];

                // match the options from the rmt2 tag names
                int commonOptions = 0;
                int score = 0;
                for (int i = 0; i < sourceRmt2Desc.Options.Length; i++)
                {
                    if (i >= destRmt2Desc.Options.Length)
                        continue;

                    if (sourceRmt2Desc.Options[i] == destRmt2Desc.Options[i])
                    {
                        score += 1 + weights[i];
                        commonOptions++;
                    }
                }

                // if we found an exact match, return it
                if (commonOptions == sourceRmt2Desc.Options.Length)
                {
                    //Console.WriteLine("Found perfect rmt2 match:");
                    //Console.WriteLine(sourceRmt2Tag.Name);
                    //Console.WriteLine(rmt2Tag.Name);
                    return rmt2Tag;
                }
                    

                // add it to the list to be considered
                relevantRmt2s.Add(new Rmt2Pairing()
                {
                    Score = score,
                    CommonOptions = commonOptions,
                    DestTag = rmt2Tag,
                    SourceTag = sourceRmt2Tag
                });

                switch (sourceRmt2Desc.Type)
                {
                    case "beam":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(beamTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "contrail":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(contrailTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "shader":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(shaderTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "halogram":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(halogramTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "terrain":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(terrainTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "particle":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(particleTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "light_volume":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(lightvolumeTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "foliage":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(foliageTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "decal":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(decalTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "screen":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(screenTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                    case "water":
                        ShaderTemplateValues.Add(rmt2Tag, Sorter.GetValue(waterTemplateSorter, Sorter.GetTemplateOptions(rmt2Tag.Name)));
                        break;
                }
            }

            if (ShaderCache.ExportTemplate(BaseCacheStream, BaseCache, tagName, out CachedTag cachedRmt2Tag))
            {
                if (PortTagCommand.FlagIsSet(Commands.Porting.PortTagCommand.PortingFlags.Print))
                    Console.WriteLine($"['{cachedRmt2Tag.Group.Tag}', 0x{cachedRmt2Tag.Index:X4}] {cachedRmt2Tag.Name}.{(cachedRmt2Tag.Group as Cache.Gen3.TagGroupGen3).Name}");
                return cachedRmt2Tag;
            }

            // potentially async here. depends on: type (cannot be an effect type) and whether the rmt2 exists already.
            if (canGenerate && TryGenerateTemplate(tagName, sourceRmt2Desc, out CachedTag generatedRmt2, (Commands.Porting.PortTagCommand.TemplateConversionResult result) =>
            {
                PortTagCommand._deferredActions.Add(() =>
                {
                    PortTagCommand.FinishConvertTemplate(result, tagName, out RenderMethodTemplate asyncRmt2, out PixelShader asyncPixl, out VertexShader asyncVtsh);

                    if (!BaseCache.TagCache.TryGetTag(tagName + ".pixl", out asyncRmt2.PixelShader))
                        asyncRmt2.PixelShader = BaseCache.TagCache.AllocateTag<PixelShader>(tagName);
                    if (!BaseCache.TagCache.TryGetTag(tagName + ".vtsh", out asyncRmt2.VertexShader))
                        asyncRmt2.VertexShader = BaseCache.TagCache.AllocateTag<VertexShader>(tagName);

                    BaseCache.Serialize(BaseCacheStream, asyncRmt2.PixelShader, asyncPixl);
                    BaseCache.Serialize(BaseCacheStream, asyncRmt2.VertexShader, asyncVtsh);
                    BaseCache.Serialize(BaseCacheStream, result.Tag, asyncRmt2);
                    
                    if (PortTagCommand.FlagIsSet(Commands.Porting.PortTagCommand.PortingFlags.Print))
                        Console.WriteLine($"['{result.Tag.Group.Tag}', 0x{result.Tag.Index:X4}] {result.Tag.Name}.{(result.Tag.Group as Cache.Gen3.TagGroupGen3).Name}");
                });
            }))
            {
                return generatedRmt2;
            }

            Console.WriteLine($"No rmt2 match found for {sourceRmt2Tag.Name}");

            if (PerfectMatchesOnly)
                return null;

            // rebuild source rmt2 tagname using updated indices
            string srcRmt2Tagname = $"shaders\\{sourceRmt2Desc.Type}_templates\\_{string.Join("_", sourceRmt2Desc.Options)}";

            // find closest rmt2

            switch (sourceRmt2Desc.Type)
            {
                case "beam":            return GetBestTag(beamTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "black":           return null;
                case "custom":          return null;
                case "contrail":        return GetBestTag(contrailTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "decal":           return GetBestTag(decalTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "foliage":         return GetBestTag(foliageTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "halogram":        return GetBestTag(halogramTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "light_volume":    return GetBestTag(lightvolumeTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "particle":        return GetBestTag(particleTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "screen":          return GetBestTag(screenTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "shader":          return GetBestTag(shaderTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "terrain":         return GetBestTag(terrainTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                case "water":           return GetBestTag(waterTemplateSorter, ShaderTemplateValues, srcRmt2Tagname, sourceRmt2Tag.Name);
                default:                return null;
            }
        }

        private bool CanGenerateAsync(string shaderType)
        {
            // todo: support rmd but avoid decs
            switch (shaderType)
            {
                case "shader":
                case "custom":
                case "cortana":
                case "halogram":
                case "glass":
                case "terrain":
                case "foliage":
                case "water":
                case "zonly":
                    return true;
                default:
                    return false;
            }
        }

        private bool TryGenerateTemplate(string tagName, Rmt2Descriptor rmt2Desc, out CachedTag generatedRmt2, Action<Commands.Porting.PortTagCommand.TemplateConversionResult> callback)
        {
            generatedRmt2 = null;

            if (!RenderMethodDefinitions.ContainsKey(rmt2Desc.Type))
            {
                new TagToolError(CommandError.CustomMessage, $"No rmdf tag present for {rmt2Desc.Type}");
                return false;
            }
            RenderMethodDefinition rmdf = RenderMethodDefinitions[rmt2Desc.Type];

            RenderMethodTemplate rmt2;
            PixelShader pixl;
            VertexShader vtsh;

            try
            {
                if (CanGenerateAsync(rmt2Desc.Type))
                {
                    CachedTag rmt2Tag = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);
                    PortTagCommand.PendingTemplates.Add(tagName);

                    var glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                    var glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);

                    // get options in numeric array
                    List<byte> options = new List<byte>();
                    foreach (var option in tagName.Split('\\')[2].Remove(0, 1).Split('_'))
                        options.Add(byte.Parse(option));

                    var allRmopParameters = ShaderGenerator.ShaderGeneratorNew.GatherParametersAsync(RenderMethodOptions, rmdf, options);

                    PortTagCommand.ConcurrencyLimiter.Wait();
                    PortTagCommand.TemplateConversionTasks.Add(tagName, Task.Run(() =>
                    {
                        try
                        {
                            Commands.Porting.PortTagCommand.TemplateConversionResult result = new Commands.Porting.PortTagCommand.TemplateConversionResult();

                            result.Tag = rmt2Tag;
                            result.Definition = ShaderGenerator.ShaderGeneratorNew.GenerateTemplate(BaseCache, rmdf, glvs, glps, allRmopParameters, tagName, out result.PixelShaderDefinition, out result.VertexShaderDefinition);

                            callback(result);
                        }
                        finally
                        {
                            PortTagCommand.ConcurrencyLimiter.Release();
                        }
                    }));

                    generatedRmt2 = rmt2Tag;
                    return true;
                }
                else
                {
                    rmt2 = ShaderGenerator.ShaderGeneratorNew.GenerateTemplateSafe(BaseCache, BaseCacheStream, rmdf, tagName, out pixl, out vtsh);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generation failed, finding best substitute");
                return false;
            }

            generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

            BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);

            ShaderCache.ImportTemplate(BaseCache, tagName, rmt2, pixl, vtsh);

            return true;
        }

        /// <summary>
        /// Returns the best render_method_template tag match using the given dictionary and source rmt2
        /// </summary>
        private CachedTag GetBestTag(SortingInterface sortingInterface, Dictionary<CachedTag, long> shaderTemplateValues, string newTagName, string srcTagName)
        {
            long targetValue = Sorter.GetValue(sortingInterface, Sorter.GetTemplateOptions(newTagName));
            long bestValue = long.MaxValue;
            CachedTag bestTag = null;

            foreach (var pair in shaderTemplateValues)
            {
                if (Math.Abs(pair.Value - targetValue) < bestValue)
                {
                    bestValue = Math.Abs(pair.Value - targetValue);
                    bestTag = pair.Key;
                }
            }
            /*
            Console.WriteLine($"Closest tag to {srcTagName} with options and value {targetValue}");
            sortingInterface.PrintOptions(Sorter.GetTemplateOptions(newTagName));
            Console.WriteLine($"is tag {bestTag.Name} with options and value {bestValue + targetValue}");
            sortingInterface.PrintOptions(Sorter.GetTemplateOptions(bestTag.Name));
            */
            return bestTag;
        }

        /// <summary>
        /// Rebuilds an rmt2's options in memory so indices match up with the base cache
        /// </summary>
        private Rmt2Descriptor RebuildRmt2Options(Rmt2Descriptor srcRmt2Descriptor)
        {
            if (srcRmt2Descriptor.Type != "black" && PortingCache.Version >= CacheVersion.Halo3Beta)
            {
                string rmdfName = $"shaders\\{srcRmt2Descriptor.Type}.rmdf";
                if (!RenderMethodDefinitions.ContainsKey(srcRmt2Descriptor.Type) || !PortingRenderMethodDefinitions.ContainsKey(srcRmt2Descriptor.Type))
                    return srcRmt2Descriptor;

                var baseRmdfDefinition = RenderMethodDefinitions[srcRmt2Descriptor.Type];
                var portingRmdfDefinition = PortingRenderMethodDefinitions[srcRmt2Descriptor.Type];

                List<byte> newOptions = new List<byte>();

                // get option indices (if we loop by basecache rmdf, the options will always be correct length and index)
                for (int i = 0; i < baseRmdfDefinition.Categories.Count; i++)
                {
                    byte option = 0;

                    string methodName = BaseCache.StringTable.GetString(baseRmdfDefinition.Categories[i].Name);

                    if (PortingCache.Version >= CacheVersion.HaloReach && methodName == "reach_compatibility")
                    {
                        if (portingRmdfDefinition.GetCategoryOption(PortingCache, "detail", srcRmt2Descriptor.Options) == "repeat")
                        {
                            int potentialIndex = baseRmdfDefinition.GetCategoryOptionIndex(BaseCache, "reach_compatibility", "enabled_detail_repeat");
                            newOptions.Add(potentialIndex != -1 ? (byte)potentialIndex : (byte)1);
                        }
                        else
                        {
                            newOptions.Add(1);
                        }
                        continue;
                    }

                    for (int j = 0; j < portingRmdfDefinition.Categories.Count; j++)
                    {
                        if (methodName != PortingCache.StringTable.GetString(portingRmdfDefinition.Categories[j].Name))
                            continue;

                        int portingOptionIndex = srcRmt2Descriptor.Options[j];
                        string optionName = PortingCache.StringTable.GetString(portingRmdfDefinition.Categories[j].ShaderOptions[portingOptionIndex].Name);

                        // these are perfect option matches
                        // do not touch unless verified
                        if (srcRmt2Descriptor.Type == "shader")
                        {
                            if (methodName == "self_illumination" && optionName == "change_color")
                                optionName = "illum_change_color";
                        }
                        if (methodName == "misc" && optionName == "default")
                            optionName = "always_calc_albedo";
                        if (methodName == "alpha_test" && optionName == "from_texture")
                            optionName = "simple";
                        //if (PortingCache.Version == CacheVersion.Halo3ODST && methodName == "material_model" && optionName == "cook_torrance")
                        //    optionName = "cook_torrance_odst";
                        //if (methodName == "material_model" && optionName == "cook_torrance_rim_fresnel")
                        //    optionName = "cook_torrance";

                        if (PortingCache.Version == CacheVersion.HaloReach)
                        {
                            // keep in sync with cubemap conversion - not needed anymore?
                            //if (methodName == "environment_mapping" && optionName == "dynamic")
                            //{
                            //    optionName = "dynamic_reach";
                            //}
                            if (methodName == "material_model")
                            {
                                if (optionName == "cook_torrance")
                                    optionName = "cook_torrance_reach";
                                else if (optionName == "two_lobe_phong")
                                    optionName = "two_lobe_phong_reach";
                                //else if (optionName == "organism")
                                //    optionName = "organism_reach";
                            }
                        }

                        // TODO: fill this switch, Reach shadergen might take some time...
                        // fixup names (remove when full rmdf + shader generation for each gen3 game)
                        switch ($"{methodName}\\{optionName}")
                        {
                            // Reach rmsh //
                            case @"albedo\patchy_emblem":
                                optionName = "emblem_change_color";
                                break;
                            case @"bump_mapping\detail_blend":
                            case @"bump_mapping\three_detail_blend":
                                optionName = "detail";
                                break;
                            case @"specular_mask\specular_mask_mult_diffuse":
                                optionName = "specular_mask_from_texture";
                                break;
                            // Reach rmtr  //
                            case @"blending\distance_blend_base":
                                optionName = "morph";
                                break;
                            // Reach rmfl //
                            case @"material_model\flat":
                            case @"material_model\specular":
                            case @"material_model\translucent":
                                optionName = "default";
                                break;
                            // Reach prt3 //
                            case @"lighting\per_pixel_smooth":
                            case @"lighting\smoke_lighting":
                                optionName = "per_pixel_ravi_order_3";
                                break;
                            case @"lighting\per_vertex_ambient":
                                optionName = "per_vertex_ravi_order_0";
                                break;
                            case @"depth_fade\low_res":
                                optionName = "on";
                                break;
                            // MCC rmsh //
                            case @"material_model\cook_torrance_pbr_maps":
                                optionName = "cook_torrance";
                                break;
                        }

                        bool matchFound = false;
                        // get basecache option index
                        for (int k = 0; k < baseRmdfDefinition.Categories[i].ShaderOptions.Count; k++)
                        {
                            if (optionName == BaseCache.StringTable.GetString(baseRmdfDefinition.Categories[i].ShaderOptions[k].Name))
                            {
                                option = (byte)k;
                                matchFound = true;
                                break;
                            }
                        }

                        if (!matchFound)
                        {
                            new TagToolWarning($"Unrecognized {srcRmt2Descriptor.Type} method option \"{methodName}\\{optionName}\"");
                        }
                        break;
                    }
                    newOptions.Add(option);
                }

                srcRmt2Descriptor.Options = newOptions.ToArray();
            }

            return srcRmt2Descriptor;
        }

        public class Rmt2Pairing
        {
            public Rmt2ParameterMatch RealParams;
            public Rmt2ParameterMatch IntParams;
            public Rmt2ParameterMatch BoolParams;
            public Rmt2ParameterMatch TextureParams;
            public int CommonOptions;
            public int Score;
            public CachedTag DestTag;
            public CachedTag SourceTag;

            public int CommonParameters =>
                  RealParams.Common
                + TextureParams.Common
                + IntParams.Common
                + BoolParams.Common;

            public int MissingFromSource =>
                  RealParams.MissingFromSource
                + TextureParams.MissingFromSource
                + IntParams.MissingFromSource
                + BoolParams.MissingFromSource;

            public int MissingFromDest =>
                  RealParams.MissingFromDest
                + TextureParams.MissingFromDest
                + IntParams.MissingFromDest
                + BoolParams.MissingFromDest;
        }

        public struct Rmt2ParameterMatch
        {
            public int MissingFromSource;
            public int MissingFromDest;
            public int Common;
            public int SourceCount;
            public int DestCount;
        }

        public struct Rmt2Descriptor
        {
            public DescriptorFlags Flags;
            public string Type;
            public byte[] Options;
            private bool HasParsed;

            [Flags]
            public enum DescriptorFlags
            {
                None = 0,
                Ms30 = (1 << 0)
            }

            public Rmt2Descriptor(string type, byte[] options)
            {
                Type = type;
                Options = options;
                HasParsed = true;
                Flags = DescriptorFlags.None;
            }

            public bool IsMs30 => Flags.HasFlag(DescriptorFlags.Ms30);

            public string GetRmdfName()
            {
                if (!HasParsed)
                    return null;
                return $"{(IsMs30 ? "ms30\\" : "")}shaders\\{Type}";
            }

            public static bool TryParse(string name, out Rmt2Descriptor descriptor)
            {
                descriptor = new Rmt2Descriptor();

                descriptor.HasParsed = false;

                var parts = name.Split(new string[] { "shaders\\" }, StringSplitOptions.None);

                var prefixParts = parts[0].Split('\\');
                if (prefixParts.Length > 0 && prefixParts[0] == "ms30")
                    descriptor.Flags |= DescriptorFlags.Ms30;

                if (parts.Length < 2)
                    return false;
                var nameParts = parts[1].Split('\\');
                if (nameParts.Length < 2)
                    return false;

                descriptor.Type = nameParts[0].Substring(0, nameParts[0].Length-10);
                descriptor.Options = nameParts[1].Split('_').Skip(1).Select(x => byte.Parse(x)).ToArray();
                descriptor.HasParsed = true;

                return true;
            }

            public HaloShaderGenerator.Generator.IShaderGenerator GetGenerator(bool applyFixes = false)
            {
                if (HasParsed && !IsMs30)
                {
                    switch (Type)
                    {
                        case "beam":            return new HaloShaderGenerator.Beam.BeamGenerator(Options, applyFixes);
                        case "black":           return new HaloShaderGenerator.Black.ShaderBlackGenerator();
                        case "contrail":        return new HaloShaderGenerator.Contrail.ContrailGenerator(Options, applyFixes);
                        case "cortana":         return new HaloShaderGenerator.Cortana.CortanaGenerator(Options, applyFixes);
                        case "custom":          return new HaloShaderGenerator.Custom.CustomGenerator(Options, applyFixes);
                        case "decal":           return new HaloShaderGenerator.Decal.DecalGenerator(Options, applyFixes);
                        case "foliage":         return new HaloShaderGenerator.Foliage.FoliageGenerator(Options, applyFixes);
                        //case "glass":           return new HaloShaderGenerator.Glass.GlassGenerator(Options, applyFixes);
                        case "halogram":        return new HaloShaderGenerator.Halogram.HalogramGenerator(Options, applyFixes);
                        case "light_volume":    return new HaloShaderGenerator.LightVolume.LightVolumeGenerator(Options, applyFixes);
                        case "particle":        return new HaloShaderGenerator.Particle.ParticleGenerator(Options, applyFixes);
                        case "screen":          return new HaloShaderGenerator.Screen.ScreenGenerator(Options, applyFixes);
                        case "shader":          return new HaloShaderGenerator.Shader.ShaderGenerator(Options, applyFixes);
                        case "terrain":         return new HaloShaderGenerator.Terrain.TerrainGenerator(Options, applyFixes);
                        case "water":           return new HaloShaderGenerator.Water.WaterGenerator(Options, applyFixes);
                        case "zonly":           return new HaloShaderGenerator.ZOnly.ZOnlyGenerator(Options, applyFixes);
                    }

                    Console.WriteLine($"\"{Type}\" shader generation is currently unsupported.");
                    return null;
                }

                Console.WriteLine("Invalid descriptor.");
                return null;
            }
        }

        public CachedTag FindRmdf(CachedTag matchedRmt2Tag)
        {
            Rmt2Descriptor rmt2Description;
            if (!Rmt2Descriptor.TryParse(matchedRmt2Tag.Name, out rmt2Description))
                throw new ArgumentException($"Invalid rmt2 name '{matchedRmt2Tag.Name}'", nameof(matchedRmt2Tag));

            string prefix = matchedRmt2Tag.Name.StartsWith("ms30") ? "ms30\\" : "";
            string type = rmt2Description.Type; // remove _templates
            string rmdfName = $"{prefix}shaders\\{type}";

            return BaseCache.TagCache.GetTag(rmdfName, "rmdf");
        }
    }
}
