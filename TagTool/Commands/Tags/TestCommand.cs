using TagTool.Cache;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Common;
using TagTool.Tags.Definitions;
using TagTool.Tags;
using TagTool.Serialization;
using TagTool.Bitmaps;
using TagTool.Tags.Resources;
using TagTool.Bitmaps.Utils;
using TagTool.Bitmaps.DDS;
using TagTool.Geometry;
using TagTool.BlamFile;
using TagTool.Tags.Definitions.Gen1;
using TagTool.Cache.HaloOnline;
using TagTool.Havok;
using System.Linq;
using System.IO.Compression;
using TagTool.Tools.Geometry;
using TagTool.Shaders;
using TagTool.Shaders.ShaderGenerator;
using TagTool.Commands.Shaders;
using System.Diagnostics;
using HaloShaderGenerator.Shader;

namespace TagTool.Commands
{
    
    public class TestCommand : Command
    {
        GameCache Cache;

        public TestCommand(GameCache cache) : base(false, "Test", "Test", "Test", "Test")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            
            using (var stream = Cache.OpenCacheReadWrite())
            {
                

                var rmdf = Cache.Deserialize<RenderMethodDefinition>(stream, Cache.TagCache.GetTag(@"shaders\shader", "rmdf"));
                var glps = Cache.Deserialize<GlobalPixelShader>(stream, Cache.TagCache.GetTag(@"shaders\shader_shared_pixel_shaders", "glps"));
                var glvs = Cache.Deserialize<GlobalVertexShader>(stream, Cache.TagCache.GetTag(@"shaders\shader_shared_vertex_shaders", "glvs"));

                var generator = new HaloShaderGenerator.Shader.ShaderGenerator(Albedo.Default, Bump_Mapping.Standard, Alpha_Test.Off, Specular_Mask.Specular_Mask_From_Diffuse,
                    Material_Model.Two_Lobe_Phong, Environment_Mapping.Dynamic, Self_Illumination.Off, Blend_Mode.Opaque, Parallax.Off, Misc.First_Person_Never, Distortion.Off);

                var rmt2 = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, @"shaders\generated_test_shader");
                var rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(@"shaders\generated_test_shader");
                Cache.Serialize(stream, rmt2Tag, rmt2);

                Cache.SaveStrings();
                (Cache as GameCacheHaloOnline).SaveTagNames();

                var guardianRmshTag = Cache.TagCache.GetTag(@"levels\multi\guardian\shaders\guardian_metal_b", "rmsh");
                var rmsh = Cache.Deserialize<Shader>(stream, guardianRmshTag);
                rmsh.ShaderProperties[0].Template = rmt2Tag;
                Cache.Serialize(stream, guardianRmshTag, rmsh);
            }
            

            /*
            var shaderType = "black";
            var rmt2Name = @"shaders\black_templates\_0";
            var generator = new HaloShaderGenerator.Black.ShaderBlackGenerator();
                
            var rmdf = ShaderGenerator.GenerateRenderMethodDefinition(Cache, stream, generator, shaderType, out var glps, out var glvs);
            var rmdfTag = Cache.TagCache.AllocateTag<RenderMethodDefinition>($"shaders\\{shaderType}");
            Cache.Serialize(stream, rmdfTag, rmdf);

            var rmt2 = ShaderGenerator.GenerateRenderMethodTemplate(Cache, stream, rmdf, glps, glvs, generator, rmt2Name);
            var rmt2Tag = Cache.TagCache.AllocateTag<RenderMethodTemplate>(rmt2Name);
            Cache.Serialize(stream, rmt2Tag, rmt2);
            */


            /*
            Dictionary<string, HashSet<int>> registers = new Dictionary<string, HashSet<int>>();

            using (var stream = Cache.OpenCacheRead())
            {
                foreach (var tag in Cache.TagCache.NonNull())
                {
                    if(tag.Name != null && tag.IsInGroup("pixl") && !tag.Name.StartsWith("ms30") && tag.Name.Contains("shader_templates"))
                    {
                        var pixl = Cache.Deserialize<PixelShader>(stream, tag);
                        foreach(var shader in pixl.Shaders)
                        {
                            foreach(var register in shader.PCParameters)
                            {
                                var name = Cache.StringTable.GetString(register.ParameterName);
                                if (registers.ContainsKey(name))
                                {
                                    registers[name].Add(register.RegisterIndex);
                                }
                                else
                                {
                                    registers[name] = new HashSet<int>();
                                    registers[name].Add(register.RegisterIndex);
                                }
                            }
                        }
                    }
                }
            }

            foreach(var reg in registers.Keys)
            {
                if(registers[reg].Count == 1)
                {
                    Console.Write($"{reg}: ");
                    foreach (var index in registers[reg])
                    {
                        Console.Write($"{index} ");
                    }
                    Console.WriteLine();
                }
            }
            */


            return true;
        }
    }
}

