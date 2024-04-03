using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Commands.Common;
using TagTool.Common;
using TagTool.Tags.Definitions;
using HaloShaderGenerator.Shader;
using static TagTool.Tags.Definitions.RenderMethodTemplate;
using TagTool.Cache.HaloOnline;

namespace TagTool.Shaders.ShaderMatching
{
    public class ShaderMatcherNew
    {
        private GameCache BaseCache;
        private GameCache PortingCache;
        private Stream BaseCacheStream;
        private Stream PortingCacheStream;
        private Dictionary<CachedTag, RenderMethodTemplate> _rmt2Cache;
        private List<string> UpdatedRmdf;

        public static string DefaultTemplate => @"shaders\shader_templates\_0_0_0_0_0_0_0_0_0_0_0.rmt2";
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

        public void Init(GameCache baseCache, GameCache portingCache, Stream baseCacheStream, Stream portingCacheStream, bool useMS30 = false, bool perfectMatchesOnly = false)
        {
            UseMs30 = useMS30;
            PerfectMatchesOnly = perfectMatchesOnly;
            BaseCache = baseCache;
            PortingCache = portingCache;
            BaseCacheStream = baseCacheStream;
            PortingCacheStream = portingCacheStream;
            _rmt2Cache = new Dictionary<CachedTag, RenderMethodTemplate>();
            UpdatedRmdf = new List<string>();
            IsInitialized = true;
        }

        public void DeInit()
        {
            UseMs30 = false;
            PerfectMatchesOnly = false;
            BaseCache = null;
            PortingCache = null;
            BaseCacheStream = null;
            PortingCacheStream = null;
            UpdatedRmdf = null;
            IsInitialized = false;
        }

        public Dictionary<StringId, RenderMethodOption.ParameterBlock.OptionDataType> GetOptionParameters(List<byte> options, RenderMethodDefinition rmdf)
        {
            Dictionary<StringId, RenderMethodOption.ParameterBlock.OptionDataType> optionParameters = new Dictionary<StringId, RenderMethodOption.ParameterBlock.OptionDataType>();

            for (int i = 0; i < options.Count; i++)
            {
                if (rmdf.Categories[i].ShaderOptions[options[i]].Option != null)
                {
                    var rmop = BaseCache.Deserialize<RenderMethodOption>(BaseCacheStream, rmdf.Categories[i].ShaderOptions[options[i]].Option);
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
                    var rmop = BaseCache.Deserialize<RenderMethodOption>(BaseCacheStream, rmdf.Categories[i].ShaderOptions[options[i]].Option);
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
                    var rmop = BaseCache.Deserialize<RenderMethodOption>(BaseCacheStream, rmdf.Categories[i].ShaderOptions[options[i]].Option);
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

            //if (!UpdatedRmdf.Contains(sourceRmt2Desc.Type)) // will update or generate rmdf as needed
            //{
            //    if (!ShaderGenerator.RenderMethodDefinitionGenerator.UpdateRenderMethodDefinition(BaseCache, BaseCacheStream, sourceRmt2Desc.Type))
            //        Console.WriteLine($"WARNING: rmdf for shader type \"{sourceRmt2Desc.Type}\" could not be updated or generated.");
            //    else
            //        Console.WriteLine($"Rmdf for shader type \"{sourceRmt2Desc.Type}\" updated or generated.");
            //    UpdatedRmdf.Add(sourceRmt2Desc.Type);
            //}

            // rebuild options to match base cache
            sourceRmt2Desc = RebuildRmt2Options(sourceRmt2Desc, BaseCacheStream, PortingCacheStream);

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
                    Console.WriteLine("Found perfect rmt2 match:");
                    Console.WriteLine(sourceRmt2Tag.Name);
                    Console.WriteLine(rmt2Tag.Name);
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
                Console.WriteLine($"Found cached rmt2: {tagName}");
                return cachedRmt2Tag;
            }

            // if we've reached here, we haven't found an extract match.
            // now we need to consider other factors such as which options they have, which parameters are missing etc..
            // whatever can be used to narrow it down.
            Console.WriteLine($"No rmt2 match found for {sourceRmt2Tag.Name}");

            if (canGenerate && TryGenerateTemplate(tagName, sourceRmt2Desc, out CachedTag generatedRmt2))
            {
                Console.WriteLine($"Generated rmt2: {generatedRmt2.Name}.{generatedRmt2.Group}");
                return generatedRmt2;
            }

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

        private bool TryGenerateTemplate(string tagName, Rmt2Descriptor rmt2Desc, out CachedTag generatedRmt2)
        {
            generatedRmt2 = null;

            //var generator = rmt2Desc.GetGenerator(true);
            //if (generator == null)
            //    return false;

            RenderMethodDefinition rmdf;
            CachedTag rmdfTag;
            if (!BaseCache.TagCache.TryGetTag($"shaders\\{rmt2Desc.Type}.rmdf", out rmdfTag))
            {
                new TagToolError(CommandError.CustomMessage, $"No rmdf tag present for {rmt2Desc.Type}");
                return false;

                //Console.WriteLine($"Generating rmdf for \"{rmt2Desc.Type}\"");
                //rmdf = ShaderGenerator.RenderMethodDefinitionGenerator.GenerateRenderMethodDefinition(BaseCache, BaseCacheStream, generator, rmt2Desc.Type, out glps, out glvs);
                //rmdfTag = BaseCache.TagCache.AllocateTag<RenderMethodDefinition>($"shaders\\{rmt2Desc.Type}");
                //BaseCache.Serialize(BaseCacheStream, rmdfTag, rmdf);
                //(BaseCache as GameCacheHaloOnlineBase).SaveTagNames();
                //
                //rmt2Desc = RebuildRmt2Options(rmt2Desc, BaseCacheStream, PortingCacheStream);
                //tagName = $"shaders\\{rmt2Desc.Type}_templates\\_{string.Join("_", rmt2Desc.Options)}";
            }
            else
            {
                rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, rmdfTag);
            }

            RenderMethodTemplate rmt2;
            PixelShader pixl;
            VertexShader vtsh;

            try
            {
                rmt2 = ShaderGenerator.ShaderGeneratorNew.GenerateTemplateSafe(BaseCache, BaseCacheStream, rmdf, tagName, out pixl, out vtsh);
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
        /// Modifies the input render method to make it work using the matchedTemplate
        /// </summary>
        private RenderMethod MatchRenderMethods(RenderMethod renderMethod, RenderMethodTemplate matchedTemplate, RenderMethodTemplate originalTemplate)
        {

            return renderMethod;
        }

        /// <summary>
        /// Rebuilds an rmt2's options in memory so indices match up with the base cache
        /// </summary>
        private Rmt2Descriptor RebuildRmt2Options(Rmt2Descriptor srcRmt2Descriptor, Stream baseStream, Stream portingStream)
        {
            if (srcRmt2Descriptor.Type != "black" && PortingCache.Version >= CacheVersion.Halo3Beta)
            {
                string rmdfName = $"shaders\\{srcRmt2Descriptor.Type}.rmdf";
                if (!BaseCache.TagCache.TryGetTag(rmdfName, out var baseRmdfTag) || !PortingCache.TagCache.TryGetTag(rmdfName, out var portingRmdfTag))
                    return srcRmt2Descriptor;

                var baseRmdfDefinition = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, baseRmdfTag);
                var portingRmdfDefinition = PortingCache.Deserialize<RenderMethodDefinition>(PortingCacheStream, portingRmdfTag);

                List<byte> newOptions = new List<byte>();

                // get option indices (if we loop by basecache rmdf, the options will always be correct length and index)
                for (int i = 0; i < baseRmdfDefinition.Categories.Count; i++)
                {
                    byte option = 0;

                    string methodName = BaseCache.StringTable.GetString(baseRmdfDefinition.Categories[i].Name);

                    if (methodName == "reach_compatibility")
                    {
                        newOptions.Add(PortingCache.Version >= CacheVersion.HaloReach ? (byte)1 : (byte)0);
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
                        if (PortingCache.Version == CacheVersion.HaloReach && methodName == "environment_mapping" && optionName == "dynamic")
                            optionName = "dynamic_reach";

                        // TODO: fill this switch, Reach shadergen might take some time...
                        // fixup names (remove when full rmdf + shader generation for each gen3 game)
                        switch ($"{methodName}\\{optionName}")
                        {
                            // Reach rmsh //
                            case @"albedo\patchy_emblem":
                                optionName = "emblem_change_color";
                                break;
                            case @"albedo\color_mask":
                                optionName = "constant_color";
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

        private Rmt2ParameterMatch MatchParameterBlocks(List<ShaderArgument> sourceBlock, List<ShaderArgument> destBlock)
        {
            var result = new Rmt2ParameterMatch();

            var destNames = destBlock.Select(x => BaseCache.StringTable.GetString(x.Name));
            var sourceNames = sourceBlock.Select(x => PortingCache.StringTable.GetString(x.Name));

            result.SourceCount = sourceNames.Count();
            result.DestCount = destNames.Count();
            result.MissingFromDest = sourceNames.Except(destNames).Count();
            result.MissingFromSource = destNames.Except(sourceNames).Count();
            result.Common = destNames.Intersect(sourceNames).Count();

            return result;
        }

        private RenderMethodTemplate GetTemplate(CachedTag tag)
        {
            RenderMethodTemplate template;
            if (!_rmt2Cache.TryGetValue(tag, out template))
                template = _rmt2Cache[tag] = BaseCache.Deserialize<RenderMethodTemplate>(BaseCacheStream, tag);

            return template;
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


        private List<string> GetRenderMethodDefinitionMethods(RenderMethodDefinition rmdf, GameCache cache)
        {
            var result = new List<string>();
            foreach(var method in rmdf.Categories)
            {
                var str = cache.StringTable.GetString(method.Name);
                result.Add(str);
            }
            return result;
        }

        private List<string> GetMethodOptions(RenderMethodDefinition rmdf, int methodIndex, GameCache cache)
        {
            var result = new List<string>();
            var method = rmdf.Categories[methodIndex];
            foreach(var option in method.ShaderOptions)
            {
                result.Add(cache.StringTable.GetString(option.Name));
            }
            return result;
        }

        private void FindClosestShaderTemplate(CachedTag sourceRmt2)
        {
            // somehow build a list of rmt2 of the same type
            List<CachedTag> candidateTemplates = new List<CachedTag>();

            // defined method search order, ignore last method from ms30
            List<int> methodOrder = new List<int> {0, 2, 3, 1, 6, 4, 5, 7, 8, 9, 10};

            Dictionary<CachedTag, int> matchLevelDictionary = new Dictionary<CachedTag, int>();
            Rmt2Descriptor sourceRmt2Desc;
            if (!Rmt2Descriptor.TryParse(sourceRmt2.Name, out sourceRmt2Desc))
                return;

            while (candidateTemplates.Count != 0)
            {
                var template = candidateTemplates.Last();
                Rmt2Descriptor destRmt2Desc;
                if (!Rmt2Descriptor.TryParse(template.Name, out destRmt2Desc))
                {
                    candidateTemplates.Remove(template);
                    matchLevelDictionary[template] = 0;
                }
                else
                {
                    var matchLevel = 0;
                    for(int i = 0; i < methodOrder.Count; i++)
                    {
                        var methodIndex = methodOrder[i];
                        // we need to define a ordering on the method options, so that there is a single best rmt2
                        if (sourceRmt2Desc.Options[methodIndex] == destRmt2Desc.Options[methodIndex])
                            matchLevel++;
                        else
                            break;
                    }
                    matchLevelDictionary[template] = matchLevel;
                } 
            }
        }
    }
}
