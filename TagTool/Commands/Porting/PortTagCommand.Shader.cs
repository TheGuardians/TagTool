using TagTool.Cache;
using TagTool.Common;
using TagTool.Geometry;
using TagTool.Havok;
using TagTool.IO;
using TagTool.Serialization;
using TagTool.Tags.Definitions;
using TagTool.Tags.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TagTool.Tags;
using TagTool.Shaders;
using System.Diagnostics;
using System.Text;
using TagTool.ShaderGenerator;
using TagTool.ShaderGenerator.Types;

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {

        private GlobalPixelShader ConvertGlobalPixelShader(GlobalPixelShader glps)
        {
            /*
            Directory.CreateDirectory(@"Temp");

            if (!File.Exists(@"Tools\xsd.exe"))
            {
                Console.WriteLine("Missing tools, please install xsd.exe before porting shaders.");
                return glps;
            }

            foreach (var shader in glps.Shaders)
            {
                var xbox_shader_reference = shader?.XboxShaderReference;
                var shader_data = xbox_shader_reference?.ShaderData;
                if (shader_data == null || shader_data.Length == 0) continue;

                string tempSHADER = @"Temp\permutation.shader";
                string tempSHADERUPDB = @"Temp\permutation.shader.updb";

                if (File.Exists(tempSHADER)) File.Delete(tempSHADER);
                if (File.Exists(tempSHADERUPDB)) File.Delete(tempSHADERUPDB);

                using (EndianWriter output = new EndianWriter(File.OpenWrite(tempSHADER), EndianFormat.BigEndian))
                {
                    output.WriteBlock(shader_data);
                }

                using (EndianWriter output = new EndianWriter(File.OpenWrite(tempSHADERUPDB), EndianFormat.BigEndian))
                {
                    output.WriteBlock(xbox_shader_reference.DebugData);
                }

                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = @"Tools\xsd.exe",
                        Arguments = "/rawps permutation.shader",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp")
                    }
                };
                process.Start();

                //* Read the output (or the error)
                string proc_output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(proc_output);
                string err = process.StandardError.ReadToEnd();
                Console.WriteLine(err);

                process.WaitForExit();

                if (!String.IsNullOrWhiteSpace(err))
                {
                    continue;
                }

                Console.WriteLine("written shader binary for glps");

            }
            */
            //add conversion code when ready
            return glps;
        }

        private GlobalVertexShader ConvertGlobalVertexShader(GlobalVertexShader glvs)
        {
            //throw new NotImplementedException();

            /*foreach (var shader in glvs.Shaders)
            {
                var shader_parser = new XboxShaderParser(glvs, shader, CacheContext);

                if (!shader_parser.IsValid) continue;

                Console.WriteLine("written shader binary for glps");

            }

            //add conversion code when ready */
            return glvs;
        }

        private PixelShader ConvertPixelShader(PixelShader pixl, CacheFile.IndexItem blamTag)
        {
            var name = blamTag.Filename;

            string[] nameParts = name.Split('\\');
            string arguments = nameParts[nameParts.Length - 1];
            arguments = arguments.Substring(1, arguments.Length - 1);
            string type = nameParts[nameParts.Length - 2];

            string[] shaderArgs = arguments.Split('_');

            Int32[] shader_args;
            try { shader_args = Array.ConvertAll(shaderArgs, Int32.Parse); }
            catch { Console.WriteLine("Invalid shader arguments! (could not parse to Int32[].)"); return null; }

            for (int i = 0; i < pixl.Shaders.Count; i++)
            {
                //
                // Parse Blam tag name to extract type and arguments
                //

                var shader = pixl.Shaders[i];

                try
                {
                    if(i != 0)
                    {
                        throw new Exception($"Unsupported shader index {i}.");
                    }

                    var drawmode = TemplateShaderGenerator.Drawmode.Default;

                    ShaderGeneratorResult shader_gen_result;
                    switch (type)
                    {
                        case "beam_templates":
                            shader_gen_result = new BeamTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "contrail_templates":
                            shader_gen_result = new ContrailTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "cortana_templates":
                            shader_gen_result = new CortanaTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "decal_templates":
                            shader_gen_result = new DecalTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "foliage_templates":
                            shader_gen_result = new FoliageTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "halogram_templates":
                            shader_gen_result = new HalogramTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "light_volume_templates":
                            shader_gen_result = new LightVolumeTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "particle_templates":
                            shader_gen_result = new ParticleTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "shader_templates":
                            shader_gen_result = new ShaderTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "terrain_templates":
                            shader_gen_result = new TerrainTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        case "water_templates":
                            shader_gen_result = new WaterTemplateShaderGenerator(CacheContext, drawmode, shader_args)?.Generate();
                            break;

                        default:
                            Console.WriteLine($"{type} is not implemented");
                            return null;
                    }

                    if (shader_gen_result == null) return null;

                    shader.PCShaderBytecode = shader_gen_result.ByteCode;
                    shader.PCParameters = shader_gen_result.Parameters;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine($"Something happened when converting  {type} {arguments} index: {i}. Setting shader bytecote and registers to null");
                    shader.PCShaderBytecode = null;
                    shader.PCParameters = new List<ShaderParameter>();
                }
            }

            return pixl;
        }

        private VertexShader ConvertVertexShader(VertexShader vtsh)
        {
            //add conversion code when ready
            return vtsh;
        }

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

        private RenderMethod ConvertRenderMethodGenerated(Stream cacheStream, RenderMethod renderMethod, string blamTagName)
        {
            //
            // Remove shader function (overlays) until they are fixed
            //

            renderMethod.ShaderProperties[0].ArgumentMappings = new List<RenderMethod.ShaderProperty.ArgumentMapping>();
            renderMethod.ShaderProperties[0].Functions = new List<RenderMethod.ShaderProperty.FunctionBlock>();

            return renderMethod;
        }

        private static bool debugUseEDFunctions;
        private static bool dDraw = false;
        private static int pRmt2 = 0;
        private static List<string> bmMaps;
        private static List<string> bmArgs;
        private static List<string> edMaps;
        private static List<string> edArgs;

        private RenderMethod ConvertRenderMethod(Stream cacheStream, RenderMethod finalRm, string blamTagName)
        {
            // finalRm.ShaderProperties[0].ShaderMaps are all ported bitmaps
            // finalRm.BaseRenderMethod is a H3 tag
            // finalRm.ShaderProperties[0].Template is a H3 tag

            // TODO hardcode shader values such as argument changes for specific shaders
            bmMaps = new List<string>();
            bmArgs = new List<string>();
            edMaps = new List<string>();
            edArgs = new List<string>();

            // Reset rmt2 preset
            pRmt2 = 0;

            // Make a template of ShaderProperty, with the correct bitmaps and arguments counts. 
            var newShaderProperty = new RenderMethod.ShaderProperty
            {
                ShaderMaps = new List<RenderMethod.ShaderProperty.ShaderMap>(),
                Arguments = new List<RenderMethod.ShaderProperty.Argument>()
            };

            // Name rmt2 tags if one of them is not named
            foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
            {
                if (!CacheContext.TagNames.ContainsKey(tag.Index))
                {
                    NameRmt2();
                    CacheContext.SaveTagNames();
                    break;
                }
            }

            /* Name rmdf tags if one of them is not named
            foreach (var tag in CacheContext.TagCache.Index.FindAllInGroup("rmdf"))
            {
                if (!CacheContext.TagNames.ContainsKey(tag.Index))
                {
                    // cheap n dirty, but there's only a few global tags that should never change their tag index
                    CacheContext.TagNames[0x02A5] = "shaders\\particle";
                    CacheContext.TagNames[0x033F] = "shaders\\shader";
                    CacheContext.TagNames[0x03AA] = "shaders\\decal";
                    CacheContext.TagNames[0x052A] = "shaders\\contrail";
                    CacheContext.TagNames[0x0595] = "shaders\\light_volume";
                    CacheContext.TagNames[0x0E86] = "shaders\\halogram";
                    CacheContext.TagNames[0x18BA] = "shaders\\screen";
                    CacheContext.TagNames[0x18CA] = "shaders\\beam";
                    CacheContext.TagNames[0x2F49] = "shaders\\terrain";
                    CacheContext.TagNames[0x3571] = "shaders\\foliage";
                    CacheContext.TagNames[0x357B] = "shaders\\water";
                    CacheContext.TagNames[0x4458] = "shaders\\shader";
                    CacheContext.TagNames[0x44DE] = "shaders\\terrain";
                    CacheContext.TagNames[0x457A] = "shaders\\foliage";
                    CacheContext.TagNames[0x466B] = "shaders\\decal";
                    CacheContext.TagNames[0x4740] = "shaders\\particle";
                    CacheContext.TagNames[0x4753] = "shaders\\beam";
                    CacheContext.TagNames[0x4762] = "shaders\\light_volume";
                    CacheContext.TagNames[0x5013] = "shaders\\water";
                    CacheContext.TagNames[0x50CC] = "shaders\\halogram";
                    CacheContext.TagNames[0x52A9] = "shaders\\custom";
                    CacheContext.SaveTagNames();
                    break;
                }
            }*/

            // Loop only once trough all ED rmt2 tags and store them globally, string lists of their bitmaps and arguments
            if (CacheContext.Rmt2TagsInfo.Count == 0)
            {
                Console.WriteLine($"Shader matching info: collecting all rmt2 tags info...");
                GetRmt2Info(cacheStream, CacheContext);
            }

            // Get a simple list of bitmaps and arguments names
            var bmRmt2Instance = BlamCache.IndexItems.Find(x => x.ID == finalRm.ShaderProperties[0].Template.Index);
            var blamContext = new CacheSerializationContext(BlamCache, bmRmt2Instance);
            var bmRmt2 = BlamCache.Deserializer.Deserialize<RenderMethodTemplate>(blamContext);

            // Get a simple list of H3 bitmaps and arguments names
            foreach (var a in bmRmt2.ShaderMaps)
                bmMaps.Add(BlamCache.Strings.GetItemByID(a.Name.Index));
            foreach (var a in bmRmt2.Arguments)
                bmArgs.Add(BlamCache.Strings.GetItemByID(a.Name.Index));

            // Find a HO equivalent rmt2
            var edRmt2Instance = FixRmt2Reference(CacheContext, bmRmt2Instance, bmRmt2);

            if (edRmt2Instance == null) // should never happen
            {
                var shaderInvalid = ArgumentParser.ParseTagName(CacheContext, $"shaders\\invalid.rmsh");

                if (shaderInvalid == null)
                    shaderInvalid = CacheContext.GetTagInstance<Shader>(@"objects\characters\masterchief\shaders\mp_masterchief_rubber");

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

                newShaderProperty.ShaderMaps.Add(new RenderMethod.ShaderProperty.ShaderMap { Bitmap = CacheContext.GetTagInstance<Bitmap>(newBitmap) });
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
                    a.Bitmap = CacheContext.GetTagInstance<Bitmap>(GetDefaultBitmapTag(edMaps[(int)finalRm.ShaderProperties[0].ShaderMaps.IndexOf(a)]));

            return finalRm;
        }

        private static void GetRmt2Info(Stream cacheStream, GameCacheContext cacheContext)
        {
            if (cacheContext.Rmt2TagsInfo.Count != 0)
                return;

            foreach (var instance in cacheContext.TagCache.Index.FindAllInGroup("rmt2"))
            {
                var edContext = new TagSerializationContext(cacheStream, cacheContext, instance);
                var edRmt2 = cacheContext.Deserializer.Deserialize<RenderMethodTemplateFast>(edContext);

                var edBitmaps_ = new List<string>();
                var edArgs_ = new List<string>();

                foreach (var ShaderMap in edRmt2.ShaderMaps)
                    edBitmaps_.Add(cacheContext.StringIdCache.GetString(ShaderMap.Name));

                foreach (var Argument in edRmt2.Arguments)
                    edArgs_.Add(cacheContext.StringIdCache.GetString(Argument.Name));

                cacheContext.Rmt2TagsInfo.Add(instance.Index, new List<List<string>> { edBitmaps_, edArgs_ });
            }
        }

        private List<ShaderTemplateItem> CollectRmt2Info(CacheFile.IndexItem bmRmt2Instance)
        {
            var edRmt2BestStats = new List<ShaderTemplateItem>();

            // loop trough all rmt2 and find the closest
            foreach (var edRmt2_ in CacheContext.Rmt2TagsInfo)
            {
                var rmt2Type = bmRmt2Instance.Filename.Split("\\".ToArray())[1];

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

        private CachedTagInstance FindEquivalentRmt2(CacheFile.IndexItem bmRmt2Instance, RenderMethodTemplate bmRmt2)
        {
            // Find similar shaders by finding tags with as many common bitmaps and arguments as possible.
            var edRmt2Temp = new List<ShaderTemplateItem>();

            // Make a new dictionary with rmt2 of the same shader type
            var edRmt2BestStats = new List<ShaderTemplateItem>();

            edRmt2BestStats = CollectRmt2Info(bmRmt2Instance);

            // rmt2 tagnames have a bunch of values, they're tagblock indexes in rmdf methods.ShaderOptions
            foreach (var d in edRmt2BestStats)
            {
                var edSplit = CacheContext.TagNames[d.rmt2TagIndex].Split("\\".ToCharArray());
                var edType = edSplit[1];
                var edRmdfValues = edSplit[2].Split("_".ToCharArray()).ToList();
                edRmdfValues.RemoveAt(0);

                var bmSplit = bmRmt2Instance.Filename.Split("\\".ToCharArray());
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
                var tempRmt2 = ArgumentParser.ParseTagSpecifier(CacheContext, @"shaders\shader_templates\_0_0_0_0_0_0_0_0_0_0_0.rmt2");

                if (tempRmt2 != null)
                    return tempRmt2;
                else return CacheContext.TagCache.Index.FindFirstInGroup("rmt2");
            }

            return CacheContext.GetTag(edRmt2BestStatsSorted.Last().rmt2TagIndex);
        }

        private class Arguments
        {
            public float Arg1;
            public float Arg2;
            public float Arg3;
            public float Arg4;
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

            res.Arg1 = val[0];
            res.Arg2 = val[1];
            res.Arg3 = val[2];
            res.Arg4 = val[3];

            return res;
        }

        private static string GetDefaultBitmapTag(string type)
        {
            switch (type)
            {
                case "base_map":
                case "base_map_m_0":
                case "base_map_m_1":
                case "base_map_m_2":
                case "detail_map":
                case "detail_map2":
                case "detail_map3":
                case "detail_map_a":
                case "detail_map_m_0":
                case "detail_map_m_1":
                case "detail_map_m_2":
                case "detail_map_m_3":
                case "overlay_detail_map":
                case "detail_map_overlay":
                case "alpha_map":
                case "blend_map":
                case "meter_map":
                case "subsurface_map":
                case "self_illum_detail_map":
                case "warp_map":
                    return @"shaders\default_bitmaps\bitmaps\gray_50_percent";

                case "change_color_map":
                case "material_texture":
                case "noise_map_a":
                case "noise_map_b":
                case "overlay_map":
                case "bump_detail_mask_map":
                case "chameleon_mask_map":
                case "environment_map":
                case "specular_map":
                case "specular_mask_texture":
                case "transparence_map":
                    return @"shaders\default_bitmaps\bitmaps\color_white";

                case "color_mask_map":
                    return @"shaders\default_bitmaps\bitmaps\reference_grids";

                case "bump_map":
                case "bump_map_m_0":
                case "bump_map_m_1":
                case "bump_map_m_2":
                case "bump_map_m_3":
                case "detail_bump_m_0":
                case "detail_bump_m_1":
                case "detail_bump_m_2":
                case "detail_bump_m_3":
                case "bump_detail_map":
                case "alpha_test_map":
                case "height_map":
                case "overlay_multiply_map":
                    return @"shaders\default_bitmaps\bitmaps\default_vector"; // WARNING engine's toast, anything bump map related is busted

                case "self_illum_map":
                    return @"shaders\default_bitmaps\bitmaps\default_alpha_test";

                case "detail_mask_a":
                    return @"shaders\default_bitmaps\bitmaps\color_red";

                case "alpha_mask_map":
                    return @"shaders\default_bitmaps\bitmaps\alpha_white";

                default:
                    return @"shaders\default_bitmaps\bitmaps\gray_50_percent";
                    // throw new NotImplementedException($"Useless exception. Bitmaps table requires an update for {type}.");
            }
        }
        
        private CachedTagInstance FixRmt2Reference(GameCacheContext CacheContext, CacheFile.IndexItem bmRmt2Instance, RenderMethodTemplate bmRmt2)
        {
            // Find existing rmt2 tags
            // If tagnames are not fixed, ms30 tags have an additional _0 or _0_0. This shouldn't happen if the tags have proper names, so it's mostly to preserve compatibility with older tagnames
            var edRmt2Instances = new List<CachedTagInstance>();
            CachedTagInstance edRmt2Instance = null;

            foreach (var a in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
                if (CacheContext.TagNames.ContainsKey(a.Index))
                    if (CacheContext.TagNames[a.Index] == $"{bmRmt2Instance.Filename}")
                        edRmt2Instances.Add(a);

            if (edRmt2Instances.Count == 0)
                foreach (var a in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
                    if (CacheContext.TagNames.ContainsKey(a.Index))
                        if (CacheContext.TagNames[a.Index] == $"{bmRmt2Instance.Filename}_0") // legacy
                            edRmt2Instances.Add(a);

            if (edRmt2Instances.Count == 0)
                foreach (var a in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
                    if (CacheContext.TagNames.ContainsKey(a.Index))
                        if (CacheContext.TagNames[a.Index] == $"{bmRmt2Instance.Filename}_0_0") // legacy
                            edRmt2Instances.Add(a);

            if (edRmt2Instances.Count == 0) // if no tagname matches, find rmt2 tags based on the most common values in the name
                edRmt2Instance = FindEquivalentRmt2(bmRmt2Instance, bmRmt2);
            else
                edRmt2Instance = edRmt2Instances.First();

            return edRmt2Instance;
        }

        public bool NameRmt2()
        {
            var validShaders = new List<string> { "rmsh", "rmtr", "rmhg", "rmfl", "rmcs", "rmss", "rmd ", "rmw ", "rmzo", "ltvl", "prt3", "beam", "decs", "cntl", "rmzo", "rmct", "rmbk" };
            var newlyNamedRmt2 = new List<int>();
            var type = "invalid";
            var rmt2Instance = -1;

            // Unname rmt2 tags
            foreach (var edInstance in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
                CacheContext.TagNames[edInstance.Index] = "blank";

            foreach (var edInstance in CacheContext.TagCache.Index.NonNull())
            {
                object rm = null;
                RenderMethod renderMethod = null;

                // ignore tag groups not in validShaders
                if (!validShaders.Contains(edInstance.Group.Tag.ToString()))
                    continue;

                // Console.WriteLine($"Checking 0x{edInstance:x4} {edInstance.Group.Tag.ToString()}");

                // Get the tagname type per tag group
                switch (edInstance.Group.Tag.ToString())
                {
                    case "rmsh": type = "shader"; break;
                    case "rmtr": type = "terrain"; break;
                    case "rmhg": type = "halogram"; break;
                    case "rmfl": type = "foliage"; break;
                    case "rmss": type = "screen"; break;
                    case "rmcs": type = "custom"; break;
                    case "prt3": type = "particle"; break;
                    case "beam": type = "beam"; break;
                    case "cntl": type = "contrail"; break;
                    case "decs": type = "decal"; break;
                    case "ltvl": type = "light_volume"; break;
                    case "rmct": type = "cortana"; break;
                    case "rmbk": type = "black"; break;
                    case "rmzo": type = "zonly"; break;
                    case "rmd ": type = "decal"; break;
                    case "rmw ": type = "water"; break;
                }

                switch (edInstance.Group.Tag.ToString())
                {
                    case "rmsh":
                    case "rmhg":
                    case "rmtr":
                    case "rmcs":
                    case "rmfl":
                    case "rmss":
                    case "rmct":
                    case "rmzo":
                    case "rmbk":
                    case "rmd ":
                    case "rmw ":
                        using (var cacheStream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                        {
                            var edContext = new TagSerializationContext(cacheStream, CacheContext, edInstance);
                            var rm2 = CacheContext.Deserializer.Deserialize<RenderMethodFast>(edContext);

                            renderMethod = new RenderMethod { Unknown = rm2.Unknown };

                            if (renderMethod.Unknown.Count == 0) // used to name the rmt2 tag
                                continue;
                        }

                        foreach (var a in edInstance.Dependencies)
                            if (CacheContext.GetTag(a).Group.ToString() == "rmt2")
                                rmt2Instance = CacheContext.GetTag(a).Index;

                        NameRmt2Part(type, renderMethod, edInstance, rmt2Instance, newlyNamedRmt2);

                        continue;

                    default:
                        break;
                }

                using (var cacheStream = CacheContext.TagCacheFile.Open(FileMode.Open, FileAccess.ReadWrite))
                {
                    var edContext = new TagSerializationContext(cacheStream, CacheContext, edInstance);
                    rm = CacheContext.Deserializer.Deserialize(new TagSerializationContext(cacheStream, CacheContext, edInstance), TagDefinition.Find(edInstance.Group.Tag));
                }

                switch (edInstance.Group.Tag.ToString())
                {
                    case "prt3": var e = (Particle)rm; NameRmt2Part(type, e.RenderMethod, edInstance, e.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "beam": var a = (BeamSystem)rm; foreach (var f in a.Beam) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "cntl": var b = (ContrailSystem)rm; foreach (var f in b.Contrail) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "decs": var c = (DecalSystem)rm; foreach (var f in c.DecalSystem2) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;
                    case "ltvl": var d = (LightVolumeSystem)rm; foreach (var f in d.LightVolume) NameRmt2Part(type, f.RenderMethod, edInstance, f.RenderMethod.ShaderProperties[0].Template.Index, newlyNamedRmt2); break;

                    default:
                        break;
                }
            }


            return true;
        }

        private void NameRmt2Part(string type, RenderMethod renderMethod, CachedTagInstance edInstance, int rmt2Instance, List<int> newlyNamedRmt2)
        {
            if (renderMethod.Unknown.Count == 0) // invalid shaders, most likely caused by ported shaders
                return;

            if (newlyNamedRmt2.Contains(rmt2Instance))
                return;
            else
                newlyNamedRmt2.Add(rmt2Instance);

            var newTagName = $"shaders\\{type}_templates\\";

            var rmdfRefValues = "";

            for (int i = 0; i < renderMethod.Unknown.Count; i++)
            {
                if (edInstance.Group.Tag.ToString() == "rmsh" && i > 9) // for better H3/ODST name matching
                    break;

                if (edInstance.Group.Tag.ToString() == "rmhg" && i > 6) // for better H3/ODST name matching
                    break;

                rmdfRefValues = $"{rmdfRefValues}_{renderMethod.Unknown[i].Unknown}";
            }

            newTagName = $"{newTagName}{rmdfRefValues}";

            CacheContext.TagNames[rmt2Instance] = newTagName;
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
            public int FunctionIndex;
            public int EDRegisterIndex = -1;
            public int ArgumentIndex;
            public int ArgumentMappingsTagblockIndex;
            public TagTool.Shaders.ShaderParameter.RType RegisterType;
        }

        private void FixRmdfTagRef(RenderMethod finalRm)
        {
            // Set rmdf
            var rmdfName = BlamCache.IndexItems.Find(x => x.ID == finalRm.BaseRenderMethod.Index).Filename;

            try
            {
                finalRm.BaseRenderMethod = CacheContext.GetTagInstance<RenderMethodDefinition>(rmdfName);
            }
            catch
            {
                finalRm.BaseRenderMethod = CacheContext.GetTagInstance<RenderMethodDefinition>(@"shaders\shader"); // all ms23 rmdf tags need to exist, using rmsh's rmdf for all rm's is a bad idea
            }
        }

        private RenderMethod FixFunctions(CacheFile blamCache, GameCacheContext CacheContext, Stream cacheStream, RenderMethod finalRm, RenderMethodTemplate edRmt2, RenderMethodTemplate bmRmt2)
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

            var blamContext = new CacheSerializationContext(blamCache, blamCache.IndexItems.Find(x => x.ID == bmRmt2.PixelShader.Index));
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

