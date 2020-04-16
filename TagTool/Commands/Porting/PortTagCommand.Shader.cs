using TagTool.Cache;
using TagTool.Common;
using TagTool.Tags.Definitions;
using System.Collections.Generic;
using System.IO;
using TagTool.Shaders;
using TagTool.Shaders.ShaderMatching;
using System;
using System.Linq;
using static TagTool.Tags.Definitions.RenderMethod;
using static TagTool.Tags.Definitions.RenderMethod.ShaderProperty;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {
        public ShaderMatcherNew Matcher = new ShaderMatcherNew();

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
                    return ConvertShaderInternal(cacheStream, blamCacheStream, (RenderMethod)definition, blamTag, (RenderMethod)blamDefinition);

                case ContrailSystem cntl:
                    var blamCntl = (ContrailSystem)blamDefinition;
                    for (int i = 0; i < cntl.Contrail.Count; i++)
                    {
                        cntl.Contrail[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, cntl.Contrail[i].RenderMethod, blamTag, blamCntl.Contrail[i].RenderMethod);
                        if (cntl.Contrail[i].RenderMethod == null) return null;
                    }
                    return cntl;

                case Particle prt3:
                    var blamPrt3 = (Particle)blamDefinition;
                    prt3.RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, prt3.RenderMethod, blamTag, blamPrt3.RenderMethod);
                    if (prt3.RenderMethod == null) return null;
                    return prt3;

                case LightVolumeSystem ltvl:
                    var blamLtvl = (LightVolumeSystem)blamDefinition;
                    for (int i = 0; i < ltvl.LightVolume.Count; i++)
                    {
                        ltvl.LightVolume[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, ltvl.LightVolume[i].RenderMethod, blamTag, blamLtvl.LightVolume[i].RenderMethod);
                        if (ltvl.LightVolume[i].RenderMethod == null) return null;
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
                    for (int i = 0; i < beamSystem.Beam.Count; i++)
                    {
                        beamSystem.Beam[i].RenderMethod = ConvertShaderInternal(cacheStream, blamCacheStream, beamSystem.Beam[i].RenderMethod, blamTag, blamBeam.Beam[i].RenderMethod);
                        if (beamSystem.Beam[i].RenderMethod == null) return null;
                    }
                    return beamSystem;

                case ShaderBlack rmbk:
                    return CreateShaderBlack(cacheStream, blamCacheStream, rmbk, blamTag, (RenderMethod)blamDefinition);

                case ShaderScreen rmss:
                case ShaderZonly rmzo:
                case ShaderCortana rmct:
                    return null;
            }
            return null;
        }

        // temp until generation is possible
        private RenderMethod CreateShaderBlack(Stream cacheStream, Stream blamCacheStream, ShaderBlack rmbk, CachedTag blamTag, RenderMethod blamDefinition)
        {
            // use default template - we only need albedo_color for this to work
            CachedTag defaultTemplateInstance = CacheContext.GetTag(ShaderMatcherNew.DefaultTemplate);
            rmbk.BaseRenderMethod = Matcher.FindRmdf(defaultTemplateInstance);

            rmbk.ShaderProperties[0].Template = defaultTemplateInstance;

            // reset rm options
            rmbk.RenderMethodDefinitionOptionIndices.Clear();
            for (int i = 0; i < 11; i++)
                rmbk.RenderMethodDefinitionOptionIndices.Add(new RenderMethodDefinitionOptionIndex());

            // setup default values
            RenderMethod result = ConvertShaderInternal(cacheStream, blamCacheStream, rmbk, blamTag, blamDefinition);

            // set albedo_color to black
            result.ShaderProperties[0].RealConstants[2].Arg0 = 0.0f;
            result.ShaderProperties[0].RealConstants[2].Arg1 = 0.0f;
            result.ShaderProperties[0].RealConstants[2].Arg2 = 0.0f;
            // enable dynamic lights
            result.ShaderProperties[0].BooleanConstants = 0;

            return result;
        }

        private RenderMethod ConvertShaderInternal(Stream cacheStream, Stream blamCacheStream, RenderMethod definition, CachedTag blamTag, RenderMethod blamDefinition)
        {
            var shaderProperty = definition.ShaderProperties[0];
            var blamShaderProperty = blamDefinition.ShaderProperties[0];

            // in case of failed match
            if (shaderProperty.Template == null)
                return null;

            return ConvertRenderMethod(cacheStream, blamCacheStream, definition, blamShaderProperty.Template, blamTag.Name);
        }

        private CachedTag GetDefaultShader(Tag groupTag)
        {
            switch (groupTag.ToString())
            {
                case "beam":
                    return CacheContext.GetTag<BeamSystem>(@"objects\weapons\support_high\spartan_laser\fx\firing_3p");

                case "cntl":
                    return CacheContext.GetTag<ContrailSystem>(@"objects\weapons\pistol\needler\fx\projectile");

                case "decs":
                    return CacheContext.GetTag<DecalSystem>(@"fx\decals\impact_plasma\impact_plasma_medium\hard");

                case "ltvl":
                    return CacheContext.GetTag<LightVolumeSystem>(@"objects\weapons\pistol\plasma_pistol\fx\charged\projectile");

                case "prt3":
                    return CacheContext.GetTag<Particle>(@"fx\particles\energy\sparks\impact_spark_orange");

                case "rmd ":
                    return CacheContext.GetTag<ShaderDecal>(@"objects\gear\human\military\shaders\human_military_decals");

                case "rmfl":
                    return CacheContext.GetTag<ShaderFoliage>(@"levels\multi\riverworld\shaders\riverworld_tree_leafa");

                case "rmtr":
                    return CacheContext.GetTag<ShaderTerrain>(@"levels\multi\riverworld\shaders\riverworld_ground");

                case "rmw ":
                    return CacheContext.GetTag<ShaderWater>(@"levels\multi\riverworld\shaders\riverworld_water_rough");

                case "rmhg":
                    return CacheContext.GetTag<ShaderHalogram>(@"objects\multi\shaders\koth_shield");

                case "rmbk": // hackfix
                    return CacheContext.GetTag<Shader>(@"levels\dlc\bunkerworld\shaders\z_black");

                case "rmrd":
                case "rmsh":
                case "rmss":
                case "rmcs":
                case "rmzo":
                case "rmct":
                    return CacheContext.GetTag<Shader>(@"shaders\invalid");
            }
            return CacheContext.GetTag<Shader>(@"shaders\invalid");
        }

        private CachedTag FindClosestRmt2(Stream cacheStream, Stream blamCacheStream, CachedTag blamRmt2)
        {
            // Verify that the ShaderMatcher is ready to use
            if (!Matcher.IsInitialized)
                Matcher.Init(CacheContext, BlamCache, cacheStream, blamCacheStream, FlagIsSet(PortingFlags.Ms30), FlagIsSet(PortingFlags.PefectShaderMatchOnly));

            return Matcher.FindClosestTemplate(blamRmt2, BlamCache.Deserialize<RenderMethodTemplate>(blamCacheStream, blamRmt2));
        }

        private RenderMethod ConvertRenderMethod(Stream cacheStream, Stream blamCacheStream, RenderMethod finalRm, CachedTag blamRmt2, string blamTagName)
        {
            var bmMaps = new List<string>();
            var bmRealConstants = new List<string>();
            var bmIntConstants = new List<string>();
            var bmBoolConstants = new List<string>();
            var edMaps = new List<string>();
            var edRealConstants = new List<string>();
            var edIntConstants = new List<string>();
            var edBoolConstants = new List<string>();

            // Make a template of ShaderProperty, with the correct bitmaps and arguments counts. 
            var newShaderProperty = new RenderMethod.ShaderProperty
            {
                TextureConstants = new List<TextureConstant>(),
                RealConstants = new List<RealConstant>(),
                IntegerConstants = new List<uint>(),
                BooleanConstants = 0
            };

            // Get a simple list of bitmaps and arguments names
            var bmRmt2Instance = blamRmt2;
            var bmRmt2 = BlamCache.Deserialize<RenderMethodTemplate>(blamCacheStream, bmRmt2Instance);

            // Get a simple list of H3 bitmaps and arguments names
            foreach (var a in bmRmt2.TextureParameterNames)
                bmMaps.Add(BlamCache.StringTable.GetString(a.Name));
            foreach (var a in bmRmt2.RealParameterNames)
                bmRealConstants.Add(BlamCache.StringTable.GetString(a.Name));
            foreach (var a in bmRmt2.IntegerParameterNames)
                bmIntConstants.Add(BlamCache.StringTable.GetString(a.Name));
            foreach (var a in bmRmt2.BooleanParameterNames)
                bmBoolConstants.Add(BlamCache.StringTable.GetString(a.Name));

            // get template previously matched from porttag rmt2
            CachedTag edRmt2Instance = finalRm.ShaderProperties[0].Template;

            if (edRmt2Instance == null)
            {
                throw new Exception($"Failed to find HO rmt2 for this RenderMethod instance");
            }

            // create blam rmt2 descriptor
            ShaderMatcherNew.Rmt2Descriptor.TryParse(bmRmt2Instance.Name, out ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor);
            // create ed rmt2 descriptor
            ShaderMatcherNew.Rmt2Descriptor.TryParse(edRmt2Instance.Name, out ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor);

            var edRmt2 = CacheContext.Deserialize<RenderMethodTemplate>(cacheStream, edRmt2Instance);

            foreach (var a in edRmt2.TextureParameterNames)
                edMaps.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.RealParameterNames)
                edRealConstants.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.IntegerParameterNames)
                edIntConstants.Add(CacheContext.StringTable.GetString(a.Name));
            foreach (var a in edRmt2.BooleanParameterNames)
                edBoolConstants.Add(CacheContext.StringTable.GetString(a.Name));

            // get relevant rmdf
            CachedTag rmdfInstance = Matcher.FindRmdf(edRmt2Instance);
            if (rmdfInstance == null) // shader matching will fail without an rmdf -- throw an exception
                throw new Exception($"Unable to find valid \"{edRmt2Descriptor.Type}\" rmdf for rmt2");
            RenderMethodDefinition renderMethodDefinition = CacheContext.Deserialize<RenderMethodDefinition>(cacheStream, rmdfInstance);

            // dictionaries for fast lookup
            //var optionParameters = Matcher.GetOptionParameters(rmt2Descriptor.Options.ToList(), renderMethodDefinition);
            var optionBlocks = Matcher.GetOptionBlocks(edRmt2Descriptor.Options.ToList(), renderMethodDefinition);
            var optionBitmaps = Matcher.GetOptionBitmaps(edRmt2Descriptor.Options.ToList(), renderMethodDefinition);

            List<string> methodNames = new List<string>();
            foreach (var method in renderMethodDefinition.Methods)
                methodNames.Add(CacheContext.StringTable.GetString(method.Type));

            foreach (var a in edRealConstants)
                newShaderProperty.RealConstants.Add(GetDefaultRealConstant(a, edRmt2Descriptor.Type, methodNames, optionBlocks));

            // apply option->option conversion where applicable
            ApplyOptionFixups(newShaderProperty.RealConstants, blamRmt2Descriptor, edRmt2Descriptor, edRmt2, renderMethodDefinition);

            foreach (var a in edIntConstants)
                newShaderProperty.IntegerConstants.Add(0);

            foreach (var a in edMaps)
                newShaderProperty.TextureConstants.Add(GetDefaultTextureConstant(a, edRmt2Descriptor, optionBitmaps));

            // if we have bits enabled by default, this actually disables boolean args. fixes a few visual issues
            for (int a = 0; a < edRmt2.BooleanParameterNames.Count; a++)
                newShaderProperty.BooleanConstants |= (1u << a);

            // Reorder blam bitmaps to match the HO rmt2 order
            // Reorder blam real constants to match the HO rmt2 order
            // Reorder blam int constants to match the HO rmt2 order
            // Reorder blam bool constants to match the HO rmt2 order
            foreach (var eM in edMaps)
                foreach (var bM in bmMaps)
                    if (eM == bM)
                        newShaderProperty.TextureConstants[edMaps.IndexOf(eM)] = finalRm.ShaderProperties[0].TextureConstants[bmMaps.IndexOf(bM)];

            foreach (var eA in edRealConstants)
                foreach (var bA in bmRealConstants)
                    if (eA == bA)
                        newShaderProperty.RealConstants[edRealConstants.IndexOf(eA)] = finalRm.ShaderProperties[0].RealConstants[bmRealConstants.IndexOf(bA)];

            foreach (var eA in edIntConstants)
                foreach (var bA in bmIntConstants)
                    if (eA == bA)
                        newShaderProperty.IntegerConstants[edIntConstants.IndexOf(eA)] = finalRm.ShaderProperties[0].IntegerConstants[bmIntConstants.IndexOf(bA)];

            foreach (var eA in edBoolConstants)
                foreach (var bA in bmBoolConstants)
                    if (eA == bA)
                    {
                        if ((finalRm.ShaderProperties[0].BooleanConstants & (1u << bmBoolConstants.IndexOf(bA))) == 0)
                            newShaderProperty.BooleanConstants &= ~(1u << edBoolConstants.IndexOf(eA));
                    }


            finalRm.ImportData = new List<ImportDatum>(); // most likely not used
            finalRm.ShaderProperties[0].Template = edRmt2Instance;
            finalRm.ShaderProperties[0].TextureConstants = newShaderProperty.TextureConstants;
            finalRm.ShaderProperties[0].RealConstants = newShaderProperty.RealConstants;
            finalRm.ShaderProperties[0].IntegerConstants = newShaderProperty.IntegerConstants;
            finalRm.ShaderProperties[0].BooleanConstants = newShaderProperty.BooleanConstants;

            // fixup runtime queryable properties
            for (int i = 0; i < finalRm.ShaderProperties[0].QueryableProperties.Length; i++)
            {
                if (finalRm.ShaderProperties[0].QueryableProperties[i] == -1)
                    continue;

                switch (i)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 5:
                        finalRm.ShaderProperties[0].QueryableProperties[i] = (short)edMaps.IndexOf(bmMaps[finalRm.ShaderProperties[0].QueryableProperties[i]]);
                        break;
                    case 4:
                        finalRm.ShaderProperties[0].QueryableProperties[i] = (short)edRealConstants.IndexOf(bmRealConstants[finalRm.ShaderProperties[0].QueryableProperties[i]]);
                        break;
                    default:
                        finalRm.ShaderProperties[0].QueryableProperties[i] = -1;
                        break;
                }
            }

            // fixup xform arguments;
            foreach (var tex in finalRm.ShaderProperties[0].TextureConstants)
            {
                if (tex.XFormArgumentIndex != -1)
                    tex.XFormArgumentIndex = (sbyte)edRealConstants.IndexOf(bmRealConstants[tex.XFormArgumentIndex]);
            }

            finalRm.BaseRenderMethod = rmdfInstance;

            // fixup rm animations
            if (finalRm.ShaderProperties[0].Functions.Count > 0)
                BuildRenderMethodAnimations(cacheStream, blamCacheStream, finalRm, edRmt2, bmRmt2, blamTagName, edRmt2Descriptor.IsMs30);

            // build new rm option indices
            finalRm.RenderMethodDefinitionOptionIndices = BuildRenderMethodOptionIndices(edRmt2Descriptor);

            return finalRm;
        }

        private ShaderParameter.RType GetRegisterType(RenderMethodTemplate.ParameterUsage parameterUsage)
        {
            switch (parameterUsage)
            {
                case RenderMethodTemplate.ParameterUsage.Texture:
                case RenderMethodTemplate.ParameterUsage.TextureExtern:
                    return ShaderParameter.RType.Sampler;
                case RenderMethodTemplate.ParameterUsage.PS_Boolean:
                case RenderMethodTemplate.ParameterUsage.VS_Boolean:
                    return ShaderParameter.RType.Boolean;
                case RenderMethodTemplate.ParameterUsage.PS_Real:
                case RenderMethodTemplate.ParameterUsage.PS_RealExtern:
                case RenderMethodTemplate.ParameterUsage.VS_Real:
                case RenderMethodTemplate.ParameterUsage.VS_RealExtern:
                    return ShaderParameter.RType.Vector;
                case RenderMethodTemplate.ParameterUsage.PS_Integer:
                case RenderMethodTemplate.ParameterUsage.PS_IntegerExtern:
                case RenderMethodTemplate.ParameterUsage.VS_Integer:
                case RenderMethodTemplate.ParameterUsage.VS_IntegerExtern:
                    return ShaderParameter.RType.Integer;
                default:
                    return ShaderParameter.RType.Vector;
            }
        }

        // Populates RM animations then remaps the original animations.
        private void BuildRenderMethodAnimations(Stream cacheStream, Stream blamCacheStream, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, string blamTagName, bool isMs30)
        {
            //
            // Store shader definitions
            //

            PixelShader blamPixl = BlamCache.Deserialize<PixelShader>(blamCacheStream, bmRmt2.PixelShader);
            PixelShader basePixl = CacheContext.Deserialize<PixelShader>(cacheStream, edRmt2.PixelShader);
            GlobalVertexShader blamGlvs = null;
            GlobalVertexShader baseGlvs = null;

            List<short> SupportedVertexTypes = new List<short>();

            string blamRmdfName = finalRm.BaseRenderMethod.Name;
            if (isMs30 && blamRmdfName.StartsWith("ms30\\")) // remove "ms30\\" from the name
                blamRmdfName = finalRm.BaseRenderMethod.Name.Remove(0, 5);
            if (BlamCache.TryGetTag(blamRmdfName + ".rmdf", out CachedTag blamRmdfTag) && CacheContext.TryGetTag(finalRm.BaseRenderMethod.Name + ".rmdf", out CachedTag baseRmdfTag))
            {
                var blamRmdf = BlamCache.Deserialize<RenderMethodDefinition>(blamCacheStream, blamRmdfTag);
                var baseRmdf = CacheContext.Deserialize<RenderMethodDefinition>(cacheStream, baseRmdfTag);

                // while rmdf is deserialized, get vertex type indices
                foreach (var vertexUnknown in baseRmdf.Unknown)
                    SupportedVertexTypes.Add(vertexUnknown.VertexType);

                if (blamRmdf.GlobalVertexShader != null && baseRmdf.GlobalVertexShader != null)
                {
                    baseGlvs = CacheContext.Deserialize<GlobalVertexShader>(cacheStream, baseRmdf.GlobalVertexShader);
                    blamGlvs = BlamCache.Deserialize<GlobalVertexShader>(blamCacheStream, blamRmdf.GlobalVertexShader);
                }
            }
            // a glvs is required. there should be no instance where this code is reached but gotta be sure.
            if (blamGlvs == null || baseGlvs == null)
            {
                Console.WriteLine("ERROR: no global vertex shader found. Shader animations will not be converted.");
                finalRm.ShaderProperties[0].EntryPoints.Clear();
                finalRm.ShaderProperties[0].ParameterTables.Clear();
                finalRm.ShaderProperties[0].Parameters.Clear();
                finalRm.ShaderProperties[0].Functions.Clear();
                foreach (var textureConstant in finalRm.ShaderProperties[0].TextureConstants)
                    textureConstant.Functions.Integer = 0;
                return;
            }

            // get entry point indices
            List<int> validEntryPoints = new List<int>();
            for (int i = 0; i < 18; i++) // ignore z_only and sfx_distort, we dont need to match their registers
            {
                uint val = (uint)edRmt2.ValidEntryPoints >> i;
                if ((val & 1) > 0)
                    validEntryPoints.Add(i);
            }

            //
            // Identify registers and map them portingcache->basecache
            //

            // loop entry points, they point to a parameter table
            // "entryPointIndex" is the entrypoint index in pixl/glvs
            // to get the correct glvs shaders, we need the supported vertex types (collected from rmdf above)
            // the purpose of this monstrous loop is to get the correct registers -- previous code just collected everything which turned out problematic

            List<Dictionary<RegisterID, int>> edEntryPointPixelRegisters = new List<Dictionary<RegisterID, int>>();
            List<Dictionary<RegisterID, int>> edEntryPointVertexRegisters = new List<Dictionary<RegisterID, int>>();
            List<Dictionary<RegisterID, int>> blamEntryPointPixelRegisters = new List<Dictionary<RegisterID, int>>();
            List<Dictionary<RegisterID, int>> blamEntryPointVertexRegisters = new List<Dictionary<RegisterID, int>>();
            // store tableindex : entrypoint
            Dictionary<int, int> tableEntryPoints = new Dictionary<int, int>();

            // entry points should be the same in both rmt2 so we can use the same loop -- if not, animations cannot be ported anyway
            for (int entryPointIndex = 0; entryPointIndex < validEntryPoints.Count; entryPointIndex++)
            {
                // register info : register index
                Dictionary<RegisterID, int> edPixelRegisters = new Dictionary<RegisterID, int>();
                Dictionary<RegisterID, int> edVertexRegisters = new Dictionary<RegisterID, int>();
                Dictionary<RegisterID, int> blamPixelRegisters = new Dictionary<RegisterID, int>();
                Dictionary<RegisterID, int> blamVertexRegisters = new Dictionary<RegisterID, int>();

                var edEntryPoint = edRmt2.EntryPoints[validEntryPoints[entryPointIndex]];
                var blamEntryPoint = bmRmt2.EntryPoints[validEntryPoints[entryPointIndex]];

                // get ed parameters
                int tableEndOffset = edEntryPoint.Offset + edEntryPoint.Count;
                for (int edTableIndex = edEntryPoint.Offset; edTableIndex < tableEndOffset; edTableIndex++)
                {
                    // store
                    tableEntryPoints.Add(edTableIndex, entryPointIndex);

                    var rmt2Table = edRmt2.ParameterTables[edTableIndex];

                    // loop through parameter table values
                    for (int parameterUsageIndex = 0; parameterUsageIndex < (int)RenderMethodTemplate.ParameterUsage.Count; parameterUsageIndex++)
                    {
                        var value = rmt2Table.Values[parameterUsageIndex];
                        if (value.Count < 1)
                            continue;

                        // if its a vertex value, we need to access glvs instead of pixl
                        bool isVertex = parameterUsageIndex > (int)RenderMethodTemplate.ParameterUsage.Texture && parameterUsageIndex < (int)RenderMethodTemplate.ParameterUsage.PS_Real;

                        // loop parameters from parameter table value
                        int parameterEndOffset = value.Offset + value.Count;
                        for (int parameterIndex = value.Offset; parameterIndex < parameterEndOffset; parameterIndex++)
                        {
                            var parameter = edRmt2.Parameters[parameterIndex];

                            // get register type from ParameterUsage index
                            var parameterType = GetRegisterType((RenderMethodTemplate.ParameterUsage)parameterUsageIndex);

                            // from here we access the shaders to get the register name

                            if (isVertex) // vertex register
                            {
                                // loop supported vertex types -- registers should be the same across them, but just to be sure
                                foreach (var supportedVertexType in SupportedVertexTypes)
                                {
                                    var vertexType = baseGlvs.VertexTypes[supportedVertexType];
                                    // get shader index from entry point block
                                    int shaderIndex = vertexType.DrawModes[validEntryPoints[entryPointIndex]].ShaderIndex;

                                    if (shaderIndex > -1)
                                    {
                                        foreach (var pcParameter in baseGlvs.Shaders[shaderIndex].PCParameters)
                                        {
                                            var registerId = new RegisterID(CacheContext.StringTable.GetString(pcParameter.ParameterName), parameterType);
                                            if (pcParameter.RegisterIndex == parameter.RegisterIndex && pcParameter.RegisterType == parameterType && !edVertexRegisters.ContainsKey(registerId))
                                            {
                                                edVertexRegisters.Add(registerId, pcParameter.RegisterIndex);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else // pixel register
                            {
                                // get shader index from entry point block
                                int shaderIndex = basePixl.DrawModes[validEntryPoints[entryPointIndex]].Offset;

                                if (shaderIndex > -1)
                                {
                                    foreach (var pcParameter in basePixl.Shaders[shaderIndex].PCParameters)
                                    {
                                        var registerId = new RegisterID(CacheContext.StringTable.GetString(pcParameter.ParameterName), parameterType);
                                        if (pcParameter.RegisterIndex == parameter.RegisterIndex && pcParameter.RegisterType == parameterType && !edPixelRegisters.ContainsKey(registerId))
                                        {
                                            edPixelRegisters.Add(registerId, pcParameter.RegisterIndex);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // get blam parameters
                int blamTableEndOffset = blamEntryPoint.Offset + blamEntryPoint.Count;
                for (int blamTableIndex = blamEntryPoint.Offset; blamTableIndex < blamTableEndOffset; blamTableIndex++)
                {
                    var rmt2Table = bmRmt2.ParameterTables[blamTableIndex];

                    for (int parameterUsageIndex = 0; parameterUsageIndex < (int)RenderMethodTemplate.ParameterUsage.Count; parameterUsageIndex++)
                    {
                        var value = rmt2Table.Values[parameterUsageIndex];
                        if (value.Count < 1)
                            continue;

                        // if its a vertex value, we need to access glvs instead of pixl
                        bool isVertex = parameterUsageIndex > (int)RenderMethodTemplate.ParameterUsage.Texture && parameterUsageIndex < (int)RenderMethodTemplate.ParameterUsage.PS_Real;

                        // loop parameters from parameter table value
                        int parameterEndOffset = value.Offset + value.Count;
                        for (int parameterIndex = value.Offset; parameterIndex < parameterEndOffset; parameterIndex++)
                        {
                            var parameter = bmRmt2.Parameters[parameterIndex];

                            // get register type from ParameterUsage index
                            var parameterType = GetRegisterType((RenderMethodTemplate.ParameterUsage)parameterUsageIndex);

                            // from here we access the shaders to get the register name

                            if (isVertex) // vertex register
                            {
                                // loop supported vertex types -- registers should be the same across them, but just to be sure
                                foreach (var supportedVertexType in SupportedVertexTypes)
                                {
                                    var vertexType = blamGlvs.VertexTypes[supportedVertexType];
                                    // get shader index from entry point block
                                    int shaderIndex = vertexType.DrawModes[validEntryPoints[entryPointIndex]].ShaderIndex;

                                    if (shaderIndex > -1)
                                    {
                                        foreach (var xboxParameter in blamGlvs.Shaders[shaderIndex].XboxParameters)
                                        {
                                            var registerId = new RegisterID(BlamCache.StringTable.GetString(xboxParameter.ParameterName), parameterType);
                                            if (xboxParameter.RegisterIndex == parameter.RegisterIndex && xboxParameter.RegisterType == parameterType && !blamVertexRegisters.ContainsKey(registerId))
                                            {
                                                blamVertexRegisters.Add(registerId, xboxParameter.RegisterIndex);
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else // pixel register
                            {
                                // get shader index from entry point block
                                int shaderIndex = blamPixl.DrawModes[validEntryPoints[entryPointIndex]].Offset;

                                if (shaderIndex > -1)
                                {
                                    foreach (var xboxParameter in blamPixl.Shaders[shaderIndex].XboxParameters)
                                    {
                                        var registerId = new RegisterID(BlamCache.StringTable.GetString(xboxParameter.ParameterName), parameterType);
                                        if (xboxParameter.RegisterIndex == parameter.RegisterIndex && xboxParameter.RegisterType == parameterType && !blamPixelRegisters.ContainsKey(registerId))
                                        {
                                            blamPixelRegisters.Add(registerId, xboxParameter.RegisterIndex);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                edEntryPointPixelRegisters.Add(edPixelRegisters);
                edEntryPointVertexRegisters.Add(edVertexRegisters);
                blamEntryPointPixelRegisters.Add(blamPixelRegisters);
                blamEntryPointVertexRegisters.Add(blamVertexRegisters);
            }

            // now we do the rm side of things
            // the idea is the same - follow entrypoint->table->Paramters
            // however this time, we are matching parameter registers then rewriting them all to the rm.
            // if a register cannot be found, that parameter will no longer be used.

            // fix table counts before doing parameter table stuff
            for (int entryPointIndex = 0; entryPointIndex < validEntryPoints.Count; entryPointIndex++)
            {
                var entryPoint = edRmt2.EntryPoints[validEntryPoints[entryPointIndex]];
                var rmEntryPoint = finalRm.ShaderProperties[0].EntryPoints[validEntryPoints[entryPointIndex]];

                int cutoffIndex = entryPoint.Offset + rmEntryPoint.Count;

                // fix table counts
                if (entryPoint.Count > rmEntryPoint.Count)
                {
                    int amount = entryPoint.Count - rmEntryPoint.Count;

                    for (int i = 0; i < amount; i++)
                    {
                        if (finalRm.ShaderProperties[0].ParameterTables.Count <= cutoffIndex)
                            finalRm.ShaderProperties[0].ParameterTables.Add(new ParameterTable());
                        else
                            finalRm.ShaderProperties[0].ParameterTables.Insert(cutoffIndex, new ParameterTable());
                    }
                }
            }
            finalRm.ShaderProperties[0].EntryPoints = edRmt2.EntryPoints;

            // table<parameters>
            List<List<ParameterMapping>> parameterTablesTextureParameters = new List<List<ParameterMapping>>();
            List<List<ParameterMapping>> parameterTablesPixelParameters = new List<List<ParameterMapping>>();
            List<List<ParameterMapping>> parameterTablesVertexParameters = new List<List<ParameterMapping>>();
            // for index preservation
            List<int> parameterTableIndices = new List<int>();

            for (int entryPointIndex = 0; entryPointIndex < validEntryPoints.Count; entryPointIndex++)
            {
                var entryPoint = edRmt2.EntryPoints[validEntryPoints[entryPointIndex]];

                int tableEndOffset = entryPoint.Offset + entryPoint.Count;
                for (int tableIndex = entryPoint.Offset; tableIndex < tableEndOffset; tableIndex++)
                {
                    var table = finalRm.ShaderProperties[0].ParameterTables[tableIndex];

                    // Add table parameters to a list

                    List<ParameterMapping> textureParameters = new List<ParameterMapping>();
                    List<ParameterMapping> pixelParameters = new List<ParameterMapping>();
                    List<ParameterMapping> vertexParameters = new List<ParameterMapping>();

                    int textureEndOffset = table.Texture.Offset + table.Texture.Count;
                    for (int textureIndex = table.Texture.Offset; textureIndex < textureEndOffset; textureIndex++)
                        textureParameters.Add(finalRm.ShaderProperties[0].Parameters[textureIndex]);
                    int vertexEndOffset = table.RealVertex.Offset + table.RealVertex.Count;
                    for (int vertexIndex = table.RealVertex.Offset; vertexIndex < vertexEndOffset; vertexIndex++)
                        vertexParameters.Add(finalRm.ShaderProperties[0].Parameters[vertexIndex]);
                    int pixelEndOffset = table.RealPixel.Offset + table.RealPixel.Count;
                    for (int pixelIndex = table.RealPixel.Offset; pixelIndex < pixelEndOffset; pixelIndex++)
                        pixelParameters.Add(finalRm.ShaderProperties[0].Parameters[pixelIndex]);

                    parameterTableIndices.Add(tableIndex);
                    parameterTablesTextureParameters.Add(textureParameters);
                    parameterTablesPixelParameters.Add(pixelParameters);
                    parameterTablesVertexParameters.Add(vertexParameters);
                }
            }

            // reorder table block indices to match rmt2 (this order gets lost with the entrypoints loop)
            List<List<ParameterMapping>> tempTable1 = new List<List<ParameterMapping>>();
            List<List<ParameterMapping>> tempTable2 = new List<List<ParameterMapping>>();
            List<List<ParameterMapping>> tempTable3 = new List<List<ParameterMapping>>();
            while (tempTable1.Count != parameterTablesTextureParameters.Count)
            {
                tempTable1.Add(new List<ParameterMapping>()); // make up count
                tempTable2.Add(new List<ParameterMapping>()); // make up count
                tempTable3.Add(new List<ParameterMapping>()); // make up count
            }
            for (int i = 0; i < parameterTablesTextureParameters.Count; i++)
            {
                tempTable1[parameterTableIndices[i]] = parameterTablesTextureParameters[i];
                tempTable2[parameterTableIndices[i]] = parameterTablesPixelParameters[i];
                tempTable3[parameterTableIndices[i]] = parameterTablesVertexParameters[i];
            }
            parameterTablesTextureParameters = tempTable1;
            parameterTablesPixelParameters = tempTable2;
            parameterTablesVertexParameters = tempTable3;

            // now that we have a collection of all the used registers in both templates with an identifier, we can remap them from blam->ms23

            // TODO: entry point -> table -> register dictionary
            // looping through all is causing issues

            // tableindex : entrypoint tableEntryPoints

            for (int tableIndex = 0; tableIndex < parameterTablesTextureParameters.Count; tableIndex++)
            {
                int registerListIndex = tableEntryPoints[tableIndex];

                foreach (var parameter in parameterTablesTextureParameters[tableIndex])
                {
                    short newRegister = -1;

                    var blamPixelRegisters = blamEntryPointPixelRegisters[registerListIndex];
                    var edPixelRegisters = edEntryPointPixelRegisters[registerListIndex];

                    foreach (var registerId in blamPixelRegisters.Keys)
                        if (registerId.Type == ShaderParameter.RType.Sampler && blamPixelRegisters[registerId] == parameter.RegisterIndex && edPixelRegisters.ContainsKey(registerId))
                            newRegister = (short)edPixelRegisters[registerId];

                    parameter.RegisterIndex = newRegister;
                }
                foreach (var parameter in parameterTablesPixelParameters[tableIndex])
                {
                    short newRegister = -1;

                    var blamPixelRegisters = blamEntryPointPixelRegisters[registerListIndex];
                    var edPixelRegisters = edEntryPointPixelRegisters[registerListIndex];

                    foreach (var registerId in blamPixelRegisters.Keys)
                        if (registerId.Type == ShaderParameter.RType.Vector && blamPixelRegisters[registerId] == parameter.RegisterIndex && edPixelRegisters.ContainsKey(registerId))
                            newRegister = (short)edPixelRegisters[registerId];

                    parameter.RegisterIndex = newRegister;
                }
                foreach (var parameter in parameterTablesVertexParameters[tableIndex])
                {
                    short newRegister = -1;

                    var blamVertexRegisters = blamEntryPointVertexRegisters[registerListIndex];
                    var edVertexRegisters = edEntryPointVertexRegisters[registerListIndex];

                    foreach (var registerId in blamVertexRegisters.Keys)
                        if (registerId.Type == ShaderParameter.RType.Vector && blamVertexRegisters[registerId] == parameter.RegisterIndex && edVertexRegisters.ContainsKey(registerId))
                            newRegister = (short)edVertexRegisters[registerId];

                    parameter.RegisterIndex = newRegister;
                }
            }

            //
            // Build and write to rm
            //

            // clear current rm data
            finalRm.ShaderProperties[0].ParameterTables.Clear();
            finalRm.ShaderProperties[0].Parameters.Clear();

            List<ParameterTable> newParameterTables = new List<ParameterTable>();
            List<ParameterMapping> newParameters = new List<ParameterMapping>();

            // count is the same for all 3
            int parameterTableCount = parameterTablesTextureParameters.Count;
            for (int i = 0; i < parameterTableCount; i++)
            {
                ParameterTable newParameterTable = new ParameterTable();

                // use variables to keep track of blocks for the table's indices

                ushort offset = (ushort)newParameters.Count;
                ushort currentCount = 0;
                foreach (var textureParameter in parameterTablesTextureParameters[i])
                {
                    if (textureParameter.RegisterIndex == -1)
                        continue; // this means the original animation isnt possible with this shader.

                    newParameters.Add(textureParameter);
                    currentCount++;
                }
                // write indices into table
                newParameterTable.Texture.Count = currentCount;
                if (newParameterTable.Texture.Count > 0)
                    newParameterTable.Texture.Offset = offset;

                offset = (ushort)newParameters.Count;
                currentCount = 0;
                foreach (var pixelParameter in parameterTablesPixelParameters[i])
                {
                    if (pixelParameter.RegisterIndex == -1)
                        continue; // this means the original animation isnt possible with this shader.

                    newParameters.Add(pixelParameter);
                    currentCount++;
                }
                // write indices into table
                newParameterTable.RealPixel.Count = currentCount;
                if (newParameterTable.RealPixel.Count > 0)
                    newParameterTable.RealPixel.Offset = offset;

                offset = (ushort)newParameters.Count;
                currentCount = 0;
                foreach (var vertexParameter in parameterTablesVertexParameters[i])
                {
                    if (vertexParameter.RegisterIndex == -1)
                        continue; // this means the original animation isnt possible with this shader.

                    newParameters.Add(vertexParameter);
                    currentCount++;
                }
                // write indices into table
                newParameterTable.RealVertex.Count = currentCount;
                if (newParameterTable.RealVertex.Count > 0)
                    newParameterTable.RealVertex.Offset = offset;

                newParameterTables.Add(newParameterTable);
            }

            // add new blocks to rm
            finalRm.ShaderProperties[0].ParameterTables = newParameterTables;
            finalRm.ShaderProperties[0].Parameters = newParameters;

            // remap argument indices within parameters to match new rmt2 (pixel and vertex only)

            foreach (var parameterTable in finalRm.ShaderProperties[0].ParameterTables)
            {
                int pixelEndOffset = parameterTable.RealPixel.Offset + parameterTable.RealPixel.Count;
                for (int i = parameterTable.RealPixel.Offset; i < pixelEndOffset; i++)
                {
                    string blamArgName = BlamCache.StringTable.GetString(bmRmt2.RealParameterNames[finalRm.ShaderProperties[0].Parameters[i].SourceIndex].Name);

                    for (int realIndex = 0; realIndex < edRmt2.RealParameterNames.Count; realIndex++)
                    {
                        string edArgName = CacheContext.StringTable.GetString(edRmt2.RealParameterNames[realIndex].Name);

                        if (blamArgName == edArgName)
                        {
                            finalRm.ShaderProperties[0].Parameters[i].SourceIndex = (byte)realIndex;
                            break;
                        }
                    }
                }
                int vertexEndOffset = parameterTable.RealVertex.Offset + parameterTable.RealVertex.Count;
                for (int i = parameterTable.RealVertex.Offset; i < vertexEndOffset; i++)
                {
                    string blamArgName = BlamCache.StringTable.GetString(bmRmt2.RealParameterNames[finalRm.ShaderProperties[0].Parameters[i].SourceIndex].Name);

                    for (int realIndex = 0; realIndex < edRmt2.RealParameterNames.Count; realIndex++)
                    {
                        string edArgName = CacheContext.StringTable.GetString(edRmt2.RealParameterNames[realIndex].Name);

                        if (blamArgName == edArgName)
                        {
                            finalRm.ShaderProperties[0].Parameters[i].SourceIndex = (byte)realIndex;
                            break;
                        }
                    }
                }
            }
        }

        private RenderMethod FixAnimationProperties(Stream cacheStream, Stream blamCacheStream, GameCacheHaloOnlineBase CacheContext, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2, string blamTagName, bool isMs30)
        {
            // finalRm is a H3 rendermethod with ported bitmaps, 
            if (finalRm.ShaderProperties[0].Functions.Count == 0)
                return finalRm;

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

            var RegistersList = new Dictionary<int, int>();

            // match pixl registers
            foreach (var xboxShader in bmPixl.Shaders)
                foreach (var xboxParameter in xboxShader.XboxParameters)
                {
                    if (RegistersList.Keys.Contains(xboxParameter.RegisterIndex))
                        continue;

                    var xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                    // loop pc params, find and match register
                    foreach (var pcShader in edPixl.Shaders)
                        foreach (var pcParameter in pcShader.PCParameters)
                        {
                            var pcParameterName = CacheContext.StringTable.GetString(pcParameter.ParameterName);

                            if (pcParameterName == xboxParameterName && xboxParameter.RegisterType == pcParameter.RegisterType && !RegistersList.ContainsKey(xboxParameter.RegisterIndex))
                                RegistersList.Add(xboxParameter.RegisterIndex, pcParameter.RegisterIndex);
                        }
                }

            // match glvs registers
            CachedTag blamRmdfTag = null;
            CachedTag edRmdfTag = null;

            string blamRmdfName = finalRm.BaseRenderMethod.Name;
            if (isMs30) // remove "ms30\\" from the name
                blamRmdfName = finalRm.BaseRenderMethod.Name.Remove(0, 5);

            if (BlamCache.TryGetTag(blamRmdfName + ".rmdf", out blamRmdfTag) && CacheContext.TryGetTag(finalRm.BaseRenderMethod.Name + ".rmdf", out edRmdfTag))
            {
                var bmRmdf = BlamCache.Deserialize<RenderMethodDefinition>(blamCacheStream, blamRmdfTag);
                var edRmdf = CacheContext.Deserialize<RenderMethodDefinition>(cacheStream, edRmdfTag);

                if (bmRmdf.GlobalVertexShader != null && edRmdf.GlobalVertexShader != null)
                {
                    var bmGlvs = BlamCache.Deserialize<GlobalVertexShader>(blamCacheStream, bmRmdf.GlobalVertexShader);
                    var edGlvs = CacheContext.Deserialize<GlobalVertexShader>(cacheStream, edRmdf.GlobalVertexShader);

                    foreach (var xboxShader in bmGlvs.Shaders)
                        foreach (var xboxParameter in xboxShader.XboxParameters)
                        {
                            if (RegistersList.Keys.Contains(xboxParameter.RegisterIndex))
                                continue;

                            var xboxParameterName = BlamCache.StringTable.GetString(xboxParameter.ParameterName);

                            // loop pc params, find and match register
                            foreach (var pcShader in edGlvs.Shaders)
                                foreach (var pcParameter in pcShader.PCParameters)
                                {
                                    var pcParameterName = CacheContext.StringTable.GetString(pcParameter.ParameterName);

                                    if (pcParameterName == xboxParameterName && xboxParameter.RegisterType == pcParameter.RegisterType && !RegistersList.ContainsKey(xboxParameter.RegisterIndex))
                                        RegistersList.Add(xboxParameter.RegisterIndex, pcParameter.RegisterIndex);
                                }
                        }
                }
            }

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
            foreach (var a in finalRm.ShaderProperties[0].EntryPoints)
            {
                DrawModeIndex++;

                // These are not modes. This is an indireciton table of packed 10_6 shorts
                // from RMT2 ShaderDrawmodes to RegisterOffsets
                // register_offset = ShaderDrawmodes[current_drawmode].Offset
                var drawmodeRegisterOffset = (int)a.Offset;
                var drawmodeRegisterCount = (int)a.Count;


                var ArgumentMappingsIndexSampler = (byte)finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].Texture.Offset;
                var ArgumentMappingsCountSampler = finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].Texture.Count;
                var ArgumentMappingsIndexUnknown = (byte)finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].RealVertex.Offset;
                var ArgumentMappingsCountUnknown = finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].RealVertex.Count;
                var ArgumentMappingsIndexVector = (byte)finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].RealPixel.Offset;
                var ArgumentMappingsCountVector = finalRm.ShaderProperties[0].ParameterTables[drawmodeRegisterOffset].RealPixel.Count;
                var ArgumentMappings = new List<ArgumentMapping>();

                for (int j = 0; j < ArgumentMappingsCountSampler; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexSampler + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexSampler + j].SourceIndex, // i don't think i can use it to match stuf
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexSampler + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Sampler,
                        ShaderIndex = drawmodeRegisterOffset,
                        // WARNING i think drawmodes in rm aren't the same as in pixl, because rm drawmodes can point to a global shader .
                        // say rm.drawmodes[17]'s value is 13, pixl.drawmodes[17] would typically be 12
                    });
                }

                for (int j = 0; j < ArgumentMappingsCountUnknown; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexUnknown + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexUnknown + j].SourceIndex,
                        ArgumentMappingsTagblockIndex = ArgumentMappingsIndexUnknown + j,
                        RegisterType = TagTool.Shaders.ShaderParameter.RType.Vector,
                        ShaderIndex = drawmodeRegisterOffset,
                        // it's something else, uses a global shader or some shit, one water shader pointed to a vtsh in rasg, but not in H3, maybe coincidence
                        // yeah guaranteed rmdf's glvs or rasg shaders
                    });
                }

                for (int j = 0; j < ArgumentMappingsCountVector; j++)
                {
                    ArgumentMappings.Add(new ArgumentMapping
                    {
                        RegisterIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexVector + j].RegisterIndex,
                        ArgumentIndex = finalRm.ShaderProperties[0].Parameters[ArgumentMappingsIndexVector + j].SourceIndex,
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

            // Now that we know which register is what for each drawmode, find its halo online equivalent register indexes based on register name.
            // This is where it gets tricky because drawmodes count changed in HO. 
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
                            if (!RegistersList.ContainsKey(b.RegisterIndex))
                                RegistersList.Add(b.RegisterIndex, c.RegisterIndex);

                            b.EDRegisterIndex = c.RegisterIndex;
                        }
                    }
                }
            }

            // Store registers, in case they are set to -1 and are lost
            List<short> priorRegisters = new List<short>();
            foreach (var parameter in finalRm.ShaderProperties[0].Parameters)
                priorRegisters.Add(parameter.RegisterIndex);

            // Set new registers
            foreach (var a in bmDrawmodesFunctions)
            {
                if (a.Value.Unknown3Count == 0)
                    continue;

                foreach (var b in a.Value.ArgumentMappings)
                {
                    finalRm.ShaderProperties[0].Parameters[b.ArgumentMappingsTagblockIndex].RegisterIndex = (short)b.EDRegisterIndex;
                }
            }

            // replace bm registers with ed registers if the above failed
            // TODO: properly fix the above code so this isnt needed (this will work fine for now tho)
            for (int i = 0; i < finalRm.ShaderProperties[0].Parameters.Count; i++)
            {
                if (finalRm.ShaderProperties[0].Parameters[i].RegisterIndex == -1)
                {
                    int bmRegister = (int)priorRegisters[i];

                    if (!RegistersList.TryGetValue(bmRegister, out int newRegister))
                    {
                        Console.WriteLine($"WARNING: Could not match blam register \"{bmRegister}\"");
                    }
                    else
                        finalRm.ShaderProperties[0].Parameters[i].RegisterIndex = (short)newRegister;
                }
            }

            //
            // Gather all register indices from relevant pixl and glvs
            //

            var validEdRegisters = new List<int>();

            foreach (var a in edPixl.Shaders)
                foreach (var b in a.PCParameters)
                    if (!validEdRegisters.Contains(b.RegisterIndex))
                        validEdRegisters.Add(b.RegisterIndex);

            if (edRmdfTag != null)
            {
                var edRmdf = CacheContext.Deserialize<RenderMethodDefinition>(cacheStream, edRmdfTag);
                if (edRmdf.GlobalVertexShader != null)
                {
                    var edGlvs = CacheContext.Deserialize<GlobalVertexShader>(cacheStream, edRmdf.GlobalVertexShader);

                    foreach (var pcShader in edGlvs.Shaders)
                        foreach (var pcParameter in pcShader.PCParameters)
                            if (!validEdRegisters.Contains(pcParameter.RegisterIndex))
                                validEdRegisters.Add(pcParameter.RegisterIndex);
                }
            }

            //
            // Check that all register indices are valid 
            //

            foreach (var a in finalRm.ShaderProperties[0].Parameters)
            {
                if (!validEdRegisters.Contains(a.RegisterIndex))
                {
                    //Console.WriteLine($"INVALID REGISTER \"{a.RegisterIndex}\" IN TAG {blamTagName}!");

                    // this code is only reached if an invalid register was found - this throws off the indices for other animations
                    // TODO: add system to preserve other animations

                    finalRm.ShaderProperties[0].EntryPoints = new List<RenderMethodTemplate.PackedInteger_10_6>();
                    finalRm.ShaderProperties[0].ParameterTables = new List<ParameterTable>();
                    finalRm.ShaderProperties[0].Parameters = new List<ParameterMapping>();
                    finalRm.ShaderProperties[0].Functions = new List<ShaderFunction>();
                    foreach (var textureConstant in finalRm.ShaderProperties[0].TextureConstants)
                        textureConstant.Functions.Integer = 0;
                    break;
                }
            }

            return finalRm;
        }

        private TextureConstant GetDefaultTextureConstant(string parameter, ShaderMatcherNew.Rmt2Descriptor rmt2Descriptor, Dictionary<StringId, CachedTag> optionBitmaps)
        {
            TextureConstant textureConstant = new TextureConstant { XFormArgumentIndex = -1 };

            if (rmt2Descriptor.Type == "particle") // not sure what this is but all prt3 have it
                textureConstant.SamplerFlags = 17;

            // hardcoded because the default bitmaps for these parameters produces bad results
            switch (parameter)
            {
                case "alpha_test_map": // the default bitmap for this puts a transparent "ALPHA" all over the shader
                    if (rmt2Descriptor.IsMs30)
                        textureConstant.Bitmap = CacheContext.GetTag(@"ms30\shaders\default_bitmaps\bitmaps\default_detail.bitm");
                    else
                        textureConstant.Bitmap = CacheContext.GetTag(@"shaders\default_bitmaps\bitmaps\default_detail.bitm");
                    return textureConstant;
            }

            // get default bitmap from dictionary
            if (!optionBitmaps.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out textureConstant.Bitmap) || textureConstant.Bitmap == null)
            {
                // fallback
                Console.WriteLine($"WARNING: Sampler parameter \"{parameter}\" has no default bitmap");

                // null bitmap causes bad rendering, use default_detail in these cases
                if (rmt2Descriptor.IsMs30)
                    textureConstant.Bitmap = CacheContext.GetTag(@"ms30\shaders\default_bitmaps\bitmaps\default_detail.bitm");
                else
                    textureConstant.Bitmap = CacheContext.GetTag(@"shaders\default_bitmaps\bitmaps\default_detail.bitm");
            }

            return textureConstant;
        }

        private RealConstant GetDefaultRealConstant(string parameter, string type, List<string> methodNames, Dictionary<StringId, RenderMethodOption.OptionBlock> optionBlocks)
        {
            if (!optionBlocks.TryGetValue(CacheContext.StringTable.GetStringId(parameter), out var optionBlock))
            {
                // TODO: verify these -- some parameters are method names???
                if (!methodNames.Contains(parameter))
                    Console.WriteLine($"WARNING: No type found for {type} parameter \"{parameter}\"");

                // just 1.0f for now (see above)
                optionBlock = new RenderMethodOption.OptionBlock
                {
                    Type = RenderMethodOption.OptionBlock.OptionDataType.Float,
                    DefaultFloatArgument = 1.0f
                };
            }

            switch (optionBlock.Type)
            {
                case RenderMethodOption.OptionBlock.OptionDataType.Sampler:
                    return new RealConstant { Arg0 = 1.0f, Arg1 = 1.0f, Arg2 = 0.0f, Arg3 = 0.0f };

                case RenderMethodOption.OptionBlock.OptionDataType.Float:
                    return new RealConstant { Arg0 = optionBlock.DefaultFloatArgument, Arg1 = optionBlock.DefaultFloatArgument, Arg2 = optionBlock.DefaultFloatArgument, Arg3 = optionBlock.DefaultFloatArgument };

                // convert ARGB to RealRGBA
                case RenderMethodOption.OptionBlock.OptionDataType.Float4:
                case RenderMethodOption.OptionBlock.OptionDataType.IntegerColor:
                    return new RealConstant { Arg0 = optionBlock.DefaultColor.Red / 255.0f, Arg1 = optionBlock.DefaultColor.Blue / 255.0f, Arg2 = optionBlock.DefaultColor.Green / 255.0f, Arg3 = optionBlock.DefaultColor.Alpha / 255.0f };

                default:
                    return new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
            }
        }

        private List<RenderMethodDefinitionOptionIndex> BuildRenderMethodOptionIndices(ShaderMatcherNew.Rmt2Descriptor rmt2Descriptor)
        {
            List<RenderMethodDefinitionOptionIndex> newRmIndices = new List<RenderMethodDefinitionOptionIndex>();

            foreach (var option in rmt2Descriptor.Options)
            {
                RenderMethodDefinitionOptionIndex optionIndex = new RenderMethodDefinitionOptionIndex();
                optionIndex.OptionIndex = option;
                newRmIndices.Add(optionIndex);
            }

            return newRmIndices;
        }

        private bool OptionChanged(string rmt2Type, string methodName, out int edOptionIndex, ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor, ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor, RenderMethodDefinition rmdf)
        {
            //
            // This compares the original rmt2 with the matched rmt2 from the matching process and checks if the specified option has changed.
            //

            edOptionIndex = -1;

            if (edRmt2Descriptor.Type == rmt2Type)
            {
                for (int i = 0; i < rmdf.Methods.Count; i++)
                {
                    // find name, and compare the options at that index. maybe need to loop blam rmdf too?
                    if (CacheContext.StringTable.GetString(rmdf.Methods[i].Type) == methodName && blamRmt2Descriptor.Options[i] != edRmt2Descriptor.Options[i])
                    {
                        edOptionIndex = i;
                        return true;
                    }
                }
            }

            return false;
        }

        private void ApplyOptionFixups(List<RealConstant> realConstants, ShaderMatcherNew.Rmt2Descriptor blamRmt2Descriptor, ShaderMatcherNew.Rmt2Descriptor edRmt2Descriptor, RenderMethodTemplate edRmt2, RenderMethodDefinition rmdf)
        {
            //
            // This applies manual option->option fixups for matching. This concept should not be used for rm creation/generation.
            //

            // special case, has no options.
            if (blamRmt2Descriptor.Type == "black" || edRmt2Descriptor.Type == "black")
                return;

            // TODO: cleaner way of doing this

            int optionIndex = -1;

            if (OptionChanged("halogram", "overlay", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if overlay is new to the shader, set its intensity to 0
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] != 0)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "overlay_intensity")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                            break;
                        }
                }
            }
            if (OptionChanged("water", "bankalpha", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if the water shader uses bankalpha.depth when it didnt previously, set the depth to 0 to prevent transparent puddles
                if (blamRmt2Descriptor.Options[optionIndex] != 1 && edRmt2Descriptor.Options[optionIndex] == 1)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "bankalpha_infuence_depth")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                            break;
                        }
                }
            }
            if (OptionChanged("shader", "parallax", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if parallax is new to the shader, set height map scale to 0
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] != 0)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "height_scale")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                            break;
                        }
                }
            }
            if (OptionChanged("shader", "self_illumination", out optionIndex, blamRmt2Descriptor, edRmt2Descriptor, rmdf))
            {
                // if self_illumination is new to the shader, set self_illum_intensity to 0
                if (blamRmt2Descriptor.Options[optionIndex] == 0 && edRmt2Descriptor.Options[optionIndex] != 0)
                {
                    for (int i = 0; i < edRmt2.RealParameterNames.Count; i++)
                        if (CacheContext.StringTable.GetString(edRmt2.RealParameterNames[i].Name) == "self_illum_intensity")
                        {
                            realConstants[i] = new RealConstant { Arg0 = 0.0f, Arg1 = 0.0f, Arg2 = 0.0f, Arg3 = 0.0f };
                            break;
                        }
                }
            }
        }
    }
}