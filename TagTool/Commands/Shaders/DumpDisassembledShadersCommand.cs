using TagTool.Cache;
using TagTool.Common;
using TagTool.Commands.Common;
using TagTool.IO;
using System.Collections.Generic;
using System.IO;
using System;
using TagTool.Tags.Definitions;
using TagTool.Geometry;
using TagTool.Cache.HaloOnline;
using TagTool.Shaders;
using System.Diagnostics;

namespace TagTool.Commands.Shaders
{
    // TODO: Support Reach gpix
    public class DumpDisassembledShadersCommand : Command
    {
        GameCache Cache;

        public DumpDisassembledShadersCommand(GameCache cache) : base(false, "DumpDisassembledShaders", "Dump disassembled shaders", "DumpDisassembledShaders", "")
        {
            Cache = cache;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count > 0)
                return new TagToolError(CommandError.ArgCount);

            if (Cache.GetType() == typeof(GameCacheGen3) && UseXSDCommand.XSDFileInfo == null)
                return new TagToolError(CommandError.CustomError, "You must use the \"UseXSD\" command first!");

            Type entryPointEnum = typeof(EntryPoint);
            if (Cache.Version >= CacheVersion.HaloReach)
                entryPointEnum = typeof(EntryPointReach);
            else if (Cache.Version >= CacheVersion.HaloOnline301003 && Cache.Version <= CacheVersion.HaloOnline700123)
                entryPointEnum = typeof(EntryPointMs30);

            using (var stream = Cache.OpenCacheRead())
            {
                GlobalCacheFilePixelShaders gpix = null;
                if (Cache.Version == CacheVersion.HaloReach)
                    gpix = Cache.Deserialize<GlobalCacheFilePixelShaders>(stream, Cache.TagCache.FindFirstInGroup("gpix"));

                List<string> shaderTypes = new List<string>();

                foreach (var tag in Cache.TagCache.NonNull())
                {
                    if (tag.Name == null || tag.Name == "" || tag.Group.Tag != "rmdf")
                        continue;
                    if (Cache.Version == CacheVersion.HaloOnline106708 && tag.Name.StartsWith("ms30\\"))
                        continue; // ignore ms30 in ms23, disassemble from ms30 directly instead
                    var pieces = tag.Name.Split('\\');
                    shaderTypes.Add(pieces[pieces.Length - 1]);
                }

                foreach (var shaderType in shaderTypes)
                {
                    var glvsTagName = $"shaders\\{shaderType}_shared_vertex_shaders.glvs";
                    var glpsTagName = $"shaders\\{shaderType}_shared_pixel_shaders.glps";

                    if (!Cache.TagCache.TryGetTag(glvsTagName, out CachedTag glvsTag) || !Cache.TagCache.TryGetTag(glpsTagName, out CachedTag glpsTag))
                    {
                        Console.WriteLine($"WARNING: Cache \"{Cache.DisplayName}\" has invalid shader type \"{shaderType}\"");
                        continue;
                    }

                    var glvs = Cache.Deserialize<GlobalVertexShader>(stream, glvsTag);
                    var glps = Cache.Deserialize<GlobalPixelShader>(stream, glpsTag);

                    foreach (var tag in Cache.TagCache.NonNull())
                    {
                        if (tag.IsInGroup("rmt2") && tag.Name.StartsWith($"shaders\\{shaderType}_templates"))
                        {
                            // disassemble specified shaders related to rmt2
                            var tagName = tag.Name;
                            var rmt2Tag = Cache.TagCache.GetTag(tagName, "rmt2");
                            var rmt2 = Cache.Deserialize<RenderMethodTemplate>(stream, rmt2Tag);

                            if (rmt2.PixelShader == null)
                            {
                                Console.WriteLine("ERROR: Template pixel shader was null");
                                continue;
                            }

                            var pixl = Cache.Deserialize<PixelShader>(stream, rmt2.PixelShader);

                            Directory.CreateDirectory(Cache.Version.ToString() + "\\" + tagName);


                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                int entryIndex = GetEntryPointIndex(entry);

                                if (entryIndex < pixl.EntryPointShaders.Count)
                                {
                                    var entryShader = pixl.EntryPointShaders[entryIndex].Offset;

                                    if (pixl.EntryPointShaders[entryIndex].Count != 0)
                                    {
                                        string entryName = entry.ToString().ToLower() + ".pixel_shader";
                                        string pixelShaderFilename = Path.Combine(tagName, entryName);

                                        DisassembleShader(pixl, entryShader, pixelShaderFilename, Cache, pixl.Shaders[entryShader].GlobalCachePixelShaderIndex != -1 ? gpix : null);
                                    }
                                }
                            }
                        }
                    }

                    // glps
                    Directory.CreateDirectory(Cache.Version.ToString() + "\\" + glpsTagName);
                    foreach (var entry in Enum.GetValues(entryPointEnum))
                    {
                        int entryIndex = GetEntryPointIndex(entry);

                        if (entryIndex < glps.EntryPoints.Count)
                        {
                            var entryShader = glps.EntryPoints[entryIndex].ShaderIndex;
                            if (entryShader != -1)
                            {
                                string entryName = entry.ToString().ToLower() + ".shared_pixel_shader";
                                string pixelShaderFilename = Path.Combine(glpsTagName, entryName);

                                DisassembleShader(glps, entryShader, pixelShaderFilename, Cache, glps.Shaders[entryShader].GlobalCachePixelShaderIndex != -1 ? gpix : null);
                            }
                            else if (glps.EntryPoints[entryIndex].Option.Count > 0)
                            {
                                foreach (var option in glps.EntryPoints[entryIndex].Option)
                                {
                                    var methodIndex = option.RenderMethodOptionIndex;
                                    for (int i = 0; i < option.OptionMethodShaderIndices.Count; i++)
                                    {
                                        var optionIndex = i;
                                        string glpsFilename = entry.ToString().ToLower() + $"_{methodIndex}_{optionIndex}" + ".shared_pixel_shader";
                                        glpsFilename = Path.Combine(glpsTagName, glpsFilename);
                                        DisassembleShader(glps, option.OptionMethodShaderIndices[i], glpsFilename, Cache, glps.Shaders[option.OptionMethodShaderIndices[i]].GlobalCachePixelShaderIndex != -1 ? gpix : null);
                                    }
                                }
                            }
                        }
                    }

                    // glvs
                    foreach (VertexType vert in Enum.GetValues(typeof(VertexType)))
                    {
                        if ((int)vert < glvs.VertexTypes.Count)
                        {
                            var vertexFormat = glvs.VertexTypes[(int)vert];
                            var dirName = Path.Combine(glvsTagName, vert.ToString().ToLower());
                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                int entryIndex = GetEntryPointIndex(entry);

                                if (entryIndex < vertexFormat.DrawModes.Count)
                                {
                                    var entryShader = vertexFormat.DrawModes[entryIndex].ShaderIndex;
                                    if (entryShader != -1)
                                    {
                                        Directory.CreateDirectory(Cache.Version.ToString() + "\\" + dirName);
                                        string entryName = entry.ToString().ToLower() + ".shared_vertex_shader";
                                        string vertexShaderFileName = Path.Combine(dirName, entryName);

                                        DisassembleShader(glvs, entryShader, vertexShaderFileName, Cache, null);
                                    }
                                }
                            }
                        }
                    }
                }

                CachedTag rasterizerGlobalsTag = Cache.TagCache.FindFirstInGroup("rasg");
                if (rasterizerGlobalsTag != null)
                {
                    var rasterizerGlobals = Cache.Deserialize<RasterizerGlobals>(stream, rasterizerGlobalsTag);

                    for (int i = 0; i < rasterizerGlobals.DefaultShaders.Count; i++)
                    {
                        var explicitShader = rasterizerGlobals.DefaultShaders[i];

                        if (explicitShader.PixelShader == null)
                        {
                            Console.WriteLine($"Invalid explicit pixel shader {i}");
                        }
                        else
                        {
                            Directory.CreateDirectory(Cache.Version.ToString() + "\\" + explicitShader.PixelShader.Name);

                            var pixl = Cache.Deserialize<PixelShader>(stream, explicitShader.PixelShader);
                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                int entryIndex = GetEntryPointIndex(entry);

                                if (pixl.EntryPointShaders.Count <= entryIndex)
                                    break;

                                for (int j = 0; j < pixl.EntryPointShaders[entryIndex].Count; j++)
                                {
                                    int shaderIndex = pixl.EntryPointShaders[entryIndex].Offset + j;
                                    string entryName = shaderIndex + "_" + entry.ToString().ToLower() + ".pixel_shader";
                                    string pixelShaderFilename = Path.Combine(explicitShader.PixelShader.Name, entryName);

                                    DisassembleShader(pixl, shaderIndex, pixelShaderFilename, Cache, pixl.Shaders[shaderIndex].GlobalCachePixelShaderIndex != -1 ? gpix : null);
                                }
                            }
                        }

                        if (explicitShader.VertexShader == null)
                        {
                            Console.WriteLine($"Invalid explicit vertex shader {i}");
                        }
                        else
                        {
                            var vtsh = Cache.Deserialize<VertexShader>(stream, explicitShader.VertexShader);
                            foreach (var entry in Enum.GetValues(entryPointEnum))
                            {
                                int entryIndex = GetEntryPointIndex(entry);

                                if (vtsh.EntryPoints.Count <= entryIndex)
                                    break;

                                for (int j = 0; j < vtsh.EntryPoints[entryIndex].SupportedVertexTypes.Count; j++)
                                {
                                    for (int k = 0; k < vtsh.EntryPoints[entryIndex].SupportedVertexTypes[j].Count; k++)
                                    {
                                        int shaderIndex = vtsh.EntryPoints[entryIndex].SupportedVertexTypes[j].Offset + k;

                                        var dirName = Path.Combine(explicitShader.VertexShader.Name + "\\",  ((VertexType)k).ToString().ToLower() + "\\");
                                        Directory.CreateDirectory(Cache.Version.ToString() + "\\" + dirName);

                                        string entryName = shaderIndex + "_" + entry.ToString().ToLower() + ".vertex_shader";
                                        string vertexShaderFilename = Path.Combine(dirName, entryName);

                                        DisassembleShader(vtsh, shaderIndex, vertexShaderFilename, Cache, null);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

        private string DisassembleShader(object definition, int shaderIndex, string filename, GameCache cache, GlobalCacheFilePixelShaders gpix)
        {
            //version the directory
            string path = $"{cache.Version.ToString()}\\{filename}";

            if (cache.GetType() == typeof(GameCacheGen3))
            {
                switch (definition)
                {
                    case PixelShader pixl:
                        if (pixl.Shaders[shaderIndex].XboxShaderReference == null)
                            return "NO DATA";
                        break;
                    case VertexShader vtsh:
                        if (vtsh.Shaders[shaderIndex].XboxShaderReference == null)
                            return "NO DATA";
                        break;
                    case GlobalPixelShader glps:
                        if (glps.Shaders[shaderIndex].XboxShaderReference == null)
                            return "NO DATA";
                        break;
                    case GlobalVertexShader glvs:
                        if (glvs.Shaders[shaderIndex].XboxShaderReference == null)
                            return "NO DATA";
                        break;
                }
                
                return DisassembleGen3Shader(definition, shaderIndex, path, gpix);
            }
            else if (Cache.GetType() == typeof(GameCacheHaloOnline))
            {
                return DisassembleHaloOnlineShader(definition, shaderIndex, path);
            }
            return null;
        }

        private string DisassembleGen3Shader(object definition, int shaderIndex, string filename, GlobalCacheFilePixelShaders gpix)
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
            List<int> disassemblyConstants = new List<int>();

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
                        if (gpix == null && shaderIndex < _definition.Shaders.Count)
                            shaderBlock = _definition.Shaders[shaderIndex];
                        else if (gpix != null)
                            shaderBlock = gpix.Shaders[_definition.Shaders[shaderIndex].GlobalCachePixelShaderIndex];
                        else
                            return null;
                    }

                    if (definition.GetType() == typeof(GlobalPixelShader))
                    {
                        var _definition = definition as GlobalPixelShader;
                        if (gpix == null && shaderIndex < _definition.Shaders.Count)
                            shaderBlock = _definition.Shaders[shaderIndex];
                        else if (gpix != null)
                            shaderBlock = gpix.Shaders[_definition.Shaders[shaderIndex].GlobalCachePixelShaderIndex];
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

                //xsd.StandardOutput.BaseStream.Seek(0, SeekOrigin.Begin);
                //while (!xsd.StandardOutput.EndOfStream)
                //{
                //    string line = xsd.StandardOutput.ReadLine();
                //}

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

            // get all constants used in the shader



            using (var writer = File.CreateText(filename))
            {
                GenerateGen3ShaderHeader(definition, shaderIndex, writer, gpix/*, disassemblyConstants*/);
                writer.WriteLine(disassembly);
            }

            return disassembly;
        }


        private void GenerateGen3ShaderHeader(object definition, int shaderIndex, StreamWriter writer, GlobalCacheFilePixelShaders gpix, List<int> disassemblyConstants = null)
        {
            List<ShaderParameter> parameters = null;
            List<RealQuaternion> constants = new List<RealQuaternion>();

            if (definition.GetType() == typeof(PixelShader) || definition.GetType() == typeof(GlobalPixelShader))
            {
                PixelShaderBlock shader_block = null;
                if (definition.GetType() == typeof(PixelShader))
                {
                    var _definition = definition as PixelShader;
                    if (gpix == null && shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else if (gpix != null)
                        shader_block = gpix.Shaders[_definition.Shaders[shaderIndex].GlobalCachePixelShaderIndex];
                    else
                        return;
                }

                if (definition.GetType() == typeof(GlobalPixelShader))
                {
                    var _definition = definition as GlobalPixelShader;
                    if (gpix == null && shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else if (gpix != null)
                        shader_block = gpix.Shaders[_definition.Shaders[shaderIndex].GlobalCachePixelShaderIndex];
                    else
                        return;
                }

                if (shader_block.XboxShaderReference != null)
                    constants = GetShaderConstants(shader_block.XboxShaderReference.ConstantData);
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

                if (shaderBlock.XboxShaderReference != null)
                    constants = GetShaderConstants(shaderBlock.XboxShaderReference.ConstantData);
                parameters = shaderBlock.XboxParameters;
            }

            List<string> parameterNames = new List<string>();

            for (int i = 0; i < parameters.Count; i++)
                parameterNames.Add(Cache.StringTable.GetString(parameters[i].ParameterName));


            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Generated by TagTool and Xbox Shader Disassembler");
            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Parameters:");
            WriteHeaderLine(writer);
            for (int i = 0; i < parameters.Count; i++)
            {
                var param = parameters[i];
                WriteHeaderLine(writer, $"\t{GetTypeString(param.RegisterType, param.RegisterCount)} {parameterNames[i]};");
            }
            WriteHeaderLine(writer);
            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Registers:");
            WriteHeaderLine(writer);
            WriteHeaderLine(writer, $"\t\t{"Name".ToLength(36)}{"Reg".ToLength(6)}{"Size".ToLength(4)}");
            WriteHeaderLine(writer, "\t\t----------------------------------- ----- ----");

            List<int> usedConstants = new List<int>();
            for (int i = 0; i < parameters.Count; i++)
            {
                // write
                var param = parameters[i];
                WriteHeaderLine(writer, $"\t\t{parameterNames[i].ToLength(36)}{GetRegisterString(param.RegisterType, param.RegisterIndex).ToLength(6)}{param.RegisterCount.ToString().ToLength(4)}");

                // store used registers to sort literal constants
                for (int j = 0; j < param.RegisterCount; j++)
                    usedConstants.Add(j + param.RegisterIndex);
            }
            WriteHeaderLine(writer);
            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Constants:");
            WriteHeaderLine(writer);
            int constantRegister = 255;
            for (int i = 0; i < constants.Count; i++)
            {
                while (usedConstants.Contains(constantRegister) || 
                    (disassemblyConstants != null && !disassemblyConstants.Contains(constantRegister)))
                {
                    constantRegister--;
                    if (constantRegister < 0)
                        throw new Exception("Constant register could not be matched");
                }

                WriteHeaderLine(writer, $"\t\tc{constantRegister} {constants[i]} ");
                constantRegister--;
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

            if (definition.GetType() == typeof(PixelShader) || definition.GetType() == typeof(GlobalPixelShader))
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

        private int GetEntryPointIndex(object input)
        {
            if (Cache.Version >= CacheVersion.HaloReach)
                return (int)((EntryPointReach)input);
            else if (Cache.Version >= CacheVersion.HaloOnline301003 && Cache.Version <= CacheVersion.HaloOnline700123)
                return (int)((EntryPointMs30)input);
            else
                return (int)((EntryPoint)input);
        }

        private List<RealQuaternion> GetShaderConstants(byte[] constantData)
        {
            var constants = new List<RealQuaternion>();
            using (var stream = new MemoryStream(constantData))
            using (var reader = new EndianReader(stream, EndianFormat.BigEndian))
            {
                for (var i = 0; i < constantData.Length / 16; i++)
                {
                    constants.Add(new RealQuaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()));
                }
                constants.Reverse(); //they are stored in reverse order
            }

            return constants;
        }
    }
}

