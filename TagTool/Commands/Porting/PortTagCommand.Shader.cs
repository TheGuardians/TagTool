using TagTool.Cache;
using TagTool.Common;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        private RasterizerGlobals ConvertRasterizerGlobals(RasterizerGlobals rasg)
        {
            if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                rasg.Unknown6HO = 6;
            }
            else
            {
                rasg.Unknown6HO = rasg.Unknown6;
            }
            return rasg;
        }

        private static List<string> RmhgUnknownTemplates { get; } = new List<string>
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
        /// A dictionary of all ElDorado RenderMethodTemplates and lists of their bitmaps and arguments names.
        /// </summary>
        public static Dictionary<int, List<List<string>>> Rmt2TagsInfo { get; set; } = new Dictionary<int, List<List<string>>>();

        private RenderMethod ConvertRenderMethod(Stream cacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, RenderMethod finalRm, string blamTagName)
        {
            // finalRm.ShaderProperties[0].ShaderMaps are all ported bitmaps
            // finalRm.BaseRenderMethod is a H3 tag
            // finalRm.ShaderProperties[0].Template is a H3 tag

            // TODO hardcode shader values such as argument changes for specific shaders
            var bmMaps = new List<string>();
            var bmArgs = new List<string>();
            var edMaps = new List<string>();
            var edArgs = new List<string>();

            // Reset rmt2 preset
            var pRmt2 = 0;

            // Make a template of ShaderProperty, with the correct bitmaps and arguments counts. 
            var newShaderProperty = new RenderMethod.ShaderProperty
            {
                ShaderMaps = new List<RenderMethod.ShaderProperty.ShaderMap>(),
                Arguments = new List<RenderMethod.ShaderProperty.Argument>()
            };

            // Loop only once trough all ED rmt2 tags and store them globally, string lists of their bitmaps and arguments
            if (Rmt2TagsInfo.Count == 0)
                GetRmt2Info(cacheStream, CacheContext);

            // Get a simple list of bitmaps and arguments names
            var bmRmt2Instance = BlamCache.IndexItems.Find(x => x.ID == finalRm.ShaderProperties[0].Template.Index);
            var blamContext = new CacheSerializationContext(ref BlamCache, bmRmt2Instance);
            var bmRmt2 = BlamCache.Deserializer.Deserialize<RenderMethodTemplate>(blamContext);

            // Get a simple list of H3 bitmaps and arguments names
            foreach (var a in bmRmt2.ShaderMaps)
                bmMaps.Add(BlamCache.Strings.GetItemByID(a.Name.Index));
            foreach (var a in bmRmt2.Arguments)
                bmArgs.Add(BlamCache.Strings.GetItemByID(a.Name.Index));

            // Find a HO equivalent rmt2
            var edRmt2Instance = FixRmt2Reference(CacheContext, bmRmt2Instance, bmRmt2, bmMaps, bmArgs);

            if (edRmt2Instance == null)
            {
                var shaderInvalid = CacheContext.GetTag<Shader>(@"shaders\invalid");

                var edContext2 = new TagSerializationContext(cacheStream, CacheContext, shaderInvalid);
                return CacheContext.Deserializer.Deserialize<Shader>(edContext2);
            }

            var edRmt2Tagname = CacheContext.TagNames.ContainsKey(edRmt2Instance.Index) ? CacheContext.TagNames[edRmt2Instance.Index] : $"0x{edRmt2Instance.Index:X4}";

            // pRmsh pRmt2 now potentially have a new value
            if (pRmt2 != 0)
            {
                if (CacheContext.TagCache.Index.Contains(pRmt2))
                {
                    var a = CacheContext.GetTag(pRmt2);
                    if (a != null)
                        edRmt2Instance = a;
                }
            }

            var edContext = new TagSerializationContext(cacheStream, CacheContext, edRmt2Instance);
            var edRmt2 = CacheContext.Deserializer.Deserialize<RenderMethodTemplate>(edContext);

            foreach (var a in edRmt2.ShaderMaps)
                edMaps.Add(CacheContext.StringIdCache.GetString(a.Name));
            foreach (var a in edRmt2.Arguments)
                edArgs.Add(CacheContext.StringIdCache.GetString(a.Name));

            // The bitmaps are default textures.
            // Arguments are probably default values. I took the values that appeared the most frequently, assuming they are the default value.
            foreach (var a in edMaps)
            {
                var newBitmap = GetDefaultBitmapTag(a);
                if (!CacheContext.TagCache.Index.Contains(pRmt2))
                    newBitmap = @"shaders\default_bitmaps\bitmaps\default_detail"; // would only happen for removed shaders

                CachedTagInstance bitmap = null;

                try
                {
                    bitmap = CacheContext.GetTag<Bitmap>(newBitmap);
                }
                catch (KeyNotFoundException)
                {
                    bitmap = ConvertTag(cacheStream, resourceStreams, ParseLegacyTag($"{newBitmap}.bitm")[0]);
                }

                newShaderProperty.ShaderMaps.Add(
                    new RenderMethod.ShaderProperty.ShaderMap
                    {
                        Bitmap = bitmap
                    });
            }

            foreach (var a in edArgs)
                newShaderProperty.Arguments.Add(DefaultArgumentsValues(a));

            // Reorder blam bitmaps to match the HO rmt2 order
            // Reorder blam arguments to match the HO rmt2 order
            foreach (var eM in edMaps)
                foreach (var bM in bmMaps)
                    if (eM == bM)
                        newShaderProperty.ShaderMaps[edMaps.IndexOf(eM)] = finalRm.ShaderProperties[0].ShaderMaps[bmMaps.IndexOf(bM)];

            foreach (var eA in edArgs)
                foreach (var bA in bmArgs)
                    if (eA == bA)
                        newShaderProperty.Arguments[edArgs.IndexOf(eA)] = finalRm.ShaderProperties[0].Arguments[bmArgs.IndexOf(bA)];

            // Remove some tagblocks
            // finalRm.Unknown = new List<RenderMethod.UnknownBlock>(); // hopefully not used; this gives rmt2's name. They correspond to the first tagblocks in rmdf, they tell what the shader does
            finalRm.ImportData = new List<RenderMethod.ImportDatum>(); // most likely not used
            finalRm.ShaderProperties[0].Template = edRmt2Instance;
            finalRm.ShaderProperties[0].ShaderMaps = newShaderProperty.ShaderMaps;
            finalRm.ShaderProperties[0].Arguments = newShaderProperty.Arguments;

            FixRmdfTagRef(finalRm);

            FixFunctions(BlamCache, CacheContext, cacheStream, finalRm, edRmt2, bmRmt2);

            // Fix any null bitmaps, caused by bitm port failure
            foreach (var a in finalRm.ShaderProperties[0].ShaderMaps)
                if (a.Bitmap == null)
                    a.Bitmap = CacheContext.GetTag<Bitmap>(GetDefaultBitmapTag(edMaps[(int)finalRm.ShaderProperties[0].ShaderMaps.IndexOf(a)]));

            if (CacheContext.TagNames.ContainsKey(edRmt2Instance.Index) && RmhgUnknownTemplates.Contains(CacheContext.TagNames[edRmt2Instance.Index]))
                if (finalRm.ShaderProperties[0].Unknown.Count == 0)
                    finalRm.ShaderProperties[0].Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>
                    {
                        new RenderMethod.ShaderProperty.UnknownBlock1
                        {
                            Unknown = 1
                        }
                    };

            return finalRm;
        }

        private static void GetRmt2Info(Stream cacheStream, HaloOnlineCacheContext cacheContext)
        {
            if (Rmt2TagsInfo.Count != 0)
                return;

            foreach (var instance in cacheContext.TagCache.Index)
            {
                if (instance == null || !instance.IsInGroup("rmt2") || !cacheContext.TagNames.ContainsKey(instance.Index))
                    continue;

                var tagContext = new TagSerializationContext(cacheStream, cacheContext, instance);
                var template = cacheContext.Deserializer.Deserialize<RenderMethodTemplateFast>(tagContext);

                var bitmaps = new List<string>();
                var arguments = new List<string>();

                foreach (var shaderMap in template.ShaderMaps)
                    bitmaps.Add(cacheContext.StringIdCache.GetString(shaderMap.Name));

                foreach (var argument in template.Arguments)
                    arguments.Add(cacheContext.StringIdCache.GetString(argument.Name));

                Rmt2TagsInfo.Add(instance.Index, new List<List<string>> { bitmaps, arguments });
            }
        }

        private List<ShaderTemplateItem> CollectRmt2Info(CacheFile.IndexItem bmRmt2Instance, List<string> bmMaps, List<string> bmArgs)
        {
            var edRmt2BestStats = new List<ShaderTemplateItem>();

            // loop trough all rmt2 and find the closest
            foreach (var edRmt2_ in Rmt2TagsInfo)
            {
                var rmt2Type = bmRmt2Instance.Name.Split("\\".ToArray())[1];

                // Ignore all rmt2 that are not of the same type. 
                if (!CacheContext.TagNames[edRmt2_.Key].Contains(rmt2Type))
                    continue;

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

        private CachedTagInstance FindEquivalentRmt2(CacheFile.IndexItem blamRmt2Tag, RenderMethodTemplate blamRmt2Definition, List<string> bmMaps, List<string> bmArgs)
        {
            // Find similar shaders by finding tags with as many common bitmaps and arguments as possible.
            var edRmt2Temp = new List<ShaderTemplateItem>();

            // Make a new dictionary with rmt2 of the same shader type
            var edRmt2BestStats = new List<ShaderTemplateItem>();

            edRmt2BestStats = CollectRmt2Info(blamRmt2Tag, bmMaps, bmArgs);

            // rmt2 tagnames have a bunch of values, they're tagblock indexes in rmdf methods.ShaderOptions
            foreach (var d in edRmt2BestStats)
            {
                var edSplit = CacheContext.TagNames[d.rmt2TagIndex].Split("\\".ToCharArray());
                var edType = edSplit[1];
                var edRmdfValues = edSplit[2].Split("_".ToCharArray()).ToList();
                edRmdfValues.RemoveAt(0);

                var bmSplit = blamRmt2Tag.Name.Split("\\".ToCharArray());
                var bmType = bmSplit[1];
                var bmRmdfValues = bmSplit[2].Split("_".ToCharArray()).ToList();
                bmRmdfValues.RemoveAt(0);

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

            return CacheContext.GetTag(edRmt2BestStatsSorted.Last().rmt2TagIndex);
        }
        
        private class ShaderTemplateItem
        {
            public int rmt2TagIndex;
            public int rmdfValuesMatchingCount = 0;
            public int mapsCountEd;
            public int argsCountEd;
            public int mapsCountBm;
            public int argsCountBm;
            public int mapsCommon;
            public int argsCommon;
            public int mapsUncommon;
            public int argsUncommon;
            public int mapsMissing;
            public int argsMissing;
        }

        [TagStructure(Name = "render_method_template", Tag = "rmt2")]
        private class RenderMethodTemplateFast // used to deserialize as fast as possible
        {
            [TagField(Length = 18)]
            public uint[] Unknown00 = new uint[18];

            public List<Argument> Arguments = new List<Argument>();

            [TagField(Length = 6)]
            public uint[] Unknown02 = new uint[6];

            public List<ShaderMap> ShaderMaps = new List<ShaderMap>();

            [TagStructure]
            public class Argument
            {
                public StringId Name = StringId.Invalid;
            }

            [TagStructure]
            public class ShaderMap
            {
                public StringId Name = StringId.Invalid;
            }
        }

        [TagStructure(Name = "render_method", Tag = "rm  ", Size = 0x20)]
        public class RenderMethodFast
        {
            public CachedTagInstance BaseRenderMethod;
            public List<RenderMethod.UnknownBlock> Unknown;
        }

        private static RenderMethod.ShaderProperty.Argument DefaultArgumentsValues(string arg)
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
                case "specular_coefficient_m_3":val = new float[] { 0, 0, 0, 0 }; break;

                default: val = new float[] { 0, 0, 0, 0 }; break;
            }

            res.Values = val;

            return res;
        }

        private static string GetDefaultBitmapTag(string type)
        {
            switch (type)
            {
                case "base_map":
                case "palette":
                case "self_illum_map":
                case "occlusion_parameter_map":
                case "change_color_map":
                case "noise_map_a":
                case "noise_map_b":
                case "overlay_map":
                case "bump_detail_mask_map":
                case "chameleon_mask_map":
                case "specular_map":
                case "specular_mask_texture":
                case "transparence_map":
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
                case "self_illum_detail_map":
                    return @"shaders\default_bitmaps\bitmaps\default_detail";

                case "base_map_m_0":
                case "base_map_m_1":
                case "base_map_m_2":
                case "base_map_m_3":
                case "blend_map":
                case "material_texture":
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

                case "alpha_test_map":
                    return @"shaders\default_bitmaps\bitmaps\color_black_alpha_black";

                case "alpha_map":
                    return @"shaders\default_bitmaps\bitmaps\alpha_grey50";

                default:
                    Console.WriteLine($"WARNING: Shader map type \"{type}\" default bitmap not implemented.");
                    return @"shaders\default_bitmaps\bitmaps\gray_50_percent";
            }
        }
        
        private CachedTagInstance FixRmt2Reference(HaloOnlineCacheContext CacheContext, CacheFile.IndexItem blamRmt2Tag, RenderMethodTemplate blamRmt2Definition, List<string> bmMaps, List<string> bmArgs)
        {
            // Find existing rmt2 tags
            // If tagnames are not fixed, ms30 tags have an additional _0 or _0_0. This shouldn't happen if the tags have proper names, so it's mostly to preserve compatibility with older tagnames
            foreach (var instance in CacheContext.TagCache.Index)
            {
                if (instance == null || !instance.IsInGroup("rmt2") || !CacheContext.TagNames.ContainsKey(instance.Index))
                    continue;

                if (CacheContext.TagNames[instance.Index].StartsWith(blamRmt2Tag.Name))
                    return instance;
            }

			// if no tagname matches, find rmt2 tags based on the most common values in the name
			return FindEquivalentRmt2(blamRmt2Tag, blamRmt2Definition, bmMaps, bmArgs);
		}
        
        private class Unknown3Tagblock
        {
            public int Unknown3Index;
            public int Unknown3Count;
            public int ArgumentMappingsIndexVector;
            public int ArgumentMappingsCountVector;
            public int ArgumentMappingsIndexUnknown;
            public int ArgumentMappingsCountUnknown;
            public int ArgumentMappingsIndexSampler;
            public int ArgumentMappingsCountSampler;
            public List<ArgumentMapping> ArgumentMappings;
        }

        private class ArgumentMapping
        {
            public string ParameterName;
            public int ShaderIndex = -1;
            public int RegisterIndex;
            public int EDRegisterIndex = -1;
            public int ArgumentIndex;
            public int ArgumentMappingsTagblockIndex;
            public TagTool.Shaders.ShaderParameter.RType RegisterType;
        }

		private void FixRmdfTagRef(RenderMethod finalRm)
		{
			// Set rmdf
			var rmdfName = BlamCache.IndexItems.Find(x => x.ID == finalRm.BaseRenderMethod.Index).Name;
			if (CacheContext.TagNames.ContainsValue(rmdfName))
				finalRm.BaseRenderMethod = CacheContext.GetTag<RenderMethodDefinition>(rmdfName);
			else
			{
				// all ms23 rmdf tags need to exist, using rmsh's rmdf for all rm's is a bad idea
				finalRm.BaseRenderMethod = CacheContext.GetTag<RenderMethodDefinition>(@"shaders\shader");
				Console.WriteLine($"WARNING: Unable to locate `{rmdfName}.rmdf`; using `shaders\\shader.rmdf` instead.");
			}
		}

		private RenderMethod FixFunctions(CacheFile blamCache, HaloOnlineCacheContext CacheContext, Stream cacheStream, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2)
        {
            // finalRm is a H3 rendermethod with ported bitmaps, 
            if (finalRm.ShaderProperties[0].Functions.Count == 0)
                return finalRm;

            foreach (var a in finalRm.ShaderProperties[0].Functions)
            {
                a.Name = ConvertStringId(a.Name);
                ConvertTagFunction(a.Function);
            }    

            var pixlTag = CacheContext.Deserializer.Deserialize(new TagSerializationContext(cacheStream, CacheContext, edRmt2.PixelShader), TagDefinition.Find(edRmt2.PixelShader.Group.Tag));
            var edPixl = (PixelShader)pixlTag;

            var blamContext = new CacheSerializationContext(ref BlamCache, blamCache.IndexItems.Find(x => x.ID == bmRmt2.PixelShader.Index));
            var bmPixl = BlamCache.Deserializer.Deserialize<PixelShader>(blamContext);

            // Make a collection of drawmodes and their DrawModeItem's
            // DrawModeItem are has info about all registers modified by functions for each drawmode.

            var bmPixlParameters = new Dictionary<int, List<ArgumentMapping>>(); // key is shader index

            // pixl side
            // For each drawmode, find its shader, and get all that shader's parameter.
            // Each parameter has a registerIndex, a registerType, and a registerName.
            // We'll use this to know which function acts on what shader and which registers

            var RegistersList = new Dictionary<int, string>();

            foreach (var a in finalRm.ShaderProperties[0].ArgumentMappings)
                if (!RegistersList.ContainsKey(a.RegisterIndex))
                    RegistersList.Add(a.RegisterIndex, "");

            var DrawModeIndex = -1;
            foreach (var a in bmPixl.DrawModes)
            {
                DrawModeIndex++;

                bmPixlParameters.Add(DrawModeIndex, new List<ArgumentMapping>());

                if (a.Count == 0)
                    continue;

                foreach (var b in bmPixl.Shaders[a.Index].XboxParameters)
                {
                    var ParameterName = BlamCache.Strings.GetItemByID(b.ParameterName.Index);

                    bmPixlParameters[DrawModeIndex].Add(new ArgumentMapping
                    {
                        ShaderIndex = a.Index,
                        ParameterName = ParameterName,
                        RegisterIndex = b.RegisterIndex,
                        RegisterType = b.RegisterType
                    });
                }
            }

            // rm side
            var bmDrawmodesFunctions = new Dictionary<int, Unknown3Tagblock>(); // key is shader index
            DrawModeIndex = -1;
            foreach (var a in finalRm.ShaderProperties[0].DrawModes)
            {
                DrawModeIndex++;
                var Unknown3Index = (byte)a.DataHandle;
                var Unknown3Count = a.DataHandle >> 8;

                var ArgumentMappingsIndexSampler = (byte)finalRm.ShaderProperties[0].Unknown3[Unknown3Index].DataHandleSampler;
                var ArgumentMappingsCountSampler = finalRm.ShaderProperties[0].Unknown3[Unknown3Index].DataHandleSampler >> 8;
                var ArgumentMappingsIndexUnknown = (byte)finalRm.ShaderProperties[0].Unknown3[Unknown3Index].DataHandleUnknown;
                var ArgumentMappingsCountUnknown = finalRm.ShaderProperties[0].Unknown3[Unknown3Index].DataHandleUnknown >> 8;
                var ArgumentMappingsIndexVector = (byte)finalRm.ShaderProperties[0].Unknown3[Unknown3Index].DataHandleVector;
                var ArgumentMappingsCountVector = finalRm.ShaderProperties[0].Unknown3[Unknown3Index].DataHandleVector >> 8;
                var ArgumentMappings = new List<ArgumentMapping>();

                for (int j = 0; j < ArgumentMappingsCountSampler / 4; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexSampler + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexSampler + j].ArgumentIndex, // i don't think i can use it to match stuf
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexSampler + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Sampler,
                        ShaderIndex = Unknown3Index,
                        // WARNING i think drawmodes in rm aren't the same as in pixl, because rm drawmodes can point to a global shader .
                        // say rm.drawmodes[17]'s value is 13, pixl.drawmodes[17] would typically be 12
                    });
                }

                for (int j = 0; j < ArgumentMappingsCountUnknown / 4; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexUnknown + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexUnknown + j].ArgumentIndex,
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexUnknown + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Vector,
                        ShaderIndex = Unknown3Index,
                        // it's something else, uses a global shader or some shit, one water shader pointed to a vtsh in rasg, but not in H3, maybe coincidence
                        // yeah guaranteed rmdf's glvs or rasg shaders
                    });
                }

                for (int j = 0; j < ArgumentMappingsCountVector / 4; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexVector + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexVector + j].ArgumentIndex,
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexVector + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Vector,
                        ShaderIndex = Unknown3Index,
                    });
                }

                bmDrawmodesFunctions.Add(DrawModeIndex, new Unknown3Tagblock
                {
                    Unknown3Index = Unknown3Index, // not shader index for rm and rmt2
                    Unknown3Count = Unknown3Count, // should always be 4 for enabled drawmodes
                    ArgumentMappingsIndexSampler = ArgumentMappingsIndexSampler,
                    ArgumentMappingsCountSampler = ArgumentMappingsCountSampler,
                    ArgumentMappingsIndexUnknown = ArgumentMappingsIndexUnknown, // no clue what it's used for, global shaders? i know one of the drawmodes will use one or more shaders from glvs, no idea if always or based on something
                    ArgumentMappingsCountUnknown = ArgumentMappingsCountUnknown,
                    ArgumentMappingsIndexVector = ArgumentMappingsIndexVector,
                    ArgumentMappingsCountVector = ArgumentMappingsCountVector,
                    ArgumentMappings = ArgumentMappings
                });
            }

            DrawModeIndex = -1;
            foreach (var a in bmDrawmodesFunctions)
            {
                DrawModeIndex++;
                if (a.Value.Unknown3Count == 0)
                    continue;

                foreach (var b in a.Value.ArgumentMappings)
                {
                    foreach (var c in bmPixlParameters[a.Key])
                    {
                        if (b.RegisterIndex == c.RegisterIndex && b.RegisterType == c.RegisterType)
                        {
                            b.ParameterName = c.ParameterName;
                            goto opp;
                        }
                    }
                    opp:
                    ;
                }
            }

            // // Now that we know which register is what for each drawmode, find its halo online equivalent register indexes based on register name.
            // // This is where it gets tricky because drawmodes count changed in HO. 
            foreach (var a in bmDrawmodesFunctions)
            {
                if (a.Value.Unknown3Count == 0)
                    continue;

                foreach (var b in a.Value.ArgumentMappings)
                {
                    foreach (var c in edPixl.Shaders[edPixl.DrawModes[a.Key].Index].PCParameters)
                    {
                        var ParameterName = CacheContext.StringIdCache.GetString(c.ParameterName);

                        if (ParameterName == b.ParameterName && b.RegisterType == c.RegisterType)
                        {
                            if (RegistersList[b.RegisterIndex] == "")
                                RegistersList[b.RegisterIndex] = $"{c.RegisterIndex}";
                            else
                                RegistersList[b.RegisterIndex] = $"{RegistersList[b.RegisterIndex]},{c.RegisterIndex}";

                            b.EDRegisterIndex = c.RegisterIndex;
                        }
                    }
                }
            }

            // DEBUG draw registers
            // DEBUG check for invalid registers
            foreach (var a in bmDrawmodesFunctions)
            {
                if (a.Value.Unknown3Count == 0)
                    continue;

                foreach (var b in a.Value.ArgumentMappings)
                    finalRm.ShaderProperties[0].ArgumentMappings[b.ArgumentMappingsTagblockIndex].RegisterIndex = (short)b.EDRegisterIndex;
            }

            // one final check
            // nope nopenopenepe this needs to be verified against it's pixl tag, not global registers
            var validEDRegisters = new List<int> { 000, 001, 002, 003, 004, 005, 006, 007, 008, 009, 010, 011, 012, 013, 014, 015, 016, 017, 018, 023, 024, 025, 026, 027, 028, 030, 031, 032, 033, 034, 035, 036, 037, 038, 039, 040, 041, 042, 043, 044, 045, 046, 047, 048, 049, 050, 051, 053, 057, 058, 059, 060, 061, 062, 063, 064, 065, 066, 067, 068, 069, 070, 071, 072, 073, 074, 075, 076, 077, 078, 079, 080, 081, 082, 083, 084, 085, 086, 087, 088, 089, 090, 091, 092, 093, 094, 095, 096, 097, 099, 100, 102, 103, 104, 105, 106, 107, 108, 109, 114, 120, 121, 122, 164, 168, 172, 173, 174, 175, 190, 191, 200, 201, 203, 204, 210, 211, 212, 213, 215, 216, 217, 218, 219, 220, 221, 222 };
            foreach (var a in finalRm.ShaderProperties[0].ArgumentMappings)
            {
                if (!validEDRegisters.Contains((a.RegisterIndex)))
                {
                    // Abort, disable functions
                    finalRm.ShaderProperties[0].Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>(); // no idea what it does, but it crashes sometimes if it's null. on Shrine, it's the shader loop effect
                    finalRm.ShaderProperties[0].Functions = new List<RenderMethod.ShaderProperty.FunctionBlock>();
                    finalRm.ShaderProperties[0].ArgumentMappings = new List<RenderMethod.ShaderProperty.ArgumentMapping>();
                    finalRm.ShaderProperties[0].Unknown3 = new List<RenderMethod.ShaderProperty.UnknownBlock3>();
                    foreach (var b in edRmt2.DrawModeRegisterOffsets) // stops crashing for some shaders if the drawmodes count is still the same
                        finalRm.ShaderProperties[0].Unknown3.Add(new RenderMethod.ShaderProperty.UnknownBlock3());

                    return finalRm;
                }
            }

            return finalRm;
        }
    }
}