using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using System.IO;
using TagTool.Serialization;
using TagTool.Shaders.ShaderMatching;
using System;
using System.Linq;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public ShaderMatcher Matcher = new ShaderMatcher();

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

        private RenderMethod ConvertRenderMethod(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, RenderMethod finalRm, string blamTagName)
        {
            // Verify that the ShaderMatcher is ready to use
            if (!Matcher.IsInitialized())
                Matcher.Init(cacheStream, CacheContext, BlamCache);

            // Set flags
            Matcher.SetMS30Flag(cacheStream, FlagIsSet(PortingFlags.Ms30));

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

            // Get a simple list of bitmaps and arguments names
            var bmRmt2Instance = finalRm.ShaderProperties[0].Template;
            var bmRmt2 = BlamCache.Deserialize<RenderMethodTemplate>(blamCacheStream, bmRmt2Instance);

            // Get a simple list of H3 bitmaps and arguments names
            foreach (var a in bmRmt2.SamplerArguments)
                bmMaps.Add(BlamCache.StringTable.GetString(a.Name));
            foreach (var a in bmRmt2.VectorArguments)
                bmArgs.Add(BlamCache.StringTable.GetString(a.Name));

            // Find a HO equivalent rmt2
            var edRmt2Instance = Matcher.FixRmt2Reference(cacheStream, blamTagName, bmRmt2Instance, bmRmt2, bmMaps, bmArgs);

            if (edRmt2Instance == null)
            {
                throw new Exception($"Failed to find HO rmt2 for this RenderMethod instance");
            }
                

            var edRmt2Tagname = edRmt2Instance.Name ?? $"0x{edRmt2Instance.Index:X4}";

            // pRmsh pRmt2 now potentially have a new value
            if (pRmt2 != 0)
            {
                if (pRmt2 < CacheContext.TagCache.Count && pRmt2 >= 0)
                {
                    var a = CacheContext.TagCache.GetTag(pRmt2);
                    if (a != null)
                        edRmt2Instance = a;
                }
            }

            var edRmt2 = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, edRmt2Instance);

            // fixup for no use_material_texture vector arg in ms23
            for (int index = 0; index < edRmt2.BooleanArguments.Count; index++)
            {
                if (CacheContext.StringTable.GetString(edRmt2.BooleanArguments[index].Name) == "use_material_texture")
                {
                    finalRm.ShaderProperties[0].DisableBooleanArg = (ushort)(index + 1);
                    break;
                }
            }

            foreach (var a in edRmt2.SamplerArguments)
                edMaps.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.VectorArguments)
                edArgs.Add(CacheContext.StringTable.GetString(a.Name));

            // The bitmaps are default textures.
            // Arguments are probably default values. I took the values that appeared the most frequently, assuming they are the default value.
            foreach (var a in edMaps)
            {
                var newBitmap = Matcher.GetDefaultBitmapTag(a);

                if (pRmt2 >= CacheContext.TagCache.Count || pRmt2 < 0)
                    newBitmap = @"shaders\default_bitmaps\bitmaps\default_detail"; // would only happen for removed shaders

                CachedTag bitmap = null;

                try
                {
                    bitmap = CacheContext.GetTag<Bitmap>(newBitmap);
                }
                catch
                {
                    bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag($"{newBitmap}.bitm")[0]);
                }

                newShaderProperty.ShaderMaps.Add(
                    new RenderMethod.ShaderProperty.ShaderMap
                    {
                        Bitmap = bitmap
                    });
            }

            foreach (var a in edArgs)
                newShaderProperty.Arguments.Add(Matcher.DefaultArgumentsValues(a));

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

            Matcher.FixRmdfTagRef(finalRm);

            FixAnimationProperties(cacheStream, blamCacheStream, resourceStreams, BlamCache, CacheContext, finalRm, edRmt2, bmRmt2, blamTagName);

            // Fix any null bitmaps, caused by bitm port failure
            foreach (var a in finalRm.ShaderProperties[0].ShaderMaps)
            {
                if (a.Bitmap != null)
                    continue;

                var defaultBitmap = Matcher.GetDefaultBitmapTag(edMaps[finalRm.ShaderProperties[0].ShaderMaps.IndexOf(a)]);

                try
                {
                    a.Bitmap = CacheContext.GetTag<Bitmap>(defaultBitmap);
                }
                catch
                {
                    a.Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag($"{defaultBitmap}.bitm")[0]);
                }
            }

            if (Matcher.RmhgUnknownTemplates.Contains(edRmt2Instance.Name))
                if (finalRm.ShaderProperties[0].Unknown.Count == 0)
                    finalRm.ShaderProperties[0].Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>
                    {
                        new RenderMethod.ShaderProperty.UnknownBlock1
                        {
                            Unknown = 1
                        }
                    };

            switch (blamTagName)
            {
                case @"levels\dlc\chillout\shaders\chillout_flood_godrays" when finalRm is ShaderHalogram:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.SamplerArguments.Count; i++)
                        {
                            if (CacheContext.StringTable.GetString(edRmt2.SamplerArguments[i].Name) == "overlay_map")
                            {
                                finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\dlc\chillout\bitmaps\chillout_flood_godrays.bitmap")[0]);
                                break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.VectorArguments.Count; i++)
                        {
                            var templateArg = edRmt2.VectorArguments[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "overlay_map":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                    break;

                                case "overlay_tint":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.3764706f, 0.7254902f, 0.9215687f, 1f };
                                    break;

                                case "overlay_intensity":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1.25f, 1.25f, 1.25f, 1.25f };
                                    break;
                            }
                        }
                        break;
                    }

                case @"levels\dlc\chillout\shaders\chillout_invis_godrays" when finalRm is ShaderHalogram:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.SamplerArguments.Count; i++)
                        {
                            if (CacheContext.StringTable.GetString(edRmt2.SamplerArguments[i].Name) == "overlay_map")
                            {
                                finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\dlc\chillout\bitmaps\chillout_invis_godrays.bitmap")[0]);
                                break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.VectorArguments.Count; i++)
                        {
                            var templateArg = edRmt2.VectorArguments[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "overlay_map":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                    break;

                                case "overlay_tint":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.3058824f, 0.7098039f, 0.937255f, 1f };
                                    break;

                                case "overlay_intensity":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1f, 1f, 1f, 1f };
                                    break;
                            }
                        }
                        break;
                    }

                case @"levels\solo\020_base\lights\light_volume_hatlight" when finalRm is ShaderHalogram:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.SamplerArguments.Count; i++)
                        {
                            if (CacheContext.StringTable.GetString(edRmt2.SamplerArguments[i].Name) == "overlay_map")
                            {
                                finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\solo\020_base\bitmaps\light_volume_hatlight.bitmap")[0]);
                                break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.VectorArguments.Count; i++)
                        {
                            var templateArg = edRmt2.VectorArguments[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "overlay_map":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 2f, 1f, 0f, 0f };
                                    break;

                                case "overlay_tint":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.9960785f, 1f, 0.8039216f, 1f };
                                    break;

                                case "overlay_intensity":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1f, 1f, 1f, 1f };
                                    break;
                            }
                        }
                        break;
                    }

                case @"objects\vehicles\ghost\shaders\ghost_dash_zcam" when finalRm is ShaderHalogram:
                case @"objects\weapons\rifle\sniper_rifle\shaders\scope_alpha" when finalRm is ShaderHalogram:
                    finalRm.InputVariable = TagTool.Tags.TagMapping.VariableTypeValue.ParticleRandom1;
                    finalRm.RangeVariable = TagTool.Tags.TagMapping.VariableTypeValue.ParticleAge;
                    break;

                case @"levels\dlc\armory\shaders\concrete_floor_smooth" when finalRm is Shader:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.SamplerArguments.Count; i++)
                        {
                            if (CacheContext.StringTable.GetString(edRmt2.SamplerArguments[i].Name) == "bump_map")
                            {
                                finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\dlc\armory\bitmaps\concrete_floor_bump.bitmap")[0]);
                                break;
                            }
                        }
                        break;
                    }

                case @"levels\dlc\sidewinder\shaders\side_tree_branch_snow" when finalRm is Shader:
                    for (var i = 0; i < edRmt2.VectorArguments.Count; i++)
                    {
                        var templateArg = edRmt2.VectorArguments[i];

                        if (CacheContext.StringTable.GetString(templateArg.Name) == "env_tint_color")
                        {
                            finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0f, 0f, 0f, 0f };
                            break;
                        }
                    }
                    break;

                case @"levels\multi\isolation\sky\shaders\skydome" when finalRm is Shader:
                    for (var i = 0; i < edRmt2.VectorArguments.Count; i++)
                    {
                        var templateArg = edRmt2.VectorArguments[i];

                        if (CacheContext.StringTable.GetString(templateArg.Name) == "albedo_color")
                        {
                            finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.447059f, 0.376471f, 0.898039f, 1.0f };
                            break;
                        }
                    }
                    break;

                case @"levels\multi\snowbound\shaders\cov_grey_icy" when finalRm is Shader:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.SamplerArguments.Count; i++)
                        {
                            switch (CacheContext.StringTable.GetString(edRmt2.SamplerArguments[i].Name))
                            {
                                case "base_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\for_metal_greytech_dif.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "detail_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\for_metal_greytech_icy.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "bump_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\for_metal_greytech_platebump.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "bump_detail_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\for_metal_greytech_bump.bitmap")[0]);
                                    }
                                    catch { }
                                    break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.VectorArguments.Count; i++)
                        {
                            var templateArg = edRmt2.VectorArguments[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "base_map":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                    break;

                                case "detail_map":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1.5f, 1.5f, 0f, 0f };
                                    break;

                                case "albedo_color":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.554902f, 0.5588236f, 0.5921569f, 1f };
                                    break;

                                case "bump_map":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                    break;

                                case "bump_detail_map":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 2f, 2f, 0f, 0f };
                                    break;

                                case "diffuse_coefficient":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.4f, 0.4f, 0.4f, 0.4f };
                                    break;

                                case "specular_coefficient":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1f, 1f, 1f, 1f };
                                    break;

                                case "roughness":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.2f, 0.2f, 0.2f, 0.2f };
                                    break;

                                case "analytical_specular_contribution":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.2f, 0.2f, 0.2f, 0.2f };
                                    break;

                                case "area_specular_contribution":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.1f, 0.1f, 0.1f, 0.1f };
                                    break;

                                case "environment_map_specular_contribution":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.15f, 0.15f, 0.15f, 0.15f };
                                    break;

                                case "specular_tint":
                                    finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0.8431373f, 0.8470589f, 0.8117648f, 1f };
                                    break;
                            }
                        }
                        break;
                    }

                case @"levels\multi\snowbound\shaders\rock_cliffs" when finalRm is Shader:
                case @"levels\multi\snowbound\shaders\rock_rocky" when finalRm is Shader:
                case @"levels\multi\snowbound\shaders\rock_rocky_icy" when finalRm is Shader:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.SamplerArguments.Count; i++)
                        {
                            switch (CacheContext.StringTable.GetString(edRmt2.SamplerArguments[i].Name))
                            {
                                case "base_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_horiz.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "detail_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_granite_detail.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "detail_map2":
                                    try
                                    {
                                        switch (blamTagName)
                                        {
                                            case @"levels\multi\snowbound\shaders\rock_rocky_icy":
                                                finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_icy_blend.bitmap")[0]);
                                                break;

                                            default:
                                                finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_cliff_dif.bitmap")[0]);
                                                break;
                                        }
                                    }
                                    catch { }
                                    break;

                                case "bump_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_horiz_bump.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "bump_detail_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_granite_bump.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "height_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_horiz_parallax.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "environment_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"shaders\default_bitmaps\bitmaps\color_white.bitmap")[0]);
                                    }
                                    catch { }
                                    break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.VectorArguments.Count; i++)
                        {
                            var templateArg = edRmt2.VectorArguments[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "base_map":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_cliffs":
                                            finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 16f, 2f, 0f, 0.5f };
                                            break;
                                    }
                                    break;

                                case "detail_map":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_cliffs":
                                        case @"levels\multi\snowbound\shaders\rock_rocky":
                                            finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 320f, 20f, 0f, 0f };
                                            break;
                                    }
                                    break;

                                case "detail_map2":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_cliffs":
                                            finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                            break;
                                    }
                                    break;

                                case "bump_detail_map":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_cliffs":
                                        case @"levels\multi\snowbound\shaders\rock_rocky":
                                            finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 320f, 20f, 0f, 0f };
                                            break;
                                    }
                                    break;

                                case "diffuse_coefficient":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_rocky_icy":
                                            finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 1.2f, 1.2f, 1.2f, 1.2f };
                                            break;
                                    }
                                    break;
                            }
                        }
                        break;
                    }

                case @"levels\multi\snowbound\shaders\cov_metalplates_icy" when finalRm is Shader:
                    // Fixup bitmaps
                    for (var i = 0; i < edRmt2.SamplerArguments.Count; i++)
                    {
                        switch (CacheContext.StringTable.GetString(edRmt2.SamplerArguments[i].Name))
                        {
                            case "detail_map":
                                try
                                {
                                    finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = CacheContext.GetTag<Bitmap>(@"levels\multi\snowbound\bitmaps\for_metal_greytech_icy4");
                                }
                                catch { }
                                break;

                            case "detail_map2":
                                try
                                {
                                    finalRm.ShaderProperties[0].ShaderMaps[i].Bitmap = CacheContext.GetTag<Bitmap>(@"levels\multi\snowbound\bitmaps\for_metal_greytech_icy3");
                                }
                                catch { }
                                break;
                        }
                    }
                    break;

                case @"levels\multi\snowbound\shaders\invis_col_glass" when finalRm is Shader:
                    // Fixup arguments
                    for (var i = 0; i < edRmt2.VectorArguments.Count; i++)
                    {
                        var templateArg = edRmt2.VectorArguments[i];

                        switch (CacheContext.StringTable.GetString(templateArg.Name))
                        {
                            case "albedo_color":
                                finalRm.ShaderProperties[0].Arguments[i].Values = new float[] { 0f, 0f, 0f, 0f };
                                break;
                        }
                    }
                    break;
            }

            return finalRm;
        }

        private RenderMethod FixAnimationProperties(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, GameCache blamCache, GameCacheHaloOnline CacheContext, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, string blamTagName)
        {
            // finalRm is a H3 rendermethod with ported bitmaps, 
            if (finalRm.ShaderProperties[0].AnimationProperties.Count == 0)
                return finalRm;

            foreach (var properties in finalRm.ShaderProperties[0].AnimationProperties)
            {
                properties.InputName = ConvertStringId(properties.InputName);
                properties.RangeName = ConvertStringId(properties.RangeName);

                ConvertTagFunction(properties.Function);
            }

            var pixlTag = CacheContext.Deserialize(cacheStream, edRmt2.PixelShader);
            var edPixl = (PixelShader)pixlTag;
            var bmPixl = BlamCache.Deserialize<PixelShader>(blamCacheStream, bmRmt2.PixelShader);
            

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

                foreach (var b in bmPixl.Shaders[a.Offset].XboxParameters)
                {
                    var ParameterName = BlamCache.StringTable.GetString(b.ParameterName);

                    bmPixlParameters[DrawModeIndex].Add(new ArgumentMapping
                    {
                        ShaderIndex = a.Offset,
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

                // These are not modes. This is an indireciton table of packed 10_6 shorts
                // from RMT2 ShaderDrawmodes to RegisterOffsets
                // register_offset = ShaderDrawmodes[current_drawmode].Offset
                var drawmodeRegisterOffset = (int)a.Offset;
                var drawmodeRegisterCount = (int)a.Count;


                var ArgumentMappingsIndexSampler = (byte)finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleSampler;
                var ArgumentMappingsCountSampler = finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleSampler >> 8;
                var ArgumentMappingsIndexUnknown = (byte)finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleUnknown;
                var ArgumentMappingsCountUnknown = finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleUnknown >> 8;
                var ArgumentMappingsIndexVector = (byte)finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleVector;
                var ArgumentMappingsCountVector = finalRm.ShaderProperties[0].Unknown3[drawmodeRegisterOffset].DataHandleVector >> 8;
                var ArgumentMappings = new List<ArgumentMapping>();

                for (int j = 0; j < ArgumentMappingsCountSampler / 4; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexSampler + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].ArgumentMappings[ArgumentMappingsIndexSampler + j].ArgumentIndex, // i don't think i can use it to match stuf
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexSampler + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Sampler,
                        ShaderIndex = drawmodeRegisterOffset,
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
                        ShaderIndex = drawmodeRegisterOffset,
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
                        ShaderIndex = drawmodeRegisterOffset,
                    });
                }

                bmDrawmodesFunctions.Add(DrawModeIndex, new Unknown3Tagblock
                {
                    Unknown3Index = drawmodeRegisterOffset, // not shader index for rm and rmt2
                    Unknown3Count = drawmodeRegisterCount, // should always be 4 for enabled drawmodes
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
                            break;
                        }
                    }
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
                    foreach (var c in edPixl.Shaders[edPixl.DrawModes[a.Key].Offset].PCParameters)
                    {
                        var ParameterName = CacheContext.StringTable.GetString(c.ParameterName);

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
            // Gather all register indexes from pixl tag. Then check against all the converted register indexes. 
            // It should detect registers that are invalid and would crash, but it does not verify if the register is valid.
            var validEDRegisters = new List<int>();

            foreach (var a in edPixl.Shaders)
                foreach (var b in a.PCParameters)
                    if (!validEDRegisters.Contains(b.RegisterIndex))
                        validEDRegisters.Add(b.RegisterIndex);

            foreach (var a in finalRm.ShaderProperties[0].ArgumentMappings)
            {
                if (!validEDRegisters.Contains((a.RegisterIndex)))
                {
                    // Display a warning
                    // Console.WriteLine($"INVALID REGISTERS IN TAG {blamTagName}!");

                    // Abort, disable functions
                    finalRm.ShaderProperties[0].Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>(); // no idea what it does, but it crashes sometimes if it's null. on Shrine, it's the shader loop effect
                    finalRm.ShaderProperties[0].AnimationProperties = new List<RenderMethod.AnimationPropertiesBlock>();
                    finalRm.ShaderProperties[0].ArgumentMappings = new List<RenderMethod.ShaderProperty.ArgumentMapping>();
                    finalRm.ShaderProperties[0].Unknown3 = new List<RenderMethod.ShaderProperty.UnknownBlock3>();
                    foreach (var b in edRmt2.RegisterOffsets) // stops crashing for some shaders if the drawmodes count is still the same
                        finalRm.ShaderProperties[0].Unknown3.Add(new RenderMethod.ShaderProperty.UnknownBlock3());

                    return finalRm;
                }
            }

            return finalRm;
        }
    }
}