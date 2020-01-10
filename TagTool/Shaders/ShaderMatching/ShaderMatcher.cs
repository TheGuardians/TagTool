using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Cache;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders.ShaderMatching
{
    public class ShaderMatcher
    {
        /// <summary>
        /// A dictionary of all ElDorado RenderMethodTemplates and lists of their bitmaps and arguments names.
        /// </summary>
        public Dictionary<int, List<List<string>>> Rmt2TagsInfo { get; set; } = new Dictionary<int, List<List<string>>>();

        /// <summary>
        /// A list of unknown rmhg templates
        /// </summary>
        public List<string> RmhgUnknownTemplates { get; } = new List<string>
        {
            @"shaders\halogram_templates\_0_10_1_1_0_0_0",
            @"shaders\halogram_templates\_0_9_1_1_0_0_0",
            @"shaders\halogram_templates\_0_9_0_1_0_2_0",
            @"shaders\halogram_templates\_0_8_1_0_0_0_0",
            @"shaders\halogram_templates\_0_8_1_0_0_4_1",
            @"shaders\halogram_templates\_0_8_1_0_1_0_0",
            @"shaders\halogram_templates\_2_9_1_1_0_2_0",
            @"shaders\halogram_templates\_0_8_1_0_0_1_0",
            @"shaders\halogram_templates\_0_8_1_0_0_1_1",
            @"shaders\halogram_templates\_0_8_1_0_0_3_1"
        };

        /// <summary>
        /// Halo Online cache context reference
        /// </summary>
        private GameCacheContextHaloOnline CacheContext;

        /// <summary>
        /// CacheFile Reference
        /// </summary>
        private GameCache BlamCache;

        /// <summary>
        /// Cache stream reference
        /// </summary>
        private Stream CacheStream;

        /// <summary>
        /// Use MS30 shaders boolean value. Will affect the selection of
        /// </summary>
        private bool UseMS30;

        /// <summary>
        /// 
        /// </summary>
        private bool IsValid = false;

        //
        // Methods
        //

        /// <summary>
        /// Initializer for ShaderMatcher.
        /// </summary>
        /// <param name="cacheStream"></param>
        /// <param name="cacheContext"></param>
        /// <param name="blamCache"></param>
        public void Init(Stream cacheStream, GameCacheContextHaloOnline cacheContext, GameCache blamCache)
        {
            CacheStream = cacheStream;
            CacheContext = cacheContext;
            BlamCache = blamCache;

            if (Rmt2TagsInfo.Count == 0)
                GetRmt2Info(cacheStream);

            IsValid = true;
        }

        /// <summary>
        /// Check if the MatcherShader class has been properly instantiated.
        /// </summary>
        /// <returns></returns>
        public bool IsInitialized()
        {
            return IsValid;
        }

        /// <summary>
        /// Set the flag to use MS30 to true or false. Reloads the Rmt2TagsInfo Dictionnary
        /// </summary>
        /// <param name="cacheStream"></param>
        /// <param name="useMS30PortTag"></param>
        public void SetMS30Flag(Stream cacheStream, bool useMS30PortTag)
        {
            if (useMS30PortTag != UseMS30)
            {
                UseMS30 = useMS30PortTag;
                Rmt2TagsInfo = new Dictionary<int, List<List<string>>>();
                GetRmt2Info(cacheStream);
            }
        }

        public void GetRmt2Info(Stream cacheStream)
        {
            if (Rmt2TagsInfo.Count != 0)
                return;

            if (!cacheStream.CanRead)
            {
                Console.Write("Waiting for cache stream to become available...");
                while (!cacheStream.CanRead) ;
                Console.Write("done.");
            }

            using (var reader = new EndianReader(cacheStream, true))
            {
                foreach (var instance in CacheContext.TagCache.TagTable)
                {
                    if (instance == null || !instance.IsInGroup("rmt2") || instance.Name == null || instance.Name.StartsWith("s3d"))
                        continue;

                    if (!UseMS30 && instance.Name.StartsWith("ms30"))
                        continue;

                    var bitmaps = new List<string>();
                    var arguments = new List<string>();

                    reader.SeekTo(instance.DefinitionOffset + instance.DefinitionOffset + 0x48);
                    var vectorArgsCount = reader.ReadInt32();
                    var vectorArgsAddress = reader.ReadUInt32();

                    if (vectorArgsCount != 0 && vectorArgsAddress != 0)
                    {
                        for (var i = 0; i < vectorArgsCount; i++)
                        {
                            reader.SeekTo(instance.DefinitionOffset + (vectorArgsAddress - 0x40000000) + (i * 0x4));
                            arguments.Add(CacheContext.StringIdCache.GetString(reader.ReadStringId()));
                        }
                    }

                    reader.SeekTo(instance.DefinitionOffset + instance.DefinitionOffset + 0x6C);
                    var samplerArgsCount = reader.ReadInt32();
                    var samplerArgsAddress = reader.ReadUInt32();

                    if (samplerArgsCount != 0 && samplerArgsAddress != 0)
                    {
                        for (var i = 0; i < samplerArgsCount; i++)
                        {
                            reader.SeekTo(instance.DefinitionOffset + (samplerArgsAddress - 0x40000000) + (i * 0x4));
                            bitmaps.Add(CacheContext.StringIdCache.GetString(reader.ReadStringId()));
                        }
                    }

                    Rmt2TagsInfo.Add(instance.Index, new List<List<string>> { bitmaps, arguments });
                }
            }
        }

        private List<ShaderTemplateItem> CollectRmt2Info(Stream cacheStream, CachedTag bmRmt2Instance, List<string> bmMaps, List<string> bmArgs)
        {
            var edRmt2BestStats = new List<ShaderTemplateItem>();

            RenderMethodTemplate bmRmt2;
            PixelShader bmPixl;

            using(var blamStream = BlamCache.TagCache.OpenTagCacheRead())
            {
                bmRmt2 = BlamCache.Deserialize<RenderMethodTemplate>(blamStream, bmRmt2Instance);
                bmPixl = BlamCache.Deserialize<PixelShader>(blamStream, bmRmt2.PixelShader);
            }
            

            // loop trough all rmt2 and find the closest
            foreach (var edRmt2_ in Rmt2TagsInfo)
            {
                var rmt2Type = bmRmt2Instance.Name.Split("\\".ToArray())[1];

                var edRmt2Tag = CacheContext.TagCache.GetTag(edRmt2_.Key);

                // Ignore all rmt2 that are not of the same type. 
                if (edRmt2Tag == null || !(edRmt2Tag.Name?.Contains(rmt2Type) ?? false))
                    continue;

                using (var reader = new EndianReader(cacheStream, true))
                {
                    reader.SeekTo(edRmt2Tag.DefinitionOffset + edRmt2Tag.DefinitionOffset + 28);
                    var edPixl = CacheContext.TagCache.GetTag(reader.ReadInt32());

                    if (edPixl == null)
                        continue;

                    reader.SeekTo(edPixl.DefinitionOffset + edPixl.DefinitionOffset + 0x4);
                    var drawModeCount = reader.ReadInt32();

                    reader.SeekTo(edPixl.DefinitionOffset + edPixl.DefinitionOffset + 0x14);
                    var shaderCount = reader.ReadInt32();

                    if (bmPixl.DrawModes.Count > drawModeCount || bmPixl.Shaders.Count > shaderCount)
                        continue;
                }

                int mapsCommon = 0;
                int argsCommon = 0;
                int mapsUncommon = 0;
                int argsUncommon = 0;
                int mapsMissing = 0;
                int argsMissing = 0;

                var edMaps_ = new List<string>();
                var edArgs_ = new List<string>();

                foreach (var a in edRmt2_.Value[0])
                    edMaps_.Add(a);

                foreach (var a in edRmt2_.Value[1])
                    edArgs_.Add(a);

                foreach (var a in bmMaps)
                    if (edMaps_.Contains(a))
                        mapsCommon++;

                foreach (var a in bmMaps)
                    if (!edMaps_.Contains(a))
                        mapsMissing++;

                foreach (var a in edMaps_)
                    if (!bmMaps.Contains(a))
                        mapsUncommon++;

                foreach (var a in bmArgs)
                    if (edArgs_.Contains(a))
                        argsCommon++;

                foreach (var a in bmArgs)
                    if (!edArgs_.Contains(a))
                        argsMissing++;

                foreach (var a in edArgs_)
                    if (!bmArgs.Contains(a))
                        argsUncommon++;

                edRmt2BestStats.Add(new ShaderTemplateItem
                {
                    rmt2TagIndex = edRmt2_.Key,
                    rmdfValuesMatchingCount = 0,
                    mapsCountEd = edRmt2_.Value[0].Count,
                    argsCountEd = edRmt2_.Value[1].Count,
                    mapsCountBm = bmMaps.Count,
                    argsCountBm = bmArgs.Count,
                    mapsCommon = mapsCommon,
                    argsCommon = argsCommon,
                    mapsUncommon = mapsUncommon,
                    argsUncommon = argsUncommon,
                    mapsMissing = mapsMissing,
                    argsMissing = argsMissing
                });
            }

            return edRmt2BestStats;
        }

        private List<string> SplitRenderMethodTemplateName(string rmt2Name, ref string type)
        {
            var split = rmt2Name.Split("\\".ToCharArray()).ToList();
            if (rmt2Name.StartsWith("ms30"))
                    split.RemoveAt(0);

            type = split[1];    // shader_template, halogram_template and so on
            var rmdfOptions = split.Last().Split("_".ToCharArray()).ToList();
            rmdfOptions.RemoveAt(0);    // remove empty option
            return rmdfOptions;
        }

        public CachedTag FindEquivalentRmt2(Stream cacheStream, CachedTag blamRmt2Tag, RenderMethodTemplate blamRmt2Definition, List<string> bmMaps, List<string> bmArgs)
        {
            // Find similar shaders by finding tags with as many common bitmaps and arguments as possible.
            var edRmt2Temp = new List<ShaderTemplateItem>();

            // Make a new dictionary with rmt2 of the same shader type
            var edRmt2BestStats = new List<ShaderTemplateItem>();

            edRmt2BestStats = CollectRmt2Info(cacheStream, blamRmt2Tag, bmMaps, bmArgs);

            // rmt2 tagnames have a bunch of values, they're tagblock indexes in rmdf methods.ShaderOptions
            foreach (var d in edRmt2BestStats)
            {
                var dInstance = CacheContext.TagCache.GetTag(d.rmt2TagIndex);

                if (dInstance == null || dInstance.Name == null)
                    continue;
                
                string edType = "";
                var edRmdfValues = SplitRenderMethodTemplateName(dInstance.Name, ref edType);

                string bmType = "";
                var bmRmdfValues = SplitRenderMethodTemplateName(blamRmt2Tag.Name, ref bmType);

                int matchingValues = 0;
                for (int i = 0; i < bmRmdfValues.Count; i++)
                {
                    var weight = 0;
                    if (bmRmdfValues[i] == edRmdfValues[i])
                    {
                        switch (i)
                        {
                            case 00:
                            case 06: weight = 1; break;
                        }
                        matchingValues = matchingValues + 1 + weight;
                    }
                }

                d.rmdfValuesMatchingCount = matchingValues;

                if (edType != bmType)
                    d.rmdfValuesMatchingCount = 0;
            }

            var edRmt2BestStatsSorted = edRmt2BestStats.OrderBy(x => x.rmdfValuesMatchingCount);

            if (edRmt2BestStats.Count == 0)
            {
                try
                {
                    return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_0_0_0_0_0_0_0_0_0_0");
                }
                catch (KeyNotFoundException)
                {
                    return null;
                }
            }

            return CacheContext.TagCache.GetTag(edRmt2BestStatsSorted.Last().rmt2TagIndex);
        }

        public RenderMethod.ShaderProperty.Argument DefaultArgumentsValues(string arg)
        {
            var res = new RenderMethod.ShaderProperty.Argument();
            var val = new float[4];
            switch (arg)
            {
                // Default argument values based on how frequently they appear in shaders, so I assumed it as an average argument value.
                case "transparence_normal_bias": val = new float[] { -1f, -1f, -1f, -1f }; break;
                case "warp_amount": val = new float[] { 0.005f, 0.005f, 0.005f, 0.005f }; break;
                case "area_specular_coefficient": val = new float[] { 0.01f, 0.01f, 0.01f, 0.01f }; break;
                case "sunspot_cut": val = new float[] { 0.01f, 0.01f, 0.01f, 0.01f }; break;
                case "antialias_tweak": val = new float[] { 0.025f, 0.025f, 0.025f, 0.025f }; break;
                case "height_scale": val = new float[] { 0.02f, 0.02f, 0.02f, 0.02f }; break;
                case "water_color_pure": val = new float[] { 0.03529412f, 0.1333333f, 0.1294118f, 1f }; break;
                case "analytical_specular_coefficient": val = new float[] { 0.03f, 0.03f, 0.03f, 0.03f }; break;
                case "animation_amplitude_horizontal": val = new float[] { 0.04f, 0.04f, 0.04f, 0.04f }; break;
                case "water_diffuse": val = new float[] { 0.05490196f, 0.08627451f, 0.09803922f, 1f }; break;
                case "displacement_range_y": val = new float[] { 0.07f, 0.07f, 0.07f, 0.07f }; break;
                case "refraction_texcoord_shift": val = new float[] { 0.12f, 0.12f, 0.12f, 0.12f }; break;
                case "meter_color_on": val = new float[] { 0.1333333f, 1f, 0.1686275f, 1f }; break;
                case "displacement_range_x": val = new float[] { 0.14f, 0.14f, 0.14f, 0.14f }; break;
                case "displacement_range_z": val = new float[] { 0.14f, 0.14f, 0.14f, 0.14f }; break;
                case "specular_tint_m_2": val = new float[] { 0.1764706f, 0.1372549f, 0.09411766f, 1f }; break;
                case "color_sharp": val = new float[] { 0.2156863f, 0.6745098f, 1f, 1f }; break;
                case "self_illum_heat_color": val = new float[] { 0.2392157f, 0.6470588f, 1f, 1f }; break;
                case "fresnel_coefficient": val = new float[] { 0.25f, 0.25f, 0.25f, 0.25f }; break;
                case "channel_b": val = new float[] { 0.2784314f, 0.04705883f, 0.04705883f, 2.411765f }; break;
                case "bankalpha_infuence_depth": val = new float[] { 0.27f, 0.27f, 0.27f, 0.27f }; break;
                case "roughness": val = new float[] { 0.27f, 0.27f, 0.27f, 0.27f }; break;
                case "rim_tint": val = new float[] { 0.3215686f, 0.3843138f, 0.5450981f, 1f }; break;
                case "chameleon_color1": val = new float[] { 0.3254902f, 0.2745098f, 0.8431373f, 1f }; break;
                case "chameleon_color_offset1": val = new float[] { 0.3333f, 0.3333f, 0.3333f, 0.3333f }; break;
                case "watercolor_coefficient": val = new float[] { 0.35f, 0.35f, 0.35f, 0.35f }; break;
                case "detail_slope_steepness": val = new float[] { 0.3f, 0.3f, 0.3f, 0.3f }; break;
                case "layer_depth": val = new float[] { 0.3f, 0.3f, 0.3f, 0.3f }; break;
                case "transparence_coefficient": val = new float[] { 0.3f, 0.3f, 0.3f, 0.3f }; break;
                case "wave_height_aux": val = new float[] { 0.3f, 0.3f, 0.3f, 0.3f }; break;
                case "environment_specular_contribution_m_2": val = new float[] { 0.4f, 0.4f, 0.4f, 0.4f }; break;
                case "rim_start": val = new float[] { 0.4f, 0.4f, 0.4f, 0.4f }; break;
                case "fresnel_color": val = new float[] { 0.5019608f, 0.5019608f, 0.5019608f, 1f }; break;
                case "fresnel_color_environment": val = new float[] { 0.5019608f, 0.5019608f, 0.5019608f, 1f }; break;
                case "channel_c": val = new float[] { 0.5490196f, 0.8588236f, 1f, 8f }; break;
                case "chameleon_color3": val = new float[] { 0.5529412f, 0.7137255f, 0.572549f, 1f }; break;
                case "chameleon_color0": val = new float[] { 0.627451f, 0.3098039f, 0.7803922f, 1f }; break;
                case "chameleon_color_offset2": val = new float[] { 0.6666f, 0.6666f, 0.6666f, 0.6666f }; break;
                case "subsurface_propagation_bias": val = new float[] { 0.66f, 0.66f, 0.66f, 0.66f }; break;
                case "add_color": val = new float[] { 0.6f, 0.6f, 0.6f, 0f }; break;
                case "wave_slope_array": val = new float[] { 0.7773f, 1.3237f, 0f, 0f }; break;
                case "detail_map_a": val = new float[] { 0.8140022f, 1.628004f, 43.13726f, 12.31073f }; break;
                case "slope_range_y": val = new float[] { 0.84f, 0.84f, 0.84f, 0.84f }; break;
                case "transparence_tint": val = new float[] { 0.8705883f, 0.8470589f, 0.6941177f, 1f }; break;
                case "channel_a": val = new float[] { 0.9254903f, 0.4862745f, 0.01960784f, 2.147059f }; break;
                case "subsurface_coefficient": val = new float[] { 0.9f, 0.9f, 0.9f, 0.9f }; break;
                case "color_medium": val = new float[] { 0f, 0f, 0f, 1f }; break;
                case "color_wide": val = new float[] { 0f, 0f, 0f, 1f }; break;
                case "edge_fade_edge_tint": val = new float[] { 0f, 0f, 0f, 1f }; break;
                case "meter_color_off": val = new float[] { 0f, 0f, 0f, 1f }; break;
                case "slope_range_x": val = new float[] { 1.39f, 1.39f, 1.39f, 1.39f }; break;
                case "wave_displacement_array": val = new float[] { 1.7779f, 1.7779f, 0f, 0f }; break;
                case "foam_texture_detail": val = new float[] { 1.97f, 1.377f, 1f, 0f }; break;
                case "vector_sharpness": val = new float[] { 1000f, 1000f, 1000f, 1000f }; break;
                case "detail_map_m_2": val = new float[] { 100f, 100f, 0f, 0f }; break;
                case "warp_map": val = new float[] { 8f, 5f, 0f, 0f }; break;
                case "water_murkiness": val = new float[] { 12f, 12f, 12f, 12f }; break;
                case "base_map_m_3": val = new float[] { 15f, 30f, 0f, 0f }; break;
                case "thinness_medium": val = new float[] { 16f, 16f, 16f, 16f }; break;
                case "chameleon_color2": val = new float[] { 1f, 1f, 0.5843138f, 1f }; break;
                case "specular_power": val = new float[] { 25f, 25f, 25f, 25f }; break;
                case "reflection_coefficient": val = new float[] { 30f, 30f, 30f, 30f }; break;
                case "refraction_extinct_distance": val = new float[] { 30f, 30f, 30f, 30f }; break;
                case "thinness_sharp": val = new float[] { 32f, 32f, 32f, 32f }; break;
                case "detail_multiplier_a": val = new float[] { 4.59479f, 4.59479f, 4.59479f, 4.59479f }; break;
                case "detail_map3": val = new float[] { 6.05f, 6.05f, 0.6f, 0f }; break;
                case "foam_texture": val = new float[] { 5f, 4f, 1f, 1f }; break;
                case "ambient_coefficient":
                case "area_specular_contribution_m_0":
                case "area_specular_contribution_m_2":
                case "area_specular_contribution_m_3":
                case "depth_fade_range":
                case "foam_height": val = new float[] { 0.1f, 0.1f, 0.1f, 0.1f }; break;
                case "area_specular_contribution_m_1":
                case "detail_fade_a":
                case "globalshape_infuence_depth":
                case "minimal_wave_disturbance": val = new float[] { 0.2f, 0.2f, 0.2f, 0.2f }; break;
                case "analytical_specular_contribution":
                case "analytical_specular_contribution_m_0":
                case "analytical_specular_contribution_m_1":
                case "analytical_specular_contribution_m_2":
                case "analytical_specular_contribution_m_3":
                case "rim_coefficient":
                case "shadow_intensity_mark":
                case "wave_height": val = new float[] { 0.5f, 0.5f, 0.5f, 0.5f }; break;
                case "glancing_specular_power":
                case "normal_specular_power":
                case "specular_power_m_0":
                case "specular_power_m_1":
                case "specular_power_m_2":
                case "specular_power_m_3": val = new float[] { 10f, 10f, 10f, 10f }; break;
                case "choppiness_forward":
                case "distortion_scale":
                case "foam_pow":
                case "global_shape":
                case "rim_fresnel_power":
                case "choppiness_backward":
                case "choppiness_side":
                case "detail_slope_scale_z":
                case "self_illum_intensity": val = new float[] { 3f, 3f, 3f, 3f }; break;
                case "layer_contrast":
                case "layers_of_4":
                case "thinness_wide": val = new float[] { 4f, 4f, 4f, 4f }; break;
                case "fresnel_curve_steepness":
                case "fresnel_curve_steepness_m_0":
                case "fresnel_curve_steepness_m_1":
                case "fresnel_curve_steepness_m_2":
                case "fresnel_curve_steepness_m_3": val = new float[] { 5f, 5f, 5f, 5f }; break;
                case "blend_mode":
                case "detail_slope_scale_x":
                case "detail_slope_scale_y":
                case "rim_power":
                case "wave_visual_damping_distance": val = new float[] { 8f, 8f, 8f, 8f }; break;
                case "bump_map_m_0":
                case "bump_map_m_2":
                case "bump_map_m_3": val = new float[] { 50f, 50f, 0f, 0f }; break;
                case "albedo":
                case "albedo_blend":
                case "albedo_blend_with_specular_tint":
                case "albedo_specular_tint_blend":
                case "albedo_specular_tint_blend_m_0":
                case "albedo_specular_tint_blend_m_1":
                case "albedo_specular_tint_blend_m_2":
                case "albedo_specular_tint_blend_m_3":
                case "analytical_anti_shadow_control":
                case "area_specular_contribution":
                case "environment_map_coefficient":
                case "environment_specular_contribution_m_0":
                case "environment_specular_contribution_m_1":
                case "environment_specular_contribution_m_3":
                case "fog":
                case "frame_blend":
                case "fresnel_curve_bias":
                case "invert_mask":
                case "lighting":
                case "meter_value":
                case "no_dynamic_lights":
                case "order3_area_specular":
                case "primary_change_color_blend":
                case "refraction_depth_dominant_ratio":
                case "rim_fresnel_albedo_blend":
                case "rim_fresnel_coefficient":
                case "rim_maps_transition_ratio":
                case "self_illumination":
                case "specialized_rendering":
                case "subsurface_normal_detail":
                case "time_warp":
                case "time_warp_aux":
                case "use_fresnel_color_environment":
                case "warp_amount_x":
                case "warp_amount_y":
                case "waveshape": val = new float[] { 0f, 0f, 0f, 0f }; break;
                case "alpha_map":
                case "alpha_mask_map":
                case "alpha_test_map":
                case "base_map":
                case "base_map_m_0":
                case "base_map_m_1":
                case "base_map_m_2":
                case "blend_map":
                case "bump_detail_map":
                case "bump_detail_mask_map":
                case "bump_map":
                case "bump_map_m_1":
                case "chameleon_mask_map":
                case "change_color_map":
                case "color_mask_map":
                case "detail_bump_m_0":
                case "detail_bump_m_1":
                case "detail_bump_m_2":
                case "detail_bump_m_3":
                case "detail_map":
                case "detail_map2":
                case "detail_map_m_0":
                case "detail_map_m_1":
                case "detail_map_m_3":
                case "detail_map_overlay":
                case "detail_mask_a":
                case "height_map":
                case "material_texture":
                case "meter_map":
                case "multiply_map":
                case "noise_map_a":
                case "noise_map_b":
                case "overlay_detail_map":
                case "overlay_map":
                case "overlay_multiply_map":
                case "self_illum_detail_map":
                case "self_illum_map":
                case "specular_mask_texture": val = new float[] { 1f, 1f, 0f, 0f }; break;
                case "albedo_color":
                case "albedo_color2":
                case "albedo_color3":
                case "ambient_tint":
                case "bump_detail_coefficient":
                case "chameleon_fresnel_power":
                case "depth_darken":
                case "diffuse_coefficient":
                case "diffuse_coefficient_m_0":
                case "diffuse_coefficient_m_1":
                case "diffuse_coefficient_m_2":
                case "diffuse_coefficient_m_3":
                case "diffuse_tint":
                case "edge_fade_center_tint":
                case "edge_fade_power":
                case "ending_uv_scale":
                case "env_roughness_scale":
                case "env_tint_color":
                case "environment_map_specular_contribution":
                case "environment_map_tint":
                case "final_tint":
                case "fresnel_power":
                case "glancing_specular_tint":
                case "global_albedo_tint":
                case "intensity":
                case "modulation_factor":
                case "neutral_gray":
                case "normal_specular_tint":
                case "overlay_intensity":
                case "overlay_tint":
                case "rim_fresnel_color":
                case "self_illum_color":
                case "specular_tint":
                case "specular_tint_m_0":
                case "specular_tint_m_1":
                case "specular_tint_m_3":
                case "starting_uv_scale":
                case "subsurface_tint":
                case "texcoord_aspect_ratio":
                case "tint_color":
                case "transparence_normal_detail":
                case "u_tiles":
                case "v_tiles": val = new float[] { 1f, 1f, 1f, 1f }; break;
                case "specular_coefficient":
                case "specular_coefficient_m_0":
                case "specular_coefficient_m_1":
                case "specular_coefficient_m_2":
                case "specular_coefficient_m_3": val = new float[] { 0, 0, 0, 0 }; break;

                default: val = new float[] { 0, 0, 0, 0 }; break;
            }

            res.Values = val;

            return res;
        }

        public string GetDefaultBitmapTag(string type)
        {
            switch (type)
            {
                case "change_color_map":
                case "material_texture":
                case "overlay_map":
                case "bump_detail_mask_map":
                case "chameleon_mask_map":
                case "specular_map":
                case "specular_mask_texture":
                case "blend_map":
                case "base_map":
                case "palette":
                case "occlusion_parameter_map":
                    return @"shaders\default_bitmaps\bitmaps\color_white";

                case "detail_map":
                case "detail_map2":
                case "detail_map3":
                case "detail_map_a":
                case "detail_map_m_0":
                case "detail_map_m_1":
                case "detail_map_m_2":
                case "detail_map_m_3":
                case "detail_map_overlay":
                    return @"shaders\default_bitmaps\bitmaps\default_detail";

                case "base_map_m_0":
                case "base_map_m_1":
                case "base_map_m_2":
                case "base_map_m_3":
                case "meter_map":
                case "overlay_multiply_map":
                case "subsurface_map":
                case "warp_map":
                    return @"shaders\default_bitmaps\bitmaps\gray_50_percent";

                case "bump_map":
                case "bump_map_m_0":
                case "bump_map_m_1":
                case "bump_map_m_2":
                case "bump_map_m_3":
                case "detail_bump_m_0":
                case "detail_bump_m_1":
                case "detail_bump_m_2":
                case "detail_bump_m_3":
                case "height_map":
                case "vector_map":
                    return @"shaders\default_bitmaps\bitmaps\default_vector";

                case "bump_detail_map":
                    return @"shaders\default_bitmaps\bitmaps\bump_detail";

                case "environment_map":
                    return @"shaders\default_bitmaps\bitmaps\default_dynamic_cube_map";

                case "color_mask_map":
                    return @"shaders\default_bitmaps\bitmaps\reference_grids";

                case "detail_mask_a":
                    return @"shaders\default_bitmaps\bitmaps\color_red";

                case "alpha_mask_map":
                    return @"shaders\default_bitmaps\bitmaps\alpha_white";

                case "overlay_detail_map":
                    return @"shaders\default_bitmaps\bitmaps\dither_pattern";

                case "self_illum_map":
                case "self_illum_detail_map":
                case "transparence_map":
                case "alpha_test_map":
                    return @"shaders\default_bitmaps\bitmaps\color_black_alpha_black";

                case "alpha_map":
                    return @"shaders\default_bitmaps\bitmaps\alpha_grey50";

                case "noise_map_a":
                    return @"shaders\default_bitmaps\bitmaps\clouds_a";

                case "noise_map_b":
                    return @"shaders\default_bitmaps\bitmaps\clouds_b";

                default:
                    Console.WriteLine($"WARNING: Shader map type \"{type}\" default bitmap not implemented.");
                    return @"shaders\default_bitmaps\bitmaps\gray_50_percent";
            }
        }

        public CachedTag FixRmt2Reference(Stream cacheStream, string blamTagName, CachedTag blamRmt2Tag, RenderMethodTemplate blamRmt2Definition, List<string> bmMaps, List<string> bmArgs)
        {
            switch (blamTagName)
            {
                case @"levels\multi\snowbound\shaders\cov_grey_icy":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"ms30\shaders\shader_templates\_0_2_0_1_7_2_0_0_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"objects\vehicles\ghost\shaders\ghost_damage":
                case @"objects\vehicles\wraith\shaders\wraith_blown_open":
                case @"objects\vehicles\banshee\shaders\banshee_damage":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_2_0_1_2_2_2_0_1_1_0");
                    }
                    catch { }
                    break;

                case @"objects\vehicles\ghost\shaders\ghost_torn":
                case @"objects\vehicles\banshee\shaders\banshee_torn":
                case @"objects\vehicles\wraith\shaders\wraith_torn":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_2_1_1_2_2_0_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"objects\vehicles\wraith\shaders\wraith_torn_metal":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_2_1_1_1_2_0_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"objects\vehicles\ghost\shaders\ghost_dash_zcam":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\halogram_templates\_0_11_0_1_0_2_0");
                    }
                    catch { }
                    break;

                case @"fx\particles\energy\electric_arcs_blue":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\particle_templates\_2_8_0_0_0_0_1_0_0_0");
                    }
                    catch { }
                    break;

                case @"objects\weapons\melee\energy_blade\fx\particles\plasma_wispy":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\particle_templates\_5_8_0_0_0_1_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"objects\weapons\melee\energy_blade\fx\particles\energy_pulse":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\particle_templates\_3_7_0_0_1_0_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"levels\dlc\fortress\shaders\panel_platform_center":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_1_0_1_2_2_5_0_1_0_0");
                    }
                    catch
                    { }
                    break;

                case @"levels\dlc\sidewinder\shaders\side_tree_branch_snow":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_1_1_0_0_2_5_0_0_0_0");
                    }
                    catch
                    { }
                    break;

                case @"levels\dlc\sidewinder\shaders\justin\sw_ground_ice1":
                case @"levels\dlc\sidewinder\shaders\justin\sw_ground_rock1":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\terrain_templates\_0_0_1_1_1_2");
                    }
                    catch
                    { }
                    break;

                case @"levels\multi\snowbound\sky\shaders\skydome":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_0_0_0_0_0_0_0_0_0_0");
                    }
                    catch
                    { }
                    break;

                case @"levels\solo\020_base\lights\light_volume_hatlight":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\halogram_templates\_0_5_1_0_0_1_1");
                    }
                    catch { }
                    break;

                case @"levels\dlc\armory\shaders\metal_doodad_a":
                case @"levels\dlc\armory\shaders\metal_doodad_a_illum_blue":
                case @"levels\dlc\armory\shaders\metal_doodad_a_illum_cool":
                case @"levels\dlc\armory\shaders\metal_doodad_a_illum_red":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_7_2_0_1_1_0_1_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"levels\dlc\armory\shaders\razor_wire":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_1_1_2_5_2_0_1_0_0_0");
                    }
                    catch { }
                    break;

                case @"levels\multi\deadlock\shaders\deadlock_concrete_wall_a_rubble":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_2_0_0_0_0_0_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"levels\multi\snowbound\shaders\rock_cliffs":
                case @"levels\multi\snowbound\shaders\rock_rocky":
                case @"levels\multi\snowbound\shaders\rock_rocky_icy":
                case @"levels\solo\020_base\shaders\hb_metal_arch_unwrap_a":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_7_2_0_1_2_1_0_0_1_0_0");
                    }
                    catch { }
                    break;

                case @"levels\dlc\armory\shaders\concrete_wall_01":
                case @"levels\dlc\armory\shaders\concrete_wall_01_blue":
                case @"levels\dlc\armory\shaders\concrete_wall_01_red":
                case @"levels\dlc\armory\shaders\concrete_wall_02_blue":
                case @"levels\dlc\armory\shaders\concrete_wall_02_red":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_7_2_0_1_7_0_0_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"objects\levels\solo\010_jungle\shaders\dam_fence":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_1_1_1_1_2_0_0_0_1_0");
                    }
                    catch { }
                    break;

                case @"levels\dlc\chillout\shaders\chillout_capsule_liquid":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\halogram_templates\_2_5_1_0_1_2_1");
                    }
                    catch { }
                    break;

                case @"levels\dlc\chillout\shaders\chillout_flood_godrays":
                case @"levels\dlc\chillout\shaders\chillout_invis_godrays":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\halogram_templates\_0_5_1_0_0_1_1");
                    }
                    catch { }
                    break;

                case @"objects\levels\dlc\chillout\shaders\chill_energy_blocker_small":
                case @"objects\levels\dlc\chillout\shaders\chill_viewing_area_blocker":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\halogram_templates\_0_8_1_0_0_4_1");
                    }
                    catch { }
                    break;

                case @"levels\dlc\chillout\shaders\chillout_floodroom01":
                case @"levels\dlc\chillout\shaders\chillout_floodroom02":
                case @"levels\dlc\chillout\shaders\chillout_floodroom03":
                case @"levels\dlc\chillout\shaders\chillout_transporter":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_2_0_1_1_2_5_0_0_1_0");
                    }
                    catch { }
                    break;

                case @"levels\dlc\chillout\shaders\chillout_flood_suckers":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_1_1_2_1_1_0_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"levels\shared\shaders\flood\flood_sackyb":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_1_0_0_1_0_0_3_1_1_0");
                    }
                    catch { }
                    break;

                case @"objects\characters\flood_infection\shaders\flood_fronds":
                case @"objects\characters\flood_tank\shaders\flood_fronds":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_1_1_0_0_2_5_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"objects\characters\flood_infection\shaders\flood_infection":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"ms30\shaders\shader_templates\_0_2_0_1_1_0_1_0_0_0_0_0");
                    }
                    catch { }
                    break;

                case @"objects\weapons\melee\energy_blade\shaders\energy_blade":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_2_0_1_1_0_3_3_1_1_0");
                    }
                    catch
                    { }
                    break;

                case @"objects\weapons\rifle\plasma_rifle_red\shaders\plasma_rifle_red":
                case @"objects\weapons\rifle\plasma_rifle\shaders\plasma_rifle":
                case @"objects\weapons\rifle\covenant_carbine\shaders\carbine":
                case @"objects\weapons\rifle\covenant_carbine\shaders\carbine_dull":
                case @"objects\weapons\pistol\plasma_pistol\shaders\plasma_pistol_metal":
                case @"objects\weapons\pistol\needler\shaders\needler_blue":
                case @"objects\weapons\pistol\needler\shaders\needler_pink":
                case @"objects\weapons\support_high\flak_cannon\shaders\flak_cannon":
                case @"objects\weapons\rifle\beam_rifle\shaders\beam_rifle":
                case @"objects\weapons\rifle\beam_rifle\shaders\beam_rifle2":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_2_0_1_2_1_1_0_0_0_0");
                    }
                    catch
                    { }
                    break;

                case @"objects\weapons\pistol\needler\shaders\needler_glass":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_0_0_0_0_0_1_0_3_0_0_0");
                    }
                    catch
                    { }
                    break;

                case @"objects\weapons\rifle\sniper_rifle\shaders\scope_alpha":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\halogram_templates\_0_11_0_1_0_2_0");
                    }
                    catch { }
                    break;

                //Vehicles
                case @"objects\vehicles\scorpion\turrets\cannon\shaders\turret_cannon":
                    try
                    {
                        return CacheContext.GetTag<RenderMethodTemplate>(@"shaders\shader_templates\_3_2_0_1_1_0_0_0_0_0_0");
                    }
                    catch { }
                    break;
            }

            // Find existing rmt2 tags
            // If tagnames are not fixed, ms30 tags have an additional _0 or _0_0. This shouldn't happen if the tags have proper names, so it's mostly to preserve compatibility with older tagnames
            using (var reader = new EndianReader(cacheStream, true))
            {
                foreach (var instance in CacheContext.TagCache.TagTable)
                {
                    if (instance == null || !instance.IsInGroup("rmt2") || instance.Name == null)
                        continue;

                    var rmt2Name = instance.Name;

                    // ignore s3d rmt2s
                    if (rmt2Name.StartsWith("s3d"))
                        continue;

                    // check if rmt2 is from ms30
                    if (rmt2Name.StartsWith("ms30"))
                    {
                        rmt2Name = rmt2Name.Replace("ms30\\", "");
                        // skip over ms30 rmt2s
                        if (!UseMS30)
                            continue;
                    }

                    //match found    
                    if (rmt2Name.StartsWith(blamRmt2Tag.Name))
                        return instance;

                }
            }

            // if no tagname matches, find rmt2 tags based on the most common values in the name
            return FindEquivalentRmt2(cacheStream, blamRmt2Tag, blamRmt2Definition, bmMaps, bmArgs);
        }

        public void FixRmdfTagRef(RenderMethod finalRm)
        {
            var rmdfName = BlamCache.TagCache.TagTable.ToList().Find(x => x.ID == finalRm.BaseRenderMethod.Index).Name;

            foreach (var instance in CacheContext.TagCache.TagTable)
            {
                if (instance == null || !instance.IsInGroup("rmdf") || instance.Name == null)
                    continue;

                if (instance.Name == rmdfName)
                {
                    finalRm.BaseRenderMethod = instance;
                    return;
                }
            }

            // Note: All ms23 rmdf tags need to exist. Using shader rmdf for all render methods is a bad idea
            finalRm.BaseRenderMethod = CacheContext.GetTag<RenderMethodDefinition>(@"shaders\shader");
            Console.WriteLine($"WARNING: Unable to locate `{rmdfName}.rmdf`; using `shaders\\shader.rmdf` instead.");
        }
    }
}