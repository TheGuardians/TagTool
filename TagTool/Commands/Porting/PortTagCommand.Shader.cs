using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.IO;
using TagTool.Shaders.ShaderMatching;
using System;
using TagTool.Shaders.ShaderConverter;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public ShaderMatcherNew Matcher = new ShaderMatcherNew();

        private RasterizerGlobals ConvertRasterizerGlobals(RasterizerGlobals rasg)
        {
            if (BlamCache.Version == CacheVersion.Halo3ODST)
            {
                rasg.MotionBlurParametersLegacy.NumberOfTaps = 6;
            }
            return rasg;
        }

        private object ConvertShader(Stream cacheStream, Stream blamCacheStream, object definition, CachedTag blamTag, object blamDefinition)
        {
            switch (definition)
            {
                case ShaderFoliage rmfl:
                case ShaderTerrain rmtr:
                case ShaderCustom rmcs:
                case ShaderDecal rmd:
                case ShaderHalogram rmhg:
                case Shader rmsh:
                case ShaderWater rmw:
                case ShaderBlack rmbk:
                case ShaderGlass rmgl:
                case ShaderScreen rmss:
                case ShaderZonly rmzo:
                case ShaderCortana rmct:
                    return ConvertShaderInternal(cacheStream, blamCacheStream, (RenderMethod)definition, blamTag, (RenderMethod)blamDefinition);

                case ContrailSystem cntl:
                    var blamCntl = (ContrailSystem)blamDefinition;
                    for (int i = 0; i < cntl.Contrails.Count; i++)
                    {
                        cntl.Contrails[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, cntl.Contrails[i].RenderMethod, blamTag, blamCntl.Contrails[i].RenderMethod);
                        if (cntl.Contrails[i].RenderMethod == null) return null;
                    }
                    return cntl;

                case Particle prt3:
                    var blamPrt3 = (Particle)blamDefinition;
                    prt3.RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, prt3.RenderMethod, blamTag, blamPrt3.RenderMethod);
                    if (prt3.RenderMethod == null) return null;
                    return prt3;

                case LightVolumeSystem ltvl:
                    var blamLtvl = (LightVolumeSystem)blamDefinition;
                    for (int i = 0; i < ltvl.LightVolumes.Count; i++)
                    {
                        ltvl.LightVolumes[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, ltvl.LightVolumes[i].RenderMethod, blamTag, blamLtvl.LightVolumes[i].RenderMethod);
                        if (ltvl.LightVolumes[i].RenderMethod == null) return null;
                    }
                    return ltvl;
                case DecalSystem decs:
                    var blamDecs = (DecalSystem)blamDefinition;
                    for (int i = 0; i < decs.Decal.Count; i++)
                    {
                        decs.Decal[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, decs.Decal[i].RenderMethod, blamTag, blamDecs.Decal[i].RenderMethod);
                        if (decs.Decal[i].RenderMethod == null) return null;
                    }
                    return decs;
                case BeamSystem beamSystem:
                    var blamBeam = (BeamSystem)blamDefinition;
                    for (int i = 0; i < beamSystem.Beams.Count; i++)
                    {
                        beamSystem.Beams[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, beamSystem.Beams[i].RenderMethod, blamTag, blamBeam.Beams[i].RenderMethod);
                        if (beamSystem.Beams[i].RenderMethod == null) return null;
                    }
                    return beamSystem;
            }
            return null;
        }

        private RenderMethod ConvertShaderInternal(Stream cacheStream, Stream blamCacheStream, RenderMethod definition, CachedTag blamTag, RenderMethod blamDefinition)
        {
            var shaderProperty = definition.ShaderProperties[0];
            var blamShaderProperty = blamDefinition.ShaderProperties[0];

            // in case of failed match
            if (shaderProperty.Template == null)
                return null;

            return ConvertRenderMethod(cacheStream, blamCacheStream, definition, blamDefinition, blamShaderProperty.Template, blamTag);
        }

        private CachedTag GetDefaultShader(Tag groupTag)
        {
            CachedTag shaderTag;

            switch (groupTag.ToString())
            {
                case "beam" when CacheContext.TagCache.TryGetTag(@"objects\weapons\support_high\spartan_laser\fx\firing_3p.beam", out shaderTag):
                    return shaderTag;

                case "cntl" when CacheContext.TagCache.TryGetTag(@"objects\weapons\pistol\needler\fx\projectile.cntl", out shaderTag):
                    return shaderTag;

                case "decs" when CacheContext.TagCache.TryGetTag(@"fx\decals\impact_plasma\impact_plasma_medium\hard.decs", out shaderTag):
                    return shaderTag;

                case "ltvl" when CacheContext.TagCache.TryGetTag(@"objects\weapons\pistol\plasma_pistol\fx\charged\projectile.ltvl", out shaderTag):
                    return shaderTag;

                case "prt3" when CacheContext.TagCache.TryGetTag(@"fx\particles\energy\sparks\impact_spark_orange.prt3", out shaderTag):
                    return shaderTag;

                case "rmd " when CacheContext.TagCache.TryGetTag(@"objects\gear\human\military\shaders\human_military_decals.rmd", out shaderTag):
                    return shaderTag;

                case "rmfl" when CacheContext.TagCache.TryGetTag(@"levels\multi\riverworld\shaders\riverworld_tree_leafa.rmfl", out shaderTag):
                    return shaderTag;

                case "rmtr" when CacheContext.TagCache.TryGetTag(@"levels\multi\riverworld\shaders\riverworld_ground.rmtr", out shaderTag):
                    return shaderTag;

                case "rmw " when CacheContext.TagCache.TryGetTag(@"levels\multi\riverworld\shaders\riverworld_water_rough.rmw", out shaderTag):
                    return shaderTag;

                case "rmhg" when CacheContext.TagCache.TryGetTag(@"objects\multi\shaders\koth_shield.rmhg", out shaderTag):
                    return shaderTag;

                case "rmbk" when CacheContext.TagCache.TryGetTag(@"levels\dlc\bunkerworld\shaders\z_black.rmsh", out shaderTag):
                    return shaderTag;
                case "rmgl" when CacheContext.TagCache.TryGetTag(@"levels\dlc\sidewinder\shaders\side_hall_glass03.rmsh", out shaderTag):
                    return shaderTag;
                case "rmrd":
                case "rmsh":
                case "rmss":
                case "rmcs":
                case "rmzo":
                case "rmct":
                    return CacheContext.TagCache.GetTag<Shader>(@"shaders\invalid");
            }

            Console.WriteLine($"No default shader found for \"{groupTag.ToString()}\", using \"shaders\\invalid.rmsh\"");
            return CacheContext.TagCache.GetTag<Shader>(@"shaders\invalid");
        }

        private CachedTag FindClosestRmt2(Stream cacheStream, Stream blamCacheStream, CachedTag blamRmt2)
        {
            // Verify that the ShaderMatcher is ready to use
            if (!Matcher.IsInitialized)
                Matcher.Init(CacheContext, BlamCache, cacheStream, blamCacheStream, FlagIsSet(PortingFlags.Ms30), FlagIsSet(PortingFlags.PefectShaderMatchOnly));

            return Matcher.FindClosestTemplate(blamRmt2, BlamCache.Deserialize<RenderMethodTemplate>(blamCacheStream, blamRmt2), FlagIsSet(PortingFlags.GenerateShaders));
        }

        private RenderMethod ConvertRenderMethod(Stream cacheStream, Stream blamCacheStream, RenderMethod finalRm, RenderMethod blamRm, CachedTag blamRmt2, CachedTag blamTag)
        {
            ShaderConverter shaderConverter = new ShaderConverter(CacheContext, 
                BlamCache, 
                cacheStream, 
                blamCacheStream,
                finalRm, 
                blamRm, 
                Matcher);
            RenderMethod newRm = shaderConverter.ConvertRenderMethod();

            // copy each field as at this point in conversion,
            // we don't know the original tag type and what extra fields exist

            finalRm.BaseRenderMethod = newRm.BaseRenderMethod;
            finalRm.Options = newRm.Options;
            finalRm.Parameters = newRm.Parameters;
            finalRm.ShaderProperties = newRm.ShaderProperties;
            finalRm.RenderFlags = newRm.RenderFlags;
            finalRm.SortLayer = newRm.SortLayer;
            finalRm.Version = newRm.Version;
            finalRm.CustomFogSettingIndex = newRm.CustomFogSettingIndex;
            finalRm.PredictionAtomIndex = newRm.PredictionAtomIndex;

            return finalRm;
        }
    }
}