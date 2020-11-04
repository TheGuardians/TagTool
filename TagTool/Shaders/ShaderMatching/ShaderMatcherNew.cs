using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using HaloShaderGenerator.Shader;
using static TagTool.Tags.Definitions.RenderMethodTemplate;

namespace TagTool.Shaders.ShaderMatching
{
    public class ShaderMatcherNew
    {
        private GameCache BaseCache;
        private GameCache PortingCache;
        private Stream BaseCacheStream;
        private Stream PortingCacheStream;
        private Dictionary<CachedTag, RenderMethodTemplate> _rmt2Cache;

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
            IsInitialized = false;
        }

        public Dictionary<StringId, RenderMethodOption.OptionBlock.OptionDataType> GetOptionParameters(List<byte> options, RenderMethodDefinition rmdf)
        {
            Dictionary<StringId, RenderMethodOption.OptionBlock.OptionDataType> optionParameters = new Dictionary<StringId, RenderMethodOption.OptionBlock.OptionDataType>();

            for (int i = 0; i < options.Count; i++)
            {
                if (rmdf.Methods[i].ShaderOptions[options[i]].Option != null)
                {
                    var rmop = BaseCache.Deserialize<RenderMethodOption>(BaseCacheStream, rmdf.Methods[i].ShaderOptions[options[i]].Option);
                    foreach (var parameter in rmop.Options)
                        if (!optionParameters.ContainsKey(parameter.Name))
                            optionParameters.Add(parameter.Name, parameter.Type);
                }
            }

            return optionParameters;
        }

        public Dictionary<StringId, RenderMethodOption.OptionBlock> GetOptionBlocks(List<byte> options, RenderMethodDefinition rmdf)
        {
            Dictionary<StringId, RenderMethodOption.OptionBlock> optionBlocks = new Dictionary<StringId, RenderMethodOption.OptionBlock>();

            for (int i = 0; i < options.Count; i++)
            {
                if (rmdf.Methods[i].ShaderOptions[options[i]].Option != null)
                {
                    var rmop = BaseCache.Deserialize<RenderMethodOption>(BaseCacheStream, rmdf.Methods[i].ShaderOptions[options[i]].Option);
                    foreach (var parameter in rmop.Options)
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
                if (rmdf.Methods[i].ShaderOptions[options[i]].Option != null)
                {
                    var rmop = BaseCache.Deserialize<RenderMethodOption>(BaseCacheStream, rmdf.Methods[i].ShaderOptions[options[i]].Option);
                    foreach (var parameter in rmop.Options)
                        if (parameter.Type == RenderMethodOption.OptionBlock.OptionDataType.Sampler && parameter.DefaultSamplerBitmap != null && !optionBitmaps.ContainsKey(parameter.Name))
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
                throw new ArgumentException($"Invalid rmt2 name '{sourceRmt2Tag.Name}'", nameof(sourceRmt2Tag));

            // rebuild options to match base cache
            sourceRmt2Desc = RebuildRmt2Options(sourceRmt2Desc, BaseCacheStream, PortingCacheStream);

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

            // if we've reached here, we haven't found an extract match.
            // now we need to consider other factors such as which options they have, which parameters are missing etc..
            // whatever can be used to narrow it down.
            Console.WriteLine($"No rmt2 match found for {sourceRmt2Tag.Name}");

            if (canGenerate && TryGenerateTemplate(sourceRmt2Desc, out CachedTag generatedRmt2))
            {
                Console.WriteLine($"Generated rmt2 \"{generatedRmt2.Name}\"");
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

        private bool TryGenerateTemplate(Rmt2Descriptor rmt2Desc, out CachedTag generatedRmt2)
        {
            generatedRmt2 = null;

            if (rmt2Desc.Type == "shader")
            {
                string tagName = $"shaders\\shader_templates\\_{string.Join("_", rmt2Desc.Options)}";

                // needs unapply rotation for misc3 + parallax
                bool miscUnsupported = rmt2Desc.Options[9] == 3 && rmt2Desc.Options[8] != 0;

                if (rmt2Desc.Options[4] != 8 && !miscUnsupported) // 8 is hair in odst
                {
                    var generator = new HaloShaderGenerator.Shader.ShaderGenerator((Albedo)rmt2Desc.Options[0], (Bump_Mapping)rmt2Desc.Options[1], (Alpha_Test)rmt2Desc.Options[2], (Specular_Mask)rmt2Desc.Options[3], (Material_Model)rmt2Desc.Options[4], (Environment_Mapping)rmt2Desc.Options[5], (Self_Illumination)rmt2Desc.Options[6], (Blend_Mode)rmt2Desc.Options[7], (Parallax)rmt2Desc.Options[8], (Misc)rmt2Desc.Options[9], 0);

                    // TODO: generate rmdf\glvs\glps if not found

                    var rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, BaseCache.TagCache.GetTag("shaders\\shader.rmdf"));
                    var glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                    var glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);

                    var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, generator, tagName);

                    generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

                    BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                    return true;
                }
            }
            else if (rmt2Desc.Type == "black")
            {
                string tagName = $"shaders\\black_templates\\_0";

                var bkGenerator = new HaloShaderGenerator.Black.ShaderBlackGenerator();

                RenderMethodDefinition rmdf;
                GlobalPixelShader glps;
                GlobalVertexShader glvs;

                if (!BaseCache.TagCache.TryGetTag("shaders\\black.rmdf", out var rmdfTag))
                {
                    rmdf = ShaderGenerator.ShaderGenerator.GenerateRenderMethodDefinition(BaseCache, BaseCacheStream, bkGenerator, "black", out glps, out glvs);
                    rmdfTag = BaseCache.TagCache.AllocateTag<RenderMethodDefinition>("shaders\\black");
                    BaseCache.Serialize(BaseCacheStream, rmdfTag, rmdf);
                }
                else
                {
                    rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, rmdfTag);

                    if (rmdf.GlobalVertexShader == null || rmdf.GlobalPixelShader == null)
                    {
                        // generate rmdf (this is not serialized) to get global shaders
                        var tempRmdf = ShaderGenerator.ShaderGenerator.GenerateRenderMethodDefinition(BaseCache, BaseCacheStream, bkGenerator, "black", out _, out _);
                        rmdf.GlobalVertexShader = tempRmdf.GlobalVertexShader;
                        rmdf.GlobalPixelShader = tempRmdf.GlobalPixelShader;
                        BaseCache.Serialize(BaseCacheStream, rmdfTag, rmdf);
                    }

                    glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                    glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);
                }

                var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, bkGenerator, tagName);

                generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

                BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                return true;
            }
            else if (rmt2Desc.Type == "particle")
            {
                string tagName = $"shaders\\particle_templates\\_{string.Join("_", rmt2Desc.Options)}";

                HaloShaderGenerator.Particle.Albedo albedo = (HaloShaderGenerator.Particle.Albedo)rmt2Desc.Options[0];
                HaloShaderGenerator.Particle.Blend_Mode blend_mode = (HaloShaderGenerator.Particle.Blend_Mode)rmt2Desc.Options[1];
                HaloShaderGenerator.Particle.Specialized_Rendering specialized_rendering = (HaloShaderGenerator.Particle.Specialized_Rendering)rmt2Desc.Options[2];
                HaloShaderGenerator.Particle.Lighting lighting = (HaloShaderGenerator.Particle.Lighting)rmt2Desc.Options[3];
                HaloShaderGenerator.Particle.Render_Targets render_targets = (HaloShaderGenerator.Particle.Render_Targets)rmt2Desc.Options[4];
                HaloShaderGenerator.Particle.Depth_Fade depth_fade = (HaloShaderGenerator.Particle.Depth_Fade)rmt2Desc.Options[5];
                HaloShaderGenerator.Particle.Black_Point black_point = (HaloShaderGenerator.Particle.Black_Point)rmt2Desc.Options[6];
                HaloShaderGenerator.Particle.Fog fog = (HaloShaderGenerator.Particle.Fog)rmt2Desc.Options[7];
                HaloShaderGenerator.Particle.Frame_Blend frame_blend = (HaloShaderGenerator.Particle.Frame_Blend)rmt2Desc.Options[8];
                HaloShaderGenerator.Particle.Self_Illumination self_Illumination = (HaloShaderGenerator.Particle.Self_Illumination)rmt2Desc.Options[9];

                if (rmt2Desc.Options[0] == 7) // no examples for this in odst, use another build or reach maybe?
                    return false;

                var generator = new HaloShaderGenerator.Particle.ParticleGenerator(albedo, blend_mode, specialized_rendering, lighting, render_targets, depth_fade, black_point, fog, frame_blend, self_Illumination, true);

                // TODO: generate rmdf\glvs\glps if not found

                var rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, BaseCache.TagCache.GetTag("shaders\\particle.rmdf"));
                var glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                var glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);

                var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, generator, tagName);

                generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

                BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                return true;
            }
            else if (rmt2Desc.Type == "contrail")
            {
                string tagName = $"shaders\\contrail_templates\\_{string.Join("_", rmt2Desc.Options)}";

                HaloShaderGenerator.Contrail.Albedo albedo = (HaloShaderGenerator.Contrail.Albedo)rmt2Desc.Options[0];
                HaloShaderGenerator.Contrail.Blend_Mode blend_mode = (HaloShaderGenerator.Contrail.Blend_Mode)rmt2Desc.Options[1];
                HaloShaderGenerator.Contrail.Black_Point black_point = (HaloShaderGenerator.Contrail.Black_Point)rmt2Desc.Options[2];
                HaloShaderGenerator.Contrail.Fog fog = (HaloShaderGenerator.Contrail.Fog)rmt2Desc.Options[3];

                var generator = new HaloShaderGenerator.Contrail.ContrailGenerator(albedo, blend_mode, black_point, fog, true);

                // TODO: generate rmdf\glvs\glps if not found

                var rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, BaseCache.TagCache.GetTag($"shaders\\{rmt2Desc.Type}.rmdf"));
                var glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                var glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);

                var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, generator, tagName);

                generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

                BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                return true;
            }
            else if (rmt2Desc.Type == "beam")
            {
                string tagName = $"shaders\\beam_templates\\_{string.Join("_", rmt2Desc.Options)}";

                HaloShaderGenerator.Beam.Albedo albedo = (HaloShaderGenerator.Beam.Albedo)rmt2Desc.Options[0];
                HaloShaderGenerator.Beam.Blend_Mode blend_mode = (HaloShaderGenerator.Beam.Blend_Mode)rmt2Desc.Options[1];
                HaloShaderGenerator.Beam.Black_Point black_point = (HaloShaderGenerator.Beam.Black_Point)rmt2Desc.Options[2];
                HaloShaderGenerator.Beam.Fog fog = (HaloShaderGenerator.Beam.Fog)rmt2Desc.Options[3];

                var generator = new HaloShaderGenerator.Beam.BeamGenerator(albedo, blend_mode, black_point, fog, true);

                // TODO: generate rmdf\glvs\glps if not found

                var rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, BaseCache.TagCache.GetTag($"shaders\\{rmt2Desc.Type}.rmdf"));
                var glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                var glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);

                var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, generator, tagName);

                generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

                BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                return true;
            }
            else if (rmt2Desc.Type == "light_volume")
            {
                string tagName = $"shaders\\light_volume_templates\\_{string.Join("_", rmt2Desc.Options)}";

                HaloShaderGenerator.LightVolume.Albedo albedo = (HaloShaderGenerator.LightVolume.Albedo)rmt2Desc.Options[0];
                HaloShaderGenerator.LightVolume.Blend_Mode blend_mode = (HaloShaderGenerator.LightVolume.Blend_Mode)rmt2Desc.Options[1];
                HaloShaderGenerator.LightVolume.Fog fog = (HaloShaderGenerator.LightVolume.Fog)rmt2Desc.Options[2];

                var generator = new HaloShaderGenerator.LightVolume.LightVolumeGenerator(albedo, blend_mode, fog, true);

                // TODO: generate rmdf\glvs\glps if not found

                var rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, BaseCache.TagCache.GetTag($"shaders\\{rmt2Desc.Type}.rmdf"));
                var glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                var glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);

                var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, generator, tagName);

                generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

                BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                return true;
            }
            else if (rmt2Desc.Type == "decal")
            {
                string tagName = $"shaders\\decal_templates\\_{string.Join("_", rmt2Desc.Options)}";

                HaloShaderGenerator.Decal.Albedo albedo = (HaloShaderGenerator.Decal.Albedo)rmt2Desc.Options[0];
                HaloShaderGenerator.Decal.Blend_Mode blend_mode = (HaloShaderGenerator.Decal.Blend_Mode)rmt2Desc.Options[1];
                HaloShaderGenerator.Decal.Render_Pass render_pass = (HaloShaderGenerator.Decal.Render_Pass)rmt2Desc.Options[2];
                HaloShaderGenerator.Decal.Specular specular = (HaloShaderGenerator.Decal.Specular)rmt2Desc.Options[3];
                HaloShaderGenerator.Decal.Bump_Mapping bump_mapping = (HaloShaderGenerator.Decal.Bump_Mapping)rmt2Desc.Options[4];
                HaloShaderGenerator.Decal.Tinting tinting = (HaloShaderGenerator.Decal.Tinting)rmt2Desc.Options[5];

                var generator = new HaloShaderGenerator.Decal.DecalGenerator(albedo, blend_mode, render_pass, specular, bump_mapping, tinting, true);

                // TODO: generate rmdf\glvs\glps if not found

                var rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, BaseCache.TagCache.GetTag($"shaders\\{rmt2Desc.Type}.rmdf"));
                var glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                var glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);

                var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, generator, tagName);

                generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

                BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                return true;
            }
            else if (rmt2Desc.Type == "halogram")
            {
                string tagName = $"shaders\\halogram_templates\\_{string.Join("_", rmt2Desc.Options)}";

                HaloShaderGenerator.Halogram.Albedo albedo = (HaloShaderGenerator.Halogram.Albedo)rmt2Desc.Options[0];
                HaloShaderGenerator.Halogram.Self_Illumination self_illumination = (HaloShaderGenerator.Halogram.Self_Illumination)rmt2Desc.Options[1];
                HaloShaderGenerator.Halogram.Blend_Mode blend_mode = (HaloShaderGenerator.Halogram.Blend_Mode)rmt2Desc.Options[2];
                HaloShaderGenerator.Halogram.Misc misc = (HaloShaderGenerator.Halogram.Misc)rmt2Desc.Options[3];
                HaloShaderGenerator.Halogram.Warp warp = (HaloShaderGenerator.Halogram.Warp)rmt2Desc.Options[4];
                HaloShaderGenerator.Halogram.Overlay overlay = (HaloShaderGenerator.Halogram.Overlay)rmt2Desc.Options[5];
                HaloShaderGenerator.Halogram.Edge_Fade edge_fade = (HaloShaderGenerator.Halogram.Edge_Fade)rmt2Desc.Options[6];

                var generator = new HaloShaderGenerator.Halogram.HalogramGenerator(albedo, self_illumination, blend_mode, misc, warp, overlay, edge_fade, true);

                // TODO: generate rmdf\glvs\glps if not found

                var rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, BaseCache.TagCache.GetTag($"shaders\\{rmt2Desc.Type}.rmdf"));
                var glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                var glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);

                var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, generator, tagName);

                generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

                BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                return true;
            }
            else if (rmt2Desc.Type == "custom")
            {
                string tagName = $"shaders\\custom_templates\\_{string.Join("_", rmt2Desc.Options)}";

                // needs unapply rotation for misc3 + parallax
                bool miscUnsupported = rmt2Desc.Options[9] == 3 && rmt2Desc.Options[8] != 0;

                if (!miscUnsupported)
                {
                    var generator = new HaloShaderGenerator.Custom.CustomGenerator((HaloShaderGenerator.Custom.Albedo)rmt2Desc.Options[0], (HaloShaderGenerator.Custom.Bump_Mapping)rmt2Desc.Options[1], (HaloShaderGenerator.Custom.Alpha_Test)rmt2Desc.Options[2], (HaloShaderGenerator.Custom.Specular_Mask)rmt2Desc.Options[3], (HaloShaderGenerator.Custom.Material_Model)rmt2Desc.Options[4], (HaloShaderGenerator.Custom.Environment_Mapping)rmt2Desc.Options[5], (HaloShaderGenerator.Custom.Self_Illumination)rmt2Desc.Options[6], (HaloShaderGenerator.Custom.Blend_Mode)rmt2Desc.Options[7], (HaloShaderGenerator.Custom.Parallax)rmt2Desc.Options[8], (HaloShaderGenerator.Custom.Misc)rmt2Desc.Options[9]);

                    GlobalPixelShader glps;
                    GlobalVertexShader glvs;
                    RenderMethodDefinition rmdf;
                    CachedTag rmdfTag;
                    if (!BaseCache.TagCache.TryGetTag("shaders\\custom.rmdf", out rmdfTag))
                    {
                        rmdf = ShaderGenerator.ShaderGenerator.GenerateRenderMethodDefinition(BaseCache, BaseCacheStream, generator, "custom", out glps, out glvs);
                        rmdfTag = BaseCache.TagCache.AllocateTag<RenderMethodDefinition>("shaders\\custom");
                        BaseCache.Serialize(BaseCacheStream, rmdfTag, rmdf);
                        (BaseCache as GameCacheHaloOnlineBase).SaveTagNames();
                    }
                    else
                    {
                        rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, rmdfTag);
                        glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                        glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);
                    }

                    var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, generator, tagName);

                    generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);

                    BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                    return true;
                }
            }
            else if (rmt2Desc.Type == "screen")
            {
                string tagName = $"shaders\\{rmt2Desc.Type}_templates\\_{string.Join("_", rmt2Desc.Options)}";


                var generator = new HaloShaderGenerator.Screen.ScreenGenerator((HaloShaderGenerator.Screen.Warp)rmt2Desc.Options[0], (HaloShaderGenerator.Screen.Base)rmt2Desc.Options[1], (HaloShaderGenerator.Screen.Overlay_A)rmt2Desc.Options[2], (HaloShaderGenerator.Screen.Overlay_B)rmt2Desc.Options[3], (HaloShaderGenerator.Screen.Blend_Mode)rmt2Desc.Options[4]);
            
                GlobalPixelShader glps;
                GlobalVertexShader glvs;
                RenderMethodDefinition rmdf;
                CachedTag rmdfTag;
                if (!BaseCache.TagCache.TryGetTag($"shaders\\{rmt2Desc.Type}.rmdf", out rmdfTag))
                {
                    rmdf = ShaderGenerator.ShaderGenerator.GenerateRenderMethodDefinition(BaseCache, BaseCacheStream, generator, rmt2Desc.Type, out glps, out glvs);
                    rmdfTag = BaseCache.TagCache.AllocateTag<RenderMethodDefinition>($"shaders\\{rmt2Desc.Type}");
                    BaseCache.Serialize(BaseCacheStream, rmdfTag, rmdf);
                    (BaseCache as GameCacheHaloOnlineBase).SaveTagNames();
                }
                else
                {
                    rmdf = BaseCache.Deserialize<RenderMethodDefinition>(BaseCacheStream, rmdfTag);
                    glps = BaseCache.Deserialize<GlobalPixelShader>(BaseCacheStream, rmdf.GlobalPixelShader);
                    glvs = BaseCache.Deserialize<GlobalVertexShader>(BaseCacheStream, rmdf.GlobalVertexShader);
                }
            
                var rmt2 = ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(BaseCache, BaseCacheStream, rmdf, glps, glvs, generator, tagName);
            
                generatedRmt2 = BaseCache.TagCache.AllocateTag<RenderMethodTemplate>(tagName);
            
                BaseCache.Serialize(BaseCacheStream, generatedRmt2, rmt2);
                return true;
            }

            return false;
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
                for (int i = 0; i < baseRmdfDefinition.Methods.Count; i++)
                {
                    byte option = 0;

                    string methodName = BaseCache.StringTable.GetString(baseRmdfDefinition.Methods[i].Type);

                    for (int j = 0; j < portingRmdfDefinition.Methods.Count; j++)
                    {
                        if (methodName != PortingCache.StringTable.GetString(portingRmdfDefinition.Methods[j].Type))
                            continue;

                        int portingOptionIndex = srcRmt2Descriptor.Options[j];
                        string optionName = PortingCache.StringTable.GetString(portingRmdfDefinition.Methods[j].ShaderOptions[portingOptionIndex].Type);

                        // TODO: fill this switch, Reach shadergen might take some time...
                        // fixup names (remove when full rmdf + shader generation for each gen3 game)
                        switch ($"{methodName}\\{optionName}")
                        {
                            case @"lighting\per_pixel_smooth":
                            case @"lighting\smoke_lighting":
                                optionName = "per_pixel_ravi_order_3";
                                break;
                            case @"lighting\per_vertex_ambient":
                                optionName = "per_vertex_ravi_order_0";
                                break;
                            case @"depth_fade\low_res":
                            case @"depth_fade\palette_shift":
                                optionName = "on";
                                break;
                            case @"self_illumination\palettized_depth_fade":
                                optionName = "palettized_plasma";
                                break;
                            case @"albedo\patchy_emblem":
                                optionName = "emblem_change_color";
                                break;
                        }

                        // get basecache option index
                        for (int k = 0; k < baseRmdfDefinition.Methods[i].ShaderOptions.Count; k++)
                        {
                            if (optionName == BaseCache.StringTable.GetString(baseRmdfDefinition.Methods[i].ShaderOptions[k].Type))
                            {
                                option = (byte)k;
                                break;
                            }
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

        class Rmt2Pairing
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

        struct Rmt2ParameterMatch
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

            [Flags]
            public enum DescriptorFlags
            {
                Ms30 = (1 << 0)
            }

            public bool IsMs30 => Flags.HasFlag(DescriptorFlags.Ms30);

            public static bool TryParse(string name, out Rmt2Descriptor descriptor)
            {
                descriptor = new Rmt2Descriptor();

                var parts = name.Split(new string[] { "shaders\\" }, StringSplitOptions.None);

                var prefixParts = parts[0].Split('\\');
                if (prefixParts.Length > 0 && prefixParts[0] == "ms30")
                    descriptor.Flags |= DescriptorFlags.Ms30;

                var nameParts = parts[1].Split('\\');
                if (nameParts.Length < 2)
                    return false;

                descriptor.Type = nameParts[0].Substring(0, nameParts[0].Length-10);
                descriptor.Options = nameParts[1].Split('_').Skip(1).Select(x => byte.Parse(x)).ToArray();

                return true;
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
            foreach(var method in rmdf.Methods)
            {
                var str = cache.StringTable.GetString(method.Type);
                result.Add(str);
            }
            return result;
        }

        private List<string> GetMethodOptions(RenderMethodDefinition rmdf, int methodIndex, GameCache cache)
        {
            var result = new List<string>();
            var method = rmdf.Methods[methodIndex];
            foreach(var option in method.ShaderOptions)
            {
                result.Add(cache.StringTable.GetString(option.Type));
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

            CachedTag bestRmt2 = null;
            var bestScore = -1;

        }
    }
}
