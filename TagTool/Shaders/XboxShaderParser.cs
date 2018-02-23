using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagTool.Cache;
using TagTool.Geometry;
using TagTool.IO;
using TagTool.Tags.Definitions;

namespace TagTool.Shaders
{
    class XboxShaderParser
    {
        public UPDBParser UPDB;
        public byte[] ShaderData => IsVertexShader ? (Block as VertexShaderBlock).XboxShaderReference?.ShaderData : (Block as PixelShaderBlock).XboxShaderReference?.ShaderData;
        public byte[] DebugData => IsVertexShader ? (Block as VertexShaderBlock).XboxShaderReference?.DebugData : (Block as PixelShaderBlock).XboxShaderReference?.DebugData;
        public byte[] ConstantData => IsVertexShader ? (Block as VertexShaderBlock).XboxShaderReference?.ConstantData : (Block as PixelShaderBlock).XboxShaderReference?.ConstantData;

        public bool IsVertexShader => Block?.GetType() == typeof(VertexShaderBlock);
        public bool IsPixelShader => Block?.GetType() == typeof(PixelShaderBlock);
        public bool IsValid => !(CacheContext == null || Tag == null || Block == null || ShaderData == null || ShaderData.Length == 0);

        private GameCacheContext CacheContext { get; }

        public object Block { get; set; }
        public object Tag { get; set; }

        public enum OutputFile
        {
            ShaderData,
            DebugData,
            ConstantData
        }

        public XboxShaderParser(VertexShader tag, VertexShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(PixelShader tag, PixelShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(GlobalVertexShader tag, VertexShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(GlobalPixelShader tag, PixelShaderBlock block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null) : this((object)tag, (object)block, gamecachecontext, updb_parser) { }
        public XboxShaderParser(object tag, object block, GameCacheContext gamecachecontext, UPDBParser updb_parser = null)
        {
            CacheContext = gamecachecontext;
            Block = block;
            Tag = tag;
            if (!IsValid) return;
            WriteOutput(OutputFile.ShaderData);
            WriteOutput(OutputFile.ConstantData);
            WriteOutput(OutputFile.DebugData);


            //UPDB = updb_parser != null ? updb_parser : new UPDBParser(DebugData); //TODO: It appears that the current format is for Vertex uPDB's only...

            //if (typeof(T) == typeof(GlobalVertexShader)) ProcessShader(Tag as GlobalVertexShader, Block as VertexShaderBlock);
            //if (typeof(T) == typeof(VertexShader)) ProcessShader(Tag as VertexShader, Block as VertexShaderBlock);
            //if (typeof(T) == typeof(GlobalPixelShader)) ProcessShader(Tag as GlobalPixelShader, Block as PixelShaderBlock);
            //if (typeof(T) == typeof(PixelShader)) ProcessShader(Tag as PixelShader, Block as PixelShaderBlock);

        }

        private List<ShaderParameter> GetParameters()
        {
            if (Tag.GetType() == typeof(PixelShader)) return GetParameters(Tag as PixelShader);
            throw new NotImplementedException();
        }
        private List<ShaderParameter> GetParameters(PixelShader Tag)
        {
            var Block = this.Block as PixelShaderBlock;
            return Block.XboxParameters.ToList();
        }
        private string GetShaderParameterTypeString(ShaderParameter.RType type)
        {
            if (type == ShaderParameter.RType.Boolean) return "b";
            if (type == ShaderParameter.RType.Float) return "c";
            if (type == ShaderParameter.RType.Integer) return "i";
            if (type == ShaderParameter.RType.Sampler) return "s";
            throw new NotImplementedException();
        }
        private string GetShaderParameterNameString(ShaderParameter.RType type)
        {
            if (type == ShaderParameter.RType.Boolean) return "bool";
            if (type == ShaderParameter.RType.Float) return "float4";
            if (type == ShaderParameter.RType.Integer) return "integer";
            if (type == ShaderParameter.RType.Sampler) return "sampler";
            throw new NotImplementedException();
        }

        public string GetParametersBlock()
        {
            StringBuilder sb = new StringBuilder();
            var parameters = GetParameters();
            foreach (var parameter in parameters)
            {
                sb.AppendLine($"//   {GetShaderParameterNameString(parameter.RegisterType)} {CacheContext.GetString(parameter.ParameterName)};");
            }
            return sb.ToString();

        }

        private string FormatRegistersBlockEntry(string _name, string _reg, string _size, int max_name_length, int reg_length, int size_length)
        {
            string name_col = $"{_name}{new String(' ', max_name_length - _name.Length + 1)}";
            string reg_col = $"{_reg}{new String(' ', reg_length - _reg.Length + 1)}";
            string size_col = $"{new String(' ', size_length - _size.Length)}{_size}";
            return name_col + reg_col + size_col;
        }
        
        public string GetRegistersBlock()
        {
            int max_name_length = 14;
            int reg_length = 5;
            int size_length = 4;

            var registers = GetParameters();
            foreach (var register in registers)
            {
                var name = CacheContext.GetString(register.ParameterName);
                max_name_length = Math.Max(max_name_length, name.Length);
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"//   {FormatRegistersBlockEntry("Name", "Reg", "Size", max_name_length, reg_length, size_length)}");
            sb.AppendLine($"//   {FormatRegistersBlockEntry(new String('-', max_name_length), new String('-', reg_length), new String('-', size_length), max_name_length, reg_length, size_length)}");
            foreach (var register in registers)
            {
                string name = CacheContext.GetString(register.ParameterName);
                string reg = $"{GetShaderParameterTypeString(register.RegisterType)}{register.RegisterIndex}";
                string size = register.RegisterCount.ToString();
                sb.AppendLine($"//   {FormatRegistersBlockEntry(name, reg, size, max_name_length, reg_length, size_length)}");
            }
            return sb.ToString();
        }

        public string Disassemble(string _raw_shader_code = null)
        {
            var raw_shader_code = _raw_shader_code == null ? XSDDisassemble() : _raw_shader_code;
            var formatted_raw_block = CommentStringBlock(raw_shader_code, true);
            var converted_shader_code = ConvertXboxShader(raw_shader_code);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("//");
            sb.AppendLine($"// Generated by Tag Tool {Environment.Version}");
            sb.AppendLine("//");
            sb.AppendLine("// Parameters:");
            sb.Append(GetParametersBlock());
            sb.AppendLine("//");
            sb.AppendLine("//");
            sb.AppendLine("// Registers:");
            sb.AppendLine("//");
            sb.Append(GetRegistersBlock());
            sb.AppendLine("//");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("// Raw Xbox 360 Shader Assembly");
            sb.AppendLine(formatted_raw_block);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("// Converted PC Shader Assembly");
            sb.AppendLine(converted_shader_code);
            sb.AppendLine();
            sb.AppendLine();

            return sb.ToString();
        }

        private string ConvertXboxShader(string raw_shader_code)
        {
            if (IsVertexShader) throw new NotImplementedException();

            var instructions = raw_shader_code.Split(new[] { "\r\n", "\r", "\n", Environment.NewLine }, StringSplitOptions.None).ToList();
            for (var i = instructions.Count - 1; i >= 0; i--)
            {
                var instruction = instructions[i].Trim();

                // Take the left side of any comments
                instruction = instruction.Split(new[] { "//" }, StringSplitOptions.None)[0];

                if (String.IsNullOrWhiteSpace(instruction))
                {
                    instructions.RemoveAt(i);
                    continue;
                }

                var assembly_codes = instruction.Split(new string[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);

                if (assembly_codes[0] == "exec") instruction = "//" + instruction;
                if (assembly_codes[0] == "exece") instruction = "//" + instruction;
                if (assembly_codes[0] == "alloc") instruction = "//" + instruction;

                instructions[i] = instruction;
            }

            List<string> header = new List<string>();
            if (IsVertexShader) header.Insert(0, "vs_3_0");
            if (IsPixelShader) header.Insert(0, "ps_3_0");

            var registers = GetParameters();
            foreach (var register in registers)
            {
                if (register.RegisterType == ShaderParameter.RType.Sampler)
                {
                    header.Add($"dcl_2d {GetShaderParameterTypeString(register.RegisterType)}{register.RegisterIndex} // {CacheContext.GetString(register.ParameterName)}");
                }
            }

            instructions.InsertRange(0, header);

            return String.Join("\n", instructions);
        }

        private byte[] ProcessVertexShader(VertexShaderBlock Block)
        {
            throw new NotImplementedException();
        }

        private byte[] ProcessPixelShader(PixelShaderBlock Block)
        {
            var raw_shader_code = Disassemble();
            var converted_shader_code = ConvertXboxShader(raw_shader_code);

            var shader_bytecode = Utilities.DirectXUtilities.AssembleShader(converted_shader_code);

            Console.WriteLine("Processed PixelShader");
            return shader_bytecode;
        }

        public byte[] ProcessShader()
        {
            if (IsVertexShader) return ProcessVertexShader(Block as VertexShaderBlock);
            if (IsPixelShader) return ProcessPixelShader(Block as PixelShaderBlock);
            return null;
        }

        private string CommentStringBlock(string input, bool trim)
        {
            var linesInText = input.Split(new string[] { Environment.NewLine, "\n", "\r" }, StringSplitOptions.None);

            StringBuilder stringWithRowNumbers = new StringBuilder();

            foreach (var line in linesInText)
            {
                var str = line;
                if (trim) str = line.Trim();

                stringWithRowNumbers.AppendLine($"// {str}");
            }
            string result = stringWithRowNumbers.ToString();
            return result;
        }

        public void WriteOutput(OutputFile file)
        {
            Directory.CreateDirectory(@"Temp");

            if (file == OutputFile.ShaderData)
            {
                WriteOutput(@"Temp\permutation.shader", ShaderData);
            }

            if (file == OutputFile.ConstantData)
            {
                WriteOutput(@"Temp\permutation.shader.updb", DebugData);
            }

            if (file == OutputFile.ShaderData)
            {
                WriteOutput(@"Temp\permutation.shader.cbin", ConstantData);
            }
        }

        private string XSDDisassemble()
        {
            if (!File.Exists(@"Tools\xsd.exe"))
            {
                Console.WriteLine("Missing tools, please install xsd.exe before porting shaders.");
                return null;
            }

            WriteOutput(OutputFile.ShaderData);

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = @"Tools\xsd.exe",
                    Arguments = IsVertexShader ? "/rawvs permutation.shader" : "/rawps permutation.shader",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "temp")
                }
            };
            process.Start();

            string shader_code = process.StandardOutput.ReadToEnd();
            string err = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (!String.IsNullOrWhiteSpace(err)) throw new Exception(err);

            return shader_code;
        }

        private void WriteOutput(string file, byte[] data)
        {
            if (File.Exists(file)) File.Delete(file);
            if (data.Length > 0)
                using (EndianWriter output = new EndianWriter(File.OpenWrite(file), EndianFormat.BigEndian))
                    output.WriteBlock(data);
        }
    }
}
