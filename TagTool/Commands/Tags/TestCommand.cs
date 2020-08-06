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
using HaloShaderGenerator.Globals;

namespace TagTool.Commands
{

    public class TestCommand : Command
    {
        GameCache Cache;

        public TestCommand(GameCache cache) : base(false, "Test", "Test", "Test", "Test")
        {
            Cache = cache;
        }

        public void RegenShaderGLVS()
        {
            using (var stream = Cache.OpenCacheReadWrite())
            {
                var generator = new HaloShaderGenerator.Shader.ShaderGenerator();
                // recompile glvs

                var tag = Cache.TagCache.GetTag(@"shaders\shader_shared_vertex_shaders", "glvs");

                var glvs = Cache.Deserialize<GlobalVertexShader>(stream, tag);
                // world rigid skinned
                for (int i = 0; i < 3; i++)
                {
                    var vertexBlock = glvs.VertexTypes[i];
                    for (int j = 0; j < vertexBlock.DrawModes.Count; j++)
                    {
                        var entryPoint = vertexBlock.DrawModes[j];
                        var entryPointEnum = (ShaderStage)j;
                        if (entryPoint.ShaderIndex != -1)
                        {
                            if (generator.IsEntryPointSupported(entryPointEnum) && generator.IsVertexShaderShared(entryPointEnum))
                            {
                                var result = generator.GenerateSharedVertexShader((HaloShaderGenerator.Globals.VertexType)i, entryPointEnum);
                                glvs.Shaders[entryPoint.ShaderIndex] = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GenerateVertexShaderBlock(Cache, result);
                            }
                        }
                    }
                }

                Cache.Serialize(stream, tag, glvs);

                // recompile glps

                tag = Cache.TagCache.GetTag(@"shaders\shader_shared_pixel_shaders", "glps");
                var glps = Cache.Deserialize<GlobalPixelShader>(stream, tag);

                for (int i = 0; i < 2; i++)
                {
                    var result = generator.GenerateSharedPixelShader(ShaderStage.Shadow_Generate, 2, i);
                    glps.Shaders[i] = TagTool.Shaders.ShaderGenerator.ShaderGenerator.GeneratePixelShaderBlock(Cache, result);
                }
                Cache.Serialize(stream, tag, glps);

            }
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return false;

            using (var stream = Cache.OpenCacheReadWrite())
            {
                int newNameCount = 0;
                foreach (var tag in Cache.TagCache.NonNull())
                {
                    var tagGroups = new Tag[] { "rm  ", new Tag("beam"), new Tag("cntl"), new Tag("ltvl"), new Tag("decs"), new Tag("prt3") };
                    if (tag.IsInGroup(tagGroups))
                    {
                        CachedTagHaloOnline hoTag = tag as CachedTagHaloOnline;
                        foreach (var dep in hoTag.Dependencies)
                        {
                            var depName = Cache.TagCache.GetTag(dep).Name;
                            if (depName.Contains("ms30"))
                            {
                                if (!tag.Name.StartsWith("ms30"))
                                {
                                    Console.WriteLine(tag.Name);
                                    tag.Name = "ms30\\" + tag.Name;
                                    newNameCount += 1;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < 10; i++)
                {
                    foreach (var tag in Cache.TagCache.NonNull())
                    {
                        var tagGroups = new Tag[] { "scen", "mode", "mach", "item", "hlmt", "dctr", "bloc" };
                        if (!tag.IsInGroup(tagGroups) || tag.Name.Contains("eldewrito") || tag.Name.Contains("reforge"))
                            continue;

                        CachedTagHaloOnline hoTag = tag as CachedTagHaloOnline;
                        foreach (var dep in hoTag.Dependencies)
                        {
                            var depName = Cache.TagCache.GetTag(dep).Name;
                            if (depName.Contains("ms30"))
                            {
                                if (!tag.Name.StartsWith("ms30"))
                                {
                                    Console.WriteLine($"{tag.Name}.{tag.Group}");
                                    tag.Name = "ms30\\" + tag.Name;
                                    newNameCount += 1;
                                    break;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine($"Added ms30 prefix to {newNameCount} tags.");
                var hoCache = Cache as GameCacheHaloOnline;
                hoCache.SaveTagNames();


                foreach (var file in Cache.Directory.GetFiles("*.map"))
                {
                    using (var mapFileStream = file.Open(FileMode.Open, FileAccess.ReadWrite))
                    {
                        var reader = new EndianReader(mapFileStream);
                        var writer = new EndianWriter(mapFileStream);

                        var mapFile = new MapFile();
                        mapFile.Read(reader);

                        if (mapFile.MapFileBlf.MapVariant == null)
                            continue;

                        var tagNamesChunk = mapFile.MapFileBlf.MapVariantTagNames;
                        var palette = mapFile.MapFileBlf.MapVariant.MapVariant.Palette;
                        for (int i = 0; i < palette.Length; i++)
                        {
                            if (palette[i].TagIndex == -1)
                                continue;

                            var name = tagNamesChunk.Names[i].Name;
                            string newName = $"ms30\\{name}";
                            if (Cache.TagCache.TryGetTag(newName, out CachedTag tag))
                            {
                                tagNamesChunk.Names[i].Name = newName;
                                Console.WriteLine($"Prefixed '{tag}'");
                            }
                        }

                        mapFileStream.Position = 0;
                        mapFile.Write(writer);
                    }
                }

            }

            return true;
            /*
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
            */
        }
    }
}

