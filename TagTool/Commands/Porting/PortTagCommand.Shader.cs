using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using System.IO;
using TagTool.Serialization;
using TagTool.Shaders.ShaderMatching;
using System;
using System.Linq;
using static TagTool.Tags.Definitions.RenderMethod;
using static TagTool.Tags.Definitions.RenderMethod.ShaderProperty;

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
                TextureConstants = new List<RenderMethod.ShaderProperty.TextureConstant>(),
                RealConstants = new List<RenderMethod.ShaderProperty.RealConstant>()
            };

            // Get a simple list of bitmaps and arguments names
            var bmRmt2Instance = finalRm.ShaderProperties[0].Template;
            var bmRmt2 = BlamCache.Deserialize<RenderMethodTemplate>(blamCacheStream, bmRmt2Instance);

            // Get a simple list of H3 bitmaps and arguments names
            foreach (var a in bmRmt2.TextureParameterNames)
                bmMaps.Add(BlamCache.StringTable.GetString(a.Name));
            foreach (var a in bmRmt2.RealParameterNames)
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
           //for (int index = 0; index < edRmt2.BooleanArguments.Count; index++)
           //{
           //    if (CacheContext.StringTable.GetString(edRmt2.BooleanArguments[index].Name) == "use_material_texture")
           //    {
           //        finalRm.ShaderProperties[0].BooleanArguments = (ushort)(index + 1);
           //        break;
           //    }
           //}

            foreach (var a in edRmt2.TextureParameterNames)
                edMaps.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.RealParameterNames)
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

                newShaderProperty.TextureConstants.Add(
                    new RenderMethod.ShaderProperty.TextureConstant
                    {
                        Bitmap = bitmap
                    });
            }

            foreach (var a in edArgs)
                newShaderProperty.RealConstants.Add(Matcher.DefaultArgumentsValues(a));

            // Reorder blam bitmaps to match the HO rmt2 order
            // Reorder blam arguments to match the HO rmt2 order
            foreach (var eM in edMaps)
                foreach (var bM in bmMaps)
                    if (eM == bM)
                        newShaderProperty.TextureConstants[edMaps.IndexOf(eM)] = finalRm.ShaderProperties[0].TextureConstants[bmMaps.IndexOf(bM)];

            foreach (var eA in edArgs)
                foreach (var bA in bmArgs)
                    if (eA == bA)
                        newShaderProperty.RealConstants[edArgs.IndexOf(eA)] = finalRm.ShaderProperties[0].RealConstants[bmArgs.IndexOf(bA)];

            // Remove some tagblocks
            // finalRm.Unknown = new List<RenderMethod.UnknownBlock>(); // hopefully not used; this gives rmt2's name. They correspond to the first tagblocks in rmdf, they tell what the shader does
            finalRm.ImportData = new List<RenderMethod.ImportDatum>(); // most likely not used
            finalRm.ShaderProperties[0].Template = edRmt2Instance;
            finalRm.ShaderProperties[0].TextureConstants = newShaderProperty.TextureConstants;
            finalRm.ShaderProperties[0].RealConstants = newShaderProperty.RealConstants;

            Matcher.FixRmdfTagRef(finalRm);

            FixAnimationProperties(cacheStream, blamCacheStream, resourceStreams, BlamCache, CacheContext, finalRm, edRmt2, bmRmt2, blamTagName);

            // Fix any null bitmaps, caused by bitm port failure
            foreach (var a in finalRm.ShaderProperties[0].TextureConstants)
            {
                if (a.Bitmap != null)
                    continue;

                var defaultBitmap = Matcher.GetDefaultBitmapTag(edMaps[finalRm.ShaderProperties[0].TextureConstants.IndexOf(a)]);

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
                if (finalRm.ShaderProperties[0].IntegerConstants.Count == 0)
                    finalRm.ShaderProperties[0].IntegerConstants = new List<RenderMethod.ShaderProperty.IntegerConstant>
                    {
                        new RenderMethod.ShaderProperty.IntegerConstant
                        {
                            Value = 1
                        }
                    };

            switch (blamTagName)
            {
                case @"levels\dlc\chillout\shaders\chillout_flood_godrays" when finalRm is ShaderHalogram:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.TextureParameterNames.Count; i++)
                        {
                            if (CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name) == "overlay_map")
                            {
                                finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\dlc\chillout\bitmaps\chillout_flood_godrays.bitmap")[0]);
                                break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        {
                            var templateArg = edRmt2.RealParameterNames[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "overlay_map":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                    break;

                                case "overlay_tint":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.3764706f, 0.7254902f, 0.9215687f, 1f };
                                    break;

                                case "overlay_intensity":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1.25f, 1.25f, 1.25f, 1.25f };
                                    break;
                            }
                        }
                        break;
                    }

                case @"levels\dlc\chillout\shaders\chillout_invis_godrays" when finalRm is ShaderHalogram:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.TextureParameterNames.Count; i++)
                        {
                            if (CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name) == "overlay_map")
                            {
                                finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\dlc\chillout\bitmaps\chillout_invis_godrays.bitmap")[0]);
                                break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        {
                            var templateArg = edRmt2.RealParameterNames[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "overlay_map":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                    break;

                                case "overlay_tint":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.3058824f, 0.7098039f, 0.937255f, 1f };
                                    break;

                                case "overlay_intensity":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1f, 1f, 1f, 1f };
                                    break;
                            }
                        }
                        break;
                    }

                case @"levels\solo\020_base\lights\light_volume_hatlight" when finalRm is ShaderHalogram:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.TextureParameterNames.Count; i++)
                        {
                            if (CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name) == "overlay_map")
                            {
                                finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\solo\020_base\bitmaps\light_volume_hatlight.bitmap")[0]);
                                break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        {
                            var templateArg = edRmt2.RealParameterNames[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "overlay_map":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 2f, 1f, 0f, 0f };
                                    break;

                                case "overlay_tint":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.9960785f, 1f, 0.8039216f, 1f };
                                    break;

                                case "overlay_intensity":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1f, 1f, 1f, 1f };
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
                        for (var i = 0; i < edRmt2.TextureParameterNames.Count; i++)
                        {
                            if (CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name) == "bump_map")
                            {
                                finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\dlc\armory\bitmaps\concrete_floor_bump.bitmap")[0]);
                                break;
                            }
                        }
                        break;
                    }

                case @"levels\dlc\sidewinder\shaders\side_tree_branch_snow" when finalRm is Shader:
                    for (var i = 0; i < edRmt2.RealParameterNames.Count; i++)
                    {
                        var templateArg = edRmt2.RealParameterNames[i];

                        if (CacheContext.StringTable.GetString(templateArg.Name) == "env_tint_color")
                        {
                            finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0f, 0f, 0f, 0f };
                            break;
                        }
                    }
                    break;

                case @"levels\multi\isolation\sky\shaders\skydome" when finalRm is Shader:
                    for (var i = 0; i < edRmt2.RealParameterNames.Count; i++)
                    {
                        var templateArg = edRmt2.RealParameterNames[i];

                        if (CacheContext.StringTable.GetString(templateArg.Name) == "albedo_color")
                        {
                            finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.447059f, 0.376471f, 0.898039f, 1.0f };
                            break;
                        }
                    }
                    break;

                case @"levels\multi\snowbound\shaders\cov_grey_icy" when finalRm is Shader:
                    {
                        // Fixup bitmaps
                        for (var i = 0; i < edRmt2.TextureParameterNames.Count; i++)
                        {
                            switch (CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name))
                            {
                                case "base_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\for_metal_greytech_dif.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "detail_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\for_metal_greytech_icy.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "bump_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\for_metal_greytech_platebump.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "bump_detail_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\for_metal_greytech_bump.bitmap")[0]);
                                    }
                                    catch { }
                                    break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        {
                            var templateArg = edRmt2.RealParameterNames[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "base_map":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                    break;

                                case "detail_map":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1.5f, 1.5f, 0f, 0f };
                                    break;

                                case "albedo_color":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.554902f, 0.5588236f, 0.5921569f, 1f };
                                    break;

                                case "bump_map":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                    break;

                                case "bump_detail_map":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 2f, 2f, 0f, 0f };
                                    break;

                                case "diffuse_coefficient":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.4f, 0.4f, 0.4f, 0.4f };
                                    break;

                                case "specular_coefficient":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1f, 1f, 1f, 1f };
                                    break;

                                case "roughness":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.2f, 0.2f, 0.2f, 0.2f };
                                    break;

                                case "analytical_specular_contribution":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.2f, 0.2f, 0.2f, 0.2f };
                                    break;

                                case "area_specular_contribution":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.1f, 0.1f, 0.1f, 0.1f };
                                    break;

                                case "environment_map_specular_contribution":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.15f, 0.15f, 0.15f, 0.15f };
                                    break;

                                case "specular_tint":
                                    finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0.8431373f, 0.8470589f, 0.8117648f, 1f };
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
                        for (var i = 0; i < edRmt2.TextureParameterNames.Count; i++)
                        {
                            switch (CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name))
                            {
                                case "base_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_horiz.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "detail_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_granite_detail.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "detail_map2":
                                    try
                                    {
                                        switch (blamTagName)
                                        {
                                            case @"levels\multi\snowbound\shaders\rock_rocky_icy":
                                                finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_icy_blend.bitmap")[0]);
                                                break;

                                            default:
                                                finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_cliff_dif.bitmap")[0]);
                                                break;
                                        }
                                    }
                                    catch { }
                                    break;

                                case "bump_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_horiz_bump.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "bump_detail_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_granite_bump.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "height_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"levels\multi\snowbound\bitmaps\rock_horiz_parallax.bitmap")[0]);
                                    }
                                    catch { }
                                    break;

                                case "environment_map":
                                    try
                                    {
                                        finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = ConvertTag(cacheStream, blamCacheStream, resourceStreams, ParseLegacyTag(@"shaders\default_bitmaps\bitmaps\color_white.bitmap")[0]);
                                    }
                                    catch { }
                                    break;
                            }
                        }

                        // Fixup arguments
                        for (var i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        {
                            var templateArg = edRmt2.RealParameterNames[i];

                            switch (CacheContext.StringTable.GetString(templateArg.Name))
                            {
                                case "base_map":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_cliffs":
                                            finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 16f, 2f, 0f, 0.5f };
                                            break;
                                    }
                                    break;

                                case "detail_map":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_cliffs":
                                        case @"levels\multi\snowbound\shaders\rock_rocky":
                                            finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 320f, 20f, 0f, 0f };
                                            break;
                                    }
                                    break;

                                case "detail_map2":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_cliffs":
                                            finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1f, 1f, 0f, 0f };
                                            break;
                                    }
                                    break;

                                case "bump_detail_map":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_cliffs":
                                        case @"levels\multi\snowbound\shaders\rock_rocky":
                                            finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 320f, 20f, 0f, 0f };
                                            break;
                                    }
                                    break;

                                case "diffuse_coefficient":
                                    switch (blamTagName)
                                    {
                                        case @"levels\multi\snowbound\shaders\rock_rocky_icy":
                                            finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 1.2f, 1.2f, 1.2f, 1.2f };
                                            break;
                                    }
                                    break;
                            }
                        }
                        break;
                    }

                case @"levels\multi\snowbound\shaders\cov_metalplates_icy" when finalRm is Shader:
                    // Fixup bitmaps
                    for (var i = 0; i < edRmt2.TextureParameterNames.Count; i++)
                    {
                        switch (CacheContext.StringTable.GetString(edRmt2.TextureParameterNames[i].Name))
                        {
                            case "detail_map":
                                try
                                {
                                    finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = CacheContext.GetTag<Bitmap>(@"levels\multi\snowbound\bitmaps\for_metal_greytech_icy4");
                                }
                                catch { }
                                break;

                            case "detail_map2":
                                try
                                {
                                    finalRm.ShaderProperties[0].TextureConstants[i].Bitmap = CacheContext.GetTag<Bitmap>(@"levels\multi\snowbound\bitmaps\for_metal_greytech_icy3");
                                }
                                catch { }
                                break;
                        }
                    }
                    break;

                case @"levels\multi\snowbound\shaders\invis_col_glass" when finalRm is Shader:
                    // Fixup arguments
                    for (var i = 0; i < edRmt2.RealParameterNames.Count; i++)
                    {
                        var templateArg = edRmt2.RealParameterNames[i];

                        switch (CacheContext.StringTable.GetString(templateArg.Name))
                        {
                            case "albedo_color":
                                finalRm.ShaderProperties[0].RealConstants[i].Values = new float[] { 0f, 0f, 0f, 0f };
                                break;
                        }
                    }
                    break;
            }

            return finalRm;
        }

        private RenderMethod FixAnimationProperties(Stream cacheStream, Stream blamCacheStream, Dictionary<ResourceLocation, Stream> resourceStreams, GameCache blamCache, GameCacheHaloOnlineBase CacheContext, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, string blamTagName)
        {
            // TODO fix
            finalRm.ShaderProperties[0].EntryPoints = new List<RenderMethodTemplate.PackedInteger_10_6>();
            finalRm.ShaderProperties[0].ParameterTables = new List<ParameterTable>();
            finalRm.ShaderProperties[0].Parameters = new List<ParameterMapping>();
            finalRm.ShaderProperties[0].Functions = new List<ShaderFunction>();

            foreach(var texture in finalRm.ShaderProperties[0].TextureConstants)
                texture.Functions.Integer = 0;

            return finalRm;
        }
    }
}