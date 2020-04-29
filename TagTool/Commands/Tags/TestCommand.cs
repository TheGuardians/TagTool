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


            //string filename = "test";
            //BlamModelFile geometryFormat = new BlamModelFile();
            
            using (var stream = Cache.OpenCacheRead())
            {/*
                CachedTag glvsTag = Cache.TagCache.GetTag(@"shaders\shader_shared_vertex_shaders", "glvs");

                var glvs = Cache.Deserialize<GlobalVertexShader>(stream, glvsTag);

                var worldResult = ShaderGenerator.GenerateGlobalVertexShaderShader(VertexType.World, EntryPoint.Albedo);
                var rigidResult = ShaderGenerator.GenerateGlobalVertexShaderShader(VertexType.Rigid, EntryPoint.Albedo);

                int worldAlbedoIndex = glvs.VertexTypes[(int)VertexType.World].DrawModes[(int)EntryPoint.Albedo].ShaderIndex;
                int rigidAlbedoIndex = glvs.VertexTypes[(int)VertexType.Rigid].DrawModes[(int)EntryPoint.Albedo].ShaderIndex;

                glvs.Shaders[worldAlbedoIndex].PCShaderBytecode = worldResult.Bytecode;
                glvs.Shaders[rigidAlbedoIndex].PCShaderBytecode = rigidResult.Bytecode;

                Cache.Serialize(stream, glvsTag, glvs);*/

                
                // disassemble specified shaders related to rmt2
                var tagName = @"shaders\black_templates\_0";

                var rmt2Tag = Cache.TagCache.GetTag(tagName, "rmt2");
                var glvsTag = Cache.TagCache.GetTag(@"shaders\black_shared_vertex_shaders", "glvs");
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

                                    DisassembleShader(glvs, entryShader, vertexShaderFilename, Cache);
                                }
                            }
                        }
                    }
                }

                foreach (EntryPoint entry in Enum.GetValues(typeof(EntryPoint)))
                {
                    if ((int)entry < pixl.EntryPointShaders.Count)
                    {
                        var entryShader = pixl.EntryPointShaders[(int)entry].Offset;

                        if (pixl.EntryPointShaders[(int)entry].Count != 0)
                        {
                            string entryName = entry.ToString().ToLower() + ".pixel_shader";
                            string pixelShaderFilename = Path.Combine(tagName, entryName);

                            DisassembleShader(pixl, entryShader, pixelShaderFilename, Cache);
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

        private string DisassembleShader(object definition, int shaderIndex, string filename, GameCache cache)
        {
            if (cache.GetType() == typeof(GameCacheGen3))
            {
                return DisassembleGen3Shader(definition, shaderIndex, filename);
            }
            else if (Cache.GetType() == typeof(GameCacheHaloOnline))
            {
                return DisassembleHaloOnlineShader(definition, shaderIndex, filename);
            }
            return null;
        }


        private string DisassembleGen3Shader(object definition, int shaderIndex, string filename)
        {
            var file = UseXSDCommand.XSDFileInfo;
            if (file == null)
            {
                Console.WriteLine("xsd.exe not found! use command usexsd <directory> to load xsd.exe");
                return null;
            }
            var tempFile = Path.GetTempFileName();
            string disassembly = null;
            string xsdArguments = " \"" + tempFile + "\"";
            byte[] microcode = new byte[0];
            byte[] debugData = new byte[0];
            byte[] constantData = new byte[0];

            //
            // Set the arguments for xsd.exe according to the XDK documentation
            //
            try
            {
                if (definition.GetType() == typeof(PixelShader) || definition.GetType() == typeof(GlobalPixelShader))
                {
                    xsdArguments = "/rawps" + xsdArguments;
                    PixelShaderBlock shaderBlock = null;
                    if (definition.GetType() == typeof(PixelShader))
                    {
                        var _definition = definition as PixelShader;
                        if (shaderIndex < _definition.Shaders.Count)
                            shaderBlock = _definition.Shaders[shaderIndex];
                        else
                            return null;
                    }

                    if (definition.GetType() == typeof(GlobalPixelShader))
                    {
                        var _definition = definition as GlobalPixelShader;
                        if (shaderIndex < _definition.Shaders.Count)
                            shaderBlock = _definition.Shaders[shaderIndex];
                        else
                            return null;
                    }

                    microcode = shaderBlock.XboxShaderReference.ShaderData;
                    debugData = shaderBlock.XboxShaderReference.DebugData;
                    constantData = shaderBlock.XboxShaderReference.ConstantData;
                }

                if (definition.GetType() == typeof(VertexShader) || definition.GetType() == typeof(GlobalVertexShader))
                {
                    xsdArguments = "/rawvs" + xsdArguments;
                    VertexShaderBlock shaderBlock = null;
                    if (definition.GetType() == typeof(VertexShader))
                    {
                        var _definition = definition as VertexShader;
                        if (shaderIndex < _definition.Shaders.Count)
                            shaderBlock = _definition.Shaders[shaderIndex];
                        else
                            return null;
                    }

                    if (definition.GetType() == typeof(GlobalVertexShader))
                    {
                        var _definition = definition as GlobalVertexShader;
                        if (shaderIndex < _definition.Shaders.Count)
                            shaderBlock = _definition.Shaders[shaderIndex];
                        else
                            return null;
                    }

                    microcode = shaderBlock.XboxShaderReference.ShaderData;
                    debugData = shaderBlock.XboxShaderReference.DebugData;
                    constantData = shaderBlock.XboxShaderReference.ConstantData;
                }

                File.WriteAllBytes(tempFile, debugData);
                File.WriteAllBytes(tempFile, constantData);
                File.WriteAllBytes(tempFile, microcode);

                ProcessStartInfo info = new ProcessStartInfo(UseXSDCommand.XSDFileInfo.FullName)
                {
                    Arguments = xsdArguments,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = false
                };
                Process xsd = Process.Start(info);
                disassembly = xsd.StandardOutput.ReadToEnd();
                xsd.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed to disassemble shader: {e.ToString()}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            using (var writer = File.CreateText(filename))
            {
                GenerateGen3ShaderHeader(definition, shaderIndex, writer);
                writer.WriteLine(disassembly);
            }

            return disassembly;
        }


        private void GenerateGen3ShaderHeader(object definition, int shaderIndex, StreamWriter writer)
        {
            List<ShaderParameter> parameters = null;

            if (definition.GetType() == typeof(PixelShader) || definition.GetType() == typeof(GlobalPixelShader))
            {
                PixelShaderBlock shader_block = null;
                if (definition.GetType() == typeof(PixelShader))
                {
                    var _definition = definition as PixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return;
                }

                if (definition.GetType() == typeof(GlobalPixelShader))
                {
                    var _definition = definition as GlobalPixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return;
                }

                parameters = shader_block.XboxParameters;
            }

            if (definition.GetType() == typeof(VertexShader) || definition.GetType() == typeof(GlobalVertexShader))
            {
                VertexShaderBlock shaderBlock = null;
                if (definition.GetType() == typeof(VertexShader))
                {
                    var _definition = definition as VertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shaderBlock = _definition.Shaders[shaderIndex];
                    else
                        return;
                }

                if (definition.GetType() == typeof(GlobalVertexShader))
                {
                    var _definition = definition as GlobalVertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shaderBlock = _definition.Shaders[shaderIndex];
                    else
                        return;
                }

                parameters = shaderBlock.XboxParameters;
            }

            List<string> parameterNames = new List<string>();

            for (int i = 0; i < parameters.Count; i++)
                parameterNames.Add(Cache.StringTable.GetString(parameters[i].ParameterName));


            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Generated by TagTool and Xbox Shader Disassembler");
            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Parameters");
            WriteHeaderLine(writer);
            for (int i = 0; i < parameters.Count; i++)
            {
                var param = parameters[i];
                WriteHeaderLine(writer, $"    {GetTypeString(param.RegisterType, param.RegisterCount)} {parameterNames[i]};");
            }
            WriteHeaderLine(writer);
            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Registers");
            WriteHeaderLine(writer);
            // sort later
            for (int i = 0; i < parameters.Count; i++)
            {
                var param = parameters[i];
                WriteHeaderLine(writer, $"    {parameterNames[i]} {GetRegisterString(param.RegisterType, param.RegisterIndex)} {param.RegisterCount}");
            }
            WriteHeaderLine(writer);
        }

        private void WriteHeaderLine(StreamWriter writer, string entry = null)
        {
            string result = "// ";
            if (entry != null)
                result += entry;
            writer.WriteLine(result);
        }

        private string GetRegisterString(ShaderParameter.RType type, int registerIndex)
        {
            string result = "";
            switch (type)
            {
                case ShaderParameter.RType.Boolean:
                    result = "b";
                    break;
                case ShaderParameter.RType.Integer:
                case ShaderParameter.RType.Vector:
                    result = "c";
                    break;
                case ShaderParameter.RType.Sampler:
                    result = "s";
                    break;
            }
            result += registerIndex.ToString();
            return result;
        }

        private string GetTypeString(ShaderParameter.RType type, int size)
        {
            string result = "";
            switch (type)
            {
                case ShaderParameter.RType.Boolean:
                    result = "bool";
                    break;
                case ShaderParameter.RType.Integer:
                    result = "integer";
                    break;
                case ShaderParameter.RType.Vector:
                    result = "float4";
                    if (size > 1)
                        result += $"x{size}";
                    break;
                case ShaderParameter.RType.Sampler:
                    result = "sampler2D";
                    break;
            }
            return result;
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

