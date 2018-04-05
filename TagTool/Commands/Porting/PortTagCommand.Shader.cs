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

namespace TagTool.Commands.Porting
{
    partial class PortTagCommand
    {

        private GlobalPixelShader ConvertGlobalPixelShader(GlobalPixelShader glps)
        {
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

            //add conversion code when ready
            return glps;
        }

        private GlobalVertexShader ConvertGlobalVertexShader(GlobalVertexShader glvs)
        {
            throw new NotImplementedException();

            /*foreach (var shader in glvs.Shaders)
            {
                var shader_parser = new XboxShaderParser(glvs, shader, CacheContext);

                if (!shader_parser.IsValid) continue;

                Console.WriteLine("written shader binary for glps");

            }

            //add conversion code when ready
            return glvs;*/
        }

        private PixelShader ConvertPixelShader(PixelShader pixl)
        {
            foreach (var shader in pixl.Shaders)
            {
                var shader_parser = new XboxShaderParser(pixl, shader, CacheContext);

                if (!shader_parser.IsValid) continue;

                Console.WriteLine(shader_parser.Disassemble());

#if !DEBUG
                try
                {
#endif
                    shader.PCShaderBytecode = shader_parser.ProcessShader();
                    shader.PCParameters = shader.XboxParameters;
                    shader.XboxShaderReference = null;

                    Console.WriteLine("written shader binary for glps");
                #if !DEBUG
                }
                catch(Exception e)
                {
                    Console.WriteLine("ConvertPixelShader Errors:");
                    Console.WriteLine(e.Message);
                }
#endif
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

        private static bool debugUseEDFunctions;
        private static int pRmt2 = 0;
        private static List<string> bmMaps;
        private static List<string> bmArgs;
        private static List<string> edMaps;
        private static List<string> edArgs;

        private RenderMethod ConvertRenderMethod(Stream cacheStream, RenderMethod bmRm, string blamTagName)
        {
            debugUseEDFunctions = false;
            pRmt2 = 0;

            // GetShaderPresets(CacheContext, blamTag.Filename);
            bmMaps = new List<string>();
            bmArgs = new List<string>();
            edMaps = new List<string>();
            edArgs = new List<string>();

            // Loop only once trough all ED rmt2 tags and store them globally, string lists of their bitmaps and arguments
            if (CacheContext.Rmt2TagsInfo.Count == 0)
            {
                Console.WriteLine($"Shader matching info: collecting all rmt2 tags info...");
                GetRmt2Info(cacheStream, CacheContext);

                Console.WriteLine($"Shader matching info: naming all rmt2 tags...");
                NameRmt2(CacheContext, cacheStream);
                CacheContext.SaveTagNames();
            }

            // Make a template of ShaderProperty, with the correct bitmaps and arguments counts. 
            var newShaderProperty = new RenderMethod.ShaderProperty
            {
                ShaderMaps = new List<RenderMethod.ShaderProperty.ShaderMap>(),
                Arguments = new List<RenderMethod.ShaderProperty.Argument>()
            };

            var bmRmt2Instance = BlamCache.IndexItems.Find(x => x.ID == bmRm.ShaderProperties[0].Template.Index);

            // Get a simple list of bitmaps and arguments names
            var blamContext = new CacheSerializationContext(BlamCache, bmRmt2Instance);
            var bmRmt2 = BlamCache.Deserializer.Deserialize<RenderMethodTemplate>(blamContext);

            foreach (var a in bmRmt2.ShaderMaps)
                bmMaps.Add(BlamCache.Strings.GetItemByID(a.Name.Index));
            foreach (var a in bmRmt2.Arguments)
                bmArgs.Add(BlamCache.Strings.GetItemByID(a.Name.Index));

            // Find existing rmt2 tags
            // If tagnames are not fixed, ms30 tags have an additional _0 or _0_0. This shouldn't happen if the tags have proper names, so it's mostly to preserve compatibility with older tagnames
            var edRmt2Instances = new List<CachedTagInstance>();
            CachedTagInstance edRmt2Instance = null;
            foreach (var a in CacheContext.TagCache.Index.FindAllInGroup("rmt2"))
            {
                if (CacheContext.TagNames.ContainsKey(a.Index))
                {
                    if (CacheContext.TagNames[a.Index] == $"{bmRmt2Instance.Filename}")
                        edRmt2Instances.Add(a);
                    if (CacheContext.TagNames[a.Index] == $"{bmRmt2Instance.Filename}_0")
                        edRmt2Instances.Add(a);
                    if (CacheContext.TagNames[a.Index] == $"{bmRmt2Instance.Filename}_0_0")
                        edRmt2Instances.Add(a);
                }
            }

            if (edRmt2Instances.Count == 0)
                edRmt2Instance = FindEquivalentRmt2(bmRmt2Instance);
            else
                edRmt2Instance = edRmt2Instances.First();

            if (edRmt2Instance == null) // should never happen
            {
                var shaderInvalid = ArgumentParser.ParseTagName(CacheContext, $"shaders\\invalid.rmsh");

                if (shaderInvalid == null)
                    shaderInvalid = CacheContext.GetTag(0x101F);

                var edContext2 = new TagSerializationContext(cacheStream, CacheContext, shaderInvalid);

                return CacheContext.Deserializer.Deserialize<Shader>(edContext2);
            }

            var edRmt2Tagname = CacheContext.TagNames.ContainsKey(edRmt2Instance.Index) ? CacheContext.TagNames[edRmt2Instance.Index] : $"0x{edRmt2Instance.Index:X4}";

            // To prevent a billion lines of bad code, let it find the rmt2 tag, and only replace it with a preset now
            GetShaderPresets(CacheContext, blamTagName);

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

            // Get a simple list of bitmaps and arguments names
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
                var newBitmap = DefaultBitmapsTags(a);
                if (!CacheContext.TagCache.Index.Contains(pRmt2))
                    newBitmap = 0x0343; // would only happen for removed shaders

                newShaderProperty.ShaderMaps.Add(new RenderMethod.ShaderProperty.ShaderMap
                {
                    Bitmap = CacheContext.GetTag(newBitmap)
                });
            }

            foreach (var a in edArgs)
                newShaderProperty.Arguments.Add(DefaultArgumentsValues(a));

            // Reorder blam bitmaps to match the HO rmt2 order
            // Reorder blam arguments to match the HO rmt2 order
            foreach (var eM in edMaps)
                foreach (var bM in bmMaps)
                    if (eM == bM)
                        newShaderProperty.ShaderMaps[edMaps.IndexOf(eM)] = bmRm.ShaderProperties[0].ShaderMaps[bmMaps.IndexOf(bM)];

            foreach (var eA in edArgs)
                foreach (var bA in bmArgs)
                    if (eA == bA)
                        newShaderProperty.Arguments[edArgs.IndexOf(eA)] = bmRm.ShaderProperties[0].Arguments[bmArgs.IndexOf(bA)];

            FixTagblocks(CacheContext, cacheStream, edRmt2Instance, bmRm);

            bmRm.ShaderProperties[0].Template = edRmt2Instance;
            bmRm.ShaderProperties[0].ShaderMaps = newShaderProperty.ShaderMaps;
            bmRm.ShaderProperties[0].Arguments = newShaderProperty.Arguments;

            // Fix any null bitmaps, caused by bitm port failure
            foreach (var a in bmRm.ShaderProperties[0].ShaderMaps)
                if (a.Bitmap == null)
                    a.Bitmap = CacheContext.GetTag(DefaultBitmapsTags(edMaps[(int)bmRm.ShaderProperties[0].ShaderMaps.IndexOf(a)]));

            // Verify that none of the bitmaps are still H3 bitmaps
            foreach (var a in bmRm.ShaderProperties[0].ShaderMaps)
            {
                if (a.Bitmap == null)
                    throw new Exception("ERROR: bitmap is null even after trying to replace with default bitmaps.");

                try
                {
                    var b = CacheContext.GetTag(a.Bitmap.Index);

                    if (b.Group.Tag.ToString() != "bitm")
                        throw new Exception("ERROR: tag index exists, but the tag doesn't actually exist.");
                }
                catch
                {
                    throw new Exception("ERROR: bitmap is a H3 bitmap where only valid HO bitmaps should be.");
                }

            }

            return bmRm;
        }

        private static void GetRmt2Info(Stream cacheStream, GameCacheContext cacheContext)
        {
            if (cacheContext.Rmt2TagsInfo.Count != 0)
                return;

            foreach (var instance in cacheContext.TagCache.Index.FindAllInGroup("rmt2"))
            {
                var edContext = new TagSerializationContext(cacheStream, cacheContext, instance);
                var edRmt2 = cacheContext.Deserializer.Deserialize<RenderMethodTemplateEssentials>(edContext);

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

                // If rmt2 aren't named, name them all; TODO
                if (!CacheContext.TagNames.ContainsKey(edRmt2_.Key))
                {

                }

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

        private CachedTagInstance FindEquivalentRmt2(CacheFile.IndexItem bmRmt2Instance)
        {
            // Find similar shaders by finding tags with as many common bitmaps and arguments as possible.
            var edRmt2Temp = new List<ShaderTemplateItem>();

            // Make a new dictionary with rmt2 of the same shader type
            var edRmt2BestStats = new List<ShaderTemplateItem>();

            edRmt2BestStats = CollectRmt2Info(bmRmt2Instance);

            // Filter by common bitmaps count
            foreach (var d in edRmt2BestStats)
                if (d.mapsCommon == edRmt2BestStats.OrderBy(x => x.mapsCommon).Last().mapsCommon)
                    edRmt2Temp.Add(d);

            edRmt2BestStats = edRmt2Temp;
            edRmt2Temp = new List<ShaderTemplateItem>();

            // Filter by common arguments count
            foreach (var d in edRmt2BestStats)
                if (d.argsCommon == edRmt2BestStats.OrderBy(x => x.argsCommon).Last().argsCommon)
                    edRmt2Temp.Add(d);

            edRmt2BestStats = edRmt2Temp;
            edRmt2Temp = new List<ShaderTemplateItem>();

            // Filter by lowest uncommon bitmaps count
            foreach (var d in edRmt2BestStats)
                if (d.mapsUncommon == edRmt2BestStats.OrderBy(x => x.mapsUncommon).First().mapsUncommon)
                    edRmt2Temp.Add(d);

            edRmt2BestStats = edRmt2Temp;
            edRmt2Temp = new List<ShaderTemplateItem>();

            // Filter by lowest uncommon arguments count
            foreach (var d in edRmt2BestStats)
                if (d.argsUncommon == edRmt2BestStats.OrderBy(x => x.argsUncommon).First().argsUncommon)
                    edRmt2Temp.Add(d);

            edRmt2BestStats = edRmt2Temp;
            edRmt2Temp = new List<ShaderTemplateItem>();

            // Filter by the highest count of common rmdf reference values
            // Count how many rmdf values match
            // This should greatly improve rmt2 matching
            foreach (var d in edRmt2BestStats)
            {
                var edRmdfValues = CacheContext.TagNames[d.rmt2TagIndex].Split("_".ToCharArray()).ToList(); // rmdfValues = new List<string> {"0", "0", ... "3", "0", ...};
                edRmdfValues.RemoveAt(0);

                var bmRmdfValues = bmRmt2Instance.Filename.Split("_".ToCharArray()).ToList();
                bmRmdfValues.RemoveAt(0);

                int matchingValues = 0;
                for (int i = 0; i < bmRmdfValues.Count; i++)
                    if (bmRmdfValues[i] == edRmdfValues[i])
                        matchingValues++;

                d.rmdfValuesMatchingCount = matchingValues;
            }

            foreach (var d in edRmt2BestStats)
                if (d.rmdfValuesMatchingCount == edRmt2BestStats.OrderBy(
                    x => x.rmdfValuesMatchingCount).First().rmdfValuesMatchingCount) // or Last()
                    edRmt2Temp.Add(d);

            edRmt2BestStats = edRmt2Temp;

            var pRmt2Item = edRmt2BestStats.First();
            pRmt2 = pRmt2Item.rmt2TagIndex;

            return CacheContext.GetTag(pRmt2);
        }

        private class Arguments
        {
            public float Arg1 = 0.0f;
            public float Arg2 = 0.0f;
            public float Arg3 = 0.0f;
            public float Arg4 = 0.0f;
        }

        private class ShaderTemplateItem
        {
            public int rmt2TagIndex;
            public int rmdfValuesMatchingCount = 0;
            public int mapsCountEd;
            public int argsCountEd;
            public int mapsCommon;
            public int argsCommon;
            public int mapsUncommon;
            public int argsUncommon;
            public int mapsMissing;
            public int argsMissing;
        }

        [TagStructure(Name = "render_method_template", Tag = "rmt2")]
        private class RenderMethodTemplateEssentials // used to deserialize as fast as possible
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
                case "specular_coefficient_m_3":
                    val = new float[] { 0, 0, 0, 0 }; break;

                default: val = new float[] { 0, 0, 0, 0 }; break;
            }

            res.Arg1 = val[0];
            res.Arg2 = val[1];
            res.Arg3 = val[2];
            res.Arg4 = val[3];

            return res;
        }

        private static int DefaultBitmapsTags(string type)
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
                    return 0x02B7;

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
                    return 0x02B9;

                case "color_mask_map":
                    return 0x0375;

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
                    return 0x0376; // WARNING engine's toast, anything bump map related is busted

                case "self_illum_map":
                    return 0x0377;

                case "detail_mask_a":
                    return 0x037A;

                case "alpha_mask_map":
                    return 0x037B;

                default:
                    return 0x02B7;
                    // throw new NotImplementedException($"Useless exception. Bitmaps table requires an update for {type}.");
                    // Better replace with return 0x02B7;
            }
        }

        private static void GetShaderPresets(GameCacheContext CacheContext, string tagname)
        {
            var pEdArgs = new Dictionary<string, float[]>();

            switch (tagname)
            {
                case "Example":
                    // Use a forced shader tag index
                    // pRmsh = 0x3AB0;

                    // Use a forced shader tag name
                    // pRmsh = ArgumentParser.ParseTagName(CacheContext, @"levels\multi\guardian\shaders\guardian_metal_b").Index;

                    // Use a forced rmt2 tag index
                    pRmt2 = 0x02A7;

                    // Use a forced rmt2 tag name
                    pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_0_0_0_0_0_0_3_0_0_0.rmt2").Index;

                    // Use forced shader arguments TODO
                    pEdArgs.Add("roughness", new float[4] { 0f, 1f, 2f, 3.4f });

                    // Or force various debug stuf
                    debugUseEDFunctions = false;
                    break;

                // Snowbound Skybox:
                // case @"levels\multi\snowbound\sky\shaders\skydome":
                //     // pRmsh = 0x35BB;
                //     pEdArgs.Add("albedo_color", new float[] { 0.254901975f, 0.384313732f, 1f, 0f });
                //     break;
                // 
                // case @"levels\multi\snowbound\sky\shaders\aurora":
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_0_0_0_0_0_0_3_0_0_0.rmt2").Index;
                //     break;
                // 
                // case @"levels\multi\snowbound\shaders\plasma_spire_a":
                // case @"levels\multi\snowbound\sky\shaders\sun_clouds":
                // case @"levels\multi\snowbound\sky\shaders\dust_clouds":
                // case @"levels\multi\snowbound\sky\shaders\clouds_cirrus":
                //     // forceFunctions = true; // MatchShader7 would always bring the function if the H3 shader has one
                //     break;
                // 
                // // Snowbound:
                // case @"levels\multi\snowbound\shaders\cov_battery":
                // case @"levels\multi\snowbound\shaders\cov_metalplates_icy":
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_1_0_0_1_2_0_1_0_0_0.rmt2").Index;
                //     pEdArgs.Add("albedo_color", new float[] { 0.8f, 0.2f, 0.3f, 1f }); // 2
                //     pEdArgs.Add("env_tint_color", new float[] { 0.05f, 0.05f, 0.4f, 1f }); // 23
                //     break;
                // 
                // case @"levels\multi\snowbound\shaders\terrain_snow":
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\terrain_templates\_0_0_1_1_0_0.rmt2").Index;
                //     pEdArgs.Add("diffuse_coefficient_m_0", new float[] { 2f, 0, 0, 0 }); // 14
                //     break;
                // 
                // case @"levels\multi\snowbound\shaders\cov_glass_window_opaque":
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_3_0_3_1_4_0_0_0_1.rmt2").Index;
                //     pEdArgs.Add("env_roughness_scale", new float[] { 0.1f, 0, 0, 0 });
                //     pEdArgs.Add("env_tint_color", new float[] { 0.02f, 0.01f, 0, 0 });
                //     break;
                // 
                // // Chillout/ Cold Storage
                // case @"levels\dlc\chillout\shaders\chillout_floodroom02":
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_1_0_0_1_4_9_0_0_1.rmt2").Index;
                //     break;
                // 
                // case @"levels\dlc\chillout\shaders\chillout_trim_a":
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_1_0_1_1_2_1_0_0_0.rmt2").Index;
                //     break;
                // 
                // case @"levels\dlc\chillout\shaders\chillout_floor_glass_a":
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_0_0_1_5_2_0_1_0_0.rmt2").Index;
                //     break;
                // 
                // case @"levels\dlc\chillout\shaders\chillout_floodroom03":
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_3_0_0_1_4_0_0_0_0_0.rmt2").Index;
                //     break;
                // 
                // // Weapons
                // // compass 0x0EEA
                // // tens 0x0EEC
                // // ones 0x0EEB
                // case @"objects\weapons\rifle\assault_rifle\shaders\ones":
                // case @"objects\weapons\rifle\battle_rifle\shaders\ones":
                //     // pRmsh = 0x0EEB;
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_2_0_0_0_0_0_1_1_0_1.rmt2").Index;
                //     debugUseEDFunctions = true;
                //     break;
                // case @"objects\weapons\rifle\assault_rifle\shaders\tens":
                // case @"objects\weapons\rifle\battle_rifle\shaders\tens":
                //     // pRmsh = 0x0EEC;
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_2_0_0_0_0_0_1_1_0_1.rmt2").Index;
                //     debugUseEDFunctions = true;
                //     break;
                // case @"objects\weapons\rifle\assault_rifle\shaders\compass":
                //     // pRmsh = 0x0EEA;
                //     pRmt2 = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_2_0_0_0_0_0_8_1_0_1.rmt2").Index;
                //     debugUseEDFunctions = true;
                //     break;
                // 
                // case @"objects\weapons\melee\energy_blade\shaders\energy_blade":
                // case @"objects\weapons\melee\energy_blade\shaders\energy_blade_illum":
                //     // debugUseEDFunctions = true;
                //     break;
                // 
                // // case @"objects\weapons\rifle\assault_rifle\shaders\assault_rifle":
                // //     rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_1_0_1_2_0_1_0_0_0_0.rmt2").Index;
                // //     break;
                // // 
                // // case @"objects\weapons\rifle\battle_rifle\shaders\battle_rifle":
                // //     rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_2_0_1_2_2_1_0_0_0_0.rmt2").Index;
                // //     break;
                // 
                // // case @"objects\weapons\rifle\smg\shaders\smg_metal":
                // //     rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_2_0_1_2_1_1_0_1_0_0.rmt2").Index;
                // //     break;
                // // 
                // // case @"objects\weapons\rifle\shotgun\shaders\shotgun":
                // //     rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_2_0_1_2_1_1_0_1_0_0.rmt2").Index;
                // //     break;
                // // 
                // // case @"objects\weapons\rifle\sniper_rifle\shaders\sniper_rifle_metal":
                // //     rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_2_0_1_2_1_1_0_1_0_0.rmt2").Index;
                // //     break;
                // 
                // case @"objects\weapons\rifle\covenant_carbine\shaders\carbine_display": // done trough script
                // case @"objects\weapons\rifle\covenant_carbine\shaders\carbine_switch":
                //     // pRmsh = 0x11A9;
                //     // debugConvertFunctions = true;
                //     // rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_2_0_0_0_4_0_6_1_0_1_0.rmt2").Index;
                //     break;
                // 
                // case @"objects\weapons\rifle\beam_rifle\shaders\beam_rifle_luminous":
                //     // debugUseEDFunctions = true;
                //     // rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_2_0_0_0_4_0_6_1_0_1_0.rmt2").Index;
                //     break;
                // 
                // case @"levels\atlas\sc140\shaders\ext_metal_trim_red":
                // case @"levels\atlas\sc140\shaders\ext_wall_column":
                // case @"levels\atlas\sc140\shaders\glass_light_b_blue":
                // case @"levels\atlas\sc140\shaders\glass_light_b_blue_off":
                // case @"levels\atlas\sc140\shaders\mc_stairs_ext":
                // case @"levels\atlas\sc140\shaders\wall_ext":
                // case @"levels\atlas\sc140\shaders\wall_ext_2":
                // case @"levels\atlas\sc140\shaders\wall_ext_center":
                // case @"levels\atlas\shared\shaders\light_white":
                // case @"levels\dlc\spacecamp\shaders\glass_light_a":
                // case @"levels\dlc\spacecamp\shaders\glass_light_b_blue":
                // case @"levels\dlc\spacecamp\shaders\glass_light_sign_d":
                //     pEdArgs.Add("specular_coefficient", new float[] { 0, 0, 0, 0 }); // should add to all shaders
                //     break;
                // 
                // case @"objects\weapons\pistol\plasma_pistol\shaders\plasma_pistol_metal": // UNTESTED
                //     // debugConvertFunctions = true;
                //     break;
                // 
                // case @"objects\weapons\rifle\battle_rifle\shaders\battle_rifle_lens":
                // // rmt2Preset = ArgumentParser.ParseTagName(CacheContext, @"shaders\shader_templates\_0_1_0_1_1_2_0_0_0_1.rmt2").Index;

                default:
                    break;
            }

            // if (tagname.Contains("\\z_"))
            //     pRmsh = 0x3AB0;
        }

        private static void FixTagblocks(GameCacheContext CacheContext, Stream cacheStream, CachedTagInstance edRmt2Instance, RenderMethod bmRm)
        {
            CachedTagInstance templateShader = null;

            // Find what rmdf this rmt2 is supposed to use
            // Find what shader does this rmt2 belong to.
            var dependsOn = CacheContext.TagCache.Index.NonNull().Where(t => t.Dependencies.Contains(edRmt2Instance.Index));

            var validShaders = new List<string> { "beam", "decs", "ltvl", "prt3", "rmcs", "rmd ", "rmfl", "rmhg", "rmsh", "rmss", "rmtr", "rmw ", "rmzo", "cntl" };

            var orderedDependsOn = dependsOn.OrderBy(x => x.Index);

            foreach (var dependency in orderedDependsOn)
            {
                if (validShaders.Contains(dependency.Group.Tag.ToString()))
                {
                    templateShader = dependency;
                    break;
                }
            }

            // Incase it's a ported rmt2 without a shader, find any shader that uses this type of rmt2.
            if (dependsOn.ToArray().Length == 0)
            {
                var tagname = CacheContext.TagNames[edRmt2Instance.Index];
                var type = tagname.Split("\\".ToCharArray())[1];
                var shaderType = "";

                switch (type)
                {
                    case "shader_templates": shaderType = "rmsh"; break;
                    case "black_templates": shaderType = "rmbk"; break;
                    case "cortana_templates": shaderType = "rmct"; break;
                    case "custom_templates": shaderType = "rmcs"; break;
                    case "decal_templates": shaderType = "rmd "; break;
                    case "foliage_templates": shaderType = "rmfl"; break;
                    case "screen_templates": shaderType = "rmss"; break;
                    case "terrain_templates": shaderType = "rmtr"; break;
                    case "water_templates": shaderType = "rmw "; break;
                    case "zonly_templates": shaderType = "rmzo"; break;
                    case "halogram_templates": shaderType = "rmhg"; break;
                    case "particle_templates": shaderType = "prt3"; break;
                    case "beam_templates": shaderType = "beam"; break;
                    case "contrail_templates": shaderType = "cntl"; break;
                    case "light_volume_templates": shaderType = "ltvl"; break;

                    default:
                        break;
                }

                templateShader = CacheContext.TagCache.Index.FindAllInGroup(shaderType).First();

            }

            // Fix tagblock at the top
            var parentShaderDef = CacheContext.Deserializer.Deserialize(new TagSerializationContext(cacheStream, CacheContext, templateShader), TagDefinition.Find(templateShader.Group.Tag));

            RenderMethod renderMethod = null;

            switch (templateShader.Group.Tag.ToString())
            {
                case "rmsh": renderMethod = (Shader)parentShaderDef; break;
                case "rmbk": renderMethod = (ShaderBlack)parentShaderDef; break;
                case "rmct": renderMethod = (ShaderCortana)parentShaderDef; break;
                case "rmcs": renderMethod = (ShaderCustom)parentShaderDef; break;
                case "rmd ": renderMethod = (ShaderDecal)parentShaderDef; break;
                case "rmfl": renderMethod = (ShaderFoliage)parentShaderDef; break;
                case "rmss": renderMethod = (ShaderScreen)parentShaderDef; break;
                case "rmtr": renderMethod = (ShaderTerrain)parentShaderDef; break;
                case "rmw ": renderMethod = (ShaderWater)parentShaderDef; break;
                case "rmzo": renderMethod = (ShaderZonly)parentShaderDef; break;
                case "beam": var a = (BeamSystem)parentShaderDef; foreach (var f in a.Beam) renderMethod = f.RenderMethod; break; // any would do
                case "cntl": var b = (ContrailSystem)parentShaderDef; foreach (var f in b.Contrail) renderMethod = f.RenderMethod; break;
                case "decs": var c = (DecalSystem)parentShaderDef; foreach (var f in c.DecalSystem2) renderMethod = f.RenderMethod; break;
                case "ltvl": var d = (LightVolumeSystem)parentShaderDef; foreach (var f in d.LightVolume) renderMethod = f.RenderMethod; break;
                case "prt3": var e = (Particle)parentShaderDef; renderMethod = e.RenderMethod; break;
                case "rmhg":
                    renderMethod = (ShaderHalogram)parentShaderDef;
                    debugUseEDFunctions = true;// rmhg seems to crash if it doesn't have functions
                    break;

                default: break;
            }

            bmRm.BaseRenderMethod = renderMethod.BaseRenderMethod;
            bmRm.Unknown = renderMethod.Unknown;

            bmRm.ImportData = new List<RenderMethod.ImportDatum>(); // probably not used

            if (debugUseEDFunctions && renderMethod.ShaderProperties[0].Functions.Count > 0) // trash, definitely not the way to go
            {
                bmRm.ShaderProperties[0].Unknown = renderMethod.ShaderProperties[0].Unknown;
                bmRm.ShaderProperties[0].DrawModes = renderMethod.ShaderProperties[0].DrawModes;
                bmRm.ShaderProperties[0].Unknown4 = renderMethod.ShaderProperties[0].Unknown4;
                bmRm.ShaderProperties[0].Unknown5 = renderMethod.ShaderProperties[0].Unknown5;
                bmRm.ShaderProperties[0].Functions = renderMethod.ShaderProperties[0].Functions;
                return;
            }

            // Nuke functions, affects various things, not just animations.
            // H3 functions should definitely not be ported automatically without going trough them manually, some tagblocks have common data as their rmt2 tag.
            bmRm.ShaderProperties[0].Unknown = new List<RenderMethod.ShaderProperty.UnknownBlock1>();
            bmRm.ShaderProperties[0].DrawModes = new List<RenderMethod.ShaderProperty.DrawMode>();
            bmRm.ShaderProperties[0].Unknown4 = new List<RenderMethod.ShaderProperty.UnknownBlock2>();
            bmRm.ShaderProperties[0].Unknown5 = new List<RenderMethod.ShaderProperty.UnknownBlock3>();
            bmRm.ShaderProperties[0].Functions = new List<RenderMethod.ShaderProperty.FunctionBlock>();

        }

        private static void NameRmt2(GameCacheContext CacheContext, Stream cacheStream)
        {
            var validShaders = new List<string> { "rmsh", "beam", "decs", "ltvl", "prt3", "rmcs", "rmd ", "rmfl", "rmhg", "rmss", "rmtr", "rmw ", "rmzo", "cntl" };

            foreach (var edInstance in CacheContext.TagCache.Index.NonNull())
            {
                if (!validShaders.Contains(edInstance.Group.Tag.ToString()))
                    continue;

                var edContext = new TagSerializationContext(cacheStream, CacheContext, edInstance);
                var parentShaderDef = CacheContext.Deserializer.Deserialize(new TagSerializationContext(cacheStream, CacheContext, edInstance), TagDefinition.Find(edInstance.Group.Tag));

                RenderMethod renderMethod = null;

                var type = "invalid";

                switch (edInstance.Group.Tag.ToString()) // none of these are correct
                {
                    case "rmsh": renderMethod = (Shader)parentShaderDef; type = "shader"; break;
                    case "rmbk": renderMethod = (ShaderBlack)parentShaderDef; type = "black"; break;
                    case "rmct": renderMethod = (ShaderCortana)parentShaderDef; type = "cortana"; break;
                    case "rmcs": renderMethod = (ShaderCustom)parentShaderDef; type = "custom"; break;
                    case "rmd ": renderMethod = (ShaderDecal)parentShaderDef; type = "decal"; break;
                    case "rmfl": renderMethod = (ShaderFoliage)parentShaderDef; type = "foliage"; break;
                    case "rmss": renderMethod = (ShaderScreen)parentShaderDef; type = "screen"; break;
                    case "rmtr": renderMethod = (ShaderTerrain)parentShaderDef; type = "terrain"; break;
                    case "rmw ": renderMethod = (ShaderWater)parentShaderDef; type = "water"; break;
                    case "rmzo": renderMethod = (ShaderZonly)parentShaderDef; type = "zonly"; break;
                    case "rmhg": renderMethod = (ShaderHalogram)parentShaderDef; type = "halogram"; break;
                    case "prt3": var e = (Particle)parentShaderDef; renderMethod = e.RenderMethod; type = "particle"; break;
                    case "beam": var a = (BeamSystem)parentShaderDef; foreach (var f in a.Beam) renderMethod = f.RenderMethod; type = "beam"; break; // any would do
                    case "cntl": var b = (ContrailSystem)parentShaderDef; foreach (var f in b.Contrail) renderMethod = f.RenderMethod; type = "contrail"; break;
                    case "decs": var c = (DecalSystem)parentShaderDef; foreach (var f in c.DecalSystem2) renderMethod = f.RenderMethod; type = "decal"; break;
                    case "ltvl": var d = (LightVolumeSystem)parentShaderDef; foreach (var f in d.LightVolume) renderMethod = f.RenderMethod; type = "light_volume"; break;

                    default: break;
                }

                var newTagName = $"shaders\\{type}_templates\\";

                var rmdfRefValues = "";

                for (int i = 0; i < renderMethod.Unknown.Count; i++)
                {
                    if (edInstance.Group.Tag.ToString() == "rmsh" && i > 9)
                        break;

                    rmdfRefValues = $"{rmdfRefValues}_{renderMethod.Unknown[i].Unknown}";
                }

                newTagName = $"{newTagName}{rmdfRefValues}";

                CacheContext.TagNames[renderMethod.ShaderProperties[0].Template.Index] = newTagName;
            }
        }

    }
}

