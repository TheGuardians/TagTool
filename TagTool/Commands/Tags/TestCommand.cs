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

            
            var mapFilesFolder = new DirectoryInfo(@"D:\Halo\Maps\Halo3");
            var outDir = new DirectoryInfo("CacheTest");
            if (!outDir.Exists)
                outDir.Create();


            //
            // Insert what test command you want below
            //


            //var file = new FileInfo(Path.Combine(mapFilesFolder.FullName, @"descent.map"));

            //var cache = GameCache.Open(file);

            string filename = "test";
            BlamModelFile geometryFormat = new BlamModelFile();

            using (var stream = Cache.OpenCacheRead())
            {
                // disassemble specified shaders related to rmt2
                var tagName = @"shaders\shader_templates\_0_0_0_0_0_0_0_0_0_0_0";

                var rmt2Tag = Cache.TagCache.GetTag(tagName, "rmt2");
                var glvsTag = Cache.TagCache.GetTag(@"shaders\shader_shared_vertex_shaders", "glvs");
                var rmt2 = Cache.Deserialize<RenderMethodTemplate>(stream, rmt2Tag);
                var glvs = Cache.Deserialize<GlobalVertexShader>(stream, glvsTag);

                var pixl = Cache.Deserialize<PixelShader>(stream, rmt2.PixelShader);

                Directory.CreateDirectory(tagName);
                // get vertex shaders (TODO: check vtsh if it has an overriding shader for the entry point?)
                foreach(VertexType vertex in Enum.GetValues(typeof(VertexType)))
                {
                    if( (int)vertex < glvs.VertexTypes.Count)
                    {
                        var currentVertex = glvs.VertexTypes[(int)vertex];
                        
                        foreach (EntryPoint entry in Enum.GetValues(typeof(EntryPoint)))
                        {
                            if ((int)entry < currentVertex.DrawModes.Count)
                            {
                                var entryShader = currentVertex.DrawModes[(int)entry].ShaderIndex;

                                if (entryShader != -1)
                                {
                                    string entryName = entry.ToString() + ".vertex_shader";
                                    string vertexFolderName = Path.Combine(tagName, vertex.ToString().ToLower());
                                    Directory.CreateDirectory(vertexFolderName);
                                    string vertexShaderFilename = Path.Combine(vertexFolderName, entryName);

                                    DisassembleHaloOnlineShader(glvs, entryShader, vertexShaderFilename);
                                }
                            }
                        }
                    }
                }

                foreach (EntryPoint entry in Enum.GetValues(typeof(EntryPoint)))
                {
                    if ((int)entry < pixl.DrawModes.Count)
                    {
                        var entryShader = pixl.DrawModes[(int)entry].Offset;

                        if (pixl.DrawModes[(int)entry].Count != 0)
                        {
                            string entryName = entry.ToString().ToLower() + ".pixel_shader";
                            string pixelShaderFilename = Path.Combine(tagName, entryName);

                            DisassembleHaloOnlineShader(pixl, entryShader, pixelShaderFilename);
                        }
                    }
                }

                /*
                //objects\gear\human\industrial\toolbox_small\toolbox_small
                var tag = Cache.TagCache.GetTag(@"objects\vehicles\warthog\warthog", "mode"); // objects\vehicles\warthog\warthog
                var mode = Cache.Deserialize<RenderModel>(stream, tag);
                var resource = Cache.ResourceCache.GetRenderGeometryApiResourceDefinition(mode.Geometry.Resource);
                mode.Geometry.SetResourceBuffers(resource);

                geometryFormat.InitGen3(Cache, mode);

                using (var modelStream = new FileStream($"3dsmax/{filename}.bmf", FileMode.Create))
                using (var writer = new EndianWriter(modelStream))
                {
                    geometryFormat.SerializeToFile(writer);
                }*/
            }
            return true;
        }


        private string DisassembleHaloOnlineShader(object definition, int shaderIndex, string filename)
        {
            string disassembly = null;

            if (definition.GetType()  == typeof(PixelShader) || definition.GetType() == typeof(GlobalPixelShader))
            {
                PixelShaderBlock shader_block = null;
                if (definition.GetType() == typeof(PixelShader))
                {
                    var _definition = definition as PixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                if (definition.GetType() == typeof(GlobalPixelShader))
                {
                    var _definition = definition as GlobalPixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                var pc_shader = shader_block.PCShaderBytecode;
                disassembly = D3DCompiler.Disassemble(pc_shader);
                if (pc_shader == null)
                    disassembly = null;
            }

            if (definition.GetType() == typeof(VertexShader) || definition.GetType() == typeof(GlobalVertexShader))
            {
                VertexShaderBlock shader_block = null;
                if (definition.GetType() == typeof(VertexShader))
                {
                    var _definition = definition as VertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                if (definition.GetType() == typeof(GlobalVertexShader))
                {
                    var _definition = definition as GlobalVertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                var pc_shader = shader_block.PCShaderBytecode;
                disassembly = D3DCompiler.Disassemble(pc_shader);
                if (pc_shader == null)
                    disassembly = null;
            }

            using (var writer = File.CreateText(filename))
            {
                writer.WriteLine(disassembly);
            }

            return disassembly;
        }
    }
}

