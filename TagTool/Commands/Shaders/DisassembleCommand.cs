using TagTool.Cache;
using TagTool.Tags.Definitions;
using System;
using System.Collections.Generic;
using TagTool.Shaders;
using TagTool.Common;
using System.IO;
using TagTool.IO;
using System.Linq;
using System.Diagnostics;
using TagTool.Cache.HaloOnline;

namespace TagTool.Commands.Shaders
{
    public class DisassembleCommand<T> : Command
    {
        private GameCache Cache { get; }
        private CachedTag Tag { get; }
        private T Definition { get; }

        public DisassembleCommand(GameCache cache, CachedTag tag, T definition) :
            base(true,

                "Disassemble",
                "Disassembles a VertexShader at the specified index.",

                "Disassemble <index> [file]",

                "<index> - index into the VertexShaders tagblock.")
        {
            Cache = cache;
            Tag = tag;
            Definition = definition;
        }

        public override object Execute(List<string> args)
        {
            if (args.Count < 1 || args.Count > 2)
                return false;

            var disassemblies = new List<string> { };
            var indices = new List<int>();

            if (args[0] == "*")
            {
                for (var i = 0; ; i++)
                {
                    string disassembly = null;
                    if(Cache.GetType() == typeof(GameCacheGen3))
                    {
                        disassembly = DisassembleGen3Shader(i);
                    }
                    else if (Cache.GetType() == typeof(GameCacheHaloOnline))
                    {
                        disassembly = DisassembleHaloOnlineShader(i);
                    }
                    
                    if (disassembly == null)
                        break;
                    else
                    {
                        disassemblies.Add(disassembly);
                        indices.Add(i);
                    }
                }
            }
            else
            {
                if (int.TryParse(args[0], out int shaderIndex))
                {
                    if (Cache.GetType() == typeof(GameCacheGen3))
                    {
                        disassemblies.Add(DisassembleGen3Shader(shaderIndex));
                    }
                    else if (Cache.GetType() == typeof(GameCacheHaloOnline))
                    {
                        disassemblies.Add(DisassembleHaloOnlineShader(shaderIndex));
                    }
                    indices.Add(shaderIndex);
                }
                    
            }

            string filename = args.Count == 2 ? args[1] : "Shaders";

            for (var i = 0; i < disassemblies.Count; i++)
                using (var writer = File.CreateText(Path.Combine(filename, $"{Tag.Name.Split('\\').Last()}_{indices[i]}.{Tag.Group}.txt")))
                {
                    if (Cache.GetType() == typeof(GameCacheGen3))
                        GenerateGen3ShaderHeader(indices[i], writer);
                    writer.WriteLine(disassemblies[i]);
                }
                    

            return true;
        }

        private string DisassembleGen3Shader(int shaderIndex)
        {
            var file = UseXSDCommand.XSDFileInfo;
            if(file == null)
            {
                Console.WriteLine("xsd.exe not found! use command usexsd <directory> to load xsd.exe");
                return null;
            }
            var tempFile = Path.GetTempFileName();
            string disassembly = null;
            string xsdArguments = " \"" + tempFile +"\"";
            byte[] microcode = new byte[0];
            byte[] debugData = new byte[0];
            byte[] constantData = new byte[0];

            //
            // Set the arguments for xsd.exe according to the XDK documentation
            //
            try
            {
                if (typeof(T) == typeof(PixelShader) || typeof(T) == typeof(GlobalPixelShader))
                {
                    xsdArguments = "/rawps" + xsdArguments;
                    PixelShaderBlock shaderBlock = null;
                    if (typeof(T) == typeof(PixelShader))
                    {
                        var _definition = Definition as PixelShader;
                        if (shaderIndex < _definition.Shaders.Count)
                            shaderBlock = _definition.Shaders[shaderIndex];
                        else
                            return null;
                    }

                    if (typeof(T) == typeof(GlobalPixelShader))
                    {
                        var _definition = Definition as GlobalPixelShader;
                        if (shaderIndex < _definition.Shaders.Count)
                            shaderBlock = _definition.Shaders[shaderIndex];
                        else
                            return null;
                    }

                    microcode = shaderBlock.XboxShaderReference.ShaderData;
                    debugData = shaderBlock.XboxShaderReference.DebugData;
                    constantData = shaderBlock.XboxShaderReference.ConstantData;
                }

                if (typeof(T) == typeof(VertexShader) || typeof(T) == typeof(GlobalVertexShader))
                {
                    xsdArguments = "/rawvs" + xsdArguments;
                    VertexShaderBlock shaderBlock = null;
                    if (typeof(T) == typeof(VertexShader))
                    {
                        var _definition = Definition as VertexShader;
                        if (shaderIndex < _definition.Shaders.Count)
                            shaderBlock = _definition.Shaders[shaderIndex];
                        else
                            return null;
                    }

                    if (typeof(T) == typeof(GlobalVertexShader))
                    {
                        var _definition = Definition as GlobalVertexShader;
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
            catch(Exception e)
            {
                Console.WriteLine($"Failed to disassemble shader: {e.ToString()}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            return disassembly;
        }

        private string DisassembleHaloOnlineShader(int shaderIndex)
        {
            string disassembly = null;

            if (typeof(T) == typeof(PixelShader) || typeof(T) == typeof(GlobalPixelShader))
            {
                PixelShaderBlock shader_block = null;
                if (typeof(T) == typeof(PixelShader))
                {
                    var _definition = Definition as PixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                if (typeof(T) == typeof(GlobalPixelShader))
                {
                    var _definition = Definition as GlobalPixelShader;
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

            if (typeof(T) == typeof(VertexShader) || typeof(T) == typeof(GlobalVertexShader))
            {
                VertexShaderBlock shader_block = null;
                if (typeof(T) == typeof(VertexShader))
                {
                    var _definition = Definition as VertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return null;
                }

                if (typeof(T) == typeof(GlobalVertexShader))
                {
                    var _definition = Definition as GlobalVertexShader;
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

            return disassembly;
        }

        private void GenerateGen3ShaderHeader(int shaderIndex, StreamWriter writer)
        {
            List<ShaderParameter> parameters = null;
            List<RealQuaternion> constants = null;

            if (typeof(T) == typeof(PixelShader) || typeof(T) == typeof(GlobalPixelShader))
            {
                PixelShaderBlock shader_block = null;
                if (typeof(T) == typeof(PixelShader))
                {
                    var _definition = Definition as PixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return;
                }

                if (typeof(T) == typeof(GlobalPixelShader))
                {
                    var _definition = Definition as GlobalPixelShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shader_block = _definition.Shaders[shaderIndex];
                    else
                        return;
                }

                constants = GetShaderConstants(shader_block.XboxShaderReference.ConstantData);
                parameters = shader_block.XboxParameters;
            }

            if (typeof(T) == typeof(VertexShader) || typeof(T) == typeof(GlobalVertexShader))
            {
                VertexShaderBlock shaderBlock = null;
                if (typeof(T) == typeof(VertexShader))
                {
                    var _definition = Definition as VertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shaderBlock = _definition.Shaders[shaderIndex];
                    else
                        return;
                }

                if (typeof(T) == typeof(GlobalVertexShader))
                {
                    var _definition = Definition as GlobalVertexShader;
                    if (shaderIndex < _definition.Shaders.Count)
                        shaderBlock = _definition.Shaders[shaderIndex];
                    else
                        return;
                }

                constants = GetShaderConstants(shaderBlock.XboxShaderReference.ConstantData);
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
            for(int i = 0; i < parameters.Count; i++)
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
            WriteHeaderLine(writer);
            WriteHeaderLine(writer, "Constants");
            WriteHeaderLine(writer);
            int constantregister = 255;
            for (int i = 0; i < constants.Count; i++)
            {
                WriteHeaderLine(writer, $"    c{constantregister} {constants[i]} ");
                constantregister--;
            }
        }

        private void WriteHeaderLine(StreamWriter writer, string entry = null)
        {
            string result = "// ";
            if(entry != null)
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

        private List<RealQuaternion> GetShaderConstants(byte[] constantData)
        {
            var constants = new List<RealQuaternion>();
            using (var stream = new MemoryStream(constantData))
            using (var reader = new EndianReader(stream, EndianFormat.BigEndian))
            {
                for (var i = 0; i < constantData.Length / 16; i++)
                {
                    constants.Add(reader.ReadRealQuaternion());
                }
                constants.Reverse(); //they are stored in reverse order
            }

            return constants;
        }
    }
}
